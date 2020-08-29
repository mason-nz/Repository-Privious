using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.IO;
using System.Timers;

namespace CC2015_HollyFormsApp
{
    public partial class Main : Form
    {
        System.Timers.Timer timer_after = new System.Timers.Timer();
        System.Timers.Timer timer_zhaiji = new System.Timers.Timer();
        System.Timers.Timer timer_jishi = new System.Timers.Timer();

        System.Timers.Timer timer_update = new System.Timers.Timer();
        System.Timers.Timer timer_buludata = new System.Timers.Timer();
        System.Timers.Timer timer_LoginTime = new System.Timers.Timer();
        System.Timers.Timer timer_net = new System.Timers.Timer();

        private void InitMainTimer()
        {
            //自动退出话后
            timer_after.Interval = 1000;
            timer_after.Enabled = false;
            timer_after.Elapsed += new ElapsedEventHandler(timer_after_Tick);
            //自动摘机
            timer_zhaiji.Interval = 1000;
            timer_zhaiji.Enabled = false;
            timer_zhaiji.Elapsed += new ElapsedEventHandler(timer_zhaiji_Tick);
            //通话计时
            timer_jishi.Interval = 1000;
            timer_jishi.Elapsed += new ElapsedEventHandler(timer_jishi_Tick);
            ///////////////////////////////////////////////////////////////////////////////

            //10s一次网络监测
            timer_net.Interval = 10 * 1000;
            timer_net.Elapsed += new ElapsedEventHandler(timer_net_Tick);
            timer_net.Start();
            //1分钟检查一次更新
            timer_update.Interval = 1 * 60 * 1000;
            timer_update.Elapsed += new ElapsedEventHandler(timer_update_Tick);
            timer_update.Start();
            //3分钟一次补录数据
            timer_buludata.Interval = 3 * 60 * 1000;
            timer_buludata.Elapsed += new ElapsedEventHandler(timer_buludata_Tick);
            timer_buludata.Start();
            //5分钟一次更新坐席状态
            timer_LoginTime.Interval = 5 * 60 * 1000;
            timer_LoginTime.Elapsed += new ElapsedEventHandler(timer_LoginTime_Tick);
            timer_LoginTime.Start();
        }

