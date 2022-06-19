using System.Windows.Forms;

namespace Idmr.Yogeme
{
	partial class StartForm
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
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.grpPlatform = new System.Windows.Forms.GroupBox();
            this.optLastMission = new System.Windows.Forms.RadioButton();
            this.chkBoP = new System.Windows.Forms.CheckBox();
            this.optXWING = new System.Windows.Forms.RadioButton();
            this.optTIE = new System.Windows.Forms.RadioButton();
            this.optXvT = new System.Windows.Forms.RadioButton();
            this.optXWA = new System.Windows.Forms.RadioButton();
            this.lblChoose = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.grpPlatform.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPlatform
            // 
            this.grpPlatform.Controls.Add(this.optLastMission);
            this.grpPlatform.Controls.Add(this.chkBoP);
            this.grpPlatform.Controls.Add(this.optXWING);
            this.grpPlatform.Controls.Add(this.optTIE);
            this.grpPlatform.Controls.Add(this.optXvT);
            this.grpPlatform.Controls.Add(this.optXWA);
            this.grpPlatform.Location = new System.Drawing.Point(24, 48);
            this.grpPlatform.Name = "grpPlatform";
            this.grpPlatform.Size = new System.Drawing.Size(200, 131);
            this.grpPlatform.TabIndex = 0;
            this.grpPlatform.TabStop = false;
            // 
            // optLastMission
            // 
            this.optLastMission.AutoSize = true;
            this.optLastMission.Location = new System.Drawing.Point(16, 98);
            this.optLastMission.Name = "optLastMission";
            this.optLastMission.Size = new System.Drawing.Size(83, 17);
            this.optLastMission.TabIndex = 2;
            this.optLastMission.TabStop = true;
            this.optLastMission.Text = "Last Mission";
            this.optLastMission.UseVisualStyleBackColor = true;
            // 
            // chkBoP
            // 
            this.chkBoP.Enabled = false;
            this.chkBoP.Location = new System.Drawing.Point(144, 58);
            this.chkBoP.Name = "chkBoP";
            this.chkBoP.Size = new System.Drawing.Size(48, 16);
            this.chkBoP.TabIndex = 1;
            this.chkBoP.Text = "BoP";
            this.chkBoP.CheckedChanged += new System.EventHandler(this.chkBoP_CheckedChanged);
            // 
            // optXWING
            // 
            this.optXWING.Enabled = false;
            this.optXWING.Location = new System.Drawing.Point(16, 18);
            this.optXWING.Name = "optXWING";
            this.optXWING.Size = new System.Drawing.Size(160, 18);
            this.optXWING.TabIndex = 0;
            this.optXWING.Text = "X-wing";
            // 
            // optTIE
            // 
            this.optTIE.Enabled = false;
            this.optTIE.Location = new System.Drawing.Point(16, 38);
            this.optTIE.Name = "optTIE";
            this.optTIE.Size = new System.Drawing.Size(160, 18);
            this.optTIE.TabIndex = 0;
            this.optTIE.Text = "TIE Fighter";
            // 
            // optXvT
            // 
            this.optXvT.Enabled = false;
            this.optXvT.Location = new System.Drawing.Point(16, 58);
            this.optXvT.Name = "optXvT";
            this.optXvT.Size = new System.Drawing.Size(128, 18);
            this.optXvT.TabIndex = 0;
            this.optXvT.Text = "Xwing v TIE Fighter";
            // 
            // optXWA
            // 
            this.optXWA.Enabled = false;
            this.optXWA.Location = new System.Drawing.Point(16, 78);
            this.optXWA.Name = "optXWA";
            this.optXWA.Size = new System.Drawing.Size(160, 18);
            this.optXWA.TabIndex = 0;
            this.optXWA.Text = "Xwing Alliance";
            // 
            // lblChoose
            // 
            this.lblChoose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChoose.Location = new System.Drawing.Point(40, 16);
            this.lblChoose.Name = "lblChoose";
            this.lblChoose.Size = new System.Drawing.Size(176, 32);
            this.lblChoose.TabIndex = 1;
            this.lblChoose.Text = "Choose a platform...";
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(24, 185);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 32);
            this.cmdOK.TabIndex = 2;
            this.cmdOK.Text = "&OK";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(144, 185);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 32);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // StartForm
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(250, 232);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.lblChoose);
            this.Controls.Add(this.grpPlatform);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ye Olde Galactic Empire Mission Editor";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.form_Closing);
            this.grpPlatform.ResumeLayout(false);
            this.grpPlatform.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		GroupBox grpPlatform;
		Label lblChoose;
		RadioButton optTIE;
		RadioButton optXvT;
		RadioButton optXWA;
		Button cmdOK;
		Button cmdCancel;
		CheckBox chkBoP;
        private RadioButton optXWING;
        private RadioButton optLastMission;
    }
}