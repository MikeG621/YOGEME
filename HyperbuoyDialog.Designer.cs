namespace Idmr.Yogeme
{
	partial class HyperbuoyDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HyperbuoyDialog));
			this.lstBuoys = new System.Windows.Forms.ListBox();
			this.cmdClose = new System.Windows.Forms.Button();
			this.cmdGenerate = new System.Windows.Forms.Button();
			this.chkReturn = new System.Windows.Forms.CheckBox();
			this.cboFrom = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cboTo = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lstBuoys
			// 
			this.lstBuoys.FormattingEnabled = true;
			this.lstBuoys.Items.AddRange(new object[] {
            "FG Name only (T/F Region)"});
			this.lstBuoys.Location = new System.Drawing.Point(12, 26);
			this.lstBuoys.Name = "lstBuoys";
			this.lstBuoys.Size = new System.Drawing.Size(219, 225);
			this.lstBuoys.TabIndex = 0;
			// 
			// cmdClose
			// 
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdClose.Location = new System.Drawing.Point(471, 208);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(75, 32);
			this.cmdClose.TabIndex = 1;
			this.cmdClose.Text = "&Close";
			this.cmdClose.UseVisualStyleBackColor = true;
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// cmdGenerate
			// 
			this.cmdGenerate.Location = new System.Drawing.Point(240, 203);
			this.cmdGenerate.Name = "cmdGenerate";
			this.cmdGenerate.Size = new System.Drawing.Size(75, 43);
			this.cmdGenerate.TabIndex = 2;
			this.cmdGenerate.Text = "&Generate";
			this.cmdGenerate.UseVisualStyleBackColor = true;
			this.cmdGenerate.Click += new System.EventHandler(this.cmdGenerate_Click);
			// 
			// chkReturn
			// 
			this.chkReturn.AutoSize = true;
			this.chkReturn.Location = new System.Drawing.Point(240, 26);
			this.chkReturn.Name = "chkReturn";
			this.chkReturn.Size = new System.Drawing.Size(107, 17);
			this.chkReturn.TabIndex = 3;
			this.chkReturn.Text = "Create return pair";
			this.chkReturn.UseVisualStyleBackColor = true;
			// 
			// cboFrom
			// 
			this.cboFrom.FormattingEnabled = true;
			this.cboFrom.Location = new System.Drawing.Point(313, 56);
			this.cboFrom.Name = "cboFrom";
			this.cboFrom.Size = new System.Drawing.Size(165, 21);
			this.cboFrom.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(237, 59);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "From Region:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(237, 85);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "To Region:";
			// 
			// cboTo
			// 
			this.cboTo.FormattingEnabled = true;
			this.cboTo.Location = new System.Drawing.Point(313, 82);
			this.cboTo.Name = "cboTo";
			this.cboTo.Size = new System.Drawing.Size(165, 21);
			this.cboTo.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(237, 117);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(320, 70);
			this.label3.TabIndex = 6;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 10);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(102, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Existing Hyperbuoys";
			// 
			// HyperbuoyDialog
			// 
			this.AcceptButton = this.cmdClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdClose;
			this.ClientSize = new System.Drawing.Size(575, 263);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboTo);
			this.Controls.Add(this.cboFrom);
			this.Controls.Add(this.chkReturn);
			this.Controls.Add(this.cmdGenerate);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.lstBuoys);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HyperbuoyDialog";
			this.Text = "HyperbuoyDialog";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lstBuoys;
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Button cmdGenerate;
		private System.Windows.Forms.CheckBox chkReturn;
		private System.Windows.Forms.ComboBox cboFrom;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cboTo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
	}
}