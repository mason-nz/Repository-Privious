using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BitAuto.ISDC.CC2015.CallSync.Service.Text
{
    public partial class 手动执行任务 : Form
    {
        public 手动执行任务()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReplenishCallRecordJob job = new ReplenishCallRecordJob();
            job.Run();
        }
    }
}
