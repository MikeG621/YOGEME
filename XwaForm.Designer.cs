using System.Windows.Forms;

namespace Idmr.Yogeme
{
	partial class XwaForm
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
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XwaForm));
            this.menuXWA = new System.Windows.Forms.MainMenu(this.components);
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
            this.menuLibrary = new System.Windows.Forms.MenuItem();
            this.menuTest = new System.Windows.Forms.MenuItem();
            this.menuHyperbuoy = new System.Windows.Forms.MenuItem();
            this.menuSuperBackdrops = new System.Windows.Forms.MenuItem();
            this.menuHooks = new System.Windows.Forms.MenuItem();
            this.menuWav = new System.Windows.Forms.MenuItem();
            this.menuMissionCraft = new System.Windows.Forms.MenuItem();
            this.menuGlobalSummary = new System.Windows.Forms.MenuItem();
            this.menuHelp = new System.Windows.Forms.MenuItem();
            this.menuHelpInfo = new System.Windows.Forms.MenuItem();
            this.menuAbout = new System.Windows.Forms.MenuItem();
            this.menuIDMR = new System.Windows.Forms.MenuItem();
            this.menuER = new System.Windows.Forms.MenuItem();
            this.imgToolbar = new System.Windows.Forms.ImageList(this.components);
            this.toolXWA = new System.Windows.Forms.ToolBar();
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
            this.toolWav = new System.Windows.Forms.ToolBarButton();
            this.toolHelp = new System.Windows.Forms.ToolBarButton();
            this.opnXWA = new System.Windows.Forms.OpenFileDialog();
            this.savXWA = new System.Windows.Forms.SaveFileDialog();
            this.dataWaypoints = new System.Data.DataView();
            this.dataWaypoints_Raw = new System.Data.DataView();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabFG = new System.Windows.Forms.TabPage();
            this.numFilterRegion = new System.Windows.Forms.NumericUpDown();
            this.chkRegionFilter = new System.Windows.Forms.CheckBox();
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
            this.lblCargo = new System.Windows.Forms.Label();
            this.txtCargo = new System.Windows.Forms.TextBox();
            this.lblSC = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.grpCraft3 = new System.Windows.Forms.GroupBox();
            this.lblWaveDelay = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.numWaveDelay = new System.Windows.Forms.NumericUpDown();
            this.chkGU = new System.Windows.Forms.CheckBox();
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
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
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
            this.cboGlobSpecCargo = new System.Windows.Forms.ComboBox();
            this.cboGlobCargo = new System.Windows.Forms.ComboBox();
            this.numBackdrop = new System.Windows.Forms.NumericUpDown();
            this.label143 = new System.Windows.Forms.Label();
            this.lblGC = new System.Windows.Forms.Label();
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
            this.cmdMissionCraft = new System.Windows.Forms.Button();
            this.cboADPara = new System.Windows.Forms.ComboBox();
            this.chkArrHuman = new System.Windows.Forms.CheckBox();
            this.cmdCopyAD = new System.Windows.Forms.Button();
            this.label36 = new System.Windows.Forms.Label();
            this.grpDep = new System.Windows.Forms.GroupBox();
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
            this.optDepRegion = new System.Windows.Forms.RadioButton();
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
            this.optArrRegion = new System.Windows.Forms.RadioButton();
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
            this.lblGoalTimeLimitNote = new System.Windows.Forms.Label();
            this.lblGoalTimeLimitSec = new System.Windows.Forms.Label();
            this.lblGoalTimeLimit = new System.Windows.Forms.Label();
            this.numGoalTimeLimit = new System.Windows.Forms.NumericUpDown();
            this.grpGoal = new System.Windows.Forms.GroupBox();
            this.cboGoalPara = new System.Windows.Forms.ComboBox();
            this.label31 = new System.Windows.Forms.Label();
            this.numGoalActSeq = new System.Windows.Forms.NumericUpDown();
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
            this.numGoalTeam = new System.Windows.Forms.NumericUpDown();
            this.tabWP = new System.Windows.Forms.TabPage();
            this.numWPOrder = new System.Windows.Forms.NumericUpDown();
            this.label150 = new System.Windows.Forms.Label();
            this.numWPOrderRegion = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.cmdCopyOrderWP = new System.Windows.Forms.Button();
            this.cmdPasteOrderWP = new System.Windows.Forms.Button();
            this.numHYP = new System.Windows.Forms.NumericUpDown();
            this.numWPCapture = new System.Windows.Forms.NumericUpDown();
            this.numSP2 = new System.Windows.Forms.NumericUpDown();
            this.numSP1 = new System.Windows.Forms.NumericUpDown();
            this.label76 = new System.Windows.Forms.Label();
            this.numRoll = new System.Windows.Forms.NumericUpDown();
            this.numPitch = new System.Windows.Forms.NumericUpDown();
            this.numYaw = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.dataO_Raw = new System.Windows.Forms.DataGrid();
            this.dataO = new System.Windows.Forms.DataGrid();
            this.dataWP = new System.Windows.Forms.DataGrid();
            this.dataWP_Raw = new System.Windows.Forms.DataGrid();
            this.chkWPHyp = new System.Windows.Forms.CheckBox();
            this.chkWP8 = new System.Windows.Forms.CheckBox();
            this.chkWP7 = new System.Windows.Forms.CheckBox();
            this.chkWP2 = new System.Windows.Forms.CheckBox();
            this.chkWP1 = new System.Windows.Forms.CheckBox();
            this.chkWPCapture = new System.Windows.Forms.CheckBox();
            this.chkSP2 = new System.Windows.Forms.CheckBox();
            this.chkSP1 = new System.Windows.Forms.CheckBox();
            this.chkWP6 = new System.Windows.Forms.CheckBox();
            this.chkWP5 = new System.Windows.Forms.CheckBox();
            this.chkWP4 = new System.Windows.Forms.CheckBox();
            this.chkWP3 = new System.Windows.Forms.CheckBox();
            this.label77 = new System.Windows.Forms.Label();
            this.label78 = new System.Windows.Forms.Label();
            this.tabOrders = new System.Windows.Forms.TabPage();
            this.label95 = new System.Windows.Forms.Label();
            this.cboHandicap = new System.Windows.Forms.ComboBox();
            this.label104 = new System.Windows.Forms.Label();
            this.cboOSpeed = new System.Windows.Forms.ComboBox();
            this.lblOVar2Note = new System.Windows.Forms.Label();
            this.lblOVar3Note = new System.Windows.Forms.Label();
            this.lblOVar1Note = new System.Windows.Forms.Label();
            this.lblOSpeedNote = new System.Windows.Forms.Label();
            this.numORegion = new System.Windows.Forms.NumericUpDown();
            this.label103 = new System.Windows.Forms.Label();
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
            this.numOVar3 = new System.Windows.Forms.NumericUpDown();
            this.numOVar1 = new System.Windows.Forms.NumericUpDown();
            this.lblOVar3 = new System.Windows.Forms.Label();
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
            this.cboPilot = new System.Windows.Forms.ComboBox();
            this.label80 = new System.Windows.Forms.Label();
            this.grpRole = new System.Windows.Forms.GroupBox();
            this.cboRole2Teams = new System.Windows.Forms.ComboBox();
            this.cboRole1Teams = new System.Windows.Forms.ComboBox();
            this.label73 = new System.Windows.Forms.Label();
            this.txtRole = new System.Windows.Forms.TextBox();
            this.cboRole1 = new System.Windows.Forms.ComboBox();
            this.cboRole2 = new System.Windows.Forms.ComboBox();
            this.grpSkip = new System.Windows.Forms.GroupBox();
            this.cboSkipPara = new System.Windows.Forms.ComboBox();
            this.cboSkipOrder = new System.Windows.Forms.ComboBox();
            this.label74 = new System.Windows.Forms.Label();
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
            this.chkOptCCluster = new System.Windows.Forms.CheckBox();
            this.chkOptCNone = new System.Windows.Forms.CheckBox();
            this.chkOptCChaff = new System.Windows.Forms.CheckBox();
            this.chkOptCFlare = new System.Windows.Forms.CheckBox();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.chkOptBEnergy = new System.Windows.Forms.CheckBox();
            this.chkOptBNone = new System.Windows.Forms.CheckBox();
            this.chkOptBTractor = new System.Windows.Forms.CheckBox();
            this.chkOptBJamming = new System.Windows.Forms.CheckBox();
            this.chkOptBDecoy = new System.Windows.Forms.CheckBox();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.chkOptWNone = new System.Windows.Forms.CheckBox();
            this.chkOptWBomb = new System.Windows.Forms.CheckBox();
            this.chkOptWRocket = new System.Windows.Forms.CheckBox();
            this.chkOptWMissile = new System.Windows.Forms.CheckBox();
            this.chkOptWTorp = new System.Windows.Forms.CheckBox();
            this.chkOptWAdvMissile = new System.Windows.Forms.CheckBox();
            this.chkOptWAdvTorp = new System.Windows.Forms.CheckBox();
            this.chkOptWIonPulse = new System.Windows.Forms.CheckBox();
            this.chkOptWMagPulse = new System.Windows.Forms.CheckBox();
            this.tabArrDep2 = new System.Windows.Forms.TabPage();
            this.grpMoreDep = new System.Windows.Forms.GroupBox();
            this.label94 = new System.Windows.Forms.Label();
            this.label93 = new System.Windows.Forms.Label();
            this.label92 = new System.Windows.Forms.Label();
            this.numDepClockSec = new System.Windows.Forms.NumericUpDown();
            this.numDepClockMin = new System.Windows.Forms.NumericUpDown();
            this.grpMoreArrival = new System.Windows.Forms.GroupBox();
            this.numArrRandMin = new System.Windows.Forms.NumericUpDown();
            this.numArrRandSec = new System.Windows.Forms.NumericUpDown();
            this.lblRandomArrivalDelayMinutes = new System.Windows.Forms.Label();
            this.lblRandomArrivalDelaySeconds = new System.Windows.Forms.Label();
            this.lblRandomArrivalDelayDesc = new System.Windows.Forms.Label();
            this.cboStopArrivingWhen = new System.Windows.Forms.ComboBox();
            this.lblStopArrivingWhen = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstFG = new System.Windows.Forms.ListBox();
            this.tabMess = new System.Windows.Forms.TabPage();
            this.cboMessSpecial = new System.Windows.Forms.ComboBox();
            this.label106 = new System.Windows.Forms.Label();
            this.label105 = new System.Windows.Forms.Label();
            this.chkMessHeader = new System.Windows.Forms.CheckBox();
            this.numMessType = new System.Windows.Forms.NumericUpDown();
            this.lblDelay = new System.Windows.Forms.Label();
            this.cmdMoveMessDown = new System.Windows.Forms.Button();
            this.cmdMoveMessUp = new System.Windows.Forms.Button();
            this.cboMessPara = new System.Windows.Forms.ComboBox();
            this.cboMessFG = new System.Windows.Forms.ComboBox();
            this.txtVoice = new System.Windows.Forms.TextBox();
            this.grpMessCancel = new System.Windows.Forms.GroupBox();
            this.optMessC1AND2 = new System.Windows.Forms.RadioButton();
            this.optMessC1OR2 = new System.Windows.Forms.RadioButton();
            this.lblMess5 = new System.Windows.Forms.Label();
            this.lblMess6 = new System.Windows.Forms.Label();
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
            this.numMessDelay = new System.Windows.Forms.NumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtMessNote = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label149 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.lstMessages = new System.Windows.Forms.ListBox();
            this.tabGlob = new System.Windows.Forms.TabPage();
            this.lblGlobDelay = new System.Windows.Forms.Label();
            this.label113 = new System.Windows.Forms.Label();
            this.numGlobTrigPts = new System.Windows.Forms.NumericUpDown();
            this.label107 = new System.Windows.Forms.Label();
            this.cboGlobalPara = new System.Windows.Forms.ComboBox();
            this.label128 = new System.Windows.Forms.Label();
            this.numGlobActSeq = new System.Windows.Forms.NumericUpDown();
            this.numGlobDelay = new System.Windows.Forms.NumericUpDown();
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
            this.grpTeamPMF = new System.Windows.Forms.GroupBox();
            this.lblPMFDelay = new System.Windows.Forms.Label();
            this.cboPMFEomFG = new System.Windows.Forms.ComboBox();
            this.label116 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.txtPMFVoiceID = new System.Windows.Forms.TextBox();
            this.txtPrimFail1 = new System.Windows.Forms.TextBox();
            this.txtPrimFailNote = new System.Windows.Forms.TextBox();
            this.txtPrimFail2 = new System.Windows.Forms.TextBox();
            this.numPMFEomDelay = new System.Windows.Forms.NumericUpDown();
            this.grpTeamOMC = new System.Windows.Forms.GroupBox();
            this.lblOMCDelay = new System.Windows.Forms.Label();
            this.label115 = new System.Windows.Forms.Label();
            this.cboOMCEomFG = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.txtOMCVoiceID = new System.Windows.Forms.TextBox();
            this.txtOutstanding1 = new System.Windows.Forms.TextBox();
            this.txtOutstandingNote = new System.Windows.Forms.TextBox();
            this.txtOutstanding2 = new System.Windows.Forms.TextBox();
            this.numOMCEomDelay = new System.Windows.Forms.NumericUpDown();
            this.grpTeamPMC = new System.Windows.Forms.GroupBox();
            this.lblPMCDelay = new System.Windows.Forms.Label();
            this.label114 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.cboPMCEomFG = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPMCVoiceID = new System.Windows.Forms.TextBox();
            this.txtPrimComp1 = new System.Windows.Forms.TextBox();
            this.txtPrimCompNote = new System.Windows.Forms.TextBox();
            this.txtPrimComp2 = new System.Windows.Forms.TextBox();
            this.numPMCEomDelay = new System.Windows.Forms.NumericUpDown();
            this.txtTeamName = new System.Windows.Forms.TextBox();
            this.label96 = new System.Windows.Forms.Label();
            this.groupBox30 = new System.Windows.Forms.GroupBox();
            this.panel12 = new System.Windows.Forms.Panel();
            this.optAllies3 = new System.Windows.Forms.RadioButton();
            this.optAllies1 = new System.Windows.Forms.RadioButton();
            this.optAllies2 = new System.Windows.Forms.RadioButton();
            this.panel20 = new System.Windows.Forms.Panel();
            this.optAllies30 = new System.Windows.Forms.RadioButton();
            this.optAllies28 = new System.Windows.Forms.RadioButton();
            this.optAllies29 = new System.Windows.Forms.RadioButton();
            this.panel19 = new System.Windows.Forms.Panel();
            this.optAllies27 = new System.Windows.Forms.RadioButton();
            this.optAllies25 = new System.Windows.Forms.RadioButton();
            this.optAllies26 = new System.Windows.Forms.RadioButton();
            this.panel18 = new System.Windows.Forms.Panel();
            this.optAllies24 = new System.Windows.Forms.RadioButton();
            this.optAllies22 = new System.Windows.Forms.RadioButton();
            this.optAllies23 = new System.Windows.Forms.RadioButton();
            this.panel17 = new System.Windows.Forms.Panel();
            this.optAllies21 = new System.Windows.Forms.RadioButton();
            this.optAllies19 = new System.Windows.Forms.RadioButton();
            this.optAllies20 = new System.Windows.Forms.RadioButton();
            this.panel16 = new System.Windows.Forms.Panel();
            this.optAllies18 = new System.Windows.Forms.RadioButton();
            this.optAllies16 = new System.Windows.Forms.RadioButton();
            this.optAllies17 = new System.Windows.Forms.RadioButton();
            this.panel15 = new System.Windows.Forms.Panel();
            this.optAllies15 = new System.Windows.Forms.RadioButton();
            this.optAllies13 = new System.Windows.Forms.RadioButton();
            this.optAllies14 = new System.Windows.Forms.RadioButton();
            this.panel14 = new System.Windows.Forms.Panel();
            this.optAllies12 = new System.Windows.Forms.RadioButton();
            this.optAllies10 = new System.Windows.Forms.RadioButton();
            this.optAllies11 = new System.Windows.Forms.RadioButton();
            this.panel13 = new System.Windows.Forms.Panel();
            this.optAllies9 = new System.Windows.Forms.RadioButton();
            this.optAllies7 = new System.Windows.Forms.RadioButton();
            this.optAllies8 = new System.Windows.Forms.RadioButton();
            this.panel11 = new System.Windows.Forms.Panel();
            this.optAllies6 = new System.Windows.Forms.RadioButton();
            this.optAllies4 = new System.Windows.Forms.RadioButton();
            this.optAllies5 = new System.Windows.Forms.RadioButton();
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
            this.chkGoalsUnimportant = new System.Windows.Forms.CheckBox();
            this.label123 = new System.Windows.Forms.Label();
            this.txtFailNote = new System.Windows.Forms.TextBox();
            this.label82 = new System.Windows.Forms.Label();
            this.txtSuccNote = new System.Windows.Forms.TextBox();
            this.label75 = new System.Windows.Forms.Label();
            this.txtDescNote = new System.Windows.Forms.TextBox();
            this.pctLogo = new System.Windows.Forms.PictureBox();
            this.cboLogo = new System.Windows.Forms.ComboBox();
            this.cboFailOfficer = new System.Windows.Forms.ComboBox();
            this.cboWinOfficer = new System.Windows.Forms.ComboBox();
            this.cboOfficer = new System.Windows.Forms.ComboBox();
            this.label91 = new System.Windows.Forms.Label();
            this.label90 = new System.Windows.Forms.Label();
            this.label130 = new System.Windows.Forms.Label();
            this.label129 = new System.Windows.Forms.Label();
            this.chkEnd = new System.Windows.Forms.CheckBox();
            this.label102 = new System.Windows.Forms.Label();
            this.numMissTimeMin = new System.Windows.Forms.NumericUpDown();
            this.label100 = new System.Windows.Forms.Label();
            this.cboHangar = new System.Windows.Forms.ComboBox();
            this.label97 = new System.Windows.Forms.Label();
            this.txtMissDesc = new System.Windows.Forms.TextBox();
            this.txtMissSucc = new System.Windows.Forms.TextBox();
            this.txtMissFail = new System.Windows.Forms.TextBox();
            this.label98 = new System.Windows.Forms.Label();
            this.label99 = new System.Windows.Forms.Label();
            this.tabMission2 = new System.Windows.Forms.TabPage();
            this.groupBox40 = new System.Windows.Forms.GroupBox();
            this.label28 = new System.Windows.Forms.Label();
            this.cboGCVolatility = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.cboGCType = new System.Windows.Forms.ComboBox();
            this.numGCCount = new System.Windows.Forms.NumericUpDown();
            this.label29 = new System.Windows.Forms.Label();
            this.numGCValue = new System.Windows.Forms.NumericUpDown();
            this.label141 = new System.Windows.Forms.Label();
            this.numGCVolume = new System.Windows.Forms.NumericUpDown();
            this.label140 = new System.Windows.Forms.Label();
            this.txtGlobCargo = new System.Windows.Forms.TextBox();
            this.numGCIndex = new System.Windows.Forms.NumericUpDown();
            this.label138 = new System.Windows.Forms.Label();
            this.grpGlobalUnits = new System.Windows.Forms.GroupBox();
            this.chkGURandom = new System.Windows.Forms.CheckBox();
            this.numGUSpecial = new System.Windows.Forms.NumericUpDown();
            this.label86 = new System.Windows.Forms.Label();
            this.txtGUCargo = new System.Windows.Forms.TextBox();
            this.label87 = new System.Windows.Forms.Label();
            this.cboGULeader = new System.Windows.Forms.ComboBox();
            this.label88 = new System.Windows.Forms.Label();
            this.numGUIndex = new System.Windows.Forms.NumericUpDown();
            this.label89 = new System.Windows.Forms.Label();
            this.txtGUName = new System.Windows.Forms.TextBox();
            this.grpGlobalGroups = new System.Windows.Forms.GroupBox();
            this.chkGGRandom = new System.Windows.Forms.CheckBox();
            this.numGGSpecial = new System.Windows.Forms.NumericUpDown();
            this.label85 = new System.Windows.Forms.Label();
            this.txtGGCargo = new System.Windows.Forms.TextBox();
            this.label84 = new System.Windows.Forms.Label();
            this.cboGGLeader = new System.Windows.Forms.ComboBox();
            this.label83 = new System.Windows.Forms.Label();
            this.numGGIndex = new System.Windows.Forms.NumericUpDown();
            this.label81 = new System.Windows.Forms.Label();
            this.txtGGName = new System.Windows.Forms.TextBox();
            this.grpRegions = new System.Windows.Forms.GroupBox();
            this.label137 = new System.Windows.Forms.Label();
            this.label136 = new System.Windows.Forms.Label();
            this.label135 = new System.Windows.Forms.Label();
            this.label134 = new System.Windows.Forms.Label();
            this.txtRegion4 = new System.Windows.Forms.TextBox();
            this.txtRegion3 = new System.Windows.Forms.TextBox();
            this.txtRegion2 = new System.Windows.Forms.TextBox();
            this.txtRegion1 = new System.Windows.Forms.TextBox();
            this.groupBox37 = new System.Windows.Forms.GroupBox();
            this.label133 = new System.Windows.Forms.Label();
            this.label132 = new System.Windows.Forms.Label();
            this.label131 = new System.Windows.Forms.Label();
            this.label108 = new System.Windows.Forms.Label();
            this.txtIFF6 = new System.Windows.Forms.TextBox();
            this.txtIFF5 = new System.Windows.Forms.TextBox();
            this.txtIFF4 = new System.Windows.Forms.TextBox();
            this.txtIFF3 = new System.Windows.Forms.TextBox();
            this.label101 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.dataOrders = new System.Data.DataView();
            this.dataOrders_Raw = new System.Data.DataView();
            this.ttActiveSequence = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataWaypoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataWaypoints_Raw)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tabFG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterRegion)).BeginInit();
            this.tabFGMinor.SuspendLayout();
            this.tabCraft.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSC)).BeginInit();
            this.grpCraft3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWaveDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWaves)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCraft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGU)).BeginInit();
            this.grpCraft2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSpacing)).BeginInit();
            this.grpCraft4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBackdrop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExplode)).BeginInit();
            this.tabArrDep.SuspendLayout();
            this.grpDep.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.numGoalTimeLimit)).BeginInit();
            this.grpGoal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGoalActSeq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGoalPoints)).BeginInit();
            this.groupBox16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGoalTeam)).BeginInit();
            this.tabWP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWPOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWPOrderRegion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHYP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWPCapture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSP2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRoll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYaw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataO_Raw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataWP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataWP_Raw)).BeginInit();
            this.tabOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numORegion)).BeginInit();
            this.grpSecOrder.SuspendLayout();
            this.grpPrimOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOVar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOVar1)).BeginInit();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOVar2)).BeginInit();
            this.tapOption.SuspendLayout();
            this.grpRole.SuspendLayout();
            this.grpSkip.SuspendLayout();
            this.groupBox22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOptWaves)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOptCraft)).BeginInit();
            this.groupBox21.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.tabArrDep2.SuspendLayout();
            this.grpMoreDep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDepClockSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDepClockMin)).BeginInit();
            this.grpMoreArrival.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArrRandMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArrRandSec)).BeginInit();
            this.tabMess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMessType)).BeginInit();
            this.grpMessCancel.SuspendLayout();
            this.grpMessages.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.grpSend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMessDelay)).BeginInit();
            this.tabGlob.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGlobTrigPts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGlobActSeq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGlobDelay)).BeginInit();
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
            this.grpTeamPMF.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPMFEomDelay)).BeginInit();
            this.grpTeamOMC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOMCEomDelay)).BeginInit();
            this.grpTeamPMC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPMCEomDelay)).BeginInit();
            this.groupBox30.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel20.SuspendLayout();
            this.panel19.SuspendLayout();
            this.panel18.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panel14.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel11.SuspendLayout();
            this.tabMission.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMissTimeMin)).BeginInit();
            this.tabMission2.SuspendLayout();
            this.groupBox40.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGCCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGCValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGCVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGCIndex)).BeginInit();
            this.grpGlobalUnits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGUSpecial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGUIndex)).BeginInit();
            this.grpGlobalGroups.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGGSpecial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGGIndex)).BeginInit();
            this.grpRegions.SuspendLayout();
            this.groupBox37.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataOrders_Raw)).BeginInit();
            this.SuspendLayout();
            // 
            // menuXWA
            // 
            this.menuXWA.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuTools,
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
            this.menuNewXWA.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
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
            this.menuCut.Click += new System.EventHandler(this.menuCut_Click);
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
            this.menuGoalSummary,
            this.menuLibrary,
            this.menuTest,
            this.menuHyperbuoy,
            this.menuSuperBackdrops,
            this.menuHooks,
            this.menuWav,
            this.menuMissionCraft,
            this.menuGlobalSummary});
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
            // menuLibrary
            // 
            this.menuLibrary.Index = 6;
            this.menuLibrary.Text = "FG Librar&y";
            this.menuLibrary.Click += new System.EventHandler(this.menuLibrary_Click);
            // 
            // menuTest
            // 
            this.menuTest.Index = 7;
            this.menuTest.Text = "&Test";
            this.menuTest.Click += new System.EventHandler(this.menuTest_Click);
            // 
            // menuHyperbuoy
            // 
            this.menuHyperbuoy.Index = 8;
            this.menuHyperbuoy.Text = "&Hyperbouy Wizard";
            this.menuHyperbuoy.Click += new System.EventHandler(this.menuHyperbuoy_Click);
            // 
            // menuSuperBackdrops
            // 
            this.menuSuperBackdrops.Index = 9;
            this.menuSuperBackdrops.Text = "Apply &Super Backdrops";
            this.menuSuperBackdrops.Click += new System.EventHandler(this.menuSuperBackdrops_Click);
            // 
            // menuHooks
            // 
            this.menuHooks.Enabled = false;
            this.menuHooks.Index = 10;
            this.menuHooks.Text = "Hoo&k Assignment";
            this.menuHooks.Click += new System.EventHandler(this.menuHooks_Click);
            // 
            // menuWav
            // 
            this.menuWav.Index = 11;
            this.menuWav.Text = "&Wave Manager";
            this.menuWav.Click += new System.EventHandler(this.menuWav_Click);
            // 
            // menuMissionCraft
            // 
            this.menuMissionCraft.Index = 12;
            this.menuMissionCraft.Text = "Mission &Craft List";
            this.menuMissionCraft.Click += new System.EventHandler(this.menuMissionCraft_Click);
            // 
            // menuGlobalSummary
            // 
            this.menuGlobalSummary.Index = 13;
            this.menuGlobalSummary.Text = "GG and G&U Summary";
            this.menuGlobalSummary.Click += new System.EventHandler(this.menuGlobalSummary_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.Index = 3;
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
            this.menuIDMR.Text = "&Github Page";
            this.menuIDMR.Click += new System.EventHandler(this.menuIDMR_Click);
            // 
            // menuER
            // 
            this.menuER.Index = 3;
            this.menuER.Text = "&Empirereborn.net";
            this.menuER.Click += new System.EventHandler(this.menuER_Click);
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
            this.imgToolbar.Images.SetKeyName(14, "speaker.png");
            // 
            // toolXWA
            // 
            this.toolXWA.AutoSize = false;
            this.toolXWA.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
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
            this.toolWav,
            this.toolHelp});
            this.toolXWA.DropDownArrows = true;
            this.toolXWA.ImageList = this.imgToolbar;
            this.toolXWA.Location = new System.Drawing.Point(0, 0);
            this.toolXWA.Name = "toolXWA";
            this.toolXWA.ShowToolTips = true;
            this.toolXWA.Size = new System.Drawing.Size(794, 30);
            this.toolXWA.TabIndex = 2;
            this.toolXWA.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolXWA_ButtonClick);
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
            // toolWav
            // 
            this.toolWav.ImageIndex = 14;
            this.toolWav.Name = "toolWav";
            this.toolWav.ToolTipText = "Wave Manager";
            // 
            // toolHelp
            // 
            this.toolHelp.ImageIndex = 13;
            this.toolHelp.Name = "toolHelp";
            this.toolHelp.ToolTipText = "Help";
            // 
            // opnXWA
            // 
            this.opnXWA.DefaultExt = "tie";
            this.opnXWA.Filter = "Mission Files|*.tie|X-wing Missions|*.xwi";
            this.opnXWA.FileOk += new System.ComponentModel.CancelEventHandler(this.opnXWA_FileOk);
            // 
            // savXWA
            // 
            this.savXWA.DefaultExt = "tie";
            this.savXWA.FileName = "NewMission.tie";
            this.savXWA.Filter = "Mission Files|*.tie|X-wing Missions|*.xwi";
            this.savXWA.FileOk += new System.ComponentModel.CancelEventHandler(this.savXWA_FileOk);
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
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabFG);
            this.tabMain.Controls.Add(this.tabMess);
            this.tabMain.Controls.Add(this.tabGlob);
            this.tabMain.Controls.Add(this.tabTeam);
            this.tabMain.Controls.Add(this.tabMission);
            this.tabMain.Controls.Add(this.tabMission2);
            this.tabMain.Location = new System.Drawing.Point(0, 32);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(793, 545);
            this.tabMain.TabIndex = 3;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // tabFG
            // 
            this.tabFG.Controls.Add(this.numFilterRegion);
            this.tabFG.Controls.Add(this.chkRegionFilter);
            this.tabFG.Controls.Add(this.tabFGMinor);
            this.tabFG.Controls.Add(this.label1);
            this.tabFG.Controls.Add(this.lstFG);
            this.tabFG.Location = new System.Drawing.Point(4, 22);
            this.tabFG.Name = "tabFG";
            this.tabFG.Size = new System.Drawing.Size(785, 519);
            this.tabFG.TabIndex = 0;
            this.tabFG.Text = "Flight Groups";
            this.tabFG.UseVisualStyleBackColor = true;
            // 
            // numFilterRegion
            // 
            this.numFilterRegion.Location = new System.Drawing.Point(92, 496);
            this.numFilterRegion.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numFilterRegion.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFilterRegion.Name = "numFilterRegion";
            this.numFilterRegion.Size = new System.Drawing.Size(34, 20);
            this.numFilterRegion.TabIndex = 7;
            this.numFilterRegion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFilterRegion.ValueChanged += new System.EventHandler(this.numFilterRegion_ValueChanged);
            // 
            // chkRegionFilter
            // 
            this.chkRegionFilter.AutoSize = true;
            this.chkRegionFilter.Location = new System.Drawing.Point(7, 497);
            this.chkRegionFilter.Name = "chkRegionFilter";
            this.chkRegionFilter.Size = new System.Drawing.Size(85, 17);
            this.chkRegionFilter.TabIndex = 6;
            this.chkRegionFilter.Text = "Filter Region";
            this.chkRegionFilter.UseVisualStyleBackColor = true;
            this.chkRegionFilter.CheckedChanged += new System.EventHandler(this.chkRegionFilter_CheckedChanged);
            // 
            // tabFGMinor
            // 
            this.tabFGMinor.Controls.Add(this.tabCraft);
            this.tabFGMinor.Controls.Add(this.tabArrDep);
            this.tabFGMinor.Controls.Add(this.tabGoals);
            this.tabFGMinor.Controls.Add(this.tabWP);
            this.tabFGMinor.Controls.Add(this.tabOrders);
            this.tabFGMinor.Controls.Add(this.tapOption);
            this.tabFGMinor.Controls.Add(this.tabArrDep2);
            this.tabFGMinor.Location = new System.Drawing.Point(232, 0);
            this.tabFGMinor.Name = "tabFGMinor";
            this.tabFGMinor.SelectedIndex = 0;
            this.tabFGMinor.Size = new System.Drawing.Size(552, 516);
            this.tabFGMinor.TabIndex = 5;
            this.tabFGMinor.SelectedIndexChanged += new System.EventHandler(this.tabFGMinor_SelectedIndexChanged);
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
            this.tabCraft.Size = new System.Drawing.Size(544, 490);
            this.tabCraft.TabIndex = 0;
            this.tabCraft.Text = "Craft";
            // 
            // cmdMoveFGDown
            // 
            this.cmdMoveFGDown.Location = new System.Drawing.Point(445, 421);
            this.cmdMoveFGDown.Name = "cmdMoveFGDown";
            this.cmdMoveFGDown.Size = new System.Drawing.Size(75, 23);
            this.cmdMoveFGDown.TabIndex = 18;
            this.cmdMoveFGDown.Text = "Move Down";
            this.cmdMoveFGDown.UseVisualStyleBackColor = true;
            this.cmdMoveFGDown.Click += new System.EventHandler(this.cmdMoveFGDown_Click);
            // 
            // cmdMoveFGUp
            // 
            this.cmdMoveFGUp.Location = new System.Drawing.Point(445, 396);
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
            this.groupBox1.Controls.Add(this.lblCargo);
            this.groupBox1.Controls.Add(this.txtCargo);
            this.groupBox1.Controls.Add(this.lblSC);
            this.groupBox1.Controls.Add(this.lblName);
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
            this.txtName.MaxLength = 19;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(128, 20);
            this.txtName.TabIndex = 4;
            this.txtName.Text = "New Ship";
            // 
            // lblNotUsed
            // 
            this.lblNotUsed.Location = new System.Drawing.Point(88, 66);
            this.lblNotUsed.Name = "lblNotUsed";
            this.lblNotUsed.Size = new System.Drawing.Size(80, 16);
            this.lblNotUsed.TabIndex = 3;
            this.lblNotUsed.Text = "(not used)";
            // 
            // txtSpecCargo
            // 
            this.txtSpecCargo.Location = new System.Drawing.Point(88, 64);
            this.txtSpecCargo.MaxLength = 19;
            this.txtSpecCargo.Name = "txtSpecCargo";
            this.txtSpecCargo.Size = new System.Drawing.Size(128, 20);
            this.txtSpecCargo.TabIndex = 7;
            this.txtSpecCargo.Visible = false;
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
            // lblCargo
            // 
            this.lblCargo.Location = new System.Drawing.Point(8, 42);
            this.lblCargo.Name = "lblCargo";
            this.lblCargo.Size = new System.Drawing.Size(80, 16);
            this.lblCargo.TabIndex = 3;
            this.lblCargo.Text = "Cargo";
            // 
            // txtCargo
            // 
            this.txtCargo.Location = new System.Drawing.Point(88, 40);
            this.txtCargo.MaxLength = 19;
            this.txtCargo.Name = "txtCargo";
            this.txtCargo.Size = new System.Drawing.Size(128, 20);
            this.txtCargo.TabIndex = 6;
            // 
            // lblSC
            // 
            this.lblSC.Location = new System.Drawing.Point(8, 66);
            this.lblSC.Name = "lblSC";
            this.lblSC.Size = new System.Drawing.Size(80, 16);
            this.lblSC.TabIndex = 3;
            this.lblSC.Text = "Special Cargo";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(8, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(40, 16);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(8, 90);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(80, 16);
            this.label24.TabIndex = 3;
            this.label24.Text = "Special Ship #";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpCraft3
            // 
            this.grpCraft3.Controls.Add(this.lblWaveDelay);
            this.grpCraft3.Controls.Add(this.label17);
            this.grpCraft3.Controls.Add(this.numWaveDelay);
            this.grpCraft3.Controls.Add(this.chkGU);
            this.grpCraft3.Controls.Add(this.numWaves);
            this.grpCraft3.Controls.Add(this.label10);
            this.grpCraft3.Controls.Add(this.label11);
            this.grpCraft3.Controls.Add(this.label12);
            this.grpCraft3.Controls.Add(this.numCraft);
            this.grpCraft3.Controls.Add(this.numGG);
            this.grpCraft3.Controls.Add(this.numGU);
            this.grpCraft3.Controls.Add(this.label3);
            this.grpCraft3.Location = new System.Drawing.Point(268, 24);
            this.grpCraft3.Name = "grpCraft3";
            this.grpCraft3.Size = new System.Drawing.Size(252, 120);
            this.grpCraft3.TabIndex = 15;
            this.grpCraft3.TabStop = false;
            // 
            // lblWaveDelay
            // 
            this.lblWaveDelay.Location = new System.Drawing.Point(132, 67);
            this.lblWaveDelay.Name = "lblWaveDelay";
            this.lblWaveDelay.Size = new System.Drawing.Size(86, 13);
            this.lblWaveDelay.TabIndex = 45;
            this.lblWaveDelay.Text = "0:00";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 66);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(66, 13);
            this.label17.TabIndex = 25;
            this.label17.Text = "Wave Delay";
            // 
            // numWaveDelay
            // 
            this.numWaveDelay.Location = new System.Drawing.Point(78, 64);
            this.numWaveDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numWaveDelay.Name = "numWaveDelay";
            this.numWaveDelay.Size = new System.Drawing.Size(48, 20);
            this.numWaveDelay.TabIndex = 24;
            this.numWaveDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numWaveDelay.ValueChanged += new System.EventHandler(this.numWaveDelay_ValueChanged);
            // 
            // chkGU
            // 
            this.chkGU.AutoSize = true;
            this.chkGU.Location = new System.Drawing.Point(107, 92);
            this.chkGU.Name = "chkGU";
            this.chkGU.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkGU.Size = new System.Drawing.Size(136, 17);
            this.chkGU.TabIndex = 23;
            this.chkGU.Text = "Prevent GU Numbering";
            this.chkGU.UseVisualStyleBackColor = true;
            // 
            // numWaves
            // 
            this.numWaves.Location = new System.Drawing.Point(68, 14);
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
            this.numWaves.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numWaves.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "# of waves";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "# of craft";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(128, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Global Group";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // numCraft
            // 
            this.numCraft.Location = new System.Drawing.Point(68, 38);
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
            this.numCraft.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numCraft.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numGG
            // 
            this.numGG.Location = new System.Drawing.Point(203, 14);
            this.numGG.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGG.Name = "numGG";
            this.numGG.Size = new System.Drawing.Size(40, 20);
            this.numGG.TabIndex = 22;
            this.numGG.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGG.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numGG_KeyDown);
            this.numGG.Leave += new System.EventHandler(this.numGG_Leave);
            // 
            // numGU
            // 
            this.numGU.Location = new System.Drawing.Point(203, 40);
            this.numGU.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGU.Name = "numGU";
            this.numGU.Size = new System.Drawing.Size(40, 20);
            this.numGU.TabIndex = 22;
            this.numGU.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGU.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numGU_KeyDown);
            this.numGU.Leave += new System.EventHandler(this.numGU_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(128, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Global Unit";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // grpCraft2
            // 
            this.grpCraft2.Controls.Add(this.cboCraft);
            this.grpCraft2.Controls.Add(this.label7);
            this.grpCraft2.Controls.Add(this.label8);
            this.grpCraft2.Controls.Add(this.label9);
            this.grpCraft2.Controls.Add(this.label13);
            this.grpCraft2.Controls.Add(this.label14);
            this.grpCraft2.Controls.Add(this.label15);
            this.grpCraft2.Controls.Add(this.label16);
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
            this.grpCraft2.Size = new System.Drawing.Size(232, 269);
            this.grpCraft2.TabIndex = 14;
            this.grpCraft2.TabStop = false;
            // 
            // cboCraft
            // 
            this.cboCraft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCraft.Location = new System.Drawing.Point(88, 16);
            this.cboCraft.MaxDropDownItems = 20;
            this.cboCraft.Name = "cboCraft";
            this.cboCraft.Size = new System.Drawing.Size(136, 21);
            this.cboCraft.TabIndex = 10;
            this.cboCraft.SelectedIndexChanged += new System.EventHandler(this.cboCraft_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 16);
            this.label7.TabIndex = 0;
            this.label7.Text = "Craft Type";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 16);
            this.label8.TabIndex = 0;
            this.label8.Text = "IFF";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 92);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 16);
            this.label9.TabIndex = 0;
            this.label9.Text = "AI skill";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(8, 140);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(40, 16);
            this.label13.TabIndex = 0;
            this.label13.Text = "Player";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(8, 116);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 16);
            this.label14.TabIndex = 0;
            this.label14.Text = "Markings";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(8, 188);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(56, 16);
            this.label15.TabIndex = 0;
            this.label15.Text = "Radio";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(8, 235);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(72, 16);
            this.label16.TabIndex = 0;
            this.label16.Text = "FG spacing";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numSpacing
            // 
            this.numSpacing.Location = new System.Drawing.Point(88, 235);
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
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
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
            // 
            // cmdForms
            // 
            this.cmdForms.Location = new System.Drawing.Point(160, 233);
            this.cmdForms.Name = "cmdForms";
            this.cmdForms.Size = new System.Drawing.Size(64, 24);
            this.cmdForms.TabIndex = 19;
            this.cmdForms.Text = "&Forms...";
            this.cmdForms.Click += new System.EventHandler(this.cmdForms_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Formation";
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
            this.label58.Location = new System.Drawing.Point(8, 164);
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
            this.label111.Location = new System.Drawing.Point(8, 68);
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
            this.lblFG.Location = new System.Drawing.Point(266, 399);
            this.lblFG.Name = "lblFG";
            this.lblFG.Size = new System.Drawing.Size(147, 16);
            this.lblFG.TabIndex = 13;
            this.lblFG.Text = "Flight Group #0 of 0";
            // 
            // lblStarting
            // 
            this.lblStarting.Location = new System.Drawing.Point(266, 431);
            this.lblStarting.Name = "lblStarting";
            this.lblStarting.Size = new System.Drawing.Size(147, 16);
            this.lblStarting.TabIndex = 12;
            this.lblStarting.Text = "1 Craft at 30 seconds";
            // 
            // grpCraft4
            // 
            this.grpCraft4.Controls.Add(this.cmdBackdrop);
            this.grpCraft4.Controls.Add(this.cboGlobSpecCargo);
            this.grpCraft4.Controls.Add(this.cboGlobCargo);
            this.grpCraft4.Controls.Add(this.numBackdrop);
            this.grpCraft4.Controls.Add(this.label143);
            this.grpCraft4.Controls.Add(this.lblGC);
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
            this.grpCraft4.Location = new System.Drawing.Point(268, 146);
            this.grpCraft4.Name = "grpCraft4";
            this.grpCraft4.Size = new System.Drawing.Size(252, 242);
            this.grpCraft4.TabIndex = 11;
            this.grpCraft4.TabStop = false;
            // 
            // cmdBackdrop
            // 
            this.cmdBackdrop.Location = new System.Drawing.Point(155, 160);
            this.cmdBackdrop.Name = "cmdBackdrop";
            this.cmdBackdrop.Size = new System.Drawing.Size(68, 20);
            this.cmdBackdrop.TabIndex = 33;
            this.cmdBackdrop.Text = "&Backdrops";
            this.cmdBackdrop.UseVisualStyleBackColor = true;
            this.cmdBackdrop.Click += new System.EventHandler(this.cmdBackdrop_Click);
            // 
            // cboGlobSpecCargo
            // 
            this.cboGlobSpecCargo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGlobSpecCargo.FormattingEnabled = true;
            this.cboGlobSpecCargo.Location = new System.Drawing.Point(111, 209);
            this.cboGlobSpecCargo.Name = "cboGlobSpecCargo";
            this.cboGlobSpecCargo.Size = new System.Drawing.Size(135, 21);
            this.cboGlobSpecCargo.TabIndex = 31;
            // 
            // cboGlobCargo
            // 
            this.cboGlobCargo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGlobCargo.FormattingEnabled = true;
            this.cboGlobCargo.Location = new System.Drawing.Point(111, 185);
            this.cboGlobCargo.Name = "cboGlobCargo";
            this.cboGlobCargo.Size = new System.Drawing.Size(135, 21);
            this.cboGlobCargo.TabIndex = 31;
            // 
            // numBackdrop
            // 
            this.numBackdrop.Enabled = false;
            this.numBackdrop.Location = new System.Drawing.Point(96, 160);
            this.numBackdrop.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numBackdrop.Name = "numBackdrop";
            this.numBackdrop.Size = new System.Drawing.Size(48, 20);
            this.numBackdrop.TabIndex = 30;
            this.numBackdrop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label143
            // 
            this.label143.AutoSize = true;
            this.label143.Location = new System.Drawing.Point(8, 212);
            this.label143.Name = "label143";
            this.label143.Size = new System.Drawing.Size(96, 13);
            this.label143.TabIndex = 29;
            this.label143.Text = "Global Spec Cargo";
            // 
            // lblGC
            // 
            this.lblGC.AutoSize = true;
            this.lblGC.Location = new System.Drawing.Point(8, 188);
            this.lblGC.Name = "lblGC";
            this.lblGC.Size = new System.Drawing.Size(68, 13);
            this.lblGC.TabIndex = 29;
            this.lblGC.Text = "Global Cargo";
            // 
            // label122
            // 
            this.label122.AutoSize = true;
            this.label122.Location = new System.Drawing.Point(8, 164);
            this.label122.Name = "label122";
            this.label122.Size = new System.Drawing.Size(53, 13);
            this.label122.TabIndex = 29;
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
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(8, 140);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(74, 13);
            this.label67.TabIndex = 26;
            this.label67.Text = "Explosion time";
            this.label67.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(8, 92);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(34, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Beam";
            // 
            // cboWarheads
            // 
            this.cboWarheads.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWarheads.Location = new System.Drawing.Point(96, 64);
            this.cboWarheads.Name = "cboWarheads";
            this.cboWarheads.Size = new System.Drawing.Size(150, 21);
            this.cboWarheads.TabIndex = 24;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(8, 20);
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
            this.cboBeam.Size = new System.Drawing.Size(150, 21);
            this.cboBeam.TabIndex = 25;
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Location = new System.Drawing.Point(96, 16);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(150, 21);
            this.cboStatus.TabIndex = 23;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(8, 68);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(56, 13);
            this.label20.TabIndex = 0;
            this.label20.Text = "Warheads";
            // 
            // cboStatus2
            // 
            this.cboStatus2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus2.Location = new System.Drawing.Point(96, 40);
            this.cboStatus2.Name = "cboStatus2";
            this.cboStatus2.Size = new System.Drawing.Size(150, 21);
            this.cboStatus2.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
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
            this.cboCounter.Size = new System.Drawing.Size(150, 21);
            this.cboCounter.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Countermeasures";
            // 
            // tabArrDep
            // 
            this.tabArrDep.Controls.Add(this.cmdMissionCraft);
            this.tabArrDep.Controls.Add(this.cboADPara);
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
            this.tabArrDep.Size = new System.Drawing.Size(544, 490);
            this.tabArrDep.TabIndex = 1;
            this.tabArrDep.Text = "Arr/Dep";
            // 
            // cmdMissionCraft
            // 
            this.cmdMissionCraft.Location = new System.Drawing.Point(3, 441);
            this.cmdMissionCraft.Name = "cmdMissionCraft";
            this.cmdMissionCraft.Size = new System.Drawing.Size(77, 34);
            this.cmdMissionCraft.TabIndex = 43;
            this.cmdMissionCraft.Text = "Set Mission Craft";
            this.cmdMissionCraft.UseVisualStyleBackColor = true;
            this.cmdMissionCraft.Click += new System.EventHandler(this.cmdMissionCraft_Click);
            // 
            // cboADPara
            // 
            this.cboADPara.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboADPara.FormattingEnabled = true;
            this.cboADPara.Location = new System.Drawing.Point(446, 412);
            this.cboADPara.Name = "cboADPara";
            this.cboADPara.Size = new System.Drawing.Size(88, 21);
            this.cboADPara.TabIndex = 42;
            // 
            // chkArrHuman
            // 
            this.chkArrHuman.Location = new System.Drawing.Point(368, 441);
            this.chkArrHuman.Name = "chkArrHuman";
            this.chkArrHuman.Size = new System.Drawing.Size(136, 24);
            this.chkArrHuman.TabIndex = 39;
            this.chkArrHuman.Text = "Only if Human player";
            // 
            // cmdCopyAD
            // 
            this.cmdCopyAD.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.cmdCopyAD.ImageIndex = 6;
            this.cmdCopyAD.ImageList = this.imgToolbar;
            this.cmdCopyAD.Location = new System.Drawing.Point(72, 384);
            this.cmdCopyAD.Name = "cmdCopyAD";
            this.cmdCopyAD.Size = new System.Drawing.Size(24, 23);
            this.cmdCopyAD.TabIndex = 37;
            this.cmdCopyAD.Click += new System.EventHandler(this.cmdCopyAD_Click);
            // 
            // label36
            // 
            this.label36.Location = new System.Drawing.Point(256, 384);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(16, 16);
            this.label36.TabIndex = 29;
            this.label36.Text = "of";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpDep
            // 
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
            // 
            // numDepMin
            // 
            this.numDepMin.Location = new System.Drawing.Point(98, 328);
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
            this.numDepSec.Location = new System.Drawing.Point(176, 328);
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
            this.optDepAND.Location = new System.Drawing.Point(88, 216);
            this.optDepAND.Name = "optDepAND";
            this.optDepAND.Size = new System.Drawing.Size(56, 24);
            this.optDepAND.TabIndex = 19;
            this.optDepAND.Text = "AND";
            // 
            // optDepOR
            // 
            this.optDepOR.Checked = true;
            this.optDepOR.Location = new System.Drawing.Point(144, 216);
            this.optDepOR.Name = "optDepOR";
            this.optDepOR.Size = new System.Drawing.Size(56, 24);
            this.optDepOR.TabIndex = 20;
            this.optDepOR.TabStop = true;
            this.optDepOR.Text = "OR";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(112, 352);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(82, 13);
            this.label47.TabIndex = 8;
            this.label47.Text = "after trigger fires";
            // 
            // label41
            // 
            this.label41.Location = new System.Drawing.Point(8, 328);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(96, 16);
            this.label41.TabIndex = 7;
            this.label41.Text = "Flight will depart";
            this.label41.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label40
            // 
            this.label40.Location = new System.Drawing.Point(145, 328);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(24, 16);
            this.label40.TabIndex = 6;
            this.label40.Text = "Min";
            this.label40.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label39
            // 
            this.label39.Location = new System.Drawing.Point(222, 328);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(32, 16);
            this.label39.TabIndex = 5;
            this.label39.Text = "Sec";
            this.label39.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label37
            // 
            this.label37.Location = new System.Drawing.Point(16, 280);
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
            this.groupBox10.Text = "After Capture:";
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
            this.groupBox9.Controls.Add(this.optDepRegion);
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
            // optDepRegion
            // 
            this.optDepRegion.AutoSize = true;
            this.optDepRegion.Location = new System.Drawing.Point(108, 20);
            this.optDepRegion.Name = "optDepRegion";
            this.optDepRegion.Size = new System.Drawing.Size(116, 17);
            this.optDepRegion.TabIndex = 9;
            this.optDepRegion.TabStop = true;
            this.optDepRegion.Text = "Hyper in Region of:";
            this.optDepRegion.UseVisualStyleBackColor = true;
            this.optDepRegion.CheckedChanged += new System.EventHandler(this.optDepRegion_CheckedChanged);
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
            this.optDepHyp.CheckedChanged += new System.EventHandler(this.optDepHyp_CheckedChanged);
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
            // 
            // lblDep1
            // 
            this.lblDep1.BackColor = System.Drawing.Color.RosyBrown;
            this.lblDep1.Location = new System.Drawing.Point(8, 184);
            this.lblDep1.Name = "lblDep1";
            this.lblDep1.Size = new System.Drawing.Size(240, 32);
            this.lblDep1.TabIndex = 2;
            this.lblDep1.Text = "always (TRUE)";
            this.lblDep1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboAbort
            // 
            this.cboAbort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAbort.Location = new System.Drawing.Point(88, 296);
            this.cboAbort.Name = "cboAbort";
            this.cboAbort.Size = new System.Drawing.Size(144, 21);
            this.cboAbort.TabIndex = 16;
            // 
            // lblDep2
            // 
            this.lblDep2.BackColor = System.Drawing.Color.RosyBrown;
            this.lblDep2.Location = new System.Drawing.Point(8, 240);
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
            this.groupBox11.Text = "Alternate:";
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
            this.groupBox12.Controls.Add(this.optArrRegion);
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
            // optArrRegion
            // 
            this.optArrRegion.AutoSize = true;
            this.optArrRegion.Location = new System.Drawing.Point(112, 20);
            this.optArrRegion.Name = "optArrRegion";
            this.optArrRegion.Size = new System.Drawing.Size(116, 17);
            this.optArrRegion.TabIndex = 3;
            this.optArrRegion.TabStop = true;
            this.optArrRegion.Text = "Hyper in Region of:";
            this.optArrRegion.UseVisualStyleBackColor = true;
            this.optArrRegion.CheckedChanged += new System.EventHandler(this.optArrRegion_CheckedChanged);
            // 
            // cboArrMS
            // 
            this.cboArrMS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboArrMS.Enabled = false;
            this.cboArrMS.Location = new System.Drawing.Point(96, 40);
            this.cboArrMS.Name = "cboArrMS";
            this.cboArrMS.Size = new System.Drawing.Size(136, 21);
            this.cboArrMS.TabIndex = 2;
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
            this.optArrHyp.CheckedChanged += new System.EventHandler(this.optArrHyp_CheckedChanged);
            // 
            // optArrMS
            // 
            this.optArrMS.Location = new System.Drawing.Point(16, 40);
            this.optArrMS.Name = "optArrMS";
            this.optArrMS.Size = new System.Drawing.Size(80, 24);
            this.optArrMS.TabIndex = 1;
            this.optArrMS.Text = "Mothership";
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
            this.cboADTrigAmount.Location = new System.Drawing.Point(104, 384);
            this.cboADTrigAmount.Name = "cboADTrigAmount";
            this.cboADTrigAmount.Size = new System.Drawing.Size(144, 21);
            this.cboADTrigAmount.TabIndex = 32;
            // 
            // cboADTrigType
            // 
            this.cboADTrigType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboADTrigType.Location = new System.Drawing.Point(280, 384);
            this.cboADTrigType.Name = "cboADTrigType";
            this.cboADTrigType.Size = new System.Drawing.Size(160, 21);
            this.cboADTrigType.TabIndex = 33;
            this.cboADTrigType.SelectedIndexChanged += new System.EventHandler(this.cboADTrigType_SelectedIndexChanged);
            // 
            // cboADTrigVar
            // 
            this.cboADTrigVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboADTrigVar.Location = new System.Drawing.Point(104, 412);
            this.cboADTrigVar.Name = "cboADTrigVar";
            this.cboADTrigVar.Size = new System.Drawing.Size(144, 21);
            this.cboADTrigVar.TabIndex = 34;
            // 
            // cboADTrig
            // 
            this.cboADTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboADTrig.Location = new System.Drawing.Point(280, 412);
            this.cboADTrig.Name = "cboADTrig";
            this.cboADTrig.Size = new System.Drawing.Size(160, 21);
            this.cboADTrig.TabIndex = 35;
            this.cboADTrig.SelectedIndexChanged += new System.EventHandler(this.cboADTrig_SelectedIndexChanged);
            // 
            // label44
            // 
            this.label44.Location = new System.Drawing.Point(248, 412);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(32, 16);
            this.label44.TabIndex = 28;
            this.label44.Text = "must";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboDiff
            // 
            this.cboDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDiff.Location = new System.Drawing.Point(192, 444);
            this.cboDiff.Name = "cboDiff";
            this.cboDiff.Size = new System.Drawing.Size(112, 21);
            this.cboDiff.TabIndex = 36;
            // 
            // label45
            // 
            this.label45.Location = new System.Drawing.Point(304, 444);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(48, 16);
            this.label45.TabIndex = 30;
            this.label45.Text = "difficulty";
            this.label45.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label46
            // 
            this.label46.Location = new System.Drawing.Point(104, 444);
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
            this.cmdPasteAD.Location = new System.Drawing.Point(72, 412);
            this.cmdPasteAD.Name = "cmdPasteAD";
            this.cmdPasteAD.Size = new System.Drawing.Size(24, 23);
            this.cmdPasteAD.TabIndex = 38;
            this.cmdPasteAD.Click += new System.EventHandler(this.cmdPasteAD_Click);
            // 
            // tabGoals
            // 
            this.tabGoals.Controls.Add(this.lblGoalTimeLimitNote);
            this.tabGoals.Controls.Add(this.lblGoalTimeLimitSec);
            this.tabGoals.Controls.Add(this.lblGoalTimeLimit);
            this.tabGoals.Controls.Add(this.numGoalTimeLimit);
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
            this.tabGoals.Controls.Add(this.numGoalTeam);
            this.tabGoals.Location = new System.Drawing.Point(4, 22);
            this.tabGoals.Name = "tabGoals";
            this.tabGoals.Size = new System.Drawing.Size(544, 490);
            this.tabGoals.TabIndex = 2;
            this.tabGoals.Text = "Goals";
            // 
            // lblGoalTimeLimitNote
            // 
            this.lblGoalTimeLimitNote.Location = new System.Drawing.Point(66, 451);
            this.lblGoalTimeLimitNote.Name = "lblGoalTimeLimitNote";
            this.lblGoalTimeLimitNote.Size = new System.Drawing.Size(412, 26);
            this.lblGoalTimeLimitNote.TabIndex = 52;
            this.lblGoalTimeLimitNote.Text = "lblGoalTimeLimitNote";
            this.lblGoalTimeLimitNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGoalTimeLimitNote.Visible = false;
            // 
            // lblGoalTimeLimitSec
            // 
            this.lblGoalTimeLimitSec.AutoSize = true;
            this.lblGoalTimeLimitSec.Location = new System.Drawing.Point(462, 345);
            this.lblGoalTimeLimitSec.Name = "lblGoalTimeLimitSec";
            this.lblGoalTimeLimitSec.Size = new System.Drawing.Size(63, 13);
            this.lblGoalTimeLimitSec.TabIndex = 51;
            this.lblGoalTimeLimitSec.Text = "No time limit";
            // 
            // lblGoalTimeLimit
            // 
            this.lblGoalTimeLimit.AutoSize = true;
            this.lblGoalTimeLimit.Location = new System.Drawing.Point(337, 345);
            this.lblGoalTimeLimit.Name = "lblGoalTimeLimit";
            this.lblGoalTimeLimit.Size = new System.Drawing.Size(57, 13);
            this.lblGoalTimeLimit.TabIndex = 50;
            this.lblGoalTimeLimit.Text = "Time Limit:";
            // 
            // numGoalTimeLimit
            // 
            this.numGoalTimeLimit.Location = new System.Drawing.Point(400, 341);
            this.numGoalTimeLimit.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGoalTimeLimit.Name = "numGoalTimeLimit";
            this.numGoalTimeLimit.Size = new System.Drawing.Size(56, 20);
            this.numGoalTimeLimit.TabIndex = 18;
            this.numGoalTimeLimit.ValueChanged += new System.EventHandler(this.numGoalTimeLimit_ValueChanged);
            // 
            // grpGoal
            // 
            this.grpGoal.Controls.Add(this.cboGoalPara);
            this.grpGoal.Controls.Add(this.label31);
            this.grpGoal.Controls.Add(this.numGoalActSeq);
            this.grpGoal.Controls.Add(this.label61);
            this.grpGoal.Controls.Add(this.cboGoalAmount);
            this.grpGoal.Controls.Add(this.cboGoalArgument);
            this.grpGoal.Controls.Add(this.cboGoalTrigger);
            this.grpGoal.Location = new System.Drawing.Point(16, 256);
            this.grpGoal.Name = "grpGoal";
            this.grpGoal.Size = new System.Drawing.Size(288, 102);
            this.grpGoal.TabIndex = 48;
            this.grpGoal.TabStop = false;
            // 
            // cboGoalPara
            // 
            this.cboGoalPara.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGoalPara.FormattingEnabled = true;
            this.cboGoalPara.Location = new System.Drawing.Point(155, 76);
            this.cboGoalPara.Name = "cboGoalPara";
            this.cboGoalPara.Size = new System.Drawing.Size(125, 21);
            this.cboGoalPara.TabIndex = 13;
            this.cboGoalPara.SelectedIndexChanged += new System.EventHandler(this.cboGoalPara_SelectedIndexChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(8, 78);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(89, 13);
            this.label31.TabIndex = 43;
            this.label31.Text = "Active Sequence";
            // 
            // numGoalActSeq
            // 
            this.numGoalActSeq.Location = new System.Drawing.Point(103, 76);
            this.numGoalActSeq.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGoalActSeq.Name = "numGoalActSeq";
            this.numGoalActSeq.Size = new System.Drawing.Size(46, 20);
            this.numGoalActSeq.TabIndex = 14;
            this.numGoalActSeq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ttActiveSequence.SetToolTip(this.numGoalActSeq, resources.GetString("numGoalActSeq.ToolTip"));
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(171, 19);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(94, 13);
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
            this.cboGoalAmount.TabIndex = 10;
            // 
            // cboGoalArgument
            // 
            this.cboGoalArgument.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGoalArgument.Items.AddRange(new object[] {
            "must",
            "must not",
            "BONUS must",
            "BONUS must not"});
            this.cboGoalArgument.Location = new System.Drawing.Point(8, 46);
            this.cboGoalArgument.Name = "cboGoalArgument";
            this.cboGoalArgument.Size = new System.Drawing.Size(112, 21);
            this.cboGoalArgument.TabIndex = 11;
            // 
            // cboGoalTrigger
            // 
            this.cboGoalTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGoalTrigger.Location = new System.Drawing.Point(120, 46);
            this.cboGoalTrigger.Name = "cboGoalTrigger";
            this.cboGoalTrigger.Size = new System.Drawing.Size(160, 21);
            this.cboGoalTrigger.TabIndex = 12;
            this.cboGoalTrigger.SelectedIndexChanged += new System.EventHandler(this.cboGoalTrigger_SelectedIndexChanged);
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
            this.chkGoalEnable.Location = new System.Drawing.Point(379, 316);
            this.chkGoalEnable.Name = "chkGoalEnable";
            this.chkGoalEnable.Size = new System.Drawing.Size(119, 17);
            this.chkGoalEnable.TabIndex = 17;
            this.chkGoalEnable.Text = "Enabled for Team 1";
            this.chkGoalEnable.CheckedChanged += new System.EventHandler(this.chkGoalEnable_CheckedChanged);
            // 
            // numGoalPoints
            // 
            this.numGoalPoints.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numGoalPoints.Location = new System.Drawing.Point(462, 264);
            this.numGoalPoints.Maximum = new decimal(new int[] {
            3175,
            0,
            0,
            0});
            this.numGoalPoints.Minimum = new decimal(new int[] {
            3200,
            0,
            0,
            -2147483648});
            this.numGoalPoints.Name = "numGoalPoints";
            this.numGoalPoints.Size = new System.Drawing.Size(56, 20);
            this.numGoalPoints.TabIndex = 15;
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(417, 266);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(39, 13);
            this.label65.TabIndex = 44;
            this.label65.Text = "Points:";
            // 
            // label62
            // 
            this.label62.Location = new System.Drawing.Point(16, 368);
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
            this.txtGoalInc.Location = new System.Drawing.Point(120, 368);
            this.txtGoalInc.MaxLength = 63;
            this.txtGoalInc.Name = "txtGoalInc";
            this.txtGoalInc.Size = new System.Drawing.Size(376, 20);
            this.txtGoalInc.TabIndex = 19;
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
            this.txtGoalComp.Location = new System.Drawing.Point(120, 398);
            this.txtGoalComp.Name = "txtGoalComp";
            this.txtGoalComp.Size = new System.Drawing.Size(376, 20);
            this.txtGoalComp.TabIndex = 20;
            // 
            // txtGoalFail
            // 
            this.txtGoalFail.BackColor = System.Drawing.Color.Black;
            this.txtGoalFail.ForeColor = System.Drawing.Color.Red;
            this.txtGoalFail.Location = new System.Drawing.Point(120, 428);
            this.txtGoalFail.Name = "txtGoalFail";
            this.txtGoalFail.Size = new System.Drawing.Size(376, 20);
            this.txtGoalFail.TabIndex = 21;
            // 
            // label63
            // 
            this.label63.Location = new System.Drawing.Point(16, 398);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(88, 16);
            this.label63.TabIndex = 43;
            this.label63.Text = "Goal Complete";
            this.label63.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label64
            // 
            this.label64.Location = new System.Drawing.Point(16, 428);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(88, 16);
            this.label64.TabIndex = 43;
            this.label64.Text = "Goal Failed";
            this.label64.TextAlign = System.Drawing.ContentAlignment.BottomRight;
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
            this.numGoalTeam.TabIndex = 16;
            this.numGoalTeam.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGoalTeam.ValueChanged += new System.EventHandler(this.numGoalTeam_ValueChanged);
            // 
            // tabWP
            // 
            this.tabWP.Controls.Add(this.numWPOrder);
            this.tabWP.Controls.Add(this.label150);
            this.tabWP.Controls.Add(this.numWPOrderRegion);
            this.tabWP.Controls.Add(this.label19);
            this.tabWP.Controls.Add(this.cmdCopyOrderWP);
            this.tabWP.Controls.Add(this.cmdPasteOrderWP);
            this.tabWP.Controls.Add(this.numHYP);
            this.tabWP.Controls.Add(this.numWPCapture);
            this.tabWP.Controls.Add(this.numSP2);
            this.tabWP.Controls.Add(this.numSP1);
            this.tabWP.Controls.Add(this.label76);
            this.tabWP.Controls.Add(this.numRoll);
            this.tabWP.Controls.Add(this.numPitch);
            this.tabWP.Controls.Add(this.numYaw);
            this.tabWP.Controls.Add(this.label25);
            this.tabWP.Controls.Add(this.label56);
            this.tabWP.Controls.Add(this.dataO_Raw);
            this.tabWP.Controls.Add(this.dataO);
            this.tabWP.Controls.Add(this.dataWP);
            this.tabWP.Controls.Add(this.dataWP_Raw);
            this.tabWP.Controls.Add(this.chkWPHyp);
            this.tabWP.Controls.Add(this.chkWP8);
            this.tabWP.Controls.Add(this.chkWP7);
            this.tabWP.Controls.Add(this.chkWP2);
            this.tabWP.Controls.Add(this.chkWP1);
            this.tabWP.Controls.Add(this.chkWPCapture);
            this.tabWP.Controls.Add(this.chkSP2);
            this.tabWP.Controls.Add(this.chkSP1);
            this.tabWP.Controls.Add(this.chkWP6);
            this.tabWP.Controls.Add(this.chkWP5);
            this.tabWP.Controls.Add(this.chkWP4);
            this.tabWP.Controls.Add(this.chkWP3);
            this.tabWP.Controls.Add(this.label77);
            this.tabWP.Controls.Add(this.label78);
            this.tabWP.Location = new System.Drawing.Point(4, 22);
            this.tabWP.Name = "tabWP";
            this.tabWP.Size = new System.Drawing.Size(544, 490);
            this.tabWP.TabIndex = 3;
            this.tabWP.Text = "Waypoints";
            // 
            // numWPOrder
            // 
            this.numWPOrder.Location = new System.Drawing.Point(181, 208);
            this.numWPOrder.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numWPOrder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWPOrder.Name = "numWPOrder";
            this.numWPOrder.Size = new System.Drawing.Size(33, 20);
            this.numWPOrder.TabIndex = 57;
            this.numWPOrder.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWPOrder.ValueChanged += new System.EventHandler(this.numWPOrder_ValueChanged);
            // 
            // label150
            // 
            this.label150.AutoSize = true;
            this.label150.Location = new System.Drawing.Point(142, 210);
            this.label150.Name = "label150";
            this.label150.Size = new System.Drawing.Size(33, 13);
            this.label150.TabIndex = 56;
            this.label150.Text = "Order";
            // 
            // numWPOrderRegion
            // 
            this.numWPOrderRegion.Location = new System.Drawing.Point(264, 208);
            this.numWPOrderRegion.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numWPOrderRegion.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWPOrderRegion.Name = "numWPOrderRegion";
            this.numWPOrderRegion.Size = new System.Drawing.Size(33, 20);
            this.numWPOrderRegion.TabIndex = 55;
            this.numWPOrderRegion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWPOrderRegion.ValueChanged += new System.EventHandler(this.numWPOrder_ValueChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(217, 210);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 13);
            this.label19.TabIndex = 54;
            this.label19.Text = "Region";
            // 
            // cmdCopyOrderWP
            // 
            this.cmdCopyOrderWP.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.cmdCopyOrderWP.ImageIndex = 6;
            this.cmdCopyOrderWP.ImageList = this.imgToolbar;
            this.cmdCopyOrderWP.Location = new System.Drawing.Point(303, 208);
            this.cmdCopyOrderWP.Name = "cmdCopyOrderWP";
            this.cmdCopyOrderWP.Size = new System.Drawing.Size(24, 23);
            this.cmdCopyOrderWP.TabIndex = 52;
            this.cmdCopyOrderWP.Click += new System.EventHandler(this.cmdCopyOrderWP_Click);
            // 
            // cmdPasteOrderWP
            // 
            this.cmdPasteOrderWP.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.cmdPasteOrderWP.ImageIndex = 7;
            this.cmdPasteOrderWP.ImageList = this.imgToolbar;
            this.cmdPasteOrderWP.Location = new System.Drawing.Point(335, 208);
            this.cmdPasteOrderWP.Name = "cmdPasteOrderWP";
            this.cmdPasteOrderWP.Size = new System.Drawing.Size(24, 23);
            this.cmdPasteOrderWP.TabIndex = 53;
            this.cmdPasteOrderWP.Click += new System.EventHandler(this.cmdPasteOrderWP_Click);
            // 
            // numHYP
            // 
            this.numHYP.Location = new System.Drawing.Point(266, 104);
            this.numHYP.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numHYP.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHYP.Name = "numHYP";
            this.numHYP.Size = new System.Drawing.Size(31, 20);
            this.numHYP.TabIndex = 51;
            this.numHYP.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numWPCapture
            // 
            this.numWPCapture.Location = new System.Drawing.Point(266, 84);
            this.numWPCapture.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numWPCapture.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWPCapture.Name = "numWPCapture";
            this.numWPCapture.Size = new System.Drawing.Size(31, 20);
            this.numWPCapture.TabIndex = 51;
            this.numWPCapture.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numSP2
            // 
            this.numSP2.Location = new System.Drawing.Point(266, 64);
            this.numSP2.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numSP2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSP2.Name = "numSP2";
            this.numSP2.Size = new System.Drawing.Size(31, 20);
            this.numSP2.TabIndex = 51;
            this.numSP2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numSP1
            // 
            this.numSP1.Location = new System.Drawing.Point(266, 44);
            this.numSP1.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numSP1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSP1.Name = "numSP1";
            this.numSP1.Size = new System.Drawing.Size(31, 20);
            this.numSP1.TabIndex = 51;
            this.numSP1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label76
            // 
            this.label76.Image = ((System.Drawing.Image)(resources.GetObject("label76.Image")));
            this.label76.Location = new System.Drawing.Point(480, 24);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(48, 16);
            this.label76.TabIndex = 27;
            // 
            // numRoll
            // 
            this.numRoll.Location = new System.Drawing.Point(480, 142);
            this.numRoll.Maximum = new decimal(new int[] {
            179,
            0,
            0,
            0});
            this.numRoll.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numRoll.Name = "numRoll";
            this.numRoll.Size = new System.Drawing.Size(48, 20);
            this.numRoll.TabIndex = 49;
            // 
            // numPitch
            // 
            this.numPitch.Location = new System.Drawing.Point(480, 95);
            this.numPitch.Maximum = new decimal(new int[] {
            179,
            0,
            0,
            0});
            this.numPitch.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numPitch.Name = "numPitch";
            this.numPitch.Size = new System.Drawing.Size(48, 20);
            this.numPitch.TabIndex = 48;
            // 
            // numYaw
            // 
            this.numYaw.Location = new System.Drawing.Point(480, 48);
            this.numYaw.Maximum = new decimal(new int[] {
            179,
            0,
            0,
            0});
            this.numYaw.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numYaw.Name = "numYaw";
            this.numYaw.Size = new System.Drawing.Size(48, 20);
            this.numYaw.TabIndex = 47;
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(246, 26);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(64, 16);
            this.label25.TabIndex = 21;
            this.label25.Text = "Region";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label56
            // 
            this.label56.Location = new System.Drawing.Point(359, 177);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(64, 16);
            this.label56.TabIndex = 21;
            this.label56.Text = "Raw Data";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataO_Raw
            // 
            this.dataO_Raw.AllowSorting = false;
            this.dataO_Raw.CaptionVisible = false;
            this.dataO_Raw.DataMember = "";
            this.dataO_Raw.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataO_Raw.Location = new System.Drawing.Point(312, 238);
            this.dataO_Raw.Name = "dataO_Raw";
            this.dataO_Raw.PreferredColumnWidth = 52;
            this.dataO_Raw.PreferredRowHeight = 20;
            this.dataO_Raw.RowHeadersVisible = false;
            this.dataO_Raw.Size = new System.Drawing.Size(160, 183);
            this.dataO_Raw.TabIndex = 20;
            // 
            // dataO
            // 
            this.dataO.AllowSorting = false;
            this.dataO.CaptionVisible = false;
            this.dataO.DataMember = "";
            this.dataO.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataO.Location = new System.Drawing.Point(8, 238);
            this.dataO.Name = "dataO";
            this.dataO.PreferredColumnWidth = 52;
            this.dataO.PreferredRowHeight = 20;
            this.dataO.RowHeadersVisible = false;
            this.dataO.Size = new System.Drawing.Size(160, 183);
            this.dataO.TabIndex = 20;
            // 
            // dataWP
            // 
            this.dataWP.AllowSorting = false;
            this.dataWP.CaptionVisible = false;
            this.dataWP.DataMember = "";
            this.dataWP.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataWP.Location = new System.Drawing.Point(8, 24);
            this.dataWP.Name = "dataWP";
            this.dataWP.PreferredColumnWidth = 52;
            this.dataWP.PreferredRowHeight = 20;
            this.dataWP.RowHeadersVisible = false;
            this.dataWP.Size = new System.Drawing.Size(160, 103);
            this.dataWP.TabIndex = 20;
            // 
            // dataWP_Raw
            // 
            this.dataWP_Raw.AllowSorting = false;
            this.dataWP_Raw.CaptionVisible = false;
            this.dataWP_Raw.DataMember = "";
            this.dataWP_Raw.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataWP_Raw.Location = new System.Drawing.Point(312, 24);
            this.dataWP_Raw.Name = "dataWP_Raw";
            this.dataWP_Raw.PreferredColumnWidth = 52;
            this.dataWP_Raw.PreferredRowHeight = 20;
            this.dataWP_Raw.RowHeadersVisible = false;
            this.dataWP_Raw.Size = new System.Drawing.Size(160, 103);
            this.dataWP_Raw.TabIndex = 19;
            // 
            // chkWPHyp
            // 
            this.chkWPHyp.Location = new System.Drawing.Point(176, 106);
            this.chkWPHyp.Name = "chkWPHyp";
            this.chkWPHyp.Size = new System.Drawing.Size(96, 16);
            this.chkWPHyp.TabIndex = 38;
            this.chkWPHyp.Text = "Hyperspace";
            // 
            // chkWP8
            // 
            this.chkWP8.Location = new System.Drawing.Point(176, 400);
            this.chkWP8.Name = "chkWP8";
            this.chkWP8.Size = new System.Drawing.Size(96, 16);
            this.chkWP8.TabIndex = 36;
            this.chkWP8.Text = "Waypoint 8";
            // 
            // chkWP7
            // 
            this.chkWP7.Location = new System.Drawing.Point(176, 380);
            this.chkWP7.Name = "chkWP7";
            this.chkWP7.Size = new System.Drawing.Size(96, 16);
            this.chkWP7.TabIndex = 35;
            this.chkWP7.Text = "Waypoint 7";
            // 
            // chkWP2
            // 
            this.chkWP2.Location = new System.Drawing.Point(176, 280);
            this.chkWP2.Name = "chkWP2";
            this.chkWP2.Size = new System.Drawing.Size(96, 16);
            this.chkWP2.TabIndex = 28;
            this.chkWP2.Text = "Waypoint 2";
            // 
            // chkWP1
            // 
            this.chkWP1.Location = new System.Drawing.Point(176, 260);
            this.chkWP1.Name = "chkWP1";
            this.chkWP1.Size = new System.Drawing.Size(96, 16);
            this.chkWP1.TabIndex = 26;
            this.chkWP1.Text = "Waypoint 1";
            // 
            // chkWPCapture
            // 
            this.chkWPCapture.Location = new System.Drawing.Point(176, 86);
            this.chkWPCapture.Name = "chkWPCapture";
            this.chkWPCapture.Size = new System.Drawing.Size(96, 16);
            this.chkWPCapture.TabIndex = 24;
            this.chkWPCapture.Text = "Capture HYP";
            // 
            // chkSP2
            // 
            this.chkSP2.Location = new System.Drawing.Point(176, 66);
            this.chkSP2.Name = "chkSP2";
            this.chkSP2.Size = new System.Drawing.Size(96, 16);
            this.chkSP2.TabIndex = 23;
            this.chkSP2.Text = "Start Point2";
            // 
            // chkSP1
            // 
            this.chkSP1.Location = new System.Drawing.Point(176, 46);
            this.chkSP1.Name = "chkSP1";
            this.chkSP1.Size = new System.Drawing.Size(96, 16);
            this.chkSP1.TabIndex = 22;
            this.chkSP1.Text = "Start Point 1";
            // 
            // chkWP6
            // 
            this.chkWP6.Location = new System.Drawing.Point(176, 360);
            this.chkWP6.Name = "chkWP6";
            this.chkWP6.Size = new System.Drawing.Size(96, 16);
            this.chkWP6.TabIndex = 34;
            this.chkWP6.Text = "Waypoint 6";
            // 
            // chkWP5
            // 
            this.chkWP5.Location = new System.Drawing.Point(176, 340);
            this.chkWP5.Name = "chkWP5";
            this.chkWP5.Size = new System.Drawing.Size(96, 16);
            this.chkWP5.TabIndex = 33;
            this.chkWP5.Text = "Waypoint 5";
            // 
            // chkWP4
            // 
            this.chkWP4.Location = new System.Drawing.Point(176, 320);
            this.chkWP4.Name = "chkWP4";
            this.chkWP4.Size = new System.Drawing.Size(96, 16);
            this.chkWP4.TabIndex = 32;
            this.chkWP4.Text = "Waypoint 4";
            // 
            // chkWP3
            // 
            this.chkWP3.Location = new System.Drawing.Point(176, 300);
            this.chkWP3.Name = "chkWP3";
            this.chkWP3.Size = new System.Drawing.Size(96, 16);
            this.chkWP3.TabIndex = 31;
            this.chkWP3.Text = "Waypoint 3";
            // 
            // label77
            // 
            this.label77.Image = ((System.Drawing.Image)(resources.GetObject("label77.Image")));
            this.label77.Location = new System.Drawing.Point(480, 71);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(56, 16);
            this.label77.TabIndex = 29;
            // 
            // label78
            // 
            this.label78.Image = ((System.Drawing.Image)(resources.GetObject("label78.Image")));
            this.label78.Location = new System.Drawing.Point(480, 118);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(56, 16);
            this.label78.TabIndex = 30;
            // 
            // tabOrders
            // 
            this.tabOrders.Controls.Add(this.label95);
            this.tabOrders.Controls.Add(this.cboHandicap);
            this.tabOrders.Controls.Add(this.label104);
            this.tabOrders.Controls.Add(this.cboOSpeed);
            this.tabOrders.Controls.Add(this.lblOVar2Note);
            this.tabOrders.Controls.Add(this.lblOVar3Note);
            this.tabOrders.Controls.Add(this.lblOVar1Note);
            this.tabOrders.Controls.Add(this.lblOSpeedNote);
            this.tabOrders.Controls.Add(this.numORegion);
            this.tabOrders.Controls.Add(this.label103);
            this.tabOrders.Controls.Add(this.label57);
            this.tabOrders.Controls.Add(this.txtOString);
            this.tabOrders.Controls.Add(this.label54);
            this.tabOrders.Controls.Add(this.cmdCopyOrder);
            this.tabOrders.Controls.Add(this.lblODesc);
            this.tabOrders.Controls.Add(this.grpSecOrder);
            this.tabOrders.Controls.Add(this.grpPrimOrder);
            this.tabOrders.Controls.Add(this.numOVar3);
            this.tabOrders.Controls.Add(this.numOVar1);
            this.tabOrders.Controls.Add(this.lblOVar3);
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
            this.tabOrders.Size = new System.Drawing.Size(544, 490);
            this.tabOrders.TabIndex = 4;
            this.tabOrders.Text = "Orders";
            // 
            // label95
            // 
            this.label95.AutoSize = true;
            this.label95.Location = new System.Drawing.Point(202, 470);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(103, 13);
            this.label95.TabIndex = 43;
            this.label95.Text = "(applies to all orders)";
            // 
            // cboHandicap
            // 
            this.cboHandicap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHandicap.FormattingEnabled = true;
            this.cboHandicap.Items.AddRange(new object[] {
            "None",
            "FavorRebels",
            "EvenOnly",
            "FavorImperials",
            "FavorEvenRebels",
            "FavorEvenImperials"});
            this.cboHandicap.Location = new System.Drawing.Point(75, 467);
            this.cboHandicap.Name = "cboHandicap";
            this.cboHandicap.Size = new System.Drawing.Size(121, 21);
            this.cboHandicap.TabIndex = 42;
            // 
            // label104
            // 
            this.label104.AutoSize = true;
            this.label104.Location = new System.Drawing.Point(16, 470);
            this.label104.Name = "label104";
            this.label104.Size = new System.Drawing.Size(53, 13);
            this.label104.TabIndex = 41;
            this.label104.Text = "Handicap";
            // 
            // cboOSpeed
            // 
            this.cboOSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOSpeed.FormattingEnabled = true;
            this.cboOSpeed.Items.AddRange(new object[] {
            "default"});
            this.cboOSpeed.Location = new System.Drawing.Point(168, 206);
            this.cboOSpeed.MaxDropDownItems = 12;
            this.cboOSpeed.Name = "cboOSpeed";
            this.cboOSpeed.Size = new System.Drawing.Size(58, 21);
            this.cboOSpeed.TabIndex = 39;
            this.cboOSpeed.SelectedIndexChanged += new System.EventHandler(this.cboOSpeed_SelectedIndexChanged);
            // 
            // lblOVar2Note
            // 
            this.lblOVar2Note.Location = new System.Drawing.Point(409, 226);
            this.lblOVar2Note.Name = "lblOVar2Note";
            this.lblOVar2Note.Size = new System.Drawing.Size(120, 16);
            this.lblOVar2Note.TabIndex = 38;
            this.lblOVar2Note.Text = "lblOVar2Note";
            this.lblOVar2Note.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblOVar2Note.Visible = false;
            // 
            // lblOVar3Note
            // 
            this.lblOVar3Note.Location = new System.Drawing.Point(372, 248);
            this.lblOVar3Note.Name = "lblOVar3Note";
            this.lblOVar3Note.Size = new System.Drawing.Size(120, 16);
            this.lblOVar3Note.TabIndex = 38;
            this.lblOVar3Note.Text = "lblOVar3Note";
            this.lblOVar3Note.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblOVar3Note.Visible = false;
            // 
            // lblOVar1Note
            // 
            this.lblOVar1Note.Location = new System.Drawing.Point(246, 227);
            this.lblOVar1Note.Name = "lblOVar1Note";
            this.lblOVar1Note.Size = new System.Drawing.Size(120, 16);
            this.lblOVar1Note.TabIndex = 38;
            this.lblOVar1Note.Text = "lblOVar1Note";
            this.lblOVar1Note.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblOVar1Note.Visible = false;
            // 
            // lblOSpeedNote
            // 
            this.lblOSpeedNote.AutoSize = true;
            this.lblOSpeedNote.Location = new System.Drawing.Point(176, 230);
            this.lblOSpeedNote.Name = "lblOSpeedNote";
            this.lblOSpeedNote.Size = new System.Drawing.Size(37, 13);
            this.lblOSpeedNote.TabIndex = 37;
            this.lblOSpeedNote.Text = "MGLT";
            // 
            // numORegion
            // 
            this.numORegion.Location = new System.Drawing.Point(471, 12);
            this.numORegion.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numORegion.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numORegion.Name = "numORegion";
            this.numORegion.Size = new System.Drawing.Size(33, 20);
            this.numORegion.TabIndex = 36;
            this.numORegion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numORegion.ValueChanged += new System.EventHandler(this.numORegion_ValueChanged);
            // 
            // label103
            // 
            this.label103.AutoSize = true;
            this.label103.Location = new System.Drawing.Point(423, 14);
            this.label103.Name = "label103";
            this.label103.Size = new System.Drawing.Size(41, 13);
            this.label103.TabIndex = 35;
            this.label103.Text = "Region";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(124, 208);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(41, 13);
            this.label57.TabIndex = 33;
            this.label57.Text = "Speed:";
            this.label57.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // txtOString
            // 
            this.txtOString.Location = new System.Drawing.Point(115, 40);
            this.txtOString.MaxLength = 63;
            this.txtOString.Name = "txtOString";
            this.txtOString.Size = new System.Drawing.Size(389, 20);
            this.txtOString.TabIndex = 32;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(11, 43);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(98, 13);
            this.label54.TabIndex = 31;
            this.label54.Text = "CMD Display String";
            // 
            // cmdCopyOrder
            // 
            this.cmdCopyOrder.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.cmdCopyOrder.ImageIndex = 6;
            this.cmdCopyOrder.ImageList = this.imgToolbar;
            this.cmdCopyOrder.Location = new System.Drawing.Point(19, 9);
            this.cmdCopyOrder.Name = "cmdCopyOrder";
            this.cmdCopyOrder.Size = new System.Drawing.Size(24, 23);
            this.cmdCopyOrder.TabIndex = 29;
            this.cmdCopyOrder.Click += new System.EventHandler(this.cmdCopyOrder_Click);
            // 
            // lblODesc
            // 
            this.lblODesc.Location = new System.Drawing.Point(18, 65);
            this.lblODesc.Name = "lblODesc";
            this.lblODesc.Size = new System.Drawing.Size(512, 16);
            this.lblODesc.TabIndex = 28;
            this.lblODesc.Text = "lblODesc";
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
            this.grpSecOrder.Location = new System.Drawing.Point(16, 367);
            this.grpSecOrder.Name = "grpSecOrder";
            this.grpSecOrder.Size = new System.Drawing.Size(512, 98);
            this.grpSecOrder.TabIndex = 27;
            this.grpSecOrder.TabStop = false;
            this.grpSecOrder.Text = "Secondary Target";
            // 
            // label49
            // 
            this.label49.Location = new System.Drawing.Point(16, 30);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(96, 48);
            this.label49.TabIndex = 3;
            this.label49.Text = "Selecting \"OR\" allows for multiple targets";
            // 
            // optOT3T4OR
            // 
            this.optOT3T4OR.AutoSize = true;
            this.optOT3T4OR.Checked = true;
            this.optOT3T4OR.Location = new System.Drawing.Point(312, 46);
            this.optOT3T4OR.Name = "optOT3T4OR";
            this.optOT3T4OR.Size = new System.Drawing.Size(41, 17);
            this.optOT3T4OR.TabIndex = 13;
            this.optOT3T4OR.TabStop = true;
            this.optOT3T4OR.Text = "OR";
            // 
            // cboOT3
            // 
            this.cboOT3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOT3.Location = new System.Drawing.Point(304, 18);
            this.cboOT3.Name = "cboOT3";
            this.cboOT3.Size = new System.Drawing.Size(184, 21);
            this.cboOT3.TabIndex = 11;
            // 
            // cboOT3Type
            // 
            this.cboOT3Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOT3Type.Location = new System.Drawing.Point(128, 18);
            this.cboOT3Type.Name = "cboOT3Type";
            this.cboOT3Type.Size = new System.Drawing.Size(160, 21);
            this.cboOT3Type.TabIndex = 10;
            this.cboOT3Type.SelectedIndexChanged += new System.EventHandler(this.cboOT3Type_SelectedIndexChanged);
            // 
            // cboOT4Type
            // 
            this.cboOT4Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOT4Type.Location = new System.Drawing.Point(128, 69);
            this.cboOT4Type.Name = "cboOT4Type";
            this.cboOT4Type.Size = new System.Drawing.Size(160, 21);
            this.cboOT4Type.TabIndex = 14;
            this.cboOT4Type.SelectedIndexChanged += new System.EventHandler(this.cboOT4Type_SelectedIndexChanged);
            // 
            // cboOT4
            // 
            this.cboOT4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOT4.Location = new System.Drawing.Point(304, 69);
            this.cboOT4.Name = "cboOT4";
            this.cboOT4.Size = new System.Drawing.Size(184, 21);
            this.cboOT4.TabIndex = 15;
            // 
            // optOT3T4AND
            // 
            this.optOT3T4AND.AutoSize = true;
            this.optOT3T4AND.Location = new System.Drawing.Point(240, 46);
            this.optOT3T4AND.Name = "optOT3T4AND";
            this.optOT3T4AND.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.optOT3T4AND.Size = new System.Drawing.Size(48, 17);
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
            this.grpPrimOrder.Location = new System.Drawing.Point(16, 265);
            this.grpPrimOrder.Name = "grpPrimOrder";
            this.grpPrimOrder.Size = new System.Drawing.Size(512, 98);
            this.grpPrimOrder.TabIndex = 26;
            this.grpPrimOrder.TabStop = false;
            this.grpPrimOrder.Text = "Primary Target";
            // 
            // label50
            // 
            this.label50.Location = new System.Drawing.Point(16, 23);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(96, 56);
            this.label50.TabIndex = 2;
            this.label50.Text = "Selecting \"AND\" will require that the target meet both settings";
            // 
            // optOT1T2OR
            // 
            this.optOT1T2OR.AutoSize = true;
            this.optOT1T2OR.Checked = true;
            this.optOT1T2OR.Location = new System.Drawing.Point(304, 47);
            this.optOT1T2OR.Name = "optOT1T2OR";
            this.optOT1T2OR.Size = new System.Drawing.Size(41, 17);
            this.optOT1T2OR.TabIndex = 7;
            this.optOT1T2OR.TabStop = true;
            this.optOT1T2OR.Text = "OR";
            // 
            // cboOT1
            // 
            this.cboOT1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOT1.Location = new System.Drawing.Point(304, 20);
            this.cboOT1.Name = "cboOT1";
            this.cboOT1.Size = new System.Drawing.Size(184, 21);
            this.cboOT1.TabIndex = 5;
            // 
            // cboOT1Type
            // 
            this.cboOT1Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOT1Type.Location = new System.Drawing.Point(128, 20);
            this.cboOT1Type.Name = "cboOT1Type";
            this.cboOT1Type.Size = new System.Drawing.Size(160, 21);
            this.cboOT1Type.TabIndex = 4;
            this.cboOT1Type.SelectedIndexChanged += new System.EventHandler(this.cboOT1Type_SelectedIndexChanged);
            // 
            // cboOT2Type
            // 
            this.cboOT2Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOT2Type.Location = new System.Drawing.Point(128, 70);
            this.cboOT2Type.Name = "cboOT2Type";
            this.cboOT2Type.Size = new System.Drawing.Size(160, 21);
            this.cboOT2Type.TabIndex = 8;
            this.cboOT2Type.SelectedIndexChanged += new System.EventHandler(this.cboOT2Type_SelectedIndexChanged);
            // 
            // cboOT2
            // 
            this.cboOT2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOT2.Location = new System.Drawing.Point(304, 70);
            this.cboOT2.Name = "cboOT2";
            this.cboOT2.Size = new System.Drawing.Size(184, 21);
            this.cboOT2.TabIndex = 9;
            // 
            // optOT1T2AND
            // 
            this.optOT1T2AND.AutoSize = true;
            this.optOT1T2AND.Location = new System.Drawing.Point(240, 47);
            this.optOT1T2AND.Name = "optOT1T2AND";
            this.optOT1T2AND.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.optOT1T2AND.Size = new System.Drawing.Size(48, 17);
            this.optOT1T2AND.TabIndex = 6;
            this.optOT1T2AND.Text = "AND";
            // 
            // numOVar3
            // 
            this.numOVar3.Location = new System.Drawing.Point(326, 246);
            this.numOVar3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numOVar3.Name = "numOVar3";
            this.numOVar3.Size = new System.Drawing.Size(40, 20);
            this.numOVar3.TabIndex = 22;
            this.numOVar3.ValueChanged += new System.EventHandler(this.numOVar3_ValueChanged);
            // 
            // numOVar1
            // 
            this.numOVar1.Location = new System.Drawing.Point(320, 204);
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
            // lblOVar3
            // 
            this.lblOVar3.Location = new System.Drawing.Point(206, 248);
            this.lblOVar3.Name = "lblOVar3";
            this.lblOVar3.Size = new System.Drawing.Size(120, 16);
            this.lblOVar3.TabIndex = 25;
            this.lblOVar3.Text = "lblOVar3";
            this.lblOVar3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lblOVar1
            // 
            this.lblOVar1.Location = new System.Drawing.Point(200, 206);
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
            this.cboOThrottle.Location = new System.Drawing.Point(70, 206);
            this.cboOThrottle.Name = "cboOThrottle";
            this.cboOThrottle.Size = new System.Drawing.Size(48, 21);
            this.cboOThrottle.TabIndex = 19;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(11, 208);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(54, 13);
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
            this.groupBox15.Location = new System.Drawing.Point(8, 82);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(528, 116);
            this.groupBox15.TabIndex = 20;
            this.groupBox15.TabStop = false;
            // 
            // lblOrder1
            // 
            this.lblOrder1.BackColor = System.Drawing.Color.RosyBrown;
            this.lblOrder1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblOrder1.Location = new System.Drawing.Point(8, 12);
            this.lblOrder1.Name = "lblOrder1";
            this.lblOrder1.Size = new System.Drawing.Size(512, 24);
            this.lblOrder1.TabIndex = 0;
            this.lblOrder1.Text = "Order 1:";
            this.lblOrder1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOrder2
            // 
            this.lblOrder2.BackColor = System.Drawing.Color.RosyBrown;
            this.lblOrder2.Location = new System.Drawing.Point(8, 36);
            this.lblOrder2.Name = "lblOrder2";
            this.lblOrder2.Size = new System.Drawing.Size(512, 24);
            this.lblOrder2.TabIndex = 0;
            this.lblOrder2.Text = "Order 2:";
            this.lblOrder2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOrder3
            // 
            this.lblOrder3.BackColor = System.Drawing.Color.RosyBrown;
            this.lblOrder3.Location = new System.Drawing.Point(8, 60);
            this.lblOrder3.Name = "lblOrder3";
            this.lblOrder3.Size = new System.Drawing.Size(512, 24);
            this.lblOrder3.TabIndex = 0;
            this.lblOrder3.Text = "Order 3:";
            this.lblOrder3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOrder4
            // 
            this.lblOrder4.BackColor = System.Drawing.Color.RosyBrown;
            this.lblOrder4.Location = new System.Drawing.Point(8, 84);
            this.lblOrder4.Name = "lblOrder4";
            this.lblOrder4.Size = new System.Drawing.Size(512, 24);
            this.lblOrder4.TabIndex = 0;
            this.lblOrder4.Text = "Order 4:";
            this.lblOrder4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboOrders
            // 
            this.cboOrders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOrders.Location = new System.Drawing.Point(83, 9);
            this.cboOrders.Name = "cboOrders";
            this.cboOrders.Size = new System.Drawing.Size(178, 21);
            this.cboOrders.TabIndex = 18;
            this.cboOrders.SelectedIndexChanged += new System.EventHandler(this.cboOrders_SelectedIndexChanged);
            // 
            // lblOVar2
            // 
            this.lblOVar2.Location = new System.Drawing.Point(376, 205);
            this.lblOVar2.Name = "lblOVar2";
            this.lblOVar2.Size = new System.Drawing.Size(112, 16);
            this.lblOVar2.TabIndex = 24;
            this.lblOVar2.Text = "lblOVar2";
            this.lblOVar2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // numOVar2
            // 
            this.numOVar2.Location = new System.Drawing.Point(488, 203);
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
            this.cmdPasteOrder.Location = new System.Drawing.Point(51, 9);
            this.cmdPasteOrder.Name = "cmdPasteOrder";
            this.cmdPasteOrder.Size = new System.Drawing.Size(24, 23);
            this.cmdPasteOrder.TabIndex = 30;
            this.cmdPasteOrder.Click += new System.EventHandler(this.cmdPasteOrder_Click);
            // 
            // tapOption
            // 
            this.tapOption.Controls.Add(this.cboPilot);
            this.tapOption.Controls.Add(this.label80);
            this.tapOption.Controls.Add(this.grpRole);
            this.tapOption.Controls.Add(this.grpSkip);
            this.tapOption.Controls.Add(this.groupBox22);
            this.tapOption.Controls.Add(this.groupBox21);
            this.tapOption.Controls.Add(this.groupBox20);
            this.tapOption.Controls.Add(this.groupBox19);
            this.tapOption.Location = new System.Drawing.Point(4, 22);
            this.tapOption.Name = "tapOption";
            this.tapOption.Size = new System.Drawing.Size(544, 490);
            this.tapOption.TabIndex = 6;
            this.tapOption.Text = "Options";
            // 
            // cboPilot
            // 
            this.cboPilot.FormattingEnabled = true;
            this.cboPilot.Items.AddRange(new object[] {
            "Aeron",
            "Emon",
            "Emkay",
            "RP1",
            "RP2",
            "RP3",
            "RP4",
            "RP5",
            "RP6",
            "RP7",
            "RP8",
            "RP9",
            "RP10",
            "RP11",
            "RP12"});
            this.cboPilot.Location = new System.Drawing.Point(423, 266);
            this.cboPilot.MaxLength = 15;
            this.cboPilot.Name = "cboPilot";
            this.cboPilot.Size = new System.Drawing.Size(100, 21);
            this.cboPilot.TabIndex = 8;
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Location = new System.Drawing.Point(391, 269);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(27, 13);
            this.label80.TabIndex = 7;
            this.label80.Text = "Pilot";
            // 
            // grpRole
            // 
            this.grpRole.Controls.Add(this.cboRole2Teams);
            this.grpRole.Controls.Add(this.cboRole1Teams);
            this.grpRole.Controls.Add(this.label73);
            this.grpRole.Controls.Add(this.txtRole);
            this.grpRole.Controls.Add(this.cboRole1);
            this.grpRole.Controls.Add(this.cboRole2);
            this.grpRole.Location = new System.Drawing.Point(394, 98);
            this.grpRole.Name = "grpRole";
            this.grpRole.Size = new System.Drawing.Size(136, 160);
            this.grpRole.TabIndex = 5;
            this.grpRole.TabStop = false;
            this.grpRole.Text = "Roles";
            // 
            // cboRole2Teams
            // 
            this.cboRole2Teams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole2Teams.FormattingEnabled = true;
            this.cboRole2Teams.Location = new System.Drawing.Point(11, 69);
            this.cboRole2Teams.Name = "cboRole2Teams";
            this.cboRole2Teams.Size = new System.Drawing.Size(120, 21);
            this.cboRole2Teams.TabIndex = 6;
            this.cboRole2Teams.SelectedIndexChanged += new System.EventHandler(this.cboRole2Teams_SelectedIndexChanged);
            // 
            // cboRole1Teams
            // 
            this.cboRole1Teams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole1Teams.FormattingEnabled = true;
            this.cboRole1Teams.Location = new System.Drawing.Point(11, 17);
            this.cboRole1Teams.Name = "cboRole1Teams";
            this.cboRole1Teams.Size = new System.Drawing.Size(120, 21);
            this.cboRole1Teams.TabIndex = 5;
            this.cboRole1Teams.SelectedIndexChanged += new System.EventHandler(this.cboRole1Teams_SelectedIndexChanged);
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Location = new System.Drawing.Point(9, 115);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(85, 13);
            this.label73.TabIndex = 4;
            this.label73.Text = "Role Description";
            // 
            // txtRole
            // 
            this.txtRole.Location = new System.Drawing.Point(13, 132);
            this.txtRole.MaxLength = 19;
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(118, 20);
            this.txtRole.TabIndex = 3;
            // 
            // cboRole1
            // 
            this.cboRole1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole1.Enabled = false;
            this.cboRole1.Location = new System.Drawing.Point(11, 39);
            this.cboRole1.Name = "cboRole1";
            this.cboRole1.Size = new System.Drawing.Size(120, 21);
            this.cboRole1.TabIndex = 0;
            // 
            // cboRole2
            // 
            this.cboRole2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole2.Enabled = false;
            this.cboRole2.Location = new System.Drawing.Point(11, 91);
            this.cboRole2.Name = "cboRole2";
            this.cboRole2.Size = new System.Drawing.Size(120, 21);
            this.cboRole2.TabIndex = 0;
            // 
            // grpSkip
            // 
            this.grpSkip.Controls.Add(this.cboSkipPara);
            this.grpSkip.Controls.Add(this.cboSkipOrder);
            this.grpSkip.Controls.Add(this.label74);
            this.grpSkip.Controls.Add(this.cmdCopySkip);
            this.grpSkip.Controls.Add(this.label71);
            this.grpSkip.Controls.Add(this.cboSkipAmount);
            this.grpSkip.Controls.Add(this.cboSkipType);
            this.grpSkip.Controls.Add(this.cboSkipVar);
            this.grpSkip.Controls.Add(this.cboSkipTrig);
            this.grpSkip.Controls.Add(this.label72);
            this.grpSkip.Controls.Add(this.cmdPasteSkip);
            this.grpSkip.Controls.Add(this.optSkipAND);
            this.grpSkip.Controls.Add(this.optSkipOR);
            this.grpSkip.Controls.Add(this.lblSkipTrig1);
            this.grpSkip.Controls.Add(this.lblSkipTrig2);
            this.grpSkip.Location = new System.Drawing.Point(16, 288);
            this.grpSkip.Name = "grpSkip";
            this.grpSkip.Size = new System.Drawing.Size(494, 176);
            this.grpSkip.TabIndex = 4;
            this.grpSkip.TabStop = false;
            this.grpSkip.Text = "Skip to Order";
            // 
            // cboSkipPara
            // 
            this.cboSkipPara.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkipPara.FormattingEnabled = true;
            this.cboSkipPara.Location = new System.Drawing.Point(378, 144);
            this.cboSkipPara.Name = "cboSkipPara";
            this.cboSkipPara.Size = new System.Drawing.Size(110, 21);
            this.cboSkipPara.TabIndex = 55;
            // 
            // cboSkipOrder
            // 
            this.cboSkipOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkipOrder.FormattingEnabled = true;
            this.cboSkipOrder.Items.AddRange(new object[] {
            "1, Region 1",
            "2, Region 1",
            "3, Region 1",
            "4, Region 1",
            "1, Region 2",
            "2, Region 2",
            "3, Region 2",
            "4, Region 2",
            "1, Region 3",
            "2, Region 3",
            "3, Region 3",
            "4, Region 3",
            "1, Region 4",
            "2, Region 4",
            "3, Region 4",
            "4, Region 4"});
            this.cboSkipOrder.Location = new System.Drawing.Point(388, 59);
            this.cboSkipOrder.Name = "cboSkipOrder";
            this.cboSkipOrder.Size = new System.Drawing.Size(98, 21);
            this.cboSkipOrder.TabIndex = 52;
            this.cboSkipOrder.SelectedIndexChanged += new System.EventHandler(this.cboSkipOrder_SelectedIndexChanged);
            // 
            // label74
            // 
            this.label74.Location = new System.Drawing.Point(412, 36);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(42, 20);
            this.label74.TabIndex = 51;
            this.label74.Text = "Order:";
            this.label74.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.cmdCopySkip.Click += new System.EventHandler(this.cmdCopySkip_Click);
            // 
            // label71
            // 
            this.label71.Location = new System.Drawing.Point(190, 120);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(16, 16);
            this.label71.TabIndex = 40;
            this.label71.Text = "of";
            this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboSkipAmount
            // 
            this.cboSkipAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkipAmount.Location = new System.Drawing.Point(38, 120);
            this.cboSkipAmount.Name = "cboSkipAmount";
            this.cboSkipAmount.Size = new System.Drawing.Size(144, 21);
            this.cboSkipAmount.TabIndex = 41;
            // 
            // cboSkipType
            // 
            this.cboSkipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkipType.Location = new System.Drawing.Point(214, 120);
            this.cboSkipType.Name = "cboSkipType";
            this.cboSkipType.Size = new System.Drawing.Size(160, 21);
            this.cboSkipType.TabIndex = 42;
            this.cboSkipType.SelectedIndexChanged += new System.EventHandler(this.cboSkipType_SelectedIndexChanged);
            // 
            // cboSkipVar
            // 
            this.cboSkipVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkipVar.Location = new System.Drawing.Point(38, 144);
            this.cboSkipVar.Name = "cboSkipVar";
            this.cboSkipVar.Size = new System.Drawing.Size(144, 21);
            this.cboSkipVar.TabIndex = 43;
            // 
            // cboSkipTrig
            // 
            this.cboSkipTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSkipTrig.Location = new System.Drawing.Point(214, 144);
            this.cboSkipTrig.Name = "cboSkipTrig";
            this.cboSkipTrig.Size = new System.Drawing.Size(160, 21);
            this.cboSkipTrig.TabIndex = 44;
            this.cboSkipTrig.SelectedIndexChanged += new System.EventHandler(this.cboSkipTrig_SelectedIndexChanged);
            // 
            // label72
            // 
            this.label72.Location = new System.Drawing.Point(182, 144);
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
            this.cmdPasteSkip.Click += new System.EventHandler(this.cmdPasteSkip_Click);
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
            this.groupBox21.Controls.Add(this.chkOptCCluster);
            this.groupBox21.Controls.Add(this.chkOptCNone);
            this.groupBox21.Controls.Add(this.chkOptCChaff);
            this.groupBox21.Controls.Add(this.chkOptCFlare);
            this.groupBox21.Location = new System.Drawing.Point(394, 8);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(112, 86);
            this.groupBox21.TabIndex = 2;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "Countermeasures";
            // 
            // chkOptCCluster
            // 
            this.chkOptCCluster.Location = new System.Drawing.Point(8, 64);
            this.chkOptCCluster.Name = "chkOptCCluster";
            this.chkOptCCluster.Size = new System.Drawing.Size(96, 20);
            this.chkOptCCluster.TabIndex = 9;
            this.chkOptCCluster.Text = "(Cluster Mine)";
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
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.chkOptBEnergy);
            this.groupBox20.Controls.Add(this.chkOptBNone);
            this.groupBox20.Controls.Add(this.chkOptBTractor);
            this.groupBox20.Controls.Add(this.chkOptBJamming);
            this.groupBox20.Controls.Add(this.chkOptBDecoy);
            this.groupBox20.Location = new System.Drawing.Point(262, 178);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(112, 102);
            this.groupBox20.TabIndex = 1;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Beams";
            // 
            // chkOptBEnergy
            // 
            this.chkOptBEnergy.Location = new System.Drawing.Point(8, 80);
            this.chkOptBEnergy.Name = "chkOptBEnergy";
            this.chkOptBEnergy.Size = new System.Drawing.Size(96, 20);
            this.chkOptBEnergy.TabIndex = 4;
            this.chkOptBEnergy.Text = "(Energy)";
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
            this.chkOptBTractor.TabIndex = 1;
            this.chkOptBTractor.Text = "Tractor";
            // 
            // chkOptBJamming
            // 
            this.chkOptBJamming.Location = new System.Drawing.Point(8, 48);
            this.chkOptBJamming.Name = "chkOptBJamming";
            this.chkOptBJamming.Size = new System.Drawing.Size(96, 20);
            this.chkOptBJamming.TabIndex = 2;
            this.chkOptBJamming.Text = "Jamming";
            // 
            // chkOptBDecoy
            // 
            this.chkOptBDecoy.Location = new System.Drawing.Point(8, 64);
            this.chkOptBDecoy.Name = "chkOptBDecoy";
            this.chkOptBDecoy.Size = new System.Drawing.Size(96, 20);
            this.chkOptBDecoy.TabIndex = 3;
            this.chkOptBDecoy.Text = "Decoy";
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
            this.groupBox19.Controls.Add(this.chkOptWIonPulse);
            this.groupBox19.Controls.Add(this.chkOptWMagPulse);
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
            // chkOptWIonPulse
            // 
            this.chkOptWIonPulse.Location = new System.Drawing.Point(8, 144);
            this.chkOptWIonPulse.Name = "chkOptWIonPulse";
            this.chkOptWIonPulse.Size = new System.Drawing.Size(96, 20);
            this.chkOptWIonPulse.TabIndex = 0;
            this.chkOptWIonPulse.Text = "(Ion Pulse)";
            // 
            // chkOptWMagPulse
            // 
            this.chkOptWMagPulse.Location = new System.Drawing.Point(8, 128);
            this.chkOptWMagPulse.Name = "chkOptWMagPulse";
            this.chkOptWMagPulse.Size = new System.Drawing.Size(96, 20);
            this.chkOptWMagPulse.TabIndex = 0;
            this.chkOptWMagPulse.Text = "Mag Pulse";
            // 
            // tabArrDep2
            // 
            this.tabArrDep2.Controls.Add(this.grpMoreDep);
            this.tabArrDep2.Controls.Add(this.grpMoreArrival);
            this.tabArrDep2.Location = new System.Drawing.Point(4, 22);
            this.tabArrDep2.Name = "tabArrDep2";
            this.tabArrDep2.Size = new System.Drawing.Size(544, 490);
            this.tabArrDep2.TabIndex = 5;
            this.tabArrDep2.Text = "More Arr/Dep";
            // 
            // grpMoreDep
            // 
            this.grpMoreDep.Controls.Add(this.label94);
            this.grpMoreDep.Controls.Add(this.label93);
            this.grpMoreDep.Controls.Add(this.label92);
            this.grpMoreDep.Controls.Add(this.numDepClockSec);
            this.grpMoreDep.Controls.Add(this.numDepClockMin);
            this.grpMoreDep.Location = new System.Drawing.Point(273, 15);
            this.grpMoreDep.Name = "grpMoreDep";
            this.grpMoreDep.Size = new System.Drawing.Size(256, 81);
            this.grpMoreDep.TabIndex = 14;
            this.grpMoreDep.TabStop = false;
            this.grpMoreDep.Text = "Departure";
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.Location = new System.Drawing.Point(143, 42);
            this.label94.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(26, 13);
            this.label94.TabIndex = 8;
            this.label94.Text = "Sec";
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Location = new System.Drawing.Point(69, 42);
            this.label93.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(24, 13);
            this.label93.TabIndex = 7;
            this.label93.Text = "Min";
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Location = new System.Drawing.Point(22, 24);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(113, 13);
            this.label92.TabIndex = 0;
            this.label92.Text = "Depart at mission time:";
            // 
            // numDepClockSec
            // 
            this.numDepClockSec.Location = new System.Drawing.Point(99, 40);
            this.numDepClockSec.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numDepClockSec.Name = "numDepClockSec";
            this.numDepClockSec.Size = new System.Drawing.Size(40, 20);
            this.numDepClockSec.TabIndex = 6;
            this.numDepClockSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numDepClockMin
            // 
            this.numDepClockMin.Location = new System.Drawing.Point(25, 40);
            this.numDepClockMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numDepClockMin.Name = "numDepClockMin";
            this.numDepClockMin.Size = new System.Drawing.Size(40, 20);
            this.numDepClockMin.TabIndex = 6;
            this.numDepClockMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // grpMoreArrival
            // 
            this.grpMoreArrival.Controls.Add(this.numArrRandMin);
            this.grpMoreArrival.Controls.Add(this.numArrRandSec);
            this.grpMoreArrival.Controls.Add(this.lblRandomArrivalDelayMinutes);
            this.grpMoreArrival.Controls.Add(this.lblRandomArrivalDelaySeconds);
            this.grpMoreArrival.Controls.Add(this.lblRandomArrivalDelayDesc);
            this.grpMoreArrival.Controls.Add(this.cboStopArrivingWhen);
            this.grpMoreArrival.Controls.Add(this.lblStopArrivingWhen);
            this.grpMoreArrival.Location = new System.Drawing.Point(11, 15);
            this.grpMoreArrival.Name = "grpMoreArrival";
            this.grpMoreArrival.Size = new System.Drawing.Size(256, 225);
            this.grpMoreArrival.TabIndex = 13;
            this.grpMoreArrival.TabStop = false;
            this.grpMoreArrival.Text = "Arrival";
            // 
            // numArrRandMin
            // 
            this.numArrRandMin.Location = new System.Drawing.Point(36, 195);
            this.numArrRandMin.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.numArrRandMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numArrRandMin.Name = "numArrRandMin";
            this.numArrRandMin.Size = new System.Drawing.Size(42, 20);
            this.numArrRandMin.TabIndex = 2;
            this.numArrRandMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numArrRandSec
            // 
            this.numArrRandSec.Location = new System.Drawing.Point(128, 195);
            this.numArrRandSec.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numArrRandSec.Name = "numArrRandSec";
            this.numArrRandSec.Size = new System.Drawing.Size(40, 20);
            this.numArrRandSec.TabIndex = 3;
            this.numArrRandSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblRandomArrivalDelayMinutes
            // 
            this.lblRandomArrivalDelayMinutes.AutoSize = true;
            this.lblRandomArrivalDelayMinutes.Location = new System.Drawing.Point(80, 197);
            this.lblRandomArrivalDelayMinutes.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.lblRandomArrivalDelayMinutes.Name = "lblRandomArrivalDelayMinutes";
            this.lblRandomArrivalDelayMinutes.Size = new System.Drawing.Size(24, 13);
            this.lblRandomArrivalDelayMinutes.TabIndex = 3;
            this.lblRandomArrivalDelayMinutes.Text = "Min";
            // 
            // lblRandomArrivalDelaySeconds
            // 
            this.lblRandomArrivalDelaySeconds.AutoSize = true;
            this.lblRandomArrivalDelaySeconds.Location = new System.Drawing.Point(172, 197);
            this.lblRandomArrivalDelaySeconds.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.lblRandomArrivalDelaySeconds.Name = "lblRandomArrivalDelaySeconds";
            this.lblRandomArrivalDelaySeconds.Size = new System.Drawing.Size(26, 13);
            this.lblRandomArrivalDelaySeconds.TabIndex = 3;
            this.lblRandomArrivalDelaySeconds.Text = "Sec";
            // 
            // lblRandomArrivalDelayDesc
            // 
            this.lblRandomArrivalDelayDesc.Location = new System.Drawing.Point(17, 78);
            this.lblRandomArrivalDelayDesc.Name = "lblRandomArrivalDelayDesc";
            this.lblRandomArrivalDelayDesc.Size = new System.Drawing.Size(228, 114);
            this.lblRandomArrivalDelayDesc.TabIndex = 2;
            this.lblRandomArrivalDelayDesc.Text = resources.GetString("lblRandomArrivalDelayDesc.Text");
            // 
            // cboStopArrivingWhen
            // 
            this.cboStopArrivingWhen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStopArrivingWhen.FormattingEnabled = true;
            this.cboStopArrivingWhen.Items.AddRange(new object[] {
            "Never (Continues to Arrive)",
            "Mission Complete",
            "Team Wins",
            "Team Loses"});
            this.cboStopArrivingWhen.Location = new System.Drawing.Point(20, 40);
            this.cboStopArrivingWhen.Name = "cboStopArrivingWhen";
            this.cboStopArrivingWhen.Size = new System.Drawing.Size(208, 21);
            this.cboStopArrivingWhen.TabIndex = 1;
            // 
            // lblStopArrivingWhen
            // 
            this.lblStopArrivingWhen.AutoSize = true;
            this.lblStopArrivingWhen.Location = new System.Drawing.Point(17, 24);
            this.lblStopArrivingWhen.Name = "lblStopArrivingWhen";
            this.lblStopArrivingWhen.Size = new System.Drawing.Size(98, 13);
            this.lblStopArrivingWhen.TabIndex = 0;
            this.lblStopArrivingWhen.Text = "Stop arriving when:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Tm - GG - waves x craft (GU)";
            // 
            // lstFG
            // 
            this.lstFG.BackColor = System.Drawing.Color.Black;
            this.lstFG.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstFG.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstFG.ForeColor = System.Drawing.Color.Gray;
            this.lstFG.ItemHeight = 15;
            this.lstFG.Items.AddRange(new object[] {
            "3 - 12 - *1x(3) Ship name"});
            this.lstFG.Location = new System.Drawing.Point(4, 24);
            this.lstFG.Name = "lstFG";
            this.lstFG.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstFG.Size = new System.Drawing.Size(222, 454);
            this.lstFG.TabIndex = 3;
            this.lstFG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstFG_DrawItem);
            this.lstFG.SelectedIndexChanged += new System.EventHandler(this.lstFG_SelectedIndexChanged);
            // 
            // tabMess
            // 
            this.tabMess.BackColor = System.Drawing.SystemColors.Control;
            this.tabMess.Controls.Add(this.cboMessSpecial);
            this.tabMess.Controls.Add(this.label106);
            this.tabMess.Controls.Add(this.label105);
            this.tabMess.Controls.Add(this.chkMessHeader);
            this.tabMess.Controls.Add(this.numMessType);
            this.tabMess.Controls.Add(this.lblDelay);
            this.tabMess.Controls.Add(this.cmdMoveMessDown);
            this.tabMess.Controls.Add(this.cmdMoveMessUp);
            this.tabMess.Controls.Add(this.cboMessPara);
            this.tabMess.Controls.Add(this.cboMessFG);
            this.tabMess.Controls.Add(this.txtVoice);
            this.tabMess.Controls.Add(this.grpMessCancel);
            this.tabMess.Controls.Add(this.cboMessColor);
            this.tabMess.Controls.Add(this.label109);
            this.tabMess.Controls.Add(this.cboMessAmount);
            this.tabMess.Controls.Add(this.cboMessType);
            this.tabMess.Controls.Add(this.cboMessVar);
            this.tabMess.Controls.Add(this.cboMessTrig);
            this.tabMess.Controls.Add(this.label110);
            this.tabMess.Controls.Add(this.grpMessages);
            this.tabMess.Controls.Add(this.grpSend);
            this.tabMess.Controls.Add(this.numMessDelay);
            this.tabMess.Controls.Add(this.label55);
            this.tabMess.Controls.Add(this.lblMessage);
            this.tabMess.Controls.Add(this.txtMessNote);
            this.tabMess.Controls.Add(this.txtMessage);
            this.tabMess.Controls.Add(this.label26);
            this.tabMess.Controls.Add(this.label149);
            this.tabMess.Controls.Add(this.label52);
            this.tabMess.Controls.Add(this.lstMessages);
            this.tabMess.Location = new System.Drawing.Point(4, 22);
            this.tabMess.Name = "tabMess";
            this.tabMess.Size = new System.Drawing.Size(785, 519);
            this.tabMess.TabIndex = 1;
            this.tabMess.Text = "Messages";
            // 
            // cboMessSpecial
            // 
            this.cboMessSpecial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessSpecial.FormattingEnabled = true;
            this.cboMessSpecial.Items.AddRange(new object[] {
            "Ignore",
            "Stop",
            "Finished",
            "Both"});
            this.cboMessSpecial.Location = new System.Drawing.Point(672, 465);
            this.cboMessSpecial.Name = "cboMessSpecial";
            this.cboMessSpecial.Size = new System.Drawing.Size(105, 21);
            this.cboMessSpecial.TabIndex = 49;
            // 
            // label106
            // 
            this.label106.AutoSize = true;
            this.label106.Location = new System.Drawing.Point(669, 449);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(89, 13);
            this.label106.TabIndex = 48;
            this.label106.Text = "Special Meaning:";
            // 
            // label105
            // 
            this.label105.AutoSize = true;
            this.label105.Location = new System.Drawing.Point(691, 405);
            this.label105.Name = "label105";
            this.label105.Size = new System.Drawing.Size(31, 13);
            this.label105.TabIndex = 47;
            this.label105.Text = "Type";
            // 
            // chkMessHeader
            // 
            this.chkMessHeader.AutoSize = true;
            this.chkMessHeader.Location = new System.Drawing.Point(669, 429);
            this.chkMessHeader.Name = "chkMessHeader";
            this.chkMessHeader.Size = new System.Drawing.Size(104, 17);
            this.chkMessHeader.TabIndex = 46;
            this.chkMessHeader.Text = "Speaker Header";
            this.chkMessHeader.UseVisualStyleBackColor = true;
            // 
            // numMessType
            // 
            this.numMessType.Location = new System.Drawing.Point(730, 403);
            this.numMessType.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
            this.numMessType.Minimum = new decimal(new int[] {
            65000,
            0,
            0,
            -2147483648});
            this.numMessType.Name = "numMessType";
            this.numMessType.Size = new System.Drawing.Size(52, 20);
            this.numMessType.TabIndex = 45;
            // 
            // lblDelay
            // 
            this.lblDelay.Location = new System.Drawing.Point(691, 94);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(86, 13);
            this.lblDelay.TabIndex = 44;
            this.lblDelay.Text = "0:00";
            // 
            // cmdMoveMessDown
            // 
            this.cmdMoveMessDown.Location = new System.Drawing.Point(531, 3);
            this.cmdMoveMessDown.Name = "cmdMoveMessDown";
            this.cmdMoveMessDown.Size = new System.Drawing.Size(75, 23);
            this.cmdMoveMessDown.TabIndex = 43;
            this.cmdMoveMessDown.Text = "Move Down";
            this.cmdMoveMessDown.UseVisualStyleBackColor = true;
            this.cmdMoveMessDown.Click += new System.EventHandler(this.cmdMoveMessDown_Click);
            // 
            // cmdMoveMessUp
            // 
            this.cmdMoveMessUp.Location = new System.Drawing.Point(450, 3);
            this.cmdMoveMessUp.Name = "cmdMoveMessUp";
            this.cmdMoveMessUp.Size = new System.Drawing.Size(75, 23);
            this.cmdMoveMessUp.TabIndex = 42;
            this.cmdMoveMessUp.Text = "Move Up";
            this.cmdMoveMessUp.UseVisualStyleBackColor = true;
            this.cmdMoveMessUp.Click += new System.EventHandler(this.cmdMoveMessUp_Click);
            // 
            // cboMessPara
            // 
            this.cboMessPara.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessPara.Enabled = false;
            this.cboMessPara.FormattingEnabled = true;
            this.cboMessPara.Location = new System.Drawing.Point(564, 363);
            this.cboMessPara.Name = "cboMessPara";
            this.cboMessPara.Size = new System.Drawing.Size(106, 21);
            this.cboMessPara.TabIndex = 41;
            // 
            // cboMessFG
            // 
            this.cboMessFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessFG.Enabled = false;
            this.cboMessFG.FormattingEnabled = true;
            this.cboMessFG.Location = new System.Drawing.Point(487, 90);
            this.cboMessFG.Name = "cboMessFG";
            this.cboMessFG.Size = new System.Drawing.Size(99, 21);
            this.cboMessFG.TabIndex = 40;
            // 
            // txtVoice
            // 
            this.txtVoice.Enabled = false;
            this.txtVoice.Location = new System.Drawing.Point(382, 92);
            this.txtVoice.MaxLength = 7;
            this.txtVoice.Name = "txtVoice";
            this.txtVoice.Size = new System.Drawing.Size(73, 20);
            this.txtVoice.TabIndex = 39;
            this.txtVoice.Leave += new System.EventHandler(this.txtVoice_Leave);
            // 
            // grpMessCancel
            // 
            this.grpMessCancel.Controls.Add(this.optMessC1AND2);
            this.grpMessCancel.Controls.Add(this.optMessC1OR2);
            this.grpMessCancel.Controls.Add(this.lblMess5);
            this.grpMessCancel.Controls.Add(this.lblMess6);
            this.grpMessCancel.Enabled = false;
            this.grpMessCancel.Location = new System.Drawing.Point(349, 387);
            this.grpMessCancel.Name = "grpMessCancel";
            this.grpMessCancel.Size = new System.Drawing.Size(314, 118);
            this.grpMessCancel.TabIndex = 37;
            this.grpMessCancel.TabStop = false;
            this.grpMessCancel.Text = "Special (Cancel) Trigger";
            // 
            // optMessC1AND2
            // 
            this.optMessC1AND2.Location = new System.Drawing.Point(101, 48);
            this.optMessC1AND2.Name = "optMessC1AND2";
            this.optMessC1AND2.Size = new System.Drawing.Size(56, 24);
            this.optMessC1AND2.TabIndex = 23;
            this.optMessC1AND2.Text = "AND";
            // 
            // optMessC1OR2
            // 
            this.optMessC1OR2.Checked = true;
            this.optMessC1OR2.Location = new System.Drawing.Point(157, 48);
            this.optMessC1OR2.Name = "optMessC1OR2";
            this.optMessC1OR2.Size = new System.Drawing.Size(56, 24);
            this.optMessC1OR2.TabIndex = 24;
            this.optMessC1OR2.TabStop = true;
            this.optMessC1OR2.Text = "OR";
            // 
            // lblMess5
            // 
            this.lblMess5.BackColor = System.Drawing.Color.RosyBrown;
            this.lblMess5.Location = new System.Drawing.Point(6, 16);
            this.lblMess5.Name = "lblMess5";
            this.lblMess5.Size = new System.Drawing.Size(299, 32);
            this.lblMess5.TabIndex = 21;
            this.lblMess5.Text = "always (TRUE)";
            this.lblMess5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMess6
            // 
            this.lblMess6.BackColor = System.Drawing.Color.RosyBrown;
            this.lblMess6.Location = new System.Drawing.Point(6, 72);
            this.lblMess6.Name = "lblMess6";
            this.lblMess6.Size = new System.Drawing.Size(299, 32);
            this.lblMess6.TabIndex = 22;
            this.lblMess6.Text = "always (TRUE)";
            this.lblMess6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboMessColor
            // 
            this.cboMessColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessColor.Enabled = false;
            this.cboMessColor.Items.AddRange(new object[] {
            "Green",
            "Red",
            "Blue",
            "Yellow",
            "Red",
            "Purple"});
            this.cboMessColor.Location = new System.Drawing.Point(657, 8);
            this.cboMessColor.Name = "cboMessColor";
            this.cboMessColor.Size = new System.Drawing.Size(120, 21);
            this.cboMessColor.TabIndex = 36;
            // 
            // label109
            // 
            this.label109.Location = new System.Drawing.Point(486, 305);
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
            this.cboMessAmount.Location = new System.Drawing.Point(334, 305);
            this.cboMessAmount.Name = "cboMessAmount";
            this.cboMessAmount.Size = new System.Drawing.Size(144, 21);
            this.cboMessAmount.TabIndex = 30;
            // 
            // cboMessType
            // 
            this.cboMessType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessType.Enabled = false;
            this.cboMessType.Location = new System.Drawing.Point(510, 305);
            this.cboMessType.Name = "cboMessType";
            this.cboMessType.Size = new System.Drawing.Size(160, 21);
            this.cboMessType.TabIndex = 31;
            this.cboMessType.SelectedIndexChanged += new System.EventHandler(this.cboMessType_SelectedIndexChanged);
            // 
            // cboMessVar
            // 
            this.cboMessVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessVar.Enabled = false;
            this.cboMessVar.Location = new System.Drawing.Point(334, 337);
            this.cboMessVar.Name = "cboMessVar";
            this.cboMessVar.Size = new System.Drawing.Size(144, 21);
            this.cboMessVar.TabIndex = 32;
            // 
            // cboMessTrig
            // 
            this.cboMessTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessTrig.Enabled = false;
            this.cboMessTrig.Location = new System.Drawing.Point(510, 337);
            this.cboMessTrig.Name = "cboMessTrig";
            this.cboMessTrig.Size = new System.Drawing.Size(160, 21);
            this.cboMessTrig.TabIndex = 33;
            this.cboMessTrig.SelectedIndexChanged += new System.EventHandler(this.cboMessTrig_SelectedIndexChanged);
            // 
            // label110
            // 
            this.label110.Location = new System.Drawing.Point(478, 337);
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
            this.grpMessages.Location = new System.Drawing.Point(341, 112);
            this.grpMessages.Name = "grpMessages";
            this.grpMessages.Size = new System.Drawing.Size(322, 184);
            this.grpMessages.TabIndex = 27;
            this.grpMessages.TabStop = false;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.optMess3OR4);
            this.panel8.Controls.Add(this.optMess3AND4);
            this.panel8.Location = new System.Drawing.Point(246, 112);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(66, 64);
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
            this.panel7.Location = new System.Drawing.Point(246, 24);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(66, 64);
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
            this.lblMess1.Location = new System.Drawing.Point(12, 24);
            this.lblMess1.Name = "lblMess1";
            this.lblMess1.Size = new System.Drawing.Size(228, 32);
            this.lblMess1.TabIndex = 6;
            this.lblMess1.Text = "always (TRUE)";
            this.lblMess1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMess2
            // 
            this.lblMess2.BackColor = System.Drawing.Color.RosyBrown;
            this.lblMess2.Location = new System.Drawing.Point(12, 56);
            this.lblMess2.Name = "lblMess2";
            this.lblMess2.Size = new System.Drawing.Size(228, 32);
            this.lblMess2.TabIndex = 5;
            this.lblMess2.Text = "always (TRUE)";
            this.lblMess2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMess4
            // 
            this.lblMess4.BackColor = System.Drawing.Color.RosyBrown;
            this.lblMess4.Location = new System.Drawing.Point(12, 144);
            this.lblMess4.Name = "lblMess4";
            this.lblMess4.Size = new System.Drawing.Size(228, 32);
            this.lblMess4.TabIndex = 5;
            this.lblMess4.Text = "always (TRUE)";
            this.lblMess4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMess3
            // 
            this.lblMess3.BackColor = System.Drawing.Color.RosyBrown;
            this.lblMess3.Location = new System.Drawing.Point(12, 112);
            this.lblMess3.Name = "lblMess3";
            this.lblMess3.Size = new System.Drawing.Size(228, 32);
            this.lblMess3.TabIndex = 6;
            this.lblMess3.Text = "always (TRUE)";
            this.lblMess3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // optMess12AND34
            // 
            this.optMess12AND34.Location = new System.Drawing.Point(88, 88);
            this.optMess12AND34.Name = "optMess12AND34";
            this.optMess12AND34.Size = new System.Drawing.Size(48, 24);
            this.optMess12AND34.TabIndex = 3;
            this.optMess12AND34.Text = "AND";
            // 
            // optMess12OR34
            // 
            this.optMess12OR34.Checked = true;
            this.optMess12OR34.Location = new System.Drawing.Point(144, 88);
            this.optMess12OR34.Name = "optMess12OR34";
            this.optMess12OR34.Size = new System.Drawing.Size(48, 24);
            this.optMess12OR34.TabIndex = 4;
            this.optMess12OR34.TabStop = true;
            this.optMess12OR34.Text = "OR";
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
            this.grpSend.Location = new System.Drawing.Point(686, 112);
            this.grpSend.Name = "grpSend";
            this.grpSend.Size = new System.Drawing.Size(91, 253);
            this.grpSend.TabIndex = 29;
            this.grpSend.TabStop = false;
            this.grpSend.Text = "Send To...";
            // 
            // chkMess1
            // 
            this.chkMess1.Checked = true;
            this.chkMess1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMess1.Location = new System.Drawing.Point(12, 20);
            this.chkMess1.Name = "chkMess1";
            this.chkMess1.Size = new System.Drawing.Size(64, 16);
            this.chkMess1.TabIndex = 28;
            this.chkMess1.Text = "Team 1";
            // 
            // chkMess2
            // 
            this.chkMess2.Location = new System.Drawing.Point(12, 44);
            this.chkMess2.Name = "chkMess2";
            this.chkMess2.Size = new System.Drawing.Size(64, 16);
            this.chkMess2.TabIndex = 28;
            this.chkMess2.Text = "Team 2";
            // 
            // chkMess3
            // 
            this.chkMess3.Location = new System.Drawing.Point(12, 68);
            this.chkMess3.Name = "chkMess3";
            this.chkMess3.Size = new System.Drawing.Size(64, 16);
            this.chkMess3.TabIndex = 28;
            this.chkMess3.Text = "Team 3";
            // 
            // chkMess4
            // 
            this.chkMess4.Location = new System.Drawing.Point(12, 90);
            this.chkMess4.Name = "chkMess4";
            this.chkMess4.Size = new System.Drawing.Size(64, 16);
            this.chkMess4.TabIndex = 28;
            this.chkMess4.Text = "Team 4";
            // 
            // chkMess5
            // 
            this.chkMess5.Location = new System.Drawing.Point(12, 112);
            this.chkMess5.Name = "chkMess5";
            this.chkMess5.Size = new System.Drawing.Size(64, 16);
            this.chkMess5.TabIndex = 28;
            this.chkMess5.Text = "Team 5";
            // 
            // chkMess10
            // 
            this.chkMess10.Location = new System.Drawing.Point(12, 228);
            this.chkMess10.Name = "chkMess10";
            this.chkMess10.Size = new System.Drawing.Size(72, 16);
            this.chkMess10.TabIndex = 28;
            this.chkMess10.Text = "Team 10";
            // 
            // chkMess9
            // 
            this.chkMess9.Location = new System.Drawing.Point(12, 204);
            this.chkMess9.Name = "chkMess9";
            this.chkMess9.Size = new System.Drawing.Size(64, 16);
            this.chkMess9.TabIndex = 28;
            this.chkMess9.Text = "Team 9";
            // 
            // chkMess8
            // 
            this.chkMess8.Location = new System.Drawing.Point(12, 180);
            this.chkMess8.Name = "chkMess8";
            this.chkMess8.Size = new System.Drawing.Size(64, 16);
            this.chkMess8.TabIndex = 28;
            this.chkMess8.Text = "Team 8";
            // 
            // chkMess7
            // 
            this.chkMess7.Location = new System.Drawing.Point(12, 156);
            this.chkMess7.Name = "chkMess7";
            this.chkMess7.Size = new System.Drawing.Size(64, 16);
            this.chkMess7.TabIndex = 28;
            this.chkMess7.Text = "Team 7";
            // 
            // chkMess6
            // 
            this.chkMess6.Location = new System.Drawing.Point(12, 134);
            this.chkMess6.Name = "chkMess6";
            this.chkMess6.Size = new System.Drawing.Size(64, 16);
            this.chkMess6.TabIndex = 28;
            this.chkMess6.Text = "Team 6";
            // 
            // numMessDelay
            // 
            this.numMessDelay.Enabled = false;
            this.numMessDelay.Location = new System.Drawing.Point(638, 90);
            this.numMessDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numMessDelay.Name = "numMessDelay";
            this.numMessDelay.Size = new System.Drawing.Size(47, 20);
            this.numMessDelay.TabIndex = 25;
            this.numMessDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMessDelay.ValueChanged += new System.EventHandler(this.numMessDelay_ValueChanged);
            // 
            // label55
            // 
            this.label55.Location = new System.Drawing.Point(595, 94);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(37, 13);
            this.label55.TabIndex = 26;
            this.label55.Text = "Delay:";
            this.label55.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(344, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(120, 16);
            this.lblMessage.TabIndex = 24;
            this.lblMessage.Text = "Message #0 of 0";
            // 
            // txtMessNote
            // 
            this.txtMessNote.Enabled = false;
            this.txtMessNote.Location = new System.Drawing.Point(418, 61);
            this.txtMessNote.MaxLength = 63;
            this.txtMessNote.Name = "txtMessNote";
            this.txtMessNote.Size = new System.Drawing.Size(360, 20);
            this.txtMessNote.TabIndex = 20;
            this.txtMessNote.Leave += new System.EventHandler(this.txtMessNote_Leave);
            // 
            // txtMessage
            // 
            this.txtMessage.Enabled = false;
            this.txtMessage.Location = new System.Drawing.Point(418, 35);
            this.txtMessage.MaxLength = 68;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(360, 20);
            this.txtMessage.TabIndex = 20;
            this.txtMessage.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(335, 94);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(45, 13);
            this.label26.TabIndex = 23;
            this.label26.Text = "VoiceID";
            this.label26.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label149
            // 
            this.label149.AutoSize = true;
            this.label149.Location = new System.Drawing.Point(344, 64);
            this.label149.Name = "label149";
            this.label149.Size = new System.Drawing.Size(35, 13);
            this.label149.TabIndex = 23;
            this.label149.Text = "Notes";
            this.label149.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(344, 38);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(50, 13);
            this.label52.TabIndex = 23;
            this.label52.Text = "Message";
            this.label52.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lstMessages
            // 
            this.lstMessages.BackColor = System.Drawing.Color.Black;
            this.lstMessages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstMessages.ForeColor = System.Drawing.Color.Gray;
            this.lstMessages.ItemHeight = 15;
            this.lstMessages.Location = new System.Drawing.Point(8, 8);
            this.lstMessages.Name = "lstMessages";
            this.lstMessages.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstMessages.Size = new System.Drawing.Size(320, 484);
            this.lstMessages.TabIndex = 1;
            this.lstMessages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstMessages_DrawItem);
            this.lstMessages.SelectedIndexChanged += new System.EventHandler(this.lstMessages_SelectedIndexChanged);
            // 
            // tabGlob
            // 
            this.tabGlob.BackColor = System.Drawing.SystemColors.Control;
            this.tabGlob.Controls.Add(this.lblGlobDelay);
            this.tabGlob.Controls.Add(this.label113);
            this.tabGlob.Controls.Add(this.numGlobTrigPts);
            this.tabGlob.Controls.Add(this.label107);
            this.tabGlob.Controls.Add(this.cboGlobalPara);
            this.tabGlob.Controls.Add(this.label128);
            this.tabGlob.Controls.Add(this.numGlobActSeq);
            this.tabGlob.Controls.Add(this.numGlobDelay);
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
            this.tabGlob.Size = new System.Drawing.Size(785, 519);
            this.tabGlob.TabIndex = 2;
            this.tabGlob.Text = "Globals";
            // 
            // lblGlobDelay
            // 
            this.lblGlobDelay.Location = new System.Drawing.Point(625, 378);
            this.lblGlobDelay.Name = "lblGlobDelay";
            this.lblGlobDelay.Size = new System.Drawing.Size(86, 13);
            this.lblGlobDelay.TabIndex = 50;
            this.lblGlobDelay.Text = "0:00";
            // 
            // label113
            // 
            this.label113.AutoSize = true;
            this.label113.Location = new System.Drawing.Point(533, 378);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(34, 13);
            this.label113.TabIndex = 49;
            this.label113.Text = "Delay";
            // 
            // numGlobTrigPts
            // 
            this.numGlobTrigPts.Location = new System.Drawing.Point(704, 231);
            this.numGlobTrigPts.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numGlobTrigPts.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.numGlobTrigPts.Name = "numGlobTrigPts";
            this.numGlobTrigPts.Size = new System.Drawing.Size(48, 20);
            this.numGlobTrigPts.TabIndex = 48;
            this.numGlobTrigPts.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGlobTrigPts.Leave += new System.EventHandler(this.numGlobTrigPts_Leave);
            // 
            // label107
            // 
            this.label107.AutoSize = true;
            this.label107.Location = new System.Drawing.Point(631, 232);
            this.label107.Name = "label107";
            this.label107.Size = new System.Drawing.Size(72, 13);
            this.label107.TabIndex = 47;
            this.label107.Text = "Trigger Points";
            // 
            // cboGlobalPara
            // 
            this.cboGlobalPara.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGlobalPara.FormattingEnabled = true;
            this.cboGlobalPara.Location = new System.Drawing.Point(646, 203);
            this.cboGlobalPara.Name = "cboGlobalPara";
            this.cboGlobalPara.Size = new System.Drawing.Size(106, 21);
            this.cboGlobalPara.TabIndex = 46;
            this.cboGlobalPara.SelectedIndexChanged += new System.EventHandler(this.cboGlobalPara_SelectedIndexChanged);
            // 
            // label128
            // 
            this.label128.AutoSize = true;
            this.label128.Location = new System.Drawing.Point(415, 205);
            this.label128.Name = "label128";
            this.label128.Size = new System.Drawing.Size(89, 13);
            this.label128.TabIndex = 45;
            this.label128.Text = "Active Sequence";
            // 
            // numGlobActSeq
            // 
            this.numGlobActSeq.Location = new System.Drawing.Point(510, 203);
            this.numGlobActSeq.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGlobActSeq.Name = "numGlobActSeq";
            this.numGlobActSeq.Size = new System.Drawing.Size(46, 20);
            this.numGlobActSeq.TabIndex = 44;
            this.numGlobActSeq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ttActiveSequence.SetToolTip(this.numGlobActSeq, resources.GetString("numGlobActSeq.ToolTip"));
            this.numGlobActSeq.Leave += new System.EventHandler(this.numGlobActSeq_Leave);
            // 
            // numGlobDelay
            // 
            this.numGlobDelay.Location = new System.Drawing.Point(571, 376);
            this.numGlobDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGlobDelay.Name = "numGlobDelay";
            this.numGlobDelay.Size = new System.Drawing.Size(48, 20);
            this.numGlobDelay.TabIndex = 13;
            this.numGlobDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGlobDelay.ValueChanged += new System.EventHandler(this.numGlobDelay_ValueChanged);
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
            this.txtGlobalInc.Leave += new System.EventHandler(this.txtGlobalInc_Leave);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(421, 231);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(61, 13);
            this.label32.TabIndex = 31;
            this.label32.Text = "Goal Points";
            this.label32.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // numGlobalPoints
            // 
            this.numGlobalPoints.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numGlobalPoints.Location = new System.Drawing.Point(488, 229);
            this.numGlobalPoints.Maximum = new decimal(new int[] {
            3175,
            0,
            0,
            0});
            this.numGlobalPoints.Minimum = new decimal(new int[] {
            3200,
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
            this.cboGlobalAmount.SelectedIndexChanged += new System.EventHandler(this.cboGlobalAmount_SelectedIndexChanged);
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
            this.cboGlobalVar.SelectedIndexChanged += new System.EventHandler(this.cboGlobalVar_SelectedIndexChanged);
            // 
            // cboGlobalTrig
            // 
            this.cboGlobalTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGlobalTrig.Location = new System.Drawing.Point(592, 176);
            this.cboGlobalTrig.Name = "cboGlobalTrig";
            this.cboGlobalTrig.Size = new System.Drawing.Size(160, 21);
            this.cboGlobalTrig.TabIndex = 26;
            this.cboGlobalTrig.SelectedIndexChanged += new System.EventHandler(this.cboGlobalTrig_SelectedIndexChanged);
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
            this.groupBox18.Text = "Primary Goal";
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
            this.groupBox5.Text = "Prevent Goal";
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
            this.groupBox6.Text = "Secondary Goal";
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
            this.txtGlobalComp.Leave += new System.EventHandler(this.txtGlobalComp_Leave);
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
            this.txtGlobalFail.Leave += new System.EventHandler(this.txtGlobalFail_Leave);
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
            this.tabTeam.BackColor = System.Drawing.SystemColors.Control;
            this.tabTeam.Controls.Add(this.groupBox32);
            this.tabTeam.Controls.Add(this.txtTeamName);
            this.tabTeam.Controls.Add(this.label96);
            this.tabTeam.Controls.Add(this.groupBox30);
            this.tabTeam.Location = new System.Drawing.Point(4, 22);
            this.tabTeam.Name = "tabTeam";
            this.tabTeam.Size = new System.Drawing.Size(785, 519);
            this.tabTeam.TabIndex = 3;
            this.tabTeam.Text = "Teams";
            // 
            // groupBox32
            // 
            this.groupBox32.Controls.Add(this.grpTeamPMF);
            this.groupBox32.Controls.Add(this.grpTeamOMC);
            this.groupBox32.Controls.Add(this.grpTeamPMC);
            this.groupBox32.Location = new System.Drawing.Point(378, 37);
            this.groupBox32.Name = "groupBox32";
            this.groupBox32.Size = new System.Drawing.Size(400, 447);
            this.groupBox32.TabIndex = 18;
            this.groupBox32.TabStop = false;
            this.groupBox32.Text = "End of Mission Messages";
            // 
            // grpTeamPMF
            // 
            this.grpTeamPMF.Controls.Add(this.lblPMFDelay);
            this.grpTeamPMF.Controls.Add(this.cboPMFEomFG);
            this.grpTeamPMF.Controls.Add(this.label116);
            this.grpTeamPMF.Controls.Add(this.label53);
            this.grpTeamPMF.Controls.Add(this.label23);
            this.grpTeamPMF.Controls.Add(this.txtPMFVoiceID);
            this.grpTeamPMF.Controls.Add(this.txtPrimFail1);
            this.grpTeamPMF.Controls.Add(this.txtPrimFailNote);
            this.grpTeamPMF.Controls.Add(this.txtPrimFail2);
            this.grpTeamPMF.Controls.Add(this.numPMFEomDelay);
            this.grpTeamPMF.Location = new System.Drawing.Point(10, 300);
            this.grpTeamPMF.Name = "grpTeamPMF";
            this.grpTeamPMF.Size = new System.Drawing.Size(384, 132);
            this.grpTeamPMF.TabIndex = 2;
            this.grpTeamPMF.TabStop = false;
            this.grpTeamPMF.Text = "Primary Mission Failed";
            this.grpTeamPMF.Leave += new System.EventHandler(this.grpTeamPMF_Leave);
            // 
            // lblPMFDelay
            // 
            this.lblPMFDelay.Location = new System.Drawing.Point(106, 22);
            this.lblPMFDelay.Name = "lblPMFDelay";
            this.lblPMFDelay.Size = new System.Drawing.Size(86, 13);
            this.lblPMFDelay.TabIndex = 51;
            this.lblPMFDelay.Text = "0:00";
            // 
            // cboPMFEomFG
            // 
            this.cboPMFEomFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPMFEomFG.FormattingEnabled = true;
            this.cboPMFEomFG.Location = new System.Drawing.Point(257, 106);
            this.cboPMFEomFG.Name = "cboPMFEomFG";
            this.cboPMFEomFG.Size = new System.Drawing.Size(121, 21);
            this.cboPMFEomFG.TabIndex = 22;
            // 
            // label116
            // 
            this.label116.AutoSize = true;
            this.label116.Location = new System.Drawing.Point(13, 22);
            this.label116.Name = "label116";
            this.label116.Size = new System.Drawing.Size(37, 13);
            this.label116.TabIndex = 41;
            this.label116.Text = "Delay:";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(13, 109);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(30, 13);
            this.label53.TabIndex = 14;
            this.label53.Text = "Note";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(219, 22);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(48, 13);
            this.label23.TabIndex = 13;
            this.label23.Text = "Voice ID";
            // 
            // txtPMFVoiceID
            // 
            this.txtPMFVoiceID.Location = new System.Drawing.Point(273, 19);
            this.txtPMFVoiceID.MaxLength = 17;
            this.txtPMFVoiceID.Name = "txtPMFVoiceID";
            this.txtPMFVoiceID.Size = new System.Drawing.Size(95, 20);
            this.txtPMFVoiceID.TabIndex = 12;
            // 
            // txtPrimFail1
            // 
            this.txtPrimFail1.BackColor = System.Drawing.Color.Black;
            this.txtPrimFail1.ForeColor = System.Drawing.Color.Red;
            this.txtPrimFail1.Location = new System.Drawing.Point(16, 45);
            this.txtPrimFail1.MaxLength = 64;
            this.txtPrimFail1.Name = "txtPrimFail1";
            this.txtPrimFail1.Size = new System.Drawing.Size(352, 20);
            this.txtPrimFail1.TabIndex = 14;
            // 
            // txtPrimFailNote
            // 
            this.txtPrimFailNote.Location = new System.Drawing.Point(54, 106);
            this.txtPrimFailNote.MaxLength = 100;
            this.txtPrimFailNote.Name = "txtPrimFailNote";
            this.txtPrimFailNote.Size = new System.Drawing.Size(195, 20);
            this.txtPrimFailNote.TabIndex = 15;
            // 
            // txtPrimFail2
            // 
            this.txtPrimFail2.BackColor = System.Drawing.Color.Black;
            this.txtPrimFail2.ForeColor = System.Drawing.Color.Red;
            this.txtPrimFail2.Location = new System.Drawing.Point(16, 77);
            this.txtPrimFail2.MaxLength = 64;
            this.txtPrimFail2.Name = "txtPrimFail2";
            this.txtPrimFail2.Size = new System.Drawing.Size(352, 20);
            this.txtPrimFail2.TabIndex = 15;
            // 
            // numPMFEomDelay
            // 
            this.numPMFEomDelay.Location = new System.Drawing.Point(52, 20);
            this.numPMFEomDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numPMFEomDelay.Name = "numPMFEomDelay";
            this.numPMFEomDelay.Size = new System.Drawing.Size(48, 20);
            this.numPMFEomDelay.TabIndex = 15;
            this.numPMFEomDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPMFEomDelay.ValueChanged += new System.EventHandler(this.numPMFEomDelay_ValueChanged);
            // 
            // grpTeamOMC
            // 
            this.grpTeamOMC.Controls.Add(this.lblOMCDelay);
            this.grpTeamOMC.Controls.Add(this.label115);
            this.grpTeamOMC.Controls.Add(this.cboOMCEomFG);
            this.grpTeamOMC.Controls.Add(this.label30);
            this.grpTeamOMC.Controls.Add(this.label22);
            this.grpTeamOMC.Controls.Add(this.txtOMCVoiceID);
            this.grpTeamOMC.Controls.Add(this.txtOutstanding1);
            this.grpTeamOMC.Controls.Add(this.txtOutstandingNote);
            this.grpTeamOMC.Controls.Add(this.txtOutstanding2);
            this.grpTeamOMC.Controls.Add(this.numOMCEomDelay);
            this.grpTeamOMC.Location = new System.Drawing.Point(10, 162);
            this.grpTeamOMC.Name = "grpTeamOMC";
            this.grpTeamOMC.Size = new System.Drawing.Size(384, 132);
            this.grpTeamOMC.TabIndex = 1;
            this.grpTeamOMC.TabStop = false;
            this.grpTeamOMC.Text = "Outstanding Mission Complete";
            this.grpTeamOMC.Leave += new System.EventHandler(this.grpTeamOMC_Leave);
            // 
            // lblOMCDelay
            // 
            this.lblOMCDelay.Location = new System.Drawing.Point(106, 21);
            this.lblOMCDelay.Name = "lblOMCDelay";
            this.lblOMCDelay.Size = new System.Drawing.Size(86, 13);
            this.lblOMCDelay.TabIndex = 51;
            this.lblOMCDelay.Text = "0:00";
            // 
            // label115
            // 
            this.label115.AutoSize = true;
            this.label115.Location = new System.Drawing.Point(13, 19);
            this.label115.Name = "label115";
            this.label115.Size = new System.Drawing.Size(37, 13);
            this.label115.TabIndex = 41;
            this.label115.Text = "Delay:";
            // 
            // cboOMCEomFG
            // 
            this.cboOMCEomFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOMCEomFG.FormattingEnabled = true;
            this.cboOMCEomFG.Location = new System.Drawing.Point(257, 106);
            this.cboOMCEomFG.Name = "cboOMCEomFG";
            this.cboOMCEomFG.Size = new System.Drawing.Size(121, 21);
            this.cboOMCEomFG.TabIndex = 22;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(13, 109);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(30, 13);
            this.label30.TabIndex = 14;
            this.label30.Text = "Note";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(219, 22);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(48, 13);
            this.label22.TabIndex = 13;
            this.label22.Text = "Voice ID";
            // 
            // txtOMCVoiceID
            // 
            this.txtOMCVoiceID.Location = new System.Drawing.Point(273, 19);
            this.txtOMCVoiceID.MaxLength = 17;
            this.txtOMCVoiceID.Name = "txtOMCVoiceID";
            this.txtOMCVoiceID.Size = new System.Drawing.Size(95, 20);
            this.txtOMCVoiceID.TabIndex = 12;
            // 
            // txtOutstanding1
            // 
            this.txtOutstanding1.BackColor = System.Drawing.Color.Black;
            this.txtOutstanding1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtOutstanding1.Location = new System.Drawing.Point(16, 45);
            this.txtOutstanding1.MaxLength = 64;
            this.txtOutstanding1.Name = "txtOutstanding1";
            this.txtOutstanding1.Size = new System.Drawing.Size(352, 20);
            this.txtOutstanding1.TabIndex = 12;
            // 
            // txtOutstandingNote
            // 
            this.txtOutstandingNote.Location = new System.Drawing.Point(54, 106);
            this.txtOutstandingNote.MaxLength = 100;
            this.txtOutstandingNote.Name = "txtOutstandingNote";
            this.txtOutstandingNote.Size = new System.Drawing.Size(195, 20);
            this.txtOutstandingNote.TabIndex = 13;
            // 
            // txtOutstanding2
            // 
            this.txtOutstanding2.BackColor = System.Drawing.Color.Black;
            this.txtOutstanding2.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtOutstanding2.Location = new System.Drawing.Point(16, 77);
            this.txtOutstanding2.MaxLength = 64;
            this.txtOutstanding2.Name = "txtOutstanding2";
            this.txtOutstanding2.Size = new System.Drawing.Size(352, 20);
            this.txtOutstanding2.TabIndex = 13;
            // 
            // numOMCEomDelay
            // 
            this.numOMCEomDelay.Location = new System.Drawing.Point(52, 19);
            this.numOMCEomDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numOMCEomDelay.Name = "numOMCEomDelay";
            this.numOMCEomDelay.Size = new System.Drawing.Size(48, 20);
            this.numOMCEomDelay.TabIndex = 13;
            this.numOMCEomDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numOMCEomDelay.ValueChanged += new System.EventHandler(this.numOMCEomDelay_ValueChanged);
            // 
            // grpTeamPMC
            // 
            this.grpTeamPMC.Controls.Add(this.lblPMCDelay);
            this.grpTeamPMC.Controls.Add(this.label114);
            this.grpTeamPMC.Controls.Add(this.label27);
            this.grpTeamPMC.Controls.Add(this.cboPMCEomFG);
            this.grpTeamPMC.Controls.Add(this.label6);
            this.grpTeamPMC.Controls.Add(this.txtPMCVoiceID);
            this.grpTeamPMC.Controls.Add(this.txtPrimComp1);
            this.grpTeamPMC.Controls.Add(this.txtPrimCompNote);
            this.grpTeamPMC.Controls.Add(this.txtPrimComp2);
            this.grpTeamPMC.Controls.Add(this.numPMCEomDelay);
            this.grpTeamPMC.Location = new System.Drawing.Point(8, 24);
            this.grpTeamPMC.Name = "grpTeamPMC";
            this.grpTeamPMC.Size = new System.Drawing.Size(384, 132);
            this.grpTeamPMC.TabIndex = 0;
            this.grpTeamPMC.TabStop = false;
            this.grpTeamPMC.Text = "Primary Mission Complete";
            this.grpTeamPMC.Leave += new System.EventHandler(this.grpTeamPMC_Leave);
            // 
            // lblPMCDelay
            // 
            this.lblPMCDelay.Location = new System.Drawing.Point(112, 22);
            this.lblPMCDelay.Name = "lblPMCDelay";
            this.lblPMCDelay.Size = new System.Drawing.Size(86, 13);
            this.lblPMCDelay.TabIndex = 51;
            this.lblPMCDelay.Text = "0:00";
            // 
            // label114
            // 
            this.label114.AutoSize = true;
            this.label114.Location = new System.Drawing.Point(15, 21);
            this.label114.Name = "label114";
            this.label114.Size = new System.Drawing.Size(37, 13);
            this.label114.TabIndex = 41;
            this.label114.Text = "Delay:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(18, 107);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(30, 13);
            this.label27.TabIndex = 14;
            this.label27.Text = "Note";
            // 
            // cboPMCEomFG
            // 
            this.cboPMCEomFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPMCEomFG.FormattingEnabled = true;
            this.cboPMCEomFG.Location = new System.Drawing.Point(257, 106);
            this.cboPMCEomFG.Name = "cboPMCEomFG";
            this.cboPMCEomFG.Size = new System.Drawing.Size(121, 21);
            this.cboPMCEomFG.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(221, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Voice ID";
            // 
            // txtPMCVoiceID
            // 
            this.txtPMCVoiceID.Location = new System.Drawing.Point(275, 19);
            this.txtPMCVoiceID.MaxLength = 17;
            this.txtPMCVoiceID.Name = "txtPMCVoiceID";
            this.txtPMCVoiceID.Size = new System.Drawing.Size(93, 20);
            this.txtPMCVoiceID.TabIndex = 12;
            // 
            // txtPrimComp1
            // 
            this.txtPrimComp1.BackColor = System.Drawing.Color.Black;
            this.txtPrimComp1.ForeColor = System.Drawing.Color.Lime;
            this.txtPrimComp1.Location = new System.Drawing.Point(16, 45);
            this.txtPrimComp1.MaxLength = 64;
            this.txtPrimComp1.Name = "txtPrimComp1";
            this.txtPrimComp1.Size = new System.Drawing.Size(352, 20);
            this.txtPrimComp1.TabIndex = 10;
            // 
            // txtPrimCompNote
            // 
            this.txtPrimCompNote.Location = new System.Drawing.Point(54, 106);
            this.txtPrimCompNote.MaxLength = 100;
            this.txtPrimCompNote.Name = "txtPrimCompNote";
            this.txtPrimCompNote.Size = new System.Drawing.Size(197, 20);
            this.txtPrimCompNote.TabIndex = 11;
            // 
            // txtPrimComp2
            // 
            this.txtPrimComp2.BackColor = System.Drawing.Color.Black;
            this.txtPrimComp2.ForeColor = System.Drawing.Color.Lime;
            this.txtPrimComp2.Location = new System.Drawing.Point(16, 77);
            this.txtPrimComp2.MaxLength = 64;
            this.txtPrimComp2.Name = "txtPrimComp2";
            this.txtPrimComp2.Size = new System.Drawing.Size(352, 20);
            this.txtPrimComp2.TabIndex = 11;
            // 
            // numPMCEomDelay
            // 
            this.numPMCEomDelay.Location = new System.Drawing.Point(58, 20);
            this.numPMCEomDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numPMCEomDelay.Name = "numPMCEomDelay";
            this.numPMCEomDelay.Size = new System.Drawing.Size(48, 20);
            this.numPMCEomDelay.TabIndex = 11;
            this.numPMCEomDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPMCEomDelay.ValueChanged += new System.EventHandler(this.numPMCEomDelay_ValueChanged);
            // 
            // txtTeamName
            // 
            this.txtTeamName.Location = new System.Drawing.Point(68, 37);
            this.txtTeamName.MaxLength = 15;
            this.txtTeamName.Name = "txtTeamName";
            this.txtTeamName.Size = new System.Drawing.Size(88, 20);
            this.txtTeamName.TabIndex = 5;
            this.txtTeamName.Text = "Imperials";
            this.txtTeamName.Leave += new System.EventHandler(this.txtTeamName_Leave);
            // 
            // label96
            // 
            this.label96.Location = new System.Drawing.Point(20, 37);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(40, 16);
            this.label96.TabIndex = 4;
            this.label96.Text = "Name:";
            this.label96.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // groupBox30
            // 
            this.groupBox30.Controls.Add(this.panel12);
            this.groupBox30.Controls.Add(this.panel20);
            this.groupBox30.Controls.Add(this.panel19);
            this.groupBox30.Controls.Add(this.panel18);
            this.groupBox30.Controls.Add(this.panel17);
            this.groupBox30.Controls.Add(this.panel16);
            this.groupBox30.Controls.Add(this.panel15);
            this.groupBox30.Controls.Add(this.panel14);
            this.groupBox30.Controls.Add(this.panel13);
            this.groupBox30.Controls.Add(this.panel11);
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
            this.groupBox30.Location = new System.Drawing.Point(23, 63);
            this.groupBox30.Name = "groupBox30";
            this.groupBox30.Size = new System.Drawing.Size(349, 216);
            this.groupBox30.TabIndex = 1;
            this.groupBox30.TabStop = false;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.optAllies3);
            this.panel12.Controls.Add(this.optAllies1);
            this.panel12.Controls.Add(this.optAllies2);
            this.panel12.Location = new System.Drawing.Point(148, 16);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(195, 19);
            this.panel12.TabIndex = 19;
            // 
            // optAllies3
            // 
            this.optAllies3.AutoSize = true;
            this.optAllies3.Location = new System.Drawing.Point(133, 2);
            this.optAllies3.Name = "optAllies3";
            this.optAllies3.Size = new System.Drawing.Size(59, 17);
            this.optAllies3.TabIndex = 19;
            this.optAllies3.Text = "Neutral";
            this.optAllies3.UseVisualStyleBackColor = true;
            // 
            // optAllies1
            // 
            this.optAllies1.AutoSize = true;
            this.optAllies1.Location = new System.Drawing.Point(3, 2);
            this.optAllies1.Name = "optAllies1";
            this.optAllies1.Size = new System.Drawing.Size(57, 17);
            this.optAllies1.TabIndex = 19;
            this.optAllies1.Text = "Enemy";
            this.optAllies1.UseVisualStyleBackColor = true;
            // 
            // optAllies2
            // 
            this.optAllies2.AutoSize = true;
            this.optAllies2.Checked = true;
            this.optAllies2.Location = new System.Drawing.Point(66, 2);
            this.optAllies2.Name = "optAllies2";
            this.optAllies2.Size = new System.Drawing.Size(61, 17);
            this.optAllies2.TabIndex = 19;
            this.optAllies2.TabStop = true;
            this.optAllies2.Text = "Friendly";
            this.optAllies2.UseVisualStyleBackColor = true;
            // 
            // panel20
            // 
            this.panel20.Controls.Add(this.optAllies30);
            this.panel20.Controls.Add(this.optAllies28);
            this.panel20.Controls.Add(this.optAllies29);
            this.panel20.Location = new System.Drawing.Point(148, 187);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(195, 19);
            this.panel20.TabIndex = 19;
            // 
            // optAllies30
            // 
            this.optAllies30.AutoSize = true;
            this.optAllies30.Location = new System.Drawing.Point(133, 2);
            this.optAllies30.Name = "optAllies30";
            this.optAllies30.Size = new System.Drawing.Size(59, 17);
            this.optAllies30.TabIndex = 19;
            this.optAllies30.Text = "Neutral";
            this.optAllies30.UseVisualStyleBackColor = true;
            // 
            // optAllies28
            // 
            this.optAllies28.AutoSize = true;
            this.optAllies28.Checked = true;
            this.optAllies28.Location = new System.Drawing.Point(3, 2);
            this.optAllies28.Name = "optAllies28";
            this.optAllies28.Size = new System.Drawing.Size(57, 17);
            this.optAllies28.TabIndex = 19;
            this.optAllies28.TabStop = true;
            this.optAllies28.Text = "Enemy";
            this.optAllies28.UseVisualStyleBackColor = true;
            // 
            // optAllies29
            // 
            this.optAllies29.AutoSize = true;
            this.optAllies29.Location = new System.Drawing.Point(66, 2);
            this.optAllies29.Name = "optAllies29";
            this.optAllies29.Size = new System.Drawing.Size(61, 17);
            this.optAllies29.TabIndex = 19;
            this.optAllies29.Text = "Friendly";
            this.optAllies29.UseVisualStyleBackColor = true;
            // 
            // panel19
            // 
            this.panel19.Controls.Add(this.optAllies27);
            this.panel19.Controls.Add(this.optAllies25);
            this.panel19.Controls.Add(this.optAllies26);
            this.panel19.Location = new System.Drawing.Point(148, 168);
            this.panel19.Name = "panel19";
            this.panel19.Size = new System.Drawing.Size(195, 19);
            this.panel19.TabIndex = 19;
            // 
            // optAllies27
            // 
            this.optAllies27.AutoSize = true;
            this.optAllies27.Location = new System.Drawing.Point(133, 2);
            this.optAllies27.Name = "optAllies27";
            this.optAllies27.Size = new System.Drawing.Size(59, 17);
            this.optAllies27.TabIndex = 19;
            this.optAllies27.Text = "Neutral";
            this.optAllies27.UseVisualStyleBackColor = true;
            // 
            // optAllies25
            // 
            this.optAllies25.AutoSize = true;
            this.optAllies25.Checked = true;
            this.optAllies25.Location = new System.Drawing.Point(3, 2);
            this.optAllies25.Name = "optAllies25";
            this.optAllies25.Size = new System.Drawing.Size(57, 17);
            this.optAllies25.TabIndex = 19;
            this.optAllies25.TabStop = true;
            this.optAllies25.Text = "Enemy";
            this.optAllies25.UseVisualStyleBackColor = true;
            // 
            // optAllies26
            // 
            this.optAllies26.AutoSize = true;
            this.optAllies26.Location = new System.Drawing.Point(66, 2);
            this.optAllies26.Name = "optAllies26";
            this.optAllies26.Size = new System.Drawing.Size(61, 17);
            this.optAllies26.TabIndex = 19;
            this.optAllies26.Text = "Friendly";
            this.optAllies26.UseVisualStyleBackColor = true;
            // 
            // panel18
            // 
            this.panel18.Controls.Add(this.optAllies24);
            this.panel18.Controls.Add(this.optAllies22);
            this.panel18.Controls.Add(this.optAllies23);
            this.panel18.Location = new System.Drawing.Point(148, 149);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(195, 19);
            this.panel18.TabIndex = 19;
            // 
            // optAllies24
            // 
            this.optAllies24.AutoSize = true;
            this.optAllies24.Location = new System.Drawing.Point(133, 2);
            this.optAllies24.Name = "optAllies24";
            this.optAllies24.Size = new System.Drawing.Size(59, 17);
            this.optAllies24.TabIndex = 19;
            this.optAllies24.Text = "Neutral";
            this.optAllies24.UseVisualStyleBackColor = true;
            // 
            // optAllies22
            // 
            this.optAllies22.AutoSize = true;
            this.optAllies22.Checked = true;
            this.optAllies22.Location = new System.Drawing.Point(3, 2);
            this.optAllies22.Name = "optAllies22";
            this.optAllies22.Size = new System.Drawing.Size(57, 17);
            this.optAllies22.TabIndex = 19;
            this.optAllies22.TabStop = true;
            this.optAllies22.Text = "Enemy";
            this.optAllies22.UseVisualStyleBackColor = true;
            // 
            // optAllies23
            // 
            this.optAllies23.AutoSize = true;
            this.optAllies23.Location = new System.Drawing.Point(66, 2);
            this.optAllies23.Name = "optAllies23";
            this.optAllies23.Size = new System.Drawing.Size(61, 17);
            this.optAllies23.TabIndex = 19;
            this.optAllies23.Text = "Friendly";
            this.optAllies23.UseVisualStyleBackColor = true;
            // 
            // panel17
            // 
            this.panel17.Controls.Add(this.optAllies21);
            this.panel17.Controls.Add(this.optAllies19);
            this.panel17.Controls.Add(this.optAllies20);
            this.panel17.Location = new System.Drawing.Point(148, 130);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(195, 19);
            this.panel17.TabIndex = 19;
            // 
            // optAllies21
            // 
            this.optAllies21.AutoSize = true;
            this.optAllies21.Location = new System.Drawing.Point(133, 2);
            this.optAllies21.Name = "optAllies21";
            this.optAllies21.Size = new System.Drawing.Size(59, 17);
            this.optAllies21.TabIndex = 19;
            this.optAllies21.Text = "Neutral";
            this.optAllies21.UseVisualStyleBackColor = true;
            // 
            // optAllies19
            // 
            this.optAllies19.AutoSize = true;
            this.optAllies19.Checked = true;
            this.optAllies19.Location = new System.Drawing.Point(3, 2);
            this.optAllies19.Name = "optAllies19";
            this.optAllies19.Size = new System.Drawing.Size(57, 17);
            this.optAllies19.TabIndex = 19;
            this.optAllies19.TabStop = true;
            this.optAllies19.Text = "Enemy";
            this.optAllies19.UseVisualStyleBackColor = true;
            // 
            // optAllies20
            // 
            this.optAllies20.AutoSize = true;
            this.optAllies20.Location = new System.Drawing.Point(66, 2);
            this.optAllies20.Name = "optAllies20";
            this.optAllies20.Size = new System.Drawing.Size(61, 17);
            this.optAllies20.TabIndex = 19;
            this.optAllies20.Text = "Friendly";
            this.optAllies20.UseVisualStyleBackColor = true;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.optAllies18);
            this.panel16.Controls.Add(this.optAllies16);
            this.panel16.Controls.Add(this.optAllies17);
            this.panel16.Location = new System.Drawing.Point(148, 111);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(195, 19);
            this.panel16.TabIndex = 19;
            // 
            // optAllies18
            // 
            this.optAllies18.AutoSize = true;
            this.optAllies18.Location = new System.Drawing.Point(133, 2);
            this.optAllies18.Name = "optAllies18";
            this.optAllies18.Size = new System.Drawing.Size(59, 17);
            this.optAllies18.TabIndex = 19;
            this.optAllies18.Text = "Neutral";
            this.optAllies18.UseVisualStyleBackColor = true;
            // 
            // optAllies16
            // 
            this.optAllies16.AutoSize = true;
            this.optAllies16.Checked = true;
            this.optAllies16.Location = new System.Drawing.Point(3, 2);
            this.optAllies16.Name = "optAllies16";
            this.optAllies16.Size = new System.Drawing.Size(57, 17);
            this.optAllies16.TabIndex = 19;
            this.optAllies16.TabStop = true;
            this.optAllies16.Text = "Enemy";
            this.optAllies16.UseVisualStyleBackColor = true;
            // 
            // optAllies17
            // 
            this.optAllies17.AutoSize = true;
            this.optAllies17.Location = new System.Drawing.Point(66, 2);
            this.optAllies17.Name = "optAllies17";
            this.optAllies17.Size = new System.Drawing.Size(61, 17);
            this.optAllies17.TabIndex = 19;
            this.optAllies17.Text = "Friendly";
            this.optAllies17.UseVisualStyleBackColor = true;
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.optAllies15);
            this.panel15.Controls.Add(this.optAllies13);
            this.panel15.Controls.Add(this.optAllies14);
            this.panel15.Location = new System.Drawing.Point(148, 92);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(195, 19);
            this.panel15.TabIndex = 19;
            // 
            // optAllies15
            // 
            this.optAllies15.AutoSize = true;
            this.optAllies15.Location = new System.Drawing.Point(133, 2);
            this.optAllies15.Name = "optAllies15";
            this.optAllies15.Size = new System.Drawing.Size(59, 17);
            this.optAllies15.TabIndex = 19;
            this.optAllies15.Text = "Neutral";
            this.optAllies15.UseVisualStyleBackColor = true;
            // 
            // optAllies13
            // 
            this.optAllies13.AutoSize = true;
            this.optAllies13.Checked = true;
            this.optAllies13.Location = new System.Drawing.Point(3, 2);
            this.optAllies13.Name = "optAllies13";
            this.optAllies13.Size = new System.Drawing.Size(57, 17);
            this.optAllies13.TabIndex = 19;
            this.optAllies13.TabStop = true;
            this.optAllies13.Text = "Enemy";
            this.optAllies13.UseVisualStyleBackColor = true;
            // 
            // optAllies14
            // 
            this.optAllies14.AutoSize = true;
            this.optAllies14.Location = new System.Drawing.Point(66, 2);
            this.optAllies14.Name = "optAllies14";
            this.optAllies14.Size = new System.Drawing.Size(61, 17);
            this.optAllies14.TabIndex = 19;
            this.optAllies14.Text = "Friendly";
            this.optAllies14.UseVisualStyleBackColor = true;
            // 
            // panel14
            // 
            this.panel14.Controls.Add(this.optAllies12);
            this.panel14.Controls.Add(this.optAllies10);
            this.panel14.Controls.Add(this.optAllies11);
            this.panel14.Location = new System.Drawing.Point(148, 73);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(195, 19);
            this.panel14.TabIndex = 19;
            // 
            // optAllies12
            // 
            this.optAllies12.AutoSize = true;
            this.optAllies12.Location = new System.Drawing.Point(133, 2);
            this.optAllies12.Name = "optAllies12";
            this.optAllies12.Size = new System.Drawing.Size(59, 17);
            this.optAllies12.TabIndex = 19;
            this.optAllies12.Text = "Neutral";
            this.optAllies12.UseVisualStyleBackColor = true;
            // 
            // optAllies10
            // 
            this.optAllies10.AutoSize = true;
            this.optAllies10.Checked = true;
            this.optAllies10.Location = new System.Drawing.Point(3, 2);
            this.optAllies10.Name = "optAllies10";
            this.optAllies10.Size = new System.Drawing.Size(57, 17);
            this.optAllies10.TabIndex = 19;
            this.optAllies10.TabStop = true;
            this.optAllies10.Text = "Enemy";
            this.optAllies10.UseVisualStyleBackColor = true;
            // 
            // optAllies11
            // 
            this.optAllies11.AutoSize = true;
            this.optAllies11.Location = new System.Drawing.Point(66, 2);
            this.optAllies11.Name = "optAllies11";
            this.optAllies11.Size = new System.Drawing.Size(61, 17);
            this.optAllies11.TabIndex = 19;
            this.optAllies11.Text = "Friendly";
            this.optAllies11.UseVisualStyleBackColor = true;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.optAllies9);
            this.panel13.Controls.Add(this.optAllies7);
            this.panel13.Controls.Add(this.optAllies8);
            this.panel13.Location = new System.Drawing.Point(148, 54);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(195, 19);
            this.panel13.TabIndex = 19;
            // 
            // optAllies9
            // 
            this.optAllies9.AutoSize = true;
            this.optAllies9.Location = new System.Drawing.Point(133, 2);
            this.optAllies9.Name = "optAllies9";
            this.optAllies9.Size = new System.Drawing.Size(59, 17);
            this.optAllies9.TabIndex = 19;
            this.optAllies9.Text = "Neutral";
            this.optAllies9.UseVisualStyleBackColor = true;
            // 
            // optAllies7
            // 
            this.optAllies7.AutoSize = true;
            this.optAllies7.Checked = true;
            this.optAllies7.Location = new System.Drawing.Point(3, 2);
            this.optAllies7.Name = "optAllies7";
            this.optAllies7.Size = new System.Drawing.Size(57, 17);
            this.optAllies7.TabIndex = 19;
            this.optAllies7.TabStop = true;
            this.optAllies7.Text = "Enemy";
            this.optAllies7.UseVisualStyleBackColor = true;
            // 
            // optAllies8
            // 
            this.optAllies8.AutoSize = true;
            this.optAllies8.Location = new System.Drawing.Point(66, 2);
            this.optAllies8.Name = "optAllies8";
            this.optAllies8.Size = new System.Drawing.Size(61, 17);
            this.optAllies8.TabIndex = 19;
            this.optAllies8.Text = "Friendly";
            this.optAllies8.UseVisualStyleBackColor = true;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.optAllies6);
            this.panel11.Controls.Add(this.optAllies4);
            this.panel11.Controls.Add(this.optAllies5);
            this.panel11.Location = new System.Drawing.Point(148, 35);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(195, 19);
            this.panel11.TabIndex = 19;
            // 
            // optAllies6
            // 
            this.optAllies6.AutoSize = true;
            this.optAllies6.Location = new System.Drawing.Point(133, 2);
            this.optAllies6.Name = "optAllies6";
            this.optAllies6.Size = new System.Drawing.Size(59, 17);
            this.optAllies6.TabIndex = 19;
            this.optAllies6.Text = "Neutral";
            this.optAllies6.UseVisualStyleBackColor = true;
            // 
            // optAllies4
            // 
            this.optAllies4.AutoSize = true;
            this.optAllies4.Checked = true;
            this.optAllies4.Location = new System.Drawing.Point(3, 2);
            this.optAllies4.Name = "optAllies4";
            this.optAllies4.Size = new System.Drawing.Size(57, 17);
            this.optAllies4.TabIndex = 19;
            this.optAllies4.TabStop = true;
            this.optAllies4.Text = "Enemy";
            this.optAllies4.UseVisualStyleBackColor = true;
            // 
            // optAllies5
            // 
            this.optAllies5.AutoSize = true;
            this.optAllies5.Location = new System.Drawing.Point(66, 2);
            this.optAllies5.Name = "optAllies5";
            this.optAllies5.Size = new System.Drawing.Size(61, 17);
            this.optAllies5.TabIndex = 19;
            this.optAllies5.Text = "Friendly";
            this.optAllies5.UseVisualStyleBackColor = true;
            // 
            // lblTeam1
            // 
            this.lblTeam1.BackColor = System.Drawing.Color.RosyBrown;
            this.lblTeam1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblTeam1.Location = new System.Drawing.Point(6, 16);
            this.lblTeam1.Name = "lblTeam1";
            this.lblTeam1.Size = new System.Drawing.Size(138, 19);
            this.lblTeam1.TabIndex = 0;
            this.lblTeam1.Text = "Team 1: Imperials";
            this.lblTeam1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTeam2
            // 
            this.lblTeam2.BackColor = System.Drawing.Color.RosyBrown;
            this.lblTeam2.Location = new System.Drawing.Point(6, 35);
            this.lblTeam2.Name = "lblTeam2";
            this.lblTeam2.Size = new System.Drawing.Size(138, 19);
            this.lblTeam2.TabIndex = 0;
            this.lblTeam2.Text = "Team 2: Rebels";
            this.lblTeam2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTeam3
            // 
            this.lblTeam3.BackColor = System.Drawing.Color.RosyBrown;
            this.lblTeam3.Location = new System.Drawing.Point(6, 54);
            this.lblTeam3.Name = "lblTeam3";
            this.lblTeam3.Size = new System.Drawing.Size(138, 19);
            this.lblTeam3.TabIndex = 0;
            this.lblTeam3.Text = "Team 3: Team 3";
            this.lblTeam3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTeam4
            // 
            this.lblTeam4.BackColor = System.Drawing.Color.RosyBrown;
            this.lblTeam4.Location = new System.Drawing.Point(6, 73);
            this.lblTeam4.Name = "lblTeam4";
            this.lblTeam4.Size = new System.Drawing.Size(138, 19);
            this.lblTeam4.TabIndex = 0;
            this.lblTeam4.Text = "Team 4: Team 4";
            this.lblTeam4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTeam5
            // 
            this.lblTeam5.BackColor = System.Drawing.Color.RosyBrown;
            this.lblTeam5.Location = new System.Drawing.Point(6, 92);
            this.lblTeam5.Name = "lblTeam5";
            this.lblTeam5.Size = new System.Drawing.Size(138, 19);
            this.lblTeam5.TabIndex = 0;
            this.lblTeam5.Text = "Team 5: Team 5";
            this.lblTeam5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTeam6
            // 
            this.lblTeam6.BackColor = System.Drawing.Color.RosyBrown;
            this.lblTeam6.Location = new System.Drawing.Point(6, 111);
            this.lblTeam6.Name = "lblTeam6";
            this.lblTeam6.Size = new System.Drawing.Size(138, 19);
            this.lblTeam6.TabIndex = 0;
            this.lblTeam6.Text = "Team 6: Team 6";
            this.lblTeam6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTeam7
            // 
            this.lblTeam7.BackColor = System.Drawing.Color.RosyBrown;
            this.lblTeam7.Location = new System.Drawing.Point(6, 130);
            this.lblTeam7.Name = "lblTeam7";
            this.lblTeam7.Size = new System.Drawing.Size(138, 19);
            this.lblTeam7.TabIndex = 0;
            this.lblTeam7.Text = "Team 7: Team 7";
            this.lblTeam7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTeam8
            // 
            this.lblTeam8.BackColor = System.Drawing.Color.RosyBrown;
            this.lblTeam8.Location = new System.Drawing.Point(6, 149);
            this.lblTeam8.Name = "lblTeam8";
            this.lblTeam8.Size = new System.Drawing.Size(138, 19);
            this.lblTeam8.TabIndex = 0;
            this.lblTeam8.Text = "Team 8: Team 8";
            this.lblTeam8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTeam9
            // 
            this.lblTeam9.BackColor = System.Drawing.Color.RosyBrown;
            this.lblTeam9.Location = new System.Drawing.Point(6, 168);
            this.lblTeam9.Name = "lblTeam9";
            this.lblTeam9.Size = new System.Drawing.Size(138, 19);
            this.lblTeam9.TabIndex = 0;
            this.lblTeam9.Text = "Team 9: Team 9";
            this.lblTeam9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTeam10
            // 
            this.lblTeam10.BackColor = System.Drawing.Color.RosyBrown;
            this.lblTeam10.Location = new System.Drawing.Point(6, 187);
            this.lblTeam10.Name = "lblTeam10";
            this.lblTeam10.Size = new System.Drawing.Size(138, 19);
            this.lblTeam10.TabIndex = 0;
            this.lblTeam10.Text = "Team 10: Team 10";
            this.lblTeam10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabMission
            // 
            this.tabMission.BackColor = System.Drawing.SystemColors.Control;
            this.tabMission.Controls.Add(this.chkGoalsUnimportant);
            this.tabMission.Controls.Add(this.label123);
            this.tabMission.Controls.Add(this.txtFailNote);
            this.tabMission.Controls.Add(this.label82);
            this.tabMission.Controls.Add(this.txtSuccNote);
            this.tabMission.Controls.Add(this.label75);
            this.tabMission.Controls.Add(this.txtDescNote);
            this.tabMission.Controls.Add(this.pctLogo);
            this.tabMission.Controls.Add(this.cboLogo);
            this.tabMission.Controls.Add(this.cboFailOfficer);
            this.tabMission.Controls.Add(this.cboWinOfficer);
            this.tabMission.Controls.Add(this.cboOfficer);
            this.tabMission.Controls.Add(this.label91);
            this.tabMission.Controls.Add(this.label90);
            this.tabMission.Controls.Add(this.label130);
            this.tabMission.Controls.Add(this.label129);
            this.tabMission.Controls.Add(this.chkEnd);
            this.tabMission.Controls.Add(this.label102);
            this.tabMission.Controls.Add(this.numMissTimeMin);
            this.tabMission.Controls.Add(this.label100);
            this.tabMission.Controls.Add(this.cboHangar);
            this.tabMission.Controls.Add(this.label97);
            this.tabMission.Controls.Add(this.txtMissDesc);
            this.tabMission.Controls.Add(this.txtMissSucc);
            this.tabMission.Controls.Add(this.txtMissFail);
            this.tabMission.Controls.Add(this.label98);
            this.tabMission.Controls.Add(this.label99);
            this.tabMission.Location = new System.Drawing.Point(4, 22);
            this.tabMission.Name = "tabMission";
            this.tabMission.Size = new System.Drawing.Size(785, 519);
            this.tabMission.TabIndex = 4;
            this.tabMission.Text = "Mission";
            // 
            // chkGoalsUnimportant
            // 
            this.chkGoalsUnimportant.AutoSize = true;
            this.chkGoalsUnimportant.Location = new System.Drawing.Point(34, 488);
            this.chkGoalsUnimportant.Name = "chkGoalsUnimportant";
            this.chkGoalsUnimportant.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkGoalsUnimportant.Size = new System.Drawing.Size(111, 17);
            this.chkGoalsUnimportant.TabIndex = 16;
            this.chkGoalsUnimportant.Text = "Goals unimportant";
            this.chkGoalsUnimportant.UseVisualStyleBackColor = true;
            this.chkGoalsUnimportant.Leave += new System.EventHandler(this.chkGoalsUnimportant_Leave);
            // 
            // label123
            // 
            this.label123.AutoSize = true;
            this.label123.Location = new System.Drawing.Point(521, 369);
            this.label123.Name = "label123";
            this.label123.Size = new System.Drawing.Size(30, 13);
            this.label123.TabIndex = 15;
            this.label123.Text = "Note";
            // 
            // txtFailNote
            // 
            this.txtFailNote.Location = new System.Drawing.Point(557, 366);
            this.txtFailNote.MaxLength = 100;
            this.txtFailNote.Name = "txtFailNote";
            this.txtFailNote.Size = new System.Drawing.Size(203, 20);
            this.txtFailNote.TabIndex = 5;
            this.txtFailNote.Leave += new System.EventHandler(this.txtFailNote_Leave);
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Location = new System.Drawing.Point(267, 369);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(30, 13);
            this.label82.TabIndex = 15;
            this.label82.Text = "Note";
            // 
            // txtSuccNote
            // 
            this.txtSuccNote.Location = new System.Drawing.Point(303, 366);
            this.txtSuccNote.MaxLength = 100;
            this.txtSuccNote.Name = "txtSuccNote";
            this.txtSuccNote.Size = new System.Drawing.Size(203, 20);
            this.txtSuccNote.TabIndex = 4;
            this.txtSuccNote.Leave += new System.EventHandler(this.txtSuccNote_Leave);
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Location = new System.Drawing.Point(13, 369);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(30, 13);
            this.label75.TabIndex = 15;
            this.label75.Text = "Note";
            // 
            // txtDescNote
            // 
            this.txtDescNote.Location = new System.Drawing.Point(49, 366);
            this.txtDescNote.MaxLength = 100;
            this.txtDescNote.Name = "txtDescNote";
            this.txtDescNote.Size = new System.Drawing.Size(203, 20);
            this.txtDescNote.TabIndex = 3;
            this.txtDescNote.Leave += new System.EventHandler(this.txtDescNote_Leave);
            // 
            // pctLogo
            // 
            this.pctLogo.Location = new System.Drawing.Point(440, 414);
            this.pctLogo.Name = "pctLogo";
            this.pctLogo.Size = new System.Drawing.Size(115, 73);
            this.pctLogo.TabIndex = 13;
            this.pctLogo.TabStop = false;
            // 
            // cboLogo
            // 
            this.cboLogo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLogo.FormattingEnabled = true;
            this.cboLogo.Location = new System.Drawing.Point(313, 415);
            this.cboLogo.Name = "cboLogo";
            this.cboLogo.Size = new System.Drawing.Size(121, 21);
            this.cboLogo.TabIndex = 10;
            this.cboLogo.SelectedIndexChanged += new System.EventHandler(this.cboLogo_SelectedIndexChanged);
            // 
            // cboFailOfficer
            // 
            this.cboFailOfficer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFailOfficer.FormattingEnabled = true;
            this.cboFailOfficer.Location = new System.Drawing.Point(313, 468);
            this.cboFailOfficer.Name = "cboFailOfficer";
            this.cboFailOfficer.Size = new System.Drawing.Size(121, 21);
            this.cboFailOfficer.TabIndex = 9;
            this.cboFailOfficer.Leave += new System.EventHandler(this.cboFailOfficer_Leave);
            // 
            // cboWinOfficer
            // 
            this.cboWinOfficer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWinOfficer.FormattingEnabled = true;
            this.cboWinOfficer.Location = new System.Drawing.Point(179, 468);
            this.cboWinOfficer.Name = "cboWinOfficer";
            this.cboWinOfficer.Size = new System.Drawing.Size(121, 21);
            this.cboWinOfficer.TabIndex = 9;
            this.cboWinOfficer.Leave += new System.EventHandler(this.cboWinOfficer_Leave);
            // 
            // cboOfficer
            // 
            this.cboOfficer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOfficer.FormattingEnabled = true;
            this.cboOfficer.Location = new System.Drawing.Point(179, 416);
            this.cboOfficer.Name = "cboOfficer";
            this.cboOfficer.Size = new System.Drawing.Size(121, 21);
            this.cboOfficer.TabIndex = 9;
            this.cboOfficer.Leave += new System.EventHandler(this.cboOfficer_Leave);
            // 
            // label91
            // 
            this.label91.AutoSize = true;
            this.label91.Location = new System.Drawing.Point(310, 451);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(94, 13);
            this.label91.TabIndex = 11;
            this.label91.Text = "Fail Debrief Officer";
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.Location = new System.Drawing.Point(176, 451);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(97, 13);
            this.label90.TabIndex = 11;
            this.label90.Text = "Win Debrief Officer";
            // 
            // label130
            // 
            this.label130.AutoSize = true;
            this.label130.Location = new System.Drawing.Point(310, 399);
            this.label130.Name = "label130";
            this.label130.Size = new System.Drawing.Size(82, 13);
            this.label130.TabIndex = 11;
            this.label130.Text = "Briefing Emblem";
            // 
            // label129
            // 
            this.label129.AutoSize = true;
            this.label129.Location = new System.Drawing.Point(176, 399);
            this.label129.Name = "label129";
            this.label129.Size = new System.Drawing.Size(76, 13);
            this.label129.TabIndex = 11;
            this.label129.Text = "Briefing Officer";
            // 
            // chkEnd
            // 
            this.chkEnd.AutoSize = true;
            this.chkEnd.Location = new System.Drawing.Point(24, 470);
            this.chkEnd.Name = "chkEnd";
            this.chkEnd.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkEnd.Size = new System.Drawing.Size(121, 17);
            this.chkEnd.TabIndex = 8;
            this.chkEnd.Text = "End when Complete";
            this.chkEnd.UseVisualStyleBackColor = true;
            this.chkEnd.Leave += new System.EventHandler(this.chkEnd_Leave);
            // 
            // label102
            // 
            this.label102.AutoSize = true;
            this.label102.Location = new System.Drawing.Point(16, 446);
            this.label102.Name = "label102";
            this.label102.Size = new System.Drawing.Size(77, 13);
            this.label102.TabIndex = 6;
            this.label102.Text = "Time Limit: Min";
            // 
            // numMissTimeMin
            // 
            this.numMissTimeMin.Location = new System.Drawing.Point(97, 444);
            this.numMissTimeMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numMissTimeMin.Name = "numMissTimeMin";
            this.numMissTimeMin.Size = new System.Drawing.Size(48, 20);
            this.numMissTimeMin.TabIndex = 7;
            this.numMissTimeMin.Leave += new System.EventHandler(this.numMissTimeMin_Leave);
            // 
            // label100
            // 
            this.label100.AutoSize = true;
            this.label100.Location = new System.Drawing.Point(24, 399);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(69, 13);
            this.label100.TabIndex = 3;
            this.label100.Text = "Mission Type";
            // 
            // cboHangar
            // 
            this.cboHangar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHangar.Location = new System.Drawing.Point(24, 416);
            this.cboHangar.Name = "cboHangar";
            this.cboHangar.Size = new System.Drawing.Size(121, 21);
            this.cboHangar.TabIndex = 6;
            this.cboHangar.Leave += new System.EventHandler(this.cboHangar_Leave);
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
            this.txtMissDesc.MaxLength = 4096;
            this.txtMissDesc.Multiline = true;
            this.txtMissDesc.Name = "txtMissDesc";
            this.txtMissDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMissDesc.Size = new System.Drawing.Size(236, 328);
            this.txtMissDesc.TabIndex = 0;
            this.txtMissDesc.Text = "#";
            this.txtMissDesc.Leave += new System.EventHandler(this.txtMissDesc_Leave);
            // 
            // txtMissSucc
            // 
            this.txtMissSucc.BackColor = System.Drawing.Color.Black;
            this.txtMissSucc.ForeColor = System.Drawing.Color.Lime;
            this.txtMissSucc.Location = new System.Drawing.Point(270, 32);
            this.txtMissSucc.MaxLength = 4096;
            this.txtMissSucc.Multiline = true;
            this.txtMissSucc.Name = "txtMissSucc";
            this.txtMissSucc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMissSucc.Size = new System.Drawing.Size(236, 328);
            this.txtMissSucc.TabIndex = 1;
            this.txtMissSucc.Leave += new System.EventHandler(this.txtMissSucc_Leave);
            // 
            // txtMissFail
            // 
            this.txtMissFail.BackColor = System.Drawing.Color.Black;
            this.txtMissFail.ForeColor = System.Drawing.Color.Red;
            this.txtMissFail.Location = new System.Drawing.Point(524, 32);
            this.txtMissFail.MaxLength = 4096;
            this.txtMissFail.Multiline = true;
            this.txtMissFail.Name = "txtMissFail";
            this.txtMissFail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMissFail.Size = new System.Drawing.Size(236, 328);
            this.txtMissFail.TabIndex = 2;
            this.txtMissFail.Text = "#";
            this.txtMissFail.Leave += new System.EventHandler(this.txtMissFail_Leave);
            // 
            // label98
            // 
            this.label98.Location = new System.Drawing.Point(270, 16);
            this.label98.Name = "label98";
            this.label98.Size = new System.Drawing.Size(176, 16);
            this.label98.TabIndex = 1;
            this.label98.Text = "Mission Successful Debrief";
            // 
            // label99
            // 
            this.label99.Location = new System.Drawing.Point(524, 16);
            this.label99.Name = "label99";
            this.label99.Size = new System.Drawing.Size(152, 16);
            this.label99.TabIndex = 1;
            this.label99.Text = "Mission Failed Debrief";
            // 
            // tabMission2
            // 
            this.tabMission2.BackColor = System.Drawing.SystemColors.Control;
            this.tabMission2.Controls.Add(this.groupBox40);
            this.tabMission2.Controls.Add(this.grpGlobalUnits);
            this.tabMission2.Controls.Add(this.grpGlobalGroups);
            this.tabMission2.Controls.Add(this.grpRegions);
            this.tabMission2.Controls.Add(this.groupBox37);
            this.tabMission2.Controls.Add(this.label101);
            this.tabMission2.Controls.Add(this.txtNotes);
            this.tabMission2.Location = new System.Drawing.Point(4, 22);
            this.tabMission2.Name = "tabMission2";
            this.tabMission2.Padding = new System.Windows.Forms.Padding(3);
            this.tabMission2.Size = new System.Drawing.Size(785, 519);
            this.tabMission2.TabIndex = 5;
            this.tabMission2.Text = "Mission2";
            // 
            // groupBox40
            // 
            this.groupBox40.Controls.Add(this.label28);
            this.groupBox40.Controls.Add(this.cboGCVolatility);
            this.groupBox40.Controls.Add(this.label21);
            this.groupBox40.Controls.Add(this.cboGCType);
            this.groupBox40.Controls.Add(this.numGCCount);
            this.groupBox40.Controls.Add(this.label29);
            this.groupBox40.Controls.Add(this.numGCValue);
            this.groupBox40.Controls.Add(this.label141);
            this.groupBox40.Controls.Add(this.numGCVolume);
            this.groupBox40.Controls.Add(this.label140);
            this.groupBox40.Controls.Add(this.txtGlobCargo);
            this.groupBox40.Controls.Add(this.numGCIndex);
            this.groupBox40.Controls.Add(this.label138);
            this.groupBox40.Location = new System.Drawing.Point(8, 274);
            this.groupBox40.Name = "groupBox40";
            this.groupBox40.Size = new System.Drawing.Size(461, 104);
            this.groupBox40.TabIndex = 5;
            this.groupBox40.TabStop = false;
            this.groupBox40.Text = "Global Cargo";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(142, 52);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(45, 13);
            this.label28.TabIndex = 13;
            this.label28.Text = "Volatility";
            // 
            // cboGCVolatility
            // 
            this.cboGCVolatility.FormattingEnabled = true;
            this.cboGCVolatility.Items.AddRange(new object[] {
            "Low",
            "Medium",
            "High",
            "Kaboom!"});
            this.cboGCVolatility.Location = new System.Drawing.Point(193, 49);
            this.cboGCVolatility.Name = "cboGCVolatility";
            this.cboGCVolatility.Size = new System.Drawing.Size(83, 21);
            this.cboGCVolatility.TabIndex = 12;
            this.cboGCVolatility.Leave += new System.EventHandler(this.cboGCVolatility_Leave);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 52);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(31, 13);
            this.label21.TabIndex = 11;
            this.label21.Text = "Type";
            // 
            // cboGCType
            // 
            this.cboGCType.FormattingEnabled = true;
            this.cboGCType.Items.AddRange(new object[] {
            "Solid",
            "Liquid",
            "Gas"});
            this.cboGCType.Location = new System.Drawing.Point(57, 49);
            this.cboGCType.Name = "cboGCType";
            this.cboGCType.Size = new System.Drawing.Size(79, 21);
            this.cboGCType.TabIndex = 10;
            this.cboGCType.Leave += new System.EventHandler(this.cboGCType_Leave);
            // 
            // numGCCount
            // 
            this.numGCCount.Location = new System.Drawing.Point(318, 76);
            this.numGCCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numGCCount.Name = "numGCCount";
            this.numGCCount.Size = new System.Drawing.Size(71, 20);
            this.numGCCount.TabIndex = 9;
            this.numGCCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGCCount.Leave += new System.EventHandler(this.numGCCount_Leave);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(278, 78);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(35, 13);
            this.label29.TabIndex = 8;
            this.label29.Text = "Count";
            this.label29.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // numGCValue
            // 
            this.numGCValue.Location = new System.Drawing.Point(182, 76);
            this.numGCValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGCValue.Name = "numGCValue";
            this.numGCValue.Size = new System.Drawing.Size(48, 20);
            this.numGCValue.TabIndex = 9;
            this.numGCValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGCValue.Leave += new System.EventHandler(this.numGCValue_Leave);
            // 
            // label141
            // 
            this.label141.AutoSize = true;
            this.label141.Location = new System.Drawing.Point(142, 78);
            this.label141.Name = "label141";
            this.label141.Size = new System.Drawing.Size(34, 13);
            this.label141.TabIndex = 8;
            this.label141.Text = "Value";
            this.label141.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // numGCVolume
            // 
            this.numGCVolume.Location = new System.Drawing.Point(57, 76);
            this.numGCVolume.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGCVolume.Name = "numGCVolume";
            this.numGCVolume.Size = new System.Drawing.Size(48, 20);
            this.numGCVolume.TabIndex = 7;
            this.numGCVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGCVolume.Leave += new System.EventHandler(this.numGCVolume_Leave);
            // 
            // label140
            // 
            this.label140.AutoSize = true;
            this.label140.Location = new System.Drawing.Point(9, 78);
            this.label140.Name = "label140";
            this.label140.Size = new System.Drawing.Size(42, 13);
            this.label140.TabIndex = 6;
            this.label140.Text = "Volume";
            this.label140.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // txtGlobCargo
            // 
            this.txtGlobCargo.Location = new System.Drawing.Point(104, 23);
            this.txtGlobCargo.MaxLength = 63;
            this.txtGlobCargo.Name = "txtGlobCargo";
            this.txtGlobCargo.Size = new System.Drawing.Size(351, 20);
            this.txtGlobCargo.TabIndex = 2;
            this.txtGlobCargo.Leave += new System.EventHandler(this.txtGlobCargo_Leave);
            // 
            // numGCIndex
            // 
            this.numGCIndex.Location = new System.Drawing.Point(57, 24);
            this.numGCIndex.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numGCIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGCIndex.Name = "numGCIndex";
            this.numGCIndex.Size = new System.Drawing.Size(41, 20);
            this.numGCIndex.TabIndex = 1;
            this.numGCIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGCIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGCIndex.ValueChanged += new System.EventHandler(this.numGCIndex_ValueChanged);
            // 
            // label138
            // 
            this.label138.AutoSize = true;
            this.label138.Location = new System.Drawing.Point(6, 26);
            this.label138.Name = "label138";
            this.label138.Size = new System.Drawing.Size(45, 13);
            this.label138.TabIndex = 0;
            this.label138.Text = "Cargo #";
            // 
            // grpGlobalUnits
            // 
            this.grpGlobalUnits.Controls.Add(this.chkGURandom);
            this.grpGlobalUnits.Controls.Add(this.numGUSpecial);
            this.grpGlobalUnits.Controls.Add(this.label86);
            this.grpGlobalUnits.Controls.Add(this.txtGUCargo);
            this.grpGlobalUnits.Controls.Add(this.label87);
            this.grpGlobalUnits.Controls.Add(this.cboGULeader);
            this.grpGlobalUnits.Controls.Add(this.label88);
            this.grpGlobalUnits.Controls.Add(this.numGUIndex);
            this.grpGlobalUnits.Controls.Add(this.label89);
            this.grpGlobalUnits.Controls.Add(this.txtGUName);
            this.grpGlobalUnits.Location = new System.Drawing.Point(205, 142);
            this.grpGlobalUnits.Name = "grpGlobalUnits";
            this.grpGlobalUnits.Size = new System.Drawing.Size(303, 126);
            this.grpGlobalUnits.TabIndex = 4;
            this.grpGlobalUnits.TabStop = false;
            this.grpGlobalUnits.Text = "Global Units";
            // 
            // chkGURandom
            // 
            this.chkGURandom.AutoSize = true;
            this.chkGURandom.Location = new System.Drawing.Point(140, 100);
            this.chkGURandom.Name = "chkGURandom";
            this.chkGURandom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkGURandom.Size = new System.Drawing.Size(66, 17);
            this.chkGURandom.TabIndex = 12;
            this.chkGURandom.Text = "Random";
            this.chkGURandom.CheckedChanged += new System.EventHandler(this.chkGURandom_CheckedChanged);
            // 
            // numGUSpecial
            // 
            this.numGUSpecial.Location = new System.Drawing.Point(92, 99);
            this.numGUSpecial.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGUSpecial.Name = "numGUSpecial";
            this.numGUSpecial.Size = new System.Drawing.Size(42, 20);
            this.numGUSpecial.TabIndex = 11;
            this.numGUSpecial.Leave += new System.EventHandler(this.numGUSpecial_Leave);
            // 
            // label86
            // 
            this.label86.Location = new System.Drawing.Point(6, 99);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(80, 16);
            this.label86.TabIndex = 10;
            this.label86.Text = "Special Ship #";
            this.label86.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtGUCargo
            // 
            this.txtGUCargo.Location = new System.Drawing.Point(86, 73);
            this.txtGUCargo.MaxLength = 19;
            this.txtGUCargo.Name = "txtGUCargo";
            this.txtGUCargo.Size = new System.Drawing.Size(128, 20);
            this.txtGUCargo.TabIndex = 9;
            this.txtGUCargo.Leave += new System.EventHandler(this.txtGUCargo_Leave);
            // 
            // label87
            // 
            this.label87.Location = new System.Drawing.Point(6, 75);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(80, 16);
            this.label87.TabIndex = 8;
            this.label87.Text = "Special Cargo";
            // 
            // cboGULeader
            // 
            this.cboGULeader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGULeader.FormattingEnabled = true;
            this.cboGULeader.Location = new System.Drawing.Point(56, 46);
            this.cboGULeader.Name = "cboGULeader";
            this.cboGULeader.Size = new System.Drawing.Size(121, 21);
            this.cboGULeader.TabIndex = 5;
            this.cboGULeader.Leave += new System.EventHandler(this.cboGULeader_Leave);
            // 
            // label88
            // 
            this.label88.AutoSize = true;
            this.label88.Location = new System.Drawing.Point(6, 49);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(40, 13);
            this.label88.TabIndex = 4;
            this.label88.Text = "Leader";
            // 
            // numGUIndex
            // 
            this.numGUIndex.Location = new System.Drawing.Point(56, 20);
            this.numGUIndex.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numGUIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGUIndex.Name = "numGUIndex";
            this.numGUIndex.Size = new System.Drawing.Size(41, 20);
            this.numGUIndex.TabIndex = 3;
            this.numGUIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGUIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGUIndex.ValueChanged += new System.EventHandler(this.numGUIndex_ValueChanged);
            // 
            // label89
            // 
            this.label89.AutoSize = true;
            this.label89.Location = new System.Drawing.Point(5, 22);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(36, 13);
            this.label89.TabIndex = 2;
            this.label89.Text = "Unit #";
            // 
            // txtGUName
            // 
            this.txtGUName.Location = new System.Drawing.Point(103, 19);
            this.txtGUName.Name = "txtGUName";
            this.txtGUName.Size = new System.Drawing.Size(194, 20);
            this.txtGUName.TabIndex = 0;
            this.txtGUName.Leave += new System.EventHandler(this.txtGUName_Leave);
            // 
            // grpGlobalGroups
            // 
            this.grpGlobalGroups.Controls.Add(this.chkGGRandom);
            this.grpGlobalGroups.Controls.Add(this.numGGSpecial);
            this.grpGlobalGroups.Controls.Add(this.label85);
            this.grpGlobalGroups.Controls.Add(this.txtGGCargo);
            this.grpGlobalGroups.Controls.Add(this.label84);
            this.grpGlobalGroups.Controls.Add(this.cboGGLeader);
            this.grpGlobalGroups.Controls.Add(this.label83);
            this.grpGlobalGroups.Controls.Add(this.numGGIndex);
            this.grpGlobalGroups.Controls.Add(this.label81);
            this.grpGlobalGroups.Controls.Add(this.txtGGName);
            this.grpGlobalGroups.Location = new System.Drawing.Point(204, 10);
            this.grpGlobalGroups.Name = "grpGlobalGroups";
            this.grpGlobalGroups.Size = new System.Drawing.Size(303, 126);
            this.grpGlobalGroups.TabIndex = 4;
            this.grpGlobalGroups.TabStop = false;
            this.grpGlobalGroups.Text = "Global Groups";
            // 
            // chkGGRandom
            // 
            this.chkGGRandom.AutoSize = true;
            this.chkGGRandom.Location = new System.Drawing.Point(140, 100);
            this.chkGGRandom.Name = "chkGGRandom";
            this.chkGGRandom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkGGRandom.Size = new System.Drawing.Size(66, 17);
            this.chkGGRandom.TabIndex = 12;
            this.chkGGRandom.Text = "Random";
            this.chkGGRandom.CheckedChanged += new System.EventHandler(this.chkGGRandom_CheckedChanged);
            // 
            // numGGSpecial
            // 
            this.numGGSpecial.Location = new System.Drawing.Point(92, 99);
            this.numGGSpecial.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGGSpecial.Name = "numGGSpecial";
            this.numGGSpecial.Size = new System.Drawing.Size(42, 20);
            this.numGGSpecial.TabIndex = 11;
            this.numGGSpecial.Leave += new System.EventHandler(this.numGGSpecial_Leave);
            // 
            // label85
            // 
            this.label85.Location = new System.Drawing.Point(6, 99);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(80, 16);
            this.label85.TabIndex = 10;
            this.label85.Text = "Special Ship #";
            this.label85.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtGGCargo
            // 
            this.txtGGCargo.Location = new System.Drawing.Point(86, 73);
            this.txtGGCargo.MaxLength = 19;
            this.txtGGCargo.Name = "txtGGCargo";
            this.txtGGCargo.Size = new System.Drawing.Size(128, 20);
            this.txtGGCargo.TabIndex = 9;
            this.txtGGCargo.Leave += new System.EventHandler(this.txtGGCargo_Leave);
            // 
            // label84
            // 
            this.label84.Location = new System.Drawing.Point(6, 75);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(80, 16);
            this.label84.TabIndex = 8;
            this.label84.Text = "Special Cargo";
            // 
            // cboGGLeader
            // 
            this.cboGGLeader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGGLeader.FormattingEnabled = true;
            this.cboGGLeader.Location = new System.Drawing.Point(56, 46);
            this.cboGGLeader.Name = "cboGGLeader";
            this.cboGGLeader.Size = new System.Drawing.Size(121, 21);
            this.cboGGLeader.TabIndex = 5;
            this.cboGGLeader.Leave += new System.EventHandler(this.cboGGLeader_Leave);
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(6, 49);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(40, 13);
            this.label83.TabIndex = 4;
            this.label83.Text = "Leader";
            // 
            // numGGIndex
            // 
            this.numGGIndex.Location = new System.Drawing.Point(56, 20);
            this.numGGIndex.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numGGIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGGIndex.Name = "numGGIndex";
            this.numGGIndex.Size = new System.Drawing.Size(41, 20);
            this.numGGIndex.TabIndex = 3;
            this.numGGIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numGGIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGGIndex.ValueChanged += new System.EventHandler(this.numGGIndex_ValueChanged);
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.Location = new System.Drawing.Point(5, 22);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(46, 13);
            this.label81.TabIndex = 2;
            this.label81.Text = "Group #";
            // 
            // txtGGName
            // 
            this.txtGGName.Location = new System.Drawing.Point(103, 19);
            this.txtGGName.Name = "txtGGName";
            this.txtGGName.Size = new System.Drawing.Size(194, 20);
            this.txtGGName.TabIndex = 0;
            this.txtGGName.Leave += new System.EventHandler(this.txtGGName_Leave);
            // 
            // grpRegions
            // 
            this.grpRegions.Controls.Add(this.label137);
            this.grpRegions.Controls.Add(this.label136);
            this.grpRegions.Controls.Add(this.label135);
            this.grpRegions.Controls.Add(this.label134);
            this.grpRegions.Controls.Add(this.txtRegion4);
            this.grpRegions.Controls.Add(this.txtRegion3);
            this.grpRegions.Controls.Add(this.txtRegion2);
            this.grpRegions.Controls.Add(this.txtRegion1);
            this.grpRegions.Location = new System.Drawing.Point(8, 142);
            this.grpRegions.Name = "grpRegions";
            this.grpRegions.Size = new System.Drawing.Size(191, 126);
            this.grpRegions.TabIndex = 3;
            this.grpRegions.TabStop = false;
            this.grpRegions.Text = "Regions";
            this.grpRegions.Leave += new System.EventHandler(this.grpRegions_Leave);
            // 
            // label137
            // 
            this.label137.AutoSize = true;
            this.label137.Location = new System.Drawing.Point(6, 96);
            this.label137.Name = "label137";
            this.label137.Size = new System.Drawing.Size(39, 13);
            this.label137.TabIndex = 5;
            this.label137.Text = "Reg. 4";
            // 
            // label136
            // 
            this.label136.AutoSize = true;
            this.label136.Location = new System.Drawing.Point(6, 71);
            this.label136.Name = "label136";
            this.label136.Size = new System.Drawing.Size(39, 13);
            this.label136.TabIndex = 5;
            this.label136.Text = "Reg. 3";
            // 
            // label135
            // 
            this.label135.AutoSize = true;
            this.label135.Location = new System.Drawing.Point(6, 45);
            this.label135.Name = "label135";
            this.label135.Size = new System.Drawing.Size(39, 13);
            this.label135.TabIndex = 5;
            this.label135.Text = "Reg. 2";
            // 
            // label134
            // 
            this.label134.AutoSize = true;
            this.label134.Location = new System.Drawing.Point(6, 19);
            this.label134.Name = "label134";
            this.label134.Size = new System.Drawing.Size(39, 13);
            this.label134.TabIndex = 5;
            this.label134.Text = "Reg. 1";
            // 
            // txtRegion4
            // 
            this.txtRegion4.Location = new System.Drawing.Point(51, 93);
            this.txtRegion4.MaxLength = 131;
            this.txtRegion4.Name = "txtRegion4";
            this.txtRegion4.Size = new System.Drawing.Size(133, 20);
            this.txtRegion4.TabIndex = 7;
            // 
            // txtRegion3
            // 
            this.txtRegion3.Location = new System.Drawing.Point(51, 67);
            this.txtRegion3.MaxLength = 131;
            this.txtRegion3.Name = "txtRegion3";
            this.txtRegion3.Size = new System.Drawing.Size(133, 20);
            this.txtRegion3.TabIndex = 6;
            // 
            // txtRegion2
            // 
            this.txtRegion2.Location = new System.Drawing.Point(50, 41);
            this.txtRegion2.MaxLength = 131;
            this.txtRegion2.Name = "txtRegion2";
            this.txtRegion2.Size = new System.Drawing.Size(133, 20);
            this.txtRegion2.TabIndex = 5;
            // 
            // txtRegion1
            // 
            this.txtRegion1.Location = new System.Drawing.Point(50, 16);
            this.txtRegion1.MaxLength = 131;
            this.txtRegion1.Name = "txtRegion1";
            this.txtRegion1.Size = new System.Drawing.Size(133, 20);
            this.txtRegion1.TabIndex = 4;
            // 
            // groupBox37
            // 
            this.groupBox37.Controls.Add(this.label133);
            this.groupBox37.Controls.Add(this.label132);
            this.groupBox37.Controls.Add(this.label131);
            this.groupBox37.Controls.Add(this.label108);
            this.groupBox37.Controls.Add(this.txtIFF6);
            this.groupBox37.Controls.Add(this.txtIFF5);
            this.groupBox37.Controls.Add(this.txtIFF4);
            this.groupBox37.Controls.Add(this.txtIFF3);
            this.groupBox37.Location = new System.Drawing.Point(6, 10);
            this.groupBox37.Name = "groupBox37";
            this.groupBox37.Size = new System.Drawing.Size(192, 126);
            this.groupBox37.TabIndex = 2;
            this.groupBox37.TabStop = false;
            this.groupBox37.Text = "IFFs";
            // 
            // label133
            // 
            this.label133.AutoSize = true;
            this.label133.Location = new System.Drawing.Point(6, 100);
            this.label133.Name = "label133";
            this.label133.Size = new System.Drawing.Size(37, 13);
            this.label133.TabIndex = 1;
            this.label133.Text = "Purple";
            // 
            // label132
            // 
            this.label132.AutoSize = true;
            this.label132.Location = new System.Drawing.Point(6, 74);
            this.label132.Name = "label132";
            this.label132.Size = new System.Drawing.Size(27, 13);
            this.label132.TabIndex = 1;
            this.label132.Text = "Red";
            // 
            // label131
            // 
            this.label131.AutoSize = true;
            this.label131.Location = new System.Drawing.Point(6, 48);
            this.label131.Name = "label131";
            this.label131.Size = new System.Drawing.Size(38, 13);
            this.label131.TabIndex = 1;
            this.label131.Text = "Yellow";
            // 
            // label108
            // 
            this.label108.AutoSize = true;
            this.label108.Location = new System.Drawing.Point(6, 22);
            this.label108.Name = "label108";
            this.label108.Size = new System.Drawing.Size(28, 13);
            this.label108.TabIndex = 1;
            this.label108.Text = "Blue";
            // 
            // txtIFF6
            // 
            this.txtIFF6.Location = new System.Drawing.Point(50, 97);
            this.txtIFF6.MaxLength = 19;
            this.txtIFF6.Name = "txtIFF6";
            this.txtIFF6.Size = new System.Drawing.Size(133, 20);
            this.txtIFF6.TabIndex = 3;
            // 
            // txtIFF5
            // 
            this.txtIFF5.Location = new System.Drawing.Point(50, 71);
            this.txtIFF5.MaxLength = 19;
            this.txtIFF5.Name = "txtIFF5";
            this.txtIFF5.Size = new System.Drawing.Size(133, 20);
            this.txtIFF5.TabIndex = 2;
            // 
            // txtIFF4
            // 
            this.txtIFF4.Location = new System.Drawing.Point(50, 45);
            this.txtIFF4.MaxLength = 19;
            this.txtIFF4.Name = "txtIFF4";
            this.txtIFF4.Size = new System.Drawing.Size(133, 20);
            this.txtIFF4.TabIndex = 1;
            // 
            // txtIFF3
            // 
            this.txtIFF3.Location = new System.Drawing.Point(50, 16);
            this.txtIFF3.MaxLength = 19;
            this.txtIFF3.Name = "txtIFF3";
            this.txtIFF3.Size = new System.Drawing.Size(133, 20);
            this.txtIFF3.TabIndex = 0;
            // 
            // label101
            // 
            this.label101.AutoSize = true;
            this.label101.Location = new System.Drawing.Point(526, 10);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(92, 13);
            this.label101.TabIndex = 1;
            this.label101.Text = "Notes (editor only)";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(529, 27);
            this.txtNotes.MaxLength = 19086;
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(246, 446);
            this.txtNotes.TabIndex = 0;
            this.txtNotes.Leave += new System.EventHandler(this.txtNotes_Leave);
            // 
            // dataOrders
            // 
            this.dataOrders.AllowDelete = false;
            this.dataOrders.AllowNew = false;
            // 
            // dataOrders_Raw
            // 
            this.dataOrders_Raw.AllowDelete = false;
            this.dataOrders_Raw.AllowNew = false;
            // 
            // ttActiveSequence
            // 
            this.ttActiveSequence.ToolTipTitle = "Active Sequence";
            // 
            // XwaForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(794, 571);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.toolXWA);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Menu = this.menuXWA;
            this.Name = "XwaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ye Olde Galactic Empire Mission Editor - XWA";
            this.Activated += new System.EventHandler(this.form_Activated);
            this.Deactivate += new System.EventHandler(this.form_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.form_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataWaypoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataWaypoints_Raw)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tabFG.ResumeLayout(false);
            this.tabFG.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterRegion)).EndInit();
            this.tabFGMinor.ResumeLayout(false);
            this.tabCraft.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSC)).EndInit();
            this.grpCraft3.ResumeLayout(false);
            this.grpCraft3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWaveDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWaves)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCraft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGU)).EndInit();
            this.grpCraft2.ResumeLayout(false);
            this.grpCraft2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSpacing)).EndInit();
            this.grpCraft4.ResumeLayout(false);
            this.grpCraft4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBackdrop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExplode)).EndInit();
            this.tabArrDep.ResumeLayout(false);
            this.grpDep.ResumeLayout(false);
            this.grpDep.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDepMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDepSec)).EndInit();
            this.groupBox10.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numArrSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArrMin)).EndInit();
            this.panel10.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.tabGoals.ResumeLayout(false);
            this.tabGoals.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGoalTimeLimit)).EndInit();
            this.grpGoal.ResumeLayout(false);
            this.grpGoal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGoalActSeq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGoalPoints)).EndInit();
            this.groupBox16.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numGoalTeam)).EndInit();
            this.tabWP.ResumeLayout(false);
            this.tabWP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWPOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWPOrderRegion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHYP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWPCapture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSP2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRoll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataO_Raw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataWP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataWP_Raw)).EndInit();
            this.tabOrders.ResumeLayout(false);
            this.tabOrders.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numORegion)).EndInit();
            this.grpSecOrder.ResumeLayout(false);
            this.grpSecOrder.PerformLayout();
            this.grpPrimOrder.ResumeLayout(false);
            this.grpPrimOrder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOVar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOVar1)).EndInit();
            this.groupBox15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numOVar2)).EndInit();
            this.tapOption.ResumeLayout(false);
            this.tapOption.PerformLayout();
            this.grpRole.ResumeLayout(false);
            this.grpRole.PerformLayout();
            this.grpSkip.ResumeLayout(false);
            this.groupBox22.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numOptWaves)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOptCraft)).EndInit();
            this.groupBox21.ResumeLayout(false);
            this.groupBox20.ResumeLayout(false);
            this.groupBox19.ResumeLayout(false);
            this.tabArrDep2.ResumeLayout(false);
            this.grpMoreDep.ResumeLayout(false);
            this.grpMoreDep.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDepClockSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDepClockMin)).EndInit();
            this.grpMoreArrival.ResumeLayout(false);
            this.grpMoreArrival.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArrRandMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArrRandSec)).EndInit();
            this.tabMess.ResumeLayout(false);
            this.tabMess.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMessType)).EndInit();
            this.grpMessCancel.ResumeLayout(false);
            this.grpMessages.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.grpSend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numMessDelay)).EndInit();
            this.tabGlob.ResumeLayout(false);
            this.tabGlob.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGlobTrigPts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGlobActSeq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGlobDelay)).EndInit();
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
            this.grpTeamPMF.ResumeLayout(false);
            this.grpTeamPMF.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPMFEomDelay)).EndInit();
            this.grpTeamOMC.ResumeLayout(false);
            this.grpTeamOMC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOMCEomDelay)).EndInit();
            this.grpTeamPMC.ResumeLayout(false);
            this.grpTeamPMC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPMCEomDelay)).EndInit();
            this.groupBox30.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel20.ResumeLayout(false);
            this.panel20.PerformLayout();
            this.panel19.ResumeLayout(false);
            this.panel19.PerformLayout();
            this.panel18.ResumeLayout(false);
            this.panel18.PerformLayout();
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.tabMission.ResumeLayout(false);
            this.tabMission.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMissTimeMin)).EndInit();
            this.tabMission2.ResumeLayout(false);
            this.tabMission2.PerformLayout();
            this.groupBox40.ResumeLayout(false);
            this.groupBox40.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGCCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGCValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGCVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGCIndex)).EndInit();
            this.grpGlobalUnits.ResumeLayout(false);
            this.grpGlobalUnits.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGUSpecial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGUIndex)).EndInit();
            this.grpGlobalGroups.ResumeLayout(false);
            this.grpGlobalGroups.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGGSpecial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGGIndex)).EndInit();
            this.grpRegions.ResumeLayout(false);
            this.grpRegions.PerformLayout();
            this.groupBox37.ResumeLayout(false);
            this.groupBox37.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataOrders_Raw)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private MainMenu menuXWA;
		private MenuItem menuFile;
		private MenuItem menuNew;
		private MenuItem menuNewTIE;
		private MenuItem menuNewXvT;
		private MenuItem menuNewBoP;
		private MenuItem menuNewXWA;
		private MenuItem menuOpen;
		private MenuItem menuSave;
		private MenuItem menuSaveAs;
		private MenuItem menuSaveAsTIE;
		private MenuItem menuSaveAsXvT;
		private MenuItem menuSaveAsBoP;
		private MenuItem menuSaveAsXWA;
		private MenuItem menuItem23;
		private MenuItem menuExit;
		private MenuItem menuEdit;
		private MenuItem menuUndo;
		private MenuItem menuItem14;
		private MenuItem menuCut;
		private MenuItem menuCopy;
		private MenuItem menuPaste;
		private MenuItem menuDelete;
		private MenuItem menuTools;
		private MenuItem menuVerify;
		private MenuItem menuMap;
		private MenuItem menuBrief;
		private MenuItem menuOptions;
		private MenuItem menuHelp;
		private MenuItem menuHelpInfo;
		private MenuItem menuAbout;
		private MenuItem menuIDMR;
		private MenuItem menuER;
		private ImageList imgToolbar;
		private ToolBar toolXWA;
		private ToolBarButton toolNew;
		private ToolBarButton toolOpen;
		private ToolBarButton toolSave;
		private ToolBarButton toolSaveAs;
		private ToolBarButton toolSep1;
		private ToolBarButton toolNewItem;
		private ToolBarButton toolDeleteItem;
		private ToolBarButton toolCopy;
		private ToolBarButton toolPaste;
		private ToolBarButton toolSep2;
		private ToolBarButton toolMap;
		private ToolBarButton toolBriefing;
		private ToolBarButton toolVerify;
		private ToolBarButton toolSep3;
		private ToolBarButton toolOptions;
		private ToolBarButton toolLst;
		private ToolBarButton toolHelp;
		private OpenFileDialog opnXWA;
		private SaveFileDialog savXWA;
		private System.Data.DataView dataWaypoints;
		private TabControl tabMain;
		private TabPage tabFG;
		private TabControl tabFGMinor;
		private TabPage tabCraft;
		private GroupBox groupBox1;
		private CheckBox chkRandSC;
		private TextBox txtName;
		private Label lblNotUsed;
		private TextBox txtSpecCargo;
		private NumericUpDown numSC;
		private Label lblCargo;
		private TextBox txtCargo;
		private Label lblSC;
		private Label lblName;
		private Label label24;
		private GroupBox grpCraft3;
		private NumericUpDown numWaves;
		private Label label10;
		private Label label11;
		private Label label12;
		private NumericUpDown numCraft;
		private NumericUpDown numGG;
		private NumericUpDown numGU;
		private Label label3;
		private GroupBox grpCraft2;
		private ComboBox cboCraft;
		private Label label7;
		private Label label8;
		private Label label9;
		private Label label13;
		private Label label14;
		private Label label15;
		private Label label16;
		private NumericUpDown numSpacing;
		private ComboBox cboIFF;
		private ComboBox cboAI;
		private ComboBox cboMarkings;
		private ComboBox cboPlayer;
		private ComboBox cboFormation;
		private Button cmdForms;
		private Label label4;
		private ComboBox cboRadio;
		private Label label58;
		private ComboBox cboPosition;
		private Label label111;
		private ComboBox cboTeam;
		private Label lblFG;
		private Label lblStarting;
		private GroupBox grpCraft4;
		private Label lblExplode;
		private NumericUpDown numExplode;
		private Label label67;
		private Label label18;
		private ComboBox cboWarheads;
		private Label lblStatus;
		private ComboBox cboBeam;
		private ComboBox cboStatus;
		private Label label20;
		private ComboBox cboStatus2;
		private Label label2;
		private ComboBox cboCounter;
		private Label label5;
		private TabPage tabArrDep;
		private CheckBox chkArrHuman;
		private Button cmdCopyAD;
		private Label label36;
		private GroupBox grpDep;
		private NumericUpDown numDepMin;
		private NumericUpDown numDepSec;
		private RadioButton optDepAND;
		private RadioButton optDepOR;
		private Label label47;
		private Label label41;
		private Label label40;
		private Label label39;
		private Label label37;
		private GroupBox groupBox10;
		private RadioButton optDepMSAlt;
		private RadioButton optDepHypAlt;
		private ComboBox cboDepMSAlt;
		private GroupBox groupBox9;
		private RadioButton optDepHyp;
		private ComboBox cboDepMS;
		private RadioButton optDepMS;
		private Label lblDep1;
		private ComboBox cboAbort;
		private Label lblDep2;
		private GroupBox groupBox8;
		private NumericUpDown numArrSec;
		private NumericUpDown numArrMin;
		private Panel panel10;
		private RadioButton optArr3AND4;
		private RadioButton optArr3OR4;
		private Panel panel9;
		private RadioButton optArr1AND2;
		private RadioButton optArr1OR2;
		private RadioButton optArr12AND34;
		private RadioButton optArr12OR34;
		private Label label38;
		private Label lblArr1;
		private GroupBox groupBox11;
		private RadioButton optArrHypAlt;
		private ComboBox cboArrMSAlt;
		private RadioButton optArrMSAlt;
		private GroupBox groupBox12;
		private ComboBox cboArrMS;
		private RadioButton optArrHyp;
		private RadioButton optArrMS;
		private Label lblArr2;
		private Label label42;
		private Label label43;
		private Label lblArr3;
		private Label lblArr4;
		private ComboBox cboADTrigAmount;
		private ComboBox cboADTrigType;
		private ComboBox cboADTrigVar;
		private ComboBox cboADTrig;
		private Label label44;
		private ComboBox cboDiff;
		private Label label45;
		private Label label46;
		private Button cmdPasteAD;
		private TabPage tabGoals;
		private GroupBox grpGoal;
		private Label label61;
		private ComboBox cboGoalAmount;
		private ComboBox cboGoalArgument;
		private ComboBox cboGoalTrigger;
		private Label label66;
		private CheckBox chkGoalEnable;
		private NumericUpDown numGoalPoints;
		private Label label65;
		private Label label62;
		private TextBox txtGoalInc;
		private Label label60;
		private GroupBox groupBox16;
		private Label lblGoal1;
		private Label lblGoal2;
		private Label lblGoal3;
		private Label lblGoal4;
		private Label lblGoal5;
		private Label lblGoal8;
		private Label lblGoal6;
		private Label lblGoal7;
		private TextBox txtGoalComp;
		private TextBox txtGoalFail;
		private Label label63;
		private Label label64;
		private NumericUpDown numGoalTeam;
		private TabPage tabWP;
		private Label label76;
		private NumericUpDown numRoll;
		private NumericUpDown numPitch;
		private NumericUpDown numYaw;
		private Label label56;
		private DataGrid dataWP;
		private DataGrid dataWP_Raw;
		private CheckBox chkWPHyp;
		private CheckBox chkWP8;
		private CheckBox chkWP7;
		private CheckBox chkWP2;
		private CheckBox chkWP1;
		private CheckBox chkWPCapture;
		private CheckBox chkSP2;
		private CheckBox chkSP1;
		private CheckBox chkWP6;
		private CheckBox chkWP5;
		private CheckBox chkWP4;
		private CheckBox chkWP3;
		private Label label77;
		private Label label78;
        private TabPage tabOrders;
		private Label label57;
		private TextBox txtOString;
		private Label label54;
		private Button cmdCopyOrder;
		private Label lblODesc;
		private GroupBox grpSecOrder;
		private Label label49;
		private RadioButton optOT3T4OR;
		private ComboBox cboOT3;
		private ComboBox cboOT3Type;
		private ComboBox cboOT4Type;
		private ComboBox cboOT4;
		private RadioButton optOT3T4AND;
		private GroupBox grpPrimOrder;
		private Label label50;
		private RadioButton optOT1T2OR;
		private ComboBox cboOT1;
		private ComboBox cboOT1Type;
		private ComboBox cboOT2Type;
		private ComboBox cboOT2;
		private RadioButton optOT1T2AND;
		private NumericUpDown numOVar1;
		private Label lblOVar1;
		private ComboBox cboOThrottle;
		private Label label51;
		private GroupBox groupBox15;
		private Label lblOrder1;
		private Label lblOrder2;
		private Label lblOrder3;
		private Label lblOrder4;
		private ComboBox cboOrders;
		private Label lblOVar2;
		private NumericUpDown numOVar2;
		private Button cmdPasteOrder;
		private TabPage tapOption;
		private GroupBox grpRole;
		private ComboBox cboRole1;
		private ComboBox cboRole2;
		private GroupBox grpSkip;
		private Button cmdCopySkip;
		private Label label71;
		private ComboBox cboSkipAmount;
		private ComboBox cboSkipType;
		private ComboBox cboSkipVar;
		private ComboBox cboSkipTrig;
		private Label label72;
		private Button cmdPasteSkip;
		private RadioButton optSkipAND;
		private RadioButton optSkipOR;
		private Label lblSkipTrig1;
		private Label lblSkipTrig2;
		private GroupBox groupBox22;
		private Label lblOptCraft1;
		private ComboBox cboOptCraft;
		private Label label70;
		private NumericUpDown numOptWaves;
		private Label label69;
		private Label label68;
		private ComboBox cboOptCat;
		private NumericUpDown numOptCraft;
		private Label lblOptCraft2;
		private Label lblOptCraft3;
		private Label lblOptCraft4;
		private Label lblOptCraft5;
		private Label lblOptCraft6;
		private Label lblOptCraft7;
		private Label lblOptCraft8;
		private Label lblOptCraft9;
		private Label lblOptCraft10;
		private GroupBox groupBox21;
		private CheckBox chkOptCNone;
		private CheckBox chkOptCChaff;
		private CheckBox chkOptCFlare;
        private CheckBox chkOptCCluster;
        private GroupBox groupBox20;
		private CheckBox chkOptBNone;
		private CheckBox chkOptBTractor;
		private CheckBox chkOptBJamming;
		private CheckBox chkOptBDecoy;
        private CheckBox chkOptBEnergy;
        private GroupBox groupBox19;
		private CheckBox chkOptWNone;
		private CheckBox chkOptWBomb;
		private CheckBox chkOptWRocket;
		private CheckBox chkOptWMissile;
		private CheckBox chkOptWTorp;
		private CheckBox chkOptWAdvMissile;
		private CheckBox chkOptWAdvTorp;
		private CheckBox chkOptWMagPulse;
		private TabPage tabArrDep2;
		private Label label1;
		private ListBox lstFG;
		private TabPage tabMess;
		private ComboBox cboMessColor;
		private Label label109;
		private ComboBox cboMessAmount;
		private ComboBox cboMessType;
		private ComboBox cboMessVar;
		private ComboBox cboMessTrig;
		private Label label110;
		private GroupBox grpMessages;
		private Panel panel8;
		private RadioButton optMess3OR4;
		private RadioButton optMess3AND4;
		private Panel panel7;
		private RadioButton optMess1OR2;
		private RadioButton optMess1AND2;
		private Label lblMess1;
		private Label lblMess2;
		private Label lblMess4;
		private Label lblMess3;
		private RadioButton optMess12AND34;
		private RadioButton optMess12OR34;
		private NumericUpDown numMessDelay;
		private Label label55;
		private Label lblMessage;
		private TextBox txtMessage;
		private Label label52;
		private ListBox lstMessages;
		private GroupBox grpSend;
		private CheckBox chkMess1;
		private CheckBox chkMess2;
		private CheckBox chkMess3;
		private CheckBox chkMess4;
		private CheckBox chkMess5;
		private CheckBox chkMess10;
		private CheckBox chkMess9;
		private CheckBox chkMess8;
		private CheckBox chkMess7;
		private CheckBox chkMess6;
		private TabPage tabGlob;
		private Label label112;
		private ComboBox cboGlobalTeam;
		private Label label33;
		private TextBox txtGlobalInc;
		private Label label32;
		private NumericUpDown numGlobalPoints;
		private Label label79;
		private Label label48;
		private ComboBox cboGlobalAmount;
		private ComboBox cboGlobalType;
		private ComboBox cboGlobalVar;
		private ComboBox cboGlobalTrig;
		private Label label59;
		private GroupBox groupBox18;
		private Panel panel2;
		private RadioButton optPrim1OR2;
		private RadioButton optPrim1AND2;
		private Panel panel1;
		private RadioButton optPrim3OR4;
		private RadioButton optPrim3AND4;
		private Label lblPrim1;
		private Label lblPrim2;
		private Label lblPrim4;
		private Label lblPrim3;
		private RadioButton optPrim12AND34;
		private RadioButton optPrim12OR34;
		private GroupBox groupBox5;
		private Panel panel3;
		private RadioButton optPrev1AND2;
		private RadioButton optPrev1OR2;
		private Label lblPrev1;
		private Label lblPrev2;
		private Label lblPrev4;
		private Label lblPrev3;
		private RadioButton optPrev12AND34;
		private RadioButton optPrev12OR34;
		private Panel panel4;
		private RadioButton optPrev3OR4;
		private RadioButton optPrev3AND4;
		private GroupBox groupBox6;
		private Panel panel6;
		private RadioButton optSec3OR4;
		private RadioButton optSec3AND4;
		private Panel panel5;
		private RadioButton optSec1OR2;
		private RadioButton optSec1AND2;
		private Label lblSec1;
		private Label lblSec2;
		private Label lblSec4;
		private Label lblSec3;
		private RadioButton optSec12AND34;
		private RadioButton optSec12OR34;
		private TextBox txtGlobalComp;
		private TextBox txtGlobalFail;
		private Label label34;
		private Label label35;
		private TabPage tabTeam;
		private GroupBox groupBox32;
		private GroupBox grpTeamPMF;
		private TextBox txtPrimFail1;
		private TextBox txtPrimFail2;
		private GroupBox grpTeamOMC;
		private TextBox txtOutstanding1;
		private TextBox txtOutstanding2;
		private GroupBox grpTeamPMC;
		private TextBox txtPrimComp1;
		private TextBox txtPrimComp2;
		private TextBox txtTeamName;
		private Label label96;
		private GroupBox groupBox30;
		private Label lblTeam1;
		private Label lblTeam2;
		private Label lblTeam3;
		private Label lblTeam4;
		private Label lblTeam5;
		private Label lblTeam6;
		private Label lblTeam7;
		private Label lblTeam8;
		private Label lblTeam9;
		private Label lblTeam10;
		private TabPage tabMission;
		private Label label102;
		private NumericUpDown numMissTimeMin;
		private Label label100;
		private ComboBox cboHangar;
		private Label label97;
		private TextBox txtMissDesc;
		private TextBox txtMissSucc;
		private TextBox txtMissFail;
		private Label label98;
		private Label label99;
		private DataGrid dataO_Raw;
		private DataGrid dataO;
		private NumericUpDown numHYP;
		private NumericUpDown numWPCapture;
		private NumericUpDown numSP2;
		private NumericUpDown numSP1;
		private Label label25;
		private System.Data.DataView dataOrders;
		private System.Data.DataView dataOrders_Raw;
		private System.Data.DataView dataWaypoints_Raw;
		private GroupBox grpMessCancel;
		private RadioButton optMessC1AND2;
		private RadioButton optMessC1OR2;
		private Label lblMess5;
		private Label lblMess6;
		private TextBox txtVoice;
		private Label label26;
		private ComboBox cboMessFG;
		private CheckBox chkGU;
		private Label label31;
		private NumericUpDown numGoalActSeq;
		private Label label103;
		private NumericUpDown numOVar3;
		private Label lblOVar3;
		private NumericUpDown numORegion;
		private Label label73;
		private TextBox txtRole;
		private ComboBox cboSkipOrder;
		private Label label74;
        private Label label80;
		private NumericUpDown numDepClockSec;
		private NumericUpDown numDepClockMin;
		private NumericUpDown numBackdrop;
		private Label label122;
		private ComboBox cboGlobCargo;
		private NumericUpDown numGlobDelay;
		private Label label128;
		private NumericUpDown numGlobActSeq;
		private ComboBox cboLogo;
		private ComboBox cboOfficer;
		private Label label130;
		private Label label129;
		private CheckBox chkEnd;
		private TabPage tabMission2;
		private GroupBox grpGlobalGroups;
		private GroupBox grpRegions;
		private Label label137;
		private Label label136;
		private Label label135;
		private Label label134;
		private TextBox txtRegion4;
		private TextBox txtRegion3;
		private TextBox txtRegion2;
		private TextBox txtRegion1;
		private GroupBox groupBox37;
		private Label label133;
		private Label label132;
		private Label label131;
		private Label label108;
		private TextBox txtIFF6;
		private TextBox txtIFF5;
		private TextBox txtIFF4;
		private TextBox txtIFF3;
		private Label label101;
		private TextBox txtNotes;
		private TextBox txtGGName;
		private GroupBox groupBox40;
		private NumericUpDown numGCValue;
		private Label label141;
		private NumericUpDown numGCVolume;
		private Label label140;
		private TextBox txtGlobCargo;
		private NumericUpDown numGCIndex;
		private Label label138;
		private ComboBox cboGlobSpecCargo;
		private Button cmdBackdrop;
		private TextBox txtPMFVoiceID;
		private TextBox txtOMCVoiceID;
		private TextBox txtPMCVoiceID;
		private Panel panel12;
		private RadioButton optAllies3;
		private RadioButton optAllies1;
		private RadioButton optAllies2;
		private Panel panel11;
		private RadioButton optAllies6;
		private RadioButton optAllies4;
		private RadioButton optAllies5;
		private Panel panel20;
		private RadioButton optAllies30;
		private RadioButton optAllies28;
		private RadioButton optAllies29;
		private Panel panel19;
		private RadioButton optAllies27;
		private RadioButton optAllies25;
		private RadioButton optAllies26;
		private Panel panel18;
		private RadioButton optAllies24;
		private RadioButton optAllies22;
		private RadioButton optAllies23;
		private Panel panel17;
		private RadioButton optAllies21;
		private RadioButton optAllies19;
		private RadioButton optAllies20;
		private Panel panel16;
		private RadioButton optAllies18;
		private RadioButton optAllies16;
		private RadioButton optAllies17;
		private Panel panel15;
		private RadioButton optAllies15;
		private RadioButton optAllies13;
		private RadioButton optAllies14;
		private Panel panel14;
		private RadioButton optAllies12;
		private RadioButton optAllies10;
		private RadioButton optAllies11;
		private Panel panel13;
		private RadioButton optAllies9;
		private RadioButton optAllies7;
		private RadioButton optAllies8;
		private NumericUpDown numPMFEomDelay;
		private NumericUpDown numOMCEomDelay;
		private NumericUpDown numPMCEomDelay;
		private Label label143;
		private Label lblGC;
		private MenuItem menuLST;
		private ComboBox cboSkipPara;
		private ComboBox cboGoalPara;
		private ComboBox cboADPara;
		private ComboBox cboMessPara;
		private ComboBox cboGlobalPara;
		private Label label23;
		private Label label22;
		private Label label6;
		private Label label53;
		private TextBox txtPrimFailNote;
		private Label label30;
		private TextBox txtOutstandingNote;
		private Label label27;
		private TextBox txtPrimCompNote;
		private PictureBox pctLogo;
		private Label label123;
		private TextBox txtFailNote;
		private Label label82;
		private TextBox txtSuccNote;
		private Label label75;
		private TextBox txtDescNote;
		private TextBox txtMessNote;
		private Label label149;
		private MenuItem menuRecent;
		private MenuItem menuRec1;
		private MenuItem menuRec2;
		private MenuItem menuRec3;
		private MenuItem menuRec4;
		private MenuItem menuRec5;
		private MenuItem menuTest;
        private CheckBox chkOptWIonPulse;
        private MenuItem menuGoalSummary;
        private ComboBox cboRole2Teams;
        private ComboBox cboRole1Teams;
        private Button cmdMoveFGDown;
        private Button cmdMoveFGUp;
        private Button cmdMoveMessDown;
        private Button cmdMoveMessUp;
        private MenuItem menuNewXwing;
        private MenuItem menuHyperbuoy;
		private MenuItem menuSuperBackdrops;
        private Label lblDelay;
        private Button cmdMissionCraft;
        private ComboBox cboPilot;
        private Label lblOSpeedNote;
        private Label lblOVar1Note;
        private Label lblOVar2Note;
        private Label lblGoalTimeLimitSec;
        private Label lblGoalTimeLimit;
        private NumericUpDown numGoalTimeLimit;
        private Label lblGoalTimeLimitNote;
        private ComboBox cboOSpeed;
		private MenuItem menuHooks;
		private MenuItem menuLibrary;
		private ToolBarButton toolWav;
		private MenuItem menuWav;
		private MenuItem menuMissionCraft;
		private Button cmdCopyOrderWP;
		private Button cmdPasteOrderWP;
		private NumericUpDown numWPOrder;
		private Label label150;
		private NumericUpDown numWPOrderRegion;
		private Label label19;
		private ToolTip ttActiveSequence;
        private NumericUpDown numFilterRegion;
        private CheckBox chkRegionFilter;
        private RadioButton optArrRegion;
        private RadioButton optDepRegion;
		private MenuItem menuGlobalSummary;
		private Label label17;
		private NumericUpDown numWaveDelay;
		private NumericUpDown numArrRandSec;
		private Label lblWaveDelay;
		private Label label28;
		private ComboBox cboGCVolatility;
		private Label label21;
		private ComboBox cboGCType;
		private NumericUpDown numGCCount;
		private Label label29;
		private NumericUpDown numGGIndex;
		private Label label81;
		private Label label83;
		private ComboBox cboGGLeader;
		private TextBox txtGGCargo;
		private Label label84;
		private CheckBox chkGGRandom;
		private NumericUpDown numGGSpecial;
		private Label label85;
		private GroupBox grpGlobalUnits;
		private CheckBox chkGURandom;
		private NumericUpDown numGUSpecial;
		private Label label86;
		private TextBox txtGUCargo;
		private Label label87;
		private ComboBox cboGULeader;
		private Label label88;
		private NumericUpDown numGUIndex;
		private Label label89;
		private TextBox txtGUName;
		private CheckBox chkGoalsUnimportant;
		private ComboBox cboFailOfficer;
		private ComboBox cboWinOfficer;
		private Label label91;
		private Label label90;
		private GroupBox grpMoreArrival;
		private NumericUpDown numArrRandMin;
		private Label lblRandomArrivalDelayMinutes;
		private Label lblRandomArrivalDelaySeconds;
		private Label lblRandomArrivalDelayDesc;
		private ComboBox cboStopArrivingWhen;
		private Label lblStopArrivingWhen;
		private GroupBox grpMoreDep;
		private Label label94;
		private Label label93;
		private Label label92;
		private Label label95;
		private ComboBox cboHandicap;
		private Label label104;
		private ComboBox cboMessSpecial;
		private Label label106;
		private Label label105;
		private CheckBox chkMessHeader;
		private NumericUpDown numMessType;
		private NumericUpDown numGlobTrigPts;
		private Label label107;
		private Label lblGlobDelay;
		private Label label113;
		private ComboBox cboPMFEomFG;
		private ComboBox cboOMCEomFG;
		private ComboBox cboPMCEomFG;
		private Label label116;
		private Label label115;
		private Label label114;
		private Label lblPMFDelay;
		private Label lblOMCDelay;
		private Label lblPMCDelay;
		private Label lblOVar3Note;
	}
}