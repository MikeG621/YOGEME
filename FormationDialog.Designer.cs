namespace Idmr.Yogeme
{
	partial class FormationDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormationDialog));
			this.cboFormation = new System.Windows.Forms.ComboBox();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.pctFormation = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pctFormation)).BeginInit();
			this.SuspendLayout();
			// 
			// cboFormation
			// 
			this.cboFormation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFormation.FormattingEnabled = true;
			this.cboFormation.Location = new System.Drawing.Point(96, 276);
			this.cboFormation.Name = "cboFormation";
			this.cboFormation.Size = new System.Drawing.Size(190, 21);
			this.cboFormation.TabIndex = 0;
			this.cboFormation.SelectedIndexChanged += new System.EventHandler(this.cboFormation_SelectedIndexChanged);
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(15, 274);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 1;
			this.cmdOK.Text = "&OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(292, 276);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 254);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(297, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Formations are shown with Spacing=2 and LeaderDistance=0";
			// 
			// pctFormation
			// 
			this.pctFormation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.pctFormation.Location = new System.Drawing.Point(12, 12);
			this.pctFormation.Name = "pctFormation";
			this.pctFormation.Size = new System.Drawing.Size(500, 239);
			this.pctFormation.TabIndex = 4;
			this.pctFormation.TabStop = false;
			// 
			// dlgFormation
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(526, 298);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.cboFormation);
			this.Controls.Add(this.pctFormation);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "dlgFormation";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FG Formation Dialog";
			((System.ComponentModel.ISupportInitialize)(this.pctFormation)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cboFormation;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pctFormation;
	}
}