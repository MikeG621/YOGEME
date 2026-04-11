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
	internal class HookFile
	{
		static string _iniPath = "";

		public HookFile(string path) => _iniPath = path;

		public void AddComment(string comment)
		{
			if (comment != null) comment = comment.Trim();
			if (string.IsNullOrEmpty(comment)) return;

			if (!IsComment(comment)) comment = ";" + comment;
			Comments.Add(comment);
		}

		public string GetComments() => string.Join("\r\n", Comments);
		public string GetContents()
		{
			string contents = "";
			if (Comments.Count > 0) contents = GetComments() + "\r\n\r\n";	// unless the user manually cleared, this should always be true
			foreach (var section in Sections) contents += section.GetContents();
			return contents;
		}

		public static bool IsComment(string line) => line.StartsWith(";") || line.StartsWith("//") || line.StartsWith("#");

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
		public HookSection GetSectionByName(string name)
		{
			foreach (var section in Sections)
				if (section.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) return section;
			return null;
		}
		public bool SectionExists(string name)
		{
			foreach (var section in Sections)
				if (section.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) return true;
			return false;
		}

		public List<HookSection> Sections { get; private set; } = new List<HookSection>();
		public List<string> Comments { get; private set; } = new List<string>();

		internal class HookSection
		{
			public HookSection(string name) => Name = name;
			public HookSection(string name, StreamReader sr) : this(name)
			{
				while (!sr.EndOfStream && sr.Peek() != '[')
				{
					string line = sr.ReadLine().Trim();
					if (string.IsNullOrEmpty(line)) continue;
					AddLine(line);
				}
			}

			public void AddComment(string comment)
			{
				if (comment != null) comment = comment.Trim();
				if (string.IsNullOrEmpty(comment)) return;

				if (!IsComment(comment)) comment = ";" + comment;
				Comments.Add(comment);
			}
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

			public string GetContents()
			{
				if (Entries.Count == 0 && Comments.Count == 0) return "";

				string contents = ToString() + "\r\n";
				if (Entries.Count > 0) contents += GetEntries() + "\r\n";
				if (Comments.Count > 0) contents += GetComments() + "\r\n";
				contents += "\r\n";
				return contents;
			}
			public string GetComments() => string.Join("\r\n", Comments);
			public string GetEntries() => string.Join("\r\n", Entries);

			public void MergeSection(HookSection section)
			{
				foreach (var item in section.Entries) add(Entries, item);
				foreach (var item in section.Comments) add(Comments, item);
			}
			/*
			public void ReadFromTxt(string path)
			{
				string line;
				using (var sr = new StreamReader(path))
					while ((line = sr.ReadLine()) != null)
					{
						if (line.StartsWith("[")) break;	// shouldn't happen, since this is for TXT files

						AddLine(line);
					}
			}

			public void ReadFromIni()
			{
				if (_iniPath == "") throw new InvalidOperationException("INI file has not been defined.");
				if (Name == "") throw new InvalidOperationException("Section name has not been defined.");

				string line;
				bool isReading = false;
				using (var sr = new StreamReader(_iniPath))
					while ((line = sr.ReadLine()) != null)
					{
						line = line.Trim();
						if ((!isReading && !line.StartsWith("[")) || string.IsNullOrEmpty(line)) continue;
						if (isReading && line.StartsWith("[")) break;

						if (!isReading && line.StartsWith("[") && line.IndexOf(']') != -1)
						{
							line = line.Substring(1, line.LastIndexOf(']') - 1);
							if (string.Equals(line, Name, StringComparison.OrdinalIgnoreCase)) isReading = true;
							continue;
						}
						AddLine(line);
					}
			}
			*/
			public override string ToString() => $"[{Name}]";

			public string Name { get; private set; } = "";
			public List<string> Entries { get; private set; } = new List<string>();
			public List<string> Comments { get; private set; } = new List<string>();
		}
	}
}