        /// 话后时间限制
        /// <summary>
        /// 话后时间限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_after_Tick(object sender, ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new System.Action(() => { timer_after_Tick(sender, e); }));
            }
            else
            {
                //由于异步问题，可能运行到此的时候，还未到达话后，所以前4s内(55)，暂不做判断
                if (Main_PhoneStatus == PhoneStatus.PS06_话后 || AfterTime > 55)
                {
                    if (AfterTime > 1)
                    {
                        AfterTime--;
                        SetlblAgentStatusName("话后 " + AfterTime);
                    }
                    else
                    {
                        //归0
                        AfterTime = 0;
                        if (UpdateTip.HasShow)
                        {
                            HollyContactHelper.Instance.ToReady();
                            AfterToReadyCallBack(o =>
                            {
                                HollyContactHelper.Instance.RestStart(BusyStatus.BS0_自动);
                            }, "话后到期，自动置闲");
                        }
                        else
                        {
                            HollyContactHelper.Instance.ToReady();
                        }
                        timer_after.Enabled = false;
                    }
                }
                else
                {
                    timer_after.Enabled = false;
                }
            }
        }
        /// 自动摘机
        /// <summary>
        /// 自动摘机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_zhaiji_Tick(object sender, ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new System.Action(() => { timer_zhaiji_Tick(sender, e); }));
            }
            else
            {
                if (Main_PhoneStatus == PhoneStatus.PS07_来电振铃)
                {
                    if (AfterTime > 1)
                    {
                        AfterTime--;
                        SetlblAgentStatusName(LabelTitle + " " + AfterTime, LabelColor);
                    }
                    else
                    {
                        //设置提示
                        SetlblAgentStatusName(LabelTitle, LabelColor);
                        //点击摘机按钮
                        toolSbtnAgentReleaseCall_Click(null, null);
                        //归0
                        AfterTime = 0;
                    }
                }
                else
                {
                    //提示文字异常，重新设置
                    if (this.lblAgentStatusName.Text.Contains(LabelTitle))
                    {
                        SetTitleLable(Main_PhoneStatus, Main_BusyStatus);
                    }
                    timer_zhaiji.Enabled = false;
                }
            }
        }
        /// 记录通话时长
        /// <summary>
        /// 记录通话时长
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_jishi_Tick(object sender, ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new System.Action(() => { timer_jishi_Tick(sender, e); }));
            }
            else
            {
                CallRecordLength++;
                toolStripStatusTime.Text = CallRecordLength.ToString() + " 秒";
            }
        }

        /// 网络监测
        /// <summary>
        /// 网络监测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_net_Tick(object sender, ElapsedEventArgs e)
        {
            timer_net.Stop();
            NetMonitor();
            timer_net.Start();
        }
        /// 自动更新
        /// <summary>
        /// 自动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_update_Tick(object sender, EventArgs e)
        {
            timer_update.Stop();
            //检查版本
            CheckVersion();
            //推送日志
            UpLoadLog();
            timer_update.Start();
        }
        /// 补录数据定时服务
        /// <summary>
        /// 补录数据定时服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_buludata_Tick(object sender, ElapsedEventArgs e)
        {
            timer_buludata.Stop();
            MDBFileHelper.Instance.AdditionalRecording();
            timer_buludata.Start();
        }
        /// 定时更新客服状态
        /// <summary>
        /// 定时更新客服状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_LoginTime_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                timer_LoginTime.Stop();
                //记录客服退出状态时间
                DateTime tdate = Common.GetCurrentTime();
                if (LoginUser.LoginOnOid != null)
                {
                    //更新在线时长
                    Loger.Log4Net.Info("[Main]timer_LoginTime_Tick LoginOnOid=" + LoginUser.LoginOnOid);
                    AgentTimeStateHelper.Instance.UpdateStateDetail2DB(Convert.ToInt32(LoginUser.LoginOnOid), tdate);
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[Main]timer_LoginTime_Tick errorMessage:" + ex.Message + ",StackTrace:" + ex.StackTrace);
            }
            finally
            {
                timer_LoginTime.Start();
            }
        }

        /// 网络监测
        /// <summary>
        /// 网络监测
        /// </summary>
        /// <param name="mainform"></param>
        private void NetMonitor()
        {
            try
            {
                //电话服务器
                string telip = HollyContactHelper.Instance.SearchIPByDN(CommonFunction.ObjectToInteger(LoginUser.ExtensionNum));
                //获取延时ms
                long ms = NetworkMonitoring.GetNetworkMonitoringMS(telip);
                //调用接口时间
                long call = NetworkMonitoring.GetNetworkMonitoringMS();
                //提示语句
                string msstr = "";
                if (call < 0)
                {
                    ms = 9000;
                    msstr = "断网";
                }
                else
                {
                    ms = Math.Max(ms, call);
                    msstr = ms + "ms";
                }
                var image = NetworkMonitoring.GetImage(ms);
                if (this.InvokeRequired)
                {
                    this.Invoke(new System.Action(() =>
                    {
                        this.imgtitle.Text = "网络延时：" + msstr + "";
                        this.toolStripStatusImg.Image = image;
                    }));
                }
                else
                {
                    this.imgtitle.Text = "网络延时：" + msstr + "";
                    this.toolStripStatusImg.Image = image;
                }
            }
            catch
            {
            }
        }
        /// 检查版本
        /// <summary>
        /// 检查版本
        /// </summary>
        private void CheckVersion()
        {
            //旧版本是否可用
            string OldVersionEnable = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("OldVersionEnable", false);
            //不可用（3种状态 true false null 只有ture不更新）
            if (OldVersionEnable != "true")
            {
                //签入之后 校验版本新旧
                if (LoginUser.isLoggedIn && UpdateTip.HasShow == false)
                {
                    //服务器版本
                    string serverVersion = Common.GetSeverVersion();
                    if (serverVersion != null)
                    {
                        //本地客户端版本
                        string myVersion = Common.GetValByKey("Versions_Local", "HTTP").Trim();
                        if (myVersion != serverVersion)
                        {
                            UpdateTip form = new UpdateTip();
                            if (this.InvokeRequired)
                            {
                                this.Invoke(new System.Action(() => { form.Show(); }));
                            }
                            else
                            {
                                form.Show();
                            }
                        }
                    }
                }
            }
        }
        /// 推送日志
        /// <summary>
        /// 推送日志
        /// </summary>
        private void UpLoadLog()
        {
            Loger.Log4Net.Info("[客户端推送日志信息] 开始");
            try
            {
                DataTable dt = ClientAssistantHelper.Instance.GetClientLogRequireInfo(LoginUser.UserID.Value);
                Loger.Log4Net.Info("[客户端推送日志信息] 获取到请求数：" + dt.Rows.Count);

                int i = 1;
                int count = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    string LogDate = dr["LogDate"].ToString();
                    DateTime logdate = CommonFunction.ObjectToDateTime(LogDate);
                    string filepath = AppDomain.CurrentDomain.BaseDirectory + "log\\";
                    string temppath = AppDomain.CurrentDomain.BaseDirectory + "log\\" + "log_" + logdate.ToString("yyyyMMdd") + "\\";
                    string filename = "log_" + logdate.ToString("yyyyMMdd") + ".log";
                    Loger.Log4Net.Info("[客户端推送日志信息] (" + i + "/" + count + ") 请求日期：" + LogDate + " ,请求文件：" + filename);

                    byte[] data = null;
                    if (File.Exists(filepath + filename))
                    {
                        if (!Directory.Exists(temppath))
                        {
                            Directory.CreateDirectory(temppath);
                        }
                        //移动主日志文件
                        File.Copy(filepath + filename, temppath + filename);
                        //移动厂家日志文件
                        DirectoryInfo dir = new DirectoryInfo(filepath);
                        var array = dir.GetFiles("all_msg.*");
                        if (array != null && array.Length != 0)
                        {
                            foreach (var item in array)
                            {
                                item.CopyTo(temppath + item.Name, true);
                                Loger.Log4Net.Info("[客户端推送日志信息] (" + i + "/" + count + ") 厂家文件：" + item.Name);
                            }
                        }
                        //移动本地数据库文件
                        File.Copy(AppDomain.CurrentDomain.BaseDirectory + "holly.mdb", temppath + "holly.mdb");
                        //移动话务错误日志
                        string calllog = filepath + "CallRecord_ORIG\\" + "SendError_" + logdate.ToString("yyyy_MM_dd") + ".txt";
                        if (File.Exists(calllog))
                        {
                            File.Copy(calllog, temppath + "SendError_" + logdate.ToString("yyyy_MM_dd") + ".txt");
                        }

                        //压缩文件
                        string bakname = "log_" + logdate.ToString("yyyyMMdd") + ".zip";
                        Common.ZipDir(temppath, filepath + bakname, 9);
                        //转二进制
                        if (File.Exists(filepath + bakname))
                        {
                            FileInfo bak = new FileInfo(filepath + bakname);
                            Loger.Log4Net.Info("[客户端推送日志信息] (" + i + "/" + count + ") 压缩后文件：" + bak.Name + " 大小：" + (bak.Length / 1024.0).ToString("0.000") + "KB");
                            data = BitAuto.ISDC.CC2012.BLL.Util.FileToBinary(filepath + bakname);
                            File.Delete(filepath + bakname);
                        }
                        Directory.Delete(temppath, true);
                    }
                    else
                    {
                        Loger.Log4Net.Info("[客户端推送日志信息] 文件不存在");
                    }

                    bool a = ClientAssistantHelper.Instance.PushClientLogForAgent(data, logdate, LoginUser.UserID.Value);
                    Loger.Log4Net.Info("[客户端推送日志信息] 上传文件：" + a);
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("客户端推送日志信息异常", ex);
            }
        }
    }
}
