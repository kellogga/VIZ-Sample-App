namespace Maestro
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                _client.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.cmdVIZEngineMiniConnect = new System.Windows.Forms.Button();
            this.cmdVIZEngineMainConnect = new System.Windows.Forms.Button();
            this.lblVIZEngineMini = new System.Windows.Forms.Label();
            this.lblVIZEngineMain = new System.Windows.Forms.Label();
            this.DgvStates = new System.Windows.Forms.DataGridView();
            this.colStateNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStateCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTake = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colECVotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInProgress = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDemocrats = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRepublicans = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colOther = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colApCall = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTimeZone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWinningParty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMini = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnRepublicanCall = new System.Windows.Forms.Button();
            this.btnDemocratCall = new System.Windows.Forms.Button();
            this.grpCanopyData = new System.Windows.Forms.GroupBox();
            this.lblCanopyStatusValue = new System.Windows.Forms.Label();
            this.lblCanopyStatus = new System.Windows.Forms.Label();
            this.grpCanopyTimer = new System.Windows.Forms.GroupBox();
            this.lblCanopyRefreshInterval = new System.Windows.Forms.Label();
            this.CanopyRefreshSeconds = new System.Windows.Forms.NumericUpDown();
            this.optFeedOff = new System.Windows.Forms.RadioButton();
            this.optFeedOn = new System.Windows.Forms.RadioButton();
            this.lblCurrentAPFile = new System.Windows.Forms.Label();
            this.pnlSenateHouse = new System.Windows.Forms.Panel();
            this.lblSOthTotal = new System.Windows.Forms.Label();
            this.lblSGopTotal = new System.Windows.Forms.Label();
            this.lblSDemTotal = new System.Windows.Forms.Label();
            this.lblSOthHold = new System.Windows.Forms.Label();
            this.lblSGopHold = new System.Windows.Forms.Label();
            this.lblSDemHold = new System.Windows.Forms.Label();
            this.lblHouseOth = new System.Windows.Forms.Label();
            this.txtHOth = new System.Windows.Forms.TextBox();
            this.lblSenateOth = new System.Windows.Forms.Label();
            this.txtSOth = new System.Windows.Forms.TextBox();
            this.lblHouseDemocrat = new System.Windows.Forms.Label();
            this.cmdHouse = new System.Windows.Forms.Button();
            this.cmdSenate = new System.Windows.Forms.Button();
            this.lblHouseRepublican = new System.Windows.Forms.Label();
            this.txtHRep = new System.Windows.Forms.TextBox();
            this.txtHDem = new System.Windows.Forms.TextBox();
            this.lblHouse = new System.Windows.Forms.Label();
            this.lblSenateDemocrat = new System.Windows.Forms.Label();
            this.lblSenateRepublican = new System.Windows.Forms.Label();
            this.txtSRep = new System.Windows.Forms.TextBox();
            this.txtSDem = new System.Windows.Forms.TextBox();
            this.lblSenate = new System.Windows.Forms.Label();
            this.btnContinue = new System.Windows.Forms.Button();
            this.pnlVIZBackground = new System.Windows.Forms.Panel();
            this.optBackgroundOff = new System.Windows.Forms.RadioButton();
            this.optBackgroundOn = new System.Windows.Forms.RadioButton();
            this.cmdMapPrevious = new System.Windows.Forms.Button();
            this.cmdMap = new System.Windows.Forms.Button();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdTimeZone1 = new System.Windows.Forms.Button();
            this.cmdTimeZone9 = new System.Windows.Forms.Button();
            this.cmdTimeZone2 = new System.Windows.Forms.Button();
            this.cmdTimeZone8 = new System.Windows.Forms.Button();
            this.cmdTimeZone3 = new System.Windows.Forms.Button();
            this.cmdTimeZone7 = new System.Windows.Forms.Button();
            this.cmdTimeZone4 = new System.Windows.Forms.Button();
            this.cmdTimeZone6 = new System.Windows.Forms.Button();
            this.cmdTimeZone5 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allowCanopyUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disallowCanopyUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.importStateGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpMini = new System.Windows.Forms.GroupBox();
            this.grpMaestroFR = new System.Windows.Forms.GroupBox();
            this.chkAnimatedBG = new System.Windows.Forms.CheckBox();
            this.chkMapYear = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.optMapOff = new System.Windows.Forms.RadioButton();
            this.optMapOn = new System.Windows.Forms.RadioButton();
            this.btnResetMain = new System.Windows.Forms.Button();
            this.btnResetMini = new System.Windows.Forms.Button();
            this.btnStanding = new System.Windows.Forms.Button();
            this.grpMiniBackground = new System.Windows.Forms.GroupBox();
            this.cmdMiniBackgroundOff = new System.Windows.Forms.Button();
            this.lblMiniBackground = new System.Windows.Forms.Label();
            this.grpMiniProjection = new System.Windows.Forms.GroupBox();
            this.lblMiniProjection = new System.Windows.Forms.Label();
            this.optMiniProjectionOff = new System.Windows.Forms.RadioButton();
            this.optMiniProjectionOn = new System.Windows.Forms.RadioButton();
            this.grpZeroMini = new System.Windows.Forms.GroupBox();
            this.optZeroMiniOff = new System.Windows.Forms.RadioButton();
            this.optZeroMiniOn = new System.Windows.Forms.RadioButton();
            this.grpMiniResults = new System.Windows.Forms.GroupBox();
            this.lblUSStateCodeResults = new System.Windows.Forms.Label();
            this.lblMiniResults = new System.Windows.Forms.Label();
            this.optMiniResultsOff = new System.Windows.Forms.RadioButton();
            this.optMiniResultsOn = new System.Windows.Forms.RadioButton();
            this.grpGem = new System.Windows.Forms.GroupBox();
            this.lblGem = new System.Windows.Forms.Label();
            this.optGemOff = new System.Windows.Forms.RadioButton();
            this.optGemOn = new System.Windows.Forms.RadioButton();
            this.grpLive = new System.Windows.Forms.GroupBox();
            this.lblLive = new System.Windows.Forms.Label();
            this.optLiveOff = new System.Windows.Forms.RadioButton();
            this.optLiveOn = new System.Windows.Forms.RadioButton();
            this.lblPopVote = new System.Windows.Forms.Label();
            this.lblECVote = new System.Windows.Forms.Label();
            this.txtOthPopularVote = new System.Windows.Forms.TextBox();
            this.lblOthECVotes = new System.Windows.Forms.Label();
            this.lblLibertarian = new System.Windows.Forms.Label();
            this.txtGopPopularVote = new System.Windows.Forms.TextBox();
            this.txtDemPopularVote = new System.Windows.Forms.TextBox();
            this.grpBrand = new System.Windows.Forms.GroupBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.optBrandOff = new System.Windows.Forms.RadioButton();
            this.optBrandOn = new System.Windows.Forms.RadioButton();
            this.btnMiniClear = new System.Windows.Forms.Button();
            this.btnMiniVoteShare = new System.Windows.Forms.Button();
            this.btnMiniECVotes = new System.Windows.Forms.Button();
            this.chkMiniMessage = new System.Windows.Forms.CheckBox();
            this.lblMiniMessage = new System.Windows.Forms.Label();
            this.cboMiniMessages = new System.Windows.Forms.ComboBox();
            this.lblNeededToWin = new System.Windows.Forms.Label();
            this.lblRepECVotes = new System.Windows.Forms.Label();
            this.lblRepublican = new System.Windows.Forms.Label();
            this.lblDemECVotes = new System.Windows.Forms.Label();
            this.lblDemocrat = new System.Windows.Forms.Label();
            this.cboVIZEngineMini = new System.Windows.Forms.ComboBox();
            this.cboVIZEngineMain = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.chkMaestroFR = new System.Windows.Forms.CheckBox();
            this.btnTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DgvStates)).BeginInit();
            this.grpCanopyData.SuspendLayout();
            this.grpCanopyTimer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CanopyRefreshSeconds)).BeginInit();
            this.pnlSenateHouse.SuspendLayout();
            this.pnlVIZBackground.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.grpMini.SuspendLayout();
            this.grpMaestroFR.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpMiniBackground.SuspendLayout();
            this.grpMiniProjection.SuspendLayout();
            this.grpZeroMini.SuspendLayout();
            this.grpMiniResults.SuspendLayout();
            this.grpGem.SuspendLayout();
            this.grpLive.SuspendLayout();
            this.grpBrand.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdVIZEngineMiniConnect
            // 
            this.cmdVIZEngineMiniConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdVIZEngineMiniConnect.Location = new System.Drawing.Point(686, 720);
            this.cmdVIZEngineMiniConnect.Name = "cmdVIZEngineMiniConnect";
            this.cmdVIZEngineMiniConnect.Size = new System.Drawing.Size(59, 20);
            this.cmdVIZEngineMiniConnect.TabIndex = 2;
            this.cmdVIZEngineMiniConnect.Text = "Connect";
            this.cmdVIZEngineMiniConnect.UseVisualStyleBackColor = true;
            this.cmdVIZEngineMiniConnect.Click += new System.EventHandler(this.CmdVIZEngineMiniConnect_Click);
            // 
            // cmdVIZEngineMainConnect
            // 
            this.cmdVIZEngineMainConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdVIZEngineMainConnect.Location = new System.Drawing.Point(288, 720);
            this.cmdVIZEngineMainConnect.Name = "cmdVIZEngineMainConnect";
            this.cmdVIZEngineMainConnect.Size = new System.Drawing.Size(59, 20);
            this.cmdVIZEngineMainConnect.TabIndex = 1;
            this.cmdVIZEngineMainConnect.Text = "Connect";
            this.cmdVIZEngineMainConnect.UseVisualStyleBackColor = true;
            this.cmdVIZEngineMainConnect.Click += new System.EventHandler(this.CmdVIZEngineMainConnect_Click);
            // 
            // lblVIZEngineMini
            // 
            this.lblVIZEngineMini.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVIZEngineMini.AutoSize = true;
            this.lblVIZEngineMini.Location = new System.Drawing.Point(408, 704);
            this.lblVIZEngineMini.Name = "lblVIZEngineMini";
            this.lblVIZEngineMini.Size = new System.Drawing.Size(29, 13);
            this.lblVIZEngineMini.TabIndex = 57;
            this.lblVIZEngineMini.Text = "Mini:";
            // 
            // lblVIZEngineMain
            // 
            this.lblVIZEngineMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVIZEngineMain.AutoSize = true;
            this.lblVIZEngineMain.Location = new System.Drawing.Point(13, 703);
            this.lblVIZEngineMain.Name = "lblVIZEngineMain";
            this.lblVIZEngineMain.Size = new System.Drawing.Size(33, 13);
            this.lblVIZEngineMain.TabIndex = 56;
            this.lblVIZEngineMain.Text = "Main:";
            // 
            // DgvStates
            // 
            this.DgvStates.AllowUserToAddRows = false;
            this.DgvStates.AllowUserToDeleteRows = false;
            this.DgvStates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvStates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DgvStates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvStates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStateNumber,
            this.colStateCode,
            this.colStateName,
            this.colTake,
            this.colECVotes,
            this.colInProgress,
            this.colDemocrats,
            this.colRepublicans,
            this.colOther,
            this.colApCall,
            this.colTimeZone,
            this.colWinningParty,
            this.colMini});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvStates.DefaultCellStyle = dataGridViewCellStyle8;
            this.DgvStates.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvStates.Location = new System.Drawing.Point(12, 36);
            this.DgvStates.Name = "DgvStates";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvStates.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.DgvStates.Size = new System.Drawing.Size(766, 664);
            this.DgvStates.TabIndex = 0;
            this.DgvStates.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvStates_CellContentClick);
            this.DgvStates.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvStates_CellLeave);
            this.DgvStates.SelectionChanged += new System.EventHandler(this.DgvStates_SelectionChanged);
            // 
            // colStateNumber
            // 
            this.colStateNumber.HeaderText = "";
            this.colStateNumber.Name = "colStateNumber";
            this.colStateNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colStateNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStateNumber.Width = 30;
            // 
            // colStateCode
            // 
            this.colStateCode.HeaderText = "State Code";
            this.colStateCode.Name = "colStateCode";
            this.colStateCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStateCode.Visible = false;
            // 
            // colStateName
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colStateName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colStateName.HeaderText = "State";
            this.colStateName.Name = "colStateName";
            this.colStateName.ReadOnly = true;
            this.colStateName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStateName.Width = 200;
            // 
            // colTake
            // 
            this.colTake.HeaderText = "Take";
            this.colTake.Name = "colTake";
            this.colTake.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTake.Width = 70;
            // 
            // colECVotes
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colECVotes.DefaultCellStyle = dataGridViewCellStyle3;
            this.colECVotes.HeaderText = "Electoral College Votes";
            this.colECVotes.Name = "colECVotes";
            this.colECVotes.ReadOnly = true;
            this.colECVotes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colECVotes.Width = 70;
            // 
            // colInProgress
            // 
            this.colInProgress.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colInProgress.HeaderText = "In Progress";
            this.colInProgress.Name = "colInProgress";
            this.colInProgress.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colInProgress.Width = 70;
            // 
            // colDemocrats
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            this.colDemocrats.DefaultCellStyle = dataGridViewCellStyle4;
            this.colDemocrats.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colDemocrats.HeaderText = "Democrats";
            this.colDemocrats.Name = "colDemocrats";
            this.colDemocrats.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDemocrats.Width = 70;
            // 
            // colRepublicans
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            this.colRepublicans.DefaultCellStyle = dataGridViewCellStyle5;
            this.colRepublicans.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colRepublicans.HeaderText = "Republicans";
            this.colRepublicans.Name = "colRepublicans";
            this.colRepublicans.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRepublicans.Width = 70;
            // 
            // colOther
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            this.colOther.DefaultCellStyle = dataGridViewCellStyle6;
            this.colOther.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colOther.HeaderText = "Other";
            this.colOther.Name = "colOther";
            this.colOther.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colOther.Width = 70;
            // 
            // colApCall
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colApCall.DefaultCellStyle = dataGridViewCellStyle7;
            this.colApCall.HeaderText = "AP";
            this.colApCall.Name = "colApCall";
            this.colApCall.Width = 50;
            // 
            // colTimeZone
            // 
            this.colTimeZone.DataPropertyName = "Take";
            this.colTimeZone.HeaderText = "Time Zone";
            this.colTimeZone.Name = "colTimeZone";
            this.colTimeZone.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTimeZone.Visible = false;
            // 
            // colWinningParty
            // 
            this.colWinningParty.HeaderText = "Winning Party";
            this.colWinningParty.Name = "colWinningParty";
            this.colWinningParty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colWinningParty.Visible = false;
            // 
            // colMini
            // 
            this.colMini.HeaderText = "Mini";
            this.colMini.Name = "colMini";
            this.colMini.Visible = false;
            this.colMini.Width = 60;
            // 
            // btnRepublicanCall
            // 
            this.btnRepublicanCall.Location = new System.Drawing.Point(1077, 541);
            this.btnRepublicanCall.Name = "btnRepublicanCall";
            this.btnRepublicanCall.Size = new System.Drawing.Size(103, 23);
            this.btnRepublicanCall.TabIndex = 71;
            this.btnRepublicanCall.Text = "Republican Call";
            this.btnRepublicanCall.UseVisualStyleBackColor = true;
            this.btnRepublicanCall.Click += new System.EventHandler(this.BtnRepublicanCall_Click);
            // 
            // btnDemocratCall
            // 
            this.btnDemocratCall.Location = new System.Drawing.Point(968, 541);
            this.btnDemocratCall.Name = "btnDemocratCall";
            this.btnDemocratCall.Size = new System.Drawing.Size(103, 23);
            this.btnDemocratCall.TabIndex = 70;
            this.btnDemocratCall.Text = "Democrat Call";
            this.btnDemocratCall.UseVisualStyleBackColor = true;
            this.btnDemocratCall.Click += new System.EventHandler(this.BtnDemocratCall_Click);
            // 
            // grpCanopyData
            // 
            this.grpCanopyData.Controls.Add(this.lblCanopyStatusValue);
            this.grpCanopyData.Controls.Add(this.lblCanopyStatus);
            this.grpCanopyData.Controls.Add(this.grpCanopyTimer);
            this.grpCanopyData.Controls.Add(this.lblCurrentAPFile);
            this.grpCanopyData.Location = new System.Drawing.Point(784, 625);
            this.grpCanopyData.Name = "grpCanopyData";
            this.grpCanopyData.Size = new System.Drawing.Size(400, 89);
            this.grpCanopyData.TabIndex = 69;
            this.grpCanopyData.TabStop = false;
            this.grpCanopyData.Text = "CBC.CA Canopy Data:";
            // 
            // lblCanopyStatusValue
            // 
            this.lblCanopyStatusValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCanopyStatusValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCanopyStatusValue.Location = new System.Drawing.Point(160, 58);
            this.lblCanopyStatusValue.Name = "lblCanopyStatusValue";
            this.lblCanopyStatusValue.Size = new System.Drawing.Size(100, 20);
            this.lblCanopyStatusValue.TabIndex = 52;
            this.lblCanopyStatusValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCanopyStatus
            // 
            this.lblCanopyStatus.AutoSize = true;
            this.lblCanopyStatus.Location = new System.Drawing.Point(69, 62);
            this.lblCanopyStatus.Name = "lblCanopyStatus";
            this.lblCanopyStatus.Size = new System.Drawing.Size(79, 13);
            this.lblCanopyStatus.TabIndex = 52;
            this.lblCanopyStatus.Text = "Canopy Status:";
            // 
            // grpCanopyTimer
            // 
            this.grpCanopyTimer.BackColor = System.Drawing.Color.Red;
            this.grpCanopyTimer.Controls.Add(this.lblCanopyRefreshInterval);
            this.grpCanopyTimer.Controls.Add(this.CanopyRefreshSeconds);
            this.grpCanopyTimer.Controls.Add(this.optFeedOff);
            this.grpCanopyTimer.Controls.Add(this.optFeedOn);
            this.grpCanopyTimer.Location = new System.Drawing.Point(11, 18);
            this.grpCanopyTimer.Name = "grpCanopyTimer";
            this.grpCanopyTimer.Size = new System.Drawing.Size(372, 37);
            this.grpCanopyTimer.TabIndex = 51;
            this.grpCanopyTimer.TabStop = false;
            // 
            // lblCanopyRefreshInterval
            // 
            this.lblCanopyRefreshInterval.AutoSize = true;
            this.lblCanopyRefreshInterval.ForeColor = System.Drawing.Color.White;
            this.lblCanopyRefreshInterval.Location = new System.Drawing.Point(165, 15);
            this.lblCanopyRefreshInterval.Name = "lblCanopyRefreshInterval";
            this.lblCanopyRefreshInterval.Size = new System.Drawing.Size(133, 13);
            this.lblCanopyRefreshInterval.TabIndex = 51;
            this.lblCanopyRefreshInterval.Text = "Refresh interval (seconds):";
            // 
            // CanopyRefreshSeconds
            // 
            this.CanopyRefreshSeconds.Location = new System.Drawing.Point(297, 13);
            this.CanopyRefreshSeconds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CanopyRefreshSeconds.Name = "CanopyRefreshSeconds";
            this.CanopyRefreshSeconds.Size = new System.Drawing.Size(47, 20);
            this.CanopyRefreshSeconds.TabIndex = 50;
            this.CanopyRefreshSeconds.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // optFeedOff
            // 
            this.optFeedOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.optFeedOff.Checked = true;
            this.optFeedOff.Location = new System.Drawing.Point(56, 10);
            this.optFeedOff.Name = "optFeedOff";
            this.optFeedOff.Size = new System.Drawing.Size(39, 23);
            this.optFeedOff.TabIndex = 49;
            this.optFeedOff.TabStop = true;
            this.optFeedOff.Text = "Off";
            this.optFeedOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optFeedOff.UseVisualStyleBackColor = true;
            this.optFeedOff.CheckedChanged += new System.EventHandler(this.OptFeed_CheckedChanged);
            // 
            // optFeedOn
            // 
            this.optFeedOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.optFeedOn.Location = new System.Drawing.Point(11, 10);
            this.optFeedOn.Name = "optFeedOn";
            this.optFeedOn.Size = new System.Drawing.Size(39, 23);
            this.optFeedOn.TabIndex = 48;
            this.optFeedOn.Text = "On";
            this.optFeedOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optFeedOn.UseVisualStyleBackColor = true;
            this.optFeedOn.CheckedChanged += new System.EventHandler(this.OptFeed_CheckedChanged);
            // 
            // lblCurrentAPFile
            // 
            this.lblCurrentAPFile.AutoSize = true;
            this.lblCurrentAPFile.Location = new System.Drawing.Point(196, 23);
            this.lblCurrentAPFile.Name = "lblCurrentAPFile";
            this.lblCurrentAPFile.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentAPFile.TabIndex = 45;
            // 
            // pnlSenateHouse
            // 
            this.pnlSenateHouse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSenateHouse.Controls.Add(this.lblSOthTotal);
            this.pnlSenateHouse.Controls.Add(this.lblSGopTotal);
            this.pnlSenateHouse.Controls.Add(this.lblSDemTotal);
            this.pnlSenateHouse.Controls.Add(this.lblSOthHold);
            this.pnlSenateHouse.Controls.Add(this.lblSGopHold);
            this.pnlSenateHouse.Controls.Add(this.lblSDemHold);
            this.pnlSenateHouse.Controls.Add(this.lblHouseOth);
            this.pnlSenateHouse.Controls.Add(this.txtHOth);
            this.pnlSenateHouse.Controls.Add(this.lblSenateOth);
            this.pnlSenateHouse.Controls.Add(this.txtSOth);
            this.pnlSenateHouse.Controls.Add(this.lblHouseDemocrat);
            this.pnlSenateHouse.Controls.Add(this.cmdHouse);
            this.pnlSenateHouse.Controls.Add(this.cmdSenate);
            this.pnlSenateHouse.Controls.Add(this.lblHouseRepublican);
            this.pnlSenateHouse.Controls.Add(this.txtHRep);
            this.pnlSenateHouse.Controls.Add(this.txtHDem);
            this.pnlSenateHouse.Controls.Add(this.lblHouse);
            this.pnlSenateHouse.Controls.Add(this.lblSenateDemocrat);
            this.pnlSenateHouse.Controls.Add(this.lblSenateRepublican);
            this.pnlSenateHouse.Controls.Add(this.txtSRep);
            this.pnlSenateHouse.Controls.Add(this.txtSDem);
            this.pnlSenateHouse.Controls.Add(this.lblSenate);
            this.pnlSenateHouse.Location = new System.Drawing.Point(785, 398);
            this.pnlSenateHouse.Name = "pnlSenateHouse";
            this.pnlSenateHouse.Size = new System.Drawing.Size(400, 138);
            this.pnlSenateHouse.TabIndex = 67;
            // 
            // lblSOthTotal
            // 
            this.lblSOthTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSOthTotal.Location = new System.Drawing.Point(165, 74);
            this.lblSOthTotal.Name = "lblSOthTotal";
            this.lblSOthTotal.Size = new System.Drawing.Size(35, 23);
            this.lblSOthTotal.TabIndex = 53;
            this.lblSOthTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSGopTotal
            // 
            this.lblSGopTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSGopTotal.Location = new System.Drawing.Point(165, 50);
            this.lblSGopTotal.Name = "lblSGopTotal";
            this.lblSGopTotal.Size = new System.Drawing.Size(35, 23);
            this.lblSGopTotal.TabIndex = 52;
            this.lblSGopTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSDemTotal
            // 
            this.lblSDemTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSDemTotal.Location = new System.Drawing.Point(165, 22);
            this.lblSDemTotal.Name = "lblSDemTotal";
            this.lblSDemTotal.Size = new System.Drawing.Size(35, 23);
            this.lblSDemTotal.TabIndex = 51;
            this.lblSDemTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSOthHold
            // 
            this.lblSOthHold.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSOthHold.Location = new System.Drawing.Point(80, 76);
            this.lblSOthHold.Name = "lblSOthHold";
            this.lblSOthHold.Size = new System.Drawing.Size(35, 23);
            this.lblSOthHold.TabIndex = 50;
            this.lblSOthHold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSGopHold
            // 
            this.lblSGopHold.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSGopHold.Location = new System.Drawing.Point(80, 50);
            this.lblSGopHold.Name = "lblSGopHold";
            this.lblSGopHold.Size = new System.Drawing.Size(35, 23);
            this.lblSGopHold.TabIndex = 49;
            this.lblSGopHold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSDemHold
            // 
            this.lblSDemHold.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSDemHold.Location = new System.Drawing.Point(80, 24);
            this.lblSDemHold.Name = "lblSDemHold";
            this.lblSDemHold.Size = new System.Drawing.Size(35, 23);
            this.lblSDemHold.TabIndex = 48;
            this.lblSDemHold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHouseOth
            // 
            this.lblHouseOth.AutoSize = true;
            this.lblHouseOth.Location = new System.Drawing.Point(250, 79);
            this.lblHouseOth.Name = "lblHouseOth";
            this.lblHouseOth.Size = new System.Drawing.Size(36, 13);
            this.lblHouseOth.TabIndex = 47;
            this.lblHouseOth.Text = "Other:";
            // 
            // txtHOth
            // 
            this.txtHOth.Location = new System.Drawing.Point(325, 76);
            this.txtHOth.Name = "txtHOth";
            this.txtHOth.Size = new System.Drawing.Size(38, 20);
            this.txtHOth.TabIndex = 5;
            this.txtHOth.Text = "0";
            this.txtHOth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblSenateOth
            // 
            this.lblSenateOth.AutoSize = true;
            this.lblSenateOth.Location = new System.Drawing.Point(9, 79);
            this.lblSenateOth.Name = "lblSenateOth";
            this.lblSenateOth.Size = new System.Drawing.Size(36, 13);
            this.lblSenateOth.TabIndex = 45;
            this.lblSenateOth.Text = "Other:";
            // 
            // txtSOth
            // 
            this.txtSOth.Location = new System.Drawing.Point(121, 76);
            this.txtSOth.Name = "txtSOth";
            this.txtSOth.Size = new System.Drawing.Size(38, 20);
            this.txtSOth.TabIndex = 2;
            this.txtSOth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSOth.TextChanged += new System.EventHandler(this.TxtSOth_TextChanged);
            this.txtSOth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtSOth_KeyPress);
            // 
            // lblHouseDemocrat
            // 
            this.lblHouseDemocrat.AutoSize = true;
            this.lblHouseDemocrat.Location = new System.Drawing.Point(248, 27);
            this.lblHouseDemocrat.Name = "lblHouseDemocrat";
            this.lblHouseDemocrat.Size = new System.Drawing.Size(56, 13);
            this.lblHouseDemocrat.TabIndex = 41;
            this.lblHouseDemocrat.Text = "Democrat:";
            // 
            // cmdHouse
            // 
            this.cmdHouse.Location = new System.Drawing.Point(289, 102);
            this.cmdHouse.Name = "cmdHouse";
            this.cmdHouse.Size = new System.Drawing.Size(75, 23);
            this.cmdHouse.TabIndex = 8;
            this.cmdHouse.Text = "House";
            this.cmdHouse.UseVisualStyleBackColor = true;
            this.cmdHouse.Click += new System.EventHandler(this.CmdHouse_Click);
            // 
            // cmdSenate
            // 
            this.cmdSenate.Location = new System.Drawing.Point(121, 102);
            this.cmdSenate.Name = "cmdSenate";
            this.cmdSenate.Size = new System.Drawing.Size(75, 23);
            this.cmdSenate.TabIndex = 6;
            this.cmdSenate.Text = "Senate";
            this.cmdSenate.UseVisualStyleBackColor = true;
            this.cmdSenate.Click += new System.EventHandler(this.CmdSenate_Click);
            // 
            // lblHouseRepublican
            // 
            this.lblHouseRepublican.AutoSize = true;
            this.lblHouseRepublican.Location = new System.Drawing.Point(248, 53);
            this.lblHouseRepublican.Name = "lblHouseRepublican";
            this.lblHouseRepublican.Size = new System.Drawing.Size(64, 13);
            this.lblHouseRepublican.TabIndex = 42;
            this.lblHouseRepublican.Text = "Republican:";
            // 
            // txtHRep
            // 
            this.txtHRep.Location = new System.Drawing.Point(325, 50);
            this.txtHRep.Name = "txtHRep";
            this.txtHRep.Size = new System.Drawing.Size(38, 20);
            this.txtHRep.TabIndex = 4;
            this.txtHRep.Text = "0";
            this.txtHRep.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtHDem
            // 
            this.txtHDem.Location = new System.Drawing.Point(325, 24);
            this.txtHDem.Name = "txtHDem";
            this.txtHDem.Size = new System.Drawing.Size(38, 20);
            this.txtHDem.TabIndex = 3;
            this.txtHDem.Text = "0";
            this.txtHDem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblHouse
            // 
            this.lblHouse.AutoSize = true;
            this.lblHouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHouse.Location = new System.Drawing.Point(246, 4);
            this.lblHouse.Name = "lblHouse";
            this.lblHouse.Size = new System.Drawing.Size(47, 13);
            this.lblHouse.TabIndex = 38;
            this.lblHouse.Text = "House:";
            // 
            // lblSenateDemocrat
            // 
            this.lblSenateDemocrat.AutoSize = true;
            this.lblSenateDemocrat.Location = new System.Drawing.Point(7, 27);
            this.lblSenateDemocrat.Name = "lblSenateDemocrat";
            this.lblSenateDemocrat.Size = new System.Drawing.Size(56, 13);
            this.lblSenateDemocrat.TabIndex = 36;
            this.lblSenateDemocrat.Text = "Democrat:";
            // 
            // lblSenateRepublican
            // 
            this.lblSenateRepublican.AutoSize = true;
            this.lblSenateRepublican.Location = new System.Drawing.Point(7, 53);
            this.lblSenateRepublican.Name = "lblSenateRepublican";
            this.lblSenateRepublican.Size = new System.Drawing.Size(64, 13);
            this.lblSenateRepublican.TabIndex = 37;
            this.lblSenateRepublican.Text = "Republican:";
            // 
            // txtSRep
            // 
            this.txtSRep.Location = new System.Drawing.Point(121, 50);
            this.txtSRep.Name = "txtSRep";
            this.txtSRep.Size = new System.Drawing.Size(38, 20);
            this.txtSRep.TabIndex = 1;
            this.txtSRep.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSRep.TextChanged += new System.EventHandler(this.TxtSRep_TextChanged);
            this.txtSRep.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtSRep_KeyPress);
            // 
            // txtSDem
            // 
            this.txtSDem.Location = new System.Drawing.Point(121, 24);
            this.txtSDem.Name = "txtSDem";
            this.txtSDem.Size = new System.Drawing.Size(38, 20);
            this.txtSDem.TabIndex = 0;
            this.txtSDem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSDem.TextChanged += new System.EventHandler(this.TxtSDem_TextChanged);
            this.txtSDem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtSDem_KeyPress);
            // 
            // lblSenate
            // 
            this.lblSenate.AutoSize = true;
            this.lblSenate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSenate.Location = new System.Drawing.Point(7, 4);
            this.lblSenate.Name = "lblSenate";
            this.lblSenate.Size = new System.Drawing.Size(51, 13);
            this.lblSenate.TabIndex = 0;
            this.lblSenate.Text = "Senate:";
            // 
            // btnContinue
            // 
            this.btnContinue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinue.Location = new System.Drawing.Point(788, 579);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(115, 28);
            this.btnContinue.TabIndex = 4;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.BtnContinue_Click);
            // 
            // pnlVIZBackground
            // 
            this.pnlVIZBackground.Controls.Add(this.optBackgroundOff);
            this.pnlVIZBackground.Controls.Add(this.optBackgroundOn);
            this.pnlVIZBackground.Location = new System.Drawing.Point(1043, 579);
            this.pnlVIZBackground.Name = "pnlVIZBackground";
            this.pnlVIZBackground.Size = new System.Drawing.Size(141, 28);
            this.pnlVIZBackground.TabIndex = 65;
            // 
            // optBackgroundOff
            // 
            this.optBackgroundOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.optBackgroundOff.Location = new System.Drawing.Point(73, 2);
            this.optBackgroundOff.Name = "optBackgroundOff";
            this.optBackgroundOff.Size = new System.Drawing.Size(62, 23);
            this.optBackgroundOff.TabIndex = 1;
            this.optBackgroundOff.TabStop = true;
            this.optBackgroundOff.Text = "Bkgrd Off";
            this.optBackgroundOff.UseVisualStyleBackColor = true;
            this.optBackgroundOff.CheckedChanged += new System.EventHandler(this.OptBackgroundOff_CheckedChanged);
            // 
            // optBackgroundOn
            // 
            this.optBackgroundOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.optBackgroundOn.Location = new System.Drawing.Point(5, 2);
            this.optBackgroundOn.Name = "optBackgroundOn";
            this.optBackgroundOn.Size = new System.Drawing.Size(62, 23);
            this.optBackgroundOn.TabIndex = 0;
            this.optBackgroundOn.TabStop = true;
            this.optBackgroundOn.Text = "Bkgrd On";
            this.optBackgroundOn.UseVisualStyleBackColor = true;
            this.optBackgroundOn.CheckedChanged += new System.EventHandler(this.OptBackgroundOn_CheckedChanged);
            // 
            // cmdMapPrevious
            // 
            this.cmdMapPrevious.Location = new System.Drawing.Point(869, 541);
            this.cmdMapPrevious.Name = "cmdMapPrevious";
            this.cmdMapPrevious.Size = new System.Drawing.Size(75, 23);
            this.cmdMapPrevious.TabIndex = 6;
            this.cmdMapPrevious.Text = "Map (2012)";
            this.cmdMapPrevious.UseVisualStyleBackColor = true;
            this.cmdMapPrevious.Click += new System.EventHandler(this.CmdMapPrevious_Click);
            // 
            // cmdMap
            // 
            this.cmdMap.Location = new System.Drawing.Point(788, 541);
            this.cmdMap.Name = "cmdMap";
            this.cmdMap.Size = new System.Drawing.Size(75, 23);
            this.cmdMap.TabIndex = 5;
            this.cmdMap.Text = "Map (2016)";
            this.cmdMap.UseVisualStyleBackColor = true;
            this.cmdMap.Click += new System.EventHandler(this.CmdMap_Click);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.cmdTimeZone1);
            this.GroupBox1.Controls.Add(this.cmdTimeZone9);
            this.GroupBox1.Controls.Add(this.cmdTimeZone2);
            this.GroupBox1.Controls.Add(this.cmdTimeZone8);
            this.GroupBox1.Controls.Add(this.cmdTimeZone3);
            this.GroupBox1.Controls.Add(this.cmdTimeZone7);
            this.GroupBox1.Controls.Add(this.cmdTimeZone4);
            this.GroupBox1.Controls.Add(this.cmdTimeZone6);
            this.GroupBox1.Controls.Add(this.cmdTimeZone5);
            this.GroupBox1.Location = new System.Drawing.Point(784, 36);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(400, 81);
            this.GroupBox1.TabIndex = 64;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Poll Closing Time (EST):";
            // 
            // cmdTimeZone1
            // 
            this.cmdTimeZone1.Location = new System.Drawing.Point(18, 19);
            this.cmdTimeZone1.Name = "cmdTimeZone1";
            this.cmdTimeZone1.Size = new System.Drawing.Size(64, 23);
            this.cmdTimeZone1.TabIndex = 0;
            this.cmdTimeZone1.Text = "1800h";
            this.cmdTimeZone1.UseVisualStyleBackColor = true;
            this.cmdTimeZone1.Click += new System.EventHandler(this.CmdTimeZone1_Click);
            // 
            // cmdTimeZone9
            // 
            this.cmdTimeZone9.Location = new System.Drawing.Point(300, 19);
            this.cmdTimeZone9.Name = "cmdTimeZone9";
            this.cmdTimeZone9.Size = new System.Drawing.Size(64, 23);
            this.cmdTimeZone9.TabIndex = 8;
            this.cmdTimeZone9.Text = "2400h";
            this.cmdTimeZone9.UseVisualStyleBackColor = true;
            this.cmdTimeZone9.Click += new System.EventHandler(this.CmdTimeZone9_Click);
            // 
            // cmdTimeZone2
            // 
            this.cmdTimeZone2.Location = new System.Drawing.Point(18, 48);
            this.cmdTimeZone2.Name = "cmdTimeZone2";
            this.cmdTimeZone2.Size = new System.Drawing.Size(64, 23);
            this.cmdTimeZone2.TabIndex = 1;
            this.cmdTimeZone2.Text = "1900h";
            this.cmdTimeZone2.UseVisualStyleBackColor = true;
            this.cmdTimeZone2.Click += new System.EventHandler(this.CmdTimeZone2_Click);
            // 
            // cmdTimeZone8
            // 
            this.cmdTimeZone8.Location = new System.Drawing.Point(230, 48);
            this.cmdTimeZone8.Name = "cmdTimeZone8";
            this.cmdTimeZone8.Size = new System.Drawing.Size(64, 23);
            this.cmdTimeZone8.TabIndex = 7;
            this.cmdTimeZone8.Text = "2300h";
            this.cmdTimeZone8.UseVisualStyleBackColor = true;
            this.cmdTimeZone8.Click += new System.EventHandler(this.CmdTimeZone8_Click);
            // 
            // cmdTimeZone3
            // 
            this.cmdTimeZone3.Location = new System.Drawing.Point(88, 19);
            this.cmdTimeZone3.Name = "cmdTimeZone3";
            this.cmdTimeZone3.Size = new System.Drawing.Size(64, 23);
            this.cmdTimeZone3.TabIndex = 2;
            this.cmdTimeZone3.Text = "1930h";
            this.cmdTimeZone3.UseVisualStyleBackColor = true;
            this.cmdTimeZone3.Click += new System.EventHandler(this.CmdTimeZone3_Click);
            // 
            // cmdTimeZone7
            // 
            this.cmdTimeZone7.Location = new System.Drawing.Point(230, 19);
            this.cmdTimeZone7.Name = "cmdTimeZone7";
            this.cmdTimeZone7.Size = new System.Drawing.Size(64, 23);
            this.cmdTimeZone7.TabIndex = 6;
            this.cmdTimeZone7.Text = "2200h";
            this.cmdTimeZone7.UseVisualStyleBackColor = true;
            this.cmdTimeZone7.Click += new System.EventHandler(this.CmdTimeZone7_Click);
            // 
            // cmdTimeZone4
            // 
            this.cmdTimeZone4.Location = new System.Drawing.Point(88, 48);
            this.cmdTimeZone4.Name = "cmdTimeZone4";
            this.cmdTimeZone4.Size = new System.Drawing.Size(64, 23);
            this.cmdTimeZone4.TabIndex = 3;
            this.cmdTimeZone4.Text = "2000h";
            this.cmdTimeZone4.UseVisualStyleBackColor = true;
            this.cmdTimeZone4.Click += new System.EventHandler(this.CmdTimeZone4_Click);
            // 
            // cmdTimeZone6
            // 
            this.cmdTimeZone6.Location = new System.Drawing.Point(160, 48);
            this.cmdTimeZone6.Name = "cmdTimeZone6";
            this.cmdTimeZone6.Size = new System.Drawing.Size(64, 23);
            this.cmdTimeZone6.TabIndex = 5;
            this.cmdTimeZone6.Text = "2100h";
            this.cmdTimeZone6.UseVisualStyleBackColor = true;
            this.cmdTimeZone6.Click += new System.EventHandler(this.CmdTimeZone6_Click);
            // 
            // cmdTimeZone5
            // 
            this.cmdTimeZone5.Location = new System.Drawing.Point(160, 19);
            this.cmdTimeZone5.Name = "cmdTimeZone5";
            this.cmdTimeZone5.Size = new System.Drawing.Size(64, 23);
            this.cmdTimeZone5.TabIndex = 4;
            this.cmdTimeZone5.Text = "2030h";
            this.cmdTimeZone5.UseVisualStyleBackColor = true;
            this.cmdTimeZone5.Click += new System.EventHandler(this.CmdTimeZone5_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1188, 24);
            this.menuStrip1.TabIndex = 73;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLogToolStripMenuItem,
            this.allowCanopyUpdatesToolStripMenuItem,
            this.disallowCanopyUpdatesToolStripMenuItem,
            this.resetResultsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.importStateGridToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // viewLogToolStripMenuItem
            // 
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.viewLogToolStripMenuItem.Text = "&View Log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.ViewLogToolStripMenuItem_Click);
            // 
            // allowCanopyUpdatesToolStripMenuItem
            // 
            this.allowCanopyUpdatesToolStripMenuItem.Name = "allowCanopyUpdatesToolStripMenuItem";
            this.allowCanopyUpdatesToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.allowCanopyUpdatesToolStripMenuItem.Text = "Allow Canopy Updates";
            this.allowCanopyUpdatesToolStripMenuItem.Click += new System.EventHandler(this.AllowCanopyUpdatesToolStripMenuItem_Click);
            // 
            // disallowCanopyUpdatesToolStripMenuItem
            // 
            this.disallowCanopyUpdatesToolStripMenuItem.Name = "disallowCanopyUpdatesToolStripMenuItem";
            this.disallowCanopyUpdatesToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.disallowCanopyUpdatesToolStripMenuItem.Text = "Disallow Canopy Updates";
            this.disallowCanopyUpdatesToolStripMenuItem.Click += new System.EventHandler(this.DisallowCanopyUpdatesToolStripMenuItem_Click);
            // 
            // resetResultsToolStripMenuItem
            // 
            this.resetResultsToolStripMenuItem.Name = "resetResultsToolStripMenuItem";
            this.resetResultsToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.resetResultsToolStripMenuItem.Text = "Reset Results";
            this.resetResultsToolStripMenuItem.Click += new System.EventHandler(this.ResetResultsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(205, 6);
            // 
            // importStateGridToolStripMenuItem
            // 
            this.importStateGridToolStripMenuItem.Name = "importStateGridToolStripMenuItem";
            this.importStateGridToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.importStateGridToolStripMenuItem.Text = "Import State Elects";
            this.importStateGridToolStripMenuItem.Click += new System.EventHandler(this.ImportStateElectsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(205, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // grpMini
            // 
            this.grpMini.Controls.Add(this.grpMaestroFR);
            this.grpMini.Controls.Add(this.grpMiniBackground);
            this.grpMini.Controls.Add(this.grpMiniProjection);
            this.grpMini.Controls.Add(this.grpZeroMini);
            this.grpMini.Controls.Add(this.grpMiniResults);
            this.grpMini.Controls.Add(this.grpGem);
            this.grpMini.Controls.Add(this.grpLive);
            this.grpMini.Controls.Add(this.lblPopVote);
            this.grpMini.Controls.Add(this.lblECVote);
            this.grpMini.Controls.Add(this.txtOthPopularVote);
            this.grpMini.Controls.Add(this.lblOthECVotes);
            this.grpMini.Controls.Add(this.lblLibertarian);
            this.grpMini.Controls.Add(this.txtGopPopularVote);
            this.grpMini.Controls.Add(this.txtDemPopularVote);
            this.grpMini.Controls.Add(this.grpBrand);
            this.grpMini.Controls.Add(this.btnMiniClear);
            this.grpMini.Controls.Add(this.btnMiniVoteShare);
            this.grpMini.Controls.Add(this.btnMiniECVotes);
            this.grpMini.Controls.Add(this.chkMiniMessage);
            this.grpMini.Controls.Add(this.lblMiniMessage);
            this.grpMini.Controls.Add(this.cboMiniMessages);
            this.grpMini.Controls.Add(this.lblNeededToWin);
            this.grpMini.Controls.Add(this.lblRepECVotes);
            this.grpMini.Controls.Add(this.lblRepublican);
            this.grpMini.Controls.Add(this.lblDemECVotes);
            this.grpMini.Controls.Add(this.lblDemocrat);
            this.grpMini.Location = new System.Drawing.Point(784, 123);
            this.grpMini.Name = "grpMini";
            this.grpMini.Size = new System.Drawing.Size(400, 268);
            this.grpMini.TabIndex = 76;
            this.grpMini.TabStop = false;
            this.grpMini.Text = "Mini";
            // 
            // grpMaestroFR
            // 
            this.grpMaestroFR.Controls.Add(this.chkAnimatedBG);
            this.grpMaestroFR.Controls.Add(this.chkMapYear);
            this.grpMaestroFR.Controls.Add(this.panel1);
            this.grpMaestroFR.Controls.Add(this.btnResetMain);
            this.grpMaestroFR.Controls.Add(this.btnResetMini);
            this.grpMaestroFR.Controls.Add(this.btnStanding);
            this.grpMaestroFR.Location = new System.Drawing.Point(1, 132);
            this.grpMaestroFR.Name = "grpMaestroFR";
            this.grpMaestroFR.Size = new System.Drawing.Size(399, 135);
            this.grpMaestroFR.TabIndex = 83;
            this.grpMaestroFR.TabStop = false;
            this.grpMaestroFR.Text = "Maestro français";
            this.grpMaestroFR.Visible = false;
            // 
            // chkAnimatedBG
            // 
            this.chkAnimatedBG.AutoSize = true;
            this.chkAnimatedBG.Checked = true;
            this.chkAnimatedBG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnimatedBG.Location = new System.Drawing.Point(304, 31);
            this.chkAnimatedBG.Name = "chkAnimatedBG";
            this.chkAnimatedBG.Size = new System.Drawing.Size(81, 17);
            this.chkAnimatedBG.TabIndex = 87;
            this.chkAnimatedBG.Text = "Fond animé";
            this.chkAnimatedBG.UseVisualStyleBackColor = true;
            this.chkAnimatedBG.CheckedChanged += new System.EventHandler(this.ChkAnimatedBG_CheckedChanged);
            // 
            // chkMapYear
            // 
            this.chkMapYear.AutoSize = true;
            this.chkMapYear.Location = new System.Drawing.Point(24, 66);
            this.chkMapYear.Name = "chkMapYear";
            this.chkMapYear.Size = new System.Drawing.Size(87, 17);
            this.chkMapYear.TabIndex = 86;
            this.chkMapYear.Text = "Année/Carte";
            this.chkMapYear.UseVisualStyleBackColor = true;
            this.chkMapYear.CheckedChanged += new System.EventHandler(this.ChkMapYear_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.optMapOff);
            this.panel1.Controls.Add(this.optMapOn);
            this.panel1.Location = new System.Drawing.Point(16, 95);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(141, 28);
            this.panel1.TabIndex = 85;
            // 
            // optMapOff
            // 
            this.optMapOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.optMapOff.Location = new System.Drawing.Point(73, 2);
            this.optMapOff.Name = "optMapOff";
            this.optMapOff.Size = new System.Drawing.Size(62, 23);
            this.optMapOff.TabIndex = 1;
            this.optMapOff.TabStop = true;
            this.optMapOff.Text = "BG Off";
            this.optMapOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optMapOff.UseVisualStyleBackColor = true;
            this.optMapOff.CheckedChanged += new System.EventHandler(this.OptMapOff_CheckedChanged);
            // 
            // optMapOn
            // 
            this.optMapOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.optMapOn.Location = new System.Drawing.Point(5, 2);
            this.optMapOn.Name = "optMapOn";
            this.optMapOn.Size = new System.Drawing.Size(62, 23);
            this.optMapOn.TabIndex = 0;
            this.optMapOn.TabStop = true;
            this.optMapOn.Text = "BG On";
            this.optMapOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optMapOn.UseVisualStyleBackColor = true;
            this.optMapOn.CheckedChanged += new System.EventHandler(this.OptMapOn_CheckedChanged);
            // 
            // btnResetMain
            // 
            this.btnResetMain.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnResetMain.Location = new System.Drawing.Point(310, 74);
            this.btnResetMain.Name = "btnResetMain";
            this.btnResetMain.Size = new System.Drawing.Size(75, 23);
            this.btnResetMain.TabIndex = 84;
            this.btnResetMain.Text = "Reset Main";
            this.btnResetMain.UseVisualStyleBackColor = false;
            this.btnResetMain.Click += new System.EventHandler(this.BtnResetMain_Click);
            // 
            // btnResetMini
            // 
            this.btnResetMini.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnResetMini.Location = new System.Drawing.Point(310, 103);
            this.btnResetMini.Name = "btnResetMini";
            this.btnResetMini.Size = new System.Drawing.Size(75, 23);
            this.btnResetMini.TabIndex = 83;
            this.btnResetMini.Text = "Reset Mini";
            this.btnResetMini.UseVisualStyleBackColor = false;
            this.btnResetMini.Click += new System.EventHandler(this.BtnResetMini_Click);
            // 
            // btnStanding
            // 
            this.btnStanding.Location = new System.Drawing.Point(17, 27);
            this.btnStanding.Name = "btnStanding";
            this.btnStanding.Size = new System.Drawing.Size(75, 23);
            this.btnStanding.TabIndex = 82;
            this.btnStanding.Text = "Standing";
            this.btnStanding.UseVisualStyleBackColor = true;
            this.btnStanding.Click += new System.EventHandler(this.BtnStanding_Click);
            // 
            // grpMiniBackground
            // 
            this.grpMiniBackground.Controls.Add(this.cmdMiniBackgroundOff);
            this.grpMiniBackground.Controls.Add(this.lblMiniBackground);
            this.grpMiniBackground.Location = new System.Drawing.Point(160, 235);
            this.grpMiniBackground.Name = "grpMiniBackground";
            this.grpMiniBackground.Size = new System.Drawing.Size(232, 32);
            this.grpMiniBackground.TabIndex = 77;
            this.grpMiniBackground.TabStop = false;
            // 
            // cmdMiniBackgroundOff
            // 
            this.cmdMiniBackgroundOff.Location = new System.Drawing.Point(138, 8);
            this.cmdMiniBackgroundOff.Name = "cmdMiniBackgroundOff";
            this.cmdMiniBackgroundOff.Size = new System.Drawing.Size(45, 23);
            this.cmdMiniBackgroundOff.TabIndex = 3;
            this.cmdMiniBackgroundOff.Text = "Off";
            this.cmdMiniBackgroundOff.UseVisualStyleBackColor = true;
            this.cmdMiniBackgroundOff.Click += new System.EventHandler(this.CmdMiniBackgroundOff_Click);
            // 
            // lblMiniBackground
            // 
            this.lblMiniBackground.AutoSize = true;
            this.lblMiniBackground.Location = new System.Drawing.Point(18, 13);
            this.lblMiniBackground.Name = "lblMiniBackground";
            this.lblMiniBackground.Size = new System.Drawing.Size(68, 13);
            this.lblMiniBackground.TabIndex = 2;
            this.lblMiniBackground.Text = "Background:";
            // 
            // grpMiniProjection
            // 
            this.grpMiniProjection.Controls.Add(this.lblMiniProjection);
            this.grpMiniProjection.Controls.Add(this.optMiniProjectionOff);
            this.grpMiniProjection.Controls.Add(this.optMiniProjectionOn);
            this.grpMiniProjection.Location = new System.Drawing.Point(160, 198);
            this.grpMiniProjection.Name = "grpMiniProjection";
            this.grpMiniProjection.Size = new System.Drawing.Size(232, 32);
            this.grpMiniProjection.TabIndex = 76;
            this.grpMiniProjection.TabStop = false;
            // 
            // lblMiniProjection
            // 
            this.lblMiniProjection.AutoSize = true;
            this.lblMiniProjection.Location = new System.Drawing.Point(29, 13);
            this.lblMiniProjection.Name = "lblMiniProjection";
            this.lblMiniProjection.Size = new System.Drawing.Size(57, 13);
            this.lblMiniProjection.TabIndex = 2;
            this.lblMiniProjection.Text = "Projection:";
            // 
            // optMiniProjectionOff
            // 
            this.optMiniProjectionOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.optMiniProjectionOff.Location = new System.Drawing.Point(138, 8);
            this.optMiniProjectionOff.Name = "optMiniProjectionOff";
            this.optMiniProjectionOff.Size = new System.Drawing.Size(45, 23);
            this.optMiniProjectionOff.TabIndex = 0;
            this.optMiniProjectionOff.TabStop = true;
            this.optMiniProjectionOff.Text = "Off";
            this.optMiniProjectionOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optMiniProjectionOff.UseVisualStyleBackColor = true;
            this.optMiniProjectionOff.CheckedChanged += new System.EventHandler(this.OptMiniProjectionOff_CheckedChanged);
            // 
            // optMiniProjectionOn
            // 
            this.optMiniProjectionOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.optMiniProjectionOn.Location = new System.Drawing.Point(92, 8);
            this.optMiniProjectionOn.Name = "optMiniProjectionOn";
            this.optMiniProjectionOn.Size = new System.Drawing.Size(45, 23);
            this.optMiniProjectionOn.TabIndex = 1;
            this.optMiniProjectionOn.TabStop = true;
            this.optMiniProjectionOn.Text = "On";
            this.optMiniProjectionOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optMiniProjectionOn.UseVisualStyleBackColor = true;
            this.optMiniProjectionOn.CheckedChanged += new System.EventHandler(this.OptMiniProjectionOn_CheckedChanged);
            // 
            // grpZeroMini
            // 
            this.grpZeroMini.Controls.Add(this.optZeroMiniOff);
            this.grpZeroMini.Controls.Add(this.optZeroMiniOn);
            this.grpZeroMini.Location = new System.Drawing.Point(275, 89);
            this.grpZeroMini.Name = "grpZeroMini";
            this.grpZeroMini.Size = new System.Drawing.Size(111, 40);
            this.grpZeroMini.TabIndex = 75;
            this.grpZeroMini.TabStop = false;
            this.grpZeroMini.Text = "Zero Mini:";
            // 
            // optZeroMiniOff
            // 
            this.optZeroMiniOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.optZeroMiniOff.Location = new System.Drawing.Point(57, 11);
            this.optZeroMiniOff.Name = "optZeroMiniOff";
            this.optZeroMiniOff.Size = new System.Drawing.Size(45, 23);
            this.optZeroMiniOff.TabIndex = 0;
            this.optZeroMiniOff.TabStop = true;
            this.optZeroMiniOff.Text = "Off";
            this.optZeroMiniOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optZeroMiniOff.UseVisualStyleBackColor = true;
            this.optZeroMiniOff.CheckedChanged += new System.EventHandler(this.OptZeroMiniOff_CheckedChanged);
            // 
            // optZeroMiniOn
            // 
            this.optZeroMiniOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.optZeroMiniOn.Location = new System.Drawing.Point(6, 11);
            this.optZeroMiniOn.Name = "optZeroMiniOn";
            this.optZeroMiniOn.Size = new System.Drawing.Size(45, 23);
            this.optZeroMiniOn.TabIndex = 1;
            this.optZeroMiniOn.TabStop = true;
            this.optZeroMiniOn.Text = "On";
            this.optZeroMiniOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optZeroMiniOn.UseVisualStyleBackColor = true;
            this.optZeroMiniOn.CheckedChanged += new System.EventHandler(this.OptZeroMiniOn_CheckedChanged);
            // 
            // grpMiniResults
            // 
            this.grpMiniResults.Controls.Add(this.lblUSStateCodeResults);
            this.grpMiniResults.Controls.Add(this.lblMiniResults);
            this.grpMiniResults.Controls.Add(this.optMiniResultsOff);
            this.grpMiniResults.Controls.Add(this.optMiniResultsOn);
            this.grpMiniResults.Location = new System.Drawing.Point(161, 159);
            this.grpMiniResults.Name = "grpMiniResults";
            this.grpMiniResults.Size = new System.Drawing.Size(231, 32);
            this.grpMiniResults.TabIndex = 74;
            this.grpMiniResults.TabStop = false;
            // 
            // lblUSStateCodeResults
            // 
            this.lblUSStateCodeResults.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblUSStateCodeResults.Location = new System.Drawing.Point(188, 11);
            this.lblUSStateCodeResults.Name = "lblUSStateCodeResults";
            this.lblUSStateCodeResults.Size = new System.Drawing.Size(34, 18);
            this.lblUSStateCodeResults.TabIndex = 78;
            this.lblUSStateCodeResults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMiniResults
            // 
            this.lblMiniResults.AutoSize = true;
            this.lblMiniResults.Location = new System.Drawing.Point(12, 14);
            this.lblMiniResults.Name = "lblMiniResults";
            this.lblMiniResults.Size = new System.Drawing.Size(73, 13);
            this.lblMiniResults.TabIndex = 2;
            this.lblMiniResults.Text = "State Results:";
            // 
            // optMiniResultsOff
            // 
            this.optMiniResultsOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.optMiniResultsOff.Location = new System.Drawing.Point(137, 9);
            this.optMiniResultsOff.Name = "optMiniResultsOff";
            this.optMiniResultsOff.Size = new System.Drawing.Size(45, 23);
            this.optMiniResultsOff.TabIndex = 0;
            this.optMiniResultsOff.TabStop = true;
            this.optMiniResultsOff.Text = "Off";
            this.optMiniResultsOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optMiniResultsOff.UseVisualStyleBackColor = true;
            this.optMiniResultsOff.CheckedChanged += new System.EventHandler(this.OptMiniResultsOff_CheckedChanged);
            // 
            // optMiniResultsOn
            // 
            this.optMiniResultsOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.optMiniResultsOn.Location = new System.Drawing.Point(91, 9);
            this.optMiniResultsOn.Name = "optMiniResultsOn";
            this.optMiniResultsOn.Size = new System.Drawing.Size(45, 23);
            this.optMiniResultsOn.TabIndex = 1;
            this.optMiniResultsOn.TabStop = true;
            this.optMiniResultsOn.Text = "On";
            this.optMiniResultsOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optMiniResultsOn.UseVisualStyleBackColor = true;
            this.optMiniResultsOn.CheckedChanged += new System.EventHandler(this.OptMiniResultsOn_CheckedChanged);
            // 
            // grpGem
            // 
            this.grpGem.Controls.Add(this.lblGem);
            this.grpGem.Controls.Add(this.optGemOff);
            this.grpGem.Controls.Add(this.optGemOn);
            this.grpGem.Location = new System.Drawing.Point(11, 231);
            this.grpGem.Name = "grpGem";
            this.grpGem.Size = new System.Drawing.Size(144, 32);
            this.grpGem.TabIndex = 73;
            this.grpGem.TabStop = false;
            // 
            // lblGem
            // 
            this.lblGem.AutoSize = true;
            this.lblGem.Location = new System.Drawing.Point(9, 14);
            this.lblGem.Name = "lblGem";
            this.lblGem.Size = new System.Drawing.Size(32, 13);
            this.lblGem.TabIndex = 2;
            this.lblGem.Text = "Gem:";
            // 
            // optGemOff
            // 
            this.optGemOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.optGemOff.Location = new System.Drawing.Point(92, 8);
            this.optGemOff.Name = "optGemOff";
            this.optGemOff.Size = new System.Drawing.Size(45, 23);
            this.optGemOff.TabIndex = 0;
            this.optGemOff.TabStop = true;
            this.optGemOff.Text = "Off";
            this.optGemOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optGemOff.UseVisualStyleBackColor = true;
            this.optGemOff.CheckedChanged += new System.EventHandler(this.OptGemOff_CheckedChanged);
            // 
            // optGemOn
            // 
            this.optGemOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.optGemOn.Location = new System.Drawing.Point(45, 9);
            this.optGemOn.Name = "optGemOn";
            this.optGemOn.Size = new System.Drawing.Size(45, 23);
            this.optGemOn.TabIndex = 1;
            this.optGemOn.TabStop = true;
            this.optGemOn.Text = "On";
            this.optGemOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optGemOn.UseVisualStyleBackColor = true;
            this.optGemOn.CheckedChanged += new System.EventHandler(this.OptGemOn_CheckedChanged);
            // 
            // grpLive
            // 
            this.grpLive.Controls.Add(this.lblLive);
            this.grpLive.Controls.Add(this.optLiveOff);
            this.grpLive.Controls.Add(this.optLiveOn);
            this.grpLive.Location = new System.Drawing.Point(6, 197);
            this.grpLive.Name = "grpLive";
            this.grpLive.Size = new System.Drawing.Size(149, 33);
            this.grpLive.TabIndex = 72;
            this.grpLive.TabStop = false;
            // 
            // lblLive
            // 
            this.lblLive.AutoSize = true;
            this.lblLive.Location = new System.Drawing.Point(16, 14);
            this.lblLive.Name = "lblLive";
            this.lblLive.Size = new System.Drawing.Size(30, 13);
            this.lblLive.TabIndex = 2;
            this.lblLive.Text = "Live:";
            // 
            // optLiveOff
            // 
            this.optLiveOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.optLiveOff.Location = new System.Drawing.Point(97, 9);
            this.optLiveOff.Name = "optLiveOff";
            this.optLiveOff.Size = new System.Drawing.Size(45, 23);
            this.optLiveOff.TabIndex = 0;
            this.optLiveOff.TabStop = true;
            this.optLiveOff.Text = "Off";
            this.optLiveOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optLiveOff.UseVisualStyleBackColor = true;
            this.optLiveOff.CheckedChanged += new System.EventHandler(this.OptLiveOff_CheckedChanged);
            // 
            // optLiveOn
            // 
            this.optLiveOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.optLiveOn.Location = new System.Drawing.Point(50, 9);
            this.optLiveOn.Name = "optLiveOn";
            this.optLiveOn.Size = new System.Drawing.Size(45, 23);
            this.optLiveOn.TabIndex = 1;
            this.optLiveOn.TabStop = true;
            this.optLiveOn.Text = "On";
            this.optLiveOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optLiveOn.UseVisualStyleBackColor = true;
            this.optLiveOn.CheckedChanged += new System.EventHandler(this.OptLiveOn_CheckedChanged);
            // 
            // lblPopVote
            // 
            this.lblPopVote.AutoSize = true;
            this.lblPopVote.Location = new System.Drawing.Point(22, 67);
            this.lblPopVote.Name = "lblPopVote";
            this.lblPopVote.Size = new System.Drawing.Size(109, 13);
            this.lblPopVote.TabIndex = 71;
            this.lblPopVote.Text = "Vote Share (Percent):";
            // 
            // lblECVote
            // 
            this.lblECVote.AutoSize = true;
            this.lblECVote.Location = new System.Drawing.Point(37, 44);
            this.lblECVote.Name = "lblECVote";
            this.lblECVote.Size = new System.Drawing.Size(89, 13);
            this.lblECVote.TabIndex = 70;
            this.lblECVote.Text = "Electoral College:";
            // 
            // txtOthPopularVote
            // 
            this.txtOthPopularVote.Location = new System.Drawing.Point(304, 64);
            this.txtOthPopularVote.Name = "txtOthPopularVote";
            this.txtOthPopularVote.Size = new System.Drawing.Size(71, 20);
            this.txtOthPopularVote.TabIndex = 57;
            this.txtOthPopularVote.Text = "0";
            this.txtOthPopularVote.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtOthPopularVote.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtOthPopularVote_KeyPress);
            this.txtOthPopularVote.Leave += new System.EventHandler(this.TxtOthPopularVote_Leave);
            // 
            // lblOthECVotes
            // 
            this.lblOthECVotes.BackColor = System.Drawing.Color.DarkGray;
            this.lblOthECVotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOthECVotes.ForeColor = System.Drawing.Color.White;
            this.lblOthECVotes.Location = new System.Drawing.Point(304, 38);
            this.lblOthECVotes.Name = "lblOthECVotes";
            this.lblOthECVotes.Size = new System.Drawing.Size(71, 23);
            this.lblOthECVotes.TabIndex = 58;
            this.lblOthECVotes.Text = "0";
            this.lblOthECVotes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLibertarian
            // 
            this.lblLibertarian.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibertarian.Location = new System.Drawing.Point(300, 16);
            this.lblLibertarian.Name = "lblLibertarian";
            this.lblLibertarian.Size = new System.Drawing.Size(75, 20);
            this.lblLibertarian.TabIndex = 69;
            this.lblLibertarian.Text = "Other";
            this.lblLibertarian.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtGopPopularVote
            // 
            this.txtGopPopularVote.Location = new System.Drawing.Point(223, 64);
            this.txtGopPopularVote.Name = "txtGopPopularVote";
            this.txtGopPopularVote.Size = new System.Drawing.Size(71, 20);
            this.txtGopPopularVote.TabIndex = 56;
            this.txtGopPopularVote.Text = "0";
            this.txtGopPopularVote.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGopPopularVote.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtGopPopularVote_KeyPress);
            this.txtGopPopularVote.Leave += new System.EventHandler(this.TxtGopPopularVote_Leave);
            // 
            // txtDemPopularVote
            // 
            this.txtDemPopularVote.Location = new System.Drawing.Point(142, 64);
            this.txtDemPopularVote.Name = "txtDemPopularVote";
            this.txtDemPopularVote.Size = new System.Drawing.Size(71, 20);
            this.txtDemPopularVote.TabIndex = 53;
            this.txtDemPopularVote.Text = "0";
            this.txtDemPopularVote.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDemPopularVote.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtDemPopularVote_KeyPress);
            this.txtDemPopularVote.Leave += new System.EventHandler(this.TxtDemPopularVote_Leave);
            // 
            // grpBrand
            // 
            this.grpBrand.Controls.Add(this.lblBrand);
            this.grpBrand.Controls.Add(this.optBrandOff);
            this.grpBrand.Controls.Add(this.optBrandOn);
            this.grpBrand.Location = new System.Drawing.Point(6, 159);
            this.grpBrand.Name = "grpBrand";
            this.grpBrand.Size = new System.Drawing.Size(149, 32);
            this.grpBrand.TabIndex = 68;
            this.grpBrand.TabStop = false;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(8, 14);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(38, 13);
            this.lblBrand.TabIndex = 2;
            this.lblBrand.Text = "Brand:";
            // 
            // optBrandOff
            // 
            this.optBrandOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.optBrandOff.Location = new System.Drawing.Point(97, 9);
            this.optBrandOff.Name = "optBrandOff";
            this.optBrandOff.Size = new System.Drawing.Size(45, 23);
            this.optBrandOff.TabIndex = 0;
            this.optBrandOff.TabStop = true;
            this.optBrandOff.Text = "Off";
            this.optBrandOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optBrandOff.UseVisualStyleBackColor = true;
            this.optBrandOff.CheckedChanged += new System.EventHandler(this.OptBrandOff_CheckedChanged);
            // 
            // optBrandOn
            // 
            this.optBrandOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.optBrandOn.Location = new System.Drawing.Point(50, 9);
            this.optBrandOn.Name = "optBrandOn";
            this.optBrandOn.Size = new System.Drawing.Size(45, 23);
            this.optBrandOn.TabIndex = 1;
            this.optBrandOn.TabStop = true;
            this.optBrandOn.Text = "On";
            this.optBrandOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.optBrandOn.UseVisualStyleBackColor = true;
            this.optBrandOn.CheckedChanged += new System.EventHandler(this.OptBrandOn_CheckedChanged);
            // 
            // btnMiniClear
            // 
            this.btnMiniClear.Location = new System.Drawing.Point(189, 102);
            this.btnMiniClear.Name = "btnMiniClear";
            this.btnMiniClear.Size = new System.Drawing.Size(80, 24);
            this.btnMiniClear.TabIndex = 61;
            this.btnMiniClear.Text = "Mini - Clear";
            this.btnMiniClear.UseVisualStyleBackColor = true;
            this.btnMiniClear.Click += new System.EventHandler(this.BtnMiniClear_Click);
            // 
            // btnMiniVoteShare
            // 
            this.btnMiniVoteShare.Location = new System.Drawing.Point(103, 102);
            this.btnMiniVoteShare.Name = "btnMiniVoteShare";
            this.btnMiniVoteShare.Size = new System.Drawing.Size(80, 24);
            this.btnMiniVoteShare.TabIndex = 60;
            this.btnMiniVoteShare.Text = "Mini Share";
            this.btnMiniVoteShare.UseVisualStyleBackColor = true;
            this.btnMiniVoteShare.Click += new System.EventHandler(this.BtnMiniVoteShare_Click);
            // 
            // btnMiniECVotes
            // 
            this.btnMiniECVotes.Location = new System.Drawing.Point(17, 102);
            this.btnMiniECVotes.Name = "btnMiniECVotes";
            this.btnMiniECVotes.Size = new System.Drawing.Size(80, 24);
            this.btnMiniECVotes.TabIndex = 59;
            this.btnMiniECVotes.Text = "Mini";
            this.btnMiniECVotes.UseVisualStyleBackColor = true;
            this.btnMiniECVotes.Click += new System.EventHandler(this.BtnMiniECVotes_Click);
            // 
            // chkMiniMessage
            // 
            this.chkMiniMessage.AutoSize = true;
            this.chkMiniMessage.Location = new System.Drawing.Point(368, 135);
            this.chkMiniMessage.Name = "chkMiniMessage";
            this.chkMiniMessage.Size = new System.Drawing.Size(15, 14);
            this.chkMiniMessage.TabIndex = 67;
            this.chkMiniMessage.UseVisualStyleBackColor = true;
            // 
            // lblMiniMessage
            // 
            this.lblMiniMessage.AutoSize = true;
            this.lblMiniMessage.Location = new System.Drawing.Point(15, 135);
            this.lblMiniMessage.Name = "lblMiniMessage";
            this.lblMiniMessage.Size = new System.Drawing.Size(53, 13);
            this.lblMiniMessage.TabIndex = 66;
            this.lblMiniMessage.Text = "Message:";
            this.lblMiniMessage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboMiniMessages
            // 
            this.cboMiniMessages.FormattingEnabled = true;
            this.cboMiniMessages.Location = new System.Drawing.Point(68, 132);
            this.cboMiniMessages.Name = "cboMiniMessages";
            this.cboMiniMessages.Size = new System.Drawing.Size(294, 21);
            this.cboMiniMessages.TabIndex = 62;
            // 
            // lblNeededToWin
            // 
            this.lblNeededToWin.AutoSize = true;
            this.lblNeededToWin.Location = new System.Drawing.Point(26, 23);
            this.lblNeededToWin.Name = "lblNeededToWin";
            this.lblNeededToWin.Size = new System.Drawing.Size(100, 13);
            this.lblNeededToWin.TabIndex = 65;
            this.lblNeededToWin.Text = "Needed to win: 270";
            // 
            // lblRepECVotes
            // 
            this.lblRepECVotes.BackColor = System.Drawing.Color.Red;
            this.lblRepECVotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRepECVotes.ForeColor = System.Drawing.Color.White;
            this.lblRepECVotes.Location = new System.Drawing.Point(223, 38);
            this.lblRepECVotes.Name = "lblRepECVotes";
            this.lblRepECVotes.Size = new System.Drawing.Size(71, 23);
            this.lblRepECVotes.TabIndex = 55;
            this.lblRepECVotes.Text = "0";
            this.lblRepECVotes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRepublican
            // 
            this.lblRepublican.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRepublican.Location = new System.Drawing.Point(219, 16);
            this.lblRepublican.Name = "lblRepublican";
            this.lblRepublican.Size = new System.Drawing.Size(75, 20);
            this.lblRepublican.TabIndex = 64;
            this.lblRepublican.Text = "Gop";
            this.lblRepublican.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDemECVotes
            // 
            this.lblDemECVotes.BackColor = System.Drawing.Color.Blue;
            this.lblDemECVotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDemECVotes.ForeColor = System.Drawing.Color.White;
            this.lblDemECVotes.Location = new System.Drawing.Point(142, 38);
            this.lblDemECVotes.Name = "lblDemECVotes";
            this.lblDemECVotes.Size = new System.Drawing.Size(71, 23);
            this.lblDemECVotes.TabIndex = 54;
            this.lblDemECVotes.Text = "0";
            this.lblDemECVotes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDemocrat
            // 
            this.lblDemocrat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDemocrat.Location = new System.Drawing.Point(137, 16);
            this.lblDemocrat.Name = "lblDemocrat";
            this.lblDemocrat.Size = new System.Drawing.Size(76, 20);
            this.lblDemocrat.TabIndex = 63;
            this.lblDemocrat.Text = "Dem";
            this.lblDemocrat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboVIZEngineMini
            // 
            this.cboVIZEngineMini.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboVIZEngineMini.FormattingEnabled = true;
            this.cboVIZEngineMini.Location = new System.Drawing.Point(411, 719);
            this.cboVIZEngineMini.Name = "cboVIZEngineMini";
            this.cboVIZEngineMini.Size = new System.Drawing.Size(269, 21);
            this.cboVIZEngineMini.TabIndex = 77;
            // 
            // cboVIZEngineMain
            // 
            this.cboVIZEngineMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboVIZEngineMain.FormattingEnabled = true;
            this.cboVIZEngineMain.Location = new System.Drawing.Point(13, 719);
            this.cboVIZEngineMain.Name = "cboVIZEngineMain";
            this.cboVIZEngineMain.Size = new System.Drawing.Size(269, 21);
            this.cboVIZEngineMain.TabIndex = 78;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnReset.Location = new System.Drawing.Point(925, 583);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(91, 23);
            this.btnReset.TabIndex = 79;
            this.btnReset.Text = "Reset VIZ";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // chkMaestroFR
            // 
            this.chkMaestroFR.AutoSize = true;
            this.chkMaestroFR.Location = new System.Drawing.Point(785, 723);
            this.chkMaestroFR.Name = "chkMaestroFR";
            this.chkMaestroFR.Size = new System.Drawing.Size(81, 17);
            this.chkMaestroFR.TabIndex = 80;
            this.chkMaestroFR.Text = "Maestro FR";
            this.chkMaestroFR.UseVisualStyleBackColor = true;
            this.chkMaestroFR.CheckedChanged += new System.EventHandler(this.ChkMaestroFR_CheckedChanged);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(1011, 719);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 81;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1188, 747);
            this.Controls.Add(this.grpMini);
            this.Controls.Add(this.chkMaestroFR);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cboVIZEngineMain);
            this.Controls.Add(this.cboVIZEngineMini);
            this.Controls.Add(this.btnRepublicanCall);
            this.Controls.Add(this.btnDemocratCall);
            this.Controls.Add(this.grpCanopyData);
            this.Controls.Add(this.pnlSenateHouse);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.pnlVIZBackground);
            this.Controls.Add(this.cmdMapPrevious);
            this.Controls.Add(this.cmdMap);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.cmdVIZEngineMiniConnect);
            this.Controls.Add(this.cmdVIZEngineMainConnect);
            this.Controls.Add(this.lblVIZEngineMini);
            this.Controls.Add(this.lblVIZEngineMain);
            this.Controls.Add(this.DgvStates);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Maestro - US Election 2016";
            ((System.ComponentModel.ISupportInitialize)(this.DgvStates)).EndInit();
            this.grpCanopyData.ResumeLayout(false);
            this.grpCanopyData.PerformLayout();
            this.grpCanopyTimer.ResumeLayout(false);
            this.grpCanopyTimer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CanopyRefreshSeconds)).EndInit();
            this.pnlSenateHouse.ResumeLayout(false);
            this.pnlSenateHouse.PerformLayout();
            this.pnlVIZBackground.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.grpMini.ResumeLayout(false);
            this.grpMini.PerformLayout();
            this.grpMaestroFR.ResumeLayout(false);
            this.grpMaestroFR.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.grpMiniBackground.ResumeLayout(false);
            this.grpMiniBackground.PerformLayout();
            this.grpMiniProjection.ResumeLayout(false);
            this.grpMiniProjection.PerformLayout();
            this.grpZeroMini.ResumeLayout(false);
            this.grpMiniResults.ResumeLayout(false);
            this.grpMiniResults.PerformLayout();
            this.grpGem.ResumeLayout(false);
            this.grpGem.PerformLayout();
            this.grpLive.ResumeLayout(false);
            this.grpLive.PerformLayout();
            this.grpBrand.ResumeLayout(false);
            this.grpBrand.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Button cmdVIZEngineMiniConnect;
        internal System.Windows.Forms.Button cmdVIZEngineMainConnect;
        internal System.Windows.Forms.Label lblVIZEngineMini;
        internal System.Windows.Forms.Label lblVIZEngineMain;
        internal System.Windows.Forms.DataGridView DgvStates;
        internal System.Windows.Forms.Button btnRepublicanCall;
        internal System.Windows.Forms.Button btnDemocratCall;
        internal System.Windows.Forms.GroupBox grpCanopyData;
        internal System.Windows.Forms.Label lblCurrentAPFile;
        internal System.Windows.Forms.Panel pnlSenateHouse;
        internal System.Windows.Forms.Label lblHouseOth;
        internal System.Windows.Forms.TextBox txtHOth;
        internal System.Windows.Forms.Label lblSenateOth;
        internal System.Windows.Forms.TextBox txtSOth;
        internal System.Windows.Forms.Label lblHouseDemocrat;
        internal System.Windows.Forms.Button cmdHouse;
        internal System.Windows.Forms.Button cmdSenate;
        internal System.Windows.Forms.Label lblHouseRepublican;
        internal System.Windows.Forms.TextBox txtHRep;
        internal System.Windows.Forms.TextBox txtHDem;
        internal System.Windows.Forms.Label lblHouse;
        internal System.Windows.Forms.Label lblSenateDemocrat;
        internal System.Windows.Forms.Label lblSenateRepublican;
        internal System.Windows.Forms.TextBox txtSRep;
        internal System.Windows.Forms.TextBox txtSDem;
        internal System.Windows.Forms.Label lblSenate;
        internal System.Windows.Forms.Button btnContinue;
        internal System.Windows.Forms.Panel pnlVIZBackground;
        internal System.Windows.Forms.RadioButton optBackgroundOff;
        internal System.Windows.Forms.RadioButton optBackgroundOn;
        internal System.Windows.Forms.Button cmdMapPrevious;
        internal System.Windows.Forms.Button cmdMap;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Button cmdTimeZone1;
        internal System.Windows.Forms.Button cmdTimeZone9;
        internal System.Windows.Forms.Button cmdTimeZone2;
        internal System.Windows.Forms.Button cmdTimeZone8;
        internal System.Windows.Forms.Button cmdTimeZone3;
        internal System.Windows.Forms.Button cmdTimeZone7;
        internal System.Windows.Forms.Button cmdTimeZone4;
        internal System.Windows.Forms.Button cmdTimeZone6;
        internal System.Windows.Forms.Button cmdTimeZone5;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox grpMini;
        private System.Windows.Forms.Label lblPopVote;
        private System.Windows.Forms.Label lblECVote;
        private System.Windows.Forms.TextBox txtOthPopularVote;
        internal System.Windows.Forms.Label lblOthECVotes;
        internal System.Windows.Forms.Label lblLibertarian;
        private System.Windows.Forms.TextBox txtGopPopularVote;
        private System.Windows.Forms.TextBox txtDemPopularVote;
        internal System.Windows.Forms.GroupBox grpBrand;
        internal System.Windows.Forms.RadioButton optBrandOff;
        internal System.Windows.Forms.RadioButton optBrandOn;
        internal System.Windows.Forms.Button btnMiniClear;
        internal System.Windows.Forms.Button btnMiniVoteShare;
        internal System.Windows.Forms.Button btnMiniECVotes;
        internal System.Windows.Forms.CheckBox chkMiniMessage;
        internal System.Windows.Forms.Label lblMiniMessage;
        internal System.Windows.Forms.ComboBox cboMiniMessages;
        internal System.Windows.Forms.Label lblNeededToWin;
        internal System.Windows.Forms.Label lblRepECVotes;
        internal System.Windows.Forms.Label lblRepublican;
        internal System.Windows.Forms.Label lblDemECVotes;
        internal System.Windows.Forms.Label lblDemocrat;
        private System.Windows.Forms.Label lblBrand;
        internal System.Windows.Forms.GroupBox grpLive;
        private System.Windows.Forms.Label lblLive;
        internal System.Windows.Forms.RadioButton optLiveOff;
        internal System.Windows.Forms.RadioButton optLiveOn;
        internal System.Windows.Forms.GroupBox grpGem;
        private System.Windows.Forms.Label lblGem;
        internal System.Windows.Forms.RadioButton optGemOff;
        internal System.Windows.Forms.RadioButton optGemOn;
        internal System.Windows.Forms.GroupBox grpMiniResults;
        private System.Windows.Forms.Label lblMiniResults;
        internal System.Windows.Forms.RadioButton optMiniResultsOff;
        internal System.Windows.Forms.RadioButton optMiniResultsOn;
        internal System.Windows.Forms.GroupBox grpZeroMini;
        internal System.Windows.Forms.RadioButton optZeroMiniOff;
        internal System.Windows.Forms.RadioButton optZeroMiniOn;
        private System.Windows.Forms.GroupBox grpCanopyTimer;
        private System.Windows.Forms.Label lblCanopyRefreshInterval;
        private System.Windows.Forms.NumericUpDown CanopyRefreshSeconds;
        internal System.Windows.Forms.RadioButton optFeedOff;
        internal System.Windows.Forms.RadioButton optFeedOn;
        private System.Windows.Forms.ToolStripMenuItem resetResultsToolStripMenuItem;
        private System.Windows.Forms.Label lblUSStateCodeResults;
        private System.Windows.Forms.ToolStripMenuItem allowCanopyUpdatesToolStripMenuItem;
        private System.Windows.Forms.Label lblCanopyStatusValue;
        private System.Windows.Forms.Label lblCanopyStatus;
        private System.Windows.Forms.ToolStripMenuItem disallowCanopyUpdatesToolStripMenuItem;
        private System.Windows.Forms.Label lblSOthTotal;
        private System.Windows.Forms.Label lblSGopTotal;
        private System.Windows.Forms.Label lblSDemTotal;
        private System.Windows.Forms.Label lblSOthHold;
        private System.Windows.Forms.Label lblSGopHold;
        private System.Windows.Forms.Label lblSDemHold;
        private System.Windows.Forms.ToolStripMenuItem importStateGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ComboBox cboVIZEngineMini;
        private System.Windows.Forms.ComboBox cboVIZEngineMain;
        internal System.Windows.Forms.GroupBox grpMiniBackground;
        private System.Windows.Forms.Label lblMiniBackground;
        internal System.Windows.Forms.GroupBox grpMiniProjection;
        private System.Windows.Forms.Label lblMiniProjection;
        internal System.Windows.Forms.RadioButton optMiniProjectionOff;
        internal System.Windows.Forms.RadioButton optMiniProjectionOn;
        internal System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox chkMaestroFR;
        private System.Windows.Forms.Button cmdMiniBackgroundOff;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnStanding;
        private System.Windows.Forms.GroupBox grpMaestroFR;
        private System.Windows.Forms.Button btnResetMain;
        private System.Windows.Forms.Button btnResetMini;
        internal System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.RadioButton optMapOff;
        internal System.Windows.Forms.RadioButton optMapOn;
        private System.Windows.Forms.CheckBox chkMapYear;
        private System.Windows.Forms.CheckBox chkAnimatedBG;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStateNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStateCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStateName;
        private System.Windows.Forms.DataGridViewButtonColumn colTake;
        private System.Windows.Forms.DataGridViewTextBoxColumn colECVotes;
        private System.Windows.Forms.DataGridViewButtonColumn colInProgress;
        private System.Windows.Forms.DataGridViewButtonColumn colDemocrats;
        private System.Windows.Forms.DataGridViewButtonColumn colRepublicans;
        private System.Windows.Forms.DataGridViewButtonColumn colOther;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApCall;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimeZone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWinningParty;
        private System.Windows.Forms.DataGridViewButtonColumn colMini;
    }
}

