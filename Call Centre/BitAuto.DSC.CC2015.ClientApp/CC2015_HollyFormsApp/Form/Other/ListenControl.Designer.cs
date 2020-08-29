namespace CC2015_HollyFormsApp
{
    partial class ListenControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.search_panel = new System.Windows.Forms.Panel();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lb_count = new System.Windows.Forms.Label();
            this.lb_tip = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_state = new System.Windows.Forms.LinkLabel();
            this.lb_no = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lb_clear = new System.Windows.Forms.LinkLabel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.cb_state = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_group = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_list = new System.Windows.Forms.Panel();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Mi_zhimang = new System.Windows.Forms.ToolStripMenuItem();
            this.Mi_zhixian = new System.Windows.Forms.ToolStripMenuItem();
            this.Mi_qianchu = new System.Windows.Forms.ToolStripMenuItem();
            this.Mi_jianting = new System.Windows.Forms.ToolStripMenuItem();
            this.Mi_qiangchai = new System.Windows.Forms.ToolStripMenuItem();
            this.Mi_qiangcha = new System.Windows.Forms.ToolStripMenuItem();
            this.Mi_jieguan = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.search_panel.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel_list.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // search_panel
            // 
            this.search_panel.Controls.Add(this.groupBox);
            this.search_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.search_panel.Location = new System.Drawing.Point(0, 0);
            this.search_panel.Name = "search_panel";
            this.search_panel.Size = new System.Drawing.Size(1262, 81);
            this.search_panel.TabIndex = 3;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.panel2);
            this.groupBox.Controls.Add(this.panel1);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(1262, 75);
            this.groupBox.TabIndex = 3;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "查询条件";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lb_count);
            this.panel2.Controls.Add(this.lb_tip);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.lb_state);
            this.panel2.Controls.Add(this.lb_no);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 52);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1256, 23);
            this.panel2.TabIndex = 4;
            // 
            // lb_count
            // 
            this.lb_count.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_count.AutoSize = true;
            this.lb_count.Location = new System.Drawing.Point(1192, 8);
            this.lb_count.Name = "lb_count";
            this.lb_count.Size = new System.Drawing.Size(35, 12);
            this.lb_count.TabIndex = 26;
            this.lb_count.Text = "共0条";
            this.lb_count.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lb_tip
            // 
            this.lb_tip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_tip.AutoSize = true;
            this.lb_tip.Location = new System.Drawing.Point(1013, 8);
            this.lb_tip.Name = "lb_tip";
            this.lb_tip.Size = new System.Drawing.Size(167, 12);
            this.lb_tip.TabIndex = 24;
            this.lb_tip.Text = "提示：当前页面每5秒自动刷新";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(105, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 24;
            this.label4.Text = "||";
            // 
            // lb_state
            // 
            this.lb_state.AutoSize = true;
            this.lb_state.Location = new System.Drawing.Point(123, 8);
            this.lb_state.Name = "lb_state";
            this.lb_state.Size = new System.Drawing.Size(65, 12);
            this.lb_state.TabIndex = 25;
            this.lb_state.TabStop = true;
            this.lb_state.Text = "按状态排序";
            this.lb_state.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lb_state_LinkClicked);
            // 
            // lb_no
            // 
            this.lb_no.AutoSize = true;
            this.lb_no.Location = new System.Drawing.Point(41, 8);
            this.lb_no.Name = "lb_no";
            this.lb_no.Size = new System.Drawing.Size(65, 12);
            this.lb_no.TabIndex = 24;
            this.lb_no.TabStop = true;
            this.lb_no.Text = "按工号排序";
            this.lb_no.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lb_no_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lb_clear);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.tb_name);
            this.panel1.Controls.Add(this.cb_state);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cb_group);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1256, 35);
            this.panel1.TabIndex = 3;
            // 
            // lb_clear
            // 
            this.lb_clear.AutoSize = true;
            this.lb_clear.Location = new System.Drawing.Point(1040, 12);
            this.lb_clear.Name = "lb_clear";
            this.lb_clear.Size = new System.Drawing.Size(65, 12);
            this.lb_clear.TabIndex = 23;
            this.lb_clear.TabStop = true;
            this.lb_clear.Text = "清空已选项";
            this.lb_clear.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lb_clear_LinkClicked);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(941, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 22;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(508, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "工号,分机号或姓名";
            // 
            // tb_name
            // 
            this.tb_name.Location = new System.Drawing.Point(621, 8);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(201, 21);
            this.tb_name.TabIndex = 4;
            // 
            // cb_state
            // 
            this.cb_state.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_state.FormattingEnabled = true;
            this.cb_state.Location = new System.Drawing.Point(375, 9);
            this.cb_state.Name = "cb_state";
            this.cb_state.Size = new System.Drawing.Size(103, 20);
            this.cb_state.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(304, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "客服状态：";
            // 
            // cb_group
            // 
            this.cb_group.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_group.FormattingEnabled = true;
            this.cb_group.Location = new System.Drawing.Point(112, 9);
            this.cb_group.Name = "cb_group";
            this.cb_group.Size = new System.Drawing.Size(164, 20);
            this.cb_group.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "所属分组：";
            // 
            // panel_list
            // 
            this.panel_list.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_list.Controls.Add(this.flowPanel);
            this.panel_list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_list.Location = new System.Drawing.Point(0, 81);
            this.panel_list.Name = "panel_list";
            this.panel_list.Size = new System.Drawing.Size(1262, 247);
            this.panel_list.TabIndex = 4;
            // 
            // flowPanel
            // 
            this.flowPanel.AutoScroll = true;
            this.flowPanel.AutoScrollMargin = new System.Drawing.Size(0, 540);
            this.flowPanel.AutoSize = true;
            this.flowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanel.Location = new System.Drawing.Point(0, 0);
            this.flowPanel.Margin = new System.Windows.Forms.Padding(0);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.flowPanel.Size = new System.Drawing.Size(1260, 245);
            this.flowPanel.TabIndex = 3;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Mi_zhimang,
            this.Mi_zhixian,
            this.Mi_qianchu,
            this.Mi_jianting,
            this.Mi_qiangchai,
            this.Mi_qiangcha,
            this.Mi_jieguan});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(125, 158);
            // 
            // Mi_zhimang
            // 
            this.Mi_zhimang.Name = "Mi_zhimang";
            this.Mi_zhimang.Size = new System.Drawing.Size(124, 22);
            this.Mi_zhimang.Text = "强制置忙";
            this.Mi_zhimang.Click += new System.EventHandler(this.Mi_zhimang_Click);
            // 
            // Mi_zhixian
            // 
            this.Mi_zhixian.Name = "Mi_zhixian";
            this.Mi_zhixian.Size = new System.Drawing.Size(124, 22);
            this.Mi_zhixian.Text = "强制置闲";
            this.Mi_zhixian.Click += new System.EventHandler(this.Mi_zhixian_Click);
            // 
            // Mi_qianchu
            // 
            this.Mi_qianchu.Name = "Mi_qianchu";
            this.Mi_qianchu.Size = new System.Drawing.Size(124, 22);
            this.Mi_qianchu.Text = "强制签出";
            this.Mi_qianchu.Click += new System.EventHandler(this.Mi_qianchu_Click);
            // 
            // Mi_jianting
            // 
            this.Mi_jianting.Name = "Mi_jianting";
            this.Mi_jianting.Size = new System.Drawing.Size(124, 22);
            this.Mi_jianting.Text = "监听";
            this.Mi_jianting.Click += new System.EventHandler(this.Mi_jianting_Click);
            // 
            // Mi_qiangchai
            // 
            this.Mi_qiangchai.Name = "Mi_qiangchai";
            this.Mi_qiangchai.Size = new System.Drawing.Size(124, 22);
            this.Mi_qiangchai.Text = "强拆";
            this.Mi_qiangchai.Click += new System.EventHandler(this.Mi_qiangchai_Click);
            // 
            // Mi_qiangcha
            // 
            this.Mi_qiangcha.Name = "Mi_qiangcha";
            this.Mi_qiangcha.Size = new System.Drawing.Size(124, 22);
            this.Mi_qiangcha.Text = "强插";
            this.Mi_qiangcha.Click += new System.EventHandler(this.Mi_qiangcha_Click);
            // 
            // Mi_jieguan
            // 
            this.Mi_jieguan.Name = "Mi_jieguan";
            this.Mi_jieguan.Size = new System.Drawing.Size(124, 22);
            this.Mi_jieguan.Text = "接管";
            this.Mi_jieguan.Click += new System.EventHandler(this.Mi_jieguan_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // ListenControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel_list);
            this.Controls.Add(this.search_panel);
            this.Name = "ListenControl";
            this.Size = new System.Drawing.Size(1262, 328);
            this.search_panel.ResumeLayout(false);
            this.groupBox.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel_list.ResumeLayout(false);
            this.panel_list.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel search_panel;
        private System.Windows.Forms.Panel panel_list;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem Mi_zhimang;
        private System.Windows.Forms.ToolStripMenuItem Mi_zhixian;
        private System.Windows.Forms.ToolStripMenuItem Mi_jianting;
        private System.Windows.Forms.ToolStripMenuItem Mi_qiangchai;
        private System.Windows.Forms.ToolStripMenuItem Mi_qiangcha;
        private System.Windows.Forms.ToolStripMenuItem Mi_jieguan;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_group;
        private System.Windows.Forms.ComboBox cb_state;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.LinkLabel lb_clear;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel lb_state;
        private System.Windows.Forms.LinkLabel lb_no;
        private System.Windows.Forms.Label lb_tip;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripMenuItem Mi_qianchu;
        private System.Windows.Forms.Label lb_count;

    }
}
