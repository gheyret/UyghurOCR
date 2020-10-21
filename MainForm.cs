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
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Tesseract;
using System.Drawing;
using System.Threading.Tasks;

namespace UyghurOCR
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		Random      grand = new System.Random();
		Double item0, item1, item2;
		long   itemcnt;
		TesseractEngine         gOcr;
		OCRText  gOcrTxtForm = null;

		private  Microsoft.Win32.RegistryKey     gRegKey= Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Kenjsoft\KenjiResim");
		
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			ramka.MousePoint = this.MousePoint;
			ramka.Selected = SelectionChanged;
			ramka.RightMouseClick = RightClick;
			
			System.Reflection.Assembly asm =System.Reflection.Assembly.GetExecutingAssembly();
			this.Icon=new Icon(asm.GetManifestResourceStream("UyghurOCR.icon.ico"));
			
			//TestDynamicJson();
		}
		
		
		
		void RightClick(int x, int y){
			Vec3b color;
			Mat src = ramka.Image;
			color = src.Get<Vec3b>(y, x);
			String rgb =String.Format("RGB:({0},{1},{2})",color.Item2,color.Item1,color.Item0);
			
			item0 = color.Item0;
			item1 = color.Item1;
			item2 = color.Item2;
			
		}
		
		
		unsafe void OpVec3b(Vec3b *bt, int *pos){
			byte r,g,b;
			r = (*bt).Item2;
			g = (*bt).Item1;
			b = (*bt).Item0;
			
			//if ( r> 80 && g > 80 && b > 80)
			{
				itemcnt++;
				item0 +=r;
				item1 +=g;
				item2 +=b;
			}
		}
		
		
		unsafe  public void SelectionChanged(Mat roi){
			System.Diagnostics.Debug.WriteLine("Tallandi");
			itemcnt = 0;
			item0 = 0;
			item1 = 0;
			item2 = 0;
			//roi.ForEachAsVec3b(OpVec3b);
			Vec3b color;
			for( int y = 0; y< roi.Height;y++){
				for(int x =0; x< roi.Width; x++){
					color = roi.Get<Vec3b>(y, x);
					if(color.Item0>80 && color.Item1 > 80 && color.Item2>80){
						itemcnt ++;
						item0 += color.Item0;
						item1 += color.Item1;
						item2 += color.Item2;
					}
				}
			}
			if(itemcnt == 0)
				return;
			
			item0 = item0/itemcnt;
			item1 = item1/itemcnt;
			item2 = item2/itemcnt;
			String rgb =String.Format("RGB:({0:0},{1:0},{2:0})",item2,item1,item0);
			
			//test();
			
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			Cv2.DestroyAllWindows();
			if(gRegKey!=null){
				gRegKey.SetValue("LAST",label1.Text);
			}
		}
		
		
		void MousePoint(int x, int y){
			Vec3b color;
			Mat src = ramka.Image;
			color = src.Get<Vec3b>(y, x);
			String rgb =String.Format("RGB:({0},{1},{2})",color.Item2,color.Item1,color.Item0);
			//label3.Text = rgb;
			
		}
		
		async void ButtonRight(object sender, EventArgs e)
		{
			buttonRight.Enabled = false;
			buttonNext.Enabled = false;
			button2.Enabled = false;
			
			Bitmap roibmp;
			Pix    roipix;
			OpenCvSharp.Rect roi = ramka.getRoi();
			Mat    src;
			Cursor=Cursors.WaitCursor;
			if(roi.X != -1)
			{
				src = ramka.Image[roi];
			}
			else{
				src = ramka.Image;
			}
			
			roibmp = src.ToBitmap();
			roipix = PixConverter.ToPix(roibmp);

			Task<string> ocr = Task.Run<string>(() =>{
			                                    	return DoOCR(roipix);
			                                    });
			string txt = await ocr;
			System.Diagnostics.Debug.WriteLine(txt);
			if(gOcrTxtForm==null || gOcrTxtForm.IsDisposed){
				gOcrTxtForm=new OCRText();
				gOcrTxtForm.Show();
			}
			gOcrTxtForm.SetText(txt);
			buttonRight.Enabled = true;
			buttonNext.Enabled = true;
			button2.Enabled = true;
			Cursor=Cursors.Default;
			roibmp.Dispose();
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
			string baseName;
			listBox1.Items.Clear();
			String[] images = Directory.GetFiles(label1.Text,"*.*");
			foreach(string afile in images){
				baseName = Path.GetFileName(afile).ToLower();
				if(baseName.EndsWith(".png") || baseName.EndsWith(".jpg")|| baseName.EndsWith(".jpeg")||baseName.EndsWith(".bmp")){
					listBox1.Items.Add(Path.GetFileName(afile));
				}
			}
			if(listBox1.Items.Count>0){
				listBox1.SelectedIndex = 0;
				//ShowImg();
			}
		}
		
		void ShowImg(){
			Mat src;
			String path = Path.Combine(label1.Text, (string)listBox1.SelectedItem);
			byte[] bmp = File.ReadAllBytes(path);
			src = Mat.FromImageData(bmp,ImreadModes.Unchanged);
			ramka.Image=src;
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

			button1.Enabled=false;
			this.Cursor=System.Windows.Forms.Cursors.WaitCursor;
			if(ExtractImage(pdfFile,path)){
				label1.Text = path;
				listAllImg();
			}
			button1.Enabled=true;
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
			Mat    src;
			buttonRight.Enabled = false;
			buttonNext.Enabled = false;
			button2.Enabled = false;
			OpenCvSharp.Rect roi = ramka.getRoi();
			Cursor=Cursors.WaitCursor;
			if(roi.X != -1)
			{
				src = ramka.Image[roi];
			}
			else{
				src = ramka.Image;
			}
			roibmp = src.ToBitmap();
			roipix = PixConverter.ToPix(roibmp);
			Pix roipix1 = roipix.Deskew();
			
			Bitmap newbm = PixConverter.ToBitmap(roipix1);
			Mat newmat = BitmapConverter.ToMat(newbm);
			ramka.Image = newmat;
			Cursor=Cursors.Default;
			src.Dispose();
			newbm.Dispose();
			roipix.Dispose();
			buttonRight.Enabled = true;
			buttonNext.Enabled = true;
			button2.Enabled = true;
		}
		
		void MainFormDragEnter(object sender, DragEventArgs e)
		{
			String[] file=(String[])e.Data.GetData(DataFormats.FileDrop);
			String  baseName = Path.GetFileName(file[0]).ToLower();
			if(baseName.EndsWith(".png") || baseName.EndsWith(".jpg")|| baseName.EndsWith(".jpeg")||baseName.EndsWith(".bmp"))
			{
				e.Effect= DragDropEffects.All;
			}
		}
		
		void MainFormDragDrop(object sender, DragEventArgs e)
		{
			String[] file=(String[])e.Data.GetData(DataFormats.FileDrop);
			string 	imgFile=file[0];
			Mat src;
			byte[] bmp = File.ReadAllBytes(imgFile);
			src = Mat.FromImageData(bmp,ImreadModes.Unchanged);
			ramka.Image=src;
			
		}
		
		void ChkLangSelectedIndexChanged(object sender, EventArgs e)
		{
			string lang = (string)chkLang.SelectedItem;
			gOcr= new TesseractEngine(@".\tessdata",lang,EngineMode.LstmOnly);
			Text ="Simple Uyghur OCR using Tessract[V" +  gOcr.Version + "]";			
		}
	}
}