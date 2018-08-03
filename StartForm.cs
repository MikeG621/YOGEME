/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2018 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.2.3+
 */

/* CHANGELOG
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
		Settings _config;
		bool _exit = true;

		public StartForm(Settings settings)
		{
			_config = settings;
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
		}

		void chkBoP_CheckedChanged(object sender, EventArgs e) { if (chkBoP.Checked) optXvT.Checked = true; }

		void cmdCancel_Click(object sender, EventArgs e) { Close(); }
		void cmdOK_Click(object sender, EventArgs e)
		{
			if (!optXWING.Checked && !optTIE.Checked && !optXvT.Checked && !optXWA.Checked) 
			{
				MessageBox.Show("Please select a platform");
				return;
			}
            if (optXWING.Checked)
            {
                Hide();
                new XwingForm(_config).Show();
            } 
            if (optTIE.Checked)
			{
				Hide();
				new TieForm(_config).Show();
			}
			if (optXvT.Checked)
			{
				Hide();
				new XvtForm(_config, chkBoP.Checked).Show();
			}
			if (optXWA.Checked)
			{
				Hide();
				new XwaForm(_config).Show();
			}
		}

		void frmStart_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (_exit)
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
}
