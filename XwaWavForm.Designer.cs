
namespace Idmr.Yogeme
{
	partial class XwaWavForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XwaWavForm));
			this.tabsWav = new System.Windows.Forms.TabControl();
			this.tabMessages = new System.Windows.Forms.TabPage();
			this.tabEom = new System.Windows.Forms.TabPage();
			this.tabBriefing = new System.Windows.Forms.TabPage();
			this.tabPrePost = new System.Windows.Forms.TabPage();
			this.lstMessages = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lblNotes = new System.Windows.Forms.Label();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cmdMessage = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.lblFG = new System.Windows.Forms.Label();
			this.cmdPlayMessage = new System.Windows.Forms.Button();
			this.cmdPlayEom = new System.Windows.Forms.Button();
			this.cmdEom = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.txtEom = new System.Windows.Forms.TextBox();
			this.lblEomNote = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.lblEom = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.lstEom = new System.Windows.Forms.ListBox();
			this.opnWav = new System.Windows.Forms.OpenFileDialog();
			this.cmdPlayBriefing = new System.Windows.Forms.Button();
			this.cmdBriefing = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.txtBriefing = new System.Windows.Forms.TextBox();
			this.lblBriefingNote = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.lblBriefing = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.lstBriefing = new System.Windows.Forms.ListBox();
			this.cmdPlayPrePost = new System.Windows.Forms.Button();
			this.cmdPrePost = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.txtPrePostWav = new System.Windows.Forms.TextBox();
			this.lblPrePostNote = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.lstPrePostCategories = new System.Windows.Forms.ListBox();
			this.txtPrePost = new System.Windows.Forms.TextBox();
			this.lstPrePost = new System.Windows.Forms.ListBox();
			this.label14 = new System.Windows.Forms.Label();
			this.tabsWav.SuspendLayout();
			this.tabMessages.SuspendLayout();
			this.tabEom.SuspendLayout();
			this.tabBriefing.SuspendLayout();
			this.tabPrePost.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabsWav
			// 
			this.tabsWav.Controls.Add(this.tabMessages);
			this.tabsWav.Controls.Add(this.tabEom);
			this.tabsWav.Controls.Add(this.tabBriefing);
			this.tabsWav.Controls.Add(this.tabPrePost);
			this.tabsWav.Location = new System.Drawing.Point(12, 12);
			this.tabsWav.Name = "tabsWav";
			this.tabsWav.SelectedIndex = 0;
			this.tabsWav.Size = new System.Drawing.Size(626, 347);
			this.tabsWav.TabIndex = 0;
			// 
			// tabMessages
			// 
			this.tabMessages.BackColor = System.Drawing.SystemColors.Control;
			this.tabMessages.Controls.Add(this.cmdPlayMessage);
			this.tabMessages.Controls.Add(this.lblFG);
			this.tabMessages.Controls.Add(this.label8);
			this.tabMessages.Controls.Add(this.cmdMessage);
			this.tabMessages.Controls.Add(this.label7);
			this.tabMessages.Controls.Add(this.txtMessage);
			this.tabMessages.Controls.Add(this.lblNotes);
			this.tabMessages.Controls.Add(this.label6);
			this.tabMessages.Controls.Add(this.lblMessage);
			this.tabMessages.Controls.Add(this.label5);
			this.tabMessages.Controls.Add(this.label1);
			this.tabMessages.Controls.Add(this.lstMessages);
			this.tabMessages.Location = new System.Drawing.Point(4, 22);
			this.tabMessages.Name = "tabMessages";
			this.tabMessages.Padding = new System.Windows.Forms.Padding(3);
			this.tabMessages.Size = new System.Drawing.Size(618, 321);
			this.tabMessages.TabIndex = 0;
			this.tabMessages.Text = "In-Flight Messages";
			// 
			// tabEom
			// 
			this.tabEom.BackColor = System.Drawing.SystemColors.Control;
			this.tabEom.Controls.Add(this.cmdPlayEom);
			this.tabEom.Controls.Add(this.cmdEom);
			this.tabEom.Controls.Add(this.label11);
			this.tabEom.Controls.Add(this.txtEom);
			this.tabEom.Controls.Add(this.lblEomNote);
			this.tabEom.Controls.Add(this.label13);
			this.tabEom.Controls.Add(this.lblEom);
			this.tabEom.Controls.Add(this.label15);
			this.tabEom.Controls.Add(this.lstEom);
			this.tabEom.Controls.Add(this.label2);
			this.tabEom.Location = new System.Drawing.Point(4, 22);
			this.tabEom.Name = "tabEom";
			this.tabEom.Padding = new System.Windows.Forms.Padding(3);
			this.tabEom.Size = new System.Drawing.Size(618, 321);
			this.tabEom.TabIndex = 1;
			this.tabEom.Text = "EOM Messages";
			// 
			// tabBriefing
			// 
			this.tabBriefing.BackColor = System.Drawing.SystemColors.Control;
			this.tabBriefing.Controls.Add(this.cmdPlayBriefing);
			this.tabBriefing.Controls.Add(this.cmdBriefing);
			this.tabBriefing.Controls.Add(this.label9);
			this.tabBriefing.Controls.Add(this.txtBriefing);
			this.tabBriefing.Controls.Add(this.lblBriefingNote);
			this.tabBriefing.Controls.Add(this.label12);
			this.tabBriefing.Controls.Add(this.lblBriefing);
			this.tabBriefing.Controls.Add(this.label16);
			this.tabBriefing.Controls.Add(this.lstBriefing);
			this.tabBriefing.Controls.Add(this.label3);
			this.tabBriefing.Location = new System.Drawing.Point(4, 22);
			this.tabBriefing.Name = "tabBriefing";
			this.tabBriefing.Size = new System.Drawing.Size(618, 321);
			this.tabBriefing.TabIndex = 2;
			this.tabBriefing.Text = "Briefing Strings";
			// 
			// tabPrePost
			// 
			this.tabPrePost.BackColor = System.Drawing.SystemColors.Control;
			this.tabPrePost.Controls.Add(this.txtPrePost);
			this.tabPrePost.Controls.Add(this.cmdPlayPrePost);
			this.tabPrePost.Controls.Add(this.cmdPrePost);
			this.tabPrePost.Controls.Add(this.label10);
			this.tabPrePost.Controls.Add(this.txtPrePostWav);
			this.tabPrePost.Controls.Add(this.lblPrePostNote);
			this.tabPrePost.Controls.Add(this.label17);
			this.tabPrePost.Controls.Add(this.label14);
			this.tabPrePost.Controls.Add(this.label19);
			this.tabPrePost.Controls.Add(this.lstPrePost);
			this.tabPrePost.Controls.Add(this.lstPrePostCategories);
			this.tabPrePost.Controls.Add(this.label4);
			this.tabPrePost.Location = new System.Drawing.Point(4, 22);
			this.tabPrePost.Name = "tabPrePost";
			this.tabPrePost.Size = new System.Drawing.Size(618, 321);
			this.tabPrePost.TabIndex = 3;
			this.tabPrePost.Text = "Pre/Post Briefing";
			// 
			// lstMessages
			// 
			this.lstMessages.FormattingEnabled = true;
			this.lstMessages.Location = new System.Drawing.Point(6, 56);
			this.lstMessages.Name = "lstMessages";
			this.lstMessages.ScrollAlwaysVisible = true;
			this.lstMessages.Size = new System.Drawing.Size(120, 173);
			this.lstMessages.TabIndex = 0;
			this.lstMessages.SelectedIndexChanged += new System.EventHandler(this.lstMessages_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(452, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "These .WAV files must reside in the \\Wave\\ subdirectory, custom child directories" +
    " are allowed.";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(452, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "These .WAV files must reside in the \\Wave\\ subdirectory, custom child directories" +
    " are allowed.";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(559, 26);
			this.label3.TabIndex = 2;
			this.label3.Text = "These .WAV files follow a specific location and naming convention. Files chosen w" +
    "ill be copied/renamed accordingly.\r\nThese are for Strings only and fire during t" +
    "he matching Caption event.";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 3);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(559, 26);
			this.label4.TabIndex = 3;
			this.label4.Text = "These .WAV files follow a specific location and naming convention. Files chosen w" +
    "ill be copied/renamed accordingly.\r\nIt\'s suggested that each paragraph be its ow" +
    "n WAV.";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 40);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 13);
			this.label5.TabIndex = 2;
			this.label5.Text = "Messages";
			// 
			// lblMessage
			// 
			this.lblMessage.AutoSize = true;
			this.lblMessage.Location = new System.Drawing.Point(132, 56);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(75, 13);
			this.lblMessage.TabIndex = 3;
			this.lblMessage.Text = "(message text)";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(132, 87);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(38, 13);
			this.label6.TabIndex = 4;
			this.label6.Text = "Notes:";
			// 
			// lblNotes
			// 
			this.lblNotes.AutoSize = true;
			this.lblNotes.Location = new System.Drawing.Point(176, 87);
			this.lblNotes.Name = "lblNotes";
			this.lblNotes.Size = new System.Drawing.Size(39, 13);
			this.lblNotes.TabIndex = 5;
			this.lblNotes.Text = "(notes)";
			// 
			// txtMessage
			// 
			this.txtMessage.Location = new System.Drawing.Point(193, 132);
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(184, 20);
			this.txtMessage.TabIndex = 6;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(132, 135);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(55, 13);
			this.label7.TabIndex = 7;
			this.label7.Text = "Wave file:";
			// 
			// cmdMessage
			// 
			this.cmdMessage.Location = new System.Drawing.Point(383, 130);
			this.cmdMessage.Name = "cmdMessage";
			this.cmdMessage.Size = new System.Drawing.Size(25, 23);
			this.cmdMessage.TabIndex = 8;
			this.cmdMessage.Text = "...";
			this.cmdMessage.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(132, 109);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(62, 13);
			this.label8.TabIndex = 9;
			this.label8.Text = "Flightgroup:";
			// 
			// lblFG
			// 
			this.lblFG.AutoSize = true;
			this.lblFG.Location = new System.Drawing.Point(200, 109);
			this.lblFG.Name = "lblFG";
			this.lblFG.Size = new System.Drawing.Size(62, 13);
			this.lblFG.TabIndex = 10;
			this.lblFG.Text = "(flightgroup)";
			// 
			// cmdPlayMessage
			// 
			this.cmdPlayMessage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmdPlayMessage.BackgroundImage")));
			this.cmdPlayMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.cmdPlayMessage.Location = new System.Drawing.Point(414, 130);
			this.cmdPlayMessage.Name = "cmdPlayMessage";
			this.cmdPlayMessage.Size = new System.Drawing.Size(25, 23);
			this.cmdPlayMessage.TabIndex = 11;
			this.cmdPlayMessage.UseVisualStyleBackColor = true;
			this.cmdPlayMessage.Click += new System.EventHandler(this.cmdPlay_Click);
			// 
			// cmdPlayEom
			// 
			this.cmdPlayEom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmdPlayEom.BackgroundImage")));
			this.cmdPlayEom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.cmdPlayEom.Location = new System.Drawing.Point(414, 130);
			this.cmdPlayEom.Name = "cmdPlayEom";
			this.cmdPlayEom.Size = new System.Drawing.Size(25, 23);
			this.cmdPlayEom.TabIndex = 22;
			this.cmdPlayEom.UseVisualStyleBackColor = true;
			this.cmdPlayEom.Click += new System.EventHandler(this.cmdPlay_Click);
			// 
			// cmdEom
			// 
			this.cmdEom.Location = new System.Drawing.Point(383, 130);
			this.cmdEom.Name = "cmdEom";
			this.cmdEom.Size = new System.Drawing.Size(25, 23);
			this.cmdEom.TabIndex = 19;
			this.cmdEom.Text = "...";
			this.cmdEom.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(132, 135);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(55, 13);
			this.label11.TabIndex = 18;
			this.label11.Text = "Wave file:";
			// 
			// txtEom
			// 
			this.txtEom.Location = new System.Drawing.Point(193, 132);
			this.txtEom.Name = "txtEom";
			this.txtEom.Size = new System.Drawing.Size(184, 20);
			this.txtEom.TabIndex = 17;
			// 
			// lblEomNote
			// 
			this.lblEomNote.AutoSize = true;
			this.lblEomNote.Location = new System.Drawing.Point(176, 87);
			this.lblEomNote.Name = "lblEomNote";
			this.lblEomNote.Size = new System.Drawing.Size(34, 13);
			this.lblEomNote.TabIndex = 16;
			this.lblEomNote.Text = "(note)";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(132, 87);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(38, 13);
			this.label13.TabIndex = 15;
			this.label13.Text = "Notes:";
			// 
			// lblEom
			// 
			this.lblEom.AutoSize = true;
			this.lblEom.Location = new System.Drawing.Point(132, 56);
			this.lblEom.Name = "lblEom";
			this.lblEom.Size = new System.Drawing.Size(55, 13);
			this.lblEom.TabIndex = 14;
			this.lblEom.Text = "(message)";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(6, 40);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(55, 13);
			this.label15.TabIndex = 13;
			this.label15.Text = "Messages";
			// 
			// lstEom
			// 
			this.lstEom.FormattingEnabled = true;
			this.lstEom.Items.AddRange(new object[] {
            "Primary Complete 1",
            "Primary Complete 2",
            "Primary Failed 1",
            "Primary Failed 2",
            "Outstanding Comp 1",
            "Outstanding Comp 2"});
			this.lstEom.Location = new System.Drawing.Point(6, 56);
			this.lstEom.Name = "lstEom";
			this.lstEom.Size = new System.Drawing.Size(120, 82);
			this.lstEom.TabIndex = 12;
			this.lstEom.SelectedIndexChanged += new System.EventHandler(this.lstEom_SelectedIndexChanged);
			// 
			// opnWav
			// 
			this.opnWav.Filter = "Wave files|*.wav";
			// 
			// cmdPlayBriefing
			// 
			this.cmdPlayBriefing.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmdPlayBriefing.BackgroundImage")));
			this.cmdPlayBriefing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.cmdPlayBriefing.Location = new System.Drawing.Point(414, 130);
			this.cmdPlayBriefing.Name = "cmdPlayBriefing";
			this.cmdPlayBriefing.Size = new System.Drawing.Size(25, 23);
			this.cmdPlayBriefing.TabIndex = 31;
			this.cmdPlayBriefing.UseVisualStyleBackColor = true;
			this.cmdPlayBriefing.Click += new System.EventHandler(this.cmdPlay_Click);
			// 
			// cmdBriefing
			// 
			this.cmdBriefing.Location = new System.Drawing.Point(383, 130);
			this.cmdBriefing.Name = "cmdBriefing";
			this.cmdBriefing.Size = new System.Drawing.Size(25, 23);
			this.cmdBriefing.TabIndex = 30;
			this.cmdBriefing.Text = "...";
			this.cmdBriefing.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(132, 135);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(55, 13);
			this.label9.TabIndex = 29;
			this.label9.Text = "Wave file:";
			// 
			// txtBriefing
			// 
			this.txtBriefing.Location = new System.Drawing.Point(193, 132);
			this.txtBriefing.Name = "txtBriefing";
			this.txtBriefing.ReadOnly = true;
			this.txtBriefing.Size = new System.Drawing.Size(184, 20);
			this.txtBriefing.TabIndex = 28;
			// 
			// lblBriefingNote
			// 
			this.lblBriefingNote.AutoSize = true;
			this.lblBriefingNote.Location = new System.Drawing.Point(176, 87);
			this.lblBriefingNote.Name = "lblBriefingNote";
			this.lblBriefingNote.Size = new System.Drawing.Size(34, 13);
			this.lblBriefingNote.TabIndex = 27;
			this.lblBriefingNote.Text = "(note)";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(132, 87);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(38, 13);
			this.label12.TabIndex = 26;
			this.label12.Text = "Notes:";
			// 
			// lblBriefing
			// 
			this.lblBriefing.Location = new System.Drawing.Point(132, 56);
			this.lblBriefing.Name = "lblBriefing";
			this.lblBriefing.Size = new System.Drawing.Size(483, 31);
			this.lblBriefing.TabIndex = 25;
			this.lblBriefing.Text = "(message)";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(6, 40);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(55, 13);
			this.label16.TabIndex = 24;
			this.label16.Text = "Messages";
			// 
			// lstBriefing
			// 
			this.lstBriefing.FormattingEnabled = true;
			this.lstBriefing.Location = new System.Drawing.Point(6, 56);
			this.lstBriefing.Name = "lstBriefing";
			this.lstBriefing.ScrollAlwaysVisible = true;
			this.lstBriefing.Size = new System.Drawing.Size(120, 173);
			this.lstBriefing.TabIndex = 23;
			this.lstBriefing.SelectedIndexChanged += new System.EventHandler(this.lstBriefing_SelectedIndexChanged);
			// 
			// cmdPlayPrePost
			// 
			this.cmdPlayPrePost.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmdPlayPrePost.BackgroundImage")));
			this.cmdPlayPrePost.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.cmdPlayPrePost.Location = new System.Drawing.Point(411, 288);
			this.cmdPlayPrePost.Name = "cmdPlayPrePost";
			this.cmdPlayPrePost.Size = new System.Drawing.Size(25, 23);
			this.cmdPlayPrePost.TabIndex = 31;
			this.cmdPlayPrePost.UseVisualStyleBackColor = true;
			this.cmdPlayPrePost.Click += new System.EventHandler(this.cmdPlay_Click);
			// 
			// cmdPrePost
			// 
			this.cmdPrePost.Location = new System.Drawing.Point(380, 288);
			this.cmdPrePost.Name = "cmdPrePost";
			this.cmdPrePost.Size = new System.Drawing.Size(25, 23);
			this.cmdPrePost.TabIndex = 30;
			this.cmdPrePost.Text = "...";
			this.cmdPrePost.UseVisualStyleBackColor = true;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(129, 293);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(55, 13);
			this.label10.TabIndex = 29;
			this.label10.Text = "Wave file:";
			// 
			// txtPrePostWav
			// 
			this.txtPrePostWav.Location = new System.Drawing.Point(190, 290);
			this.txtPrePostWav.Name = "txtPrePostWav";
			this.txtPrePostWav.ReadOnly = true;
			this.txtPrePostWav.Size = new System.Drawing.Size(184, 20);
			this.txtPrePostWav.TabIndex = 28;
			// 
			// lblPrePostNote
			// 
			this.lblPrePostNote.AutoSize = true;
			this.lblPrePostNote.Location = new System.Drawing.Point(176, 56);
			this.lblPrePostNote.Name = "lblPrePostNote";
			this.lblPrePostNote.Size = new System.Drawing.Size(34, 13);
			this.lblPrePostNote.TabIndex = 27;
			this.lblPrePostNote.Text = "(note)";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(132, 56);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(38, 13);
			this.label17.TabIndex = 26;
			this.label17.Text = "Notes:";
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(6, 40);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(57, 13);
			this.label19.TabIndex = 24;
			this.label19.Text = "Categories";
			// 
			// lstPrePostCategories
			// 
			this.lstPrePostCategories.FormattingEnabled = true;
			this.lstPrePostCategories.Items.AddRange(new object[] {
            "Pre-mission Banter",
            "Mission Description",
            "Successful Debrief"});
			this.lstPrePostCategories.Location = new System.Drawing.Point(6, 56);
			this.lstPrePostCategories.Name = "lstPrePostCategories";
			this.lstPrePostCategories.Size = new System.Drawing.Size(120, 43);
			this.lstPrePostCategories.TabIndex = 23;
			this.lstPrePostCategories.SelectedIndexChanged += new System.EventHandler(this.lstPrePostCategories_SelectedIndexChanged);
			// 
			// txtPrePost
			// 
			this.txtPrePost.Location = new System.Drawing.Point(135, 79);
			this.txtPrePost.Multiline = true;
			this.txtPrePost.Name = "txtPrePost";
			this.txtPrePost.ReadOnly = true;
			this.txtPrePost.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtPrePost.Size = new System.Drawing.Size(480, 203);
			this.txtPrePost.TabIndex = 32;
			// 
			// lstPrePost
			// 
			this.lstPrePost.FormattingEnabled = true;
			this.lstPrePost.Location = new System.Drawing.Point(6, 174);
			this.lstPrePost.Name = "lstPrePost";
			this.lstPrePost.Size = new System.Drawing.Size(120, 108);
			this.lstPrePost.TabIndex = 23;
			this.lstPrePost.SelectedIndexChanged += new System.EventHandler(this.lstPrePost_SelectedIndexChanged);
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(6, 158);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(55, 13);
			this.label14.TabIndex = 24;
			this.label14.Text = "Messages";
			// 
			// XwaWavForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(645, 418);
			this.Controls.Add(this.tabsWav);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "XwaWavForm";
			this.Text = "XWA WAV Manager";
			this.tabsWav.ResumeLayout(false);
			this.tabMessages.ResumeLayout(false);
			this.tabMessages.PerformLayout();
			this.tabEom.ResumeLayout(false);
			this.tabEom.PerformLayout();
			this.tabBriefing.ResumeLayout(false);
			this.tabBriefing.PerformLayout();
			this.tabPrePost.ResumeLayout(false);
			this.tabPrePost.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabsWav;
		private System.Windows.Forms.TabPage tabMessages;
		private System.Windows.Forms.TabPage tabEom;
		private System.Windows.Forms.TabPage tabBriefing;
		private System.Windows.Forms.TabPage tabPrePost;
		private System.Windows.Forms.Button cmdMessage;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.Label lblNotes;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lstMessages;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblFG;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button cmdPlayMessage;
		private System.Windows.Forms.Button cmdPlayEom;
		private System.Windows.Forms.Button cmdEom;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txtEom;
		private System.Windows.Forms.Label lblEomNote;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label lblEom;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.ListBox lstEom;
		private System.Windows.Forms.OpenFileDialog opnWav;
		private System.Windows.Forms.Button cmdPlayBriefing;
		private System.Windows.Forms.Button cmdBriefing;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtBriefing;
		private System.Windows.Forms.Label lblBriefingNote;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label lblBriefing;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.ListBox lstBriefing;
		private System.Windows.Forms.TextBox txtPrePost;
		private System.Windows.Forms.Button cmdPlayPrePost;
		private System.Windows.Forms.Button cmdPrePost;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtPrePostWav;
		private System.Windows.Forms.Label lblPrePostNote;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.ListBox lstPrePost;
		private System.Windows.Forms.ListBox lstPrePostCategories;
	}
}