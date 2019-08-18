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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.grpBackdrops = new System.Windows.Forms.GroupBox();
			this.chkBackdrops = new System.Windows.Forms.CheckBox();
			this.lstBackdrops = new System.Windows.Forms.ListBox();
			this.cmdAddBD = new System.Windows.Forms.Button();
			this.cmdRemoveBD = new System.Windows.Forms.Button();
			this.opnBackdrop = new System.Windows.Forms.OpenFileDialog();
			this.grpBackdrops.SuspendLayout();
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
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 211);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Mission Tie";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 256);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(47, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Hangars";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 294);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(81, 13);
			this.label4.TabIndex = 1;
			this.label4.Text = "Mission Objects";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 340);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(74, 13);
			this.label5.TabIndex = 1;
			this.label5.Text = "Engine Sound";
			// 
			// grpBackdrops
			// 
			this.grpBackdrops.Controls.Add(this.cmdRemoveBD);
			this.grpBackdrops.Controls.Add(this.cmdAddBD);
			this.grpBackdrops.Controls.Add(this.lstBackdrops);
			this.grpBackdrops.Controls.Add(this.chkBackdrops);
			this.grpBackdrops.Location = new System.Drawing.Point(12, 12);
			this.grpBackdrops.Name = "grpBackdrops";
			this.grpBackdrops.Size = new System.Drawing.Size(289, 96);
			this.grpBackdrops.TabIndex = 2;
			this.grpBackdrops.TabStop = false;
			this.grpBackdrops.Text = "Backdrops";
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
			// lstBackdrops
			// 
			this.lstBackdrops.Enabled = false;
			this.lstBackdrops.FormattingEnabled = true;
			this.lstBackdrops.Location = new System.Drawing.Point(6, 42);
			this.lstBackdrops.Name = "lstBackdrops";
			this.lstBackdrops.Size = new System.Drawing.Size(141, 43);
			this.lstBackdrops.TabIndex = 1;
			// 
			// cmdAddBD
			// 
			this.cmdAddBD.Enabled = false;
			this.cmdAddBD.Location = new System.Drawing.Point(153, 42);
			this.cmdAddBD.Name = "cmdAddBD";
			this.cmdAddBD.Size = new System.Drawing.Size(60, 23);
			this.cmdAddBD.TabIndex = 2;
			this.cmdAddBD.Text = "&Add";
			this.cmdAddBD.UseVisualStyleBackColor = true;
			this.cmdAddBD.Click += new System.EventHandler(this.cmdAddBD_Click);
			// 
			// cmdRemoveBD
			// 
			this.cmdRemoveBD.Enabled = false;
			this.cmdRemoveBD.Location = new System.Drawing.Point(219, 42);
			this.cmdRemoveBD.Name = "cmdRemoveBD";
			this.cmdRemoveBD.Size = new System.Drawing.Size(60, 23);
			this.cmdRemoveBD.TabIndex = 2;
			this.cmdRemoveBD.Text = "&Remove";
			this.cmdRemoveBD.UseVisualStyleBackColor = true;
			this.cmdRemoveBD.Click += new System.EventHandler(this.cmdRemoveBD_Click);
			// 
			// opnBackdrop
			// 
			this.opnBackdrop.Filter = "Dat Files|*.dat";
			// 
			// XwaHookDialog
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(891, 553);
			this.Controls.Add(this.grpBackdrops);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "XwaHookDialog";
			this.Text = "Mission Hook Settings";
			this.grpBackdrops.ResumeLayout(false);
			this.grpBackdrops.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox grpBackdrops;
		private System.Windows.Forms.Button cmdRemoveBD;
		private System.Windows.Forms.Button cmdAddBD;
		private System.Windows.Forms.ListBox lstBackdrops;
		private System.Windows.Forms.CheckBox chkBackdrops;
		private System.Windows.Forms.OpenFileDialog opnBackdrop;
	}
}