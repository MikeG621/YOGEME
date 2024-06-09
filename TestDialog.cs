/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.2.3
 */

/* CHANGELOG
 * v1.2.3, 141214
 * [UPD] change to MPL
 * v1.1.1, 120814
 * - class renamed
 * v1.1, 120715
 * - Created, enabled in v1.2
 */

using System;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	public partial class TestDialog : Form
	{
		Settings _config;

		public TestDialog(Settings settings)
		{
			_config = settings;
			InitializeComponent();
			chkVerify.Checked = _config.VerifyTest;
			chkDelete.Checked = _config.DeleteTestPilots;
			// can't check DoNotShow, otherwise this wouldn't have been launched in the first place :P
			if (!_config.Verify) chkVerify.Enabled = true;
		}

		void cmdCancel_Click(object sender, EventArgs e) => Close();

		void cmdTest_Click(object sender, EventArgs e)
		{
			_config.VerifyTest = chkVerify.Checked;
			_config.DeleteTestPilots = chkDelete.Checked;
			_config.ConfirmTest = !chkDoNotShow.Checked;
			Close();
		}
	}
}