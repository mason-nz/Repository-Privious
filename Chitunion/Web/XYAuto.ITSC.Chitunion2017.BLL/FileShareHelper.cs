using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    /// <summary>
    /// 注释：FileShareHelper
    /// 作者：masj
    /// 日期：2018/6/1 11:51:23
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    //public class FileShareHelper
    //{
    //    //public static readonly FileShareHelper Instance = new FileShareHelper();

    //    public static bool ConnectState(string path)
    //    {
    //        return ConnectState(path, "", "");
    //    }

    //    public static bool ConnectState(string path, string userName, string passWord)
    //    {
    //        bool Flag = false;
    //        Process proc = new Process();
    //        try
    //        {
    //            proc.StartInfo.FileName = "cmd.exe";
    //            proc.StartInfo.UseShellExecute = false;
    //            proc.StartInfo.RedirectStandardInput = true;
    //            proc.StartInfo.RedirectStandardOutput = true;
    //            proc.StartInfo.RedirectStandardError = true;
    //            proc.StartInfo.CreateNoWindow = true;
    //            proc.Start();
    //            string dosLine = @"net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
    //            proc.StandardInput.WriteLine(dosLine);
    //            proc.StandardInput.WriteLine("exit");
    //            while (!proc.HasExited)
    //            {
    //                proc.WaitForExit(1000);
    //            }
    //            string errormsg = proc.StandardError.ReadToEnd();
    //            proc.StandardError.Close();
    //            if (string.IsNullOrEmpty(errormsg))
    //            {
    //                Flag = true;
    //            }
    //            else
    //            {
    //                throw new Exception(errormsg);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            BLL.Loger.Log4Net.Error("访问共享目录文件中的文件出错", ex);
    //            throw ex;
    //        }
    //        finally
    //        {
    //            proc.Close();
    //            proc.Dispose();
    //        }
    //        return Flag;
    //    }


    //    //read file  
    //    public static string ReadFiles(string path)
    //    {
    //        string content;
    //        try
    //        {

    //            // Create an instance of StreamReader to read from a file.  
    //            // The using statement also closes the StreamReader.  
    //            using (StreamReader sr = new StreamReader(path))
    //            {
    //                //String line;
    //                // Read and display lines from the file until the end of   
    //                // the file is reached.  
    //                content = sr.ReadToEnd();
    //                //while ((line = sr.ReadLine()) != null)
    //                //{
    //                //    Console.WriteLine(line);

    //                //}
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //            // Let the user know what went wrong.  
    //            //Console.WriteLine("The file could not be read:");
    //            //Console.WriteLine(e.Message);
    //        }
    //        return content;
    //    }
    //}


    public class FileShareHelper : IDisposable
    {
        // obtains user token         
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        // closes open handes returned by LogonUser         
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        extern static bool CloseHandle(IntPtr handle);

        [DllImport("Advapi32.DLL")]
        static extern bool ImpersonateLoggedOnUser(IntPtr hToken);

        [DllImport("Advapi32.DLL")]
        static extern bool RevertToSelf();
        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_LOGON_NEWCREDENTIALS = 9;//域控中的需要用:Interactive = 2         
        private bool disposed;

        public FileShareHelper(string username, string password, string ip)
        {
            // initialize tokens         
            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);

            try
            {
                // get handle to token         
                bool bImpersonated = LogonUser(username, ip, password,
                    LOGON32_LOGON_NEWCREDENTIALS, LOGON32_PROVIDER_DEFAULT, ref pExistingTokenHandle);

                if (bImpersonated)
                {
                    if (!ImpersonateLoggedOnUser(pExistingTokenHandle))
                    {
                        int nErrorCode = Marshal.GetLastWin32Error();
                        throw new Exception("ImpersonateLoggedOnUser error;Code=" + nErrorCode);
                    }
                }
                else
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    throw new Exception("LogonUser error;Code=" + nErrorCode);
                }
            }
            finally
            {
                // close handle(s)         
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                RevertToSelf();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
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
