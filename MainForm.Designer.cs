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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.butRecognize = new System.Windows.Forms.Button();
            this.butNext = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.ramka = new UyghurOCR.ResimRamka2();
            this.butPDF = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdSingleRow = new System.Windows.Forms.RadioButton();
            this.rdSingle = new System.Windows.Forms.RadioButton();
            this.rdAuto = new System.Windows.Forms.RadioButton();
            this.butDeskew = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tilRusche = new System.Windows.Forms.CheckBox();
            this.tilTurkche = new System.Windows.Forms.CheckBox();
            this.tilEnglizche = new System.Windows.Forms.CheckBox();
            this.tilXenzuche = new System.Windows.Forms.CheckBox();
            this.tilUyghurche = new System.Windows.Forms.CheckBox();
            this.tilUyghurUKIJ = new System.Windows.Forms.CheckBox();
            this.butRecAll = new System.Windows.Forms.Button();
            this.butOpenFolder = new System.Windows.Forms.Button();
            this.copyImage = new System.Windows.Forms.Button();
            this.chkSaveFile = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // butRecognize
            // 
            this.butRecognize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butRecognize.Location = new System.Drawing.Point(859, 637);
            this.butRecognize.Name = "butRecognize";
            this.butRecognize.Size = new System.Drawing.Size(143, 35);
            this.butRecognize.TabIndex = 37;
            this.butRecognize.Text = "Tonu";
            this.butRecognize.UseVisualStyleBackColor = true;
            this.butRecognize.Click += new System.EventHandler(this.ButtonTonu);
            // 
            // butNext
            // 
            this.butNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butNext.Location = new System.Drawing.Point(859, 518);
            this.butNext.Name = "butNext";
            this.butNext.Size = new System.Drawing.Size(143, 35);
            this.butNext.TabIndex = 40;
            this.butNext.Text = "Kéyinkisi";
            this.butNext.UseVisualStyleBackColor = true;
            this.butNext.Visible = false;
            this.butNext.Click += new System.EventHandler(this.ButtonNextClick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(0, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(847, 26);
            this.label1.TabIndex = 41;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 39);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ramka);
            this.splitContainer1.Size = new System.Drawing.Size(847, 651);
            this.splitContainer1.SplitterDistance = 124;
            this.splitContainer1.TabIndex = 42;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(124, 651);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1SelectedIndexChanged);
            // 
            // ramka
            // 
            this.ramka.BackColor = System.Drawing.Color.Gray;
            this.ramka.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ramka.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ramka.Image = null;
            this.ramka.Location = new System.Drawing.Point(0, 0);
            this.ramka.Name = "ramka";
            this.ramka.Size = new System.Drawing.Size(719, 651);
            this.ramka.TabIndex = 34;
            // 
            // butPDF
            // 
            this.butPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butPDF.Location = new System.Drawing.Point(151, 696);
            this.butPDF.Name = "butPDF";
            this.butPDF.Size = new System.Drawing.Size(129, 35);
            this.butPDF.TabIndex = 43;
            this.butPDF.Text = "PDF ni ach";
            this.butPDF.UseVisualStyleBackColor = true;
            this.butPDF.Click += new System.EventHandler(this.ButOpenPDF);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(301, 696);
            this.progressBar1.MinimumSize = new System.Drawing.Size(35, 35);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(402, 35);
            this.progressBar1.TabIndex = 44;
            this.progressBar1.Visible = false;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rdSingleRow);
            this.groupBox1.Controls.Add(this.rdSingle);
            this.groupBox1.Controls.Add(this.rdAuto);
            this.groupBox1.Location = new System.Drawing.Point(853, 265);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 112);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bet Qurulmisi";
            // 
            // rdSingleRow
            // 
            this.rdSingleRow.Checked = true;
            this.rdSingleRow.Location = new System.Drawing.Point(6, 80);
            this.rdSingleRow.Name = "rdSingleRow";
            this.rdSingleRow.Size = new System.Drawing.Size(141, 26);
            this.rdSingleRow.TabIndex = 2;
            this.rdSingleRow.TabStop = true;
            this.rdSingleRow.Text = "Qur";
            this.rdSingleRow.UseVisualStyleBackColor = true;
            this.rdSingleRow.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // rdSingle
            // 
            this.rdSingle.Location = new System.Drawing.Point(6, 50);
            this.rdSingle.Name = "rdSingle";
            this.rdSingle.Size = new System.Drawing.Size(141, 26);
            this.rdSingle.TabIndex = 1;
            this.rdSingle.Text = "Birla Bölek";
            this.rdSingle.UseVisualStyleBackColor = true;
            this.rdSingle.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // rdAuto
            // 
            this.rdAuto.Location = new System.Drawing.Point(6, 20);
            this.rdAuto.Name = "rdAuto";
            this.rdAuto.Size = new System.Drawing.Size(141, 26);
            this.rdAuto.TabIndex = 0;
            this.rdAuto.Text = "Özüng Tap";
            this.rdAuto.UseVisualStyleBackColor = true;
            this.rdAuto.CheckedChanged += new System.EventHandler(this.rd_CheckedChanged);
            // 
            // butDeskew
            // 
            this.butDeskew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butDeskew.Location = new System.Drawing.Point(859, 559);
            this.butDeskew.Name = "butDeskew";
            this.butDeskew.Size = new System.Drawing.Size(143, 35);
            this.butDeskew.TabIndex = 46;
            this.butDeskew.Text = "Qiysiqni tüzle";
            this.butDeskew.UseVisualStyleBackColor = true;
            this.butDeskew.Visible = false;
            this.butDeskew.Click += new System.EventHandler(this.Button2Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tilRusche);
            this.groupBox2.Controls.Add(this.tilTurkche);
            this.groupBox2.Controls.Add(this.tilEnglizche);
            this.groupBox2.Controls.Add(this.tilXenzuche);
            this.groupBox2.Controls.Add(this.tilUyghurche);
            this.groupBox2.Controls.Add(this.tilUyghurUKIJ);
            this.groupBox2.Location = new System.Drawing.Point(853, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(153, 212);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tillar";
            // 
            // tilRusche
            // 
            this.tilRusche.Location = new System.Drawing.Point(5, 182);
            this.tilRusche.Name = "tilRusche";
            this.tilRusche.Size = new System.Drawing.Size(137, 26);
            this.tilRusche.TabIndex = 5;
            this.tilRusche.Text = "Rusche";
            this.tilRusche.UseVisualStyleBackColor = true;
            this.tilRusche.CheckedChanged += new System.EventHandler(this.ChkLangSelectedIndexChanged);
            // 
            // tilTurkche
            // 
            this.tilTurkche.Location = new System.Drawing.Point(5, 117);
            this.tilTurkche.Name = "tilTurkche";
            this.tilTurkche.Size = new System.Drawing.Size(137, 26);
            this.tilTurkche.TabIndex = 4;
            this.tilTurkche.Text = "Turkche";
            this.tilTurkche.UseVisualStyleBackColor = true;
            this.tilTurkche.CheckedChanged += new System.EventHandler(this.ChkLangSelectedIndexChanged);
            // 
            // tilEnglizche
            // 
            this.tilEnglizche.Location = new System.Drawing.Point(5, 84);
            this.tilEnglizche.Name = "tilEnglizche";
            this.tilEnglizche.Size = new System.Drawing.Size(137, 26);
            this.tilEnglizche.TabIndex = 3;
            this.tilEnglizche.Text = "In’glizche";
            this.tilEnglizche.UseVisualStyleBackColor = true;
            this.tilEnglizche.CheckedChanged += new System.EventHandler(this.ChkLangSelectedIndexChanged);
            // 
            // tilXenzuche
            // 
            this.tilXenzuche.Location = new System.Drawing.Point(5, 150);
            this.tilXenzuche.Name = "tilXenzuche";
            this.tilXenzuche.Size = new System.Drawing.Size(137, 26);
            this.tilXenzuche.TabIndex = 2;
            this.tilXenzuche.Text = "Xenzuche";
            this.tilXenzuche.UseVisualStyleBackColor = true;
            this.tilXenzuche.CheckedChanged += new System.EventHandler(this.ChkLangSelectedIndexChanged);
            // 
            // tilUyghurche
            // 
            this.tilUyghurche.Location = new System.Drawing.Point(5, 52);
            this.tilUyghurche.Name = "tilUyghurche";
            this.tilUyghurche.Size = new System.Drawing.Size(137, 26);
            this.tilUyghurche.TabIndex = 1;
            this.tilUyghurche.Text = "Uyghurche";
            this.tilUyghurche.UseVisualStyleBackColor = true;
            this.tilUyghurche.CheckedChanged += new System.EventHandler(this.ChkLangSelectedIndexChanged);
            // 
            // tilUyghurUKIJ
            // 
            this.tilUyghurUKIJ.Location = new System.Drawing.Point(6, 20);
            this.tilUyghurUKIJ.Name = "tilUyghurUKIJ";
            this.tilUyghurUKIJ.Size = new System.Drawing.Size(137, 26);
            this.tilUyghurUKIJ.TabIndex = 0;
            this.tilUyghurUKIJ.Text = "Uyghurche(UKIJ)";
            this.tilUyghurUKIJ.UseVisualStyleBackColor = true;
            this.tilUyghurUKIJ.CheckedChanged += new System.EventHandler(this.ChkLangSelectedIndexChanged);
            // 
            // butRecAll
            // 
            this.butRecAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butRecAll.Location = new System.Drawing.Point(859, 676);
            this.butRecAll.Name = "butRecAll";
            this.butRecAll.Size = new System.Drawing.Size(143, 35);
            this.butRecAll.TabIndex = 48;
            this.butRecAll.Text = "Hemmini tonu";
            this.butRecAll.UseVisualStyleBackColor = true;
            this.butRecAll.Click += new System.EventHandler(this.Button3Click);
            // 
            // butOpenFolder
            // 
            this.butOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butOpenFolder.Location = new System.Drawing.Point(3, 696);
            this.butOpenFolder.Name = "butOpenFolder";
            this.butOpenFolder.Size = new System.Drawing.Size(129, 35);
            this.butOpenFolder.TabIndex = 49;
            this.butOpenFolder.Text = "Qisquchni ach";
            this.butOpenFolder.UseVisualStyleBackColor = true;
            this.butOpenFolder.Click += new System.EventHandler(this.butOpenFolder_Click);
            // 
            // copyImage
            // 
            this.copyImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.copyImage.Location = new System.Drawing.Point(709, 696);
            this.copyImage.Name = "copyImage";
            this.copyImage.Size = new System.Drawing.Size(143, 35);
            this.copyImage.TabIndex = 50;
            this.copyImage.Text = "Resimni köchür";
            this.copyImage.UseVisualStyleBackColor = true;
            this.copyImage.Click += new System.EventHandler(this.copyImage_Click);
            // 
            // chkSaveFile
            // 
            this.chkSaveFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSaveFile.AutoSize = true;
            this.chkSaveFile.Enabled = false;
            this.chkSaveFile.Location = new System.Drawing.Point(890, 714);
            this.chkSaveFile.Name = "chkSaveFile";
            this.chkSaveFile.Size = new System.Drawing.Size(116, 17);
            this.chkSaveFile.TabIndex = 51;
            this.chkSaveFile.Text = "Höjjetkimu saqlisun";
            this.chkSaveFile.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(965, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 52;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            this.pictureBox1.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 736);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.chkSaveFile);
            this.Controls.Add(this.copyImage);
            this.Controls.Add(this.butOpenFolder);
            this.Controls.Add(this.butRecAll);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.butDeskew);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.butPDF);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butNext);
            this.Controls.Add(this.butRecognize);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Addiy Uyghurche OCR";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.Shown += new System.EventHandler(this.MainFormShown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainFormDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainFormDragEnter);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private UyghurOCR.ResimRamka2 ramka;
		private System.Windows.Forms.Button butRecognize;
		private System.Windows.Forms.Button butNext;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button butPDF;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button butDeskew;
		private System.Windows.Forms.RadioButton rdSingle;
		private System.Windows.Forms.RadioButton rdAuto;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button butRecAll;
		private System.Windows.Forms.CheckBox tilRusche;
		private System.Windows.Forms.CheckBox tilTurkche;
		private System.Windows.Forms.CheckBox tilEnglizche;
		private System.Windows.Forms.CheckBox tilXenzuche;
		private System.Windows.Forms.CheckBox tilUyghurche;
		private System.Windows.Forms.CheckBox tilUyghurUKIJ;
        private System.Windows.Forms.Button butOpenFolder;
        private System.Windows.Forms.RadioButton rdSingleRow;
        private System.Windows.Forms.Button copyImage;
        private System.Windows.Forms.CheckBox chkSaveFile;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}