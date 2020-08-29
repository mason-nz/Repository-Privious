using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;

namespace XYAuto.ITSC.Chitunion2017.NUnitTest.Utils
{
    /// <summary>
    /// 注释：FileShareHelper
    /// 作者：masj
    /// 日期：2018/6/1 11:51:23
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class FileShareHelper
    {
        //public static readonly FileShareHelper Instance = new FileShareHelper();

        public static bool connectState(string path)
        {
            return connectState(path, "", "");
        }

        public static bool connectState(string path, string userName, string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = @"net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }


        //read file  
        public static string ReadFiles(string path)
        {
            string content;
            try
            {

                // Create an instance of StreamReader to read from a file.  
                // The using statement also closes the StreamReader.  
                using (StreamReader sr = new StreamReader(path))
                {
                    //String line;
                    // Read and display lines from the file until the end of   
                    // the file is reached.  
                    content = sr.ReadToEnd();
                    //while ((line = sr.ReadLine()) != null)
                    //{
                    //    Console.WriteLine(line);

                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
                // Let the user know what went wrong.  
                //Console.WriteLine("The file could not be read:");
                //Console.WriteLine(e.Message);
            }
            return content;
        }
    }
}
