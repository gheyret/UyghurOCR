/*
 * Created by SharpDevelop.
 * User: nk1449
 * Date: 2017/05/29
 * Time: 11:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using OpenCvSharp;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;
namespace UyghurOCR
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form, IMessageFilter
    {
		Random      grand = new System.Random();
		TesseractEngine         gOcr;
		OCRText  gOcrTxtForm = null;
		private  Microsoft.Win32.RegistryKey     gRegKey= Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Kenjisoft\KenjiOCR");
		
		static string gImgExts;
        private enum HALET {QISQUCH, PDF};

		private HALET _halet;
        
        PdfDocument _pdfDoc = null;
        TextDetector _lineDetector = null;
        float _img_dpi = 300.0f;
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
            rd_CheckedChanged(null, null);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x0100)
            {
                Keys mk = (Keys)m.WParam.ToInt32();
                return Kunupkilar(mk);
            }
            else
            {
                return false;
            }
        }

        bool Kunupkilar(Keys mk)
        {
            bool ret = false;
			if (mk == Keys.Down)
			{
				int ind = listBox1.SelectedIndex;
				ind = ind + 1;
				if (ind < listBox1.Items.Count)
				{
					listBox1.SelectedIndex = ind;
				}
				ret = true;
			}
			else if (mk == Keys.Up)
			{
				int ind = listBox1.SelectedIndex;
				ind = ind - 1;
				if (ind >= 0)
				{
					listBox1.SelectedIndex = ind;
				}
				ret = true;
			}
			else if (mk == Keys.V && ModifierKeys == Keys.Control)
			{
				ret = Chapla();
			}
			else if (mk == Keys.C && ModifierKeys == Keys.Control)
			{
				System.Drawing.Rectangle roi = ramka.getRoi();
				Bitmap roibmp = ramka.Image.Clone(roi, ramka.Image.PixelFormat);
				Clipboard.SetImage(roibmp);
				ret = true;
			}
			else if (mk == Keys.Delete)
			{
				ramka.DeleteSelectedRectangle();
				ret = true;
			}
			return ret;
        }

        bool Chapla()
        {
            bool b = false;
            IDataObject dataObject = Clipboard.GetDataObject();
            if (dataObject == null) return b;
            if (dataObject.GetDataPresent(DataFormats.Bitmap))
            {
                Bitmap img = (Bitmap)dataObject.GetData(DataFormats.Bitmap);
                ramka.Image = img;
                b = true;
            }
            return b;
        }


        bool IsImage(string filename)
		{
			string ext = Path.GetExtension(filename);
			if (ext.Equals(""))
			{
				return false;
			}
			else
			{
				return gImgExts.IndexOf(ext, StringComparison.OrdinalIgnoreCase) == -1 ? false : true;
			}
		}

        void MainFormShown(object sender, EventArgs e)
        {
            String lastdir = null;
            if (gRegKey != null)
            {
                lastdir = (string)gRegKey.GetValue("LAST");
				label1.Text = lastdir;
            }
            if (lastdir == null)
            {
                label1.Text = Path.Combine(Application.StartupPath, "sinaq_resim");
            }
			listAllImg();
            tilUyghurUKIJ.Checked = true;
        }

        void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(gRegKey!=null){
				gRegKey.SetValue("LAST",label1.Text);
			}
            if (_pdfDoc != null)
            {
                _pdfDoc.Dispose();
            }
			if (_lineDetector != null)
			{
				_lineDetector.Dispose();
			}
            Application.RemoveMessageFilter(this);
        }

        void ChkLangSelectedIndexChanged(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			string lang = "";
			char[] tr = {'+'};
			if(tilUyghurUKIJ.Checked){
				lang += "ukij";
			}

			if(tilUyghurche.Checked){
				lang += "+uig";
			}

			if(tilEnglizche.Checked){
				lang += "+eng";
			}
			
			if(tilTurkche.Checked){
				lang += "+tur";
			}

			if(tilXenzuche.Checked){
				lang += "+chi_sim";
			}

			if(tilRusche.Checked){
				lang += "+rus";
			}
			lang = lang.Trim(tr);
			
			System.Diagnostics.Debug.WriteLine(lang);
			if(gOcr!=null){
				gOcr.Dispose();
				gOcr = null;
			}
			
			if(lang.Length == 0)
			{
				MessageBox.Show("Hech qandaq tilni tallimidingiz","Uyghurche OCR Agahlandurush",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				butRecAll.Enabled = false;
				butRecognize.Enabled = false;
			}
			else
			{
				butRecAll.Enabled = true;
				butRecognize.Enabled = true;
				gOcr= new TesseractEngine(@".\tessdata",lang,EngineMode.LstmOnly);
				//gOcr.SetVariable("user_defined_dpi","300");
				Text ="Addiy Uyghurche OCR(Tesseract OCR[V" +  gOcr.Version + "] Neshri Ishlitilgen)";
			}
			this.Cursor = Cursors.Default;
			Invalidate();
			
		}
		
		async void ButtonTonu(object sender, EventArgs e)
		{
			EnableAll(false);
			Pix    roipix;
			string txt;
			Cursor=Cursors.WaitCursor;
			var selrect = ramka.getRoi();
			var roiimg = ramka.Image.Clone(selrect, ramka.Image.PixelFormat);
            roipix = PixConverter.ToPix(roiimg).Scale(1.0f,1.0f).Deskew();

			if (rdSingleRow.Checked)
			{
				txt = LineRecognition(roipix);
			}
			else
			{
				Task<string> ocr = Task.Run<string>(() => { return DoOCR(roipix); });
				txt = await ocr;
			}
			roipix.Dispose();
			roiimg.Dispose();
			if(gOcrTxtForm==null || gOcrTxtForm.IsDisposed){
				gOcrTxtForm=new OCRText(label1.Text);
				gOcrTxtForm.Show();
			}
			gOcrTxtForm.SetText(txt);
			
			Cursor=Cursors.Default;
			EnableAll(true);
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
			buf = buf.Replace("ی", "ي").Replace("ه", "ە").Replace("\n", Environment.NewLine);
			return buf;
        }

		string abzasla(string ocrtext)
		{
			char[] trch = { ' ', '-' };
			string[] qurlar = ocrtext.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			StringBuilder buf = new StringBuilder();
			int otturcheQur = 0;
			int cnt = 0;
			string tmp;

			//foreach(string qur in qurlar)
			if (qurlar.Length > 3)
			{
				for (int i = 1; i < qurlar.Length - 1; i++)
				{
					tmp = qurlar[i].Trim();
					if (tmp.Length > 10)
					{
						otturcheQur += tmp.Length;
						cnt++;
					}
				}

				otturcheQur = otturcheQur / cnt;

				foreach (string qur in qurlar)
				{
					tmp = qur.Trim();
					if (tmp.Length == 0)
					{
						continue;
					}

                    if (qur.Length > (0.8 * otturcheQur))
					{
                        tmp = qur.Trim();
                        if (tmp.EndsWith("-"))
						{
							buf.Append(tmp.TrimEnd(trch).TrimEnd());
						}
						else
						{
							buf.Append(tmp + " ");
						}
					}
					else
					{
                        tmp += Environment.NewLine;
                        buf.Append(tmp);
					}
				}
				return buf.ToString();
			}
			return ocrtext;
		}


        string BirlaAbzas(List<string> qurlar)
        {
            char[] trch = { ' ', '-' };
            StringBuilder buf = new StringBuilder();
            foreach(string qur in qurlar) {
				if (qur.EndsWith("-"))
				{
					buf.Append(qur.TrimEnd(trch).TrimEnd());
				}
				else 
				{ 
					buf.Append(qur.TrimEnd(trch).TrimEnd() + " ");
				}
            }
            return buf.ToString();
        }

        void MainFormLoad(object sender, EventArgs e)
		{
            Application.AddMessageFilter(this);
        }
		
		
		void listAllImg(){
			listBox1.Items.Clear();
			if (Directory.Exists(label1.Text))
			{
				_halet = HALET.QISQUCH;
            }
			else if (System.IO.File.Exists(label1.Text))
			{
				if (label1.Text.ToLower().EndsWith(".pdf"))
				{
					_halet = HALET.PDF;
                }
				else
				{
                    _halet = HALET.QISQUCH;
					label1.Text = Path.GetDirectoryName(label1.Text);
                }
            }
            else
            {
                _halet = HALET.QISQUCH;
                label1.Text = Path.Combine(Application.StartupPath, "sinaq_resim");
            }

			if (_halet == HALET.QISQUCH && Directory.Exists(label1.Text))
			{
				String[] images = Directory.GetFiles(label1.Text, "*.*");
				listBox1.BeginUpdate();
				foreach (string afile in images)
				{
					if (IsImage(afile))
					{
						listBox1.Items.Add(Path.GetFileName(afile));
					}
				}
				listBox1.EndUpdate();
			}
			else if(_halet == HALET.PDF)
			{
				if (_pdfDoc != null)
				{
					_pdfDoc.Dispose();
				}

				try
				{
                    _pdfDoc = PdfDocument.Load(label1.Text);

					listBox1.BeginUpdate();
                    int pgCnt = _pdfDoc.PageCount;
                    String fileNn;
                    for (int i = 0; i < pgCnt; i++)
					{
						fileNn = String.Format("Bet_{0:0000}.png", (i + 1));
						listBox1 .Items.Add(fileNn);
					}
					listBox1.EndUpdate();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "UyghurOCR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
            }

            if (listBox1.Items.Count>0){
				listBox1.SelectedIndex = 0;
			}
		}
		

		void ButtonNextClick(object sender, EventArgs e)
		{
			int index = listBox1.SelectedIndex;
			index = index +1;
			if(index<listBox1.Items.Count){
				listBox1.SelectedIndex = index;
			}
		}
		
		void ListBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_halet == HALET.QISQUCH)
			{
				String path = Path.Combine(label1.Text, (string)listBox1.SelectedItem);
				Bitmap bimg = new Bitmap(path);
				ramka.Image = bimg;
			}
			else
			{
                Image img = _pdfDoc.Render(listBox1.SelectedIndex, _img_dpi, _img_dpi, PdfRenderFlags.CorrectFromDpi);
                ramka.Image = (Bitmap)img;
            }
        }

        void ButOpenPDF(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "PDF hojjetning isimini tallang";
            ofd.Filter = "PDF file(*.pdf)|*.pdf";
            if (ofd.ShowDialog() != DialogResult.OK) return;
			label1.Text = ofd.FileName;
			listAllImg();
        }


  //      void ButPdftoImageClick(object sender, EventArgs e)
		//{
		//	OpenFileDialog ofd=new OpenFileDialog();
		//	ofd.Title="PDF hojjetning atini tallang";
		//	ofd.Filter ="PDF file(*.pdf)|*.pdf";
		//	if(ofd.ShowDialog()!= DialogResult.OK) return;

		//	String pdfFile=ofd.FileName;
		//	String path=System.IO.Path.GetDirectoryName(pdfFile)+"\\"+System.IO.Path.GetFileNameWithoutExtension(pdfFile);
		//	if(Directory.Exists(path)==false){
		//		Directory.CreateDirectory(path);
		//	}

		//	EnableAll(false);
		//	this.Cursor=System.Windows.Forms.Cursors.WaitCursor;
		//	if(ExtractImage(pdfFile,path)){
		//		label1.Text = path;
		//		listAllImg();
		//	}
		//	this.Cursor=System.Windows.Forms.Cursors.Arrow;
		//	EnableAll(true);
		//}
		
//		private  bool ExtractImage(String pdfFile, string imgPath)
//		{
//			bool ret = true;
//			progressBar1.Visible = true;
//			try{
//				String fileNn;
//				using (MuPDFContext ctx = new MuPDFContext())
//				{
//					using (MuPDFDocument pdfdoc = new MuPDFDocument(ctx, pdfFile))
//					{
//						int pgCnt = pdfdoc.Pages.Count;
//                        progressBar1.Minimum = 0;
//						progressBar1.Maximum = pgCnt;

//						double zoomLevel = 300.0 / 72.0; //Save Image as 300 DPI
//						for (int i = 0; i < pgCnt; i++)
//						{
//							fileNn = String.Format("Bet_{0:0000}.png", (i + 1));
//							fileNn = Path.Combine(imgPath, fileNn);
//							pdfdoc.SaveImage(i, zoomLevel, PixelFormats.RGB, fileNn, RasterOutputFileTypes.PNG);
//							progressBar1.Value = i;
//							Application.DoEvents();
//						}
//					}
//				}
//			}catch(Exception ee){
//				MessageBox.Show(ee.Message,"UyghurOCR",MessageBoxButtons.OK,MessageBoxIcon.Error);
////				System.Diagnostics.Debug.WriteLine(ee.StackTrace);
//				ret = false;
//			}
//            progressBar1.Visible = false;
//            return ret;
//		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Bitmap roibmp;
			Pix    roipix;
			EnableAll(false);
			System.Drawing.Rectangle roi = ramka.getRoi();
			Cursor=Cursors.WaitCursor;
			roibmp = ramka.Image.Clone(roi,ramka.Image.PixelFormat);
			roipix = PixConverter.ToPix(roibmp); //.Deskew();
			roibmp.Dispose();

//			roipix = PixConverter.ToPix(roibmp);
			System.Diagnostics.Debug.WriteLine(roipix.XRes + " = " + roipix.YRes);
			Bitmap newbm = PixConverter.ToBitmap(roipix);
			roipix.Dispose();
			ramka.Image = newbm;
			Cursor=Cursors.Default;
			EnableAll(true);
		}
		
		void MainFormDragEnter(object sender, DragEventArgs e)
		{
			String[] file=(String[])e.Data.GetData(DataFormats.FileDrop);
			FileAttributes fattr = File.GetAttributes(file[0]);
			if(IsImage(file[0])|| file[0].ToLower().EndsWith(".pdf")|| fattr.HasFlag(FileAttributes.Directory))
			{
				e.Effect= DragDropEffects.All;
			}
		}
		
		void MainFormDragDrop(object sender, DragEventArgs e)
		{
			String[] file=(String[])e.Data.GetData(DataFormats.FileDrop);
			string 	dFile=file[0];
			if (IsImage(dFile))
			{
				Bitmap bimg = new Bitmap(dFile);
				ramka.Image = bimg;
			}
			else
			{
				label1.Text = dFile;
				listAllImg();
            }
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
			butRecAll.Enabled = vv;
			copyImage.Enabled = vv;
			butOpenFolder.Enabled = vv;	
		}

		async void Button3Click(object sender, EventArgs e)
		{
			if(butRecAll.Text.Equals("Toxta")){
				butRecAll.Enabled=false;
				butRecAll.Text="Hemmini Tonu";
				return;
			}
			listBox1.SelectedIndex = 0;
			progressBar1.Visible = true;
			progressBar1.Value = 1;
			progressBar1.Maximum = listBox1.Items.Count;
			chkSaveFile.Enabled = true;
			Pix    roipix;


			if (gOcrTxtForm==null || gOcrTxtForm.IsDisposed){
				gOcrTxtForm=new OCRText(label1.Text);
				gOcrTxtForm.Show();
			}

			String dirName;
			if (File.Exists(label1.Text) || Directory.Exists(label1.Text))
			{
				FileAttributes attr = File.GetAttributes(label1.Text);
				if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
				{
					dirName = label1.Text;
				}
				else
				{
					dirName = Path.GetDirectoryName(label1.Text);
				}
			}
			else
			{
				dirName = "";
			}

			EnableAll(false);
			
			Cursor=Cursors.WaitCursor;
			butRecAll.Text="Toxta";
			butRecAll.Enabled = true;
			string txt;
			String fileName;
			string aItem;
            while (butRecAll.Text.Equals("Toxta"))
			{
                var roi = ramka.getRoi();
                roipix = PixConverter.ToPix(ramka.Image).Scale(1.0f,1.0f).Deskew();
                if (rdSingleRow.Checked)
				{
                    Task<string> ocr = Task.Run<string>(() =>
                    {
                        return LineRecognition(roipix);
                    });
                    txt = await ocr;
                }
				else
				{
					Task<string> ocr = Task.Run<string>(() =>
					{
						return DoOCR(roipix);
					});
					txt = await ocr;
				}
                roipix.Dispose();
                gOcrTxtForm.SetText(txt);

				if (chkSaveFile.Checked)
				{
					aItem = listBox1.SelectedItem.ToString();
					aItem = Path.GetFileNameWithoutExtension(aItem) + ".txt";
					fileName = Path.Combine(dirName, aItem);
					File.AppendAllText(fileName, txt, Encoding.UTF8);
				}

                if ((listBox1.SelectedIndex+1)>=listBox1.Items.Count){
					break;
				}
				ButtonNextClick(null,null);
				progressBar1.Value = listBox1.SelectedIndex+1;
			}
			progressBar1.Visible = false;
            chkSaveFile.Enabled = false;
            Cursor =Cursors.Default;
			EnableAll(true);
			butRecAll.Text="Hemmini Tonu";
		}

		private string LineRecognition(Pix dsPix)
		{
			List<String> qurlar = new List<string>();
			String qur;

            gOcr.DefaultPageSegMode = PageSegMode.RawLine;
            using (Bitmap dskBitmap = PixConverter.ToBitmap(dsPix))
            {
				//Graphics g = Graphics.FromImage(dskBitmap);
                using (Mat _input_mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(dskBitmap))
                {
                    var orgW = _input_mat.Width;
                    var orgH = _input_mat.Height;
                    var (quads, scores) = _lineDetector.Detect(_input_mat);
                    //Pen qurPen = new Pen(Color.Blue, 2);
                    quads = quads.OrderBy(q => q[0].Y).ToList();
                    foreach (var quad in quads)
                    {
                        int minX = quad.Min(p => p.X);
                        int minY = quad.Min(p => p.Y);
                        int maxX = quad.Max(p => p.X);
                        int maxY = quad.Max(p => p.Y);

                        if ((maxX + 5) < orgW)
                        {
                            maxX += 5;
                        }
                        if ((maxY + 5) < orgH)
                        {
                            maxY += 5;
                        }
						var rect = new Tesseract.Rect(minX, minY, maxX - minX, maxY - minY);
						Page pg = gOcr.Process(dsPix,rect,pageSegMode:PageSegMode.RawLine);
                        qur = pg.GetText();
                        pg.Dispose();
                        qur = qur.Replace("ی", "ي").Replace("ه", "ە").Trim();
						qurlar.Add(qur);
						//g.DrawRectangle(qurPen, minX, minY, maxX - minX, maxY - minY);
                    }
                }
				//g.Dispose();
				//dskBitmap.Save("sinaq.png");
            }
			return BirlaAbzas(qurlar)+System.Environment.NewLine + System.Environment.NewLine;
        }



        private void butOpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = label1.Text;
            dlg.Description = "Resimler bar qisquchni tallang:";
            dlg.ShowNewFolderButton = false;

            DialogResult drs = dlg.ShowDialog(this);
            if (drs == DialogResult.OK)
            {
                label1.Text = dlg.SelectedPath;
                listAllImg();
            }
        }

        private void rd_CheckedChanged(object sender, EventArgs e)
        {
			if (_lineDetector == null)
			{
				_lineDetector = new TextDetector();
			}
        }

        private void copyImage_Click(object sender, EventArgs e)
        {
			ramka.CopytoclipBorad();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
			if (ramka.Image == null)
			{
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                Bitmap tugh = new Bitmap(asm.GetManifestResourceStream("UyghurOCR.tugh.png"));
				ramka.Image = tugh;
            }
            bool status = ramka.Image != null;
            butRecognize.Enabled = status;
			butRecognize.Enabled = status;
			copyImage.Enabled = status;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
			pictureBox1.BorderStyle = BorderStyle.FixedSingle;
			pictureBox1.Cursor = Cursors.Hand;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            Bitmap tugh = new Bitmap(asm.GetManifestResourceStream("UyghurOCR.tugh.png"));
            ramka.Image = tugh;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
			pictureBox1.BorderStyle = BorderStyle.None;
            pictureBox1.Cursor = Cursors.Default;
        }
    }
}