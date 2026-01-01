/*
 * Created by SharpDevelop.
 * User: nk1449
 * Date: 2018/12/19
 * Time: 16:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace UyghurOCR
{
	partial class OCRText
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.butSaqla = new System.Windows.Forms.Button();
            this.butTazila = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("UKIJ Tuz", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 13);
            this.textBox1.MaxLength = 0;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(618, 411);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(12, 431);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "Köchür";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // butSaqla
            // 
            this.butSaqla.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butSaqla.Location = new System.Drawing.Point(122, 431);
            this.butSaqla.Name = "butSaqla";
            this.butSaqla.Size = new System.Drawing.Size(104, 25);
            this.butSaqla.TabIndex = 2;
            this.butSaqla.Text = "Saqla";
            this.butSaqla.UseVisualStyleBackColor = true;
            this.butSaqla.Click += new System.EventHandler(this.butSaqla_Click);
            // 
            // butTazila
            // 
            this.butTazila.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butTazila.Location = new System.Drawing.Point(526, 431);
            this.butTazila.Name = "butTazila";
            this.butTazila.Size = new System.Drawing.Size(104, 25);
            this.butTazila.TabIndex = 3;
            this.butTazila.Text = "Tazila";
            this.butTazila.UseVisualStyleBackColor = true;
            this.butTazila.Click += new System.EventHandler(this.butTazila_Click);
            // 
            // OCRText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 468);
            this.Controls.Add(this.butTazila);
            this.Controls.Add(this.butSaqla);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OCRText";
            this.Text = "OCRText";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private System.Windows.Forms.Button butSaqla;
        private System.Windows.Forms.Button butTazila;
    }
}
