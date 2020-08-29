using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using mshtml;
using System.Web;
using System.Configuration;

namespace CC2015_HollyFormsApp
{
    /// 状态关联界面实现
    /// <summary>
    /// 状态关联界面实现
    /// </summary>
    public partial class Main : Form
    {
        /// 当前电话状态
        /// <summary>
        /// 当前电话状态
        /// </summary>
        public static PhoneStatus Main_PhoneStatus { set; get; }
        /// 当前置忙状态
        /// <summary>
        /// 当前置忙状态
        /// </summary>
        public static BusyStatus Main_BusyStatus { set; get; }
        /// 当前监控状态
        /// <summary>
        /// 当前监控状态
        /// </summary>
        public static HollyContactHelper.ConMonitorType Main_ConMonitor { set; get; }
        /// 当前监控电话的呼叫方向
        /// <summary>
        /// 当前监控电话的呼叫方向
        /// </summary>
        public static Calltype Main_ConMonitor_Calltype { set; get; }

        /// 对外提供接口，设置当前窗口的状态
        /// <summary>
        /// 对外提供接口，设置当前窗口的状态
        /// </summary>
        /// <param name="phonestatus"></param>
        /// <param name="busystatus"></param>
        public void SetMainStatus(PhoneStatus phonestatus)
        {
            string str = "";
            if (phonestatus == PhoneStatus.PS04_置忙)
            {
                str = "(" + Main_BusyStatus + ")";
            }
            Loger.Log4Net.Info("[Main][SetMainStatus]设置状态：" + phonestatus + str);
            //设置当前状态
            Main_PhoneStatus = phonestatus;
            //设置样式和文本
            SetButtonStyle(phonestatus, HollyContactHelper.Instance.GetCallDir());
            SetTitleLable(phonestatus, Main_BusyStatus);
        }

