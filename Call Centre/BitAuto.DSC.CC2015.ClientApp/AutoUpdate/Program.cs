using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace CC2015_HollyFormsApp.AutoUpdate
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //所有的进程
            Process[] updates = Process.GetProcessesByName(Application.ProductName);
            if (updates != null && updates.Length > 1)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainHTTP());
        }
    }
}
