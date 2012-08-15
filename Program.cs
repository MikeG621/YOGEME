/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.1.1
 */

/* CHANGELOG
 * v1.0, 110921
 * - Release
 */

using System;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	static class Program
	{
		[STAThread]
		static void Main(string[] Args)
		{
			Settings start = new Settings();
			if (Args.Length != 1 && start.Startup == (byte)Settings.StartupMode.Normal) Application.Run(new StartForm());
			else if (Args.Length != 1 && start.Startup == Settings.StartupMode.LastPlatform)
			{
				switch (start.LastPlatform)	//open the last platform directly
				{
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
			else if (Args.Length != 1 && start.Startup == Settings.StartupMode.LastMission)	//open the last mission directly
			{
				if (start.LastMission == "")	//if errors occur or new mission, LM could be blank
				{
					MessageBox.Show("Last Mission value not set, taking you to last platform", "Error");
					switch (start.LastPlatform)
					{
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
				else
				{
					switch (start.LastPlatform)
					{
						case Settings.Platform.TIE:
							new TieForm(start.LastMission).Show();
							break;
						case Settings.Platform.XvT:
							new XvtForm(start.LastMission).Show();
							break;
						case Settings.Platform.BoP:
							new XvtForm(start.LastMission).Show();
							break;
						case Settings.Platform.XWA:
							new XwaForm(start.LastMission).Show();
							break;
					}
				}
			}
			else	//else process Args normally
			{
				if (Args[0] == "/?")
				{
					MessageBox.Show("Available arguments:\n\tnone\t\t\tStart normally\n\t/?\t\tShows this help\n\t[path]\t\tLoads mission as declared by [path]\n\t/TIE\t\tLoads TIE config directly\n\t/XvT\t\tLoads XvT config directly\n\t/BoP\t\tLoads BoP config directly\n\t/XWA\t\tLoads XWA config directly", "Program use");
					return;
				}
				else if (Args[0].ToUpper() == "/TIE") new TieForm().Show();
				else if (Args[0].ToUpper() == "/XVT") new XvtForm(false).Show();
				else if (Args[0].ToUpper() == "/BOP") new XvtForm(true).Show();
				else if (Args[0].ToUpper() == "/XWA") new XwaForm().Show();
				else new StartForm(Args[0]).Show();
			}
			Application.Run();
		}
	}
}
