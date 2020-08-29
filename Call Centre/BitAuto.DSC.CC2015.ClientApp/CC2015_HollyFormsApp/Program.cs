using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CC2015_HollyFormsApp
{
    static class Program
    {
        static System.Threading.Mutex mutex;  //这个静态类型的Mutex是必需的  

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool IsFirstRun;
            string mutexName = Application.ProductName;
            mutex = new System.Threading.Mutex(true, mutexName, out IsFirstRun);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (IsFirstRun)
            {
                Application.Run(new Login());
                GC.SuppressFinalize(mutex);
            }
            else
            {
                MessageBox.Show("客户端程序已经启动，无法重复启动！");
            }
        }

        public static void Trace(string s)
        {
            System.Console.WriteLine(s);
            Loger.Log4Net.Error(s);
        }
    }
}
