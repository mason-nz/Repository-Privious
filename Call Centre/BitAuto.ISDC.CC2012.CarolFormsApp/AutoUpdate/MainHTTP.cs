using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace AutoUpdate
{
    public partial class MainHTTP : Form
    {
        public MainHTTP()
        {
            InitializeComponent();
        }

        string serverVersion = "";//服务器上的版本号
        public delegate void myDelegate();

        private void MainHTTP_Load(object sender, EventArgs e)
        {
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
                MessageBox.Show("升级失败:" + ex.Message.ToString());

                //启动主程序
                //System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + Common.GetValByKey("StartApp", false, "HTTP"));

                this.Close();
            }

        }

        /// <summary>
        /// 启动升级
        /// </summary>
        private void StartUpdate()
        {
            try
            {
                this.picBusy.Visible = true;
                this.label1.Text = "正在升级...";

                string myVersion = Common.GetValByKey("Versions", false, "HTTP").Trim();//本地客户端版本
                string configFilePath = AppDomain.CurrentDomain.BaseDirectory + Common.GetValByKey("StartApp", false, "HTTP") + ".config";
                string myDefaultURL = Common.GetConfiguration("defaultURL", configFilePath);//系统默认登录地址
                string serverVersion = GetSeverVersion();//服务器版本
                
                if (myVersion != serverVersion)
                {
                    #region 版本不同，下载文件

                    string strImageURL = Common.GetValByKey("BaseURL", false, "HTTP") + Common.GetValByKey("UpdateFilePath", false, "HTTP") + "/update.zip";
                    System.Net.WebClient webClient = new System.Net.WebClient();
                    webClient.DownloadFile(strImageURL, AppDomain.CurrentDomain.BaseDirectory + "Temp.zip");

                    #endregion

                    #region 解压

                    Common.UnZip(AppDomain.CurrentDomain.BaseDirectory + "Temp.zip", AppDomain.CurrentDomain.BaseDirectory);

                    #endregion

                    #region 修改默认登录地址
                    Common.SetConfiguration("defaultURL",
                        string.IsNullOrEmpty(myDefaultURL) ? Common.GetValByKey("BaseURL", false, "HTTP") : myDefaultURL,
                        configFilePath);
                    #endregion

                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Temp.zip");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 获取服务器端的最新版本号
        /// </summary>
        /// <returns></returns>
        private string GetSeverVersion()
        {
            string strConfigURL = Common.GetValByKey("BaseURL", false, "HTTP") + Common.GetValByKey("UpdateFilePath", false, "HTTP") + "/config.xml";
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.DownloadFile(strConfigURL, AppDomain.CurrentDomain.BaseDirectory + "Temp.xml");

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

            try
            {
                Common.SetValByKey("Versions", serverVersion, "HTTP");

                ///启动主程序
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + Common.GetValByKey("StartApp", false, "HTTP"));

            }
            catch (Exception ex)
            {
                MessageBox.Show("升级失败:" + ex.Message.ToString());
            }
            finally
            {
                this.Close();

            }
        }
    }
}
