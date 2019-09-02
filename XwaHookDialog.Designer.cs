namespace Idmr.Yogeme
{
	partial class XwaHookDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XwaHookDialog));
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.grpBackdrops = new System.Windows.Forms.GroupBox();
			this.cmdRemoveBD = new System.Windows.Forms.Button();
			this.cmdAddBD = new System.Windows.Forms.Button();
			this.lstBackdrops = new System.Windows.Forms.ListBox();
			this.chkBackdrops = new System.Windows.Forms.CheckBox();
			this.opnBackdrop = new System.Windows.Forms.OpenFileDialog();
			this.grpMission = new System.Windows.Forms.GroupBox();
			this.txtPilot = new System.Windows.Forms.TextBox();
			this.lstMission = new System.Windows.Forms.ListBox();
			this.optPilot = new System.Windows.Forms.RadioButton();
			this.optIff = new System.Windows.Forms.RadioButton();
			this.optMarkings = new System.Windows.Forms.RadioButton();
			this.cmdRemoveMiss = new System.Windows.Forms.Button();
			this.cmdAddMiss = new System.Windows.Forms.Button();
			this.cboIff = new System.Windows.Forms.ComboBox();
			this.cboMarkings = new System.Windows.Forms.ComboBox();
			this.cboFG = new System.Windows.Forms.ComboBox();
			this.chkMission = new System.Windows.Forms.CheckBox();
			this.grpSounds = new System.Windows.Forms.GroupBox();
			this.cmdRemoveSounds = new System.Windows.Forms.Button();
			this.cmdAddSounds = new System.Windows.Forms.Button();
			this.lstSounds = new System.Windows.Forms.ListBox();
			this.chkSounds = new System.Windows.Forms.CheckBox();
			this.opnSounds = new System.Windows.Forms.OpenFileDialog();
			this.grpObjects = new System.Windows.Forms.GroupBox();
			this.cmdRemoveObjects = new System.Windows.Forms.Button();
			this.cmdAddObjects = new System.Windows.Forms.Button();
			this.lstObjects = new System.Windows.Forms.ListBox();
			this.chkObjects = new System.Windows.Forms.CheckBox();
			this.opnObjects = new System.Windows.Forms.OpenFileDialog();
			this.grpBackdrops.SuspendLayout();
			this.grpMission.SuspendLayout();
			this.grpSounds.SuspendLayout();
			this.grpObjects.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(180, 462);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 0;
			this.cmdOK.Text = "&OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(465, 432);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 0;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(604, 219);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(47, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Hangars";
			// 
			// grpBackdrops
			// 
			this.grpBackdrops.Controls.Add(this.cmdRemoveBD);
			this.grpBackdrops.Controls.Add(this.cmdAddBD);
			this.grpBackdrops.Controls.Add(this.lstBackdrops);
			this.grpBackdrops.Controls.Add(this.chkBackdrops);
			this.grpBackdrops.Location = new System.Drawing.Point(12, 12);
			this.grpBackdrops.Name = "grpBackdrops";
			this.grpBackdrops.Size = new System.Drawing.Size(332, 96);
			this.grpBackdrops.TabIndex = 2;
			this.grpBackdrops.TabStop = false;
			this.grpBackdrops.Text = "Backdrops";
			// 
			// cmdRemoveBD
			// 
			this.cmdRemoveBD.Enabled = false;
			this.cmdRemoveBD.Location = new System.Drawing.Point(264, 42);
			this.cmdRemoveBD.Name = "cmdRemoveBD";
			this.cmdRemoveBD.Size = new System.Drawing.Size(60, 23);
			this.cmdRemoveBD.TabIndex = 2;
			this.cmdRemoveBD.Text = "&Remove";
			this.cmdRemoveBD.UseVisualStyleBackColor = true;
			this.cmdRemoveBD.Click += new System.EventHandler(this.cmdRemoveBD_Click);
			// 
			// cmdAddBD
			// 
			this.cmdAddBD.Enabled = false;
			this.cmdAddBD.Location = new System.Drawing.Point(198, 42);
			this.cmdAddBD.Name = "cmdAddBD";
			this.cmdAddBD.Size = new System.Drawing.Size(60, 23);
			this.cmdAddBD.TabIndex = 2;
			this.cmdAddBD.Text = "&Add";
			this.cmdAddBD.UseVisualStyleBackColor = true;
			this.cmdAddBD.Click += new System.EventHandler(this.cmdAddBD_Click);
			// 
			// lstBackdrops
			// 
			this.lstBackdrops.Enabled = false;
			this.lstBackdrops.FormattingEnabled = true;
			this.lstBackdrops.Location = new System.Drawing.Point(6, 42);
			this.lstBackdrops.Name = "lstBackdrops";
			this.lstBackdrops.Size = new System.Drawing.Size(186, 43);
			this.lstBackdrops.TabIndex = 1;
			// 
			// chkBackdrops
			// 
			this.chkBackdrops.AutoSize = true;
			this.chkBackdrops.Location = new System.Drawing.Point(6, 19);
			this.chkBackdrops.Name = "chkBackdrops";
			this.chkBackdrops.Size = new System.Drawing.Size(99, 17);
			this.chkBackdrops.TabIndex = 0;
			this.chkBackdrops.Text = "Use Backdrops";
			this.chkBackdrops.UseVisualStyleBackColor = true;
			this.chkBackdrops.CheckedChanged += new System.EventHandler(this.chkBackdrops_CheckedChanged);
			// 
			// opnBackdrop
			// 
			this.opnBackdrop.Filter = "Dat Files|*.dat";
			// 
			// grpMission
			// 
			this.grpMission.Controls.Add(this.txtPilot);
			this.grpMission.Controls.Add(this.lstMission);
			this.grpMission.Controls.Add(this.optPilot);
			this.grpMission.Controls.Add(this.optIff);
			this.grpMission.Controls.Add(this.optMarkings);
			this.grpMission.Controls.Add(this.cmdRemoveMiss);
			this.grpMission.Controls.Add(this.cmdAddMiss);
			this.grpMission.Controls.Add(this.cboIff);
			this.grpMission.Controls.Add(this.cboMarkings);
			this.grpMission.Controls.Add(this.cboFG);
			this.grpMission.Controls.Add(this.chkMission);
			this.grpMission.Location = new System.Drawing.Point(12, 114);
			this.grpMission.Name = "grpMission";
			this.grpMission.Size = new System.Drawing.Size(332, 183);
			this.grpMission.TabIndex = 3;
			this.grpMission.TabStop = false;
			this.grpMission.Text = "Mission Tie";
			// 
			// txtPilot
			// 
			this.txtPilot.Enabled = false;
			this.txtPilot.Location = new System.Drawing.Point(227, 151);
			this.txtPilot.MaxLength = 15;
			this.txtPilot.Name = "txtPilot";
			this.txtPilot.Size = new System.Drawing.Size(97, 20);
			this.txtPilot.TabIndex = 11;
			// 
			// lstMission
			// 
			this.lstMission.Enabled = false;
			this.lstMission.FormattingEnabled = true;
			this.lstMission.Location = new System.Drawing.Point(6, 42);
			this.lstMission.Name = "lstMission";
			this.lstMission.Size = new System.Drawing.Size(155, 134);
			this.lstMission.TabIndex = 10;
			// 
			// optPilot
			// 
			this.optPilot.AutoSize = true;
			this.optPilot.Enabled = false;
			this.optPilot.Location = new System.Drawing.Point(167, 152);
			this.optPilot.Name = "optPilot";
			this.optPilot.Size = new System.Drawing.Size(45, 17);
			this.optPilot.TabIndex = 9;
			this.optPilot.Text = "Pilot";
			this.optPilot.UseVisualStyleBackColor = true;
			// 
			// optIff
			// 
			this.optIff.AutoSize = true;
			this.optIff.Enabled = false;
			this.optIff.Location = new System.Drawing.Point(167, 125);
			this.optIff.Name = "optIff";
			this.optIff.Size = new System.Drawing.Size(40, 17);
			this.optIff.TabIndex = 9;
			this.optIff.Text = "IFF";
			this.optIff.UseVisualStyleBackColor = true;
			// 
			// optMarkings
			// 
			this.optMarkings.AutoSize = true;
			this.optMarkings.Checked = true;
			this.optMarkings.Enabled = false;
			this.optMarkings.Location = new System.Drawing.Point(167, 98);
			this.optMarkings.Name = "optMarkings";
			this.optMarkings.Size = new System.Drawing.Size(54, 17);
			this.optMarkings.TabIndex = 9;
			this.optMarkings.TabStop = true;
			this.optMarkings.Text = "Marks";
			this.optMarkings.UseVisualStyleBackColor = true;
			// 
			// cmdRemoveMiss
			// 
			this.cmdRemoveMiss.Enabled = false;
			this.cmdRemoveMiss.Location = new System.Drawing.Point(264, 69);
			this.cmdRemoveMiss.Name = "cmdRemoveMiss";
			this.cmdRemoveMiss.Size = new System.Drawing.Size(60, 23);
			this.cmdRemoveMiss.TabIndex = 7;
			this.cmdRemoveMiss.Text = "&Remove";
			this.cmdRemoveMiss.UseVisualStyleBackColor = true;
			this.cmdRemoveMiss.Click += new System.EventHandler(this.cmdRemoveMiss_Click);
			// 
			// cmdAddMiss
			// 
			this.cmdAddMiss.Enabled = false;
			this.cmdAddMiss.Location = new System.Drawing.Point(167, 69);
			this.cmdAddMiss.Name = "cmdAddMiss";
			this.cmdAddMiss.Size = new System.Drawing.Size(60, 23);
			this.cmdAddMiss.TabIndex = 8;
			this.cmdAddMiss.Text = "&Add";
			this.cmdAddMiss.UseVisualStyleBackColor = true;
			this.cmdAddMiss.Click += new System.EventHandler(this.cmdAddMiss_Click);
			// 
			// cboIff
			// 
			this.cboIff.Enabled = false;
			this.cboIff.FormattingEnabled = true;
			this.cboIff.Location = new System.Drawing.Point(227, 124);
			this.cboIff.Name = "cboIff";
			this.cboIff.Size = new System.Drawing.Size(97, 21);
			this.cboIff.TabIndex = 6;
			// 
			// cboMarkings
			// 
			this.cboMarkings.Enabled = false;
			this.cboMarkings.FormattingEnabled = true;
			this.cboMarkings.Location = new System.Drawing.Point(227, 97);
			this.cboMarkings.Name = "cboMarkings";
			this.cboMarkings.Size = new System.Drawing.Size(97, 21);
			this.cboMarkings.TabIndex = 6;
			// 
			// cboFG
			// 
			this.cboFG.Enabled = false;
			this.cboFG.FormattingEnabled = true;
			this.cboFG.Location = new System.Drawing.Point(167, 42);
			this.cboFG.Name = "cboFG";
			this.cboFG.Size = new System.Drawing.Size(157, 21);
			this.cboFG.TabIndex = 6;
			// 
			// chkMission
			// 
			this.chkMission.AutoSize = true;
			this.chkMission.Location = new System.Drawing.Point(6, 19);
			this.chkMission.Name = "chkMission";
			this.chkMission.Size = new System.Drawing.Size(101, 17);
			this.chkMission.TabIndex = 4;
			this.chkMission.Text = "Use Mission Tie";
			this.chkMission.UseVisualStyleBackColor = true;
			this.chkMission.CheckedChanged += new System.EventHandler(this.chkMission_CheckedChanged);
			// 
			// grpSounds
			// 
			this.grpSounds.Controls.Add(this.cmdRemoveSounds);
			this.grpSounds.Controls.Add(this.cmdAddSounds);
			this.grpSounds.Controls.Add(this.lstSounds);
			this.grpSounds.Controls.Add(this.chkSounds);
			this.grpSounds.Location = new System.Drawing.Point(12, 303);
			this.grpSounds.Name = "grpSounds";
			this.grpSounds.Size = new System.Drawing.Size(332, 96);
			this.grpSounds.TabIndex = 4;
			this.grpSounds.TabStop = false;
			this.grpSounds.Text = "Sounds";
			// 
			// cmdRemoveSounds
			// 
			this.cmdRemoveSounds.Enabled = false;
			this.cmdRemoveSounds.Location = new System.Drawing.Point(264, 42);
			this.cmdRemoveSounds.Name = "cmdRemoveSounds";
			this.cmdRemoveSounds.Size = new System.Drawing.Size(60, 23);
			this.cmdRemoveSounds.TabIndex = 2;
			this.cmdRemoveSounds.Text = "&Remove";
			this.cmdRemoveSounds.UseVisualStyleBackColor = true;
			this.cmdRemoveSounds.Click += new System.EventHandler(this.cmdRemoveSounds_Click);
			// 
			// cmdAddSounds
			// 
			this.cmdAddSounds.Enabled = false;
			this.cmdAddSounds.Location = new System.Drawing.Point(198, 42);
			this.cmdAddSounds.Name = "cmdAddSounds";
			this.cmdAddSounds.Size = new System.Drawing.Size(60, 23);
			this.cmdAddSounds.TabIndex = 2;
			this.cmdAddSounds.Text = "&Add";
			this.cmdAddSounds.UseVisualStyleBackColor = true;
			this.cmdAddSounds.Click += new System.EventHandler(this.cmdAddSounds_Click);
			// 
			// lstSounds
			// 
			this.lstSounds.Enabled = false;
			this.lstSounds.FormattingEnabled = true;
			this.lstSounds.Location = new System.Drawing.Point(6, 42);
			this.lstSounds.Name = "lstSounds";
			this.lstSounds.Size = new System.Drawing.Size(186, 43);
			this.lstSounds.TabIndex = 1;
			// 
			// chkSounds
			// 
			this.chkSounds.AutoSize = true;
			this.chkSounds.Location = new System.Drawing.Point(6, 19);
			this.chkSounds.Name = "chkSounds";
			this.chkSounds.Size = new System.Drawing.Size(84, 17);
			this.chkSounds.TabIndex = 0;
			this.chkSounds.Text = "Use Sounds";
			this.chkSounds.UseVisualStyleBackColor = true;
			this.chkSounds.CheckedChanged += new System.EventHandler(this.chkSounds_CheckedChanged);
			// 
			// opnSounds
			// 
			this.opnSounds.Filter = "WAV Files|*.wav";
			// 
			// grpObjects
			// 
			this.grpObjects.Controls.Add(this.cmdRemoveObjects);
			this.grpObjects.Controls.Add(this.cmdAddObjects);
			this.grpObjects.Controls.Add(this.lstObjects);
			this.grpObjects.Controls.Add(this.chkObjects);
			this.grpObjects.Location = new System.Drawing.Point(353, 12);
			this.grpObjects.Name = "grpObjects";
			this.grpObjects.Size = new System.Drawing.Size(332, 96);
			this.grpObjects.TabIndex = 5;
			this.grpObjects.TabStop = false;
			this.grpObjects.Text = "Objects";
			// 
			// cmdRemoveObjects
			// 
			this.cmdRemoveObjects.Enabled = false;
			this.cmdRemoveObjects.Location = new System.Drawing.Point(264, 42);
			this.cmdRemoveObjects.Name = "cmdRemoveObjects";
			this.cmdRemoveObjects.Size = new System.Drawing.Size(60, 23);
			this.cmdRemoveObjects.TabIndex = 2;
			this.cmdRemoveObjects.Text = "&Remove";
			this.cmdRemoveObjects.UseVisualStyleBackColor = true;
			this.cmdRemoveObjects.Click += new System.EventHandler(this.cmdRemoveObjects_Click);
			// 
			// cmdAddObjects
			// 
			this.cmdAddObjects.Enabled = false;
			this.cmdAddObjects.Location = new System.Drawing.Point(198, 42);
			this.cmdAddObjects.Name = "cmdAddObjects";
			this.cmdAddObjects.Size = new System.Drawing.Size(60, 23);
			this.cmdAddObjects.TabIndex = 2;
			this.cmdAddObjects.Text = "&Add";
			this.cmdAddObjects.UseVisualStyleBackColor = true;
			this.cmdAddObjects.Click += new System.EventHandler(this.cmdAddObjects_Click);
			// 
			// lstObjects
			// 
			this.lstObjects.Enabled = false;
			this.lstObjects.FormattingEnabled = true;
			this.lstObjects.Location = new System.Drawing.Point(6, 42);
			this.lstObjects.Name = "lstObjects";
			this.lstObjects.Size = new System.Drawing.Size(186, 43);
			this.lstObjects.TabIndex = 1;
			// 
			// chkObjects
			// 
			this.chkObjects.AutoSize = true;
			this.chkObjects.Location = new System.Drawing.Point(6, 19);
			this.chkObjects.Name = "chkObjects";
			this.chkObjects.Size = new System.Drawing.Size(84, 17);
			this.chkObjects.TabIndex = 0;
			this.chkObjects.Text = "Use Sounds";
			this.chkObjects.UseVisualStyleBackColor = true;
			this.chkObjects.CheckedChanged += new System.EventHandler(this.chkObjects_CheckedChanged);
			// 
			// opnObjects
			// 
			this.opnObjects.Filter = "OPT Files|*.opt";
			// 
			// XwaHookDialog
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(891, 553);
			this.Controls.Add(this.grpObjects);
			this.Controls.Add(this.grpSounds);
			this.Controls.Add(this.grpMission);
			this.Controls.Add(this.grpBackdrops);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "XwaHookDialog";
			this.Text = "Mission Hook Settings";
			this.grpBackdrops.ResumeLayout(false);
			this.grpBackdrops.PerformLayout();
			this.grpMission.ResumeLayout(false);
			this.grpMission.PerformLayout();
			this.grpSounds.ResumeLayout(false);
			this.grpSounds.PerformLayout();
			this.grpObjects.ResumeLayout(false);
			this.grpObjects.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox grpBackdrops;
		private System.Windows.Forms.Button cmdRemoveBD;
		private System.Windows.Forms.Button cmdAddBD;
		private System.Windows.Forms.ListBox lstBackdrops;
		private System.Windows.Forms.CheckBox chkBackdrops;
		private System.Windows.Forms.OpenFileDialog opnBackdrop;
		private System.Windows.Forms.GroupBox grpMission;
		private System.Windows.Forms.CheckBox chkMission;
		private System.Windows.Forms.ListBox lstMission;
		private System.Windows.Forms.RadioButton optPilot;
		private System.Windows.Forms.RadioButton optIff;
		private System.Windows.Forms.RadioButton optMarkings;
		private System.Windows.Forms.Button cmdRemoveMiss;
		private System.Windows.Forms.Button cmdAddMiss;
		private System.Windows.Forms.ComboBox cboIff;
		private System.Windows.Forms.ComboBox cboMarkings;
		private System.Windows.Forms.ComboBox cboFG;
		private System.Windows.Forms.TextBox txtPilot;
		private System.Windows.Forms.GroupBox grpSounds;
		private System.Windows.Forms.Button cmdRemoveSounds;
		private System.Windows.Forms.Button cmdAddSounds;
		private System.Windows.Forms.ListBox lstSounds;
		private System.Windows.Forms.CheckBox chkSounds;
		private System.Windows.Forms.OpenFileDialog opnSounds;
		private System.Windows.Forms.GroupBox grpObjects;
		private System.Windows.Forms.Button cmdRemoveObjects;
		private System.Windows.Forms.Button cmdAddObjects;
		private System.Windows.Forms.ListBox lstObjects;
		private System.Windows.Forms.CheckBox chkObjects;
		private System.Windows.Forms.OpenFileDialog opnObjects;
	}
}