        /// 设置按钮样式
        /// <summary>
        /// 设置按钮样式
        /// </summary>
        /// <param name="phonestatus"></param>
        public void SetButtonStyle(PhoneStatus phonestatus, Calltype calltype)
        {
            //禁用全部按钮
            ButtonInit();
            switch (phonestatus)
            {
                case PhoneStatus.PS01_就绪:
                    break;
                case PhoneStatus.PS02_签出:
                    //签入
                    this.toolSbtnAgentCheckIn.Enabled = true;
                    break;
                case PhoneStatus.PS03_置闲:
                case PhoneStatus.PS18_客服被锁定:
                    //签出
                    this.toolSbtnAgentCheckOff.Enabled = true;
                    //置忙
                    this.toolSbtnSetBusy.Enabled = true;
                    //拨号
                    this.toolSbtnAgentMakeCall.Enabled = true;
                    //监控
                    this.toolSbtnAgentListen.Enabled = true;
                    this.toolSbtnAgentListen.Text = "监控";
                    break;
                case PhoneStatus.PS04_置忙:
                case PhoneStatus.PS05_休息:
                    //签出
                    this.toolSbtnAgentCheckOff.Enabled = true;
                    //置闲
                    this.toolSbtnAgentReady.Enabled = true;
                    //拨号
                    this.toolSbtnAgentMakeCall.Enabled = true;
                    //监控
                    this.toolSbtnAgentListen.Enabled = true;
                    this.toolSbtnAgentListen.Text = "监控";
                    break;
                case PhoneStatus.PS06_话后:
                    //签出
                    this.toolSbtnAgentCheckOff.Enabled = true;
                    //置闲
                    this.toolSbtnAgentReady.Enabled = true;
                    //置忙
                    this.toolSbtnSetBusy.Enabled = true;
                    break;
                case PhoneStatus.PS07_来电振铃:
                    //摘机
                    this.toolSbtnAgentReleaseCall.Enabled = true;
                    this.toolSbtnAgentReleaseCall.Text = "摘机";
                    this.toolSbtnAgentReleaseCall.Image = System.Drawing.Bitmap.FromFile(System.Environment.CurrentDirectory + "\\images\\摘机.png");
                    break;
                case PhoneStatus.PS08_普通通话:
                    //保持
                    this.toolSbtnHold.Enabled = true;
                    this.toolSbtnHold.Text = "保持";
                    if (calltype == Calltype.C1_呼入)
                    {
                        //转接
                        this.toolSbtnAgentConsult.Enabled = true;
                        this.toolSbtnAgentConsult.Text = "转接";
                        if (HollyContactHelper.Instance.IsAutoCall == false)
                        {
                            //转IVR
                            this.toolSbtnIVRSatisfaction.Enabled = true;
                        }
                    }
                    //挂断
                    this.toolSbtnAgentReleaseCall.Enabled = true;
                    this.toolSbtnAgentReleaseCall.Text = "挂断";
                    break;
                case PhoneStatus.PS09_咨询通话_发起方:
                    //完成
                    this.toolSbtnAgentConsult.Enabled = true;
                    this.toolSbtnAgentConsult.Text = "完成";
                    //会议
                    this.toolSbtnAgentConference.Enabled = true;
                    this.toolSbtnAgentConference.Text = "会议";
                    //恢复
                    this.toolSbtnAgentReconnect.Enabled = true;
                    //挂断
                    this.toolSbtnAgentReleaseCall.Enabled = true;
                    this.toolSbtnAgentReleaseCall.Text = "挂断";
                    break;
                case PhoneStatus.PS10_咨询方通话_接受者:
                    //挂断
                    this.toolSbtnAgentReleaseCall.Enabled = true;
                    this.toolSbtnAgentReleaseCall.Text = "挂断";
                    break;
                case PhoneStatus.PS11_会议通话_发起方:
                    //会议
                    this.toolSbtnAgentConference.Enabled = true;
                    this.toolSbtnAgentConference.Text = "结束";
                    //挂断
                    this.toolSbtnAgentReleaseCall.Enabled = true;
                    this.toolSbtnAgentReleaseCall.Text = "挂断";
                    break;
                case PhoneStatus.PS12_会议方通话_接受者:
                    //挂断
                    this.toolSbtnAgentReleaseCall.Enabled = true;
                    this.toolSbtnAgentReleaseCall.Text = "挂断";
                    break;
                case PhoneStatus.PS13_保持:
                    //取消
                    this.toolSbtnHold.Enabled = true;
                    this.toolSbtnHold.Text = "取消";
                    if (calltype == Calltype.C1_呼入)
                    {
                        //转接
                        this.toolSbtnAgentConsult.Enabled = true;
                        this.toolSbtnAgentConsult.Text = "转接";
                        if (HollyContactHelper.Instance.IsAutoCall == false)
                        {
                            //转IVR
                            this.toolSbtnIVRSatisfaction.Enabled = true;
                        }
                    }
                    //挂断
                    this.toolSbtnAgentReleaseCall.Enabled = true;
                    this.toolSbtnAgentReleaseCall.Text = "挂断";
                    break;
                case PhoneStatus.PS14_呼出拨号中:
                case PhoneStatus.PS15_咨询拨号中:
                case PhoneStatus.PS16_会议拨号中:
                case PhoneStatus.PS17_转接拨号中:
                    //挂断
                    this.toolSbtnAgentReleaseCall.Enabled = true;
                    this.toolSbtnAgentReleaseCall.Text = "挂断";
                    break;
                case PhoneStatus.PS19_监听振铃:
                case PhoneStatus.PS21_强插振铃:
                    ////摘机
                    //this.toolSbtnAgentReleaseCall.Enabled = true;
                    //this.toolSbtnAgentReleaseCall.Text = "摘机";
                    //this.toolSbtnAgentReleaseCall.Image = System.Drawing.Bitmap.FromFile(System.Environment.CurrentDirectory + "\\images\\摘机.png");
                    break;
                case PhoneStatus.PS20_监听中:
                    //挂断
                    this.toolSbtnAgentReleaseCall.Enabled = true;
                    this.toolSbtnAgentReleaseCall.Text = "取消监听";
                    break;
                case PhoneStatus.PS22_强插中:
                    //挂断
                    this.toolSbtnAgentReleaseCall.Enabled = true;
                    this.toolSbtnAgentReleaseCall.Text = "取消强插";
                    break;
            }
        }
        /// 设置标题文字
        /// <summary>
        /// 设置标题文字
        /// </summary>
        /// <param name="phonestatus"></param>
        /// <param name="busystatus"></param>
        public void SetTitleLable(PhoneStatus phonestatus, BusyStatus busystatus)
        {
            string title = "";
            Color c = Color.Black;
            switch (phonestatus)
            {
                case PhoneStatus.PS01_就绪:
                    SetlblAgentStatusName("");
                    break;
                case PhoneStatus.PS02_签出:
                    SetlblAgentStatusName("签出", Color.Red);
                    break;
                case PhoneStatus.PS03_置闲:
                    SetlblAgentStatusName("置闲");
                    break;
                case PhoneStatus.PS04_置忙:
                case PhoneStatus.PS05_休息:
                    title = "置忙";
                    string busy = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BusyStatus), (int)busystatus);
                    if (busy != "")
                    {
                        title += "\r\n(" + busy + ")";
                    }
                    SetlblAgentStatusName(title, Color.Red);
                    break;
                case PhoneStatus.PS06_话后:
                    title = "话后";
                    if (AfterTime > 0)
                    {
                        return;
                    }
                    SetlblAgentStatusName(title);
                    break;
                case PhoneStatus.PS07_来电振铃:
                    //在振铃事件中设置
                    break;
                case PhoneStatus.PS08_普通通话:
                    title = "通话中";
                    title = MonitorAddString(title, out c);
                    SetlblAgentStatusName(title, c);
                    break;
                case PhoneStatus.PS09_咨询通话_发起方:
                    SetlblAgentStatusName("咨询（发起）");
                    break;
                case PhoneStatus.PS10_咨询方通话_接受者:
                    SetlblAgentStatusName("咨询（接受）");
                    break;
                case PhoneStatus.PS11_会议通话_发起方:
                    SetlblAgentStatusName("会议（发起）");
                    break;
                case PhoneStatus.PS12_会议方通话_接受者:
                    SetlblAgentStatusName("会议（接受）");
                    break;
                case PhoneStatus.PS13_保持:
                    SetlblAgentStatusName("保持中");
                    break;
                case PhoneStatus.PS14_呼出拨号中:
                    SetlblAgentStatusName("拨号中");
                    break;
                case PhoneStatus.PS15_咨询拨号中:
                    SetlblAgentStatusName("拨号（咨询）");
                    break;
                case PhoneStatus.PS16_会议拨号中:
                    SetlblAgentStatusName("拨号（会议）");
                    break;
                case PhoneStatus.PS17_转接拨号中:
                    SetlblAgentStatusName("拨号（转接）");
                    break;
                case PhoneStatus.PS18_客服被锁定:
                    SetlblAgentStatusName("锁定客服", Color.Red);
                    break;
                case PhoneStatus.PS19_监听振铃:
                    SetlblAgentStatusName("监控\r\n（监听振铃）", Color.Blue);
                    break;
                case PhoneStatus.PS20_监听中:
                    SetlblAgentStatusName("监控\r\n（监听中）", Color.Blue);
                    break;
                case PhoneStatus.PS21_强插振铃:
                    SetlblAgentStatusName("监控\r\n（强插振铃）", Color.Blue);
                    break;
                case PhoneStatus.PS22_强插中:
                    SetlblAgentStatusName("监控\r\n（强插中）", Color.Blue);
                    break;
            }
            Application.DoEvents();
        }
        /// 监控设置
        /// <summary>
        /// 监控设置
        /// </summary>
        private string MonitorAddString(string title, out Color c)
        {
            c = Color.Black;
            string agentid = "";
            if (HollyContactHelper.Instance.IsSpecialAgentCall(out agentid))
            {
                title += "\r\n（专属客户）";
                c = Color.Orange;
            }
            if (Main_ConMonitor == HollyContactHelper.ConMonitorType.拦截)
            {
                title += "\r\n（" + Main_ConMonitor.ToString() + "）";
                c = Color.Blue;
            }
            return title;
        }
        /// 按钮初始化
        /// <summary>
        /// 按钮初始化
        /// </summary>
        private void ButtonInit()
        {
            //签入
            this.toolSbtnAgentCheckIn.Enabled = false;
            //签出
            this.toolSbtnAgentCheckOff.Enabled = false;
            //保持
            this.toolSbtnHold.Enabled = false;
            this.toolSbtnHold.Text = "保持";
            //转接
            this.toolSbtnAgentConsult.Enabled = false;
            this.toolSbtnAgentConsult.Text = "转接";
            //会议
            this.toolSbtnAgentConference.Enabled = false;
            this.toolSbtnAgentConference.Text = "会议";
            //恢复
            this.toolSbtnAgentReconnect.Enabled = false;

            //置忙
            this.toolSbtnSetBusy.Enabled = false;
            //置闲
            this.toolSbtnAgentReady.Enabled = false;
            //话后
            this.toolSbtnAfterCallWork.Enabled = false;

            //拨号
            this.toolSbtnAgentMakeCall.Enabled = false;
            //挂断
            this.toolSbtnAgentReleaseCall.Enabled = false;
            this.toolSbtnAgentReleaseCall.Text = "挂断";
            this.toolSbtnAgentReleaseCall.Image = System.Drawing.Bitmap.FromFile(System.Environment.CurrentDirectory + "\\images\\挂断.png");

            //转IVR
            this.toolSbtnIVRSatisfaction.Enabled = false;
            //监控
            this.toolSbtnAgentListen.Enabled = false;
            this.toolSbtnAgentListen.Text = "监控";
        }
        /// 设置状态文字
        /// <summary>
        /// 设置状态文字
        /// </summary>
        /// <param name="msg"></param>
        private void SetlblAgentStatusName(string title)
        {
            this.lblAgentStatusName.Text = title;
            this.lblAgentStatusName.ActiveLinkColor = Color.Black;
            this.lblAgentStatusName.ForeColor = Color.Black;
            Application.DoEvents();
        }
        /// 设置状态文字
        /// <summary>
        /// 设置状态文字
        /// </summary>
        /// <param name="title"></param>
        /// <param name="c"></param>
        private void SetlblAgentStatusName(string title, Color c)
        {
            this.lblAgentStatusName.Text = title;
            this.lblAgentStatusName.ActiveLinkColor = c;
            this.lblAgentStatusName.ForeColor = c;
            Application.DoEvents();
        }
    }
}
