/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2021 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.9.2
 */

/* CHANGELOG
 * v1.9.2, 210328
 * [FIX #49] Crash due to multi-digit battle or mission numbers
 * [FIX] Wrong EoM button hiding if message doesn't exist
 * [UPD] Notes about random pre-briefing messages
 * v1.9, 210108
 * [NEW] Created
 */

using Idmr.Platform.Xwa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>GUI to handle the mission .WAV files for XWA</summary>
	public partial class XwaWavForm : Form
	{
		readonly Settings _config;
		readonly Mission _mission;
		readonly string _lstFile;
		string[] _messages;
		readonly List<System.Windows.Media.MediaPlayer> _activeSounds = new List<System.Windows.Media.MediaPlayer>();	// prevents early GC
		readonly string _frontend;
		readonly string _wave;
		readonly int _battleNumber;
		readonly int _missionNumber;
		static readonly string _wavExt = ".WAV";
		static readonly string _tmpExt = ".tmp";
		static readonly string _bakExt = ".bak";

		/// <summary>Create a new instance of the form</summary>
		/// <param name="config">The current configuration</param>
		/// <param name="mission">The current mission</param>
		public XwaWavForm(Settings config, Mission mission)
		{
			InitializeComponent();
			_config = config;
			_mission = mission;
			_wave = _config.XwaPath + "\\Wave\\";
			_lstFile = _wave + "MissionVoice\\" + Path.GetFileNameWithoutExtension(_mission.MissionFileName) + ".LST";
			Regex rx = new Regex(@"1B(\d+)M(\d+)\D\w*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			Match match = rx.Match(Path.GetFileNameWithoutExtension(_mission.MissionFileName));
			if (!match.Success)
			{
				MessageBox.Show("Mission is not named correctly. Must start with \"1B#M#\".");
				Close();
				return;
			}
			_battleNumber = int.Parse(match.Groups[1].Value);
			_missionNumber = int.Parse(match.Groups[2].Value);
			_frontend = _wave + "FrontEnd\\B" + _battleNumber + "M" + _missionNumber + "\\";
			opnWav.InitialDirectory = _wave;
			Reload();
		}

		// Mission WAVs are located in /Wave/MISSIONVOICE. There's a <mission>.LST with 64 message WAVs and 6 EOM WAVs.
		// Not all are necessarily present, but they need to be listed properly so there's 70 lines, path is from /Wave/
		// The EOM WAVs are PMC1, PMC2, PMF1, PMF2, OMC1, OMC2

		// Over in /Wave/Frontend there's the other mission WAVs. Everything is auto, no LST, based on filename prefix (B0M1, etc).
		// Briefing strings are B's, the pre-briefing is N, S is mission description, W is mission complete
		// Looks like there's usually an "S" or "W" per paragraph, B's start with 2 (since string#1 is the title)
		// naming convention is [prefix][battle##][mission##][message##].WAV

		/// <summary>Refresh the form with the current mission data</summary>
		public void Reload()
		{
			lstMessages.Items.Clear();
			for (int i = 0; i < _mission.Messages.Count; i++)
				lstMessages.Items.Add("#" + (i + 1) + ": " + _mission.Messages[i].MessageString);
			lstBriefing.Items.Clear();
			for (int i = 0; i < _mission.Briefings[0].BriefingString.Length; i++)
				lstBriefing.Items.Add("#" + (i + 1) + ": " + (_mission.Briefings[0].BriefingString[i] != "" ? _mission.Briefings[0].BriefingString[i] : "(none)"));
			if (File.Exists(_lstFile))
			{
				StreamReader sr = File.OpenText(_lstFile);
				_messages = sr.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			}
		}

		void backupPrePost()
		{
			if (Directory.GetFiles(_frontend, _prefix + "*" + _bakExt).Length == 0)
			{
				string[] waves = Directory.GetFiles(_frontend, _prefix + "*" + _wavExt);
				for (int i = 0; i < waves.Length; i++) File.Copy(waves[i], waves[i] + _bakExt);
			}
		}

		void stopPlayback()
		{
			// this should really only have 1 entry at any time
			for (int i = 0; i < _activeSounds.Count; i++)
			{
				_activeSounds[i].Stop();
			}
		}

		#region controls
		private void cmdClose_Click(object sender, EventArgs e)
		{
			Close();
		}
		void cmdPlay_Click(object sender, EventArgs e)
		{
			Button cmd = (Button)sender;
			TextBox txt;
			if (cmd.Name == "cmdPlayMessage") txt = txtMessage;
			else if (cmd.Name == "cmdPlayEom") txt = txtEom;
			else if (cmd.Name == "cmdPlayBriefing") txt = txtBriefing;
			else txt = txtPrePostWav;

			if (!File.Exists(_wave + txt.Text)) return;
			var plr = new System.Windows.Media.MediaPlayer();
			plr.MediaEnded += mediaPlayer_MediaEnded;
			plr.Open(new Uri(_wave + txt.Text));
			plr.Play();
			_activeSounds.Add(plr);
		}

		private void mediaPlayer_MediaEnded(object sender, EventArgs e)
		{
			var plr = (System.Windows.Media.MediaPlayer)sender;
			_activeSounds.Remove(plr);
			plr.Close();
		}

		private void cmdMessage_Click(object sender, EventArgs e)
		{
			bool isEom = ((Button)sender).Name == "cmdEom";
			var lst = (isEom ? lstEom : lstMessages);

			if (lst.SelectedIndex == -1) return;
			var txt = (isEom ? txtEom : txtMessage);

			DialogResult res = opnWav.ShowDialog();
			if (res != DialogResult.OK) return;
			if (opnWav.FileName.IndexOf(opnWav.InitialDirectory, StringComparison.InvariantCultureIgnoreCase) == -1)
			{
				MessageBox.Show("WAV file must be within the \"\\Wave\\\" directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			txt.Text = opnWav.FileName.Substring(opnWav.InitialDirectory.Length);

			if (_messages == null)
			{
				_messages = new string[70];
				for (int i = 0; i < _messages.Length; i++) _messages[i] = "none.wav";
			}
			_messages[isEom ? lst.SelectedIndex + 64 : lst.SelectedIndex] = txt.Text;
			(isEom ? cmdPlayEom : cmdPlayMessage).Visible = File.Exists(_wave + txt.Text);

			cmdSaveEom.Enabled = true;
			cmdSaveMessage.Enabled = true;
		}
		private void cmdSave_Click(object sender, EventArgs e)
		{
			string contents = string.Join("\r\n", _messages) + "\r\n";
			if (!File.Exists(_lstFile + _bakExt) && File.Exists(_lstFile)) File.Copy(_lstFile, _lstFile + _bakExt);
			File.Delete(_lstFile);
			StreamWriter sw = new FileInfo(_lstFile).CreateText();
			sw.Write(contents);
			sw.Close();

			cmdSaveMessage.Enabled = false;
			cmdSaveEom.Enabled = false;
		}
		private void lstEom_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstEom.SelectedIndex == -1) return;

			stopPlayback();
			lblEom.Text = _mission.Teams[0].EndOfMissionMessages[lstEom.SelectedIndex];
			lblEomNote.Text = _mission.Teams[0].EomNotes[lstEom.SelectedIndex / 2];

			if (_messages != null) txtEom.Text = _messages[lstEom.SelectedIndex + 64];
			cmdPlayEom.Visible = File.Exists(_wave + txtEom.Text);
		}
		private void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstMessages.SelectedIndex == -1) return;

			stopPlayback();
			lblMessage.Text = _mission.Messages[lstMessages.SelectedIndex].MessageString;
			lblNotes.Text = _mission.Messages[lstMessages.SelectedIndex].Note;
			lblFG.Text = _mission.FlightGroups[_mission.Messages[lstMessages.SelectedIndex].OriginatingFG].ToString();

			if (_messages != null) txtMessage.Text = _messages[lstMessages.SelectedIndex];
			cmdPlayMessage.Visible = File.Exists(_wave + txtMessage.Text);
		}

		private void cmdAdd_Click(object sender, EventArgs e)
		{
			if (lstPrePostCategories.SelectedIndex == -1) return;

			stopPlayback();
			DialogResult res = opnWav.ShowDialog();
			if (res != DialogResult.OK) return;

			backupPrePost();

			lstPrePost.Items.Add("Message #" + (lstPrePost.Items.Count + 1));
			lstPrePost.SelectedIndex = lstPrePost.Items.Count - 1;
			File.Copy(opnWav.FileName, _wave + txtPrePostWav.Text, true);
		}
		private void cmdDown_Click(object sender, EventArgs e)
		{
			backupPrePost();

			int index = lstPrePost.SelectedIndex + 1;
			if (index == lstPrePost.Items.Count) return; // shouldn't be possible

			string wave = _frontend + _prefix + _battleNumber.ToString("D2") + _missionNumber.ToString("D2");
			File.Copy(wave + index.ToString("D2") + _wavExt, wave + _tmpExt);
			File.Copy(wave + (index + 1).ToString("D2") + _wavExt, wave + index.ToString("D2") + _wavExt, true);
			File.Copy(wave + _tmpExt, wave + (index + 1).ToString("D2") + ".WAVE", true);
			File.Delete(wave + _tmpExt);

			lstPrePost.SelectedIndex++;
		}
		private void cmdPrePost_Click(object sender, EventArgs e)
		{
			bool isBriefing = ((Button)sender).Name == "cmdBriefing";

			if ((isBriefing && lstBriefing.SelectedIndex == -1) || (!isBriefing && (lstPrePostCategories.SelectedIndex == -1 || lstPrePost.SelectedIndex == -1))) return;

			DialogResult res = opnWav.ShowDialog();
			if (res != DialogResult.OK) return;

			string wave = _wave + (isBriefing ? txtBriefing.Text : txtPrePostWav.Text);
			if (isBriefing && File.Exists(wave) && !File.Exists(wave + _bakExt)) File.Copy(wave, wave + _bakExt);
			else if (!isBriefing) backupPrePost();

			File.Copy(opnWav.FileName, wave, true);
		}
		private void cmdRemove_Click(object sender, EventArgs e)
		{
			if (lstPrePostCategories.SelectedIndex == -1 || lstPrePost.SelectedIndex == -1) return;

			stopPlayback();
			backupPrePost();

			int index = lstPrePost.SelectedIndex + 1;
			string wave = _frontend + _prefix + _battleNumber.ToString("D2") + _missionNumber.ToString("D2");
			for (; index < lstPrePost.Items.Count; index++)
				File.Copy(wave + (index + 1).ToString("D2") + _wavExt, wave + index.ToString("D2") + _wavExt, true);
			File.Delete(wave + index.ToString("D2") + _wavExt);

			lstPrePost.Items.RemoveAt(lstPrePost.Items.Count - 1);
		}
		private void cmdUp_Click(object sender, EventArgs e)
		{
			backupPrePost();

			int index = lstPrePost.SelectedIndex + 1;
			if (index == 0) return; // shouldn't be possible

			string wave = _frontend + _prefix + _battleNumber.ToString("D2") + _missionNumber.ToString("D2");
			File.Copy(wave + index.ToString("D2") + _wavExt, wave + _tmpExt);
			File.Copy(wave + (index - 1).ToString("D2") + _wavExt, wave + index.ToString("D2") + _wavExt, true);
			File.Copy(wave + _tmpExt, wave + (index - 1).ToString("D2") + _wavExt, true);
			File.Delete(wave + _tmpExt);

			lstPrePost.SelectedIndex--;
		}
		private void lstBriefing_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstBriefing.SelectedIndex == -1) return;

			stopPlayback();
			lblBriefing.Text = _mission.Briefings[0].BriefingString[lstBriefing.SelectedIndex];
			lblBriefingNote.Text = _mission.Briefings[0].BriefingStringsNotes[lstBriefing.SelectedIndex];

			txtBriefing.Text = _frontend.Substring(_wave.Length) + "B" + _battleNumber.ToString("D2") + _missionNumber.ToString("D2") + (lstBriefing.SelectedIndex + 1).ToString("D2") + _wavExt;
			cmdPlayBriefing.Visible = File.Exists(_wave + txtBriefing.Text);
		}
		private void lstPrePost_SelectedIndexChanged(object sender, EventArgs e)
		{
			cmdRemove.Enabled = lstPrePost.SelectedIndex != -1;
			if (lstPrePost.SelectedIndex == -1 || lstPrePostCategories.SelectedIndex == -1)
			{
				cmdUp.Enabled = false;
				cmdDown.Enabled = false;
				return;
			}

			stopPlayback();
			txtPrePostWav.Text = _frontend.Substring(_wave.Length) + _prefix + _battleNumber.ToString("D2") + _missionNumber.ToString("D2") + (lstPrePost.SelectedIndex + 1).ToString("D2") + _wavExt;

			cmdUp.Enabled = lstPrePost.SelectedIndex != 0;
			cmdDown.Enabled = lstPrePost.SelectedIndex != lstPrePost.Items.Count - 1;

			if (_prefix == "N")
			{
				lblPrePostNote.Text = "N/A";
				txtPrePost.Text = "N/A";
			}
			else if (_prefix == "S")
			{
				lblPrePostNote.Text = _mission.DescriptionNotes;
				txtPrePost.Text = _mission.MissionDescription;
			}
			else
			{
				lblPrePostNote.Text = _mission.SuccessfulNotes;
				txtPrePost.Text = _mission.MissionSuccessful;
			}
		}
		private void lstPrePostCategories_SelectedIndexChanged(object sender, EventArgs e)
		{
			cmdUp.Enabled = false;
			cmdDown.Enabled = false;
			cmdRemove.Enabled = false;

			if (lstPrePostCategories.SelectedIndex == -1) return;

			stopPlayback();
			lstPrePost.Items.Clear();


			int index = 1;
			while (File.Exists(_frontend + _prefix + _battleNumber.ToString("D2") + _missionNumber.ToString("D2") + index.ToString("D2") + _wavExt))
			{
				lstPrePost.Items.Add("Message #" + index++);
			}
			cmdAdd.Enabled = true;

			if (lstPrePostCategories.SelectedIndex == 0 && lstPrePost.Items.Count == 0)
			{
				string off = "??", officer = "Unknown";
				switch (_mission.Officer)
				{
					case 0: off = "DE"; officer = "Devers"; break;
					case 1: off = "KU"; officer = "Kupalo"; break;
					case 2: off = "ZL"; officer = "Zaletta"; break;
					case 8: off = "MC"; officer = "Emkay";  break;
				}
				txtPrePost.Text = "(None defined, randomly selected based on Briefing Officer, " + officer + ")";
				txtPrePostWav.Text = "FrontEnd\\N01" + off + "##.WAV";
			}
			else
			{
				txtPrePost.Text = "";
				txtPrePostWav.Text = "";
			}
		}
		#endregion controls

		/// <summary>Gets the letter prefix for PrePost messages, lstPrePostCategories.SI must not be <b>-1</b></summary>
		string _prefix => (lstPrePostCategories.SelectedIndex == 0 ? "N" : (lstPrePostCategories.SelectedIndex == 1 ? "S" : "W"));
	}
}
