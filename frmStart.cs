/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.1
 */

/* CHANGELOG
 * v1.0, 110921
 * - Release
 */

using System;
using System.IO;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>The initial interface and effective program start</summary>
	public partial class frmStart : Form
	{
		Settings config = new Settings();

		public frmStart()
		{
			InitializeComponent();
			//load config, create if needed

			// Settings.LoadSettings already called, if applicable so has Settings.CheckPlatforms
			if (config.RestrictPlatforms && !config.TieInstalled && !config.XvtInstalled && !config.XwaInstalled) config.RestrictPlatforms = false;
			if (config.TieInstalled || !config.RestrictPlatforms) optTIE.Enabled = true;
			if (config.XvtInstalled || !config.RestrictPlatforms) optXvT.Enabled = true;
			if (config.BopInstalled || !config.RestrictPlatforms) chkBoP.Enabled = true;
			if (config.XwaInstalled || !config.RestrictPlatforms) optXWA.Enabled = true;
			//not using switch here to save lines
			if (config.LastPlatform == Settings.Platform.TIE) optTIE.Checked = true;
			else if (config.LastPlatform == Settings.Platform.XvT) optXvT.Checked = true;
			else if (config.LastPlatform == Settings.Platform.BoP) { optXvT.Checked = true; chkBoP.Checked = true; }
			else if (config.LastPlatform == Settings.Platform.XWA) optXWA.Checked = true;
		}

		/// <summary>From the command line or if Settings.Startup == LastMission</summary>
		/// <param name="missionPath">Path to the mission file</param>
		public frmStart(string missionPath)
		{
			InitializeComponent();

			Hide();
			try
			{
				FileStream fsPlat = File.OpenRead(missionPath);
				BinaryReader br = new BinaryReader(fsPlat);
				short t = br.ReadInt16();
				fsPlat.Close();
				switch (t)
				{
					case 12:
						new frmXvT(missionPath).Show();
						break;
					case 14:
						new frmXvT(missionPath).Show();
						break;
					case 18:
						new frmXWA(missionPath).Show();
						break;
					case -1:
						new frmTIE(missionPath).Show();
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

		void chkBoP_CheckedChanged(object sender, EventArgs e)
		{
			if (chkBoP.Checked) optXvT.Checked = true;
		}

		void cmdCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
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
				frmTIE fTIE = new frmTIE();
				fTIE.Show();
			}
			if (optXvT.Checked)
			{
				Hide();
				bool BoP = chkBoP.Checked; 
				frmXvT fXvT = new frmXvT(BoP);
				fXvT.Show();
			}
			if (optXWA.Checked)
			{
				Hide();
				frmXWA fXWA = new frmXWA();
				fXWA.Show();
			}
		}

		void frmStart_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (config.ConfirmExit)
			{
				DialogResult res = MessageBox.Show("Do you really want to exit?","Confirm Exit",MessageBoxButtons.YesNo);
				if (res == DialogResult.No) { e.Cancel = true; return; }
			}
			Application.Exit();
		}
	}
}
