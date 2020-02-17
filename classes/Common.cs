/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2018 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.6.2+
 */

/* CHANGELOG
 * [FIX #32] help path now explicitly uses Startup Path to prevent implicit from defaulting to sys32
 * v1.6.2, 190928
 * [UPD] added Exists check to Verify so the exception is clearer [Issue #28]
 * v1.5, 180910
 * [NEW] FG Counter functions, IsValidArray, GetFormattedTime, SafeSetCBO [JB]
 * v1.4, 171016
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
using System.Collections.Generic;

namespace Idmr.Yogeme
{
	/// <summary>Functions that apply to all platforms</summary>
	public static class Common
	{
		/// <summary>Launch a browser at the EmpireReborn site</summary>
		public static void LaunchER() { Process.Start("http://empirereborn.net"); }

		/// <summary>Launch a browser at the IDMR site</summary>
		public static void LaunchIdmr() { Process.Start("http://idmr.empirereborn.net"); }

		public static void LaunchHelp() { Process.Start(Application.StartupPath + "yogeme.chm"); }	// BUG: this breaks if startup path borked
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
				if (!File.Exists(verifyPath)) throw new ArgumentException("Verify executable path is not valid.");
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
			if (form.Text.IndexOf("*") == -1 && oldValue.ToString() != newValue.ToString())
                form.Text += "*";
			return newValue;
		}

        /// <summary>Add or increase the count of a keyed Flight Group.</summary>
		/// <param name="craftType">The FG's <see cref="Platform.BaseFlightGroup.CraftType"/> index</param>
		/// <param name="IFF">The FG's <see cref="Platform.BaseFlightGroup.IFF"/> index</param>
		/// <param name="craftName">The FG's <see cref="Platform.BaseFlightGroup.Name"/></param>
		/// <param name="amount">The FG's <see cref="Platform.BaseFlightGroup.NumberOfCraft"/> value</param>
		/// <param name="counter">The keyed Flight Group listing</param>
        /// <remarks>This function adds a FG (with the specified parameters) to the list if it does not exist.<br/>
		/// <paramref name="counter"/> is formatted as &lt;<i>key</i>, &lt;<i><paramref name="craftName"/></i>, <i>count</i>&gt;&gt;.<br/>
		/// Keys are defined as <b>(<paramref name="craftType"/> + (<paramref name="IFF"/> &lt;&lt; 16))</b>. If it does not exist in <paramref name="counter"/> it's added.
		/// If the key does exist with the same <paramref name="craftName"/>, then its count is increased by <paramref name="amount"/>, otherwise it's added and set to <paramref name="amount"/>.</remarks>
        /// <returns>The number of craft now assigned to the resulting key.</returns>
        public static int AddFGCounter(int craftType, int IFF, string craftName, int amount, Dictionary<int, Dictionary<string, int>> counter)
        {
            int key = craftType + (IFF << 16);   //Combine IFF into the key, so that same names of different IFFs are not counted together.
            if (!counter.ContainsKey(key))
                counter.Add(key, new Dictionary<string, int>());

            if (counter[key].ContainsKey(craftName))
                counter[key][craftName] += amount;
            else
                counter[key][craftName] = amount;   //Adds to dictionary and assigns.

            return counter[key][craftName];
        }

		/// <summary>Gets the count of a keyed Flight Group</summary>
		/// <param name="craftType">The FG's <see cref="Platform.BaseFlightGroup.CraftType"/> index</param>
		/// <param name="IFF">The FG's <see cref="Platform.BaseFlightGroup.IFF"/> index</param>
		/// <param name="craftName">The FG's <see cref="Platform.BaseFlightGroup.Name"/></param>
		/// <param name="counter">The keyed Flight Group listing</param>
		/// <remarks><paramref name="counter"/> is formatted as &lt;<i>key</i>, &lt;<i><paramref name="craftName"/></i>, <i>count</i>&gt;&gt;.<br/>
		/// Keys are defined as <b>(<paramref name="craftType"/> + (<paramref name="IFF"/> &lt;&lt; 16))</b>.</remarks>
		/// <returns>The number of craft assigned to this key, otherwise <b>zero</b>.</returns>
		public static int GetFGCounter(int craftType, int IFF, string craftName, Dictionary<int, Dictionary<string, int>> counter)
        {
            int key = craftType + (IFF << 16);
            if (!counter.ContainsKey(key)) return 0;
            if (!counter[key].ContainsKey(craftName)) return 0;
            return counter[key][craftName];
        }

        /// <summary>Determines if an array object exists and has a valid index.</summary>
		/// <param name="array">The array to check</param>
		/// <param name="index">The index to search for</param>
		/// <returns><b>true</b> is <paramref name="array"/> is not <b>null</b> and <paramref name="index"/> is within bounds.</returns>
        public static bool IsValidArray<T>(IList<T> array, int index)
        {
            if (array == null) return false;
            return (index >= 0 && index < array.Count);
        }

        /// <summary>Returns a formatted string of a time (m:ss)</summary>
        /// <param name="seconds">Total number of seconds in the time.</param>
        /// <param name="verbose">Whethe or not to append " second(s)".</param>
		/// <returns>Formatted time</returns>
        public static string GetFormattedTime(int seconds, bool verbose)
        {
            return string.Format("{0}:{1:00}", seconds / 60, seconds % 60) + (verbose ? " second" + (seconds == 1 ? "" : "s") : "");
        }

        /// <summary>Safe alternative to <b>cbo.SelectedIndex = n</b> which can automatically handle cases of invalid indexes.</summary>
        /// <param name="cbo">ComboBox to operate on.</param>
        /// <param name="index">Requested SelectedIndex value.</param>
        /// <param name="autoExpand">When <paramref name="index"/> is invalid, expand the list with numbered slots.</param>
		/// <remarks>If <paramref name="index"/> is negative or exceeds the item count, <b>-1</b> is used.</remarks>
        public static void SafeSetCBO(ComboBox cbo, int index, bool autoExpand)
        {
            if (index < -1)
                index = -1;
            if (index >= cbo.Items.Count)
            {
                if (autoExpand == true)
                {
                    for (int i = cbo.Items.Count; i < index + 1; i++)
                        cbo.Items.Add(i.ToString());
                }
                else
                {
                    //index = cbo.Items.Count - 1;
                    index = -1;
                }
            }
            cbo.SelectedIndex = index;
        }    
    }
}
