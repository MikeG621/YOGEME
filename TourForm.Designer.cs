
namespace Idmr.Yogeme
{
	partial class TourForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TourForm));
			this.cmdMoveUp = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.cmdAdd = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.lstMiss = new System.Windows.Forms.ListBox();
			this.txtDesc = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cmdRemove = new System.Windows.Forms.Button();
			this.cmdMoveDown = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.lblTour = new System.Windows.Forms.Label();
			this.cmdPrev = new System.Windows.Forms.Button();
			this.cmdNext = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.opnMission = new System.Windows.Forms.OpenFileDialog();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmdMoveUp
			// 
			this.cmdMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("cmdMoveUp.Image")));
			this.cmdMoveUp.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdMoveUp.Location = new System.Drawing.Point(43, 202);
			this.cmdMoveUp.Name = "cmdMoveUp";
			this.cmdMoveUp.Size = new System.Drawing.Size(24, 24);
			this.cmdMoveUp.TabIndex = 14;
			this.cmdMoveUp.Click += new System.EventHandler(this.cmdMoveUp_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(420, 88);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(80, 96);
			this.label8.TabIndex = 12;
			this.label8.Text = "Line breaks are required, do not rely on word wrap for this text box";
			// 
			// cmdAdd
			// 
			this.cmdAdd.Location = new System.Drawing.Point(164, 112);
			this.cmdAdd.Name = "cmdAdd";
			this.cmdAdd.Size = new System.Drawing.Size(64, 24);
			this.cmdAdd.TabIndex = 6;
			this.cmdAdd.Text = "&Add...";
			this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(12, 64);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 16);
			this.label6.TabIndex = 8;
			this.label6.Text = "Missions";
			// 
			// lstMiss
			// 
			this.lstMiss.Location = new System.Drawing.Point(12, 88);
			this.lstMiss.Name = "lstMiss";
			this.lstMiss.Size = new System.Drawing.Size(136, 108);
			this.lstMiss.TabIndex = 9;
			this.lstMiss.SelectedIndexChanged += new System.EventHandler(this.lstMiss_SelectedIndexChanged);
			// 
			// txtDesc
			// 
			this.txtDesc.AcceptsReturn = true;
			this.txtDesc.Location = new System.Drawing.Point(244, 88);
			this.txtDesc.Multiline = true;
			this.txtDesc.Name = "txtDesc";
			this.txtDesc.Size = new System.Drawing.Size(176, 168);
			this.txtDesc.TabIndex = 11;
			this.txtDesc.TextChanged += new System.EventHandler(this.txtDesc_TextChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(241, 64);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 16);
			this.label7.TabIndex = 10;
			this.label7.Text = "Description";
			// 
			// cmdRemove
			// 
			this.cmdRemove.Location = new System.Drawing.Point(164, 160);
			this.cmdRemove.Name = "cmdRemove";
			this.cmdRemove.Size = new System.Drawing.Size(64, 24);
			this.cmdRemove.TabIndex = 7;
			this.cmdRemove.Text = "&Remove";
			this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
			// 
			// cmdMoveDown
			// 
			this.cmdMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("cmdMoveDown.Image")));
			this.cmdMoveDown.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdMoveDown.Location = new System.Drawing.Point(91, 202);
			this.cmdMoveDown.Name = "cmdMoveDown";
			this.cmdMoveDown.Size = new System.Drawing.Size(24, 24);
			this.cmdMoveDown.TabIndex = 13;
			this.cmdMoveDown.Click += new System.EventHandler(this.cmdMoveDown_Click);
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(356, 12);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(64, 24);
			this.cmdOK.TabIndex = 17;
			this.cmdOK.Text = "&OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// lblTour
			// 
			this.lblTour.Location = new System.Drawing.Point(84, 12);
			this.lblTour.Name = "lblTour";
			this.lblTour.Size = new System.Drawing.Size(64, 24);
			this.lblTour.TabIndex = 15;
			this.lblTour.Text = "Tour1";
			this.lblTour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmdPrev
			// 
			this.cmdPrev.Location = new System.Drawing.Point(12, 12);
			this.cmdPrev.Name = "cmdPrev";
			this.cmdPrev.Size = new System.Drawing.Size(64, 24);
			this.cmdPrev.TabIndex = 19;
			this.cmdPrev.Text = "&Previous";
			this.cmdPrev.Click += new System.EventHandler(this.cmdPrev_Click);
			// 
			// cmdNext
			// 
			this.cmdNext.Location = new System.Drawing.Point(156, 12);
			this.cmdNext.Name = "cmdNext";
			this.cmdNext.Size = new System.Drawing.Size(64, 24);
			this.cmdNext.TabIndex = 18;
			this.cmdNext.Text = "&Next";
			this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(435, 12);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(64, 24);
			this.cmdCancel.TabIndex = 17;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// opnMission
			// 
			this.opnMission.Filter = "X-wing Missions|*.xwi";
			this.opnMission.FileOk += new System.ComponentModel.CancelEventHandler(this.opnMission_FileOk);
			// 
			// txtTitle
			// 
			this.txtTitle.Location = new System.Drawing.Point(42, 237);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(186, 20);
			this.txtTitle.TabIndex = 20;
			this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 240);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 13);
			this.label1.TabIndex = 21;
			this.label1.Text = "Title";
			// 
			// TourForm
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(511, 283);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.lblTour);
			this.Controls.Add(this.cmdPrev);
			this.Controls.Add(this.cmdNext);
			this.Controls.Add(this.cmdMoveUp);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.cmdAdd);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.lstMiss);
			this.Controls.Add(this.txtDesc);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.cmdRemove);
			this.Controls.Add(this.cmdMoveDown);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TourForm";
			this.Text = "YOGEME X-wing Tour Editor";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdMoveUp;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button cmdAdd;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ListBox lstMiss;
		private System.Windows.Forms.TextBox txtDesc;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button cmdRemove;
		private System.Windows.Forms.Button cmdMoveDown;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Label lblTour;
		private System.Windows.Forms.Button cmdPrev;
		private System.Windows.Forms.Button cmdNext;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.OpenFileDialog opnMission;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label label1;
	}
}