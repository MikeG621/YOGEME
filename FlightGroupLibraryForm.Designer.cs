namespace Idmr.Yogeme
{
	partial class FlightGroupLibraryForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlightGroupLibraryForm));
			this.cboLibraryGroup = new System.Windows.Forms.ComboBox();
			this.cmdNewGroup = new System.Windows.Forms.Button();
			this.cmdDeleteGroup = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.lstMissionCraft = new System.Windows.Forms.ListBox();
			this.lstLibraryCraft = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cmdAddToLibrary = new System.Windows.Forms.Button();
			this.cmdAddToMission = new System.Windows.Forms.Button();
			this.lblMissionCraft = new System.Windows.Forms.Label();
			this.lblLibraryCraft = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.cmdRenameGroup = new System.Windows.Forms.Button();
			this.cmdDeleteCraft = new System.Windows.Forms.Button();
			this.lstLibraryGroup = new System.Windows.Forms.ListBox();
			this.grpGroup = new System.Windows.Forms.GroupBox();
			this.cmdMoveCraftUp = new System.Windows.Forms.Button();
			this.cmdMoveCraftDown = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdMoveCraftToGroup = new System.Windows.Forms.Button();
			this.grpCraftManager = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cmdScrubProblems = new System.Windows.Forms.Button();
			this.cmdViewProblems = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.chkAutoscrubAddMission = new System.Windows.Forms.CheckBox();
			this.grpGroup.SuspendLayout();
			this.grpCraftManager.SuspendLayout();
			this.SuspendLayout();
			// 
			// cboLibraryGroup
			// 
			this.cboLibraryGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboLibraryGroup.FormattingEnabled = true;
			this.cboLibraryGroup.Location = new System.Drawing.Point(10, 84);
			this.cboLibraryGroup.Name = "cboLibraryGroup";
			this.cboLibraryGroup.Size = new System.Drawing.Size(127, 21);
			this.cboLibraryGroup.TabIndex = 16;
			// 
			// cmdNewGroup
			// 
			this.cmdNewGroup.Location = new System.Drawing.Point(25, 117);
			this.cmdNewGroup.Name = "cmdNewGroup";
			this.cmdNewGroup.Size = new System.Drawing.Size(46, 23);
			this.cmdNewGroup.TabIndex = 3;
			this.cmdNewGroup.Text = "New";
			this.cmdNewGroup.UseVisualStyleBackColor = true;
			this.cmdNewGroup.Click += new System.EventHandler(this.cmdNewGroup_Click);
			// 
			// cmdDeleteGroup
			// 
			this.cmdDeleteGroup.Location = new System.Drawing.Point(125, 117);
			this.cmdDeleteGroup.Name = "cmdDeleteGroup";
			this.cmdDeleteGroup.Size = new System.Drawing.Size(46, 23);
			this.cmdDeleteGroup.TabIndex = 4;
			this.cmdDeleteGroup.Text = "Delete";
			this.cmdDeleteGroup.UseVisualStyleBackColor = true;
			this.cmdDeleteGroup.Click += new System.EventHandler(this.cmdDeleteGroup_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(115, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "FlightGroups in Mission";
			// 
			// lstMissionCraft
			// 
			this.lstMissionCraft.BackColor = System.Drawing.Color.Black;
			this.lstMissionCraft.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstMissionCraft.FormattingEnabled = true;
			this.lstMissionCraft.Location = new System.Drawing.Point(15, 59);
			this.lstMissionCraft.Name = "lstMissionCraft";
			this.lstMissionCraft.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstMissionCraft.Size = new System.Drawing.Size(213, 498);
			this.lstMissionCraft.TabIndex = 7;
			this.lstMissionCraft.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstMissionCraft_DrawItem);
			// 
			// lstLibraryCraft
			// 
			this.lstLibraryCraft.BackColor = System.Drawing.Color.Black;
			this.lstLibraryCraft.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstLibraryCraft.FormattingEnabled = true;
			this.lstLibraryCraft.Location = new System.Drawing.Point(436, 59);
			this.lstLibraryCraft.Name = "lstLibraryCraft";
			this.lstLibraryCraft.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstLibraryCraft.Size = new System.Drawing.Size(223, 498);
			this.lstLibraryCraft.TabIndex = 8;
			this.lstLibraryCraft.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstGroupCraft_DrawItem);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(433, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(143, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "FlightGroups in Library Group";
			// 
			// cmdAddToLibrary
			// 
			this.cmdAddToLibrary.Location = new System.Drawing.Point(234, 229);
			this.cmdAddToLibrary.Name = "cmdAddToLibrary";
			this.cmdAddToLibrary.Size = new System.Drawing.Size(117, 23);
			this.cmdAddToLibrary.TabIndex = 9;
			this.cmdAddToLibrary.Text = "Add To Library >>";
			this.cmdAddToLibrary.UseVisualStyleBackColor = true;
			this.cmdAddToLibrary.Click += new System.EventHandler(this.cmdAddToLibrary_Click);
			// 
			// cmdAddToMission
			// 
			this.cmdAddToMission.Location = new System.Drawing.Point(311, 272);
			this.cmdAddToMission.Name = "cmdAddToMission";
			this.cmdAddToMission.Size = new System.Drawing.Size(117, 23);
			this.cmdAddToMission.TabIndex = 10;
			this.cmdAddToMission.Text = "<< Add To Mission";
			this.cmdAddToMission.UseVisualStyleBackColor = true;
			this.cmdAddToMission.Click += new System.EventHandler(this.cmdAddToMission_Click);
			// 
			// lblMissionCraft
			// 
			this.lblMissionCraft.AutoSize = true;
			this.lblMissionCraft.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMissionCraft.Location = new System.Drawing.Point(12, 42);
			this.lblMissionCraft.Name = "lblMissionCraft";
			this.lblMissionCraft.Size = new System.Drawing.Size(143, 13);
			this.lblMissionCraft.TabIndex = 8;
			this.lblMissionCraft.Text = "Tm - GG  - waves x craft (GU)";
			// 
			// lblLibraryCraft
			// 
			this.lblLibraryCraft.AutoSize = true;
			this.lblLibraryCraft.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLibraryCraft.Location = new System.Drawing.Point(433, 42);
			this.lblLibraryCraft.Name = "lblLibraryCraft";
			this.lblLibraryCraft.Size = new System.Drawing.Size(143, 13);
			this.lblLibraryCraft.TabIndex = 9;
			this.lblLibraryCraft.Text = "Tm - GG  - waves x craft (GU)";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(10, 150);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(113, 20);
			this.txtName.TabIndex = 5;
			// 
			// cmdRenameGroup
			// 
			this.cmdRenameGroup.Location = new System.Drawing.Point(129, 148);
			this.cmdRenameGroup.Name = "cmdRenameGroup";
			this.cmdRenameGroup.Size = new System.Drawing.Size(56, 23);
			this.cmdRenameGroup.TabIndex = 6;
			this.cmdRenameGroup.Text = "Rename";
			this.cmdRenameGroup.UseVisualStyleBackColor = true;
			this.cmdRenameGroup.Click += new System.EventHandler(this.cmdRenameGroup_Click);
			// 
			// cmdDeleteCraft
			// 
			this.cmdDeleteCraft.Location = new System.Drawing.Point(134, 48);
			this.cmdDeleteCraft.Name = "cmdDeleteCraft";
			this.cmdDeleteCraft.Size = new System.Drawing.Size(51, 23);
			this.cmdDeleteCraft.TabIndex = 15;
			this.cmdDeleteCraft.Text = "Delete";
			this.cmdDeleteCraft.UseVisualStyleBackColor = true;
			this.cmdDeleteCraft.Click += new System.EventHandler(this.cmdDeleteCraft_Click);
			// 
			// lstLibraryGroup
			// 
			this.lstLibraryGroup.Location = new System.Drawing.Point(10, 29);
			this.lstLibraryGroup.Margin = new System.Windows.Forms.Padding(7, 3, 7, 3);
			this.lstLibraryGroup.Name = "lstLibraryGroup";
			this.lstLibraryGroup.Size = new System.Drawing.Size(176, 82);
			this.lstLibraryGroup.TabIndex = 2;
			this.lstLibraryGroup.SelectedIndexChanged += new System.EventHandler(this.lstLibraryGroup_SelectedIndexChanged);
			// 
			// grpGroup
			// 
			this.grpGroup.Controls.Add(this.lstLibraryGroup);
			this.grpGroup.Controls.Add(this.txtName);
			this.grpGroup.Controls.Add(this.cmdRenameGroup);
			this.grpGroup.Controls.Add(this.cmdDeleteGroup);
			this.grpGroup.Controls.Add(this.cmdNewGroup);
			this.grpGroup.Location = new System.Drawing.Point(234, 12);
			this.grpGroup.Name = "grpGroup";
			this.grpGroup.Size = new System.Drawing.Size(196, 182);
			this.grpGroup.TabIndex = 1;
			this.grpGroup.TabStop = false;
			this.grpGroup.Text = "Manage Library Groups";
			// 
			// cmdMoveCraftUp
			// 
			this.cmdMoveCraftUp.Location = new System.Drawing.Point(77, 19);
			this.cmdMoveCraftUp.Name = "cmdMoveCraftUp";
			this.cmdMoveCraftUp.Size = new System.Drawing.Size(51, 23);
			this.cmdMoveCraftUp.TabIndex = 13;
			this.cmdMoveCraftUp.Text = "Up";
			this.cmdMoveCraftUp.UseVisualStyleBackColor = true;
			this.cmdMoveCraftUp.Click += new System.EventHandler(this.cmdMoveCraftUp_Click);
			// 
			// cmdMoveCraftDown
			// 
			this.cmdMoveCraftDown.Location = new System.Drawing.Point(134, 19);
			this.cmdMoveCraftDown.Name = "cmdMoveCraftDown";
			this.cmdMoveCraftDown.Size = new System.Drawing.Size(51, 23);
			this.cmdMoveCraftDown.TabIndex = 14;
			this.cmdMoveCraftDown.Text = "Down";
			this.cmdMoveCraftDown.UseVisualStyleBackColor = true;
			this.cmdMoveCraftDown.Click += new System.EventHandler(this.cmdMoveCraftDown_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 65);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 13);
			this.label1.TabIndex = 15;
			this.label1.Text = "Move to Group:";
			// 
			// cmdMoveCraftToGroup
			// 
			this.cmdMoveCraftToGroup.Location = new System.Drawing.Point(143, 82);
			this.cmdMoveCraftToGroup.Name = "cmdMoveCraftToGroup";
			this.cmdMoveCraftToGroup.Size = new System.Drawing.Size(42, 23);
			this.cmdMoveCraftToGroup.TabIndex = 17;
			this.cmdMoveCraftToGroup.Text = "Move";
			this.cmdMoveCraftToGroup.UseVisualStyleBackColor = true;
			this.cmdMoveCraftToGroup.Click += new System.EventHandler(this.cmdMoveCraftToGroup_Click);
			// 
			// grpCraftManager
			// 
			this.grpCraftManager.Controls.Add(this.label5);
			this.grpCraftManager.Controls.Add(this.cmdScrubProblems);
			this.grpCraftManager.Controls.Add(this.cmdViewProblems);
			this.grpCraftManager.Controls.Add(this.label4);
			this.grpCraftManager.Controls.Add(this.label1);
			this.grpCraftManager.Controls.Add(this.cmdMoveCraftDown);
			this.grpCraftManager.Controls.Add(this.cmdMoveCraftUp);
			this.grpCraftManager.Controls.Add(this.cmdMoveCraftToGroup);
			this.grpCraftManager.Controls.Add(this.cmdDeleteCraft);
			this.grpCraftManager.Controls.Add(this.cboLibraryGroup);
			this.grpCraftManager.Location = new System.Drawing.Point(234, 333);
			this.grpCraftManager.Name = "grpCraftManager";
			this.grpCraftManager.Size = new System.Drawing.Size(196, 153);
			this.grpCraftManager.TabIndex = 12;
			this.grpCraftManager.TabStop = false;
			this.grpCraftManager.Text = "Manage selected craft in Library";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 125);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(53, 13);
			this.label5.TabIndex = 19;
			this.label5.Text = "Problems:";
			// 
			// cmdScrubProblems
			// 
			this.cmdScrubProblems.Location = new System.Drawing.Point(129, 120);
			this.cmdScrubProblems.Name = "cmdScrubProblems";
			this.cmdScrubProblems.Size = new System.Drawing.Size(52, 23);
			this.cmdScrubProblems.TabIndex = 19;
			this.cmdScrubProblems.Text = "Scrub";
			this.cmdScrubProblems.UseVisualStyleBackColor = true;
			this.cmdScrubProblems.Click += new System.EventHandler(this.cmdScrubProblems_Click);
			// 
			// cmdViewProblems
			// 
			this.cmdViewProblems.Location = new System.Drawing.Point(71, 120);
			this.cmdViewProblems.Name = "cmdViewProblems";
			this.cmdViewProblems.Size = new System.Drawing.Size(52, 23);
			this.cmdViewProblems.TabIndex = 18;
			this.cmdViewProblems.Text = "View";
			this.cmdViewProblems.UseVisualStyleBackColor = true;
			this.cmdViewProblems.Click += new System.EventHandler(this.cmdViewProblems_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(34, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(37, 13);
			this.label4.TabIndex = 16;
			this.label4.Text = "Move:";
			// 
			// chkAutoscrubAddMission
			// 
			this.chkAutoscrubAddMission.AutoSize = true;
			this.chkAutoscrubAddMission.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkAutoscrubAddMission.Checked = true;
			this.chkAutoscrubAddMission.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoscrubAddMission.Location = new System.Drawing.Point(290, 300);
			this.chkAutoscrubAddMission.Name = "chkAutoscrubAddMission";
			this.chkAutoscrubAddMission.Size = new System.Drawing.Size(138, 17);
			this.chkAutoscrubAddMission.TabIndex = 11;
			this.chkAutoscrubAddMission.Text = "Autoscrub when adding";
			this.chkAutoscrubAddMission.UseVisualStyleBackColor = true;
			// 
			// FlightGroupLibraryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(675, 557);
			this.Controls.Add(this.chkAutoscrubAddMission);
			this.Controls.Add(this.grpCraftManager);
			this.Controls.Add(this.grpGroup);
			this.Controls.Add(this.lblLibraryCraft);
			this.Controls.Add(this.lblMissionCraft);
			this.Controls.Add(this.cmdAddToMission);
			this.Controls.Add(this.cmdAddToLibrary);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lstLibraryCraft);
			this.Controls.Add(this.lstMissionCraft);
			this.Controls.Add(this.label2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(691, 2048);
			this.MinimumSize = new System.Drawing.Size(691, 532);
			this.Name = "FlightGroupLibraryForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FlightGroup Library";
			this.Activated += new System.EventHandler(this.FlightGroupLibraryForm_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FlightGroupLibraryForm_FormClosing);
			this.SizeChanged += new System.EventHandler(this.FlightGroupLibraryForm_SizeChanged);
			this.grpGroup.ResumeLayout(false);
			this.grpGroup.PerformLayout();
			this.grpCraftManager.ResumeLayout(false);
			this.grpCraftManager.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ComboBox cboLibraryGroup;
		private System.Windows.Forms.Button cmdNewGroup;
		private System.Windows.Forms.Button cmdDeleteGroup;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lstMissionCraft;
		private System.Windows.Forms.ListBox lstLibraryCraft;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button cmdAddToLibrary;
		private System.Windows.Forms.Button cmdAddToMission;
		private System.Windows.Forms.Label lblMissionCraft;
		private System.Windows.Forms.Label lblLibraryCraft;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Button cmdRenameGroup;
		private System.Windows.Forms.Button cmdDeleteCraft;
		private System.Windows.Forms.ListBox lstLibraryGroup;
		private System.Windows.Forms.GroupBox grpGroup;
		private System.Windows.Forms.Button cmdMoveCraftUp;
		private System.Windows.Forms.Button cmdMoveCraftDown;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cmdMoveCraftToGroup;
		private System.Windows.Forms.GroupBox grpCraftManager;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button cmdScrubProblems;
		private System.Windows.Forms.Button cmdViewProblems;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkAutoscrubAddMission;
	}
}