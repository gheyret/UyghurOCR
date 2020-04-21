/*
 * Created by SharpDevelop.
 * User: nk1449
 * Date: 2018/12/19
 * Time: 16:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UyghurOCR
{
	/// <summary>
	/// Description of OCRText.
	/// </summary>
	public partial class OCRText : Form
	{
		public OCRText()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			textBox1.Text="";
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public void SetText(string ocrtext){
			this.textBox1.AppendText(ocrtext);
			//this.textBox1.SelectionStart = this.textBox1.Text.Length;
			//this.textBox1.SelectionLength = 0;
		}
		void Button1Click(object sender, EventArgs e)
		{
			Clipboard.SetDataObject(this.textBox1.Text);
		}
	}
}
