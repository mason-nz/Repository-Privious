namespace CC2015_HollyFormsApp.AutoUpdate
{
    partial class MainHTTP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainHTTP));
            this.picBusy = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBusy)).BeginInit();
            this.SuspendLayout();
            // 
            // picBusy
            // 
            this.picBusy.Image = global::CC2015_HollyFormsApp.AutoUpdate.Properties.Resources.blue_loading;
            this.picBusy.Location = new System.Drawing.Point(49, 32);
            this.picBusy.Name = "picBusy";
            this.picBusy.Size = new System.Drawing.Size(32, 32);
            this.picBusy.TabIndex = 4;
            this.picBusy.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "正在检测新版本";
            // 
            // MainHTTP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 113);
            this.ControlBox = false;
            this.Controls.Add(this.picBusy);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainHTTP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动升级";
            this.Load += new System.EventHandler(this.MainHTTP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBusy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBusy;
        private System.Windows.Forms.Label label1;
    }
}