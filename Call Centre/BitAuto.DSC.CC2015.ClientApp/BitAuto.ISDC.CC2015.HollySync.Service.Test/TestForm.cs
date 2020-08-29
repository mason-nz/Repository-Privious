using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BitAuto.ISDC.CC2015.HollySync.Service;
using BitAuto.DSC.CC2015.AutoCall.Service.Helper;
using System.Threading;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2015.HollySync.Service.Test
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SyncCallDataJob job = new SyncCallDataJob();
            MessageBox.Show(job.CanRun().ToString());
            job.Run();
            MessageBox.Show("完成");


        }

        private void button2_Click(object sender, EventArgs e)
        {
            SyncBWDataJob job = new SyncBWDataJob();
            job.Run();
            MessageBox.Show("完成");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SyncMissDataJob job = new SyncMissDataJob();
            job.Run();
            MessageBox.Show("完成");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AutoCallSyncB_XJob job = new AutoCallSyncB_XJob();
            job.Run();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            CommonHelper.AutoCallHollyCallBack("31229", 3);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UpdateLineReportFromHollyToBJ job = new UpdateLineReportFromHollyToBJ();
            job.Run();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var a = CommonFunction.ObjectToDateTime("aaaa", DateTime.Today);
            var b = CommonFunction.ObjectToDouble("aaaa", 123);
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            SyncCallDisplayJob job = new SyncCallDisplayJob();
            job.Run();
        }
        Random random = new Random();
        private void button9_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int d = 100;
            List<string> list = new List<string>();
            for (int i = 0; i < d; i++)
            {
                string taskid = OrderCRMStopCustTask.Instance.CreatTaskID();
                if (!list.Contains(taskid))
                {
                    list.Add(taskid);
                }
            }
            int c = list.Count;
            sw.Stop();
            MessageBox.Show((c == d).ToString() + " " + sw.Elapsed);
        }
    }

    public static class testc
    {
        public static void GetTest(this SysRightUserInfo sysinfo)
        {
        }
    }
}
