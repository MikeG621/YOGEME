/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.2.1
 */

/* CHANGELOG
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

		public StartForm(Settings settings)
		{
			_config = settings;
			InitializeComponent();

			// Settings.LoadSettings already called, so has Settings.CheckPlatforms
			if (_config.RestrictPlatforms && !_config.TieInstalled && !_config.XvtInstalled && !_config.XwaInstalled) _config.RestrictPlatforms = false;
			optTIE.Enabled = (_config.TieInstalled || !_config.RestrictPlatforms);
			optXvT.Enabled = (_config.XvtInstalled || !_config.RestrictPlatforms);
			chkBoP.Enabled = (_config.BopInstalled || !_config.RestrictPlatforms);
			optXWA.Enabled = (_config.XwaInstalled || !_config.RestrictPlatforms);
			optTIE.Checked = (_config.LastPlatform == Settings.Platform.TIE);
			optXvT.Checked = (_config.LastPlatform == Settings.Platform.XvT);
			chkBoP.Checked = (_config.LastPlatform == Settings.Platform.BoP);
			optXWA.Checked = (_config.LastPlatform == Settings.Platform.XWA);
		}

		/// <summary>From the command line or if Settings.Startup == LastMission</summary>
		/// <param name="missionPath">Path to the mission file</param>
		public StartForm(Settings settings, string missionPath)
		{
			_config = settings;
			InitializeComponent();

			Hide();
			try
			{
				FileStream fsPlat = File.OpenRead(missionPath);
				short t = new BinaryReader(fsPlat).ReadInt16();
				fsPlat.Close();
				switch (t)
				{
					case 12:
						new XvtForm(_config, missionPath).Show();
						break;
					case 14:
						new XvtForm(_config, missionPath).Show();
						break;
					case 18:
						new XwaForm(_config, missionPath).Show();
						break;
					case -1:
						new TieForm(_config, missionPath).Show();
						break;
					default:
						throw new Exception("File is either invalid or corrupted.\nPlease ensure the correct file was selected.");
				}
			}
			catch (Exception e)
			{
				Show();
				MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}

		void chkBoP_CheckedChanged(object sender, EventArgs e) { if (chkBoP.Checked) optXvT.Checked = true; }

		void cmdCancel_Click(object sender, EventArgs e) { Close(); }
		void cmdOK_Click(object sender, EventArgs e)
		{
			if (!optTIE.Checked && !optXvT.Checked && !optXWA.Checked) 
			{
				MessageBox.Show("Please select a platform");
				return;
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
			if (_config.ConfirmExit)
			{
				DialogResult res = MessageBox.Show("Do you really want to exit?", "Confirm Exit", MessageBoxButtons.YesNo);
				if (res == DialogResult.No) { e.Cancel = true; return; }
			}
			Application.Exit();
		}
	}
}
