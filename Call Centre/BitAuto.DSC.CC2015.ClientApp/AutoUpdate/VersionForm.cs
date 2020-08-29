using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CC2015_HollyFormsApp.AutoUpdate
{
    public partial class VersionForm : Form
    {
        public VersionForm()
        {
            InitializeComponent();
            //赋初始值
            Uri url = new Uri(Common.GetValByKey("BaseURL", false, "HTTP"));
            if (url == new Uri(VersionUrl.OnLine))
            {
                rg1.Checked = true;
                rg2.Checked = false;
                MainHTTP.versiontype = VersionType.正式;
            }
            else
            {
                rg1.Checked = false;
                rg2.Checked = true;
                MainHTTP.versiontype = VersionType.测试;
            }
        }

        private void rg1_CheckedChanged(object sender, EventArgs e)
        {
            if (rg1.Checked)
            {
                MainHTTP.versiontype = VersionType.正式;
            }
        }
        private void rg2_CheckedChanged(object sender, EventArgs e)
        {
            if (rg2.Checked)
            {
                MainHTTP.versiontype = VersionType.测试;
            }
        }

        private void SwitchVersion()
        {
            string url = Common.GetLineUrl();
            Common.SetValByKey("BaseURL", url, "HTTP");
            //校验
            url = Common.GetValByKey("BaseURL", false, "HTTP");
            if (url != Common.GetLineUrl())
            {
                MessageBox.Show("线上和线下版本切换失败");
                Application.Exit();
            }
        }
        private void btn_Click(object sender, EventArgs e)
        {
            SwitchVersion();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
