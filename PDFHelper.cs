/*
 * Created by SharpDevelop.
 * User: nk1449
 * Date: 2017/07/06
 * Time: 13:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;
using System.Windows.Forms;

namespace UyghurOCR
{
	/// <summary>
	/// Description of PDFHelper.
	/// </summary>
	
	public class PDFHelper : IRenderListener
	{
		int      _imgNo = 1;
		string   _outputFolder;
		int      _currentPage=1;
		string   _imgname;
		int      _pageRotation;
		
		public PDFHelper()
		{
		}

		
		
		public bool ExtractImages(string pdfPath, string outputFolder, ProgressBar prBar)
		{
			_outputFolder = outputFolder;
			_imgname=System.IO.Path.GetFileNameWithoutExtension(pdfPath);
			try{
				using (PdfReader pdfReader = new PdfReader(pdfPath))
				{
					if (pdfReader.IsEncrypted())
						throw new ApplicationException( pdfPath + " is encrypted.");

					PdfReaderContentParser pdfParser = new PdfReaderContentParser(pdfReader);

					prBar.Maximum=pdfReader.NumberOfPages;
					prBar.Minimum=1;
					prBar.Visible=true;

					while (_currentPage <= pdfReader.NumberOfPages)
					{
						_imgNo = 1;
						prBar.Value=_currentPage;
						Application.DoEvents();
						_pageRotation=pdfReader.GetPageRotation(_currentPage);
						pdfParser.ProcessContent(_currentPage, this);
						_currentPage++;
					}
				}
				prBar.Visible=false;
				return true;
			}catch(Exception ee){
				throw new Exception("PDF ni resimge aylandurushtiki xataliq", ee);
			}
		}

		public void BeginTextBlock() { }
		public void EndTextBlock() { }
		public void RenderText(TextRenderInfo renderInfo) { }

		public void RenderImage(ImageRenderInfo renderInfo)
		{
			PdfImageObject pdfimage = renderInfo.GetImage();
			string pp= pdfimage.GetImageBytesType().FileExtension;
			
			string imgtp=pdfimage.GetFileType();
			if("jpg".Equals(imgtp,StringComparison.OrdinalIgnoreCase)||
			   "png".Equals(imgtp,StringComparison.OrdinalIgnoreCase)||
			   "gif".Equals(imgtp,StringComparison.OrdinalIgnoreCase)){
				
			}
			else{
				imgtp=pp;
				imgtp="png";
			}
			String imageFileName = String.Format("{0}_{1:000}_{2}.{3}", _imgname , _currentPage, _imgNo, imgtp);
//			imageFileName= _outputFolder+"\\"+_imgname+_currentPage.ToString("_000")+"_"+_imgNo+".png";
			imageFileName= _outputFolder+"\\"+imageFileName;
			try{
				using (Image dotnetImg = pdfimage.GetDrawingImage())
				{
					if (dotnetImg != null)
					{
						if(_pageRotation==270){
							dotnetImg.RotateFlip(RotateFlipType.Rotate270FlipNone);
						}
						else if(_pageRotation==90){
							dotnetImg.RotateFlip(RotateFlipType.Rotate90FlipNone);
						}
						else if(_pageRotation==180){
							dotnetImg.RotateFlip(RotateFlipType.Rotate180FlipNone);
						}
						dotnetImg.Save(imageFileName);
					}
				}
			}catch(Exception ee){
				System.Diagnostics.Debug.WriteLine(ee.StackTrace);
			}
			_imgNo++;
		}
		
		public void RenderImageOrg(ImageRenderInfo renderInfo)
		{
			PdfImageObject pdfimage = renderInfo.GetImage();
			string imgtp=pdfimage.GetFileType();
			String imageFileName = String.Format("{0}_{1:000}_{2}.{3}", _imgname , _currentPage, _imgNo, imgtp);
			imageFileName= _outputFolder+"\\"+imageFileName;			
			var imageRawBytes = pdfimage.GetImageAsBytes();
			File.WriteAllBytes(imageFileName, imageRawBytes);
			_imgNo++;
		}
		
		
		private  bool ExtractImageOLD(String pdfFile, string imgPath, ProgressBar progressBar1)
		{
			bool ret=true;
			int imgNo=1;
//			string pdfFile=@"F:\gheyret\kitablar\Matitey we anarqiz pajiesi.pdf";
//			string imgPath=@"F:\gheyret\kitab_suret\Hazirqi Zaman Uyghur Tili Gramatikisi\";
			string imgname=System.IO.Path.GetFileNameWithoutExtension(pdfFile);
			progressBar1.Visible=true;
			progressBar1.Minimum=1;
			try{
				PdfDictionary pg,tg;
				PdfDictionary res;
				PdfDictionary xobj;
				PdfImageObject pdfimage;
				PdfObject     lpdfobjW;
				PdfObject     lpdfobjH;
				ImageRenderInfo imgRI;
				string width,height;
				int       pageRotation;
				PdfReader pdfReader = new PdfReader(pdfFile);
				progressBar1.Maximum=pdfReader.NumberOfPages;
				for (int pageNumber = 1; pageNumber <= pdfReader.NumberOfPages; pageNumber++)
				{
//					if(gStop) break;
//					pg = pdfReader.GetPageN(pageNumber);
					pg = pdfReader.GetPageNRelease(pageNumber);
					res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
					xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
					imgNo=1;
					pageRotation=pdfReader.GetPageRotation(pageNumber);
					foreach (PdfName name in xobj.Keys)
					{
						PdfObject obj = xobj.Get(name);
						if (obj.IsIndirect())
						{
							tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
							lpdfobjW=tg.Get(PdfName.WIDTH);
							lpdfobjH=tg.Get(PdfName.HEIGHT);
							if(lpdfobjW!=null && lpdfobjH!=null){
								width = lpdfobjW.ToString();
								height = lpdfobjH.ToString();
//								imgRI = ImageRenderInfo.CreateForXObject(new Matrix(float.Parse(width), float.Parse(height)), (PRIndirectReference)obj, tg);
								imgRI = ImageRenderInfo.CreateForXObject(new GraphicsState(), (PRIndirectReference)obj, tg);
								pdfimage = imgRI.GetImage();
								ExtractImageOLD(pdfimage,imgPath+"\\"+imgname+pageNumber.ToString("_000")+"_"+imgNo+".png",pageRotation);
								imgNo++;
							}
						}
					}
					progressBar1.Value=pageNumber;
					Application.DoEvents();
				}
				pdfReader.Close();
			}
			catch(Exception ee){
//				MessageBox.Show(ee.Message,"Xataliq koruldi");
				System.Diagnostics.Debug.WriteLine(ee.StackTrace);
				ret=false;
			}
			progressBar1.Visible=false;
			return ret;
		}
		
		private void ExtractImageOLD(PdfImageObject pdfimage,string path, int pageRotation)
		{
			using (Image dotnetImg = pdfimage.GetDrawingImage())
			{
				if (dotnetImg != null)
				{
					if(pageRotation==270){
						dotnetImg.RotateFlip(RotateFlipType.Rotate270FlipNone);
					}
					else if(pageRotation==90){
						dotnetImg.RotateFlip(RotateFlipType.Rotate90FlipNone);
					}
					else if(pageRotation==180){
						dotnetImg.RotateFlip(RotateFlipType.Rotate180FlipNone);
					}
					dotnetImg.Save(path);
				}
			}
		}
		
	}
}
