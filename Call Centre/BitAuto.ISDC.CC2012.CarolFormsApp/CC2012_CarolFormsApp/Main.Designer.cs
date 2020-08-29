using System;
using System.Collections;
using System.Data;
namespace CC2012_CarolFormsApp
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tscbUrl = new System.Windows.Forms.ToolStripComboBox();
            this.gotoButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatuslblCTIEventName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCallerNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCalledNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCallRecordLength = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAllCallRecordLength = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSYSDNIS = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.toolStripAgentTool = new System.Windows.Forms.ToolStrip();
            this.lblAgentStatusName = new System.Windows.Forms.ToolStripLabel();
            this.toolSbtnHomePage = new System.Windows.Forms.ToolStripButton();
            this.toolSbtnHelp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSbtnAgentCheckIn = new System.Windows.Forms.ToolStripButton();
            this.toolSbtnAgentCheckOff = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSbtnAgentConsult = new System.Windows.Forms.ToolStripButton();
            this.toolSbtnAgentReconnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSbtnSetBusy = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.任务回访ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.业务处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.会议ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.培训ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.离席ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSbtnAgentReady = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSbtnAfterCallWork = new System.Windows.Forms.ToolStripButton();
            this.toolSbtnAgentMakeCall = new System.Windows.Forms.ToolStripButton();
            this.toolSbtnAgentReleaseCall = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSbtnIVRSatisfaction = new System.Windows.Forms.ToolStripButton();
            this.toolSbtnChangeSkin = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton19 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.backButton = new System.Windows.Forms.ToolStripButton();
            this.forwordButton = new System.Windows.Forms.ToolStripButton();
            this.stopButton = new System.Windows.Forms.ToolStripButton();
            this.refreshButton = new System.Windows.Forms.ToolStripButton();
            this.homeButton = new System.Windows.Forms.ToolStripButton();
            this.searchButton = new System.Windows.Forms.ToolStripButton();
            this.printButton = new System.Windows.Forms.ToolStripButton();
            this.newButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.timer3 = new System.Timers.Timer();
            this.menuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStripAgentTool.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timer3)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.设置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(942, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tipToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "帮助";
            // 
            // tipToolStripMenuItem
            // 
            this.tipToolStripMenuItem.Name = "tipToolStripMenuItem";
            this.tipToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.tipToolStripMenuItem.Text = "提示";
            this.tipToolStripMenuItem.Visible = false;
            this.tipToolStripMenuItem.Click += new System.EventHandler(this.tipToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.aboutToolStripMenuItem.Text = "关于";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.设置ToolStripMenuItem.Text = "设置(&s)";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.设置ToolStripMenuItem_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscbUrl,
            this.gotoButton});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(946, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            this.toolStrip2.Visible = false;
            // 
            // tscbUrl
            // 
            this.tscbUrl.Name = "tscbUrl";
            this.tscbUrl.Size = new System.Drawing.Size(400, 25);
            this.tscbUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tscbUrl_KeyDown);
            // 
            // gotoButton
            // 
            this.gotoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.gotoButton.Name = "gotoButton";
            this.gotoButton.Size = new System.Drawing.Size(36, 22);
            this.gotoButton.Text = "转到";
            this.gotoButton.Click += new System.EventHandler(this.gotoButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel2,
            this.toolStripStatuslblCTIEventName,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabelCallerNum,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabelCalledNum,
            this.toolStripStatusLabel4,
            this.lblCallRecordLength,
            this.toolStripStatusLabel6,
            this.lblAllCallRecordLength,
            this.toolStripStatusLabel7,
            this.lblSYSDNIS});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 362);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(946, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(555, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "状态";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.AutoSize = false;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(200, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(63, 17);
            this.toolStripStatusLabel2.Text = "CTI状态：";
            // 
            // toolStripStatuslblCTIEventName
            // 
            this.toolStripStatuslblCTIEventName.AutoSize = false;
            this.toolStripStatuslblCTIEventName.Name = "toolStripStatuslblCTIEventName";
            this.toolStripStatuslblCTIEventName.Size = new System.Drawing.Size(90, 17);
            this.toolStripStatuslblCTIEventName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel3.Text = "主叫号码:";
            // 
            // toolStripStatusLabelCallerNum
            // 
            this.toolStripStatusLabelCallerNum.AutoSize = false;
            this.toolStripStatusLabelCallerNum.Name = "toolStripStatusLabelCallerNum";
            this.toolStripStatusLabelCallerNum.Size = new System.Drawing.Size(110, 17);
            this.toolStripStatusLabelCallerNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel5.Text = "被叫号码:";
            // 
            // toolStripStatusLabelCalledNum
            // 
            this.toolStripStatusLabelCalledNum.AutoSize = false;
            this.toolStripStatusLabelCalledNum.Name = "toolStripStatusLabelCalledNum";
            this.toolStripStatusLabelCalledNum.Size = new System.Drawing.Size(110, 17);
            this.toolStripStatusLabelCalledNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel4.Text = "通话时长：";
            // 
            // lblCallRecordLength
            // 
            this.lblCallRecordLength.AutoSize = false;
            this.lblCallRecordLength.Name = "lblCallRecordLength";
            this.lblCallRecordLength.Size = new System.Drawing.Size(30, 17);
            this.lblCallRecordLength.Text = "0";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(80, 17);
            this.toolStripStatusLabel6.Text = "通话总时长：";
            this.toolStripStatusLabel6.Visible = false;
            // 
            // lblAllCallRecordLength
            // 
            this.lblAllCallRecordLength.AutoSize = false;
            this.lblAllCallRecordLength.Name = "lblAllCallRecordLength";
            this.lblAllCallRecordLength.Size = new System.Drawing.Size(30, 17);
            this.lblAllCallRecordLength.Text = "0";
            this.lblAllCallRecordLength.Visible = false;
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel7.Text = "接入号码：";
            // 
            // lblSYSDNIS
            // 
            this.lblSYSDNIS.AutoSize = false;
            this.lblSYSDNIS.Name = "lblSYSDNIS";
            this.lblSYSDNIS.Size = new System.Drawing.Size(100, 17);
            this.lblSYSDNIS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 61);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(946, 301);
            this.tabControl1.TabIndex = 5;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Elapsed);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Elapsed);
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // toolStripAgentTool
            // 
            this.toolStripAgentTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblAgentStatusName,
            this.toolSbtnHomePage,
            this.toolSbtnHelp,
            this.toolStripSeparator1,
            this.toolSbtnAgentCheckIn,
            this.toolSbtnAgentCheckOff,
            this.toolStripSeparator2,
            this.toolSbtnAgentConsult,
            this.toolSbtnAgentReconnect,
            this.toolStripSeparator7,
            this.toolSbtnSetBusy,
            this.toolSbtnAgentReady,
            this.toolStripSeparator8,
            this.toolSbtnAfterCallWork,
            this.toolSbtnAgentMakeCall,
            this.toolSbtnAgentReleaseCall,
            this.toolStripSeparator9,
            this.toolSbtnIVRSatisfaction,
            this.toolSbtnChangeSkin,
            this.toolStripButton19,
            this.toolStripButton1});
            this.toolStripAgentTool.Location = new System.Drawing.Point(0, 0);
            this.toolStripAgentTool.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.toolStripAgentTool.Name = "toolStripAgentTool";
            this.toolStripAgentTool.Size = new System.Drawing.Size(946, 61);
            this.toolStripAgentTool.TabIndex = 7;
            this.toolStripAgentTool.Text = "toolStrip4";
            // 
            // lblAgentStatusName
            // 
            this.lblAgentStatusName.AutoSize = false;
            this.lblAgentStatusName.Font = new System.Drawing.Font("Verdana", 14F);
            this.lblAgentStatusName.ForeColor = System.Drawing.Color.Red;
            this.lblAgentStatusName.Name = "lblAgentStatusName";
            this.lblAgentStatusName.Size = new System.Drawing.Size(120, 58);
            this.lblAgentStatusName.Text = "置忙";
            this.lblAgentStatusName.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.lblAgentStatusName.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolSbtnHomePage
            // 
            this.toolSbtnHomePage.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolSbtnHomePage.ForeColor = System.Drawing.Color.RoyalBlue;
            this.toolSbtnHomePage.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnHomePage.Image")));
            this.toolSbtnHomePage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnHomePage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnHomePage.Name = "toolSbtnHomePage";
            this.toolSbtnHomePage.Size = new System.Drawing.Size(41, 58);
            this.toolSbtnHomePage.Text = "主页";
            this.toolSbtnHomePage.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.toolSbtnHomePage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnHomePage.Click += new System.EventHandler(this.homeButton_Click);
            // 
            // toolSbtnHelp
            // 
            this.toolSbtnHelp.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnHelp.Image")));
            this.toolSbtnHelp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnHelp.Name = "toolSbtnHelp";
            this.toolSbtnHelp.Size = new System.Drawing.Size(38, 58);
            this.toolSbtnHelp.Text = "帮助";
            this.toolSbtnHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnHelp.Click += new System.EventHandler(this.toolSbtnHelp_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 61);
            // 
            // toolSbtnAgentCheckIn
            // 
            this.toolSbtnAgentCheckIn.AutoSize = false;
            this.toolSbtnAgentCheckIn.Enabled = false;
            this.toolSbtnAgentCheckIn.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnAgentCheckIn.Image")));
            this.toolSbtnAgentCheckIn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnAgentCheckIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnAgentCheckIn.Name = "toolSbtnAgentCheckIn";
            this.toolSbtnAgentCheckIn.Size = new System.Drawing.Size(38, 50);
            this.toolSbtnAgentCheckIn.Text = "签入";
            this.toolSbtnAgentCheckIn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnAgentCheckIn.Click += new System.EventHandler(this.toolSbtnAgentCheckIn_Click);
            // 
            // toolSbtnAgentCheckOff
            // 
            this.toolSbtnAgentCheckOff.AutoSize = false;
            this.toolSbtnAgentCheckOff.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnAgentCheckOff.Image")));
            this.toolSbtnAgentCheckOff.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnAgentCheckOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnAgentCheckOff.Name = "toolSbtnAgentCheckOff";
            this.toolSbtnAgentCheckOff.Size = new System.Drawing.Size(38, 50);
            this.toolSbtnAgentCheckOff.Text = "签出";
            this.toolSbtnAgentCheckOff.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnAgentCheckOff.Click += new System.EventHandler(this.toolSbtnAgentCheckOff_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 61);
            // 
            // toolSbtnAgentConsult
            // 
            this.toolSbtnAgentConsult.AutoSize = false;
            this.toolSbtnAgentConsult.Enabled = false;
            this.toolSbtnAgentConsult.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnAgentConsult.Image")));
            this.toolSbtnAgentConsult.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnAgentConsult.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnAgentConsult.Name = "toolSbtnAgentConsult";
            this.toolSbtnAgentConsult.Size = new System.Drawing.Size(38, 50);
            this.toolSbtnAgentConsult.Text = "转接";
            this.toolSbtnAgentConsult.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnAgentConsult.Click += new System.EventHandler(this.ConsultCall_Click);
            // 
            // toolSbtnAgentReconnect
            // 
            this.toolSbtnAgentReconnect.AutoSize = false;
            this.toolSbtnAgentReconnect.Enabled = false;
            this.toolSbtnAgentReconnect.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnAgentReconnect.Image")));
            this.toolSbtnAgentReconnect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnAgentReconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnAgentReconnect.Name = "toolSbtnAgentReconnect";
            this.toolSbtnAgentReconnect.Size = new System.Drawing.Size(38, 50);
            this.toolSbtnAgentReconnect.Text = "恢复";
            this.toolSbtnAgentReconnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnAgentReconnect.Click += new System.EventHandler(this.ReconnectCall_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 61);
            // 
            // toolSbtnSetBusy
            // 
            this.toolSbtnSetBusy.AutoSize = false;
            this.toolSbtnSetBusy.DropDownButtonWidth = 15;
            this.toolSbtnSetBusy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.任务回访ToolStripMenuItem,
            this.业务处理ToolStripMenuItem,
            this.会议ToolStripMenuItem,
            this.培训ToolStripMenuItem,
            this.离席ToolStripMenuItem});
            this.toolSbtnSetBusy.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnSetBusy.Image")));
            this.toolSbtnSetBusy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnSetBusy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnSetBusy.Name = "toolSbtnSetBusy";
            this.toolSbtnSetBusy.Size = new System.Drawing.Size(63, 50);
            this.toolSbtnSetBusy.Text = "置忙";
            this.toolSbtnSetBusy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnSetBusy.ButtonClick += new System.EventHandler(this.toolSbtnSetBusy_ButtonClick);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem5.Tag = "1";
            this.toolStripMenuItem5.Text = "小休";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.AgentSetStateNotReady_Click);
            // 
            // 任务回访ToolStripMenuItem
            // 
            this.任务回访ToolStripMenuItem.Name = "任务回访ToolStripMenuItem";
            this.任务回访ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.任务回访ToolStripMenuItem.Tag = "2";
            this.任务回访ToolStripMenuItem.Text = "任务回访";
            this.任务回访ToolStripMenuItem.Click += new System.EventHandler(this.AgentSetStateNotReady_Click);
            // 
            // 业务处理ToolStripMenuItem
            // 
            this.业务处理ToolStripMenuItem.Name = "业务处理ToolStripMenuItem";
            this.业务处理ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.业务处理ToolStripMenuItem.Tag = "3";
            this.业务处理ToolStripMenuItem.Text = "业务处理";
            this.业务处理ToolStripMenuItem.Click += new System.EventHandler(this.AgentSetStateNotReady_Click);
            // 
            // 会议ToolStripMenuItem
            // 
            this.会议ToolStripMenuItem.Name = "会议ToolStripMenuItem";
            this.会议ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.会议ToolStripMenuItem.Tag = "4";
            this.会议ToolStripMenuItem.Text = "会议";
            this.会议ToolStripMenuItem.Click += new System.EventHandler(this.AgentSetStateNotReady_Click);
            // 
            // 培训ToolStripMenuItem
            // 
            this.培训ToolStripMenuItem.Name = "培训ToolStripMenuItem";
            this.培训ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.培训ToolStripMenuItem.Tag = "5";
            this.培训ToolStripMenuItem.Text = "培训";
            this.培训ToolStripMenuItem.Click += new System.EventHandler(this.AgentSetStateNotReady_Click);
            // 
            // 离席ToolStripMenuItem
            // 
            this.离席ToolStripMenuItem.Name = "离席ToolStripMenuItem";
            this.离席ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.离席ToolStripMenuItem.Tag = "6";
            this.离席ToolStripMenuItem.Text = "离席";
            this.离席ToolStripMenuItem.Click += new System.EventHandler(this.AgentSetStateNotReady_Click);
            // 
            // toolSbtnAgentReady
            // 
            this.toolSbtnAgentReady.AutoSize = false;
            this.toolSbtnAgentReady.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnAgentReady.Image")));
            this.toolSbtnAgentReady.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnAgentReady.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnAgentReady.Name = "toolSbtnAgentReady";
            this.toolSbtnAgentReady.Size = new System.Drawing.Size(38, 50);
            this.toolSbtnAgentReady.Text = "置闲";
            this.toolSbtnAgentReady.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnAgentReady.Click += new System.EventHandler(this.AgentSetStateReady_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 61);
            // 
            // toolSbtnAfterCallWork
            // 
            this.toolSbtnAfterCallWork.AutoSize = false;
            this.toolSbtnAfterCallWork.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnAfterCallWork.Image")));
            this.toolSbtnAfterCallWork.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnAfterCallWork.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnAfterCallWork.Name = "toolSbtnAfterCallWork";
            this.toolSbtnAfterCallWork.Size = new System.Drawing.Size(38, 50);
            this.toolSbtnAfterCallWork.Text = "话后";
            this.toolSbtnAfterCallWork.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnAfterCallWork.Click += new System.EventHandler(this.AgentSetStateAfterCallWork_Click);
            // 
            // toolSbtnAgentMakeCall
            // 
            this.toolSbtnAgentMakeCall.AutoSize = false;
            this.toolSbtnAgentMakeCall.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnAgentMakeCall.Image")));
            this.toolSbtnAgentMakeCall.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnAgentMakeCall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnAgentMakeCall.Name = "toolSbtnAgentMakeCall";
            this.toolSbtnAgentMakeCall.Size = new System.Drawing.Size(38, 50);
            this.toolSbtnAgentMakeCall.Text = "拨号";
            this.toolSbtnAgentMakeCall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnAgentMakeCall.Click += new System.EventHandler(this.toolSbtnAgentMakeCall_Click);
            // 
            // toolSbtnAgentReleaseCall
            // 
            this.toolSbtnAgentReleaseCall.AutoSize = false;
            this.toolSbtnAgentReleaseCall.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnAgentReleaseCall.Image")));
            this.toolSbtnAgentReleaseCall.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnAgentReleaseCall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnAgentReleaseCall.Name = "toolSbtnAgentReleaseCall";
            this.toolSbtnAgentReleaseCall.Size = new System.Drawing.Size(38, 50);
            this.toolSbtnAgentReleaseCall.Text = "挂断";
            this.toolSbtnAgentReleaseCall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnAgentReleaseCall.Click += new System.EventHandler(this.ReleaseCall_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 61);
            // 
            // toolSbtnIVRSatisfaction
            // 
            this.toolSbtnIVRSatisfaction.AutoSize = false;
            this.toolSbtnIVRSatisfaction.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnIVRSatisfaction.Image")));
            this.toolSbtnIVRSatisfaction.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnIVRSatisfaction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnIVRSatisfaction.Name = "toolSbtnIVRSatisfaction";
            this.toolSbtnIVRSatisfaction.Size = new System.Drawing.Size(38, 50);
            this.toolSbtnIVRSatisfaction.Text = "转IVR";
            this.toolSbtnIVRSatisfaction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnIVRSatisfaction.ToolTipText = "转IVR";
            this.toolSbtnIVRSatisfaction.Click += new System.EventHandler(this.toolSbtnIVRSatisfaction_Click);
            // 
            // toolSbtnChangeSkin
            // 
            this.toolSbtnChangeSkin.AutoSize = false;
            this.toolSbtnChangeSkin.Image = ((System.Drawing.Image)(resources.GetObject("toolSbtnChangeSkin.Image")));
            this.toolSbtnChangeSkin.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolSbtnChangeSkin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSbtnChangeSkin.Name = "toolSbtnChangeSkin";
            this.toolSbtnChangeSkin.Size = new System.Drawing.Size(38, 50);
            this.toolSbtnChangeSkin.Text = "换肤";
            this.toolSbtnChangeSkin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolSbtnChangeSkin.Visible = false;
            this.toolSbtnChangeSkin.Click += new System.EventHandler(this.toolSbtnChangeSkin_Click);
            // 
            // toolStripButton19
            // 
            this.toolStripButton19.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton19.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton19.Image")));
            this.toolStripButton19.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton19.Name = "toolStripButton19";
            this.toolStripButton19.Size = new System.Drawing.Size(36, 58);
            this.toolStripButton19.Text = "刷新";
            this.toolStripButton19.Visible = false;
            this.toolStripButton19.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(84, 58);
            this.toolStripButton1.Text = "弹出转接列表";
            this.toolStripButton1.Visible = false;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // backButton
            // 
            this.backButton.AutoSize = false;
            this.backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(60, 60);
            this.backButton.Text = "后退";
            this.backButton.Visible = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // forwordButton
            // 
            this.forwordButton.AutoSize = false;
            this.forwordButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.forwordButton.Name = "forwordButton";
            this.forwordButton.Size = new System.Drawing.Size(60, 60);
            this.forwordButton.Text = "前进";
            this.forwordButton.Visible = false;
            this.forwordButton.Click += new System.EventHandler(this.forwordButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.AutoSize = false;
            this.stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(60, 60);
            this.stopButton.Text = "停止";
            this.stopButton.Visible = false;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.AutoSize = false;
            this.refreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(60, 60);
            this.refreshButton.Text = "刷新";
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // homeButton
            // 
            this.homeButton.AutoSize = false;
            this.homeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.homeButton.Name = "homeButton";
            this.homeButton.Size = new System.Drawing.Size(60, 60);
            this.homeButton.Text = "主页";
            this.homeButton.Click += new System.EventHandler(this.homeButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.AutoSize = false;
            this.searchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(60, 60);
            this.searchButton.Text = "搜索";
            this.searchButton.Visible = false;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // printButton
            // 
            this.printButton.AutoSize = false;
            this.printButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(60, 60);
            this.printButton.Text = "打印";
            this.printButton.Visible = false;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // newButton
            // 
            this.newButton.AutoSize = false;
            this.newButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(60, 60);
            this.newButton.Text = "新建";
            this.newButton.Visible = false;
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backButton,
            this.forwordButton,
            this.stopButton,
            this.refreshButton,
            this.homeButton,
            this.searchButton,
            this.printButton,
            this.newButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(942, 60);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // timer3
            // 
            this.timer3.Enabled = true;
            this.timer3.Interval = 30000D;
            this.timer3.SynchronizingObject = this;
            this.timer3.Elapsed += new System.Timers.ElapsedEventHandler(this.timer3_Elapsed);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 384);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStripAgentTool);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripAgentTool.ResumeLayout(false);
            this.toolStripAgentTool.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timer3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripComboBox tscbUrl;
        private System.Windows.Forms.ToolStripButton gotoButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tipToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatuslblCTIEventName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel lblCallRecordLength;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel lblAllCallRecordLength;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel lblSYSDNIS;

        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCallerNum;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCalledNum;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        public Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.ToolStrip toolStripAgentTool;
        private System.Windows.Forms.ToolStripButton toolSbtnHomePage;
        private System.Windows.Forms.ToolStripButton toolSbtnHelp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolSbtnAgentCheckIn;
        private System.Windows.Forms.ToolStripButton toolSbtnAgentCheckOff;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolSbtnAgentConsult;
        private System.Windows.Forms.ToolStripButton toolSbtnAgentReconnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton toolSbtnAgentReady;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton toolSbtnAfterCallWork;
        private System.Windows.Forms.ToolStripButton toolSbtnAgentMakeCall;
        private System.Windows.Forms.ToolStripButton toolSbtnAgentReleaseCall;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton toolSbtnChangeSkin;
        private System.Windows.Forms.ToolStripButton backButton;
        private System.Windows.Forms.ToolStripButton forwordButton;
        private System.Windows.Forms.ToolStripButton stopButton;
        private System.Windows.Forms.ToolStripButton refreshButton;
        private System.Windows.Forms.ToolStripButton homeButton;
        private System.Windows.Forms.ToolStripButton searchButton;
        private System.Windows.Forms.ToolStripButton printButton;
        private System.Windows.Forms.ToolStripButton newButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblAgentStatusName;
        private System.Windows.Forms.ToolStripButton toolStripButton19;
        private System.Windows.Forms.ToolStripSplitButton toolSbtnSetBusy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem 任务回访ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 业务处理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 会议ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 培训ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 离席ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolSbtnIVRSatisfaction;
        private System.Timers.Timer timer3;
    }
}