/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.1
 */

/* CHANGELOG
 * v1.1, 120715
 * - added ConfirmTest, DeleteTestPilots and VerifyTest
 * - Deprecated values now write 0x00
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
		string _verifyLocation = System.Windows.Forms.Application.StartupPath + "\\MissionVerify.exe";
		string _lastMission = "";
		int _waypoints = 1;
		string _tiePath = "";
		int _tieCraft = 0x85;
		string _xvtPath = "";
		int _xvtCraft = 0x85;
		string _bopPath = "";
		string _xwaPath = "";
		int _xwaCraft = 0x105;
		string _settingsDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) 
			+ "\\Imperial Department of Military Research\\YOGEME\\";
		#endregion
		public enum Platform { None, TIE, XvT, BoP, XWA }
		public enum StartupMode { Normal, LastPlatform, LastMission }
		[Flags]
		public enum MapOpts { None, FGTags, Traces }

		public Settings()
		{
			_loadDefaults();
			LoadSettings();
		}
		
		void _loadDefaults()
		{
			RestrictPlatforms = true;
			ConfirmExit = true;
			ConfirmSave = true;
			ConfirmTest = true;
			DeleteTestPilots = true;
			LastPlatform = Platform.None;
			MapOptions = MapOpts.FGTags | MapOpts.Traces;
			Startup = StartupMode.Normal;
			Verify = true;
			VerifyTest = true;
		}
		
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
				BopInstalled = br.ReadBoolean();
				_bopPath = br.ReadString();
				fs.Position++;	// CheckInstall **DEPRECATED**
				ConfirmExit = br.ReadBoolean();
				ConfirmSave = br.ReadBoolean();
				_lastMission = br.ReadString();
				LastPlatform = (Platform)br.ReadByte();
				MapOptions = (MapOpts)br.ReadByte();
				RestrictPlatforms = br.ReadBoolean();
				fs.Position++;	// ShowDebug **DEPRECATED**
				Startup = (StartupMode)br.ReadByte();
				TieInstalled = br.ReadBoolean();
				_tieCraft = br.ReadInt32();
				_tiePath = br.ReadString();
				Verify = br.ReadBoolean();
				_waypoints = br.ReadInt32();
				XvtInstalled = br.ReadBoolean();
				_xvtCraft = br.ReadInt32();
				_xvtPath = br.ReadString();
				XwaInstalled = br.ReadBoolean();
				_xwaCraft = br.ReadInt32();
				_xwaPath = br.ReadString();
				try
				{
					_verifyLocation = br.ReadString();	// added after v0.9.1
					ConfirmTest = br.ReadBoolean();	// added after v1.0
					DeleteTestPilots = br.ReadBoolean();	// added after 1.0
					VerifyTest = br.ReadBoolean();	// added after v1.0
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
				_tieCraft = (int)key.GetValue("TIECraft", 0x85);
				_tiePath = (string)key.GetValue("TIEInstall", "");
				Verify = Convert.ToBoolean(key.GetValue("Verify", true));
				_waypoints = (int)key.GetValue("Waypoints", 1);
				XvtInstalled = Convert.ToBoolean(key.GetValue("XvT", false));
				_xvtCraft = (int)key.GetValue("XvTCraft", 0x85);
				_xvtPath = (string)key.GetValue("XvTInstall", "");
				XwaInstalled = Convert.ToBoolean(key.GetValue("XWA", false));
				_xwaCraft = (int)key.GetValue("XWACraft", 0x85);
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
				string[] subs = keyplat.GetSubKeyNames();
				foreach (string k in subs)
				{
					string s = keyplat.Name + "\\" + k + "\\InstallProperties";
					RegistryKey sub = Registry.LocalMachine.OpenSubKey(s.Substring(19));
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
		}
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
			bw.Write(BopInstalled);
			bw.Write(_bopPath);
			fs.WriteByte(0);	// CheckInstall **DEPRECATED**
			bw.Write(ConfirmExit);
			bw.Write(ConfirmSave);
			bw.Write(_lastMission);
			bw.Write((byte)LastPlatform);
			bw.Write((byte)MapOptions);
			bw.Write(RestrictPlatforms);
			fs.WriteByte(0);	// ShowDebug **DEPRECATED**
			bw.Write((byte)Startup);
			bw.Write(TieInstalled);
			bw.Write(_tieCraft);
			bw.Write(_tiePath);
			bw.Write(Verify);
			bw.Write(_waypoints);
			bw.Write(XvtInstalled);
			bw.Write(_xvtCraft);
			bw.Write(_xvtPath);
			bw.Write(XwaInstalled);
			bw.Write(_xwaCraft);
			bw.Write(_xwaPath);
			bw.Write(_verifyLocation);
			bw.Write(ConfirmTest);
			bw.Write(DeleteTestPilots);
			bw.Write(VerifyTest);
			fs.SetLength(fs.Position);
			fs.Close();
			#endregion
			// remove Regkey if needed
			Registry.CurrentUser.DeleteSubKey("Software\\IDMR\\MissionEditor", false);
		}

		#region Properties
		public bool BopInstalled { get; set; }
		public string BoPPath
		{
			get { return _bopPath; }
			set { if (Directory.Exists(value)) { _bopPath = value; } }
		}
		public bool ConfirmExit { get; set; }
		public bool ConfirmSave { get; set; }
		public bool ConfirmTest { get; set; }
		public bool DeleteTestPilots { get; set; }
		public string LastMission
		{
			get { return _lastMission; }
			set { if (File.Exists(value)) { _lastMission = value; } }
		}
		public Platform LastPlatform { get; set; }
		public MapOpts MapOptions { get; set; }
		public bool RestrictPlatforms { get; set; }
		public StartupMode Startup { get; set; }
		public byte TIECraft
		{
			get { return (byte)(_tieCraft & 0x7F); }
			set { if ((value & 0x7F) <= 0x57) { _tieCraft = (_tieCraft & 0x380) + value; } }
		}
		public byte TIEIFF
		{
			get { return (byte)((_tieCraft & 0x380) >> 7); }
			set { if (value <= 5) { _tieCraft = (_tieCraft & 0x7F) + (value << 7); } }
		}
		public bool TieInstalled { get; set; }
		public string TIEPath
		{
			get { return _tiePath; }
			set { if (Directory.Exists(value)) { _tiePath = value; } }
		}
		public bool Verify { get; set; }
		public string VerifyLocation
		{
			get { return _verifyLocation; }
			set { if (File.Exists(value)) _verifyLocation = value; }
		}
		public bool VerifyTest { get; set; }
		public int Waypoints
		{
			get { return _waypoints; }
			set { if (value <= 0x7FFF) { _waypoints = value; } }
		}
		public byte XvTCraft
		{
			get { return (byte)(_xvtCraft & 0x7F); }
			set { if ((value & 0x7F) <= 0x5C) { _xvtCraft = (_xvtCraft & 0x380) + value; } }
		}
		public byte XvTIFF
		{
			get { return (byte)((_xvtCraft & 0x380) >> 7); }
			set { if (value <= 5) { _xvtCraft = (_xvtCraft & 0x7F) + (value << 7); } }
		}
		public bool XvtInstalled { get; set; }
		public string XvTPath
		{
			get { return _xvtPath; }
			set { if (Directory.Exists(value)) { _xvtPath = value; } }
		}
		public byte XWACraft
		{
			get { return (byte)(_xwaCraft & 0xFF); }
			set { if ((value & 0xFF) <= 0xE7) { _xwaCraft = (_xwaCraft & 0x700) + value; } }
		}
		public byte XWAIFF
		{
			get { return (byte)((_xwaCraft & 0x700) >> 8); }
			set { if (value <= 5) { _xwaCraft = (_xwaCraft & 0xFF) + (value << 8); } }
		}
		public bool XwaInstalled { get; set; }
		public string XWAPath
		{
			get { return _xwaPath; }
			set { if (Directory.Exists(value)) { _xwaPath = value; } }
		}
		#endregion
	}
	/* Settings and values
	 * (Registry) (AppData type)
	 * RESERVED BYTE: 0xFF
	 * Version BYTE
	 * BopInstalled BOOL:
	 * BoPPath STR: path to BoP directory
	 * CheckInstall BOOL: **DEPRECATED**
	 * ConfirmExit BOOL:
	 * ConfirmSave BOOL:
	 * LastMission STR: path to last open mission file
	 * LastPlatform BYTE: last platform edited; 0=none, 1=TIE, 2=XvT, 3=BoP, 4=XWA
	 * MapOptions BYTE: 1+=FG Tags, 2+=Traces
	 * RestrictPlatforms BOOL: false=all platforms editable, true=only installed platforms editable
	 * ShowDebug BYTE: **DEPRECATED**
	 * Startup BYTE: 0=normal, 1=open to last platform, 2=open last mission
	 * TIECraft INT: &0x7F=Default Craft, &0x380>>7=Default IFF
	 * TieInstalled BOOL:
	 * TIEPath STR: path to TIE directory
	 * Verify BOOL: false=no action, true=run MissVerify on Save
	 * Waypoints INT: 1+=SP1, 2+=SP2, 4+=SP4, 8+=SP4, 16+=WP1, 32+=WP2, 64+=WP3, 128+=WP4, 256+=WP5, 512+=WP6, 1024+=WP7, 2048+=WP8, 4096+=RND, 8192+=HYP, 16384+=BRF
	 * XvTCraft INT: &0x7F=Default Craft, &0x380>>7=Default IFF
	 * XvtInstalled BOOL:
	 * XvTPath STR: path to XvT directory
	 * XWACraft INT: &0xFF=Default Craft, &0x700>>8=Default IFF
	 * XwaInstalled BOOL:
	 * XWAPath STR: path to XWA directory
	 * VerifyLocation STR: path to mission verification file (MissionVerify.exe)
	 * ConfirmTest BOOL: show confirm dialog before executing test function
	 * DeleteTestPilots BOOL: after done flying, delete the testing pilot file
	 * VerifyTest BOOL: ignored if (Verify), Verify mission before launching test
	 */
}
