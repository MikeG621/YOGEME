/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2020 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.8.2+
 */

/* CHANGELOG
 * [NEW] Created
 */

using Idmr.Platform.Xwa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	public partial class XwaWavForm : Form
	{
		readonly Settings _config;
		readonly Mission _mission;
		readonly string _lstFile;
		string[] _messages;
		readonly List<System.Windows.Media.MediaPlayer> _activeSounds = new List<System.Windows.Media.MediaPlayer>();
		readonly string _frontend;
		readonly int _battleNumber;
		readonly int _missionNumber;

		/// <summary>Create a new instance of the form</summary>
		/// <param name="config">The current configuration</param>
		/// <param name="mission">The current mission</param>
		public XwaWavForm(Settings config, Mission mission)
		{
			InitializeComponent();
			_config = config;
			_mission = mission;
			_lstFile = _config.XwaPath + "\\Wave\\MissionVoice\\" + Path.GetFileNameWithoutExtension(_mission.MissionFileName) + ".LST";
			// need to confirm if double-digits are allowed in B or M numbers, and if triple is allowed in message indexes
			_frontend = _config.XwaPath + "\\Wave\\FrontEnd\\" + Path.GetFileNameWithoutExtension(_mission.MissionFileName).Substring(1).Remove(4) + "\\";
			_battleNumber = int.Parse(Directory.GetParent(_frontend).Name.Substring(1, 1));
			_missionNumber = int.Parse(Directory.GetParent(_frontend).Name.Substring(3, 1));
			opnWav.InitialDirectory = _config.XwaPath + "\\Wave\\";
			Reload();
		}

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

		void cmdPlay_Click(object sender, EventArgs e)
		{
			Button cmd = (Button)sender;
			TextBox txt;
			if (cmd.Name == "cmdPlayMessage") txt = txtMessage;
			else if (cmd.Name == "cmdPlayEom") txt = txtEom;
			else if (cmd.Name == "cmdPlayBriefing") txt = txtBriefing;
			else txt = txtPrePostWav;

			if (!File.Exists(_config.XwaPath + "\\Wave\\" + txt.Text)) return;
			var plr = new System.Windows.Media.MediaPlayer();
			plr.MediaEnded += mediaPlayer_MediaEnded;
			plr.Open(new Uri(_config.XwaPath + "\\Wave\\" + txt.Text));
			plr.Play();
			_activeSounds.Add(plr);
		}

		private void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstMessages.SelectedIndex == -1) return;

			lblMessage.Text = _mission.Messages[lstMessages.SelectedIndex].MessageString;
			lblNotes.Text = _mission.Messages[lstMessages.SelectedIndex].Note;
			lblFG.Text = _mission.FlightGroups[_mission.Messages[lstMessages.SelectedIndex].OriginatingFG].ToString();

			if (_messages != null) txtMessage.Text = _messages[lstMessages.SelectedIndex];
		}

		private void mediaPlayer_MediaEnded(object sender, EventArgs e)
		{
			_activeSounds.Remove((System.Windows.Media.MediaPlayer)sender);
		}

		private void lstEom_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstEom.SelectedIndex == -1) return;

			lblEom.Text = _mission.Teams[0].EndOfMissionMessages[lstEom.SelectedIndex];
			lblEomNote.Text = _mission.Teams[0].EomNotes[lstEom.SelectedIndex / 2];

			if (_messages != null) txtEom.Text = _messages[lstEom.SelectedIndex + 64];
		}

		private void lstBriefing_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstBriefing.SelectedIndex == -1) return;

			lblBriefing.Text = _mission.Briefings[0].BriefingString[lstBriefing.SelectedIndex];
			lblBriefingNote.Text = _mission.Briefings[0].BriefingStringsNotes[lstBriefing.SelectedIndex];

			txtBriefing.Text = _frontend.Substring(_config.XwaPath.Length + 6) + "B" + _battleNumber.ToString("D2") + _missionNumber.ToString("D2") + (lstBriefing.SelectedIndex + 1).ToString("D2") + ".WAV";
			cmdPlayBriefing.Visible = File.Exists(_config.XwaPath + "\\Wave\\" + txtBriefing.Text);
		}

		private void lstPrePostCategories_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstPrePostCategories.SelectedIndex == -1) return;

			lstPrePost.Items.Clear();
			string prefix = (lstPrePostCategories.SelectedIndex == 0 ? "N" : (lstPrePostCategories.SelectedIndex == 1 ? "S" : "W"));

			int index = 1;
			while (File.Exists(_frontend + prefix + _battleNumber.ToString("D2") + _missionNumber.ToString("D2") + index.ToString("D2") + ".WAV"))
			{
				lstPrePost.Items.Add("Message #" + index++);
			}
		}

		private void lstPrePost_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstPrePost.SelectedIndex == -1 || lstPrePostCategories.SelectedIndex == -1) return;

			string prefix = (lstPrePostCategories.SelectedIndex == 0 ? "N" : (lstPrePostCategories.SelectedIndex == 1 ? "S" : "W"));
			txtPrePostWav.Text = _frontend.Substring(_config.XwaPath.Length + 6) + prefix + _battleNumber.ToString("D2") + _missionNumber.ToString("D2") + (lstPrePost.SelectedIndex + 1).ToString("D2") + ".WAV";

			if (prefix == "N")
			{
				lblPrePostNote.Text = "N/A";
				txtPrePost.Text = "N/A";
			}
			else if (prefix == "S")
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
	}
}
