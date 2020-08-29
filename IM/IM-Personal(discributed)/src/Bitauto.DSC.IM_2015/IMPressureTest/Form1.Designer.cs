namespace IMPressureTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWYNum = new System.Windows.Forms.TextBox();
            this.txtLongConnetInterval = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtAgentNum = new System.Windows.Forms.TextBox();
            this.txtSendMsgInterval = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckSendMsg = new System.Windows.Forms.CheckBox();
            this.ckAgentLong = new System.Windows.Forms.CheckBox();
            this.ckWYLong = new System.Windows.Forms.CheckBox();
            this.btnInitAgent = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnInitWY = new System.Windows.Forms.Button();
            this.BtnStartTest = new System.Windows.Forms.Button();
            this.listNotes = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "模拟网友并发个数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "长连接间隔";
            // 
            // txtWYNum
            // 
            this.txtWYNum.Location = new System.Drawing.Point(158, 30);
            this.txtWYNum.Name = "txtWYNum";
            this.txtWYNum.Size = new System.Drawing.Size(194, 21);
            this.txtWYNum.TabIndex = 2;
            this.txtWYNum.Text = "3000";
            // 
            // txtLongConnetInterval
            // 
            this.txtLongConnetInterval.Location = new System.Drawing.Point(158, 72);
            this.txtLongConnetInterval.Name = "txtLongConnetInterval";
            this.txtLongConnetInterval.Size = new System.Drawing.Size(194, 21);
            this.txtLongConnetInterval.TabIndex = 3;
            this.txtLongConnetInterval.Text = "5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "发随机消息间隔时长";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "模拟坐席并发个数";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAgentNum);
            this.groupBox1.Controls.Add(this.txtSendMsgInterval);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtWYNum);
            this.groupBox1.Controls.Add(this.txtLongConnetInterval);
            this.groupBox1.Location = new System.Drawing.Point(3, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(373, 213);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "配置";
            // 
            // txtAgentNum
            // 
            this.txtAgentNum.Location = new System.Drawing.Point(158, 184);
            this.txtAgentNum.Name = "txtAgentNum";
            this.txtAgentNum.Size = new System.Drawing.Size(194, 21);
            this.txtAgentNum.TabIndex = 7;
            this.txtAgentNum.Text = "10";
            // 
            // txtSendMsgInterval
            // 
            this.txtSendMsgInterval.Location = new System.Drawing.Point(158, 123);
            this.txtSendMsgInterval.Name = "txtSendMsgInterval";
            this.txtSendMsgInterval.Size = new System.Drawing.Size(194, 21);
            this.txtSendMsgInterval.TabIndex = 6;
            this.txtSendMsgInterval.Text = "3";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ckSendMsg);
            this.groupBox2.Controls.Add(this.ckAgentLong);
            this.groupBox2.Controls.Add(this.ckWYLong);
            this.groupBox2.Controls.Add(this.btnInitAgent);
            this.groupBox2.Controls.Add(this.btnClear);
            this.groupBox2.Controls.Add(this.btnInitWY);
            this.groupBox2.Location = new System.Drawing.Point(6, 250);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(370, 190);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作";
            // 
            // ckSendMsg
            // 
            this.ckSendMsg.AutoSize = true;
            this.ckSendMsg.Checked = true;
            this.ckSendMsg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckSendMsg.Location = new System.Drawing.Point(252, 158);
            this.ckSendMsg.Name = "ckSendMsg";
            this.ckSendMsg.Size = new System.Drawing.Size(108, 16);
            this.ckSendMsg.TabIndex = 6;
            this.ckSendMsg.Text = "监控网友发消息";
            this.ckSendMsg.UseVisualStyleBackColor = true;
            // 
            // ckAgentLong
            // 
            this.ckAgentLong.AutoSize = true;
            this.ckAgentLong.Checked = true;
            this.ckAgentLong.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckAgentLong.Location = new System.Drawing.Point(6, 158);
            this.ckAgentLong.Name = "ckAgentLong";
            this.ckAgentLong.Size = new System.Drawing.Size(108, 16);
            this.ckAgentLong.TabIndex = 5;
            this.ckAgentLong.Text = "监控坐席长连接";
            this.ckAgentLong.UseVisualStyleBackColor = true;
            // 
            // ckWYLong
            // 
            this.ckWYLong.AutoSize = true;
            this.ckWYLong.Checked = true;
            this.ckWYLong.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckWYLong.Location = new System.Drawing.Point(129, 158);
            this.ckWYLong.Name = "ckWYLong";
            this.ckWYLong.Size = new System.Drawing.Size(108, 16);
            this.ckWYLong.TabIndex = 4;
            this.ckWYLong.Text = "监控网友长连接";
            this.ckWYLong.UseVisualStyleBackColor = true;
            // 
            // btnInitAgent
            // 
            this.btnInitAgent.Location = new System.Drawing.Point(27, 20);
            this.btnInitAgent.Name = "btnInitAgent";
            this.btnInitAgent.Size = new System.Drawing.Size(98, 35);
            this.btnInitAgent.TabIndex = 3;
            this.btnInitAgent.Text = "初始化坐席";
            this.btnInitAgent.UseVisualStyleBackColor = true;
            this.btnInitAgent.Click += new System.EventHandler(this.btnInitAgent_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(232, 76);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(101, 37);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "清屏";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnInitWY
            // 
            this.btnInitWY.Location = new System.Drawing.Point(145, 20);
            this.btnInitWY.Name = "btnInitWY";
            this.btnInitWY.Size = new System.Drawing.Size(118, 35);
            this.btnInitWY.TabIndex = 0;
            this.btnInitWY.Text = "初始化网友并测试";
            this.btnInitWY.UseVisualStyleBackColor = true;
            this.btnInitWY.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // BtnStartTest
            // 
            this.BtnStartTest.Enabled = false;
            this.BtnStartTest.Location = new System.Drawing.Point(293, 460);
            this.BtnStartTest.Name = "BtnStartTest";
            this.BtnStartTest.Size = new System.Drawing.Size(73, 21);
            this.BtnStartTest.TabIndex = 1;
            this.BtnStartTest.Text = "开始测试";
            this.BtnStartTest.UseVisualStyleBackColor = true;
            this.BtnStartTest.Click += new System.EventHandler(this.BtnStartTest_Click);
            // 
            // listNotes
            // 
            this.listNotes.ColumnWidth = 500;
            this.listNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listNotes.FormattingEnabled = true;
            this.listNotes.HorizontalScrollbar = true;
            this.listNotes.ItemHeight = 12;
            this.listNotes.Location = new System.Drawing.Point(0, 0);
            this.listNotes.Name = "listNotes";
            this.listNotes.ScrollAlwaysVisible = true;
            this.listNotes.Size = new System.Drawing.Size(821, 525);
            this.listNotes.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.BtnStartTest);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listNotes);
            this.splitContainer1.Size = new System.Drawing.Size(1209, 525);
            this.splitContainer1.SplitterDistance = 384;
            this.splitContainer1.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1209, 525);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWYNum;
        private System.Windows.Forms.TextBox txtLongConnetInterval;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtAgentNum;
        private System.Windows.Forms.TextBox txtSendMsgInterval;
        private System.Windows.Forms.Button BtnStartTest;
        private System.Windows.Forms.Button btnInitWY;
        private System.Windows.Forms.ListBox listNotes;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnInitAgent;
        private System.Windows.Forms.CheckBox ckAgentLong;
        private System.Windows.Forms.CheckBox ckWYLong;
        private System.Windows.Forms.CheckBox ckSendMsg;
    }
}

