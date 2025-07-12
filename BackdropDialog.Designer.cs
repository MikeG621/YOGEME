namespace Idmr.Yogeme
{
	partial class BackdropDialog
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackdropDialog));
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.pctBackdrop = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.numShadow = new System.Windows.Forms.NumericUpDown();
			this.numBackdrop = new System.Windows.Forms.NumericUpDown();
			this.vsbThumbs = new System.Windows.Forms.VScrollBar();
			this.pnlThumbs = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblWindow = new System.Windows.Forms.Label();
			this.lblImage = new System.Windows.Forms.Label();
			this.lblColor = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.pctSample = new System.Windows.Forms.PictureBox();
			this.label6 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pctBackdrop)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numShadow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numBackdrop)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pctSample)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(614, 204);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 9;
			this.cmdOK.Text = "&OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(614, 245);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 10;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// pctBackdrop
			// 
			this.pctBackdrop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pctBackdrop.Location = new System.Drawing.Point(322, 12);
			this.pctBackdrop.Name = "pctBackdrop";
			this.pctBackdrop.Size = new System.Drawing.Size(256, 256);
			this.pctBackdrop.TabIndex = 1;
			this.pctBackdrop.TabStop = false;
			this.pctBackdrop.Click += new System.EventHandler(this.pctBackdrop_Click);
			this.pctBackdrop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pctBackdrop_MouseMove);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(584, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Backdrop:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(584, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(90, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Shadow / Variant";
			// 
			// numShadow
			// 
			this.numShadow.Enabled = false;
			this.numShadow.Location = new System.Drawing.Point(686, 38);
			this.numShadow.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.numShadow.Name = "numShadow";
			this.numShadow.Size = new System.Drawing.Size(33, 20);
			this.numShadow.TabIndex = 11;
			this.numShadow.ValueChanged += new System.EventHandler(this.numShadow_ValueChanged);
			// 
			// numBackdrop
			// 
			this.numBackdrop.Location = new System.Drawing.Point(672, 12);
			this.numBackdrop.Name = "numBackdrop";
			this.numBackdrop.Size = new System.Drawing.Size(47, 20);
			this.numBackdrop.TabIndex = 11;
			this.numBackdrop.ValueChanged += new System.EventHandler(this.numBackdrop_ValueChanged);
			// 
			// vsbThumbs
			// 
			this.vsbThumbs.Enabled = false;
			this.vsbThumbs.LargeChange = 2;
			this.vsbThumbs.Location = new System.Drawing.Point(303, 11);
			this.vsbThumbs.Maximum = 17;
			this.vsbThumbs.Name = "vsbThumbs";
			this.vsbThumbs.Size = new System.Drawing.Size(16, 241);
			this.vsbThumbs.TabIndex = 13;
			this.vsbThumbs.ValueChanged += new System.EventHandler(this.vsbThumbs_ValueChanged);
			// 
			// pnlThumbs
			// 
			this.pnlThumbs.Location = new System.Drawing.Point(12, 12);
			this.pnlThumbs.Name = "pnlThumbs";
			this.pnlThumbs.Size = new System.Drawing.Size(288, 240);
			this.pnlThumbs.TabIndex = 14;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(584, 69);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(70, 13);
			this.label3.TabIndex = 15;
			this.label3.Text = "Window size:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(584, 95);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 13);
			this.label4.TabIndex = 15;
			this.label4.Text = "Image size:";
			// 
			// lblWindow
			// 
			this.lblWindow.AutoSize = true;
			this.lblWindow.Location = new System.Drawing.Point(656, 69);
			this.lblWindow.Name = "lblWindow";
			this.lblWindow.Size = new System.Drawing.Size(48, 13);
			this.lblWindow.TabIndex = 15;
			this.lblWindow.Text = "256x256";
			// 
			// lblImage
			// 
			this.lblImage.AutoSize = true;
			this.lblImage.Location = new System.Drawing.Point(656, 95);
			this.lblImage.Name = "lblImage";
			this.lblImage.Size = new System.Drawing.Size(60, 13);
			this.lblImage.TabIndex = 15;
			this.lblImage.Text = "1024x1024";
			// 
			// lblColor
			// 
			this.lblColor.AutoSize = true;
			this.lblColor.Location = new System.Drawing.Point(624, 120);
			this.lblColor.Name = "lblColor";
			this.lblColor.Size = new System.Drawing.Size(31, 13);
			this.lblColor.TabIndex = 16;
			this.lblColor.Text = "0 0 0";
			this.lblColor.Visible = false;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(584, 120);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(34, 13);
			this.label5.TabIndex = 17;
			this.label5.Text = "Color:";
			this.label5.Visible = false;
			// 
			// pctSample
			// 
			this.pctSample.Location = new System.Drawing.Point(697, 115);
			this.pctSample.Name = "pctSample";
			this.pctSample.Size = new System.Drawing.Size(22, 22);
			this.pctSample.TabIndex = 18;
			this.pctSample.TabStop = false;
			this.pctSample.Visible = false;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(584, 143);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(134, 13);
			this.label6.TabIndex = 17;
			this.label6.Text = "Color is copied to clipboard";
			this.label6.Visible = false;
			// 
			// BackdropDialog
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(728, 275);
			this.Controls.Add(this.pctSample);
			this.Controls.Add(this.lblColor);
			this.Controls.Add(this.lblImage);
			this.Controls.Add(this.lblWindow);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.pnlThumbs);
			this.Controls.Add(this.vsbThumbs);
			this.Controls.Add(this.numBackdrop);
			this.Controls.Add(this.numShadow);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.pctBackdrop);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BackdropDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Backdrop Dialog";
			((System.ComponentModel.ISupportInitialize)(this.pctBackdrop)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numShadow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numBackdrop)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pctSample)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox pctBackdrop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numShadow;
        private System.Windows.Forms.NumericUpDown numBackdrop;
		private System.Windows.Forms.VScrollBar vsbThumbs;
		private System.Windows.Forms.Panel pnlThumbs;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblWindow;
		private System.Windows.Forms.Label lblImage;
		private System.Windows.Forms.Label lblColor;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.PictureBox pctSample;
        private System.Windows.Forms.Label label6;
    }
}