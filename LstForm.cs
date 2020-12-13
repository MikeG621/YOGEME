/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2020 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.8.1
 */

/* CHANGELOG
 * v1.8.1, 201213
 * [UPD] Settings passed in instead of re-init
 * v1.4, 171016
 * [ADD #11] made form resizable, although Height only
 * v1.2.3, 141214
 * [UPD] change to MPL
 * [FIX] close the form if platform isn't isntalled
 * v1.1.1, 120814
 * - class renamed
 * v1.0, 110921
 * - Release
 */

using System;
using System.IO;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>One of the simplest forms, allows for LST editing</summary>
	public partial class LstForm : Form
	{
		readonly string _installPath;

		/// <summary>Initialize the form, gather file listings from all categories</summary>
		/// <param name="plat">Platform identifier</param>
		public LstForm(Settings.Platform platform, Settings config)
		{
			InitializeComponent();
			if (platform==Settings.Platform.XvT) _installPath = config.XvtPath;
			else if (platform==Settings.Platform.BoP) _installPath = config.BopPath;
			else if (platform==Settings.Platform.XWA) _installPath = config.XwaPath;
			if (!Directory.Exists(_installPath))
			{
				MessageBox.Show("Error reading install path for platform, LST editor unavailable", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
				return;
			}
			if (platform==Settings.Platform.XWA) 
			{
				System.Drawing.Size max = MaximumSize;
				max.Width += 190;
				MaximumSize = max;
				Width = max.Width;
				System.Drawing.Size min = MinimumSize;
				min.Width = Width;
				MinimumSize = min;
				lblEx.Left += 75;
				txtLST.Width += 75;
				lblEx.Width += 115;
				lblEx.Text="Example:\r\n\r\n//\r\n!BATTLE_0_HEADER![Section Title]\r\n//\r\n0\r\n* 1B0M1ex.tie\r\n!MISSION_0_DESC!Mission description\r\n1\r\n* 1B0M2ex.tie\r\n"+
					"!MISSION_1_DESC!Keep to 1 line\r\n//\r\n!BATTLE_!_HEADER![Next Section]\r\n//\r\n2\r\n* 1B1M1ex.tie\r\n!MISSION_2_DESC!\"1B#M#*.tie\" req'd\r\n3\r\n"+
					"* 1B1M2ex.tie\r\n!MISSION_3_DESC!etc";
				cboFile.Items.Add("MELEE\\MISSION.LST");
				cboFile.Items.Add("MISSIONS\\MISSION.LST");
				cboFile.SelectedIndex = 1;
			}
			if (platform==Settings.Platform.XvT || platform==Settings.Platform.BoP)
			{
				string[] str1 = Directory.GetFiles(_installPath+"\\Battle", "*.LST");
				string[] str6 = null;
				if (platform==Settings.Platform.BoP) str6 = Directory.GetFiles(_installPath+"\\Campaign", "*.LST");
				string[] str2 = Directory.GetFiles(_installPath+"\\Combat", "*.LST");
				string[] str3 = Directory.GetFiles(_installPath+"\\Melee", "*.LST");
				string[] str4 = Directory.GetFiles(_installPath+"\\Tourn", "*.LST");
				string[] str5 = Directory.GetFiles(_installPath+"\\Train", "*.LST");
				string[] strFiles = new string[str1.Length + (str6 != null ? str6.Length : 0) + str2.Length + str3.Length + str4.Length + str5.Length];
				int j=0, start;
				for (int i=0;i<str1.Length;i++, j++) strFiles[j] = str1[i].Substring(_installPath.Length+1);
				if (platform==Settings.Platform.BoP) for (int i=0;i<str6.Length;i++, j++) strFiles[j] = str6[i].Substring(_installPath.Length+1);
				for (int i=0;i<str2.Length;i++, j++) strFiles[j] = str2[i].Substring(_installPath.Length+1);
				for (int i=0;i<str3.Length;i++, j++) strFiles[j] = str3[i].Substring(_installPath.Length+1);
				for (int i=0;i<str4.Length;i++, j++) strFiles[j] = str4[i].Substring(_installPath.Length+1);
				start=j;
				for (int i=0;i<str5.Length;i++, j++) strFiles[j] = str5[i].Substring(_installPath.Length+1);
				cboFile.Items.AddRange(strFiles);
				cboFile.SelectedIndex = start;
			}
		}

		private void cboFile_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboFile.SelectedIndex == -1) return;
			txtLST.Text = "";
			StreamReader sr = File.OpenText(_installPath+"\\"+cboFile.Text);
			string s;
			while ((s = sr.ReadLine()) != null) txtLST.Text += s + "\r\n";
			sr.Close();
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			StreamWriter sw = new FileInfo(_installPath+"\\"+cboFile.Text).CreateText();
			sw.Write(txtLST.Text);
			sw.Close();
		}

		private void form_Resize(object sender, EventArgs e)
		{
			txtLST.Height = Height - 117;
		}
	}
}
