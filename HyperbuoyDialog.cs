/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.16
 *
 * CHANGELOG
 * v1.16, 241013
 * [UPD] Updates per format spec and enum use
 * v1.5, 180910
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
		readonly Mission _mission;
		readonly byte _iff;
		readonly byte _team;

		public HyperbuoyDialog(Mission mission)
		{
			InitializeComponent();
			_mission = mission;
			lstBuoys.Items.Clear();
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (_mission.FlightGroups[i].PlayerNumber == 1)
				{
					System.Diagnostics.Debug.WriteLine("player found");
					_iff = _mission.FlightGroups[i].IFF;
					_team = _mission.FlightGroups[i].Team;
				}
				if (_mission.FlightGroups[i].CraftType == 0x55)
					lstBuoys.Items.Add(getBuoyText(_mission.FlightGroups[i]));
			}
			for (int i = 0; i < 4; i++)
			{
				cboFrom.Items.Add(_mission.Regions[i].Name);
				cboTo.Items.Add(_mission.Regions[i].Name);
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

		void cmdClose_Click(object sender, EventArgs e) => Close();

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "Don't want to")]
		void cmdGenerate_Click(object sender, EventArgs e)
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
			int index = (int)FlightGroup.ArrDepTriggerIndex.Departure1;
			to.ArrDepTriggers[index].Amount = (byte)Mission.Trigger.AmountList.Percent100;
			to.ArrDepTriggers[index].VariableType = (byte)Mission.Trigger.TypeList.CraftWhen;
			to.ArrDepTriggers[index].Variable = 9; // Player's craft
			to.ArrDepTriggers[index].Condition = (byte)Mission.Trigger.ConditionList.DepartedRegion;
			to.ArrDepTriggers[index].Parameter = (short)(cboFrom.SelectedIndex + 1);
			to.Orders[(byte)cboFrom.SelectedIndex, 0].Command = (byte)FlightGroup.Order.CommandList.Beacon;
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
			from.ArrDepTriggers[index].Amount = (byte)Mission.Trigger.AmountList.Percent100;
			from.ArrDepTriggers[index].VariableType = (byte)Mission.Trigger.TypeList.CraftWhen;
			from.ArrDepTriggers[index].Variable = 9; // Player's craft
			from.ArrDepTriggers[index].Condition = (byte)Mission.Trigger.ConditionList.ArrivedInRegion;
			from.ArrDepTriggers[index].Parameter = (short)(cboTo.SelectedIndex + 1);
			from.Orders[(byte)cboTo.SelectedIndex, 0].Command = (byte)FlightGroup.Order.CommandList.Beacon;
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
				index = (int)FlightGroup.ArrDepTriggerIndex.Arrival1;
				to.ArrDepTriggers[index].Amount = (byte)Mission.Trigger.AmountList.AtLeast1;
				to.ArrDepTriggers[index].VariableType = (byte)Mission.Trigger.TypeList.CraftWhen;
				to.ArrDepTriggers[index].Variable = 9; // Player's craft
				to.ArrDepTriggers[index].Condition = (byte)Mission.Trigger.ConditionList.ArrivedInRegion;
				to.ArrDepTriggers[index].Parameter = (short)(cboTo.SelectedIndex + 1);
				index = (int)FlightGroup.ArrDepTriggerIndex.Departure1;
				to.DepartureTimerSeconds = 5;
				to.ArrDepTriggers[index].Amount = (byte)Mission.Trigger.AmountList.Percent100;
				to.ArrDepTriggers[index].VariableType = (byte)Mission.Trigger.TypeList.CraftWhen;
				to.ArrDepTriggers[index].Variable = 9; // Player's craft
				to.ArrDepTriggers[index].Condition = (byte)Mission.Trigger.ConditionList.DepartedRegion;
				to.ArrDepTriggers[index].Parameter = (short)(cboTo.SelectedIndex + 1);
				to.Orders[(byte)cboTo.SelectedIndex, 0].Command = (byte)FlightGroup.Order.CommandList.Beacon;
				_mission.FlightGroups.Add(to);
				lstBuoys.Items.Add(getBuoyText(to));

				from = new FlightGroup();
				from.CraftType = 0x55;
				from.Name = "From " + cboTo.Text;
				from.IFF = _iff;
				from.EnableDesignation1 = _team;
				from.Designation1 = (byte)(0xC + cboTo.SelectedIndex);
				from.Waypoints[0].Region = (byte)cboFrom.SelectedIndex;
				index = (int)FlightGroup.ArrDepTriggerIndex.Arrival1;
				from.ArrDepTriggers[index].Amount = (byte)Mission.Trigger.AmountList.AtLeast1;
				from.ArrDepTriggers[index].VariableType = (byte)Mission.Trigger.TypeList.CraftWhen;
				from.ArrDepTriggers[index].Variable = 9; // Player's craft
				from.ArrDepTriggers[index].Condition = (byte)Mission.Trigger.ConditionList.ArrivedInRegion;
				from.ArrDepTriggers[index].Parameter = (short)(cboTo.SelectedIndex + 1);
				index = (int)FlightGroup.ArrDepTriggerIndex.Departure1;
				from.DepartureTimerSeconds = 5;
				from.ArrDepTriggers[index].Amount = (byte)Mission.Trigger.AmountList.Percent100;
				from.ArrDepTriggers[index].VariableType = (byte)Mission.Trigger.TypeList.CraftWhen;
				from.ArrDepTriggers[index].Variable = 9; // Player's craft
				from.ArrDepTriggers[index].Condition = (byte)Mission.Trigger.ConditionList.ArrivedInRegion;
				from.ArrDepTriggers[index].Parameter = (short)(cboFrom.SelectedIndex + 1);
				from.Orders[(byte)cboFrom.SelectedIndex, 0].Command = (byte)FlightGroup.Order.CommandList.Beacon;
				_mission.FlightGroups.Add(from);
				lstBuoys.Items.Add(getBuoyText(from));
			}
		}
	}
}