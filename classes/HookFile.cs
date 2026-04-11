/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2026 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.17.7+
 *
 * CHANGELOG
 * - Release
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Idmr.Yogeme
{
	/// <summary>Represents the hook configuration for a mission.</summary>
	internal class HookFile
	{
		static string _iniPath = "";

		/// <summary>Initialize the configuration.</summary>
		/// <param name="path">The path to the misison INI.</param>
		/// <remarks>Does not load the file yet.</remarks>
		public HookFile(string path) => _iniPath = path;

		/// <summary>Adds a comment to the base level of the file.</summary>
		/// <param name="comment">The text to add.</param>
		/// <remarks>Comments start with ";", "//", or "#".<br/>
		/// If <paramref name="comment"/> doesn't contain one of those prefixes, ";" will be used.<br/>
		/// If <paramref name="comment"/> is empty, it will be ignored. To add blank lines, it must already include a comment prefix.</remarks>
		public void AddComment(string comment)
		{
			if (comment != null) comment = comment.Trim();
			if (string.IsNullOrEmpty(comment)) return;

			if (!IsComment(comment)) comment = ";" + comment;
			Comments.Add(comment);
		}

		/// <summary>Gets a concatenated string of <see cref="Comments"/>.</summary>
		/// <returns>A joined string using line breaks.</returns>
		public string GetComments() => string.Join("\r\n", Comments);
		/// <summary>Gets the full contents of the file.</summary>
		/// <returns>A string as it will be written to file.</returns>
		/// <remarks>The <see cref="Comments"/> will be first, followed by each <see cref="HookSection"/> as ordered in <see cref="Sections"/>.<br/>
		/// Note that comments such as a Changelog added to the end of the file will be viewed as belonging to the last Section, not the file.</remarks>
		public string GetContents()
		{
			string contents = "";
			if (Comments.Count > 0) contents = GetComments() + "\r\n\r\n";	// unless the user manually cleared, this should always be true
			foreach (var section in Sections) contents += section.GetContents();
			return contents;
		}

		/// <summary>Gets if the supplied text is a comment.</summary>
		/// <param name="line">The line of text.</param>
		/// <returns>Returns <b>true</b> if <paramref name="line"/> starts with ";", "//", or "#".</returns>
		public static bool IsComment(string line) => line.StartsWith(";") || line.StartsWith("//") || line.StartsWith("#");

		/// <summary>Add the contents of a text file to a <see cref="HookSection"/>.</summary>
		/// <param name="path">Full path to a text file.</param>
		/// <param name="sectionName">Name of the section to add to.</param>
		/// <remarks>If the section does not exist, it will be created.<br/>
		/// If the contents of the file appear to include the start of a heading, it will read up to that point.</remarks>
		public void MergeTextFile(string path, string sectionName)
		{
			var section = GetOrCreateSection(sectionName);
			string line;
			using (var sr = new StreamReader(path))
				while ((line = sr.ReadLine()) != null)
				{
					if (line.StartsWith("[")) break;    // shouldn't happen, since this is for TXT files

					section.AddLine(line);
				}
		}

		/// <summary>Loads the contents of the INI file.</summary>
		/// <exception cref="InvalidDataException"><see cref="IniPath"/> has not been set.</exception>
		/// <exception cref="FileNotFoundException"><see cref="IniPath"/> not found.</exception>
		public void Read()
		{
			if (_iniPath == null) throw new InvalidDataException("Ini path has not been set.");
			if (!File.Exists(_iniPath)) throw new FileNotFoundException($"Ini file ({_iniPath}) not found.");

			Sections.Clear();
			Comments.Clear();
			using (var sr = new StreamReader(_iniPath))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					line = line.Trim();
					if (string.IsNullOrEmpty(line)) continue;

					if (!line.StartsWith("[")) { AddComment(line); continue; }	// in addition to real comments, will capture anything before the first section

					// at a section header
					string sectionName = line.Substring(1, line.IndexOf(']') - 1);
					if (!SectionExists(sectionName)) Sections.Add(new HookSection(sectionName, sr));
					else GetSectionByName(sectionName).MergeSection(new HookSection(sectionName, sr));
				}
			}
		}

		/// <summary>Saves the contents of the INI file.</summary>
		/// <returns>Returns <b>true</b> if successful.</returns>
		/// <exception cref="InvalidOperationException"><see cref="IniPath"/> has not been set.</exception>
		public bool Write()
		{
			if (string.IsNullOrEmpty(_iniPath)) throw new InvalidOperationException("INI path has not been set.");

			string backup = Path.ChangeExtension(_iniPath, "inibak");
			if (File.Exists(_iniPath))
			{
				File.Copy(_iniPath, backup);
				File.Delete(_iniPath);
			}
			StreamWriter sw = null;
			try
			{
				sw = new StreamWriter(_iniPath);
				sw.Write(GetContents());
				sw.Flush();
				sw.Close();
			}
			catch
			{
				sw?.Close();
				if (File.Exists(backup))
				{
					File.Delete(_iniPath);
					File.Copy(backup, _iniPath);
					File.Delete(backup);
				}
				return false;
			}
			File.Delete(backup);
			return true;
		}

		/// <summary>Gets the indicated <see cref="HookSection"/>, or creates a new one if it does not exist.</summary>
		/// <param name="name">The name of the section, case insensitive.</param>
		/// <returns>The indicated section.</returns>
		public HookSection GetOrCreateSection(string name)
		{
			var section = GetSectionByName(name);
			if (section == null)
			{
				section = new HookSection(name);
				Sections.Add(section);
			}
			return section;
		}
		/// <summary>Gets the inidicated <see cref="HookSection"/>.</summary>
		/// <param name="name">The name of the section, case insensitive.</param>
		/// <returns>The indicated section, otherwise <b>null</b>.</returns>
		public HookSection GetSectionByName(string name)
		{
			foreach (var section in Sections)
				if (section.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) return section;
			return null;
		}
		/// <summary>Checks if the <see cref="HookSection"/> exists.</summary>
		/// <param name="name">The name of the section, case insensitive.</param>
		/// <returns>Returns <b>true</b> if the section exists, otherwise <b>false</b>.</returns>
		public bool SectionExists(string name)
		{
			foreach (var section in Sections)
				if (section.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) return true;
			return false;
		}

		/// <summary>Gets the collection of sections in the file.</summary>
		public List<HookSection> Sections { get; private set; } = new List<HookSection>();
		/// <summary>Gets the collection of comments in the file header.</summary>
		public List<string> Comments { get; private set; } = new List<string>();
		/// <summary>Gets the as-stored path to the INI file.</summary>
		public string IniPath => _iniPath;

		/// <summary>Represents an individual section within the file.</summary>
		internal class HookSection
		{
			/// <summary>Initializes an empty section.</summary>
			/// <param name="name">The name of the section.</param>
			/// <remarks>The <see cref="Name"/> will be saved in the case provided.</remarks>
			public HookSection(string name) => Name = name;
			/// <summary>Initializes and loads the section from a stream.</summary>
			/// <param name="name">The name of the section.</param>
			/// <param name="sr">The opened stream to the INI file.</param>
			/// <remarks>The <see cref="Name"/> will be saved in the case provided.</remarks>
			public HookSection(string name, StreamReader sr) : this(name)
			{
				while (!sr.EndOfStream && sr.Peek() != '[')
				{
					string line = sr.ReadLine().Trim();
					if (string.IsNullOrEmpty(line)) continue;
					AddLine(line);
				}
			}

			/// <summary>Adds a comment to the section.</summary>
			/// <param name="comment">The text to add.</param>
			/// <remarks>Comments start with ";", "//", or "#".<br/>
			/// If <paramref name="comment"/> doesn't contain one of those prefixes, ";" will be used.<br/>
			/// If <paramref name="comment"/> is empty, it will be ignored. To add blank lines, it must already include a comment prefix.</remarks>
			public void AddComment(string comment)
			{
				if (comment != null) comment = comment.Trim();
				if (string.IsNullOrEmpty(comment)) return;

				if (!IsComment(comment)) comment = ";" + comment;
				Comments.Add(comment);
			}
			/// <summary>Adds a line to the section.</summary>
			/// <param name="line">The text to add.</param>
			/// <returns>Returns <b>true</b> if <paramref name="line"/> is added.</returns>
			/// <remarks>If <paramref name="line"/> is empty, no changes made.<br/>
			/// If <paramref name="line"/> is a comment, adds to <see cref="Comments"/>.<br/>
			/// If <paramref name="line"/> is normal text, but includes a comment afterwards, they will be split out between <see cref="Entries"/> and <see cref="Comments"/>.<br/>
			/// Normal text is added to <see cref="Entries"/>.</remarks>
			public bool AddLine(string line)
			{
				if (line != null) line = line.Trim();
				if (string.IsNullOrEmpty(line)) return false;

				if (IsComment(line)) return add(Comments, line);
				else
				{
					if (line.IndexOf(';') != -1)
					{
						AddComment(line.Substring(line.IndexOf(';')));
						line = line.Substring(0, line.IndexOf(";"));
					}
					if (line.IndexOf("//") != -1)
					{
						AddComment(line.Substring(line.IndexOf("//")));
						line = line.Substring(0, line.IndexOf("//"));
					}
					if (line.IndexOf('#') != -1)
					{
						AddComment(line.Substring(line.IndexOf('#')));
						line = line.Substring(0, line.IndexOf("#"));
					}
					return add(Entries, line.Trim());
				}
			}

			bool add (List<string> list, string line)
			{
				bool found = false;
				foreach (var item in list)
					if (string.Equals(item, line, StringComparison.OrdinalIgnoreCase)) { found = true; break; }
				if (!found) list.Add(line);
				return !found;
			}

			/// <summary>Gets the text of the entire section.</summary>
			/// <returns>A string formatted to write to file.</returns>
			/// <remarks>Format is:<br/>
			/// <b>"[<see cref="Name"/>]<br/><see cref="GetEntries"/><br/><see cref="GetComments"/><br/>(spacing line break)"</b><br/><br/>
			/// Note that the comments in sections are after the contents, whereas the file comments come first.</remarks>
			public string GetContents()
			{
				if (Entries.Count == 0 && Comments.Count == 0) return "";

				string contents = ToString() + "\r\n";
				if (Entries.Count > 0) contents += GetEntries() + "\r\n";
				if (Comments.Count > 0) contents += GetComments() + "\r\n";
				contents += "\r\n";
				return contents;
			}
			/// <summary>Gets a concatenated string of <see cref="Comments"/>.</summary>
			/// <returns>A joined string using line breaks.</returns>
			public string GetComments() => string.Join("\r\n", Comments);
			/// <summary>Gets a concatenated string of <see cref="Entries"/>.</summary>
			/// <returns>A joined string using line breaks.</returns>
			public string GetEntries() => string.Join("\r\n", Entries);

			/// <summary>Adds the contents of another section.</summary>
			/// <param name="section">The section to add from.</param>
			/// <remarks>Appends both the <see cref="Entries"/> and <see cref="Comments"/> of <paramref name="section"/>.</remarks>
			public void MergeSection(HookSection section)
			{
				foreach (var item in section.Entries) add(Entries, item);
				foreach (var item in section.Comments) add(Comments, item);
			}
			
			/// <summary>Gets the formatted section name.</summary>
			/// <returns>A string in the form <b>"[<see cref="Name"/>]"</b>.</returns>
			public override string ToString() => $"[{Name}]";

			/// <summary>Gets the section name.</summary>
			public string Name { get; private set; } = "";
			/// <summary>Gets the collection of settings in the section.</summary>
			public List<string> Entries { get; private set; } = new List<string>();
			/// <summary>Gets the collection of comments in the section.</summary>
			public List<string> Comments { get; private set; } = new List<string>();
		}
	}
}
