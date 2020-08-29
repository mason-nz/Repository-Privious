namespace CC2015_HollyFormsApp
{
    partial class ItemUI
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
            this.picbox = new System.Windows.Forms.PictureBox();
            this.panel = new System.Windows.Forms.Panel();
            this.lb_name = new System.Windows.Forms.Label();
            this.lb_date = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picbox)).BeginInit();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // picbox
            // 
            this.picbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.picbox.Location = new System.Drawing.Point(0, 0);
            this.picbox.MaximumSize = new System.Drawing.Size(85, 48);
            this.picbox.MinimumSize = new System.Drawing.Size(85, 48);
            this.picbox.Name = "picbox";
            this.picbox.Size = new System.Drawing.Size(85, 48);
            this.picbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picbox.TabIndex = 0;
            this.picbox.TabStop = false;
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.White;
            this.panel.Controls.Add(this.lb_name);
            this.panel.Controls.Add(this.lb_date);
            this.panel.Controls.Add(this.picbox);
            this.panel.Location = new System.Drawing.Point(3, 3);
            this.panel.MaximumSize = new System.Drawing.Size(85, 85);
            this.panel.MinimumSize = new System.Drawing.Size(85, 85);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(85, 85);
            this.panel.TabIndex = 4;
            // 
            // lb_name
            // 
            this.lb_name.AutoSize = true;
            this.lb_name.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lb_name.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_name.Location = new System.Drawing.Point(0, 49);
            this.lb_name.Name = "lb_name";
            this.lb_name.Size = new System.Drawing.Size(37, 20);
            this.lb_name.TabIndex = 5;
            this.lb_name.Text = "强斐";
            this.lb_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_date
            // 
            this.lb_date.AutoSize = true;
            this.lb_date.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lb_date.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_date.Location = new System.Drawing.Point(0, 69);
            this.lb_date.Name = "lb_date";
            this.lb_date.Size = new System.Drawing.Size(48, 16);
            this.lb_date.TabIndex = 4;
            this.lb_date.Text = "3分16秒";
            this.lb_date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ItemUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel);
            this.Name = "ItemUI";
            this.Size = new System.Drawing.Size(91, 91);
            ((System.ComponentModel.ISupportInitialize)(this.picbox)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picbox;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label lb_name;
        private System.Windows.Forms.Label lb_date;
    }
}
