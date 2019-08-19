using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

		public XwaHookDialog(string missionFile)
		{
			InitializeComponent();
			_mission = Idmr.Common.StringFunctions.GetFileName(missionFile, false);
			if (_mission == "NewMission")
			{
				MessageBox.Show("Please perform inital save prior to hook assignment.", "New Mission detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cmdCancel_Click("NewMission", new EventArgs());
			}
			_fileName = missionFile.Replace(".tie", ".ini");
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
				_hangarMapFile = checkFile("_HamgarMap.txt");
				_famHangarMapFile = checkFile("_FamHangarMap.txt");
			}
			StreamReader srMission = null;
			string line = "";
			bool readLine = false;
			if (File.Exists(_fileName)) srMission = new StreamReader(_fileName);
			if (_bdFile != "")
			{
				StreamReader srBD = new StreamReader(_bdFile);
				while ((line = srBD.ReadLine()) != null)
				{
					lstBackdrops.Items.Add(line);
				}
				srBD.Close();
			}
			else if (srMission != null)
			{
				while ((line = srMission.ReadLine()) != null)
				{
					// TODO: this will need to be redone so it'll read in any order
					if (line.StartsWith("[")) readLine = false;
					if (readLine) lstBackdrops.Items.Add(line);
					else if (line.ToLower() == "[resdata]") readLine = true;
				}
			}

			srMission.Close();
			chkBackdrops.Checked = (lstBackdrops.Items.Count > 0);
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

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (!chkBackdrops.Checked)
			{
				File.Delete(_fileName);
				if (_bdFile != "") File.Delete(_bdFile);
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
				sw.Flush();
				sw.Close();
				if (_bdFile != "") File.Delete(_bdFile);
			}
			catch
			{
				if (sw != null) sw.Close();
				if (File.Exists(backup))
				{
					File.Copy(backup, _fileName);
					File.Delete(backup);
				}
			}
			Close();
		}
	}
}
