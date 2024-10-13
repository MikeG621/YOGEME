/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.16
 *
 * CHANGELOG
 * v1.16, 241013
 * [UPD] updates due to Settings
 * v1.11.1, 210814
 * [FIX] Set InvariantCulture to prevent text read errors [JB]
 * v1.5, 180910
 * [NEW] added X-wing [JB]
 * v1.3, 170107
 * [FIX] Recent mission fixes [JB]
 * v1.2.3, 141214
 * [UPD] change to MPL
 * v1.2.2, 121022
 * [FIX] Mission files opened from command line or "Open with" will no longer show StartForm
 * v1.2, 121006
 * - Settings initialized here, passed on
 * [NEW] RecentMission functionality
 * v1.0, 110921
 * - Release
 */

/* A note on compilation: be sure to add the following assembly references to the project.
 * - System
 * - System.Core
 * - System.Data
 * - System.Drawing
 * - System.Windows.Forms
 * - System.XML
 * - Idmr.LfdReader (and required Idmr DLLs)
 * - Idmr.Platform (and required Idmr DLLs)
 * 
 * If including Idmr.Platform (or others) within the YOGEME project solution,
 * set their application output type to Class Library and adjust the assembly
 * name to Idmr.Platform (or other applicable library name).
 * 
 * When launching YOGEME.exe, certain dialogs (like the briefing) will fail to
 * open unless the "images" subfolder and necessary data files exist.
 */


using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace Idmr.Yogeme
{
	static class Program
	{
		[STAThread]
		static void Main(string[] Args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Settings config = Settings.GetInstance();
            if (Args.Length != 1 && config.Startup == Settings.StartupMode.Normal) Application.Run(new StartForm());
			else if (Args.Length != 1 && config.Startup == Settings.StartupMode.LastPlatform)
			{
				switch (config.LastPlatform)	//open the last platform directly
				{
					case Settings.Platform.XWING:
						new XwingForm().Show();
						break;
					case Settings.Platform.TIE:
						new TieForm().Show();
						break;
					case Settings.Platform.XvT:
						new XvtForm(false).Show();
						break;
					case Settings.Platform.BoP:
						new XvtForm(true).Show(); ;
						break;
					case Settings.Platform.XWA:
						new XwaForm().Show();
						break;
				}
			}
			else if (Args.Length != 1 && config.Startup == Settings.StartupMode.LastMission)	//open the last mission directly
			{
				if (config.LastMission != "")
				{
					switch (config.LastPlatform)
					{
						case Settings.Platform.XWING:
							new XwingForm(config.LastMission).Show();
							break;
						case Settings.Platform.TIE:
							new TieForm(config.LastMission).Show();
							break;
						case Settings.Platform.XvT:
							new XvtForm(config.LastMission).Show();
							break;
						case Settings.Platform.BoP:
							new XvtForm(config.LastMission).Show();
							break;
						case Settings.Platform.XWA:
							new XwaForm(config.LastMission).Show();
							break;
					}
				}
				else if (config.RecentMissions[1] != "")  //JB Fixed.  [0] is the currently loaded mission, [1..5] are the Recent missions.
				{
					switch (config.RecentPlatforms[1])
					{
						case Settings.Platform.XWING:
							new XwingForm(config.RecentMissions[1]).Show();
							break;
						case Settings.Platform.TIE:
							new TieForm(config.RecentMissions[1]).Show();
							break;
						case Settings.Platform.XvT:
							new XvtForm(config.RecentMissions[1]).Show();
							break;
						case Settings.Platform.BoP:
							new XvtForm(config.RecentMissions[1]).Show();
							break;
						case Settings.Platform.XWA:
							new XwaForm(config.RecentMissions[1]).Show();
							break;
					}
				}
				else
				{
					MessageBox.Show("Last Mission value not set, taking you to last platform", "Error");
					switch (config.LastPlatform)
					{
						case Settings.Platform.XWING:
							new XwingForm().Show();
							break;
						case Settings.Platform.TIE:
							new TieForm().Show();
							break;
						case Settings.Platform.XvT:
							new XvtForm(false).Show();
							break;
						case Settings.Platform.BoP:
							new XvtForm(true).Show();
							break;
						case Settings.Platform.XWA:
							new XwaForm().Show();
							break;
					}
				}
			}
			else	//else process Args normally
			{
				if (Args[0] == "/?")
				{
					MessageBox.Show("Available arguments:\n\tnone\t\t\tStart normally\n\t/?\t\tShows this help\n\t[path]\t\tLoads mission as declared by [path]\n\t/Xwing\t\tLoads Xwing config directly\n\t/TIE\t\tLoads TIE config directly\n\t/XvT\t\tLoads XvT config directly\n\t/BoP\t\tLoads BoP config directly\n\t/XWA\t\tLoads XWA config directly", "Program use");
					return;
				}
                else if (Args[0].ToUpper() == "/XWING") new XwingForm().Show();
                else if (Args[0].ToUpper() == "/TIE") new TieForm().Show();
                else if (Args[0].ToUpper() == "/XVT") new XvtForm(false).Show();
                else if (Args[0].ToUpper() == "/BOP") new XvtForm(true).Show();
                else if (Args[0].ToUpper() == "/XWA") new XwaForm().Show();
                else
                {
                    try
                    {
                        FileStream fsPlat = File.OpenRead(Args[0]);
                        short t = new BinaryReader(fsPlat).ReadInt16();
                        fsPlat.Close();
                        System.Diagnostics.Debug.WriteLine("about to fire");
                        switch (t)
                        {
                            case 12:
                                new XvtForm(Args[0]).Show();
                                break;
                            case 14:
                                new XvtForm(Args[0]).Show();
                                break;
                            case 18:
                                new XwaForm(Args[0]).Show();
                                break;
                            case -1:
                                new TieForm(Args[0]).Show();
                                break;
                            case 2:
                                new XwingForm(Args[0]).Show();
                                break;
                            default:
                                throw new Exception("File is either invalid or corrupted.\nPlease ensure the correct file was selected.");
                        }
                        System.Diagnostics.Debug.WriteLine("fired");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Run(new StartForm());
                    }
                }
			}
			Application.Run();
		}
	}
}
