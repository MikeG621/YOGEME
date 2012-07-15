namespace Idmr.Yogeme
{
	partial class dlgAbout
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgAbout));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblVersion = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.linkGE = new System.Windows.Forms.LinkLabel();
			this.linkIdmr = new System.Windows.Forms.LinkLabel();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.cmdClose = new System.Windows.Forms.Button();
			this.lnkMail = new System.Windows.Forms.LinkLabel();
			this.label6 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
			this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.pictureBox1.Location = new System.Drawing.Point(12, 6);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(471, 62);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
			// 
			// pictureBox2
			// 
			this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
			this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.pictureBox2.Location = new System.Drawing.Point(383, 74);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(100, 100);
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(19, 74);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(323, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Ye Olde Galactic Empire Mission Editor";
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.Location = new System.Drawing.Point(20, 94);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(45, 13);
			this.lblVersion.TabIndex = 3;
			this.lblVersion.Text = "Version ";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(20, 110);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Produced by";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(20, 126);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Copyright by";
			// 
			// linkGE
			// 
			this.linkGE.AutoSize = true;
			this.linkGE.LinkColor = System.Drawing.Color.Blue;
			this.linkGE.Location = new System.Drawing.Point(91, 126);
			this.linkGE.Name = "linkGE";
			this.linkGE.Size = new System.Drawing.Size(179, 13);
			this.linkGE.TabIndex = 5;
			this.linkGE.TabStop = true;
			this.linkGE.Text = "The Galactic Empire: Empire Reborn";
			this.linkGE.VisitedLinkColor = System.Drawing.Color.Blue;
			this.linkGE.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGE_LinkClicked);
			// 
			// linkIdmr
			// 
			this.linkIdmr.AutoSize = true;
			this.linkIdmr.LinkColor = System.Drawing.Color.Blue;
			this.linkIdmr.Location = new System.Drawing.Point(91, 110);
			this.linkIdmr.Name = "linkIdmr";
			this.linkIdmr.Size = new System.Drawing.Size(219, 13);
			this.linkIdmr.TabIndex = 5;
			this.linkIdmr.TabStop = true;
			this.linkIdmr.Text = "The Imperial Department of Military Research";
			this.linkIdmr.VisitedLinkColor = System.Drawing.Color.Blue;
			this.linkIdmr.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkIdmr_LinkClicked);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(20, 142);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(162, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Lead Designer: Imperial Officer";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(20, 158);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(345, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "This product is not affiliated with LucasArts Entertainment Company LLC";
			// 
			// cmdClose
			// 
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdClose.Location = new System.Drawing.Point(78, 231);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(75, 23);
			this.cmdClose.TabIndex = 0;
			this.cmdClose.Text = "Close";
			this.cmdClose.UseVisualStyleBackColor = true;
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// lnkMail
			// 
			this.lnkMail.AutoSize = true;
			this.lnkMail.LinkColor = System.Drawing.Color.Blue;
			this.lnkMail.Location = new System.Drawing.Point(178, 142);
			this.lnkMail.Name = "lnkMail";
			this.lnkMail.Size = new System.Drawing.Size(59, 13);
			this.lnkMail.TabIndex = 7;
			this.lnkMail.TabStop = true;
			this.lnkMail.Text = "Jagged Fel";
			this.lnkMail.VisitedLinkColor = System.Drawing.Color.Blue;
			this.lnkMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMail_LinkClicked);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(20, 174);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(428, 13);
			this.label6.TabIndex = 8;
			this.label6.Text = "Many thanks to the programmers and modders before me who made much of this possib" +
				"le";
			// 
			// dlgAbout
			// 
			this.AcceptButton = this.cmdClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdClose;
			this.ClientSize = new System.Drawing.Size(495, 282);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.lnkMail);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.linkIdmr);
			this.Controls.Add(this.linkGE);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "dlgAbout";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel linkGE;
		private System.Windows.Forms.LinkLabel linkIdmr;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.LinkLabel lnkMail;
		private System.Windows.Forms.Label label6;
	}
}