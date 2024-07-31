/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.13.6+
 */

/* CHANGELOG
 * [UPD] cleanup
 * v1.13.6, 220619
 * [NEW] Last Mission option
 * v1.5, 180910
 * [NEW] Xwing [JB]
 * v1.2.3, 141214
 * [UPD] change to MPL
 * v1.2.2, 121022
 * [DEL] ctr(Settings, string)
 * v1.2, 121006
 * - Settings passed in from Program
 * v1.1.1, 120814
 * - condensed
 * - class renamed
 * v1.0, 110921
 * - Release
 */

using System;
using System.IO;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>The initial interface and effective program start</summary>
	public partial class StartForm : Form
	{
        readonly Settings _config = Settings.GetInstance();

		public StartForm()
		{
			InitializeComponent();

			// Settings.LoadSettings already called, so has Settings.CheckPlatforms
			if (_config.RestrictPlatforms && !_config.XwingInstalled && !_config.TieInstalled && !_config.XvtInstalled && !_config.XwaInstalled) _config.RestrictPlatforms = false;
            optXWING.Enabled = (_config.XwingInstalled || !_config.RestrictPlatforms);
            optTIE.Enabled = (_config.TieInstalled || !_config.RestrictPlatforms);
			optXvT.Enabled = (_config.XvtInstalled || !_config.RestrictPlatforms);
			chkBoP.Enabled = (_config.BopInstalled || !_config.RestrictPlatforms);
			optXWA.Enabled = (_config.XwaInstalled || !_config.RestrictPlatforms);
            optXWING.Checked = (_config.LastPlatform == Settings.Platform.XWING);
            optTIE.Checked = (_config.LastPlatform == Settings.Platform.TIE);
			optXvT.Checked = (_config.LastPlatform == Settings.Platform.XvT);
			chkBoP.Checked = (_config.LastPlatform == Settings.Platform.BoP);
			optXWA.Checked = (_config.LastPlatform == Settings.Platform.XWA);
			// using RecentMissions[1] here, since LastMission gets wiped if launching to a platform with a new file
			if (_config.RecentMissions[1] == "") optLastMission.Enabled = false;
			else optLastMission.Text += " (" + Path.GetFileNameWithoutExtension(_config.RecentMissions[1]) + ")";
		}

		void chkBoP_CheckedChanged(object sender, EventArgs e) { if (chkBoP.Checked) optXvT.Checked = true; }

		void cmdCancel_Click(object sender, EventArgs e) => Close();
		void cmdOK_Click(object sender, EventArgs e)
		{
			if (!optXWING.Checked && !optTIE.Checked && !optXvT.Checked && !optXWA.Checked && !optLastMission.Checked) 
			{
				MessageBox.Show("Please select a platform");
				return;
			}
			Hide();
            if (optXWING.Checked) new XwingForm().Show();
            else if (optTIE.Checked) new TieForm().Show();
			else if (optXvT.Checked) new XvtForm(chkBoP.Checked).Show();
			else if (optXWA.Checked) new XwaForm().Show();
			else
            {
				switch (_config.LastPlatform)
				{
					case Settings.Platform.XWING: new XwingForm(_config.RecentMissions[1]).Show(); break;
					case Settings.Platform.TIE: new TieForm(_config.RecentMissions[1]).Show(); break;
					case Settings.Platform.XvT: new XvtForm(_config.RecentMissions[1]).Show(); break;
					case Settings.Platform.BoP: new XvtForm(_config.RecentMissions[1]).Show(); break;
					case Settings.Platform.XWA: new XwaForm(_config.RecentMissions[1]).Show(); break;
				}
			}
		}

		void form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (_config.ConfirmExit)
			{
				DialogResult res = MessageBox.Show("Do you really want to exit?", "Confirm Exit", MessageBoxButtons.YesNo);
				if (res == DialogResult.No) { e.Cancel = true; return; }
			}
			Application.Exit();
		}
	}
}