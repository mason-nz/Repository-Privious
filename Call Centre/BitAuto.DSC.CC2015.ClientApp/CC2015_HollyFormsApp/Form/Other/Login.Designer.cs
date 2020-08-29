namespace CC2015_HollyFormsApp
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbx_rpaw = new System.Windows.Forms.CheckBox();
            this.cbx_alogin = new System.Windows.Forms.CheckBox();
            this.lb_error = new System.Windows.Forms.Label();
            this.txtDomainAccount = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.lblPwd = new System.Windows.Forms.Label();
            this.labelDomainAccount = new System.Windows.Forms.Label();
            this.button = new System.Windows.Forms.Button();
            this.txtExtensionName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.axUniSoftPhone1 = new AxUniSoftPhoneControl.AxUniSoftPhone();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axUniSoftPhone1)).BeginInit();
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
            this.panel1.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.Login_navbg;
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
            this.label2.Size = new System.Drawing.Size(188, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "客户呼叫中心客户端（西安地区）";
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label2_MouseDown);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.Login_close;
            this.panel4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel4.Location = new System.Drawing.Point(340, 8);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 10);
            this.panel4.TabIndex = 6;
            this.panel4.Click += new System.EventHandler(this.Close_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.Login_user;
            this.panel3.Location = new System.Drawing.Point(7, 8);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(16, 16);
            this.panel3.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.Login_lgbg;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Controls.Add(this.cbx_rpaw);
            this.panel2.Controls.Add(this.cbx_alogin);
            this.panel2.Controls.Add(this.lb_error);
            this.panel2.Controls.Add(this.txtDomainAccount);
            this.panel2.Controls.Add(this.txtPwd);
            this.panel2.Controls.Add(this.lblPwd);
            this.panel2.Controls.Add(this.labelDomainAccount);
            this.panel2.Controls.Add(this.button);
            this.panel2.Controls.Add(this.txtExtensionName);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.axUniSoftPhone1);
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(354, 200);
            this.panel2.TabIndex = 4;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
            // 
            // cbx_rpaw
            // 
            this.cbx_rpaw.AutoSize = true;
            this.cbx_rpaw.Location = new System.Drawing.Point(278, 176);
            this.cbx_rpaw.Name = "cbx_rpaw";
            this.cbx_rpaw.Size = new System.Drawing.Size(72, 16);
            this.cbx_rpaw.TabIndex = 12;
            this.cbx_rpaw.Text = "记住密码";
            this.cbx_rpaw.UseVisualStyleBackColor = true;
            // 
            // cbx_alogin
            // 
            this.cbx_alogin.AutoSize = true;
            this.cbx_alogin.Location = new System.Drawing.Point(201, 176);
            this.cbx_alogin.Name = "cbx_alogin";
            this.cbx_alogin.Size = new System.Drawing.Size(72, 16);
            this.cbx_alogin.TabIndex = 11;
            this.cbx_alogin.Text = "自动登录";
            this.cbx_alogin.UseVisualStyleBackColor = true;
            // 
            // lb_error
            // 
            this.lb_error.AutoSize = true;
            this.lb_error.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_error.ForeColor = System.Drawing.Color.Red;
            this.lb_error.Location = new System.Drawing.Point(84, 123);
            this.lb_error.Name = "lb_error";
            this.lb_error.Size = new System.Drawing.Size(92, 17);
            this.lb_error.TabIndex = 10;
            this.lb_error.Text = "用户名密码错误";
            this.lb_error.Visible = false;
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
            // button
            // 
            this.button.BackColor = System.Drawing.Color.Transparent;
            this.button.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.Login_btn;
            this.button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button.FlatAppearance.BorderSize = 0;
            this.button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button.ForeColor = System.Drawing.Color.White;
            this.button.Location = new System.Drawing.Point(112, 145);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(76, 25);
            this.button.TabIndex = 5;
            this.button.Text = "登 录";
            this.button.UseVisualStyleBackColor = false;
            this.button.Click += new System.EventHandler(this.btnLogin_Click);
            this.button.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.button.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            // 
            // txtExtensionName
            // 
            this.txtExtensionName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExtensionName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtExtensionName.Location = new System.Drawing.Point(87, 97);
            this.txtExtensionName.Name = "txtExtensionName";
            this.txtExtensionName.Size = new System.Drawing.Size(139, 23);
            this.txtExtensionName.TabIndex = 3;
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
            // axUniSoftPhone1
            // 
            this.axUniSoftPhone1.Location = new System.Drawing.Point(3, 0);
            this.axUniSoftPhone1.Name = "axUniSoftPhone1";
            this.axUniSoftPhone1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axUniSoftPhone1.OcxState")));
            this.axUniSoftPhone1.Size = new System.Drawing.Size(192, 192);
            this.axUniSoftPhone1.TabIndex = 9;
            this.axUniSoftPhone1.Visible = false;
            // 
            // Login
            // 
            this.AcceptButton = this.button;
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
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "易车客服中心管理系统（西安地区）";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axUniSoftPhone1)).EndInit();
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
        private System.Windows.Forms.Button button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtExtensionName;
        private System.Windows.Forms.Label labelDomainAccount;
        private System.Windows.Forms.TextBox txtDomainAccount;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Label lblPwd;
        private AxUniSoftPhoneControl.AxUniSoftPhone axUniSoftPhone1;
        private System.Windows.Forms.Label lb_error;
        private System.Windows.Forms.CheckBox cbx_alogin;
        private System.Windows.Forms.CheckBox cbx_rpaw;
    }
}