/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2018 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.4+
 */

/* CHANGELOG
 * [UPD] added _team for proper Roles [JB]
 * [UPD] ADT[4] changed to "100% of CraftWhen Player's Craft" [JB]
 * [UPD] return ADT[0] changed to ">=1 CraftWhen Player's Craft" [JB]
 * [UPD] beacon orders added [JB]
 * v1.4, 171016
 * [NEW] created (#13)
 */

using System;
using System.Windows.Forms;
using Idmr.Platform.Xwa;

namespace Idmr.Yogeme
{
	public partial class HyperbuoyDialog : Form
	{
		Mission _mission;
		byte _iff;
        byte _team;
		byte _playerFG;

		public HyperbuoyDialog(Mission mission)
		{
			InitializeComponent();
			_mission = mission;
			lstBuoys.Items.Clear();
			for(int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (_mission.FlightGroups[i].PlayerNumber == 1)
				{
					System.Diagnostics.Debug.WriteLine("player found");
					_iff = _mission.FlightGroups[i].IFF;
                    _team = _mission.FlightGroups[i].Team;
					_playerFG = (byte)i;
				}
				if (_mission.FlightGroups[i].CraftType == 0x55)
					lstBuoys.Items.Add(getBuoyText(_mission.FlightGroups[i]));
			}
			for (int i = 0; i < 4; i++)
			{
				cboFrom.Items.Add(_mission.Regions[i]);
				cboTo.Items.Add(_mission.Regions[i]);
			}
			cboFrom.SelectedIndex = 0;
			cboTo.SelectedIndex = 1;
		}

