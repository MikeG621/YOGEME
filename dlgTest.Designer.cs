namespace Idmr.Yogeme
{
	partial class dlgTest
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgTest));
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdTest = new System.Windows.Forms.Button();
			this.chkDelete = new System.Windows.Forms.CheckBox();
			this.chkDoNotShow = new System.Windows.Forms.CheckBox();
			this.chkVerify = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(187, 101);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 1;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdTest
			// 
			this.cmdTest.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdTest.Location = new System.Drawing.Point(187, 55);
			this.cmdTest.Name = "cmdTest";
			this.cmdTest.Size = new System.Drawing.Size(75, 23);
			this.cmdTest.TabIndex = 0;
			this.cmdTest.Text = "&Run Test";
			this.cmdTest.UseVisualStyleBackColor = true;
			this.cmdTest.Click += new System.EventHandler(this.cmdTest_Click);
			// 
			// chkDelete
			// 
			this.chkDelete.AutoSize = true;
			this.chkDelete.Checked = true;
			this.chkDelete.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDelete.Location = new System.Drawing.Point(12, 82);
			this.chkDelete.Name = "chkDelete";
			this.chkDelete.Size = new System.Drawing.Size(134, 17);
			this.chkDelete.TabIndex = 3;
			this.chkDelete.Text = "&Delete testing pilot files";
			this.chkDelete.UseVisualStyleBackColor = true;
			// 
			// chkDoNotShow
			// 
			this.chkDoNotShow.AutoSize = true;
			this.chkDoNotShow.Location = new System.Drawing.Point(12, 105);
			this.chkDoNotShow.Name = "chkDoNotShow";
			this.chkDoNotShow.Size = new System.Drawing.Size(146, 17);
			this.chkDoNotShow.TabIndex = 4;
			this.chkDoNotShow.Text = "Do not show dialog again";
			this.chkDoNotShow.UseVisualStyleBackColor = true;
			// 
			// chkVerify
			// 
			this.chkVerify.AutoSize = true;
			this.chkVerify.Checked = true;
			this.chkVerify.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVerify.Enabled = false;
			this.chkVerify.Location = new System.Drawing.Point(12, 59);
			this.chkVerify.Name = "chkVerify";
			this.chkVerify.Size = new System.Drawing.Size(105, 17);
			this.chkVerify.TabIndex = 2;
			this.chkVerify.Text = "&Verify before test";
			this.chkVerify.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(250, 43);
			this.label1.TabIndex = 5;
			this.label1.Text = "Select and confirm options here before launching the test. Verify option enabled " +
				"only if \"Verify on Save\" option is disabled";
			// 
			// dlgTest
			// 
			this.AcceptButton = this.cmdTest;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(272, 140);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.chkDoNotShow);
			this.Controls.Add(this.chkVerify);
			this.Controls.Add(this.chkDelete);
			this.Controls.Add(this.cmdTest);
			this.Controls.Add(this.cmdCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "dlgTest";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Confirm Test Options";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdTest;
		private System.Windows.Forms.CheckBox chkDelete;
		private System.Windows.Forms.CheckBox chkDoNotShow;
		private System.Windows.Forms.CheckBox chkVerify;
		private System.Windows.Forms.Label label1;
	}
}