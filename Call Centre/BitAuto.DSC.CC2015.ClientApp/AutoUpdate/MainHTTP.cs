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
using System.Diagnostics;

namespace CC2015_HollyFormsApp.AutoUpdate
{
    public partial class MainHTTP : Form
    {
        public static VersionType versiontype = VersionType.测试;
        //服务器上的版本号
        string serverVersion = "";
        //服务器上存储版本的节点名称
        string ServerVersionsName = "";
        //升级包名称
        string UploadFileName = "";

        public delegate void myDelegate();

        public MainHTTP()
        {
            this.Visible = false;
            //在“重启”中，需要等待主程序自己关闭
            Thread.Sleep(500);

            if (CheckMainProcess() == true || CheckChooseVersion() == true)
            {
                Environment.Exit(0);
                return;
            }

            InitializeComponent();
        }
        /// 检查主进程
        /// <summary>
        /// 检查主进程
        /// </summary>
        /// <returns></returns>
        private bool CheckMainProcess()
        {
            bool isexit = false;
            try
            {
                Process main = Common.GetMainAppProcess();
                if (main != null)
                {
                    var a = MessageBox.Show("客户端已开启，继续操作将重新启动客户端。是否继续？",
                        "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (a == System.Windows.Forms.DialogResult.Cancel)
                    {
                        isexit = true;
                    }
                    else
                    {
                        main.Kill();
                        isexit = false;
                    }
                }
            }
            catch
            {
            }
            return isexit;
        }
        /// 检查用户选择哪个版本
        /// <summary>
        /// 检查用户选择哪个版本
        /// </summary>
        /// <returns></returns>
        private bool CheckChooseVersion()
        {
            bool isexit = false;
            if (Common.IsCanShowVersionForm())
            {
                VersionForm form = new VersionForm();
                var a = form.ShowDialog();
                if (a == System.Windows.Forms.DialogResult.Cancel)
                {
                    isexit = true;
                }
            }
            return isexit;
        }

        private void MainHTTP_Load(object sender, EventArgs e)
        {
            //显示主窗口，开始更新
            this.Visible = true;
            string BaseURL = Common.GetValByKey("BaseURL", false, "HTTP");

            string title = "正式版本  ";
            if (BaseURL.Contains("1"))
            {
                title = "测试版本  ";
            }
            this.Text = title + "自动升级";

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
                this.Close();
            }
        }

        /// 启动升级
        /// <summary>
        /// 启动升级
        /// </summary>
        private void StartUpdate()
        {
            try
            {
                this.picBusy.Visible = true;
                this.label1.Text = "正在升级...";
                //节点名称
                ServerVersionsName = Common.GetValByKey("ServerVersionsName", false, "HTTP").Trim();
                //升级包名称
                UploadFileName = Common.GetValByKey("UploadFileName", false, "HTTP").Trim();
                //本地客户端版本
                string myVersion = Common.GetValByKey("Versions_Local", false, "HTTP").Trim();
                //服务器版本
                string serverVersion = GetSeverVersion();

                if (myVersion != serverVersion)
                {
                    string strImageURL = Common.GetValByKey("BaseURL", false, "HTTP") + Common.GetValByKey("UpdateFilePath", false, "HTTP") + "/" + UploadFileName;
                    System.Net.WebClient webClient = new System.Net.WebClient();
                    webClient.DownloadFile(strImageURL, AppDomain.CurrentDomain.BaseDirectory + "Temp_xian.zip");
                    Common.UnZip(AppDomain.CurrentDomain.BaseDirectory + "Temp_xian.zip", AppDomain.CurrentDomain.BaseDirectory);
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Temp_xian.zip");

                    //设置本地版本号
                    Common.SetValByKey("Versions_Local", serverVersion, "HTTP");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// 获取服务器端的最新版本号
        /// <summary>
        /// 获取服务器端的最新版本号
        /// </summary>
        /// <returns></returns>
        private string GetSeverVersion()
        {
            string strConfigURL = Common.GetValByKey("BaseURL", false, "HTTP") + Common.GetValByKey("UpdateFilePath", false, "HTTP") + "/config.xml";
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.DownloadFile(strConfigURL, AppDomain.CurrentDomain.BaseDirectory + "Temp_xian.xml");
            serverVersion = Common.GetServerValByKey(ServerVersionsName);
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Temp_xian.xml");
            return serverVersion.Trim();
        }
        /// 批量下载完成后的回调函数
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
                string appexe = Common.GetValByKey("StartApp", false, "HTTP");
                //启动之前，校验版本
                Uri update_url = new Uri(Common.GetValByKey("BaseURL", false, "HTTP"));
                //程序配置文件
                string appexeconfig = appexe + ".config";
                Uri app_url = new Uri(Common.GetConfiguration("DefaultURL", AppDomain.CurrentDomain.BaseDirectory + appexeconfig));
                if (update_url == app_url)
                {
                    //启动主程序
                    System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + appexe);
                }
                else
                {
                    MessageBox.Show("线上和线下版本切换失败，无法打开程序");
                }
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
