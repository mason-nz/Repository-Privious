namespace CC2015_HollyFormsApp
{
    partial class TransferForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_line = new System.Windows.Forms.ComboBox();
            this.btnSkill = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_skill = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAgent = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvAgentList = new System.Windows.Forms.DataGridView();
            this.AgentNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AgentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtensionNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BGName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RegionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.te_input = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgentList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cb_line);
            this.groupBox1.Controls.Add(this.btnSkill);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cb_skill);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(468, 60);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "转接技能组";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "热线：";
            // 
            // cb_line
            // 
            this.cb_line.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_line.FormattingEnabled = true;
            this.cb_line.Location = new System.Drawing.Point(53, 24);
            this.cb_line.Name = "cb_line";
            this.cb_line.Size = new System.Drawing.Size(87, 20);
            this.cb_line.TabIndex = 3;
            // 
            // btnSkill
            // 
            this.btnSkill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSkill.Location = new System.Drawing.Point(370, 22);
            this.btnSkill.Name = "btnSkill";
            this.btnSkill.Size = new System.Drawing.Size(86, 23);
            this.btnSkill.TabIndex = 2;
            this.btnSkill.Text = "转接技能组";
            this.btnSkill.UseVisualStyleBackColor = true;
            this.btnSkill.Click += new System.EventHandler(this.btnSkill_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(146, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "技能组：";
            // 
            // cb_skill
            // 
            this.cb_skill.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_skill.FormattingEnabled = true;
            this.cb_skill.Location = new System.Drawing.Point(205, 24);
            this.cb_skill.Name = "cb_skill";
            this.cb_skill.Size = new System.Drawing.Size(159, 20);
            this.cb_skill.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnAgent);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.dgvAgentList);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.te_input);
            this.groupBox2.Location = new System.Drawing.Point(12, 78);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(468, 416);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "转接其他客服";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(68, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "工号,分机号或客服名称";
            // 
            // btnAgent
            // 
            this.btnAgent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAgent.Location = new System.Drawing.Point(370, 18);
            this.btnAgent.Name = "btnAgent";
            this.btnAgent.Size = new System.Drawing.Size(86, 23);
            this.btnAgent.TabIndex = 3;
            this.btnAgent.Text = "转接客服";
            this.btnAgent.UseVisualStyleBackColor = true;
            this.btnAgent.Click += new System.EventHandler(this.btnAgent_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(11, 395);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "提示：选择一行数据，双击进行操作！";
            // 
            // dgvAgentList
            // 
            this.dgvAgentList.AllowUserToAddRows = false;
            this.dgvAgentList.AllowUserToDeleteRows = false;
            this.dgvAgentList.AllowUserToOrderColumns = true;
            this.dgvAgentList.AllowUserToResizeColumns = false;
            this.dgvAgentList.AllowUserToResizeRows = false;
            this.dgvAgentList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAgentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAgentList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AgentNum,
            this.AgentName,
            this.ExtensionNum,
            this.StateName,
            this.BGName,
            this.RegionName});
            this.dgvAgentList.Location = new System.Drawing.Point(13, 47);
            this.dgvAgentList.MultiSelect = false;
            this.dgvAgentList.Name = "dgvAgentList";
            this.dgvAgentList.ReadOnly = true;
            this.dgvAgentList.RowHeadersVisible = false;
            this.dgvAgentList.RowTemplate.Height = 23;
            this.dgvAgentList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAgentList.Size = new System.Drawing.Size(443, 345);
            this.dgvAgentList.TabIndex = 18;
            this.dgvAgentList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAgentList_CellDoubleClick);
            // 
            // AgentNum
            // 
            this.AgentNum.DataPropertyName = "AgentNum";
            this.AgentNum.HeaderText = "客服工号";
            this.AgentNum.Name = "AgentNum";
            this.AgentNum.ReadOnly = true;
            this.AgentNum.Width = 80;
            // 
            // AgentName
            // 
            this.AgentName.DataPropertyName = "AgentName";
            this.AgentName.HeaderText = "客服名称";
            this.AgentName.Name = "AgentName";
            this.AgentName.ReadOnly = true;
            this.AgentName.Width = 80;
            // 
            // ExtensionNum
            // 
            this.ExtensionNum.DataPropertyName = "ExtensionNum";
            this.ExtensionNum.HeaderText = "分机号码";
            this.ExtensionNum.Name = "ExtensionNum";
            this.ExtensionNum.ReadOnly = true;
            this.ExtensionNum.Width = 80;
            // 
            // StateName
            // 
            this.StateName.DataPropertyName = "StateName";
            this.StateName.HeaderText = "客服状态";
            this.StateName.Name = "StateName";
            this.StateName.ReadOnly = true;
            this.StateName.Width = 80;
            // 
            // BGName
            // 
            this.BGName.HeaderText = "所属分组";
            this.BGName.Name = "BGName";
            this.BGName.ReadOnly = true;
            // 
            // RegionName
            // 
            this.RegionName.HeaderText = "区域";
            this.RegionName.Name = "RegionName";
            this.RegionName.ReadOnly = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(11, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "模糊查询：";
            // 
            // te_input
            // 
            this.te_input.Location = new System.Drawing.Point(205, 20);
            this.te_input.Name = "te_input";
            this.te_input.Size = new System.Drawing.Size(159, 21);
            this.te_input.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TransferForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 503);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransferForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "客服转接";
            this.Load += new System.EventHandler(this.TransferForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgentList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cb_skill;
        private System.Windows.Forms.Button btnSkill;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox te_input;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.DataGridView dgvAgentList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnAgent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn AgentNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn AgentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtensionNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn StateName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BGName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RegionName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_line;
    }
}