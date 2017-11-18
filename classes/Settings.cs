/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2017 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.4.1
 */

/* CHANGELOG
 * v1.4.1, 171118
 * [NEW #13] Super Backdrops
 * v1.3, 170107
 * [ADD] RememberPlatformFolder, ConfirmFGDelete, MRU paths
 * v1.2.4, 141215
 * [FIX #1] x64 registry values in CheckPlatforms and null check for sub (via JeremyAnsel)
 * v1.2.3, 141214
 * [UPD] change to MPL
 * [FIX] crash in CheckPlatforms during Markus checks (registry)
 * v1.2.1, 121007
 * [FIX] Crash that occured during first run.
 * v1.2, 121006
 * [NEW] RecentMissions, RecentPlatforms
 * [UPD] v4
 * v1.1.1, 120814
 * [NEW] Settings format version implemented @v3
 * - Deprecated values no longer occupy space
 * - *Craft INT values split to *Craft BYTE and *IFF BYTE
 * - renamed a bunch of stuff
 * [FIX] Waypoints correctly save BRF2-8
 * v1.1, 120715
 * [NEW] ConfirmTest, DeleteTestPilots and VerifyTest
 * [UPD] Deprecated values now write 0x00
 * - File trims to last value
 * v1.0, 110921
 * - Release
 */

using System;
using System.IO;
using Microsoft.Win32;

