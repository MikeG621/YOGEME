/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2023 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.14.1+
 */

/* CHANGELOG
 * [ADD] Concourse, HullIcon hooks support
 * [UPD] Redid GUI
 * [UPD] No longer restricted by hook presence, added "*" to cbo as flag
 * [UPD] Unrecognized sections and comments are kept
 * [ADD] MissionObject: weapon profiles
 * [ADD] MissionTie: Mission craft text, StatsProfiles
 * [UPD] FamHangarCamera: uses current "FamKey"
 * [UPD] HangarMap and FamHangarMap: ObjectProfile and IsFloorInverted
 * [UPD] HangarObjects: LoadShuttle now correctly checks on "1" instead of "not 0", "FoldOutside" removed, loads OPT replacements into list
 * [UPD] Camera and Family Camera tabs consolidated
 * [ADD] Hangar now looks for _Opt and _Opt_IFF# sections and files
 * [ADD] HangarObjects: ShuttleAnimationElevation, ShuttleObjectProfile
 * v1.14.1, 230814
 * [ADD] Hyperspace hook support
 * [ADD] Object profile per model
 * v1.11.2, 2101005
 * [UPD] Redid the layout so lst's less likely to be cutoff.
 * v1.10, 210520
 * [ADD] Wingman markings, Droid1/2Update
 * [ADD] S-Foils, Skins (32bpp), Shield hook support
 * [FIX] Missing Droid1/2PositionZ read
 * [UPD] Layout redesigned
 * [FIX] Now can handle comments at the end of a line
 * v1.8.2, 201219
 * [FIX] Add BD/Sound/Object/Hangar was cutting off first letter due to removal of "\\"
 * [UPD] Default ObjectsProfile updated to "default"
 * v1.8.1, 201213
 * [UPD] Settings passed in instead of re-init
 * [UPD] replaced StringFunctions.GetFileName with Path
 * [UPD] added a \\ into _installDirectory so it could be pulled out of everything else
 * [ADD] ObjectProfile_fg_#
 * v1.8, 201004
 * [UPD] improved use* bool efficiency
 * [ADD] HangarRoofCranePositionX, HangarRoofCraneAxis, HangarRoofCraneLowOffset, HangarRoofCraneHighOffset, HangarIff
 * [ADD] ShuttlePositionX/Y/Z, ShuttleOrientation, IsShuttleFloorInverted
 * [ADD] Droids/Droid1/Droid2PositionZ, IsDroidsFloorInverted
 * [ADD] PlayerOffsetX/Y/Z, IsPlayerFloorInverted
 * [FIX] case-sensitivity in filename [JB]
 * [FIX] added missing checks for chkObjects and chkSounds state [JB]
 * [FIX] handling of hex values and negative integers [JB]
 * [ADD] HangarRoofCranePositionY, HangarRoofCranePositionZ, PlayerAnimationElevation [JB]
 * [FIX] missing Write section for [Objects] [JB]
 * v1.6.3, 200101
 * [ADD] ShuttleAnimation and ShuttleAnimationStraightLine
 * v1.6.2, 190928
 * [UPD] changed the INI save backup name to prevent possible clashes
 * v1.6.1, 190916
 * [FIX #27] Crash when the INI doesn't exist
 * v1.6, 190915
 * - Release
 */

