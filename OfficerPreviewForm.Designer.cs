namespace Idmr.Yogeme
{
	partial class OfficerPreviewForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OfficerPreviewForm));
			this.cmdClose = new System.Windows.Forms.Button();
			this.pctPreview = new System.Windows.Forms.PictureBox();
			this.cmdPrevious = new System.Windows.Forms.Button();
			this.cmdNext = new System.Windows.Forms.Button();
			this.optPreOff = new System.Windows.Forms.RadioButton();
			this.optPostOff = new System.Windows.Forms.RadioButton();
			this.optPreSec = new System.Windows.Forms.RadioButton();
			this.optPostSec = new System.Windows.Forms.RadioButton();
			this.tmrBlink = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pctPreview)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdClose
			// 
			this.cmdClose.Location = new System.Drawing.Point(545, 406);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(75, 23);
			this.cmdClose.TabIndex = 0;
			this.cmdClose.Text = "&Close";
			this.cmdClose.UseVisualStyleBackColor = true;
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// pctPreview
			// 
			this.pctPreview.BackColor = System.Drawing.Color.Black;
			this.pctPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.pctPreview.Location = new System.Drawing.Point(0, 0);
			this.pctPreview.Name = "pctPreview";
			this.pctPreview.Size = new System.Drawing.Size(640, 400);
			this.pctPreview.TabIndex = 1;
			this.pctPreview.TabStop = false;
			this.pctPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pctPreview_Paint);
			this.pctPreview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pctPreview_MouseUp);
			// 
			// cmdPrevious
			// 
			this.cmdPrevious.Enabled = false;
			this.cmdPrevious.Location = new System.Drawing.Point(12, 406);
			this.cmdPrevious.Name = "cmdPrevious";
			this.cmdPrevious.Size = new System.Drawing.Size(91, 23);
			this.cmdPrevious.TabIndex = 2;
			this.cmdPrevious.Text = "&Previous Page";
			this.cmdPrevious.UseVisualStyleBackColor = true;
			this.cmdPrevious.Click += new System.EventHandler(this.cmdPrevious_Click);
			// 
			// cmdNext
			// 
			this.cmdNext.Enabled = false;
			this.cmdNext.Location = new System.Drawing.Point(109, 406);
			this.cmdNext.Name = "cmdNext";
			this.cmdNext.Size = new System.Drawing.Size(91, 23);
			this.cmdNext.TabIndex = 3;
			this.cmdNext.Text = "&Next Page";
			this.cmdNext.UseVisualStyleBackColor = true;
			this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
			// 
			// optPreOff
			// 
			this.optPreOff.AutoSize = true;
			this.optPreOff.Location = new System.Drawing.Point(206, 400);
			this.optPreOff.Name = "optPreOff";
			this.optPreOff.Size = new System.Drawing.Size(112, 17);
			this.optPreOff.TabIndex = 4;
			this.optPreOff.TabStop = true;
			this.optPreOff.Text = "Pre-mission Officer";
			this.optPreOff.UseVisualStyleBackColor = true;
			// 
			// optPostOff
			// 
			this.optPostOff.AutoSize = true;
			this.optPostOff.Location = new System.Drawing.Point(354, 400);
			this.optPostOff.Name = "optPostOff";
			this.optPostOff.Size = new System.Drawing.Size(117, 17);
			this.optPostOff.TabIndex = 6;
			this.optPostOff.TabStop = true;
			this.optPostOff.Text = "Post-mission Officer";
			this.optPostOff.UseVisualStyleBackColor = true;
			// 
			// optPreSec
			// 
			this.optPreSec.AutoSize = true;
			this.optPreSec.Location = new System.Drawing.Point(206, 418);
			this.optPreSec.Name = "optPreSec";
			this.optPreSec.Size = new System.Drawing.Size(141, 17);
			this.optPreSec.TabIndex = 5;
			this.optPreSec.TabStop = true;
			this.optPreSec.Text = "Pre-mission Secret Order";
			this.optPreSec.UseVisualStyleBackColor = true;
			// 
			// optPostSec
			// 
			this.optPostSec.AutoSize = true;
			this.optPostSec.Location = new System.Drawing.Point(354, 418);
			this.optPostSec.Name = "optPostSec";
			this.optPostSec.Size = new System.Drawing.Size(146, 17);
			this.optPostSec.TabIndex = 7;
			this.optPostSec.TabStop = true;
			this.optPostSec.Text = "Post-mission Secret Order";
			this.optPostSec.UseVisualStyleBackColor = true;
			// 
			// tmrBlink
			// 
			this.tmrBlink.Enabled = true;
			this.tmrBlink.Interval = 50;
			this.tmrBlink.Tick += new System.EventHandler(this.tmrBlink_Tick);
			// 
			// OfficerPreviewForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(642, 438);
			this.Controls.Add(this.optPostOff);
			this.Controls.Add(this.optPostSec);
			this.Controls.Add(this.optPreSec);
			this.Controls.Add(this.optPreOff);
			this.Controls.Add(this.cmdNext);
			this.Controls.Add(this.cmdPrevious);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.pctPreview);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "OfficerPreviewForm";
			this.Text = "Officer Questions Preview";
			((System.ComponentModel.ISupportInitialize)(this.pctPreview)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.PictureBox pctPreview;
		private System.Windows.Forms.Button cmdPrevious;
		private System.Windows.Forms.Button cmdNext;
		private System.Windows.Forms.RadioButton optPreOff;
		private System.Windows.Forms.RadioButton optPostOff;
		private System.Windows.Forms.RadioButton optPreSec;
		private System.Windows.Forms.RadioButton optPostSec;
		private System.Windows.Forms.Timer tmrBlink;
	}
}