namespace Idmr.Yogeme
{
	public class Settings
	{
		#region defaults
		string _verifyLocation = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "MissionVerify.exe");
		string _lastMission = "";
		string _tiePath = "";
		string _xvtPath = "";
		string _bopPath = "";
		string _xwaPath = "";
        string _mruTiePath = ""; //[JB] stores the most recently used folders
        string _mruXvtPath = ""; //XvT and BoP share paths
        string _mruXwaPath = "";
		string _settingsDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) 
			+ "\\Imperial Department of Military Research\\YOGEME\\";
		#endregion
		string[] _recentMissions = new string[6];
		Platform[] _recentPlatforms = new Platform[6];
		public enum Platform { None, TIE, XvT, BoP, XWA }
		public enum StartupMode { Normal, LastPlatform, LastMission }
		[Flags]
		public enum MapOpts { None, FGTags, Traces }

		/// <summary>Creates a new Settings object and loads saved settings</summary>
		public Settings()
		{
			loadDefaults();
			LoadSettings();
		}
		
		void loadDefaults()
		{
			RestrictPlatforms = true;
			ConfirmExit = true;
			ConfirmSave = true;
			ConfirmTest = true;
			DeleteTestPilots = true;
            RememberPlatformFolder = true;
            ConfirmFGDelete = true;
			MapOptions = MapOpts.FGTags | MapOpts.Traces;
			for (int i = 0; i < 6; i++) _recentMissions[i] = "";
			Startup = StartupMode.Normal;
			TieCraft = XvtCraft = XwaCraft = 5;
			TieIff  = XvtIff = XwaIff = 1;
			Verify = true;
			VerifyTest = true;
			Waypoints = 1;
		}
		
		/// <summary>Loads saved settings</summary>
		/// <remarks>If no saved settings exist, will save defaults in the user's settings file</remarks>
		public void LoadSettings()
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\IDMR\\MissionEditor");
			string settingsFile = _settingsDir + "\\Settings.dat";
			if (key == null && !File.Exists(settingsFile))
			{
				CheckPlatforms();
				SaveSettings();
			}
			else if (File.Exists(settingsFile))
			{
				#region read from file
				FileStream fs = File.OpenRead(settingsFile);
				BinaryReader br = new BinaryReader(fs);
				byte version = 0xFF;
				if (fs.ReadByte() == 0xFF) version = br.ReadByte();
				else fs.Position = 0;
				BopInstalled = br.ReadBoolean();
				_bopPath = br.ReadString();
				if (version == 0xFF) fs.Position++;	// CheckInstall **DEPRECATED**
				ConfirmExit = br.ReadBoolean();
				ConfirmSave = br.ReadBoolean();
				_recentMissions[0] = br.ReadString();
				_recentPlatforms[0] = (Platform)br.ReadByte();
				MapOptions = (MapOpts)br.ReadByte();
				if (version >= 4 && version != 0xFF)
					for (int i = 1; i < 6; i++)
					{
						_recentMissions[i] = br.ReadString();
						_recentPlatforms[i] = (Platform)br.ReadByte();
					}
				RestrictPlatforms = br.ReadBoolean();
				if (version == 0xFF) fs.Position++;	// ShowDebug **DEPRECATED**
				Startup = (StartupMode)br.ReadByte();
				TieInstalled = br.ReadBoolean();
				if (version == 0xFF)
				{
					int tieCraft = br.ReadInt32();
					TieCraft = (byte)(tieCraft & 0x7F);
					TieIff = (byte)(tieCraft >> 7);
				}
				else
				{
					TieCraft = br.ReadByte();
					TieIff = br.ReadByte();
				}
				_tiePath = br.ReadString();
				Verify = br.ReadBoolean();
				Waypoints = br.ReadInt32();
				XvtInstalled = br.ReadBoolean();
				if (version == 0xFF)
				{
					int xvtCraft = br.ReadInt32();
					XvtCraft = (byte)(xvtCraft & 0x7F);
					XvtIff = (byte)(xvtCraft >> 7);
				}
				else
				{
					XvtCraft = br.ReadByte();
					XvtIff = br.ReadByte();
				}
				_xvtPath = br.ReadString();
				XwaInstalled = br.ReadBoolean();
				XwaCraft = br.ReadByte();
				XwaIff = br.ReadByte();
				if (version == 0xFF) fs.Position += 2;
				_xwaPath = br.ReadString();
				try
				{
					// requires try block for no version (0xFF), so just leave it
					_verifyLocation = br.ReadString();	// added in v1.0 (settings v1)
					ConfirmTest = br.ReadBoolean();	// added in v1.1 (settings v2)
					DeleteTestPilots = br.ReadBoolean();	// added in 1.1
					VerifyTest = br.ReadBoolean();  // added in v1.1
					RememberPlatformFolder = br.ReadBoolean();	// added by [JB] in 1.3 (settings v5)
					ConfirmFGDelete = br.ReadBoolean();
					_mruTiePath = br.ReadString();
					_mruXvtPath = br.ReadString();
					_mruXwaPath = br.ReadString();
					SuperBackdropsInstalled = br.ReadBoolean();	// added in 1.3.1 (settings v6)
					InitializeUsingSuperBackdrops = br.ReadBoolean();	// added in 1.3.1
				}	
				catch { /*do nothing*/ }

                fs.Close();
				#endregion
				CheckPlatforms();
			}
			else
			{
				#region read from registry (DEPRECATED)
				BopInstalled = Convert.ToBoolean(key.GetValue("BoP", false));
				_bopPath = (string)key.GetValue("BoPInstall", "");
				ConfirmExit = Convert.ToBoolean(key.GetValue("ConfirmExit", true));
				ConfirmSave = Convert.ToBoolean(key.GetValue("ConfirmSave", true));
				_lastMission = (string)key.GetValue("LastMission", "");
				LastPlatform = (Platform)key.GetValue("LastPlatform", 0);
				MapOptions = (MapOpts)key.GetValue("MapOptions", 3);
				RestrictPlatforms = Convert.ToBoolean(key.GetValue("RestrictPlatforms", true));
				Startup = (StartupMode)key.GetValue("Startup", 0);
				TieInstalled = Convert.ToBoolean(key.GetValue("TIE", false));
				int tieCraft = (int)key.GetValue("TIECraft", 0x85);
				TieCraft = (byte)(tieCraft & 0x7F);
				TieIff = (byte)(tieCraft >> 7);
				_tiePath = (string)key.GetValue("TIEInstall", "");
				Verify = Convert.ToBoolean(key.GetValue("Verify", true));
				Waypoints = (int)key.GetValue("Waypoints", 1);
				XvtInstalled = Convert.ToBoolean(key.GetValue("XvT", false));
				int xvtCraft = (int)key.GetValue("XvTCraft", 0x85);
				XvtCraft = (byte)(xvtCraft & 0x7F);
				XvtIff = (byte)(xvtCraft >> 7);
				_xvtPath = (string)key.GetValue("XvTInstall", "");
				XwaInstalled = Convert.ToBoolean(key.GetValue("XWA", false));
				int xwaCraft = (int)key.GetValue("XWACraft", 0x85);
				XwaCraft = (byte)(xwaCraft & 0xFF);
				XwaIff = (byte)(xwaCraft >> 8);
				_xwaPath = (string)key.GetValue("XWAInstall", "");
				key.Close();
				#endregion
				CheckPlatforms();
			}
		}
		/// <summary>Searches Registry for platform installations, will not reflect uninstalls</summary>
		public void CheckPlatforms()
		{
			RegistryKey keyplat;
			if (!TieInstalled)
			{
				keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\LucasArts Entertainment Company LLC\\TIE95\\1.0");
				
				if (keyplat == null)
				{
					keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\LucasArts Entertainment Company LLC\\TIE95\\1.0");
				}

				if (keyplat != null)
				{
					TieInstalled = true;
					_tiePath = (string)keyplat.GetValue("Install Path");
					keyplat.Close();
				}
			}
			if (!XvtInstalled)
			{
				keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\LucasArts Entertainment Company\\X-Wing vs. TIE Fighter\\1.0");
				
				if (keyplat == null)
				{
					keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\LucasArts Entertainment Company\\X-Wing vs. TIE Fighter\\1.0");
				}

				if (keyplat != null)
				{
					XvtInstalled = true;
					_xvtPath = (string)keyplat.GetValue("Install Path");
					keyplat.Close();
				}
			}
			if (!BopInstalled)
			{
				keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\LucasArts Entertainment Company\\X-Wing vs. TIE Fighter\\2.0");
				
				if (keyplat == null)
				{
					keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\LucasArts Entertainment Company\\X-Wing vs. TIE Fighter\\2.0");
				}

				if (keyplat != null)
				{
					BopInstalled = true;
					_bopPath = (string)keyplat.GetValue("Install Path");
					keyplat.Close();
				}
			}
			if (!XwaInstalled)
			{
				keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\LucasArts Entertainment Company LLC\\X-Wing Alliance\\v1.0");
				
				if (keyplat == null)
				{
					keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\LucasArts Entertainment Company LLC\\X-Wing Alliance\\v1.0");
				}

				if (keyplat != null)
				{
					XwaInstalled = true;
					_xwaPath = (string)keyplat.GetValue("Install Path");
					keyplat.Close();
				}
			}
			if (!TieInstalled || !XvtInstalled || !BopInstalled || !XwaInstalled)
			{
				// 64-bit detection of platforms using the MSI installers from Markus Egger (http://www.markusegger.at/Software/Games.aspx)
				keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Installer\\UserData\\S-1-5-18\\Products");
				if (keyplat == null) return;
				string[] subs = keyplat.GetSubKeyNames();
				foreach (string k in subs)
				{
					string s = keyplat.Name + "\\" + k + "\\InstallProperties";
					RegistryKey sub = Registry.LocalMachine.OpenSubKey(s.Substring(19));

					if (sub == null)
					{
						continue;
					}

					string comm = (string)sub.GetValue("DisplayName");
					if (comm == "Star Wars: X. C. S. - TIE Fighter 95" && !TieInstalled)
					{
						string path = (string)sub.GetValue("Readme");
						_tiePath = path.Remove(path.Length - 11);
						TieInstalled = true;
					}
					/*if (comm == "Star Wars X-Wing vs TIE Fighter" && !XvtInstalled)	// TODO: confirm install name
					{
						string path = (string)sub.GetValue("Readme");
						_xvtPath = path.Remove(path.Length - 11);
						XvtInstalled = true;
					}*/
					if (comm == "Star Wars: X. v. T. - Balance of Power" && !BopInstalled)
					{
						string path = (string)sub.GetValue("Readme");
						_bopPath = path.Remove(path.Length - 11);
						BopInstalled = true;
					}
					if (comm == "Star Wars X-Wing Alliance" && !XwaInstalled)
					{
						string path = (string)sub.GetValue("Readme");
						_xwaPath = path.Remove(path.Length - 11);
						XwaInstalled = true;
					}
				}
			}
			if (XwaInstalled) SuperBackdropsInstalled = File.Exists(_xwaPath + "\\BackupDTMSB\\XwingAlliance.exe");
		}
		/// <summary>Saves current settings to user's settings file</summary>
		/// <remarks>Registry use has been deprecated</remarks>
		public void SaveSettings()
		{
			#region save registry (DEPRECATED)
			/*RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\IDMR\\MissionEditor", true);
			if (key == null) key = Registry.CurrentUser.CreateSubKey("Software\\IDMR\\MissionEditor");
			key.SetValue("BoP", Convert.ToInt32(BopInstalled));
			key.SetValue("BoPInstall", _bopPath);
			key.SetValue("ConfirmExit", Convert.ToInt32(ConfirmExit));
			key.SetValue("ConfirmSave", Convert.ToInt32(ConfirmSave));
			key.SetValue("LastMission", _lastMission);
			key.SetValue("LastPlatform", Convert.ToInt32(LastPlatform));
			key.SetValue("MapOptions", Convert.ToInt32(MapOptions));
			key.SetValue("RestrictPlatforms", Convert.ToInt32(RestrictPlatforms));
			key.SetValue("Startup", Convert.ToInt32(Startup));
			key.SetValue("TIE", Convert.ToInt32(TieInstalled));
			key.SetValue("TIECraft", _tieCraft);
			key.SetValue("TIEInstall", _tiePath);
			key.SetValue("Verify", Convert.ToInt32(Verify));
			key.SetValue("Waypoints", _waypoints);
			key.SetValue("XvT", Convert.ToInt32(XvtInstalled));
			key.SetValue("XvTCraft", _xvtCraft);
			key.SetValue("XvTInstall", _xvtPath);
			key.SetValue("XWA", Convert.ToInt32(XwaInstalled));
			key.SetValue("XWACraft", _xwaCraft);
			key.SetValue("XWAInstall", _xwaPath);
			key.Close();
			File.Delete(_settingsDir + "\\Settings.dat");*/
			#endregion
			#region save file
			if (!Directory.Exists(_settingsDir)) Directory.CreateDirectory(_settingsDir);
			FileStream fs = File.OpenWrite(_settingsDir + "\\Settings.dat");
			BinaryWriter bw = new BinaryWriter(fs);
			fs.WriteByte(0xFF);
			fs.WriteByte(0x06);
			bw.Write(BopInstalled);
			bw.Write(_bopPath);
			bw.Write(ConfirmExit);
			bw.Write(ConfirmSave);
			bw.Write(_recentMissions[0]);
			bw.Write((byte)_recentPlatforms[0]);
			bw.Write((byte)MapOptions);
			for (int i = 1; i < 6; i++)
			{
				bw.Write(_recentMissions[i]);
				bw.Write((byte)_recentPlatforms[i]);
			}
			bw.Write(RestrictPlatforms);
			bw.Write((byte)Startup);
			bw.Write(TieInstalled);
			bw.Write(TieCraft);
			bw.Write(TieIff);
			bw.Write(_tiePath);
			bw.Write(Verify);
			bw.Write(Waypoints);
			bw.Write(XvtInstalled);
			bw.Write(XvtCraft);
			bw.Write(XvtIff);
			bw.Write(_xvtPath);
			bw.Write(XwaInstalled);
			bw.Write(XwaCraft);
			bw.Write(XwaIff);
			bw.Write(_xwaPath);
			bw.Write(_verifyLocation);
			bw.Write(ConfirmTest);
			bw.Write(DeleteTestPilots);
			bw.Write(VerifyTest);
            bw.Write(RememberPlatformFolder); //[JB] Added
            bw.Write(ConfirmFGDelete);
            bw.Write(_mruTiePath);
            bw.Write(_mruXvtPath);
            bw.Write(_mruXwaPath);
			bw.Write(SuperBackdropsInstalled);
			bw.Write(InitializeUsingSuperBackdrops);
			fs.SetLength(fs.Position);
			fs.Close();
			#endregion
			// remove Regkey if needed
			Registry.CurrentUser.DeleteSubKey("Software\\IDMR\\MissionEditor", false);
		}

        //[JB] This function helps centralize the use of the last-accessed folder feature.  If most recent folder is not available, it uses the detected installation path and default mission folder.
        public string GetWorkingPath()
        {
            switch (LastPlatform)
            {
                case Platform.TIE: return (RememberPlatformFolder && (_mruTiePath != "")) ? _mruTiePath : TiePath + "\\MISSION";
                case Platform.XvT: return (RememberPlatformFolder && (_mruXvtPath != "")) ? _mruXvtPath : XvtPath + "\\Train";
                case Platform.BoP: return (RememberPlatformFolder && (_mruXvtPath != "")) ? _mruXvtPath : BopPath + "\\TRAIN";
                case Platform.XWA: return (RememberPlatformFolder && (_mruXwaPath != "")) ? _mruXwaPath : XwaPath + "\\MISSIONS";
            }
            return Directory.GetCurrentDirectory();
        }
        //[JB] This function helps centralize the use of the last-opened folder feature.  If a last-opened folder is not available, it uses the default installed path with the requested subfolder (usually the default location of missions for that platform).
        /*
        public string GetWorkingPath(string defaultSubFolder)
        {
            switch (LastPlatform)
            {
                case Platform.TIE: return (RememberPlatformFolder && (_mrutiePath != "")) ? _mrutiePath : TiePath + defaultSubFolder;
                case Platform.XvT: return (RememberPlatformFolder && (_mruxvtPath != "")) ? _mruxvtPath : XvtPath + defaultSubFolder;
                case Platform.BoP: return (RememberPlatformFolder && (_mruxvtPath != "")) ? _mruxvtPath : BopPath + defaultSubFolder;
                case Platform.XWA: return (RememberPlatformFolder && (_mruxwaPath != "")) ? _mruxwaPath : XwaPath + defaultSubFolder;
            }
            return "";
        }*/
        public void SetWorkingPath(string path)
        {
            switch(LastPlatform)
            {   
                case Platform.TIE: _mruTiePath = path; break;
                case Platform.XvT: case Platform.BoP: _mruXvtPath = path; break;
                case Platform.XWA: _mruXwaPath = path; break;
            }
        }

		#region Properties
		/// <summary>Gets or sets if Balance of Power is installed</summary>
		public bool BopInstalled { get; set; }
		/// <summary>Gets or sets the install directory for Balance of Power</summary>
		/// <remarks>No action is taken when using set if the directory does not exist</remarks>
		public string BopPath
		{
			get { return _bopPath; }
			set { if (Directory.Exists(value)) { _bopPath = value; } }
		}
		/// <summary>Gets or sets if the confirmation dialog is shown when exiting YOGEME</summary>
		public bool ConfirmExit { get; set; }
		/// <summary>Gets or sets if a confirmation dialog is shown when deleting a Flight Group, if other FGs, goals, mission, or briefing triggers depend on it.</summary>
		public bool ConfirmFGDelete { get; set; }  //[JB] Added
		/// <summary>Gets or sets if a confirmation dialog is shown when closing an unsaved mission</summary>
		public bool ConfirmSave { get; set; }
		/// <summary>Gets or sets if the Test dialog is shown</summary>
		public bool ConfirmTest { get; set; }
		/// <summary>Gets or sets if pilot files created during Test are deleted when the platform is closed</summary>
		public bool DeleteTestPilots { get; set; }
		/// <summary>Gets or sets if new XWA missions will be initialized with DTM's Super Backdrops</summary>
		public bool InitializeUsingSuperBackdrops { get; set; }
        /// <summary>Gets or sets the path to last opened mission</summary>
		/// <remarks>Updates <see cref="RecentMissions"/> and <see cref="RecentPlatforms"/> during set</remarks>
		public string LastMission
		{
			get { return _recentMissions[0]; }
			set
			{
                //[JB] Sorting wasn't working correctly.  Rewritten.
                _recentMissions[0] = value;  //Index [0] holds the current mission, [1...5] hold the Recent list.
                if (value != "") 
                {
                    string[] missions = new string[5];
                    Platform[] platforms = new Platform[5];
                    for (int i = 0; i < 5; i++)  //Ensure init (avoids nulls when saving the settings)
                    {
                        missions[i] = "";
                        platforms[i] = Platform.None;
                    }
                    int n = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        if ((_recentMissions[i] == value && i > 0) || (_recentMissions[i] == ""))  //The current mission [0] is added. If exists in the former Recent list, will be ignored.
                            continue;
                        missions[n] = _recentMissions[i];
                        platforms[n] = _recentPlatforms[i];
                        if (++n >= 5)  //Got our 5 items
                            break;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        _recentMissions[1 + i] = missions[i];
                        _recentPlatforms[1 + i] = platforms[i];
                    }
                }
			}
		}
		/// <summary>Gets or sets the most recent platform used in YOGEME</summary>
		public Platform LastPlatform
		{
			get { return _recentPlatforms[0]; }
			set { _recentPlatforms[0] = value; }
		}
		/// <summary>Gets or sets the map options</summary>
		public MapOpts MapOptions { get; set; }
		/// <summary>Gets a copy of the five most recent missions</summary>
		public string[] RecentMissions { get { return (string[])_recentMissions.Clone(); } }
		/// <summary>Gets a copy of the platforms pertaining to <see cref="RecentMissions"/></summary>
		public Platform[] RecentPlatforms { get { return (Platform[])_recentPlatforms.Clone(); } }
		/// <summary>Gets or sets if the most recently used folder is remembered when Saving/Loading missions of a particular platform.</summary>
		public bool RememberPlatformFolder { get; set; }  //[JB] Added
		/// <summary>Gets or sets if the user can only platform that have been installed</summary>
		public bool RestrictPlatforms { get; set; }
		/// <summary>Gets or sets the initial mode of YOGEME</summary>
		public StartupMode Startup { get; set; }
		/// <summary>Gets or sets the installation status of DTM's Super Backdrops mod for XWA</summary>
		public bool SuperBackdropsInstalled { get; set; }
		/// <summary>Gets or sets the default craft type in TIE Fighter</summary>
		public byte TieCraft { get; set; }
		/// <summary>Gets or sets the default IFF for new ships in TIE Fighter</summary>
		public byte TieIff { get; set; }
		/// <summary>Gets or sets if TIE95 is installed</summary>
		public bool TieInstalled { get; set; }
		/// <summary>Gets or sets the install directoy for TIE95</summary>
		/// <remarks>No action is taken when using set if the directory does not exist</remarks>
		public string TiePath
		{
			get { return _tiePath; }
			set { if (Directory.Exists(value)) { _tiePath = value; } }
		}
		/// <summary>Gets or sets if the missions will be verified when saving</summary>
		public bool Verify { get; set; }
		/// <summary>Gets or sets the path to the verify application</summary>
		/// <remarks>No action is taken when using set if the file does not exist</remarks>
		public string VerifyLocation
		{
			get { return _verifyLocation; }
			set { if (File.Exists(value)) _verifyLocation = value; }
		}
		/// <summary>Gets or sets if the mission will be verified before testing</summary>
		/// <remarks>If <see "Verify"/> is <b>true</b>, this value is ignored such that the verification only occurs once</remarks>
		public bool VerifyTest { get; set; }
		/// <summary>Gets or sets the default enabled waypoints in the Map interface</summary>
		public int Waypoints { get; set; }
		/// <summary>Gets or sets the default craft type for X-wing vs TIE Fighter</summary>
		public byte XvtCraft { get; set; }
		/// <summary>Gets or sets the default IFF for new ships in X-wing vs TIE Fighter</summary>
		public byte XvtIff { get; set; }
		/// <summary>Gets or sets if X-wing vs TIE Fighter is installed</summary>
		public bool XvtInstalled { get; set; }
		/// <summary>Gets or sets the install directory for X-wing vs TIE Fighter</summary>
		/// <remarks>No action is taken when using set if the directory does not exist</remarks>
		public string XvtPath
		{
			get { return _xvtPath; }
			set { if (Directory.Exists(value)) { _xvtPath = value; } }
		}
		/// <summary>Gets or sets the default craft type for X-wing Alliance</summary>
		public byte XwaCraft { get; set; }
		/// <summary>Gets or sets the default IFF for new ships in X-wing Alliance</summary>
		public byte XwaIff { get; set; }
		/// <summary>Gets or sets if X-wing Alliance is installed</summary>
		public bool XwaInstalled { get; set; }
		/// <summary>Gets or sets the install directory for X-wing Alliance</summary>
		/// <remarks>No action is taken when using set if the directory does not exist</remarks>
		public string XwaPath
		{
			get { return _xwaPath; }
			set { if (Directory.Exists(value)) { _xwaPath = value; } }
		}
		#endregion
	}
	/* Settings and values
	 * (version) Name TYPE: notes
	 * (v3+) RESERVED BYTE: 0xFF
	 * (v3+) Version BYTE: 0x04
	 * BopInstalled BOOL:
	 * BopPath STR: path to BoP directory
	 * (v-3) CheckInstall BOOL: **DEPRECATED**
	 * ConfirmExit BOOL:
	 * ConfirmSave BOOL:
	 * LastMission STR: path to last open mission file; =RecentMission0
	 * LastPlatform BYTE: last platform edited; 0=none, 1=TIE, 2=XvT, 3=BoP, 4=XWA; =RecentPlatform0
	 * MapOptions BYTE [Flags]: 1<<0=FG Tags, 1<<1=Traces
	 * (v4+) RecentMission1 STR: automatically updated through LastMission.set
	 * (v4+) RecentPlatform1 BYTE:
	 * (v4+) RecentMission2 STR:
	 * ...
	 * (v4+) RecentPlatform5 BYTE:
	 * RestrictPlatforms BOOL: false=all platforms editable, true=only installed platforms editable
	 * (v-3) ShowDebug BYTE: **DEPRECATED**
	 * Startup BYTE: 0=normal, 1=open to last platform, 2=open last mission
	 * TieInstalled BOOL:
	 * (v-3) TIECraft INT: &0x7F=Default Craft, &0x380>>7=Default IFF
	 * (v3+) TieCraft BYTE:
	 * (v3+) TieIFF BYTE:
	 * TiePath STR: path to TIE directory
	 * Verify BOOL: false=no action, true=run MissVerify on Save
	 * Waypoints INT [Flags]: 1<<0=SP1... 1<<4=WP1... 1<<12=RND, 1<<13=HYP, 1<<14=BRF1... 1<<21=BRF8
	 * XvtInstalled BOOL:
	 * (v-3) XvTCraft INT: &0x7F=Default Craft, &0x380>>7=Default IFF
	 * (v3+) XvtCraft BYTE:
	 * (v3+) XvtIff BYTE:
	 * XvtPath STR: path to XvT directory
	 * XwaInstalled BOOL:
	 * XwaCraft BYTE:
	 * XwaIff BYTE:
	 * (v-3) RESERVED SHORT: 0x0000
	 * XwaPath STR: path to XWA directory
	 * VerifyLocation STR: path to mission verification program (MissionVerify.exe)
	 * ConfirmTest BOOL: show confirm dialog before executing test function
	 * DeleteTestPilots BOOL: after done flying, delete the testing pilot file
	 * VerifyTest BOOL: ignored if (Verify), Verify mission before launching test
	 * (v5+) RememberPlatformFolder BOOL:
	 * (v5+) ConfirmFGDelete BOOL:
	 * (v5+) _mruTiePath STRING:
	 * (v5+) _mruXvtPath STRING:
	 * (v5+) _mruXwaPath STRING:
	 * (v6+) SuperBackdropsInstalled BOOL:
	 * (v6+) InitializeUsingSuperBackdrops BOOL:
	 */
}
