/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2026 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.17+
 *
 * CHANGELOG
 * [NEW] Stats modifiers: SpeedIncrement, SpeedDecrement. MapIcon rect. CraftStats all markings Profile.
 * [UPD] ExplosionDamage renamed to CriticalDamageThreshold per hook updates
 * [UPD] major refactor
 * [DEL] removed reading hangar command opt sections for now (thought it's still calc'd) as I was importing it wrong anyway and never saved
 * [FIX] typo'd FrontPlanetPositionY in Concourse, and "Hull" in SpecRci
 * v1.17, 250215
 * [NEW] Mission: CanShootThroughtShieldOnHardDifficulty, IsMissionRanksModifierEnabled, SkipProjectilesProximityCheck
 * [NEW #107] TargetCraftKey tab under Mission_Tie
 * [NEW #103] MissionTie: SpecRci
 * [NEW] WeaponRate: Recharge/DechargeRatePercent
 * [NEW] Shields: IsShieldStrengthForStarfighterDoubled
 * v1.16.0.1, 241014
 * [FIX #109] Was restoring backup after a save due to deleting empty filenames
 * v1.16, 241013
 * [UPD] ArrDep renames
 * [FIX] FamilyCamera controls weren't doing anything
 * [FIX] reset() called in ctor primarily so Camera and FamilyCamera init properly
 * [NEW] WeaponRate: EnergyTransferRatePenalty, ..WeaponLimit, ..ShieldLimit, MaxTorpedoCountPerPass, ...PerTarget, WarheadTypeCount
 * v1.15.8, 240601
 * [FIX #101] Cleanup in v1.15 broke File.Delete, exception for empty string isn't just 2.1 and older per the documentation...
 * v1.15.6, 240314
 * [ADD] WeaponRates hook support
 * v1.15.5, 231222
 * [UPD] Sounds: Interdictor
 * [UPD] 32bpp: skin opacity
 * v1.15.2, 231027
 * [UPD] Changes due to Arr/Dep Method1
 * v1.15, 230923
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
 * [ADD] HangarObjects: ShuttleAnimationElevation, ShuttleObjectProfile, LoadDroid#, Droid#ModelIndex, Droid#Markings, Droid#ObjectProfile, IsDroid#FloorInverted,
 *         DrawShadows, LightColorIntensity, LightColorRgb, HangarFloorInvertedHeight, PlayerAnimationStraightLine, PlayerAnimationInvertedElevation, PlayerInvertedPositionX/Y/Z,
 *         PlayerModelIndices, PlayerOffsetsX/Y/Z, PlayerFloorInvertedModelIndices
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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	public partial class XwaHookDialog : Form
	{
		// this is going to be setup to read from the individual TXT files, but always write to Mission.ini
		#region vars
		readonly string _mission;
		readonly string _fileName = "";
		readonly string _bdFile = "";
		readonly string _soundFile = "";
		readonly string _interdictionFile = "";
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
		readonly string _weapRatesFile = "";
		readonly string _weapProfilesFile = "";
		readonly string _warheadProfilesFile = "";
		readonly string _energyProfilesFile = "";
		readonly string _linkingProfilesFile = "";
		readonly string _warheadTypeCountFile = "";
		readonly string _specRciFile = "";
		readonly string _installDirectory = "";
		readonly string _mis = "Missions\\";
		readonly string _res = "Resdata\\";
		readonly string _wave = "Wave\\";
		readonly string _fm = "FlightModels\\";
		enum ReadMode
		{
			None = -1, Backdrop, Mission, Sounds, Interdiction, Objects,
			HangarObjects, HangarCamera, FamilyHangarCamera, HangarMap, FamilyHangarMap,
			Skins, Shield, Hyper, Concourse, HullIcon, Stats,
			WeaponRate, WeapProfile, WarheadProfile, EnergyProfile, LinkingProfile, WarheadTypeCount, SpecRci
		}
		bool _loading = true;
		readonly int[,] _cameras = new int[5, 3];
		readonly int[,] _defaultCameras = new int[5, 3] { { 1130, -2320, -300 }, { 1240, -330, -700 }, { -1120, 1360, -790 }, { -1200, -1530, -850 }, { 1070, 4640, -130 } };
		readonly int[,] _familyCameras = new int[7, 3];
		readonly int[,] _defaultFamilyCameras = new int[7, 3] { { 780, -6471, -4977 }, { -1970, -8810, -4707 }, { 2510, -5391, -5067 },
			{ 1740, -8461, -5047 }, { 3180, 2629, -3777 }, { 8242, 6500, 10 }, { -13360, 35019, -6537 } };
		enum ShuttleAnimation { Right, Top, Bottom }
		readonly int[] _defaultShuttlePosition = new int[4] { 1127, 959, 0, 43136 };
		readonly int[] _defaultRoofCranePosition = new int[3] { -1400, 786, -282 };
		readonly Panel[] _panels = new Panel[12];
		readonly bool[] _skipIffs = new bool[255];
		readonly bool _initialLoad = true;
		string _tempModels = "";
		string _tempXs = "";
		string _tempYs = "";
		string _tempZs = "";
		readonly CheckBox[] _chkRegions = new CheckBox[4];
		readonly HookFile _hookFile = null;
		bool _txtModified = false;
		readonly string _title = "";
		#endregion

		public XwaHookDialog(Mission mission)
		{
			InitializeComponent();
			var config = Settings.GetInstance();
			_mission = Path.GetFileNameWithoutExtension(mission.MissionPath);
			if (_mission == "NewMission")
			{
				MessageBox.Show("Please perform initial save prior to hook assignment.", "New Mission detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cmdCancel_Click("NewMission", new EventArgs());
				return;
			}
			_fileName = Path.ChangeExtension(mission.MissionPath, ".ini");

			#region initialize
			Width = 786;
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
			_panels[11] = pnlWeaponRate;
			for (int i = 0; i < _panels.Length; i++)
			{
				_panels[i].Left = 395;
				_panels[i].Top = 45;
			}
			pnlWarheadCounts.Location = pnlWeapRates.Location;
			_chkRegions[0] = chkIntRegion1;
			_chkRegions[1] = chkIntRegion2;
			_chkRegions[2] = chkIntRegion3;
			_chkRegions[3] = chkIntRegion4;
			for (int i = 0; i < 4; i++) _chkRegions[i].CheckedChanged += chkRegions_CheckedChanged;
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
			cboStatMarks.Items.Add("All");
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
			cboWeapFG.Items.AddRange((mission.FlightGroups.GetList()));
			lstFgTargeting.Items.AddRange(((mission.FlightGroups.GetList())));
			for (int i = 0; i < 400; i++)
			{
				cboShuttleModel.Items.Add(i);
				cboMapIndex.Items.Add(i);
				cboFamMapIndex.Items.Add(i);
				cboDroid1Model.Items.Add(i);
				cboDroid2Model.Items.Add(i);
				cboAutoModel.Items.Add(i);
			}
			cboShuttleModel.SelectedIndex = 50;
			cboMapIndex.SelectedIndex = 0;
			cboFamMapIndex.SelectedIndex = 0;
			cboDroid1Model.SelectedIndex = 311;
			cboDroid2Model.SelectedIndex = 312;
			cboAutoModel.SelectedIndex = 0;
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
			cboSpecRci.SelectedIndex = 0;
			#endregion
			resetUI();
			if (config.XwaInstalled) _installDirectory = config.XwaPath + "\\";
			#region get CommandShip
			// CommandShip priority order: Player's CommandShip, Player's Arrival MS, Player's Departure MS
			int commandShip = -1;
			for (int i = 0; i < mission.FlightGroups.Count; i++)
			{
				if (mission.FlightGroups[i].PlayerNumber != 1) continue;

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

				if (player.ArriveViaMothership == 1) commandShip = player.ArrivalMothership;
				else if (player.DepartViaMothership == 1) commandShip = player.DepartureMothership;
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
				_interdictionFile = checkFile("_Interdiction.txt");
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
				_weapRatesFile = checkFile("_WeaponRates.txt");
				_weapProfilesFile = checkFile("_WeaponProfiles.txt");
				_warheadProfilesFile = checkFile("_WarheadProfiles.txt");
				_energyProfilesFile = checkFile("_EnergyProfiles.txt");
				_linkingProfilesFile = checkFile("_LinkingProfiles.txt");
				_warheadTypeCountFile = checkFile("_WarheadTypeCount.txt");
				_specRciFile = checkFile("_SpecRci.txt");
			}
			string line;

			_hookFile = new HookFile(_fileName);
			if (File.Exists(_fileName)) _hookFile.Read();

			if (_hookFile.Comments.Count == 0)
			{
				if (_installDirectory != "") using (var sr = new StreamReader(_installDirectory + "\\Missions\\MISSION.LST"))
						while ((line = sr.ReadLine()) != null)
						{
							line = removeComment(line);
							if (line == "" || line.StartsWith("!")) continue;

							// skip and trim until we get to #B#M
							while (line.Length > 5 && line[1] != 'b' && line[1] != 'B') line = line.Substring(1);
							if (line.Equals(_mission + ".tie", StringComparison.OrdinalIgnoreCase))
							{
								_title = sr.ReadLine();
								_title = _title.Substring(_title.IndexOf("DESC!") + 5);
								break;
							}
						}
				_hookFile.Comments.Add($";{_mission}.ini{(_title != "" ? $" - {_title}" : "")}");
			}
			#region individual files
			if (_bdFile != "") _hookFile.MergeTextFile(_bdFile, "Resdata");
			if (_missionTxtFile != "") _hookFile.MergeTextFile(_missionTxtFile, "Mission_Tie");
			if (_soundFile != "") _hookFile.MergeTextFile(_soundFile, "Sounds");
			if (_interdictionFile != "") _hookFile.MergeTextFile(_interdictionFile, "Interdiction");
			if (_objFile != "") _hookFile.MergeTextFile(_objFile, "Objects");
			if (_commandOpt != "")
			{
				if (_hangarObjectsFileSI != "") _hookFile.MergeTextFile(_hangarObjectsFileSI, $"HangarObjects_{_commandOpt}_{_commandIff}");
				if (_hangarCameraFileSI != "") _hookFile.MergeTextFile(_hangarCameraFileSI, $"HangarCamera_{_commandOpt}_{_commandIff}");
				if (_famHangarCameraFileSI != "") _hookFile.MergeTextFile(_famHangarCameraFileSI, $"FamHangarCamera_{_commandOpt}_{_commandIff}");
				if (_hangarMapFileSI != "") _hookFile.MergeTextFile(_hangarMapFileSI, $"HangarMap_{_commandOpt}_{_commandIff}");
				if (_famHangarMapFileSI != "") _hookFile.MergeTextFile(_famHangarMapFileSI, $"FamHangarMap_{_commandOpt}_{_commandIff}");
				if (_hangarObjectsFileS != "") _hookFile.MergeTextFile(_hangarObjectsFileS, $"HangarObjects_{_commandOpt}");
				if (_hangarCameraFileS != "") _hookFile.MergeTextFile(_hangarCameraFileS, $"HangarCamera_{_commandOpt}");
				if (_famHangarCameraFileS != "") _hookFile.MergeTextFile(_famHangarCameraFileS, $"FamHangarCamera_{_commandOpt}");
				if (_hangarMapFileS != "") _hookFile.MergeTextFile(_hangarMapFileS, $"HangarMap_{_commandOpt}");
				if (_famHangarMapFileS != "") _hookFile.MergeTextFile(_famHangarMapFileS, $"FamHangarMap_{_commandOpt}");
			}
			if (_hangarObjectsFile != "") _hookFile.MergeTextFile(_hangarObjectsFile, "HangarObjects");
			if (_hangarCameraFile != "") _hookFile.MergeTextFile(_hangarCameraFile, "HangarCamera");
			if (_famHangarCameraFile != "") _hookFile.MergeTextFile(_famHangarCameraFile, "FamHangarCamera");
			if (_hangarMapFile != "") _hookFile.MergeTextFile(_hangarMapFile, "HangarMap");
			if (_famHangarMapFile != "") _hookFile.MergeTextFile(_famHangarMapFile, "FamHangarMap");
			if (_32bppFile != "") _hookFile.MergeTextFile(_32bppFile, "Skins");
			if (_shieldFile != "") _hookFile.MergeTextFile(_shieldFile, "Shield");
			if (_hyperFile != "") _hookFile.MergeTextFile(_hyperFile, "Hyperspace");
			if (_concourseFile != "") _hookFile.MergeTextFile(_concourseFile, "Concourse");
			if (_hullIconFile != "") _hookFile.MergeTextFile(_hullIconFile, "HullIcon");
			if (_statsFile != "") _hookFile.MergeTextFile(_statsFile, "StatsProfiles");
			if (_weapRatesFile != "") _hookFile.MergeTextFile(_weapRatesFile, "WeaponRates");
			if (_weapProfilesFile != "") _hookFile.MergeTextFile(_weapProfilesFile, "WeaponProfiles");
			if (_warheadProfilesFile != "") _hookFile.MergeTextFile(_warheadProfilesFile, "WarheadProfiles");
			if (_energyProfilesFile != "") _hookFile.MergeTextFile(_energyProfilesFile, "EnergyProfiles");
			if (_linkingProfilesFile != "") _hookFile.MergeTextFile(_linkingProfilesFile, "LinkingProfiles");
			if (_warheadTypeCountFile != "") _hookFile.MergeTextFile(_warheadTypeCountFile, "WarheadTypeCount");
			if (_specRciFile != "") _hookFile.MergeTextFile(_specRciFile, "SpecRci");
			#endregion

			txtHook.Text = _hookFile.GetContents();
			parseHookFile();

			_initialLoad = false;
			_loading = false;
		}

		string checkFile(string extension)
		{
			string path = _installDirectory + _mis + _mission + extension;
			if (File.Exists(path)) return path;
			return "";
		}

		void createContents()
		{
			bool loading = _loading;
			_loading = true;
			txtHook.Text = _hookFile.GetContents();
			_loading = loading;
		}

		/*void parseContents()
		{
			if (_commandOpt != "")
			{
				if (lineLower == $"[hangarobjects_{_commandOpt}_{_commandIff}]") readMode = ReadMode.HangarObjects;
				else if (lineLower == $"[hangarcamera_{_commandOpt}_{_commandIff}]") readMode = ReadMode.HangarCamera;
				else if (lineLower == $"[famhangarcamera_{_commandOpt}_{_commandIff}]") readMode = ReadMode.FamilyHangarCamera;
				else if (lineLower == $"[hangarmap_{_commandOpt}_{_commandIff}]") readMode = ReadMode.HangarMap;
				else if (lineLower == $"[famhangarmap_{_commandOpt}_{_commandIff}]") readMode = ReadMode.FamilyHangarMap;
				else if (lineLower == $"[hangarobjects_{_commandOpt}]") readMode = ReadMode.HangarObjects;
				else if (lineLower == $"[hangarcamera_{_commandOpt}]") readMode = ReadMode.HangarCamera;
				else if (lineLower == $"[famhangarcamera_{_commandOpt}]") readMode = ReadMode.FamilyHangarCamera;
				else if (lineLower == $"[hangarmap_{_commandOpt}]") readMode = ReadMode.HangarMap;
				else if (lineLower == $"[famhangarmap_{_commandOpt}]") readMode = ReadMode.FamilyHangarMap;
			}
		}*/

		void parseHookFile()
		{
			if (!_initialLoad) resetUI();

			foreach (var section in _hookFile.Sections)
			{
				if (section.Name.Equals("Resdata", StringComparison.OrdinalIgnoreCase)) lstBackdrops.Items.AddRange(section.Entries.ToArray());
				else if (section.Name.Equals("Mission_Tie", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries) parseMission(entry);
				else if (section.Name.Equals("Sounds", StringComparison.OrdinalIgnoreCase)) lstSounds.Items.AddRange(section.Entries.ToArray());
				else if (section.Name.Equals("Interdiction", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries) parseInterdiction(entry);
				else if (section.Name.Equals("Objects", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries)
					{
						if (section.Name.IndexOf("_HullIcon", StringComparison.OrdinalIgnoreCase) != -1) parseHullIcon(entry);
						else lstObjects.Items.Add(entry);
					}
				else if (section.Name.Equals("HangarObjects", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries) parseHangarObjects(entry);
				else if (section.Name.Equals("HangarCamera", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries) parseHangarCamera(entry);
				else if (section.Name.Equals("FamHangarCamera", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries) parseFamilyHangarCamera(entry);
				else if (section.Name.Equals("HangarMap", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries)
					{
						MapEntry map = new MapEntry();
						if (map.Parse(entry)) lstMap.Items.Add(map.ToString());
					}
				else if (section.Name.Equals("FamHangarMap", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries)
					{
						MapEntry map = new MapEntry();
						if (map.Parse(entry)) lstFamilyMap.Items.Add(map.ToString());
					}
				/* TODO: commandOpt may not have ever worked properly, since it was mixing these in to the regular sections
				 * if (_commandOpt != "")
					{
						if (lineLower == $"[hangarobjects_{_commandOpt}_{_commandIff}]") readMode = ReadMode.HangarObjects;
						else if (lineLower == $"[hangarcamera_{_commandOpt}_{_commandIff}]") readMode = ReadMode.HangarCamera;
						else if (lineLower == $"[famhangarcamera_{_commandOpt}_{_commandIff}]") readMode = ReadMode.FamilyHangarCamera;
						else if (lineLower == $"[hangarmap_{_commandOpt}_{_commandIff}]") readMode = ReadMode.HangarMap;
						else if (lineLower == $"[famhangarmap_{_commandOpt}_{_commandIff}]") readMode = ReadMode.FamilyHangarMap;
						else if (lineLower == $"[hangarobjects_{_commandOpt}]") readMode = ReadMode.HangarObjects;
						else if (lineLower == $"[hangarcamera_{_commandOpt}]") readMode = ReadMode.HangarCamera;
						else if (lineLower == $"[famhangarcamera_{_commandOpt}]") readMode = ReadMode.FamilyHangarCamera;
						else if (lineLower == $"[hangarmap_{_commandOpt}]") readMode = ReadMode.HangarMap;
						else if (lineLower == $"[famhangarmap_{_commandOpt}]") readMode = ReadMode.FamilyHangarMap;
					}*/
				else if (section.Name.Equals("Skins", StringComparison.OrdinalIgnoreCase)) lstSkins.Items.AddRange(section.Entries.ToArray());
				else if (section.Name.Equals("Shield", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries) parseShield(entry);
				else if (section.Name.Equals("Hyperspace", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries) parseHyper(entry);
				else if (section.Name.Equals("Concourse", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries) parseConcourse(entry);
				else if (section.Name.Equals("HullIcon", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries) parseHullIcon(entry);
				else if (section.Name.Equals("StatsProfiles", StringComparison.OrdinalIgnoreCase)) lstStats.Items.AddRange(section.Entries.ToArray());
				else if (section.Name.Equals("WeaponRates", StringComparison.OrdinalIgnoreCase)) foreach (var entry in section.Entries) lstWeapons.Items.Add("WR: " + entry);
				else if (section.Name.Equals("WeaponProfiles", StringComparison.OrdinalIgnoreCase) || section.Name.Equals("WarheadProfiles", StringComparison.OrdinalIgnoreCase)
					|| section.Name.Equals("EnergyProfiles", StringComparison.OrdinalIgnoreCase) || section.Name.Equals("LinkingProfiles", StringComparison.OrdinalIgnoreCase)
					|| section.Name.Equals("WarheadTypeCount", StringComparison.OrdinalIgnoreCase)) lstWeapons.Items.AddRange(section.Entries.ToArray());
				else if (section.Name.Equals("SpecRci", StringComparison.OrdinalIgnoreCase)) lstSpecRci.Items.AddRange(section.Entries.ToArray());
			}
		}

		static string removeComment(string line)
		{
			if (line.IndexOf(";") != -1) line = line.Substring(0, line.IndexOf(";"));
			if (line.IndexOf("#") != -1) line = line.Substring(0, line.IndexOf("#"));
			if (line.IndexOf("//") != -1) line = line.Substring(0, line.IndexOf("//"));
			return line.Trim();
		}

		void resetUI()
		{
			lstBackdrops.Items.Clear();
			lstMission.Items.Clear();
			lstSounds.Items.Clear();
			chkIntRegion1.Checked = chkIntRegion2.Checked = chkIntRegion3.Checked = chkIntRegion4.Checked = false;
			lstObjects.Items.Clear();
			lstSFoils.Items.Clear();
			chkForceHangarSF.Checked = false;
			chkForceHyperLG.Checked = false;
			chkManualSF.Checked = false;
			lstSkins.Items.Clear();
			lstShield.Items.Clear();
			chkSSRecharge.Checked = true;
			chkFighterDoubled.Checked = false;
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
			cmdInvertedPlayerReset_Click("reset", new EventArgs());
			chkPlayerFloor.Checked = false;
			chkFloor.Checked = false;
			numInvertedHangarFloor.Value = 0;
			chkShadows.Checked = true;
			txtLightColor.Text = "FFFFFF";
			pctLight.BackColor = System.Drawing.Color.White;
			numIntensity.Value = 192;
			numPlayerAnimationElevation.Value = 0;
			numPlayerStraight.Value = 0;
			numInvertedPlayerFloor.Value = 0;
			lstAutoPlayer.Items.Clear();
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
			chkDisableRanks.Checked = false;
			chkSkipProx.Checked = false;
			chkHardShields.Checked = false;
			cboTargetMethod.SelectedIndex = 0;
			lstStats.Items.Clear();
			chkWeapDecharge.Checked = false;
			chkWeapRecharge.Checked = false;
			chkTransfer.Checked = false;
			chkRatePenalty.Checked = false;
			chkTransferShieldLimit.Checked = false;
			chkTransferWeapLimit.Checked = false;
			chkMaxTorpPass.Checked = false;
			chkMaxTorpTarget.Checked = false;
			chkImpact.Checked = true;
			chkImpactAngle.Checked = false;
			chkImpactSpeed.Checked = false;
			lstWeapons.Items.Clear();
			lstSpecRci.Items.Clear();
		}

		void updateHookFile()
		{
			HookFile.HookSection section = null;
			_hookFile.Sections.Clear();
			_hookFile.Comments.Clear();
			foreach (string line in txtHook.Lines)
			{
				if (string.IsNullOrEmpty(line.Trim())) continue;

				if (section == null && !line.StartsWith("[")) _hookFile.AddComment(line);
				else if (line.StartsWith("[")) section = _hookFile.GetOrCreateSection(line.Substring(1, line.IndexOf(']') - 1));
				else section.AddLine(line);
			}
		}

		void updateSectionFromLst(string name, ListBox lst)
		{
			var section = _hookFile.GetOrCreateSection(name);
			section.Entries.Clear();
			foreach (var entry in lst.Items) section.AddLine(entry.ToString());
			createContents();
		}

		#region Backdrops
		private void cmdAddBD_Click(object sender, EventArgs e)
		{
			if (_installDirectory != "") opnBackdrop.InitialDirectory = _installDirectory + _res;
			DialogResult res = opnBackdrop.ShowDialog();
			if (res != DialogResult.OK) return;
			
			lstBackdrops.Items.Add(opnBackdrop.FileName.Substring(opnBackdrop.FileName.IndexOf(_res)));
			updateSectionFromLst("Resdata", lstBackdrops);
		}
		private void cmdRemoveBD_Click(object sender, EventArgs e)
		{
			if (lstBackdrops.SelectedIndex == -1) return;

			lstBackdrops.Items.RemoveAt(lstBackdrops.SelectedIndex);
			updateSectionFromLst("Resdata", lstBackdrops);
		}
		#endregion Backdrops

		#region MissionTie
		/// <remarks>This also parses S-Foils</remarks>
		void parseMission(string line)
		{
			string[] parts = line.Replace(" ", "").Split(',');
			try
			{
				if (parts[0].Equals("FG", StringComparison.OrdinalIgnoreCase))
				{
					int fg = int.Parse(parts[1]);
					if (parts[2].Equals("Markings", StringComparison.OrdinalIgnoreCase)) lstMission.Items.Add($"{cboFG.Items[fg]},marks,{cboMarkings.Items[int.Parse(parts[3])]}");
					else if (parts[2].Equals("Index", StringComparison.OrdinalIgnoreCase)) lstMission.Items.Add($"{cboFG.Items[fg]},wing,{int.Parse(parts[3])},{cboMarkings.Items[int.Parse(parts[5])]}");
					else if (parts[2].Equals("Iff", StringComparison.OrdinalIgnoreCase)) lstMission.Items.Add($"{cboFG.Items[fg]},iff,{cboIff.Items[int.Parse(parts[3])]}");
					else if (parts[2].Equals("PilotVoice", StringComparison.OrdinalIgnoreCase)) lstMission.Items.Add($"{cboFG.Items[fg]},pilot,{parts[3]}");
					else if (parts[2].Equals("Close_SFoils", StringComparison.OrdinalIgnoreCase)) lstSFoils.Items.Add($"{cboSFoilFG.Items[fg]},closed");
					else if (parts[2].Equals("Open_LandingGears", StringComparison.OrdinalIgnoreCase)) lstSFoils.Items.Add($"{cboSFoilFG.Items[fg]},open");
					else throw new InvalidDataException();
				}
				else if (parts[0].Equals("craft", StringComparison.OrdinalIgnoreCase))
				{
					int craft = int.Parse(parts[1]);
					if (parts[2].Equals("Name", StringComparison.OrdinalIgnoreCase)) lstCraftText.Items.Add($"{cboCraftText.Items[craft]},name,{parts[3]}");
					else if (parts[2].Equals("SpecName", StringComparison.OrdinalIgnoreCase)) lstCraftText.Items.Add($"{cboCraftText.Items[craft]},species,{parts[3]}");
					else if (parts[2].Equals("PluralName", StringComparison.OrdinalIgnoreCase)) lstCraftText.Items.Add($"{cboCraftText.Items[craft]},plural,{parts[3]}");
					else if (parts[2].Equals("ShortName", StringComparison.OrdinalIgnoreCase)) lstCraftText.Items.Add($"{cboCraftText.Items[craft]},abbrv,{parts[3]}");
					else if (parts[2].Equals("MapIcon", StringComparison.OrdinalIgnoreCase) && parts.Length > 6) lstCraftText.Items.Add($"{cboCraftText.Items[craft]},mapicon,{parts[3]},{parts[4]},{parts[5]},{parts[6]}");
					else throw new InvalidDataException();
				}
				else if (parts[0].StartsWith("Key_O", StringComparison.OrdinalIgnoreCase))
				{
					// this one's separate because the format is key=#,#,#...
					var firstPart = parts[0].Split('=');
					if (firstPart[0].Equals("Key_O_TargetCraftFGs", StringComparison.OrdinalIgnoreCase))
					{
						lstFgTargeting.ClearSelected();
						lstFgTargeting.SetSelected(int.Parse(firstPart[1]), true);
						for (int i = 1; i < parts.Length; i++) { lstFgTargeting.SetSelected(int.Parse(parts[i]), true); }
					}
					else throw new InvalidDataException();
				}
				else
				{
					parts = parts[0].Split('=');
					if (parts[0].Equals("CloseCFoilsAndOpenLandingGearsBeforeEnterHangar", StringComparison.OrdinalIgnoreCase)) chkForceHangarSF.Checked = parts[1] == "1";
					else if (parts[0].Equals("CloseLandingGearsBeforeEnterHyperspace", StringComparison.OrdinalIgnoreCase)) chkForceHyperLG.Checked = parts[1] == "1";
					else if (parts[0].Equals("AutoCloseSFoils", StringComparison.OrdinalIgnoreCase)) chkManualSF.Checked = parts[1] == "0";
					else if (parts[0].Equals("IsRedAlertEnabled", StringComparison.OrdinalIgnoreCase)) chkRedAlert.Checked = parts[1] == "1";
					else if (parts[0].Equals("SkipHyperspacedMessages", StringComparison.OrdinalIgnoreCase)) chkSkipHyper.Checked = parts[1] == "1";
					else if (parts[0].Equals("SkipObjectsMessagesIff", StringComparison.OrdinalIgnoreCase))
					{
						chkSkipIffMessages.Checked = (parts[1] != "-1");
						if (parts[1].Equals("255", StringComparison.OrdinalIgnoreCase)) chkSkipAllIff.Checked = true;
						else
						{
							chkSkipAllIff.Checked = false;
							_skipIffs[int.Parse(parts[1])] = true;
						}
					}
					else if (parts[0].Equals("ForcePlayerInTurret", StringComparison.OrdinalIgnoreCase)) chkForceTurret.Checked = parts[1] == "1";
					else if (parts[0].Equals("ForcePlayerInTurretHours", StringComparison.OrdinalIgnoreCase)) numTurretH.Value = int.Parse(parts[1]);
					else if (parts[0].Equals("ForcePlayerInTurretMinutes", StringComparison.OrdinalIgnoreCase)) numTurretM.Value = int.Parse(parts[1]);
					else if (parts[0].Equals("ForcePlayerInTurretSeconds", StringComparison.OrdinalIgnoreCase)) numTurretS.Value = int.Parse(parts[1]);
					else if (parts[0].Equals("DisablePlayerLaserShoot", StringComparison.OrdinalIgnoreCase)) chkDisableLaser.Checked = parts[1] == "1";
					else if (parts[0].Equals("DisablePlayerWarheadShoot", StringComparison.OrdinalIgnoreCase)) chkDisableWarhead.Checked = parts[1] == "1";
					else if (parts[0].Equals("IsWarheadCollisionDamagesEnabled", StringComparison.OrdinalIgnoreCase)) chkDisableCollision.Checked = parts[1] == "0";
					else if (parts[0].Equals("CanShootThroughtShieldOnHardDifficulty", StringComparison.OrdinalIgnoreCase)) chkHardShields.Checked = parts[1] == "1";
					else if (parts[0].Equals("IsMissionRanksModifierEnabled", StringComparison.OrdinalIgnoreCase)) chkDisableRanks.Checked = parts[1] == "0";
					else if (parts[0].Equals("TargetCraftKeyMethod", StringComparison.OrdinalIgnoreCase)) cboTargetMethod.SelectedIndex = int.Parse(parts[1]) + 1;
					else if (parts[0].Equals("TargetCraftKeySelectOnlyNotInspected", StringComparison.OrdinalIgnoreCase)) chkNotInspected.Checked = parts[1] == "1";
					else if (parts[0].Equals("SkipProjectilesProximityCheck", StringComparison.OrdinalIgnoreCase)) chkSkipProx.Checked = parts[1] == "1";
					// TODO: "CampaignCraftsList" = { fg, fg2... }; similar to targeting FGs
					// TODO: "CampaignCraftName_## = name"; where ## is the fg index of the ship.
					// TODO: "CampaignCraftsListLines" = { string1, string2...}; comma separated list of strings.
					// TODO: "CampaignCraftsListLinesTop" = -1; define the top position of the custom list
					else throw new InvalidDataException();
				}
			}
			catch { _hookFile.GetOrCreateSection("Mission_Tie").AddComment(line); }
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
			updateMissionTie();
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
			updateMissionTie();
		}
		private void chkSkipAllIff_CheckedChanged(object sender, EventArgs e)
		{
			if (!chkSkipAllIff.Enabled) return;
			
			cboSkipIff.Enabled = chkSkipIff.Enabled = !chkSkipAllIff.Checked;
			updateMissionTie();
		}

		private void cmdAddMiss_Click(object sender, EventArgs e)
		{
			if (cboFG.SelectedIndex == -1 || ((optWingman.Checked || optMarkings.Checked) && cboMarkings.SelectedIndex == -1) || (optIff.Checked && cboIff.SelectedIndex == -1)
				|| (optPilot.Checked && txtPilot.Text == "")) return;

			if (optMarkings.Checked) lstMission.Items.Add($"{cboFG.Text},marks,{cboMarkings.Text}");
			else if (optWingman.Checked) lstMission.Items.Add($"{cboFG.Text},wing,{numWingman.Value},{cboMarkings.Text}");
			else if (optIff.Checked) lstMission.Items.Add($"{cboFG.Text},iff,{cboIff.Text}");
			else if (optPilot.Checked) lstMission.Items.Add($"{cboFG.Text},pilot,{txtPilot.Text}");
			updateMissionTie();
		}
		private void cmdRemoveMiss_Click(object sender, EventArgs e)
		{
			if (lstMission.SelectedIndex == -1) return;

			lstMission.Items.RemoveAt(lstMission.SelectedIndex);
			updateMissionTie();
		}
		private void cmdAddCraftText_Click(object sender, EventArgs e)
		{
			if (cboCraftText.SelectedIndex == -1 || (!optCraftText.Checked && !optPluralText.Checked && !optAbbrvText.Checked && !optSpeciesText.Checked) || txtCraftText.Text == "") return;

			if (optCraftText.Checked) lstCraftText.Items.Add($"{cboCraftText.Text},name,{txtCraftText.Text}");
			else if (optPluralText.Checked) lstCraftText.Items.Add($"{cboCraftText.Text},plural,{txtCraftText.Text}");
			else if (optAbbrvText.Checked) lstCraftText.Items.Add($"{cboCraftText.Text},abbrv,{txtCraftText.Text}");
			else if (optSpeciesText.Checked) lstCraftText.Items.Add($"{cboCraftText.Text},species,{txtCraftText.Text}");
			else if (optMapIcon.Checked) lstCraftText.Items.Add($"{cboCraftText.Text},mapicon,{numMapLeft.Value},{numMapTop.Value},{numMapRight.Value},{numMapBottom.Value}");
			updateMissionTie();
		}
		private void cmdRemoveTextCraft_Click(object sender, EventArgs e)
		{
			if (lstCraftText.SelectedIndex == -1) return;

			lstCraftText.Items.RemoveAt(lstCraftText.SelectedIndex);
			updateMissionTie();
		}
		private void cmdAddStat_Click(object sender, EventArgs e)
		{
			if (cboStatType.SelectedIndex == -1) return;

			if (cboStatType.SelectedIndex == 0 && txtStatProfile.Text.ToLower() != "default" && (chkStatPlayer.Checked || cboStatMarks.SelectedIndex != -1))
			{
				opnObjects.Title = "Select object...";
				var res = opnObjects.ShowDialog();
				if (res != DialogResult.OK) return;
				
				string line = Path.GetFileNameWithoutExtension(opnObjects.FileName) + "_fg";
				if (chkStatPlayer.Checked) line += "_player";
				else if (cboStatMarks.Text == "All") line = Path.GetFileNameWithoutExtension(opnObjects.FileName);
				else line += "c_" + cboStatMarks.SelectedIndex;
				line += " = " + txtStatProfile.Text;
				lstStats.Items.Add(line);
			}
			else if (cboStatType.SelectedIndex != 0)
			{
				string line = $"{cboStatType.Text}Percent = {numStatPercent.Value}";
				if (chkStatPlayer.Checked) line = "Player" + line;
				lstStats.Items.Add(line);
			}
			updateSectionFromLst("StatsProfiles", lstStats);
		}
		private void cmdRemoveStat_Click(object sender, EventArgs e)
		{
			if (lstStats.SelectedIndex == -1) return;

			lstStats.Items.RemoveAt(lstStats.SelectedIndex);
			updateSectionFromLst("StatsProfiles", lstStats);
		}
		private void cmdClearTargeting_Click(object sender, EventArgs e) => lstFgTargeting.ClearSelected();

		private void cmdAddSpecRci_Click(object sender, EventArgs e)
		{
			if (cboSpecRci.SelectedIndex == -1 || numSpecRci.Value == -1) return;

			if (_installDirectory != "") opnObjects.InitialDirectory = _installDirectory + _fm;
			opnObjects.Title = "Select Craft...";
			DialogResult res = opnObjects.ShowDialog();
			if (res != DialogResult.OK) return;

			string line = $"{Path.GetFileNameWithoutExtension(opnObjects.FileName)}_{cboSpecRci.Text} = {numSpecRci.Value}";
			lstSpecRci.Items.Add(line);
			updateSectionFromLst("SpecRci", lstSpecRci);
		}
		private void cmdRemoveSpecRci_Click(object sender, EventArgs e)
		{
			if (lstSpecRci.SelectedIndex == -1) return;

			lstSpecRci.Items.RemoveAt(lstSpecRci.SelectedIndex);
			updateSectionFromLst("SpecRci", lstSpecRci);
		}

		private void optMapIcon_CheckedChanged(object sender, EventArgs e) => pnlMapIcon.Enabled = optMapIcon.Checked;

		void uiMissionTie_DefaultEvent(object sender, EventArgs e) => updateMissionTie();

		void updateMissionTie()
		{
			if (_loading) return;

			var section = _hookFile.GetOrCreateSection("Mission_Tie");
			section.Entries.Clear();
			if (lstMission.Items.Count > 0)
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
								section.AddLine($"fg, {fg}, markings, {m}");
								break;
							}
					}
					else if (parts[1] == "wing")
					{
						for (int m = 0; m < cboMarkings.Items.Count; m++)
							if (cboMarkings.Items[m].ToString() == parts[3])
							{
								section.AddLine($"fg, {fg}, index, {parts[2]}, markings, {m}");
								break;
							}
					}
					else if (parts[1] == "iff")
					{
						for (int iff = 0; iff < cboIff.Items.Count; iff++)
							if (cboIff.Items[iff].ToString() == parts[2])
							{
								section.AddLine($"fg, {fg}, iff, {iff}");
								break;
							}
					}
					else if (parts[1] == "pilot") section.AddLine($"fg, {fg}, pilotvoice, {parts[2]}");
				}
			if (useSFoils)
			{
				for (int i = 0; i < lstSFoils.Items.Count; i++)
				{
					string[] parts = lstSFoils.Items[i].ToString().Split(',');
					int fg;
					for (fg = 0; fg < cboSFoilFG.Items.Count; fg++) if (cboSFoilFG.Items[fg].ToString() == parts[0]) break;
					if (parts[1] == "closed") section.AddLine($"fg, {fg}, close_SFoils, 1");
					else if (parts[1] == "open") section.AddLine($"fg, {fg}, open_LandingGears, 1");
				}
				if (chkForceHangarSF.Checked) section.AddLine("CloseSFoilsAndOpenLandingGearsBeforeEnterHangar = 1");
				if (chkForceHyperLG.Checked) section.AddLine("CloseLandingGearsBeforeEnterHyperspace = 1");
				if (chkManualSF.Checked) section.AddLine("AutoCloseSFoils = 0");
			}
			if (lstCraftText.Items.Count > 0)
			{
				for (int i = 0; i < lstCraftText.Items.Count; i++)
				{
					string[] parts = lstCraftText.Items[i].ToString().Split(',');
					int craft;
					for (craft = 0; craft < cboCraftText.Items.Count; craft++) if (cboCraftText.Items[craft].ToString() == parts[0]) break;
					if (parts[1] == "name") section.AddLine($"craft, {craft}, name, {parts[2]}");
					else if (parts[1] == "species") section.AddLine($"craft, {craft}, specname, {parts[2]}");
					else if (parts[1] == "plural") section.AddLine($"craft, {craft}, pluralname, {parts[2]}");
					else if (parts[1] == "abbrv") section.AddLine($"craft, {craft}, shortname, {parts[2]}");
					else if (parts[1] == "mapicon") section.AddLine($"craft, {craft}, mapicon, {parts[2]}, {parts[3]}, {parts[4]}, {parts[5]}");
				}
			}
			if (useMissionSettings)
			{
				if (chkRedAlert.Checked) section.AddLine("IsRedAlertEnabled = 1");
				if (chkSkipHyper.Checked) section.AddLine("SkipHyperspacedMessages = 1");
				if (chkSkipIffMessages.Checked)
				{
					if (chkSkipAllIff.Checked) section.AddLine("SkipObjectsMessagesIff = 255");
					else for (int i = 0; i < _skipIffs.Length; i++) if (_skipIffs[i]) section.AddLine($"SkipObjectsMessagesIff = {i}");
				}
				if (chkForceTurret.Checked) section.AddLine("ForcePlayerInTurret = 1");
				if (numTurretH.Value != 0) section.AddLine($"ForcePlayerInTurretHours = {(int)numTurretH.Value}");
				if (numTurretM.Value != 0) section.AddLine($"ForcePlayerInTurretMinutes = {(int)numTurretM.Value}");
				if (numTurretS.Value != 8) section.AddLine($"ForcePlayerInTurretSeconds = {(int)numTurretS.Value}");
				if (chkDisableLaser.Checked) section.AddLine("DisablePlayerLaserShoot = 1");
				if (chkDisableWarhead.Checked) section.AddLine("DisablePlayerWarheadShoot = 1");
				if (chkDisableCollision.Checked) section.AddLine("IsWarheadCollisionDamagesEnabled = 0");
				if (chkHardShields.Checked) section.AddLine("CanShootThroughtShieldOnHardDifficulty = 1");
				if (chkDisableRanks.Checked) section.AddLine("IsMissionRanksModifierEnabled = 0");
				if (lstFgTargeting.SelectedIndices.Count > 0)
				{
					string targets = "KEY_O_TargetCraftFGs = ";
					for (int i = 0; i < lstFgTargeting.SelectedIndices.Count; i++)
						targets += lstFgTargeting.SelectedIndices[i].ToString() + ",";
					section.AddLine(targets.Substring(0, targets.Length - 1));	// trims off the last ','
				}
				if (cboTargetMethod.SelectedIndex != 0) section.AddLine($"TargetCraftKeyMethod = {cboTargetMethod.SelectedIndex - 1}");
				if (chkNotInspected.Checked) section.AddLine("TargetCraftKeySelectOnlyNotInspected = 1");
				if (chkSkipProx.Checked) section.AddLine("SkipProjectilesProximityCheck = 1");
			}
			createContents();
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
					|| chkDisableLaser.Checked || chkDisableWarhead.Checked || chkDisableCollision.Checked || chkDisableRanks.Checked
					|| chkHardShields.Checked || chkSkipProx.Checked
					|| cboTargetMethod.SelectedIndex != 0 || chkNotInspected.Checked || lstFgTargeting.SelectedIndices.Count != 0) return true;
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
			if (res != DialogResult.OK) return;

			string line = opnSounds.FileName.Substring(opnSounds.FileName.IndexOf(_wave)) + " = ";
			opnSounds.Title = "Select new sound...";
			res = opnSounds.ShowDialog();
			if (res != DialogResult.OK) return;

			lstSounds.Items.Add(line + opnSounds.FileName.Substring(opnSounds.FileName.IndexOf(_wave) + 1));
			updateSectionFromLst("Sounds", lstSounds);
		}
		private void cmdRemoveSounds_Click(object sender, EventArgs e)
		{
			if (lstSounds.SelectedIndex == -1) return;

			lstSounds.Items.RemoveAt(lstSounds.SelectedIndex);
			updateSectionFromLst("Sounds", lstSounds);
		}
		#endregion

		#region Interdiction
		void parseInterdiction(string line)
		{
			bool loading = _loading;
			_loading = true;
			string[] parts = line.Replace(" ", "").Split('=');
			try
			{
				if (parts[0].Equals("region", StringComparison.OrdinalIgnoreCase) && parts.Length > 1)
				{
					parts = parts[1].Split(',');
					for (int i = 0; i < parts.Length; i++) _chkRegions[int.Parse(parts[i])].Checked = true;
				}
				else throw new InvalidDataException();
			}
			catch { _hookFile.GetOrCreateSection("Interdiction").AddComment(line); }
			_loading = loading;
		}

		private void chkRegions_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			var section = _hookFile.GetOrCreateSection("Interdiction");
			section.Entries.Clear();
			string regions = "";
			for (int i = 0; i < 4; i++) if (_chkRegions[i].Checked) regions += (regions != "" ? ", " : "") + i;
			section.AddLine($"Region = {regions}");
			createContents();
		}

		bool useInterdiction
		{
			get
			{
				bool use = false;
				for (int i = 0; i < 4; i++) use |= _chkRegions[i].Checked;
				return use;
			}
		}
		#endregion

		#region Objects
		private void chkWeaponProfile_CheckedChanged(object sender, EventArgs e) => numWeaponProfileMarking.Enabled = chkWeaponProfile.Checked;

		private void cmdAddObjects_Click(object sender, EventArgs e)
		{
			if (_installDirectory != "") opnObjects.InitialDirectory = _installDirectory + _fm;
			if (optCraft.Checked)
			{
				opnObjects.Title = "Select original object...";
				DialogResult res = opnObjects.ShowDialog();
				if (res != DialogResult.OK) return;

				string line = opnObjects.FileName.Substring(opnObjects.FileName.IndexOf(_fm)) + " = ";
				opnObjects.Title = "Select new object...";
				res = opnObjects.ShowDialog();
				if (res != DialogResult.OK) return;

				lstObjects.Items.Add(line + opnObjects.FileName.Substring(opnObjects.FileName.IndexOf(_fm)));
			}
			else if (optFGProfile.Checked && txtProfile.Text != "" && txtProfile.Text.ToLower() != "default")
			{
				string line = $"ObjectProfile_fg_{cboProfileFG.SelectedIndex} = {txtProfile.Text}";
				lstObjects.Items.Add(line);
			}
			else if (optCraftProfile.Checked && txtProfile.Text != "" && txtProfile.Text.ToLower() != "default")
			{
				opnObjects.Title = "Select object...";
				DialogResult res = opnObjects.ShowDialog();
				if (res != DialogResult.OK) return;
				
				string line = $"ObjectProfile_{Path.GetFileNameWithoutExtension(opnObjects.FileName)} = {txtProfile.Text}";
				lstObjects.Items.Add(line);
			}
			else if (optCraftCockpit.Checked && txtProfile.Text != "" && txtProfile.Text.ToLower() != "default")
			{
				opnObjects.Title = "Select object...";
				DialogResult res = opnObjects.ShowDialog();
				if (res != DialogResult.OK) return;
				
				string line = $"FlightModels\\{Path.GetFileNameWithoutExtension(opnObjects.FileName)}_CockpitPovProfile = {txtProfile.Text}";
				lstObjects.Items.Add(line);
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
				if (res != DialogResult.OK) return;
				
				string line = $"ObjectProfile_{Path.GetFileNameWithoutExtension(opnObjects.FileName)}_{numWeaponModel.Value} = {txtProfile.Text}{(chkWeaponProfile.Checked ? "_" + numWeaponProfileMarking.Value : "")}";
				lstObjects.Items.Add(line);
			}
			updateObjects();
		}
		private void cmdRemoveObjects_Click(object sender, EventArgs e)
		{
			if (lstObjects.SelectedIndex == -1) return;

			lstObjects.Items.RemoveAt(lstObjects.SelectedIndex);
			updateObjects();
		}

		private void objectsOpt_CheckedChanged(object sender, EventArgs e)
		{
			cboProfileFG.Enabled = optFGProfile.Checked;
			txtProfile.Enabled = (optFGProfile.Checked | optCraftProfile.Checked | optCraftCockpit.Checked | optCockpit.Checked | optWeaponProfile.Checked);
			chkWeaponProfile.Enabled = numWeaponModel.Enabled = optWeaponProfile.Checked;
			numWeaponProfileMarking.Enabled = optWeaponProfile.Checked && chkWeaponProfile.Checked;
		}

		void updateObjects()
		{
			var section = _hookFile.GetOrCreateSection("Objects");
			section.Entries.Clear();
			foreach (var entry in lstObjects.Items) section.AddLine(entry.ToString());
			foreach (var entry in lstHullIcon.Items) section.AddLine(entry.ToString());
			createContents();
		}
		#endregion

		#region Hangars
		void checkIndicies()
		{
			if (_tempModels != "" && _tempXs != "" && _tempYs != "" && _tempZs != "")
			{
				var models = _tempModels.Split(',');
				var xs = _tempXs.Split(',');
				var ys = _tempYs.Split(',');
				var zs = _tempZs.Split(',');
				if (xs.Length == models.Length && ys.Length == models.Length && zs.Length == models.Length)
					for (var i = 0; i < models.Length; i++) lstAutoPlayer.Items.Add(models[i] + "," + xs[i] + "," + ys[i] + "," + zs[i]);
			}
			_tempModels = "";
			_tempXs = "";
			_tempYs = "";
			_tempZs = "";
		}
		void parseHangarCamera(string line)
		{
			string[] parts = line.Replace(" ", "").Split('=');
			try
			{
				int view;
				if (parts[0].StartsWith("Key1", StringComparison.OrdinalIgnoreCase)) view = 0;
				else if (parts[0].StartsWith("Key2", StringComparison.OrdinalIgnoreCase)) view = 1;
				else if (parts[0].StartsWith("Key3", StringComparison.OrdinalIgnoreCase)) view = 2;
				else if (parts[0].StartsWith("Key6", StringComparison.OrdinalIgnoreCase)) view = 3;
				else if (parts[0].StartsWith("Key9", StringComparison.OrdinalIgnoreCase)) view = 4;
				else throw new InvalidDataException();

				int camera;
				if (parts[0].IndexOf("_X", StringComparison.OrdinalIgnoreCase) != -1) camera = 0;
				else if (parts[0].IndexOf("_Y", StringComparison.OrdinalIgnoreCase) != -1) camera = 1;
				else if (parts[0].IndexOf("_Z", StringComparison.OrdinalIgnoreCase) != -1) camera = 2;
				else throw new InvalidDataException();

				_cameras[view, camera] = int.Parse(parts[1]);
			}
			catch { _hookFile.GetOrCreateSection("HangarCamera").AddComment(line); }
		}
		void parseFamilyHangarCamera(string line)
		{
			string[] parts = line.Replace(" ", "").Split('=');
			try
			{
				int view;
				if (parts[0].StartsWith("FamKey1", StringComparison.OrdinalIgnoreCase)) view = 0;
				else if (parts[0].StartsWith("FamKey2", StringComparison.OrdinalIgnoreCase)) view = 1;
				else if (parts[0].StartsWith("FamKey3", StringComparison.OrdinalIgnoreCase)) view = 2;
				else if (parts[0].StartsWith("FamKey6", StringComparison.OrdinalIgnoreCase)) view = 3;
				else if (parts[0].StartsWith("FamKey7", StringComparison.OrdinalIgnoreCase)) view = 4;
				else if (parts[0].StartsWith("FamKey8", StringComparison.OrdinalIgnoreCase)) view = 5;
				else if (parts[0].StartsWith("FamKey9", StringComparison.OrdinalIgnoreCase)) view = 6;
				else throw new InvalidDataException();

				int camera;
				if (parts[0].IndexOf("_X", StringComparison.OrdinalIgnoreCase) != -1) camera = 0;
				else if (parts[0].IndexOf("_Y", StringComparison.OrdinalIgnoreCase) != -1) camera = 1;
				else if (parts[0].IndexOf("_Z", StringComparison.OrdinalIgnoreCase) != -1) camera = 2;
				else throw new InvalidDataException();

				_familyCameras[view, camera] = int.Parse(parts[1]);
			}
			catch { _hookFile.GetOrCreateSection("FamHangarCamera").AddComment(line); }
		}
		void parseHangarObjects(string line)
		{
			string[] parts = line.Replace(" ", "").Split('=');
			try
			{
				if (parts[0].Equals("LoadShuttle", StringComparison.OrdinalIgnoreCase)) chkShuttle.Checked = (parts[1] == "1");
				else if (parts[0].Equals("ShuttleModelIndex", StringComparison.OrdinalIgnoreCase)) cboShuttleModel.SelectedIndex = int.Parse(parts[1]);
				else if (parts[0].Equals("ShuttleMarkings", StringComparison.OrdinalIgnoreCase)) cboShuttleMarks.SelectedIndex = int.Parse(parts[1]);
				else if (parts[0].Equals("ShuttleObjectProfile", StringComparison.OrdinalIgnoreCase)) txtShuttleProfile.Text = parts[1];
				else if (parts[0].Equals("ShuttlePositionX", StringComparison.OrdinalIgnoreCase)) numShuttlePositionX.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("ShuttlePositionY", StringComparison.OrdinalIgnoreCase)) numShuttlePositionY.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("ShuttlePositionZ", StringComparison.OrdinalIgnoreCase)) numShuttlePositionZ.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("ShuttleOrientation", StringComparison.OrdinalIgnoreCase)) numShuttleOrientation.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("IsShuttleFloorInverted", StringComparison.OrdinalIgnoreCase)) chkShuttleFloor.Checked = (parts[1] != "0");
				else if (parts[0].Equals("ShuttleAnimation", StringComparison.OrdinalIgnoreCase))
					try { cboShuAnimation.SelectedIndex = (int)Enum.Parse(typeof(ShuttleAnimation), parts[1], true); }
					catch { MessageBox.Show("Error reading ShuttleAnimation, using default.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
				else if (parts[0].Equals("ShuttleAnimationStraightLine", StringComparison.OrdinalIgnoreCase)) numShuDistance.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("ShuttleAnimationElevation", StringComparison.OrdinalIgnoreCase)) numShuElevation.Value = int.Parse(parts[1]);

				else if (parts[0].Equals("LoadDroids", StringComparison.OrdinalIgnoreCase)) chkDroids.Checked = (parts[1] == "1");
				else if (parts[0].Equals("LoadDroid1", StringComparison.OrdinalIgnoreCase)) chkLoadDroid1.Checked = (parts[1] == "1");
				else if (parts[0].Equals("LoadDroid2", StringComparison.OrdinalIgnoreCase)) chkLoadDroid2.Checked = (parts[1] == "1");
				else if (parts[0].Equals("DroidsPositionZ", StringComparison.OrdinalIgnoreCase)) numDroidsZ.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("Droid1PositionZ", StringComparison.OrdinalIgnoreCase)) numDroid1Z.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("Droid2PositionZ", StringComparison.OrdinalIgnoreCase)) numDroid2Z.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("IsDroidsFloorInverted", StringComparison.OrdinalIgnoreCase)) chkDroidsFloor.Checked = (parts[1] != "0");
				else if (parts[0].Equals("IsDroid1FloorInverted", StringComparison.OrdinalIgnoreCase)) chkDroid1Floor.Checked = (parts[1] != "0");
				else if (parts[0].Equals("IsDroid2FloorInverted", StringComparison.OrdinalIgnoreCase)) chkDroid2Floor.Checked = (parts[1] != "0");
				else if (parts[0].Equals("Droid1Update", StringComparison.OrdinalIgnoreCase)) chkDroid1Update.Checked = (parts[1] != "0");
				else if (parts[0].Equals("Droid2Update", StringComparison.OrdinalIgnoreCase)) chkDroid2Update.Checked = (parts[1] != "0");
				else if (parts[0].Equals("Droid1ModelIndex", StringComparison.OrdinalIgnoreCase)) cboDroid1Model.SelectedIndex = int.Parse(parts[1]);
				else if (parts[0].Equals("Droid1Markings", StringComparison.OrdinalIgnoreCase)) cboDroid1Markings.SelectedIndex = int.Parse(parts[1]);
				else if (parts[0].Equals("Droid1ObjectProfile", StringComparison.OrdinalIgnoreCase)) txtDroid1Profile.Text = parts[1];
				else if (parts[0].Equals("Droid2ModelIndex", StringComparison.OrdinalIgnoreCase)) cboDroid2Model.SelectedIndex = int.Parse(parts[1]);
				else if (parts[0].Equals("Droid2Markings", StringComparison.OrdinalIgnoreCase)) cboDroid2Markings.SelectedIndex = int.Parse(parts[1]);
				else if (parts[0].Equals("Droid2ObjectProfile", StringComparison.OrdinalIgnoreCase)) txtDroid2Profile.Text = parts[1];

				else if (parts[0].Equals("HangarRoofCranePositionX", StringComparison.OrdinalIgnoreCase)) numRoofCranePositionX.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("HangarRoofCranePositionY", StringComparison.OrdinalIgnoreCase)) numRoofCranePositionY.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("HangarRoofCranePositionZ", StringComparison.OrdinalIgnoreCase)) numRoofCranePositionZ.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("HangarRoofCraneAxis", StringComparison.OrdinalIgnoreCase))
				{
					if (int.Parse(parts[1]) == 1) optRoofCraneAxisY.Checked = true;
					else if (int.Parse(parts[1]) == 2) optRoofCraneAxisZ.Checked = true;
				}
				else if (parts[0].Equals("HangarRoofCraneLowOffset", StringComparison.OrdinalIgnoreCase)) numRoofCraneLowOffset.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("HangarRoofCraneHighOffset", StringComparison.OrdinalIgnoreCase)) numRoofCraneHighOffset.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("IsHangarFloorInverted", StringComparison.OrdinalIgnoreCase)) chkFloor.Checked = (parts[1] != "0");
				else if (parts[0].Equals("HangarFloorInvertedHeight", StringComparison.OrdinalIgnoreCase)) numInvertedHangarFloor.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("HangarIff", StringComparison.OrdinalIgnoreCase))
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
				else if (parts[0].Equals("DrawShadows", StringComparison.OrdinalIgnoreCase)) chkShadows.Checked = (parts[1] != "0");
				else if (parts[0].Equals("LightColorRgb", StringComparison.OrdinalIgnoreCase))
				{
					txtLightColor.Text = parts[1];
					pctLight.BackColor = System.Drawing.ColorTranslator.FromHtml("#" + txtLightColor.Text);
				}
				else if (parts[0].Equals("LightColorIntensity", StringComparison.OrdinalIgnoreCase)) numIntensity.Value = int.Parse(parts[1]);

				else if (parts[0].Equals("PlayerAnimationElevation", StringComparison.OrdinalIgnoreCase)) numPlayerAnimationElevation.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("PlayerAnimationStraightLine", StringComparison.OrdinalIgnoreCase)) numPlayerStraight.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("PlayerOffsetX", StringComparison.OrdinalIgnoreCase)) numPlayerX.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("PlayerOffsetY", StringComparison.OrdinalIgnoreCase)) numPlayerY.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("PlayerOffsetZ", StringComparison.OrdinalIgnoreCase)) numPlayerZ.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("PlayerModelIndices", StringComparison.OrdinalIgnoreCase)) _tempModels = parts[1];
				else if (parts[0].Equals("PlayerOffsetsX", StringComparison.OrdinalIgnoreCase)) _tempXs = parts[1];
				else if (parts[0].Equals("PlayerOffsetsY", StringComparison.OrdinalIgnoreCase)) _tempYs = parts[1];
				else if (parts[0].Equals("PlayerOffsetsZ", StringComparison.OrdinalIgnoreCase)) _tempZs = parts[1];
				else if (parts[0].Equals("IsPlayerFloorInverted", StringComparison.OrdinalIgnoreCase)) chkPlayerFloor.Checked = (parts[1] != "0");
				else if (parts[0].Equals("PlayerAnimationInvertedElevation", StringComparison.OrdinalIgnoreCase)) numInvertedPlayerFloor.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("PlayerInvertedOffsetX", StringComparison.OrdinalIgnoreCase)) numInvertedPlayerX.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("PlayerInvertedOffsetY", StringComparison.OrdinalIgnoreCase)) numInvertedPlayerY.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("PlayerInvertedOffsetZ", StringComparison.OrdinalIgnoreCase)) numInvertedPlayerZ.Value = int.Parse(parts[1]);
				else if (parts[0].Equals("PlayerFloorInvertedModelIndices", StringComparison.OrdinalIgnoreCase))
				{
					var models = parts[1].Split(',');
					for (int i = 0; i < models.Length; i++) lstAutoPlayer.Items.Add("Inverted," + models[i]);
				}

				else if (parts[0].StartsWith(_fm, StringComparison.OrdinalIgnoreCase)) lstHangarObjects.Items.Add(parts[1]);
				else throw new InvalidDataException();
			}
			catch { _hookFile.GetOrCreateSection("HangarObjects").AddComment(line); }
			checkIndicies();
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

		private void chkFamGrounded_CheckedChanged(object sender, EventArgs e) => numFamPosZ.Enabled = !chkFamGrounded.Checked;
		private void chkFamMarks_CheckedChanged(object sender, EventArgs e) => cboFamMapMarkings.Enabled = chkFamMarks.Checked;
		private void chkGrounded_CheckedChanged(object sender, EventArgs e) => numPosZ.Enabled = !chkGrounded.Checked;
		private void chkHangarIff_CheckedChanged(object sender, EventArgs e)
		{
			cboHangarIff.Enabled = chkHangarIff.Checked;
			if (chkHangarIff.Checked && cboHangarIff.SelectedIndex == -1) cboHangarIff.SelectedIndex = 0;
			updateHangarObjects();
		}
		private void chkMarks_CheckedChanged(object sender, EventArgs e) => cboMapMarkings.Enabled = chkMarks.Checked;
		private void chkShuttle_CheckedChanged(object sender, EventArgs e)
		{
			pnlShuttle.Enabled = chkShuttle.Checked;
			updateHangarObjects();
		}
		private void chkLoadDroid1_CheckedChanged(object sender, EventArgs e)
		{
			grpDroid1.Enabled = chkLoadDroid1.Checked;
			updateHangarObjects();
		}
		private void chkLoadDroid2_CheckedChanged(object sender, EventArgs e)
		{
			grpDroid2.Enabled = chkLoadDroid2.Checked;
			updateHangarObjects();
		}
		private void chkDroids_CheckedChanged(object sender, EventArgs e)
		{
			pnlDroids.Enabled = chkDroids.Checked;
			updateHangarObjects();
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
			updateMapSectionFromLst("FamHangarMap", lstFamilyMap);
		}
		private void cmdAddHangar_Click(object sender, EventArgs e)
		{
			if (_installDirectory != "") opnObjects.InitialDirectory = _installDirectory + _fm;
			opnObjects.Title = "Select original object...";
			DialogResult res = opnObjects.ShowDialog();
			if (res != DialogResult.OK) return;

			string line = opnObjects.FileName.Substring(opnObjects.FileName.IndexOf(_fm)) + " = ";
			opnObjects.Title = "Select new object...";
			res = opnObjects.ShowDialog();
			if (res != DialogResult.OK) return;

			lstHangarObjects.Items.Add(line + opnObjects.FileName.Substring(opnObjects.FileName.IndexOf(_fm)));
			updateHangarObjects();
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
			updateMapSectionFromLst("HangarMap", lstMap);
		}
		private void cmdAutoAdd_Click(object sender, EventArgs e)
		{
			if (chkAutoInvert.Checked) lstAutoPlayer.Items.Add("Inverted," + cboAutoModel.Text);
			else lstAutoPlayer.Items.Add($"{cboAutoModel.Text},{numAutoX.Value},{numAutoY.Value},{numAutoZ.Value}");
			updateHangarObjects();
		}
		private void cmdCraneReset_Click(object sender, EventArgs e)
		{
			bool loading = _loading;
			_loading = true;
			numRoofCranePositionX.Value = _defaultRoofCranePosition[0];
			numRoofCranePositionY.Value = _defaultRoofCranePosition[1];
			numRoofCranePositionZ.Value = _defaultRoofCranePosition[2];
			_loading = loading;
			updateHangarObjects();
		}
		private void cmdDefaultCamera_Click(object sender, EventArgs e)
		{
			bool loading = _loading;
			_loading = true;
			numCameraX.Value = _defaultCameras[cboCamera.SelectedIndex, 0];
			numCameraY.Value = _defaultCameras[cboCamera.SelectedIndex, 1];
			numCameraZ.Value = _defaultCameras[cboCamera.SelectedIndex, 2];
			_loading = loading;
			updateCamera();
		}
		private void cmdDefaultFamilyCamera_Click(object sender, EventArgs e)
		{
			bool loading = _loading;
			_loading = true;
			numFamilyCameraX.Value = _defaultFamilyCameras[cboFamilyCamera.SelectedIndex, 0];
			numFamilyCameraY.Value = _defaultFamilyCameras[cboFamilyCamera.SelectedIndex, 1];
			numFamilyCameraZ.Value = _defaultFamilyCameras[cboFamilyCamera.SelectedIndex, 2];
			_loading = loading;
			updateFamCamera();
		}
		private void cmdPlayerReset_Click(object sender, EventArgs e)
		{
			bool loading = _loading;
			_loading = true;
			numPlayerX.Value = 0;
			numPlayerY.Value = 0;
			numPlayerZ.Value = 0;
			_loading = loading;
			updateHangarObjects();
		}
		private void cmdInvertedPlayerReset_Click(object sender, EventArgs e)
		{
			bool loading = _loading;
			_loading = true;
			numInvertedPlayerX.Value = 0;
			numInvertedPlayerY.Value = 0;
			numInvertedPlayerZ.Value = 0;
			_loading = loading;
			updateHangarObjects();
		}
		private void cmdRemoveFamMap_Click(object sender, EventArgs e)
		{
			if (lstFamilyMap.SelectedIndex == -1) return;
			
			if (lstFamilyMap.Items.Count == 4)    // warn here only when initially dropping below 4
			{
				DialogResult res = MessageBox.Show("Family Hangar Map requires at least 4 line items to be saved. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (res == DialogResult.No) return;
			}

			lstFamilyMap.Items.RemoveAt(lstFamilyMap.SelectedIndex);
			updateMapSectionFromLst("FamHangarMap", lstFamilyMap);
		}
		private void cmdRemoveHangar_Click(object sender, EventArgs e)
		{
			if (lstHangarObjects.SelectedIndex == -1) return;

			lstHangarObjects.Items.RemoveAt(lstHangarObjects.SelectedIndex);
			updateHangarObjects();
		}
		private void cmdRemoveMap_Click(object sender, EventArgs e)
		{
			if (lstMap.SelectedIndex == -1) return;
			
			if (lstMap.Items.Count == 4)    // warn here only when initially dropping below 4
			{
				DialogResult res = MessageBox.Show("Hangar Map requires at least 4 line items to be saved. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (res == DialogResult.No) return;
			}

			lstMap.Items.RemoveAt(lstMap.SelectedIndex);
			updateMapSectionFromLst("HangarMap", lstMap);
		}
		private void cmdAutoRemove_Click(object sender, EventArgs e)
		{
			if (lstAutoPlayer.SelectedIndex == -1) return;

			lstAutoPlayer.Items.RemoveAt(lstAutoPlayer.SelectedIndex);
			updateHangarObjects();
		}
		private void cmdShuttleReset_Click(object sender, EventArgs e)
		{
			bool loading = _loading;
			_loading = true;
			numShuttlePositionX.Value = _defaultShuttlePosition[0];
			numShuttlePositionY.Value = _defaultShuttlePosition[1];
			numShuttlePositionZ.Value = _defaultShuttlePosition[2];
			numShuttleOrientation.Value = _defaultShuttlePosition[3];
			_loading = loading;
			updateHangarObjects();
		}

		private void numCameraX_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_cameras[cboCamera.SelectedIndex, 0] = (int)numCameraX.Value;
			updateCamera();
		}
		private void numCameraY_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_cameras[cboCamera.SelectedIndex, 1] = (int)numCameraY.Value;
			updateCamera();
		}
		private void numCameraZ_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_cameras[cboCamera.SelectedIndex, 2] = (int)numCameraZ.Value;
			updateCamera();
		}
		private void numFamilyCameraX_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_familyCameras[cboFamilyCamera.SelectedIndex, 0] = (int)numFamilyCameraX.Value;
			updateFamCamera();
		}
		private void numFamilyCameraY_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_familyCameras[cboFamilyCamera.SelectedIndex, 1] = (int)numFamilyCameraY.Value;
			updateFamCamera();
		}
		private void numFamilyCameraZ_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_familyCameras[cboFamilyCamera.SelectedIndex, 2] = (int)numFamilyCameraZ.Value;
			updateFamCamera();
		}

		private void txtLightColor_Leave(object sender, EventArgs e)
		{
			Regex rx = new Regex(@"([A-F0-9]){3}((A-F0-9]){3})?", RegexOptions.Compiled);
			Match match = rx.Match(txtLightColor.Text);
			if (!match.Success) txtLightColor.Text = "FFFFFF";
			if (txtLightColor.Text.Length == 3)
			{
				// just to shorten the code line...
				string t = txtLightColor.Text;
				txtLightColor.Text = t.Substring(0, 1) + t.Substring(0, 1) + t.Substring(1, 1) + t.Substring(1, 1) + t.Substring(2, 1) + t.Substring(2, 1);
			}
			else if (txtLightColor.Text.Length != 6) txtLightColor.Text = "FFFFFF";
			pctLight.BackColor = System.Drawing.ColorTranslator.FromHtml("#" + txtLightColor.Text);
			updateHangarObjects();
		}

		void uiHangarObj_DefaultEvent(object sender, EventArgs e) => updateHangarObjects();

		void updateCamera()
		{
			if (_loading) return;

			var section = _hookFile.GetOrCreateSection("HangarCamera");
			section.Entries.Clear();
			string[] keys = { "1", "2", "3", "6", "9" };
			for (int i = 0; i < 5; i++)
			{
				bool use = false;
				for (int j = 0; j < 3; j++) use |= (_cameras[i, j] != _defaultCameras[i, j]);
				if (use)
				{
					section.AddLine($"Key{keys[i]}_X = {_cameras[i, 0]}");
					section.AddLine($"Key{keys[i]}_Y = {_cameras[i, 1]}");
					section.AddLine($"Key{keys[i]}_Z = {_cameras[i, 2]}");
				}
			}
			createContents();
		}
		void updateFamCamera()
		{
			if (_loading) return;

			var section = _hookFile.GetOrCreateSection("FamHangarCamera");
			section.Entries.Clear();
			string[] keys = { "1", "2", "3", "6", "7", "8", "9" };
			for (int i = 0; i < 7; i++)
			{
				bool use = false;
				for (int j = 0; j < 3; j++) use |= (_familyCameras[i, j] != _defaultFamilyCameras[i, j]);
				if (use)
				{
					section.AddLine($"FamKey{keys[i]}_X = {_familyCameras[i, 0]}");
					section.AddLine($"FamKey{keys[i]}_Y = {_familyCameras[i, 1]}");
					section.AddLine($"FamKey{keys[i]}_Z = {_familyCameras[i, 2]}");
				}
			}
			createContents();
		}
		void updateHangarObjects()
		{
			if (_loading) return;

			var section = _hookFile.GetOrCreateSection("HangarObjects");
			section.Entries.Clear();
			if (!chkShuttle.Checked) section.AddLine("LoadShuttle = 0");
			else
			{
				if (cboShuttleModel.SelectedIndex != 50) section.AddLine($"ShuttleModelIndex = {cboShuttleModel.SelectedIndex}");
				if (cboShuttleMarks.SelectedIndex != 0) section.AddLine($"ShuttleMarkings = {cboShuttleMarks.SelectedIndex}");
				if (txtShuttleProfile.Text != "") section.AddLine($"ShuttleObjectProfile = {txtShuttleProfile.Text}");
				if (numShuttlePositionX.Value != _defaultShuttlePosition[0]) section.AddLine($"ShuttlePositionX = {(int)numShuttlePositionX.Value}");
				if (numShuttlePositionY.Value != _defaultShuttlePosition[1]) section.AddLine($"ShuttlePositionY = {(int)numShuttlePositionY.Value}");
				if (numShuttlePositionZ.Value != _defaultShuttlePosition[2]) section.AddLine($"ShuttlePositionZ = {(int)numShuttlePositionZ.Value}");
				if (numShuttleOrientation.Value != _defaultShuttlePosition[3]) section.AddLine($"ShuttleOrientation = {(int)numShuttleOrientation.Value}");
				if (chkShuttleFloor.Checked) section.AddLine("IsShuttleFloorInverted = 1");
				if (cboShuAnimation.SelectedIndex != 0) section.AddLine($"ShuttleAnimation = {cboShuAnimation.Text}");
				if (numShuDistance.Value != 0) section.AddLine($"ShuttleAnimationStraightLine = {(int)numShuDistance.Value}");
				if (numShuElevation.Value != 0) section.AddLine($"ShuttleAnimationElevation = {(int)numShuElevation.Value}");
			}

			if (!chkDroids.Checked) section.AddLine("LoadDroids = 0");
			else
			{
				if (numDroidsZ.Value != 0) section.AddLine($"DroidsPositionZ = {(int)numDroidsZ.Value}");
				if (chkDroidsFloor.Checked) section.AddLine("IsDroidsFloorInverted = 1");
				if (!chkLoadDroid1.Checked) section.AddLine("LoadDroid1 = 0");
				else
				{
					if (numDroid1Z.Value != numDroidsZ.Value) section.AddLine($"Droid1PositionZ = {(int)numDroid1Z.Value}");
					if (chkDroid1Floor.Checked) section.AddLine("IsDroid1FloorInverted = 1");
					if (!chkDroid1Update.Checked) section.AddLine("Droid1Update = 0");
					if (cboDroid1Model.SelectedIndex != 311) section.AddLine($"Droid1ModelIndex = {cboDroid1Model.SelectedIndex}");
					if (cboDroid1Markings.SelectedIndex != 0) section.AddLine($"Droid1Markings = {cboDroid1Markings.SelectedIndex}");
					if (txtDroid1Profile.Text != "") section.AddLine($"Droid1ObjectProfile = {txtDroid1Profile.Text}");
				}
				if (!chkLoadDroid2.Checked) section.AddLine("LoadDroid2 = 0");
				else
				{
					if (numDroid2Z.Value != numDroidsZ.Value) section.AddLine($"Droid2PositionZ = {(int)numDroid2Z.Value}");
					if (chkDroid2Floor.Checked) section.AddLine("IsDroid2FloorInverted = 1");
					if (!chkDroid2Update.Checked) section.AddLine("Droid2Update = 0");
					if (cboDroid2Model.SelectedIndex != 312) section.AddLine($"Droid2ModelIndex = {cboDroid2Model.SelectedIndex}");
					if (cboDroid2Markings.SelectedIndex != 0) section.AddLine($"Droid2Markings = {cboDroid2Markings.SelectedIndex}");
					if (txtDroid2Profile.Text != "") section.AddLine($"Droid2ObjectProfile = {txtDroid2Profile.Text}");
				}
			}

			if (numRoofCranePositionX.Value != _defaultRoofCranePosition[0]) section.AddLine($"HangarRoofCranePositionX = {(int)numRoofCranePositionX.Value}");
			if (numRoofCranePositionY.Value != _defaultRoofCranePosition[1]) section.AddLine($"HangarRoofCranePositionY = {(int)numRoofCranePositionY.Value}");
			if (numRoofCranePositionZ.Value != _defaultRoofCranePosition[2]) section.AddLine($"HangarRoofCranePositionZ = {(int)numRoofCranePositionZ.Value}");
			if (optRoofCraneAxisY.Checked) section.AddLine("HangarRoofCraneAxis = 1");
			else if (optRoofCraneAxisZ.Checked) section.AddLine("HangarRoofCraneAxis = 2");
			if (numRoofCraneLowOffset.Value != 0) section.AddLine($"HangarRoofCraneLowOffset = {(int)numRoofCraneLowOffset.Value}");
			if (numRoofCraneHighOffset.Value != 0) section.AddLine($"HangarRoofCraneHighOffset = {(int)numRoofCraneHighOffset.Value}");
			if (chkFloor.Checked) section.AddLine("IsHangarFloorInverted = 1");
			if (numInvertedHangarFloor.Value != 0) section.AddLine($"HangarFloorInvertedHeight = {(int)numInvertedHangarFloor.Value}");
			if (chkShadows.Checked == chkFloor.Checked) section.AddLine($"DrawShadows = {(chkShadows.Checked ? "1" : "0")}");
			if (numIntensity.Value != 192) section.AddLine($"LightColorIntensity = {(int)numIntensity.Value}");
			if (txtLightColor.Text != "FFFFFF") section.AddLine($"LightColorRgb = {txtLightColor.Text}");
			if (chkHangarIff.Checked) section.AddLine($"HangarIff = {cboHangarIff.SelectedIndex}");

			if (numPlayerAnimationElevation.Value != 0) section.AddLine($"PlayerAnimationElevation = {(int)numPlayerAnimationElevation.Value}");
			if (numPlayerStraight.Value != 0) section.AddLine($"PlayerAnimationStraightLine = {(int)numPlayerStraight.Value}");
			if (numPlayerX.Value != 0) section.AddLine($"PlayerOffsetX = {(int)numPlayerX.Value}");
			if (numPlayerY.Value != 0) section.AddLine($"PlayerOffsetY = {(int)numPlayerY.Value}");
			if (numPlayerZ.Value != 0) section.AddLine($"PlayerOffsetZ = {(int)numPlayerZ.Value}");
			if (chkPlayerFloor.Checked) section.AddLine("IsPlayerFloorInverted = 1");
			if (numInvertedPlayerFloor.Value != numPlayerAnimationElevation.Value) section.AddLine($"PlayerAnimationInvertedElevation = {(int)numInvertedPlayerFloor.Value}");
			if (numInvertedPlayerX.Value != 0) section.AddLine($"PlayerInvertedOffsetX = {(int)numInvertedPlayerX.Value}");
			if (numInvertedPlayerY.Value != 0) section.AddLine($"PlayerInvertedOffsetY = {(int)numInvertedPlayerY.Value}");
			if (numInvertedPlayerZ.Value != 0) section.AddLine($"PlayerInvertedOffsetZ = {(int)numInvertedPlayerZ.Value}");
			if (lstAutoPlayer.Items.Count > 0)
			{
				string invert = "";
				string model = "";
				string x = "";
				string y = "";
				string z = "";
				foreach (object entry in lstAutoPlayer.Items)
				{
					string item = entry.ToString();
					if (item.StartsWith("Inverted")) invert += (invert.Length != 0 ? ", " : "") + item.Split(',')[1];
					else
					{
						var parts = item.Split(',');
						if (parts.Length == 4)
						{
							model += (model.Length != 0 ? ", " : "") + parts[0];
							x += (x.Length != 0 ? ", " : "") + parts[1];
							y += (y.Length != 0 ? ", " : "") + parts[2];
							z += (z.Length != 0 ? ", " : "") + parts[3];
						}
					}
				}
				if (invert != "") section.AddLine($"PlayerFloorInvertedModelIndices = {invert}");
				if (model != "")
				{
					section.AddLine($"PlayerModelIndices = {model}");
					section.AddLine($"PlayerOffsetsX = {x}");
					section.AddLine($"PlayerOffsetsY = {y}");
					section.AddLine($"PlayerOffsetsZ = {z}");
				}
			}

			foreach (object entry in lstHangarObjects.Items) section.AddLine(entry.ToString());

			createContents();
		}
		void updateMapSectionFromLst(string name, ListBox lst)
		{
			if (_loading) return;

			var section = _hookFile.GetOrCreateSection(name);
			section.Entries.Clear();
			if (lst.Items.Count >= 4)
				foreach (var entry in lst.Items) section.AddLine(entry.ToString());
			createContents();
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
					chkFloor.Checked || numInvertedHangarFloor.Value != 0 || chkShadows.Checked == chkFloor.Checked || chkHangarIff.Checked || txtLightColor.Text != "FFFFFF" || numIntensity.Value != 192 ||
					(numPlayerAnimationElevation.Value != 0) || (numPlayerStraight.Value != 0) || (numPlayerX.Value != 0) || (numPlayerY.Value != 0) || (numPlayerZ.Value != 0) || chkPlayerFloor.Checked ||
					numInvertedPlayerFloor.Value != numPlayerAnimationElevation.Value || (numInvertedPlayerX.Value != 0) || (numInvertedPlayerY.Value != 0) || (numInvertedPlayerZ.Value != 0) || lstAutoPlayer.Items.Count > 0;
			}
		}
		bool useHangarMap => lstMap.Items.Count >= 4;
		bool useFamilyHangarMap => lstFamilyMap.Items.Count >= 4;
		#endregion

		#region S-Foils
		private void cmdAddSFoils_Click(object sender, EventArgs e)
		{
			if (cboSFoilFG.SelectedIndex == -1) return;

			if (chkCloseSF.Checked) lstSFoils.Items.Add(cboSFoilFG.Text + ",closed");
			if (chkOpenLG.Checked) lstSFoils.Items.Add(cboSFoilFG.Text + ",open");
			updateMissionTie();
		}
		private void cmdRemoveSFoils_Click(object sender, EventArgs e)
		{
			if (lstSFoils.SelectedIndex == -1) return;

			lstSFoils.Items.RemoveAt(lstSFoils.SelectedIndex);
			updateMissionTie();
		}

		bool useSFoils => (lstSFoils.Items.Count > 0 || chkForceHangarSF.Checked || chkForceHyperLG.Checked || chkManualSF.Checked);
		#endregion

		#region Skins
		private void chkDefaultSkin_CheckedChanged(object sender, EventArgs e) => txtSkin.Enabled = !chkDefaultSkin.Checked;
		private void chkSkinMarks_CheckedChanged(object sender, EventArgs e) => cboSkinMarks.Enabled = chkSkinMarks.Checked;
		private void chkOpacity_CheckedChanged(object sender, EventArgs e) => numOpacity.Enabled = chkOpacity.Checked;

		private void cmdAddSkin_Click(object sender, EventArgs e)
		{
			if (_installDirectory != "") opnObjects.InitialDirectory = _installDirectory + _fm;
			opnObjects.Title = "Select affected OPT...";
			DialogResult res = opnObjects.ShowDialog();
			if (res != DialogResult.OK) return;
			
			string line = $"{Path.GetFileNameWithoutExtension(opnObjects.FileName)}{(chkSkinMarks.Checked ? "_fgc_" + cboSkinMarks.SelectedIndex : "")} = {(chkDefaultSkin.Checked ? "Default" + (chkSkinMarks.Checked ? "_" + cboSkinMarks.SelectedIndex : "") : txtSkin.Text)}{(chkOpacity.Checked ? "-" + (int)numOpacity.Value : "")}";
			lstSkins.Items.Add(line);
			updateSectionFromLst("Skins", lstSkins);
		}
		private void cmdAppendSkin_Click(object sender, EventArgs e)
		{
			if (lstSkins.SelectedIndex == -1) return;

			string line = lstSkins.SelectedItem.ToString();
			line += $", {(chkDefaultSkin.Checked ? "Default" + (chkSkinMarks.Checked ? "_" + cboSkinMarks.SelectedIndex : "") : txtSkin.Text)}{(chkOpacity.Checked ? "-" + (int)numOpacity.Value : "")}";
			lstSkins.Items[lstSkins.SelectedIndex] = line;
			updateSectionFromLst("Skins", lstSkins);
		}
		private void cmdRemoveSkin_Click(object sender, EventArgs e)
		{
			if (lstSkins.SelectedIndex == -1) return;

			lstSkins.Items.RemoveAt(lstSkins.SelectedIndex);
			updateSectionFromLst("Skins", lstSkins);
		}
		#endregion Skins

		#region Shield
		void parseShield(string line)
		{
			string[] parts;
			try
			{
				if (line.StartsWith("IsShieldRechargeForStarshipsEnabled", StringComparison.OrdinalIgnoreCase))
				{
					parts = line.Split('=');
					if (parts.Length > 1 && int.Parse(parts[1]) == 0) chkSSRecharge.Checked = false;
					return;
				}
				if (line.StartsWith("IsShieldStrengthForStarfighterDoubled", StringComparison.OrdinalIgnoreCase))
				{
					parts = line.Split('=');
					if (parts.Length > 1 && int.Parse(parts[1]) == 1) chkFighterDoubled.Checked = true;
					return;
				}

				parts = line.Split(',');
				bool perGen = (parts[1] == "1");
				int rate = (perGen ? int.Parse(parts[2]) : int.Parse(parts[3]));
				lstShield.Items.Add(Strings.CraftType[int.Parse(parts[0])] + " = " + rate + (perGen ? " per" : ""));
			}
			catch { _hookFile.GetOrCreateSection("Shield").AddComment(line); }
		}

		private void cmdAddShield_Click(object sender, EventArgs e)
		{
			if (cboShield.SelectedIndex < 1) return;

			string line = $"{cboShield.Text} = {Math.Round(numShieldRate.Value)}{(chkShieldGen.Checked ? " per" : "")}";
			lstShield.Items.Add(line);
			updateShield();
		}
		private void cmdRemoveShield_Click(object sender, EventArgs e)
		{
			if (lstShield.SelectedIndex == -1) return;

			lstShield.Items.RemoveAt(lstShield.SelectedIndex);
			updateShield();
		}

		void uiShield_DefaultEvent(object sender, EventArgs e) => updateShield();

		void updateShield()
		{
			if (_loading) return;

			var section = _hookFile.GetOrCreateSection("Shield");
			section.Entries.Clear();
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
				section.AddLine($"{craft}, {(perGen ? $"1, {rate}, 0" : $"0, 0, {rate}")}");
			}
			if (!chkSSRecharge.Checked) section.AddLine("IsShieldRechargeForStarshipsEnabled = 0");
			if (chkFighterDoubled.Checked) section.AddLine("IsShieldStrengthForStarfighterDoubled = 1");
			createContents();
		}

		bool useShields => lstShield.Items.Count > 0 || chkFighterDoubled.Checked || !chkSSRecharge.Checked;
		#endregion

		#region Hyper
		void parseHyper(string line)
		{
			string[] parts = line.Replace(" ", "").Split('=');
			try
			{
				if (parts[0].Equals("ShortHyperspaceEffect", StringComparison.OrdinalIgnoreCase) && parts.Length == 2)
				{
					int value = int.Parse(parts[1]);
					if (value == -1) optHypGlobal.Checked = true;
					else if (value == 0) optHypNormal.Checked = true;
					else if (value == 1) optHypEnabled.Checked = true;
					else throw new InvalidDataException();
				}
				else throw new InvalidDataException();
			}
			catch { _hookFile.GetOrCreateSection("Hyperspace").AddComment(line); }
		}

		private void optHyp_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			var section = _hookFile.GetOrCreateSection("Hyperspace");
			if (!optHypGlobal.Checked) section.AddLine($"ShortHyperspaceEffect = {(optHypNormal.Checked ? "0" : "1")}");
			createContents();
		}
		#endregion

		#region Concourse
		void parseConcourse(string line)
		{
			string[] parts = line.Replace(" ", "").Split('=');

			try
			{
				if (parts.Length < 2) throw new InvalidDataException();

				if (parts[0].Equals("FrontPlanetIndex", StringComparison.OrdinalIgnoreCase))
				{
					int value = int.Parse(parts[1]);
					chkConcoursePlanetIndex.Checked = (value != -1);
					if (chkConcoursePlanetIndex.Checked) numConcoursePlanetIndex.Value = value;
				}
				else if (parts[0].Equals("FrontPlanetPositionX", StringComparison.OrdinalIgnoreCase))
				{
					int value = int.Parse(parts[1]);
					chkConcoursePlanetX.Checked = (value != -1);
					if (chkConcoursePlanetX.Checked) numConcoursePlanetX.Value = value;
				}
				else if (parts[0].Equals("FrontPlanetPositionY", StringComparison.OrdinalIgnoreCase))
				{
					int value = int.Parse(parts[1]);
					chkConcoursePlanetY.Checked = (value != -1);
					if (chkConcoursePlanetY.Checked) numConcoursePlanetY.Value = value;
				}
				else throw new InvalidDataException();
			}
			catch { _hookFile.GetOrCreateSection("Concourse").AddComment(line); }
		}

		private void chkConcoursePlanetIndex_CheckedChanged(object sender, EventArgs e)
		{
			numConcoursePlanetIndex.Enabled = chkConcoursePlanetIndex.Checked;
			updateConcourse();
		}
		private void chkConcoursePlanetX_CheckedChanged(object sender, EventArgs e)
		{
			numConcoursePlanetX.Enabled = chkConcoursePlanetX.Checked;
			updateConcourse();
		}
		private void chkConcoursePlanetY_CheckedChanged(object sender, EventArgs e)
		{
			numConcoursePlanetY.Enabled = chkConcoursePlanetY.Checked;
			updateConcourse();
		}

		private void numConcourse_ValueChanged(object sender, EventArgs e) => updateConcourse();

		void updateConcourse()
		{
			if (_loading) return;

			var section = _hookFile.GetOrCreateSection("Concourse");
			section.Entries.Clear();
			if (chkConcoursePlanetIndex.Checked) section.AddLine($"FrontPlanetIndex = {numConcoursePlanetIndex.Value}");
			if (chkConcoursePlanetX.Checked) section.AddLine($"FrontPlanetPositionX = {numConcoursePlanetX.Value}");
			if (chkConcoursePlanetY.Checked) section.AddLine($"FrontPlanetPositionY = {numConcoursePlanetY.Value}");
			createContents();
		}
		#endregion

		#region HullIcon
		void parseHullIcon(string line)
		{
			string[] parts = line.Replace(" ", "").Split('=');
			try
			{
				if (parts.Length < 2) throw new InvalidDataException();

				if (parts[0].Equals("PlayerHullIcon", StringComparison.OrdinalIgnoreCase))
				{
					int value = int.Parse(parts[1]);
					chkPlayerHull.Checked = value > 0;
					if (chkPlayerHull.Checked) numPlayerHull.Value = value;
				}
				else lstHullIcon.Items.Add(line);
			}
			catch { _hookFile.GetOrCreateSection("HullIcon").AddComment(line); }
		}

		private void chkPlayerHull_CheckedChanged(object sender, EventArgs e)
		{
			numPlayerHull.Enabled = chkPlayerHull.Checked;
			updatePlayerHull();
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
			if (res != DialogResult.OK) return;
			
			string line = $"FlightModels\\{Path.GetFileNameWithoutExtension(opnObjects.FileName)}_HullIcon = {numHullIcon.Value}";
			lstHullIcon.Items.Add(line);
			updateObjects();
		}
		private void cmdHullRemove_Click(object sender, EventArgs e)
		{
			if (lstHullIcon.SelectedIndex == -1) return;

			lstHullIcon.Items.RemoveAt(lstHullIcon.SelectedIndex);
			updateObjects();
		}

		private void numPlayerHull_ValueChanged(object sender, EventArgs e) => updatePlayerHull();

		void updatePlayerHull()
		{
			if (_loading) return;

			var section = _hookFile.GetOrCreateSection("HullIcon");
			section.Entries.Clear();
			if (chkPlayerHull.Checked && numPlayerHull.Value > 0) section.AddLine($"PlayerHullIcon = {numPlayerHull.Value}");
			createContents();
		}
		#endregion

		#region Weapons
		private void cmdAddWeap_Click(object sender, EventArgs e)
		{
			if (cboWeapFG.SelectedIndex == -1) return;

			if (optWeapProfiles.Checked && txtWeapProfile.Text.ToLower() != "default")
			{
				string profile = "Profile_fg_" + cboWeapFG.SelectedIndex.ToString() + " = " + txtWeapProfile.Text;
				if (optWeapProfile.Checked) profile = "Weapon" + profile;
				else if (optWarheadProfile.Checked) profile = "Warhead" + profile;
				else if (optEnergyProfile.Checked) profile = "Energy" + profile;
				else if (optLinkingProfile.Checked) profile = "Linking" + profile;
				lstWeapons.Items.Add(profile);
			}
			else if (optWeapRate.Checked)
			{
				string prefix = "WR: ";
				string fg = "_fg_" + cboWeapFG.SelectedIndex.ToString() + " = ";
				if (chkWeapDecharge.Checked) lstWeapons.Items.Add($"{prefix}DechargeRate{fg}{numDecharge.Value}");
				if (chkWeapRecharge.Checked) lstWeapons.Items.Add($"{prefix}RechargeRate{fg}{numRecharge.Value}");
				if (chkWeapDechargePercent.Checked) lstWeapons.Items.Add($"{prefix}DechargeRatePercent{fg}{numDechargePercent.Value}");
				if (chkWeapRechargePercent.Checked) lstWeapons.Items.Add($"{prefix}RechargeRatePercent{fg}{numRechargePercent.Value}");
				if (chkTransfer.Checked) lstWeapons.Items.Add($"{prefix}EnergyTransferRate{fg}{numTransfer.Value}");
				if (chkRatePenalty.Checked) lstWeapons.Items.Add($"{prefix}EnergyTransferRatePenalty{fg}{numRatePenalty.Value}");
				if (chkTransferWeapLimit.Checked) lstWeapons.Items.Add($"{prefix}EnergyTransferWeaponLimit{fg}{numTransferWeapLimit.Value}");
				if (chkTransferShieldLimit.Checked) lstWeapons.Items.Add($"{prefix}EnergyTransferShieldLimit{fg}{numTransferShieldLimit.Value}");
				if (chkMaxTorpPass.Checked) lstWeapons.Items.Add($"{prefix}MaxTorpedoCountPerPass{fg}{numMaxTorpPass.Value}");
				if (chkMaxTorpTarget.Checked) lstWeapons.Items.Add($"{prefix}MaxTorpedoCountPerTarget{fg}{numMaxTorpTarget.Value}");
				if (!chkImpact.Checked) lstWeapons.Items.Add($"{prefix}IsImpactSpinningEnabled{fg}0");
				if (chkImpactSpeed.Checked && numImpactSpeed.Value != 100) lstWeapons.Items.Add($"{prefix}ImpactSpinningSpeedFactorPercent{fg}{numImpactSpeed.Value}");
				if (chkImpactAngle.Checked && numImpactAngle.Value != 100) lstWeapons.Items.Add($"{prefix}ImpactSpinningAngleFactorPercent{fg}{numImpactAngle.Value}");
				// TODO: MaxSystemDamages_fg_# = 1000
				// TODO: Weapon284_LaserIon_Damages_fg_# = 1
				// TODO: Weapon285_LaserIonTurbo_Damages_fg_# = 2
				// TODO: Weapon290_LaserIonTurbo_Damages_fg_# = 4
				// TODO: Weapon296_MagPulse_Damages_fg_# = 30
			}
			else if (optWarheadCounts.Checked)
			{
				string prefix = "WarheadTypeCount_fg_" + cboWeapFG.SelectedIndex.ToString() + "_";
				if (numBombs.Value != -1) lstWeapons.Items.Add($"{prefix}SpaceBombs = {numBombs.Value}");
				if (numRockets.Value != -1) lstWeapons.Items.Add($"{prefix}HeavyRockets = {numRockets.Value}");
				if (numMissiles.Value != -1) lstWeapons.Items.Add($"{prefix}Missiles = {numMissiles.Value}");
				if (numTorpedos.Value != -1) lstWeapons.Items.Add($"{prefix}ProtonTorpedos = {numTorpedos.Value}");
				if (numAdvMissiles.Value != -1) lstWeapons.Items.Add($"{prefix}AdvancedMissiles = {numAdvMissiles.Value}");
				if (numAdvTorpedos.Value != -1) lstWeapons.Items.Add($"{prefix}AdvancedTorpedos = {numAdvTorpedos.Value}");
				if (numMagPulse.Value != -1) lstWeapons.Items.Add($"{prefix}MagPulse = {numMagPulse.Value}");
				if (numIonPulse.Value != -1) lstWeapons.Items.Add($"{prefix}IonPulse = {numIonPulse.Value}");
				if (numAdvMagPulse.Value != -1) lstWeapons.Items.Add($"{prefix}AdvancedMagPulse = {numAdvMagPulse.Value}");
				if (numClusterBombs.Value != -1) lstWeapons.Items.Add($"{prefix}ClusterBombs = {numClusterBombs.Value}");
			}
			updateWeapons();
		}
		private void cmdRemWeap_Click(object sender, EventArgs e)
		{
			if (lstWeapons.SelectedIndex == -1) return;

			lstWeapons.Items.RemoveAt(lstWeapons.SelectedIndex);
			updateWeapons();
		}

		private void optWarheadCounts_CheckedChanged(object sender, EventArgs e)
		{
			pnlWarheadCounts.Visible = optWarheadCounts.Checked;
			pnlWeapRates.Visible = !optWarheadCounts.Checked;
		}
		private void optWeapRate_CheckedChanged(object sender, EventArgs e)
		{
			pnlWeapRates.Enabled = optWeapRate.Checked;
		}
		private void optWeapProfiles_CheckedChanged(object sender, EventArgs e)
		{
			pnlWeapProfiles.Enabled = optWeapProfiles.Checked;
			txtWeapProfile.Enabled = optWeapProfiles.Checked;
		}

		void updateWeapons()
		{
			if (_loading) return;

			var section = _hookFile.GetSectionByName("WeaponRates");
			section?.Entries.Clear();
			section = _hookFile.GetSectionByName("WeaponProfiles");
			section?.Entries.Clear();
			section = _hookFile.GetSectionByName("WarheadProfiles");
			section?.Entries.Clear();
			section = _hookFile.GetSectionByName("EnergyProfiles");
			section?.Entries.Clear();
			section = _hookFile.GetSectionByName("LinkingProfiles");
			section?.Entries.Clear();
			section = _hookFile.GetSectionByName("WarheadTypeCount");
			section?.Entries.Clear();
			foreach (var entry in lstWeapons.Items)
			{
				section = null;
				string line = entry.ToString();
				if (line.StartsWith("WR:"))
				{
					section = _hookFile.GetOrCreateSection("WeaponRates");
					line = line.Substring(4);
				}
				if (line.StartsWith("WeaponProfile")) section = _hookFile.GetOrCreateSection("WeaponProfiles");
				if (line.StartsWith("WarheadProfile")) section = _hookFile.GetOrCreateSection("WarheadProfiles");
				if (line.StartsWith("EnergyProfile")) section = _hookFile.GetOrCreateSection("EnergyProfiles");
				if (line.StartsWith("LinkingProfile")) section = _hookFile.GetOrCreateSection("LinkingProfiles");
				if (line.StartsWith("WarheadTypeCount")) section = _hookFile.GetOrCreateSection("WarheadTypeCount");
				section.AddLine(line);
			}
			createContents();
		}
		#endregion

		private void cboHook_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboHook.SelectedIndex == -1) return;

			for (int i = 0; i < _panels.Length; i++) _panels[i].Visible = (i == cboHook.SelectedIndex);
			lblNotFound.Visible = cboHook.Text.StartsWith("*");
		}

		private void cmdCancel_Click(object sender, EventArgs e) => Close();
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

			if (lstBackdrops.Items.Count == 0 && _bdFile != "") File.Delete(_bdFile);

			if (lstMission.Items.Count == 0 && !useSFoils && lstCraftText.Items.Count == 0 && !useMissionSettings && _missionTxtFile != "") File.Delete(_missionTxtFile);

			if (lstSounds.Items.Count == 0 && _soundFile != "") File.Delete(_soundFile);
			if (!useInterdiction && _interdictionFile != "") File.Delete(_interdictionFile);

			if (lstObjects.Items.Count == 0 && lstHullIcon.Items.Count == 0 && _objFile != "") File.Delete(_objFile);

			if (!useHangarObjects)
			{
				if (_hangarObjectsFile != "") File.Delete(_hangarObjectsFile);
				if (_hangarObjectsFileSI != "") File.Delete(_hangarObjectsFileSI);
				if (_hangarObjectsFileS != "") File.Delete(_hangarObjectsFileS);
			}
			if (!useHangarCamera)
			{
				if (_hangarCameraFile != "") File.Delete(_hangarCameraFile);
				if (_hangarCameraFileSI != "") File.Delete(_hangarCameraFileSI);
				if (_hangarCameraFileS != "") File.Delete(_hangarCameraFileS);
			}
			if (!useFamilyHangarCamera)
			{
				if (_famHangarCameraFile != "") File.Delete(_famHangarCameraFile);
				if (_famHangarCameraFileSI != "") File.Delete(_famHangarCameraFileSI);
				if (_famHangarCameraFileS != "") File.Delete(_famHangarCameraFileS);
			}
			if (!useHangarMap)
			{
				if (_hangarMapFile != "") File.Delete(_hangarMapFile);
				if (_hangarMapFileSI != "") File.Delete(_hangarMapFileSI);
				if (_hangarMapFileS != "") File.Delete(_hangarMapFileS);
			}
			if (!useFamilyHangarMap)
			{
				if (_famHangarMapFile != "") File.Delete(_famHangarMapFile);
				if (_famHangarMapFileSI != "") File.Delete(_famHangarMapFileSI);
				if (_famHangarMapFileS != "") File.Delete(_famHangarMapFileS);
			}

			if (lstSkins.Items.Count == 0 && _32bppFile != "") File.Delete(_32bppFile);

			if (!useShields && _shieldFile != "") File.Delete(_shieldFile);

			if (optHypGlobal.Checked && _hyperFile != "") File.Delete(_hyperFile);

			if (!chkConcoursePlanetIndex.Checked && !chkConcoursePlanetX.Checked && !chkConcoursePlanetY.Checked && _concourseFile != "") File.Delete(_concourseFile);

			if (!chkPlayerHull.Checked && _hullIconFile != "") File.Delete(_hullIconFile);

			if (lstStats.Items.Count == 0 && _statsFile != "") File.Delete(_statsFile);
			// TODO: need usage/delete checks for weap, warheads, energy, linking, spec

			if (lstBackdrops.Items.Count == 0
				&& lstMission.Items.Count == 0 && lstCraftText.Items.Count == 0 && !useMissionSettings
				&& lstSounds.Items.Count == 0
				&& !useInterdiction
				&& lstObjects.Items.Count == 0 && lstHullIcon.Items.Count == 0
				&& !useHangarObjects && !useHangarCamera && !useFamilyHangarCamera && !useHangarMap && !useFamilyHangarMap
				&& !useSFoils
				&& lstSkins.Items.Count == 0
				&& !useShields
				&& optHypGlobal.Checked
				&& !chkConcoursePlanetIndex.Checked && !chkConcoursePlanetX.Checked && !chkConcoursePlanetY.Checked
				&& !chkPlayerHull.Checked
				&& lstStats.Items.Count == 0)
			{
				File.Delete(_fileName);
				Close();
				return;
			}

			_hookFile.Write();
			if (_bdFile != "") File.Delete(_bdFile);
			if (_missionTxtFile != "") File.Delete(_missionTxtFile);
			if (_soundFile != "") File.Delete(_soundFile);
			if (_interdictionFile != "") File.Delete(_interdictionFile);
			if (_objFile != "") File.Delete(_objFile);
			if (_hangarObjectsFile != "") File.Delete(_hangarObjectsFile);
			if (_hangarCameraFile != "") File.Delete(_hangarCameraFile);
			if (_famHangarCameraFile != "") File.Delete(_famHangarCameraFile);
			if (_hangarMapFile != "") File.Delete(_hangarMapFile);
			if (_famHangarMapFile != "") File.Delete(_famHangarMapFile);
			if (_hangarObjectsFileSI != "") File.Delete(_hangarObjectsFileSI);
			if (_hangarCameraFileSI != "") File.Delete(_hangarCameraFileSI);
			if (_famHangarCameraFileSI != "") File.Delete(_famHangarCameraFileSI);
			if (_hangarMapFileSI != "") File.Delete(_hangarMapFileSI);
			if (_famHangarMapFileSI != "") File.Delete(_famHangarMapFileSI);
			if (_hangarObjectsFileS != "") File.Delete(_hangarObjectsFileS);
			if (_hangarCameraFileS != "") File.Delete(_hangarCameraFileS);
			if (_famHangarCameraFileS != "") File.Delete(_famHangarCameraFileS);
			if (_hangarMapFileS != "") File.Delete(_hangarMapFileS);
			if (_famHangarMapFileS != "") File.Delete(_famHangarMapFileS);
			if (_32bppFile != "") File.Delete(_32bppFile);
			if (_shieldFile != "") File.Delete(_shieldFile);
			if (_hyperFile != "") File.Delete(_hyperFile);
			if (_concourseFile != "") File.Delete(_concourseFile);
			if (_hullIconFile != "") File.Delete(_hullIconFile);
			if (_statsFile != "") File.Delete(_statsFile);
			if (_weapProfilesFile != "") File.Delete(_weapProfilesFile);
			if (_weapRatesFile != "") File.Delete(_weapRatesFile);
			if (_warheadProfilesFile != "") File.Delete(_warheadProfilesFile);
			if (_energyProfilesFile != "") File.Delete(_energyProfilesFile);
			if (_linkingProfilesFile != "") File.Delete(_linkingProfilesFile);
			if (_warheadTypeCountFile != "") File.Delete(_warheadTypeCountFile);
			if (_specRciFile != "") File.Delete(_specRciFile);

			Close();
		}

		private void txtHook_Enter(object sender, EventArgs e)
		{
			_loading = true;
			createContents();
			_loading = false;
		}
		private void txtHook_Leave(object sender, EventArgs e)
		{
			if (!_txtModified) return;

			_txtModified = false;
			updateHookFile();
			resetUI();
			parseHookFile();

			/* There is a condition that causes this to skip over the first reset() call and go directly to parse() when the user clicks certain controls while
             * the focus is currently in the txt box.
             * It doesn't matter if the only reset is here or in parse(), it'll skip. That causes every ListBox to be filled again without a Clear().
             * Other times it'll be fine, which means reset runs twice.
             * Seems to be an issue with controls that toggle Enabled within reset(); toggling chk, first attempt at a cbo won't drop, etc.
			*/
		}
		private void txtHook_TextChanged(object sender, EventArgs e) { if (!_loading) _txtModified = true; }

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
				return $"{ModelIndex}, {(Markings != 0 ? $"{Markings}, " : "")}{PositionX}, {PositionY}, {(IsGrounded ? "0x7FFFFFFF" : PositionZ.ToString())}, {HeadingXY}, {HeadingZ}"
					+ $"{(Profile != "" ? $", {Profile}" : "")}{(IsFloorInverted ? $"{(Profile == "" ? ", Default" : "")}, 1" : "")}";
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