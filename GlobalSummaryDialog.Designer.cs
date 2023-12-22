namespace Idmr.Yogeme
{
	partial class GlobalSummaryDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalSummaryDialog));
			this.cmdClose = new System.Windows.Forms.Button();
			this.txtSummary = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// cmdClose
			// 
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdClose.Location = new System.Drawing.Point(125, 452);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(58, 19);
			this.cmdClose.TabIndex = 4;
			this.cmdClose.Text = "Close";
			this.cmdClose.UseVisualStyleBackColor = true;
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// txtSummary
			// 
			this.txtSummary.Location = new System.Drawing.Point(12, 12);
			this.txtSummary.Multiline = true;
			this.txtSummary.Name = "txtSummary";
			this.txtSummary.ReadOnly = true;
			this.txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSummary.Size = new System.Drawing.Size(284, 434);
			this.txtSummary.TabIndex = 3;
			// 
			// GlobalSummaryDialog
			// 
			this.AcceptButton = this.cmdClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdClose;
			this.ClientSize = new System.Drawing.Size(312, 472);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.txtSummary);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "GlobalSummaryDialog";
			this.Text = "Global Summary";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.TextBox txtSummary;
	}
}