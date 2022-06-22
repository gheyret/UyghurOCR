/*
 * Created by SharpDevelop.
 * User: nk1449
 * Date: 2017/05/29
 * Time: 11:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Tesseract;
using System.Text;

namespace UyghurOCR
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		Random      grand = new System.Random();
		TesseractEngine         gOcr;
		OCRText  gOcrTxtForm = null;
		private  Microsoft.Win32.RegistryKey     gRegKey= Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Kenjsoft\KenjiResim");
		
		static string gImgExts;
			
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			System.Reflection.Assembly asm =System.Reflection.Assembly.GetExecutingAssembly();
			this.Icon=new Icon(asm.GetManifestResourceStream("UyghurOCR.OCAq.ico"));
			
			var codecs = ImageCodecInfo.GetImageEncoders();
			foreach (var codec in codecs)
			{
				gImgExts += codec.FilenameExtension + ";";
			}
			
		}
		
		bool IsImage(string filename)
		{
			return gImgExts.IndexOf(Path.GetExtension(filename),StringComparison.OrdinalIgnoreCase) == -1? false:true;
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(gRegKey!=null){
				gRegKey.SetValue("LAST",label1.Text);
			}
		}
		
		
		async void ButtonRight(object sender, EventArgs e)
		{
			butRecognize.Enabled = false;
			butNext.Enabled = false;
			butDeskew.Enabled = false;
			
			Bitmap roibmp;
			Pix    roipix;
			Rectangle roi = ramka.getRoi();
			Cursor=Cursors.WaitCursor;
			roibmp = ramka.Image.Clone(roi,ramka.Image.PixelFormat);
			roibmp.SetResolution(400,400);
			roipix = PixConverter.ToPix(roibmp).Deskew().Scale(4.0f,4.0f);
			roibmp.Dispose();

			Task<string> ocr = Task.Run<string>(() =>{
			                                    	return DoOCR(roipix);
			                                    });
			string txt = await ocr;

			if(gOcrTxtForm==null || gOcrTxtForm.IsDisposed){
				gOcrTxtForm=new OCRText();
				gOcrTxtForm.Show();
			}
			gOcrTxtForm.SetText(txt);
			
			butRecognize.Enabled = true;
			butNext.Enabled = true;
			butDeskew.Enabled = true;
			Cursor=Cursors.Default;
			roipix.Dispose();
		}
		
		
		string DoOCR(Pix pix){
			if(rdAuto.Checked){
				gOcr.DefaultPageSegMode = PageSegMode.Auto;
			}
			else if(rdSingle.Checked){
				gOcr.DefaultPageSegMode = PageSegMode.SingleBlock;
			}
			Page pg = gOcr.Process(pix);
			String buf = pg.GetText();
			pix.Dispose();
			pg.Dispose();
			return buf.Replace("ی","ي").Replace("ه","ە").Replace("\n",Environment.NewLine);
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
		}
		
		
		void listAllImg(){
			listBox1.Items.Clear();
			if(!Directory.Exists(label1.Text)) return;
			String[] images = Directory.GetFiles(label1.Text,"*.*");
			foreach(string afile in images){
				if(IsImage(afile)){
					listBox1.Items.Add(Path.GetFileName(afile));
				}
			}
			if(listBox1.Items.Count>0){
				listBox1.SelectedIndex = 0;
				//ShowImg();
			}
		}
		
		void ShowImg(){
			String path = Path.Combine(label1.Text, (string)listBox1.SelectedItem);
			Bitmap bimg = new Bitmap(path);
			ramka.Image=bimg;
		}

		void ButtonNextClick(object sender, EventArgs e)
		{
			int index = listBox1.SelectedIndex;
			index = index +1;
			if(index<listBox1.Items.Count){
				listBox1.SelectedIndex = index;
			}
			//ShowImg();
		}
		
		void ListBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			ShowImg();
		}
		
		void Label1Click(object sender, EventArgs e)
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.SelectedPath = label1.Text;
			dlg.Description = "Select Image Folder:";
			dlg.ShowNewFolderButton = false;
			
			DialogResult drs = dlg.ShowDialog(this);
			if(drs == DialogResult.OK){
				label1.Text = dlg.SelectedPath;
				listAllImg();
			}
		}
		
		void MainFormShown(object sender, EventArgs e)
		{
			String lastdir =null;
			if(gRegKey!=null){
				lastdir=(string)gRegKey.GetValue("LAST");
			}
			if(lastdir==null || Directory.Exists(lastdir)==false){
				label1.Text=Path.Combine(Application.StartupPath,"sinaq_resim");
				listAllImg();
			}
			else{
				label1.Text = lastdir;
				listAllImg();
			}
			
			string[] langs = Directory.GetFiles(@".\tessdata","*.traineddata");
			string   lang;
			if(langs.Length>0){
				for(int i=0;i<langs.Length;i++){
					lang = langs[i].Replace(@".\tessdata\","").Replace(".traineddata","");
					chkLang.Items.Add(lang);
				}
				int ind = chkLang.Items.IndexOf("ukij");
				if(ind==-1){
					ind = 0;
				}
				chkLang.SelectedIndex = ind;
			}
			else{
				MessageBox.Show("No Language Data", "Uyghur OCR",MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		void ButPdftoImageClick(object sender, EventArgs e)
		{
			OpenFileDialog ofd=new OpenFileDialog();
			ofd.Title="PDF hojjetning atini tallang";
			ofd.Filter ="PDF file(*.pdf)|*.pdf";
			if(ofd.ShowDialog()!= DialogResult.OK) return;

			String pdfFile=ofd.FileName;
			String path=System.IO.Path.GetDirectoryName(pdfFile)+"\\"+System.IO.Path.GetFileNameWithoutExtension(pdfFile);
			if(Directory.Exists(path)==false){
				Directory.CreateDirectory(path);
			}

			butPDF.Enabled=false;
			this.Cursor=System.Windows.Forms.Cursors.WaitCursor;
			if(ExtractImage(pdfFile,path)){
				label1.Text = path;
				listAllImg();
			}
			butPDF.Enabled=true;
			this.Cursor=System.Windows.Forms.Cursors.Arrow;
		}
		
		private  bool ExtractImage(String pdfFile, string imgPath)
		{
			try{
				PDFHelper hlp=new PDFHelper();
				hlp.ExtractImages(pdfFile,imgPath, progressBar1);
				return true;
			}catch(Exception ee){
//				MessageBox.Show(ee.Message,"Xataliq koruldi");
				System.Diagnostics.Debug.WriteLine(ee.StackTrace);
				return false;
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Bitmap roibmp;
			Pix    roipix;
			butRecognize.Enabled = false;
			butNext.Enabled = false;
			butDeskew.Enabled = false;
			Rectangle roi = ramka.getRoi();
			Cursor=Cursors.WaitCursor;
			roibmp = ramka.Image.Clone(roi,ramka.Image.PixelFormat);
//			roibmp.SetResolution(00,400);

			roipix = PixConverter.ToPix(roibmp);
			roibmp.Dispose();
			
			Pix roipix1 = roipix.Deskew();
			Bitmap newbm = PixConverter.ToBitmap(roipix1);
			roipix.Dispose();
			ramka.Image = newbm;
			Cursor=Cursors.Default;
			butRecognize.Enabled = true;
			butNext.Enabled = true;
			butDeskew.Enabled = true;
		}
		
		void MainFormDragEnter(object sender, DragEventArgs e)
		{
			String[] file=(String[])e.Data.GetData(DataFormats.FileDrop);
			if(IsImage(file[0]))
			{
				e.Effect= DragDropEffects.All;
			}
		}
		
		void MainFormDragDrop(object sender, DragEventArgs e)
		{
			String[] file=(String[])e.Data.GetData(DataFormats.FileDrop);
			string 	imgFile=file[0];
			Bitmap bimg = new Bitmap(imgFile);
			ramka.Image=bimg;
			
		}
		
		void ChkLangSelectedIndexChanged(object sender, EventArgs e)
		{
			string lang = (string)chkLang.SelectedItem;
			if(lang.Equals("ukij")){
				lang = "ukij+uig";
			}
			gOcr= new TesseractEngine(@".\tessdata",lang,EngineMode.LstmOnly);
			Text ="Simple Uyghur OCR using Tessract[V" +  gOcr.Version + "]";
		}
		
		void EnableAll(bool vv){
			butPDF.Enabled = vv;
			butDeskew.Enabled = vv;
			butNext.Enabled = vv;
			butRecognize.Enabled = vv;
			label1.Enabled = vv;
			listBox1.Enabled = vv;
			groupBox1.Enabled = vv;
			groupBox2.Enabled = vv;
			ramka.Enabled = vv;
		}

		
		async void Button3Click(object sender, EventArgs e)
		{
			if(butRecAll.Text.Equals("Stop")){
				butRecAll.Enabled=false;
				butRecAll.Text="Recognize All";
				return;
			}
			listBox1.SelectedIndex = 0;
			progressBar1.Visible = true;
			progressBar1.Value = 1;
			progressBar1.Maximum = listBox1.Items.Count;
			Bitmap roibmp;
			Pix    roipix;

			String fileName = Path.Combine(label1.Text, "OCR_Text.txt");

			if(gOcrTxtForm==null || gOcrTxtForm.IsDisposed){
				gOcrTxtForm=new OCRText();
				gOcrTxtForm.Show();
			}
			gOcrTxtForm.Text = fileName;
			
			EnableAll(false);

			Cursor=Cursors.WaitCursor;
			butRecAll.Text="Stop";
			while(butRecAll.Text.Equals("Stop")){
				Rectangle roi = ramka.getRoi();
				roibmp = ramka.Image.Clone(roi,ramka.Image.PixelFormat);
				roibmp.SetResolution(400,400);				
				roipix = PixConverter.ToPix(roibmp).Deskew().Scale(4.2f,4.2f);
				
				roibmp.Dispose();
				Task<string> ocr = Task.Run<string>(() =>{
				                                    	return DoOCR(roipix);
				                                    });
				string txt = await ocr;
				roipix.Dispose();
				gOcrTxtForm.SetText(txt);
//				fileName = String.Format("imla_ocr_{0:0000}.txt",listBox1.SelectedIndex);
				File.AppendAllText(fileName,txt,Encoding.UTF8);				
				if((listBox1.SelectedIndex+1)>=listBox1.Items.Count){
					break;
				}
				ButtonNextClick(null,null);
				progressBar1.Value = listBox1.SelectedIndex+1;
			}
			progressBar1.Visible = false;
			Cursor=Cursors.Default;
			EnableAll(true);
			butRecAll.Enabled = true;
			butRecAll.Text="Recognize All";
		}
	}
}