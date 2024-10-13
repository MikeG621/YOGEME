/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.16
 *
 * CHANGELOG
 * v1.16, 241013
 * [UPD] use actual types for comparison instead of strings
 * [UPD] Title() renamed to MarkDirty(), "loading" made optional
 * v1.15.5, 231222
 * [UPD] Removed RunConverter due to being obsolete
 * v1.13.1, 220208
 * [NEW] Cut and Paste handling [JB]
 * [NEW] KeyDown handling for multi-line TextBoxes [JB]
 * v1.13, 220130
 * [UPD] more multi-edit functions pulled in [JB]
 * v1.12, 220103
 * [NEW] MultiEdit [JB]
 * v1.8.1, 201213
 * [FIX] Verify falls back to default if invalid, then saves default if present
 * [UPD] Verify now takes in entire Settings instead of just the path
 * v1.8, 201004
 * [FIX] Converter call didn't have new path
 * [NEW] Added BoP Converter support
 * [FIX] added the missing backslash in help path
 * [UPD] Replaced IDMR with Github site
 * v1.7, 200816
 * [NEW] SafeString(), ParseAfterInt() [JB]
 * v1.6.5, 200704
 * [UPD] More details to ProcessCraftList error message
 * [FIX #32] help path now explicitly uses Startup Path to prevent implicit from defaulting to sys32
 * v1.6.2, 190928
 * [UPD #28] added Exists check to Verify so the exception is clearer
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
		/// <summary>Launch a browser at the EmpireReborn site.</summary>
		public static void LaunchER() => Process.Start("http://empirereborn.net");

		/// <summary>Launch a browser at the YOGEME site.</summary>
		public static void LaunchGithub() => Process.Start("https://github.com/MikeG621/YOGEME/releases");

		/// <summary>Launch the help file.</summary>
		public static void LaunchHelp() => Process.Start(Application.StartupPath + "\\yogeme.chm");

		/// <summary>Run MissionVerify.exe on open mission</summary>
		/// <remarks>MissionVerify part is take from <see cref="Settings.VerifyLocation"/></remarks>
		/// <param name="path">Path to mission file</param>
		public static void RunVerify(string missionPath)
		{
			var config = Settings.GetInstance();
			string verifyPath = config.VerifyLocation;
			try
			{
				if (!File.Exists(verifyPath))
				{
					string path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "MissionVerify.exe");
					if (!File.Exists(path)) throw new ArgumentException("Verify executable path is not valid.");
					else
					{
						config.VerifyLocation = path;
						config.SaveSettings();
						verifyPath = path;
					}
				}
				Process MV = new Process();
				MV.StartInfo.FileName = verifyPath;
				MV.StartInfo.Arguments = "\"" + missionPath + "\"";
				MV.Start();
			}
			catch (Exception x) { MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}
		
		/// <summary>Adds an asterisk to the window title</summary>
		/// <param name="form">The relevant window.</param>
		/// <param name="loading">Must be <b>false</b> to have an effect.</param>
		public static void MarkDirty(Form form, bool loading = false) { if (form.Text.IndexOf("*") == -1 && !loading) form.Text += "*"; }

		/// <summary>Adds an asterisk to the window title if change detected</summary>
		/// <typeparam name="T">Type of value</typeparam>
		/// <param name="form">The relevant window</param>
		/// <param name="oldValue">Original value to compare against</param>
		/// <param name="newValue">New value from the UI</param>
		/// <returns><paramref name="newValue"/></returns>
        public static T Update<T>(Form form, T oldValue, T newValue)
		{
			if (oldValue.ToString() != newValue.ToString()) MarkDirty(form);
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
		/// <param name="verbose">Whether or not to append " second(s)".</param>
		/// <returns>Formatted time</returns>
		public static string GetFormattedTime(int seconds, bool verbose) => string.Format("{0}:{1:00}", seconds / 60, seconds % 60) + (verbose ? " second" + (seconds == 1 ? "" : "s") : "");

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

		/// <summary>Parses an integer value that immediately follows the first instance of a substring.</summary>
		/// <remarks>Used for extracting embedded information within a trigger or order sentence, for example <b>100% of FG:5</b></remarks>
		/// <param name="text">String to search.</param>
		/// <param name="substr">Substring to search for, including any separator that is expected to appear before the first integer character.</param>
		/// <returns>The parsed value, or zero if not found.</returns>
 		public static int ParseIntAfter(string text, string substr)
		{
			int index = text.IndexOf(substr);
			if (index < 0) return 0;
			index += substr.Length;
			int length = text.IndexOfAny(new char[] { ' ', ',', '\0' }, index) - index;
			int value;
			if (length > 0) value = int.Parse(text.Substring(index, length));
			else value = int.Parse(text.Substring(index));
			return value;
		}

		/// <summary>Safely returns a string from a string array.</summary>
		/// <remarks>Performs array bounds checking to return an item in the array.</remarks>
		/// <param name="stringArray">The array to pull from.</param>
		/// <param name="index">The selected index.</param>
		/// <param name="echo">Whether or not to return <paramref name="index"/> on error.</param>
		/// <returns>The indicated string in the array. If <paramref name="index"/> is invalid, will return an empty string unless <paramref name="echo"/> is <b>true</b>, then returns <paramref name="index"/>.</returns>
		public static string SafeString(string[] stringArray, int index, bool echo) => (stringArray != null && index >= 0 && index < stringArray.Length) ? stringArray[index] : echo ? index.ToString() : "";

		/// <summary>Independently enables or disables a pair of form Buttons depending on the state of a MultiSelect ListBox.</summary>
		/// <param name="up">The appropriate "Move Up" button.</param>
		/// <param name="down">The appropriate "Move Down" button.</param>
		/// <param name="list">The list used to determined if <paramref name="up"/> and <paramref name="down"/> are enabled.</param>
		public static void UpdateMoveButtons(Button up, Button down, ListBox list)
		{
			up.Enabled = (list.SelectedIndices.Count > 0 && list.SelectedIndices[0] > 0);
			down.Enabled = (list.SelectedIndices.Count > 0 && list.SelectedIndices[list.SelectedIndices.Count - 1] < list.Items.Count - 1);
		}

		/// <summary>Retrieves a copy of the selected indices from a ListBox control.</summary>
		/// <remarks>Some operations that modify ListBox items will interfere with the original selection before the operation has completed.</remarks>
		/// <param name="listBox">The approriate ListBox.</param>
		/// <returns>The copied list of indices.</returns>
		public static List<int> GetSelectedIndices(ListBox listBox)
		{
			List<int> ret = new List<int>();
			foreach (int fgIndex in listBox.SelectedIndices)
				ret.Add(fgIndex);
			return ret;
		}

		/// <summary>Assigns or restores a previously retrieved copy of selected indices from a ListBox control</summary>
		/// <param name="listBox">The approriate ListBox.</param>
		/// <param name="selection">The list of indices.</param>
		/// <param name="refresh">A platform-dependent flag to allow ignoring each SelectionIndexChanged event that is raised from selecting each item.</param>
		public static void SetSelectedIndices(ListBox listBox, List<int> selection, ref bool refresh)
		{
			bool btemp = refresh;
			refresh = true;
			listBox.ClearSelected();
			foreach (int itemIndex in selection)
				listBox.SetSelected(itemIndex, true);
			refresh = btemp;
		}

		/// <summary>Adds an event handler function to be processed by common input controls when their value changes.</summary>
		/// <param name="control">The control to assign the handler to.</param>
		/// <param name="handler">The handler event to assign.</param>
		public static void AddControlChangedHandler(Control control, EventHandler handler)
		{
			var type = control.GetType();
			if (type == typeof(TextBox)) ((TextBox)control).TextChanged += handler;
			else if (type == typeof(NumericUpDown)) ((NumericUpDown)control).ValueChanged += handler;
			else if (type == typeof(CheckBox)) ((CheckBox)control).CheckedChanged += handler;
			else if (type == typeof(RadioButton)) ((RadioButton)control).CheckedChanged += handler;
			else if (type == typeof(ComboBox))
			{
				ComboBox cbo = (ComboBox)control;
				if (cbo.DropDownStyle == ComboBoxStyle.DropDownList)
					cbo.SelectedIndexChanged += handler;
				else
					cbo.TextChanged += handler;
			}
			else throw new ArgumentException("Cannot register changed handler to unacceptable control type: " + type.ToString());
		}

		/// <summary>Retrieves the current value of common editable form control as a generic object.</summary>
		/// <param name="sender">A form control.</param>
		/// <returns>The value of <paramref name="sender"/> as a control-appropriate type. <br/>
		/// TextBox returns string. CheckBox and RadioButton return bool. NumericUpDown and ComboBox return int. Text editable ComboBox returns string.</returns>
		public static object GetControlValue(object sender)
		{
			object value = 0;
			var type = sender.GetType();
			if (type == typeof(TextBox)) value = ((TextBox)sender).Text;
			else if (type == typeof(NumericUpDown)) value = (int)((NumericUpDown)sender).Value;
			else if (type == typeof(CheckBox)) value = ((CheckBox)sender).Checked;
			else if (type == typeof(RadioButton)) value = ((RadioButton)sender).Checked;
			else if (type == typeof(ComboBox))
			{
				ComboBox cbo = (ComboBox)sender;
				if (cbo.DropDownStyle == ComboBoxStyle.DropDownList)
					value = cbo.SelectedIndex;  // The only style where the text isn't editable.
				else
					value = cbo.Text;
			}
			return value;
		}

		/// <summary>Attempts to handle a Paste operation for common editable form controls.</summary>
		/// <param name="active">Must be the calling form's ActiveControl.</param>
		/// <param name="data">Data to assign to the control.</param>
		/// <returns>Returns true if the operation was successfully handled.</returns>
		public static bool Paste(Control active, object data)
		{
			if (active.GetType() == typeof(TextBox))
			{
				try
				{
					string s = data.ToString();
					if (s.IndexOf("System.", 0) != -1) throw new FormatException();   // bypass byte[]
					if (s.IndexOf("Idmr.", 0) != -1) throw new FormatException(); // Prevent the class name for custom data types
					TextBox t = (TextBox)active;
					t.SelectedText = s;
					return true;
				}
				catch { }
			}
			else if (active.GetType() == typeof(NumericUpDown))
			{
				try
				{
					string str = data.ToString();
					NumericUpDown num = (NumericUpDown)active;
					decimal value = Convert.ToDecimal(str);
					if (value > num.Maximum) value = num.Maximum;
					else if (value < num.Minimum) value = num.Minimum;
					num.Value = value;
					return true;
				}
				catch { }
			}
			return false;
		}

		/// <summary>Attempts to handle a Cut operation for common editable form controls.</summary>
		/// <remarks>This could be avoided if there wasn't a MenuItem handler to trap CtrlX.</remarks>
		/// <param name="active">Must be the calling form's ActiveControl.</param>
		/// <returns>Returns true if the operation was successfully handled.</returns>
		public static bool Cut(Control active)
		{
			if (active.GetType() == typeof(TextBox))
			{
				((TextBox)active).Cut();
				return true;
			}
			else if (active.GetType() == typeof(NumericUpDown))
			{
				NumericUpDown num = (NumericUpDown)active;
				// Search for the TextBox component within the control.
				foreach (Control child in active.Controls)
				{
					try
					{
						TextBox tb = (TextBox)child;
						tb.Cut();
						// Cutting text from this type of control can cause empty strings which are desynchronized from the actual selected numeric value.
						// This still occurs even if cutting via the default right-click context menu. But since we're cutting via keyboard shortcut, we can fix it.
						string text = tb.Text;
						decimal v = num.Minimum;
						decimal.TryParse(text, out v);
						if (v < num.Minimum) v = num.Minimum;
						else if (v > num.Maximum) v = num.Maximum;
						num.Value = v;
						tb.Text = v.ToString();
						return true;
					}
					catch (InvalidCastException) { }
				}
			}
			else if (active.GetType() == typeof(DataGridTextBox))
			{
				((DataGridTextBox)active).Cut();
				return true;
			}
			return false;
		}

		/// <summary>Attempts to handle a KeyDown event for common editable form controls.</summary>
		/// <param name="active">Must be the calling form's ActiveControl.</param>
		/// <param name="e">Must be the calling form's KeyEventArgs as passed to the KeyDown event.</param>
		/// <returns>Returns true if the operation was successfully handled. <paramref name="e"/> will not be modified even if handled.</returns>
		public static bool KeyDown(Control active, KeyEventArgs e)
		{
			if (active.GetType() == typeof(TextBox))
			{
				TextBox txt = (TextBox)active;
				if (e.KeyCode == Keys.Enter)
				{
					// Multiline textboxes need to allow newlines.
					if (txt.Multiline == true)
						return false;

					// Calling Focus() on a TextBox control might cause it to select all text, so preserve the caret position.
					int caret = txt.SelectionStart;
					active.Parent.Focus(); // Triggers a Leave event.
					active.Focus();
					txt.SelectionStart = caret;
					txt.SelectionLength = 0;
					return true;
				}
				else if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
				{
					txt.SelectAll();
					return true;
				}
			}
			return false;
		}
	}

	/// <summary>Allows multi-edit properties to perform generic platform-dependent refresh operations.</summary>
	public enum MultiEditRefreshType
	{
		/// <summary>None.</summary>
		None = 0,
		/// <summary>A generic text change for an item in a multiselect ListBox (includes FG info like GG and GU, but also used for the message list)</summary>
		ItemText = 1,
		/// <summary>Flightgroup name change that affects dropdown boxes (anything that affects the name, including recalculated craft numbering)</summary>
		CraftName = 2,
		/// <summary>Flightgroup craft/object totals need to be adjusted</summary>
		CraftCount = 4,
		/// <summary>FG goal trigger label</summary>
		FgGoalLabel = 8,
		/// <summary>Order trigger label</summary>
		OrderLabel = 16,
		/// <summary>Arrival/departure trigger label</summary>
		ArrDepLabel = 32,
		/// <summary>Skip trigger label</summary>
		SkipLabel = 64,
		/// <summary>Optional craft label</summary>
		OptCraftLabel = 128,
		/// <summary> The map should be updated</summary>
		Map = 256
	};

	/// <summary>To be assigned as a custom Tag on form controls registered as a multi-edit property.</summary>
	public class MultiEditProperty
	{
		public string Name;
		public MultiEditRefreshType RefreshType;

		/// <summary>Initializes a new instance with the given values.</summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="refreshType">The type(s) of refresh operations approriate for the property.</param>
		public MultiEditProperty(string propertyName, MultiEditRefreshType refreshType)
		{
			Name = propertyName;
			RefreshType = refreshType;
		}
	}
}
