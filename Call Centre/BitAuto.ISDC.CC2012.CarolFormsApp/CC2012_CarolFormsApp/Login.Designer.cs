namespace CC2012_CarolFormsApp
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ckxIsForceLogin = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelDomainAccount = new System.Windows.Forms.Label();
            this.lblPwd = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtDomainAccount = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "分机号码：";
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.BackgroundImage = global::CC2012_CarolFormsApp.Properties.Resources.Login_navbg;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(355, 30);
            this.panel1.TabIndex = 3;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(29, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "客户呼叫中心客户端";
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label2_MouseDown);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.BackgroundImage = global::CC2012_CarolFormsApp.Properties.Resources.Login_close;
            this.panel4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel4.Location = new System.Drawing.Point(340, 8);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 10);
            this.panel4.TabIndex = 6;
            this.panel4.Click += new System.EventHandler(this.panel4_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BackgroundImage = global::CC2012_CarolFormsApp.Properties.Resources.Login_user;
            this.panel3.Location = new System.Drawing.Point(7, 8);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(16, 16);
            this.panel3.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::CC2012_CarolFormsApp.Properties.Resources.Login_lgbg;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Controls.Add(this.txtDomainAccount);
            this.panel2.Controls.Add(this.txtPwd);
            this.panel2.Controls.Add(this.lblPwd);
            this.panel2.Controls.Add(this.labelDomainAccount);
            this.panel2.Controls.Add(this.ckxIsForceLogin);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.txtUserName);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(354, 200);
            this.panel2.TabIndex = 4;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // ckxIsForceLogin
            // 
            this.ckxIsForceLogin.AutoSize = true;
            this.ckxIsForceLogin.Location = new System.Drawing.Point(87, 129);
            this.ckxIsForceLogin.Name = "ckxIsForceLogin";
            this.ckxIsForceLogin.Size = new System.Drawing.Size(96, 16);
            this.ckxIsForceLogin.TabIndex = 4;
            this.ckxIsForceLogin.Text = "是否强制登录";
            this.ckxIsForceLogin.UseVisualStyleBackColor = true;
            this.ckxIsForceLogin.Visible = false;
            this.ckxIsForceLogin.CheckedChanged += new System.EventHandler(this.ckxIsForceLogin_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "强制登录：";
            this.label4.Visible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::CC2012_CarolFormsApp.Properties.Resources.Login_btn;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(111, 156);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 25);
            this.button1.TabIndex = 5;
            this.button1.Text = "登 录";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnLogin_Click);
            this.button1.MouseEnter += new System.EventHandler(this.button1_MouseEnter);
            this.button1.MouseLeave += new System.EventHandler(this.button1_MouseLeave);
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserName.Location = new System.Drawing.Point(87, 97);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(139, 23);
            this.txtUserName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(21, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "分机号码：";
            // 
            // labelDomainAccount
            // 
            this.labelDomainAccount.AutoSize = true;
            this.labelDomainAccount.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDomainAccount.Location = new System.Drawing.Point(34, 28);
            this.labelDomainAccount.Name = "labelDomainAccount";
            this.labelDomainAccount.Size = new System.Drawing.Size(56, 17);
            this.labelDomainAccount.TabIndex = 7;
            this.labelDomainAccount.Text = "域账号：";
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPwd.Location = new System.Drawing.Point(45, 61);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(44, 17);
            this.lblPwd.TabIndex = 8;
            this.lblPwd.Text = "密码：";
            // 
            // txtPwd
            // 
            this.txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPwd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPwd.Location = new System.Drawing.Point(87, 59);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(139, 23);
            this.txtPwd.TabIndex = 2;
            // 
            // txtDomainAccount
            // 
            this.txtDomainAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDomainAccount.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDomainAccount.Location = new System.Drawing.Point(87, 22);
            this.txtDomainAccount.Name = "txtDomainAccount";
            this.txtDomainAccount.Size = new System.Drawing.Size(139, 23);
            this.txtDomainAccount.TabIndex = 1;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(355, 230);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "易车客服中心管理系统";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.CheckBox ckxIsForceLogin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelDomainAccount;
        private System.Windows.Forms.TextBox txtDomainAccount;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Label lblPwd;
    }
}

