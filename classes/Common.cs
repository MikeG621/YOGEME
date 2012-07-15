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
using System.Diagnostics;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>Functions that apply to all platforms</summary>
	public static class Common
	{
		/// <summary>Launch a browser at the EmpireReborn site</summary>
		public static void LaunchER() { Process.Start("http://empirereborn.net"); }

		/// <summary>Launch a browser at the IDMR site</summary>
		public static void LaunchIdmr() { Process.Start("http://idmr.empirereborn.net"); }

		public static void LaunchHelp() { Process.Start("yogeme.chm"); }
		public static void EmailJagged() { Process.Start("mailto:mjgaisser@gmail.com?subject=YOGEME"); }

		/// <summary>Run MissionVerify.exe on open mission</summary>
		/// <remarks>MissionVerify part is take from <see cref="Settings.VerifyLocation"/></remarks>
		/// <param name="path">Path to mission file</param>
		public static void RunVerify(string missionPath, string verifyPath)
		{
			try
			{
				Process MV = new Process();
				MV.StartInfo.FileName = verifyPath;
				MV.StartInfo.Arguments = missionPath;
				MV.Start();
			}
			catch (Exception x) { MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}
		
		/// <summary>Run Converter.exe on open mission</summary>
		/// <param name="current">Path to mission file</param>
		/// <param name="mode">0 = TIE to XvT, 1 = TIE to XWA, 2 = XvT to XWA</param>
		public static void RunConverter(string current, byte mode)
		{
			try
			{
				Process MV = new Process();
				MV.StartInfo.FileName = Application.StartupPath + "\\Converter.exe";
				MV.StartInfo.Arguments = current + " " + mode;
				MV.Start();
			}
			catch (Exception x) { MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}

		/// <summary>Adds an asterik to the window title</summary>
		/// <param name="form"></param>
		/// <param name="loading"></param>
		public static void Title(Form form, bool loading) { if (form.Text.IndexOf("*") == -1 && !loading) form.Text += "*"; }

		/// <summary>Adds an asterik to the window title if change detected</summary>
		/// <param name="form"></param>
		/// <param name="oldValue">Original value to compare against</param>
		/// <param name="newValue">New value from the UI</param>
		/// <returns>newValue</returns>
		public static object Update(Form form, object oldValue, object newValue)
		{
			if (form.Text.IndexOf("*") == -1 && oldValue.ToString() != newValue.ToString()) form.Text += "*";
			return newValue;
		}
	}
}
