using Idmr.Platform.Xwa;
using System;
using System.IO;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	public partial class XwaHookDialog : Form
	{
		// this is going to be setup to read from the individual TXT files, but always write to Mission.ini
		string _mission;
		string _fileName = "";
		string _bdFile = "";
		string _soundFile = "";
		string _objFile = "";
		string _missionTxtFile = "";
		string _hangarObjectsFile = "";
		string _hangarCameraFile = "";
		string _famHangarCameraFile = "";
		string _hangarMapFile = "";
		string _famHangarMapFile = "";
		string _installDirectory = "";
		string _mis = "\\Missions\\";
		string _res = "\\Resdata\\";
		string _wave = "\\Wave\\";
		string _fm = "\\FlightModels\\";
		enum ReadMode { None = -1, Backdrop, Mission }

		public XwaHookDialog(Mission mission)
		{
			InitializeComponent();
			_mission = Idmr.Common.StringFunctions.GetFileName(mission.MissionPath, false);
			if (_mission == "NewMission")
			{
				MessageBox.Show("Please perform inital save prior to hook assignment.", "New Mission detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cmdCancel_Click("NewMission", new EventArgs());
			}
			_fileName = mission.MissionPath.Replace(".tie", ".ini");

			cboIff.Items.AddRange(Strings.IFF);
			for (int i = cboIff.Items.Count; i < 256; i++) cboIff.Items.Add("IFF #" + (i + 1));
			cboMarkings.Items.AddRange(Strings.Color);
			for (int i = cboMarkings.Items.Count; i < 256; i++) cboMarkings.Items.Add("Clr #" + (i + 1));
			cboFG.Items.AddRange(mission.FlightGroups.GetList());

			Settings s = new Settings();
			if (s.XwaInstalled)
			{
				_installDirectory = s.XwaPath;
				grpBackdrops.Enabled = File.Exists(_installDirectory + "\\Hook_Backdrops.dll");

				_bdFile = checkFile("_Resdata.txt");
				_soundFile = checkFile("_Sounds.txt");
				_objFile = checkFile("_Objects.txt");
				_missionTxtFile = checkFile(".txt");
				_hangarObjectsFile = checkFile("_HangarObjects.txt");
				_hangarCameraFile = checkFile("_HangarCamera.txt");
				_famHangarCameraFile = checkFile("_FamHangarCamera.txt");
				_hangarMapFile = checkFile("_HangarMap.txt");
				_famHangarMapFile = checkFile("_FamHangarMap.txt");
			}
			StreamReader srMission = null;
			string line = "";
			if (File.Exists(_fileName)) srMission = new StreamReader(_fileName);
			ReadMode readMode = ReadMode.None;

			#region individual files
			if (_bdFile != "")
			{
				StreamReader srBD = new StreamReader(_bdFile);
				while ((line = srBD.ReadLine()) != null)
				{
					lstBackdrops.Items.Add(line);
				}
				srBD.Close();
			}
			if (_missionTxtFile != "")
			{
				StreamReader srMiss = new StreamReader(_missionTxtFile);
				while((line = srMiss.ReadLine()) != null)
				{
					line = line.ToLower().Replace(" ", "");
					string[] parts = line.Split(',');
					if (parts.Length == 4 && parts[0] == "fg")
					{
						int fg = int.Parse(parts[1]);
						if (parts[2] == "markings")
							lstMission.Items.Add(fg + "," + cboFG.Items[fg].ToString() + ",marks," + cboMarkings.Items[int.Parse(parts[3])].ToString());
						else if (parts[2] == "iff")
							lstMission.Items.Add(fg + "," + cboFG.Items[fg].ToString() + ",iff," + cboIff.Items[int.Parse(parts[3])].ToString());
						else if (parts[2] == "pilotvoice")
							lstMission.Items.Add(fg + "," + cboFG.Items[fg].ToString() + ",pilot," + parts[3]);
					}
				}
				srMiss.Close();
			}
			#endregion

			if (srMission != null)
			{
				while ((line = srMission.ReadLine()) != null)
				{
					if (line == "" || line.Trim().StartsWith(";")) continue;

					if (line.StartsWith("["))
					{
						readMode = ReadMode.None;
						if (line.ToLower() == "[resdata]") readMode = ReadMode.Backdrop;
						else if (line.ToLower() == "[mission_tie]") readMode = ReadMode.Mission;
					}
					else if (readMode == ReadMode.Backdrop) lstBackdrops.Items.Add(line);
					else if (readMode == ReadMode.Mission)
					{
						line = line.ToLower().Replace(" ", "");
						string[] parts = line.Split(',');
						if (parts.Length == 4 && parts[0] == "fg")
						{
							int fg = int.Parse(parts[1]);
							if (parts[2] == "markings")
								lstMission.Items.Add(cboFG.Items[fg].ToString() + ",marks," + cboMarkings.Items[int.Parse(parts[3])].ToString());
							else if (parts[2] == "iff")
								lstMission.Items.Add(cboFG.Items[fg].ToString() + ",iff," + cboIff.Items[int.Parse(parts[3])].ToString());
							else if (parts[2] == "pilotvoice")
								lstMission.Items.Add(cboFG.Items[fg].ToString() + ",pilot," + parts[3]);
						}
					}
				}
			}

			srMission.Close();
			chkBackdrops.Checked = (lstBackdrops.Items.Count > 0);
			chkMission.Checked = (lstMission.Items.Count > 0);
		}

		string checkFile(string extension)
		{
			if (File.Exists(_installDirectory + _mis + _mission + extension)) return _installDirectory + _mis + _mission + extension;
			return "";
		}

		#region Backdrops
		private void chkBackdrops_CheckedChanged(object sender, EventArgs e)
		{
			lstBackdrops.Enabled = chkBackdrops.Checked;
			cmdAddBD.Enabled = chkBackdrops.Checked;
			cmdRemoveBD.Enabled = chkBackdrops.Checked;
		}

		private void cmdAddBD_Click(object sender, EventArgs e)
		{
			if (_installDirectory != "") opnBackdrop.InitialDirectory = _installDirectory + _res;
			DialogResult res = opnBackdrop.ShowDialog();
			if (res == DialogResult.OK)
			{
				lstBackdrops.Items.Add(_res.TrimStart('\\') + Idmr.Common.StringFunctions.GetFileName(opnBackdrop.FileName, true));
			}
		}
		private void cmdRemoveBD_Click(object sender, EventArgs e)
		{
			if (lstBackdrops.SelectedIndex == -1) return;
			lstBackdrops.Items.RemoveAt(lstBackdrops.SelectedIndex);
		}
		#endregion Backdrops

		#region MissionTie
		private void chkMission_CheckedChanged(object sender, EventArgs e)
		{
			lstMission.Enabled = chkMission.Checked;
			cmdAddMiss.Enabled = chkMission.Checked;
			cmdRemoveMiss.Enabled = chkMission.Checked;
			cboFG.Enabled = chkMission.Checked;
			optMarkings.Enabled = chkMission.Checked;
			optIff.Enabled = chkMission.Checked;
			optPilot.Enabled = chkMission.Checked;
			cboMarkings.Enabled = chkMission.Checked;
			cboIff.Enabled = chkMission.Checked;
			txtPilot.Enabled = chkMission.Checked;
		}

		private void cmdAddMiss_Click(object sender, EventArgs e)
		{
			if (cboFG.SelectedIndex == -1 || (optMarkings.Checked && cboMarkings.SelectedIndex == -1) || (optIff.Checked && cboIff.SelectedIndex == -1)
				|| (optPilot.Checked && txtPilot.Text == "")) return;

			if (optMarkings.Checked) lstMission.Items.Add(cboFG.Text + ",marks," + cboMarkings.Text);
			else if (optIff.Checked) lstMission.Items.Add(cboFG.Text + ",iff," + cboIff.Text);
			else if (optPilot.Checked) lstMission.Items.Add(cboFG.Text + ",pilot," + txtPilot.Text);

		}
		private void cmdRemoveMiss_Click(object sender, EventArgs e)
		{
			if (lstMission.SelectedIndex == -1) return;
			lstMission.Items.RemoveAt(lstMission.SelectedIndex);
		}
		#endregion

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (!chkBackdrops.Checked && _bdFile != "") File.Delete(_bdFile);

			if (!chkMission.Checked && _missionTxtFile != "") File.Delete(_missionTxtFile);

			if (!chkBackdrops.Checked && !chkMission.Checked)
			{
				File.Delete(_fileName);
				Close();
				return;
			}

			string backup = _fileName.Replace(".ini", ".bak");
			if (File.Exists(_fileName))
			{
				File.Copy(_fileName, backup);
				File.Delete(_fileName);
			}
			StreamWriter sw = null;
			try
			{
				sw = new StreamWriter(_fileName);
				sw.WriteLine(";" + _mission + ".ini");
				sw.WriteLine("");

				if (chkBackdrops.Checked && lstBackdrops.Items.Count > 0)
				{
					sw.WriteLine("[Resdata]");
					for (int i = 0; i < lstBackdrops.Items.Count; i++) sw.WriteLine(lstBackdrops.Items[i]);
					sw.WriteLine("");
				}
				if (chkMission.Checked && lstMission.Items.Count > 0)
				{
					sw.WriteLine("[Mission_Tie]");
					for(int i = 0; i < lstMission.Items.Count; i++)
					{
						string[] parts = lstMission.Items[i].ToString().Split(',');
						int fg;
						for (fg = 0; fg < cboFG.Items.Count; fg++) if (cboFG.Items[fg].ToString() == parts[0]) break;
						if (parts[1] == "marks")
						{
							for (int m = 0; m < cboMarkings.Items.Count; m++)
								if (cboMarkings.Items[m].ToString() == parts[2])
								{
									sw.WriteLine("fg, " + fg + ", markings, " + m);
									break;
								}
						}
						else if (parts[1] == "iff")
						{
							for (int iff = 0; iff < cboIff.Items.Count; iff++)
								if (cboIff.Items[iff].ToString() == parts[2])
								{
									sw.WriteLine("fg, " + fg + ", iff, " + iff);
									break;
								}
						}
						else if (parts[1] == "pilot")
							sw.WriteLine("fg, " + fg + ", pilotvoice, " + parts[2]);
					}
					sw.WriteLine("");
				}
				sw.Flush();
				sw.Close();
				if (_bdFile != "") File.Delete(_bdFile);
				if (_missionTxtFile != "") File.Delete(_missionTxtFile);
			}
			catch
			{
				if (sw != null) sw.Close();
				if (File.Exists(backup))
				{
					File.Delete(_fileName);
					File.Copy(backup, _fileName);
				}
			}
			File.Delete(backup);
			Close();
		}
	}
}
