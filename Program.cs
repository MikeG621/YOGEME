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
			if (Args.Length != 1 && start.Startup == (byte)Settings.StartupMode.Normal) Application.Run(new frmStart());
			else if (Args.Length != 1 && start.Startup == Settings.StartupMode.LastPlatform)
			{
				switch (start.LastPlatform)	//open the last platform directly
				{
					case Settings.Platform.TIE:
						new frmTIE().Show();
						break;
					case Settings.Platform.XvT:
						new frmXvT(false).Show();
						break;
					case Settings.Platform.BoP:
						new frmXvT(true).Show(); ;
						break;
					case Settings.Platform.XWA:
						new frmXWA().Show();
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
							new frmTIE().Show();
							break;
						case Settings.Platform.XvT:
							new frmXvT(false).Show();
							break;
						case Settings.Platform.BoP:
							new frmXvT(true).Show();
							break;
						case Settings.Platform.XWA:
							new frmXWA().Show();
							break;
					}
				}
				else
				{
					switch (start.LastPlatform)
					{
						case Settings.Platform.TIE:
							new frmTIE(start.LastMission).Show();
							break;
						case Settings.Platform.XvT:
							new frmXvT(start.LastMission).Show();
							break;
						case Settings.Platform.BoP:
							new frmXvT(start.LastMission).Show();
							break;
						case Settings.Platform.XWA:
							new frmXWA(start.LastMission).Show();
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
				else if (Args[0].ToUpper() == "/TIE") new frmTIE().Show();
				else if (Args[0].ToUpper() == "/XVT") new frmXvT(false).Show();
				else if (Args[0].ToUpper() == "/BOP") new frmXvT(true).Show();
				else if (Args[0].ToUpper() == "/XWA") new frmXWA().Show();
				else new frmStart(Args[0]).Show();
			}
			Application.Run();
		}
	}
}
