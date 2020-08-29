namespace CC2012_CarolFormsApp
{
    partial class KeyBoard
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
            this.rdoNum1 = new System.Windows.Forms.RadioButton();
            this.rdoNum2 = new System.Windows.Forms.RadioButton();
            this.txtTelNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.labInfo = new System.Windows.Forms.Label();
            this.rdoNum3 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // rdoNum1
            // 
            this.rdoNum1.AutoSize = true;
            this.rdoNum1.Location = new System.Drawing.Point(79, 29);
            this.rdoNum1.Name = "rdoNum1";
            this.rdoNum1.Size = new System.Drawing.Size(83, 16);
            this.rdoNum1.TabIndex = 1;
            this.rdoNum1.Tag = "0";
            this.rdoNum1.Text = "4000716719";
            this.rdoNum1.UseVisualStyleBackColor = true;
            this.rdoNum1.CheckedChanged += new System.EventHandler(this.rdoNum1_CheckedChanged);
            // 
            // rdoNum2
            // 
            this.rdoNum2.AutoSize = true;
            this.rdoNum2.Checked = true;
            this.rdoNum2.Location = new System.Drawing.Point(171, 29);
            this.rdoNum2.Name = "rdoNum2";
            this.rdoNum2.Size = new System.Drawing.Size(83, 16);
            this.rdoNum2.TabIndex = 1;
            this.rdoNum2.Tag = "9";
            this.rdoNum2.Text = "4000168168";
            this.rdoNum2.UseVisualStyleBackColor = true;
            this.rdoNum2.CheckedChanged += new System.EventHandler(this.rdoNum2_CheckedChanged);
            // 
            // txtTelNum
            // 
            this.txtTelNum.Location = new System.Drawing.Point(79, 66);
            this.txtTelNum.Name = "txtTelNum";
            this.txtTelNum.Size = new System.Drawing.Size(177, 21);
            this.txtTelNum.TabIndex = 2;
            this.txtTelNum.TextChanged += new System.EventHandler(this.txtTelNum_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "对方号码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "外呼号码：";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(84, 117);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "呼 出";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(202, 117);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取 消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // labInfo
            // 
            this.labInfo.AutoSize = true;
            this.labInfo.ForeColor = System.Drawing.Color.Red;
            this.labInfo.Location = new System.Drawing.Point(118, 92);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(0, 12);
            this.labInfo.TabIndex = 5;
            // 
            // rdoNum3
            // 
            this.rdoNum3.AutoSize = true;
            this.rdoNum3.Location = new System.Drawing.Point(262, 29);
            this.rdoNum3.Name = "rdoNum3";
            this.rdoNum3.Size = new System.Drawing.Size(83, 16);
            this.rdoNum3.TabIndex = 1;
            this.rdoNum3.Tag = "5";
            this.rdoNum3.Text = "4000591591";
            this.rdoNum3.UseVisualStyleBackColor = true;
            // 
            // KeyBoard
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 172);
            this.Controls.Add(this.rdoNum3);
            this.Controls.Add(this.labInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTelNum);
            this.Controls.Add(this.rdoNum2);
            this.Controls.Add(this.rdoNum1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyBoard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "拨号";
            this.Load += new System.EventHandler(this.KeyBoard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoNum1;
        private System.Windows.Forms.RadioButton rdoNum2;
        private System.Windows.Forms.TextBox txtTelNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.Label labInfo;
        private System.Windows.Forms.RadioButton rdoNum3;
    }
}