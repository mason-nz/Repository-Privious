using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.Remoting.Messaging;


namespace AutoUpdate
{
    public partial class MainFTP : Form
    {
        private FTP ftp;
        string serverVersion = "";

        public MainFTP()
        {
            InitializeComponent();

            string RemoteHost = Common.GetValByKey("RemoteHost", true, "FTP");
            int RemotePort = int.Parse(Common.GetValByKey("RemotePort", true, "FTP"));
            string RemoteUser = Common.GetValByKey("RemoteUser", true, "FTP");
            string RemotePass = Common.GetValByKey("RemotePass", true, "FTP");

            ftp = new FTP(RemoteHost, RemotePort, RemoteUser, RemotePass);
        }

        public delegate void myDelegate();

        private void Form1_Load(object sender, EventArgs e)
        {
          // string s= Common.EncryptDES("ftpUser");
           ////s = Common.EncryptDES("Ftp.852");
           //s = Common.EncryptDES("192.168.0.130");
          // s = Common.EncryptDES(@"http://ncc.sys1.bitauto.com/");

            Control.CheckForIllegalCrossThreadCalls = false;

            Thread thread = new Thread(Run);
            thread.Start();
        }

        private void Run()
        {
            try
            {
                myDelegate updateDelegate = new myDelegate(StartUpdate);
                IAsyncResult iResult = updateDelegate.BeginInvoke(new AsyncCallback(CallBack), null);

            }
            catch (Exception ex)
            {
                //启动主程序
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + Common.GetValByKey("StartApp", false, "FTP"));
                this.Close();
            }
        }

        /// <summary>
        /// 获取服务器端的最新版本号
        /// </summary>
        /// <returns></returns>
        private string GetSeverVersion()
        {
            string info = "";
            ftp.Download("/", "config.xml", AppDomain.CurrentDomain.BaseDirectory, "Temp.xml", out info);

             serverVersion = Common.GetServerValByKey("Versions");

            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Temp.xml");
            return serverVersion.Trim();
        }

        /// <summary>
        /// 批量下载完成后的回调函数
        /// </summary>
        /// <param name="iasync"></param>
        private void CallBack(IAsyncResult iasync)
        {
            this.picBusy.Visible = false;
            this.label1.Text = "升级完成";

            Common.SetValByKey("Versions", serverVersion, "FTP");
 
            ///启动主程序
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + Common.GetValByKey("StartApp", false, "FTP"));
            this.Close();
        }


        /// <summary>  
        /// 批量下载，包括文件和文件夹
        /// </summary>  
        /// <param name="ftpPath">FTP路径</param>  
        /// <param name="name">需要下载文件路径</param>  
        /// <param name="MyPath">保存的本地路径</param>  
        public void DownAllItemByPath(FTP ftp, string ftpPath, string MyPath)
        {
            List<FtpItem> itemList = ftp.GetFileList(ftpPath);

            if (itemList != null)
            {
                foreach (FtpItem item in itemList)
                {
                    if (item.ItemType == 2)
                    {
                        //如果是文件夹，则递归调用遍历下载
                        DownAllItemByPath(ftp, ftpPath + @"\" + item.ItemName, MyPath + @"\" + item.ItemName);
                    }
                    else
                    {
                        string info = "";
                        //如果是文件，下载文件
                        ftp.Download(ftpPath, item.ItemName, MyPath, item.ItemName, out info);
                    }
                }
            }
        }

        /// <summary>
        /// 启动升级
        /// </summary>
        private void StartUpdate()
        {
            this.picBusy.Visible = true;
            this.label1.Text = "正在升级...";


            string myVersion = Common.GetValByKey("Versions",false,"FTP").Trim();//本地客户端版本
            string serverVersion = GetSeverVersion();//服务器版本

            if (myVersion != serverVersion)
            {

                #region 版本不同，升级

                DownAllItemByPath(ftp, Common.GetValByKey("UpdateFilePath", false, "FTP"), AppDomain.CurrentDomain.BaseDirectory);

                #endregion
            }
           
        }

    }
}