		string getBuoyText(FlightGroup fg)
		{
			string item = fg.Name;
			item += " (R";
			if (fg.Designation1 >= 0xC && fg.Designation1 <= 0xF)
			{
				item += (fg.Waypoints[0].Region + 1);
				item += " From R";
				item += (fg.Designation1 - 0xB);
			}
			else
			{
				item += (fg.Waypoints[0].Region + 1);
				item += " To R";
				item += (fg.Designation1 - 0xF);
			}
			item += ")";
			return item;
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void cmdGenerate_Click(object sender, EventArgs e)
		{
			if (_mission.FlightGroups.Count > Mission.FlightGroupLimit - (chkReturn.Checked ? 4 : 2))
			{
				MessageBox.Show("Adding buoys will exceed maximum number of allowed Flight Groups in the mission.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			FlightGroup to = new FlightGroup();
			to.CraftType = 0x55;
			to.Name = "To " + cboTo.Text;
			to.IFF = _iff;
            to.EnableDesignation1 = _team;
			to.Designation1 = (byte)(0x10 + cboTo.SelectedIndex);
			to.Waypoints[0].Region = (byte)cboFrom.SelectedIndex;
			to.DepartureTimerSeconds = 5;
            to.ArrDepTriggers[4].Amount = 0;                // 100% (if multiplayer, wait for all before de-spawning)
            to.ArrDepTriggers[4].VariableType = 7;          // CraftWhen
            to.ArrDepTriggers[4].Variable = 9;              // Player's craft
			to.ArrDepTriggers[4].Condition = 0x30;          // must leave Region
			to.ArrDepTriggers[4].Parameter1 = (byte)(cboFrom.SelectedIndex + 1);
            to.Orders[(byte)cboFrom.SelectedIndex, 0].Command = 0x31;  //[JB] Set beaconing order. Not required, but looks nice and follows standard convention.
			_mission.FlightGroups.Add(to);
			lstBuoys.Items.Add(getBuoyText(to));

			FlightGroup from = new FlightGroup();
			from.CraftType = 0x55;
			from.Name = "From " + cboFrom.Text;
			from.IFF = _iff;
            from.EnableDesignation1 = _team;
			from.Designation1 = (byte)(0xC + cboFrom.SelectedIndex);
			from.Waypoints[0].Region = (byte)cboTo.SelectedIndex;
			from.DepartureTimerSeconds = 5;
            from.ArrDepTriggers[4].Amount = 0;              // 100% (if multiplayer, wait for all before de-spawning)
            from.ArrDepTriggers[4].VariableType = 7;        // CraftWhen
            from.ArrDepTriggers[4].Variable = 9;            // Player's craft
			from.ArrDepTriggers[4].Condition = 0x2F;        // must arrive in Region
			from.ArrDepTriggers[4].Parameter1 = (byte)(cboTo.SelectedIndex + 1);
            from.Orders[(byte)cboTo.SelectedIndex, 0].Command = 0x31;
            _mission.FlightGroups.Add(from);
			lstBuoys.Items.Add(getBuoyText(from));

			if (chkReturn.Checked)
			{
				to = new FlightGroup();
				to.CraftType = 0x55;
				to.Name = "To " + cboFrom.Text;
				to.IFF = _iff;
                to.EnableDesignation1 = _team;
                to.Designation1 = (byte)(0x10 + cboFrom.SelectedIndex);
				to.Waypoints[0].Region = (byte)cboTo.SelectedIndex;
				// Arrival T1
                to.ArrDepTriggers[0].Amount = 4;            // At least one
                to.ArrDepTriggers[0].VariableType = 7;      // CraftWhen
                to.ArrDepTriggers[0].Variable = 9;          // Player's craft
				to.ArrDepTriggers[0].Condition = 0x2F;      // must arrive in Region
				to.ArrDepTriggers[0].Parameter1 = (byte)(cboTo.SelectedIndex + 1);
				// Depart T1
				to.DepartureTimerSeconds = 5;
                to.ArrDepTriggers[4].Amount = 0;            // 100% (if multiplayer, wait for all before de-spawning)
                to.ArrDepTriggers[4].VariableType = 7;      // CraftWhen
                to.ArrDepTriggers[4].Variable = 9;          // Player's craft
				to.ArrDepTriggers[4].Condition = 0x30;      // must leave Region
				to.ArrDepTriggers[4].Parameter1 = (byte)(cboTo.SelectedIndex + 1);
                to.Orders[(byte)cboTo.SelectedIndex, 0].Command = 0x31;
                _mission.FlightGroups.Add(to);
				lstBuoys.Items.Add(getBuoyText(to));

				from = new FlightGroup();
				from.CraftType = 0x55;
				from.Name = "From " + cboTo.Text;
				from.IFF = _iff;
                from.EnableDesignation1 = _team;
                from.Designation1 = (byte)(0xC + cboTo.SelectedIndex);
				from.Waypoints[0].Region = (byte)cboFrom.SelectedIndex;
				// Arrival T1
				from.ArrDepTriggers[0].Amount = 4;          // At least one
				from.ArrDepTriggers[0].VariableType = 7;    // CraftWhen
				from.ArrDepTriggers[0].Variable = 9;        // Player's craft
				from.ArrDepTriggers[0].Condition = 0x2F;    // must arrive in Region
				from.ArrDepTriggers[0].Parameter1 = (byte)(cboTo.SelectedIndex + 1);
				// Depart T1
				from.DepartureTimerSeconds = 5;
                from.ArrDepTriggers[4].Amount = 0;          // 100% (if multiplayer, wait for all before de-spawning)
                from.ArrDepTriggers[4].VariableType = 7;    // CraftWhen
                from.ArrDepTriggers[4].Variable = 9;        // Player's craft
				from.ArrDepTriggers[4].Condition = 0x2F;    // must arrive in Region
				from.ArrDepTriggers[4].Parameter1 = (byte)(cboFrom.SelectedIndex + 1);
                from.Orders[(byte)cboFrom.SelectedIndex, 0].Command = 0x31;
                _mission.FlightGroups.Add(from);
				lstBuoys.Items.Add(getBuoyText(from));
			}
		}
	}
}
