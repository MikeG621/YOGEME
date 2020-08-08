using System.Windows.Forms;

namespace Idmr.Yogeme
{
	partial class XvtForm
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XvtForm));
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tabFG = new System.Windows.Forms.TabPage();
			this.tabFGMinor = new System.Windows.Forms.TabControl();
			this.tabCraft = new System.Windows.Forms.TabPage();
			this.cmdMoveFGDown = new System.Windows.Forms.Button();
			this.cmdMoveFGUp = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkRandSC = new System.Windows.Forms.CheckBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblNotUsed = new System.Windows.Forms.Label();
			this.txtSpecCargo = new System.Windows.Forms.TextBox();
			this.numSC = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.txtCargo = new System.Windows.Forms.TextBox();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.grpCraft3 = new System.Windows.Forms.GroupBox();
			this.chkPreventNumbering = new System.Windows.Forms.CheckBox();
			this.numWaves = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.numCraft = new System.Windows.Forms.NumericUpDown();
			this.numGG = new System.Windows.Forms.NumericUpDown();
			this.numGU = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.grpCraft2 = new System.Windows.Forms.GroupBox();
			this.cboCraft = new System.Windows.Forms.ComboBox();
			this.numLead = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.numSpacing = new System.Windows.Forms.NumericUpDown();
			this.cboIFF = new System.Windows.Forms.ComboBox();
			this.cboAI = new System.Windows.Forms.ComboBox();
			this.cboMarkings = new System.Windows.Forms.ComboBox();
			this.cboPlayer = new System.Windows.Forms.ComboBox();
			this.cboFormation = new System.Windows.Forms.ComboBox();
			this.cmdForms = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.cboRadio = new System.Windows.Forms.ComboBox();
			this.label58 = new System.Windows.Forms.Label();
			this.cboPosition = new System.Windows.Forms.ComboBox();
			this.label111 = new System.Windows.Forms.Label();
			this.cboTeam = new System.Windows.Forms.ComboBox();
			this.lblFG = new System.Windows.Forms.Label();
			this.lblStarting = new System.Windows.Forms.Label();
			this.grpCraft4 = new System.Windows.Forms.GroupBox();
			this.cmdBackdrop = new System.Windows.Forms.Button();
			this.numBackdrop = new System.Windows.Forms.NumericUpDown();
			this.label122 = new System.Windows.Forms.Label();
			this.lblExplode = new System.Windows.Forms.Label();
			this.numExplode = new System.Windows.Forms.NumericUpDown();
			this.label67 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.cboWarheads = new System.Windows.Forms.ComboBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.cboBeam = new System.Windows.Forms.ComboBox();
			this.cboStatus = new System.Windows.Forms.ComboBox();
			this.label20 = new System.Windows.Forms.Label();
			this.cboStatus2 = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cboCounter = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tabArrDep = new System.Windows.Forms.TabPage();
			this.chkArrHuman = new System.Windows.Forms.CheckBox();
			this.cmdCopyAD = new System.Windows.Forms.Button();
			this.imgToolbar = new System.Windows.Forms.ImageList(this.components);
			this.label36 = new System.Windows.Forms.Label();
			this.grpDep = new System.Windows.Forms.GroupBox();
			this.label25 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.numDepClockSec = new System.Windows.Forms.NumericUpDown();
			this.numDepClockMin = new System.Windows.Forms.NumericUpDown();
			this.numDepMin = new System.Windows.Forms.NumericUpDown();
			this.numDepSec = new System.Windows.Forms.NumericUpDown();
			this.optDepAND = new System.Windows.Forms.RadioButton();
			this.optDepOR = new System.Windows.Forms.RadioButton();
			this.label47 = new System.Windows.Forms.Label();
			this.label41 = new System.Windows.Forms.Label();
			this.label40 = new System.Windows.Forms.Label();
			this.label39 = new System.Windows.Forms.Label();
			this.label37 = new System.Windows.Forms.Label();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.optDepMSAlt = new System.Windows.Forms.RadioButton();
			this.optDepHypAlt = new System.Windows.Forms.RadioButton();
			this.cboDepMSAlt = new System.Windows.Forms.ComboBox();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.optDepHyp = new System.Windows.Forms.RadioButton();
			this.cboDepMS = new System.Windows.Forms.ComboBox();
			this.optDepMS = new System.Windows.Forms.RadioButton();
			this.lblDep1 = new System.Windows.Forms.Label();
			this.cboAbort = new System.Windows.Forms.ComboBox();
			this.lblDep2 = new System.Windows.Forms.Label();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.numArrSec = new System.Windows.Forms.NumericUpDown();
			this.numArrMin = new System.Windows.Forms.NumericUpDown();
			this.panel10 = new System.Windows.Forms.Panel();
			this.optArr3AND4 = new System.Windows.Forms.RadioButton();
			this.optArr3OR4 = new System.Windows.Forms.RadioButton();
			this.panel9 = new System.Windows.Forms.Panel();
			this.optArr1AND2 = new System.Windows.Forms.RadioButton();
			this.optArr1OR2 = new System.Windows.Forms.RadioButton();
			this.optArr12AND34 = new System.Windows.Forms.RadioButton();
			this.optArr12OR34 = new System.Windows.Forms.RadioButton();
			this.label38 = new System.Windows.Forms.Label();
			this.lblArr1 = new System.Windows.Forms.Label();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.optArrHypAlt = new System.Windows.Forms.RadioButton();
			this.cboArrMSAlt = new System.Windows.Forms.ComboBox();
			this.optArrMSAlt = new System.Windows.Forms.RadioButton();
			this.groupBox12 = new System.Windows.Forms.GroupBox();
			this.cboArrMS = new System.Windows.Forms.ComboBox();
			this.optArrHyp = new System.Windows.Forms.RadioButton();
			this.optArrMS = new System.Windows.Forms.RadioButton();
			this.lblArr2 = new System.Windows.Forms.Label();
			this.label42 = new System.Windows.Forms.Label();
			this.label43 = new System.Windows.Forms.Label();
			this.lblArr3 = new System.Windows.Forms.Label();
			this.lblArr4 = new System.Windows.Forms.Label();
			this.cboADTrigAmount = new System.Windows.Forms.ComboBox();
			this.cboADTrigType = new System.Windows.Forms.ComboBox();
			this.cboADTrigVar = new System.Windows.Forms.ComboBox();
			this.cboADTrig = new System.Windows.Forms.ComboBox();
			this.label44 = new System.Windows.Forms.Label();
			this.cboDiff = new System.Windows.Forms.ComboBox();
			this.label45 = new System.Windows.Forms.Label();
			this.label46 = new System.Windows.Forms.Label();
			this.cmdPasteAD = new System.Windows.Forms.Button();
			this.tabGoals = new System.Windows.Forms.TabPage();
			this.lblGoalTimeLimit = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.grpGoal = new System.Windows.Forms.GroupBox();
			this.label61 = new System.Windows.Forms.Label();
			this.cboGoalAmount = new System.Windows.Forms.ComboBox();
			this.cboGoalArgument = new System.Windows.Forms.ComboBox();
			this.cboGoalTrigger = new System.Windows.Forms.ComboBox();
			this.label66 = new System.Windows.Forms.Label();
			this.chkGoalEnable = new System.Windows.Forms.CheckBox();
			this.numGoalPoints = new System.Windows.Forms.NumericUpDown();
			this.label65 = new System.Windows.Forms.Label();
			this.label62 = new System.Windows.Forms.Label();
			this.txtGoalInc = new System.Windows.Forms.TextBox();
			this.label60 = new System.Windows.Forms.Label();
			this.groupBox16 = new System.Windows.Forms.GroupBox();
			this.lblGoal1 = new System.Windows.Forms.Label();
			this.lblGoal2 = new System.Windows.Forms.Label();
			this.lblGoal3 = new System.Windows.Forms.Label();
			this.lblGoal4 = new System.Windows.Forms.Label();
			this.lblGoal5 = new System.Windows.Forms.Label();
			this.lblGoal8 = new System.Windows.Forms.Label();
			this.lblGoal6 = new System.Windows.Forms.Label();
			this.lblGoal7 = new System.Windows.Forms.Label();
			this.txtGoalComp = new System.Windows.Forms.TextBox();
			this.txtGoalFail = new System.Windows.Forms.TextBox();
			this.label63 = new System.Windows.Forms.Label();
			this.label64 = new System.Windows.Forms.Label();
			this.numGoalTimeLimit = new System.Windows.Forms.NumericUpDown();
			this.numGoalTeam = new System.Windows.Forms.NumericUpDown();
			this.tabWP = new System.Windows.Forms.TabPage();
			this.label76 = new System.Windows.Forms.Label();
			this.numRoll = new System.Windows.Forms.NumericUpDown();
			this.numPitch = new System.Windows.Forms.NumericUpDown();
			this.numYaw = new System.Windows.Forms.NumericUpDown();
			this.label56 = new System.Windows.Forms.Label();
			this.dataWP = new System.Windows.Forms.DataGrid();
			this.dataWP_Raw = new System.Windows.Forms.DataGrid();
			this.chkWPBrief1 = new System.Windows.Forms.CheckBox();
			this.chkWPHyp = new System.Windows.Forms.CheckBox();
			this.chkWP8 = new System.Windows.Forms.CheckBox();
			this.chkWP7 = new System.Windows.Forms.CheckBox();
			this.chkWP2 = new System.Windows.Forms.CheckBox();
			this.chkWP1 = new System.Windows.Forms.CheckBox();
			this.chkSP4 = new System.Windows.Forms.CheckBox();
			this.chkSP3 = new System.Windows.Forms.CheckBox();
			this.chkSP2 = new System.Windows.Forms.CheckBox();
			this.chkSP1 = new System.Windows.Forms.CheckBox();
			this.chkWP6 = new System.Windows.Forms.CheckBox();
			this.chkWP5 = new System.Windows.Forms.CheckBox();
			this.chkWP4 = new System.Windows.Forms.CheckBox();
			this.chkWP3 = new System.Windows.Forms.CheckBox();
			this.chkWPRend = new System.Windows.Forms.CheckBox();
			this.label77 = new System.Windows.Forms.Label();
			this.label78 = new System.Windows.Forms.Label();
			this.chkWPBrief2 = new System.Windows.Forms.CheckBox();
			this.chkWPBrief3 = new System.Windows.Forms.CheckBox();
			this.chkWPBrief4 = new System.Windows.Forms.CheckBox();
			this.chkWPBrief5 = new System.Windows.Forms.CheckBox();
			this.chkWPBrief6 = new System.Windows.Forms.CheckBox();
			this.chkWPBrief7 = new System.Windows.Forms.CheckBox();
			this.chkWPBrief8 = new System.Windows.Forms.CheckBox();
			this.tabOrders = new System.Windows.Forms.TabPage();
			this.cboOSpeed = new System.Windows.Forms.ComboBox();
			this.lblOSpeedNote = new System.Windows.Forms.Label();
			this.lblOVar2Note = new System.Windows.Forms.Label();
			this.lblOVar1Note = new System.Windows.Forms.Label();
			this.label57 = new System.Windows.Forms.Label();
			this.txtOString = new System.Windows.Forms.TextBox();
			this.label54 = new System.Windows.Forms.Label();
			this.cmdCopyOrder = new System.Windows.Forms.Button();
			this.lblODesc = new System.Windows.Forms.Label();
			this.grpSecOrder = new System.Windows.Forms.GroupBox();
			this.label49 = new System.Windows.Forms.Label();
			this.optOT3T4OR = new System.Windows.Forms.RadioButton();
			this.cboOT3 = new System.Windows.Forms.ComboBox();
			this.cboOT3Type = new System.Windows.Forms.ComboBox();
			this.cboOT4Type = new System.Windows.Forms.ComboBox();
			this.cboOT4 = new System.Windows.Forms.ComboBox();
			this.optOT3T4AND = new System.Windows.Forms.RadioButton();
			this.grpPrimOrder = new System.Windows.Forms.GroupBox();
			this.label50 = new System.Windows.Forms.Label();
			this.optOT1T2OR = new System.Windows.Forms.RadioButton();
			this.cboOT1 = new System.Windows.Forms.ComboBox();
			this.cboOT1Type = new System.Windows.Forms.ComboBox();
			this.cboOT2Type = new System.Windows.Forms.ComboBox();
			this.cboOT2 = new System.Windows.Forms.ComboBox();
			this.optOT1T2AND = new System.Windows.Forms.RadioButton();
			this.numOVar1 = new System.Windows.Forms.NumericUpDown();
			this.lblOVar1 = new System.Windows.Forms.Label();
			this.cboOThrottle = new System.Windows.Forms.ComboBox();
			this.label51 = new System.Windows.Forms.Label();
			this.groupBox15 = new System.Windows.Forms.GroupBox();
			this.lblOrder1 = new System.Windows.Forms.Label();
			this.lblOrder2 = new System.Windows.Forms.Label();
			this.lblOrder3 = new System.Windows.Forms.Label();
			this.lblOrder4 = new System.Windows.Forms.Label();
			this.cboOrders = new System.Windows.Forms.ComboBox();
			this.lblOVar2 = new System.Windows.Forms.Label();
			this.numOVar2 = new System.Windows.Forms.NumericUpDown();
			this.cmdPasteOrder = new System.Windows.Forms.Button();
			this.tapOption = new System.Windows.Forms.TabPage();
			this.grpRole = new System.Windows.Forms.GroupBox();
			this.cboRoleTeam4 = new System.Windows.Forms.ComboBox();
			this.cboRoleTeam3 = new System.Windows.Forms.ComboBox();
			this.cboRoleTeam2 = new System.Windows.Forms.ComboBox();
			this.cboRole1 = new System.Windows.Forms.ComboBox();
			this.cboRole2 = new System.Windows.Forms.ComboBox();
			this.cboRole3 = new System.Windows.Forms.ComboBox();
			this.cboRole4 = new System.Windows.Forms.ComboBox();
			this.cboRoleTeam1 = new System.Windows.Forms.ComboBox();
			this.groupBox23 = new System.Windows.Forms.GroupBox();
			this.cmdCopySkip = new System.Windows.Forms.Button();
			this.label71 = new System.Windows.Forms.Label();
			this.cboSkipAmount = new System.Windows.Forms.ComboBox();
			this.cboSkipType = new System.Windows.Forms.ComboBox();
			this.cboSkipVar = new System.Windows.Forms.ComboBox();
			this.cboSkipTrig = new System.Windows.Forms.ComboBox();
			this.label72 = new System.Windows.Forms.Label();
			this.cmdPasteSkip = new System.Windows.Forms.Button();
			this.optSkipAND = new System.Windows.Forms.RadioButton();
			this.optSkipOR = new System.Windows.Forms.RadioButton();
			this.lblSkipTrig1 = new System.Windows.Forms.Label();
			this.lblSkipTrig2 = new System.Windows.Forms.Label();
			this.groupBox22 = new System.Windows.Forms.GroupBox();
			this.lblOptCraft1 = new System.Windows.Forms.Label();
			this.cboOptCraft = new System.Windows.Forms.ComboBox();
			this.label70 = new System.Windows.Forms.Label();
			this.numOptWaves = new System.Windows.Forms.NumericUpDown();
			this.label69 = new System.Windows.Forms.Label();
			this.label68 = new System.Windows.Forms.Label();
			this.cboOptCat = new System.Windows.Forms.ComboBox();
			this.numOptCraft = new System.Windows.Forms.NumericUpDown();
			this.lblOptCraft2 = new System.Windows.Forms.Label();
			this.lblOptCraft3 = new System.Windows.Forms.Label();
			this.lblOptCraft4 = new System.Windows.Forms.Label();
			this.lblOptCraft5 = new System.Windows.Forms.Label();
			this.lblOptCraft6 = new System.Windows.Forms.Label();
			this.lblOptCraft7 = new System.Windows.Forms.Label();
			this.lblOptCraft8 = new System.Windows.Forms.Label();
			this.lblOptCraft9 = new System.Windows.Forms.Label();
			this.lblOptCraft10 = new System.Windows.Forms.Label();
			this.groupBox21 = new System.Windows.Forms.GroupBox();
			this.chkOptCNone = new System.Windows.Forms.CheckBox();
			this.chkOptCChaff = new System.Windows.Forms.CheckBox();
			this.chkOptCFlare = new System.Windows.Forms.CheckBox();
			this.chkOptCCluster = new System.Windows.Forms.CheckBox();
			this.groupBox20 = new System.Windows.Forms.GroupBox();
			this.chkOptBNone = new System.Windows.Forms.CheckBox();
			this.chkOptBTractor = new System.Windows.Forms.CheckBox();
			this.chkOptBJamming = new System.Windows.Forms.CheckBox();
			this.chkOptBDecoy = new System.Windows.Forms.CheckBox();
			this.chkOptBEnergy = new System.Windows.Forms.CheckBox();
			this.groupBox19 = new System.Windows.Forms.GroupBox();
			this.chkOptWNone = new System.Windows.Forms.CheckBox();
			this.chkOptWBomb = new System.Windows.Forms.CheckBox();
			this.chkOptWRocket = new System.Windows.Forms.CheckBox();
			this.chkOptWMissile = new System.Windows.Forms.CheckBox();
			this.chkOptWTorp = new System.Windows.Forms.CheckBox();
			this.chkOptWAdvMissile = new System.Windows.Forms.CheckBox();
			this.chkOptWAdvTorp = new System.Windows.Forms.CheckBox();
			this.chkOptWMagPulse = new System.Windows.Forms.CheckBox();
			this.chkOptWIonPulse = new System.Windows.Forms.CheckBox();
			this.tabUnk = new System.Windows.Forms.TabPage();
			this.grpUnkOther = new System.Windows.Forms.GroupBox();
			this.numUnk20 = new System.Windows.Forms.NumericUpDown();
			this.label93 = new System.Windows.Forms.Label();
			this.chkUnk17 = new System.Windows.Forms.CheckBox();
			this.chkUnk18 = new System.Windows.Forms.CheckBox();
			this.chkUnk19 = new System.Windows.Forms.CheckBox();
			this.numUnk21 = new System.Windows.Forms.NumericUpDown();
			this.label94 = new System.Windows.Forms.Label();
			this.chkUnk23 = new System.Windows.Forms.CheckBox();
			this.chkUnk22 = new System.Windows.Forms.CheckBox();
			this.chkUnk24 = new System.Windows.Forms.CheckBox();
			this.chkUnk25 = new System.Windows.Forms.CheckBox();
			this.chkUnk28 = new System.Windows.Forms.CheckBox();
			this.chkUnk29 = new System.Windows.Forms.CheckBox();
			this.chkUnk26 = new System.Windows.Forms.CheckBox();
			this.chkUnk27 = new System.Windows.Forms.CheckBox();
			this.grpUnkOrder = new System.Windows.Forms.GroupBox();
			this.numUnk6 = new System.Windows.Forms.NumericUpDown();
			this.label81 = new System.Windows.Forms.Label();
			this.numUnkOrder = new System.Windows.Forms.NumericUpDown();
			this.label88 = new System.Windows.Forms.Label();
			this.numUnk7 = new System.Windows.Forms.NumericUpDown();
			this.label84 = new System.Windows.Forms.Label();
			this.numUnk8 = new System.Windows.Forms.NumericUpDown();
			this.label89 = new System.Windows.Forms.Label();
			this.numUnk9 = new System.Windows.Forms.NumericUpDown();
			this.label90 = new System.Windows.Forms.Label();
			this.grpUnkAD = new System.Windows.Forms.GroupBox();
			this.numUnk5 = new System.Windows.Forms.NumericUpDown();
			this.label87 = new System.Windows.Forms.Label();
			this.numUnk4 = new System.Windows.Forms.NumericUpDown();
			this.label86 = new System.Windows.Forms.Label();
			this.numUnk3 = new System.Windows.Forms.NumericUpDown();
			this.label85 = new System.Windows.Forms.Label();
			this.grpUnkCraft = new System.Windows.Forms.GroupBox();
			this.numUnk1 = new System.Windows.Forms.NumericUpDown();
			this.label83 = new System.Windows.Forms.Label();
			this.chkUnk2 = new System.Windows.Forms.CheckBox();
			this.grpUnkGoal = new System.Windows.Forms.GroupBox();
			this.chkUnk10 = new System.Windows.Forms.CheckBox();
			this.numUnkGoal = new System.Windows.Forms.NumericUpDown();
			this.label92 = new System.Windows.Forms.Label();
			this.numUnk13 = new System.Windows.Forms.NumericUpDown();
			this.label95 = new System.Windows.Forms.Label();
			this.chkUnk11 = new System.Windows.Forms.CheckBox();
			this.chkUnk12 = new System.Windows.Forms.CheckBox();
			this.chkUnk14 = new System.Windows.Forms.CheckBox();
			this.numUnk16 = new System.Windows.Forms.NumericUpDown();
			this.label91 = new System.Windows.Forms.Label();
			this.chkUnk15 = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lstFG = new System.Windows.Forms.ListBox();
			this.tabMess = new System.Windows.Forms.TabPage();
			this.cmdMoveMessDown = new System.Windows.Forms.Button();
			this.cmdMoveMessUp = new System.Windows.Forms.Button();
			this.cboMessColor = new System.Windows.Forms.ComboBox();
			this.label109 = new System.Windows.Forms.Label();
			this.cboMessAmount = new System.Windows.Forms.ComboBox();
			this.cboMessType = new System.Windows.Forms.ComboBox();
			this.cboMessVar = new System.Windows.Forms.ComboBox();
			this.cboMessTrig = new System.Windows.Forms.ComboBox();
			this.label110 = new System.Windows.Forms.Label();
			this.grpMessages = new System.Windows.Forms.GroupBox();
			this.panel8 = new System.Windows.Forms.Panel();
			this.optMess3OR4 = new System.Windows.Forms.RadioButton();
			this.optMess3AND4 = new System.Windows.Forms.RadioButton();
			this.panel7 = new System.Windows.Forms.Panel();
			this.optMess1OR2 = new System.Windows.Forms.RadioButton();
			this.optMess1AND2 = new System.Windows.Forms.RadioButton();
			this.lblMess1 = new System.Windows.Forms.Label();
			this.lblMess2 = new System.Windows.Forms.Label();
			this.lblMess4 = new System.Windows.Forms.Label();
			this.lblMess3 = new System.Windows.Forms.Label();
			this.optMess12AND34 = new System.Windows.Forms.RadioButton();
			this.optMess12OR34 = new System.Windows.Forms.RadioButton();
			this.numMessDelay = new System.Windows.Forms.NumericUpDown();
			this.label55 = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.label52 = new System.Windows.Forms.Label();
			this.label53 = new System.Windows.Forms.Label();
			this.txtShort = new System.Windows.Forms.TextBox();
			this.lstMessages = new System.Windows.Forms.ListBox();
			this.grpSend = new System.Windows.Forms.GroupBox();
			this.chkMess1 = new System.Windows.Forms.CheckBox();
			this.chkMess2 = new System.Windows.Forms.CheckBox();
			this.chkMess3 = new System.Windows.Forms.CheckBox();
			this.chkMess4 = new System.Windows.Forms.CheckBox();
			this.chkMess5 = new System.Windows.Forms.CheckBox();
			this.chkMess10 = new System.Windows.Forms.CheckBox();
			this.chkMess9 = new System.Windows.Forms.CheckBox();
			this.chkMess8 = new System.Windows.Forms.CheckBox();
			this.chkMess7 = new System.Windows.Forms.CheckBox();
			this.chkMess6 = new System.Windows.Forms.CheckBox();
			this.tabGlob = new System.Windows.Forms.TabPage();
			this.label112 = new System.Windows.Forms.Label();
			this.cboGlobalTeam = new System.Windows.Forms.ComboBox();
			this.label33 = new System.Windows.Forms.Label();
			this.txtGlobalInc = new System.Windows.Forms.TextBox();
			this.label32 = new System.Windows.Forms.Label();
			this.numGlobalPoints = new System.Windows.Forms.NumericUpDown();
			this.label79 = new System.Windows.Forms.Label();
			this.label48 = new System.Windows.Forms.Label();
			this.cboGlobalAmount = new System.Windows.Forms.ComboBox();
			this.cboGlobalType = new System.Windows.Forms.ComboBox();
			this.cboGlobalVar = new System.Windows.Forms.ComboBox();
			this.cboGlobalTrig = new System.Windows.Forms.ComboBox();
			this.label59 = new System.Windows.Forms.Label();
			this.groupBox18 = new System.Windows.Forms.GroupBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.optPrim1OR2 = new System.Windows.Forms.RadioButton();
			this.optPrim1AND2 = new System.Windows.Forms.RadioButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.optPrim3OR4 = new System.Windows.Forms.RadioButton();
			this.optPrim3AND4 = new System.Windows.Forms.RadioButton();
			this.lblPrim1 = new System.Windows.Forms.Label();
			this.lblPrim2 = new System.Windows.Forms.Label();
			this.lblPrim4 = new System.Windows.Forms.Label();
			this.lblPrim3 = new System.Windows.Forms.Label();
			this.optPrim12AND34 = new System.Windows.Forms.RadioButton();
			this.optPrim12OR34 = new System.Windows.Forms.RadioButton();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.optPrev1AND2 = new System.Windows.Forms.RadioButton();
			this.optPrev1OR2 = new System.Windows.Forms.RadioButton();
			this.lblPrev1 = new System.Windows.Forms.Label();
			this.lblPrev2 = new System.Windows.Forms.Label();
			this.lblPrev4 = new System.Windows.Forms.Label();
			this.lblPrev3 = new System.Windows.Forms.Label();
			this.optPrev12AND34 = new System.Windows.Forms.RadioButton();
			this.optPrev12OR34 = new System.Windows.Forms.RadioButton();
			this.panel4 = new System.Windows.Forms.Panel();
			this.optPrev3OR4 = new System.Windows.Forms.RadioButton();
			this.optPrev3AND4 = new System.Windows.Forms.RadioButton();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.panel6 = new System.Windows.Forms.Panel();
			this.optSec3OR4 = new System.Windows.Forms.RadioButton();
			this.optSec3AND4 = new System.Windows.Forms.RadioButton();
			this.panel5 = new System.Windows.Forms.Panel();
			this.optSec1OR2 = new System.Windows.Forms.RadioButton();
			this.optSec1AND2 = new System.Windows.Forms.RadioButton();
			this.lblSec1 = new System.Windows.Forms.Label();
			this.lblSec2 = new System.Windows.Forms.Label();
			this.lblSec4 = new System.Windows.Forms.Label();
			this.lblSec3 = new System.Windows.Forms.Label();
			this.optSec12AND34 = new System.Windows.Forms.RadioButton();
			this.optSec12OR34 = new System.Windows.Forms.RadioButton();
			this.txtGlobalComp = new System.Windows.Forms.TextBox();
			this.txtGlobalFail = new System.Windows.Forms.TextBox();
			this.label34 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.tabTeam = new System.Windows.Forms.TabPage();
			this.groupBox32 = new System.Windows.Forms.GroupBox();
			this.groupBox33 = new System.Windows.Forms.GroupBox();
			this.cboPF2Color = new System.Windows.Forms.ComboBox();
			this.cboPF1Color = new System.Windows.Forms.ComboBox();
			this.txtPrimFail1 = new System.Windows.Forms.TextBox();
			this.txtPrimFail2 = new System.Windows.Forms.TextBox();
			this.groupBox34 = new System.Windows.Forms.GroupBox();
			this.cboSC2Color = new System.Windows.Forms.ComboBox();
			this.cboSC1Color = new System.Windows.Forms.ComboBox();
			this.txtSecComp1 = new System.Windows.Forms.TextBox();
			this.txtSecComp2 = new System.Windows.Forms.TextBox();
			this.groupBox35 = new System.Windows.Forms.GroupBox();
			this.cboPC2Color = new System.Windows.Forms.ComboBox();
			this.cboPC1Color = new System.Windows.Forms.ComboBox();
			this.txtPrimComp1 = new System.Windows.Forms.TextBox();
			this.txtPrimComp2 = new System.Windows.Forms.TextBox();
			this.txtTeamName = new System.Windows.Forms.TextBox();
			this.label96 = new System.Windows.Forms.Label();
			this.groupBox31 = new System.Windows.Forms.GroupBox();
			this.chkTeam1 = new System.Windows.Forms.CheckBox();
			this.chkTeam2 = new System.Windows.Forms.CheckBox();
			this.chkTeam3 = new System.Windows.Forms.CheckBox();
			this.chkTeam4 = new System.Windows.Forms.CheckBox();
			this.chkTeam5 = new System.Windows.Forms.CheckBox();
			this.chkTeam6 = new System.Windows.Forms.CheckBox();
			this.chkTeam7 = new System.Windows.Forms.CheckBox();
			this.chkTeam8 = new System.Windows.Forms.CheckBox();
			this.chkTeam9 = new System.Windows.Forms.CheckBox();
			this.chkTeam10 = new System.Windows.Forms.CheckBox();
			this.groupBox30 = new System.Windows.Forms.GroupBox();
			this.lblTeam1 = new System.Windows.Forms.Label();
			this.lblTeam2 = new System.Windows.Forms.Label();
			this.lblTeam3 = new System.Windows.Forms.Label();
			this.lblTeam4 = new System.Windows.Forms.Label();
			this.lblTeam5 = new System.Windows.Forms.Label();
			this.lblTeam6 = new System.Windows.Forms.Label();
			this.lblTeam7 = new System.Windows.Forms.Label();
			this.lblTeam8 = new System.Windows.Forms.Label();
			this.lblTeam9 = new System.Windows.Forms.Label();
			this.lblTeam10 = new System.Windows.Forms.Label();
			this.tabMission = new System.Windows.Forms.TabPage();
			this.grpIFF = new System.Windows.Forms.GroupBox();
			this.txtIFF6 = new System.Windows.Forms.TextBox();
			this.txtIFF5 = new System.Windows.Forms.TextBox();
			this.txtIFF4 = new System.Windows.Forms.TextBox();
			this.txtIFF3 = new System.Windows.Forms.TextBox();
			this.lblIFF6 = new System.Windows.Forms.Label();
			this.lblIFF5 = new System.Windows.Forms.Label();
			this.lblIFF4 = new System.Windows.Forms.Label();
			this.lblIFF3 = new System.Windows.Forms.Label();
			this.label150 = new System.Windows.Forms.Label();
			this.groupBox36 = new System.Windows.Forms.GroupBox();
			this.chkMissUnk3 = new System.Windows.Forms.CheckBox();
			this.numMissUnk1 = new System.Windows.Forms.NumericUpDown();
			this.label105 = new System.Windows.Forms.Label();
			this.label106 = new System.Windows.Forms.Label();
			this.numMissUnk2 = new System.Windows.Forms.NumericUpDown();
			this.chkPreventOutcome = new System.Windows.Forms.CheckBox();
			this.optXvT = new System.Windows.Forms.RadioButton();
			this.label104 = new System.Windows.Forms.Label();
			this.label102 = new System.Windows.Forms.Label();
			this.numMissTimeMin = new System.Windows.Forms.NumericUpDown();
			this.label101 = new System.Windows.Forms.Label();
			this.label100 = new System.Windows.Forms.Label();
			this.cboMissType = new System.Windows.Forms.ComboBox();
			this.label97 = new System.Windows.Forms.Label();
			this.txtMissDesc = new System.Windows.Forms.TextBox();
			this.txtMissSucc = new System.Windows.Forms.TextBox();
			this.txtMissFail = new System.Windows.Forms.TextBox();
			this.label98 = new System.Windows.Forms.Label();
			this.label99 = new System.Windows.Forms.Label();
			this.numMissTimeSec = new System.Windows.Forms.NumericUpDown();
			this.label103 = new System.Windows.Forms.Label();
			this.optBoP = new System.Windows.Forms.RadioButton();
			this.toolXvT = new System.Windows.Forms.ToolBar();
			this.toolNew = new System.Windows.Forms.ToolBarButton();
			this.toolOpen = new System.Windows.Forms.ToolBarButton();
			this.toolSave = new System.Windows.Forms.ToolBarButton();
			this.toolSaveAs = new System.Windows.Forms.ToolBarButton();
			this.toolSep1 = new System.Windows.Forms.ToolBarButton();
			this.toolNewItem = new System.Windows.Forms.ToolBarButton();
			this.toolDeleteItem = new System.Windows.Forms.ToolBarButton();
			this.toolCopy = new System.Windows.Forms.ToolBarButton();
			this.toolPaste = new System.Windows.Forms.ToolBarButton();
			this.toolSep2 = new System.Windows.Forms.ToolBarButton();
			this.toolMap = new System.Windows.Forms.ToolBarButton();
			this.toolBriefing = new System.Windows.Forms.ToolBarButton();
			this.toolVerify = new System.Windows.Forms.ToolBarButton();
			this.toolSep3 = new System.Windows.Forms.ToolBarButton();
			this.toolOptions = new System.Windows.Forms.ToolBarButton();
			this.toolLst = new System.Windows.Forms.ToolBarButton();
			this.toolHelp = new System.Windows.Forms.ToolBarButton();
			this.menuXvT = new System.Windows.Forms.MainMenu(this.components);
			this.menuFile = new System.Windows.Forms.MenuItem();
			this.menuNew = new System.Windows.Forms.MenuItem();
			this.menuNewXwing = new System.Windows.Forms.MenuItem();
			this.menuNewTIE = new System.Windows.Forms.MenuItem();
			this.menuNewXvT = new System.Windows.Forms.MenuItem();
			this.menuNewBoP = new System.Windows.Forms.MenuItem();
			this.menuNewXWA = new System.Windows.Forms.MenuItem();
			this.menuOpen = new System.Windows.Forms.MenuItem();
			this.menuRecent = new System.Windows.Forms.MenuItem();
			this.menuRec1 = new System.Windows.Forms.MenuItem();
			this.menuRec2 = new System.Windows.Forms.MenuItem();
			this.menuRec3 = new System.Windows.Forms.MenuItem();
			this.menuRec4 = new System.Windows.Forms.MenuItem();
			this.menuRec5 = new System.Windows.Forms.MenuItem();
			this.menuSave = new System.Windows.Forms.MenuItem();
			this.menuSaveAs = new System.Windows.Forms.MenuItem();
			this.menuSaveAsTIE = new System.Windows.Forms.MenuItem();
			this.menuSaveAsXvT = new System.Windows.Forms.MenuItem();
			this.menuSaveAsBoP = new System.Windows.Forms.MenuItem();
			this.menuSaveAsXWA = new System.Windows.Forms.MenuItem();
			this.menuItem23 = new System.Windows.Forms.MenuItem();
			this.menuExit = new System.Windows.Forms.MenuItem();
			this.menuEdit = new System.Windows.Forms.MenuItem();
			this.menuUndo = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuCut = new System.Windows.Forms.MenuItem();
			this.menuCopy = new System.Windows.Forms.MenuItem();
			this.menuPaste = new System.Windows.Forms.MenuItem();
			this.menuDelete = new System.Windows.Forms.MenuItem();
			this.menuTools = new System.Windows.Forms.MenuItem();
			this.menuVerify = new System.Windows.Forms.MenuItem();
			this.menuMap = new System.Windows.Forms.MenuItem();
			this.menuBrief = new System.Windows.Forms.MenuItem();
			this.menuLST = new System.Windows.Forms.MenuItem();
			this.menuOptions = new System.Windows.Forms.MenuItem();
			this.menuGoalSummary = new System.Windows.Forms.MenuItem();
			this.menuTest = new System.Windows.Forms.MenuItem();
			this.menuHelp = new System.Windows.Forms.MenuItem();
			this.menuHelpInfo = new System.Windows.Forms.MenuItem();
			this.menuAbout = new System.Windows.Forms.MenuItem();
			this.menuIDMR = new System.Windows.Forms.MenuItem();
			this.menuER = new System.Windows.Forms.MenuItem();
			this.opnXvT = new System.Windows.Forms.OpenFileDialog();
			this.savXvT = new System.Windows.Forms.SaveFileDialog();
			this.dataWaypoints = new System.Data.DataView();
			this.dataWaypoints_Raw = new System.Data.DataView();
			this.tabMain.SuspendLayout();
			this.tabFG.SuspendLayout();
			this.tabFGMinor.SuspendLayout();
			this.tabCraft.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numSC)).BeginInit();
			this.grpCraft3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numWaves)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numCraft)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numGG)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numGU)).BeginInit();
			this.grpCraft2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numLead)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numSpacing)).BeginInit();
			this.grpCraft4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numBackdrop)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numExplode)).BeginInit();
			this.tabArrDep.SuspendLayout();
			this.grpDep.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numDepClockSec)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numDepClockMin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numDepMin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numDepSec)).BeginInit();
			this.groupBox10.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.groupBox8.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numArrSec)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numArrMin)).BeginInit();
			this.panel10.SuspendLayout();
			this.panel9.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.groupBox12.SuspendLayout();
			this.tabGoals.SuspendLayout();
			this.grpGoal.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numGoalPoints)).BeginInit();
			this.groupBox16.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numGoalTimeLimit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numGoalTeam)).BeginInit();
			this.tabWP.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRoll)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPitch)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numYaw)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWP)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWP_Raw)).BeginInit();
			this.tabOrders.SuspendLayout();
			this.grpSecOrder.SuspendLayout();
			this.grpPrimOrder.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numOVar1)).BeginInit();
			this.groupBox15.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numOVar2)).BeginInit();
			this.tapOption.SuspendLayout();
			this.grpRole.SuspendLayout();
			this.groupBox23.SuspendLayout();
			this.groupBox22.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numOptWaves)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numOptCraft)).BeginInit();
			this.groupBox21.SuspendLayout();
			this.groupBox20.SuspendLayout();
			this.groupBox19.SuspendLayout();
			this.tabUnk.SuspendLayout();
			this.grpUnkOther.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUnk20)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk21)).BeginInit();
			this.grpUnkOrder.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUnk6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnkOrder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk9)).BeginInit();
			this.grpUnkAD.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUnk5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk3)).BeginInit();
			this.grpUnkCraft.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUnk1)).BeginInit();
			this.grpUnkGoal.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUnkGoal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk13)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk16)).BeginInit();
			this.tabMess.SuspendLayout();
			this.grpMessages.SuspendLayout();
			this.panel8.SuspendLayout();
			this.panel7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMessDelay)).BeginInit();
			this.grpSend.SuspendLayout();
			this.tabGlob.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numGlobalPoints)).BeginInit();
			this.groupBox18.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel5.SuspendLayout();
			this.tabTeam.SuspendLayout();
			this.groupBox32.SuspendLayout();
			this.groupBox33.SuspendLayout();
			this.groupBox34.SuspendLayout();
			this.groupBox35.SuspendLayout();
			this.groupBox31.SuspendLayout();
			this.groupBox30.SuspendLayout();
			this.tabMission.SuspendLayout();
			this.grpIFF.SuspendLayout();
			this.groupBox36.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMissUnk1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numMissUnk2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numMissTimeMin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numMissTimeSec)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWaypoints)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWaypoints_Raw)).BeginInit();
			this.SuspendLayout();
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tabFG);
			this.tabMain.Controls.Add(this.tabMess);
			this.tabMain.Controls.Add(this.tabGlob);
			this.tabMain.Controls.Add(this.tabTeam);
			this.tabMain.Controls.Add(this.tabMission);
			this.tabMain.Location = new System.Drawing.Point(0, 32);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(793, 536);
			this.tabMain.TabIndex = 0;
			this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
			// 
			// tabFG
			// 
			this.tabFG.Controls.Add(this.tabFGMinor);
			this.tabFG.Controls.Add(this.label1);
			this.tabFG.Controls.Add(this.lstFG);
			this.tabFG.Location = new System.Drawing.Point(4, 22);
			this.tabFG.Name = "tabFG";
			this.tabFG.Size = new System.Drawing.Size(785, 510);
			this.tabFG.TabIndex = 0;
			this.tabFG.Text = "Flight Groups";
			// 
			// tabFGMinor
			// 
			this.tabFGMinor.Controls.Add(this.tabCraft);
			this.tabFGMinor.Controls.Add(this.tabArrDep);
			this.tabFGMinor.Controls.Add(this.tabGoals);
			this.tabFGMinor.Controls.Add(this.tabWP);
			this.tabFGMinor.Controls.Add(this.tabOrders);
			this.tabFGMinor.Controls.Add(this.tapOption);
			this.tabFGMinor.Controls.Add(this.tabUnk);
			this.tabFGMinor.Location = new System.Drawing.Point(232, 0);
			this.tabFGMinor.Name = "tabFGMinor";
			this.tabFGMinor.SelectedIndex = 0;
			this.tabFGMinor.Size = new System.Drawing.Size(552, 504);
			this.tabFGMinor.TabIndex = 5;
			// 
			// tabCraft
			// 
			this.tabCraft.Controls.Add(this.cmdMoveFGDown);
			this.tabCraft.Controls.Add(this.cmdMoveFGUp);
			this.tabCraft.Controls.Add(this.groupBox1);
			this.tabCraft.Controls.Add(this.grpCraft3);
			this.tabCraft.Controls.Add(this.grpCraft2);
			this.tabCraft.Controls.Add(this.lblFG);
			this.tabCraft.Controls.Add(this.lblStarting);
			this.tabCraft.Controls.Add(this.grpCraft4);
			this.tabCraft.Location = new System.Drawing.Point(4, 22);
			this.tabCraft.Name = "tabCraft";
			this.tabCraft.Size = new System.Drawing.Size(544, 478);
			this.tabCraft.TabIndex = 0;
			this.tabCraft.Text = "Craft";
			// 
			// cmdMoveFGDown
			// 
			this.cmdMoveFGDown.Location = new System.Drawing.Point(437, 401);
			this.cmdMoveFGDown.Name = "cmdMoveFGDown";
			this.cmdMoveFGDown.Size = new System.Drawing.Size(75, 23);
			this.cmdMoveFGDown.TabIndex = 17;
			this.cmdMoveFGDown.Text = "Move Down";
			this.cmdMoveFGDown.UseVisualStyleBackColor = true;
			this.cmdMoveFGDown.Click += new System.EventHandler(this.cmdMoveFGDown_Click);
			// 
			// cmdMoveFGUp
			// 
			this.cmdMoveFGUp.Location = new System.Drawing.Point(437, 372);
			this.cmdMoveFGUp.Name = "cmdMoveFGUp";
			this.cmdMoveFGUp.Size = new System.Drawing.Size(75, 23);
			this.cmdMoveFGUp.TabIndex = 17;
			this.cmdMoveFGUp.Text = "Move Up";
			this.cmdMoveFGUp.UseVisualStyleBackColor = true;
			this.cmdMoveFGUp.Click += new System.EventHandler(this.cmdMoveFGUp_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkRandSC);
			this.groupBox1.Controls.Add(this.txtName);
			this.groupBox1.Controls.Add(this.lblNotUsed);
			this.groupBox1.Controls.Add(this.txtSpecCargo);
			this.groupBox1.Controls.Add(this.numSC);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.txtCargo);
			this.groupBox1.Controls.Add(this.label22);
			this.groupBox1.Controls.Add(this.label23);
			this.groupBox1.Controls.Add(this.label24);
			this.groupBox1.Location = new System.Drawing.Point(16, 24);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(232, 120);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			// 
			// chkRandSC
			// 
			this.chkRandSC.Location = new System.Drawing.Point(144, 88);
			this.chkRandSC.Name = "chkRandSC";
			this.chkRandSC.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkRandSC.Size = new System.Drawing.Size(72, 24);
			this.chkRandSC.TabIndex = 9;
			this.chkRandSC.Text = "Random";
			this.chkRandSC.CheckedChanged += new System.EventHandler(this.chkRandSC_CheckedChanged);
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(88, 16);
			this.txtName.MaxLength = 18;
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(128, 20);
			this.txtName.TabIndex = 4;
			this.txtName.Text = "New Ship";
			this.txtName.Leave += new System.EventHandler(this.txtName_Leave);
			// 
			// lblNotUsed
			// 
			this.lblNotUsed.Location = new System.Drawing.Point(88, 64);
			this.lblNotUsed.Name = "lblNotUsed";
			this.lblNotUsed.Size = new System.Drawing.Size(80, 16);
			this.lblNotUsed.TabIndex = 3;
			this.lblNotUsed.Text = "(not used)";
			// 
			// txtSpecCargo
			// 
			this.txtSpecCargo.Location = new System.Drawing.Point(88, 64);
			this.txtSpecCargo.MaxLength = 18;
			this.txtSpecCargo.Name = "txtSpecCargo";
			this.txtSpecCargo.Size = new System.Drawing.Size(128, 20);
			this.txtSpecCargo.TabIndex = 7;
			this.txtSpecCargo.Visible = false;
			this.txtSpecCargo.Leave += new System.EventHandler(this.txtSpecCargo_Leave);
			// 
			// numSC
			// 
			this.numSC.Location = new System.Drawing.Point(96, 88);
			this.numSC.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numSC.Name = "numSC";
			this.numSC.Size = new System.Drawing.Size(42, 20);
			this.numSC.TabIndex = 8;
			this.numSC.ValueChanged += new System.EventHandler(this.numSC_ValueChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 40);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(40, 16);
			this.label6.TabIndex = 3;
			this.label6.Text = "Cargo";
			// 
			// txtCargo
			// 
			this.txtCargo.Location = new System.Drawing.Point(88, 40);
			this.txtCargo.MaxLength = 18;
			this.txtCargo.Name = "txtCargo";
			this.txtCargo.Size = new System.Drawing.Size(128, 20);
			this.txtCargo.TabIndex = 6;
			this.txtCargo.Leave += new System.EventHandler(this.txtCargo_Leave);
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(8, 64);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(80, 16);
			this.label22.TabIndex = 3;
			this.label22.Text = "Special Cargo";
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(8, 16);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(40, 16);
			this.label23.TabIndex = 2;
			this.label23.Text = "Name";
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(8, 88);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(80, 16);
			this.label24.TabIndex = 3;
			this.label24.Text = "Special Ship #";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// grpCraft3
			// 
			this.grpCraft3.Controls.Add(this.chkPreventNumbering);
			this.grpCraft3.Controls.Add(this.numWaves);
			this.grpCraft3.Controls.Add(this.label10);
			this.grpCraft3.Controls.Add(this.label11);
			this.grpCraft3.Controls.Add(this.label12);
			this.grpCraft3.Controls.Add(this.numCraft);
			this.grpCraft3.Controls.Add(this.numGG);
			this.grpCraft3.Controls.Add(this.numGU);
			this.grpCraft3.Controls.Add(this.label3);
			this.grpCraft3.Location = new System.Drawing.Point(280, 24);
			this.grpCraft3.Name = "grpCraft3";
			this.grpCraft3.Size = new System.Drawing.Size(240, 100);
			this.grpCraft3.TabIndex = 15;
			this.grpCraft3.TabStop = false;
			this.grpCraft3.Leave += new System.EventHandler(this.grpCraft3_Leave);
			// 
			// chkPreventNumbering
			// 
			this.chkPreventNumbering.AutoSize = true;
			this.chkPreventNumbering.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkPreventNumbering.Location = new System.Drawing.Point(82, 74);
			this.chkPreventNumbering.Name = "chkPreventNumbering";
			this.chkPreventNumbering.Size = new System.Drawing.Size(142, 17);
			this.chkPreventNumbering.TabIndex = 23;
			this.chkPreventNumbering.Text = "Prevent Craft Numbering";
			this.chkPreventNumbering.UseVisualStyleBackColor = true;
			// 
			// numWaves
			// 
			this.numWaves.Location = new System.Drawing.Point(16, 48);
			this.numWaves.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numWaves.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWaves.Name = "numWaves";
			this.numWaves.Size = new System.Drawing.Size(40, 20);
			this.numWaves.TabIndex = 20;
			this.numWaves.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(40, 32);
			this.label10.TabIndex = 0;
			this.label10.Text = "# of waves";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(72, 16);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(40, 32);
			this.label11.TabIndex = 0;
			this.label11.Text = "# of craft";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(128, 16);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(40, 32);
			this.label12.TabIndex = 0;
			this.label12.Text = "Global Group";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// numCraft
			// 
			this.numCraft.Location = new System.Drawing.Point(72, 48);
			this.numCraft.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numCraft.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numCraft.Name = "numCraft";
			this.numCraft.Size = new System.Drawing.Size(40, 20);
			this.numCraft.TabIndex = 21;
			this.numCraft.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// numGG
			// 
			this.numGG.Location = new System.Drawing.Point(128, 48);
			this.numGG.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numGG.Name = "numGG";
			this.numGG.Size = new System.Drawing.Size(40, 20);
			this.numGG.TabIndex = 22;
			this.numGG.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numGG_KeyDown);
			this.numGG.Leave += new System.EventHandler(this.numGG_Leave);
			// 
			// numGU
			// 
			this.numGU.Location = new System.Drawing.Point(184, 48);
			this.numGU.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numGU.Name = "numGU";
			this.numGU.Size = new System.Drawing.Size(40, 20);
			this.numGU.TabIndex = 22;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(184, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 32);
			this.label3.TabIndex = 0;
			this.label3.Text = "Global Unit";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// grpCraft2
			// 
			this.grpCraft2.Controls.Add(this.cboCraft);
			this.grpCraft2.Controls.Add(this.numLead);
			this.grpCraft2.Controls.Add(this.label7);
			this.grpCraft2.Controls.Add(this.label8);
			this.grpCraft2.Controls.Add(this.label9);
			this.grpCraft2.Controls.Add(this.label13);
			this.grpCraft2.Controls.Add(this.label14);
			this.grpCraft2.Controls.Add(this.label15);
			this.grpCraft2.Controls.Add(this.label16);
			this.grpCraft2.Controls.Add(this.label17);
			this.grpCraft2.Controls.Add(this.numSpacing);
			this.grpCraft2.Controls.Add(this.cboIFF);
			this.grpCraft2.Controls.Add(this.cboAI);
			this.grpCraft2.Controls.Add(this.cboMarkings);
			this.grpCraft2.Controls.Add(this.cboPlayer);
			this.grpCraft2.Controls.Add(this.cboFormation);
			this.grpCraft2.Controls.Add(this.cmdForms);
			this.grpCraft2.Controls.Add(this.label4);
			this.grpCraft2.Controls.Add(this.cboRadio);
			this.grpCraft2.Controls.Add(this.label58);
			this.grpCraft2.Controls.Add(this.cboPosition);
			this.grpCraft2.Controls.Add(this.label111);
			this.grpCraft2.Controls.Add(this.cboTeam);
			this.grpCraft2.Location = new System.Drawing.Point(16, 160);
			this.grpCraft2.Name = "grpCraft2";
			this.grpCraft2.Size = new System.Drawing.Size(232, 296);
			this.grpCraft2.TabIndex = 14;
			this.grpCraft2.TabStop = false;
			this.grpCraft2.Leave += new System.EventHandler(this.grpCraft2_Leave);
			// 
			// cboCraft
			// 
			this.cboCraft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCraft.Location = new System.Drawing.Point(88, 16);
			this.cboCraft.Name = "cboCraft";
			this.cboCraft.Size = new System.Drawing.Size(136, 21);
			this.cboCraft.TabIndex = 10;
			this.cboCraft.SelectedIndexChanged += new System.EventHandler(this.cboCraft_SelectedIndexChanged);
			// 
			// numLead
			// 
			this.numLead.Location = new System.Drawing.Point(104, 240);
			this.numLead.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numLead.Name = "numLead";
			this.numLead.Size = new System.Drawing.Size(40, 20);
			this.numLead.TabIndex = 16;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(64, 16);
			this.label7.TabIndex = 0;
			this.label7.Text = "Craft Type";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 40);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(24, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "IFF";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 88);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(40, 16);
			this.label9.TabIndex = 0;
			this.label9.Text = "AI skill";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 136);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(40, 16);
			this.label13.TabIndex = 0;
			this.label13.Text = "Player";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(8, 112);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(56, 16);
			this.label14.TabIndex = 0;
			this.label14.Text = "Markings";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(8, 208);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(56, 16);
			this.label15.TabIndex = 0;
			this.label15.Text = "Formation";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(8, 264);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(72, 16);
			this.label16.TabIndex = 0;
			this.label16.Text = "FG spacing";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(8, 240);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(88, 16);
			this.label17.TabIndex = 0;
			this.label17.Text = "Leader spacing";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numSpacing
			// 
			this.numSpacing.Location = new System.Drawing.Point(104, 264);
			this.numSpacing.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numSpacing.Name = "numSpacing";
			this.numSpacing.Size = new System.Drawing.Size(40, 20);
			this.numSpacing.TabIndex = 17;
			this.numSpacing.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			// 
			// cboIFF
			// 
			this.cboIFF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboIFF.Location = new System.Drawing.Point(88, 40);
			this.cboIFF.Name = "cboIFF";
			this.cboIFF.Size = new System.Drawing.Size(136, 21);
			this.cboIFF.TabIndex = 11;
			// 
			// cboAI
			// 
			this.cboAI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAI.Location = new System.Drawing.Point(88, 88);
			this.cboAI.Name = "cboAI";
			this.cboAI.Size = new System.Drawing.Size(136, 21);
			this.cboAI.TabIndex = 12;
			// 
			// cboMarkings
			// 
			this.cboMarkings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMarkings.Location = new System.Drawing.Point(88, 112);
			this.cboMarkings.Name = "cboMarkings";
			this.cboMarkings.Size = new System.Drawing.Size(136, 21);
			this.cboMarkings.TabIndex = 13;
			// 
			// cboPlayer
			// 
			this.cboPlayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPlayer.Items.AddRange(new object[] {
            "AI",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
			this.cboPlayer.Location = new System.Drawing.Point(88, 136);
			this.cboPlayer.Name = "cboPlayer";
			this.cboPlayer.Size = new System.Drawing.Size(136, 21);
			this.cboPlayer.TabIndex = 14;
			// 
			// cboFormation
			// 
			this.cboFormation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFormation.Location = new System.Drawing.Point(88, 208);
			this.cboFormation.Name = "cboFormation";
			this.cboFormation.Size = new System.Drawing.Size(136, 21);
			this.cboFormation.TabIndex = 15;
			this.cboFormation.SelectedIndexChanged += new System.EventHandler(this.cboFormation_SelectedIndexChanged);
			// 
			// cmdForms
			// 
			this.cmdForms.Location = new System.Drawing.Point(160, 248);
			this.cmdForms.Name = "cmdForms";
			this.cmdForms.Size = new System.Drawing.Size(64, 24);
			this.cmdForms.TabIndex = 19;
			this.cmdForms.Text = "&Forms...";
			this.cmdForms.Click += new System.EventHandler(this.cmdForms_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 184);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 16);
			this.label4.TabIndex = 0;
			this.label4.Text = "Radio";
			// 
			// cboRadio
			// 
			this.cboRadio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRadio.Location = new System.Drawing.Point(88, 184);
			this.cboRadio.Name = "cboRadio";
			this.cboRadio.Size = new System.Drawing.Size(136, 21);
			this.cboRadio.TabIndex = 15;
			// 
			// label58
			// 
			this.label58.Location = new System.Drawing.Point(8, 160);
			this.label58.Name = "label58";
			this.label58.Size = new System.Drawing.Size(80, 16);
			this.label58.TabIndex = 0;
			this.label58.Text = "Player Position";
			// 
			// cboPosition
			// 
			this.cboPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPosition.Items.AddRange(new object[] {
            "(Default)",
            "2",
            "3",
            "4",
            "5",
            "6"});
			this.cboPosition.Location = new System.Drawing.Point(88, 160);
			this.cboPosition.Name = "cboPosition";
			this.cboPosition.Size = new System.Drawing.Size(136, 21);
			this.cboPosition.TabIndex = 15;
			// 
			// label111
			// 
			this.label111.Location = new System.Drawing.Point(8, 64);
			this.label111.Name = "label111";
			this.label111.Size = new System.Drawing.Size(40, 16);
			this.label111.TabIndex = 0;
			this.label111.Text = "Team";
			// 
			// cboTeam
			// 
			this.cboTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTeam.Location = new System.Drawing.Point(88, 64);
			this.cboTeam.Name = "cboTeam";
			this.cboTeam.Size = new System.Drawing.Size(136, 21);
			this.cboTeam.TabIndex = 11;
			// 
			// lblFG
			// 
			this.lblFG.Location = new System.Drawing.Point(288, 340);
			this.lblFG.Name = "lblFG";
			this.lblFG.Size = new System.Drawing.Size(120, 16);
			this.lblFG.TabIndex = 13;
			this.lblFG.Text = "Flight Group #0 of 0";
			// 
			// lblStarting
			// 
			this.lblStarting.Location = new System.Drawing.Point(288, 372);
			this.lblStarting.Name = "lblStarting";
			this.lblStarting.Size = new System.Drawing.Size(120, 16);
			this.lblStarting.TabIndex = 12;
			this.lblStarting.Text = "1 Craft at 30 seconds";
			// 
			// grpCraft4
			// 
			this.grpCraft4.Controls.Add(this.cmdBackdrop);
			this.grpCraft4.Controls.Add(this.numBackdrop);
			this.grpCraft4.Controls.Add(this.label122);
			this.grpCraft4.Controls.Add(this.lblExplode);
			this.grpCraft4.Controls.Add(this.numExplode);
			this.grpCraft4.Controls.Add(this.label67);
			this.grpCraft4.Controls.Add(this.label18);
			this.grpCraft4.Controls.Add(this.cboWarheads);
			this.grpCraft4.Controls.Add(this.lblStatus);
			this.grpCraft4.Controls.Add(this.cboBeam);
			this.grpCraft4.Controls.Add(this.cboStatus);
			this.grpCraft4.Controls.Add(this.label20);
			this.grpCraft4.Controls.Add(this.cboStatus2);
			this.grpCraft4.Controls.Add(this.label2);
			this.grpCraft4.Controls.Add(this.cboCounter);
			this.grpCraft4.Controls.Add(this.label5);
			this.grpCraft4.Location = new System.Drawing.Point(280, 132);
			this.grpCraft4.Name = "grpCraft4";
			this.grpCraft4.Size = new System.Drawing.Size(240, 188);
			this.grpCraft4.TabIndex = 11;
			this.grpCraft4.TabStop = false;
			this.grpCraft4.Leave += new System.EventHandler(this.grpCraft4_Leave);
			// 
			// cmdBackdrop
			// 
			this.cmdBackdrop.Location = new System.Drawing.Point(155, 159);
			this.cmdBackdrop.Name = "cmdBackdrop";
			this.cmdBackdrop.Size = new System.Drawing.Size(68, 20);
			this.cmdBackdrop.TabIndex = 36;
			this.cmdBackdrop.Text = "&Backdrops";
			this.cmdBackdrop.UseVisualStyleBackColor = true;
			this.cmdBackdrop.Click += new System.EventHandler(this.cmdBackdrop_Click);
			// 
			// numBackdrop
			// 
			this.numBackdrop.Enabled = false;
			this.numBackdrop.Location = new System.Drawing.Point(96, 159);
			this.numBackdrop.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numBackdrop.Name = "numBackdrop";
			this.numBackdrop.Size = new System.Drawing.Size(48, 20);
			this.numBackdrop.TabIndex = 35;
			this.numBackdrop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numBackdrop.Leave += new System.EventHandler(this.numBackdrop_Leave);
			// 
			// label122
			// 
			this.label122.AutoSize = true;
			this.label122.Location = new System.Drawing.Point(8, 163);
			this.label122.Name = "label122";
			this.label122.Size = new System.Drawing.Size(53, 13);
			this.label122.TabIndex = 34;
			this.label122.Text = "Backdrop";
			// 
			// lblExplode
			// 
			this.lblExplode.Location = new System.Drawing.Point(152, 136);
			this.lblExplode.Name = "lblExplode";
			this.lblExplode.Size = new System.Drawing.Size(48, 16);
			this.lblExplode.TabIndex = 28;
			this.lblExplode.Text = "default";
			this.lblExplode.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// numExplode
			// 
			this.numExplode.Location = new System.Drawing.Point(96, 136);
			this.numExplode.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numExplode.Name = "numExplode";
			this.numExplode.Size = new System.Drawing.Size(48, 20);
			this.numExplode.TabIndex = 27;
			this.numExplode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numExplode.ValueChanged += new System.EventHandler(this.numExplode_ValueChanged);
			// 
			// label67
			// 
			this.label67.Location = new System.Drawing.Point(8, 136);
			this.label67.Name = "label67";
			this.label67.Size = new System.Drawing.Size(80, 16);
			this.label67.TabIndex = 26;
			this.label67.Text = "Explosion time";
			this.label67.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(8, 88);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(40, 16);
			this.label18.TabIndex = 0;
			this.label18.Text = "Beam";
			// 
			// cboWarheads
			// 
			this.cboWarheads.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboWarheads.Location = new System.Drawing.Point(96, 64);
			this.cboWarheads.Name = "cboWarheads";
			this.cboWarheads.Size = new System.Drawing.Size(136, 21);
			this.cboWarheads.TabIndex = 24;
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(8, 16);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(37, 13);
			this.lblStatus.TabIndex = 0;
			this.lblStatus.Text = "Status";
			// 
			// cboBeam
			// 
			this.cboBeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboBeam.Location = new System.Drawing.Point(96, 88);
			this.cboBeam.Name = "cboBeam";
			this.cboBeam.Size = new System.Drawing.Size(136, 21);
			this.cboBeam.TabIndex = 25;
			// 
			// cboStatus
			// 
			this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboStatus.Location = new System.Drawing.Point(96, 16);
			this.cboStatus.Name = "cboStatus";
			this.cboStatus.Size = new System.Drawing.Size(136, 21);
			this.cboStatus.TabIndex = 23;
			this.cboStatus.Leave += new System.EventHandler(this.cboStatus_Leave);
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(8, 64);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(56, 16);
			this.label20.TabIndex = 0;
			this.label20.Text = "Warheads";
			// 
			// cboStatus2
			// 
			this.cboStatus2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboStatus2.Location = new System.Drawing.Point(96, 40);
			this.cboStatus2.Name = "cboStatus2";
			this.cboStatus2.Size = new System.Drawing.Size(136, 21);
			this.cboStatus2.TabIndex = 23;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Status 2";
			// 
			// cboCounter
			// 
			this.cboCounter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCounter.Items.AddRange(new object[] {
            "None",
            "Chaff",
            "Flare",
            "(Cluster Mine)"});
			this.cboCounter.Location = new System.Drawing.Point(96, 112);
			this.cboCounter.Name = "cboCounter";
			this.cboCounter.Size = new System.Drawing.Size(136, 21);
			this.cboCounter.TabIndex = 25;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 112);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 16);
			this.label5.TabIndex = 0;
			this.label5.Text = "Countermeasures";
			// 
			// tabArrDep
			// 
			this.tabArrDep.Controls.Add(this.chkArrHuman);
			this.tabArrDep.Controls.Add(this.cmdCopyAD);
			this.tabArrDep.Controls.Add(this.label36);
			this.tabArrDep.Controls.Add(this.grpDep);
			this.tabArrDep.Controls.Add(this.groupBox8);
			this.tabArrDep.Controls.Add(this.cboADTrigAmount);
			this.tabArrDep.Controls.Add(this.cboADTrigType);
			this.tabArrDep.Controls.Add(this.cboADTrigVar);
			this.tabArrDep.Controls.Add(this.cboADTrig);
			this.tabArrDep.Controls.Add(this.label44);
			this.tabArrDep.Controls.Add(this.cboDiff);
			this.tabArrDep.Controls.Add(this.label45);
			this.tabArrDep.Controls.Add(this.label46);
			this.tabArrDep.Controls.Add(this.cmdPasteAD);
			this.tabArrDep.Location = new System.Drawing.Point(4, 22);
			this.tabArrDep.Name = "tabArrDep";
			this.tabArrDep.Size = new System.Drawing.Size(544, 478);
			this.tabArrDep.TabIndex = 1;
			this.tabArrDep.Text = "Arr/Dep";
			// 
			// chkArrHuman
			// 
			this.chkArrHuman.Location = new System.Drawing.Point(368, 448);
			this.chkArrHuman.Name = "chkArrHuman";
			this.chkArrHuman.Size = new System.Drawing.Size(136, 24);
			this.chkArrHuman.TabIndex = 39;
			this.chkArrHuman.Text = "Only if Human player";
			this.chkArrHuman.Leave += new System.EventHandler(this.chkArrHuman_Leave);
			// 
			// cmdCopyAD
			// 
			this.cmdCopyAD.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdCopyAD.ImageIndex = 6;
			this.cmdCopyAD.ImageList = this.imgToolbar;
			this.cmdCopyAD.Location = new System.Drawing.Point(72, 392);
			this.cmdCopyAD.Name = "cmdCopyAD";
			this.cmdCopyAD.Size = new System.Drawing.Size(24, 23);
			this.cmdCopyAD.TabIndex = 37;
			this.cmdCopyAD.Click += new System.EventHandler(this.cmdCopyAD_Click);
			// 
			// imgToolbar
			// 
			this.imgToolbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgToolbar.ImageStream")));
			this.imgToolbar.TransparentColor = System.Drawing.Color.Transparent;
			this.imgToolbar.Images.SetKeyName(0, "");
			this.imgToolbar.Images.SetKeyName(1, "");
			this.imgToolbar.Images.SetKeyName(2, "");
			this.imgToolbar.Images.SetKeyName(3, "");
			this.imgToolbar.Images.SetKeyName(4, "");
			this.imgToolbar.Images.SetKeyName(5, "");
			this.imgToolbar.Images.SetKeyName(6, "");
			this.imgToolbar.Images.SetKeyName(7, "");
			this.imgToolbar.Images.SetKeyName(8, "");
			this.imgToolbar.Images.SetKeyName(9, "");
			this.imgToolbar.Images.SetKeyName(10, "");
			this.imgToolbar.Images.SetKeyName(11, "");
			this.imgToolbar.Images.SetKeyName(12, "");
			this.imgToolbar.Images.SetKeyName(13, "");
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(264, 392);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(16, 16);
			this.label36.TabIndex = 29;
			this.label36.Text = "of";
			this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// grpDep
			// 
			this.grpDep.Controls.Add(this.label25);
			this.grpDep.Controls.Add(this.label21);
			this.grpDep.Controls.Add(this.numDepClockSec);
			this.grpDep.Controls.Add(this.numDepClockMin);
			this.grpDep.Controls.Add(this.numDepMin);
			this.grpDep.Controls.Add(this.numDepSec);
			this.grpDep.Controls.Add(this.optDepAND);
			this.grpDep.Controls.Add(this.optDepOR);
			this.grpDep.Controls.Add(this.label47);
			this.grpDep.Controls.Add(this.label41);
			this.grpDep.Controls.Add(this.label40);
			this.grpDep.Controls.Add(this.label39);
			this.grpDep.Controls.Add(this.label37);
			this.grpDep.Controls.Add(this.groupBox10);
			this.grpDep.Controls.Add(this.groupBox9);
			this.grpDep.Controls.Add(this.lblDep1);
			this.grpDep.Controls.Add(this.cboAbort);
			this.grpDep.Controls.Add(this.lblDep2);
			this.grpDep.Location = new System.Drawing.Point(280, 5);
			this.grpDep.Name = "grpDep";
			this.grpDep.Size = new System.Drawing.Size(256, 371);
			this.grpDep.TabIndex = 27;
			this.grpDep.TabStop = false;
			this.grpDep.Text = "Departure";
			this.grpDep.Leave += new System.EventHandler(this.grpDep_Leave);
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(240, 347);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(12, 16);
			this.label25.TabIndex = 29;
			this.label25.Text = "S";
			this.label25.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(169, 347);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(12, 16);
			this.label21.TabIndex = 28;
			this.label21.Text = "M";
			this.label21.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// numDepClockSec
			// 
			this.numDepClockSec.Location = new System.Drawing.Point(192, 344);
			this.numDepClockSec.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numDepClockSec.Name = "numDepClockSec";
			this.numDepClockSec.Size = new System.Drawing.Size(48, 20);
			this.numDepClockSec.TabIndex = 27;
			// 
			// numDepClockMin
			// 
			this.numDepClockMin.Location = new System.Drawing.Point(121, 344);
			this.numDepClockMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numDepClockMin.Name = "numDepClockMin";
			this.numDepClockMin.Size = new System.Drawing.Size(48, 20);
			this.numDepClockMin.TabIndex = 26;
			// 
			// numDepMin
			// 
			this.numDepMin.Location = new System.Drawing.Point(98, 320);
			this.numDepMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numDepMin.Name = "numDepMin";
			this.numDepMin.Size = new System.Drawing.Size(48, 20);
			this.numDepMin.TabIndex = 25;
			// 
			// numDepSec
			// 
			this.numDepSec.Location = new System.Drawing.Point(176, 320);
			this.numDepSec.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numDepSec.Name = "numDepSec";
			this.numDepSec.Size = new System.Drawing.Size(48, 20);
			this.numDepSec.TabIndex = 25;
			// 
			// optDepAND
			// 
			this.optDepAND.Location = new System.Drawing.Point(88, 208);
			this.optDepAND.Name = "optDepAND";
			this.optDepAND.Size = new System.Drawing.Size(56, 24);
			this.optDepAND.TabIndex = 19;
			this.optDepAND.Text = "AND";
			// 
			// optDepOR
			// 
			this.optDepOR.Checked = true;
			this.optDepOR.Location = new System.Drawing.Point(144, 208);
			this.optDepOR.Name = "optDepOR";
			this.optDepOR.Size = new System.Drawing.Size(56, 24);
			this.optDepOR.TabIndex = 20;
			this.optDepOR.TabStop = true;
			this.optDepOR.Text = "OR";
			// 
			// label47
			// 
			this.label47.Location = new System.Drawing.Point(5, 349);
			this.label47.Name = "label47";
			this.label47.Size = new System.Drawing.Size(120, 16);
			this.label47.TabIndex = 8;
			this.label47.Text = "Abort on mission clock:";
			// 
			// label41
			// 
			this.label41.Location = new System.Drawing.Point(4, 320);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(100, 16);
			this.label41.TabIndex = 7;
			this.label41.Text = "Delay after trigger:";
			this.label41.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label40
			// 
			this.label40.Location = new System.Drawing.Point(145, 320);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(24, 16);
			this.label40.TabIndex = 6;
			this.label40.Text = "Min";
			this.label40.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label39
			// 
			this.label39.Location = new System.Drawing.Point(222, 320);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(32, 16);
			this.label39.TabIndex = 5;
			this.label39.Text = "Sec";
			this.label39.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(16, 272);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(184, 16);
			this.label37.TabIndex = 3;
			this.label37.Text = "Individual craft abort mission when:";
			// 
			// groupBox10
			// 
			this.groupBox10.Controls.Add(this.optDepMSAlt);
			this.groupBox10.Controls.Add(this.optDepHypAlt);
			this.groupBox10.Controls.Add(this.cboDepMSAlt);
			this.groupBox10.Location = new System.Drawing.Point(8, 96);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Size = new System.Drawing.Size(240, 72);
			this.groupBox10.TabIndex = 1;
			this.groupBox10.TabStop = false;
			this.groupBox10.Text = "Alternative:";
			// 
			// optDepMSAlt
			// 
			this.optDepMSAlt.Location = new System.Drawing.Point(16, 40);
			this.optDepMSAlt.Name = "optDepMSAlt";
			this.optDepMSAlt.Size = new System.Drawing.Size(80, 24);
			this.optDepMSAlt.TabIndex = 10;
			this.optDepMSAlt.Text = "Mothership";
			this.optDepMSAlt.CheckedChanged += new System.EventHandler(this.optDepMSAlt_CheckedChanged);
			// 
			// optDepHypAlt
			// 
			this.optDepHypAlt.Checked = true;
			this.optDepHypAlt.Location = new System.Drawing.Point(16, 16);
			this.optDepHypAlt.Name = "optDepHypAlt";
			this.optDepHypAlt.Size = new System.Drawing.Size(104, 24);
			this.optDepHypAlt.TabIndex = 9;
			this.optDepHypAlt.TabStop = true;
			this.optDepHypAlt.Text = "Hyperspace";
			// 
			// cboDepMSAlt
			// 
			this.cboDepMSAlt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDepMSAlt.Enabled = false;
			this.cboDepMSAlt.Location = new System.Drawing.Point(96, 40);
			this.cboDepMSAlt.Name = "cboDepMSAlt";
			this.cboDepMSAlt.Size = new System.Drawing.Size(136, 21);
			this.cboDepMSAlt.TabIndex = 11;
			// 
			// groupBox9
			// 
			this.groupBox9.Controls.Add(this.optDepHyp);
			this.groupBox9.Controls.Add(this.cboDepMS);
			this.groupBox9.Controls.Add(this.optDepMS);
			this.groupBox9.Location = new System.Drawing.Point(8, 16);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(240, 72);
			this.groupBox9.TabIndex = 0;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Via:";
			// 
			// optDepHyp
			// 
			this.optDepHyp.Checked = true;
			this.optDepHyp.Location = new System.Drawing.Point(16, 16);
			this.optDepHyp.Name = "optDepHyp";
			this.optDepHyp.Size = new System.Drawing.Size(104, 24);
			this.optDepHyp.TabIndex = 6;
			this.optDepHyp.TabStop = true;
			this.optDepHyp.Text = "Hyperspace";
			// 
			// cboDepMS
			// 
			this.cboDepMS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDepMS.Enabled = false;
			this.cboDepMS.Location = new System.Drawing.Point(96, 40);
			this.cboDepMS.Name = "cboDepMS";
			this.cboDepMS.Size = new System.Drawing.Size(136, 21);
			this.cboDepMS.TabIndex = 8;
			// 
			// optDepMS
			// 
			this.optDepMS.Location = new System.Drawing.Point(16, 40);
			this.optDepMS.Name = "optDepMS";
			this.optDepMS.Size = new System.Drawing.Size(80, 24);
			this.optDepMS.TabIndex = 7;
			this.optDepMS.Text = "Mothership";
			this.optDepMS.CheckedChanged += new System.EventHandler(this.optDepMS_CheckedChanged);
			// 
			// lblDep1
			// 
			this.lblDep1.BackColor = System.Drawing.Color.RosyBrown;
			this.lblDep1.Location = new System.Drawing.Point(8, 176);
			this.lblDep1.Name = "lblDep1";
			this.lblDep1.Size = new System.Drawing.Size(240, 32);
			this.lblDep1.TabIndex = 2;
			this.lblDep1.Text = "always (TRUE)";
			this.lblDep1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cboAbort
			// 
			this.cboAbort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAbort.Location = new System.Drawing.Point(88, 288);
			this.cboAbort.Name = "cboAbort";
			this.cboAbort.Size = new System.Drawing.Size(144, 21);
			this.cboAbort.TabIndex = 16;
			// 
			// lblDep2
			// 
			this.lblDep2.BackColor = System.Drawing.Color.RosyBrown;
			this.lblDep2.Location = new System.Drawing.Point(8, 232);
			this.lblDep2.Name = "lblDep2";
			this.lblDep2.Size = new System.Drawing.Size(240, 32);
			this.lblDep2.TabIndex = 2;
			this.lblDep2.Text = "always (TRUE)";
			this.lblDep2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.numArrSec);
			this.groupBox8.Controls.Add(this.numArrMin);
			this.groupBox8.Controls.Add(this.panel10);
			this.groupBox8.Controls.Add(this.panel9);
			this.groupBox8.Controls.Add(this.optArr12AND34);
			this.groupBox8.Controls.Add(this.optArr12OR34);
			this.groupBox8.Controls.Add(this.label38);
			this.groupBox8.Controls.Add(this.lblArr1);
			this.groupBox8.Controls.Add(this.groupBox11);
			this.groupBox8.Controls.Add(this.groupBox12);
			this.groupBox8.Controls.Add(this.lblArr2);
			this.groupBox8.Controls.Add(this.label42);
			this.groupBox8.Controls.Add(this.label43);
			this.groupBox8.Controls.Add(this.lblArr3);
			this.groupBox8.Controls.Add(this.lblArr4);
			this.groupBox8.Location = new System.Drawing.Point(8, 5);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(266, 371);
			this.groupBox8.TabIndex = 26;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Arrival";
			// 
			// numArrSec
			// 
			this.numArrSec.Location = new System.Drawing.Point(161, 344);
			this.numArrSec.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numArrSec.Name = "numArrSec";
			this.numArrSec.Size = new System.Drawing.Size(48, 20);
			this.numArrSec.TabIndex = 25;
			this.numArrSec.Leave += new System.EventHandler(this.numArrSec_Leave);
			// 
			// numArrMin
			// 
			this.numArrMin.Location = new System.Drawing.Point(72, 344);
			this.numArrMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numArrMin.Name = "numArrMin";
			this.numArrMin.Size = new System.Drawing.Size(48, 20);
			this.numArrMin.TabIndex = 25;
			this.numArrMin.Leave += new System.EventHandler(this.numArrMin_Leave);
			// 
			// panel10
			// 
			this.panel10.Controls.Add(this.optArr3AND4);
			this.panel10.Controls.Add(this.optArr3OR4);
			this.panel10.Location = new System.Drawing.Point(200, 264);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(60, 64);
			this.panel10.TabIndex = 24;
			// 
			// optArr3AND4
			// 
			this.optArr3AND4.Location = new System.Drawing.Point(8, 8);
			this.optArr3AND4.Name = "optArr3AND4";
			this.optArr3AND4.Size = new System.Drawing.Size(48, 24);
			this.optArr3AND4.TabIndex = 12;
			this.optArr3AND4.Text = "AND";
			// 
			// optArr3OR4
			// 
			this.optArr3OR4.Checked = true;
			this.optArr3OR4.Location = new System.Drawing.Point(8, 32);
			this.optArr3OR4.Name = "optArr3OR4";
			this.optArr3OR4.Size = new System.Drawing.Size(48, 24);
			this.optArr3OR4.TabIndex = 13;
			this.optArr3OR4.TabStop = true;
			this.optArr3OR4.Text = "OR";
			// 
			// panel9
			// 
			this.panel9.Controls.Add(this.optArr1AND2);
			this.panel9.Controls.Add(this.optArr1OR2);
			this.panel9.Location = new System.Drawing.Point(200, 176);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(60, 64);
			this.panel9.TabIndex = 23;
			// 
			// optArr1AND2
			// 
			this.optArr1AND2.Location = new System.Drawing.Point(8, 8);
			this.optArr1AND2.Name = "optArr1AND2";
			this.optArr1AND2.Size = new System.Drawing.Size(48, 24);
			this.optArr1AND2.TabIndex = 12;
			this.optArr1AND2.Text = "AND";
			// 
			// optArr1OR2
			// 
			this.optArr1OR2.Checked = true;
			this.optArr1OR2.Location = new System.Drawing.Point(8, 32);
			this.optArr1OR2.Name = "optArr1OR2";
			this.optArr1OR2.Size = new System.Drawing.Size(48, 24);
			this.optArr1OR2.TabIndex = 13;
			this.optArr1OR2.TabStop = true;
			this.optArr1OR2.Text = "OR";
			// 
			// optArr12AND34
			// 
			this.optArr12AND34.Location = new System.Drawing.Point(64, 240);
			this.optArr12AND34.Name = "optArr12AND34";
			this.optArr12AND34.Size = new System.Drawing.Size(56, 24);
			this.optArr12AND34.TabIndex = 21;
			this.optArr12AND34.Text = "AND";
			// 
			// optArr12OR34
			// 
			this.optArr12OR34.Checked = true;
			this.optArr12OR34.Location = new System.Drawing.Point(120, 240);
			this.optArr12OR34.Name = "optArr12OR34";
			this.optArr12OR34.Size = new System.Drawing.Size(56, 24);
			this.optArr12OR34.TabIndex = 22;
			this.optArr12OR34.TabStop = true;
			this.optArr12OR34.Text = "OR";
			// 
			// label38
			// 
			this.label38.Location = new System.Drawing.Point(16, 344);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(40, 16);
			this.label38.TabIndex = 4;
			this.label38.Text = "Delay:";
			this.label38.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// lblArr1
			// 
			this.lblArr1.BackColor = System.Drawing.Color.RosyBrown;
			this.lblArr1.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lblArr1.Location = new System.Drawing.Point(8, 176);
			this.lblArr1.Name = "lblArr1";
			this.lblArr1.Size = new System.Drawing.Size(192, 32);
			this.lblArr1.TabIndex = 2;
			this.lblArr1.Text = "always (TRUE)";
			this.lblArr1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox11
			// 
			this.groupBox11.Controls.Add(this.optArrHypAlt);
			this.groupBox11.Controls.Add(this.cboArrMSAlt);
			this.groupBox11.Controls.Add(this.optArrMSAlt);
			this.groupBox11.Location = new System.Drawing.Point(8, 96);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(240, 72);
			this.groupBox11.TabIndex = 1;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "Alternative:";
			// 
			// optArrHypAlt
			// 
			this.optArrHypAlt.Checked = true;
			this.optArrHypAlt.Location = new System.Drawing.Point(16, 16);
			this.optArrHypAlt.Name = "optArrHypAlt";
			this.optArrHypAlt.Size = new System.Drawing.Size(104, 24);
			this.optArrHypAlt.TabIndex = 3;
			this.optArrHypAlt.TabStop = true;
			this.optArrHypAlt.Text = "Hyperspace";
			// 
			// cboArrMSAlt
			// 
			this.cboArrMSAlt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboArrMSAlt.Enabled = false;
			this.cboArrMSAlt.Location = new System.Drawing.Point(96, 40);
			this.cboArrMSAlt.Name = "cboArrMSAlt";
			this.cboArrMSAlt.Size = new System.Drawing.Size(136, 21);
			this.cboArrMSAlt.TabIndex = 5;
			this.cboArrMSAlt.Leave += new System.EventHandler(this.cboArrMSAlt_Leave);
			// 
			// optArrMSAlt
			// 
			this.optArrMSAlt.Location = new System.Drawing.Point(16, 40);
			this.optArrMSAlt.Name = "optArrMSAlt";
			this.optArrMSAlt.Size = new System.Drawing.Size(80, 24);
			this.optArrMSAlt.TabIndex = 4;
			this.optArrMSAlt.Text = "Mothership";
			this.optArrMSAlt.CheckedChanged += new System.EventHandler(this.optArrMSAlt_CheckedChanged);
			// 
			// groupBox12
			// 
			this.groupBox12.Controls.Add(this.cboArrMS);
			this.groupBox12.Controls.Add(this.optArrHyp);
			this.groupBox12.Controls.Add(this.optArrMS);
			this.groupBox12.Location = new System.Drawing.Point(8, 16);
			this.groupBox12.Name = "groupBox12";
			this.groupBox12.Size = new System.Drawing.Size(240, 72);
			this.groupBox12.TabIndex = 0;
			this.groupBox12.TabStop = false;
			this.groupBox12.Text = "Via:";
			// 
			// cboArrMS
			// 
			this.cboArrMS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboArrMS.Enabled = false;
			this.cboArrMS.Location = new System.Drawing.Point(96, 40);
			this.cboArrMS.Name = "cboArrMS";
			this.cboArrMS.Size = new System.Drawing.Size(136, 21);
			this.cboArrMS.TabIndex = 2;
			this.cboArrMS.Leave += new System.EventHandler(this.cboArrMS_Leave);
			// 
			// optArrHyp
			// 
			this.optArrHyp.Checked = true;
			this.optArrHyp.Location = new System.Drawing.Point(16, 16);
			this.optArrHyp.Name = "optArrHyp";
			this.optArrHyp.Size = new System.Drawing.Size(104, 24);
			this.optArrHyp.TabIndex = 0;
			this.optArrHyp.TabStop = true;
			this.optArrHyp.Text = "Hyperspace";
			// 
			// optArrMS
			// 
			this.optArrMS.Location = new System.Drawing.Point(16, 40);
			this.optArrMS.Name = "optArrMS";
			this.optArrMS.Size = new System.Drawing.Size(80, 24);
			this.optArrMS.TabIndex = 1;
			this.optArrMS.Text = "Mothership";
			this.optArrMS.CheckedChanged += new System.EventHandler(this.optArrMS_CheckedChanged);
			// 
			// lblArr2
			// 
			this.lblArr2.BackColor = System.Drawing.Color.RosyBrown;
			this.lblArr2.Location = new System.Drawing.Point(8, 208);
			this.lblArr2.Name = "lblArr2";
			this.lblArr2.Size = new System.Drawing.Size(192, 32);
			this.lblArr2.TabIndex = 2;
			this.lblArr2.Text = "always (TRUE)";
			this.lblArr2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label42
			// 
			this.label42.Location = new System.Drawing.Point(120, 344);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(24, 16);
			this.label42.TabIndex = 4;
			this.label42.Text = "Min";
			this.label42.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label43
			// 
			this.label43.Location = new System.Drawing.Point(202, 344);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(42, 16);
			this.label43.TabIndex = 4;
			this.label43.Text = "Sec";
			this.label43.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// lblArr3
			// 
			this.lblArr3.BackColor = System.Drawing.Color.RosyBrown;
			this.lblArr3.Location = new System.Drawing.Point(8, 264);
			this.lblArr3.Name = "lblArr3";
			this.lblArr3.Size = new System.Drawing.Size(192, 32);
			this.lblArr3.TabIndex = 2;
			this.lblArr3.Text = "always (TRUE)";
			this.lblArr3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblArr4
			// 
			this.lblArr4.BackColor = System.Drawing.Color.RosyBrown;
			this.lblArr4.Location = new System.Drawing.Point(8, 296);
			this.lblArr4.Name = "lblArr4";
			this.lblArr4.Size = new System.Drawing.Size(192, 32);
			this.lblArr4.TabIndex = 2;
			this.lblArr4.Text = "always (TRUE)";
			this.lblArr4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cboADTrigAmount
			// 
			this.cboADTrigAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboADTrigAmount.Location = new System.Drawing.Point(112, 392);
			this.cboADTrigAmount.Name = "cboADTrigAmount";
			this.cboADTrigAmount.Size = new System.Drawing.Size(144, 21);
			this.cboADTrigAmount.TabIndex = 32;
			this.cboADTrigAmount.Leave += new System.EventHandler(this.cboADTrigAmount_Leave);
			// 
			// cboADTrigType
			// 
			this.cboADTrigType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboADTrigType.Location = new System.Drawing.Point(288, 392);
			this.cboADTrigType.Name = "cboADTrigType";
			this.cboADTrigType.Size = new System.Drawing.Size(160, 21);
			this.cboADTrigType.TabIndex = 33;
			this.cboADTrigType.SelectedIndexChanged += new System.EventHandler(this.cboADTrigType_SelectedIndexChanged);
			// 
			// cboADTrigVar
			// 
			this.cboADTrigVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboADTrigVar.Location = new System.Drawing.Point(112, 416);
			this.cboADTrigVar.Name = "cboADTrigVar";
			this.cboADTrigVar.Size = new System.Drawing.Size(144, 21);
			this.cboADTrigVar.TabIndex = 34;
			this.cboADTrigVar.Leave += new System.EventHandler(this.cboADTrigVar_Leave);
			// 
			// cboADTrig
			// 
			this.cboADTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboADTrig.Location = new System.Drawing.Point(288, 416);
			this.cboADTrig.Name = "cboADTrig";
			this.cboADTrig.Size = new System.Drawing.Size(160, 21);
			this.cboADTrig.TabIndex = 35;
			this.cboADTrig.Leave += new System.EventHandler(this.cboADTrig_Leave);
			// 
			// label44
			// 
			this.label44.Location = new System.Drawing.Point(256, 416);
			this.label44.Name = "label44";
			this.label44.Size = new System.Drawing.Size(32, 16);
			this.label44.TabIndex = 28;
			this.label44.Text = "must";
			this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cboDiff
			// 
			this.cboDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDiff.Location = new System.Drawing.Point(192, 448);
			this.cboDiff.Name = "cboDiff";
			this.cboDiff.Size = new System.Drawing.Size(112, 21);
			this.cboDiff.TabIndex = 36;
			this.cboDiff.Leave += new System.EventHandler(this.cboDiff_Leave);
			// 
			// label45
			// 
			this.label45.Location = new System.Drawing.Point(304, 448);
			this.label45.Name = "label45";
			this.label45.Size = new System.Drawing.Size(48, 16);
			this.label45.TabIndex = 30;
			this.label45.Text = "difficulty";
			this.label45.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label46
			// 
			this.label46.Location = new System.Drawing.Point(104, 448);
			this.label46.Name = "label46";
			this.label46.Size = new System.Drawing.Size(88, 16);
			this.label46.TabIndex = 31;
			this.label46.Text = "Craft appears in";
			this.label46.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// cmdPasteAD
			// 
			this.cmdPasteAD.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdPasteAD.ImageIndex = 7;
			this.cmdPasteAD.ImageList = this.imgToolbar;
			this.cmdPasteAD.Location = new System.Drawing.Point(72, 416);
			this.cmdPasteAD.Name = "cmdPasteAD";
			this.cmdPasteAD.Size = new System.Drawing.Size(24, 23);
			this.cmdPasteAD.TabIndex = 38;
			this.cmdPasteAD.Click += new System.EventHandler(this.cmdPasteAD_Click);
			// 
			// tabGoals
			// 
			this.tabGoals.Controls.Add(this.lblGoalTimeLimit);
			this.tabGoals.Controls.Add(this.label19);
			this.tabGoals.Controls.Add(this.grpGoal);
			this.tabGoals.Controls.Add(this.label66);
			this.tabGoals.Controls.Add(this.chkGoalEnable);
			this.tabGoals.Controls.Add(this.numGoalPoints);
			this.tabGoals.Controls.Add(this.label65);
			this.tabGoals.Controls.Add(this.label62);
			this.tabGoals.Controls.Add(this.txtGoalInc);
			this.tabGoals.Controls.Add(this.label60);
			this.tabGoals.Controls.Add(this.groupBox16);
			this.tabGoals.Controls.Add(this.txtGoalComp);
			this.tabGoals.Controls.Add(this.txtGoalFail);
			this.tabGoals.Controls.Add(this.label63);
			this.tabGoals.Controls.Add(this.label64);
			this.tabGoals.Controls.Add(this.numGoalTimeLimit);
			this.tabGoals.Controls.Add(this.numGoalTeam);
			this.tabGoals.Location = new System.Drawing.Point(4, 22);
			this.tabGoals.Name = "tabGoals";
			this.tabGoals.Size = new System.Drawing.Size(544, 478);
			this.tabGoals.TabIndex = 2;
			this.tabGoals.Text = "Goals";
			// 
			// lblGoalTimeLimit
			// 
			this.lblGoalTimeLimit.AutoSize = true;
			this.lblGoalTimeLimit.Location = new System.Drawing.Point(462, 340);
			this.lblGoalTimeLimit.Name = "lblGoalTimeLimit";
			this.lblGoalTimeLimit.Size = new System.Drawing.Size(63, 13);
			this.lblGoalTimeLimit.TabIndex = 50;
			this.lblGoalTimeLimit.Text = "No time limit";
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(337, 340);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(57, 13);
			this.label19.TabIndex = 49;
			this.label19.Text = "Time Limit:";
			// 
			// grpGoal
			// 
			this.grpGoal.Controls.Add(this.label61);
			this.grpGoal.Controls.Add(this.cboGoalAmount);
			this.grpGoal.Controls.Add(this.cboGoalArgument);
			this.grpGoal.Controls.Add(this.cboGoalTrigger);
			this.grpGoal.Location = new System.Drawing.Point(16, 256);
			this.grpGoal.Name = "grpGoal";
			this.grpGoal.Size = new System.Drawing.Size(288, 80);
			this.grpGoal.TabIndex = 48;
			this.grpGoal.TabStop = false;
			this.grpGoal.Leave += new System.EventHandler(this.grpGoal_Leave);
			// 
			// label61
			// 
			this.label61.Location = new System.Drawing.Point(168, 16);
			this.label61.Name = "label61";
			this.label61.Size = new System.Drawing.Size(104, 16);
			this.label61.TabIndex = 37;
			this.label61.Text = "of the Flight Group";
			this.label61.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// cboGoalAmount
			// 
			this.cboGoalAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGoalAmount.Location = new System.Drawing.Point(8, 16);
			this.cboGoalAmount.Name = "cboGoalAmount";
			this.cboGoalAmount.Size = new System.Drawing.Size(144, 21);
			this.cboGoalAmount.TabIndex = 38;
			// 
			// cboGoalArgument
			// 
			this.cboGoalArgument.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGoalArgument.Items.AddRange(new object[] {
            "must",
            "must not",
            "BONUS must",
            "BONUS must not"});
			this.cboGoalArgument.Location = new System.Drawing.Point(8, 48);
			this.cboGoalArgument.Name = "cboGoalArgument";
			this.cboGoalArgument.Size = new System.Drawing.Size(112, 21);
			this.cboGoalArgument.TabIndex = 40;
			// 
			// cboGoalTrigger
			// 
			this.cboGoalTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGoalTrigger.Location = new System.Drawing.Point(120, 48);
			this.cboGoalTrigger.Name = "cboGoalTrigger";
			this.cboGoalTrigger.Size = new System.Drawing.Size(160, 21);
			this.cboGoalTrigger.TabIndex = 41;
			// 
			// label66
			// 
			this.label66.AutoSize = true;
			this.label66.Location = new System.Drawing.Point(359, 294);
			this.label66.Name = "label66";
			this.label66.Size = new System.Drawing.Size(111, 13);
			this.label66.TabIndex = 47;
			this.label66.Text = "Goal Applies to Team:";
			// 
			// chkGoalEnable
			// 
			this.chkGoalEnable.AutoSize = true;
			this.chkGoalEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkGoalEnable.Location = new System.Drawing.Point(379, 314);
			this.chkGoalEnable.Name = "chkGoalEnable";
			this.chkGoalEnable.Size = new System.Drawing.Size(119, 17);
			this.chkGoalEnable.TabIndex = 46;
			this.chkGoalEnable.Text = "Enabled for Team 1";
			this.chkGoalEnable.CheckedChanged += new System.EventHandler(this.chkGoalEnable_CheckedChanged);
			// 
			// numGoalPoints
			// 
			this.numGoalPoints.Increment = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.numGoalPoints.Location = new System.Drawing.Point(462, 264);
			this.numGoalPoints.Maximum = new decimal(new int[] {
            31750,
            0,
            0,
            0});
			this.numGoalPoints.Minimum = new decimal(new int[] {
            32000,
            0,
            0,
            -2147483648});
			this.numGoalPoints.Name = "numGoalPoints";
			this.numGoalPoints.Size = new System.Drawing.Size(56, 20);
			this.numGoalPoints.TabIndex = 45;
			this.numGoalPoints.Leave += new System.EventHandler(this.numGoalPoints_Leave);
			// 
			// label65
			// 
			this.label65.AutoSize = true;
			this.label65.Location = new System.Drawing.Point(417, 268);
			this.label65.Name = "label65";
			this.label65.Size = new System.Drawing.Size(39, 13);
			this.label65.TabIndex = 44;
			this.label65.Text = "Points:";
			// 
			// label62
			// 
			this.label62.Location = new System.Drawing.Point(16, 360);
			this.label62.Name = "label62";
			this.label62.Size = new System.Drawing.Size(88, 16);
			this.label62.TabIndex = 43;
			this.label62.Text = "Goal Incomplete";
			this.label62.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// txtGoalInc
			// 
			this.txtGoalInc.BackColor = System.Drawing.Color.Black;
			this.txtGoalInc.ForeColor = System.Drawing.Color.Gold;
			this.txtGoalInc.Location = new System.Drawing.Point(120, 360);
			this.txtGoalInc.MaxLength = 63;
			this.txtGoalInc.Name = "txtGoalInc";
			this.txtGoalInc.Size = new System.Drawing.Size(376, 20);
			this.txtGoalInc.TabIndex = 42;
			this.txtGoalInc.Leave += new System.EventHandler(this.txtGoalInc_Leave);
			// 
			// label60
			// 
			this.label60.Location = new System.Drawing.Point(152, 16);
			this.label60.Name = "label60";
			this.label60.Size = new System.Drawing.Size(232, 16);
			this.label60.TabIndex = 30;
			this.label60.Text = "Right-click goal to copy, double-click to paste";
			// 
			// groupBox16
			// 
			this.groupBox16.Controls.Add(this.lblGoal1);
			this.groupBox16.Controls.Add(this.lblGoal2);
			this.groupBox16.Controls.Add(this.lblGoal3);
			this.groupBox16.Controls.Add(this.lblGoal4);
			this.groupBox16.Controls.Add(this.lblGoal5);
			this.groupBox16.Controls.Add(this.lblGoal8);
			this.groupBox16.Controls.Add(this.lblGoal6);
			this.groupBox16.Controls.Add(this.lblGoal7);
			this.groupBox16.Location = new System.Drawing.Point(16, 32);
			this.groupBox16.Name = "groupBox16";
			this.groupBox16.Size = new System.Drawing.Size(512, 216);
			this.groupBox16.TabIndex = 0;
			this.groupBox16.TabStop = false;
			// 
			// lblGoal1
			// 
			this.lblGoal1.BackColor = System.Drawing.Color.RosyBrown;
			this.lblGoal1.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lblGoal1.Location = new System.Drawing.Point(8, 16);
			this.lblGoal1.Name = "lblGoal1";
			this.lblGoal1.Size = new System.Drawing.Size(496, 24);
			this.lblGoal1.TabIndex = 0;
			this.lblGoal1.Text = "Goal 1:";
			this.lblGoal1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblGoal2
			// 
			this.lblGoal2.BackColor = System.Drawing.Color.RosyBrown;
			this.lblGoal2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblGoal2.Location = new System.Drawing.Point(8, 40);
			this.lblGoal2.Name = "lblGoal2";
			this.lblGoal2.Size = new System.Drawing.Size(496, 24);
			this.lblGoal2.TabIndex = 0;
			this.lblGoal2.Text = "Goal 2:";
			this.lblGoal2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblGoal3
			// 
			this.lblGoal3.BackColor = System.Drawing.Color.RosyBrown;
			this.lblGoal3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblGoal3.Location = new System.Drawing.Point(8, 64);
			this.lblGoal3.Name = "lblGoal3";
			this.lblGoal3.Size = new System.Drawing.Size(496, 24);
			this.lblGoal3.TabIndex = 0;
			this.lblGoal3.Text = "Goal 3:";
			this.lblGoal3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblGoal4
			// 
			this.lblGoal4.BackColor = System.Drawing.Color.RosyBrown;
			this.lblGoal4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblGoal4.Location = new System.Drawing.Point(8, 88);
			this.lblGoal4.Name = "lblGoal4";
			this.lblGoal4.Size = new System.Drawing.Size(496, 24);
			this.lblGoal4.TabIndex = 0;
			this.lblGoal4.Text = "Goal 4:";
			this.lblGoal4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblGoal5
			// 
			this.lblGoal5.BackColor = System.Drawing.Color.RosyBrown;
			this.lblGoal5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblGoal5.Location = new System.Drawing.Point(8, 112);
			this.lblGoal5.Name = "lblGoal5";
			this.lblGoal5.Size = new System.Drawing.Size(496, 24);
			this.lblGoal5.TabIndex = 0;
			this.lblGoal5.Text = "Goal 5:";
			this.lblGoal5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblGoal8
			// 
			this.lblGoal8.BackColor = System.Drawing.Color.RosyBrown;
			this.lblGoal8.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblGoal8.Location = new System.Drawing.Point(8, 184);
			this.lblGoal8.Name = "lblGoal8";
			this.lblGoal8.Size = new System.Drawing.Size(496, 24);
			this.lblGoal8.TabIndex = 0;
			this.lblGoal8.Text = "Goal 8:";
			this.lblGoal8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblGoal6
			// 
			this.lblGoal6.BackColor = System.Drawing.Color.RosyBrown;
			this.lblGoal6.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblGoal6.Location = new System.Drawing.Point(8, 136);
			this.lblGoal6.Name = "lblGoal6";
			this.lblGoal6.Size = new System.Drawing.Size(496, 24);
			this.lblGoal6.TabIndex = 0;
			this.lblGoal6.Text = "Goal 6:";
			this.lblGoal6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblGoal7
			// 
			this.lblGoal7.BackColor = System.Drawing.Color.RosyBrown;
			this.lblGoal7.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblGoal7.Location = new System.Drawing.Point(8, 160);
			this.lblGoal7.Name = "lblGoal7";
			this.lblGoal7.Size = new System.Drawing.Size(496, 24);
			this.lblGoal7.TabIndex = 0;
			this.lblGoal7.Text = "Goal 7:";
			this.lblGoal7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtGoalComp
			// 
			this.txtGoalComp.BackColor = System.Drawing.Color.Black;
			this.txtGoalComp.ForeColor = System.Drawing.Color.Lime;
			this.txtGoalComp.Location = new System.Drawing.Point(120, 392);
			this.txtGoalComp.Name = "txtGoalComp";
			this.txtGoalComp.Size = new System.Drawing.Size(376, 20);
			this.txtGoalComp.TabIndex = 42;
			this.txtGoalComp.Leave += new System.EventHandler(this.txtGoalComp_Leave);
			// 
			// txtGoalFail
			// 
			this.txtGoalFail.BackColor = System.Drawing.Color.Black;
			this.txtGoalFail.ForeColor = System.Drawing.Color.Red;
			this.txtGoalFail.Location = new System.Drawing.Point(120, 424);
			this.txtGoalFail.Name = "txtGoalFail";
			this.txtGoalFail.Size = new System.Drawing.Size(376, 20);
			this.txtGoalFail.TabIndex = 42;
			this.txtGoalFail.Leave += new System.EventHandler(this.txtGoalFail_Leave);
			// 
			// label63
			// 
			this.label63.Location = new System.Drawing.Point(16, 392);
			this.label63.Name = "label63";
			this.label63.Size = new System.Drawing.Size(88, 16);
			this.label63.TabIndex = 43;
			this.label63.Text = "Goal Complete";
			this.label63.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label64
			// 
			this.label64.Location = new System.Drawing.Point(16, 424);
			this.label64.Name = "label64";
			this.label64.Size = new System.Drawing.Size(88, 16);
			this.label64.TabIndex = 43;
			this.label64.Text = "Goal Failed";
			this.label64.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numGoalTimeLimit
			// 
			this.numGoalTimeLimit.Location = new System.Drawing.Point(400, 336);
			this.numGoalTimeLimit.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numGoalTimeLimit.Name = "numGoalTimeLimit";
			this.numGoalTimeLimit.Size = new System.Drawing.Size(56, 20);
			this.numGoalTimeLimit.TabIndex = 46;
			this.numGoalTimeLimit.ValueChanged += new System.EventHandler(this.numGoalTimeLimit_ValueChanged);
			// 
			// numGoalTeam
			// 
			this.numGoalTeam.Location = new System.Drawing.Point(476, 290);
			this.numGoalTeam.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
			this.numGoalTeam.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numGoalTeam.Name = "numGoalTeam";
			this.numGoalTeam.Size = new System.Drawing.Size(42, 20);
			this.numGoalTeam.TabIndex = 45;
			this.numGoalTeam.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numGoalTeam.Leave += new System.EventHandler(this.numGoalTeam_Leave);
			// 
			// tabWP
			// 
			this.tabWP.Controls.Add(this.label76);
			this.tabWP.Controls.Add(this.numRoll);
			this.tabWP.Controls.Add(this.numPitch);
			this.tabWP.Controls.Add(this.numYaw);
			this.tabWP.Controls.Add(this.label56);
			this.tabWP.Controls.Add(this.dataWP);
			this.tabWP.Controls.Add(this.dataWP_Raw);
			this.tabWP.Controls.Add(this.chkWPBrief1);
			this.tabWP.Controls.Add(this.chkWPHyp);
			this.tabWP.Controls.Add(this.chkWP8);
			this.tabWP.Controls.Add(this.chkWP7);
			this.tabWP.Controls.Add(this.chkWP2);
			this.tabWP.Controls.Add(this.chkWP1);
			this.tabWP.Controls.Add(this.chkSP4);
			this.tabWP.Controls.Add(this.chkSP3);
			this.tabWP.Controls.Add(this.chkSP2);
			this.tabWP.Controls.Add(this.chkSP1);
			this.tabWP.Controls.Add(this.chkWP6);
			this.tabWP.Controls.Add(this.chkWP5);
			this.tabWP.Controls.Add(this.chkWP4);
			this.tabWP.Controls.Add(this.chkWP3);
			this.tabWP.Controls.Add(this.chkWPRend);
			this.tabWP.Controls.Add(this.label77);
			this.tabWP.Controls.Add(this.label78);
			this.tabWP.Controls.Add(this.chkWPBrief2);
			this.tabWP.Controls.Add(this.chkWPBrief3);
			this.tabWP.Controls.Add(this.chkWPBrief4);
			this.tabWP.Controls.Add(this.chkWPBrief5);
			this.tabWP.Controls.Add(this.chkWPBrief6);
			this.tabWP.Controls.Add(this.chkWPBrief7);
			this.tabWP.Controls.Add(this.chkWPBrief8);
			this.tabWP.Location = new System.Drawing.Point(4, 22);
			this.tabWP.Name = "tabWP";
			this.tabWP.Size = new System.Drawing.Size(544, 478);
			this.tabWP.TabIndex = 3;
			this.tabWP.Text = "Waypoints";
			// 
			// label76
			// 
			this.label76.Image = ((System.Drawing.Image)(resources.GetObject("label76.Image")));
			this.label76.Location = new System.Drawing.Point(480, 28);
			this.label76.Name = "label76";
			this.label76.Size = new System.Drawing.Size(48, 16);
			this.label76.TabIndex = 27;
			// 
			// numRoll
			// 
			this.numRoll.Enabled = false;
			this.numRoll.Location = new System.Drawing.Point(480, 212);
			this.numRoll.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.numRoll.Minimum = new decimal(new int[] {
            179,
            0,
            0,
            -2147483648});
			this.numRoll.Name = "numRoll";
			this.numRoll.Size = new System.Drawing.Size(48, 20);
			this.numRoll.TabIndex = 49;
			this.numRoll.Leave += new System.EventHandler(this.numRoll_Leave);
			// 
			// numPitch
			// 
			this.numPitch.Location = new System.Drawing.Point(480, 132);
			this.numPitch.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.numPitch.Minimum = new decimal(new int[] {
            179,
            0,
            0,
            -2147483648});
			this.numPitch.Name = "numPitch";
			this.numPitch.Size = new System.Drawing.Size(48, 20);
			this.numPitch.TabIndex = 48;
			this.numPitch.Leave += new System.EventHandler(this.numPitch_Leave);
			// 
			// numYaw
			// 
			this.numYaw.Enabled = false;
			this.numYaw.Location = new System.Drawing.Point(480, 52);
			this.numYaw.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.numYaw.Minimum = new decimal(new int[] {
            179,
            0,
            0,
            -2147483648});
			this.numYaw.Name = "numYaw";
			this.numYaw.Size = new System.Drawing.Size(48, 20);
			this.numYaw.TabIndex = 47;
			this.numYaw.Leave += new System.EventHandler(this.numYaw_Leave);
			// 
			// label56
			// 
			this.label56.Location = new System.Drawing.Point(240, 12);
			this.label56.Name = "label56";
			this.label56.Size = new System.Drawing.Size(64, 16);
			this.label56.TabIndex = 21;
			this.label56.Text = "Raw Data:";
			this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// dataWP
			// 
			this.dataWP.AllowSorting = false;
			this.dataWP.CaptionVisible = false;
			this.dataWP.DataMember = "";
			this.dataWP.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataWP.Location = new System.Drawing.Point(8, 8);
			this.dataWP.Name = "dataWP";
			this.dataWP.PreferredColumnWidth = 52;
			this.dataWP.PreferredRowHeight = 20;
			this.dataWP.RowHeadersVisible = false;
			this.dataWP.Size = new System.Drawing.Size(160, 463);
			this.dataWP.TabIndex = 20;
			// 
			// dataWP_Raw
			// 
			this.dataWP_Raw.AllowSorting = false;
			this.dataWP_Raw.CaptionVisible = false;
			this.dataWP_Raw.DataMember = "";
			this.dataWP_Raw.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataWP_Raw.Location = new System.Drawing.Point(312, 8);
			this.dataWP_Raw.Name = "dataWP_Raw";
			this.dataWP_Raw.PreferredColumnWidth = 52;
			this.dataWP_Raw.PreferredRowHeight = 20;
			this.dataWP_Raw.RowHeadersVisible = false;
			this.dataWP_Raw.Size = new System.Drawing.Size(160, 463);
			this.dataWP_Raw.TabIndex = 19;
			// 
			// chkWPBrief1
			// 
			this.chkWPBrief1.Location = new System.Drawing.Point(176, 310);
			this.chkWPBrief1.Name = "chkWPBrief1";
			this.chkWPBrief1.Size = new System.Drawing.Size(96, 16);
			this.chkWPBrief1.TabIndex = 44;
			this.chkWPBrief1.Text = "Briefing 1";
			// 
			// chkWPHyp
			// 
			this.chkWPHyp.Location = new System.Drawing.Point(176, 290);
			this.chkWPHyp.Name = "chkWPHyp";
			this.chkWPHyp.Size = new System.Drawing.Size(96, 16);
			this.chkWPHyp.TabIndex = 38;
			this.chkWPHyp.Text = "Hyperspace";
			// 
			// chkWP8
			// 
			this.chkWP8.Location = new System.Drawing.Point(176, 250);
			this.chkWP8.Name = "chkWP8";
			this.chkWP8.Size = new System.Drawing.Size(96, 16);
			this.chkWP8.TabIndex = 36;
			this.chkWP8.Text = "Waypoint 8";
			// 
			// chkWP7
			// 
			this.chkWP7.Location = new System.Drawing.Point(176, 230);
			this.chkWP7.Name = "chkWP7";
			this.chkWP7.Size = new System.Drawing.Size(96, 16);
			this.chkWP7.TabIndex = 35;
			this.chkWP7.Text = "Waypoint 7";
			// 
			// chkWP2
			// 
			this.chkWP2.Location = new System.Drawing.Point(176, 130);
			this.chkWP2.Name = "chkWP2";
			this.chkWP2.Size = new System.Drawing.Size(96, 16);
			this.chkWP2.TabIndex = 28;
			this.chkWP2.Text = "Waypoint 2";
			// 
			// chkWP1
			// 
			this.chkWP1.Location = new System.Drawing.Point(176, 110);
			this.chkWP1.Name = "chkWP1";
			this.chkWP1.Size = new System.Drawing.Size(96, 16);
			this.chkWP1.TabIndex = 26;
			this.chkWP1.Text = "Waypoint 1";
			// 
			// chkSP4
			// 
			this.chkSP4.Location = new System.Drawing.Point(176, 90);
			this.chkSP4.Name = "chkSP4";
			this.chkSP4.Size = new System.Drawing.Size(96, 16);
			this.chkSP4.TabIndex = 25;
			this.chkSP4.Text = "Start Point4";
			// 
			// chkSP3
			// 
			this.chkSP3.Location = new System.Drawing.Point(176, 70);
			this.chkSP3.Name = "chkSP3";
			this.chkSP3.Size = new System.Drawing.Size(96, 16);
			this.chkSP3.TabIndex = 24;
			this.chkSP3.Text = "Start Point3";
			// 
			// chkSP2
			// 
			this.chkSP2.Location = new System.Drawing.Point(176, 50);
			this.chkSP2.Name = "chkSP2";
			this.chkSP2.Size = new System.Drawing.Size(96, 16);
			this.chkSP2.TabIndex = 23;
			this.chkSP2.Text = "Start Point2";
			// 
			// chkSP1
			// 
			this.chkSP1.Location = new System.Drawing.Point(176, 30);
			this.chkSP1.Name = "chkSP1";
			this.chkSP1.Size = new System.Drawing.Size(96, 16);
			this.chkSP1.TabIndex = 22;
			this.chkSP1.Text = "Start Point 1";
			// 
			// chkWP6
			// 
			this.chkWP6.Location = new System.Drawing.Point(176, 210);
			this.chkWP6.Name = "chkWP6";
			this.chkWP6.Size = new System.Drawing.Size(96, 16);
			this.chkWP6.TabIndex = 34;
			this.chkWP6.Text = "Waypoint 6";
			// 
			// chkWP5
			// 
			this.chkWP5.Location = new System.Drawing.Point(176, 190);
			this.chkWP5.Name = "chkWP5";
			this.chkWP5.Size = new System.Drawing.Size(96, 16);
			this.chkWP5.TabIndex = 33;
			this.chkWP5.Text = "Waypoint 5";
			// 
			// chkWP4
			// 
			this.chkWP4.Location = new System.Drawing.Point(176, 170);
			this.chkWP4.Name = "chkWP4";
			this.chkWP4.Size = new System.Drawing.Size(96, 16);
			this.chkWP4.TabIndex = 32;
			this.chkWP4.Text = "Waypoint 4";
			// 
			// chkWP3
			// 
			this.chkWP3.Location = new System.Drawing.Point(176, 150);
			this.chkWP3.Name = "chkWP3";
			this.chkWP3.Size = new System.Drawing.Size(96, 16);
			this.chkWP3.TabIndex = 31;
			this.chkWP3.Text = "Waypoint 3";
			// 
			// chkWPRend
			// 
			this.chkWPRend.Location = new System.Drawing.Point(176, 270);
			this.chkWPRend.Name = "chkWPRend";
			this.chkWPRend.Size = new System.Drawing.Size(96, 16);
			this.chkWPRend.TabIndex = 37;
			this.chkWPRend.Text = "Rendezvous";
			// 
			// label77
			// 
			this.label77.Image = ((System.Drawing.Image)(resources.GetObject("label77.Image")));
			this.label77.Location = new System.Drawing.Point(480, 108);
			this.label77.Name = "label77";
			this.label77.Size = new System.Drawing.Size(56, 16);
			this.label77.TabIndex = 29;
			// 
			// label78
			// 
			this.label78.Image = ((System.Drawing.Image)(resources.GetObject("label78.Image")));
			this.label78.Location = new System.Drawing.Point(480, 188);
			this.label78.Name = "label78";
			this.label78.Size = new System.Drawing.Size(56, 16);
			this.label78.TabIndex = 30;
			// 
			// chkWPBrief2
			// 
			this.chkWPBrief2.Location = new System.Drawing.Point(176, 330);
			this.chkWPBrief2.Name = "chkWPBrief2";
			this.chkWPBrief2.Size = new System.Drawing.Size(96, 16);
			this.chkWPBrief2.TabIndex = 40;
			this.chkWPBrief2.Text = "Briefing 2";
			// 
			// chkWPBrief3
			// 
			this.chkWPBrief3.Location = new System.Drawing.Point(176, 350);
			this.chkWPBrief3.Name = "chkWPBrief3";
			this.chkWPBrief3.Size = new System.Drawing.Size(96, 16);
			this.chkWPBrief3.TabIndex = 39;
			this.chkWPBrief3.Text = "Briefing 3";
			// 
			// chkWPBrief4
			// 
			this.chkWPBrief4.Location = new System.Drawing.Point(176, 370);
			this.chkWPBrief4.Name = "chkWPBrief4";
			this.chkWPBrief4.Size = new System.Drawing.Size(96, 16);
			this.chkWPBrief4.TabIndex = 42;
			this.chkWPBrief4.Text = "Briefing 4";
			// 
			// chkWPBrief5
			// 
			this.chkWPBrief5.Location = new System.Drawing.Point(176, 390);
			this.chkWPBrief5.Name = "chkWPBrief5";
			this.chkWPBrief5.Size = new System.Drawing.Size(96, 16);
			this.chkWPBrief5.TabIndex = 41;
			this.chkWPBrief5.Text = "Briefing 5";
			// 
			// chkWPBrief6
			// 
			this.chkWPBrief6.Location = new System.Drawing.Point(176, 410);
			this.chkWPBrief6.Name = "chkWPBrief6";
			this.chkWPBrief6.Size = new System.Drawing.Size(96, 16);
			this.chkWPBrief6.TabIndex = 45;
			this.chkWPBrief6.Text = "Briefing 6";
			// 
			// chkWPBrief7
			// 
			this.chkWPBrief7.Location = new System.Drawing.Point(176, 430);
			this.chkWPBrief7.Name = "chkWPBrief7";
			this.chkWPBrief7.Size = new System.Drawing.Size(96, 16);
			this.chkWPBrief7.TabIndex = 46;
			this.chkWPBrief7.Text = "Briefing 7";
			// 
			// chkWPBrief8
			// 
			this.chkWPBrief8.Location = new System.Drawing.Point(176, 450);
			this.chkWPBrief8.Name = "chkWPBrief8";
			this.chkWPBrief8.Size = new System.Drawing.Size(96, 16);
			this.chkWPBrief8.TabIndex = 43;
			this.chkWPBrief8.Text = "Briefing 8";
			// 
			// tabOrders
			// 
			this.tabOrders.Controls.Add(this.cboOSpeed);
			this.tabOrders.Controls.Add(this.lblOSpeedNote);
			this.tabOrders.Controls.Add(this.lblOVar2Note);
			this.tabOrders.Controls.Add(this.lblOVar1Note);
			this.tabOrders.Controls.Add(this.label57);
			this.tabOrders.Controls.Add(this.txtOString);
			this.tabOrders.Controls.Add(this.label54);
			this.tabOrders.Controls.Add(this.cmdCopyOrder);
			this.tabOrders.Controls.Add(this.lblODesc);
			this.tabOrders.Controls.Add(this.grpSecOrder);
			this.tabOrders.Controls.Add(this.grpPrimOrder);
			this.tabOrders.Controls.Add(this.numOVar1);
			this.tabOrders.Controls.Add(this.lblOVar1);
			this.tabOrders.Controls.Add(this.cboOThrottle);
			this.tabOrders.Controls.Add(this.label51);
			this.tabOrders.Controls.Add(this.groupBox15);
			this.tabOrders.Controls.Add(this.cboOrders);
			this.tabOrders.Controls.Add(this.lblOVar2);
			this.tabOrders.Controls.Add(this.numOVar2);
			this.tabOrders.Controls.Add(this.cmdPasteOrder);
			this.tabOrders.Location = new System.Drawing.Point(4, 22);
			this.tabOrders.Name = "tabOrders";
			this.tabOrders.Size = new System.Drawing.Size(544, 478);
			this.tabOrders.TabIndex = 4;
			this.tabOrders.Text = "Orders";
			// 
			// cboOSpeed
			// 
			this.cboOSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOSpeed.FormattingEnabled = true;
			this.cboOSpeed.Items.AddRange(new object[] {
            "default"});
			this.cboOSpeed.Location = new System.Drawing.Point(164, 188);
			this.cboOSpeed.Name = "cboOSpeed";
			this.cboOSpeed.Size = new System.Drawing.Size(56, 21);
			this.cboOSpeed.TabIndex = 37;
			this.cboOSpeed.SelectedIndexChanged += new System.EventHandler(this.cboOSpeed_SelectedIndexChanged);
			// 
			// lblOSpeedNote
			// 
			this.lblOSpeedNote.AutoSize = true;
			this.lblOSpeedNote.Location = new System.Drawing.Point(170, 211);
			this.lblOSpeedNote.Name = "lblOSpeedNote";
			this.lblOSpeedNote.Size = new System.Drawing.Size(37, 13);
			this.lblOSpeedNote.TabIndex = 36;
			this.lblOSpeedNote.Text = "MGLT";
			// 
			// lblOVar2Note
			// 
			this.lblOVar2Note.Location = new System.Drawing.Point(423, 211);
			this.lblOVar2Note.Name = "lblOVar2Note";
			this.lblOVar2Note.Size = new System.Drawing.Size(113, 16);
			this.lblOVar2Note.TabIndex = 35;
			this.lblOVar2Note.Text = "lblOVar2Note";
			this.lblOVar2Note.Visible = false;
			// 
			// lblOVar1Note
			// 
			this.lblOVar1Note.Location = new System.Drawing.Point(297, 211);
			this.lblOVar1Note.Name = "lblOVar1Note";
			this.lblOVar1Note.Size = new System.Drawing.Size(120, 16);
			this.lblOVar1Note.TabIndex = 35;
			this.lblOVar1Note.Text = "lblOVar1Note";
			this.lblOVar1Note.Visible = false;
			// 
			// label57
			// 
			this.label57.Location = new System.Drawing.Point(116, 190);
			this.label57.Name = "label57";
			this.label57.Size = new System.Drawing.Size(44, 16);
			this.label57.TabIndex = 33;
			this.label57.Text = "Speed:";
			this.label57.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// txtOString
			// 
			this.txtOString.Location = new System.Drawing.Point(408, 16);
			this.txtOString.MaxLength = 15;
			this.txtOString.Name = "txtOString";
			this.txtOString.Size = new System.Drawing.Size(128, 20);
			this.txtOString.TabIndex = 32;
			this.txtOString.Leave += new System.EventHandler(this.txtOString_Leave);
			// 
			// label54
			// 
			this.label54.Location = new System.Drawing.Point(296, 16);
			this.label54.Name = "label54";
			this.label54.Size = new System.Drawing.Size(104, 16);
			this.label54.TabIndex = 31;
			this.label54.Text = "Player Roster Role:";
			this.label54.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// cmdCopyOrder
			// 
			this.cmdCopyOrder.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdCopyOrder.ImageIndex = 6;
			this.cmdCopyOrder.ImageList = this.imgToolbar;
			this.cmdCopyOrder.Location = new System.Drawing.Point(16, 16);
			this.cmdCopyOrder.Name = "cmdCopyOrder";
			this.cmdCopyOrder.Size = new System.Drawing.Size(24, 23);
			this.cmdCopyOrder.TabIndex = 29;
			this.cmdCopyOrder.Click += new System.EventHandler(this.cmdCopyOrder_Click);
			// 
			// lblODesc
			// 
			this.lblODesc.Location = new System.Drawing.Point(16, 48);
			this.lblODesc.Name = "lblODesc";
			this.lblODesc.Size = new System.Drawing.Size(512, 16);
			this.lblODesc.TabIndex = 28;
			// 
			// grpSecOrder
			// 
			this.grpSecOrder.Controls.Add(this.label49);
			this.grpSecOrder.Controls.Add(this.optOT3T4OR);
			this.grpSecOrder.Controls.Add(this.cboOT3);
			this.grpSecOrder.Controls.Add(this.cboOT3Type);
			this.grpSecOrder.Controls.Add(this.cboOT4Type);
			this.grpSecOrder.Controls.Add(this.cboOT4);
			this.grpSecOrder.Controls.Add(this.optOT3T4AND);
			this.grpSecOrder.Location = new System.Drawing.Point(16, 352);
			this.grpSecOrder.Name = "grpSecOrder";
			this.grpSecOrder.Size = new System.Drawing.Size(512, 112);
			this.grpSecOrder.TabIndex = 27;
			this.grpSecOrder.TabStop = false;
			this.grpSecOrder.Text = "Secondary Target";
			// 
			// label49
			// 
			this.label49.Location = new System.Drawing.Point(16, 40);
			this.label49.Name = "label49";
			this.label49.Size = new System.Drawing.Size(96, 48);
			this.label49.TabIndex = 3;
			this.label49.Text = "Selecting \"OR\" allows for multiple targets";
			// 
			// optOT3T4OR
			// 
			this.optOT3T4OR.Checked = true;
			this.optOT3T4OR.Location = new System.Drawing.Point(312, 56);
			this.optOT3T4OR.Name = "optOT3T4OR";
			this.optOT3T4OR.Size = new System.Drawing.Size(104, 16);
			this.optOT3T4OR.TabIndex = 13;
			this.optOT3T4OR.TabStop = true;
			this.optOT3T4OR.Text = "OR";
			this.optOT3T4OR.CheckedChanged += new System.EventHandler(this.optOT3T4OR_CheckedChanged);
			// 
			// cboOT3
			// 
			this.cboOT3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOT3.Location = new System.Drawing.Point(304, 24);
			this.cboOT3.Name = "cboOT3";
			this.cboOT3.Size = new System.Drawing.Size(184, 21);
			this.cboOT3.TabIndex = 11;
			this.cboOT3.Leave += new System.EventHandler(this.cboOT3_Leave);
			// 
			// cboOT3Type
			// 
			this.cboOT3Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOT3Type.Location = new System.Drawing.Point(128, 24);
			this.cboOT3Type.Name = "cboOT3Type";
			this.cboOT3Type.Size = new System.Drawing.Size(160, 21);
			this.cboOT3Type.TabIndex = 10;
			this.cboOT3Type.SelectedIndexChanged += new System.EventHandler(this.cboOT3Type_SelectedIndexChanged);
			// 
			// cboOT4Type
			// 
			this.cboOT4Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOT4Type.Location = new System.Drawing.Point(128, 80);
			this.cboOT4Type.Name = "cboOT4Type";
			this.cboOT4Type.Size = new System.Drawing.Size(160, 21);
			this.cboOT4Type.TabIndex = 14;
			this.cboOT4Type.SelectedIndexChanged += new System.EventHandler(this.cboOT4Type_SelectedIndexChanged);
			// 
			// cboOT4
			// 
			this.cboOT4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOT4.Location = new System.Drawing.Point(304, 80);
			this.cboOT4.Name = "cboOT4";
			this.cboOT4.Size = new System.Drawing.Size(184, 21);
			this.cboOT4.TabIndex = 15;
			this.cboOT4.Leave += new System.EventHandler(this.cboOT4_Leave);
			// 
			// optOT3T4AND
			// 
			this.optOT3T4AND.Location = new System.Drawing.Point(184, 56);
			this.optOT3T4AND.Name = "optOT3T4AND";
			this.optOT3T4AND.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.optOT3T4AND.Size = new System.Drawing.Size(104, 16);
			this.optOT3T4AND.TabIndex = 12;
			this.optOT3T4AND.Text = "AND";
			// 
			// grpPrimOrder
			// 
			this.grpPrimOrder.Controls.Add(this.label50);
			this.grpPrimOrder.Controls.Add(this.optOT1T2OR);
			this.grpPrimOrder.Controls.Add(this.cboOT1);
			this.grpPrimOrder.Controls.Add(this.cboOT1Type);
			this.grpPrimOrder.Controls.Add(this.cboOT2Type);
			this.grpPrimOrder.Controls.Add(this.cboOT2);
			this.grpPrimOrder.Controls.Add(this.optOT1T2AND);
			this.grpPrimOrder.Location = new System.Drawing.Point(16, 224);
			this.grpPrimOrder.Name = "grpPrimOrder";
			this.grpPrimOrder.Size = new System.Drawing.Size(512, 112);
			this.grpPrimOrder.TabIndex = 26;
			this.grpPrimOrder.TabStop = false;
			this.grpPrimOrder.Text = "Primary Target";
			// 
			// label50
			// 
			this.label50.Location = new System.Drawing.Point(16, 40);
			this.label50.Name = "label50";
			this.label50.Size = new System.Drawing.Size(96, 56);
			this.label50.TabIndex = 2;
			this.label50.Text = "Selecting \"AND\" will require that the target meet both settings";
			// 
			// optOT1T2OR
			// 
			this.optOT1T2OR.Checked = true;
			this.optOT1T2OR.Location = new System.Drawing.Point(312, 56);
			this.optOT1T2OR.Name = "optOT1T2OR";
			this.optOT1T2OR.Size = new System.Drawing.Size(104, 16);
			this.optOT1T2OR.TabIndex = 7;
			this.optOT1T2OR.TabStop = true;
			this.optOT1T2OR.Text = "OR";
			this.optOT1T2OR.CheckedChanged += new System.EventHandler(this.optOT1T2OR_CheckedChanged);
			// 
			// cboOT1
			// 
			this.cboOT1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOT1.Location = new System.Drawing.Point(304, 24);
			this.cboOT1.Name = "cboOT1";
			this.cboOT1.Size = new System.Drawing.Size(184, 21);
			this.cboOT1.TabIndex = 5;
			this.cboOT1.Leave += new System.EventHandler(this.cboOT1_Leave);
			// 
			// cboOT1Type
			// 
			this.cboOT1Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOT1Type.Location = new System.Drawing.Point(128, 24);
			this.cboOT1Type.Name = "cboOT1Type";
			this.cboOT1Type.Size = new System.Drawing.Size(160, 21);
			this.cboOT1Type.TabIndex = 4;
			this.cboOT1Type.SelectedIndexChanged += new System.EventHandler(this.cboOT1Type_SelectedIndexChanged);
			// 
			// cboOT2Type
			// 
			this.cboOT2Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOT2Type.Location = new System.Drawing.Point(128, 80);
			this.cboOT2Type.Name = "cboOT2Type";
			this.cboOT2Type.Size = new System.Drawing.Size(160, 21);
			this.cboOT2Type.TabIndex = 8;
			this.cboOT2Type.SelectedIndexChanged += new System.EventHandler(this.cboOT2Type_SelectedIndexChanged);
			// 
			// cboOT2
			// 
			this.cboOT2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOT2.Location = new System.Drawing.Point(304, 80);
			this.cboOT2.Name = "cboOT2";
			this.cboOT2.Size = new System.Drawing.Size(184, 21);
			this.cboOT2.TabIndex = 9;
			this.cboOT2.Leave += new System.EventHandler(this.cboOT2_Leave);
			// 
			// optOT1T2AND
			// 
			this.optOT1T2AND.Location = new System.Drawing.Point(184, 56);
			this.optOT1T2AND.Name = "optOT1T2AND";
			this.optOT1T2AND.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.optOT1T2AND.Size = new System.Drawing.Size(104, 16);
			this.optOT1T2AND.TabIndex = 6;
			this.optOT1T2AND.Text = "AND";
			// 
			// numOVar1
			// 
			this.numOVar1.Location = new System.Drawing.Point(336, 188);
			this.numOVar1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numOVar1.Name = "numOVar1";
			this.numOVar1.Size = new System.Drawing.Size(46, 20);
			this.numOVar1.TabIndex = 22;
			this.numOVar1.ValueChanged += new System.EventHandler(this.numOVar1_ValueChanged);
			// 
			// lblOVar1
			// 
			this.lblOVar1.Location = new System.Drawing.Point(216, 190);
			this.lblOVar1.Name = "lblOVar1";
			this.lblOVar1.Size = new System.Drawing.Size(120, 16);
			this.lblOVar1.TabIndex = 25;
			this.lblOVar1.Text = "lblOVar1";
			this.lblOVar1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// cboOThrottle
			// 
			this.cboOThrottle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOThrottle.Items.AddRange(new object[] {
            "0",
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100"});
			this.cboOThrottle.Location = new System.Drawing.Point(64, 188);
			this.cboOThrottle.Name = "cboOThrottle";
			this.cboOThrottle.Size = new System.Drawing.Size(48, 21);
			this.cboOThrottle.TabIndex = 19;
			this.cboOThrottle.Leave += new System.EventHandler(this.cboOThrottle_Leave);
			// 
			// label51
			// 
			this.label51.Location = new System.Drawing.Point(5, 190);
			this.label51.Name = "label51";
			this.label51.Size = new System.Drawing.Size(56, 16);
			this.label51.TabIndex = 21;
			this.label51.Text = "Throttle %";
			this.label51.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// groupBox15
			// 
			this.groupBox15.Controls.Add(this.lblOrder1);
			this.groupBox15.Controls.Add(this.lblOrder2);
			this.groupBox15.Controls.Add(this.lblOrder3);
			this.groupBox15.Controls.Add(this.lblOrder4);
			this.groupBox15.Location = new System.Drawing.Point(8, 64);
			this.groupBox15.Name = "groupBox15";
			this.groupBox15.Size = new System.Drawing.Size(528, 120);
			this.groupBox15.TabIndex = 20;
			this.groupBox15.TabStop = false;
			// 
			// lblOrder1
			// 
			this.lblOrder1.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOrder1.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lblOrder1.Location = new System.Drawing.Point(8, 16);
			this.lblOrder1.Name = "lblOrder1";
			this.lblOrder1.Size = new System.Drawing.Size(512, 24);
			this.lblOrder1.TabIndex = 0;
			this.lblOrder1.Text = "Order 1:";
			this.lblOrder1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblOrder2
			// 
			this.lblOrder2.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOrder2.Location = new System.Drawing.Point(8, 40);
			this.lblOrder2.Name = "lblOrder2";
			this.lblOrder2.Size = new System.Drawing.Size(512, 24);
			this.lblOrder2.TabIndex = 0;
			this.lblOrder2.Text = "Order 2:";
			this.lblOrder2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblOrder3
			// 
			this.lblOrder3.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOrder3.Location = new System.Drawing.Point(8, 64);
			this.lblOrder3.Name = "lblOrder3";
			this.lblOrder3.Size = new System.Drawing.Size(512, 24);
			this.lblOrder3.TabIndex = 0;
			this.lblOrder3.Text = "Order 3:";
			this.lblOrder3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblOrder4
			// 
			this.lblOrder4.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOrder4.Location = new System.Drawing.Point(8, 88);
			this.lblOrder4.Name = "lblOrder4";
			this.lblOrder4.Size = new System.Drawing.Size(512, 24);
			this.lblOrder4.TabIndex = 0;
			this.lblOrder4.Text = "Order 4:";
			this.lblOrder4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cboOrders
			// 
			this.cboOrders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOrders.Location = new System.Drawing.Point(80, 16);
			this.cboOrders.Name = "cboOrders";
			this.cboOrders.Size = new System.Drawing.Size(200, 21);
			this.cboOrders.TabIndex = 18;
			this.cboOrders.SelectedIndexChanged += new System.EventHandler(this.cboOrders_SelectedIndexChanged);
			// 
			// lblOVar2
			// 
			this.lblOVar2.Location = new System.Drawing.Point(384, 190);
			this.lblOVar2.Name = "lblOVar2";
			this.lblOVar2.Size = new System.Drawing.Size(112, 16);
			this.lblOVar2.TabIndex = 24;
			this.lblOVar2.Text = "lblOVar2";
			this.lblOVar2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numOVar2
			// 
			this.numOVar2.Location = new System.Drawing.Point(496, 188);
			this.numOVar2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numOVar2.Name = "numOVar2";
			this.numOVar2.Size = new System.Drawing.Size(40, 20);
			this.numOVar2.TabIndex = 23;
			this.numOVar2.ValueChanged += new System.EventHandler(this.numOVar2_ValueChanged);
			// 
			// cmdPasteOrder
			// 
			this.cmdPasteOrder.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdPasteOrder.ImageIndex = 7;
			this.cmdPasteOrder.ImageList = this.imgToolbar;
			this.cmdPasteOrder.Location = new System.Drawing.Point(48, 16);
			this.cmdPasteOrder.Name = "cmdPasteOrder";
			this.cmdPasteOrder.Size = new System.Drawing.Size(24, 23);
			this.cmdPasteOrder.TabIndex = 30;
			this.cmdPasteOrder.Click += new System.EventHandler(this.cmdPasteOrder_Click);
			// 
			// tapOption
			// 
			this.tapOption.Controls.Add(this.grpRole);
			this.tapOption.Controls.Add(this.groupBox23);
			this.tapOption.Controls.Add(this.groupBox22);
			this.tapOption.Controls.Add(this.groupBox21);
			this.tapOption.Controls.Add(this.groupBox20);
			this.tapOption.Controls.Add(this.groupBox19);
			this.tapOption.Location = new System.Drawing.Point(4, 22);
			this.tapOption.Name = "tapOption";
			this.tapOption.Size = new System.Drawing.Size(544, 478);
			this.tapOption.TabIndex = 6;
			this.tapOption.Text = "Options";
			// 
			// grpRole
			// 
			this.grpRole.Controls.Add(this.cboRoleTeam4);
			this.grpRole.Controls.Add(this.cboRoleTeam3);
			this.grpRole.Controls.Add(this.cboRoleTeam2);
			this.grpRole.Controls.Add(this.cboRole1);
			this.grpRole.Controls.Add(this.cboRole2);
			this.grpRole.Controls.Add(this.cboRole3);
			this.grpRole.Controls.Add(this.cboRole4);
			this.grpRole.Controls.Add(this.cboRoleTeam1);
			this.grpRole.Location = new System.Drawing.Point(406, 96);
			this.grpRole.Name = "grpRole";
			this.grpRole.Size = new System.Drawing.Size(136, 368);
			this.grpRole.TabIndex = 5;
			this.grpRole.TabStop = false;
			this.grpRole.Text = "Roles (up to 4)";
			this.grpRole.Leave += new System.EventHandler(this.grpRole_Leave);
			// 
			// cboRoleTeam4
			// 
			this.cboRoleTeam4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRoleTeam4.FormattingEnabled = true;
			this.cboRoleTeam4.Location = new System.Drawing.Point(8, 205);
			this.cboRoleTeam4.Name = "cboRoleTeam4";
			this.cboRoleTeam4.Size = new System.Drawing.Size(120, 21);
			this.cboRoleTeam4.TabIndex = 6;
			// 
			// cboRoleTeam3
			// 
			this.cboRoleTeam3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRoleTeam3.FormattingEnabled = true;
			this.cboRoleTeam3.Location = new System.Drawing.Point(8, 145);
			this.cboRoleTeam3.Name = "cboRoleTeam3";
			this.cboRoleTeam3.Size = new System.Drawing.Size(120, 21);
			this.cboRoleTeam3.TabIndex = 4;
			// 
			// cboRoleTeam2
			// 
			this.cboRoleTeam2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRoleTeam2.FormattingEnabled = true;
			this.cboRoleTeam2.Location = new System.Drawing.Point(8, 85);
			this.cboRoleTeam2.Name = "cboRoleTeam2";
			this.cboRoleTeam2.Size = new System.Drawing.Size(120, 21);
			this.cboRoleTeam2.TabIndex = 2;
			// 
			// cboRole1
			// 
			this.cboRole1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRole1.Location = new System.Drawing.Point(8, 48);
			this.cboRole1.Name = "cboRole1";
			this.cboRole1.Size = new System.Drawing.Size(120, 21);
			this.cboRole1.TabIndex = 1;
			// 
			// cboRole2
			// 
			this.cboRole2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRole2.Location = new System.Drawing.Point(8, 108);
			this.cboRole2.Name = "cboRole2";
			this.cboRole2.Size = new System.Drawing.Size(120, 21);
			this.cboRole2.TabIndex = 3;
			// 
			// cboRole3
			// 
			this.cboRole3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRole3.Location = new System.Drawing.Point(8, 168);
			this.cboRole3.Name = "cboRole3";
			this.cboRole3.Size = new System.Drawing.Size(120, 21);
			this.cboRole3.TabIndex = 5;
			// 
			// cboRole4
			// 
			this.cboRole4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRole4.Location = new System.Drawing.Point(8, 228);
			this.cboRole4.Name = "cboRole4";
			this.cboRole4.Size = new System.Drawing.Size(120, 21);
			this.cboRole4.TabIndex = 7;
			// 
			// cboRoleTeam1
			// 
			this.cboRoleTeam1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRoleTeam1.Location = new System.Drawing.Point(8, 25);
			this.cboRoleTeam1.Name = "cboRoleTeam1";
			this.cboRoleTeam1.Size = new System.Drawing.Size(120, 21);
			this.cboRoleTeam1.TabIndex = 0;
			// 
			// groupBox23
			// 
			this.groupBox23.Controls.Add(this.cmdCopySkip);
			this.groupBox23.Controls.Add(this.label71);
			this.groupBox23.Controls.Add(this.cboSkipAmount);
			this.groupBox23.Controls.Add(this.cboSkipType);
			this.groupBox23.Controls.Add(this.cboSkipVar);
			this.groupBox23.Controls.Add(this.cboSkipTrig);
			this.groupBox23.Controls.Add(this.label72);
			this.groupBox23.Controls.Add(this.cmdPasteSkip);
			this.groupBox23.Controls.Add(this.optSkipAND);
			this.groupBox23.Controls.Add(this.optSkipOR);
			this.groupBox23.Controls.Add(this.lblSkipTrig1);
			this.groupBox23.Controls.Add(this.lblSkipTrig2);
			this.groupBox23.Location = new System.Drawing.Point(16, 288);
			this.groupBox23.Name = "groupBox23";
			this.groupBox23.Size = new System.Drawing.Size(384, 176);
			this.groupBox23.TabIndex = 4;
			this.groupBox23.TabStop = false;
			this.groupBox23.Text = "Skip to Order 4";
			// 
			// cmdCopySkip
			// 
			this.cmdCopySkip.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdCopySkip.ImageIndex = 6;
			this.cmdCopySkip.ImageList = this.imgToolbar;
			this.cmdCopySkip.Location = new System.Drawing.Point(8, 120);
			this.cmdCopySkip.Name = "cmdCopySkip";
			this.cmdCopySkip.Size = new System.Drawing.Size(24, 23);
			this.cmdCopySkip.TabIndex = 45;
			// 
			// label71
			// 
			this.label71.Location = new System.Drawing.Point(196, 120);
			this.label71.Name = "label71";
			this.label71.Size = new System.Drawing.Size(16, 16);
			this.label71.TabIndex = 40;
			this.label71.Text = "of";
			this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cboSkipAmount
			// 
			this.cboSkipAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSkipAmount.Location = new System.Drawing.Point(44, 120);
			this.cboSkipAmount.Name = "cboSkipAmount";
			this.cboSkipAmount.Size = new System.Drawing.Size(144, 21);
			this.cboSkipAmount.TabIndex = 41;
			this.cboSkipAmount.Leave += new System.EventHandler(this.cboSkipAmount_Leave);
			// 
			// cboSkipType
			// 
			this.cboSkipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSkipType.Location = new System.Drawing.Point(220, 120);
			this.cboSkipType.Name = "cboSkipType";
			this.cboSkipType.Size = new System.Drawing.Size(160, 21);
			this.cboSkipType.TabIndex = 42;
			this.cboSkipType.SelectedIndexChanged += new System.EventHandler(this.cboSkipType_SelectedIndexChanged);
			// 
			// cboSkipVar
			// 
			this.cboSkipVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSkipVar.Location = new System.Drawing.Point(44, 144);
			this.cboSkipVar.Name = "cboSkipVar";
			this.cboSkipVar.Size = new System.Drawing.Size(144, 21);
			this.cboSkipVar.TabIndex = 43;
			this.cboSkipVar.Leave += new System.EventHandler(this.cboSkipVar_Leave);
			// 
			// cboSkipTrig
			// 
			this.cboSkipTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSkipTrig.Location = new System.Drawing.Point(220, 144);
			this.cboSkipTrig.Name = "cboSkipTrig";
			this.cboSkipTrig.Size = new System.Drawing.Size(160, 21);
			this.cboSkipTrig.TabIndex = 44;
			this.cboSkipTrig.Leave += new System.EventHandler(this.cboSkipTrig_Leave);
			// 
			// label72
			// 
			this.label72.Location = new System.Drawing.Point(188, 144);
			this.label72.Name = "label72";
			this.label72.Size = new System.Drawing.Size(32, 16);
			this.label72.TabIndex = 39;
			this.label72.Text = "must";
			this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmdPasteSkip
			// 
			this.cmdPasteSkip.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdPasteSkip.ImageIndex = 7;
			this.cmdPasteSkip.ImageList = this.imgToolbar;
			this.cmdPasteSkip.Location = new System.Drawing.Point(8, 144);
			this.cmdPasteSkip.Name = "cmdPasteSkip";
			this.cmdPasteSkip.Size = new System.Drawing.Size(24, 23);
			this.cmdPasteSkip.TabIndex = 46;
			// 
			// optSkipAND
			// 
			this.optSkipAND.Location = new System.Drawing.Point(152, 56);
			this.optSkipAND.Name = "optSkipAND";
			this.optSkipAND.Size = new System.Drawing.Size(56, 24);
			this.optSkipAND.TabIndex = 23;
			this.optSkipAND.Text = "AND";
			// 
			// optSkipOR
			// 
			this.optSkipOR.Checked = true;
			this.optSkipOR.Location = new System.Drawing.Point(208, 56);
			this.optSkipOR.Name = "optSkipOR";
			this.optSkipOR.Size = new System.Drawing.Size(56, 24);
			this.optSkipOR.TabIndex = 24;
			this.optSkipOR.TabStop = true;
			this.optSkipOR.Text = "OR";
			this.optSkipOR.CheckedChanged += new System.EventHandler(this.optSkipOR_CheckedChanged);
			// 
			// lblSkipTrig1
			// 
			this.lblSkipTrig1.BackColor = System.Drawing.Color.RosyBrown;
			this.lblSkipTrig1.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lblSkipTrig1.Location = new System.Drawing.Point(16, 24);
			this.lblSkipTrig1.Name = "lblSkipTrig1";
			this.lblSkipTrig1.Size = new System.Drawing.Size(360, 32);
			this.lblSkipTrig1.TabIndex = 21;
			this.lblSkipTrig1.Text = "always (TRUE)";
			this.lblSkipTrig1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblSkipTrig1.Click += new System.EventHandler(this.lblSkipTrigArr_Click);
			this.lblSkipTrig1.DoubleClick += new System.EventHandler(this.lblSkipTrigArr_DoubleClick);
			this.lblSkipTrig1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblSkipTrigArr_MouseUp);
			// 
			// lblSkipTrig2
			// 
			this.lblSkipTrig2.BackColor = System.Drawing.Color.RosyBrown;
			this.lblSkipTrig2.Location = new System.Drawing.Point(16, 80);
			this.lblSkipTrig2.Name = "lblSkipTrig2";
			this.lblSkipTrig2.Size = new System.Drawing.Size(360, 32);
			this.lblSkipTrig2.TabIndex = 22;
			this.lblSkipTrig2.Text = "always (TRUE)";
			this.lblSkipTrig2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblSkipTrig2.Click += new System.EventHandler(this.lblSkipTrigArr_Click);
			this.lblSkipTrig2.DoubleClick += new System.EventHandler(this.lblSkipTrigArr_DoubleClick);
			this.lblSkipTrig2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblSkipTrigArr_MouseUp);
			// 
			// groupBox22
			// 
			this.groupBox22.Controls.Add(this.lblOptCraft1);
			this.groupBox22.Controls.Add(this.cboOptCraft);
			this.groupBox22.Controls.Add(this.label70);
			this.groupBox22.Controls.Add(this.numOptWaves);
			this.groupBox22.Controls.Add(this.label69);
			this.groupBox22.Controls.Add(this.label68);
			this.groupBox22.Controls.Add(this.cboOptCat);
			this.groupBox22.Controls.Add(this.numOptCraft);
			this.groupBox22.Controls.Add(this.lblOptCraft2);
			this.groupBox22.Controls.Add(this.lblOptCraft3);
			this.groupBox22.Controls.Add(this.lblOptCraft4);
			this.groupBox22.Controls.Add(this.lblOptCraft5);
			this.groupBox22.Controls.Add(this.lblOptCraft6);
			this.groupBox22.Controls.Add(this.lblOptCraft7);
			this.groupBox22.Controls.Add(this.lblOptCraft8);
			this.groupBox22.Controls.Add(this.lblOptCraft9);
			this.groupBox22.Controls.Add(this.lblOptCraft10);
			this.groupBox22.Location = new System.Drawing.Point(16, 8);
			this.groupBox22.Name = "groupBox22";
			this.groupBox22.Size = new System.Drawing.Size(232, 272);
			this.groupBox22.TabIndex = 3;
			this.groupBox22.TabStop = false;
			this.groupBox22.Text = "Craft";
			// 
			// lblOptCraft1
			// 
			this.lblOptCraft1.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOptCraft1.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lblOptCraft1.Location = new System.Drawing.Point(16, 104);
			this.lblOptCraft1.Name = "lblOptCraft1";
			this.lblOptCraft1.Size = new System.Drawing.Size(208, 16);
			this.lblOptCraft1.TabIndex = 6;
			this.lblOptCraft1.Text = "Craft 1:";
			// 
			// cboOptCraft
			// 
			this.cboOptCraft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOptCraft.Enabled = false;
			this.cboOptCraft.Location = new System.Drawing.Point(24, 72);
			this.cboOptCraft.Name = "cboOptCraft";
			this.cboOptCraft.Size = new System.Drawing.Size(192, 21);
			this.cboOptCraft.TabIndex = 5;
			this.cboOptCraft.Leave += new System.EventHandler(this.cboOptCraft_Leave);
			// 
			// label70
			// 
			this.label70.Location = new System.Drawing.Point(104, 48);
			this.label70.Name = "label70";
			this.label70.Size = new System.Drawing.Size(56, 16);
			this.label70.TabIndex = 4;
			this.label70.Text = "# of Craft";
			this.label70.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numOptWaves
			// 
			this.numOptWaves.Enabled = false;
			this.numOptWaves.Location = new System.Drawing.Point(56, 48);
			this.numOptWaves.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numOptWaves.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numOptWaves.Name = "numOptWaves";
			this.numOptWaves.Size = new System.Drawing.Size(48, 20);
			this.numOptWaves.TabIndex = 3;
			this.numOptWaves.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numOptWaves.Leave += new System.EventHandler(this.numOptWaves_Leave);
			// 
			// label69
			// 
			this.label69.Location = new System.Drawing.Point(8, 48);
			this.label69.Name = "label69";
			this.label69.Size = new System.Drawing.Size(40, 16);
			this.label69.TabIndex = 2;
			this.label69.Text = "Waves";
			this.label69.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label68
			// 
			this.label68.Location = new System.Drawing.Point(16, 16);
			this.label68.Name = "label68";
			this.label68.Size = new System.Drawing.Size(56, 16);
			this.label68.TabIndex = 1;
			this.label68.Text = "Category";
			this.label68.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// cboOptCat
			// 
			this.cboOptCat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOptCat.Items.AddRange(new object[] {
            "None",
            "All Flyable",
            "All Rebel Flyable",
            "All Imperial Flyable",
            "Custom"});
			this.cboOptCat.Location = new System.Drawing.Point(88, 16);
			this.cboOptCat.Name = "cboOptCat";
			this.cboOptCat.Size = new System.Drawing.Size(120, 21);
			this.cboOptCat.TabIndex = 0;
			this.cboOptCat.SelectedIndexChanged += new System.EventHandler(this.cboOptCat_SelectedIndexChanged);
			// 
			// numOptCraft
			// 
			this.numOptCraft.Enabled = false;
			this.numOptCraft.Location = new System.Drawing.Point(168, 48);
			this.numOptCraft.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numOptCraft.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numOptCraft.Name = "numOptCraft";
			this.numOptCraft.Size = new System.Drawing.Size(48, 20);
			this.numOptCraft.TabIndex = 3;
			this.numOptCraft.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numOptCraft.Leave += new System.EventHandler(this.numOptCraft_Leave);
			// 
			// lblOptCraft2
			// 
			this.lblOptCraft2.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOptCraft2.Location = new System.Drawing.Point(16, 120);
			this.lblOptCraft2.Name = "lblOptCraft2";
			this.lblOptCraft2.Size = new System.Drawing.Size(208, 16);
			this.lblOptCraft2.TabIndex = 6;
			this.lblOptCraft2.Text = "Craft 2:";
			// 
			// lblOptCraft3
			// 
			this.lblOptCraft3.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOptCraft3.Location = new System.Drawing.Point(16, 136);
			this.lblOptCraft3.Name = "lblOptCraft3";
			this.lblOptCraft3.Size = new System.Drawing.Size(208, 16);
			this.lblOptCraft3.TabIndex = 6;
			this.lblOptCraft3.Text = "Craft 3:";
			// 
			// lblOptCraft4
			// 
			this.lblOptCraft4.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOptCraft4.Location = new System.Drawing.Point(16, 152);
			this.lblOptCraft4.Name = "lblOptCraft4";
			this.lblOptCraft4.Size = new System.Drawing.Size(208, 16);
			this.lblOptCraft4.TabIndex = 6;
			this.lblOptCraft4.Text = "Craft 4:";
			// 
			// lblOptCraft5
			// 
			this.lblOptCraft5.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOptCraft5.Location = new System.Drawing.Point(16, 168);
			this.lblOptCraft5.Name = "lblOptCraft5";
			this.lblOptCraft5.Size = new System.Drawing.Size(208, 16);
			this.lblOptCraft5.TabIndex = 6;
			this.lblOptCraft5.Text = "Craft 5:";
			// 
			// lblOptCraft6
			// 
			this.lblOptCraft6.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOptCraft6.Location = new System.Drawing.Point(16, 184);
			this.lblOptCraft6.Name = "lblOptCraft6";
			this.lblOptCraft6.Size = new System.Drawing.Size(208, 16);
			this.lblOptCraft6.TabIndex = 6;
			this.lblOptCraft6.Text = "Craft 6:";
			// 
			// lblOptCraft7
			// 
			this.lblOptCraft7.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOptCraft7.Location = new System.Drawing.Point(16, 200);
			this.lblOptCraft7.Name = "lblOptCraft7";
			this.lblOptCraft7.Size = new System.Drawing.Size(208, 16);
			this.lblOptCraft7.TabIndex = 6;
			this.lblOptCraft7.Text = "Craft 7:";
			// 
			// lblOptCraft8
			// 
			this.lblOptCraft8.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOptCraft8.Location = new System.Drawing.Point(16, 216);
			this.lblOptCraft8.Name = "lblOptCraft8";
			this.lblOptCraft8.Size = new System.Drawing.Size(208, 16);
			this.lblOptCraft8.TabIndex = 6;
			this.lblOptCraft8.Text = "Craft 8:";
			// 
			// lblOptCraft9
			// 
			this.lblOptCraft9.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOptCraft9.Location = new System.Drawing.Point(16, 232);
			this.lblOptCraft9.Name = "lblOptCraft9";
			this.lblOptCraft9.Size = new System.Drawing.Size(208, 16);
			this.lblOptCraft9.TabIndex = 6;
			this.lblOptCraft9.Text = "Craft 9:";
			// 
			// lblOptCraft10
			// 
			this.lblOptCraft10.BackColor = System.Drawing.Color.RosyBrown;
			this.lblOptCraft10.Location = new System.Drawing.Point(16, 248);
			this.lblOptCraft10.Name = "lblOptCraft10";
			this.lblOptCraft10.Size = new System.Drawing.Size(208, 16);
			this.lblOptCraft10.TabIndex = 6;
			this.lblOptCraft10.Text = "Craft 10:";
			// 
			// groupBox21
			// 
			this.groupBox21.Controls.Add(this.chkOptCNone);
			this.groupBox21.Controls.Add(this.chkOptCChaff);
			this.groupBox21.Controls.Add(this.chkOptCFlare);
			this.groupBox21.Controls.Add(this.chkOptCCluster);
			this.groupBox21.Location = new System.Drawing.Point(406, 8);
			this.groupBox21.Name = "groupBox21";
			this.groupBox21.Size = new System.Drawing.Size(112, 86);
			this.groupBox21.TabIndex = 2;
			this.groupBox21.TabStop = false;
			this.groupBox21.Text = "Countermeasures";
			// 
			// chkOptCNone
			// 
			this.chkOptCNone.Checked = true;
			this.chkOptCNone.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkOptCNone.Location = new System.Drawing.Point(8, 16);
			this.chkOptCNone.Name = "chkOptCNone";
			this.chkOptCNone.Size = new System.Drawing.Size(96, 20);
			this.chkOptCNone.TabIndex = 6;
			this.chkOptCNone.Text = "None";
			// 
			// chkOptCChaff
			// 
			this.chkOptCChaff.Location = new System.Drawing.Point(8, 32);
			this.chkOptCChaff.Name = "chkOptCChaff";
			this.chkOptCChaff.Size = new System.Drawing.Size(96, 20);
			this.chkOptCChaff.TabIndex = 7;
			this.chkOptCChaff.Text = "Chaff";
			// 
			// chkOptCFlare
			// 
			this.chkOptCFlare.Location = new System.Drawing.Point(8, 48);
			this.chkOptCFlare.Name = "chkOptCFlare";
			this.chkOptCFlare.Size = new System.Drawing.Size(96, 20);
			this.chkOptCFlare.TabIndex = 8;
			this.chkOptCFlare.Text = "Flare";
			// 
			// chkOptCCluster
			// 
			this.chkOptCCluster.Location = new System.Drawing.Point(8, 64);
			this.chkOptCCluster.Name = "chkOptCCluster";
			this.chkOptCCluster.Size = new System.Drawing.Size(96, 20);
			this.chkOptCCluster.TabIndex = 9;
			this.chkOptCCluster.Text = "(Cluster Mine)";
			// 
			// groupBox20
			// 
			this.groupBox20.Controls.Add(this.chkOptBNone);
			this.groupBox20.Controls.Add(this.chkOptBTractor);
			this.groupBox20.Controls.Add(this.chkOptBJamming);
			this.groupBox20.Controls.Add(this.chkOptBDecoy);
			this.groupBox20.Controls.Add(this.chkOptBEnergy);
			this.groupBox20.Location = new System.Drawing.Point(262, 178);
			this.groupBox20.Name = "groupBox20";
			this.groupBox20.Size = new System.Drawing.Size(112, 102);
			this.groupBox20.TabIndex = 1;
			this.groupBox20.TabStop = false;
			this.groupBox20.Text = "Beams";
			// 
			// chkOptBNone
			// 
			this.chkOptBNone.Checked = true;
			this.chkOptBNone.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkOptBNone.Location = new System.Drawing.Point(8, 16);
			this.chkOptBNone.Name = "chkOptBNone";
			this.chkOptBNone.Size = new System.Drawing.Size(96, 20);
			this.chkOptBNone.TabIndex = 3;
			this.chkOptBNone.Text = "None";
			// 
			// chkOptBTractor
			// 
			this.chkOptBTractor.Location = new System.Drawing.Point(8, 32);
			this.chkOptBTractor.Name = "chkOptBTractor";
			this.chkOptBTractor.Size = new System.Drawing.Size(96, 20);
			this.chkOptBTractor.TabIndex = 4;
			this.chkOptBTractor.Text = "Tractor";
			// 
			// chkOptBJamming
			// 
			this.chkOptBJamming.Location = new System.Drawing.Point(8, 48);
			this.chkOptBJamming.Name = "chkOptBJamming";
			this.chkOptBJamming.Size = new System.Drawing.Size(96, 20);
			this.chkOptBJamming.TabIndex = 5;
			this.chkOptBJamming.Text = "Jamming";
			// 
			// chkOptBDecoy
			// 
			this.chkOptBDecoy.Location = new System.Drawing.Point(8, 64);
			this.chkOptBDecoy.Name = "chkOptBDecoy";
			this.chkOptBDecoy.Size = new System.Drawing.Size(96, 20);
			this.chkOptBDecoy.TabIndex = 6;
			this.chkOptBDecoy.Text = "Decoy";
			// 
			// chkOptBEnergy
			// 
			this.chkOptBEnergy.Location = new System.Drawing.Point(8, 80);
			this.chkOptBEnergy.Name = "chkOptBEnergy";
			this.chkOptBEnergy.Size = new System.Drawing.Size(96, 20);
			this.chkOptBEnergy.TabIndex = 7;
			this.chkOptBEnergy.Text = "(Energy)";
			// 
			// groupBox19
			// 
			this.groupBox19.Controls.Add(this.chkOptWNone);
			this.groupBox19.Controls.Add(this.chkOptWBomb);
			this.groupBox19.Controls.Add(this.chkOptWRocket);
			this.groupBox19.Controls.Add(this.chkOptWMissile);
			this.groupBox19.Controls.Add(this.chkOptWTorp);
			this.groupBox19.Controls.Add(this.chkOptWAdvMissile);
			this.groupBox19.Controls.Add(this.chkOptWAdvTorp);
			this.groupBox19.Controls.Add(this.chkOptWMagPulse);
			this.groupBox19.Controls.Add(this.chkOptWIonPulse);
			this.groupBox19.Location = new System.Drawing.Point(262, 8);
			this.groupBox19.Name = "groupBox19";
			this.groupBox19.Size = new System.Drawing.Size(112, 168);
			this.groupBox19.TabIndex = 0;
			this.groupBox19.TabStop = false;
			this.groupBox19.Text = "Warheads";
			// 
			// chkOptWNone
			// 
			this.chkOptWNone.Checked = true;
			this.chkOptWNone.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkOptWNone.Location = new System.Drawing.Point(8, 16);
			this.chkOptWNone.Name = "chkOptWNone";
			this.chkOptWNone.Size = new System.Drawing.Size(96, 20);
			this.chkOptWNone.TabIndex = 0;
			this.chkOptWNone.Text = "None";
			// 
			// chkOptWBomb
			// 
			this.chkOptWBomb.Location = new System.Drawing.Point(8, 32);
			this.chkOptWBomb.Name = "chkOptWBomb";
			this.chkOptWBomb.Size = new System.Drawing.Size(96, 20);
			this.chkOptWBomb.TabIndex = 0;
			this.chkOptWBomb.Text = "Space Bomb";
			// 
			// chkOptWRocket
			// 
			this.chkOptWRocket.Location = new System.Drawing.Point(8, 48);
			this.chkOptWRocket.Name = "chkOptWRocket";
			this.chkOptWRocket.Size = new System.Drawing.Size(96, 20);
			this.chkOptWRocket.TabIndex = 0;
			this.chkOptWRocket.Text = "Heavy Rocket";
			// 
			// chkOptWMissile
			// 
			this.chkOptWMissile.Location = new System.Drawing.Point(8, 64);
			this.chkOptWMissile.Name = "chkOptWMissile";
			this.chkOptWMissile.Size = new System.Drawing.Size(96, 20);
			this.chkOptWMissile.TabIndex = 0;
			this.chkOptWMissile.Text = "Conc. Missile";
			// 
			// chkOptWTorp
			// 
			this.chkOptWTorp.Location = new System.Drawing.Point(8, 80);
			this.chkOptWTorp.Name = "chkOptWTorp";
			this.chkOptWTorp.Size = new System.Drawing.Size(96, 20);
			this.chkOptWTorp.TabIndex = 0;
			this.chkOptWTorp.Text = "Torpedo";
			// 
			// chkOptWAdvMissile
			// 
			this.chkOptWAdvMissile.Location = new System.Drawing.Point(8, 96);
			this.chkOptWAdvMissile.Name = "chkOptWAdvMissile";
			this.chkOptWAdvMissile.Size = new System.Drawing.Size(96, 20);
			this.chkOptWAdvMissile.TabIndex = 0;
			this.chkOptWAdvMissile.Text = "Adv. Missile";
			// 
			// chkOptWAdvTorp
			// 
			this.chkOptWAdvTorp.Location = new System.Drawing.Point(8, 112);
			this.chkOptWAdvTorp.Name = "chkOptWAdvTorp";
			this.chkOptWAdvTorp.Size = new System.Drawing.Size(96, 20);
			this.chkOptWAdvTorp.TabIndex = 0;
			this.chkOptWAdvTorp.Text = "Adv. Torpedo";
			// 
			// chkOptWMagPulse
			// 
			this.chkOptWMagPulse.Location = new System.Drawing.Point(8, 128);
			this.chkOptWMagPulse.Name = "chkOptWMagPulse";
			this.chkOptWMagPulse.Size = new System.Drawing.Size(96, 20);
			this.chkOptWMagPulse.TabIndex = 0;
			this.chkOptWMagPulse.Text = "Mag Pulse";
			// 
			// chkOptWIonPulse
			// 
			this.chkOptWIonPulse.Location = new System.Drawing.Point(8, 144);
			this.chkOptWIonPulse.Name = "chkOptWIonPulse";
			this.chkOptWIonPulse.Size = new System.Drawing.Size(96, 20);
			this.chkOptWIonPulse.TabIndex = 0;
			this.chkOptWIonPulse.Text = "(Ion Pulse)";
			// 
			// tabUnk
			// 
			this.tabUnk.Controls.Add(this.grpUnkOther);
			this.tabUnk.Controls.Add(this.grpUnkOrder);
			this.tabUnk.Controls.Add(this.grpUnkAD);
			this.tabUnk.Controls.Add(this.grpUnkCraft);
			this.tabUnk.Controls.Add(this.grpUnkGoal);
			this.tabUnk.Location = new System.Drawing.Point(4, 22);
			this.tabUnk.Name = "tabUnk";
			this.tabUnk.Size = new System.Drawing.Size(544, 478);
			this.tabUnk.TabIndex = 5;
			this.tabUnk.Text = "Unknowns";
			// 
			// grpUnkOther
			// 
			this.grpUnkOther.Controls.Add(this.numUnk20);
			this.grpUnkOther.Controls.Add(this.label93);
			this.grpUnkOther.Controls.Add(this.chkUnk17);
			this.grpUnkOther.Controls.Add(this.chkUnk18);
			this.grpUnkOther.Controls.Add(this.chkUnk19);
			this.grpUnkOther.Controls.Add(this.numUnk21);
			this.grpUnkOther.Controls.Add(this.label94);
			this.grpUnkOther.Controls.Add(this.chkUnk23);
			this.grpUnkOther.Controls.Add(this.chkUnk22);
			this.grpUnkOther.Controls.Add(this.chkUnk24);
			this.grpUnkOther.Controls.Add(this.chkUnk25);
			this.grpUnkOther.Controls.Add(this.chkUnk28);
			this.grpUnkOther.Controls.Add(this.chkUnk29);
			this.grpUnkOther.Controls.Add(this.chkUnk26);
			this.grpUnkOther.Controls.Add(this.chkUnk27);
			this.grpUnkOther.Location = new System.Drawing.Point(8, 344);
			this.grpUnkOther.Name = "grpUnkOther";
			this.grpUnkOther.Size = new System.Drawing.Size(480, 104);
			this.grpUnkOther.TabIndex = 10;
			this.grpUnkOther.TabStop = false;
			this.grpUnkOther.Text = "Options/Other";
			this.grpUnkOther.Leave += new System.EventHandler(this.grpUnkOther_Leave);
			// 
			// numUnk20
			// 
			this.numUnk20.Enabled = false;
			this.numUnk20.Location = new System.Drawing.Point(224, 30);
			this.numUnk20.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk20.Name = "numUnk20";
			this.numUnk20.Size = new System.Drawing.Size(48, 20);
			this.numUnk20.TabIndex = 11;
			// 
			// label93
			// 
			this.label93.Enabled = false;
			this.label93.Location = new System.Drawing.Point(176, 30);
			this.label93.Name = "label93";
			this.label93.Size = new System.Drawing.Size(40, 16);
			this.label93.TabIndex = 12;
			this.label93.Text = "0x521";
			this.label93.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// chkUnk17
			// 
			this.chkUnk17.Location = new System.Drawing.Point(8, 24);
			this.chkUnk17.Name = "chkUnk17";
			this.chkUnk17.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk17.Size = new System.Drawing.Size(56, 32);
			this.chkUnk17.TabIndex = 10;
			this.chkUnk17.Text = "0x516";
			// 
			// chkUnk18
			// 
			this.chkUnk18.Location = new System.Drawing.Point(64, 24);
			this.chkUnk18.Name = "chkUnk18";
			this.chkUnk18.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk18.Size = new System.Drawing.Size(56, 32);
			this.chkUnk18.TabIndex = 10;
			this.chkUnk18.Text = "0x518";
			// 
			// chkUnk19
			// 
			this.chkUnk19.Enabled = false;
			this.chkUnk19.Location = new System.Drawing.Point(120, 24);
			this.chkUnk19.Name = "chkUnk19";
			this.chkUnk19.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk19.Size = new System.Drawing.Size(56, 32);
			this.chkUnk19.TabIndex = 10;
			this.chkUnk19.Text = "0x520";
			// 
			// numUnk21
			// 
			this.numUnk21.Enabled = false;
			this.numUnk21.Location = new System.Drawing.Point(320, 30);
			this.numUnk21.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk21.Name = "numUnk21";
			this.numUnk21.Size = new System.Drawing.Size(48, 20);
			this.numUnk21.TabIndex = 11;
			// 
			// label94
			// 
			this.label94.Enabled = false;
			this.label94.Location = new System.Drawing.Point(272, 30);
			this.label94.Name = "label94";
			this.label94.Size = new System.Drawing.Size(40, 16);
			this.label94.TabIndex = 12;
			this.label94.Text = "0x522";
			this.label94.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// chkUnk23
			// 
			this.chkUnk23.Location = new System.Drawing.Point(64, 64);
			this.chkUnk23.Name = "chkUnk23";
			this.chkUnk23.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk23.Size = new System.Drawing.Size(56, 32);
			this.chkUnk23.TabIndex = 10;
			this.chkUnk23.Text = "0x528";
			// 
			// chkUnk22
			// 
			this.chkUnk22.Location = new System.Drawing.Point(8, 64);
			this.chkUnk22.Name = "chkUnk22";
			this.chkUnk22.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk22.Size = new System.Drawing.Size(56, 32);
			this.chkUnk22.TabIndex = 10;
			this.chkUnk22.Text = "0x527";
			// 
			// chkUnk24
			// 
			this.chkUnk24.Location = new System.Drawing.Point(120, 64);
			this.chkUnk24.Name = "chkUnk24";
			this.chkUnk24.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk24.Size = new System.Drawing.Size(56, 32);
			this.chkUnk24.TabIndex = 10;
			this.chkUnk24.Text = "0x529";
			// 
			// chkUnk25
			// 
			this.chkUnk25.Location = new System.Drawing.Point(176, 64);
			this.chkUnk25.Name = "chkUnk25";
			this.chkUnk25.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk25.Size = new System.Drawing.Size(56, 32);
			this.chkUnk25.TabIndex = 10;
			this.chkUnk25.Text = "0x52A";
			// 
			// chkUnk28
			// 
			this.chkUnk28.Location = new System.Drawing.Point(344, 64);
			this.chkUnk28.Name = "chkUnk28";
			this.chkUnk28.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk28.Size = new System.Drawing.Size(64, 32);
			this.chkUnk28.TabIndex = 10;
			this.chkUnk28.Text = "0x52D";
			// 
			// chkUnk29
			// 
			this.chkUnk29.Location = new System.Drawing.Point(414, 64);
			this.chkUnk29.Name = "chkUnk29";
			this.chkUnk29.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk29.Size = new System.Drawing.Size(56, 32);
			this.chkUnk29.TabIndex = 10;
			this.chkUnk29.Text = "0x52E";
			// 
			// chkUnk26
			// 
			this.chkUnk26.Location = new System.Drawing.Point(232, 64);
			this.chkUnk26.Name = "chkUnk26";
			this.chkUnk26.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk26.Size = new System.Drawing.Size(56, 32);
			this.chkUnk26.TabIndex = 10;
			this.chkUnk26.Text = "0x52B";
			// 
			// chkUnk27
			// 
			this.chkUnk27.Location = new System.Drawing.Point(288, 64);
			this.chkUnk27.Name = "chkUnk27";
			this.chkUnk27.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk27.Size = new System.Drawing.Size(56, 32);
			this.chkUnk27.TabIndex = 10;
			this.chkUnk27.Text = "0x52C";
			// 
			// grpUnkOrder
			// 
			this.grpUnkOrder.Controls.Add(this.numUnk6);
			this.grpUnkOrder.Controls.Add(this.label81);
			this.grpUnkOrder.Controls.Add(this.numUnkOrder);
			this.grpUnkOrder.Controls.Add(this.label88);
			this.grpUnkOrder.Controls.Add(this.numUnk7);
			this.grpUnkOrder.Controls.Add(this.label84);
			this.grpUnkOrder.Controls.Add(this.numUnk8);
			this.grpUnkOrder.Controls.Add(this.label89);
			this.grpUnkOrder.Controls.Add(this.numUnk9);
			this.grpUnkOrder.Controls.Add(this.label90);
			this.grpUnkOrder.Location = new System.Drawing.Point(8, 152);
			this.grpUnkOrder.Name = "grpUnkOrder";
			this.grpUnkOrder.Size = new System.Drawing.Size(408, 80);
			this.grpUnkOrder.TabIndex = 9;
			this.grpUnkOrder.TabStop = false;
			this.grpUnkOrder.Text = "Orders";
			this.grpUnkOrder.Leave += new System.EventHandler(this.grpUnkOrder_Leave);
			// 
			// numUnk6
			// 
			this.numUnk6.Location = new System.Drawing.Point(64, 48);
			this.numUnk6.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk6.Name = "numUnk6";
			this.numUnk6.Size = new System.Drawing.Size(48, 20);
			this.numUnk6.TabIndex = 7;
			// 
			// label81
			// 
			this.label81.Location = new System.Drawing.Point(8, 48);
			this.label81.Name = "label81";
			this.label81.Size = new System.Drawing.Size(40, 16);
			this.label81.TabIndex = 8;
			this.label81.Text = "(0x4)";
			this.label81.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numUnkOrder
			// 
			this.numUnkOrder.Location = new System.Drawing.Point(56, 16);
			this.numUnkOrder.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.numUnkOrder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numUnkOrder.Name = "numUnkOrder";
			this.numUnkOrder.Size = new System.Drawing.Size(40, 20);
			this.numUnkOrder.TabIndex = 6;
			this.numUnkOrder.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numUnkOrder.ValueChanged += new System.EventHandler(this.numUnkOrder_ValueChanged);
			this.numUnkOrder.Enter += new System.EventHandler(this.numUnkOrder_Enter);
			// 
			// label88
			// 
			this.label88.Location = new System.Drawing.Point(8, 16);
			this.label88.Name = "label88";
			this.label88.Size = new System.Drawing.Size(40, 16);
			this.label88.TabIndex = 5;
			this.label88.Text = "Order";
			this.label88.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// numUnk7
			// 
			this.numUnk7.Location = new System.Drawing.Point(160, 48);
			this.numUnk7.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk7.Name = "numUnk7";
			this.numUnk7.Size = new System.Drawing.Size(48, 20);
			this.numUnk7.TabIndex = 7;
			// 
			// label84
			// 
			this.label84.Location = new System.Drawing.Point(112, 48);
			this.label84.Name = "label84";
			this.label84.Size = new System.Drawing.Size(40, 16);
			this.label84.TabIndex = 8;
			this.label84.Text = "(0x5)";
			this.label84.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numUnk8
			// 
			this.numUnk8.Location = new System.Drawing.Point(256, 48);
			this.numUnk8.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk8.Name = "numUnk8";
			this.numUnk8.Size = new System.Drawing.Size(48, 20);
			this.numUnk8.TabIndex = 7;
			// 
			// label89
			// 
			this.label89.Location = new System.Drawing.Point(208, 48);
			this.label89.Name = "label89";
			this.label89.Size = new System.Drawing.Size(40, 16);
			this.label89.TabIndex = 8;
			this.label89.Text = "(0xB)";
			this.label89.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numUnk9
			// 
			this.numUnk9.Location = new System.Drawing.Point(352, 48);
			this.numUnk9.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk9.Name = "numUnk9";
			this.numUnk9.Size = new System.Drawing.Size(48, 20);
			this.numUnk9.TabIndex = 7;
			// 
			// label90
			// 
			this.label90.Location = new System.Drawing.Point(304, 48);
			this.label90.Name = "label90";
			this.label90.Size = new System.Drawing.Size(40, 16);
			this.label90.TabIndex = 8;
			this.label90.Text = "(0x11)";
			this.label90.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// grpUnkAD
			// 
			this.grpUnkAD.Controls.Add(this.numUnk5);
			this.grpUnkAD.Controls.Add(this.label87);
			this.grpUnkAD.Controls.Add(this.numUnk4);
			this.grpUnkAD.Controls.Add(this.label86);
			this.grpUnkAD.Controls.Add(this.numUnk3);
			this.grpUnkAD.Controls.Add(this.label85);
			this.grpUnkAD.Location = new System.Drawing.Point(8, 80);
			this.grpUnkAD.Name = "grpUnkAD";
			this.grpUnkAD.Size = new System.Drawing.Size(312, 56);
			this.grpUnkAD.TabIndex = 8;
			this.grpUnkAD.TabStop = false;
			this.grpUnkAD.Text = "Arr/Dep";
			this.grpUnkAD.Leave += new System.EventHandler(this.grpUnkAD_Leave);
			// 
			// numUnk5
			// 
			this.numUnk5.Location = new System.Drawing.Point(256, 24);
			this.numUnk5.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk5.Name = "numUnk5";
			this.numUnk5.Size = new System.Drawing.Size(48, 20);
			this.numUnk5.TabIndex = 1;
			// 
			// label87
			// 
			this.label87.Location = new System.Drawing.Point(208, 24);
			this.label87.Name = "label87";
			this.label87.Size = new System.Drawing.Size(40, 16);
			this.label87.TabIndex = 3;
			this.label87.Text = "0x98";
			this.label87.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numUnk4
			// 
			this.numUnk4.Location = new System.Drawing.Point(160, 24);
			this.numUnk4.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk4.Name = "numUnk4";
			this.numUnk4.Size = new System.Drawing.Size(48, 20);
			this.numUnk4.TabIndex = 1;
			// 
			// label86
			// 
			this.label86.Location = new System.Drawing.Point(112, 24);
			this.label86.Name = "label86";
			this.label86.Size = new System.Drawing.Size(40, 16);
			this.label86.TabIndex = 3;
			this.label86.Text = "0x96";
			this.label86.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numUnk3
			// 
			this.numUnk3.Location = new System.Drawing.Point(64, 24);
			this.numUnk3.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk3.Name = "numUnk3";
			this.numUnk3.Size = new System.Drawing.Size(48, 20);
			this.numUnk3.TabIndex = 1;
			// 
			// label85
			// 
			this.label85.Location = new System.Drawing.Point(16, 24);
			this.label85.Name = "label85";
			this.label85.Size = new System.Drawing.Size(40, 16);
			this.label85.TabIndex = 3;
			this.label85.Text = "0x85";
			this.label85.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// grpUnkCraft
			// 
			this.grpUnkCraft.Controls.Add(this.numUnk1);
			this.grpUnkCraft.Controls.Add(this.label83);
			this.grpUnkCraft.Controls.Add(this.chkUnk2);
			this.grpUnkCraft.Location = new System.Drawing.Point(8, 8);
			this.grpUnkCraft.Name = "grpUnkCraft";
			this.grpUnkCraft.Size = new System.Drawing.Size(184, 56);
			this.grpUnkCraft.TabIndex = 7;
			this.grpUnkCraft.TabStop = false;
			this.grpUnkCraft.Text = "Craft";
			this.grpUnkCraft.Leave += new System.EventHandler(this.grpUnkCraft_Leave);
			// 
			// numUnk1
			// 
			this.numUnk1.Location = new System.Drawing.Point(56, 22);
			this.numUnk1.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk1.Name = "numUnk1";
			this.numUnk1.Size = new System.Drawing.Size(48, 20);
			this.numUnk1.TabIndex = 1;
			// 
			// label83
			// 
			this.label83.Location = new System.Drawing.Point(8, 22);
			this.label83.Name = "label83";
			this.label83.Size = new System.Drawing.Size(40, 16);
			this.label83.TabIndex = 3;
			this.label83.Text = "0x62";
			this.label83.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// chkUnk2
			// 
			this.chkUnk2.Location = new System.Drawing.Point(120, 17);
			this.chkUnk2.Name = "chkUnk2";
			this.chkUnk2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk2.Size = new System.Drawing.Size(56, 32);
			this.chkUnk2.TabIndex = 2;
			this.chkUnk2.Text = "0x63";
			// 
			// grpUnkGoal
			// 
			this.grpUnkGoal.Controls.Add(this.chkUnk10);
			this.grpUnkGoal.Controls.Add(this.numUnkGoal);
			this.grpUnkGoal.Controls.Add(this.label92);
			this.grpUnkGoal.Controls.Add(this.numUnk13);
			this.grpUnkGoal.Controls.Add(this.label95);
			this.grpUnkGoal.Controls.Add(this.chkUnk11);
			this.grpUnkGoal.Controls.Add(this.chkUnk12);
			this.grpUnkGoal.Controls.Add(this.chkUnk14);
			this.grpUnkGoal.Controls.Add(this.numUnk16);
			this.grpUnkGoal.Controls.Add(this.label91);
			this.grpUnkGoal.Controls.Add(this.chkUnk15);
			this.grpUnkGoal.Location = new System.Drawing.Point(8, 248);
			this.grpUnkGoal.Name = "grpUnkGoal";
			this.grpUnkGoal.Size = new System.Drawing.Size(496, 80);
			this.grpUnkGoal.TabIndex = 9;
			this.grpUnkGoal.TabStop = false;
			this.grpUnkGoal.Text = "Goals";
			this.grpUnkGoal.Leave += new System.EventHandler(this.grpUnkGoal_Leave);
			// 
			// chkUnk10
			// 
			this.chkUnk10.Location = new System.Drawing.Point(8, 40);
			this.chkUnk10.Name = "chkUnk10";
			this.chkUnk10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk10.Size = new System.Drawing.Size(56, 32);
			this.chkUnk10.TabIndex = 9;
			this.chkUnk10.Text = "(0x6)";
			// 
			// numUnkGoal
			// 
			this.numUnkGoal.Location = new System.Drawing.Point(56, 16);
			this.numUnkGoal.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
			this.numUnkGoal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numUnkGoal.Name = "numUnkGoal";
			this.numUnkGoal.Size = new System.Drawing.Size(40, 20);
			this.numUnkGoal.TabIndex = 6;
			this.numUnkGoal.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numUnkGoal.ValueChanged += new System.EventHandler(this.numUnkGoal_ValueChanged);
			this.numUnkGoal.Enter += new System.EventHandler(this.numUnkGoal_Enter);
			// 
			// label92
			// 
			this.label92.Location = new System.Drawing.Point(8, 16);
			this.label92.Name = "label92";
			this.label92.Size = new System.Drawing.Size(40, 16);
			this.label92.TabIndex = 5;
			this.label92.Text = "Goal";
			this.label92.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// numUnk13
			// 
			this.numUnk13.Location = new System.Drawing.Point(224, 46);
			this.numUnk13.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk13.Name = "numUnk13";
			this.numUnk13.Size = new System.Drawing.Size(48, 20);
			this.numUnk13.TabIndex = 7;
			// 
			// label95
			// 
			this.label95.Location = new System.Drawing.Point(176, 46);
			this.label95.Name = "label95";
			this.label95.Size = new System.Drawing.Size(40, 16);
			this.label95.TabIndex = 8;
			this.label95.Text = "(0xB)";
			this.label95.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// chkUnk11
			// 
			this.chkUnk11.Location = new System.Drawing.Point(64, 40);
			this.chkUnk11.Name = "chkUnk11";
			this.chkUnk11.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk11.Size = new System.Drawing.Size(56, 32);
			this.chkUnk11.TabIndex = 9;
			this.chkUnk11.Text = "(0x7)";
			// 
			// chkUnk12
			// 
			this.chkUnk12.Location = new System.Drawing.Point(120, 40);
			this.chkUnk12.Name = "chkUnk12";
			this.chkUnk12.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk12.Size = new System.Drawing.Size(56, 32);
			this.chkUnk12.TabIndex = 9;
			this.chkUnk12.Text = "(0x8)";
			// 
			// chkUnk14
			// 
			this.chkUnk14.Location = new System.Drawing.Point(272, 40);
			this.chkUnk14.Name = "chkUnk14";
			this.chkUnk14.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk14.Size = new System.Drawing.Size(56, 32);
			this.chkUnk14.TabIndex = 9;
			this.chkUnk14.Text = "(0xC)";
			// 
			// numUnk16
			// 
			this.numUnk16.Enabled = false;
			this.numUnk16.Location = new System.Drawing.Point(432, 46);
			this.numUnk16.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numUnk16.Name = "numUnk16";
			this.numUnk16.Size = new System.Drawing.Size(48, 20);
			this.numUnk16.TabIndex = 7;
			// 
			// label91
			// 
			this.label91.Location = new System.Drawing.Point(384, 46);
			this.label91.Name = "label91";
			this.label91.Size = new System.Drawing.Size(40, 16);
			this.label91.TabIndex = 8;
			this.label91.Text = "(0xE)";
			this.label91.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// chkUnk15
			// 
			this.chkUnk15.Enabled = false;
			this.chkUnk15.Location = new System.Drawing.Point(328, 40);
			this.chkUnk15.Name = "chkUnk15";
			this.chkUnk15.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkUnk15.Size = new System.Drawing.Size(56, 32);
			this.chkUnk15.TabIndex = 9;
			this.chkUnk15.Text = "(0xD)";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(208, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Tm - GG  - waves x craft (GU)";
			// 
			// lstFG
			// 
			this.lstFG.BackColor = System.Drawing.Color.Black;
			this.lstFG.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.lstFG.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstFG.ForeColor = System.Drawing.Color.Gray;
			this.lstFG.ItemHeight = 15;
			this.lstFG.Items.AddRange(new object[] {
            "3 - 12 - *1x(3) Ship name"});
			this.lstFG.Location = new System.Drawing.Point(8, 24);
			this.lstFG.Name = "lstFG";
			this.lstFG.Size = new System.Drawing.Size(216, 480);
			this.lstFG.TabIndex = 3;
			this.lstFG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstFG_DrawItem);
			this.lstFG.SelectedIndexChanged += new System.EventHandler(this.lstFG_SelectedIndexChanged);
			// 
			// tabMess
			// 
			this.tabMess.Controls.Add(this.cmdMoveMessDown);
			this.tabMess.Controls.Add(this.cmdMoveMessUp);
			this.tabMess.Controls.Add(this.cboMessColor);
			this.tabMess.Controls.Add(this.label109);
			this.tabMess.Controls.Add(this.cboMessAmount);
			this.tabMess.Controls.Add(this.cboMessType);
			this.tabMess.Controls.Add(this.cboMessVar);
			this.tabMess.Controls.Add(this.cboMessTrig);
			this.tabMess.Controls.Add(this.label110);
			this.tabMess.Controls.Add(this.grpMessages);
			this.tabMess.Controls.Add(this.numMessDelay);
			this.tabMess.Controls.Add(this.label55);
			this.tabMess.Controls.Add(this.lblMessage);
			this.tabMess.Controls.Add(this.txtMessage);
			this.tabMess.Controls.Add(this.label52);
			this.tabMess.Controls.Add(this.label53);
			this.tabMess.Controls.Add(this.txtShort);
			this.tabMess.Controls.Add(this.lstMessages);
			this.tabMess.Controls.Add(this.grpSend);
			this.tabMess.Location = new System.Drawing.Point(4, 22);
			this.tabMess.Name = "tabMess";
			this.tabMess.Size = new System.Drawing.Size(785, 510);
			this.tabMess.TabIndex = 1;
			this.tabMess.Text = "Messages";
			// 
			// cmdMoveMessDown
			// 
			this.cmdMoveMessDown.Location = new System.Drawing.Point(552, 9);
			this.cmdMoveMessDown.Name = "cmdMoveMessDown";
			this.cmdMoveMessDown.Size = new System.Drawing.Size(75, 23);
			this.cmdMoveMessDown.TabIndex = 37;
			this.cmdMoveMessDown.Text = "Move Down";
			this.cmdMoveMessDown.UseVisualStyleBackColor = true;
			this.cmdMoveMessDown.Click += new System.EventHandler(this.cmdMoveMessDown_Click);
			// 
			// cmdMoveMessUp
			// 
			this.cmdMoveMessUp.Location = new System.Drawing.Point(470, 9);
			this.cmdMoveMessUp.Name = "cmdMoveMessUp";
			this.cmdMoveMessUp.Size = new System.Drawing.Size(75, 23);
			this.cmdMoveMessUp.TabIndex = 37;
			this.cmdMoveMessUp.Text = "Move Up";
			this.cmdMoveMessUp.UseVisualStyleBackColor = true;
			this.cmdMoveMessUp.Click += new System.EventHandler(this.cmdMoveMessUp_Click);
			// 
			// cboMessColor
			// 
			this.cboMessColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMessColor.Enabled = false;
			this.cboMessColor.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Yellow"});
			this.cboMessColor.Location = new System.Drawing.Point(656, 16);
			this.cboMessColor.Name = "cboMessColor";
			this.cboMessColor.Size = new System.Drawing.Size(120, 21);
			this.cboMessColor.TabIndex = 36;
			this.cboMessColor.SelectedIndexChanged += new System.EventHandler(this.cboMessColor_SelectedIndexChanged);
			// 
			// label109
			// 
			this.label109.Location = new System.Drawing.Point(552, 304);
			this.label109.Name = "label109";
			this.label109.Size = new System.Drawing.Size(16, 16);
			this.label109.TabIndex = 35;
			this.label109.Text = "of";
			this.label109.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cboMessAmount
			// 
			this.cboMessAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMessAmount.Enabled = false;
			this.cboMessAmount.Location = new System.Drawing.Point(400, 304);
			this.cboMessAmount.Name = "cboMessAmount";
			this.cboMessAmount.Size = new System.Drawing.Size(144, 21);
			this.cboMessAmount.TabIndex = 30;
			this.cboMessAmount.Leave += new System.EventHandler(this.cboMessAmount_Leave);
			// 
			// cboMessType
			// 
			this.cboMessType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMessType.Enabled = false;
			this.cboMessType.Location = new System.Drawing.Point(576, 304);
			this.cboMessType.Name = "cboMessType";
			this.cboMessType.Size = new System.Drawing.Size(160, 21);
			this.cboMessType.TabIndex = 31;
			this.cboMessType.SelectedIndexChanged += new System.EventHandler(this.cboMessType_SelectedIndexChanged);
			// 
			// cboMessVar
			// 
			this.cboMessVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMessVar.Enabled = false;
			this.cboMessVar.Location = new System.Drawing.Point(400, 336);
			this.cboMessVar.Name = "cboMessVar";
			this.cboMessVar.Size = new System.Drawing.Size(144, 21);
			this.cboMessVar.TabIndex = 32;
			this.cboMessVar.Leave += new System.EventHandler(this.cboMessVar_Leave);
			// 
			// cboMessTrig
			// 
			this.cboMessTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMessTrig.Enabled = false;
			this.cboMessTrig.Location = new System.Drawing.Point(576, 336);
			this.cboMessTrig.Name = "cboMessTrig";
			this.cboMessTrig.Size = new System.Drawing.Size(160, 21);
			this.cboMessTrig.TabIndex = 33;
			this.cboMessTrig.Leave += new System.EventHandler(this.cboMessTrig_Leave);
			// 
			// label110
			// 
			this.label110.Location = new System.Drawing.Point(544, 336);
			this.label110.Name = "label110";
			this.label110.Size = new System.Drawing.Size(32, 16);
			this.label110.TabIndex = 34;
			this.label110.Text = "must";
			this.label110.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// grpMessages
			// 
			this.grpMessages.Controls.Add(this.panel8);
			this.grpMessages.Controls.Add(this.panel7);
			this.grpMessages.Controls.Add(this.lblMess1);
			this.grpMessages.Controls.Add(this.lblMess2);
			this.grpMessages.Controls.Add(this.lblMess4);
			this.grpMessages.Controls.Add(this.lblMess3);
			this.grpMessages.Controls.Add(this.optMess12AND34);
			this.grpMessages.Controls.Add(this.optMess12OR34);
			this.grpMessages.Enabled = false;
			this.grpMessages.Location = new System.Drawing.Point(360, 104);
			this.grpMessages.Name = "grpMessages";
			this.grpMessages.Size = new System.Drawing.Size(408, 184);
			this.grpMessages.TabIndex = 27;
			this.grpMessages.TabStop = false;
			// 
			// panel8
			// 
			this.panel8.Controls.Add(this.optMess3OR4);
			this.panel8.Controls.Add(this.optMess3AND4);
			this.panel8.Location = new System.Drawing.Point(344, 112);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(56, 56);
			this.panel8.TabIndex = 8;
			// 
			// optMess3OR4
			// 
			this.optMess3OR4.Checked = true;
			this.optMess3OR4.Location = new System.Drawing.Point(8, 32);
			this.optMess3OR4.Name = "optMess3OR4";
			this.optMess3OR4.Size = new System.Drawing.Size(48, 24);
			this.optMess3OR4.TabIndex = 4;
			this.optMess3OR4.TabStop = true;
			this.optMess3OR4.Text = "OR";
			this.optMess3OR4.CheckedChanged += new System.EventHandler(this.optMess3OR4_CheckedChanged);
			// 
			// optMess3AND4
			// 
			this.optMess3AND4.Location = new System.Drawing.Point(8, 8);
			this.optMess3AND4.Name = "optMess3AND4";
			this.optMess3AND4.Size = new System.Drawing.Size(48, 24);
			this.optMess3AND4.TabIndex = 3;
			this.optMess3AND4.Text = "AND";
			// 
			// panel7
			// 
			this.panel7.Controls.Add(this.optMess1OR2);
			this.panel7.Controls.Add(this.optMess1AND2);
			this.panel7.Location = new System.Drawing.Point(344, 24);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(56, 56);
			this.panel7.TabIndex = 7;
			// 
			// optMess1OR2
			// 
			this.optMess1OR2.Checked = true;
			this.optMess1OR2.Location = new System.Drawing.Point(8, 32);
			this.optMess1OR2.Name = "optMess1OR2";
			this.optMess1OR2.Size = new System.Drawing.Size(48, 24);
			this.optMess1OR2.TabIndex = 4;
			this.optMess1OR2.TabStop = true;
			this.optMess1OR2.Text = "OR";
			this.optMess1OR2.CheckedChanged += new System.EventHandler(this.optMess1OR2_CheckedChanged);
			// 
			// optMess1AND2
			// 
			this.optMess1AND2.Location = new System.Drawing.Point(8, 8);
			this.optMess1AND2.Name = "optMess1AND2";
			this.optMess1AND2.Size = new System.Drawing.Size(48, 24);
			this.optMess1AND2.TabIndex = 3;
			this.optMess1AND2.Text = "AND";
			// 
			// lblMess1
			// 
			this.lblMess1.BackColor = System.Drawing.Color.RosyBrown;
			this.lblMess1.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lblMess1.Location = new System.Drawing.Point(16, 24);
			this.lblMess1.Name = "lblMess1";
			this.lblMess1.Size = new System.Drawing.Size(328, 32);
			this.lblMess1.TabIndex = 6;
			this.lblMess1.Text = "always (TRUE)";
			this.lblMess1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMess2
			// 
			this.lblMess2.BackColor = System.Drawing.Color.RosyBrown;
			this.lblMess2.Location = new System.Drawing.Point(16, 56);
			this.lblMess2.Name = "lblMess2";
			this.lblMess2.Size = new System.Drawing.Size(328, 32);
			this.lblMess2.TabIndex = 5;
			this.lblMess2.Text = "always (TRUE)";
			this.lblMess2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMess4
			// 
			this.lblMess4.BackColor = System.Drawing.Color.RosyBrown;
			this.lblMess4.Location = new System.Drawing.Point(16, 144);
			this.lblMess4.Name = "lblMess4";
			this.lblMess4.Size = new System.Drawing.Size(328, 32);
			this.lblMess4.TabIndex = 5;
			this.lblMess4.Text = "always (TRUE)";
			this.lblMess4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMess3
			// 
			this.lblMess3.BackColor = System.Drawing.Color.RosyBrown;
			this.lblMess3.Location = new System.Drawing.Point(16, 112);
			this.lblMess3.Name = "lblMess3";
			this.lblMess3.Size = new System.Drawing.Size(328, 32);
			this.lblMess3.TabIndex = 6;
			this.lblMess3.Text = "always (TRUE)";
			this.lblMess3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// optMess12AND34
			// 
			this.optMess12AND34.Location = new System.Drawing.Point(136, 88);
			this.optMess12AND34.Name = "optMess12AND34";
			this.optMess12AND34.Size = new System.Drawing.Size(48, 24);
			this.optMess12AND34.TabIndex = 3;
			this.optMess12AND34.Text = "AND";
			// 
			// optMess12OR34
			// 
			this.optMess12OR34.Checked = true;
			this.optMess12OR34.Location = new System.Drawing.Point(192, 88);
			this.optMess12OR34.Name = "optMess12OR34";
			this.optMess12OR34.Size = new System.Drawing.Size(48, 24);
			this.optMess12OR34.TabIndex = 4;
			this.optMess12OR34.TabStop = true;
			this.optMess12OR34.Text = "OR";
			this.optMess12OR34.CheckedChanged += new System.EventHandler(this.optMess12OR34_CheckedChanged);
			// 
			// numMessDelay
			// 
			this.numMessDelay.Enabled = false;
			this.numMessDelay.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numMessDelay.Location = new System.Drawing.Point(664, 72);
			this.numMessDelay.Maximum = new decimal(new int[] {
            1275,
            0,
            0,
            0});
			this.numMessDelay.Name = "numMessDelay";
			this.numMessDelay.Size = new System.Drawing.Size(48, 20);
			this.numMessDelay.TabIndex = 25;
			this.numMessDelay.Leave += new System.EventHandler(this.numMessDelay_Leave);
			// 
			// label55
			// 
			this.label55.Location = new System.Drawing.Point(624, 72);
			this.label55.Name = "label55";
			this.label55.Size = new System.Drawing.Size(136, 16);
			this.label55.TabIndex = 26;
			this.label55.Text = "Delay:                   seconds";
			this.label55.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lblMessage
			// 
			this.lblMessage.Location = new System.Drawing.Point(344, 16);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(120, 16);
			this.lblMessage.TabIndex = 24;
			this.lblMessage.Text = "Message #0 of 0";
			// 
			// txtMessage
			// 
			this.txtMessage.Enabled = false;
			this.txtMessage.Location = new System.Drawing.Point(416, 40);
			this.txtMessage.MaxLength = 64;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(360, 20);
			this.txtMessage.TabIndex = 20;
			this.txtMessage.Leave += new System.EventHandler(this.txtMessage_Leave);
			// 
			// label52
			// 
			this.label52.Location = new System.Drawing.Point(344, 40);
			this.label52.Name = "label52";
			this.label52.Size = new System.Drawing.Size(56, 16);
			this.label52.TabIndex = 23;
			this.label52.Text = "Message";
			this.label52.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label53
			// 
			this.label53.Location = new System.Drawing.Point(344, 72);
			this.label53.Name = "label53";
			this.label53.Size = new System.Drawing.Size(104, 24);
			this.label53.TabIndex = 21;
			this.label53.Text = "Notes\t\t\t       (not used in game)";
			this.label53.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// txtShort
			// 
			this.txtShort.Enabled = false;
			this.txtShort.Location = new System.Drawing.Point(448, 72);
			this.txtShort.MaxLength = 15;
			this.txtShort.Name = "txtShort";
			this.txtShort.Size = new System.Drawing.Size(104, 20);
			this.txtShort.TabIndex = 22;
			this.txtShort.Leave += new System.EventHandler(this.txtShort_Leave);
			// 
			// lstMessages
			// 
			this.lstMessages.BackColor = System.Drawing.Color.Black;
			this.lstMessages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.lstMessages.ForeColor = System.Drawing.Color.Gray;
			this.lstMessages.ItemHeight = 15;
			this.lstMessages.Location = new System.Drawing.Point(8, 8);
			this.lstMessages.Name = "lstMessages";
			this.lstMessages.Size = new System.Drawing.Size(320, 500);
			this.lstMessages.TabIndex = 1;
			this.lstMessages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstMessages_DrawItem);
			this.lstMessages.SelectedIndexChanged += new System.EventHandler(this.lstMessages_SelectedIndexChanged);
			// 
			// grpSend
			// 
			this.grpSend.Controls.Add(this.chkMess1);
			this.grpSend.Controls.Add(this.chkMess2);
			this.grpSend.Controls.Add(this.chkMess3);
			this.grpSend.Controls.Add(this.chkMess4);
			this.grpSend.Controls.Add(this.chkMess5);
			this.grpSend.Controls.Add(this.chkMess10);
			this.grpSend.Controls.Add(this.chkMess9);
			this.grpSend.Controls.Add(this.chkMess8);
			this.grpSend.Controls.Add(this.chkMess7);
			this.grpSend.Controls.Add(this.chkMess6);
			this.grpSend.Enabled = false;
			this.grpSend.Location = new System.Drawing.Point(384, 376);
			this.grpSend.Name = "grpSend";
			this.grpSend.Size = new System.Drawing.Size(360, 112);
			this.grpSend.TabIndex = 29;
			this.grpSend.TabStop = false;
			this.grpSend.Text = "Send To...";
			// 
			// chkMess1
			// 
			this.chkMess1.Checked = true;
			this.chkMess1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMess1.Location = new System.Drawing.Point(16, 32);
			this.chkMess1.Name = "chkMess1";
			this.chkMess1.Size = new System.Drawing.Size(104, 16);
			this.chkMess1.TabIndex = 28;
			this.chkMess1.Text = "Team 1 (player)";
			// 
			// chkMess2
			// 
			this.chkMess2.Location = new System.Drawing.Point(16, 56);
			this.chkMess2.Name = "chkMess2";
			this.chkMess2.Size = new System.Drawing.Size(64, 16);
			this.chkMess2.TabIndex = 28;
			this.chkMess2.Text = "Team 2";
			// 
			// chkMess3
			// 
			this.chkMess3.Location = new System.Drawing.Point(16, 80);
			this.chkMess3.Name = "chkMess3";
			this.chkMess3.Size = new System.Drawing.Size(64, 16);
			this.chkMess3.TabIndex = 28;
			this.chkMess3.Text = "Team 3";
			// 
			// chkMess4
			// 
			this.chkMess4.Location = new System.Drawing.Point(152, 56);
			this.chkMess4.Name = "chkMess4";
			this.chkMess4.Size = new System.Drawing.Size(64, 16);
			this.chkMess4.TabIndex = 28;
			this.chkMess4.Text = "Team 4";
			// 
			// chkMess5
			// 
			this.chkMess5.Location = new System.Drawing.Point(152, 80);
			this.chkMess5.Name = "chkMess5";
			this.chkMess5.Size = new System.Drawing.Size(64, 16);
			this.chkMess5.TabIndex = 28;
			this.chkMess5.Text = "Team 5";
			// 
			// chkMess10
			// 
			this.chkMess10.Location = new System.Drawing.Point(280, 88);
			this.chkMess10.Name = "chkMess10";
			this.chkMess10.Size = new System.Drawing.Size(72, 16);
			this.chkMess10.TabIndex = 28;
			this.chkMess10.Text = "Team 10";
			// 
			// chkMess9
			// 
			this.chkMess9.Location = new System.Drawing.Point(280, 64);
			this.chkMess9.Name = "chkMess9";
			this.chkMess9.Size = new System.Drawing.Size(64, 16);
			this.chkMess9.TabIndex = 28;
			this.chkMess9.Text = "Team 9";
			// 
			// chkMess8
			// 
			this.chkMess8.Location = new System.Drawing.Point(280, 40);
			this.chkMess8.Name = "chkMess8";
			this.chkMess8.Size = new System.Drawing.Size(64, 16);
			this.chkMess8.TabIndex = 28;
			this.chkMess8.Text = "Team 8";
			// 
			// chkMess7
			// 
			this.chkMess7.Location = new System.Drawing.Point(280, 16);
			this.chkMess7.Name = "chkMess7";
			this.chkMess7.Size = new System.Drawing.Size(64, 16);
			this.chkMess7.TabIndex = 28;
			this.chkMess7.Text = "Team 7";
			// 
			// chkMess6
			// 
			this.chkMess6.Location = new System.Drawing.Point(152, 32);
			this.chkMess6.Name = "chkMess6";
			this.chkMess6.Size = new System.Drawing.Size(64, 16);
			this.chkMess6.TabIndex = 28;
			this.chkMess6.Text = "Team 6";
			// 
			// tabGlob
			// 
			this.tabGlob.Controls.Add(this.label112);
			this.tabGlob.Controls.Add(this.cboGlobalTeam);
			this.tabGlob.Controls.Add(this.label33);
			this.tabGlob.Controls.Add(this.txtGlobalInc);
			this.tabGlob.Controls.Add(this.label32);
			this.tabGlob.Controls.Add(this.numGlobalPoints);
			this.tabGlob.Controls.Add(this.label79);
			this.tabGlob.Controls.Add(this.label48);
			this.tabGlob.Controls.Add(this.cboGlobalAmount);
			this.tabGlob.Controls.Add(this.cboGlobalType);
			this.tabGlob.Controls.Add(this.cboGlobalVar);
			this.tabGlob.Controls.Add(this.cboGlobalTrig);
			this.tabGlob.Controls.Add(this.label59);
			this.tabGlob.Controls.Add(this.groupBox18);
			this.tabGlob.Controls.Add(this.groupBox5);
			this.tabGlob.Controls.Add(this.groupBox6);
			this.tabGlob.Controls.Add(this.txtGlobalComp);
			this.tabGlob.Controls.Add(this.txtGlobalFail);
			this.tabGlob.Controls.Add(this.label34);
			this.tabGlob.Controls.Add(this.label35);
			this.tabGlob.Location = new System.Drawing.Point(4, 22);
			this.tabGlob.Name = "tabGlob";
			this.tabGlob.Size = new System.Drawing.Size(785, 510);
			this.tabGlob.TabIndex = 2;
			this.tabGlob.Text = "Globals";
			// 
			// label112
			// 
			this.label112.Location = new System.Drawing.Point(488, 24);
			this.label112.Name = "label112";
			this.label112.Size = new System.Drawing.Size(40, 16);
			this.label112.TabIndex = 35;
			this.label112.Text = "Team:";
			this.label112.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// cboGlobalTeam
			// 
			this.cboGlobalTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGlobalTeam.Location = new System.Drawing.Point(536, 24);
			this.cboGlobalTeam.Name = "cboGlobalTeam";
			this.cboGlobalTeam.Size = new System.Drawing.Size(96, 21);
			this.cboGlobalTeam.TabIndex = 34;
			this.cboGlobalTeam.SelectedIndexChanged += new System.EventHandler(this.cboGlobalTeam_SelectedIndexChanged);
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(408, 264);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(64, 16);
			this.label33.TabIndex = 33;
			this.label33.Text = "Incomplete";
			this.label33.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// txtGlobalInc
			// 
			this.txtGlobalInc.BackColor = System.Drawing.Color.Black;
			this.txtGlobalInc.ForeColor = System.Drawing.Color.Yellow;
			this.txtGlobalInc.Location = new System.Drawing.Point(480, 264);
			this.txtGlobalInc.MaxLength = 63;
			this.txtGlobalInc.Name = "txtGlobalInc";
			this.txtGlobalInc.Size = new System.Drawing.Size(272, 20);
			this.txtGlobalInc.TabIndex = 32;
			this.txtGlobalInc.Leave += new System.EventHandler(this.txtGlobal_Leave);
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(496, 208);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(40, 16);
			this.label32.TabIndex = 31;
			this.label32.Text = "Points";
			this.label32.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numGlobalPoints
			// 
			this.numGlobalPoints.Increment = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.numGlobalPoints.Location = new System.Drawing.Point(544, 208);
			this.numGlobalPoints.Maximum = new decimal(new int[] {
            31750,
            0,
            0,
            0});
			this.numGlobalPoints.Minimum = new decimal(new int[] {
            32000,
            0,
            0,
            -2147483648});
			this.numGlobalPoints.Name = "numGlobalPoints";
			this.numGlobalPoints.Size = new System.Drawing.Size(64, 20);
			this.numGlobalPoints.TabIndex = 30;
			this.numGlobalPoints.Leave += new System.EventHandler(this.numGlobalPoints_Leave);
			// 
			// label79
			// 
			this.label79.Location = new System.Drawing.Point(480, 112);
			this.label79.Name = "label79";
			this.label79.Size = new System.Drawing.Size(232, 16);
			this.label79.TabIndex = 29;
			this.label79.Text = "Right-click goal to copy, double-click to paste";
			// 
			// label48
			// 
			this.label48.Location = new System.Drawing.Point(568, 144);
			this.label48.Name = "label48";
			this.label48.Size = new System.Drawing.Size(16, 16);
			this.label48.TabIndex = 28;
			this.label48.Text = "of";
			this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cboGlobalAmount
			// 
			this.cboGlobalAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGlobalAmount.Location = new System.Drawing.Point(416, 144);
			this.cboGlobalAmount.Name = "cboGlobalAmount";
			this.cboGlobalAmount.Size = new System.Drawing.Size(144, 21);
			this.cboGlobalAmount.TabIndex = 23;
			this.cboGlobalAmount.Leave += new System.EventHandler(this.cboGlobalAmount_Leave);
			// 
			// cboGlobalType
			// 
			this.cboGlobalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGlobalType.Location = new System.Drawing.Point(592, 144);
			this.cboGlobalType.Name = "cboGlobalType";
			this.cboGlobalType.Size = new System.Drawing.Size(160, 21);
			this.cboGlobalType.TabIndex = 24;
			this.cboGlobalType.SelectedIndexChanged += new System.EventHandler(this.cboGlobalType_SelectedIndexChanged);
			// 
			// cboGlobalVar
			// 
			this.cboGlobalVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGlobalVar.Location = new System.Drawing.Point(416, 176);
			this.cboGlobalVar.Name = "cboGlobalVar";
			this.cboGlobalVar.Size = new System.Drawing.Size(144, 21);
			this.cboGlobalVar.TabIndex = 25;
			this.cboGlobalVar.Leave += new System.EventHandler(this.cboGlobalVar_Leave);
			// 
			// cboGlobalTrig
			// 
			this.cboGlobalTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGlobalTrig.Location = new System.Drawing.Point(592, 176);
			this.cboGlobalTrig.Name = "cboGlobalTrig";
			this.cboGlobalTrig.Size = new System.Drawing.Size(160, 21);
			this.cboGlobalTrig.TabIndex = 26;
			this.cboGlobalTrig.Leave += new System.EventHandler(this.cboGlobalTrig_Leave);
			// 
			// label59
			// 
			this.label59.Location = new System.Drawing.Point(560, 176);
			this.label59.Name = "label59";
			this.label59.Size = new System.Drawing.Size(32, 16);
			this.label59.TabIndex = 27;
			this.label59.Text = "must";
			this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox18
			// 
			this.groupBox18.Controls.Add(this.panel2);
			this.groupBox18.Controls.Add(this.panel1);
			this.groupBox18.Controls.Add(this.lblPrim1);
			this.groupBox18.Controls.Add(this.lblPrim2);
			this.groupBox18.Controls.Add(this.lblPrim4);
			this.groupBox18.Controls.Add(this.lblPrim3);
			this.groupBox18.Controls.Add(this.optPrim12AND34);
			this.groupBox18.Controls.Add(this.optPrim12OR34);
			this.groupBox18.Location = new System.Drawing.Point(8, 8);
			this.groupBox18.Name = "groupBox18";
			this.groupBox18.Size = new System.Drawing.Size(392, 152);
			this.groupBox18.TabIndex = 11;
			this.groupBox18.TabStop = false;
			this.groupBox18.Text = "Primary Goals";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.optPrim1OR2);
			this.panel2.Controls.Add(this.optPrim1AND2);
			this.panel2.Location = new System.Drawing.Point(328, 16);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(56, 56);
			this.panel2.TabIndex = 8;
			// 
			// optPrim1OR2
			// 
			this.optPrim1OR2.Checked = true;
			this.optPrim1OR2.Location = new System.Drawing.Point(8, 32);
			this.optPrim1OR2.Name = "optPrim1OR2";
			this.optPrim1OR2.Size = new System.Drawing.Size(48, 24);
			this.optPrim1OR2.TabIndex = 5;
			this.optPrim1OR2.TabStop = true;
			this.optPrim1OR2.Text = "OR";
			// 
			// optPrim1AND2
			// 
			this.optPrim1AND2.Location = new System.Drawing.Point(8, 8);
			this.optPrim1AND2.Name = "optPrim1AND2";
			this.optPrim1AND2.Size = new System.Drawing.Size(48, 24);
			this.optPrim1AND2.TabIndex = 4;
			this.optPrim1AND2.Text = "AND";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.optPrim3OR4);
			this.panel1.Controls.Add(this.optPrim3AND4);
			this.panel1.Location = new System.Drawing.Point(328, 88);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(56, 56);
			this.panel1.TabIndex = 7;
			// 
			// optPrim3OR4
			// 
			this.optPrim3OR4.Checked = true;
			this.optPrim3OR4.Location = new System.Drawing.Point(8, 32);
			this.optPrim3OR4.Name = "optPrim3OR4";
			this.optPrim3OR4.Size = new System.Drawing.Size(48, 24);
			this.optPrim3OR4.TabIndex = 5;
			this.optPrim3OR4.TabStop = true;
			this.optPrim3OR4.Text = "OR";
			// 
			// optPrim3AND4
			// 
			this.optPrim3AND4.Location = new System.Drawing.Point(8, 8);
			this.optPrim3AND4.Name = "optPrim3AND4";
			this.optPrim3AND4.Size = new System.Drawing.Size(48, 24);
			this.optPrim3AND4.TabIndex = 4;
			this.optPrim3AND4.Text = "AND";
			// 
			// lblPrim1
			// 
			this.lblPrim1.BackColor = System.Drawing.Color.RosyBrown;
			this.lblPrim1.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lblPrim1.Location = new System.Drawing.Point(8, 24);
			this.lblPrim1.Name = "lblPrim1";
			this.lblPrim1.Size = new System.Drawing.Size(320, 24);
			this.lblPrim1.TabIndex = 6;
			this.lblPrim1.Text = "none (FALSE)";
			this.lblPrim1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPrim2
			// 
			this.lblPrim2.BackColor = System.Drawing.Color.RosyBrown;
			this.lblPrim2.Location = new System.Drawing.Point(8, 48);
			this.lblPrim2.Name = "lblPrim2";
			this.lblPrim2.Size = new System.Drawing.Size(320, 24);
			this.lblPrim2.TabIndex = 5;
			this.lblPrim2.Text = "none (FALSE)";
			this.lblPrim2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPrim4
			// 
			this.lblPrim4.BackColor = System.Drawing.Color.RosyBrown;
			this.lblPrim4.Location = new System.Drawing.Point(8, 120);
			this.lblPrim4.Name = "lblPrim4";
			this.lblPrim4.Size = new System.Drawing.Size(320, 24);
			this.lblPrim4.TabIndex = 5;
			this.lblPrim4.Text = "none (FALSE)";
			this.lblPrim4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPrim3
			// 
			this.lblPrim3.BackColor = System.Drawing.Color.RosyBrown;
			this.lblPrim3.Location = new System.Drawing.Point(8, 96);
			this.lblPrim3.Name = "lblPrim3";
			this.lblPrim3.Size = new System.Drawing.Size(320, 24);
			this.lblPrim3.TabIndex = 6;
			this.lblPrim3.Text = "none (FALSE)";
			this.lblPrim3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// optPrim12AND34
			// 
			this.optPrim12AND34.Location = new System.Drawing.Point(128, 72);
			this.optPrim12AND34.Name = "optPrim12AND34";
			this.optPrim12AND34.Size = new System.Drawing.Size(48, 24);
			this.optPrim12AND34.TabIndex = 4;
			this.optPrim12AND34.Text = "AND";
			// 
			// optPrim12OR34
			// 
			this.optPrim12OR34.Checked = true;
			this.optPrim12OR34.Location = new System.Drawing.Point(176, 72);
			this.optPrim12OR34.Name = "optPrim12OR34";
			this.optPrim12OR34.Size = new System.Drawing.Size(51, 24);
			this.optPrim12OR34.TabIndex = 5;
			this.optPrim12OR34.TabStop = true;
			this.optPrim12OR34.Text = "OR";
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.panel3);
			this.groupBox5.Controls.Add(this.lblPrev1);
			this.groupBox5.Controls.Add(this.lblPrev2);
			this.groupBox5.Controls.Add(this.lblPrev4);
			this.groupBox5.Controls.Add(this.lblPrev3);
			this.groupBox5.Controls.Add(this.optPrev12AND34);
			this.groupBox5.Controls.Add(this.optPrev12OR34);
			this.groupBox5.Controls.Add(this.panel4);
			this.groupBox5.Location = new System.Drawing.Point(8, 168);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(392, 152);
			this.groupBox5.TabIndex = 11;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Prevent Goals";
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.optPrev1AND2);
			this.panel3.Controls.Add(this.optPrev1OR2);
			this.panel3.Location = new System.Drawing.Point(328, 16);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(56, 56);
			this.panel3.TabIndex = 7;
			// 
			// optPrev1AND2
			// 
			this.optPrev1AND2.Location = new System.Drawing.Point(8, 8);
			this.optPrev1AND2.Name = "optPrev1AND2";
			this.optPrev1AND2.Size = new System.Drawing.Size(48, 24);
			this.optPrev1AND2.TabIndex = 4;
			this.optPrev1AND2.Text = "AND";
			// 
			// optPrev1OR2
			// 
			this.optPrev1OR2.Checked = true;
			this.optPrev1OR2.Location = new System.Drawing.Point(8, 32);
			this.optPrev1OR2.Name = "optPrev1OR2";
			this.optPrev1OR2.Size = new System.Drawing.Size(48, 24);
			this.optPrev1OR2.TabIndex = 5;
			this.optPrev1OR2.TabStop = true;
			this.optPrev1OR2.Text = "OR";
			// 
			// lblPrev1
			// 
			this.lblPrev1.BackColor = System.Drawing.Color.RosyBrown;
			this.lblPrev1.Location = new System.Drawing.Point(8, 24);
			this.lblPrev1.Name = "lblPrev1";
			this.lblPrev1.Size = new System.Drawing.Size(320, 24);
			this.lblPrev1.TabIndex = 6;
			this.lblPrev1.Text = "none (FALSE)";
			this.lblPrev1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPrev2
			// 
			this.lblPrev2.BackColor = System.Drawing.Color.RosyBrown;
			this.lblPrev2.Location = new System.Drawing.Point(8, 48);
			this.lblPrev2.Name = "lblPrev2";
			this.lblPrev2.Size = new System.Drawing.Size(320, 24);
			this.lblPrev2.TabIndex = 5;
			this.lblPrev2.Text = "none (FALSE)";
			this.lblPrev2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPrev4
			// 
			this.lblPrev4.BackColor = System.Drawing.Color.RosyBrown;
			this.lblPrev4.Location = new System.Drawing.Point(8, 120);
			this.lblPrev4.Name = "lblPrev4";
			this.lblPrev4.Size = new System.Drawing.Size(320, 24);
			this.lblPrev4.TabIndex = 5;
			this.lblPrev4.Text = "none (FALSE)";
			this.lblPrev4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPrev3
			// 
			this.lblPrev3.BackColor = System.Drawing.Color.RosyBrown;
			this.lblPrev3.Location = new System.Drawing.Point(8, 96);
			this.lblPrev3.Name = "lblPrev3";
			this.lblPrev3.Size = new System.Drawing.Size(320, 24);
			this.lblPrev3.TabIndex = 6;
			this.lblPrev3.Text = "none (FALSE)";
			this.lblPrev3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// optPrev12AND34
			// 
			this.optPrev12AND34.Location = new System.Drawing.Point(128, 72);
			this.optPrev12AND34.Name = "optPrev12AND34";
			this.optPrev12AND34.Size = new System.Drawing.Size(48, 24);
			this.optPrev12AND34.TabIndex = 4;
			this.optPrev12AND34.Text = "AND";
			// 
			// optPrev12OR34
			// 
			this.optPrev12OR34.Checked = true;
			this.optPrev12OR34.Location = new System.Drawing.Point(176, 72);
			this.optPrev12OR34.Name = "optPrev12OR34";
			this.optPrev12OR34.Size = new System.Drawing.Size(51, 24);
			this.optPrev12OR34.TabIndex = 5;
			this.optPrev12OR34.TabStop = true;
			this.optPrev12OR34.Text = "OR";
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.optPrev3OR4);
			this.panel4.Controls.Add(this.optPrev3AND4);
			this.panel4.Location = new System.Drawing.Point(328, 88);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(56, 56);
			this.panel4.TabIndex = 7;
			// 
			// optPrev3OR4
			// 
			this.optPrev3OR4.Checked = true;
			this.optPrev3OR4.Location = new System.Drawing.Point(8, 32);
			this.optPrev3OR4.Name = "optPrev3OR4";
			this.optPrev3OR4.Size = new System.Drawing.Size(48, 24);
			this.optPrev3OR4.TabIndex = 5;
			this.optPrev3OR4.TabStop = true;
			this.optPrev3OR4.Text = "OR";
			// 
			// optPrev3AND4
			// 
			this.optPrev3AND4.Location = new System.Drawing.Point(8, 8);
			this.optPrev3AND4.Name = "optPrev3AND4";
			this.optPrev3AND4.Size = new System.Drawing.Size(48, 24);
			this.optPrev3AND4.TabIndex = 4;
			this.optPrev3AND4.Text = "AND";
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.panel6);
			this.groupBox6.Controls.Add(this.panel5);
			this.groupBox6.Controls.Add(this.lblSec1);
			this.groupBox6.Controls.Add(this.lblSec2);
			this.groupBox6.Controls.Add(this.lblSec4);
			this.groupBox6.Controls.Add(this.lblSec3);
			this.groupBox6.Controls.Add(this.optSec12AND34);
			this.groupBox6.Controls.Add(this.optSec12OR34);
			this.groupBox6.Location = new System.Drawing.Point(8, 328);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(392, 152);
			this.groupBox6.TabIndex = 11;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Secondary Goals";
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.optSec3OR4);
			this.panel6.Controls.Add(this.optSec3AND4);
			this.panel6.Location = new System.Drawing.Point(328, 88);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(56, 56);
			this.panel6.TabIndex = 8;
			// 
			// optSec3OR4
			// 
			this.optSec3OR4.Checked = true;
			this.optSec3OR4.Location = new System.Drawing.Point(8, 32);
			this.optSec3OR4.Name = "optSec3OR4";
			this.optSec3OR4.Size = new System.Drawing.Size(50, 24);
			this.optSec3OR4.TabIndex = 5;
			this.optSec3OR4.TabStop = true;
			this.optSec3OR4.Text = "OR";
			// 
			// optSec3AND4
			// 
			this.optSec3AND4.Location = new System.Drawing.Point(8, 8);
			this.optSec3AND4.Name = "optSec3AND4";
			this.optSec3AND4.Size = new System.Drawing.Size(48, 24);
			this.optSec3AND4.TabIndex = 4;
			this.optSec3AND4.Text = "AND";
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.optSec1OR2);
			this.panel5.Controls.Add(this.optSec1AND2);
			this.panel5.Location = new System.Drawing.Point(328, 16);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(56, 56);
			this.panel5.TabIndex = 7;
			// 
			// optSec1OR2
			// 
			this.optSec1OR2.Checked = true;
			this.optSec1OR2.Location = new System.Drawing.Point(8, 32);
			this.optSec1OR2.Name = "optSec1OR2";
			this.optSec1OR2.Size = new System.Drawing.Size(48, 24);
			this.optSec1OR2.TabIndex = 5;
			this.optSec1OR2.TabStop = true;
			this.optSec1OR2.Text = "OR";
			// 
			// optSec1AND2
			// 
			this.optSec1AND2.Location = new System.Drawing.Point(8, 8);
			this.optSec1AND2.Name = "optSec1AND2";
			this.optSec1AND2.Size = new System.Drawing.Size(48, 24);
			this.optSec1AND2.TabIndex = 4;
			this.optSec1AND2.Text = "AND";
			// 
			// lblSec1
			// 
			this.lblSec1.BackColor = System.Drawing.Color.RosyBrown;
			this.lblSec1.Location = new System.Drawing.Point(8, 24);
			this.lblSec1.Name = "lblSec1";
			this.lblSec1.Size = new System.Drawing.Size(320, 24);
			this.lblSec1.TabIndex = 6;
			this.lblSec1.Text = "none (FALSE)";
			this.lblSec1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSec2
			// 
			this.lblSec2.BackColor = System.Drawing.Color.RosyBrown;
			this.lblSec2.Location = new System.Drawing.Point(8, 48);
			this.lblSec2.Name = "lblSec2";
			this.lblSec2.Size = new System.Drawing.Size(320, 24);
			this.lblSec2.TabIndex = 5;
			this.lblSec2.Text = "none (FALSE)";
			this.lblSec2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSec4
			// 
			this.lblSec4.BackColor = System.Drawing.Color.RosyBrown;
			this.lblSec4.Location = new System.Drawing.Point(8, 120);
			this.lblSec4.Name = "lblSec4";
			this.lblSec4.Size = new System.Drawing.Size(320, 24);
			this.lblSec4.TabIndex = 5;
			this.lblSec4.Text = "none (FALSE)";
			this.lblSec4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSec3
			// 
			this.lblSec3.BackColor = System.Drawing.Color.RosyBrown;
			this.lblSec3.Location = new System.Drawing.Point(8, 96);
			this.lblSec3.Name = "lblSec3";
			this.lblSec3.Size = new System.Drawing.Size(320, 24);
			this.lblSec3.TabIndex = 6;
			this.lblSec3.Text = "none (FALSE)";
			this.lblSec3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// optSec12AND34
			// 
			this.optSec12AND34.Location = new System.Drawing.Point(128, 72);
			this.optSec12AND34.Name = "optSec12AND34";
			this.optSec12AND34.Size = new System.Drawing.Size(48, 24);
			this.optSec12AND34.TabIndex = 4;
			this.optSec12AND34.Text = "AND";
			// 
			// optSec12OR34
			// 
			this.optSec12OR34.Checked = true;
			this.optSec12OR34.Location = new System.Drawing.Point(176, 72);
			this.optSec12OR34.Name = "optSec12OR34";
			this.optSec12OR34.Size = new System.Drawing.Size(51, 24);
			this.optSec12OR34.TabIndex = 5;
			this.optSec12OR34.TabStop = true;
			this.optSec12OR34.Text = "OR";
			// 
			// txtGlobalComp
			// 
			this.txtGlobalComp.BackColor = System.Drawing.Color.Black;
			this.txtGlobalComp.ForeColor = System.Drawing.Color.Lime;
			this.txtGlobalComp.Location = new System.Drawing.Point(480, 304);
			this.txtGlobalComp.MaxLength = 63;
			this.txtGlobalComp.Name = "txtGlobalComp";
			this.txtGlobalComp.Size = new System.Drawing.Size(272, 20);
			this.txtGlobalComp.TabIndex = 32;
			this.txtGlobalComp.Leave += new System.EventHandler(this.txtGlobal_Leave);
			// 
			// txtGlobalFail
			// 
			this.txtGlobalFail.BackColor = System.Drawing.Color.Black;
			this.txtGlobalFail.ForeColor = System.Drawing.Color.Red;
			this.txtGlobalFail.Location = new System.Drawing.Point(480, 344);
			this.txtGlobalFail.MaxLength = 63;
			this.txtGlobalFail.Name = "txtGlobalFail";
			this.txtGlobalFail.Size = new System.Drawing.Size(272, 20);
			this.txtGlobalFail.TabIndex = 32;
			this.txtGlobalFail.Leave += new System.EventHandler(this.txtGlobal_Leave);
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(408, 304);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(64, 16);
			this.label34.TabIndex = 33;
			this.label34.Text = "Complete";
			this.label34.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label35
			// 
			this.label35.Location = new System.Drawing.Point(408, 344);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(64, 16);
			this.label35.TabIndex = 33;
			this.label35.Text = "Failed";
			this.label35.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// tabTeam
			// 
			this.tabTeam.Controls.Add(this.groupBox32);
			this.tabTeam.Controls.Add(this.txtTeamName);
			this.tabTeam.Controls.Add(this.label96);
			this.tabTeam.Controls.Add(this.groupBox31);
			this.tabTeam.Controls.Add(this.groupBox30);
			this.tabTeam.Location = new System.Drawing.Point(4, 22);
			this.tabTeam.Name = "tabTeam";
			this.tabTeam.Size = new System.Drawing.Size(785, 510);
			this.tabTeam.TabIndex = 3;
			this.tabTeam.Text = "Teams";
			// 
			// groupBox32
			// 
			this.groupBox32.Controls.Add(this.groupBox33);
			this.groupBox32.Controls.Add(this.groupBox34);
			this.groupBox32.Controls.Add(this.groupBox35);
			this.groupBox32.Location = new System.Drawing.Point(360, 16);
			this.groupBox32.Name = "groupBox32";
			this.groupBox32.Size = new System.Drawing.Size(400, 453);
			this.groupBox32.TabIndex = 18;
			this.groupBox32.TabStop = false;
			this.groupBox32.Text = "End of Mission Messages";
			// 
			// groupBox33
			// 
			this.groupBox33.Controls.Add(this.cboPF2Color);
			this.groupBox33.Controls.Add(this.cboPF1Color);
			this.groupBox33.Controls.Add(this.txtPrimFail1);
			this.groupBox33.Controls.Add(this.txtPrimFail2);
			this.groupBox33.Location = new System.Drawing.Point(8, 318);
			this.groupBox33.Name = "groupBox33";
			this.groupBox33.Size = new System.Drawing.Size(384, 128);
			this.groupBox33.TabIndex = 2;
			this.groupBox33.TabStop = false;
			this.groupBox33.Text = "Primary Mission Failed";
			// 
			// cboPF2Color
			// 
			this.cboPF2Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPF2Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Yellow"});
			this.cboPF2Color.Location = new System.Drawing.Point(248, 72);
			this.cboPF2Color.Name = "cboPF2Color";
			this.cboPF2Color.Size = new System.Drawing.Size(120, 21);
			this.cboPF2Color.TabIndex = 38;
			// 
			// cboPF1Color
			// 
			this.cboPF1Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPF1Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Yellow"});
			this.cboPF1Color.Location = new System.Drawing.Point(248, 18);
			this.cboPF1Color.Name = "cboPF1Color";
			this.cboPF1Color.Size = new System.Drawing.Size(120, 21);
			this.cboPF1Color.TabIndex = 38;
			// 
			// txtPrimFail1
			// 
			this.txtPrimFail1.BackColor = System.Drawing.Color.Black;
			this.txtPrimFail1.ForeColor = System.Drawing.Color.Red;
			this.txtPrimFail1.Location = new System.Drawing.Point(16, 46);
			this.txtPrimFail1.MaxLength = 64;
			this.txtPrimFail1.Name = "txtPrimFail1";
			this.txtPrimFail1.Size = new System.Drawing.Size(352, 20);
			this.txtPrimFail1.TabIndex = 14;
			// 
			// txtPrimFail2
			// 
			this.txtPrimFail2.BackColor = System.Drawing.Color.Black;
			this.txtPrimFail2.ForeColor = System.Drawing.Color.Red;
			this.txtPrimFail2.Location = new System.Drawing.Point(16, 98);
			this.txtPrimFail2.MaxLength = 64;
			this.txtPrimFail2.Name = "txtPrimFail2";
			this.txtPrimFail2.Size = new System.Drawing.Size(352, 20);
			this.txtPrimFail2.TabIndex = 15;
			// 
			// groupBox34
			// 
			this.groupBox34.Controls.Add(this.cboSC2Color);
			this.groupBox34.Controls.Add(this.cboSC1Color);
			this.groupBox34.Controls.Add(this.txtSecComp1);
			this.groupBox34.Controls.Add(this.txtSecComp2);
			this.groupBox34.Location = new System.Drawing.Point(8, 170);
			this.groupBox34.Name = "groupBox34";
			this.groupBox34.Size = new System.Drawing.Size(384, 128);
			this.groupBox34.TabIndex = 1;
			this.groupBox34.TabStop = false;
			this.groupBox34.Text = "Secondary Mission Complete";
			// 
			// cboSC2Color
			// 
			this.cboSC2Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSC2Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Yellow"});
			this.cboSC2Color.Location = new System.Drawing.Point(248, 72);
			this.cboSC2Color.Name = "cboSC2Color";
			this.cboSC2Color.Size = new System.Drawing.Size(120, 21);
			this.cboSC2Color.TabIndex = 38;
			// 
			// cboSC1Color
			// 
			this.cboSC1Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSC1Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Yellow"});
			this.cboSC1Color.Location = new System.Drawing.Point(248, 18);
			this.cboSC1Color.Name = "cboSC1Color";
			this.cboSC1Color.Size = new System.Drawing.Size(120, 21);
			this.cboSC1Color.TabIndex = 38;
			// 
			// txtSecComp1
			// 
			this.txtSecComp1.BackColor = System.Drawing.Color.Black;
			this.txtSecComp1.ForeColor = System.Drawing.Color.DodgerBlue;
			this.txtSecComp1.Location = new System.Drawing.Point(16, 46);
			this.txtSecComp1.MaxLength = 64;
			this.txtSecComp1.Name = "txtSecComp1";
			this.txtSecComp1.Size = new System.Drawing.Size(352, 20);
			this.txtSecComp1.TabIndex = 12;
			// 
			// txtSecComp2
			// 
			this.txtSecComp2.BackColor = System.Drawing.Color.Black;
			this.txtSecComp2.ForeColor = System.Drawing.Color.DodgerBlue;
			this.txtSecComp2.Location = new System.Drawing.Point(16, 98);
			this.txtSecComp2.MaxLength = 64;
			this.txtSecComp2.Name = "txtSecComp2";
			this.txtSecComp2.Size = new System.Drawing.Size(352, 20);
			this.txtSecComp2.TabIndex = 13;
			// 
			// groupBox35
			// 
			this.groupBox35.Controls.Add(this.cboPC2Color);
			this.groupBox35.Controls.Add(this.cboPC1Color);
			this.groupBox35.Controls.Add(this.txtPrimComp1);
			this.groupBox35.Controls.Add(this.txtPrimComp2);
			this.groupBox35.Location = new System.Drawing.Point(8, 24);
			this.groupBox35.Name = "groupBox35";
			this.groupBox35.Size = new System.Drawing.Size(384, 128);
			this.groupBox35.TabIndex = 0;
			this.groupBox35.TabStop = false;
			this.groupBox35.Text = "Primary Mission Complete";
			// 
			// cboPC2Color
			// 
			this.cboPC2Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPC2Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Yellow"});
			this.cboPC2Color.Location = new System.Drawing.Point(248, 72);
			this.cboPC2Color.Name = "cboPC2Color";
			this.cboPC2Color.Size = new System.Drawing.Size(120, 21);
			this.cboPC2Color.TabIndex = 38;
			// 
			// cboPC1Color
			// 
			this.cboPC1Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPC1Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Yellow"});
			this.cboPC1Color.Location = new System.Drawing.Point(248, 18);
			this.cboPC1Color.Name = "cboPC1Color";
			this.cboPC1Color.Size = new System.Drawing.Size(120, 21);
			this.cboPC1Color.TabIndex = 38;
			// 
			// txtPrimComp1
			// 
			this.txtPrimComp1.BackColor = System.Drawing.Color.Black;
			this.txtPrimComp1.ForeColor = System.Drawing.Color.Lime;
			this.txtPrimComp1.Location = new System.Drawing.Point(16, 46);
			this.txtPrimComp1.MaxLength = 64;
			this.txtPrimComp1.Name = "txtPrimComp1";
			this.txtPrimComp1.Size = new System.Drawing.Size(352, 20);
			this.txtPrimComp1.TabIndex = 10;
			// 
			// txtPrimComp2
			// 
			this.txtPrimComp2.BackColor = System.Drawing.Color.Black;
			this.txtPrimComp2.ForeColor = System.Drawing.Color.Lime;
			this.txtPrimComp2.Location = new System.Drawing.Point(16, 98);
			this.txtPrimComp2.MaxLength = 64;
			this.txtPrimComp2.Name = "txtPrimComp2";
			this.txtPrimComp2.Size = new System.Drawing.Size(352, 20);
			this.txtPrimComp2.TabIndex = 11;
			// 
			// txtTeamName
			// 
			this.txtTeamName.Location = new System.Drawing.Point(64, 208);
			this.txtTeamName.MaxLength = 15;
			this.txtTeamName.Name = "txtTeamName";
			this.txtTeamName.Size = new System.Drawing.Size(88, 20);
			this.txtTeamName.TabIndex = 5;
			this.txtTeamName.Text = "Imperials";
			this.txtTeamName.Leave += new System.EventHandler(this.txtTeamName_Leave);
			// 
			// label96
			// 
			this.label96.Location = new System.Drawing.Point(16, 208);
			this.label96.Name = "label96";
			this.label96.Size = new System.Drawing.Size(40, 16);
			this.label96.TabIndex = 4;
			this.label96.Text = "Name:";
			this.label96.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// groupBox31
			// 
			this.groupBox31.Controls.Add(this.chkTeam1);
			this.groupBox31.Controls.Add(this.chkTeam2);
			this.groupBox31.Controls.Add(this.chkTeam3);
			this.groupBox31.Controls.Add(this.chkTeam4);
			this.groupBox31.Controls.Add(this.chkTeam5);
			this.groupBox31.Controls.Add(this.chkTeam6);
			this.groupBox31.Controls.Add(this.chkTeam7);
			this.groupBox31.Controls.Add(this.chkTeam8);
			this.groupBox31.Controls.Add(this.chkTeam9);
			this.groupBox31.Controls.Add(this.chkTeam10);
			this.groupBox31.Location = new System.Drawing.Point(200, 16);
			this.groupBox31.Name = "groupBox31";
			this.groupBox31.Size = new System.Drawing.Size(120, 184);
			this.groupBox31.TabIndex = 3;
			this.groupBox31.TabStop = false;
			this.groupBox31.Text = "Allied with...";
			// 
			// chkTeam1
			// 
			this.chkTeam1.Location = new System.Drawing.Point(8, 16);
			this.chkTeam1.Name = "chkTeam1";
			this.chkTeam1.Size = new System.Drawing.Size(104, 16);
			this.chkTeam1.TabIndex = 2;
			this.chkTeam1.Text = "Imperials";
			// 
			// chkTeam2
			// 
			this.chkTeam2.Location = new System.Drawing.Point(8, 32);
			this.chkTeam2.Name = "chkTeam2";
			this.chkTeam2.Size = new System.Drawing.Size(104, 16);
			this.chkTeam2.TabIndex = 2;
			this.chkTeam2.Text = "Rebels";
			// 
			// chkTeam3
			// 
			this.chkTeam3.Location = new System.Drawing.Point(8, 48);
			this.chkTeam3.Name = "chkTeam3";
			this.chkTeam3.Size = new System.Drawing.Size(104, 16);
			this.chkTeam3.TabIndex = 2;
			this.chkTeam3.Text = "Team 3";
			// 
			// chkTeam4
			// 
			this.chkTeam4.Location = new System.Drawing.Point(8, 64);
			this.chkTeam4.Name = "chkTeam4";
			this.chkTeam4.Size = new System.Drawing.Size(104, 16);
			this.chkTeam4.TabIndex = 2;
			this.chkTeam4.Text = "Team 4";
			// 
			// chkTeam5
			// 
			this.chkTeam5.Location = new System.Drawing.Point(8, 80);
			this.chkTeam5.Name = "chkTeam5";
			this.chkTeam5.Size = new System.Drawing.Size(104, 16);
			this.chkTeam5.TabIndex = 2;
			this.chkTeam5.Text = "Team 5";
			// 
			// chkTeam6
			// 
			this.chkTeam6.Location = new System.Drawing.Point(8, 96);
			this.chkTeam6.Name = "chkTeam6";
			this.chkTeam6.Size = new System.Drawing.Size(104, 16);
			this.chkTeam6.TabIndex = 2;
			this.chkTeam6.Text = "Team 6";
			// 
			// chkTeam7
			// 
			this.chkTeam7.Location = new System.Drawing.Point(8, 112);
			this.chkTeam7.Name = "chkTeam7";
			this.chkTeam7.Size = new System.Drawing.Size(104, 16);
			this.chkTeam7.TabIndex = 2;
			this.chkTeam7.Text = "Team 7";
			// 
			// chkTeam8
			// 
			this.chkTeam8.Location = new System.Drawing.Point(8, 128);
			this.chkTeam8.Name = "chkTeam8";
			this.chkTeam8.Size = new System.Drawing.Size(104, 16);
			this.chkTeam8.TabIndex = 2;
			this.chkTeam8.Text = "Team 8";
			// 
			// chkTeam9
			// 
			this.chkTeam9.Location = new System.Drawing.Point(8, 144);
			this.chkTeam9.Name = "chkTeam9";
			this.chkTeam9.Size = new System.Drawing.Size(104, 16);
			this.chkTeam9.TabIndex = 2;
			this.chkTeam9.Text = "Team 9";
			// 
			// chkTeam10
			// 
			this.chkTeam10.Location = new System.Drawing.Point(8, 160);
			this.chkTeam10.Name = "chkTeam10";
			this.chkTeam10.Size = new System.Drawing.Size(104, 16);
			this.chkTeam10.TabIndex = 2;
			this.chkTeam10.Text = "Team 10";
			// 
			// groupBox30
			// 
			this.groupBox30.Controls.Add(this.lblTeam1);
			this.groupBox30.Controls.Add(this.lblTeam2);
			this.groupBox30.Controls.Add(this.lblTeam3);
			this.groupBox30.Controls.Add(this.lblTeam4);
			this.groupBox30.Controls.Add(this.lblTeam5);
			this.groupBox30.Controls.Add(this.lblTeam6);
			this.groupBox30.Controls.Add(this.lblTeam7);
			this.groupBox30.Controls.Add(this.lblTeam8);
			this.groupBox30.Controls.Add(this.lblTeam9);
			this.groupBox30.Controls.Add(this.lblTeam10);
			this.groupBox30.Location = new System.Drawing.Point(8, 16);
			this.groupBox30.Name = "groupBox30";
			this.groupBox30.Size = new System.Drawing.Size(168, 184);
			this.groupBox30.TabIndex = 1;
			this.groupBox30.TabStop = false;
			// 
			// lblTeam1
			// 
			this.lblTeam1.BackColor = System.Drawing.Color.RosyBrown;
			this.lblTeam1.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lblTeam1.Location = new System.Drawing.Point(8, 16);
			this.lblTeam1.Name = "lblTeam1";
			this.lblTeam1.Size = new System.Drawing.Size(152, 16);
			this.lblTeam1.TabIndex = 0;
			this.lblTeam1.Text = "Team 1: Imperials";
			// 
			// lblTeam2
			// 
			this.lblTeam2.BackColor = System.Drawing.Color.RosyBrown;
			this.lblTeam2.Location = new System.Drawing.Point(8, 32);
			this.lblTeam2.Name = "lblTeam2";
			this.lblTeam2.Size = new System.Drawing.Size(152, 16);
			this.lblTeam2.TabIndex = 0;
			this.lblTeam2.Text = "Team 2: Rebels";
			// 
			// lblTeam3
			// 
			this.lblTeam3.BackColor = System.Drawing.Color.RosyBrown;
			this.lblTeam3.Location = new System.Drawing.Point(8, 48);
			this.lblTeam3.Name = "lblTeam3";
			this.lblTeam3.Size = new System.Drawing.Size(152, 16);
			this.lblTeam3.TabIndex = 0;
			this.lblTeam3.Text = "Team 3:";
			// 
			// lblTeam4
			// 
			this.lblTeam4.BackColor = System.Drawing.Color.RosyBrown;
			this.lblTeam4.Location = new System.Drawing.Point(8, 64);
			this.lblTeam4.Name = "lblTeam4";
			this.lblTeam4.Size = new System.Drawing.Size(152, 16);
			this.lblTeam4.TabIndex = 0;
			this.lblTeam4.Text = "Team 4:";
			// 
			// lblTeam5
			// 
			this.lblTeam5.BackColor = System.Drawing.Color.RosyBrown;
			this.lblTeam5.Location = new System.Drawing.Point(8, 80);
			this.lblTeam5.Name = "lblTeam5";
			this.lblTeam5.Size = new System.Drawing.Size(152, 16);
			this.lblTeam5.TabIndex = 0;
			this.lblTeam5.Text = "Team 5:";
			// 
			// lblTeam6
			// 
			this.lblTeam6.BackColor = System.Drawing.Color.RosyBrown;
			this.lblTeam6.Location = new System.Drawing.Point(8, 96);
			this.lblTeam6.Name = "lblTeam6";
			this.lblTeam6.Size = new System.Drawing.Size(152, 16);
			this.lblTeam6.TabIndex = 0;
			this.lblTeam6.Text = "Team 6:";
			// 
			// lblTeam7
			// 
			this.lblTeam7.BackColor = System.Drawing.Color.RosyBrown;
			this.lblTeam7.Location = new System.Drawing.Point(8, 112);
			this.lblTeam7.Name = "lblTeam7";
			this.lblTeam7.Size = new System.Drawing.Size(152, 16);
			this.lblTeam7.TabIndex = 0;
			this.lblTeam7.Text = "Team 7:";
			// 
			// lblTeam8
			// 
			this.lblTeam8.BackColor = System.Drawing.Color.RosyBrown;
			this.lblTeam8.Location = new System.Drawing.Point(8, 128);
			this.lblTeam8.Name = "lblTeam8";
			this.lblTeam8.Size = new System.Drawing.Size(152, 16);
			this.lblTeam8.TabIndex = 0;
			this.lblTeam8.Text = "Team 8:";
			// 
			// lblTeam9
			// 
			this.lblTeam9.BackColor = System.Drawing.Color.RosyBrown;
			this.lblTeam9.Location = new System.Drawing.Point(8, 144);
			this.lblTeam9.Name = "lblTeam9";
			this.lblTeam9.Size = new System.Drawing.Size(152, 16);
			this.lblTeam9.TabIndex = 0;
			this.lblTeam9.Text = "Team 9:";
			// 
			// lblTeam10
			// 
			this.lblTeam10.BackColor = System.Drawing.Color.RosyBrown;
			this.lblTeam10.Location = new System.Drawing.Point(8, 160);
			this.lblTeam10.Name = "lblTeam10";
			this.lblTeam10.Size = new System.Drawing.Size(152, 16);
			this.lblTeam10.TabIndex = 0;
			this.lblTeam10.Text = "Team 10:";
			// 
			// tabMission
			// 
			this.tabMission.Controls.Add(this.grpIFF);
			this.tabMission.Controls.Add(this.label150);
			this.tabMission.Controls.Add(this.groupBox36);
			this.tabMission.Controls.Add(this.optXvT);
			this.tabMission.Controls.Add(this.label104);
			this.tabMission.Controls.Add(this.chkPreventOutcome);
			this.tabMission.Controls.Add(this.label102);
			this.tabMission.Controls.Add(this.numMissTimeMin);
			this.tabMission.Controls.Add(this.label101);
			this.tabMission.Controls.Add(this.label100);
			this.tabMission.Controls.Add(this.cboMissType);
			this.tabMission.Controls.Add(this.label97);
			this.tabMission.Controls.Add(this.txtMissDesc);
			this.tabMission.Controls.Add(this.txtMissSucc);
			this.tabMission.Controls.Add(this.txtMissFail);
			this.tabMission.Controls.Add(this.label98);
			this.tabMission.Controls.Add(this.label99);
			this.tabMission.Controls.Add(this.numMissTimeSec);
			this.tabMission.Controls.Add(this.label103);
			this.tabMission.Controls.Add(this.optBoP);
			this.tabMission.Location = new System.Drawing.Point(4, 22);
			this.tabMission.Name = "tabMission";
			this.tabMission.Size = new System.Drawing.Size(785, 510);
			this.tabMission.TabIndex = 4;
			this.tabMission.Text = "Mission";
			// 
			// grpIFF
			// 
			this.grpIFF.Controls.Add(this.txtIFF6);
			this.grpIFF.Controls.Add(this.txtIFF5);
			this.grpIFF.Controls.Add(this.txtIFF4);
			this.grpIFF.Controls.Add(this.txtIFF3);
			this.grpIFF.Controls.Add(this.lblIFF6);
			this.grpIFF.Controls.Add(this.lblIFF5);
			this.grpIFF.Controls.Add(this.lblIFF4);
			this.grpIFF.Controls.Add(this.lblIFF3);
			this.grpIFF.Location = new System.Drawing.Point(577, 368);
			this.grpIFF.Name = "grpIFF";
			this.grpIFF.Size = new System.Drawing.Size(183, 106);
			this.grpIFF.TabIndex = 18;
			this.grpIFF.TabStop = false;
			this.grpIFF.Text = "IFFs";
			// 
			// txtIFF6
			// 
			this.txtIFF6.BackColor = System.Drawing.Color.Black;
			this.txtIFF6.ForeColor = System.Drawing.Color.DarkOrchid;
			this.txtIFF6.Location = new System.Drawing.Point(84, 78);
			this.txtIFF6.MaxLength = 19;
			this.txtIFF6.Name = "txtIFF6";
			this.txtIFF6.Size = new System.Drawing.Size(88, 20);
			this.txtIFF6.TabIndex = 4;
			// 
			// txtIFF5
			// 
			this.txtIFF5.BackColor = System.Drawing.Color.Black;
			this.txtIFF5.ForeColor = System.Drawing.Color.OrangeRed;
			this.txtIFF5.Location = new System.Drawing.Point(84, 57);
			this.txtIFF5.MaxLength = 19;
			this.txtIFF5.Name = "txtIFF5";
			this.txtIFF5.Size = new System.Drawing.Size(88, 20);
			this.txtIFF5.TabIndex = 3;
			// 
			// txtIFF4
			// 
			this.txtIFF4.BackColor = System.Drawing.Color.Black;
			this.txtIFF4.ForeColor = System.Drawing.Color.Yellow;
			this.txtIFF4.Location = new System.Drawing.Point(84, 36);
			this.txtIFF4.MaxLength = 19;
			this.txtIFF4.Name = "txtIFF4";
			this.txtIFF4.Size = new System.Drawing.Size(88, 20);
			this.txtIFF4.TabIndex = 2;
			// 
			// txtIFF3
			// 
			this.txtIFF3.BackColor = System.Drawing.Color.Black;
			this.txtIFF3.ForeColor = System.Drawing.Color.DodgerBlue;
			this.txtIFF3.Location = new System.Drawing.Point(84, 15);
			this.txtIFF3.MaxLength = 19;
			this.txtIFF3.Name = "txtIFF3";
			this.txtIFF3.Size = new System.Drawing.Size(88, 20);
			this.txtIFF3.TabIndex = 1;
			// 
			// lblIFF6
			// 
			this.lblIFF6.Location = new System.Drawing.Point(6, 79);
			this.lblIFF6.Name = "lblIFF6";
			this.lblIFF6.Size = new System.Drawing.Size(72, 16);
			this.lblIFF6.TabIndex = 4;
			this.lblIFF6.Text = "IFF6 - Purple";
			this.lblIFF6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lblIFF5
			// 
			this.lblIFF5.Location = new System.Drawing.Point(6, 58);
			this.lblIFF5.Name = "lblIFF5";
			this.lblIFF5.Size = new System.Drawing.Size(72, 16);
			this.lblIFF5.TabIndex = 3;
			this.lblIFF5.Text = "IFF5 - Red";
			this.lblIFF5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lblIFF4
			// 
			this.lblIFF4.Location = new System.Drawing.Point(6, 37);
			this.lblIFF4.Name = "lblIFF4";
			this.lblIFF4.Size = new System.Drawing.Size(72, 16);
			this.lblIFF4.TabIndex = 2;
			this.lblIFF4.Text = "IFF4 - Yellow";
			this.lblIFF4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lblIFF3
			// 
			this.lblIFF3.Location = new System.Drawing.Point(6, 16);
			this.lblIFF3.Name = "lblIFF3";
			this.lblIFF3.Size = new System.Drawing.Size(72, 16);
			this.lblIFF3.TabIndex = 1;
			this.lblIFF3.Text = "IFF3 - Blue";
			this.lblIFF3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label150
			// 
			this.label150.AutoSize = true;
			this.label150.Location = new System.Drawing.Point(345, 475);
			this.label150.Name = "label150";
			this.label150.Size = new System.Drawing.Size(415, 13);
			this.label150.TabIndex = 17;
			this.label150.Text = "Tip: Use the right-click context menu to copy/paste text between the system clipb" +
    "oard.";
			// 
			// groupBox36
			// 
			this.groupBox36.Controls.Add(this.chkMissUnk3);
			this.groupBox36.Controls.Add(this.numMissUnk1);
			this.groupBox36.Controls.Add(this.label105);
			this.groupBox36.Controls.Add(this.label106);
			this.groupBox36.Controls.Add(this.numMissUnk2);
			this.groupBox36.Location = new System.Drawing.Point(273, 368);
			this.groupBox36.Name = "groupBox36";
			this.groupBox36.Size = new System.Drawing.Size(281, 56);
			this.groupBox36.TabIndex = 9;
			this.groupBox36.TabStop = false;
			this.groupBox36.Text = "Unknown";
			// 
			// chkMissUnk3
			// 
			this.chkMissUnk3.Location = new System.Drawing.Point(213, 25);
			this.chkMissUnk3.Name = "chkMissUnk3";
			this.chkMissUnk3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkMissUnk3.Size = new System.Drawing.Size(48, 16);
			this.chkMissUnk3.TabIndex = 2;
			this.chkMissUnk3.Text = "0xB";
			this.chkMissUnk3.Leave += new System.EventHandler(this.chkMissUnk3_Leave);
			// 
			// numMissUnk1
			// 
			this.numMissUnk1.Location = new System.Drawing.Point(48, 24);
			this.numMissUnk1.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numMissUnk1.Name = "numMissUnk1";
			this.numMissUnk1.Size = new System.Drawing.Size(48, 20);
			this.numMissUnk1.TabIndex = 1;
			this.numMissUnk1.Leave += new System.EventHandler(this.numMissUnk1_Leave);
			// 
			// label105
			// 
			this.label105.Location = new System.Drawing.Point(8, 24);
			this.label105.Name = "label105";
			this.label105.Size = new System.Drawing.Size(32, 16);
			this.label105.TabIndex = 0;
			this.label105.Text = "0x6";
			this.label105.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label106
			// 
			this.label106.Location = new System.Drawing.Point(104, 24);
			this.label106.Name = "label106";
			this.label106.Size = new System.Drawing.Size(32, 16);
			this.label106.TabIndex = 0;
			this.label106.Text = "0x8";
			this.label106.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numMissUnk2
			// 
			this.numMissUnk2.Location = new System.Drawing.Point(144, 24);
			this.numMissUnk2.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numMissUnk2.Name = "numMissUnk2";
			this.numMissUnk2.Size = new System.Drawing.Size(48, 20);
			this.numMissUnk2.TabIndex = 1;
			this.numMissUnk2.Leave += new System.EventHandler(this.numMissUnk2_Leave);
			// 
			// chkPreventOutcome
			// 
			this.chkPreventOutcome.Location = new System.Drawing.Point(348, 430);
			this.chkPreventOutcome.Name = "chkPreventOutcome";
			this.chkPreventOutcome.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkPreventOutcome.Size = new System.Drawing.Size(152, 16);
			this.chkPreventOutcome.TabIndex = 2;
			this.chkPreventOutcome.Text = "Prevent mission outcome";
			this.chkPreventOutcome.Leave += new System.EventHandler(this.chkPreventOutcome_Leave);
			// 
			// optXvT
			// 
			this.optXvT.Checked = true;
			this.optXvT.Location = new System.Drawing.Point(24, 464);
			this.optXvT.Name = "optXvT";
			this.optXvT.Size = new System.Drawing.Size(56, 16);
			this.optXvT.TabIndex = 8;
			this.optXvT.TabStop = true;
			this.optXvT.Text = "XvT";
			this.optXvT.CheckedChanged += new System.EventHandler(this.optXvT_CheckedChanged);
			// 
			// label104
			// 
			this.label104.Location = new System.Drawing.Point(24, 448);
			this.label104.Name = "label104";
			this.label104.Size = new System.Drawing.Size(72, 16);
			this.label104.TabIndex = 7;
			this.label104.Text = "Platform";
			// 
			// label102
			// 
			this.label102.Location = new System.Drawing.Point(16, 424);
			this.label102.Name = "label102";
			this.label102.Size = new System.Drawing.Size(32, 16);
			this.label102.TabIndex = 6;
			this.label102.Text = "Min";
			this.label102.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numMissTimeMin
			// 
			this.numMissTimeMin.Location = new System.Drawing.Point(48, 424);
			this.numMissTimeMin.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numMissTimeMin.Name = "numMissTimeMin";
			this.numMissTimeMin.Size = new System.Drawing.Size(48, 20);
			this.numMissTimeMin.TabIndex = 5;
			this.numMissTimeMin.Leave += new System.EventHandler(this.numMissTimeMin_Leave);
			// 
			// label101
			// 
			this.label101.Location = new System.Drawing.Point(24, 408);
			this.label101.Name = "label101";
			this.label101.Size = new System.Drawing.Size(72, 16);
			this.label101.TabIndex = 4;
			this.label101.Text = "Time Limit";
			// 
			// label100
			// 
			this.label100.Location = new System.Drawing.Point(24, 368);
			this.label100.Name = "label100";
			this.label100.Size = new System.Drawing.Size(88, 16);
			this.label100.TabIndex = 3;
			this.label100.Text = "Mission Type";
			// 
			// cboMissType
			// 
			this.cboMissType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMissType.Location = new System.Drawing.Point(24, 384);
			this.cboMissType.Name = "cboMissType";
			this.cboMissType.Size = new System.Drawing.Size(121, 21);
			this.cboMissType.TabIndex = 2;
			this.cboMissType.Leave += new System.EventHandler(this.cboMissType_Leave);
			// 
			// label97
			// 
			this.label97.Location = new System.Drawing.Point(16, 16);
			this.label97.Name = "label97";
			this.label97.Size = new System.Drawing.Size(112, 16);
			this.label97.TabIndex = 1;
			this.label97.Text = "Mission Description";
			// 
			// txtMissDesc
			// 
			this.txtMissDesc.BackColor = System.Drawing.Color.Black;
			this.txtMissDesc.ForeColor = System.Drawing.Color.DodgerBlue;
			this.txtMissDesc.Location = new System.Drawing.Point(16, 32);
			this.txtMissDesc.MaxLength = 1024;
			this.txtMissDesc.Multiline = true;
			this.txtMissDesc.Name = "txtMissDesc";
			this.txtMissDesc.Size = new System.Drawing.Size(236, 328);
			this.txtMissDesc.TabIndex = 0;
			this.txtMissDesc.Leave += new System.EventHandler(this.txtMissDesc_Leave);
			// 
			// txtMissSucc
			// 
			this.txtMissSucc.BackColor = System.Drawing.Color.Black;
			this.txtMissSucc.Enabled = false;
			this.txtMissSucc.ForeColor = System.Drawing.Color.Lime;
			this.txtMissSucc.Location = new System.Drawing.Point(270, 32);
			this.txtMissSucc.MaxLength = 4096;
			this.txtMissSucc.Multiline = true;
			this.txtMissSucc.Name = "txtMissSucc";
			this.txtMissSucc.Size = new System.Drawing.Size(236, 328);
			this.txtMissSucc.TabIndex = 0;
			this.txtMissSucc.Leave += new System.EventHandler(this.txtMissSucc_Leave);
			// 
			// txtMissFail
			// 
			this.txtMissFail.BackColor = System.Drawing.Color.Black;
			this.txtMissFail.Enabled = false;
			this.txtMissFail.ForeColor = System.Drawing.Color.Red;
			this.txtMissFail.Location = new System.Drawing.Point(524, 32);
			this.txtMissFail.MaxLength = 4096;
			this.txtMissFail.Multiline = true;
			this.txtMissFail.Name = "txtMissFail";
			this.txtMissFail.Size = new System.Drawing.Size(236, 328);
			this.txtMissFail.TabIndex = 0;
			this.txtMissFail.Leave += new System.EventHandler(this.txtMissFail_Leave);
			// 
			// label98
			// 
			this.label98.Location = new System.Drawing.Point(270, 16);
			this.label98.Name = "label98";
			this.label98.Size = new System.Drawing.Size(176, 16);
			this.label98.TabIndex = 1;
			this.label98.Text = "Mission Successful Debrief (BoP)";
			// 
			// label99
			// 
			this.label99.Location = new System.Drawing.Point(524, 16);
			this.label99.Name = "label99";
			this.label99.Size = new System.Drawing.Size(152, 16);
			this.label99.TabIndex = 1;
			this.label99.Text = "Mission Failed Debrief (BoP)";
			// 
			// numMissTimeSec
			// 
			this.numMissTimeSec.Location = new System.Drawing.Point(128, 424);
			this.numMissTimeSec.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numMissTimeSec.Name = "numMissTimeSec";
			this.numMissTimeSec.Size = new System.Drawing.Size(48, 20);
			this.numMissTimeSec.TabIndex = 5;
			this.numMissTimeSec.Leave += new System.EventHandler(this.numMissTimeSec_Leave);
			// 
			// label103
			// 
			this.label103.Location = new System.Drawing.Point(96, 424);
			this.label103.Name = "label103";
			this.label103.Size = new System.Drawing.Size(32, 16);
			this.label103.TabIndex = 6;
			this.label103.Text = "Sec";
			this.label103.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// optBoP
			// 
			this.optBoP.Location = new System.Drawing.Point(88, 464);
			this.optBoP.Name = "optBoP";
			this.optBoP.Size = new System.Drawing.Size(56, 16);
			this.optBoP.TabIndex = 8;
			this.optBoP.Text = "BoP";
			// 
			// toolXvT
			// 
			this.toolXvT.AutoSize = false;
			this.toolXvT.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolNew,
            this.toolOpen,
            this.toolSave,
            this.toolSaveAs,
            this.toolSep1,
            this.toolNewItem,
            this.toolDeleteItem,
            this.toolCopy,
            this.toolPaste,
            this.toolSep2,
            this.toolMap,
            this.toolBriefing,
            this.toolVerify,
            this.toolSep3,
            this.toolOptions,
            this.toolLst,
            this.toolHelp});
			this.toolXvT.DropDownArrows = true;
			this.toolXvT.ImageList = this.imgToolbar;
			this.toolXvT.Location = new System.Drawing.Point(0, 0);
			this.toolXvT.Name = "toolXvT";
			this.toolXvT.ShowToolTips = true;
			this.toolXvT.Size = new System.Drawing.Size(794, 30);
			this.toolXvT.TabIndex = 1;
			this.toolXvT.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolXvT_ButtonClick);
			// 
			// toolNew
			// 
			this.toolNew.ImageIndex = 0;
			this.toolNew.Name = "toolNew";
			this.toolNew.ToolTipText = "New Mission";
			// 
			// toolOpen
			// 
			this.toolOpen.ImageIndex = 1;
			this.toolOpen.Name = "toolOpen";
			this.toolOpen.ToolTipText = "Open Mission";
			// 
			// toolSave
			// 
			this.toolSave.ImageIndex = 2;
			this.toolSave.Name = "toolSave";
			this.toolSave.ToolTipText = "Save Mission";
			// 
			// toolSaveAs
			// 
			this.toolSaveAs.ImageIndex = 3;
			this.toolSaveAs.Name = "toolSaveAs";
			this.toolSaveAs.ToolTipText = "Save As...";
			// 
			// toolSep1
			// 
			this.toolSep1.Name = "toolSep1";
			this.toolSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolNewItem
			// 
			this.toolNewItem.ImageIndex = 4;
			this.toolNewItem.Name = "toolNewItem";
			this.toolNewItem.ToolTipText = "New FlightGroup";
			// 
			// toolDeleteItem
			// 
			this.toolDeleteItem.ImageIndex = 5;
			this.toolDeleteItem.Name = "toolDeleteItem";
			this.toolDeleteItem.ToolTipText = "Delete FlightGroup";
			// 
			// toolCopy
			// 
			this.toolCopy.ImageIndex = 6;
			this.toolCopy.Name = "toolCopy";
			this.toolCopy.ToolTipText = "CopyFlightGroup";
			// 
			// toolPaste
			// 
			this.toolPaste.ImageIndex = 7;
			this.toolPaste.Name = "toolPaste";
			this.toolPaste.ToolTipText = "Paste FlightGroup";
			// 
			// toolSep2
			// 
			this.toolSep2.Name = "toolSep2";
			this.toolSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolMap
			// 
			this.toolMap.ImageIndex = 8;
			this.toolMap.Name = "toolMap";
			this.toolMap.ToolTipText = "Mission Map";
			// 
			// toolBriefing
			// 
			this.toolBriefing.ImageIndex = 9;
			this.toolBriefing.Name = "toolBriefing";
			this.toolBriefing.ToolTipText = "Briefing";
			// 
			// toolVerify
			// 
			this.toolVerify.ImageIndex = 10;
			this.toolVerify.Name = "toolVerify";
			this.toolVerify.ToolTipText = "Mission Verify";
			// 
			// toolSep3
			// 
			this.toolSep3.Name = "toolSep3";
			this.toolSep3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolOptions
			// 
			this.toolOptions.ImageIndex = 11;
			this.toolOptions.Name = "toolOptions";
			this.toolOptions.ToolTipText = "Options";
			// 
			// toolLst
			// 
			this.toolLst.ImageIndex = 12;
			this.toolLst.Name = "toolLst";
			this.toolLst.ToolTipText = "Edit .lst";
			// 
			// toolHelp
			// 
			this.toolHelp.ImageIndex = 13;
			this.toolHelp.Name = "toolHelp";
			this.toolHelp.ToolTipText = "Help";
			// 
			// menuXvT
			// 
			this.menuXvT.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuTools,
            this.menuTest,
            this.menuHelp});
			// 
			// menuFile
			// 
			this.menuFile.Index = 0;
			this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuNew,
            this.menuOpen,
            this.menuRecent,
            this.menuSave,
            this.menuSaveAs,
            this.menuItem23,
            this.menuExit});
			this.menuFile.Text = "&File";
			// 
			// menuNew
			// 
			this.menuNew.Index = 0;
			this.menuNew.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuNewXwing,
            this.menuNewTIE,
            this.menuNewXvT,
            this.menuNewBoP,
            this.menuNewXWA});
			this.menuNew.Text = "&New...";
			// 
			// menuNewXwing
			// 
			this.menuNewXwing.Index = 0;
			this.menuNewXwing.Text = "X-&wing mission";
			this.menuNewXwing.Click += new System.EventHandler(this.menuNewXwing_Click);
			// 
			// menuNewTIE
			// 
			this.menuNewTIE.Index = 1;
			this.menuNewTIE.Text = "&TIE mission";
			this.menuNewTIE.Click += new System.EventHandler(this.menuNewTIE_Click);
			// 
			// menuNewXvT
			// 
			this.menuNewXvT.Index = 2;
			this.menuNewXvT.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.menuNewXvT.Text = "X&vT mission";
			this.menuNewXvT.Click += new System.EventHandler(this.menuNewXvT_Click);
			// 
			// menuNewBoP
			// 
			this.menuNewBoP.Index = 3;
			this.menuNewBoP.Text = "&BoP mission";
			this.menuNewBoP.Click += new System.EventHandler(this.menuNewBoP_Click);
			// 
			// menuNewXWA
			// 
			this.menuNewXWA.Index = 4;
			this.menuNewXWA.Text = "&XWA mission";
			this.menuNewXWA.Click += new System.EventHandler(this.menuNewXWA_Click);
			// 
			// menuOpen
			// 
			this.menuOpen.Index = 1;
			this.menuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.menuOpen.Text = "&Open";
			this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
			// 
			// menuRecent
			// 
			this.menuRecent.Enabled = false;
			this.menuRecent.Index = 2;
			this.menuRecent.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuRec1,
            this.menuRec2,
            this.menuRec3,
            this.menuRec4,
            this.menuRec5});
			this.menuRecent.Text = "Open &Recent...";
			// 
			// menuRec1
			// 
			this.menuRec1.Index = 0;
			this.menuRec1.Text = "1.";
			this.menuRec1.Visible = false;
			// 
			// menuRec2
			// 
			this.menuRec2.Index = 1;
			this.menuRec2.Text = "2.";
			this.menuRec2.Visible = false;
			// 
			// menuRec3
			// 
			this.menuRec3.Index = 2;
			this.menuRec3.Text = "3.";
			this.menuRec3.Visible = false;
			// 
			// menuRec4
			// 
			this.menuRec4.Index = 3;
			this.menuRec4.Text = "4.";
			this.menuRec4.Visible = false;
			// 
			// menuRec5
			// 
			this.menuRec5.Index = 4;
			this.menuRec5.Text = "5.";
			this.menuRec5.Visible = false;
			// 
			// menuSave
			// 
			this.menuSave.Index = 3;
			this.menuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuSave.Text = "&Save";
			this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
			// 
			// menuSaveAs
			// 
			this.menuSaveAs.Index = 4;
			this.menuSaveAs.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuSaveAsTIE,
            this.menuSaveAsXvT,
            this.menuSaveAsBoP,
            this.menuSaveAsXWA});
			this.menuSaveAs.Text = "Save &as...";
			// 
			// menuSaveAsTIE
			// 
			this.menuSaveAsTIE.Index = 0;
			this.menuSaveAsTIE.Text = "&TIE mission";
			this.menuSaveAsTIE.Click += new System.EventHandler(this.menuSaveAsTIE_Click);
			// 
			// menuSaveAsXvT
			// 
			this.menuSaveAsXvT.Index = 1;
			this.menuSaveAsXvT.Text = "X&vT mission";
			this.menuSaveAsXvT.Click += new System.EventHandler(this.menuSaveAsXvT_Click);
			// 
			// menuSaveAsBoP
			// 
			this.menuSaveAsBoP.Index = 2;
			this.menuSaveAsBoP.Text = "&BoP mission";
			this.menuSaveAsBoP.Click += new System.EventHandler(this.menuSaveAsBoP_Click);
			// 
			// menuSaveAsXWA
			// 
			this.menuSaveAsXWA.Index = 3;
			this.menuSaveAsXWA.Text = "&XWA mission";
			this.menuSaveAsXWA.Click += new System.EventHandler(this.menuSaveAsXWA_Click);
			// 
			// menuItem23
			// 
			this.menuItem23.Index = 5;
			this.menuItem23.Text = "-";
			// 
			// menuExit
			// 
			this.menuExit.Index = 6;
			this.menuExit.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftX;
			this.menuExit.Text = "E&xit";
			this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
			// 
			// menuEdit
			// 
			this.menuEdit.Index = 1;
			this.menuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuUndo,
            this.menuItem14,
            this.menuCut,
            this.menuCopy,
            this.menuPaste,
            this.menuDelete});
			this.menuEdit.Text = "&Edit";
			// 
			// menuUndo
			// 
			this.menuUndo.Enabled = false;
			this.menuUndo.Index = 0;
			this.menuUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.menuUndo.Text = "&Undo";
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 1;
			this.menuItem14.Text = "-";
			// 
			// menuCut
			// 
			this.menuCut.Index = 2;
			this.menuCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.menuCut.Text = "Cu&t";
			// 
			// menuCopy
			// 
			this.menuCopy.Index = 3;
			this.menuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.menuCopy.Text = "&Copy";
			this.menuCopy.Click += new System.EventHandler(this.menuCopy_Click);
			// 
			// menuPaste
			// 
			this.menuPaste.Index = 4;
			this.menuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.menuPaste.Text = "&Paste";
			this.menuPaste.Click += new System.EventHandler(this.menuPaste_Click);
			// 
			// menuDelete
			// 
			this.menuDelete.Index = 5;
			this.menuDelete.Text = "&Delete";
			this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
			// 
			// menuTools
			// 
			this.menuTools.Index = 2;
			this.menuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuVerify,
            this.menuMap,
            this.menuBrief,
            this.menuLST,
            this.menuOptions,
            this.menuGoalSummary});
			this.menuTools.Text = "&Tools";
			// 
			// menuVerify
			// 
			this.menuVerify.Index = 0;
			this.menuVerify.Text = "&Verify Mission";
			this.menuVerify.Click += new System.EventHandler(this.menuVerify_Click);
			// 
			// menuMap
			// 
			this.menuMap.Index = 1;
			this.menuMap.Text = "&Map";
			this.menuMap.Click += new System.EventHandler(this.menuMap_Click);
			// 
			// menuBrief
			// 
			this.menuBrief.Index = 2;
			this.menuBrief.Text = "&Briefing";
			this.menuBrief.Click += new System.EventHandler(this.menuBrief_Click);
			// 
			// menuLST
			// 
			this.menuLST.Index = 3;
			this.menuLST.Text = "&LST";
			this.menuLST.Click += new System.EventHandler(this.menuLST_Click);
			// 
			// menuOptions
			// 
			this.menuOptions.Index = 4;
			this.menuOptions.Text = "&Options";
			this.menuOptions.Click += new System.EventHandler(this.menuOptions_Click);
			// 
			// menuGoalSummary
			// 
			this.menuGoalSummary.Index = 5;
			this.menuGoalSummary.Text = "FG &Goal Summary";
			this.menuGoalSummary.Click += new System.EventHandler(this.menuGoalSummary_Click);
			// 
			// menuTest
			// 
			this.menuTest.Index = 3;
			this.menuTest.Text = "Te&st";
			this.menuTest.Click += new System.EventHandler(this.menuTest_Click);
			// 
			// menuHelp
			// 
			this.menuHelp.Index = 4;
			this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuHelpInfo,
            this.menuAbout,
            this.menuIDMR,
            this.menuER});
			this.menuHelp.Text = "&Help";
			// 
			// menuHelpInfo
			// 
			this.menuHelpInfo.Index = 0;
			this.menuHelpInfo.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.menuHelpInfo.Text = "&Help";
			this.menuHelpInfo.Click += new System.EventHandler(this.menuHelpInfo_Click);
			// 
			// menuAbout
			// 
			this.menuAbout.Index = 1;
			this.menuAbout.Text = "&About";
			this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
			// 
			// menuIDMR
			// 
			this.menuIDMR.Index = 2;
			this.menuIDMR.Text = "&IDMR.ER.net";
			this.menuIDMR.Click += new System.EventHandler(this.menuIDMR_Click);
			// 
			// menuER
			// 
			this.menuER.Index = 3;
			this.menuER.Text = "&Empirereborn.net";
			this.menuER.Click += new System.EventHandler(this.menuER_Click);
			// 
			// opnXvT
			// 
			this.opnXvT.DefaultExt = "tie";
			this.opnXvT.Filter = "Mission Files|*.tie|X-wing Missions|*.xwi";
			// 
			// savXvT
			// 
			this.savXvT.DefaultExt = "tie";
			this.savXvT.FileName = "NewMission.tie";
			this.savXvT.Filter = "Mission Files|*.tie|X-wing Missions|*.xwi";
			this.savXvT.FileOk += new System.ComponentModel.CancelEventHandler(this.savXvT_FileOk);
			// 
			// dataWaypoints
			// 
			this.dataWaypoints.AllowDelete = false;
			this.dataWaypoints.AllowNew = false;
			// 
			// dataWaypoints_Raw
			// 
			this.dataWaypoints_Raw.AllowDelete = false;
			this.dataWaypoints_Raw.AllowNew = false;
			// 
			// XvtForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(794, 546);
			this.Controls.Add(this.toolXvT);
			this.Controls.Add(this.tabMain);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Menu = this.menuXvT;
			this.Name = "XvtForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Ye Olde Galactic Empire Mission Editor - XvT";
			this.Activated += new System.EventHandler(this.form_Activated);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.form_Closing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.form_KeyDown);
			this.tabMain.ResumeLayout(false);
			this.tabFG.ResumeLayout(false);
			this.tabFGMinor.ResumeLayout(false);
			this.tabCraft.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numSC)).EndInit();
			this.grpCraft3.ResumeLayout(false);
			this.grpCraft3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numWaves)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numCraft)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numGG)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numGU)).EndInit();
			this.grpCraft2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numLead)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numSpacing)).EndInit();
			this.grpCraft4.ResumeLayout(false);
			this.grpCraft4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numBackdrop)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numExplode)).EndInit();
			this.tabArrDep.ResumeLayout(false);
			this.grpDep.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numDepClockSec)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numDepClockMin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numDepMin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numDepSec)).EndInit();
			this.groupBox10.ResumeLayout(false);
			this.groupBox9.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numArrSec)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numArrMin)).EndInit();
			this.panel10.ResumeLayout(false);
			this.panel9.ResumeLayout(false);
			this.groupBox11.ResumeLayout(false);
			this.groupBox12.ResumeLayout(false);
			this.tabGoals.ResumeLayout(false);
			this.tabGoals.PerformLayout();
			this.grpGoal.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numGoalPoints)).EndInit();
			this.groupBox16.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numGoalTimeLimit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numGoalTeam)).EndInit();
			this.tabWP.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numRoll)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPitch)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numYaw)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWP)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWP_Raw)).EndInit();
			this.tabOrders.ResumeLayout(false);
			this.tabOrders.PerformLayout();
			this.grpSecOrder.ResumeLayout(false);
			this.grpPrimOrder.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numOVar1)).EndInit();
			this.groupBox15.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numOVar2)).EndInit();
			this.tapOption.ResumeLayout(false);
			this.grpRole.ResumeLayout(false);
			this.groupBox23.ResumeLayout(false);
			this.groupBox22.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numOptWaves)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numOptCraft)).EndInit();
			this.groupBox21.ResumeLayout(false);
			this.groupBox20.ResumeLayout(false);
			this.groupBox19.ResumeLayout(false);
			this.tabUnk.ResumeLayout(false);
			this.grpUnkOther.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numUnk20)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk21)).EndInit();
			this.grpUnkOrder.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numUnk6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnkOrder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk9)).EndInit();
			this.grpUnkAD.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numUnk5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk3)).EndInit();
			this.grpUnkCraft.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numUnk1)).EndInit();
			this.grpUnkGoal.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numUnkGoal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk13)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk16)).EndInit();
			this.tabMess.ResumeLayout(false);
			this.tabMess.PerformLayout();
			this.grpMessages.ResumeLayout(false);
			this.panel8.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numMessDelay)).EndInit();
			this.grpSend.ResumeLayout(false);
			this.tabGlob.ResumeLayout(false);
			this.tabGlob.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numGlobalPoints)).EndInit();
			this.groupBox18.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.tabTeam.ResumeLayout(false);
			this.tabTeam.PerformLayout();
			this.groupBox32.ResumeLayout(false);
			this.groupBox33.ResumeLayout(false);
			this.groupBox33.PerformLayout();
			this.groupBox34.ResumeLayout(false);
			this.groupBox34.PerformLayout();
			this.groupBox35.ResumeLayout(false);
			this.groupBox35.PerformLayout();
			this.groupBox31.ResumeLayout(false);
			this.groupBox30.ResumeLayout(false);
			this.tabMission.ResumeLayout(false);
			this.tabMission.PerformLayout();
			this.grpIFF.ResumeLayout(false);
			this.grpIFF.PerformLayout();
			this.groupBox36.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numMissUnk1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numMissUnk2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numMissTimeMin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numMissTimeSec)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWaypoints)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWaypoints_Raw)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		ComboBox cboPF2Color;
		ComboBox cboPF1Color;
		ComboBox cboSC2Color;
		ComboBox cboSC1Color;
		ComboBox cboPC2Color;
		ComboBox cboPC1Color;
		Label label111;
		ComboBox cboTeam;
		Panel panel1;
		Panel panel2;
		Panel panel3;
		Panel panel4;
		Panel panel5;
		Panel panel6;
		Panel panel7;
		Panel panel8;
		Panel panel9;
		Panel panel10;
		Label label112;
		ComboBox cboGlobalTeam;
		Label label109;
		ComboBox cboMessAmount;
		ComboBox cboMessType;
		ComboBox cboMessVar;
		ComboBox cboMessTrig;
		Label label110;
		ComboBox cboMessColor;
		Label lblTeam1;
		GroupBox groupBox30;
		Label lblTeam2;
		Label lblTeam3;
		Label lblTeam4;
		Label lblTeam5;
		Label lblTeam6;
		Label lblTeam7;
		Label lblTeam8;
		Label lblTeam9;
		Label lblTeam10;
		CheckBox chkTeam1;
		GroupBox groupBox31;
		CheckBox chkTeam2;
		CheckBox chkTeam3;
		CheckBox chkTeam4;
		CheckBox chkTeam5;
		CheckBox chkTeam6;
		CheckBox chkTeam7;
		CheckBox chkTeam8;
		CheckBox chkTeam9;
		CheckBox chkTeam10;
		Label label96;
		TextBox txtTeamName;
		GroupBox groupBox32;
		GroupBox groupBox33;
		TextBox txtPrimFail1;
		TextBox txtPrimFail2;
		GroupBox groupBox34;
		TextBox txtSecComp1;
		TextBox txtSecComp2;
		GroupBox groupBox35;
		TextBox txtPrimComp1;
		TextBox txtPrimComp2;
		TextBox txtMissDesc;
		TextBox txtMissSucc;
		TextBox txtMissFail;
		Label label97;
		Label label98;
		Label label99;
		ComboBox cboMissType;
		Label label100;
		Label label101;
		NumericUpDown numMissTimeMin;
		Label label102;
		NumericUpDown numMissTimeSec;
		Label label103;
		Label label104;
		RadioButton optXvT;
		RadioButton optBoP;
		GroupBox groupBox36;
		Label label105;
		NumericUpDown numMissUnk1;
		Label label106;
		NumericUpDown numMissUnk2;
		CheckBox chkMissUnk3;
		CheckBox chkPreventOutcome;
		NumericUpDown numUnk1;
		CheckBox chkUnk2;
		Label label83;
		Label label85;
		NumericUpDown numUnk3;
		NumericUpDown numUnk4;
		Label label86;
		NumericUpDown numUnk5;
		Label label87;
		Label label88;
		NumericUpDown numUnkOrder;
		GroupBox grpUnkCraft;
		GroupBox grpUnkAD;
		GroupBox grpUnkOrder;
		NumericUpDown numUnk6;
		Label label81;
		NumericUpDown numUnk7;
		Label label84;
		NumericUpDown numUnk8;
		Label label89;
		NumericUpDown numUnk9;
		Label label90;
		GroupBox grpUnkGoal;
		NumericUpDown numUnkGoal;
		Label label92;
		NumericUpDown numUnk13;
		Label label95;
		CheckBox chkUnk10;
		CheckBox chkUnk11;
		CheckBox chkUnk12;
		CheckBox chkUnk14;
		NumericUpDown numUnk16;
		Label label91;
		CheckBox chkUnk15;
		GroupBox grpUnkOther;
		CheckBox chkUnk17;
		CheckBox chkUnk18;
		CheckBox chkUnk19;
		NumericUpDown numUnk20;
		Label label93;
		NumericUpDown numUnk21;
		Label label94;
		CheckBox chkUnk23;
		CheckBox chkUnk22;
		CheckBox chkUnk24;
		CheckBox chkUnk25;
		CheckBox chkUnk28;
		CheckBox chkUnk29;
		CheckBox chkUnk26;
		CheckBox chkUnk27;
        GroupBox grpRole;
        ComboBox cboRole2;
		ComboBox cboRole3;
        ComboBox cboRole4;
        ComboBox cboRoleTeam1;
		ComboBox cboRole1;
		GroupBox groupBox22;
		ComboBox cboOptCat;
		Label label68;
		Label label69;
		NumericUpDown numOptWaves;
		Label label70;
		NumericUpDown numOptCraft;
		ComboBox cboOptCraft;
		Label lblOptCraft1;
		Label lblOptCraft2;
		Label lblOptCraft3;
		Label lblOptCraft4;
		Label lblOptCraft5;
		Label lblOptCraft6;
		Label lblOptCraft7;
		Label lblOptCraft8;
		Label lblOptCraft9;
		Label lblOptCraft10;
		GroupBox groupBox23;
		RadioButton optSkipAND;
		RadioButton optSkipOR;
		Label lblSkipTrig1;
		Label lblSkipTrig2;
		Button cmdCopySkip;
		Label label71;
		ComboBox cboSkipAmount;
		ComboBox cboSkipType;
		ComboBox cboSkipVar;
		ComboBox cboSkipTrig;
		Label label72;
		Button cmdPasteSkip;
		Label label60;
		Label label61;
		ComboBox cboGoalAmount;
		ComboBox cboGoalArgument;
		ComboBox cboGoalTrigger;
		Label label62;
		Label label63;
		Label label64;
		TextBox txtGoalInc;
		TextBox txtGoalComp;
		TextBox txtGoalFail;
		Label label65;
		NumericUpDown numGoalPoints;
		CheckBox chkGoalEnable;
		Label label66;
		NumericUpDown numGoalTeam;
		GroupBox grpGoal;
		Label label67;
		NumericUpDown numExplode;
		Label lblExplode;
		GroupBox groupBox19;
		CheckBox chkOptWNone;
		CheckBox chkOptWBomb;
		CheckBox chkOptWRocket;
		CheckBox chkOptWMissile;
		CheckBox chkOptWTorp;
		CheckBox chkOptWAdvMissile;
		CheckBox chkOptWAdvTorp;
		CheckBox chkOptWMagPulse;
        CheckBox chkOptWIonPulse;
		GroupBox groupBox20;
		CheckBox chkOptBNone;
		CheckBox chkOptBTractor;
		CheckBox chkOptBJamming;
		GroupBox groupBox21;
		CheckBox chkOptCNone;
		CheckBox chkOptCChaff;
		CheckBox chkOptCFlare;
        CheckBox chkOptCCluster;
        CheckBox chkOptBDecoy;
        CheckBox chkOptBEnergy;
        GroupBox groupBox16;
		Label lblGoal1;
		Label lblGoal2;
		Label lblGoal3;
		Label lblGoal4;
		Label lblGoal5;
		Label lblGoal8;
		Label lblGoal6;
		Label lblGoal7;
		ToolBar toolXvT;
		MainMenu menuXvT;
		MenuItem menuItem14;
		MenuItem menuItem23;
		MenuItem menuNew;
		MenuItem menuOpen;
		MenuItem menuSave;
		MenuItem menuSaveAs;
		MenuItem menuNewTIE;
		MenuItem menuNewXvT;
		MenuItem menuNewBoP;
		MenuItem menuNewXWA;
		MenuItem menuExit;
		MenuItem menuSaveAsTIE;
		MenuItem menuSaveAsXvT;
		MenuItem menuSaveAsBoP;
		MenuItem menuSaveAsXWA;
		MenuItem menuFile;
		MenuItem menuEdit;
		MenuItem menuTools;
		MenuItem menuHelp;
		MenuItem menuHelpInfo;
		MenuItem menuAbout;
		MenuItem menuIDMR;
		MenuItem menuER;
		MenuItem menuVerify;
		MenuItem menuMap;
		MenuItem menuBrief;
		MenuItem menuOptions;
		MenuItem menuUndo;
		MenuItem menuCut;
		MenuItem menuCopy;
		MenuItem menuPaste;
		MenuItem menuDelete;
		ImageList imgToolbar;
		OpenFileDialog opnXvT;
		SaveFileDialog savXvT;
		System.Data.DataView dataWaypoints;
		System.Data.DataView dataWaypoints_Raw;
		ToolBarButton toolNew;
		ToolBarButton toolOpen;
		ToolBarButton toolSave;
		ToolBarButton toolSaveAs;
		ToolBarButton toolSep1;
		ToolBarButton toolNewItem;
		ToolBarButton toolDeleteItem;
		ToolBarButton toolCopy;
		ToolBarButton toolPaste;
		ToolBarButton toolSep2;
		ToolBarButton toolMap;
		ToolBarButton toolBriefing;
		ToolBarButton toolVerify;
		ToolBarButton toolSep3;
		ToolBarButton toolOptions;
		ToolBarButton toolHelp;
		ToolBarButton toolLst;
		TabControl tabMain;
		TabPage tabMess;
		TabPage tabGlob;
		TabPage tabTeam;
		TabPage tabMission;
		Label label1;
		ListBox lstFG;
		TabControl tabFGMinor;
		TabPage tabFG;
		TabPage tabCraft;
		TabPage tabArrDep;
		TabPage tabGoals;
		TabPage tabWP;
		TabPage tabOrders;
		TabPage tabUnk;
		Label lblFG;
		Label lblStarting;
		GroupBox grpCraft2;
		ComboBox cboCraft;
		NumericUpDown numLead;
		Label label7;
		Label label8;
		Label label9;
		Label label13;
		Label label14;
		Label label15;
		Label label16;
		Label label17;
		NumericUpDown numSpacing;
		ComboBox cboIFF;
		ComboBox cboAI;
		ComboBox cboMarkings;
		ComboBox cboPlayer;
		ComboBox cboFormation;
		Button cmdForms;
		GroupBox grpCraft4;
		Label label18;
		ComboBox cboWarheads;
		Label lblStatus;
		ComboBox cboBeam;
		ComboBox cboStatus;
		Label label20;
		ComboBox cboStatus2;
		Label label2;
		GroupBox grpCraft3;
		NumericUpDown numWaves;
		Label label10;
		Label label11;
		Label label12;
		NumericUpDown numCraft;
		NumericUpDown numGG;
		NumericUpDown numGU;
		Label label3;
		Label label4;
		ComboBox cboRadio;
		ComboBox cboCounter;
		Label label5;
		TabPage tapOption;
		GroupBox groupBox1;
		CheckBox chkRandSC;
		TextBox txtName;
		Label lblNotUsed;
		TextBox txtSpecCargo;
		NumericUpDown numSC;
		Label label6;
		TextBox txtCargo;
		Label label22;
		Label label23;
		Label label24;
		ListBox lstMessages;
		Label lblMessage;
		TextBox txtMessage;
		Label label52;
		Label label53;
		TextBox txtShort;
		NumericUpDown numMessDelay;
		Label label55;
		GroupBox grpMessages;
		Label lblMess1;
		Label lblMess2;
		RadioButton optMess1OR2;
		RadioButton optMess1AND2;
		RadioButton optMess3OR4;
		Label lblMess4;
		Label lblMess3;
		RadioButton optMess3AND4;
		RadioButton optMess12AND34;
		RadioButton optMess12OR34;
		GroupBox groupBox18;
		Label lblPrim1;
		Label lblPrim2;
		RadioButton optPrim1OR2;
		RadioButton optPrim1AND2;
		Label lblPrim4;
		Label lblPrim3;
		RadioButton optPrim3OR4;
		RadioButton optPrim3AND4;
		RadioButton optPrim12AND34;
		RadioButton optPrim12OR34;
		GroupBox groupBox5;
		Label lblPrev1;
		Label lblPrev2;
		RadioButton optPrev1OR2;
		RadioButton optPrev1AND2;
		Label lblPrev4;
		Label lblPrev3;
		RadioButton optPrev3OR4;
		RadioButton optPrev3AND4;
		RadioButton optPrev12AND34;
		RadioButton optPrev12OR34;
		GroupBox groupBox6;
		Label lblSec1;
		Label lblSec2;
		RadioButton optSec1OR2;
		RadioButton optSec1AND2;
		Label lblSec4;
		Label lblSec3;
		RadioButton optSec3OR4;
		RadioButton optSec3AND4;
		RadioButton optSec12AND34;
		RadioButton optSec12OR34;
		Label label79;
		Label label48;
		ComboBox cboGlobalAmount;
		ComboBox cboGlobalType;
		ComboBox cboGlobalVar;
		ComboBox cboGlobalTrig;
		Label label59;
		NumericUpDown numGlobalPoints;
		Label label32;
		TextBox txtGlobalInc;
		TextBox txtGlobalComp;
		TextBox txtGlobalFail;
		Label label33;
		Label label34;
		Label label35;
		CheckBox chkMess1;
		CheckBox chkMess2;
		CheckBox chkMess3;
		CheckBox chkMess4;
		CheckBox chkMess5;
		CheckBox chkMess10;
		CheckBox chkMess9;
		CheckBox chkMess8;
		CheckBox chkMess7;
		CheckBox chkMess6;
		Label label76;
		NumericUpDown numRoll;
		NumericUpDown numPitch;
		NumericUpDown numYaw;
		Label label56;
		DataGrid dataWP;
		DataGrid dataWP_Raw;
		CheckBox chkWPBrief1;
		CheckBox chkWPHyp;
		CheckBox chkWP8;
		CheckBox chkWP7;
		CheckBox chkWP2;
		CheckBox chkWP1;
		CheckBox chkSP4;
		CheckBox chkSP3;
		CheckBox chkSP2;
		CheckBox chkSP1;
		CheckBox chkWP6;
		CheckBox chkWP5;
		CheckBox chkWP4;
		CheckBox chkWP3;
		CheckBox chkWPRend;
		Label label77;
		Label label78;
		CheckBox chkWPBrief2;
		CheckBox chkWPBrief3;
		CheckBox chkWPBrief4;
		CheckBox chkWPBrief5;
		CheckBox chkWPBrief6;
		CheckBox chkWPBrief7;
		CheckBox chkWPBrief8;
		GroupBox grpSend;
		Button cmdCopyAD;
		Label label36;
		GroupBox grpDep;
		Label label47;
		Label label41;
		Label label40;
		Label label39;
		Label label37;
		GroupBox groupBox10;
		RadioButton optDepMSAlt;
		RadioButton optDepHypAlt;
		ComboBox cboDepMSAlt;
		GroupBox groupBox9;
		RadioButton optDepHyp;
		ComboBox cboDepMS;
		RadioButton optDepMS;
		Label lblDep1;
		ComboBox cboAbort;
		GroupBox groupBox8;
		Label label38;
		Label lblArr1;
		GroupBox groupBox11;
		RadioButton optArrHypAlt;
		ComboBox cboArrMSAlt;
		RadioButton optArrMSAlt;
		GroupBox groupBox12;
		ComboBox cboArrMS;
		RadioButton optArrHyp;
		RadioButton optArrMS;
		Label lblArr2;
		Label label42;
		Label label43;
		ComboBox cboADTrigAmount;
		ComboBox cboADTrigType;
		ComboBox cboADTrigVar;
		ComboBox cboADTrig;
		Label label44;
		ComboBox cboDiff;
		Label label45;
		Label label46;
		Button cmdPasteAD;
		Label lblDep2;
		RadioButton optDepAND;
		RadioButton optDepOR;
		RadioButton optArr1AND2;
		RadioButton optArr1OR2;
		Label lblArr3;
		Label lblArr4;
		RadioButton optArr3AND4;
		RadioButton optArr3OR4;
		RadioButton optArr12AND34;
		RadioButton optArr12OR34;
		Button cmdCopyOrder;
		Label lblODesc;
		GroupBox grpSecOrder;
		Label label49;
		RadioButton optOT3T4OR;
		ComboBox cboOT3;
		ComboBox cboOT3Type;
		ComboBox cboOT4Type;
		ComboBox cboOT4;
		RadioButton optOT3T4AND;
		GroupBox grpPrimOrder;
		Label label50;
		RadioButton optOT1T2OR;
		ComboBox cboOT1;
		ComboBox cboOT1Type;
		ComboBox cboOT2Type;
		ComboBox cboOT2;
		RadioButton optOT1T2AND;
		NumericUpDown numOVar1;
		Label lblOVar1;
		ComboBox cboOThrottle;
		Label label51;
		GroupBox groupBox15;
		Label lblOrder1;
		Label lblOrder2;
		Label lblOrder3;
		ComboBox cboOrders;
		Label lblOVar2;
		NumericUpDown numOVar2;
		Button cmdPasteOrder;
		Label lblOrder4;
		Label label54;
		TextBox txtOString;
        Label label57;
		CheckBox chkArrHuman;
		Label label58;
		ComboBox cboPosition;
		NumericUpDown numDepMin;
		NumericUpDown numDepSec;
		NumericUpDown numArrSec;
		NumericUpDown numArrMin;
		private MenuItem menuLST;
		private Button cmdBackdrop;
		private NumericUpDown numBackdrop;
		private Label label122;
		private MenuItem menuRecent;
		private MenuItem menuRec1;
		private MenuItem menuRec2;
		private MenuItem menuRec3;
		private MenuItem menuRec4;
		private MenuItem menuRec5;
		private MenuItem menuTest;
        private MenuItem menuGoalSummary;
		private MenuItem menuNewXwing;
        private CheckBox chkPreventNumbering;
        private Label lblGoalTimeLimit;
        private Label label19;
        private NumericUpDown numGoalTimeLimit;
        private ComboBox cboRoleTeam4;
        private ComboBox cboRoleTeam3;
        private ComboBox cboRoleTeam2;
        private Button cmdMoveFGUp;
        private Button cmdMoveFGDown;
        private Button cmdMoveMessDown;
        private Button cmdMoveMessUp;
        private Label label25;
        private Label label21;
        private NumericUpDown numDepClockSec;
        private NumericUpDown numDepClockMin;
        private Label lblOVar1Note;
        private Label lblOSpeedNote;
        private Label label150;
        private Label lblOVar2Note;
        private ComboBox cboOSpeed;
        private GroupBox grpIFF;
        private Label lblIFF3;
        private Label lblIFF4;
        private Label lblIFF5;
        private Label lblIFF6;
        private TextBox txtIFF3;
        private TextBox txtIFF4;
        private TextBox txtIFF5;
        private TextBox txtIFF6;
    }
}