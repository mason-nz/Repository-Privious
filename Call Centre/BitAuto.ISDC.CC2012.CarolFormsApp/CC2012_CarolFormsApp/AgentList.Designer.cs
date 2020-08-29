namespace CC2012_CarolFormsApp
{
    partial class AgentList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgentList));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dgvAgentList = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExtension = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgentList)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Elapsed);
            // 
            // dgvAgentList
            // 
            this.dgvAgentList.AllowUserToAddRows = false;
            this.dgvAgentList.AllowUserToDeleteRows = false;
            this.dgvAgentList.AllowUserToOrderColumns = true;
            this.dgvAgentList.AllowUserToResizeColumns = false;
            this.dgvAgentList.AllowUserToResizeRows = false;
            this.dgvAgentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAgentList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column4});
            this.dgvAgentList.Location = new System.Drawing.Point(28, 78);
            this.dgvAgentList.MultiSelect = false;
            this.dgvAgentList.Name = "dgvAgentList";
            this.dgvAgentList.ReadOnly = true;
            this.dgvAgentList.RowTemplate.Height = 23;
            this.dgvAgentList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAgentList.Size = new System.Drawing.Size(314, 353);
            this.dgvAgentList.TabIndex = 1;
            this.dgvAgentList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAgentList_CellClick);
            this.dgvAgentList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAgentList_CellDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "转接坐席分机：";
            // 
            // txtExtension
            // 
            this.txtExtension.Location = new System.Drawing.Point(121, 28);
            this.txtExtension.Name = "txtExtension";
            this.txtExtension.Size = new System.Drawing.Size(100, 21);
            this.txtExtension.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(59, 446);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(220, 446);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Extension";
            this.Column1.HeaderText = "坐席分机";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "StateName";
            this.Column2.HeaderText = "坐席状态";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 150;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "OrderNum";
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Visible = false;
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // AgentList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 481);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtExtension);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvAgentList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AgentList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坐席转接";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgentList)).EndInit();
            this.Load += new System.EventHandler(this.AgentList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAgentList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExtension;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
    }
}