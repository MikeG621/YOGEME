/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * This file authored by "JB" (Random Starfighter) (randomstarfighter@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.8+
 */

/* CHANGELOG
 * [UPD] cleanup
 * v1.8, 201004
 * [NEW] GetCraftSpeed [JB]
 * [UPD] Added a blank ctor for fixed null loads
 * [UPD] XML stuff
 * [FIX] _finalizedCraftData wasn't set on first run if _detectedInstallPath was blank, causing exceptions on platform load [#37]
 * v1.7, 200816
 * [NEW] created [JB]
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/* [JB] The manager class here is responsible for loading and storing all craft data and lists of names, as
 * well as detecting installation paths to load any needed files. It operates as singleton, allowing it to be
 * accessed from anywhere with minimal programming effort.
 * 
 * The craft data contains craft names, as well as other important information required for map wireframes,
 * notably the resource names to load the models. It is designed to allow detection based off mission or game
 * installation paths, allowing it to seamlessly access resources from different installations without
 * intervention by the user.
 * 
 * The list of craft names is created from a series of optional overrides. The base strings are provided as
 * a copy of the default strings in Idmr.Platform. Expanding this, each platform has a config option which
 * allows the user to choose whether to load new names from the external craft data files. Normally this is
 * where the resource names are acquired, and the craft names merely ignored. But they can be renamed and
 * applied as the user chooses.
 * 
 * XWING is a special case, as it needs to maintain two separate lists. The names in the editor are virtualized
 * and don't directly correspond to the actual CraftType enum. The map needs its own list, as it converts the
 * craft into TIE format, in order to reuse icons and editing logic. The list used by the map needs to
 * correspond to the TIE CraftType enum.
 * 
 * XWA is also a special case, as there's yet another optional override which directly scans the EXE and
 * strings file. This allows detection of the craft list from any modded craft pack, and having them instantly
 * available in the editor's craft list, plus wireframes in the map. The user can export a craft list into the
 * XWA installation folder to edit them on a per-installation basis.
 */

namespace Idmr.Yogeme
{
	/// <summary>A singleton class that detects and resolves platform installation paths to retrieve craft data, as well as loading craft data from predefined external files.</summary>
	public class CraftDataManager
	{
		static readonly CraftDataManager _instance = new CraftDataManager();

		/// <summary>Will be true if custom craft data was exported into XWA's game folder and is being used from that location instead of YOGEME's operating path.</summary>
		public bool XwaInstallSpecificExternalDataLoaded { get; set; } = false;

		string _currentInstallPath = "";                              // The current installation path as provided from Settings.
		string _currentMissionPath = "";                              // The full path+filename of the current mission.
		Settings.Platform _currentPlatform = Settings.Platform.None;  // Used to detect if the platform has changed since the last call to update.

		// These booleans track the previous state to determine if the user's configuration has changed, and if data should be refreshed the next time a mission is loaded, even if from the same platform.
		bool _previousOverrideExternal = false;
		bool _previousOverrideScan = false;
		bool _previousFlagRemappedCraft = false;

		string _detectedInstallPath = "";                             // The path of the game platform installation, as resolved by detection from the mission path or the user's chosen install path.
		string _detectedModelPath = "";                               // The path of the game's craft models, as resolved by detection from the mission path or the user's chosen install path.

		List<CraftData> _defaultCraftData = null;                     // This only contains strings provided by Idmr.Platform.
		List<CraftData> _editorCraftData = null;                      // This is strictly for use in the editor craft type list, separate from the map. Needed because X-wing is a special case.
		List<CraftData> _externalCraftData = null;                    // Contains the craft data loaded from an external file.
		List<CraftData> _scannedCraftData = null;                     // Contains a list scanned directly from the game executable and data files.
		List<CraftData> _finalizedCraftData = null;                   // The finalized craft data (after all loading and overrides have been applied) which is accessible to the rest of the program.

		private CraftDataManager() { }

		/// <summary>Retrieves public access to the singleton.</summary>
		public static CraftDataManager GetInstance() => _instance;

		/// <summary>Retrieves the finalized, fully loaded craft list for use in the map.</summary>
		public List<CraftData> GetCraftDataList() => _finalizedCraftData;

		/// <summary>Loads a platform, manages craft name strings, performs loading and merging of craft data, and path detection.</summary>
		public void LoadPlatform(Settings.Platform platform, Settings config, string[] defaultLongNames, string[] defaultShortNames, string missionPath)
		{
			// These vars will be assigned from the user's config settings.
			_currentInstallPath = "";
			bool detectMission = false;
			bool overrideExternal = false;
			bool overrideScan = false;
			bool flagRemappedCraft = false;

			switch (platform)
			{
				case Settings.Platform.XWING:
					_currentInstallPath = config.XwingPath;
					detectMission = config.XwingDetectMission;
					overrideExternal = config.XwingOverrideExternal;
					break;
				case Settings.Platform.TIE:
					_currentInstallPath = config.TiePath;
					detectMission = config.TieDetectMission;
					overrideExternal = config.TieOverrideExternal;
					break;
				case Settings.Platform.XvT:
					_currentInstallPath = config.XvtPath;
					detectMission = config.XvtDetectMission;
					overrideExternal = config.XvtOverrideExternal;
					break;
				case Settings.Platform.BoP:
					_currentInstallPath = config.BopPath;
					detectMission = config.XvtDetectMission;
					overrideExternal = config.XvtOverrideExternal;
					break;
				case Settings.Platform.XWA:
					_currentInstallPath = config.XwaPath;
					detectMission = config.XwaDetectMission;
					overrideExternal = config.XwaOverrideExternal;
					overrideScan = config.XwaOverrideScan;
					flagRemappedCraft = config.XwaFlagRemappedCraft;
					break;
			}
			if (!detectMission) missionPath = "";
			_currentMissionPath = missionPath;

			bool configChanged = (_previousOverrideExternal != overrideExternal || _previousOverrideScan != overrideScan || _previousFlagRemappedCraft != flagRemappedCraft);
			if (_currentPlatform != platform || configChanged)
			{
				_currentPlatform = platform;
				setDefaultStrings(defaultLongNames, defaultShortNames);
				loadPlatformExternalCraftData();
				if (_currentPlatform == Settings.Platform.XWING)
				{
					// If we don't want to override, or it failed to load, use the default string list.
					if (!overrideExternal || _editorCraftData == null || _editorCraftData.Count == 0) _editorCraftData = _defaultCraftData;

					// If we did load the list, and the user modified it, then it's possible the count is incorrect.
					// It's very important for this to be the correct size. Pad the list with the defaults, or trim the excess entries.
					if (_editorCraftData.Count < _defaultCraftData.Count) _editorCraftData = mergeLists(_editorCraftData, _defaultCraftData, false);
					else if (_editorCraftData.Count > _defaultCraftData.Count) _editorCraftData.RemoveRange(_defaultCraftData.Count, _editorCraftData.Count - _defaultCraftData.Count);
				}
			}

			_previousOverrideExternal = overrideExternal;
			_previousOverrideScan = overrideScan;
			_previousFlagRemappedCraft = flagRemappedCraft;

			string previousDetectedInstall = _detectedInstallPath;
			detectInstallationPaths();
			if (previousDetectedInstall != _detectedInstallPath || configChanged || _detectedInstallPath == "")
			{
				if (_detectedInstallPath != "" && _currentPlatform == Settings.Platform.XWA)
				{
					XwaInstallSpecificExternalDataLoaded = false;

					// Try loading a custom list from the installation directory. This allows the user to customize craft lists for different versions of XWA.
					if (overrideExternal && File.Exists(Path.Combine(_detectedInstallPath, "craft_data_xwa.txt")))
					{
						_externalCraftData = loadCraftData(Path.Combine(_detectedInstallPath, "craft_data_xwa.txt"));
						XwaInstallSpecificExternalDataLoaded = true;
					}

					if (overrideScan) _scannedCraftData = scanXwa(flagRemappedCraft);
				}

				_finalizedCraftData = mergeLists(_defaultCraftData, _externalCraftData, overrideExternal);
				if (overrideScan && _scannedCraftData != null && _scannedCraftData.Count > 0) _finalizedCraftData = mergeLists(_finalizedCraftData, _scannedCraftData, true);
			}
		}

		/// <summary>Retrieves a string array of all loaded craft long names. This can be used to override the list in the editor.</summary>
		public string[] GetLongNames()
		{
			List<CraftData> container = (_currentPlatform == Settings.Platform.XWING ? _editorCraftData : _finalizedCraftData);
			string[] result = new string[container.Count];
			for (int i = 0; i < container.Count; i++) result[i] = container[i].Name;
			return result;
		}
		/// <summary>Retrieves a string array of all loaded craft short names. This can be used to override the list in the editor.</summary>
		public string[] GetShortNames()
		{
			List<CraftData> container = (_currentPlatform == Settings.Platform.XWING ? _editorCraftData : _finalizedCraftData);
			string[] result = new string[container.Count];
			for (int i = 0; i < container.Count; i++) result[i] = container[i].Abbrev;
			return result;
		}

		/// <summary>Retrieves the speed (in MGLT) of a craft.</summary>
		public int GetCraftSpeed(int craftType) => craftType < 0 || craftType >= _finalizedCraftData.Count ? 0 : _finalizedCraftData[craftType].SpeedMglt;

		/// <summary>Retrieves the detected install path.</summary>
		public string GetInstallPath() => _detectedInstallPath;

		/// <summary>Retrieves the detected model path, so that wireframes may be loaded.</summary>
		public string GetModelPath() => _detectedModelPath;

		/// <summary>Attempts to save the craft data to the specified file.</summary>
		/// <returns>If failed, returns a string containing an exception error message. Returns an empty string on success.</returns>
		public string SaveToFile(string filename)
		{
			string result = "";
			try
			{
				using (StreamWriter sw = new StreamWriter(filename))
				{
					sw.WriteLine("; Exported YOGEME craft list");
					sw.WriteLine("; Line format is: CraftName,Abbrev,SpeedMGLT,ResourceName");
					foreach (CraftData item in _finalizedCraftData) sw.WriteLine($"{item.Name},{item.Abbrev},{item.SpeedMglt},{item.ResourceNames}");
				}
			}
			catch (Exception x) { result = x.Message; }
			return result;
		}

		/// <summary>Initializes a craft list with the default strings, which should be supplied from Idmr.Platform</summary>
		/// <remarks>Any data that isn't a name will be initialized to empty.</remarks>
		void setDefaultStrings(string[] longNames, string[] shortNames)
		{
			_defaultCraftData = new List<CraftData>();
			for (int i = 0; i < longNames.Length; i++)
			{
				CraftData entry = new CraftData() { Name = longNames[i] };
				if (i < shortNames.Length) entry.Abbrev = shortNames[i];
				_defaultCraftData.Add(entry);
			}
		}

		/// <summary>Loads the craft data from an external text file. Automatically selects which source depending on current platform.</summary>
		void loadPlatformExternalCraftData()
		{
			string filename = "";
			switch (_currentPlatform)
			{
				case Settings.Platform.XWING: filename = "craft_data_xw.txt"; break;
				case Settings.Platform.TIE: filename = "craft_data_tie.txt"; break;
				case Settings.Platform.XvT: case Settings.Platform.BoP: filename = "craft_data_xvt.txt"; break;
				case Settings.Platform.XWA: filename = "craft_data_xwa.txt"; break;
			}
			_externalCraftData = loadCraftData(filename);

			var container = (_currentPlatform == Settings.Platform.XWING) ? _editorCraftData : _externalCraftData;
			if (container != null && ((_currentPlatform == Settings.Platform.XWA && container.Count < _defaultCraftData.Count) || (_currentPlatform != Settings.Platform.XWA && container.Count != _defaultCraftData.Count)))
				throw new Exception("Problem when loading " + filename.ToUpper() + ", found " + container.Count + " craft entries, expected " + (_currentPlatform == Settings.Platform.XWA ? "at least " : "exactly ") + _defaultCraftData.Count + ".\nYour craft list may be incorrect or misaligned."); // gets caught in main form's loadCraftData()
		}

		/// <summary>Tests if a given subfolder, filename, or subfolder+filename exists at the specified path.</summary>
		/// <returns>If found, returns the input path (plus subfolder if applicable), or an empty string if not found.</returns>
		string testExist(string path, string subfolder, string filename)
		{
			if (string.IsNullOrWhiteSpace(path)) return "";
			if (string.IsNullOrWhiteSpace(subfolder) && !string.IsNullOrWhiteSpace(filename))
			{
				if (File.Exists(Path.Combine(path, filename))) return path;
			}
			else if (!string.IsNullOrWhiteSpace(subfolder) && string.IsNullOrWhiteSpace(filename))
			{
				if (Directory.Exists(Path.Combine(path, subfolder))) return Path.Combine(path, subfolder);
			}
			else if (!string.IsNullOrWhiteSpace(subfolder) && !string.IsNullOrWhiteSpace(filename))
			{
				if (File.Exists(Path.Combine(path, subfolder, filename))) return Path.Combine(path, subfolder);
			}
			return "";
		}

		/// <summary>Performs detection of a subfolder, filename, or subfolder+filename, searching up and down the folder hierarchy from the starting path.</summary>
		/// <returns>Returns the found path (plus subfolder if applicable), or an empty string if not found.</returns>
		string detectExist(string startPath, string subfolder, string filename)
		{
			// If the path contains a filename, strip the filename out.
			int separator = startPath.LastIndexOf(Path.DirectorySeparatorChar);
			if (startPath.LastIndexOf('.') > separator && separator >= 0) startPath = startPath.Remove(separator);

			string result = testExist(startPath, subfolder, filename);
			if (result == "") result = downwardSearch(startPath, subfolder, filename);
			if (result == "") result = upwardSearch(startPath, subfolder, filename);
			return result;
		}

		/// <summary>Performs detection through multiple search criteria to determine if at least one match is discovered.</summary>
		/// <remarks>Search criteria elements are separated by a comma. Each element contains a path and filename, separated by a backslash. To specify only a filename then use a single punctuation character in place of the subfolder.</remarks>
		/// <returns>Returns the found path (plus subfolder if applicable), or an empty string if not found.</returns>
		string detectExistMultiple(string startPath, string searchCriteria)
		{
			if (string.IsNullOrWhiteSpace(startPath) || string.IsNullOrWhiteSpace(searchCriteria)) return "";
			foreach (string term in searchCriteria.Split(','))
			{
				string[] tokens = term.Split('\\');
				string subfolder = tokens.Length >= 1 ? tokens[0] : "";
				if (subfolder.Length == 1 && char.IsPunctuation(subfolder, 0)) subfolder = "";
				string filename = tokens.Length >= 2 ? tokens[1] : "";
				string result = detectExist(startPath, subfolder, filename);
				if (result != "") return result;
			}
			return "";
		}

		/// <summary>Performs a downward search (deeper) into the directory structure.</summary>
		/// <returns>Returns a path to the subfolder if successful. Otherwise an empty string.</returns>
		string downwardSearch(string lookPath, string subfolder, string filename)
		{
			// Check for common problems first. Directory must exist for enumeration to proceed. Don't search off the root.
			if (string.IsNullOrWhiteSpace(lookPath) || lookPath.Contains("..") || !Directory.Exists(lookPath) || lookPath == Path.GetPathRoot(lookPath)) return "";
			string result = testExist(lookPath, subfolder, filename);
			if (result != "") return result;

			// There could be permission errors in certain cases, so fail gracefully if there's a problem while enumerating.
			try
			{
				foreach (string s in Directory.EnumerateDirectories(lookPath))
				{
					result = downwardSearch(Path.Combine(lookPath, s), subfolder, filename);
					if (result != "") return result;
				}
			}
			catch { /* do nothing */ }
			return "";
		}

		/// <summary>Searches upward towards the root of the directory structure, looking for the specified subfolder.</summary>
		/// <returns>Returns a path to the subfolder if successful. Otherwise an empty string.</returns>
		string upwardSearch(string lookPath, string subfolder, string filename)
		{
			string result = testExist(lookPath, subfolder, filename);
			if (result != "") return result;

			while (true)
			{
				int pos = lookPath.LastIndexOf(Path.DirectorySeparatorChar);
				if (pos >= 0) lookPath = lookPath.Remove(pos);
				if (pos < 0 || lookPath == "") break;
				result = testExist(lookPath, subfolder, filename);
				if (result != "") return result;
			}
			return "";
		}

		/// <summary>Tests if a string contains any of the substrings in the search criteria. Comparison is case insensitive.</summary>
		/// <param name="str">String to test.</param>
		/// <param name="searchCriteria">A comma-separated list of one or more substrings to search for.</param>
		bool containsSubstring(string str, string searchCriteria)
		{
			if (string.IsNullOrWhiteSpace(str) || string.IsNullOrWhiteSpace(searchCriteria)) return false;
			foreach (string token in searchCriteria.Split(',')) if (str.IndexOf(token, StringComparison.InvariantCultureIgnoreCase) >= 0) return true;
			return false;
		}

		/// <summary>Performs detection for installation folder and models based on platform.</summary>
		/// <remarks>If the mission path was assigned, begins detection from there. If mission detection is disabled or fails, it will try the assigned installation path instead.</remarks>
		void detectInstallationPaths()
		{
			string autoCriteria = "";
			string installCriteria = "";
			string modelCriteria = "";
			switch (_currentPlatform)
			{
				case Settings.Platform.XWING:
					autoCriteria = "mission,classic";
					installCriteria = ".\\xwing.exe,.\\bwing.exe,.\\xwing95.exe";
					modelCriteria = "ivfiles\\spec320.lst,resource\\species.lfd";  // The Windows version of the game contains both, so prioritize Windows.
					break;
				case Settings.Platform.TIE:
					autoCriteria = "mission";
					installCriteria = ".\\tie.exe,.\\tie95.exe";
					modelCriteria = "ivfiles\\spec320.lst,resource\\species.lfd";  // Prioritize Windows version.
					break;
				case Settings.Platform.XvT:
				case Settings.Platform.BoP:
					autoCriteria = "train,melee,combat";
					installCriteria = ".\\z_xvt__.exe";
					modelCriteria = "ivfiles\\spec320.lst";
					break;
				case Settings.Platform.XWA:
					autoCriteria = "missions,melee,combat";
					installCriteria = ".\\xwingalliance.exe";
					modelCriteria = "flightmodels\\spacecraft0.lst";
					break;
			}
			_detectedInstallPath = "";
			_detectedModelPath = "";
			if (containsSubstring(_currentMissionPath, autoCriteria))
			{
				_detectedInstallPath = detectExistMultiple(_currentMissionPath, installCriteria);
				_detectedModelPath = detectExistMultiple(_currentMissionPath, modelCriteria);
			}
			// If not assigned, detection failed or the mission path was not specified for detection purposes. Check installation path instead.
			if (_detectedInstallPath == "") _detectedInstallPath = detectExistMultiple(_currentInstallPath, installCriteria);
			if (_detectedModelPath == "") _detectedModelPath = detectExistMultiple(_currentInstallPath, modelCriteria);
		}

		/// <summary>Scans an XWA installation, attempting to load the craft data and slot mappings directly from the executable and its data files.</summary>
		/// <param name="flagRemappedCraft">Specifies whether a suffix should be added to the name, indicating the craft slot has remapped (via mods) from the original, which would cause problems if attempting to load the mission in an unmodded game.</param>
		List<CraftData> scanXwa(bool flagRemappedCraft)
		{
			// This is the default vanilla craft mapping that converts a mission FlightGroup craft type into its corresponding object in game.
			// It's added here for an extra feature to detect new craft slots (when modded with an upgrade pack). It is unable to detect craft slots that have been replaced, only if they've been remapped to a different slot.
			int[] defaultMapping = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 50, 51, 55, 56, 52, 53, 54, 43, 44, 71, 72, 73, 74, 45, 46, 91, 92, 93, 94, 95, 57, 58, 59, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 75, 76, 77, 78, 79, 153, 154, 155, 156, 157, 158, 159, 194, 195, 160, 210, 211, 212, 212, 212, 219, 220, 221, 196, 221, 213, 214, 213, 215, 216, 218, 223, 223, 217, 80, 161, 162, 141, 142, 143, 144, 145, 61, 62, 146, 96, 147, 63, 64, 97, 98, 99, 100, 101, 102, 65, 60, 115, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 47, 81, 82, 83, 84, 85, 197, 198, 201, 202, 199, 200, 192, 193, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 48, 203, 204, 205, 206, 207, 208, 209, 209, 263, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 148, 149, 150, 0, 0 };

			List<CraftData> result = new List<CraftData>();
			string exePath = Path.Combine(_detectedInstallPath, "XwingAlliance.exe");
			string stringsTxtPath = Path.Combine(_detectedInstallPath, "strings.txt");
			string spaceCraftLstPath = Path.Combine(_detectedInstallPath, "FlightModels", "SpaceCraft0.lst");
			string equipmentLstPath = Path.Combine(_detectedInstallPath, "FlightModels", "Equipment0.lst");

			if (!File.Exists(exePath) || !File.Exists(stringsTxtPath) || !File.Exists(spaceCraftLstPath) || !File.Exists(equipmentLstPath)) return result;

			// Load all craft strings.
			List<string> longNames = new List<string>();   // Full name     "TIE Fighter"
			List<string> shortNames = new List<string>();  // Abbreviation  "T/F"
			List<string> buoyNames = new List<string>();   // These objects (mines, sats, navs) are classified separately from craft.
			longNames.Add("None"); // Null entry for craft type zero.
			shortNames.Add("");
			try
			{
				using (StreamReader sr = File.OpenText(stringsTxtPath))
				{
					while (!sr.EndOfStream)
					{
						string s = sr.ReadLine();
						if (s.StartsWith("!KBUOY") && s.LastIndexOf("!") > 0) buoyNames.Add(s.Substring(s.LastIndexOf("!") + 1));
						else if (buoyNames.Count > 0 && s.StartsWith("!KLASTBUOYSTRING")) break;
					}
					while (!sr.EndOfStream)
					{
						string s = sr.ReadLine();
						if (s.StartsWith("!KSPEC") && s.IndexOf(":") > 0) longNames.Add(s.Substring(s.LastIndexOf(":") + 1));
						else if (longNames.Count > 1 && s.StartsWith("!KLASTSPECSTRING")) break;
					}
					while (!sr.EndOfStream)
					{
						string s = sr.ReadLine();
						if (s.StartsWith("!KSPEC") && s.IndexOf("_SHORT!") > 0) shortNames.Add(s.Substring(s.LastIndexOf("!") + 1));
						else if (shortNames.Count > 1 && s.StartsWith("!KLASTSPECSTRING")) break;
					}
				}
			}
			catch { /* do nothing */ }

			// Load all OPT resource names. There are two LST files to load.
			List<string>[] optList = new List<string>[2];
			optList[0] = new List<string>();
			optList[1] = new List<string>();
			try
			{
				using (StreamReader sr = File.OpenText(spaceCraftLstPath))
				{
					while (!sr.EndOfStream) { optList[0].Add(sr.ReadLine()); }
				}
				using (StreamReader sr = File.OpenText(equipmentLstPath))
				{
					while (!sr.EndOfStream) { optList[1].Add(sr.ReadLine()); }
				}
			}
			catch { /* do nothing */ }


			Dictionary<int, int> mappingTable = new Dictionary<int, int>();
			result = new List<CraftData>();
			for (int i = 0; i < 255; i++)   // I'm not sure exactly how long this array is.
				result.Add(new CraftData());

			// Open the EXE and load all the necessary data from it.
			try
			{
				using (FileStream fs = new FileStream(exePath, FileMode.Open, FileAccess.Read))
				{
					using (BinaryReader br = new BinaryReader(fs))
					{
						// First do a data check to see if the file reasonably matches what we can expect.
						// This is right before the craft definition array.
						fs.Position = 0x1BA060;
						if (Encoding.ASCII.GetString(br.ReadBytes(10)) == "Set volume")
						{
							// Load the mapping array which resolves a FlightGroup craftType to an internal object type. 
							// We'll map the reverse so we can look up which craftType slot to place our data into.
							fs.Position = 0x1AFB70;
							for (int i = 0; i < 233; i++)
							{
								short objIndex = br.ReadInt16();
								// There are some duplicate mapper entries for mines/sats, don't overwrite.
								if (!mappingTable.ContainsKey(objIndex)) mappingTable.Add(objIndex, i);
							}

							// Iterate through the object array, which specifies internal craft and models.
							// Zero based, and the zero index is a null object. X-wing starts at 1.
							for (int i = 0; i < 255; i++)   // I'm not sure exactly how long this array is.
							{
								fs.Position = 0x1F9E40 + (i * 0x18) + 0x12;   // Struct size is 0x18, skip first 0x12 bytes of each element.
								short craftDefIndex = br.ReadInt16();         // Index into craft definition array, or -1 if not craft.
								short groupIndex = br.ReadInt16();            // Which list file it appears in (SpaceCraft or Equipment).
								short lineIndex = br.ReadInt16();             // The line within the file of the specific OPT path to load.

								if ((groupIndex == 0 || groupIndex == 1) && lineIndex >= 0)
								{
									int craftType = -1;
									if (mappingTable.ContainsKey(i)) craftType = mappingTable[i];
									if (craftType >= 0 && craftType < result.Count)
									{
										CraftData data = result[craftType];

										if (craftDefIndex >= 0 && craftDefIndex < result.Count)
										{
											// Option to flag craft names if the craft mapping is different from vanilla.
											bool flag = false;
											if (flagRemappedCraft && craftType < defaultMapping.Length && i != defaultMapping[craftType]) flag = true;

											int stringIndex = i;
											if (stringIndex >= 0 && stringIndex < shortNames.Count) data.Abbrev = shortNames[stringIndex] + (flag ? "+" : "");
											if (stringIndex >= 0 && stringIndex < longNames.Count) data.Name = longNames[stringIndex] + (flag ? " +" : "");

											// Navigate to the craft definition array to retrieve its speed. Convert from MPH to MGLT (which is really just meters/second).
											fs.Position = 0x1BA080 + (craftDefIndex * 0x3DB) + 0x20;
											data.SpeedMglt = (int)((float)br.ReadInt16() * 0.44704);
										}
										else if (i >= 210 && i < 223)
										{
											// Special case for space object names.
											int stringIndex = i - 210;
											if (stringIndex >= 0 && stringIndex < buoyNames.Count)
											{
												data.Abbrev = buoyNames[stringIndex].IndexOf(" ") > 0 ? buoyNames[stringIndex].Substring(0, buoyNames[stringIndex].IndexOf(" ")).ToUpper() : "";
												data.Name = buoyNames[stringIndex];
											}
										}
										if (lineIndex >= 0 && lineIndex < optList[groupIndex].Count)
										{
											string res = optList[groupIndex][lineIndex];
											if (res.LastIndexOf('\\') >= 0) res = res.Substring(res.LastIndexOf('\\') + 1);
											if (res.LastIndexOf('.') >= 0) res = res.Remove(res.LastIndexOf('.'));
											data.ResourceNames = res;
										}
									}
								}
							}
						}
					}
				}
			}
			catch { /* do nothing */ }

			// Trim empty slots at the end of the list.
			while (result.Count > 232 && result[result.Count - 1].Name == "") result.RemoveAt(result.Count - 1);

			return result;
		}

		/// <summary>Loads craft data from an external file.</summary>
		/// <param name="filePath">The full path to the shiplist file.</param>
		/// <remarks>Typically each line of the file must be in the format CraftName,Abbrev,SpeedMGLT,Resources</remarks>
		List<CraftData> loadCraftData(string filePath)
		{
			if (!File.Exists(filePath)) return null;

			// This editor stuff is only needed for X-wing.
			bool isEditorSection = false;
			List<CraftData> editor = new List<CraftData>();

			List<CraftData> list = new List<CraftData>();
			try
			{
				using (StreamReader sr = File.OpenText(filePath))
				{
					while (!sr.EndOfStream)
					{
						string s = sr.ReadLine();
						if (s.IndexOf(";") >= 0) s = s.Remove(s.IndexOf(";"));  // Comment.
						s = s.Trim();  // Strip newlines or spaces preceeding comment.
						if (s == "") continue;

						if (_currentPlatform == Settings.Platform.XWING)
						{
							if (s == "[EDITOR]")
							{
								isEditorSection = true;
								continue;
							}
							else if (s == "[MAP]")
							{
								isEditorSection = false;
								continue;
							}
						}
						string[] line = s.Split(',');
						CraftData data = new CraftData()
						{
							Name = Common.SafeString(line, 0, false),
							Abbrev = Common.SafeString(line, 1, false)
						};
						int.TryParse(Common.SafeString(line, 2, false), out int speed);
						data.SpeedMglt = speed;
						data.ResourceNames = Common.SafeString(line, 3, false);
						if (isEditorSection) editor.Add(data);
						else list.Add(data);
					}
					sr.Close();
				}
			}
			catch { /* do nothing */ }
			_editorCraftData = editor;

			List<CraftData> container = _currentPlatform == Settings.Platform.XWING ? editor : list;
			for (int i = 0; i < container.Count; i++)
			{
				if (container[i].Name == "-" || container[i].Name == "#" || container[i].Name == "*")
				{
					container[i].Name = container[i].Name == "-" ? " " : container[i].Name == "#" ? i.ToString() : "Slot " + i.ToString();
					container[i].Abbrev = "";
				}
			}
			return list;
		}

		/// <summary>Merges two lists of craft data together into a new list.</summary>
		/// <remarks>Does not modify the primary or secondary lists.</remarks>
		/// <param name="first">Primary list.</param>
		/// <param name="second">Secondary list. If longer than the first, excess entries will be appended to the result.</param>
		/// <param name="overrideName">Specifies that names should always be replaced by the second list. Otherwise names will only be replaced if the string is empty in the first list.</param>
		List<CraftData> mergeLists(List<CraftData> first, List<CraftData> second, bool overrideName)
		{
			List<CraftData> result = new List<CraftData>();
			int i = 0;
			if (first != null)
			{
				for (i = 0; i < first.Count; i++)
				{
					CraftData entry = new CraftData(first[i]);
					if (second != null && i < second.Count) entry.Merge(second[i], overrideName);
					result.Add(entry);
				}
			}
			if (second != null)
			{
				// If the second list is longer, add the rest.
				for (; i < second.Count; i++) result.Add(new CraftData(second[i]));
			}
			return result;
		}
	}

	/// <summary>Information and resources for a single craft type.</summary>
	public class CraftData
	{
		/// <summary>Initializes a new instance</summary>
		public CraftData() 	{ /* do nothing */ }
		/// <summary>Initializes a new instance.</summary>
		/// <param name="source">Data optionally used to initialize the instance</param>
		/// <remarks>If <paramref name="source"/> is <b>null</b>, no additional actions are taken</remarks>
		public CraftData(CraftData source) { if (source != null) CopyFrom(source); }

		/// <summary>Directly copies all data elements from the source object.</summary>
		public void CopyFrom(CraftData source)
		{
			Name = source.Name;
			Abbrev = source.Abbrev;
			SpeedMglt = source.SpeedMglt;
			ResourceNames = source.ResourceNames;
		}
		/// <summary>Merges the data of another entry into this one.</summary>
		/// <param name="source">Object to copy from.</param>
		/// <param name="overrideName">Specifies whether the long/short names will always copied from the source. Otherwise names will only be copied if the existing names are empty.</param>
		public void Merge(CraftData source, bool overrideName)
		{
			if (source.Name != "" && (overrideName || Name == ""))
			{
				Name = source.Name;
				Abbrev = source.Abbrev;  // Update both so there's no leftover names.
			}
			if (source.Abbrev != "" && (overrideName || Abbrev == "")) Abbrev = source.Abbrev;
			SpeedMglt = source.SpeedMglt;
			ResourceNames = source.ResourceNames;
		}

		/// <summary>Gets or sets the Craft type name</summary>
		public string Name { get; set; } = "";
		/// <summary>Gets or sets the Craft type abbreviation</summary>
		public string Abbrev { get; set; } = "";
		/// <summary>Gets or sets the Craft type speed</summary>
		public int SpeedMglt { get; set; } = 0;
		/// <summary>Gets or sets the resources (OPT/SHIP) used for the Craft</summary>
		/// <remarks>Multiple resources are separated by "|", old DOS resources can be scaled using "*" (e.g., "*2")</remarks>
		public string ResourceNames { get; set; } = "";
	}
}
