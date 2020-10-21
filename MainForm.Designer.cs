/*
 * Created by SharpDevelop.
 * User: nk1449
 * Date: 2017/05/29
 * Time: 11:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace UyghurOCR
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.ramka = new UyghurOCR.ResimRamka();
			this.buttonRight = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rdSingle = new System.Windows.Forms.RadioButton();
			this.rdAuto = new System.Windows.Forms.RadioButton();
			this.button2 = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkLang = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// ramka
			// 
			this.ramka.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ramka.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ramka.Image = null;
			this.ramka.Location = new System.Drawing.Point(0, 0);
			this.ramka.Name = "ramka";
			this.ramka.Size = new System.Drawing.Size(547, 463);
			this.ramka.TabIndex = 34;
			// 
			// buttonRight
			// 
			this.buttonRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRight.Location = new System.Drawing.Point(743, 467);
			this.buttonRight.Name = "buttonRight";
			this.buttonRight.Size = new System.Drawing.Size(143, 32);
			this.buttonRight.TabIndex = 37;
			this.buttonRight.Text = "Recognize";
			this.buttonRight.UseVisualStyleBackColor = true;
			this.buttonRight.Click += new System.EventHandler(this.ButtonRight);
			// 
			// buttonNext
			// 
			this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonNext.Location = new System.Drawing.Point(743, 505);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(143, 32);
			this.buttonNext.TabIndex = 40;
			this.buttonNext.Text = "Next";
			this.buttonNext.UseVisualStyleBackColor = true;
			this.buttonNext.Click += new System.EventHandler(this.ButtonNextClick);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(12, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(879, 24);
			this.label1.TabIndex = 41;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label1.Click += new System.EventHandler(this.Label1Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(0, 36);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listBox1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.ramka);
			this.splitContainer1.Size = new System.Drawing.Size(732, 463);
			this.splitContainer1.SplitterDistance = 181;
			this.splitContainer1.TabIndex = 42;
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.IntegralHeight = false;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point(0, 0);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(181, 463);
			this.listBox1.TabIndex = 0;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1SelectedIndexChanged);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(0, 505);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(181, 32);
			this.button1.TabIndex = 43;
			this.button1.Text = "PDF to Image";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.ButPdftoImageClick);
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.progressBar1.Location = new System.Drawing.Point(187, 505);
			this.progressBar1.MinimumSize = new System.Drawing.Size(431, 32);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(545, 32);
			this.progressBar1.TabIndex = 44;
			this.progressBar1.Visible = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.rdSingle);
			this.groupBox1.Controls.Add(this.rdAuto);
			this.groupBox1.Location = new System.Drawing.Point(738, 89);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(153, 117);
			this.groupBox1.TabIndex = 45;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Segmentation mode";
			// 
			// rdSingle
			// 
			this.rdSingle.Location = new System.Drawing.Point(6, 58);
			this.rdSingle.Name = "rdSingle";
			this.rdSingle.Size = new System.Drawing.Size(141, 24);
			this.rdSingle.TabIndex = 1;
			this.rdSingle.TabStop = true;
			this.rdSingle.Text = "Single block";
			this.rdSingle.UseVisualStyleBackColor = true;
			// 
			// rdAuto
			// 
			this.rdAuto.Checked = true;
			this.rdAuto.Location = new System.Drawing.Point(6, 28);
			this.rdAuto.Name = "rdAuto";
			this.rdAuto.Size = new System.Drawing.Size(141, 24);
			this.rdAuto.TabIndex = 0;
			this.rdAuto.TabStop = true;
			this.rdAuto.Text = "Automatic";
			this.rdAuto.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(743, 429);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(143, 32);
			this.button2.TabIndex = 46;
			this.button2.Text = "Deskew";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.chkLang);
			this.groupBox2.Location = new System.Drawing.Point(738, 30);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(153, 51);
			this.groupBox2.TabIndex = 47;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Model";
			// 
			// chkLang
			// 
			this.chkLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.chkLang.FormattingEnabled = true;
			this.chkLang.Location = new System.Drawing.Point(6, 18);
			this.chkLang.Name = "chkLang";
			this.chkLang.Size = new System.Drawing.Size(137, 20);
			this.chkLang.TabIndex = 0;
			this.chkLang.SelectedIndexChanged += new System.EventHandler(this.ChkLangSelectedIndexChanged);
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(893, 542);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.buttonRight);
			this.Name = "MainForm";
			this.Text = "Simple Uyghur OCR using Tessract";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.Shown += new System.EventHandler(this.MainFormShown);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainFormDragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainFormDragEnter);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		private UyghurOCR.ResimRamka ramka;
		private System.Windows.Forms.Button buttonRight;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.RadioButton rdSingle;
		private System.Windows.Forms.RadioButton rdAuto;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox chkLang;
	}
}