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
		string _res = "\\RESDATA\\";
		string _wave = "\\Wave\\";
		string _fm = "\\FlightModels\\";

		public XwaHookDialog(string missionFile)
		{
			InitializeComponent();
			_mission = Idmr.Common.StringFunctions.GetFileName(missionFile, false);
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
		}

		string checkFile(string extension)
		{
			if (File.Exists(_installDirectory + "\\Missions\\" + _mission + extension)) return _installDirectory + "\\Missions\\" + _mission + extension;
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
			Close();
		}
	}
}