using Idmr.Platform.Xwa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
    public partial class XwaHookDialog : Form
	{
		// this is going to be setup to read from the individual TXT files, but always write to Mission.ini
		readonly string _mission;
		readonly string _fileName = "";
		readonly string _bdFile = "";
		readonly string _soundFile = "";
		readonly string _objFile = "";
		readonly string _missionTxtFile = "";
		readonly string _hangarObjectsFile = "";
		readonly string _hangarCameraFile = "";
		readonly string _famHangarCameraFile = "";
		readonly string _hangarMapFile = "";
		readonly string _famHangarMapFile = "";
        readonly int _commandIff = 0;
        readonly string _commandOpt = "";
        readonly string _hangarObjectsFileS = "";
        readonly string _hangarCameraFileS = "";
        readonly string _famHangarCameraFileS = "";
        readonly string _hangarMapFileS = "";
        readonly string _famHangarMapFileS = "";
        readonly string _hangarObjectsFileSI = "";
        readonly string _hangarCameraFileSI = "";
        readonly string _famHangarCameraFileSI = "";
        readonly string _hangarMapFileSI = "";
        readonly string _famHangarMapFileSI = "";
        readonly string _32bppFile = "";
		readonly string _shieldFile = "";
		readonly string _hyperFile = "";
		readonly string _concourseFile = "";
		readonly string _hullIconFile = "";
		readonly string _statsFile = "";
		readonly string _installDirectory = "";
		readonly string _mis = "Missions\\";
		readonly string _res = "Resdata\\";
		readonly string _wave = "Wave\\";
		readonly string _fm = "FlightModels\\";
		enum ReadMode { None = -1, Backdrop, Mission, Sounds, Objects, HangarObjects, HangarCamera, FamilyHangarCamera, HangarMap, FamilyHangarMap, Skins, Shield, Hyper, Concourse, HullIcon, Stats }
		bool _loading = false;
		readonly int[,] _cameras = new int[5, 3];
		readonly int[,] _defaultCameras = new int[5, 3] { { 1130, -2320, -300 }, { 1240, -330, -700 }, { -1120, 1360, -790 }, { -1200, -1530, -850 }, { 1070, 4640, -130 } };
		readonly int[,] _familyCameras = new int[7, 3];
		readonly int[,] _defaultFamilyCameras = new int[7, 3] { { 780, -6471, -4977 }, { -1970, -8810, -4707 }, { 2510, -5391, -5067 }, { 1740, -8461, -5047 }, { 3180, 2629, -3777 }, { 8242, 6500, 10 }, { -13360, 35019, -6537 } };
		enum ShuttleAnimation { Right, Top, Bottom }
		readonly int[] _defaultShuttlePosition = new int[4] { 1127, 959, 0, 43136 };
		readonly int[] _defaultRoofCranePosition = new int[3] { -1400, 786, -282 };
		readonly Panel[] _panels = new Panel[11];
		readonly List<string> _unknown = new List<string>();
        readonly List<string>[] _comments = new List<string>[15];
		string _preComments = "";
		readonly bool[] _skipIffs = new bool[255];
        readonly bool _initialLoad = true;

		public XwaHookDialog(Mission mission, Settings config)
		{
			InitializeComponent();
			_mission = Path.GetFileNameWithoutExtension(mission.MissionPath);
			if (_mission == "NewMission")
			{
				MessageBox.Show("Please perform initial save prior to hook assignment.", "New Mission detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cmdCancel_Click("NewMission", new EventArgs());
				return;
			}
			_fileName = mission.MissionPath.ToLower().Replace(".tie", ".ini");

			#region initialize
			Width = 780;
			Height = 620;
			_panels[0] = pnlBackdrops;
			_panels[1] = pnlMission;
			_panels[2] = pnlSounds;
			_panels[3] = pnlObjects;
			_panels[4] = pnlHangar;
			_panels[5] = pnlSFoils;
			_panels[6] = pnlSkins;
			_panels[7] = pnlShield;
			_panels[8] = pnlHyper;
			_panels[9] = pnlConcourse;
			_panels[10] = pnlHullIcon;
			for (int i = 0; i < _panels.Length; i++)
			{
				_panels[i].Left = 395;
				_panels[i].Top = 45;
			}
			cboHook.SelectedIndex = 0;
			cboIff.Items.AddRange(Strings.IFF);
			cboHangarIff.Items.AddRange(Strings.IFF);
			cboSkipIff.Items.AddRange(Strings.IFF);
			for (int i = cboIff.Items.Count; i < 256; i++)
			{
				cboIff.Items.Add("IFF #" + (i + 1));
				cboHangarIff.Items.Add("IFF #" + (i + 1));
				cboSkipIff.Items.Add("IFF #" + (i + 1));
			}
			cboIff.SelectedIndex = 0;
			cboSkipIff.Items.RemoveAt(255); // special value
			cboSkipIff.SelectedIndex = 0;
			cboMarkings.Items.AddRange(Strings.Color);
			cboShuttleMarks.Items.AddRange(Strings.Color);
			cboMapMarkings.Items.AddRange(Strings.Color);
			cboFamMapMarkings.Items.AddRange(Strings.Color);
			cboSkinMarks.Items.AddRange(Strings.Color);
			cboStatMarks.Items.AddRange(Strings.Color);
			cboDroid1Markings.Items.AddRange(Strings.Color);
			cboDroid2Markings.Items.AddRange(Strings.Color);
			for (int i = cboMarkings.Items.Count; i < 256; i++)
			{
				cboMarkings.Items.Add("Clr #" + (i + 1));
				cboShuttleMarks.Items.Add("Clr #" + (i + 1));
				cboMapMarkings.Items.Add("Clr #" + (i + 1));
				cboFamMapMarkings.Items.Add("Clr #" + (i + 1));
				cboSkinMarks.Items.Add("Clr #" + (i + 1));
				cboStatMarks.Items.Add("Clr #" + (i + 1));
				cboDroid1Markings.Items.Add("Clr #" + (i + 1));
				cboDroid2Markings.Items.Add("Clr #" + (i + 1));
			}
			cboMarkings.SelectedIndex = 0;
			cboShuttleMarks.SelectedIndex = 0;
			cboShuAnimation.SelectedIndex = 0;
			cboMapMarkings.SelectedIndex = 0;
			cboFamMapMarkings.SelectedIndex = 0;
			cboSkinMarks.SelectedIndex = 0;
			cboDroid1Markings.SelectedIndex = 0;
			cboDroid2Markings.SelectedIndex = 0;
			cboFG.Items.AddRange(mission.FlightGroups.GetList());
			cboProfileFG.Items.AddRange(mission.FlightGroups.GetList());
			cboSFoilFG.Items.AddRange(mission.FlightGroups.GetList());
			for (int i = 0; i < 400; i++)
			{
				cboShuttleModel.Items.Add(i);
				cboMapIndex.Items.Add(i);
				cboFamMapIndex.Items.Add(i);
				cboDroid1Model.Items.Add(i);
				cboDroid2Model.Items.Add(i);
			}
			cboShuttleModel.SelectedIndex = 50;
			cboMapIndex.SelectedIndex = 0;
			cboFamMapIndex.SelectedIndex = 0;
			cboDroid1Model.SelectedIndex = 311;
			cboDroid2Model.SelectedIndex = 312;
			for (int i = 0; i < 5; i++)
				for (int j = 0; j < 3; j++)
					_cameras[i, j] = _defaultCameras[i, j];
            cboCamera.SelectedIndex = 0;
			for (int i = 0; i < 7; i++)
				for (int j = 0; j < 3; j++)
					_familyCameras[i, j] = _defaultFamilyCameras[i, j];
            cboFamilyCamera.SelectedIndex = 0;
            cboShield.Items.AddRange(Strings.CraftType);
			cboShield.SelectedIndex = 0;
			cboCraftText.Items.AddRange(Strings.CraftType);
			cboCraftText.SelectedIndex = 0;
			for (int i = 0; i < _comments.Length; i++) _comments[i] = new List<string>();
            #endregion

            if (config.XwaInstalled) _installDirectory = config.XwaPath + "\\";
            #region get CommandShip
            // CommandShip priority order: Player's CommandShip, Player's Arrival MS, Player's Departure MS
            int commandShip = -1;
            for (int i = 0; i < mission.FlightGroups.Count; i++)
			{
				if (mission.FlightGroups[i].PlayerNumber == 1)
				{
					var player = mission.FlightGroups[i];
					int team = player.Team;
					for (int j = 0; j < mission.FlightGroups.Count; j++)
					{
						var fg = mission.FlightGroups[j];
						if ((fg.EnableDesignation1 == team && fg.Designation1 == 0) || (fg.EnableDesignation2 == team && fg.Designation2 == 0))
						{
							commandShip = j;
							break;
						}
					}
					if (commandShip != -1) break;

					if (player.ArrivalMethod1) commandShip = player.ArrivalCraft1;
					else if (player.DepartureMethod1) commandShip = player.DepartureCraft1;
				}
                if (commandShip != -1) break;
            }
			if (commandShip != -1 && config.XwaInstalled)
			{
				try
				{
					_commandIff = mission.FlightGroups[commandShip].IFF;

					// OPT here. Use ship to get Species, to get EXE craft properties, which gets the line number in either LST, which is the OPT name
					int type = mission.FlightGroups[commandShip].CraftType;
					short lstFile = 0;
					short lstLine = 0;
					using (var exe = new BinaryReader(File.OpenRead(_installDirectory + "\\XwingAlliance.exe")))
					{
						exe.BaseStream.Position = 0x1AFB70 + type * 2;
						short species = exe.ReadInt16();
                        exe.BaseStream.Position = 0x1F9E40 + species * 0x18 + 0x14;
						lstFile = exe.ReadInt16();
						lstLine = exe.ReadInt16();
					}
					string lstFileName = "";
					if (lstFile == 0) lstFileName = "SPACECRAFT0.LST";
					else if (lstFile == 1) lstFileName = "EQUIPMENT0.LST";

					using (var lst = new StreamReader(_installDirectory + _fm + lstFileName))
					{
						int lineNumber = 0;
						while (lineNumber < lstLine)
						{
							lst.ReadLine();
							lineNumber++;
						}

						_commandOpt = Path.GetFileNameWithoutExtension(lst.ReadLine());
					}
				}
				catch { /* do nothing. If anything in here blows up, just forget it */ }
			}
            #endregion

            if (config.XwaInstalled)
			{
				for (int i = 0; i < cboHook.Items.Count; i++)
				{
					string hook = "Hook_" + cboHook.Items[i].ToString() + ".dll";
					if (!File.Exists(_installDirectory + hook)) cboHook.Items[i] = "*" + cboHook.Items[i];
                }

				_bdFile = checkFile("_Resdata.txt");
				_soundFile = checkFile("_Sounds.txt");
				_objFile = checkFile("_Objects.txt");
				_missionTxtFile = checkFile(".txt");
				_hangarObjectsFile = checkFile("_HangarObjects.txt");
				_hangarCameraFile = checkFile("_HangarCamera.txt");
				_famHangarCameraFile = checkFile("_FamHangarCamera.txt");
				_hangarMapFile = checkFile("_HangarMap.txt");
				_famHangarMapFile = checkFile("_FamHangarMap.txt");
				if (_commandOpt != "")
				{
                    _hangarObjectsFileSI = checkFile("_HangarObjects_" + _commandOpt + "_" + _commandIff + ".txt");
                    _hangarCameraFileSI = checkFile("_HangarCamera_" + _commandOpt + "_" + _commandIff + ".txt");
                    _famHangarCameraFileSI = checkFile("_FamHangarCamera_" + _commandOpt + "_" + _commandIff + ".txt");
                    _hangarMapFileSI = checkFile("_HangarMap_" + _commandOpt + "_" + _commandIff + ".txt");
                    _famHangarMapFileSI = checkFile("_FamHangarMap_" + _commandOpt + "_" + _commandIff + ".txt");
                    _hangarObjectsFileS = checkFile("_HangarObjects_" + _commandOpt + ".txt");
                    _hangarCameraFileS = checkFile("_HangarCamera_" + _commandOpt + ".txt");
                    _famHangarCameraFileS = checkFile("_FamHangarCamera_" + _commandOpt + ".txt");
                    _hangarMapFileS = checkFile("_HangarMap_" + _commandOpt + ".txt");
                    _famHangarMapFileS = checkFile("_FamHangarMap_" + _commandOpt + ".txt");
                }
				_32bppFile = checkFile("_Skins.txt");
				_shieldFile = checkFile("_Shield.txt");
				_hyperFile = checkFile("_Hyperspace.txt");
				_concourseFile = checkFile("_Concourse.txt");
				_hullIconFile = checkFile("_HullIcon.txt");
				_statsFile = checkFile("_StatsProfile.txt");
			}
			string line;

			#region individual files
			if (_bdFile != "")
			{
				using (var sr = new StreamReader(_bdFile))
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						lstBackdrops.Items.Add(line);
					}
			}
			if (_missionTxtFile != "")
			{
				using (var sr = new StreamReader(_missionTxtFile))
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						parseMission(line);
					}
			}
			if (_soundFile != "")
			{
				using (var sr = new StreamReader(_soundFile))
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						lstSounds.Items.Add(line);
					}
			}
			if (_objFile != "")
			{
				using (var sr = new StreamReader(_objFile))
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						if (line.ToLower().Contains("_hullicon")) parseHullIcon(line);
						else lstObjects.Items.Add(line);
					}
			}
            if (_commandOpt != "")
            {
                if (_hangarObjectsFileSI != "")
                {
                    using (var sr = new StreamReader(_hangarObjectsFileSI))
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = removeComment(line);
                            if (line == "") continue;

                            parseHangarObjects(line);
                        }
                }
                if (_hangarCameraFileSI != "")
                {
                    using (var sr = new StreamReader(_hangarCameraFileSI))
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = removeComment(line);
                            if (line == "") continue;

                            parseHangarCamera(line);
                        }
                }
                if (_famHangarCameraFileSI != "")
                {
                    using (var sr = new StreamReader(_famHangarCameraFileSI))
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = removeComment(line);
                            if (line == "") continue;

                            parseFamilyHangarCamera(line);
                        }
                }
                if (_hangarMapFileSI != "")
                {
                    using (var sr = new StreamReader(_hangarMapFileSI))
                    {
                        MapEntry entry = new MapEntry();
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = removeComment(line);
                            if (line == "") continue;

                            if (entry.Parse(line))
                                lstMap.Items.Add(entry.ToString());
                        }
                    }
                }
                if (_famHangarMapFileSI != "")
                {
                    using (var sr = new StreamReader(_famHangarMapFileSI))
                    {
                        MapEntry entry = new MapEntry();
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = removeComment(line);
                            if (line == "") continue;

                            if (entry.Parse(line))
                                lstFamilyMap.Items.Add(entry.ToString());
                        }
                    }
                }
                if (_hangarObjectsFileS != "")
                {
                    using (var sr = new StreamReader(_hangarObjectsFileS))
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = removeComment(line);
                            if (line == "") continue;

                            parseHangarObjects(line);
                        }
                }
                if (_hangarCameraFileS != "")
                {
                    using (var sr = new StreamReader(_hangarCameraFileS))
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = removeComment(line);
                            if (line == "") continue;

                            parseHangarCamera(line);
                        }
                }
                if (_famHangarCameraFileS != "")
                {
                    using (var sr = new StreamReader(_famHangarCameraFileS))
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = removeComment(line);
                            if (line == "") continue;

                            parseFamilyHangarCamera(line);
                        }
                }
                if (_hangarMapFileS != "")
                {
                    using (var sr = new StreamReader(_hangarMapFileS))
                    {
                        MapEntry entry = new MapEntry();
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = removeComment(line);
                            if (line == "") continue;

                            if (entry.Parse(line))
                                lstMap.Items.Add(entry.ToString());
                        }
                    }
                }
                if (_famHangarMapFileS != "")
                {
                    using (var sr = new StreamReader(_famHangarMapFileS))
                    {
                        MapEntry entry = new MapEntry();
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = removeComment(line);
                            if (line == "") continue;

                            if (entry.Parse(line))
                                lstFamilyMap.Items.Add(entry.ToString());
                        }
                    }
                }
            }
            if (_hangarObjectsFile != "")
			{
				using (var sr = new StreamReader(_hangarObjectsFile))
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						parseHangarObjects(line);
					}
			}
			if (_hangarCameraFile != "")
			{
				using (var sr = new StreamReader(_hangarCameraFile))
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						parseHangarCamera(line);
					}
			}
			if (_famHangarCameraFile != "")
			{
				using (var sr = new StreamReader(_famHangarCameraFile))
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						parseFamilyHangarCamera(line);
					}
			}
			if (_hangarMapFile != "")
			{
				using (var sr = new StreamReader(_hangarMapFile))
				{
					MapEntry entry = new MapEntry();
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						if (entry.Parse(line))
							lstMap.Items.Add(entry.ToString());
					}
				}
			}
			if (_famHangarMapFile != "")
			{
				using (var sr = new StreamReader(_famHangarMapFile))
				{
					MapEntry entry = new MapEntry();
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						if (entry.Parse(line))
							lstFamilyMap.Items.Add(entry.ToString());
					}
				}
			}
			if (_32bppFile != "")
			{
				using (var sr = new StreamReader(_32bppFile))
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						lstSkins.Items.Add(line);
					}
			}
			if (_shieldFile != "")
			{
				using (var sr = new StreamReader(_shieldFile))
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						parseShield(line);
					}
			}
			if (_hyperFile != "")
			{
				using (var sr = new StreamReader(_hyperFile))
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = removeComment(line);
                        if (line == "") continue;

                        parseHyper(line);
                    }
			}
			if (_concourseFile != "")
			{
				using (var sr = new StreamReader (_concourseFile))
					while ((line = sr.ReadLine()) != null)
					{
                        line = removeComment(line);
                        if (line == "") continue;

                        parseConcourse(line);
                    }
			}
			if (_hullIconFile != "")
			{
				using (var sr = new StreamReader (_hullIconFile))
				{
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment(line);
						if (line == "") continue;

						parseHullIcon(line);
					}
				}
			}
			if (_statsFile != "")
			{
				using (var sr = new StringReader(_statsFile))
				{
					while ((line = sr.ReadLine()) != null)
					{
						line = removeComment (line);
						if (line == "") continue;

						lstStats.Items.Add(line);
					}
				}
			}
			#endregion

			if (File.Exists(_fileName))
			{
				using (var sr = new StreamReader(_fileName))
				{
					txtHook.Text = sr.ReadToEnd();
				}
                parseContents();
            }

			_initialLoad = false;

			createContents();	// to mix in any TXT files
		}

		string checkFile(string extension)
		{
			if (File.Exists(_installDirectory + _mis + _mission + extension)) return _installDirectory + _mis + _mission + extension;
			return "";
		}

		void createContents()
		{
            string title = "";
            if (_installDirectory != "") using (var sr = new StreamReader(_installDirectory + "\\Missions\\MISSION.LST"))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = removeComment(line);
                        if (line == "" || line.StartsWith("!")) continue;

                        while (line.Length > 5 && line[1] != 'b' && line[1] != 'B')
                        {
                            // skips comment lines, battle headers, mission desc, index number
                            line = line.Substring(1);
                            // removes first char until it meets #B#M... in case lines starts with "* "
                        }
                        if (line.ToLower() == _mission.ToLower() + ".tie")
                        {
                            title = sr.ReadLine();
                            title = title.Substring(title.IndexOf("DESC!") + 5);
                            break;
                        }
                    }
                }

            string contents = ";" + _mission + ".ini" + (title != "" ? " - " + title : "") + "\r\n\r\n";
			contents += _preComments + "\r\n";
            if (lstBackdrops.Items.Count > 0)
            {
                contents += "[Resdata]\r\n";
                for (int i = 0; i < lstBackdrops.Items.Count; i++) contents += lstBackdrops.Items[i] + "\r\n";
				contents += "\r\n";
            }
			insertComments(ref contents, (int)ReadMode.Backdrop);
            if (lstMission.Items.Count > 0 || useSFoils || lstCraftText.Items.Count > 0 || useMissionSettings)
            {
                contents += "[Mission_Tie]\r\n";
                if (lstMission.Items.Count > 0)
                {
                    for (int i = 0; i < lstMission.Items.Count; i++)
                    {
                        string[] parts = lstMission.Items[i].ToString().Split(',');
                        int fg;
                        for (fg = 0; fg < cboFG.Items.Count; fg++) if (cboFG.Items[fg].ToString() == parts[0]) break;
                        if (parts[1] == "marks")
                        {
                            for (int m = 0; m < cboMarkings.Items.Count; m++)
                                if (cboMarkings.Items[m].ToString() == parts[2])
                                {
                                    contents += "fg, " + fg + ", markings, " + m + "\r\n";
                                    break;
                                }
                        }
                        else if (parts[1] == "wing")
                        {
                            for (int m = 0; m < cboMarkings.Items.Count; m++)
                                if (cboMarkings.Items[m].ToString() == parts[3])
                                {
                                    contents += "fg, " + fg + ", index, " + parts[2] + ", markings, " + m + "\r\n";
                                    break;
                                }
                        }
                        else if (parts[1] == "iff")
                        {
                            for (int iff = 0; iff < cboIff.Items.Count; iff++)
                                if (cboIff.Items[iff].ToString() == parts[2])
                                {
                                    contents += "fg, " + fg + ", iff, " + iff + "\r\n";
                                    break;
                                }
                        }
                        else if (parts[1] == "pilot")
                            contents += "fg, " + fg + ", pilotvoice, " + parts[2] + "\r\n";
                    }
                }
                if (useSFoils)
                {
                    for (int i = 0; i < lstSFoils.Items.Count; i++)
                    {
                        string[] parts = lstSFoils.Items[i].ToString().Split(',');
                        int fg;
                        for (fg = 0; fg < cboSFoilFG.Items.Count; fg++) if (cboSFoilFG.Items[fg].ToString() == parts[0]) break;
                        if (parts[1] == "closed")
                            contents += "fg, " + fg + ", close_SFoils, 1\r\n";
                        else if (parts[1] == "open")
                            contents += "fg, " + fg + ", open_LandingGears, 1\r\n";
                    }
                    if (chkForceHangarSF.Checked)
                        contents += "CloseSFoilsAndOpenLandingGearsBeforeEnterHangar = 1\r\n";
                    if (chkForceHyperLG.Checked)
                        contents += "CloseLandingGearsBeforeEnterHyperspace = 1\r\n";
                    if (chkManualSF.Checked)
                        contents += "AutoCloseSFoils = 0\r\n";
                }
				if (lstCraftText.Items.Count > 0)
				{
					for (int i = 0; i < lstCraftText.Items.Count; i++)
					{
						string[] parts = lstCraftText.Items[i].ToString().Split(',');
						int craft;
						for (craft = 0; craft < cboCraftText.Items.Count; craft++) if (cboCraftText.Items[craft].ToString() == parts[0]) break;
						if (parts[1] == "name")
							contents += "craft, " + craft + ", name, " + parts[2] + "\r\n";
						else if (parts[1] == "species")
							contents += "craft, " + craft + ", specname, " + parts[2] + "\r\n";
						else if (parts[1] == "plural")
                            contents += "craft, " + craft + ", pluralname, " + parts[2] + "\r\n";
						else if (parts[1] == "abbrv")
                            contents += "craft, " + craft + ", shortname, " + parts[2] + "\r\n";
					}
				}
				if (useMissionSettings)
				{
					if (chkRedAlert.Checked)
						contents += "IsRedAlertEnabled = 1\r\n";
					if (chkSkipHyper.Checked)
						contents += "SkipHyperspacedMessages = 1\r\n";
					if (chkSkipIffMessages.Checked)
					{
						if (chkSkipAllIff.Checked)
							contents += "SkipObjectsMessagesIff = 255\r\n";
						else
							for (int i = 0; i < _skipIffs.Length; i++)
								if (_skipIffs[i])
									contents += "SkipObjectsMessagesIff = " + i + "\r\n";
					}
					if (chkForceTurret.Checked)
						contents += "ForcePlayerInTurret = 1\r\n";
					if (numTurretH.Value != 0)
						contents += "ForcePlayerInTurretHours = " + (int)numTurretH.Value + "\r\n";
					if (numTurretM.Value != 0)
						contents += "ForcePlayerInTurretMinutes = " + (int)numTurretM.Value + "\r\n";
					if (numTurretS.Value != 8)
						contents += "ForcePlayerInTurretSeconds = " + (int)numTurretS.Value + "\r\n";
					if (chkDisableLaser.Checked)
						contents += "DisablePlayerLaserShoot = 1\r\n";
					if (chkDisableWarhead.Checked)
						contents += "DisablePlayerWarheadShoot = 1\r\n";
					if (chkDisableCollision.Checked)
						contents += "IsWarheadCollisionDamagesEnabled = 0\r\n";
				}
				contents += "\r\n";
            }
            insertComments(ref contents, (int)ReadMode.Mission);
            if (lstSounds.Items.Count > 0)
            {
                contents += "[Sounds]\r\n";
                for (int i = 0; i < lstSounds.Items.Count; i++) contents += lstSounds.Items[i] + "\r\n";
                contents += "\r\n";
            }
            insertComments(ref contents, (int)ReadMode.Sounds);
            if (lstObjects.Items.Count > 0 || lstHullIcon.Items.Count > 0)
            {
                contents += "[Objects]\r\n";
                for (int i = 0; i < lstObjects.Items.Count; i++) contents += lstObjects.Items[i] + "\r\n";
                for (int i = 0; i < lstHullIcon.Items.Count; i++) contents += lstHullIcon.Items[i] + "\r\n";
                contents += "\r\n";
            }
            insertComments(ref contents, (int)ReadMode.Objects);
			if (useHangarObjects)
			{
				contents += "[HangarObjects]\r\n";
				if (!chkShuttle.Checked) contents += "LoadShuttle = 0\r\n";
				else
				{
					if (cboShuttleModel.SelectedIndex != 50) contents += "ShuttleModelIndex = " + cboShuttleModel.SelectedIndex + "\r\n";
					if (cboShuttleMarks.SelectedIndex != 0) contents += "ShuttleMarkings = " + cboShuttleMarks.SelectedIndex + "\r\n";
					if (txtShuttleProfile.Text != "") contents += "ShuttleObjectProfile = " + txtShuttleProfile.Text + "\r\n";
					if (numShuttlePositionX.Value != _defaultShuttlePosition[0]) contents += "ShuttlePositionX = " + (int)numShuttlePositionX.Value + "\r\n";
					if (numShuttlePositionY.Value != _defaultShuttlePosition[1]) contents += "ShuttlePositionY = " + (int)numShuttlePositionY.Value + "\r\n";
					if (numShuttlePositionZ.Value != _defaultShuttlePosition[2]) contents += "ShuttlePositionZ = " + (int)numShuttlePositionZ.Value + "\r\n";
					if (numShuttleOrientation.Value != _defaultShuttlePosition[3]) contents += "ShuttleOrientation = " + (int)numShuttleOrientation.Value + "\r\n";
					if (chkShuttleFloor.Checked) contents += "IsShuttleFloorInverted = 1\r\n";
					if (cboShuAnimation.SelectedIndex != 0) contents += "ShuttleAnimation = " + cboShuAnimation.Text + "\r\n";
					if (numShuDistance.Value != 0) contents += "ShuttleAnimationStraightLine = " + (int)numShuDistance.Value + "\r\n";
					if (numShuElevation.Value != 0) contents += "ShuttleAnimationElevation = " + (int)numShuElevation.Value + "\r\n";
				}

				if (!chkDroids.Checked) contents += "LoadDroids = 0\r\n";
				else
				{
					if (numDroidsZ.Value != 0) contents += "DroidsPositionZ = " + (int)numDroidsZ.Value + "\r\n";
					if (chkDroidsFloor.Checked) contents += "IsDroidsFloorInverted = 1\r\n";
					if (!chkLoadDroid1.Checked) contents += "LoadDroid1 = 0\r\n";
					else
					{
						if (numDroid1Z.Value != numDroidsZ.Value) contents += "Droid1PositionZ = " + (int)numDroid1Z.Value + "\r\n";
						if (chkDroid1Floor.Checked) contents += "IsDroid1FloorInverted = 1\r\n";
						if (!chkDroid1Update.Checked) contents += "Droid1Update = 0\r\n";
						if (cboDroid1Model.SelectedIndex != 311) contents += "Droid1ModelIndex = " + cboDroid1Model.SelectedIndex + "\r\n";
						if (cboDroid1Markings.SelectedIndex != 0) contents += "Droid1Markings = " + cboDroid1Markings.SelectedIndex + "\r\n";
						if (txtDroid1Profile.Text != "") contents += "Droid1ObjectProfile = " + txtDroid1Profile.Text + "\r\n";
					}
					if (!chkLoadDroid2.Checked) contents += "LoadDroid2 = 0\r\n";
					else
					{
						if (numDroid2Z.Value != numDroidsZ.Value) contents += "Droid2PositionZ = " + (int)numDroid2Z.Value + "\r\n";
						if (chkDroid2Floor.Checked) contents += "IsDroid2FloorInverted = 1\r\n";
						if (!chkDroid2Update.Checked) contents += "Droid2Update = 0\r\n";
						if (cboDroid2Model.SelectedIndex != 312) contents += "Droid2ModelIndex = " + cboDroid2Model.SelectedIndex + "\r\n";
						if (cboDroid2Markings.SelectedIndex != 0) contents += "Droid2Markings = " + cboDroid2Markings.SelectedIndex + "\r\n";
						if (txtDroid2Profile.Text != "") contents += "Droid2ObjectProfile = " + txtDroid2Profile.Text + "\r\n";
					}
				}

                if (numRoofCranePositionX.Value != _defaultRoofCranePosition[0]) contents += "HangarRoofCranePositionX = " + (int)numRoofCranePositionX.Value + "\r\n";
				if (numRoofCranePositionY.Value != _defaultRoofCranePosition[1]) contents += "HangarRoofCranePositionY = " + (int)numRoofCranePositionY.Value + "\r\n";
				if (numRoofCranePositionZ.Value != _defaultRoofCranePosition[2]) contents += "HangarRoofCranePositionZ = " + (int)numRoofCranePositionZ.Value + "\r\n";
				if (optRoofCraneAxisY.Checked) contents += "HangarRoofCraneAxis = 1\r\n";
				else if (optRoofCraneAxisZ.Checked) contents += "HangarRoofCraneAxis = 2\r\n";
				if (numRoofCraneLowOffset.Value != 0) contents += "HangarRoofCraneLowOffset = " + (int)numRoofCraneLowOffset.Value + "\r\n";
				if (numRoofCraneHighOffset.Value != 0) contents += "HangarRoofCraneHighOffset = " + (int)numRoofCraneHighOffset.Value + "\r\n";
				if (chkFloor.Checked) contents += "IsHangarFloorInverted = 1\r\n";
				if (chkHangarIff.Checked) contents += "HangarIff = " + cboHangarIff.SelectedIndex + "\r\n";

				if (numPlayerAnimationElevation.Value != 0) contents += "PlayerAnimationElevation = " + (int)numPlayerAnimationElevation.Value + "\r\n";
				if (numPlayerX.Value != 0) contents += "PlayerOffsetX = " + (int)numPlayerX.Value + "\r\n";
				if (numPlayerY.Value != 0) contents += "PlayerOffsetY = " + (int)numPlayerY.Value + "\r\n";
				if (numPlayerZ.Value != 0) contents += "PlayerOffsetZ = " + (int)numPlayerZ.Value + "\r\n";
				if (chkPlayerFloor.Checked) contents += "IsPlayerFloorInverted = 1\r\n";

				for (int i = 0; i < lstHangarObjects.Items.Count; i++) contents += lstHangarObjects.Items[i] + "\r\n";
				contents += "\r\n";
			}
            insertComments(ref contents, (int)ReadMode.HangarObjects);
            if (useHangarCamera)
			{
				contents += "[HangarCamera]\r\n";
				string[] keys = { "1", "2", "3", "6", "9" };
				for (int i = 0; i < 5; i++)
				{
					bool use = false;
					for (int j = 0; j < 3; j++) use |= (_cameras[i, j] != _defaultCameras[i, j]);
					if (use)
					{
						contents += "Key" + keys[i] + "_X = " + _cameras[i, 0] + "\r\n";
						contents += "Key" + keys[i] + "_Y = " + _cameras[i, 1] + "\r\n";
						contents += "Key" + keys[i] + "_Z = " + _cameras[i, 2] + "\r\n";
					}
				}
				contents += "\r\n";
			}
            insertComments(ref contents, (int)ReadMode.HangarCamera);
            if (useFamilyHangarCamera)
			{
				contents += "[FamHangarCamera]\r\n";
				string[] keys = { "1", "2", "3", "6", "7", "8", "9" };
				for (int i = 0; i < 7; i++)
				{
					bool use = false;
					for (int j = 0; j < 3; j++) use |= (_familyCameras[i, j] != _defaultFamilyCameras[i, j]);
					if (use)
					{
						contents += "FamKey" + keys[i] + "_X = " + _familyCameras[i, 0] + "\r\n";
						contents += "FamKey" + keys[i] + "_Y = " + _familyCameras[i, 1] + "\r\n";
						contents += "FamKey" + keys[i] + "_Z = " + _familyCameras[i, 2] + "\r\n";
					}
				}
				contents += "\r\n";
			}
            insertComments(ref contents, (int)ReadMode.FamilyHangarCamera);
            if (useHangarMap)
			{
				contents += "[HangarMap]\r\n";
				for (int i = 0; i < lstMap.Items.Count; i++) contents += lstMap.Items[i].ToString() + "\r\n";
				contents += "\r\n";
			}
            insertComments(ref contents, (int)ReadMode.HangarMap);
            if (useFamilyHangarMap)
			{
				contents += "[FamHangarMap]\r\n";
				for (int i = 0; i < lstFamilyMap.Items.Count; i++) contents += lstFamilyMap.Items[i].ToString() + "\r\n";
				contents += "\r\n";
			}
            insertComments(ref contents, (int)ReadMode.FamilyHangarMap);
            if (lstSkins.Items.Count > 0)
            {
                contents += "[Skins]\r\n";
                for (int i = 0; i < lstSkins.Items.Count; i++) contents += lstSkins.Items[i] + "\r\n";
				contents += "\r\n";
            }
            insertComments(ref contents, (int)ReadMode.Skins);
            if (lstShield.Items.Count > 0 || !chkSSRecharge.Checked)
            {
                contents += "[Shield]\r\n";
                for (int i = 0; i < lstShield.Items.Count; i++)
                {
                    string[] parts = lstShield.Items[i].ToString().Split('=');
                    parts[0] = parts[0].Trim();
                    int craft;
                    for (craft = 0; craft < Strings.CraftType.Length; craft++)
                        if (parts[0] == Strings.CraftType[craft]) break;
                    parts = parts[1].Trim().Split(' ');
                    bool perGen = (parts.Length > 1);
                    int rate = int.Parse(parts[0]);
                    contents += craft + ", " + (perGen ? "1, " + rate + ", 0" : "0, 0, " + rate) + "\r\n";
                }
				if (!chkSSRecharge.Checked) contents += "IsShieldRechargeForStarshipsEnabled = 0\r\n";
				contents += "\r\n";
            }
            insertComments(ref contents, (int)ReadMode.Shield);
            if (!optHypGlobal.Checked)
            {
				contents += "[Hyperspace]\r\nShortHyperspaceEffect = " + (optHypNormal.Checked ? "0" : "1") + "\r\n\r\n";
            }
            insertComments(ref contents, (int)ReadMode.Hyper);
            if (chkConcoursePlanetIndex.Checked || chkConcoursePlanetX.Checked || chkConcoursePlanetY.Checked)
			{
				contents += "[Concourse]\r\n";
				if (chkConcoursePlanetIndex.Checked) contents += "FrontPlanetIndex = " + numConcoursePlanetIndex.Value + "\r\n";
				if (chkConcoursePlanetX.Checked) contents += "FrontPlanetPositionX = " + numConcoursePlanetX.Value + "\r\n";
				if (chkConcoursePlanetY.Checked) contents += "FontPlanetPositionY = " + numConcoursePlanetY.Value + "\r\n";
				contents += "\r\n";
			}
            insertComments(ref contents, (int)ReadMode.Concourse);
			if (chkPlayerHull.Checked && numPlayerHull.Value > 0)
			{
				contents += "[HullIcon]\r\nPlayerHullIcon = " + numPlayerHull.Value + "\r\n\r\n";
			}
			insertComments(ref contents, (int)ReadMode.HullIcon);
			if (lstStats.Items.Count > 0)
			{
				contents += "[StatsProfiles]\r\n";
				for (int i = 0; i < lstStats.Items.Count; i++) contents += lstStats.Items[i] + "\r\n";
				contents += "\r\n";
			}
			insertComments(ref contents, (int)ReadMode.Stats);

            for (int i = 0; i < _unknown.Count; i++) contents += _unknown[i] + "\r\n";

			txtHook.Text = contents;
        }

		void insertComments(ref string text, int index)
		{
            if (_comments[index].Count > 0)
            {
                text = text.Substring(0, text.Length - 2);	// trim off the extra "\r\n"
                for (int i = 0; i < _comments[index].Count; i++) text += _comments[index][i] + "\r\n";
                text += "\r\n";
            }
        }

        void parseContents()
        {
			if (!_initialLoad) reset();

            string line;
            string lineLower;
            ReadMode readMode = ReadMode.None;
			bool isPre = true;

			_preComments = "";
			_unknown.Clear();
			for (int i = 0; i < _comments.Length; i++) _comments[i].Clear();

            for (int i = 0; i < txtHook.Lines.Length; i++)
            {
                if (txtHook.Lines[i].Trim() == "") continue;
                
				line = txtHook.Lines[i];
                line = removeComment(line);
				if (line == "")
				{
					if (isPre && !txtHook.Lines[i].StartsWith(";" + _mission + ".ini")) _preComments += txtHook.Lines[i] + "\r\n";
					else if (!isPre)
					{
						if (readMode == ReadMode.None) _unknown.Add(txtHook.Lines[i]);
						else _comments[(int)readMode].Add(txtHook.Lines[i]);
					}
					continue;
				}

                lineLower = line.ToLower();

				if (line.StartsWith("["))
				{
					isPre = false;
					readMode = ReadMode.None;
					if (lineLower == "[resdata]") readMode = ReadMode.Backdrop;
					else if (lineLower == "[mission_tie]") readMode = ReadMode.Mission;
					else if (lineLower == "[sounds]") readMode = ReadMode.Sounds;
					else if (lineLower == "[objects]") readMode = ReadMode.Objects;
					else if (lineLower == "[hangarobjects]") readMode = ReadMode.HangarObjects;
					else if (lineLower == "[hangarcamera]") readMode = ReadMode.HangarCamera;
					else if (lineLower == "[famhangarcamera]") readMode = ReadMode.FamilyHangarCamera;
					else if (lineLower == "[hangarmap]") readMode = ReadMode.HangarMap;
					else if (lineLower == "[famhangarmap]") readMode = ReadMode.FamilyHangarMap;
					else if (lineLower == "[skins]") readMode = ReadMode.Skins;
					else if (lineLower == "[shield]") readMode = ReadMode.Shield;
					else if (lineLower == "[hyperspace]") readMode = ReadMode.Hyper;
					else if (lineLower == "[concourse]") readMode = ReadMode.Concourse;
					else if (lineLower == "[hullicon]") readMode = ReadMode.HullIcon;
					else if (lineLower == "[statsprofiles]") readMode = ReadMode.Stats;
					else if (_commandOpt != "")
					{
                        if (lineLower == "[hangarobjects_" + _commandOpt + "_" + _commandIff + "]") readMode = ReadMode.HangarObjects;
                        else if (lineLower == "[hangarcamera_" + _commandOpt + "_" + _commandIff + "]") readMode = ReadMode.HangarCamera;
                        else if (lineLower == "[famhangarcamera_" + _commandOpt + "_" + _commandIff + "]") readMode = ReadMode.FamilyHangarCamera;
                        else if (lineLower == "[hangarmap_" + _commandOpt + "_" + _commandIff + "]") readMode = ReadMode.HangarMap;
                        else if (lineLower == "[famhangarmap_" + _commandOpt + "_" + _commandIff + "]") readMode = ReadMode.FamilyHangarMap;
						else if (lineLower == "[hangarobjects_" + _commandOpt + "]") readMode = ReadMode.HangarObjects;
                        else if (lineLower == "[hangarcamera_" + _commandOpt + "]") readMode = ReadMode.HangarCamera;
                        else if (lineLower == "[famhangarcamera_" + _commandOpt + "]") readMode = ReadMode.FamilyHangarCamera;
                        else if (lineLower == "[hangarmap_" + _commandOpt + "]") readMode = ReadMode.HangarMap;
                        else if (lineLower == "[famhangarmap_" + _commandOpt + "]") readMode = ReadMode.FamilyHangarMap;
                        else _unknown.Add(txtHook.Lines[i]);
                    }
					else _unknown.Add(txtHook.Lines[i]);
				}
				else if (readMode == ReadMode.Backdrop) lstBackdrops.Items.Add(line);
				else if (readMode == ReadMode.Mission) parseMission(line);
				else if (readMode == ReadMode.Sounds) lstSounds.Items.Add(line);
				else if (readMode == ReadMode.Objects)
				{
					if (lineLower.Contains("_hullicon")) parseHullIcon(line);
					else lstObjects.Items.Add(line);
				}
				else if (readMode == ReadMode.HangarObjects) parseHangarObjects(line);
				else if (readMode == ReadMode.HangarCamera) parseHangarCamera(line);
				else if (readMode == ReadMode.FamilyHangarCamera) parseFamilyHangarCamera(line);
				else if (readMode == ReadMode.HangarMap)
				{
					MapEntry entry = new MapEntry();
					if (entry.Parse(line))
						lstMap.Items.Add(entry.ToString());
					else _comments[(int)readMode].Add(txtHook.Lines[i]);
				}
				else if (readMode == ReadMode.FamilyHangarMap)
				{
					MapEntry entry = new MapEntry();
					if (entry.Parse(line))
						lstFamilyMap.Items.Add(entry.ToString());
					else _comments[(int)readMode].Add(txtHook.Lines[i]);
				}
				else if (readMode == ReadMode.Skins) lstSkins.Items.Add(line);
				else if (readMode == ReadMode.Shield) parseShield(line);
				else if (readMode == ReadMode.Hyper) parseHyper(line);
				else if (readMode == ReadMode.Concourse) parseConcourse(line);
				else if (readMode == ReadMode.HullIcon) parseHullIcon(line);
				else if (readMode == ReadMode.Stats) lstStats.Items.Add(line);
				else if (readMode == ReadMode.None && !isPre) _unknown.Add(txtHook.Lines[i]);
            }
        }

        string removeComment(string line)
        {
            if (line.IndexOf(";") != -1)
                line = line.Substring(0, line.IndexOf(";"));
            if (line.IndexOf("#") != -1)
                line = line.Substring(0, line.IndexOf("#"));
            if (line.IndexOf("//") != -1)
                line = line.Substring(0, line.IndexOf("//"));
            return line.Trim();
        }

        void reset()
        {
			lstBackdrops.Items.Clear();
			lstMission.Items.Clear();
			lstSounds.Items.Clear();
			lstObjects.Items.Clear();
			lstSFoils.Items.Clear();
			chkForceHangarSF.Checked = false;
			chkForceHyperLG.Checked = false;
			chkManualSF.Checked = false;
			lstSkins.Items.Clear();
			lstShield.Items.Clear();
			chkSSRecharge.Checked = true;
			optHypGlobal.Checked = true;
			lstHangarObjects.Items.Clear();
			chkShuttle.Checked = true;
			chkShuttleFloor.Checked = false;
            cboShuttleModel.SelectedIndex = 50;
			txtShuttleProfile.Text = "";
			cboShuAnimation.SelectedIndex = 0;
			numShuDistance.Value = 0;
			numShuElevation.Value = 0;
			cboShuttleMarks.SelectedIndex = 0;
			cmdShuttleReset_Click("reset", new EventArgs());
			chkHangarIff.Checked = false;
			chkDroids.Checked = true;
			chkLoadDroid1.Checked = true;
			chkLoadDroid2.Checked = true;
			chkDroidsFloor.Checked = false;
			numDroidsZ.Value = 0;
			numDroid1Z.Value = 0;
			numDroid2Z.Value = 0;
			chkDroid1Update.Checked = true;
			chkDroid2Update.Checked = true;
			chkDroid1Floor.Checked = false;
			chkDroid2Floor.Checked = false;
			cboDroid1Model.SelectedIndex = 311;
			cboDroid2Model.SelectedIndex = 312;
			cboDroid1Markings.SelectedIndex = 0;
			cboDroid2Markings.SelectedIndex = 0;
			txtDroid1Profile.Text = "";
			txtDroid2Profile.Text = "";
			cmdCraneReset_Click("reset", new EventArgs());
			optRoofCraneAxisX.Checked = true;
			numRoofCraneHighOffset.Value = 0;
			numRoofCraneLowOffset.Value = 0;
			cmdPlayerReset_Click("reset", new EventArgs());
			chkPlayerFloor.Checked = false;
			chkFloor.Checked = false;
			numPlayerAnimationElevation.Value = 0;
			lstMap.Items.Clear();
			lstFamilyMap.Items.Clear();
			cmdDefaultCamera_Click("reset", new EventArgs());
			cmdDefaultFamilyCamera_Click("reset", new EventArgs());
			chkConcoursePlanetIndex.Checked = false;
			chkConcoursePlanetX.Checked = false;
			chkConcoursePlanetY.Checked = false;
			lstHullIcon.Items.Clear();
			chkPlayerHull.Checked = false;
			lstCraftText.Items.Clear();
			chkRedAlert.Checked = false;
			chkSkipHyper.Checked = false;
			chkSkipIffMessages.Checked = false;
			chkSkipAllIff.Checked = false;
			for (int i = 0; i < _skipIffs.Length; i++) _skipIffs[i] = false;
			chkForceTurret.Checked = false;
			numTurretH.Value = 0;
			numTurretM.Value = 0;
			numTurretS.Value = 8;
			chkDisableLaser.Checked = false;
			chkDisableWarhead.Checked = false;
			chkDisableCollision.Checked = false;
			lstStats.Items.Clear();
        }

        #region Backdrops
		private void cmdAddBD_Click(object sender, EventArgs e)
		{
			if (_installDirectory != "") opnBackdrop.InitialDirectory = _installDirectory + _res;
			DialogResult res = opnBackdrop.ShowDialog();
			if (res == DialogResult.OK)
				lstBackdrops.Items.Add(opnBackdrop.FileName.Substring(opnBackdrop.FileName.IndexOf(_res)));
		}
		private void cmdRemoveBD_Click(object sender, EventArgs e)
		{
			if (lstBackdrops.SelectedIndex != -1) lstBackdrops.Items.RemoveAt(lstBackdrops.SelectedIndex);
		}
        #endregion Backdrops

        #region MissionTie
        /// <remarks>This also parses S-Foils</remarks>
        void parseMission(string line)
		{
			string[] parts = line.ToLower().Replace(" ", "").Split(',');
			try
			{
				if (parts[0] == "fg")
				{
					int fg = int.Parse(parts[1]);
					if (parts[2] == "markings")
						lstMission.Items.Add(cboFG.Items[fg].ToString() + ",marks," + cboMarkings.Items[int.Parse(parts[3])].ToString());
					else if (parts[2] == "index")
						lstMission.Items.Add(cboFG.Items[fg].ToString() + ",wing," + int.Parse(parts[3]) + "," + cboMarkings.Items[int.Parse(parts[5])].ToString());
					else if (parts[2] == "iff")
						lstMission.Items.Add(cboFG.Items[fg].ToString() + ",iff," + cboIff.Items[int.Parse(parts[3])].ToString());
					else if (parts[2] == "pilotvoice")
						lstMission.Items.Add(cboFG.Items[fg].ToString() + ",pilot," + parts[3]);
					else if (parts[2] == "close_sfoils")
						lstSFoils.Items.Add(cboSFoilFG.Items[fg].ToString() + ",closed");
					else if (parts[2] == "open_landinggears")
						lstSFoils.Items.Add(cboSFoilFG.Items[fg].ToString() + ",open");
					else throw new InvalidDataException();
				}
				else if (parts[0] == "craft")
				{
					int craft = int.Parse(parts[1]);
					if (parts[2] == "name")
						lstCraftText.Items.Add(cboCraftText.Items[craft].ToString() + ",name," + parts[3]);
					else if (parts[2] == "specname")
						lstCraftText.Items.Add(cboCraftText.Items[craft].ToString() + ",species," + parts[3]);
					else if (parts[2] == "pluralname")
						lstCraftText.Items.Add(cboCraftText.Items[craft].ToString() + ",plural," + parts[3]);
					else if (parts[2] == "shortname")
						lstCraftText.Items.Add(cboCraftText.Items[craft].ToString() + ",abbrv," + parts[3]);
					else throw new InvalidDataException();
				}
				else
				{
					parts = parts[0].Split('=');
					if (parts[0] == "closesfoilsandopenlandinggearsbeforeenterhangar")
						chkForceHangarSF.Checked = parts[1] == "1";
					else if (parts[0] == "closelandinggearsbeforeenterhyperspace")
						chkForceHyperLG.Checked = parts[1] == "1";
					else if (parts[0] == "autoclosesfoils")
						chkManualSF.Checked = parts[1] == "0";
					else if (parts[0] == "isredalertenabled")
						chkRedAlert.Checked = parts[1] == "1";
					else if (parts[0] == "skiphyperspacedmessages")
						chkSkipHyper.Checked = parts[1] == "1";
					else if (parts[0] == "skipobjectsmessagesiff")
					{
						chkSkipIffMessages.Checked = (parts[1] != "-1");
						if (parts[1] == "255") chkSkipAllIff.Checked = true;
						else
						{
							chkSkipAllIff.Checked = false;
							_skipIffs[int.Parse(parts[1])] = true;
						}
					}
					else if (parts[0] == "forceplayerinturret")
						chkForceTurret.Checked = parts[1] == "1";
					else if (parts[0] == "forceplayerinturrethours")
						numTurretH.Value = int.Parse(parts[1]);
					else if (parts[0] == "forceplayerinturretminutes")
						numTurretM.Value = int.Parse(parts[1]);
					else if (parts[0] == "forceplayerinturretseconds")
						numTurretS.Value = int.Parse(parts[1]);
					else if (parts[0] == "disableplayerlasershoot")
						chkDisableLaser.Checked = parts[1] == "1";
					else if (parts[0] == "disableplayerwarheadshoot")
						chkDisableWarhead.Checked = parts[1] == "1";
					else if (parts[0] == "iswarheadcollisiondamagesenabled")
						chkDisableCollision.Checked = parts[1] == "0";
					else throw new InvalidDataException();
				}
			}
			catch { _comments[(int)ReadMode.Mission].Add(line); }
		}

		void optMission_CheckedChanged(object sender, EventArgs e)
		{
			cboMarkings.Enabled = optMarkings.Checked | optWingman.Checked;
			numWingman.Enabled = optWingman.Checked;
			cboIff.Enabled = optIff.Checked;
			txtPilot.Enabled = optPilot.Checked;
		}

        private void cboSkipIff_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (cboSkipIff.SelectedIndex == -1) return;

			chkSkipIff.Checked = _skipIffs[cboSkipIff.SelectedIndex];
        }
        private void cboStatType_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (cboStatType.SelectedIndex == -1) return;

			txtStatProfile.Enabled = cboStatMarks.Enabled = (cboStatType.SelectedIndex == 0);
			numStatPercent.Enabled = !txtStatProfile.Enabled;
        }

        private void chkSkipIff_CheckedChanged(object sender, EventArgs e)
        {
			if (cboSkipIff.SelectedIndex == -1) return;

            _skipIffs[cboSkipIff.SelectedIndex] = chkSkipIff.Checked;
        }
        private void chkSkipIffMessages_CheckedChanged(object sender, EventArgs e)
        {
            chkSkipAllIff.Enabled = chkSkipIffMessages.Checked;
			if (!chkSkipIffMessages.Checked)
			{
				chkSkipAllIff.Checked = false;
				cboSkipIff.Enabled = chkSkipIff.Enabled = false;
				for (int i = 0; i < _skipIffs.Length; i++) _skipIffs[i] = false;
			}
			else cboSkipIff.Enabled = chkSkipIff.Enabled = true;
        }
        private void chkSkipAllIff_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSkipAllIff.Enabled)
			{
				cboSkipIff.Enabled = chkSkipIff.Enabled = !chkSkipAllIff.Checked;
			}
        }

        private void cmdAddMiss_Click(object sender, EventArgs e)
		{
			if (cboFG.SelectedIndex == -1 || ((optWingman.Checked || optMarkings.Checked) && cboMarkings.SelectedIndex == -1) || (optIff.Checked && cboIff.SelectedIndex == -1)
				|| (optPilot.Checked && txtPilot.Text == "")) return;

			if (optMarkings.Checked) lstMission.Items.Add(cboFG.Text + ",marks," + cboMarkings.Text);
			else if (optWingman.Checked) lstMission.Items.Add(cboFG.Text + ",wing," + numWingman.Value + "," + cboMarkings.Text);
			else if (optIff.Checked) lstMission.Items.Add(cboFG.Text + ",iff," + cboIff.Text);
			else if (optPilot.Checked) lstMission.Items.Add(cboFG.Text + ",pilot," + txtPilot.Text);
		}
		private void cmdRemoveMiss_Click(object sender, EventArgs e)
		{
			if (lstMission.SelectedIndex == -1) return;
			lstMission.Items.RemoveAt(lstMission.SelectedIndex);
		}
        private void cmdAddCraftText_Click(object sender, EventArgs e)
        {
			if (cboCraftText.SelectedIndex == -1 || (!optCraftText.Checked && !optPluralText.Checked && !optAbbrvText.Checked && !optSpeciesText.Checked) || txtCraftText.Text == "") return;

			if (optCraftText.Checked) lstCraftText.Items.Add(cboCraftText.Text + ",name," + txtCraftText.Text);
            else if (optPluralText.Checked) lstCraftText.Items.Add(cboCraftText.Text + ",plural," + txtCraftText.Text);
            else if (optAbbrvText.Checked) lstCraftText.Items.Add(cboCraftText.Text + ",abbrv," + txtCraftText.Text);
            else if (optSpeciesText.Checked) lstCraftText.Items.Add(cboCraftText.Text + ",species," + txtCraftText.Text);
        }
        private void cmdRemoveTextCraft_Click(object sender, EventArgs e)
        {
			if (lstCraftText.SelectedIndex == -1) return;
			lstCraftText.Items.RemoveAt(lstCraftText.SelectedIndex);
        }
        private void cmdAddStat_Click(object sender, EventArgs e)
        {
			if (cboStatType.SelectedIndex == -1) return;

			if (cboStatType.SelectedIndex == 0 && txtStatProfile.Text.ToLower() != "default" && (chkStatPlayer.Checked || cboStatMarks.SelectedIndex != -1))
			{
				opnObjects.Title = "Select object...";
				var res = opnObjects.ShowDialog();
				if (res == DialogResult.OK)
				{
					string line = Path.GetFileNameWithoutExtension(opnObjects.FileName) + "_fg";
					if (chkStatPlayer.Checked) line += "_player";
					else line += "c_" + cboStatMarks.SelectedIndex;
					line += " = " + txtStatProfile.Text;
					lstStats.Items.Add(line);
				}
			}
			else if (cboStatType.SelectedIndex != 0)
			{
				string line = cboStatType.Text + "Percent = " + numStatPercent.Value;
				if (chkStatPlayer.Checked) line = "Player" + line;
				lstStats.Items.Add(line);
			}
        }
        private void cmdRemoveStat_Click(object sender, EventArgs e)
        {
			if (lstStats.SelectedIndex == -1) return;
			lstStats.Items.RemoveAt(lstStats.SelectedIndex);
        }

        bool useMissionSettings
		{
			get
			{
				if (chkSkipIffMessages.Checked)
				{
					for (int i = 0; i < _skipIffs.Length; i++) { if (_skipIffs[i]) return true; }
					if (chkSkipAllIff.Checked) return true;
					chkSkipIffMessages.Checked = false;
				}
                if (chkRedAlert.Checked || chkSkipHyper.Checked
                    || chkForceTurret.Checked || numTurretH.Value != 0 || numTurretM.Value != 0 || numTurretS.Value != 8
                    || chkDisableLaser.Checked || chkDisableWarhead.Checked || chkDisableCollision.Checked) return true;
                return false;
			}
		}
        #endregion

        #region Sounds
        private void cmdAddSounds_Click(object sender, EventArgs e)
		{
			if (_installDirectory != "") opnSounds.InitialDirectory = _installDirectory + _wave;
			opnSounds.Title = "Select original sound...";
			DialogResult res = opnSounds.ShowDialog();
			if (res == DialogResult.OK)
			{
				string line = opnSounds.FileName.Substring(opnSounds.FileName.IndexOf(_wave)) + " = ";
				opnSounds.Title = "Select new sound...";
				res = opnSounds.ShowDialog();
				if (res == DialogResult.OK)
					lstSounds.Items.Add(line + opnSounds.FileName.Substring(opnSounds.FileName.IndexOf(_wave) + 1));
			}
		}
		private void cmdRemoveSounds_Click(object sender, EventArgs e)
		{
			if (lstSounds.SelectedIndex != -1) lstSounds.Items.RemoveAt(lstSounds.SelectedIndex);
		}
        #endregion

        #region Objects
        private void chkWeaponProfile_CheckedChanged(object sender, EventArgs e)
        {
			numWeaponProfileMarking.Enabled = chkWeaponProfile.Checked;
        }

        private void cmdAddObjects_Click(object sender, EventArgs e)
		{
            if (_installDirectory != "") opnObjects.InitialDirectory = _installDirectory + _fm;
            if (optCraft.Checked)
			{
				opnObjects.Title = "Select original object...";
				DialogResult res = opnObjects.ShowDialog();
				if (res == DialogResult.OK)
				{
					string line = opnObjects.FileName.Substring(opnObjects.FileName.IndexOf(_fm)) + " = ";
					opnObjects.Title = "Select new object...";
					res = opnObjects.ShowDialog();
					if (res == DialogResult.OK)
						lstObjects.Items.Add(line + opnObjects.FileName.Substring(opnObjects.FileName.IndexOf(_fm)));
				}
			}
			else if (optFGProfile.Checked && txtProfile.Text != "" && txtProfile.Text.ToLower() != "default")
			{
				string line = "ObjectProfile_fg_" + cboProfileFG.SelectedIndex + " = " + txtProfile.Text;
				lstObjects.Items.Add(line);
			}
			else if (optCraftProfile.Checked && txtProfile.Text != "" && txtProfile.Text.ToLower() != "default")
			{
                opnObjects.Title = "Select object...";
                DialogResult res = opnObjects.ShowDialog();
                if (res == DialogResult.OK)
                {
                    string line = "ObjectProfile_" + Path.GetFileNameWithoutExtension(opnObjects.FileName) + " = " + txtProfile.Text;
					lstObjects.Items.Add(line);
                }
            }
            else if (optCraftCockpit.Checked && txtProfile.Text != "" && txtProfile.Text.ToLower() != "default")
            {
                opnObjects.Title = "Select object...";
                DialogResult res = opnObjects.ShowDialog();
                if (res == DialogResult.OK)
                {
                    string line = "FlightModels\\" + Path.GetFileNameWithoutExtension(opnObjects.FileName) + "_CockpitPovProfile = " + txtProfile.Text;
                    lstObjects.Items.Add(line);
                }
            }
			else if (optCockpit.Checked && txtProfile.Text != "" && txtProfile.Text.ToLower() != "default")
			{
                string line = "CockpitPovProfile = " + txtProfile.Text;
                lstObjects.Items.Add(line);
            }
			else if (optWeaponProfile.Checked && txtProfile.Text != "" && (txtProfile.Text.ToLower() != "default" || chkWeaponProfile.Checked))
			{
                opnObjects.Title = "Select object...";
                DialogResult res = opnObjects.ShowDialog();
                if (res == DialogResult.OK)
                {
                    string line = "ObjectProfile_" + Path.GetFileNameWithoutExtension(opnObjects.FileName) + "_" + numWeaponModel.Value
						+  " = " + txtProfile.Text + (chkWeaponProfile.Checked ? "_" + numWeaponProfileMarking.Value : "");
                    lstObjects.Items.Add(line);
                }
            }
        }
		private void cmdRemoveObjects_Click(object sender, EventArgs e)
		{
			if (lstObjects.SelectedIndex != -1) lstObjects.Items.RemoveAt(lstObjects.SelectedIndex);
		}

        private void objectsOpt_CheckedChanged(object sender, EventArgs e)
		{
			cboProfileFG.Enabled = optFGProfile.Checked;
			txtProfile.Enabled = (optFGProfile.Checked | optCraftProfile.Checked | optCraftCockpit.Checked | optCockpit.Checked | optWeaponProfile.Checked);
			chkWeaponProfile.Enabled = numWeaponModel.Enabled = optWeaponProfile.Checked;
			numWeaponProfileMarking.Enabled = optWeaponProfile.Checked && chkWeaponProfile.Checked;
		}
        #endregion

        #region Hangars
        void parseHangarCamera(string line)
		{
            string[] parts = line.ToLower().Replace(" ", "").Split('=');
            try
			{
                int view;
                if (parts[0].StartsWith("key1")) view = 0;
                else if (parts[0].StartsWith("key2")) view = 1;
                else if (parts[0].StartsWith("key3")) view = 2;
                else if (parts[0].StartsWith("key6")) view = 3;
                else if (parts[0].StartsWith("key9")) view = 4;
                else throw new InvalidDataException();

                int camera;
                if (parts[0].IndexOf("_x") != -1) camera = 0;
                else if (parts[0].IndexOf("_y") != -1) camera = 1;
                else if (parts[0].IndexOf("_z") != -1) camera = 2;
                else throw new InvalidDataException();

                _cameras[view, camera] = int.Parse(parts[1]);
			}
			catch { _comments[(int)ReadMode.HangarCamera].Add(line); }
        }
		void parseFamilyHangarCamera(string line)
		{
            string[] parts = line.ToLower().Replace(" ", "").Split('=');
            try
			{
                int view;
                if (parts[0].StartsWith("famkey1")) view = 0;
                else if (parts[0].StartsWith("famkey2")) view = 1;
                else if (parts[0].StartsWith("famkey3")) view = 2;
                else if (parts[0].StartsWith("famkey6")) view = 3;
                else if (parts[0].StartsWith("famkey7")) view = 4;
                else if (parts[0].StartsWith("famkey8")) view = 5;
                else if (parts[0].StartsWith("famkey9")) view = 6;
                else throw new InvalidDataException();

                int camera;
                if (parts[0].IndexOf("_x") != -1) camera = 0;
                else if (parts[0].IndexOf("_y") != -1) camera = 1;
                else if (parts[0].IndexOf("_z") != -1) camera = 2;
                else throw new InvalidDataException();

                _familyCameras[view, camera] = int.Parse(parts[1]);
			}
			catch { _comments[(int)ReadMode.FamilyHangarCamera].Add(line); }
        }
		void parseHangarObjects(string line)
		{
			string[] parts = line.ToLower().Replace(" ", "").Split('=');
			try
			{
				if (parts[0] == "loadshuttle") chkShuttle.Checked = (parts[1] == "1");
				else if (parts[0] == "shuttlemodelindex") cboShuttleModel.SelectedIndex = int.Parse(parts[1]);
				else if (parts[0] == "shuttlemarkings") cboShuttleMarks.SelectedIndex = int.Parse(parts[1]);
				else if (parts[0] == "shuttleobjectprofile") txtShuttleProfile.Text = parts[1];
				else if (parts[0] == "shuttlepositionx") numShuttlePositionX.Value = int.Parse(parts[1]);
				else if (parts[0] == "shuttlepositiony") numShuttlePositionY.Value = int.Parse(parts[1]);
				else if (parts[0] == "shuttlepositionz") numShuttlePositionZ.Value = int.Parse(parts[1]);
				else if (parts[0] == "shuttleorientation") numShuttleOrientation.Value = int.Parse(parts[1]);
				else if (parts[0] == "isshuttlefloorinverted") chkShuttleFloor.Checked = (parts[1] != "0");
				else if (parts[0] == "shuttleanimation")
					try { cboShuAnimation.SelectedIndex = (int)Enum.Parse(typeof(ShuttleAnimation), parts[1], true); }
					catch { MessageBox.Show("Error reading ShuttleAnimation, using default.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
				else if (parts[0] == "shuttleanimationstraightline") numShuDistance.Value = int.Parse(parts[1]);
				else if (parts[0] == "shuttleanimationelevation") numShuElevation.Value = int.Parse(parts[1]);

				else if (parts[0] == "loaddroids") chkDroids.Checked = (parts[1] == "1");
				else if (parts[0] == "loaddroid1") chkLoadDroid1.Checked = (parts[1] == "1");
                else if (parts[0] == "loaddroid2") chkLoadDroid2.Checked = (parts[1] == "1");
                else if (parts[0] == "droidspositionz") numDroidsZ.Value = int.Parse(parts[1]);
				else if (parts[0] == "droid1positionz") numDroid1Z.Value = int.Parse(parts[1]);
				else if (parts[0] == "droid2positionz") numDroid2Z.Value = int.Parse(parts[1]);
				else if (parts[0] == "isdroidsfloorinverted") chkDroidsFloor.Checked = (parts[1] != "0");
                else if (parts[0] == "isdroid1floorinverted") chkDroid1Floor.Checked = (parts[1] != "0");
                else if (parts[0] == "isdroid2floorinverted") chkDroid2Floor.Checked = (parts[1] != "0");
                else if (parts[0] == "droid1update") chkDroid1Update.Checked = (parts[1] != "0");
				else if (parts[0] == "droid2update") chkDroid2Update.Checked = (parts[1] != "0");
				else if (parts[0] == "droid1modelindex") cboDroid1Model.SelectedIndex = int.Parse(parts[1]);
				else if (parts[0] == "droid1markings") cboDroid1Markings.SelectedIndex = int.Parse(parts[1]);
				else if (parts[0] == "droid1objectprofile") txtDroid1Profile.Text = parts[1];
                else if (parts[0] == "droid2modelindex") cboDroid2Model.SelectedIndex = int.Parse(parts[1]);
                else if (parts[0] == "droid2markings") cboDroid2Markings.SelectedIndex = int.Parse(parts[1]);
                else if (parts[0] == "droid2objectprofile") txtDroid2Profile.Text = parts[1];

                else if (parts[0] == "hangarroofcranepositionx") numRoofCranePositionX.Value = int.Parse(parts[1]);
				else if (parts[0] == "hangarroofcranepositiony") numRoofCranePositionY.Value = int.Parse(parts[1]);
				else if (parts[0] == "hangarroofcranepositionz") numRoofCranePositionZ.Value = int.Parse(parts[1]);
				else if (parts[0] == "hangarroofcraneaxis")
				{
					if (int.Parse(parts[1]) == 1) optRoofCraneAxisY.Checked = true;
					else if (int.Parse(parts[1]) == 2) optRoofCraneAxisZ.Checked = true;
				}
				else if (parts[0] == "hangarroofcranelowoffset") numRoofCraneLowOffset.Value = int.Parse(parts[1]);
				else if (parts[0] == "hangarroofcranehighoffset") numRoofCraneHighOffset.Value = int.Parse(parts[1]);
				else if (parts[0] == "ishangarfloorinverted") chkFloor.Checked = (parts[1] != "0");
				// HangarFloorInvertedHeight
				else if (parts[0] == "hangariff")
				{
					if (int.Parse(parts[1]) != -1)
					{
						chkHangarIff.Checked = true;
						try { cboHangarIff.SelectedIndex = int.Parse(parts[1]); }
						catch
						{
							chkHangarIff.Checked = false;
							MessageBox.Show("Error reading HangarIff, using default.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}
				// DrawShadows (IsHangarFloorInverted ? 0 : 1)

				else if (parts[0] == "playeranimationelevation") numPlayerAnimationElevation.Value = int.Parse(parts[1]);
				// PlayerAnimationInvertedElevation (PlayerAnimationElevation)
				// PlayerAnimationStraightLine
				else if (parts[0] == "playeroffsetx") numPlayerX.Value = int.Parse(parts[1]);
				else if (parts[0] == "playeroffsety") numPlayerY.Value = int.Parse(parts[1]);
				else if (parts[0] == "playeroffsetz") numPlayerZ.Value = int.Parse(parts[1]);
				// PlayerInvertedOffsetX
				// PlayerInvertedOffsetY
				// PlayerInvertedOffsetZ
				// PlayerModelIndices (null)
				// PlayerOffsetsX (null)
				// PlayerOffsetsY (null)
				// PlayerOffsetsZ (null)
				// ^ comma-delim, all four arrays must be the same size
				else if (parts[0] == "isplayerfloorinverted") chkPlayerFloor.Checked = (parts[1] != "0");
				// PlayerFloorInvertedModelIndicies (null)

				// LightColorIntensity (192)
				// LightColorRgb (FFFFFF)

				else if (parts[0].StartsWith(_fm, StringComparison.InvariantCultureIgnoreCase)) lstHangarObjects.Items.Add(parts[1]);
				else throw new InvalidDataException();
            }
			catch { _comments[(int)ReadMode.HangarObjects].Add(line); }
        }

		private void cboCamera_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboCamera.SelectedIndex == -1) return;
			_loading = true;
			numCameraX.Value = _cameras[cboCamera.SelectedIndex, 0];
			numCameraY.Value = _cameras[cboCamera.SelectedIndex, 1];
			numCameraZ.Value = _cameras[cboCamera.SelectedIndex, 2];
			_loading = false;
		}
		private void cboFamilyCamera_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboFamilyCamera.SelectedIndex == -1) return;
			_loading = true;
			numFamilyCameraX.Value = _familyCameras[cboFamilyCamera.SelectedIndex, 0];
			numFamilyCameraY.Value = _familyCameras[cboFamilyCamera.SelectedIndex, 1];
			numFamilyCameraZ.Value = _familyCameras[cboFamilyCamera.SelectedIndex, 2];
			_loading = false;
		}

		private void chkFamGrounded_CheckedChanged(object sender, EventArgs e)
		{
			numFamPosZ.Enabled = !chkFamGrounded.Checked;
		}
		private void chkFamMarks_CheckedChanged(object sender, EventArgs e)
		{
			cboFamMapMarkings.Enabled = chkFamMarks.Checked;
		}
		private void chkGrounded_CheckedChanged(object sender, EventArgs e)
		{
			numPosZ.Enabled = !chkGrounded.Checked;
		}
		private void chkHangarIff_CheckedChanged(object sender, EventArgs e)
		{
			cboHangarIff.Enabled = chkHangarIff.Checked;
			if (chkHangarIff.Checked && cboHangarIff.SelectedIndex == -1) cboHangarIff.SelectedIndex = 0;
		}
		private void chkMarks_CheckedChanged(object sender, EventArgs e)
		{
			cboMapMarkings.Enabled = chkMarks.Checked;
		}
        private void chkShuttle_CheckedChanged(object sender, EventArgs e)
        {
            pnlShuttle.Enabled = chkShuttle.Checked;
        }
        private void chkLoadDroid1_CheckedChanged(object sender, EventArgs e)
        {
            grpDroid1.Enabled = chkLoadDroid1.Checked;
        }
        private void chkLoadDroid2_CheckedChanged(object sender, EventArgs e)
        {
            grpDroid2.Enabled = chkLoadDroid2.Checked;
        }
        private void chkDroids_CheckedChanged(object sender, EventArgs e)
        {
            pnlDroids.Enabled = chkDroids.Checked;
        }

        private void cmdAddFamMap_Click(object sender, EventArgs e)
		{
			MapEntry entry = new MapEntry
			{
				ModelIndex = cboFamMapIndex.SelectedIndex,
				Markings = (byte)cboFamMapMarkings.SelectedIndex,
				PositionX = (int)numFamPosX.Value,
				PositionY = (int)numFamPosY.Value,
				PositionZ = (int)numFamPosZ.Value,
				IsGrounded = chkFamGrounded.Checked,
				HeadingXY = (int)numFamHeadingXY.Value,
				HeadingZ = (int)numFamHeadingZ.Value
			};
			lstFamilyMap.Items.Add(entry.ToString());
		}
		private void cmdAddHangar_Click(object sender, EventArgs e)
		{
			if (_installDirectory != "") opnObjects.InitialDirectory = _installDirectory + _fm;
			opnObjects.Title = "Select original object...";
			DialogResult res = opnObjects.ShowDialog();
			if (res == DialogResult.OK)
			{
				string line = opnObjects.FileName.Substring(opnObjects.FileName.IndexOf(_fm)) + " = ";
				opnObjects.Title = "Select new object...";
				res = opnObjects.ShowDialog();
				if (res == DialogResult.OK)
					lstHangarObjects.Items.Add(line + opnObjects.FileName.Substring(opnObjects.FileName.IndexOf(_fm)));
			}
		}
		private void cmdAddMap_Click(object sender, EventArgs e)
		{
			MapEntry entry = new MapEntry
			{
				ModelIndex = cboMapIndex.SelectedIndex,
				Markings = (byte)cboMapMarkings.SelectedIndex,
				PositionX = (int)numPosX.Value,
				PositionY = (int)numPosY.Value,
				PositionZ = (int)numPosZ.Value,
				IsGrounded = chkGrounded.Checked,
				HeadingXY = (int)numHeadingXY.Value,
				HeadingZ = (int)numHeadingZ.Value
			};
			lstMap.Items.Add(entry.ToString());
		}
		private void cmdCraneReset_Click(object sender, EventArgs e)
		{
			numRoofCranePositionX.Value = _defaultRoofCranePosition[0];
			numRoofCranePositionY.Value = _defaultRoofCranePosition[1];
			numRoofCranePositionZ.Value = _defaultRoofCranePosition[2];
		}
		private void cmdDefaultCamera_Click(object sender, EventArgs e)
		{
			numCameraX.Value = _defaultCameras[cboCamera.SelectedIndex, 0];
            numCameraY.Value = _defaultCameras[cboCamera.SelectedIndex, 1];
            numCameraZ.Value = _defaultCameras[cboCamera.SelectedIndex, 2];
        }
		private void cmdDefaultFamilyCamera_Click(object sender, EventArgs e)
		{
            numFamilyCameraX.Value = _defaultFamilyCameras[cboFamilyCamera.SelectedIndex, 0];
            numFamilyCameraY.Value = _defaultFamilyCameras[cboFamilyCamera.SelectedIndex, 1];
            numFamilyCameraZ.Value = _defaultFamilyCameras[cboFamilyCamera.SelectedIndex, 2];
		}
		private void cmdPlayerReset_Click(object sender, EventArgs e)
		{
			numPlayerX.Value = 0;
			numPlayerY.Value = 0;
			numPlayerZ.Value = 0;
		}
		private void cmdRemoveFamMap_Click(object sender, EventArgs e)
		{
			if (lstFamilyMap.SelectedIndex != -1)
			{
				if (lstFamilyMap.Items.Count == 4)    // warn here only when initially dropping below 4
				{
					DialogResult res = MessageBox.Show("Family Hangar Map requires at least 4 line items to be saved. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					if (res == DialogResult.No) return;
				}

				lstFamilyMap.Items.RemoveAt(lstFamilyMap.SelectedIndex);
			}
		}
		private void cmdRemoveHangar_Click(object sender, EventArgs e)
		{
			if (lstHangarObjects.SelectedIndex != -1) lstHangarObjects.Items.RemoveAt(lstHangarObjects.SelectedIndex);
		}
		private void cmdRemoveMap_Click(object sender, EventArgs e)
		{
			if (lstMap.SelectedIndex != -1)
			{
				if (lstMap.Items.Count == 4)	// warn here only when initially dropping below 4
				{
					DialogResult res = MessageBox.Show("Hangar Map requires at least 4 line items to be saved. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					if (res == DialogResult.No) return;
				}

				lstMap.Items.RemoveAt(lstMap.SelectedIndex);
			}
		}
		private void cmdShuttleReset_Click(object sender, EventArgs e)
		{
			numShuttlePositionX.Value = _defaultShuttlePosition[0];
			numShuttlePositionY.Value = _defaultShuttlePosition[1];
			numShuttlePositionZ.Value = _defaultShuttlePosition[2];
			numShuttleOrientation.Value = _defaultShuttlePosition[3];
		}

		private void numCameraX_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading) _cameras[cboCamera.SelectedIndex, 0] = (int)numCameraX.Value;
		}
		private void numCameraY_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading) _cameras[cboCamera.SelectedIndex, 1] = (int)numCameraY.Value;
		}
		private void numCameraZ_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading) _cameras[cboCamera.SelectedIndex, 2] = (int)numCameraZ.Value;
		}
		private void numFamilyCameraX_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading) _familyCameras[cboFamilyCamera.SelectedIndex, 0] = (int)numFamilyCameraX.Value;
		}
		private void numFamilyCameraY_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading) _familyCameras[cboFamilyCamera.SelectedIndex, 1] = (int)numFamilyCameraY.Value;
		}
		private void numFamilyCameraZ_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading) _familyCameras[cboFamilyCamera.SelectedIndex, 2] = (int)numFamilyCameraZ.Value;
		}

		bool useHangarCamera
		{
			get
			{
				for (int i = 0; i < 5; i++)
					for (int j = 0; j < 3; j++)
						if (_cameras[i, j] != _defaultCameras[i, j]) return true;
				return false;
			}
		}
		bool useFamilyHangarCamera
		{
			get
			{
				for (int i = 0; i < 7; i++)
					for (int j = 0; j < 3; j++)
						if (_familyCameras[i, j] != _defaultFamilyCameras[i, j]) return true;
				return false;
			}
		}
		bool useHangarObjects
		{
			get
			{
				return (lstHangarObjects.Items.Count > 0) ||
					!chkShuttle.Checked || (cboShuttleModel.SelectedIndex != 50) || (cboShuttleMarks.SelectedIndex != 0) || txtShuttleProfile.Text != "" ||
					(numShuttlePositionX.Value != _defaultShuttlePosition[0]) || (numShuttlePositionY.Value != _defaultShuttlePosition[1]) || (numShuttlePositionZ.Value != _defaultShuttlePosition[2]) ||
					(numShuttleOrientation.Value != _defaultShuttlePosition[3]) || chkShuttleFloor.Checked || (cboShuAnimation.SelectedIndex != 0) || (numShuDistance.Value != 0) || numShuElevation.Value != 0 ||
					!chkDroids.Checked || !chkLoadDroid1.Checked || !chkLoadDroid2.Checked || (numDroidsZ.Value != 0) || (numDroid1Z.Value != numDroidsZ.Value) || (numDroid2Z.Value != numDroidsZ.Value) ||
					chkDroidsFloor.Checked || chkDroid1Floor.Checked || chkDroid2Floor.Checked || !chkDroid1Update.Checked || !chkDroid2Update.Checked ||
					cboDroid1Markings.SelectedIndex != 0 || cboDroid2Markings.SelectedIndex != 0 || cboDroid1Model.SelectedIndex != 311 || cboDroid2Model.SelectedIndex != 312 ||
					txtDroid1Profile.Text != "" || txtDroid2Profile.Text != "" ||
					(numRoofCranePositionX.Value != _defaultRoofCranePosition[0]) || (numRoofCranePositionY.Value != _defaultRoofCranePosition[1]) || (numRoofCranePositionZ.Value != _defaultRoofCranePosition[2]) ||
					!optRoofCraneAxisX.Checked || (numRoofCraneLowOffset.Value != 0) || (numRoofCraneHighOffset.Value != 0) ||
					chkFloor.Checked || chkHangarIff.Checked ||
					(numPlayerAnimationElevation.Value != 0) || (numPlayerX.Value != 0) || (numPlayerY.Value != 0) || (numPlayerZ.Value != 0) || chkPlayerFloor.Checked;
			}
		}
		bool useHangarMap {  get { return lstMap.Items.Count >= 4; } }
		bool useFamilyHangarMap { get { return lstFamilyMap.Items.Count >= 4; } }
		#endregion

		#region S-Foils
		private void cmdAddSFoils_Click(object sender, EventArgs e)
		{
			if (cboSFoilFG.SelectedIndex == -1) return;

			if (chkCloseSF.Checked) lstSFoils.Items.Add(cboSFoilFG.Text + ",closed");
			if (chkOpenLG.Checked) lstSFoils.Items.Add(cboSFoilFG.Text + ",open");
		}
		private void cmdRemoveSFoils_Click(object sender, EventArgs e)
		{
			if (lstSFoils.SelectedIndex == -1) return;
			lstSFoils.Items.RemoveAt(lstSFoils.SelectedIndex);
		}

        bool useSFoils => (lstSFoils.Items.Count > 0 || chkForceHangarSF.Checked || chkForceHyperLG.Checked || chkManualSF.Checked);
        #endregion

        #region Skins
        private void chkDefaultSkin_CheckedChanged(object sender, EventArgs e)
		{
			txtSkin.Enabled = !chkDefaultSkin.Checked;
		}
		private void chkSkinMarks_CheckedChanged(object sender, EventArgs e)
		{
			cboSkinMarks.Enabled = chkSkinMarks.Checked;
		}

		private void cmdAddSkin_Click(object sender, EventArgs e)
		{
			if (_installDirectory != "") opnObjects.InitialDirectory = _installDirectory + _fm;
			opnObjects.Title = "Select affected OPT...";
			DialogResult res = opnObjects.ShowDialog();
			if (res == DialogResult.OK)
			{
				string line = Path.GetFileNameWithoutExtension(opnObjects.FileName) + (chkSkinMarks.Checked ? "_fgc_" + cboSkinMarks.SelectedIndex : "") + " = "
					+ (chkDefaultSkin.Checked ? "Default" + (chkSkinMarks.Checked ? "_" + cboSkinMarks.SelectedIndex : "") : txtSkin.Text);
				lstSkins.Items.Add(line);
			}
		}
		private void cmdAppendSkin_Click(object sender, EventArgs e)
		{
			if (lstSkins.SelectedIndex == -1) return;
			string line = lstSkins.SelectedItem.ToString();
			line += ", " + (chkDefaultSkin.Checked ? "Default" + (chkSkinMarks.Checked ? "_" + cboSkinMarks.SelectedIndex : "") : txtSkin.Text);
			lstSkins.Items[lstSkins.SelectedIndex] = line;
		}
		private void cmdRemoveSkin_Click(object sender, EventArgs e)
		{
			if (lstSkins.SelectedIndex != -1) lstSkins.Items.RemoveAt(lstSkins.SelectedIndex);
		}
		#endregion Skins

		#region Shield
		void parseShield(string line)
		{
			string[] parts;
			try
			{
				if (line.StartsWith("IsShieldRechargeForStarshipsEnabled", StringComparison.InvariantCultureIgnoreCase))
				{
					parts = line.Split('=');
					if (parts.Length > 1 && int.Parse(parts[1]) == 0) chkSSRecharge.Checked = false;
					return;
				}

				parts = line.ToLower().Split(',');
				bool perGen = (parts[1] == "1");
				int rate = (perGen ? int.Parse(parts[2]) : int.Parse(parts[3]));
				lstShield.Items.Add(Strings.CraftType[int.Parse(parts[0])] + " = " + rate + (perGen ? " per" : ""));
			}
			catch { _comments[(int)ReadMode.Shield].Add(line); }
		}

		private void cmdAddShield_Click(object sender, EventArgs e)
		{
			if (cboShield.SelectedIndex < 1) return;
			string line = cboShield.Text + " = " + Math.Round(numShieldRate.Value) + (chkShieldGen.Checked ? " per" : "");
			lstShield.Items.Add(line);
		}
		private void cmdRemoveShield_Click(object sender, EventArgs e)
		{
			if (lstShield.SelectedIndex != -1) lstShield.Items.RemoveAt(lstShield.SelectedIndex);
		}
        #endregion

        #region Hyper
		void parseHyper(string line)
		{
            string[] parts = line.ToLower().Replace(" ", "").Split('=');
			try
			{
				if (parts[0] == "shorthyperspaceeffect" && parts.Length == 2)
				{
					int value = int.Parse(parts[1]);
					if (value == -1) optHypGlobal.Checked = true;
					else if (value == 0) optHypNormal.Checked = true;
					else if (value == 1) optHypEnabled.Checked = true;
					else throw new InvalidDataException();
				}
                else throw new InvalidDataException();
            }
			catch { _comments[(int)ReadMode.Hyper].Add(line);}
        }
        #endregion

        #region Concourse
		void parseConcourse(string line)
		{
            string[] parts = line.ToLower().Replace(" ", "").Split('=');
			
			try
			{
                if (parts.Length < 2) throw new InvalidDataException();

                if (parts[0] == "frontplanetindex")
				{
					int value = int.Parse(parts[1]);
					chkConcoursePlanetIndex.Checked = (value != -1);
					if (chkConcoursePlanetIndex.Checked) numConcoursePlanetIndex.Value = value;
				}
				else if (parts[0] == "frontplanetpositionx")
				{
					int value = int.Parse(parts[1]);
					chkConcoursePlanetX.Checked = (value != -1);
					if (chkConcoursePlanetX.Checked) numConcoursePlanetX.Value = value;
				}
				else if (parts[0] == "frontplanetpositiony")
				{
					int value = int.Parse(parts[1]);
					chkConcoursePlanetY.Checked = (value != -1);
					if (chkConcoursePlanetY.Checked) numConcoursePlanetY.Value = value;
				}
				else throw new InvalidDataException();
			}
			catch { _comments[(int)ReadMode.Concourse].Add(line); }
        }

        private void chkConcoursePlanetIndex_CheckedChanged(object sender, EventArgs e)
        {
            numConcoursePlanetIndex.Enabled = chkConcoursePlanetIndex.Checked;
        }
        private void chkConcoursePlanetX_CheckedChanged(object sender, EventArgs e)
        {
            numConcoursePlanetX.Enabled = chkConcoursePlanetX.Checked;
        }
        private void chkConcoursePlanetY_CheckedChanged(object sender, EventArgs e)
        {
            numConcoursePlanetY.Enabled = chkConcoursePlanetY.Checked;
        }
        #endregion

        #region HullIcon
		void parseHullIcon(string line)
		{
            string[] parts = line.ToLower().Replace(" ", "").Split('=');
			try
			{
				if (parts.Length < 2) throw new InvalidDataException();

				if (parts[0] == "playerhullicon")
				{
					int value = int.Parse(parts[1]);
					chkPlayerHull.Checked = value > 0;
					if (chkPlayerHull.Checked) numPlayerHull.Value = value;
				}
				else lstHullIcon.Items.Add(line);
			}
			catch { _comments[(int)ReadMode.HullIcon].Add(line); }
        }

        private void chkPlayerHull_CheckedChanged(object sender, EventArgs e)
        {
            numPlayerHull.Enabled = chkPlayerHull.Checked;
        }

        private void cmdHullAdd_Click(object sender, EventArgs e)
        {
			if (numHullIcon.Value == 0)
			{
				MessageBox.Show("No Hull Icon ID set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

            if (_installDirectory != "") opnObjects.InitialDirectory = _installDirectory + _fm;
            opnObjects.Title = "Select object...";
            DialogResult res = opnObjects.ShowDialog();
            if (res == DialogResult.OK)
            {
                string line = "FlightModels\\" + Path.GetFileNameWithoutExtension(opnObjects.FileName) + "_HullIcon = " + numHullIcon.Value;
                lstHullIcon.Items.Add(line);
            }
        }
        private void cmdHullRemove_Click(object sender, EventArgs e)
        {
            if (lstHullIcon.SelectedIndex != -1) lstHullIcon.Items.RemoveAt(lstHullIcon.SelectedIndex);
        }
        #endregion

        private void cboHook_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (cboHook.SelectedIndex == -1) return;

			for (int i = 0; i < _panels.Length; i++) _panels[i].Visible = (i == cboHook.SelectedIndex);
			lblNotFound.Visible = cboHook.Text.StartsWith("*"); 
        }

        private void cmdCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (lstMap.Items.Count > 0 && lstMap.Items.Count < 4)
			{
				DialogResult res = MessageBox.Show("Hangar Map must have at least 4 entries to be used. Continue without it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (res == DialogResult.No) return;
			}
			if (lstFamilyMap.Items.Count > 0 && lstFamilyMap.Items.Count < 4)
			{
				DialogResult res = MessageBox.Show("Family Hangar Map must have at least 4 entries to be used. Continue without it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (res == DialogResult.No) return;
			}

			if (lstBackdrops.Items.Count == 0) File.Delete(_bdFile);

			if (lstMission.Items.Count == 0 && !useSFoils && lstCraftText.Items.Count == 0 && !useMissionSettings) File.Delete(_missionTxtFile);

			if (lstSounds.Items.Count == 0) File.Delete(_soundFile);

			if (lstObjects.Items.Count == 0 && lstHullIcon.Items.Count == 0) File.Delete(_objFile);

			if (!useHangarObjects)
			{
				File.Delete(_hangarObjectsFile);
                File.Delete(_hangarObjectsFileSI);
                File.Delete(_hangarObjectsFileS);
            }
			if (!useHangarCamera)
			{
				File.Delete(_hangarCameraFile);
                File.Delete(_hangarCameraFileSI);
                File.Delete(_hangarCameraFileS);
            }
			if (!useFamilyHangarCamera)
			{
				File.Delete(_famHangarCameraFile);
                File.Delete(_famHangarCameraFileSI);
                File.Delete(_famHangarCameraFileS);
            }
			if (!useHangarMap)
			{
				File.Delete(_hangarMapFile);
                File.Delete(_hangarMapFileSI);
                File.Delete(_hangarMapFileS);
            }
			if (!useFamilyHangarMap)
			{
				File.Delete(_famHangarMapFile);
				File.Delete(_famHangarMapFileSI);
				File.Delete(_famHangarMapFileS);
			}

            if (lstSkins.Items.Count == 0) File.Delete(_32bppFile);

			if (lstShield.Items.Count == 0 && chkSSRecharge.Checked) File.Delete(_shieldFile);

			if (optHypGlobal.Checked) File.Delete(_hyperFile);

			if (!chkConcoursePlanetIndex.Checked && !chkConcoursePlanetX.Checked && !chkConcoursePlanetY.Checked) File.Delete(_concourseFile);

			if (!chkPlayerHull.Checked) File.Delete(_hullIconFile);

			if (lstStats.Items.Count == 0) File.Delete(_statsFile);

			if (lstBackdrops.Items.Count == 0
				&& lstMission.Items.Count == 0 && lstCraftText.Items.Count == 0 && !useMissionSettings
                && lstSounds.Items.Count == 0
				&& lstObjects.Items.Count == 0 && lstHullIcon.Items.Count == 0
                && !useHangarObjects && !useHangarCamera && !useFamilyHangarCamera && !useHangarMap && !useFamilyHangarMap
				&& !useSFoils
                && lstSkins.Items.Count == 0
                && lstShield.Items.Count == 0 && chkSSRecharge.Checked
                && optHypGlobal.Checked
				&& !chkConcoursePlanetIndex.Checked && !chkConcoursePlanetX.Checked && !chkConcoursePlanetY.Checked
                && !chkPlayerHull.Checked
				&& lstStats.Items.Count == 0)
			{
				File.Delete(_fileName);
				Close();
				return;
			}

			string backup = _fileName.Replace(".ini", "_ini.bak");
			if (File.Exists(_fileName))
			{
				File.Copy(_fileName, backup);
				File.Delete(_fileName);
			}
			StreamWriter sw = null;
			try
			{
				sw = new StreamWriter(_fileName);
                createContents();
                sw.Write(txtHook.Text);
                sw.Flush();
				sw.Close();

				File.Delete(_bdFile);
				File.Delete(_missionTxtFile);
				File.Delete(_soundFile);
				File.Delete(_objFile);
				File.Delete(_hangarObjectsFile);
				File.Delete(_hangarCameraFile);
				File.Delete(_famHangarCameraFile);
				File.Delete(_hangarMapFile);
				File.Delete(_famHangarMapFile);
                File.Delete(_hangarObjectsFileSI);
                File.Delete(_hangarCameraFileSI);
                File.Delete(_famHangarCameraFileSI);
                File.Delete(_hangarMapFileSI);
                File.Delete(_famHangarMapFileSI);
                File.Delete(_hangarObjectsFileS);
                File.Delete(_hangarCameraFileS);
                File.Delete(_famHangarCameraFileS);
                File.Delete(_hangarMapFileS);
                File.Delete(_famHangarMapFileS);
                File.Delete(_32bppFile);
				File.Delete(_shieldFile);
				File.Delete(_hyperFile);
				File.Delete(_concourseFile);
				File.Delete(_hullIconFile);
				File.Delete(_statsFile);
			}
			catch
			{
				sw?.Close();
				if (File.Exists(backup))
				{
					File.Delete(_fileName);
					File.Copy(backup, _fileName);
				}
			}
			File.Delete(backup);
			Close();
		}

        private void txtHook_Enter(object sender, EventArgs e)
        {
			createContents();
			parseContents();
        }
        private void txtHook_Leave(object sender, EventArgs e)
        {
            reset();
            parseContents();

            /* There is a condition that causes this to skip over the first reset() call and go directly to parse() when the user clicks certain controls while
             * the focus is currently in the txt box.
             * It doesn't matter if the only reset is here or in parse(), it'll skip. That causes every ListBox to be filled again without a Clear().
             * Other times it'll be fine, which means reset runs twice.
             * Seems to be an issue with controls that toggle Enabled within reset(); toggling chk, first attempt at a cbo won't drop, etc.
			*/
        }

        struct MapEntry
		{
			// doing it this way so the output processing is in only one spot
			public int ModelIndex;
			public byte Markings;
			public int PositionX;
			public int PositionY;
			public int PositionZ;
			public int HeadingXY;
			public int HeadingZ;
			public bool IsGrounded;
			public string Profile;
			public bool IsFloorInverted;

			public override string ToString()
			{
				return ModelIndex + ", " + (Markings != 0 ? Markings.ToString() + ", " : "")
					+ PositionX + ", " + PositionY + ", " + (IsGrounded ? "0x7FFFFFFF" : PositionZ.ToString())
					+ ", " + HeadingXY + ", " + HeadingZ
					+ (Profile != "" ? ", " + Profile : "")
					+ (IsFloorInverted ? (Profile == "" ? ", Default" : "") + ", 1" : "");
			}

			public bool Parse(string line)
			{
				// line should aleady have comments removed

				int offset = 0;
				string[] parts = line.Replace(" ", "").Split(',');
				if (parts.Length >= 7) offset = 1;
				else if (parts.Length != 6) return false;

				ModelIndex = parseInt32(parts[0]);
				if (offset != 0) Markings = Convert.ToByte(parseInt32(parts[1]) & 0xFF);
				else Markings = 0;
				PositionX = parseInt32(parts[1 + offset]);
				PositionY = parseInt32(parts[2 + offset]);
				IsGrounded = (parts[3 + offset].ToLower() == "0x7fffffff");
				if (!IsGrounded) PositionZ = parseInt32(parts[3 + offset]);
				HeadingXY = parseInt32(parts[4 + offset]);
				HeadingZ = parseInt32(parts[5 + offset]);
				if (parts.Length >= 8) Profile = parts[7];
				else Profile = "";
				if (parts.Length == 9) IsFloorInverted = (parts[8] == "1");

				return true;
			}

			private int parseInt32(string token)
			{
				// Using this because Convert.ToInt32 was throwing an exception on signed integers.
				token = token.Trim();
                if (token.StartsWith("0x") && int.TryParse(token.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out int result))
                    return result;
                if (int.TryParse(token, System.Globalization.NumberStyles.Integer, null, out result))
					return result;
				int.TryParse(token, System.Globalization.NumberStyles.HexNumber, null, out result);
				return result;
			}
		}
    }
}
