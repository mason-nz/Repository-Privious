namespace CC2015_HollyFormsApp.AutoUpdate
{
    partial class VersionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionForm));
            this.rg1 = new System.Windows.Forms.RadioButton();
            this.rg2 = new System.Windows.Forms.RadioButton();
            this.btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rg1
            // 
            this.rg1.AutoSize = true;
            this.rg1.Location = new System.Drawing.Point(12, 13);
            this.rg1.Name = "rg1";
            this.rg1.Size = new System.Drawing.Size(95, 16);
            this.rg1.TabIndex = 0;
            this.rg1.Text = "线上正式版本";
            this.rg1.UseVisualStyleBackColor = true;
            this.rg1.CheckedChanged += new System.EventHandler(this.rg1_CheckedChanged);
            // 
            // rg2
            // 
            this.rg2.AutoSize = true;
            this.rg2.Checked = true;
            this.rg2.Location = new System.Drawing.Point(113, 13);
            this.rg2.Name = "rg2";
            this.rg2.Size = new System.Drawing.Size(95, 16);
            this.rg2.TabIndex = 1;
            this.rg2.TabStop = true;
            this.rg2.Text = "线下测试版本";
            this.rg2.UseVisualStyleBackColor = true;
            this.rg2.CheckedChanged += new System.EventHandler(this.rg2_CheckedChanged);
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(214, 10);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(75, 23);
            this.btn.TabIndex = 2;
            this.btn.Text = "确认";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // VersionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 38);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.rg2);
            this.Controls.Add(this.rg1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VersionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "版本切换";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rg1;
        private System.Windows.Forms.RadioButton rg2;
        private System.Windows.Forms.Button btn;
    }
}