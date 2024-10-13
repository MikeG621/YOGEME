/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.16
 *
 * CHANGELOG
 * v1.16, 241013
 * [UPD] class is now a Singleton
 * [UPD] LoadSettings and CheckPlatforms now private, weren't used elsewhere anyway
 * [UPD] Defaults now initialized directly instead of load function
 * v1.13.12, 230116
 * [NEW] RememberSelectedOrder
 * v1.13.4, 220606
 * [NEW] OneIndexedFGs
 * v1.9.1, 210130
 * [FIX] Crash in SBD detection if GetFiles() dir doesn't exist [JB]
 * v1.9, 210108
 * [UPD] Added detection for SBD via XWAUP Mega Patch 1.1
 * [UPD] Added generic detection of planet2.dat for SBD
 * v1.8.1, 201213
 * [UPD] Added detection for SBD via XWAUP Mega Patch
 * v1.8, 201004
 * [UPD] More flags for MapOpts [JB]
 * [NEW] MapMiddleClick settings, MapSnap settings [JB]
 * [UPD] Added detection for SBD v3.1
 * v1.7, 200816
 * [NEW] Map options for Wireframe implementation [JB]
 * v1.6.3, 200101
 * [FIX #29] Fixed a settings write corruption due to partial platform detection
 * v1.6, 190915
 * [UPD] SBD detection changed to Readme instead of Backup since XWAUCP is different
 * v1.5.1, 190513
 * [UPD] Removed Steam X-Wing detection, as it doesn't work the same for some reason
 * v1.5, 180910
 * [ADD] Xwing and theming settings [JB]
 * v1.4.2, 180224
 * [ADD #17] Added explicit platform detection for Steam
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

using Idmr.Yogeme.MapWireframe;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;

namespace Idmr.Yogeme
{
	public class Settings
	{
		static readonly Settings _instance = new Settings();

		#region defaults
		string _verifyLocation = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "MissionVerify.exe");
		string _xwingPath = "";
		string _tiePath = "";
		string _xvtPath = "";
		string _bopPath = "";
		string _xwaPath = "";
		string _mruXwingPath = "";
		string _mruTiePath = ""; //[JB] stores the most recently used folders
		string _mruXvtPath = ""; //XvT and BoP share paths
		string _mruXwaPath = "";
		readonly string _settingsDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
			+ "\\Imperial Department of Military Research\\YOGEME\\";
		#endregion
		readonly string[] _recentMissions = new string[6];
		readonly Platform[] _recentPlatforms = new Platform[6];
		public enum Platform { None, TIE, XvT, BoP, XWA, XWING }
		public enum StartupMode { Normal, LastPlatform, LastMission }
		[Flags]
		public enum MapOpts { None = 0, FGTags = 1, Traces = 2, TraceDistance = 4, TraceTime = 8, TraceHideFade = 16, TraceSelected = 32 }

		/// <summary>Creates a new Settings object and loads saved settings</summary>
		private Settings()
		{
			for (int i = 0; i < 6; i++) _recentMissions[i] = "";
			loadSettings();
		}

		public static Settings GetInstance() => _instance;

		/// <summary>Loads saved settings</summary>
		/// <remarks>If no saved settings exist, will save defaults in the user's settings file</remarks>
		void loadSettings()
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\IDMR\\MissionEditor");
			string settingsFile = _settingsDir + "\\Settings.dat";
			if (key == null && !File.Exists(settingsFile))
			{
				checkPlatforms();
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
				if (version == 0xFF) fs.Position++; // CheckInstall **DEPRECATED**
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
				if (version == 0xFF) fs.Position++; // ShowDebug **DEPRECATED**
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
					_verifyLocation = br.ReadString();  // added in v1.0 (settings v1)
					ConfirmTest = br.ReadBoolean(); // added in v1.1 (settings v2)
					DeleteTestPilots = br.ReadBoolean();    // added in 1.1
					VerifyTest = br.ReadBoolean();  // added in v1.1
					RememberPlatformFolder = br.ReadBoolean();  // added by [JB] in 1.3 (settings v5)
					ConfirmFGDelete = br.ReadBoolean();
					_mruTiePath = br.ReadString();
					_mruXvtPath = br.ReadString();
					_mruXwaPath = br.ReadString();
					SuperBackdropsInstalled = br.ReadBoolean(); // added in 1.3.1 (settings v6)
					InitializeUsingSuperBackdrops = br.ReadBoolean();   // added in 1.3.1
					XwingInstalled = br.ReadBoolean();  // added in 1.5 (settings v7) [JB]
					XwingCraft = br.ReadByte();
					XwingIff = br.ReadByte();
					_xwingPath = br.ReadString();
					_mruXwingPath = br.ReadString();
					ColorizedDropDowns = br.ReadBoolean();
					ColorInteractSelected = Color.FromArgb(br.ReadInt32());
					ColorInteractNonSelected = Color.FromArgb(br.ReadInt32());
					ColorInteractBackground = Color.FromArgb(br.ReadInt32());

					MapMouseWheelZoomPercentage = br.ReadDouble(); // added in 1.7 [JB]
					WireframeEnabled = br.ReadBoolean();
					WireframeIconThresholdEnabled = br.ReadBoolean();
					WireframeIconThresholdSize = br.ReadInt32();
					WireframeMeshIconEnabled = br.ReadBoolean();
					WireframeMeshIconSize = br.ReadInt32();
					WireframeMeshTypeVisibility = br.ReadInt64();

					XwingDetectMission = br.ReadBoolean();
					TieDetectMission = br.ReadBoolean();
					XvtDetectMission = br.ReadBoolean();
					XwaDetectMission = br.ReadBoolean();

					XwingOverrideExternal = br.ReadBoolean();
					TieOverrideExternal = br.ReadBoolean();
					XvtOverrideExternal = br.ReadBoolean();
					XwaOverrideExternal = br.ReadBoolean();
					XwaOverrideScan = br.ReadBoolean();
					XwaFlagRemappedCraft = br.ReadBoolean();

					MapMiddleClickActionSelected = (MapForm.MiddleClickAction)br.ReadInt32(); // added in 1.8 [JB]
					MapMiddleClickActionNoneSelected = (MapForm.MiddleClickAction)br.ReadInt32();
					MapSnapTo = br.ReadByte();
					MapSnapAmount = br.ReadSingle();
					MapSnapUnit = br.ReadByte();

					OneIndexedFGs = br.ReadBoolean();	// added in 1.13.4

					RememberSelectedOrder= br.ReadBoolean();	// added in 1.13.12
				}
				catch { System.Diagnostics.Debug.WriteLine("old settings file"); /*do nothing*/ }

				fs.Close();
				#endregion
				checkPlatforms();
			}
			else
			{
				#region read from registry (DEPRECATED)
				BopInstalled = Convert.ToBoolean(key.GetValue("BoP", false));
				_bopPath = (string)key.GetValue("BoPInstall", "");
				ConfirmExit = Convert.ToBoolean(key.GetValue("ConfirmExit", true));
				ConfirmSave = Convert.ToBoolean(key.GetValue("ConfirmSave", true));
				_recentMissions[0] = (string)key.GetValue("LastMission", "");
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
				checkPlatforms();
			}
		}

		/// <summary>Searches Registry for platform installations, will not reflect uninstalls</summary>
		void checkPlatforms()
		{
			RegistryKey keyplat;
			#region original registry
			if (!XwingInstalled)
			{
				keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\LucasArts Entertainment Company LLC\\X-Wing95\\1.0");

				if (keyplat == null) keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\LucasArts Entertainment Company LLC\\X-Wing95\\1.0");

				if (keyplat != null)
				{
					XwingInstalled = true;
					_xwingPath = (string)keyplat.GetValue("Install Path");
					keyplat.Close();
				}
			}
			if (!TieInstalled)
			{
				keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\LucasArts Entertainment Company LLC\\TIE95\\1.0");

				if (keyplat == null) keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\LucasArts Entertainment Company LLC\\TIE95\\1.0");

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

				if (keyplat == null) keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\LucasArts Entertainment Company\\X-Wing vs. TIE Fighter\\1.0");

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

				if (keyplat == null) keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\LucasArts Entertainment Company\\X-Wing vs. TIE Fighter\\2.0");

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

				if (keyplat == null) keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\LucasArts Entertainment Company LLC\\X-Wing Alliance\\v1.0");

				if (keyplat != null)
				{
					XwaInstalled = true;
					_xwaPath = (string)keyplat.GetValue("Install Path");
					keyplat.Close();
				}
			}
			#endregion
			#region MSI installers
			if (!XwingInstalled || !TieInstalled || !XvtInstalled || !BopInstalled || !XwaInstalled)
			{
				// 64-bit detection of platforms using the MSI installers from Markus Egger (http://www.markusegger.at/Software/Games.aspx)
				keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Installer\\UserData\\S-1-5-18\\Products");
				if (keyplat == null) return;
				string[] subs = keyplat.GetSubKeyNames();
				foreach (string k in subs)
				{
					string s = keyplat.Name + "\\" + k + "\\InstallProperties";
					RegistryKey sub = Registry.LocalMachine.OpenSubKey(s.Substring(19));

					if (sub == null) continue;

					string comm = (string)sub.GetValue("DisplayName");
					if (comm == "Star Wars: X. C. S. - X-Wing 95" && !XwingInstalled)
					{
						string path = (string)sub.GetValue("Readme");
						_xwingPath = path.Remove(path.Length - 11);
						XwingInstalled = true;
					}
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
			#endregion
			#region Steam detection
			// since I can't rely on the normal registry values, we'll go about it this way...
			/*if (!XwingInstalled) //This doesn't work! Steam apparently installs differently, doesn't appear in Windows programs list, need a new way to detect
            {
                keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 354430");
                if (keyplat != null)
                {
                    _xwingPath = (string)keyplat.GetValue("InstallLocation") + "\\remastered";
                    keyplat.Close();
                    // verify it's actually there, just in case uninstall borked
                    if (File.Exists(_xwingPath + "\\XWING95.exe")) XwingInstalled = true;
                }
            }*/
			if (!TieInstalled)
			{
				keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 355250");
				if (keyplat != null)
				{
					_tiePath = (string)keyplat.GetValue("InstallLocation") + "\\remastered";
					keyplat.Close();
					// verify it's actually there, just in case uninstall borked
					if (File.Exists(_tiePath + "\\TIE95.exe")) TieInstalled = true;
				}
			}
			if (!XvtInstalled || !BopInstalled)
			{
				keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 361690");
				if (keyplat != null)
				{
					_xvtPath = (string)keyplat.GetValue("InstallLocation");
					keyplat.Close();
					if (File.Exists(_xvtPath + "\\z_xvt__.exe")) XvtInstalled = true;
					_bopPath = _xvtPath += "\\BalanceOfPower";
					if (File.Exists(_bopPath + "\\z_xvt__.exe")) BopInstalled = true;
				}
			}
			if (!XwaInstalled)
			{
				keyplat = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 361670");
				if (keyplat != null)
				{
					_xwaPath = (string)keyplat.GetValue("InstallLocation");
					keyplat.Close();
					if (File.Exists(_xwaPath + "\\alliance.exe")) XwaInstalled = true;
				}
			}
			#endregion
			if (XwaInstalled) SuperBackdropsInstalled = (File.Exists(_xwaPath + "\\DTMSBReadme.rtf") || File.Exists(_xwaPath + "\\Backup\\SBPReadme_v3.1.rtf") || Directory.Exists(_xwaPath + "\\Readme\\SuperBackdropPatch") || (Directory.Exists(_xwaPath + "\\Readme\\Upgrades") && Directory.GetFiles(_xwaPath + "\\Readme\\Upgrades", "SuperBackdrop*").Length != 0) || File.Exists(_xwaPath + "\\Resdata\\Planet2.dat"));
		}

		/// <summary>Saves current settings to user's settings file</summary>
		/// <remarks>Registry use has been deprecated</remarks>
		public void SaveSettings()
		{
			if (!Directory.Exists(_settingsDir)) Directory.CreateDirectory(_settingsDir);
			FileStream fs = File.OpenWrite(_settingsDir + "\\Settings.dat");
			BinaryWriter bw = new BinaryWriter(fs);
			fs.WriteByte(0xFF);
			fs.WriteByte(0x09);
			bw.Write(BopInstalled);
			if (_bopPath != null) bw.Write(_bopPath);
			else bw.Write("");
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
			if (_tiePath != null) bw.Write(_tiePath);
			else bw.Write("");
			bw.Write(Verify);
			bw.Write(Waypoints);
			bw.Write(XvtInstalled);
			bw.Write(XvtCraft);
			bw.Write(XvtIff);
			if (_xvtPath != null) bw.Write(_xvtPath);
			else bw.Write("");
			bw.Write(XwaInstalled);
			bw.Write(XwaCraft);
			bw.Write(XwaIff);
			if (_xwaPath != null) bw.Write(_xwaPath);
			else bw.Write("");
			bw.Write(_verifyLocation);
			bw.Write(ConfirmTest);
			bw.Write(DeleteTestPilots);
			bw.Write(VerifyTest);
			bw.Write(RememberPlatformFolder);
			bw.Write(ConfirmFGDelete);
			bw.Write(_mruTiePath);
			bw.Write(_mruXvtPath);
			bw.Write(_mruXwaPath);
			bw.Write(SuperBackdropsInstalled);
			bw.Write(InitializeUsingSuperBackdrops);

			bw.Write(XwingInstalled);
			bw.Write(XwingCraft);
			bw.Write(XwingIff);
			if (_xwingPath != null) bw.Write(_xwingPath);
			else bw.Write("");
			bw.Write(_mruXwingPath);
			bw.Write(ColorizedDropDowns);
			bw.Write(ColorInteractSelected.ToArgb());
			bw.Write(ColorInteractNonSelected.ToArgb());
			bw.Write(ColorInteractBackground.ToArgb());

			bw.Write(MapMouseWheelZoomPercentage);
			bw.Write(WireframeEnabled);
			bw.Write(WireframeIconThresholdEnabled);
			bw.Write(WireframeIconThresholdSize);
			bw.Write(WireframeMeshIconEnabled);
			bw.Write(WireframeMeshIconSize);
			bw.Write(WireframeMeshTypeVisibility);

			bw.Write(XwingDetectMission);
			bw.Write(TieDetectMission);
			bw.Write(XvtDetectMission);
			bw.Write(XwaDetectMission);

			bw.Write(XwingOverrideExternal);
			bw.Write(TieOverrideExternal);
			bw.Write(XvtOverrideExternal);
			bw.Write(XwaOverrideExternal);

			bw.Write(XwaOverrideScan);
			bw.Write(XwaFlagRemappedCraft);

			bw.Write((int)MapMiddleClickActionSelected);
			bw.Write((int)MapMiddleClickActionNoneSelected);
			bw.Write(MapSnapTo);
			bw.Write(MapSnapAmount);
			bw.Write(MapSnapUnit);

			bw.Write(OneIndexedFGs);

			bw.Write(RememberSelectedOrder);

			fs.SetLength(fs.Position);
			fs.Close();
			// remove Regkey if needed
			Registry.CurrentUser.DeleteSubKey("Software\\IDMR\\MissionEditor", false);
		}

		/// <summary>Gets the most recently used directory</summary>
		/// <returns>Most recent directory, otherwise default platform directory</returns>
		public string GetWorkingPath()
		{
			switch (LastPlatform)
			{
				case Platform.XWING: return (RememberPlatformFolder && (_mruXwingPath != "")) ? _mruXwingPath : XwingPath + "\\MISSION";
				case Platform.TIE: return (RememberPlatformFolder && (_mruTiePath != "")) ? _mruTiePath : TiePath + "\\MISSION";
				case Platform.XvT: return (RememberPlatformFolder && (_mruXvtPath != "")) ? _mruXvtPath : XvtPath + "\\Train";
				case Platform.BoP: return (RememberPlatformFolder && (_mruXvtPath != "")) ? _mruXvtPath : BopPath + "\\TRAIN";
				case Platform.XWA: return (RememberPlatformFolder && (_mruXwaPath != "")) ? _mruXwaPath : XwaPath + "\\MISSIONS";
			}
			return Directory.GetCurrentDirectory();
		}
		/// <summary>Sets the MRU directory</summary>
		/// <param name="path">Full directory</param>
		public void SetWorkingPath(string path)
		{
			switch (LastPlatform)
			{
				case Platform.XWING: _mruXwingPath = path; break;
				case Platform.TIE: _mruTiePath = path; break;
				case Platform.XvT: case Platform.BoP: _mruXvtPath = path; break;    // TODO: BoP really should be separate
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
			get => _bopPath;
			set { if (Directory.Exists(value)) { _bopPath = value; } }
		}
		/// <summary>Gets or sets the foreground color for the selected Goal, Trigger, Order, etc.</summary>
		public Color ColorInteractSelected { get; set; } = Color.Blue;
		/// <summary>Gets or sets the foreground color for non-selected Goals, Triggers, Orders, etc.</summary>
		public Color ColorInteractNonSelected { get; set; } = Color.Black;
		/// <summary>Gets or sets the background color for Goals, Triggers, Orders, etc.</summary>
		public Color ColorInteractBackground { get; set; } = Color.RosyBrown;
		/// <summary>Gets or sets whether FlightGroup ComboBox dropdowns are colorized according to IFF.</summary>
		public bool ColorizedDropDowns { get; set; } = true;
		/// <summary>Gets or sets if the confirmation dialog is shown when exiting YOGEME</summary>
		public bool ConfirmExit { get; set; } = true;
		/// <summary>Gets or sets if a confirmation dialog is shown when deleting a Flight Group, if other FGs, goals, mission, or briefing triggers depend on it.</summary>
		public bool ConfirmFGDelete { get; set; } = true;
		/// <summary>Gets or sets if a confirmation dialog is shown when closing an unsaved mission</summary>
		public bool ConfirmSave { get; set; } = true;
		/// <summary>Gets or sets if the Test dialog is shown</summary>
		public bool ConfirmTest { get; set; } = true;
		/// <summary>Gets or sets if pilot files created during Test are deleted when the platform is closed</summary>
		public bool DeleteTestPilots { get; set; } = true;
		/// <summary>Gets or sets if new XWA missions will be initialized with DTM's Super Backdrops</summary>
		public bool InitializeUsingSuperBackdrops { get; set; }
		/// <summary>Gets or sets the path to last opened mission</summary>
		/// <remarks>Updates <see cref="RecentMissions"/> and <see cref="RecentPlatforms"/> during set</remarks>
		public string LastMission
		{
			get => _recentMissions[0];
			set
			{
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
						if (++n >= 5) break;
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
			get => _recentPlatforms[0];
			set => _recentPlatforms[0] = value;
		}
		/// <summary>Gets or sets the map options</summary>
		public MapOpts MapOptions { get; set; } = MapOpts.FGTags | MapOpts.Traces;
		/// <summary>Gets a copy of the five most recent missions</summary>
		public string[] RecentMissions => (string[])_recentMissions.Clone();
		/// <summary>Gets a copy of the platforms pertaining to <see cref="RecentMissions"/></summary>
		public Platform[] RecentPlatforms => (Platform[])_recentPlatforms.Clone();
		/// <summary>Gets or sets if the most recently used folder is remembered when Saving/Loading missions of a particular platform.</summary>
		public bool RememberPlatformFolder { get; set; } = true;
		/// <summary>Gets or sets if the user can only platform that have been installed</summary>
		public bool RestrictPlatforms { get; set; } = true;
		/// <summary>Gets or sets the initial mode of YOGEME</summary>
		public StartupMode Startup { get; set; } = StartupMode.Normal;
		/// <summary>Gets or sets the installation status of DTM's Super Backdrops mod for XWA</summary>
		public bool SuperBackdropsInstalled { get; set; }

		/// <summary>Gets or sets the default craft type in TIE Fighter</summary>
		public byte TieCraft { get; set; } = 5;
		/// <summary>Gets or sets the default IFF for new ships in TIE Fighter</summary>
		public byte TieIff { get; set; } = 1;
		/// <summary>Gets or sets if TIE95 is installed</summary>
		public bool TieInstalled { get; set; }
		/// <summary>Gets or sets the install directoy for TIE95</summary>
		/// <remarks>No action is taken when using set if the directory does not exist</remarks>
		public string TiePath
		{
			get => _tiePath;
			set { if (Directory.Exists(value)) { _tiePath = value; } }
		}
		/// <summary>Gets or sets if the missions will be verified when saving</summary>
		public bool Verify { get; set; } = true;
		/// <summary>Gets or sets the path to the verify application</summary>
		/// <remarks>No action is taken when using set if the file does not exist</remarks>
		public string VerifyLocation
		{
			get => _verifyLocation;
			set { if (File.Exists(value)) _verifyLocation = value; }
		}
		/// <summary>Gets or sets if the mission will be verified before testing</summary>
		/// <remarks>If <see cref="Verify"/> is <b>true</b>, this value is ignored such that the verification only occurs once</remarks>
		public bool VerifyTest { get; set; } = true;
		/// <summary>Gets or sets the default enabled waypoints in the Map interface</summary>
		public int Waypoints { get; set; } = 1;
		/// <summary>Gets or sets the default craft type for X-wing vs TIE Fighter</summary>
		public byte XvtCraft { get; set; } = 5;
		/// <summary>Gets or sets the default IFF for new ships in X-wing vs TIE Fighter</summary>
		public byte XvtIff { get; set; } = 1;
		/// <summary>Gets or sets if X-wing vs TIE Fighter is installed</summary>
		public bool XvtInstalled { get; set; }
		/// <summary>Gets or sets the install directory for X-wing vs TIE Fighter</summary>
		/// <remarks>No action is taken when using set if the directory does not exist</remarks>
		public string XvtPath
		{
			get => _xvtPath;
			set { if (Directory.Exists(value)) { _xvtPath = value; } }
		}
		/// <summary>Gets or sets the default craft type for X-wing Alliance</summary>
		public byte XwaCraft { get; set; } = 5;
		/// <summary>Gets or sets the default IFF for new ships in X-wing Alliance</summary>
		public byte XwaIff { get; set; } = 1;
		/// <summary>Gets or sets if X-wing Alliance is installed</summary>
		public bool XwaInstalled { get; set; }
		/// <summary>Gets or sets the install directory for X-wing Alliance</summary>
		/// <remarks>No action is taken when using set if the directory does not exist</remarks>
		public string XwaPath
		{
			get => _xwaPath;
			set { if (Directory.Exists(value)) { _xwaPath = value; } }
		}
		/// <summary>Gets or sets the default craft type in X-wing</summary>
		public byte XwingCraft { get; set; } = 1;
		/// <summary>Gets or sets the default IFF for new ships in X-wing</summary>
		public byte XwingIff { get; set; } = 1; // Rebel
		/// <summary>Gets or sets if XWING95 is installed</summary>
		public bool XwingInstalled { get; set; }
		/// <summary>Gets or sets the install directoy for XWING95</summary>
		/// <remarks>No action is taken when using set if the directory does not exist</remarks>
		public string XwingPath
		{
			get => _xwingPath;
			set { if (Directory.Exists(value)) { _xwingPath = value; } }
		}
		/// <summary>Gets or sets the percentage (of the current zoom level) to adjust when using mousewheel zoom in the map.</summary>
		public double MapMouseWheelZoomPercentage { get; set; } = 10;
		/// <summary>Gets or sets whether craft wireframes are enabled for drawing in the map.</summary>
		public bool WireframeEnabled { get; set; } = true;
		/// <summary>Gets or sets whether a bitmap icon should be drawn instead of a wireframe when a craft's length is too short.</summary>
		public bool WireframeIconThresholdEnabled { get; set; }
		/// <summary>Gets or sets the craft size threshold (in meters) that must be achieved for a wireframe to be drawn.</summary>
		public int WireframeIconThresholdSize { get; set; } = 25;
		/// <summary>Gets or sets whether to scale up a wireframe to simulate an icon if its render size is too small.</summary>
		public bool WireframeMeshIconEnabled { get; set; } = true;
		/// <summary>Gets or sets the minimum size (in pixels) that a wireframe should be scaled to simulate an icon.</summary>
		public int WireframeMeshIconSize { get; set; } = 18;
		/// <summary>Gets or sets the collection of bit flags that determine which mesh types should be drawn.</summary>
		public long WireframeMeshTypeVisibility { get; set; } = MeshTypeHelper.GetDefaultFlags();

		/// <summary>Gets or sets whether to detect the platform installation path from a loaded X-wing mission.</summary>
		public bool XwingDetectMission { get; set; } = true;
		/// <summary>Gets or sets whether to detect the platform installation path from a loaded TIE Fighter mission.</summary>
		public bool TieDetectMission { get; set; } = true;
		/// <summary>Gets or sets whether to detect the platform installation path from a loaded X-wing vs TIE Fighter (or Balance of Power) mission.</summary>
		public bool XvtDetectMission { get; set; } = true;
		/// <summary>Gets or sets whether to detect the platform installation path from a loaded X-wing Alliance mission.</summary>
		public bool XwaDetectMission { get; set; } = true;

		/// <summary>Gets or sets whether craft names should be overridden by the external X-wing craft data file.</summary>
		public bool XwingOverrideExternal { get; set; }
		/// <summary>Gets or sets whether craft names should be overridden by the external TIE Fighter craft data file.</summary>
		public bool TieOverrideExternal { get; set; }
		/// <summary>Gets or sets whether craft names should be overridden by the external X-wing vs TIE Fighter (and Balance of Power) craft data file.</summary>
		public bool XvtOverrideExternal { get; set; }
		/// <summary>Gets or sets whether craft names should be overridden by the external X-wing Alliance craft data file.</summary>
		public bool XwaOverrideExternal { get; set; }

		/// <summary>Gets or sets whether to scan the craft list directly from the XWA installation files. This will further override <see cref="XwaOverrideExternal"/> if enabled.</summary>
		public bool XwaOverrideScan { get; set; } = true;
		/// <summary>Gets or sets whether a suffix should be added to craft names, indicating a craft type that has been remapped. Only applies when <see cref="XwaOverrideScan"/> is used.</summary>
		public bool XwaFlagRemappedCraft { get; set; } = true;

		/// <summary>Gets or sets the action to perform when middle-clicking the map with something selected.</summary>
		public MapForm.MiddleClickAction MapMiddleClickActionSelected { get; set; } = MapForm.MiddleClickAction.FitToSelection;
		/// <summary>Gets or sets the action to perform when middle-clicking the map with nothing selected.</summary>
		public MapForm.MiddleClickAction MapMiddleClickActionNoneSelected { get; set; } = MapForm.MiddleClickAction.FitToWorld;
		/// <summary>Gets or sets whether map movement snapping is enabled, and to what.</summary>
		public byte MapSnapTo { get; set; }
		/// <summary>Gets or sets the distance if map movement snapping is enabled.</summary>
		public float MapSnapAmount { get; set; } = .1f;
		/// <summary>Gets or sets the unit of measurement for map movement snapping.</summary>
		public byte MapSnapUnit { get; set; }

		/// <summary>Gets or sets if the FlightGroup counter display starts at 1 instead of 0.</summary>
		public bool OneIndexedFGs { get; set; } = true;

		/// <summary>Gets or sets if the selected order resets when changing FlightGroups.</summary>
		public bool RememberSelectedOrder { get; set; }
		#endregion
	}
	/* Settings and values
	 * (version) Name TYPE: notes
	 * (v3+) RESERVED BYTE: 0xFF
	 * (v3+) Version BYTE: 0x07
	 * BopInstalled BOOL:
	 * BopPath STR: path to BoP directory
	 * (v-3) CheckInstall BOOL: **DEPRECATED**
	 * ConfirmExit BOOL:
	 * ConfirmSave BOOL:
	 * LastMission STR: path to last open mission file; =RecentMission0
	 * LastPlatform BYTE: last platform edited; 0=none, 1=TIE, 2=XvT, 3=BoP, 4=XWA, 5=XW; =RecentPlatform0
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
	 * (v7+) XwingInstalled BOOL:
	 * (v7+) XwingCraft BYTE:
	 * (v7+) XwingIFF BYTE:
	 * (v7+) XwingPath STR: path to Xwing directory
	 * (v7+) _mruXwingPath STRING:
	 * (v7+) ColorizedDropdowns BOOL:
	 * (v7+) ColorInteractSelected INT: ARGB value
	 * (v7+) ColorInteractNonSelected INT: ARGB value
	 * (v7+) ColorInteractBackground INT: ARGB value
	 * Said I'd inc the version for here down, but forgot. Doesn't make a difference when things are only added.
	 * (v7+) MapMouseWheelZoomPercentage DOUBLE:
	 * (v7+) WireframeEnabled BOOL:
	 * (v7+) WireframeIconThresholdEnabled BOOL:
	 * (v7+) WireframeIconThresholdSize INT:
	 * (v7+) WireframeMeshIconEnabled BOOL:
	 * (v7+) WireframeMeshIconSize INT:
	 * (v7+) WireframeMeshTypeVisiblity LONG [Flags]: See MeshType enum (MapWireframe.cs) for values
	 * (v7+) XwingDetectMission BOOL:
	 * (v7+) TieDetectMission BOOL:
	 * (v7+) XvtDetectMission BOOL:
	 * (v7+) XwaDetectMission BOOL:
	 * (v7+) XwingOverrideExternal BOOL:
	 * (v7+) TieOverrideExternal BOOL:
	 * (v7+) XvtOverrideExternal BOOL:
	 * (v7+) XwaOverrideExternal BOOL:
	 * (v7+) XwaOverrideScan BOOL:
	 * (v7+) XwaFlagRemappedCraft BOOL:
	 * (v7+) MapMiddleClickActionSelected INT: enum MapForm.MiddleClickAction, only needed to be a BYTE
	 * (v7+) MapMiddleClickActionNoneSelected INT: enum MapForm.MiddleClickAction, only needed to be a BYTE
	 * (v7+) MapSnapTo BYTE: 0 = None, 1 = Self, 2 = Grid
	 * (v7+) MapSnapAmount FLOAT:
	 * (v7+) MapSnapUnit BYTE: 0 = km, 1 = Raw
	 * (v8+) OneIndexedFGs BOOL:
	 */
}
