namespace CC2015_HollyFormsApp
{
    partial class UpdateTip
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
            this.BtnRe = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panelclose = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BtnTc = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnRe
            // 
            this.BtnRe.BackColor = System.Drawing.Color.Transparent;
            this.BtnRe.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.Login_btn;
            this.BtnRe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnRe.FlatAppearance.BorderSize = 0;
            this.BtnRe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRe.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnRe.ForeColor = System.Drawing.Color.White;
            this.BtnRe.Location = new System.Drawing.Point(48, 77);
            this.BtnRe.Name = "BtnRe";
            this.BtnRe.Size = new System.Drawing.Size(76, 25);
            this.BtnRe.TabIndex = 6;
            this.BtnRe.Text = "重  启";
            this.BtnRe.UseVisualStyleBackColor = false;
            this.BtnRe.Click += new System.EventHandler(this.BtnRe_Click);
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.Login_navbg;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.panelclose);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(274, 30);
            this.panel1.TabIndex = 7;
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
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "新版本提示";
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label2_MouseDown);
            // 
            // panelclose
            // 
            this.panelclose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelclose.BackColor = System.Drawing.Color.Transparent;
            this.panelclose.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.Login_close;
            this.panelclose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelclose.Location = new System.Drawing.Point(259, 8);
            this.panelclose.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.panelclose.Name = "panelclose";
            this.panelclose.Size = new System.Drawing.Size(10, 10);
            this.panelclose.TabIndex = 6;
            this.panelclose.Click += new System.EventHandler(this.panelclose_Click);
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
            // BtnTc
            // 
            this.BtnTc.BackColor = System.Drawing.Color.Transparent;
            this.BtnTc.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.graybtn;
            this.BtnTc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnTc.FlatAppearance.BorderSize = 0;
            this.BtnTc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTc.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnTc.ForeColor = System.Drawing.Color.Gray;
            this.BtnTc.Location = new System.Drawing.Point(154, 77);
            this.BtnTc.Name = "BtnTc";
            this.BtnTc.Size = new System.Drawing.Size(76, 25);
            this.BtnTc.TabIndex = 8;
            this.BtnTc.Text = "推  迟";
            this.BtnTc.UseVisualStyleBackColor = false;
            this.BtnTc.Click += new System.EventHandler(this.BtnTc_Click);
            // 
            // panel2
            // 
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.BtnRe);
            this.panel2.Controls.Add(this.BtnTc);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(274, 112);
            this.panel2.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(87, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "请尽快重新启动！";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(27, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "您好，客户端有新版本，通话功能已禁用";
            // 
            // UpdateTip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.Login_lgbg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(274, 142);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UpdateTip";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "更新提示";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnRe;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelclose;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button BtnTc;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
    }
}