/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.1.1
 */

/* CHANGELOG
 * v1.1.1, 120814
 * - class renamed
 * v1.1, 120715
 * - Controls (Disabled) added for ConfirmTest, DeleteTestPilots and Verify Test
 * v1.0, 110921
 * - Release
 */

using System;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>Options dialog for YOGEME</summary>
	public partial class OptionsDialog : Form
	{
		CheckBox[] chkWP = new CheckBox[22];
		Settings _config;

		/// <summary>Initialize and load the user's settings</summary>
		/// <param name="config">The Settings config of the current user</param>
		public OptionsDialog(Settings config)
		{
			InitializeComponent();
			chkWP[0] = chkSP1;
			chkWP[1] = chkSP2;
			chkWP[2] = chkSP3;
			chkWP[3] = chkSP4;
			chkWP[4] = chkWP1;
			chkWP[5] = chkWP2;
			chkWP[6] = chkWP3;
			chkWP[7] = chkWP4;
			chkWP[8] = chkWP5;
			chkWP[9] = chkWP6;
			chkWP[10] = chkWP7;
			chkWP[11] = chkWP8;
			chkWP[12] = chkRND;
			chkWP[13] = chkHYP;
			chkWP[14] = chkBRF;
			chkWP[15] = chkBRF2;
			chkWP[16] = chkBRF3;
			chkWP[17] = chkBRF4;
			chkWP[18] = chkBRF5;
			chkWP[19] = chkBRF6;
			chkWP[20] = chkBRF7;
			chkWP[21] = chkBRF8;
			_config = config;
			switch(_config.Startup)
			{
				case Settings.StartupMode.Normal:
					optStartNormal.Checked = true;
					break;
				case Settings.StartupMode.LastPlatform:
					optStartPlat.Checked = true;
					break;
				case Settings.StartupMode.LastMission:
					optStartMiss.Checked = true;
					break;
				default:
					optStartNormal.Checked = true;
					break;
			}
			chkRestrict.Checked = _config.RestrictPlatforms;
			chkExit.Checked = _config.ConfirmExit;
			chkSave.Checked = _config.ConfirmSave;
			chkVerify.Checked = _config.Verify;
			txtVerify.Text = _config.VerifyLocation;
			chkTIEInstall.Checked = _config.TieInstalled;
			txtTIE.Text = _config.TiePath;
			txtTIE.Enabled = chkTIEInstall.Checked;
			cboTIECraft.Items.AddRange(Platform.Tie.Strings.CraftType);
			cboTIECraft.SelectedIndex = _config.TieCraft;
			cboTIEIFF.SelectedIndex = _config.TieIff;
			chkXvTInstall.Checked = _config.XvtInstalled;
			txtXvT.Text = _config.XvtPath;
			txtXvT.Enabled = chkXvTInstall.Checked;
			cboXvTCraft.Items.AddRange(Platform.Xvt.Strings.CraftType);
			cboXvTCraft.SelectedIndex = _config.XvtCraft;
			cboXvTIFF.SelectedIndex = _config.XvtIff;
			chkBoPInstall.Checked = _config.BopInstalled;
			txtBoP.Text = _config.BopPath;
			txtBoP.Enabled = chkBoPInstall.Checked;
			chkXWAInstall.Checked = _config.XwaInstalled;
			txtXWA.Text = _config.XwaPath;
			txtXWA.Enabled = chkXWAInstall.Checked;
			cboXWACraft.Items.AddRange(Platform.Xwa.Strings.CraftType);
			cboXWACraft.SelectedIndex = _config.XwaCraft;
			cboXWAIFF.SelectedIndex = _config.XwaIff;
			chkFG.Checked = Convert.ToBoolean(_config.MapOptions & Settings.MapOpts.FGTags);
			chkTrace.Checked = Convert.ToBoolean(_config.MapOptions & Settings.MapOpts.Traces);
			chkDeletePilot.Checked = _config.DeleteTestPilots;
			chkTest.Checked = _config.ConfirmTest;
			chkVerifyTest.Checked = _config.VerifyTest;
			chkVerifyTest.Enabled = !_config.Verify;
			int t = _config.Waypoints;
			for (int i=0;i<22;i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
		}

		void selectPlatform(TextBox txt, CheckBox chk)
		{
			try { dirPlatform.SelectedPath = txt.Text; }
			catch { /*do nothing*/ }
			if (dirPlatform.ShowDialog() == DialogResult.OK)
			{
				txt.Text = dirPlatform.SelectedPath;
				if (!chk.Checked) chk.Checked = true;
			}
		}

		void chkTIEInstall_CheckedChanged(object sender, EventArgs e)
		{
			txtTIE.Enabled = chkTIEInstall.Checked;
		}
		void chkXvTInstall_CheckedChanged(object sender, EventArgs e)
		{
			txtXvT.Enabled = chkXvTInstall.Checked;
		}
		void chkBoPInstall_CheckedChanged(object sender, EventArgs e)
		{
			txtBoP.Enabled = chkBoPInstall.Checked;
		}
		void chkXWAInstall_CheckedChanged(object sender, EventArgs e)
		{
			txtXWA.Enabled = chkXWAInstall.Checked;
		}
		void chkVerify_CheckedChanged(object sender, EventArgs e)
		{
			chkVerifyTest.Enabled = chkVerify.Checked;
		}

		void cmdBop_Click(object sender, EventArgs e)
		{
			selectPlatform(txtBoP, chkBoPInstall);
		}
		void cmdCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
		void cmdOK_Click(object sender, EventArgs e)
		{
			if (optStartNormal.Checked) _config.Startup = Settings.StartupMode.Normal;
			else if (optStartPlat.Checked) _config.Startup = Settings.StartupMode.LastPlatform;
			else _config.Startup = Settings.StartupMode.LastMission;
			_config.RestrictPlatforms = chkRestrict.Checked;
			_config.ConfirmExit = chkExit.Checked;
			_config.ConfirmSave = chkSave.Checked;
			_config.TieInstalled = chkTIEInstall.Checked;
			_config.TiePath = txtTIE.Text;
			_config.XvtInstalled = chkXvTInstall.Checked;
			_config.XvtPath = txtXvT.Text;
			_config.BopInstalled = chkBoPInstall.Checked;
			_config.BopPath = txtBoP.Text;
			_config.XwaInstalled = chkXWAInstall.Checked;
			_config.XwaPath = txtXWA.Text;
			_config.Verify = chkVerify.Checked;
			_config.VerifyLocation = txtVerify.Text;
			int temp=0;
			for(int i=0;i<22;i++) temp += (chkWP[i].Checked ? 1<<i : 0);
			_config.Waypoints = temp;
			_config.MapOptions = (chkFG.Checked ? Settings.MapOpts.FGTags : Settings.MapOpts.None) | (chkTrace.Checked ? Settings.MapOpts.Traces : Settings.MapOpts.None);
			_config.TieCraft = (byte)cboTIECraft.SelectedIndex;
			_config.TieIff = (byte)cboTIEIFF.SelectedIndex;
			_config.XvtCraft = (byte)cboXvTCraft.SelectedIndex;
			_config.XvtIff = (byte)cboXvTIFF.SelectedIndex;
			_config.XwaCraft = (byte)cboXWACraft.SelectedIndex;
			_config.XwaIff = (byte)cboXWAIFF.SelectedIndex;
			_config.ConfirmTest = chkTest.Checked;
			_config.DeleteTestPilots = chkDeletePilot.Checked;
			_config.VerifyTest = chkVerifyTest.Checked;
			Close();
		}
		void cmdTie_Click(object sender, EventArgs e)
		{
			selectPlatform(txtTIE, chkTIEInstall);
		}
		void cmdVerify_Click(object sender, EventArgs e)
		{
			try { opnVerify.InitialDirectory = txtVerify.Text.Substring(0, txtVerify.Text.LastIndexOf("\\")); }
			catch { opnVerify.InitialDirectory = Application.StartupPath; }
			if (opnVerify.ShowDialog() == DialogResult.OK)
				txtVerify.Text = opnVerify.FileName;
		}
		void cmdXvt_Click(object sender, EventArgs e)
		{
			selectPlatform(txtXvT, chkXvTInstall);
		}
		void cmdXwa_Click(object sender, EventArgs e)
		{
			selectPlatform(txtXWA, chkXWAInstall);
		}
	}
}
