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
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace UyghurOCR
{
	/// <summary>
	/// Description of OCRText.
	/// </summary>
	public partial class OCRText : Form
	{
        string _nishan;
		public OCRText(string hojjetyakiqisquch)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			textBox1.Text="";
            _nishan = hojjetyakiqisquch;
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            Text = "Menbe: ["+ _nishan + "]";
        }
		
		public void SetText(string ocrtext)
		{
			this.textBox1.AppendText(ocrtext);
		}
		void Button1Click(object sender, EventArgs e)
		{
			Clipboard.SetDataObject(this.textBox1.Text);
		}

        private void butTazila_Click(object sender, EventArgs e)
        {
			textBox1.Text = "";
        }

        private void butSaqla_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Tékist höjjiti (*.txt)|*.txt|Barliq höjjet (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.Title = "Tékist Höjjitini Saqlash";
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.InitialDirectory = Directory.Exists(_nishan)? _nishan:Path.GetDirectoryName(_nishan);
                saveFileDialog.FileName = "ocr_netijisi.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, textBox1.Text, System.Text.Encoding.UTF8);
                        MessageBox.Show("Höjjet saqlandi", "Utuqluq boldi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Xataliq körüldi:\n{ex.Message}", "Xataliq", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
