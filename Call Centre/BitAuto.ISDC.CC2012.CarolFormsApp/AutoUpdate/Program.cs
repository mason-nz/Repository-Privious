using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoUpdate
{
    static class Program
    {
        static System.Threading.Mutex mutex;  //这个静态类型的Mutex是必需的  

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool IsFirstRun;
            string mutexName = Application.ProductName;
            mutex = new System.Threading.Mutex(true, mutexName, out IsFirstRun);

            if (IsFirstRun)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainHTTP());
            }
            else
            {
                MessageBox.Show(Application.ProductName + "程序已经启动！");
            }
        }
    }
}
