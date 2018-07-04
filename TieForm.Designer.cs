using System.Windows.Forms;

namespace Idmr.Yogeme
{
	partial class TieForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TieForm));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabFG = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.lstFG = new System.Windows.Forms.ListBox();
            this.tabFGMinor = new System.Windows.Forms.TabControl();
            this.tabCraft = new System.Windows.Forms.TabPage();
            this.cmdMoveFGDown = new System.Windows.Forms.Button();
            this.cmdMoveFGUp = new System.Windows.Forms.Button();
            this.lblFG = new System.Windows.Forms.Label();
            this.grpCraft4 = new System.Windows.Forms.GroupBox();
            this.numBackdrop = new System.Windows.Forms.NumericUpDown();
            this.cmdBackdrop = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.cboWarheads = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboBeam = new System.Windows.Forms.ComboBox();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.lblBackdrop = new System.Windows.Forms.Label();
            this.grpCraft2 = new System.Windows.Forms.GroupBox();
            this.chkRadio = new System.Windows.Forms.CheckBox();
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
            this.grpCraft3 = new System.Windows.Forms.GroupBox();
            this.numWaves = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.numCraft = new System.Windows.Forms.NumericUpDown();
            this.numGlobal = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkRandSC = new System.Windows.Forms.CheckBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblNotUsed = new System.Windows.Forms.Label();
            this.txtSpecCargo = new System.Windows.Forms.TextBox();
            this.numSC = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCargo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPilot = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblStarting = new System.Windows.Forms.Label();
            this.tabArrDep = new System.Windows.Forms.TabPage();
            this.cmdCopyAD = new System.Windows.Forms.Button();
            this.imgToolbar = new System.Windows.Forms.ImageList(this.components);
            this.label21 = new System.Windows.Forms.Label();
            this.grpDep = new System.Windows.Forms.GroupBox();
            this.numDepSec = new System.Windows.Forms.NumericUpDown();
            this.numDepMin = new System.Windows.Forms.NumericUpDown();
            this.label47 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.optDepMSAlt = new System.Windows.Forms.RadioButton();
            this.optDepHypAlt = new System.Windows.Forms.RadioButton();
            this.cboDepMSAlt = new System.Windows.Forms.ComboBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.optDepHyp = new System.Windows.Forms.RadioButton();
            this.cboDepMS = new System.Windows.Forms.ComboBox();
            this.optDepMS = new System.Windows.Forms.RadioButton();
            this.lblDep = new System.Windows.Forms.Label();
            this.cboAbort = new System.Windows.Forms.ComboBox();
            this.grpArr = new System.Windows.Forms.GroupBox();
            this.numArrSec = new System.Windows.Forms.NumericUpDown();
            this.numArrMin = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.optArrAND = new System.Windows.Forms.RadioButton();
            this.lblArr1 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.optArrHypAlt = new System.Windows.Forms.RadioButton();
            this.cboArrMSAlt = new System.Windows.Forms.ComboBox();
            this.optArrMSAlt = new System.Windows.Forms.RadioButton();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cboArrMS = new System.Windows.Forms.ComboBox();
            this.optArrHyp = new System.Windows.Forms.RadioButton();
            this.optArrMS = new System.Windows.Forms.RadioButton();
            this.lblArr2 = new System.Windows.Forms.Label();
            this.optArrOR = new System.Windows.Forms.RadioButton();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.cboADTrigAmount = new System.Windows.Forms.ComboBox();
            this.cboADTrigType = new System.Windows.Forms.ComboBox();
            this.cboADTrigVar = new System.Windows.Forms.ComboBox();
            this.cboADTrig = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.cboDiff = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.cmdPasteAD = new System.Windows.Forms.Button();
            this.tabGoals = new System.Windows.Forms.TabPage();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.label36 = new System.Windows.Forms.Label();
            this.numBonGoalP = new System.Windows.Forms.NumericUpDown();
            this.label35 = new System.Windows.Forms.Label();
            this.cboBonGoalA = new System.Windows.Forms.ComboBox();
            this.cboBonGoalT = new System.Windows.Forms.ComboBox();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.label34 = new System.Windows.Forms.Label();
            this.cboSecretGoalA = new System.Windows.Forms.ComboBox();
            this.cboSecretGoalT = new System.Windows.Forms.ComboBox();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.label33 = new System.Windows.Forms.Label();
            this.cboSecGoalA = new System.Windows.Forms.ComboBox();
            this.cboSecGoalT = new System.Windows.Forms.ComboBox();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.label32 = new System.Windows.Forms.Label();
            this.cboPrimGoalA = new System.Windows.Forms.ComboBox();
            this.cboPrimGoalT = new System.Windows.Forms.ComboBox();
            this.tabWP = new System.Windows.Forms.TabPage();
            this.label80 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.numRoll = new System.Windows.Forms.NumericUpDown();
            this.numPitch = new System.Windows.Forms.NumericUpDown();
            this.numYaw = new System.Windows.Forms.NumericUpDown();
            this.label56 = new System.Windows.Forms.Label();
            this.dataWP = new System.Windows.Forms.DataGrid();
            this.dataWP_Raw = new System.Windows.Forms.DataGrid();
            this.chkWPBrief = new System.Windows.Forms.CheckBox();
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
            this.tabOrders = new System.Windows.Forms.TabPage();
            this.lblOVar2Note = new System.Windows.Forms.Label();
            this.lblOVar1Note = new System.Windows.Forms.Label();
            this.cmdCopyOrder = new System.Windows.Forms.Button();
            this.lblODesc = new System.Windows.Forms.Label();
            this.grpSecOrder = new System.Windows.Forms.GroupBox();
            this.label31 = new System.Windows.Forms.Label();
            this.optOT3T4OR = new System.Windows.Forms.RadioButton();
            this.cboOT3 = new System.Windows.Forms.ComboBox();
            this.cboOT3Type = new System.Windows.Forms.ComboBox();
            this.cboOT4Type = new System.Windows.Forms.ComboBox();
            this.cboOT4 = new System.Windows.Forms.ComboBox();
            this.optOT3T4AND = new System.Windows.Forms.RadioButton();
            this.grpPrimOrder = new System.Windows.Forms.GroupBox();
            this.label30 = new System.Windows.Forms.Label();
            this.optOT1T2OR = new System.Windows.Forms.RadioButton();
            this.cboOT1 = new System.Windows.Forms.ComboBox();
            this.cboOT1Type = new System.Windows.Forms.ComboBox();
            this.cboOT2Type = new System.Windows.Forms.ComboBox();
            this.cboOT2 = new System.Windows.Forms.ComboBox();
            this.optOT1T2AND = new System.Windows.Forms.RadioButton();
            this.numOVar1 = new System.Windows.Forms.NumericUpDown();
            this.lblOVar1 = new System.Windows.Forms.Label();
            this.cboOThrottle = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.lblOrder1 = new System.Windows.Forms.Label();
            this.lblOrder2 = new System.Windows.Forms.Label();
            this.lblOrder3 = new System.Windows.Forms.Label();
            this.cboOrders = new System.Windows.Forms.ComboBox();
            this.lblOVar2 = new System.Windows.Forms.Label();
            this.numOVar2 = new System.Windows.Forms.NumericUpDown();
            this.cmdPasteOrder = new System.Windows.Forms.Button();
            this.tabOptions = new System.Windows.Forms.TabPage();
            this.grpPermaDeath = new System.Windows.Forms.GroupBox();
            this.chkPermaDeath = new System.Windows.Forms.CheckBox();
            this.label82 = new System.Windows.Forms.Label();
            this.numPermaDeathID = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.tabUnk = new System.Windows.Forms.TabPage();
            this.label84 = new System.Windows.Forms.Label();
            this.label83 = new System.Windows.Forms.Label();
            this.chkUnk21 = new System.Windows.Forms.CheckBox();
            this.chkUnk19 = new System.Windows.Forms.CheckBox();
            this.numUnk20 = new System.Windows.Forms.NumericUpDown();
            this.numUnk17 = new System.Windows.Forms.NumericUpDown();
            this.numUnk16 = new System.Windows.Forms.NumericUpDown();
            this.numUnk15 = new System.Windows.Forms.NumericUpDown();
            this.numUnk12 = new System.Windows.Forms.NumericUpDown();
            this.numUnk11 = new System.Windows.Forms.NumericUpDown();
            this.numUnk5 = new System.Windows.Forms.NumericUpDown();
            this.numUnk1 = new System.Windows.Forms.NumericUpDown();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.tabMess = new System.Windows.Forms.TabPage();
            this.cmdMoveMessDown = new System.Windows.Forms.Button();
            this.cmdMoveMessUp = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.numMessDelay = new System.Windows.Forms.NumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.cboMessAmount = new System.Windows.Forms.ComboBox();
            this.cboMessType = new System.Windows.Forms.ComboBox();
            this.cboMessVar = new System.Windows.Forms.ComboBox();
            this.cboMessTrig = new System.Windows.Forms.ComboBox();
            this.label58 = new System.Windows.Forms.Label();
            this.grpMessages = new System.Windows.Forms.GroupBox();
            this.lblMess1 = new System.Windows.Forms.Label();
            this.lblMess2 = new System.Windows.Forms.Label();
            this.optMessOR = new System.Windows.Forms.RadioButton();
            this.optMessAND = new System.Windows.Forms.RadioButton();
            this.label54 = new System.Windows.Forms.Label();
            this.cboMessColor = new System.Windows.Forms.ComboBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label52 = new System.Windows.Forms.Label();
            this.lstMessages = new System.Windows.Forms.ListBox();
            this.label53 = new System.Windows.Forms.Label();
            this.txtShort = new System.Windows.Forms.TextBox();
            this.tabGlobal = new System.Windows.Forms.TabPage();
            this.label79 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.cboGlobalAmount = new System.Windows.Forms.ComboBox();
            this.cboGlobalType = new System.Windows.Forms.ComboBox();
            this.cboGlobalVar = new System.Windows.Forms.ComboBox();
            this.cboGlobalTrig = new System.Windows.Forms.ComboBox();
            this.label59 = new System.Windows.Forms.Label();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.lblBon1 = new System.Windows.Forms.Label();
            this.lblBon2 = new System.Windows.Forms.Label();
            this.optBonOR = new System.Windows.Forms.RadioButton();
            this.optBonAND = new System.Windows.Forms.RadioButton();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.lblSec1 = new System.Windows.Forms.Label();
            this.lblSec2 = new System.Windows.Forms.Label();
            this.optSecOR = new System.Windows.Forms.RadioButton();
            this.optSecAND = new System.Windows.Forms.RadioButton();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.lblPrim1 = new System.Windows.Forms.Label();
            this.lblPrim2 = new System.Windows.Forms.Label();
            this.optPrimOR = new System.Windows.Forms.RadioButton();
            this.optPrimAND = new System.Windows.Forms.RadioButton();
            this.tabOfficer = new System.Windows.Forms.TabPage();
            this.label150 = new System.Windows.Forms.Label();
            this.lblQuestionNote = new System.Windows.Forms.Label();
            this.cmdAutoAlign = new System.Windows.Forms.Button();
            this.cmdPreview = new System.Windows.Forms.Button();
            this.label61 = new System.Windows.Forms.Label();
            this.txtQuestion = new System.Windows.Forms.TextBox();
            this.label60 = new System.Windows.Forms.Label();
            this.cboOfficer = new System.Windows.Forms.ComboBox();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.optFO = new System.Windows.Forms.RadioButton();
            this.optSO = new System.Windows.Forms.RadioButton();
            this.optBoth = new System.Windows.Forms.RadioButton();
            this.cboQuestion = new System.Windows.Forms.ComboBox();
            this.txtAnswer = new System.Windows.Forms.TextBox();
            this.label62 = new System.Windows.Forms.Label();
            this.cboQTrigType = new System.Windows.Forms.ComboBox();
            this.cboQTrig = new System.Windows.Forms.ComboBox();
            this.label63 = new System.Windows.Forms.Label();
            this.tabMission = new System.Windows.Forms.TabPage();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.cboPF2Color = new System.Windows.Forms.ComboBox();
            this.cboPF1Color = new System.Windows.Forms.ComboBox();
            this.txtPrimFail1 = new System.Windows.Forms.TextBox();
            this.txtPrimFail2 = new System.Windows.Forms.TextBox();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.cboSC2Color = new System.Windows.Forms.ComboBox();
            this.cboSC1Color = new System.Windows.Forms.ComboBox();
            this.txtSecComp1 = new System.Windows.Forms.TextBox();
            this.txtSecComp2 = new System.Windows.Forms.TextBox();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.cboPC2Color = new System.Windows.Forms.ComboBox();
            this.cboPC1Color = new System.Windows.Forms.ComboBox();
            this.txtPrimComp1 = new System.Windows.Forms.TextBox();
            this.txtPrimComp2 = new System.Windows.Forms.TextBox();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.chkIFF3 = new System.Windows.Forms.CheckBox();
            this.txtIFF3 = new System.Windows.Forms.TextBox();
            this.label64 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.txtIFF5 = new System.Windows.Forms.TextBox();
            this.txtIFF6 = new System.Windows.Forms.TextBox();
            this.txtIFF4 = new System.Windows.Forms.TextBox();
            this.chkIFF4 = new System.Windows.Forms.CheckBox();
            this.chkIFF5 = new System.Windows.Forms.CheckBox();
            this.chkIFF6 = new System.Windows.Forms.CheckBox();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.optCapture = new System.Windows.Forms.RadioButton();
            this.optRescue = new System.Windows.Forms.RadioButton();
            this.toolTIE = new System.Windows.Forms.ToolBar();
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
            this.toolBattle = new System.Windows.Forms.ToolBarButton();
            this.toolHelp = new System.Windows.Forms.ToolBarButton();
            this.menuTIE = new System.Windows.Forms.MainMenu(this.components);
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
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuEdit = new System.Windows.Forms.MenuItem();
            this.menuUndo = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuCut = new System.Windows.Forms.MenuItem();
            this.menuCopy = new System.Windows.Forms.MenuItem();
            this.menuPaste = new System.Windows.Forms.MenuItem();
            this.menuDelete = new System.Windows.Forms.MenuItem();
            this.menuTools = new System.Windows.Forms.MenuItem();
            this.menuVerify = new System.Windows.Forms.MenuItem();
            this.menuMap = new System.Windows.Forms.MenuItem();
            this.menuBriefing = new System.Windows.Forms.MenuItem();
            this.menuBattle = new System.Windows.Forms.MenuItem();
            this.menuOptions = new System.Windows.Forms.MenuItem();
            this.menuGoalSummary = new System.Windows.Forms.MenuItem();
            this.menuTest = new System.Windows.Forms.MenuItem();
            this.menuHelp = new System.Windows.Forms.MenuItem();
            this.menuHelpInfo = new System.Windows.Forms.MenuItem();
            this.menuAbout = new System.Windows.Forms.MenuItem();
            this.menuIDMR = new System.Windows.Forms.MenuItem();
            this.menuER = new System.Windows.Forms.MenuItem();
            this.opnTIE = new System.Windows.Forms.OpenFileDialog();
            this.savTIE = new System.Windows.Forms.SaveFileDialog();
            this._dataWaypoints = new System.Data.DataView();
            this._dataWaypointsRaw = new System.Data.DataView();
            this.tabMain.SuspendLayout();
            this.tabFG.SuspendLayout();
            this.tabFGMinor.SuspendLayout();
            this.tabCraft.SuspendLayout();
            this.grpCraft4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBackdrop)).BeginInit();
            this.grpCraft2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpacing)).BeginInit();
            this.grpCraft3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWaves)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCraft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGlobal)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSC)).BeginInit();
            this.tabArrDep.SuspendLayout();
            this.grpDep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDepSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDepMin)).BeginInit();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.grpArr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArrSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArrMin)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabGoals.SuspendLayout();
            this.groupBox17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBonGoalP)).BeginInit();
            this.groupBox16.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox14.SuspendLayout();
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
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOVar2)).BeginInit();
            this.tabOptions.SuspendLayout();
            this.grpPermaDeath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPermaDeathID)).BeginInit();
            this.tabUnk.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk1)).BeginInit();
            this.tabMess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMessDelay)).BeginInit();
            this.grpMessages.SuspendLayout();
            this.tabGlobal.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.tabOfficer.SuspendLayout();
            this.groupBox27.SuspendLayout();
            this.tabMission.SuspendLayout();
            this.groupBox23.SuspendLayout();
            this.groupBox26.SuspendLayout();
            this.groupBox25.SuspendLayout();
            this.groupBox24.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.groupBox21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataWaypoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataWaypointsRaw)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabFG);
            this.tabMain.Controls.Add(this.tabMess);
            this.tabMain.Controls.Add(this.tabGlobal);
            this.tabMain.Controls.Add(this.tabOfficer);
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
            this.tabFG.Controls.Add(this.label1);
            this.tabFG.Controls.Add(this.lstFG);
            this.tabFG.Controls.Add(this.tabFGMinor);
            this.tabFG.Location = new System.Drawing.Point(4, 22);
            this.tabFG.Name = "tabFG";
            this.tabFG.Size = new System.Drawing.Size(785, 510);
            this.tabFG.TabIndex = 0;
            this.tabFG.Text = "Flight Groups";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "IFF - GG - waves x craft";
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
            this.lstFG.TabIndex = 1;
            this.lstFG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstFG_DrawItem);
            this.lstFG.SelectedIndexChanged += new System.EventHandler(this.lstFG_SelectedIndexChanged);
            // 
            // tabFGMinor
            // 
            this.tabFGMinor.Controls.Add(this.tabCraft);
            this.tabFGMinor.Controls.Add(this.tabArrDep);
            this.tabFGMinor.Controls.Add(this.tabGoals);
            this.tabFGMinor.Controls.Add(this.tabWP);
            this.tabFGMinor.Controls.Add(this.tabOrders);
            this.tabFGMinor.Controls.Add(this.tabOptions);
            this.tabFGMinor.Controls.Add(this.tabUnk);
            this.tabFGMinor.Location = new System.Drawing.Point(232, 0);
            this.tabFGMinor.Name = "tabFGMinor";
            this.tabFGMinor.SelectedIndex = 0;
            this.tabFGMinor.Size = new System.Drawing.Size(552, 504);
            this.tabFGMinor.TabIndex = 0;
            // 
            // tabCraft
            // 
            this.tabCraft.Controls.Add(this.cmdMoveFGDown);
            this.tabCraft.Controls.Add(this.cmdMoveFGUp);
            this.tabCraft.Controls.Add(this.lblFG);
            this.tabCraft.Controls.Add(this.grpCraft4);
            this.tabCraft.Controls.Add(this.grpCraft2);
            this.tabCraft.Controls.Add(this.grpCraft3);
            this.tabCraft.Controls.Add(this.groupBox1);
            this.tabCraft.Controls.Add(this.lblStarting);
            this.tabCraft.Location = new System.Drawing.Point(4, 22);
            this.tabCraft.Name = "tabCraft";
            this.tabCraft.Size = new System.Drawing.Size(544, 478);
            this.tabCraft.TabIndex = 0;
            this.tabCraft.Text = "Craft";
            // 
            // cmdMoveFGDown
            // 
            this.cmdMoveFGDown.Location = new System.Drawing.Point(449, 384);
            this.cmdMoveFGDown.Name = "cmdMoveFGDown";
            this.cmdMoveFGDown.Size = new System.Drawing.Size(75, 23);
            this.cmdMoveFGDown.TabIndex = 12;
            this.cmdMoveFGDown.Text = "Move Down";
            this.cmdMoveFGDown.UseVisualStyleBackColor = true;
            this.cmdMoveFGDown.Click += new System.EventHandler(this.cmdMoveFGDown_Click);
            // 
            // cmdMoveFGUp
            // 
            this.cmdMoveFGUp.Location = new System.Drawing.Point(449, 355);
            this.cmdMoveFGUp.Name = "cmdMoveFGUp";
            this.cmdMoveFGUp.Size = new System.Drawing.Size(75, 23);
            this.cmdMoveFGUp.TabIndex = 12;
            this.cmdMoveFGUp.Text = "Move Up";
            this.cmdMoveFGUp.UseVisualStyleBackColor = true;
            this.cmdMoveFGUp.Click += new System.EventHandler(this.cmdMoveFGUp_Click);
            // 
            // lblFG
            // 
            this.lblFG.Location = new System.Drawing.Point(288, 253);
            this.lblFG.Name = "lblFG";
            this.lblFG.Size = new System.Drawing.Size(120, 16);
            this.lblFG.TabIndex = 11;
            this.lblFG.Text = "Flight Group #0 of 0";
            // 
            // grpCraft4
            // 
            this.grpCraft4.Controls.Add(this.numBackdrop);
            this.grpCraft4.Controls.Add(this.cmdBackdrop);
            this.grpCraft4.Controls.Add(this.label18);
            this.grpCraft4.Controls.Add(this.cboWarheads);
            this.grpCraft4.Controls.Add(this.lblStatus);
            this.grpCraft4.Controls.Add(this.cboBeam);
            this.grpCraft4.Controls.Add(this.cboStatus);
            this.grpCraft4.Controls.Add(this.label20);
            this.grpCraft4.Controls.Add(this.lblBackdrop);
            this.grpCraft4.Location = new System.Drawing.Point(280, 112);
            this.grpCraft4.Name = "grpCraft4";
            this.grpCraft4.Size = new System.Drawing.Size(240, 95);
            this.grpCraft4.TabIndex = 10;
            this.grpCraft4.TabStop = false;
            this.grpCraft4.Leave += new System.EventHandler(this.grpCraft4_Leave);
            // 
            // numBackdrop
            // 
            this.numBackdrop.Location = new System.Drawing.Point(96, 16);
            this.numBackdrop.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numBackdrop.Name = "numBackdrop";
            this.numBackdrop.Size = new System.Drawing.Size(40, 20);
            this.numBackdrop.TabIndex = 27;
            this.numBackdrop.Visible = false;
            this.numBackdrop.Leave += new System.EventHandler(this.numBackdrop_Leave);
            // 
            // cmdBackdrop
            // 
            this.cmdBackdrop.Location = new System.Drawing.Point(170, 16);
            this.cmdBackdrop.Name = "cmdBackdrop";
            this.cmdBackdrop.Size = new System.Drawing.Size(64, 22);
            this.cmdBackdrop.TabIndex = 20;
            this.cmdBackdrop.Text = "&Backdrops...";
            this.cmdBackdrop.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cmdBackdrop.UseVisualStyleBackColor = true;
            this.cmdBackdrop.Visible = false;
            this.cmdBackdrop.Click += new System.EventHandler(this.cmdBackdrop_Click);
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(8, 64);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 16);
            this.label18.TabIndex = 0;
            this.label18.Text = "Beam";
            // 
            // cboWarheads
            // 
            this.cboWarheads.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWarheads.Location = new System.Drawing.Point(96, 40);
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
            this.cboBeam.Location = new System.Drawing.Point(96, 64);
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
            this.label20.Location = new System.Drawing.Point(8, 40);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(56, 16);
            this.label20.TabIndex = 0;
            this.label20.Text = "Warheads";
            // 
            // lblBackdrop
            // 
            this.lblBackdrop.AutoSize = true;
            this.lblBackdrop.Location = new System.Drawing.Point(8, 16);
            this.lblBackdrop.Name = "lblBackdrop";
            this.lblBackdrop.Size = new System.Drawing.Size(53, 13);
            this.lblBackdrop.TabIndex = 26;
            this.lblBackdrop.Text = "Backdrop";
            this.lblBackdrop.Visible = false;
            // 
            // grpCraft2
            // 
            this.grpCraft2.Controls.Add(this.chkRadio);
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
            this.grpCraft2.Location = new System.Drawing.Point(16, 184);
            this.grpCraft2.Name = "grpCraft2";
            this.grpCraft2.Size = new System.Drawing.Size(232, 240);
            this.grpCraft2.TabIndex = 9;
            this.grpCraft2.TabStop = false;
            this.grpCraft2.Leave += new System.EventHandler(this.grpCraft2_Leave);
            // 
            // chkRadio
            // 
            this.chkRadio.Location = new System.Drawing.Point(8, 216);
            this.chkRadio.Name = "chkRadio";
            this.chkRadio.Size = new System.Drawing.Size(144, 16);
            this.chkRadio.TabIndex = 18;
            this.chkRadio.Text = "FG obeys player orders";
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
            this.numLead.Location = new System.Drawing.Point(104, 168);
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
            this.label8.Size = new System.Drawing.Size(56, 16);
            this.label8.TabIndex = 0;
            this.label8.Text = "IFF";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 16);
            this.label9.TabIndex = 0;
            this.label9.Text = "AI skill";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(8, 112);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(72, 16);
            this.label13.TabIndex = 0;
            this.label13.Text = "Player";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(8, 88);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 16);
            this.label14.TabIndex = 0;
            this.label14.Text = "Markings";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(8, 136);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 16);
            this.label15.TabIndex = 0;
            this.label15.Text = "Formation";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(8, 192);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(72, 16);
            this.label16.TabIndex = 0;
            this.label16.Text = "FG spacing";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(8, 168);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(88, 16);
            this.label17.TabIndex = 0;
            this.label17.Text = "Leader spacing";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numSpacing
            // 
            this.numSpacing.Location = new System.Drawing.Point(104, 192);
            this.numSpacing.Name = "numSpacing";
            this.numSpacing.Size = new System.Drawing.Size(40, 20);
            this.numSpacing.TabIndex = 17;
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
            this.cboAI.Location = new System.Drawing.Point(88, 64);
            this.cboAI.Name = "cboAI";
            this.cboAI.Size = new System.Drawing.Size(136, 21);
            this.cboAI.TabIndex = 12;
            // 
            // cboMarkings
            // 
            this.cboMarkings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMarkings.Location = new System.Drawing.Point(88, 88);
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
            this.cboPlayer.Location = new System.Drawing.Point(88, 112);
            this.cboPlayer.Name = "cboPlayer";
            this.cboPlayer.Size = new System.Drawing.Size(136, 21);
            this.cboPlayer.TabIndex = 14;
            // 
            // cboFormation
            // 
            this.cboFormation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFormation.Location = new System.Drawing.Point(88, 136);
            this.cboFormation.Name = "cboFormation";
            this.cboFormation.Size = new System.Drawing.Size(136, 21);
            this.cboFormation.TabIndex = 15;
            this.cboFormation.SelectedIndexChanged += new System.EventHandler(this.cboFormation_SelectedIndexChanged);
            // 
            // cmdForms
            // 
            this.cmdForms.Location = new System.Drawing.Point(160, 180);
            this.cmdForms.Name = "cmdForms";
            this.cmdForms.Size = new System.Drawing.Size(64, 24);
            this.cmdForms.TabIndex = 19;
            this.cmdForms.Text = "&Forms...";
            this.cmdForms.Click += new System.EventHandler(this.cmdForms_Click);
            // 
            // grpCraft3
            // 
            this.grpCraft3.Controls.Add(this.numWaves);
            this.grpCraft3.Controls.Add(this.label10);
            this.grpCraft3.Controls.Add(this.label11);
            this.grpCraft3.Controls.Add(this.label12);
            this.grpCraft3.Controls.Add(this.numCraft);
            this.grpCraft3.Controls.Add(this.numGlobal);
            this.grpCraft3.Location = new System.Drawing.Point(280, 24);
            this.grpCraft3.Name = "grpCraft3";
            this.grpCraft3.Size = new System.Drawing.Size(240, 72);
            this.grpCraft3.TabIndex = 8;
            this.grpCraft3.TabStop = false;
            this.grpCraft3.Leave += new System.EventHandler(this.grpCraft3_Leave);
            // 
            // numWaves
            // 
            this.numWaves.Location = new System.Drawing.Point(24, 40);
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
            this.label10.Size = new System.Drawing.Size(64, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "# of waves";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(96, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 16);
            this.label11.TabIndex = 0;
            this.label11.Text = "# of craft";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(160, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 16);
            this.label12.TabIndex = 0;
            this.label12.Text = "Global Group";
            // 
            // numCraft
            // 
            this.numCraft.Location = new System.Drawing.Point(104, 40);
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
            // numGlobal
            // 
            this.numGlobal.Location = new System.Drawing.Point(176, 40);
            this.numGlobal.Name = "numGlobal";
            this.numGlobal.Size = new System.Drawing.Size(40, 20);
            this.numGlobal.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkRandSC);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.lblNotUsed);
            this.groupBox1.Controls.Add(this.txtSpecCargo);
            this.groupBox1.Controls.Add(this.numSC);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtCargo);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPilot);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(16, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 144);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // chkRandSC
            // 
            this.chkRandSC.Location = new System.Drawing.Point(144, 112);
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
            this.txtName.MaxLength = 12;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(128, 20);
            this.txtName.TabIndex = 4;
            this.txtName.Leave += new System.EventHandler(this.txtName_Leave);
            // 
            // lblNotUsed
            // 
            this.lblNotUsed.Location = new System.Drawing.Point(88, 88);
            this.lblNotUsed.Name = "lblNotUsed";
            this.lblNotUsed.Size = new System.Drawing.Size(80, 16);
            this.lblNotUsed.TabIndex = 3;
            this.lblNotUsed.Text = "(not used)";
            // 
            // txtSpecCargo
            // 
            this.txtSpecCargo.Location = new System.Drawing.Point(88, 88);
            this.txtSpecCargo.MaxLength = 12;
            this.txtSpecCargo.Name = "txtSpecCargo";
            this.txtSpecCargo.Size = new System.Drawing.Size(128, 20);
            this.txtSpecCargo.TabIndex = 7;
            this.txtSpecCargo.Visible = false;
            this.txtSpecCargo.Leave += new System.EventHandler(this.txtSpecCargo_Leave);
            // 
            // numSC
            // 
            this.numSC.Location = new System.Drawing.Point(96, 112);
            this.numSC.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numSC.Name = "numSC";
            this.numSC.Size = new System.Drawing.Size(32, 20);
            this.numSC.TabIndex = 8;
            this.numSC.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numSC.ValueChanged += new System.EventHandler(this.numSC_ValueChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Cargo";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Pilot (unused)";
            // 
            // txtCargo
            // 
            this.txtCargo.Location = new System.Drawing.Point(88, 64);
            this.txtCargo.MaxLength = 12;
            this.txtCargo.Name = "txtCargo";
            this.txtCargo.Size = new System.Drawing.Size(128, 20);
            this.txtCargo.TabIndex = 6;
            this.txtCargo.Leave += new System.EventHandler(this.txtCargo_Leave);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "Special Cargo";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // txtPilot
            // 
            this.txtPilot.Location = new System.Drawing.Point(88, 40);
            this.txtPilot.MaxLength = 12;
            this.txtPilot.Name = "txtPilot";
            this.txtPilot.Size = new System.Drawing.Size(128, 20);
            this.txtPilot.TabIndex = 5;
            this.txtPilot.Leave += new System.EventHandler(this.txtPilot_Leave);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 16);
            this.label6.TabIndex = 3;
            this.label6.Text = "Special Ship #";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStarting
            // 
            this.lblStarting.Location = new System.Drawing.Point(288, 285);
            this.lblStarting.Name = "lblStarting";
            this.lblStarting.Size = new System.Drawing.Size(120, 16);
            this.lblStarting.TabIndex = 11;
            this.lblStarting.Text = "1 Craft at 30 seconds";
            // 
            // tabArrDep
            // 
            this.tabArrDep.Controls.Add(this.cmdCopyAD);
            this.tabArrDep.Controls.Add(this.label21);
            this.tabArrDep.Controls.Add(this.grpDep);
            this.tabArrDep.Controls.Add(this.grpArr);
            this.tabArrDep.Controls.Add(this.cboADTrigAmount);
            this.tabArrDep.Controls.Add(this.cboADTrigType);
            this.tabArrDep.Controls.Add(this.cboADTrigVar);
            this.tabArrDep.Controls.Add(this.cboADTrig);
            this.tabArrDep.Controls.Add(this.label22);
            this.tabArrDep.Controls.Add(this.cboDiff);
            this.tabArrDep.Controls.Add(this.label27);
            this.tabArrDep.Controls.Add(this.label28);
            this.tabArrDep.Controls.Add(this.cmdPasteAD);
            this.tabArrDep.Location = new System.Drawing.Point(4, 22);
            this.tabArrDep.Name = "tabArrDep";
            this.tabArrDep.Size = new System.Drawing.Size(544, 478);
            this.tabArrDep.TabIndex = 1;
            this.tabArrDep.Text = "Arr/Dep";
            // 
            // cmdCopyAD
            // 
            this.cmdCopyAD.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.cmdCopyAD.ImageIndex = 6;
            this.cmdCopyAD.ImageList = this.imgToolbar;
            this.cmdCopyAD.Location = new System.Drawing.Point(72, 360);
            this.cmdCopyAD.Name = "cmdCopyAD";
            this.cmdCopyAD.Size = new System.Drawing.Size(24, 23);
            this.cmdCopyAD.TabIndex = 24;
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
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(264, 360);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(16, 16);
            this.label21.TabIndex = 2;
            this.label21.Text = "of";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpDep
            // 
            this.grpDep.Controls.Add(this.numDepSec);
            this.grpDep.Controls.Add(this.numDepMin);
            this.grpDep.Controls.Add(this.label47);
            this.grpDep.Controls.Add(this.label41);
            this.grpDep.Controls.Add(this.label40);
            this.grpDep.Controls.Add(this.label39);
            this.grpDep.Controls.Add(this.label26);
            this.grpDep.Controls.Add(this.groupBox10);
            this.grpDep.Controls.Add(this.groupBox9);
            this.grpDep.Controls.Add(this.lblDep);
            this.grpDep.Controls.Add(this.cboAbort);
            this.grpDep.Location = new System.Drawing.Point(280, 8);
            this.grpDep.Name = "grpDep";
            this.grpDep.Size = new System.Drawing.Size(256, 328);
            this.grpDep.TabIndex = 1;
            this.grpDep.TabStop = false;
            this.grpDep.Text = "Departure";
            this.grpDep.Leave += new System.EventHandler(this.grpDep_Leave);
            // 
            // numDepSec
            // 
            this.numDepSec.Location = new System.Drawing.Point(176, 280);
            this.numDepSec.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numDepSec.Name = "numDepSec";
            this.numDepSec.Size = new System.Drawing.Size(47, 20);
            this.numDepSec.TabIndex = 17;
            // 
            // numDepMin
            // 
            this.numDepMin.Location = new System.Drawing.Point(104, 280);
            this.numDepMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numDepMin.Name = "numDepMin";
            this.numDepMin.Size = new System.Drawing.Size(47, 20);
            this.numDepMin.TabIndex = 17;
            // 
            // label47
            // 
            this.label47.Location = new System.Drawing.Point(112, 304);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(96, 16);
            this.label47.TabIndex = 8;
            this.label47.Text = "after mission start";
            // 
            // label41
            // 
            this.label41.Location = new System.Drawing.Point(8, 280);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(96, 16);
            this.label41.TabIndex = 7;
            this.label41.Text = "Flight will depart";
            this.label41.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label40
            // 
            this.label40.Location = new System.Drawing.Point(152, 280);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(24, 16);
            this.label40.TabIndex = 6;
            this.label40.Text = "Min";
            this.label40.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label39
            // 
            this.label39.Location = new System.Drawing.Point(224, 280);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(24, 16);
            this.label39.TabIndex = 5;
            this.label39.Text = "Sec";
            this.label39.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(16, 224);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(184, 16);
            this.label26.TabIndex = 3;
            this.label26.Text = "Individual craft abort mission when:";
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
            // lblDep
            // 
            this.lblDep.BackColor = System.Drawing.Color.RosyBrown;
            this.lblDep.Location = new System.Drawing.Point(8, 184);
            this.lblDep.Name = "lblDep";
            this.lblDep.Size = new System.Drawing.Size(240, 32);
            this.lblDep.TabIndex = 2;
            this.lblDep.Text = "(always TRUE)";
            this.lblDep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDep.Click += new System.EventHandler(this.lblADTrigArr_Click);
            this.lblDep.DoubleClick += new System.EventHandler(this.lblADTrigArr_DoubleClick);
            this.lblDep.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblADTrigArr_MouseUp);
            // 
            // cboAbort
            // 
            this.cboAbort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAbort.Location = new System.Drawing.Point(88, 248);
            this.cboAbort.Name = "cboAbort";
            this.cboAbort.Size = new System.Drawing.Size(144, 21);
            this.cboAbort.TabIndex = 16;
            // 
            // grpArr
            // 
            this.grpArr.Controls.Add(this.numArrSec);
            this.grpArr.Controls.Add(this.numArrMin);
            this.grpArr.Controls.Add(this.label23);
            this.grpArr.Controls.Add(this.optArrAND);
            this.grpArr.Controls.Add(this.lblArr1);
            this.grpArr.Controls.Add(this.groupBox8);
            this.grpArr.Controls.Add(this.groupBox7);
            this.grpArr.Controls.Add(this.lblArr2);
            this.grpArr.Controls.Add(this.optArrOR);
            this.grpArr.Controls.Add(this.label24);
            this.grpArr.Controls.Add(this.label25);
            this.grpArr.Location = new System.Drawing.Point(8, 8);
            this.grpArr.Name = "grpArr";
            this.grpArr.Size = new System.Drawing.Size(256, 328);
            this.grpArr.TabIndex = 0;
            this.grpArr.TabStop = false;
            this.grpArr.Text = "Arrival";
            this.grpArr.Leave += new System.EventHandler(this.grpArr_Leave);
            // 
            // numArrSec
            // 
            this.numArrSec.Location = new System.Drawing.Point(168, 288);
            this.numArrSec.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numArrSec.Name = "numArrSec";
            this.numArrSec.Size = new System.Drawing.Size(46, 20);
            this.numArrSec.TabIndex = 16;
            // 
            // numArrMin
            // 
            this.numArrMin.Location = new System.Drawing.Point(72, 288);
            this.numArrMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numArrMin.Name = "numArrMin";
            this.numArrMin.Size = new System.Drawing.Size(46, 20);
            this.numArrMin.TabIndex = 16;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(16, 288);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(40, 16);
            this.label23.TabIndex = 4;
            this.label23.Text = "Delay:";
            this.label23.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // optArrAND
            // 
            this.optArrAND.Location = new System.Drawing.Point(88, 216);
            this.optArrAND.Name = "optArrAND";
            this.optArrAND.Size = new System.Drawing.Size(56, 24);
            this.optArrAND.TabIndex = 12;
            this.optArrAND.Text = "AND";
            // 
            // lblArr1
            // 
            this.lblArr1.BackColor = System.Drawing.Color.RosyBrown;
            this.lblArr1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblArr1.Location = new System.Drawing.Point(8, 184);
            this.lblArr1.Name = "lblArr1";
            this.lblArr1.Size = new System.Drawing.Size(240, 32);
            this.lblArr1.TabIndex = 2;
            this.lblArr1.Text = "(always TRUE)";
            this.lblArr1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblArr1.Click += new System.EventHandler(this.lblADTrigArr_Click);
            this.lblArr1.DoubleClick += new System.EventHandler(this.lblADTrigArr_DoubleClick);
            this.lblArr1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblADTrigArr_MouseUp);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.optArrHypAlt);
            this.groupBox8.Controls.Add(this.cboArrMSAlt);
            this.groupBox8.Controls.Add(this.optArrMSAlt);
            this.groupBox8.Location = new System.Drawing.Point(8, 96);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(240, 72);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Alternative:";
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
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cboArrMS);
            this.groupBox7.Controls.Add(this.optArrHyp);
            this.groupBox7.Controls.Add(this.optArrMS);
            this.groupBox7.Location = new System.Drawing.Point(8, 16);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(240, 72);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Via:";
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
            this.lblArr2.Location = new System.Drawing.Point(8, 240);
            this.lblArr2.Name = "lblArr2";
            this.lblArr2.Size = new System.Drawing.Size(240, 32);
            this.lblArr2.TabIndex = 2;
            this.lblArr2.Text = "(always TRUE)";
            this.lblArr2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblArr2.Click += new System.EventHandler(this.lblADTrigArr_Click);
            this.lblArr2.DoubleClick += new System.EventHandler(this.lblADTrigArr_DoubleClick);
            this.lblArr2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblADTrigArr_MouseUp);
            // 
            // optArrOR
            // 
            this.optArrOR.Checked = true;
            this.optArrOR.Location = new System.Drawing.Point(144, 216);
            this.optArrOR.Name = "optArrOR";
            this.optArrOR.Size = new System.Drawing.Size(56, 24);
            this.optArrOR.TabIndex = 13;
            this.optArrOR.TabStop = true;
            this.optArrOR.Text = "OR";
            this.optArrOR.CheckedChanged += new System.EventHandler(this.optArrOR_CheckedChanged);
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(120, 288);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(24, 16);
            this.label24.TabIndex = 4;
            this.label24.Text = "Min";
            this.label24.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(216, 288);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(24, 16);
            this.label25.TabIndex = 4;
            this.label25.Text = "Sec";
            this.label25.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cboADTrigAmount
            // 
            this.cboADTrigAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboADTrigAmount.Location = new System.Drawing.Point(112, 360);
            this.cboADTrigAmount.Name = "cboADTrigAmount";
            this.cboADTrigAmount.Size = new System.Drawing.Size(144, 21);
            this.cboADTrigAmount.TabIndex = 19;
            this.cboADTrigAmount.Leave += new System.EventHandler(this.cboADTrigAmount_Leave);
            // 
            // cboADTrigType
            // 
            this.cboADTrigType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboADTrigType.Location = new System.Drawing.Point(288, 360);
            this.cboADTrigType.Name = "cboADTrigType";
            this.cboADTrigType.Size = new System.Drawing.Size(160, 21);
            this.cboADTrigType.TabIndex = 20;
            this.cboADTrigType.SelectedIndexChanged += new System.EventHandler(this.cboADTrigType_SelectedIndexChanged);
            // 
            // cboADTrigVar
            // 
            this.cboADTrigVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboADTrigVar.Location = new System.Drawing.Point(112, 392);
            this.cboADTrigVar.Name = "cboADTrigVar";
            this.cboADTrigVar.Size = new System.Drawing.Size(144, 21);
            this.cboADTrigVar.TabIndex = 21;
            this.cboADTrigVar.Leave += new System.EventHandler(this.cboADTrigVar_Leave);
            // 
            // cboADTrig
            // 
            this.cboADTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboADTrig.Location = new System.Drawing.Point(288, 392);
            this.cboADTrig.Name = "cboADTrig";
            this.cboADTrig.Size = new System.Drawing.Size(160, 21);
            this.cboADTrig.TabIndex = 22;
            this.cboADTrig.Leave += new System.EventHandler(this.cboADTrig_Leave);
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(256, 392);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(32, 16);
            this.label22.TabIndex = 2;
            this.label22.Text = "must";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboDiff
            // 
            this.cboDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDiff.Location = new System.Drawing.Point(216, 440);
            this.cboDiff.Name = "cboDiff";
            this.cboDiff.Size = new System.Drawing.Size(112, 21);
            this.cboDiff.TabIndex = 23;
            this.cboDiff.Leave += new System.EventHandler(this.cboDiff_Leave);
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(328, 440);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(48, 16);
            this.label27.TabIndex = 3;
            this.label27.Text = "difficulty";
            this.label27.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(128, 440);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(88, 16);
            this.label28.TabIndex = 3;
            this.label28.Text = "Craft appears in";
            this.label28.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cmdPasteAD
            // 
            this.cmdPasteAD.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.cmdPasteAD.ImageIndex = 7;
            this.cmdPasteAD.ImageList = this.imgToolbar;
            this.cmdPasteAD.Location = new System.Drawing.Point(72, 392);
            this.cmdPasteAD.Name = "cmdPasteAD";
            this.cmdPasteAD.Size = new System.Drawing.Size(24, 23);
            this.cmdPasteAD.TabIndex = 25;
            this.cmdPasteAD.Click += new System.EventHandler(this.cmdPasteAD_Click);
            // 
            // tabGoals
            // 
            this.tabGoals.Controls.Add(this.groupBox17);
            this.tabGoals.Controls.Add(this.groupBox16);
            this.tabGoals.Controls.Add(this.groupBox15);
            this.tabGoals.Controls.Add(this.groupBox14);
            this.tabGoals.Location = new System.Drawing.Point(4, 22);
            this.tabGoals.Name = "tabGoals";
            this.tabGoals.Size = new System.Drawing.Size(544, 478);
            this.tabGoals.TabIndex = 3;
            this.tabGoals.Text = "Goals";
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.label36);
            this.groupBox17.Controls.Add(this.numBonGoalP);
            this.groupBox17.Controls.Add(this.label35);
            this.groupBox17.Controls.Add(this.cboBonGoalA);
            this.groupBox17.Controls.Add(this.cboBonGoalT);
            this.groupBox17.Location = new System.Drawing.Point(8, 336);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(528, 96);
            this.groupBox17.TabIndex = 3;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Bonus Goal";
            // 
            // label36
            // 
            this.label36.Location = new System.Drawing.Point(152, 64);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(88, 16);
            this.label36.TabIndex = 6;
            this.label36.Text = "Points awarded:";
            this.label36.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // numBonGoalP
            // 
            this.numBonGoalP.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numBonGoalP.Location = new System.Drawing.Point(256, 64);
            this.numBonGoalP.Maximum = new decimal(new int[] {
            6350,
            0,
            0,
            0});
            this.numBonGoalP.Minimum = new decimal(new int[] {
            6400,
            0,
            0,
            -2147483648});
            this.numBonGoalP.Name = "numBonGoalP";
            this.numBonGoalP.Size = new System.Drawing.Size(72, 20);
            this.numBonGoalP.TabIndex = 8;
            this.numBonGoalP.Leave += new System.EventHandler(this.numBonGoalP_Leave);
            // 
            // label35
            // 
            this.label35.Location = new System.Drawing.Point(184, 30);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(120, 16);
            this.label35.TabIndex = 4;
            this.label35.Text = "of the flight group must";
            this.label35.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cboBonGoalA
            // 
            this.cboBonGoalA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBonGoalA.Location = new System.Drawing.Point(24, 30);
            this.cboBonGoalA.Name = "cboBonGoalA";
            this.cboBonGoalA.Size = new System.Drawing.Size(144, 21);
            this.cboBonGoalA.TabIndex = 6;
            // 
            // cboBonGoalT
            // 
            this.cboBonGoalT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBonGoalT.Location = new System.Drawing.Point(312, 30);
            this.cboBonGoalT.Name = "cboBonGoalT";
            this.cboBonGoalT.Size = new System.Drawing.Size(184, 21);
            this.cboBonGoalT.TabIndex = 7;
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.label34);
            this.groupBox16.Controls.Add(this.cboSecretGoalA);
            this.groupBox16.Controls.Add(this.cboSecretGoalT);
            this.groupBox16.Location = new System.Drawing.Point(8, 232);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(528, 72);
            this.groupBox16.TabIndex = 2;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Secret Goal";
            // 
            // label34
            // 
            this.label34.Location = new System.Drawing.Point(184, 32);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(120, 16);
            this.label34.TabIndex = 4;
            this.label34.Text = "of the flight group must";
            this.label34.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cboSecretGoalA
            // 
            this.cboSecretGoalA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSecretGoalA.Location = new System.Drawing.Point(24, 32);
            this.cboSecretGoalA.Name = "cboSecretGoalA";
            this.cboSecretGoalA.Size = new System.Drawing.Size(144, 21);
            this.cboSecretGoalA.TabIndex = 4;
            // 
            // cboSecretGoalT
            // 
            this.cboSecretGoalT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSecretGoalT.Location = new System.Drawing.Point(312, 32);
            this.cboSecretGoalT.Name = "cboSecretGoalT";
            this.cboSecretGoalT.Size = new System.Drawing.Size(184, 21);
            this.cboSecretGoalT.TabIndex = 5;
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.label33);
            this.groupBox15.Controls.Add(this.cboSecGoalA);
            this.groupBox15.Controls.Add(this.cboSecGoalT);
            this.groupBox15.Location = new System.Drawing.Point(8, 128);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(528, 72);
            this.groupBox15.TabIndex = 1;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Secondary Goal";
            // 
            // label33
            // 
            this.label33.Location = new System.Drawing.Point(184, 32);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(120, 16);
            this.label33.TabIndex = 4;
            this.label33.Text = "of the flight group must";
            this.label33.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cboSecGoalA
            // 
            this.cboSecGoalA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSecGoalA.Location = new System.Drawing.Point(24, 32);
            this.cboSecGoalA.Name = "cboSecGoalA";
            this.cboSecGoalA.Size = new System.Drawing.Size(144, 21);
            this.cboSecGoalA.TabIndex = 2;
            // 
            // cboSecGoalT
            // 
            this.cboSecGoalT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSecGoalT.Location = new System.Drawing.Point(312, 32);
            this.cboSecGoalT.Name = "cboSecGoalT";
            this.cboSecGoalT.Size = new System.Drawing.Size(184, 21);
            this.cboSecGoalT.TabIndex = 3;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.label32);
            this.groupBox14.Controls.Add(this.cboPrimGoalA);
            this.groupBox14.Controls.Add(this.cboPrimGoalT);
            this.groupBox14.Location = new System.Drawing.Point(8, 24);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(528, 72);
            this.groupBox14.TabIndex = 0;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Primary Goal";
            // 
            // label32
            // 
            this.label32.Location = new System.Drawing.Point(184, 32);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(120, 16);
            this.label32.TabIndex = 1;
            this.label32.Text = "of the flight group must";
            this.label32.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cboPrimGoalA
            // 
            this.cboPrimGoalA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrimGoalA.Location = new System.Drawing.Point(24, 32);
            this.cboPrimGoalA.Name = "cboPrimGoalA";
            this.cboPrimGoalA.Size = new System.Drawing.Size(144, 21);
            this.cboPrimGoalA.TabIndex = 0;
            // 
            // cboPrimGoalT
            // 
            this.cboPrimGoalT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrimGoalT.Location = new System.Drawing.Point(312, 32);
            this.cboPrimGoalT.Name = "cboPrimGoalT";
            this.cboPrimGoalT.Size = new System.Drawing.Size(184, 21);
            this.cboPrimGoalT.TabIndex = 1;
            // 
            // tabWP
            // 
            this.tabWP.Controls.Add(this.label80);
            this.tabWP.Controls.Add(this.label76);
            this.tabWP.Controls.Add(this.numRoll);
            this.tabWP.Controls.Add(this.numPitch);
            this.tabWP.Controls.Add(this.numYaw);
            this.tabWP.Controls.Add(this.label56);
            this.tabWP.Controls.Add(this.dataWP);
            this.tabWP.Controls.Add(this.dataWP_Raw);
            this.tabWP.Controls.Add(this.chkWPBrief);
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
            this.tabWP.Location = new System.Drawing.Point(4, 22);
            this.tabWP.Name = "tabWP";
            this.tabWP.Size = new System.Drawing.Size(544, 478);
            this.tabWP.TabIndex = 4;
            this.tabWP.Text = "Waypoints";
            // 
            // label80
            // 
            this.label80.Location = new System.Drawing.Point(342, 408);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(178, 28);
            this.label80.TabIndex = 19;
            this.label80.Text = "Note: interface displays corrected pitch angle";
            // 
            // label76
            // 
            this.label76.Image = ((System.Drawing.Image)(resources.GetObject("label76.Image")));
            this.label76.Location = new System.Drawing.Point(248, 376);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(20, 20);
            this.label76.TabIndex = 6;
            // 
            // numRoll
            // 
            this.numRoll.Location = new System.Drawing.Point(288, 440);
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
            this.numRoll.TabIndex = 18;
            this.numRoll.Leave += new System.EventHandler(this.numRoll_Leave);
            // 
            // numPitch
            // 
            this.numPitch.Location = new System.Drawing.Point(288, 408);
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
            this.numPitch.TabIndex = 17;
            this.numPitch.Leave += new System.EventHandler(this.numPitch_Leave);
            // 
            // numYaw
            // 
            this.numYaw.Location = new System.Drawing.Point(288, 376);
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
            this.numYaw.TabIndex = 16;
            this.numYaw.Leave += new System.EventHandler(this.numYaw_Leave);
            // 
            // label56
            // 
            this.label56.Location = new System.Drawing.Point(368, 16);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(144, 16);
            this.label56.TabIndex = 1;
            this.label56.Text = "Raw Data - position*160";
            // 
            // dataWP
            // 
            this.dataWP.AllowSorting = false;
            this.dataWP.CaptionVisible = false;
            this.dataWP.DataMember = "";
            this.dataWP.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataWP.Location = new System.Drawing.Point(24, 40);
            this.dataWP.Name = "dataWP";
            this.dataWP.PreferredColumnWidth = 52;
            this.dataWP.PreferredRowHeight = 20;
            this.dataWP.RowHeadersVisible = false;
            this.dataWP.Size = new System.Drawing.Size(160, 323);
            this.dataWP.TabIndex = 0;
            // 
            // dataWP_Raw
            // 
            this.dataWP_Raw.AllowSorting = false;
            this.dataWP_Raw.CaptionVisible = false;
            this.dataWP_Raw.DataMember = "";
            this.dataWP_Raw.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataWP_Raw.Location = new System.Drawing.Point(360, 40);
            this.dataWP_Raw.Name = "dataWP_Raw";
            this.dataWP_Raw.PreferredColumnWidth = 52;
            this.dataWP_Raw.PreferredRowHeight = 20;
            this.dataWP_Raw.RowHeadersVisible = false;
            this.dataWP_Raw.Size = new System.Drawing.Size(160, 323);
            this.dataWP_Raw.TabIndex = 0;
            // 
            // chkWPBrief
            // 
            this.chkWPBrief.Location = new System.Drawing.Point(192, 344);
            this.chkWPBrief.Name = "chkWPBrief";
            this.chkWPBrief.Size = new System.Drawing.Size(96, 16);
            this.chkWPBrief.TabIndex = 15;
            this.chkWPBrief.Text = "Briefing";
            // 
            // chkWPHyp
            // 
            this.chkWPHyp.Location = new System.Drawing.Point(192, 324);
            this.chkWPHyp.Name = "chkWPHyp";
            this.chkWPHyp.Size = new System.Drawing.Size(96, 16);
            this.chkWPHyp.TabIndex = 14;
            this.chkWPHyp.Text = "Hyperspace";
            // 
            // chkWP8
            // 
            this.chkWP8.Location = new System.Drawing.Point(192, 284);
            this.chkWP8.Name = "chkWP8";
            this.chkWP8.Size = new System.Drawing.Size(96, 16);
            this.chkWP8.TabIndex = 12;
            this.chkWP8.Text = "Waypoint 8";
            // 
            // chkWP7
            // 
            this.chkWP7.Location = new System.Drawing.Point(192, 264);
            this.chkWP7.Name = "chkWP7";
            this.chkWP7.Size = new System.Drawing.Size(96, 16);
            this.chkWP7.TabIndex = 11;
            this.chkWP7.Text = "Waypoint 7";
            // 
            // chkWP2
            // 
            this.chkWP2.Location = new System.Drawing.Point(192, 164);
            this.chkWP2.Name = "chkWP2";
            this.chkWP2.Size = new System.Drawing.Size(96, 16);
            this.chkWP2.TabIndex = 6;
            this.chkWP2.Text = "Waypoint 2";
            // 
            // chkWP1
            // 
            this.chkWP1.Location = new System.Drawing.Point(192, 144);
            this.chkWP1.Name = "chkWP1";
            this.chkWP1.Size = new System.Drawing.Size(96, 16);
            this.chkWP1.TabIndex = 5;
            this.chkWP1.Text = "Waypoint 1";
            // 
            // chkSP4
            // 
            this.chkSP4.Location = new System.Drawing.Point(192, 124);
            this.chkSP4.Name = "chkSP4";
            this.chkSP4.Size = new System.Drawing.Size(96, 16);
            this.chkSP4.TabIndex = 4;
            this.chkSP4.Text = "Start Point4";
            // 
            // chkSP3
            // 
            this.chkSP3.Location = new System.Drawing.Point(192, 104);
            this.chkSP3.Name = "chkSP3";
            this.chkSP3.Size = new System.Drawing.Size(96, 16);
            this.chkSP3.TabIndex = 3;
            this.chkSP3.Text = "Start Point3";
            // 
            // chkSP2
            // 
            this.chkSP2.Location = new System.Drawing.Point(192, 84);
            this.chkSP2.Name = "chkSP2";
            this.chkSP2.Size = new System.Drawing.Size(96, 16);
            this.chkSP2.TabIndex = 2;
            this.chkSP2.Text = "Start Point2";
            // 
            // chkSP1
            // 
            this.chkSP1.Location = new System.Drawing.Point(192, 64);
            this.chkSP1.Name = "chkSP1";
            this.chkSP1.Size = new System.Drawing.Size(96, 16);
            this.chkSP1.TabIndex = 1;
            this.chkSP1.Text = "Start Point 1";
            // 
            // chkWP6
            // 
            this.chkWP6.Location = new System.Drawing.Point(192, 244);
            this.chkWP6.Name = "chkWP6";
            this.chkWP6.Size = new System.Drawing.Size(96, 16);
            this.chkWP6.TabIndex = 10;
            this.chkWP6.Text = "Waypoint 6";
            // 
            // chkWP5
            // 
            this.chkWP5.Location = new System.Drawing.Point(192, 224);
            this.chkWP5.Name = "chkWP5";
            this.chkWP5.Size = new System.Drawing.Size(96, 16);
            this.chkWP5.TabIndex = 9;
            this.chkWP5.Text = "Waypoint 5";
            // 
            // chkWP4
            // 
            this.chkWP4.Location = new System.Drawing.Point(192, 204);
            this.chkWP4.Name = "chkWP4";
            this.chkWP4.Size = new System.Drawing.Size(96, 16);
            this.chkWP4.TabIndex = 8;
            this.chkWP4.Text = "Waypoint 4";
            // 
            // chkWP3
            // 
            this.chkWP3.Location = new System.Drawing.Point(192, 184);
            this.chkWP3.Name = "chkWP3";
            this.chkWP3.Size = new System.Drawing.Size(96, 16);
            this.chkWP3.TabIndex = 7;
            this.chkWP3.Text = "Waypoint 3";
            // 
            // chkWPRend
            // 
            this.chkWPRend.Location = new System.Drawing.Point(192, 304);
            this.chkWPRend.Name = "chkWPRend";
            this.chkWPRend.Size = new System.Drawing.Size(96, 16);
            this.chkWPRend.TabIndex = 13;
            this.chkWPRend.Text = "Rendezvous";
            // 
            // label77
            // 
            this.label77.Image = ((System.Drawing.Image)(resources.GetObject("label77.Image")));
            this.label77.Location = new System.Drawing.Point(248, 408);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(20, 20);
            this.label77.TabIndex = 6;
            // 
            // label78
            // 
            this.label78.Image = ((System.Drawing.Image)(resources.GetObject("label78.Image")));
            this.label78.Location = new System.Drawing.Point(248, 440);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(20, 20);
            this.label78.TabIndex = 6;
            // 
            // tabOrders
            // 
            this.tabOrders.Controls.Add(this.lblOVar2Note);
            this.tabOrders.Controls.Add(this.lblOVar1Note);
            this.tabOrders.Controls.Add(this.cmdCopyOrder);
            this.tabOrders.Controls.Add(this.lblODesc);
            this.tabOrders.Controls.Add(this.grpSecOrder);
            this.tabOrders.Controls.Add(this.grpPrimOrder);
            this.tabOrders.Controls.Add(this.numOVar1);
            this.tabOrders.Controls.Add(this.lblOVar1);
            this.tabOrders.Controls.Add(this.cboOThrottle);
            this.tabOrders.Controls.Add(this.label29);
            this.tabOrders.Controls.Add(this.groupBox11);
            this.tabOrders.Controls.Add(this.cboOrders);
            this.tabOrders.Controls.Add(this.lblOVar2);
            this.tabOrders.Controls.Add(this.numOVar2);
            this.tabOrders.Controls.Add(this.cmdPasteOrder);
            this.tabOrders.Location = new System.Drawing.Point(4, 22);
            this.tabOrders.Name = "tabOrders";
            this.tabOrders.Size = new System.Drawing.Size(544, 478);
            this.tabOrders.TabIndex = 2;
            this.tabOrders.Text = "Orders";
            // 
            // lblOVar2Note
            // 
            this.lblOVar2Note.Location = new System.Drawing.Point(423, 186);
            this.lblOVar2Note.Name = "lblOVar2Note";
            this.lblOVar2Note.Size = new System.Drawing.Size(113, 16);
            this.lblOVar2Note.TabIndex = 18;
            this.lblOVar2Note.Text = "lblOVar2Note";
            // 
            // lblOVar1Note
            // 
            this.lblOVar1Note.Location = new System.Drawing.Point(297, 186);
            this.lblOVar1Note.Name = "lblOVar1Note";
            this.lblOVar1Note.Size = new System.Drawing.Size(120, 16);
            this.lblOVar1Note.TabIndex = 18;
            this.lblOVar1Note.Text = "lblOVar1Note";
            // 
            // cmdCopyOrder
            // 
            this.cmdCopyOrder.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.cmdCopyOrder.ImageIndex = 6;
            this.cmdCopyOrder.ImageList = this.imgToolbar;
            this.cmdCopyOrder.Location = new System.Drawing.Point(112, 8);
            this.cmdCopyOrder.Name = "cmdCopyOrder";
            this.cmdCopyOrder.Size = new System.Drawing.Size(24, 23);
            this.cmdCopyOrder.TabIndex = 16;
            this.cmdCopyOrder.Click += new System.EventHandler(this.cmdCopyOrder_Click);
            // 
            // lblODesc
            // 
            this.lblODesc.Location = new System.Drawing.Point(16, 40);
            this.lblODesc.Name = "lblODesc";
            this.lblODesc.Size = new System.Drawing.Size(512, 16);
            this.lblODesc.TabIndex = 8;
            // 
            // grpSecOrder
            // 
            this.grpSecOrder.Controls.Add(this.label31);
            this.grpSecOrder.Controls.Add(this.optOT3T4OR);
            this.grpSecOrder.Controls.Add(this.cboOT3);
            this.grpSecOrder.Controls.Add(this.cboOT3Type);
            this.grpSecOrder.Controls.Add(this.cboOT4Type);
            this.grpSecOrder.Controls.Add(this.cboOT4);
            this.grpSecOrder.Controls.Add(this.optOT3T4AND);
            this.grpSecOrder.Location = new System.Drawing.Point(16, 328);
            this.grpSecOrder.Name = "grpSecOrder";
            this.grpSecOrder.Size = new System.Drawing.Size(512, 112);
            this.grpSecOrder.TabIndex = 7;
            this.grpSecOrder.TabStop = false;
            this.grpSecOrder.Text = "Secondary Target";
            // 
            // label31
            // 
            this.label31.Location = new System.Drawing.Point(16, 40);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(96, 48);
            this.label31.TabIndex = 3;
            this.label31.Text = "Selecting \"OR\" allows for multiple targets";
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
            this.grpPrimOrder.Controls.Add(this.label30);
            this.grpPrimOrder.Controls.Add(this.optOT1T2OR);
            this.grpPrimOrder.Controls.Add(this.cboOT1);
            this.grpPrimOrder.Controls.Add(this.cboOT1Type);
            this.grpPrimOrder.Controls.Add(this.cboOT2Type);
            this.grpPrimOrder.Controls.Add(this.cboOT2);
            this.grpPrimOrder.Controls.Add(this.optOT1T2AND);
            this.grpPrimOrder.Location = new System.Drawing.Point(16, 200);
            this.grpPrimOrder.Name = "grpPrimOrder";
            this.grpPrimOrder.Size = new System.Drawing.Size(512, 112);
            this.grpPrimOrder.TabIndex = 6;
            this.grpPrimOrder.TabStop = false;
            this.grpPrimOrder.Text = "Primary Target";
            // 
            // label30
            // 
            this.label30.Location = new System.Drawing.Point(16, 40);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(96, 56);
            this.label30.TabIndex = 2;
            this.label30.Text = "Selecting \"AND\" will require that the target meet both settings";
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
            this.numOVar1.Location = new System.Drawing.Point(320, 162);
            this.numOVar1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numOVar1.Name = "numOVar1";
            this.numOVar1.Size = new System.Drawing.Size(46, 20);
            this.numOVar1.TabIndex = 2;
            this.numOVar1.ValueChanged += new System.EventHandler(this.numOVar1_ValueChanged);
            // 
            // lblOVar1
            // 
            this.lblOVar1.Location = new System.Drawing.Point(192, 164);
            this.lblOVar1.Name = "lblOVar1";
            this.lblOVar1.Size = new System.Drawing.Size(120, 16);
            this.lblOVar1.TabIndex = 4;
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
            this.cboOThrottle.Location = new System.Drawing.Point(136, 162);
            this.cboOThrottle.Name = "cboOThrottle";
            this.cboOThrottle.Size = new System.Drawing.Size(48, 21);
            this.cboOThrottle.TabIndex = 1;
            this.cboOThrottle.Leave += new System.EventHandler(this.cboOThrottle_Leave);
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(16, 164);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(120, 16);
            this.label29.TabIndex = 2;
            this.label29.Text = "Percent of Full Speed:";
            this.label29.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.lblOrder1);
            this.groupBox11.Controls.Add(this.lblOrder2);
            this.groupBox11.Controls.Add(this.lblOrder3);
            this.groupBox11.Location = new System.Drawing.Point(8, 56);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(528, 96);
            this.groupBox11.TabIndex = 1;
            this.groupBox11.TabStop = false;
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
            // cboOrders
            // 
            this.cboOrders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOrders.Location = new System.Drawing.Point(176, 8);
            this.cboOrders.Name = "cboOrders";
            this.cboOrders.Size = new System.Drawing.Size(192, 21);
            this.cboOrders.TabIndex = 0;
            this.cboOrders.SelectedIndexChanged += new System.EventHandler(this.cboOrders_SelectedIndexChanged);
            // 
            // lblOVar2
            // 
            this.lblOVar2.Location = new System.Drawing.Point(368, 164);
            this.lblOVar2.Name = "lblOVar2";
            this.lblOVar2.Size = new System.Drawing.Size(112, 16);
            this.lblOVar2.TabIndex = 4;
            this.lblOVar2.Text = "lblOVar2";
            this.lblOVar2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // numOVar2
            // 
            this.numOVar2.Location = new System.Drawing.Point(488, 162);
            this.numOVar2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numOVar2.Name = "numOVar2";
            this.numOVar2.Size = new System.Drawing.Size(40, 20);
            this.numOVar2.TabIndex = 3;
            this.numOVar2.ValueChanged += new System.EventHandler(this.numOVar2_ValueChanged);
            // 
            // cmdPasteOrder
            // 
            this.cmdPasteOrder.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.cmdPasteOrder.ImageIndex = 7;
            this.cmdPasteOrder.ImageList = this.imgToolbar;
            this.cmdPasteOrder.Location = new System.Drawing.Point(144, 8);
            this.cmdPasteOrder.Name = "cmdPasteOrder";
            this.cmdPasteOrder.Size = new System.Drawing.Size(24, 23);
            this.cmdPasteOrder.TabIndex = 17;
            this.cmdPasteOrder.Click += new System.EventHandler(this.cmdPasteOrder_Click);
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.grpPermaDeath);
            this.tabOptions.Location = new System.Drawing.Point(4, 22);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new System.Drawing.Size(544, 478);
            this.tabOptions.TabIndex = 6;
            this.tabOptions.Text = "Options";
            this.tabOptions.UseVisualStyleBackColor = true;
            // 
            // grpPermaDeath
            // 
            this.grpPermaDeath.Controls.Add(this.chkPermaDeath);
            this.grpPermaDeath.Controls.Add(this.label82);
            this.grpPermaDeath.Controls.Add(this.numPermaDeathID);
            this.grpPermaDeath.Controls.Add(this.label19);
            this.grpPermaDeath.Location = new System.Drawing.Point(14, 23);
            this.grpPermaDeath.Name = "grpPermaDeath";
            this.grpPermaDeath.Size = new System.Drawing.Size(517, 190);
            this.grpPermaDeath.TabIndex = 0;
            this.grpPermaDeath.TabStop = false;
            this.grpPermaDeath.Text = "Permanent Death Throughout Campaign";
            this.grpPermaDeath.Leave += new System.EventHandler(this.grpPermaDeath_Leave);
            // 
            // chkPermaDeath
            // 
            this.chkPermaDeath.AutoSize = true;
            this.chkPermaDeath.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPermaDeath.Location = new System.Drawing.Point(74, 111);
            this.chkPermaDeath.Name = "chkPermaDeath";
            this.chkPermaDeath.Size = new System.Drawing.Size(65, 17);
            this.chkPermaDeath.TabIndex = 13;
            this.chkPermaDeath.Text = "Enabled";
            this.chkPermaDeath.UseVisualStyleBackColor = true;
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Location = new System.Drawing.Point(61, 136);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(56, 13);
            this.label82.TabIndex = 12;
            this.label82.Text = "ID number";
            // 
            // numPermaDeathID
            // 
            this.numPermaDeathID.Location = new System.Drawing.Point(123, 134);
            this.numPermaDeathID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numPermaDeathID.Name = "numPermaDeathID";
            this.numPermaDeathID.Size = new System.Drawing.Size(48, 20);
            this.numPermaDeathID.TabIndex = 10;
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(6, 25);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(486, 56);
            this.label19.TabIndex = 0;
            this.label19.Text = resources.GetString("label19.Text");
            // 
            // tabUnk
            // 
            this.tabUnk.Controls.Add(this.label84);
            this.tabUnk.Controls.Add(this.label83);
            this.tabUnk.Controls.Add(this.chkUnk21);
            this.tabUnk.Controls.Add(this.chkUnk19);
            this.tabUnk.Controls.Add(this.numUnk20);
            this.tabUnk.Controls.Add(this.numUnk17);
            this.tabUnk.Controls.Add(this.numUnk16);
            this.tabUnk.Controls.Add(this.numUnk15);
            this.tabUnk.Controls.Add(this.numUnk12);
            this.tabUnk.Controls.Add(this.numUnk11);
            this.tabUnk.Controls.Add(this.numUnk5);
            this.tabUnk.Controls.Add(this.numUnk1);
            this.tabUnk.Controls.Add(this.label38);
            this.tabUnk.Controls.Add(this.label37);
            this.tabUnk.Controls.Add(this.label42);
            this.tabUnk.Controls.Add(this.label43);
            this.tabUnk.Controls.Add(this.label44);
            this.tabUnk.Controls.Add(this.label45);
            this.tabUnk.Controls.Add(this.label46);
            this.tabUnk.Controls.Add(this.label49);
            this.tabUnk.Controls.Add(this.label50);
            this.tabUnk.Controls.Add(this.label81);
            this.tabUnk.Controls.Add(this.label51);
            this.tabUnk.Controls.Add(this.label68);
            this.tabUnk.Controls.Add(this.label69);
            this.tabUnk.Controls.Add(this.label70);
            this.tabUnk.Controls.Add(this.label71);
            this.tabUnk.Controls.Add(this.label72);
            this.tabUnk.Controls.Add(this.label73);
            this.tabUnk.Controls.Add(this.label74);
            this.tabUnk.Controls.Add(this.label75);
            this.tabUnk.Location = new System.Drawing.Point(4, 22);
            this.tabUnk.Name = "tabUnk";
            this.tabUnk.Size = new System.Drawing.Size(544, 478);
            this.tabUnk.TabIndex = 5;
            this.tabUnk.Text = "Unknowns";
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.Location = new System.Drawing.Point(141, 392);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(81, 13);
            this.label84.TabIndex = 12;
            this.label84.Text = "Perma-death ID";
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(141, 360);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(109, 13);
            this.label83.TabIndex = 11;
            this.label83.Text = "Perma-death Enabled";
            // 
            // chkUnk21
            // 
            this.chkUnk21.AutoSize = true;
            this.chkUnk21.Location = new System.Drawing.Point(269, 391);
            this.chkUnk21.Name = "chkUnk21";
            this.chkUnk21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkUnk21.Size = new System.Drawing.Size(105, 17);
            this.chkUnk21.TabIndex = 10;
            this.chkUnk21.Text = "      Unknown 21";
            this.chkUnk21.UseVisualStyleBackColor = true;
            this.chkUnk21.Leave += new System.EventHandler(this.chkUnk21_Leave);
            // 
            // chkUnk19
            // 
            this.chkUnk19.AutoSize = true;
            this.chkUnk19.Location = new System.Drawing.Point(269, 327);
            this.chkUnk19.Name = "chkUnk19";
            this.chkUnk19.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkUnk19.Size = new System.Drawing.Size(105, 17);
            this.chkUnk19.TabIndex = 10;
            this.chkUnk19.Text = "      Unknown 19";
            this.chkUnk19.UseVisualStyleBackColor = true;
            this.chkUnk19.Leave += new System.EventHandler(this.chkUnk19_Leave);
            // 
            // numUnk20
            // 
            this.numUnk20.Location = new System.Drawing.Point(360, 358);
            this.numUnk20.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUnk20.Name = "numUnk20";
            this.numUnk20.Size = new System.Drawing.Size(48, 20);
            this.numUnk20.TabIndex = 9;
            // 
            // numUnk17
            // 
            this.numUnk17.Location = new System.Drawing.Point(360, 294);
            this.numUnk17.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUnk17.Name = "numUnk17";
            this.numUnk17.Size = new System.Drawing.Size(48, 20);
            this.numUnk17.TabIndex = 9;
            // 
            // numUnk16
            // 
            this.numUnk16.Location = new System.Drawing.Point(360, 262);
            this.numUnk16.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUnk16.Name = "numUnk16";
            this.numUnk16.Size = new System.Drawing.Size(48, 20);
            this.numUnk16.TabIndex = 9;
            // 
            // numUnk15
            // 
            this.numUnk15.Location = new System.Drawing.Point(360, 230);
            this.numUnk15.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUnk15.Name = "numUnk15";
            this.numUnk15.Size = new System.Drawing.Size(48, 20);
            this.numUnk15.TabIndex = 9;
            // 
            // numUnk12
            // 
            this.numUnk12.Location = new System.Drawing.Point(360, 134);
            this.numUnk12.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUnk12.Name = "numUnk12";
            this.numUnk12.Size = new System.Drawing.Size(48, 20);
            this.numUnk12.TabIndex = 9;
            // 
            // numUnk11
            // 
            this.numUnk11.Location = new System.Drawing.Point(360, 102);
            this.numUnk11.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUnk11.Name = "numUnk11";
            this.numUnk11.Size = new System.Drawing.Size(48, 20);
            this.numUnk11.TabIndex = 9;
            // 
            // numUnk5
            // 
            this.numUnk5.Location = new System.Drawing.Point(144, 230);
            this.numUnk5.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUnk5.Name = "numUnk5";
            this.numUnk5.Size = new System.Drawing.Size(48, 20);
            this.numUnk5.TabIndex = 9;
            // 
            // numUnk1
            // 
            this.numUnk1.Location = new System.Drawing.Point(144, 102);
            this.numUnk1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUnk1.Name = "numUnk1";
            this.numUnk1.Size = new System.Drawing.Size(48, 20);
            this.numUnk1.TabIndex = 9;
            // 
            // label38
            // 
            this.label38.Location = new System.Drawing.Point(56, 104);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(72, 16);
            this.label38.TabIndex = 1;
            this.label38.Text = "Unknown 1";
            // 
            // label37
            // 
            this.label37.Location = new System.Drawing.Point(16, 24);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(512, 49);
            this.label37.TabIndex = 0;
            this.label37.Text = resources.GetString("label37.Text");
            // 
            // label42
            // 
            this.label42.Location = new System.Drawing.Point(56, 232);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(72, 16);
            this.label42.TabIndex = 1;
            this.label42.Text = "Unknown 5";
            // 
            // label43
            // 
            this.label43.Location = new System.Drawing.Point(56, 360);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(72, 16);
            this.label43.TabIndex = 1;
            this.label43.Text = "Unknown 9";
            // 
            // label44
            // 
            this.label44.Location = new System.Drawing.Point(56, 392);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(72, 16);
            this.label44.TabIndex = 1;
            this.label44.Text = "Unknown 10";
            // 
            // label45
            // 
            this.label45.Location = new System.Drawing.Point(272, 104);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(72, 16);
            this.label45.TabIndex = 1;
            this.label45.Text = "Unknown 11";
            // 
            // label46
            // 
            this.label46.Location = new System.Drawing.Point(272, 136);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(72, 16);
            this.label46.TabIndex = 1;
            this.label46.Text = "Unknown 12";
            // 
            // label49
            // 
            this.label49.Location = new System.Drawing.Point(272, 232);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(72, 16);
            this.label49.TabIndex = 1;
            this.label49.Text = "Unknown 15";
            // 
            // label50
            // 
            this.label50.Location = new System.Drawing.Point(272, 264);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(72, 16);
            this.label50.TabIndex = 1;
            this.label50.Text = "Unknown 16";
            // 
            // label81
            // 
            this.label81.Location = new System.Drawing.Point(272, 360);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(72, 16);
            this.label81.TabIndex = 1;
            this.label81.Text = "Unknown 20";
            // 
            // label51
            // 
            this.label51.Location = new System.Drawing.Point(272, 296);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(72, 16);
            this.label51.TabIndex = 1;
            this.label51.Text = "Unknown 17";
            // 
            // label68
            // 
            this.label68.Location = new System.Drawing.Point(56, 136);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(192, 16);
            this.label68.TabIndex = 1;
            this.label68.Text = "Unknown 2          Formation Distance";
            // 
            // label69
            // 
            this.label69.Location = new System.Drawing.Point(56, 168);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(192, 16);
            this.label69.TabIndex = 1;
            this.label69.Text = "Unknown 3          Global Group";
            // 
            // label70
            // 
            this.label70.Location = new System.Drawing.Point(56, 200);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(192, 16);
            this.label70.TabIndex = 1;
            this.label70.Text = "Unknown 4          Form Leader Dist";
            // 
            // label71
            // 
            this.label71.Location = new System.Drawing.Point(56, 264);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(192, 16);
            this.label71.TabIndex = 1;
            this.label71.Text = "Unknown 6          Object Z Rotation";
            // 
            // label72
            // 
            this.label72.Location = new System.Drawing.Point(56, 296);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(192, 16);
            this.label72.TabIndex = 1;
            this.label72.Text = "Unknown 7          Object Y Rotation";
            // 
            // label73
            // 
            this.label73.Location = new System.Drawing.Point(56, 328);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(192, 16);
            this.label73.TabIndex = 1;
            this.label73.Text = "Unknown 8          Object Z Rotation II";
            // 
            // label74
            // 
            this.label74.Location = new System.Drawing.Point(272, 168);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(192, 16);
            this.label74.TabIndex = 1;
            this.label74.Text = "Unknown 13        Dep Timer: Min";
            // 
            // label75
            // 
            this.label75.Location = new System.Drawing.Point(272, 200);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(192, 16);
            this.label75.TabIndex = 1;
            this.label75.Text = "Unknown 14        Dep Timer: Sec";
            // 
            // tabMess
            // 
            this.tabMess.Controls.Add(this.cmdMoveMessDown);
            this.tabMess.Controls.Add(this.cmdMoveMessUp);
            this.tabMess.Controls.Add(this.lblMessage);
            this.tabMess.Controls.Add(this.numMessDelay);
            this.tabMess.Controls.Add(this.label55);
            this.tabMess.Controls.Add(this.label57);
            this.tabMess.Controls.Add(this.cboMessAmount);
            this.tabMess.Controls.Add(this.cboMessType);
            this.tabMess.Controls.Add(this.cboMessVar);
            this.tabMess.Controls.Add(this.cboMessTrig);
            this.tabMess.Controls.Add(this.label58);
            this.tabMess.Controls.Add(this.grpMessages);
            this.tabMess.Controls.Add(this.label54);
            this.tabMess.Controls.Add(this.cboMessColor);
            this.tabMess.Controls.Add(this.txtMessage);
            this.tabMess.Controls.Add(this.label52);
            this.tabMess.Controls.Add(this.lstMessages);
            this.tabMess.Controls.Add(this.label53);
            this.tabMess.Controls.Add(this.txtShort);
            this.tabMess.Location = new System.Drawing.Point(4, 22);
            this.tabMess.Name = "tabMess";
            this.tabMess.Size = new System.Drawing.Size(785, 510);
            this.tabMess.TabIndex = 1;
            this.tabMess.Text = "Messages";
            // 
            // cmdMoveMessDown
            // 
            this.cmdMoveMessDown.Location = new System.Drawing.Point(574, 11);
            this.cmdMoveMessDown.Name = "cmdMoveMessDown";
            this.cmdMoveMessDown.Size = new System.Drawing.Size(75, 23);
            this.cmdMoveMessDown.TabIndex = 21;
            this.cmdMoveMessDown.Text = "Move Down";
            this.cmdMoveMessDown.UseVisualStyleBackColor = true;
            this.cmdMoveMessDown.Click += new System.EventHandler(this.cmdMoveMessDown_Click);
            // 
            // cmdMoveMessUp
            // 
            this.cmdMoveMessUp.Location = new System.Drawing.Point(493, 11);
            this.cmdMoveMessUp.Name = "cmdMoveMessUp";
            this.cmdMoveMessUp.Size = new System.Drawing.Size(75, 23);
            this.cmdMoveMessUp.TabIndex = 20;
            this.cmdMoveMessUp.Text = "Move Up";
            this.cmdMoveMessUp.UseVisualStyleBackColor = true;
            this.cmdMoveMessUp.Click += new System.EventHandler(this.cmdMoveMessUp_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(344, 16);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(120, 16);
            this.lblMessage.TabIndex = 19;
            this.lblMessage.Text = "Message #0 of 0";
            // 
            // numMessDelay
            // 
            this.numMessDelay.Enabled = false;
            this.numMessDelay.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMessDelay.Location = new System.Drawing.Point(520, 432);
            this.numMessDelay.Maximum = new decimal(new int[] {
            1275,
            0,
            0,
            0});
            this.numMessDelay.Name = "numMessDelay";
            this.numMessDelay.Size = new System.Drawing.Size(48, 20);
            this.numMessDelay.TabIndex = 9;
            this.numMessDelay.Leave += new System.EventHandler(this.numMessDelay_Leave);
            // 
            // label55
            // 
            this.label55.Location = new System.Drawing.Point(480, 432);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(176, 16);
            this.label55.TabIndex = 17;
            this.label55.Text = "Delay:                   seconds";
            this.label55.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label57
            // 
            this.label57.Location = new System.Drawing.Point(544, 336);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(16, 16);
            this.label57.TabIndex = 15;
            this.label57.Text = "of";
            this.label57.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboMessAmount
            // 
            this.cboMessAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessAmount.Enabled = false;
            this.cboMessAmount.Location = new System.Drawing.Point(392, 336);
            this.cboMessAmount.Name = "cboMessAmount";
            this.cboMessAmount.Size = new System.Drawing.Size(144, 21);
            this.cboMessAmount.TabIndex = 5;
            this.cboMessAmount.Leave += new System.EventHandler(this.cboMessAmount_Leave);
            // 
            // cboMessType
            // 
            this.cboMessType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessType.Enabled = false;
            this.cboMessType.Location = new System.Drawing.Point(568, 336);
            this.cboMessType.Name = "cboMessType";
            this.cboMessType.Size = new System.Drawing.Size(160, 21);
            this.cboMessType.TabIndex = 6;
            this.cboMessType.SelectedIndexChanged += new System.EventHandler(this.cboMessType_SelectedIndexChanged);
            // 
            // cboMessVar
            // 
            this.cboMessVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessVar.Enabled = false;
            this.cboMessVar.Location = new System.Drawing.Point(392, 368);
            this.cboMessVar.Name = "cboMessVar";
            this.cboMessVar.Size = new System.Drawing.Size(144, 21);
            this.cboMessVar.TabIndex = 7;
            this.cboMessVar.Leave += new System.EventHandler(this.cboMessVar_Leave);
            // 
            // cboMessTrig
            // 
            this.cboMessTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessTrig.Enabled = false;
            this.cboMessTrig.Location = new System.Drawing.Point(568, 368);
            this.cboMessTrig.Name = "cboMessTrig";
            this.cboMessTrig.Size = new System.Drawing.Size(160, 21);
            this.cboMessTrig.TabIndex = 8;
            this.cboMessTrig.Leave += new System.EventHandler(this.cboMessTrig_Leave);
            // 
            // label58
            // 
            this.label58.Location = new System.Drawing.Point(536, 368);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(32, 16);
            this.label58.TabIndex = 14;
            this.label58.Text = "must";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpMessages
            // 
            this.grpMessages.Controls.Add(this.lblMess1);
            this.grpMessages.Controls.Add(this.lblMess2);
            this.grpMessages.Controls.Add(this.optMessOR);
            this.grpMessages.Controls.Add(this.optMessAND);
            this.grpMessages.Enabled = false;
            this.grpMessages.Location = new System.Drawing.Point(352, 128);
            this.grpMessages.Name = "grpMessages";
            this.grpMessages.Size = new System.Drawing.Size(408, 168);
            this.grpMessages.TabIndex = 9;
            this.grpMessages.TabStop = false;
            // 
            // lblMess1
            // 
            this.lblMess1.BackColor = System.Drawing.Color.RosyBrown;
            this.lblMess1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblMess1.Location = new System.Drawing.Point(16, 24);
            this.lblMess1.Name = "lblMess1";
            this.lblMess1.Size = new System.Drawing.Size(376, 32);
            this.lblMess1.TabIndex = 6;
            this.lblMess1.Text = "(always TRUE)";
            this.lblMess1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMess1.Click += new System.EventHandler(this.lblMessArr_Click);
            this.lblMess1.DoubleClick += new System.EventHandler(this.lblMessArr_DoubleClick);
            this.lblMess1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblMessArr_MouseUp);
            // 
            // lblMess2
            // 
            this.lblMess2.BackColor = System.Drawing.Color.RosyBrown;
            this.lblMess2.Location = new System.Drawing.Point(16, 112);
            this.lblMess2.Name = "lblMess2";
            this.lblMess2.Size = new System.Drawing.Size(376, 32);
            this.lblMess2.TabIndex = 5;
            this.lblMess2.Text = "(always TRUE)";
            this.lblMess2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMess2.Click += new System.EventHandler(this.lblMessArr_Click);
            this.lblMess2.DoubleClick += new System.EventHandler(this.lblMessArr_DoubleClick);
            this.lblMess2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblMessArr_MouseUp);
            // 
            // optMessOR
            // 
            this.optMessOR.Checked = true;
            this.optMessOR.Location = new System.Drawing.Point(216, 72);
            this.optMessOR.Name = "optMessOR";
            this.optMessOR.Size = new System.Drawing.Size(56, 24);
            this.optMessOR.TabIndex = 4;
            this.optMessOR.TabStop = true;
            this.optMessOR.Text = "OR";
            this.optMessOR.CheckedChanged += new System.EventHandler(this.optMessOR_CheckedChanged);
            // 
            // optMessAND
            // 
            this.optMessAND.Location = new System.Drawing.Point(152, 72);
            this.optMessAND.Name = "optMessAND";
            this.optMessAND.Size = new System.Drawing.Size(56, 24);
            this.optMessAND.TabIndex = 3;
            this.optMessAND.Text = "AND";
            // 
            // label54
            // 
            this.label54.Location = new System.Drawing.Point(608, 72);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(80, 16);
            this.label54.TabIndex = 4;
            this.label54.Text = "Message Color";
            this.label54.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cboMessColor
            // 
            this.cboMessColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMessColor.Enabled = false;
            this.cboMessColor.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Purple"});
            this.cboMessColor.Location = new System.Drawing.Point(696, 72);
            this.cboMessColor.Name = "cboMessColor";
            this.cboMessColor.Size = new System.Drawing.Size(72, 21);
            this.cboMessColor.TabIndex = 2;
            this.cboMessColor.SelectedIndexChanged += new System.EventHandler(this.cboMessColor_SelectedIndexChanged);
            // 
            // txtMessage
            // 
            this.txtMessage.Enabled = false;
            this.txtMessage.Location = new System.Drawing.Point(416, 40);
            this.txtMessage.MaxLength = 62;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(360, 20);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.Leave += new System.EventHandler(this.txtMessage_Leave);
            // 
            // label52
            // 
            this.label52.Location = new System.Drawing.Point(344, 40);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(56, 16);
            this.label52.TabIndex = 1;
            this.label52.Text = "Message";
            this.label52.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
            this.lstMessages.TabIndex = 0;
            this.lstMessages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstMessages_DrawItem);
            this.lstMessages.SelectedIndexChanged += new System.EventHandler(this.lstMessages_SelectedIndexChanged);
            // 
            // label53
            // 
            this.label53.Location = new System.Drawing.Point(344, 72);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(104, 24);
            this.label53.TabIndex = 1;
            this.label53.Text = "Notes                   (not used in game)";
            this.label53.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtShort
            // 
            this.txtShort.Enabled = false;
            this.txtShort.Location = new System.Drawing.Point(448, 72);
            this.txtShort.MaxLength = 15;
            this.txtShort.Name = "txtShort";
            this.txtShort.Size = new System.Drawing.Size(104, 20);
            this.txtShort.TabIndex = 1;
            this.txtShort.Leave += new System.EventHandler(this.txtShort_Leave);
            // 
            // tabGlobal
            // 
            this.tabGlobal.Controls.Add(this.label79);
            this.tabGlobal.Controls.Add(this.label48);
            this.tabGlobal.Controls.Add(this.cboGlobalAmount);
            this.tabGlobal.Controls.Add(this.cboGlobalType);
            this.tabGlobal.Controls.Add(this.cboGlobalVar);
            this.tabGlobal.Controls.Add(this.cboGlobalTrig);
            this.tabGlobal.Controls.Add(this.label59);
            this.tabGlobal.Controls.Add(this.groupBox20);
            this.tabGlobal.Controls.Add(this.groupBox19);
            this.tabGlobal.Controls.Add(this.groupBox18);
            this.tabGlobal.Location = new System.Drawing.Point(4, 22);
            this.tabGlobal.Name = "tabGlobal";
            this.tabGlobal.Size = new System.Drawing.Size(785, 510);
            this.tabGlobal.TabIndex = 2;
            this.tabGlobal.Text = "Globals";
            // 
            // label79
            // 
            this.label79.Location = new System.Drawing.Point(488, 80);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(232, 24);
            this.label79.TabIndex = 22;
            this.label79.Text = "Right-click goal to copy, double-click to paste";
            // 
            // label48
            // 
            this.label48.Location = new System.Drawing.Point(584, 232);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(16, 16);
            this.label48.TabIndex = 21;
            this.label48.Text = "of";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboGlobalAmount
            // 
            this.cboGlobalAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGlobalAmount.Location = new System.Drawing.Point(432, 232);
            this.cboGlobalAmount.Name = "cboGlobalAmount";
            this.cboGlobalAmount.Size = new System.Drawing.Size(144, 21);
            this.cboGlobalAmount.TabIndex = 0;
            this.cboGlobalAmount.Leave += new System.EventHandler(this.cboGlobalAmount_Leave);
            // 
            // cboGlobalType
            // 
            this.cboGlobalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGlobalType.Location = new System.Drawing.Point(608, 232);
            this.cboGlobalType.Name = "cboGlobalType";
            this.cboGlobalType.Size = new System.Drawing.Size(160, 21);
            this.cboGlobalType.TabIndex = 1;
            this.cboGlobalType.SelectedIndexChanged += new System.EventHandler(this.cboGlobalType_SelectedIndexChanged);
            // 
            // cboGlobalVar
            // 
            this.cboGlobalVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGlobalVar.Location = new System.Drawing.Point(432, 264);
            this.cboGlobalVar.Name = "cboGlobalVar";
            this.cboGlobalVar.Size = new System.Drawing.Size(144, 21);
            this.cboGlobalVar.TabIndex = 2;
            this.cboGlobalVar.Leave += new System.EventHandler(this.cboGlobalVar_Leave);
            // 
            // cboGlobalTrig
            // 
            this.cboGlobalTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGlobalTrig.Location = new System.Drawing.Point(608, 264);
            this.cboGlobalTrig.Name = "cboGlobalTrig";
            this.cboGlobalTrig.Size = new System.Drawing.Size(160, 21);
            this.cboGlobalTrig.TabIndex = 3;
            this.cboGlobalTrig.Leave += new System.EventHandler(this.cboGlobalTrig_Leave);
            // 
            // label59
            // 
            this.label59.Location = new System.Drawing.Point(576, 264);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(32, 16);
            this.label59.TabIndex = 20;
            this.label59.Text = "must";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.lblBon1);
            this.groupBox20.Controls.Add(this.lblBon2);
            this.groupBox20.Controls.Add(this.optBonOR);
            this.groupBox20.Controls.Add(this.optBonAND);
            this.groupBox20.Location = new System.Drawing.Point(8, 344);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(408, 160);
            this.groupBox20.TabIndex = 12;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Bonus Goals";
            // 
            // lblBon1
            // 
            this.lblBon1.BackColor = System.Drawing.Color.RosyBrown;
            this.lblBon1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblBon1.Location = new System.Drawing.Point(16, 24);
            this.lblBon1.Name = "lblBon1";
            this.lblBon1.Size = new System.Drawing.Size(376, 32);
            this.lblBon1.TabIndex = 6;
            this.lblBon1.Text = "none (FALSE)";
            this.lblBon1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBon2
            // 
            this.lblBon2.BackColor = System.Drawing.Color.RosyBrown;
            this.lblBon2.Location = new System.Drawing.Point(16, 112);
            this.lblBon2.Name = "lblBon2";
            this.lblBon2.Size = new System.Drawing.Size(376, 32);
            this.lblBon2.TabIndex = 5;
            this.lblBon2.Text = "none (FALSE)";
            this.lblBon2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // optBonOR
            // 
            this.optBonOR.Checked = true;
            this.optBonOR.Location = new System.Drawing.Point(216, 72);
            this.optBonOR.Name = "optBonOR";
            this.optBonOR.Size = new System.Drawing.Size(56, 24);
            this.optBonOR.TabIndex = 9;
            this.optBonOR.TabStop = true;
            this.optBonOR.Text = "OR";
            this.optBonOR.CheckedChanged += new System.EventHandler(this.optBonOR_CheckedChanged);
            // 
            // optBonAND
            // 
            this.optBonAND.Location = new System.Drawing.Point(152, 72);
            this.optBonAND.Name = "optBonAND";
            this.optBonAND.Size = new System.Drawing.Size(56, 24);
            this.optBonAND.TabIndex = 8;
            this.optBonAND.Text = "AND";
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.lblSec1);
            this.groupBox19.Controls.Add(this.lblSec2);
            this.groupBox19.Controls.Add(this.optSecOR);
            this.groupBox19.Controls.Add(this.optSecAND);
            this.groupBox19.Location = new System.Drawing.Point(8, 176);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(408, 160);
            this.groupBox19.TabIndex = 11;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Secondary Goals";
            // 
            // lblSec1
            // 
            this.lblSec1.BackColor = System.Drawing.Color.RosyBrown;
            this.lblSec1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSec1.Location = new System.Drawing.Point(16, 24);
            this.lblSec1.Name = "lblSec1";
            this.lblSec1.Size = new System.Drawing.Size(376, 32);
            this.lblSec1.TabIndex = 6;
            this.lblSec1.Text = "none (FALSE)";
            this.lblSec1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSec2
            // 
            this.lblSec2.BackColor = System.Drawing.Color.RosyBrown;
            this.lblSec2.Location = new System.Drawing.Point(16, 112);
            this.lblSec2.Name = "lblSec2";
            this.lblSec2.Size = new System.Drawing.Size(376, 32);
            this.lblSec2.TabIndex = 5;
            this.lblSec2.Text = "none (FALSE)";
            this.lblSec2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // optSecOR
            // 
            this.optSecOR.Checked = true;
            this.optSecOR.Location = new System.Drawing.Point(216, 72);
            this.optSecOR.Name = "optSecOR";
            this.optSecOR.Size = new System.Drawing.Size(56, 24);
            this.optSecOR.TabIndex = 7;
            this.optSecOR.TabStop = true;
            this.optSecOR.Text = "OR";
            this.optSecOR.CheckedChanged += new System.EventHandler(this.optSecOR_CheckedChanged);
            // 
            // optSecAND
            // 
            this.optSecAND.Location = new System.Drawing.Point(152, 72);
            this.optSecAND.Name = "optSecAND";
            this.optSecAND.Size = new System.Drawing.Size(56, 24);
            this.optSecAND.TabIndex = 6;
            this.optSecAND.Text = "AND";
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.lblPrim1);
            this.groupBox18.Controls.Add(this.lblPrim2);
            this.groupBox18.Controls.Add(this.optPrimOR);
            this.groupBox18.Controls.Add(this.optPrimAND);
            this.groupBox18.Location = new System.Drawing.Point(8, 8);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(408, 160);
            this.groupBox18.TabIndex = 10;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Primary Goals";
            // 
            // lblPrim1
            // 
            this.lblPrim1.BackColor = System.Drawing.Color.RosyBrown;
            this.lblPrim1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblPrim1.Location = new System.Drawing.Point(16, 24);
            this.lblPrim1.Name = "lblPrim1";
            this.lblPrim1.Size = new System.Drawing.Size(376, 32);
            this.lblPrim1.TabIndex = 6;
            this.lblPrim1.Text = "none (FALSE)";
            this.lblPrim1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPrim2
            // 
            this.lblPrim2.BackColor = System.Drawing.Color.RosyBrown;
            this.lblPrim2.Location = new System.Drawing.Point(16, 112);
            this.lblPrim2.Name = "lblPrim2";
            this.lblPrim2.Size = new System.Drawing.Size(376, 32);
            this.lblPrim2.TabIndex = 5;
            this.lblPrim2.Text = "none (FALSE)";
            this.lblPrim2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // optPrimOR
            // 
            this.optPrimOR.Checked = true;
            this.optPrimOR.Location = new System.Drawing.Point(216, 72);
            this.optPrimOR.Name = "optPrimOR";
            this.optPrimOR.Size = new System.Drawing.Size(56, 24);
            this.optPrimOR.TabIndex = 5;
            this.optPrimOR.TabStop = true;
            this.optPrimOR.Text = "OR";
            this.optPrimOR.CheckedChanged += new System.EventHandler(this.optPrimOR_CheckedChanged);
            // 
            // optPrimAND
            // 
            this.optPrimAND.Location = new System.Drawing.Point(152, 72);
            this.optPrimAND.Name = "optPrimAND";
            this.optPrimAND.Size = new System.Drawing.Size(56, 24);
            this.optPrimAND.TabIndex = 4;
            this.optPrimAND.Text = "AND";
            // 
            // tabOfficer
            // 
            this.tabOfficer.Controls.Add(this.label150);
            this.tabOfficer.Controls.Add(this.lblQuestionNote);
            this.tabOfficer.Controls.Add(this.cmdAutoAlign);
            this.tabOfficer.Controls.Add(this.cmdPreview);
            this.tabOfficer.Controls.Add(this.label61);
            this.tabOfficer.Controls.Add(this.txtQuestion);
            this.tabOfficer.Controls.Add(this.label60);
            this.tabOfficer.Controls.Add(this.cboOfficer);
            this.tabOfficer.Controls.Add(this.groupBox27);
            this.tabOfficer.Controls.Add(this.cboQuestion);
            this.tabOfficer.Controls.Add(this.txtAnswer);
            this.tabOfficer.Controls.Add(this.label62);
            this.tabOfficer.Controls.Add(this.cboQTrigType);
            this.tabOfficer.Controls.Add(this.cboQTrig);
            this.tabOfficer.Controls.Add(this.label63);
            this.tabOfficer.Location = new System.Drawing.Point(4, 22);
            this.tabOfficer.Name = "tabOfficer";
            this.tabOfficer.Size = new System.Drawing.Size(785, 510);
            this.tabOfficer.TabIndex = 3;
            this.tabOfficer.Text = "Officers";
            // 
            // label150
            // 
            this.label150.AutoSize = true;
            this.label150.Location = new System.Drawing.Point(253, 488);
            this.label150.Name = "label150";
            this.label150.Size = new System.Drawing.Size(415, 13);
            this.label150.TabIndex = 17;
            this.label150.Text = "Tip: Use the right-click context menu to copy/paste text between the system clipb" +
    "oard.";
            // 
            // lblQuestionNote
            // 
            this.lblQuestionNote.AutoSize = true;
            this.lblQuestionNote.Location = new System.Drawing.Point(560, 451);
            this.lblQuestionNote.Name = "lblQuestionNote";
            this.lblQuestionNote.Size = new System.Drawing.Size(82, 13);
            this.lblQuestionNote.TabIndex = 12;
            this.lblQuestionNote.Text = "lblQuestionNote";
            // 
            // cmdAutoAlign
            // 
            this.cmdAutoAlign.Location = new System.Drawing.Point(563, 335);
            this.cmdAutoAlign.Name = "cmdAutoAlign";
            this.cmdAutoAlign.Size = new System.Drawing.Size(75, 23);
            this.cmdAutoAlign.TabIndex = 11;
            this.cmdAutoAlign.Text = "Best Fit";
            this.cmdAutoAlign.UseVisualStyleBackColor = true;
            this.cmdAutoAlign.Click += new System.EventHandler(this.cmdBestFit_Click);
            // 
            // cmdPreview
            // 
            this.cmdPreview.Location = new System.Drawing.Point(563, 290);
            this.cmdPreview.Name = "cmdPreview";
            this.cmdPreview.Size = new System.Drawing.Size(75, 23);
            this.cmdPreview.TabIndex = 10;
            this.cmdPreview.Text = "&Preview";
            this.cmdPreview.UseVisualStyleBackColor = true;
            this.cmdPreview.Click += new System.EventHandler(this.cmdPreview_Click);
            // 
            // label61
            // 
            this.label61.Location = new System.Drawing.Point(176, 88);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(72, 16);
            this.label61.TabIndex = 4;
            this.label61.Text = "Question";
            // 
            // txtQuestion
            // 
            this.txtQuestion.Location = new System.Drawing.Point(256, 88);
            this.txtQuestion.Name = "txtQuestion";
            this.txtQuestion.Size = new System.Drawing.Size(400, 20);
            this.txtQuestion.TabIndex = 6;
            this.txtQuestion.Leave += new System.EventHandler(this.txtQuestion_Leave);
            // 
            // label60
            // 
            this.label60.Location = new System.Drawing.Point(552, 160);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(195, 120);
            this.label60.TabIndex = 2;
            this.label60.Text = resources.GetString("label60.Text");
            // 
            // cboOfficer
            // 
            this.cboOfficer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOfficer.Items.AddRange(new object[] {
            "Briefing Officer",
            "Pre-flight Secret Order",
            "Debriefing Officer",
            "Post-flight Secret Order"});
            this.cboOfficer.Location = new System.Drawing.Point(256, 40);
            this.cboOfficer.Name = "cboOfficer";
            this.cboOfficer.Size = new System.Drawing.Size(144, 21);
            this.cboOfficer.TabIndex = 4;
            this.cboOfficer.SelectedIndexChanged += new System.EventHandler(this.cboOfficer_SelectedIndexChanged);
            // 
            // groupBox27
            // 
            this.groupBox27.Controls.Add(this.optFO);
            this.groupBox27.Controls.Add(this.optSO);
            this.groupBox27.Controls.Add(this.optBoth);
            this.groupBox27.Location = new System.Drawing.Point(24, 24);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Size = new System.Drawing.Size(116, 119);
            this.groupBox27.TabIndex = 0;
            this.groupBox27.TabStop = false;
            this.groupBox27.Text = "Officers Present";
            // 
            // optFO
            // 
            this.optFO.Location = new System.Drawing.Point(15, 16);
            this.optFO.Name = "optFO";
            this.optFO.Size = new System.Drawing.Size(88, 24);
            this.optFO.TabIndex = 1;
            this.optFO.Text = "Flight Officer";
            // 
            // optSO
            // 
            this.optSO.Location = new System.Drawing.Point(15, 48);
            this.optSO.Name = "optSO";
            this.optSO.Size = new System.Drawing.Size(88, 24);
            this.optSO.TabIndex = 2;
            this.optSO.Text = "Secret Order";
            // 
            // optBoth
            // 
            this.optBoth.Location = new System.Drawing.Point(15, 80);
            this.optBoth.Name = "optBoth";
            this.optBoth.Size = new System.Drawing.Size(88, 24);
            this.optBoth.TabIndex = 3;
            this.optBoth.Text = "Both";
            // 
            // cboQuestion
            // 
            this.cboQuestion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQuestion.Items.AddRange(new object[] {
            "Question 1",
            "Question 2",
            "Question 3",
            "Question 4",
            "Question 5"});
            this.cboQuestion.Location = new System.Drawing.Point(488, 40);
            this.cboQuestion.Name = "cboQuestion";
            this.cboQuestion.Size = new System.Drawing.Size(121, 21);
            this.cboQuestion.TabIndex = 5;
            this.cboQuestion.SelectedIndexChanged += new System.EventHandler(this.cboQuestion_SelectedIndexChanged);
            // 
            // txtAnswer
            // 
            this.txtAnswer.AcceptsReturn = true;
            this.txtAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAnswer.Location = new System.Drawing.Point(256, 152);
            this.txtAnswer.Multiline = true;
            this.txtAnswer.Name = "txtAnswer";
            this.txtAnswer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAnswer.Size = new System.Drawing.Size(280, 312);
            this.txtAnswer.TabIndex = 9;
            this.txtAnswer.TextChanged += new System.EventHandler(this.txtAnswer_TextChanged);
            this.txtAnswer.Leave += new System.EventHandler(this.txtAnswer_Leave);
            // 
            // label62
            // 
            this.label62.Location = new System.Drawing.Point(176, 160);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(72, 16);
            this.label62.TabIndex = 4;
            this.label62.Text = "Answer";
            // 
            // cboQTrigType
            // 
            this.cboQTrigType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQTrigType.Enabled = false;
            this.cboQTrigType.Items.AddRange(new object[] {
            "None",
            "Primary Goals",
            "Secondary Goals",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cboQTrigType.Location = new System.Drawing.Point(328, 120);
            this.cboQTrigType.Name = "cboQTrigType";
            this.cboQTrigType.Size = new System.Drawing.Size(136, 21);
            this.cboQTrigType.TabIndex = 7;
            this.cboQTrigType.Leave += new System.EventHandler(this.cboQTrigType_Leave);
            // 
            // cboQTrig
            // 
            this.cboQTrig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQTrig.Enabled = false;
            this.cboQTrig.Items.AddRange(new object[] {
            "None",
            "1",
            "2",
            "3",
            "successful",
            "failed",
            "6",
            "7",
            "8"});
            this.cboQTrig.Location = new System.Drawing.Point(512, 120);
            this.cboQTrig.Name = "cboQTrig";
            this.cboQTrig.Size = new System.Drawing.Size(144, 21);
            this.cboQTrig.TabIndex = 8;
            this.cboQTrig.Leave += new System.EventHandler(this.cboQTrig_Leave);
            // 
            // label63
            // 
            this.label63.Location = new System.Drawing.Point(256, 120);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(248, 23);
            this.label63.TabIndex = 5;
            this.label63.Text = "Shows when\t\t\t\t\t\t\t\t\t\t\t\t    are";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabMission
            // 
            this.tabMission.Controls.Add(this.groupBox23);
            this.tabMission.Controls.Add(this.groupBox22);
            this.tabMission.Controls.Add(this.groupBox21);
            this.tabMission.Location = new System.Drawing.Point(4, 22);
            this.tabMission.Name = "tabMission";
            this.tabMission.Size = new System.Drawing.Size(785, 510);
            this.tabMission.TabIndex = 4;
            this.tabMission.Text = "Mission";
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.groupBox26);
            this.groupBox23.Controls.Add(this.groupBox25);
            this.groupBox23.Controls.Add(this.groupBox24);
            this.groupBox23.Location = new System.Drawing.Point(360, 32);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(400, 453);
            this.groupBox23.TabIndex = 17;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "End of Mission Messages";
            // 
            // groupBox26
            // 
            this.groupBox26.Controls.Add(this.cboPF2Color);
            this.groupBox26.Controls.Add(this.cboPF1Color);
            this.groupBox26.Controls.Add(this.txtPrimFail1);
            this.groupBox26.Controls.Add(this.txtPrimFail2);
            this.groupBox26.Location = new System.Drawing.Point(8, 318);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Size = new System.Drawing.Size(384, 128);
            this.groupBox26.TabIndex = 2;
            this.groupBox26.TabStop = false;
            this.groupBox26.Text = "Primary Mission Failed";
            // 
            // cboPF2Color
            // 
            this.cboPF2Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPF2Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Purple"});
            this.cboPF2Color.Location = new System.Drawing.Point(248, 72);
            this.cboPF2Color.Name = "cboPF2Color";
            this.cboPF2Color.Size = new System.Drawing.Size(120, 21);
            this.cboPF2Color.TabIndex = 44;
            // 
            // cboPF1Color
            // 
            this.cboPF1Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPF1Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Purple"});
            this.cboPF1Color.Location = new System.Drawing.Point(248, 18);
            this.cboPF1Color.Name = "cboPF1Color";
            this.cboPF1Color.Size = new System.Drawing.Size(120, 21);
            this.cboPF1Color.TabIndex = 43;
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
            // groupBox25
            // 
            this.groupBox25.Controls.Add(this.cboSC2Color);
            this.groupBox25.Controls.Add(this.cboSC1Color);
            this.groupBox25.Controls.Add(this.txtSecComp1);
            this.groupBox25.Controls.Add(this.txtSecComp2);
            this.groupBox25.Location = new System.Drawing.Point(8, 170);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(384, 128);
            this.groupBox25.TabIndex = 1;
            this.groupBox25.TabStop = false;
            this.groupBox25.Text = "Secondary Mission Complete";
            // 
            // cboSC2Color
            // 
            this.cboSC2Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSC2Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Purple"});
            this.cboSC2Color.Location = new System.Drawing.Point(248, 72);
            this.cboSC2Color.Name = "cboSC2Color";
            this.cboSC2Color.Size = new System.Drawing.Size(120, 21);
            this.cboSC2Color.TabIndex = 42;
            // 
            // cboSC1Color
            // 
            this.cboSC1Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSC1Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Purple"});
            this.cboSC1Color.Location = new System.Drawing.Point(248, 18);
            this.cboSC1Color.Name = "cboSC1Color";
            this.cboSC1Color.Size = new System.Drawing.Size(120, 21);
            this.cboSC1Color.TabIndex = 41;
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
            // groupBox24
            // 
            this.groupBox24.Controls.Add(this.cboPC2Color);
            this.groupBox24.Controls.Add(this.cboPC1Color);
            this.groupBox24.Controls.Add(this.txtPrimComp1);
            this.groupBox24.Controls.Add(this.txtPrimComp2);
            this.groupBox24.Location = new System.Drawing.Point(8, 24);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Size = new System.Drawing.Size(384, 128);
            this.groupBox24.TabIndex = 0;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "Primary Mission Complete";
            // 
            // cboPC2Color
            // 
            this.cboPC2Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPC2Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Purple"});
            this.cboPC2Color.Location = new System.Drawing.Point(248, 72);
            this.cboPC2Color.Name = "cboPC2Color";
            this.cboPC2Color.Size = new System.Drawing.Size(120, 21);
            this.cboPC2Color.TabIndex = 40;
            // 
            // cboPC1Color
            // 
            this.cboPC1Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPC1Color.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Purple"});
            this.cboPC1Color.Location = new System.Drawing.Point(248, 18);
            this.cboPC1Color.Name = "cboPC1Color";
            this.cboPC1Color.Size = new System.Drawing.Size(120, 21);
            this.cboPC1Color.TabIndex = 39;
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
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.chkIFF3);
            this.groupBox22.Controls.Add(this.txtIFF3);
            this.groupBox22.Controls.Add(this.label64);
            this.groupBox22.Controls.Add(this.label65);
            this.groupBox22.Controls.Add(this.label66);
            this.groupBox22.Controls.Add(this.label67);
            this.groupBox22.Controls.Add(this.txtIFF5);
            this.groupBox22.Controls.Add(this.txtIFF6);
            this.groupBox22.Controls.Add(this.txtIFF4);
            this.groupBox22.Controls.Add(this.chkIFF4);
            this.groupBox22.Controls.Add(this.chkIFF5);
            this.groupBox22.Controls.Add(this.chkIFF6);
            this.groupBox22.Location = new System.Drawing.Point(16, 112);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(320, 152);
            this.groupBox22.TabIndex = 16;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "IFFs";
            // 
            // chkIFF3
            // 
            this.chkIFF3.Location = new System.Drawing.Point(208, 24);
            this.chkIFF3.Name = "chkIFF3";
            this.chkIFF3.Size = new System.Drawing.Size(104, 24);
            this.chkIFF3.TabIndex = 3;
            this.chkIFF3.Text = "Enemy/Hostile";
            // 
            // txtIFF3
            // 
            this.txtIFF3.BackColor = System.Drawing.Color.Black;
            this.txtIFF3.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtIFF3.Location = new System.Drawing.Point(104, 24);
            this.txtIFF3.MaxLength = 12;
            this.txtIFF3.Name = "txtIFF3";
            this.txtIFF3.Size = new System.Drawing.Size(88, 20);
            this.txtIFF3.TabIndex = 2;
            // 
            // label64
            // 
            this.label64.Location = new System.Drawing.Point(16, 24);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(72, 16);
            this.label64.TabIndex = 0;
            this.label64.Text = "IFF3 -Blue";
            this.label64.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label65
            // 
            this.label65.Location = new System.Drawing.Point(16, 56);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(72, 16);
            this.label65.TabIndex = 0;
            this.label65.Text = "IFF4 - Purple";
            this.label65.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label66
            // 
            this.label66.Location = new System.Drawing.Point(16, 88);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(72, 16);
            this.label66.TabIndex = 0;
            this.label66.Text = "IFF5 - Red";
            this.label66.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label67
            // 
            this.label67.Location = new System.Drawing.Point(16, 120);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(72, 16);
            this.label67.TabIndex = 0;
            this.label67.Text = "IFF6 - Purple";
            this.label67.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtIFF5
            // 
            this.txtIFF5.BackColor = System.Drawing.Color.Black;
            this.txtIFF5.ForeColor = System.Drawing.Color.OrangeRed;
            this.txtIFF5.Location = new System.Drawing.Point(104, 88);
            this.txtIFF5.MaxLength = 12;
            this.txtIFF5.Name = "txtIFF5";
            this.txtIFF5.Size = new System.Drawing.Size(88, 20);
            this.txtIFF5.TabIndex = 6;
            // 
            // txtIFF6
            // 
            this.txtIFF6.BackColor = System.Drawing.Color.Black;
            this.txtIFF6.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtIFF6.Location = new System.Drawing.Point(104, 120);
            this.txtIFF6.MaxLength = 12;
            this.txtIFF6.Name = "txtIFF6";
            this.txtIFF6.Size = new System.Drawing.Size(88, 20);
            this.txtIFF6.TabIndex = 8;
            // 
            // txtIFF4
            // 
            this.txtIFF4.BackColor = System.Drawing.Color.Black;
            this.txtIFF4.ForeColor = System.Drawing.Color.MediumOrchid;
            this.txtIFF4.Location = new System.Drawing.Point(104, 56);
            this.txtIFF4.MaxLength = 12;
            this.txtIFF4.Name = "txtIFF4";
            this.txtIFF4.Size = new System.Drawing.Size(88, 20);
            this.txtIFF4.TabIndex = 4;
            // 
            // chkIFF4
            // 
            this.chkIFF4.Location = new System.Drawing.Point(208, 56);
            this.chkIFF4.Name = "chkIFF4";
            this.chkIFF4.Size = new System.Drawing.Size(104, 24);
            this.chkIFF4.TabIndex = 5;
            this.chkIFF4.Text = "Enemy/Hostile";
            // 
            // chkIFF5
            // 
            this.chkIFF5.Location = new System.Drawing.Point(208, 88);
            this.chkIFF5.Name = "chkIFF5";
            this.chkIFF5.Size = new System.Drawing.Size(104, 24);
            this.chkIFF5.TabIndex = 7;
            this.chkIFF5.Text = "Enemy/Hostile";
            // 
            // chkIFF6
            // 
            this.chkIFF6.Location = new System.Drawing.Point(208, 120);
            this.chkIFF6.Name = "chkIFF6";
            this.chkIFF6.Size = new System.Drawing.Size(104, 24);
            this.chkIFF6.TabIndex = 9;
            this.chkIFF6.Text = "Enemy/Hostile";
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.optCapture);
            this.groupBox21.Controls.Add(this.optRescue);
            this.groupBox21.Location = new System.Drawing.Point(16, 32);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(320, 56);
            this.groupBox21.TabIndex = 15;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "After ejection pilot is...";
            // 
            // optCapture
            // 
            this.optCapture.Location = new System.Drawing.Point(184, 24);
            this.optCapture.Name = "optCapture";
            this.optCapture.Size = new System.Drawing.Size(104, 24);
            this.optCapture.TabIndex = 1;
            this.optCapture.Text = "Captured";
            this.optCapture.CheckedChanged += new System.EventHandler(this.optCapture_CheckedChanged);
            // 
            // optRescue
            // 
            this.optRescue.Checked = true;
            this.optRescue.Location = new System.Drawing.Point(56, 24);
            this.optRescue.Name = "optRescue";
            this.optRescue.Size = new System.Drawing.Size(104, 24);
            this.optRescue.TabIndex = 0;
            this.optRescue.TabStop = true;
            this.optRescue.Text = "Rescued";
            // 
            // toolTIE
            // 
            this.toolTIE.AutoSize = false;
            this.toolTIE.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
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
            this.toolBattle,
            this.toolHelp});
            this.toolTIE.DropDownArrows = true;
            this.toolTIE.ImageList = this.imgToolbar;
            this.toolTIE.Location = new System.Drawing.Point(0, 0);
            this.toolTIE.Name = "toolTIE";
            this.toolTIE.ShowToolTips = true;
            this.toolTIE.Size = new System.Drawing.Size(794, 30);
            this.toolTIE.TabIndex = 1;
            this.toolTIE.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolTIE_ButtonClick);
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
            this.toolCopy.ToolTipText = "Copy FlightGroup";
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
            this.toolVerify.ImageIndex = 11;
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
            this.toolOptions.ImageIndex = 10;
            this.toolOptions.Name = "toolOptions";
            this.toolOptions.ToolTipText = "Options";
            // 
            // toolBattle
            // 
            this.toolBattle.ImageIndex = 12;
            this.toolBattle.Name = "toolBattle";
            // 
            // toolHelp
            // 
            this.toolHelp.ImageIndex = 13;
            this.toolHelp.Name = "toolHelp";
            this.toolHelp.ToolTipText = "Help";
            // 
            // menuTIE
            // 
            this.menuTIE.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
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
            this.menuItem7,
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
            this.menuNewTIE.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
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
            this.menuNewXWA.Text = "&XWA mission";
            this.menuNewXWA.Click += new System.EventHandler(this.menuNewXWA_Click);
            // 
            // menuOpen
            // 
            this.menuOpen.Index = 1;
            this.menuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.menuOpen.Text = "&Open...";
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
            this.menuSaveAs.Text = "Save &As...";
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
            this.menuSaveAsBoP.Enabled = false;
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
            // menuItem7
            // 
            this.menuItem7.Index = 5;
            this.menuItem7.Text = "-";
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
            this.menuItem6,
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
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Text = "-";
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
            this.menuBriefing,
            this.menuBattle,
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
            // menuBriefing
            // 
            this.menuBriefing.Index = 2;
            this.menuBriefing.Text = "&Briefing";
            this.menuBriefing.Click += new System.EventHandler(this.menuBriefing_Click);
            // 
            // menuBattle
            // 
            this.menuBattle.Index = 3;
            this.menuBattle.Text = "Batt&le";
            this.menuBattle.Click += new System.EventHandler(this.menuBattle_Click);
            // 
            // menuOptions
            // 
            this.menuOptions.Index = 4;
            this.menuOptions.Text = "&Options...";
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
            // opnTIE
            // 
            this.opnTIE.DefaultExt = "tie";
            this.opnTIE.Filter = "Mission Files|*.tie|X-wing Missions|*.xwi";
            // 
            // savTIE
            // 
            this.savTIE.DefaultExt = "tie";
            this.savTIE.FileName = "NewMission.tie";
            this.savTIE.Filter = "Mission Files|*.tie|X-wing Missions|*.xwi";
            this.savTIE.FileOk += new System.ComponentModel.CancelEventHandler(this.savTIE_FileOk);
            // 
            // _dataWaypoints
            // 
            this._dataWaypoints.AllowDelete = false;
            this._dataWaypoints.AllowNew = false;
            // 
            // _dataWaypointsRaw
            // 
            this._dataWaypointsRaw.AllowDelete = false;
            this._dataWaypointsRaw.AllowNew = false;
            // 
            // TieForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(794, 575);
            this.Controls.Add(this.toolTIE);
            this.Controls.Add(this.tabMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Menu = this.menuTIE;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "TieForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ye Olde Galactic Empire Mission Editor - TIE";
            this.Activated += new System.EventHandler(this.frmTIE_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmTIE_Closing);
            this.tabMain.ResumeLayout(false);
            this.tabFG.ResumeLayout(false);
            this.tabFGMinor.ResumeLayout(false);
            this.tabCraft.ResumeLayout(false);
            this.grpCraft4.ResumeLayout(false);
            this.grpCraft4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBackdrop)).EndInit();
            this.grpCraft2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpacing)).EndInit();
            this.grpCraft3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numWaves)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCraft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGlobal)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSC)).EndInit();
            this.tabArrDep.ResumeLayout(false);
            this.grpDep.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numDepSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDepMin)).EndInit();
            this.groupBox10.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.grpArr.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numArrSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArrMin)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.tabGoals.ResumeLayout(false);
            this.groupBox17.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numBonGoalP)).EndInit();
            this.groupBox16.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.tabWP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numRoll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataWP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataWP_Raw)).EndInit();
            this.tabOrders.ResumeLayout(false);
            this.grpSecOrder.ResumeLayout(false);
            this.grpPrimOrder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numOVar1)).EndInit();
            this.groupBox11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numOVar2)).EndInit();
            this.tabOptions.ResumeLayout(false);
            this.grpPermaDeath.ResumeLayout(false);
            this.grpPermaDeath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPermaDeathID)).EndInit();
            this.tabUnk.ResumeLayout(false);
            this.tabUnk.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnk1)).EndInit();
            this.tabMess.ResumeLayout(false);
            this.tabMess.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMessDelay)).EndInit();
            this.grpMessages.ResumeLayout(false);
            this.tabGlobal.ResumeLayout(false);
            this.groupBox20.ResumeLayout(false);
            this.groupBox19.ResumeLayout(false);
            this.groupBox18.ResumeLayout(false);
            this.tabOfficer.ResumeLayout(false);
            this.tabOfficer.PerformLayout();
            this.groupBox27.ResumeLayout(false);
            this.tabMission.ResumeLayout(false);
            this.groupBox23.ResumeLayout(false);
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            this.groupBox25.ResumeLayout(false);
            this.groupBox25.PerformLayout();
            this.groupBox24.ResumeLayout(false);
            this.groupBox24.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            this.groupBox21.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dataWaypoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataWaypointsRaw)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		Label label79;
		Button cmdCopyOrder;
		Button cmdPasteOrder;
		Button cmdCopyAD;
		Button cmdPasteAD;
		NumericUpDown numYaw;
		NumericUpDown numPitch;
		NumericUpDown numRoll;
		Label label76;
		Label label77;
		Label label78;
		ImageList imgToolbar;
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
		ToolBarButton toolBattle;
		ToolBarButton toolHelp;
		MenuItem menuHelpInfo;
		MenuItem menuAbout;
		MenuItem menuIDMR;
		MenuItem menuER;
		Label label68;
		Label label69;
		Label label70;
		Label label71;
		Label label72;
		Label label73;
		Label label74;
		Label label75;
		MenuItem menuVerify;
		MenuItem menuMap;
		MenuItem menuBriefing;
		MenuItem menuOptions;
		MenuItem menuUndo;
		MenuItem menuCut;
		MenuItem menuCopy;
		MenuItem menuPaste;
		MenuItem menuDelete;
		MenuItem menuItem6;
		MenuItem menuItem7;
		GroupBox groupBox27;
		RadioButton optFO;
		RadioButton optSO;
		RadioButton optBoth;
		ComboBox cboOfficer;
		ComboBox cboQuestion;
		Label label60;
		TextBox txtQuestion;
		TextBox txtAnswer;
		Label label61;
		Label label62;
		ComboBox cboQTrigType;
		ComboBox cboQTrig;
		Label label63;
		Label label39;
		Label label40;
		Label label41;
		Label label47;
		Label label48;
		ComboBox cboGlobalAmount;
		ComboBox cboGlobalType;
		ComboBox cboGlobalVar;
		ComboBox cboGlobalTrig;
		Label label59;
		Label lblMessage;
		DataGrid dataWP;
		DataGrid dataWP_Raw;
		Label label56;
		CheckBox chkWPRend;
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
		GroupBox groupBox18;
		GroupBox grpMessages;
		Label lblPrim1;
		Label lblPrim2;
		RadioButton optPrimOR;
		RadioButton optPrimAND;
		GroupBox groupBox19;
		Label lblSec1;
		Label lblSec2;
		RadioButton optSecOR;
		RadioButton optSecAND;
		GroupBox groupBox20;
		Label lblBon1;
		Label lblBon2;
		RadioButton optBonOR;
		RadioButton optBonAND;
		GroupBox groupBox22;
		CheckBox chkIFF3;
		TextBox txtIFF3;
		Label label64;
		Label label65;
		Label label66;
		Label label67;
		TextBox txtIFF5;
		TextBox txtIFF6;
		TextBox txtIFF4;
		CheckBox chkIFF4;
		CheckBox chkIFF5;
		CheckBox chkIFF6;
		GroupBox groupBox21;
		RadioButton optCapture;
		RadioButton optRescue;
		GroupBox groupBox23;
		GroupBox groupBox24;
		TextBox txtPrimComp1;
		TextBox txtPrimComp2;
		GroupBox groupBox25;
		TextBox txtSecComp1;
		TextBox txtSecComp2;
		GroupBox groupBox26;
		TextBox txtPrimFail1;
		TextBox txtPrimFail2;
		ListBox lstMessages;
		ToolBar toolTIE;
		MainMenu menuTIE;
		MenuItem menuFile;
		MenuItem menuEdit;
		MenuItem menuTools;
		MenuItem menuHelp;
		MenuItem menuNew;
		MenuItem menuOpen;
		MenuItem menuNewTIE;
		MenuItem menuNewXvT;
		MenuItem menuNewBoP;
		MenuItem menuNewXWA;
		MenuItem menuSave;
		MenuItem menuSaveAs;
		MenuItem menuSaveAsTIE;
		MenuItem menuSaveAsBoP;
		MenuItem menuSaveAsXvT;
		OpenFileDialog opnTIE;
		SaveFileDialog savTIE;
		MenuItem menuSaveAsXWA;
		TabPage tabFG;
		TabPage tabMess;
		MenuItem menuExit;
		TabPage tabGlobal;
		TabPage tabOfficer;
		TabPage tabMission;
		ListBox lstFG;
		Label label1;
		TabPage tabCraft;
		TabPage tabArrDep;
		TabPage tabOrders;
		TabPage tabGoals;
		TabPage tabWP;
		TabPage tabUnk;
		Label label2;
		Label label3;
		Label label4;
		Label label5;
		Label lblNotUsed;
		TextBox txtName;
		TextBox txtPilot;
		TextBox txtCargo;
		TextBox txtSpecCargo;
		Label label6;
		CheckBox chkRandSC;
		NumericUpDown numSC;
		GroupBox groupBox1;
		GroupBox grpCraft3;
		Label label7;
		Label label8;
		Label label9;
		Label label10;
		Label label11;
		Label label12;
		Label label13;
		NumericUpDown numWaves;
		NumericUpDown numCraft;
		NumericUpDown numGlobal;
		GroupBox grpCraft2;
		Label label14;
		Label label15;
		Label label16;
		Label label17;
		NumericUpDown numLead;
		NumericUpDown numSpacing;
		ComboBox cboCraft;
		ComboBox cboIFF;
		ComboBox cboAI;
		ComboBox cboMarkings;
		ComboBox cboPlayer;
		ComboBox cboFormation;
		CheckBox chkRadio;
		GroupBox grpCraft4;
		Label label18;
		ComboBox cboWarheads;
		Label lblStatus;
		ComboBox cboBeam;
		ComboBox cboStatus;
		Label label20;
		Label lblFG;
		Label lblStarting;
		Button cmdForms;
		GroupBox grpArr;
		GroupBox grpDep;
		GroupBox groupBox7;
		RadioButton optArrHyp;
		RadioButton optArrMS;
		ComboBox cboArrMS;
		GroupBox groupBox8;
		RadioButton optArrHypAlt;
		ComboBox cboArrMSAlt;
		RadioButton optArrMSAlt;
		GroupBox groupBox9;
		GroupBox groupBox10;
		RadioButton optDepHyp;
		ComboBox cboDepMS;
		RadioButton optDepMS;
		RadioButton optDepMSAlt;
		RadioButton optDepHypAlt;
		ComboBox cboDepMSAlt;
		ComboBox cboADTrigAmount;
		ComboBox cboADTrigType;
		ComboBox cboADTrigVar;
		ComboBox cboADTrig;
		Label label21;
		Label label22;
		Label lblArr1;
		Label lblArr2;
		Label lblDep;
		RadioButton optArrAND;
		Label label23;
		Label label24;
		Label label25;
		Label label26;
		ComboBox cboAbort;
		RadioButton optArrOR;
		ComboBox cboDiff;
		Label label27;
		Label label28;
		TabControl tabMain;
		TabControl tabFGMinor;
		ComboBox cboOrders;
		GroupBox groupBox11;
		Label lblOrder1;
		Label lblOrder2;
		Label lblOrder3;
		Label label29;
		ComboBox cboOThrottle;
		Label lblOVar1;
		Label lblOVar2;
		NumericUpDown numOVar1;
		NumericUpDown numOVar2;
		GroupBox grpPrimOrder;
		ComboBox cboOT1;
		ComboBox cboOT2Type;
		ComboBox cboOT2;
		RadioButton optOT1T2OR;
		Label label30;
		GroupBox grpSecOrder;
		ComboBox cboOT3Type;
		ComboBox cboOT4Type;
		ComboBox cboOT3;
		ComboBox cboOT4;
		RadioButton optOT3T4OR;
		RadioButton optOT3T4AND;
		Label label31;
		ComboBox cboOT1Type;
		RadioButton optOT1T2AND;
		GroupBox groupBox14;
		ComboBox cboPrimGoalT;
		Label label32;
		GroupBox groupBox15;
		Label label33;
		ComboBox cboSecGoalA;
		ComboBox cboSecGoalT;
		GroupBox groupBox16;
		Label label34;
		ComboBox cboSecretGoalA;
		ComboBox cboSecretGoalT;
		GroupBox groupBox17;
		Label label35;
		ComboBox cboBonGoalA;
		ComboBox cboBonGoalT;
		NumericUpDown numBonGoalP;
		Label label36;
		ComboBox cboPrimGoalA;
		Label lblODesc;
		Label label37;
		Label label38;
		Label label42;
		Label label43;
		Label label44;
		Label label45;
		Label label46;
		Label label49;
		Label label50;
		Label label51;
		Label label52;
		Label label53;
		TextBox txtMessage;
		TextBox txtShort;
		ComboBox cboMessColor;
		Label label54;
		Label lblMess1;
		Label lblMess2;
		Label label57;
		ComboBox cboMessAmount;
		ComboBox cboMessType;
		ComboBox cboMessVar;
		ComboBox cboMessTrig;
		Label label58;
		Label label55;
		NumericUpDown numMessDelay;
		RadioButton optMessAND;
		RadioButton optMessOR;
		CheckBox chkWPBrief;
		NumericUpDown numArrMin;
		NumericUpDown numArrSec;
		NumericUpDown numDepSec;
		NumericUpDown numDepMin;
        NumericUpDown numUnk1;
		NumericUpDown numUnk5;
		NumericUpDown numUnk17;
		NumericUpDown numUnk16;
		NumericUpDown numUnk15;
		NumericUpDown numUnk12;
		NumericUpDown numUnk11;
		Label label80;
		private MenuItem menuBattle;
		private Button cmdBackdrop;
		private Label lblBackdrop;
		private NumericUpDown numBackdrop;
		private MenuItem menuTest;
		private Button cmdPreview;
		private CheckBox chkUnk19;
		private NumericUpDown numUnk20;
		private Label label81;
		private CheckBox chkUnk21;
		private MenuItem menuRecent;
		private MenuItem menuRec1;
		private MenuItem menuRec2;
		private MenuItem menuRec3;
		private MenuItem menuRec4;
		private MenuItem menuRec5;
        private MenuItem menuGoalSummary;
		private MenuItem menuNewXwing;
        private Button cmdMoveFGDown;
        private Button cmdMoveFGUp;
        private Button cmdMoveMessDown;
        private Button cmdMoveMessUp;
        private TabPage tabOptions;
        private GroupBox grpPermaDeath;
        private Label label19;
        private CheckBox chkPermaDeath;
        private Label label82;
        private NumericUpDown numPermaDeathID;
        private Label label84;
        private Label label83;
        private Label lblOVar1Note;
        private Button cmdAutoAlign;
        private Label lblQuestionNote;
        private Label label150;
        private Label lblOVar2Note;
        private ComboBox cboPC1Color;
        private ComboBox cboPC2Color;
        private ComboBox cboPF2Color;
        private ComboBox cboPF1Color;
        private ComboBox cboSC2Color;
        private ComboBox cboSC1Color;
	}
}