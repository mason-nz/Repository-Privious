using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ReplaceStr
{
    class Program
    {

        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

        public static bool HandlerRoutine(int CtrlType)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger(typeof(Program));
            switch (CtrlType)
            {
                case 0:
                    logger.Info("工具被强制关闭"); //Ctrl+C关闭

                    break;
                case 2:
                    Console.WriteLine("按控制台关闭按钮关闭");//按控制台关闭按钮关闭
                    break;
            }
            Console.ReadLine();
            return false;
        }
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger(typeof(Program));
            string replaceStrs = ConfigurationManager.AppSettings["ReplaceStr"];
            DirectoryInfo TheFolder = new DirectoryInfo(ConfigurationManager.AppSettings["Path"]);
            Dictionary<string, string> dicStr = new Dictionary<string, string>();
            foreach (var item in replaceStrs.Split('#'))
            {
                if (item.Contains("$"))
                {
                    string[] a = item.Split('$');
                    dicStr.Add(a[0], a[1]);
                }
            }
            if (dicStr.Count <= 0)
            {
                logger.Info("未查询到替换字符");
                return;
            }
            try
            {
                foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
                {
                    Thread oGetArgThread = new Thread(new System.Threading.ThreadStart(() =>
                    {
                        foreach (FileInfo NextFile in NextFolder.GetFiles())
                        {
                            string strContent = File.ReadAllText(NextFile.FullName);
                            foreach (var item in dicStr)
                            {
                                strContent = Regex.Replace(strContent, item.Key, item.Value);
                            }
                            File.WriteAllText(NextFile.FullName, strContent);
                            logger.Info(NextFile.FullName + "->替换成功");
                        }
                    }));
                    //oGetArgThread.IsBackground = true;
                    oGetArgThread.Start();
                    //string path =  NextFile.FullName;
                    //string con = "";
                    //FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    //StreamReader sr = new StreamReader(fs);
                    //con = sr.ReadToEnd();
                    //foreach (var item in dicStr)
                    //{
                    //    con = con.Replace(item.Key, item.Value);
                    //}
                    //sr.Close();
                    //fs.Close();
                    //FileStream fs2 = new FileStream(path, FileMode.Open, FileAccess.Write);
                    //StreamWriter sw = new StreamWriter(fs2);
                    //sw.WriteLine(con);
                    //sw.Close();
                    //fs2.Close();

                }
            }
            catch (Exception ex)
            {
                logger.Error("替换失败", ex);
            }
        }
    }
}
