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
			this.pctFormation = new System.Windows.Forms.PictureBox();
			this.chkFormFitPanel = new System.Windows.Forms.CheckBox();
			this.lblSpacing = new System.Windows.Forms.Label();
			this.numFormSpacing = new System.Windows.Forms.NumericUpDown();
			this.chkFormHangar = new System.Windows.Forms.CheckBox();
			this.lblFormInfo = new System.Windows.Forms.Label();
			this.lblCount = new System.Windows.Forms.Label();
			this.numFormCount = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.pctFormation)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numFormSpacing)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numFormCount)).BeginInit();
			this.SuspendLayout();
			// 
			// cboFormation
			// 
			this.cboFormation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFormation.FormattingEnabled = true;
			this.cboFormation.Location = new System.Drawing.Point(89, 528);
			this.cboFormation.Name = "cboFormation";
			this.cboFormation.Size = new System.Drawing.Size(190, 21);
			this.cboFormation.TabIndex = 0;
			this.cboFormation.SelectedIndexChanged += new System.EventHandler(this.cboFormation_SelectedIndexChanged);
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(8, 526);
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
			this.cmdCancel.Location = new System.Drawing.Point(384, 526);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// pctFormation
			// 
			this.pctFormation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.pctFormation.Location = new System.Drawing.Point(0, 0);
			this.pctFormation.Name = "pctFormation";
			this.pctFormation.Size = new System.Drawing.Size(600, 490);
			this.pctFormation.TabIndex = 4;
			this.pctFormation.TabStop = false;
			this.pctFormation.Paint += new System.Windows.Forms.PaintEventHandler(this.pctFormation_Paint);
			// 
			// chkFormFitPanel
			// 
			this.chkFormFitPanel.AutoSize = true;
			this.chkFormFitPanel.Checked = true;
			this.chkFormFitPanel.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFormFitPanel.Location = new System.Drawing.Point(302, 505);
			this.chkFormFitPanel.Name = "chkFormFitPanel";
			this.chkFormFitPanel.Size = new System.Drawing.Size(143, 17);
			this.chkFormFitPanel.TabIndex = 5;
			this.chkFormFitPanel.Text = "Center formation in panel";
			this.chkFormFitPanel.UseVisualStyleBackColor = true;
			this.chkFormFitPanel.CheckedChanged += new System.EventHandler(this.chkFormOrigin_CheckedChanged);
			// 
			// lblSpacing
			// 
			this.lblSpacing.AutoSize = true;
			this.lblSpacing.Location = new System.Drawing.Point(285, 531);
			this.lblSpacing.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.lblSpacing.Name = "lblSpacing";
			this.lblSpacing.Size = new System.Drawing.Size(49, 13);
			this.lblSpacing.TabIndex = 6;
			this.lblSpacing.Text = "Spacing:";
			// 
			// numFormSpacing
			// 
			this.numFormSpacing.Location = new System.Drawing.Point(337, 528);
			this.numFormSpacing.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numFormSpacing.Name = "numFormSpacing";
			this.numFormSpacing.Size = new System.Drawing.Size(41, 20);
			this.numFormSpacing.TabIndex = 7;
			this.numFormSpacing.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.numFormSpacing.ValueChanged += new System.EventHandler(this.numFormSpacing_ValueChanged);
			// 
			// chkFormHangar
			// 
			this.chkFormHangar.AutoSize = true;
			this.chkFormHangar.Location = new System.Drawing.Point(449, 505);
			this.chkFormHangar.Name = "chkFormHangar";
			this.chkFormHangar.Size = new System.Drawing.Size(129, 17);
			this.chkFormHangar.TabIndex = 8;
			this.chkFormHangar.Text = "Show hangar spacing";
			this.chkFormHangar.UseVisualStyleBackColor = true;
			this.chkFormHangar.CheckedChanged += new System.EventHandler(this.chkFormHangar_CheckedChanged);
			// 
			// lblFormInfo
			// 
			this.lblFormInfo.Location = new System.Drawing.Point(350, 40);
			this.lblFormInfo.Name = "lblFormInfo";
			this.lblFormInfo.Size = new System.Drawing.Size(211, 162);
			this.lblFormInfo.TabIndex = 9;
			this.lblFormInfo.Text = "lblFormInfo";
			// 
			// lblCount
			// 
			this.lblCount.AutoSize = true;
			this.lblCount.Location = new System.Drawing.Point(510, 531);
			this.lblCount.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.lblCount.Name = "lblCount";
			this.lblCount.Size = new System.Drawing.Size(38, 13);
			this.lblCount.TabIndex = 10;
			this.lblCount.Text = "Count:";
			// 
			// numFormCount
			// 
			this.numFormCount.Location = new System.Drawing.Point(551, 528);
			this.numFormCount.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
			this.numFormCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numFormCount.Name = "numFormCount";
			this.numFormCount.Size = new System.Drawing.Size(41, 20);
			this.numFormCount.TabIndex = 11;
			this.numFormCount.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.numFormCount.ValueChanged += new System.EventHandler(this.numFormCount_ValueChanged);
			// 
			// FormationDialog
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(604, 561);
			this.Controls.Add(this.numFormCount);
			this.Controls.Add(this.lblCount);
			this.Controls.Add(this.lblFormInfo);
			this.Controls.Add(this.chkFormHangar);
			this.Controls.Add(this.numFormSpacing);
			this.Controls.Add(this.lblSpacing);
			this.Controls.Add(this.chkFormFitPanel);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.cboFormation);
			this.Controls.Add(this.pctFormation);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormationDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FG Formation Dialog";
			((System.ComponentModel.ISupportInitialize)(this.pctFormation)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numFormSpacing)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numFormCount)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cboFormation;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.PictureBox pctFormation;
		private System.Windows.Forms.CheckBox chkFormFitPanel;
		private System.Windows.Forms.Label lblSpacing;
		private System.Windows.Forms.NumericUpDown numFormSpacing;
		private System.Windows.Forms.CheckBox chkFormHangar;
		private System.Windows.Forms.Label lblFormInfo;
		private System.Windows.Forms.Label lblCount;
		private System.Windows.Forms.NumericUpDown numFormCount;
	}
}