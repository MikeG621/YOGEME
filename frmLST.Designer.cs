namespace Idmr.Yogeme
{
	partial class frmLST
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLST));
			this.cmdSave = new System.Windows.Forms.Button();
			this.cboFile = new System.Windows.Forms.ComboBox();
			this.txtLST = new System.Windows.Forms.TextBox();
			this.lblEx = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmdSave
			// 
			this.cmdSave.Location = new System.Drawing.Point(222, 22);
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(65, 21);
			this.cmdSave.TabIndex = 2;
			this.cmdSave.Text = "&Save";
			this.cmdSave.UseVisualStyleBackColor = true;
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// cboFile
			// 
			this.cboFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFile.FormattingEnabled = true;
			this.cboFile.Location = new System.Drawing.Point(15, 22);
			this.cboFile.Name = "cboFile";
			this.cboFile.Size = new System.Drawing.Size(191, 21);
			this.cboFile.TabIndex = 0;
			this.cboFile.SelectedIndexChanged += new System.EventHandler(this.cboFile_SelectedIndexChanged);
			// 
			// txtLST
			// 
			this.txtLST.Location = new System.Drawing.Point(16, 66);
			this.txtLST.Multiline = true;
			this.txtLST.Name = "txtLST";
			this.txtLST.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtLST.Size = new System.Drawing.Size(271, 309);
			this.txtLST.TabIndex = 1;
			// 
			// lblEx
			// 
			this.lblEx.Location = new System.Drawing.Point(293, 71);
			this.lblEx.Name = "lblEx";
			this.lblEx.Size = new System.Drawing.Size(101, 304);
			this.lblEx.TabIndex = 3;
			this.lblEx.Text = "Example:\r\n\r\n//\r\n[Section Title]\r\n//\r\n0\r\nm1.tie\r\nMission description\r\n1\r\nm2.tie\r\nK" +
				"eep to 1 line\r\n//\r\n[Next Section]\r\n//\r\n2\r\nm3.tie\r\netc";
			// 
			// frmLST
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(396, 388);
			this.Controls.Add(this.lblEx);
			this.Controls.Add(this.txtLST);
			this.Controls.Add(this.cboFile);
			this.Controls.Add(this.cmdSave);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmLST";
			this.Text = "YOGEME LST Editor";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdSave;
		private System.Windows.Forms.ComboBox cboFile;
		private System.Windows.Forms.TextBox txtLST;
		private System.Windows.Forms.Label lblEx;
	}
}