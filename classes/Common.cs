/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2015 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.2.7+
 */

/* CHANGELOG
 * [NEW #10] Craft list override
 * v1.2.7, 150405
 * [FIX #7] Added quotes to Verify arguments
 * v1.2.5, 150110
 * [UPD #3] Update changed to generic [JeremyAnsel]
 * v1.2.3, 141214
 * [UPD] change to MPL
 * v1.0, 110921
 * - Release
 */

using System;
using System.Diagnostics;
using System.IO;
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

		/// <summary>Processes properly formatted custom craft list files to be used in YOGEME.</summary>
		/// <param name="filePath">The full path to the shiplist file.</param>
		/// <param name="types">The output array of craft types.</param>
		/// <param name="abbrvs">The output array of craft type abbreviations.</param>
		/// <exception cref="ApplicationException">Throw if an error occurs processing the file.</exception>
		/// <remarks>Each line of the file must be in the format "<b>CraftType</b>,<b>Abbrv</b>" without quotes, such as "X-Wing,X-W".</remarks>
		public static void ProcessCraftList(string filePath, out string[] types, out string[] abbrvs)
		{
			if (!File.Exists(filePath))
			{
				types = null;
				abbrvs = null;
				return;
			}
			StreamReader sr = null;
			try
			{
				sr = File.OpenText(filePath);
				string contents = "";
				string s = null;
				while ((s = sr.ReadLine()) != null) contents += s + "\r\n";
				sr.Close();

				string[] fullList = contents.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				types = new string[fullList.Length];
				abbrvs = new string[fullList.Length];
				for (int i = 0; i < fullList.Length; i++)
				{
					string[] line = fullList[i].Split(',');
					types[i] = line[0];
					abbrvs[i] = line[1];
				}
			}
			catch { throw new ApplicationException("Error processing shiplist, ensure proper format."); }
			finally { if (sr != null) sr.Close(); }
		}

		/// <summary>Run MissionVerify.exe on open mission</summary>
		/// <remarks>MissionVerify part is take from <see cref="Settings.VerifyLocation"/></remarks>
		/// <param name="path">Path to mission file</param>
		public static void RunVerify(string missionPath, string verifyPath)
		{
			try
			{
				Process MV = new Process();
				MV.StartInfo.FileName = verifyPath;
				MV.StartInfo.Arguments = "\"" + missionPath + "\"";
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
		/// <typeparam name="T">Type of value</typeparam>
		/// <param name="form"></param>
		/// <param name="oldValue">Original value to compare against</param>
		/// <param name="newValue">New value from the UI</param>
		/// <returns>newValue</returns>
        public static T Update<T>(Form form, T oldValue, T newValue)
		{
			if (form.Text.IndexOf("*") == -1 && oldValue.ToString() != newValue.ToString()) form.Text += "*";
			return newValue;
		}
	}
}
