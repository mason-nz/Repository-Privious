using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;
using System.Web;
using mshtml;
using System.Runtime.InteropServices;

namespace CC2015_HollyFormsApp
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Main : Form
    {
        //当前呼出窗口名称(GUID)
        private string CurrentOutBoundTabPageName = "";
        //外呼号码
        private string OutCallNumber = "";
        //新窗口打开
        private bool IsNewWin = false;
        //合力外呼消息（计算合力bug：真假失败）
        private bool ActCallOutResult = false;
        //合力外呼成功事件
        private Action ActCallOutSuccess = null;

        public Main()
        {
            InitializeComponent();
            InitUI();
            InitMainTimer();
            Microsoft.Win32.SystemEvents.TimeChanged += new EventHandler(SystemEvents_TimeChanged);
        }

        /// 窗口初始化
        /// <summary>
        /// 窗口初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Init_Load(object sender, EventArgs e)
        {
            //界面初始化
            InitUI_Load();
            //注册电话事件
            RegisterEvent();
            //重置数据
            LoginHelper.PreOid = -1;
            //取消代理
            RefreshIESettings("");
            //测试按钮
            ShowTest();
        }
        /// 本地时间被修改时触发
        /// <summary>
        /// 本地时间被修改时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            Common.RefreshTime();
        }

        #region 按钮
        /// 签入
        /// <summary>
        /// 签入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentCheckIn_Click(object sender, EventArgs e)
        {
            //预防代码进入
            if (sender != null && !Common.IsOldVersionContinue())
            {
                return;
            }
            Loger.Log4Net.Info("[Main] 点击按钮==签入");
            string errorMsg = "";
            HollyContactHelper.Instance.SignIn(LoginUser.AgentNum, LoginUser.ExtensionNum, out errorMsg);
        }
        /// 签出
        /// <summary>
        /// 签出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentCheckOff_Click(object sender, EventArgs e)
        {
            //预防代码进入
            if (sender != null && !Common.IsOldVersionContinue())
            {
                return;
            }
            Loger.Log4Net.Info("[Main] 点击按钮==签出");
            HollyContactHelper.Instance.SignOut();
        }

        /// 置闲
        /// <summary>
        /// 置闲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentReady_Click(object sender, EventArgs e)
        {
            //预防代码进入
            if (sender != null && !Common.IsOldVersionContinue())
            {
                return;
            }
            Loger.Log4Net.Info("[Main] 点击按钮==置闲");
            HollyContactHelper.Instance.ToReady();
            timer_after.Enabled = false;
        }
        /// 置忙
        /// <summary>
        /// 置忙
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnSetBusy_ButtonClick(object sender, EventArgs e)
        {
            //预防代码进入
            if (sender != null && !Common.IsOldVersionContinue())
            {
                return;
            }
            toolSbtnSetBusy.DropDown.Show(1, 2);
        }
        /// 客服置忙状态
        /// <summary>
        /// 客服置忙状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgentSetStateNotReady_Click(object sender, EventArgs e)
        {
            //预防代码进入
            if (sender != null && !Common.IsOldVersionContinue())
            {
                return;
            }
            ToolStripItem model = (ToolStripItem)sender;
            string name = model.Tag.ToString();
            Loger.Log4Net.Info("[Main] 点击按钮==置忙==" + name);
            BusyStatus busy = (BusyStatus)Enum.Parse(typeof(BusyStatus), name);

            //定义回调事件
            Action<object> callback = new Action<object>((x) =>
            {
                if (!HollyContactHelper.Instance.RestStart(busy))
                {
                    MessageBox.Show("置忙失败！");
                }
                else
                {
                    //窗口变量赋值
                    Main_BusyStatus = busy;
                }
            });
            //置闲成功后调用
            AfterToReadyCallBack(callback, "置闲后置忙");
        }
        /// 话后
        /// <summary>
        /// 话后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAfterCallWork_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此功能暂不支持！");
        }

        /// 外呼
        /// <summary>
        /// 外呼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentMakeCall_Click(object sender, EventArgs e)
        {
            //预防代码进入
            if (sender != null && !Common.IsOldVersionContinue())
            {
                return;
            }
            Loger.Log4Net.Info("[Main] 点击按钮==外呼");
            KeyBoard kform = new KeyBoard();
            if (kform.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string phoneNum = kform.ReturnNum;
                HollyContactHelper.ConDeviceType cdt = kform.ConDeviceType;
                //客户端呼出
                BusinessProcess.OutBoundType = OutBoundTypeEnum.OT2_客户端呼出;
                //呼叫具体实现
                MakeCall(phoneNum, cdt);
            }
        }
        /// 呼叫具体实现
        /// <summary>
        /// 呼叫具体实现
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="cdt"></param>
        private void MakeCall(string phoneNum, HollyContactHelper.ConDeviceType cdt, Action Success = null)
        {
            if (!Common.IsOldVersionContinue())
            {
                return;
            }
            //定义回调事件
            Action<object> callback = new Action<object>((x) =>
            {
                ActCallOutResult = HollyContactHelper.Instance.ActCallOut(cdt, phoneNum);
                //测试代码 ==ActCallOutResult = false; ==测试代码
                //记录外呼号码和事件
                OutCallNumber = phoneNum;
                ActCallOutSuccess = Success;
                // 测试代码：ActCallOutResult = false;
                if (!ActCallOutResult)
                {
                    //呼叫失败，自动休息
                    HollyContactHelper.Instance.RestStart(Main_BusyStatus);
                }
                else
                {
                    OutCall();
                }
            });
            //置闲成功后调用
            AfterToReadyCallBack(callback, "置闲后外呼");
        }
        private void OutCall()
        {
            //呼叫成功事件
            if (ActCallOutSuccess != null)
            {
                ActCallOutSuccess();
            }
            //外拨初始化事件
            AxUniSoftPhone_OnCallOutInitiated();
        }
        /// 挂断或者摘机
        /// <summary>
        /// 挂断或者摘机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentReleaseCall_Click(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[Main] 点击按钮==摘机/挂断");
            if (toolSbtnAgentReleaseCall.Enabled)
            {
                if (toolSbtnAgentReleaseCall.Text == "摘机")
                {
                    HollyContactHelper.Instance.ActAnswer();
                }
                else if (toolSbtnAgentReleaseCall.Text == "挂断")
                {
                    HollyContactHelper.Instance.ActHangup();
                }
                else if (toolSbtnAgentReleaseCall.Text == "取消监听")
                {
                    HollyContactHelper.Instance.ListenEnd();
                }
                else if (toolSbtnAgentReleaseCall.Text == "取消强插")
                {
                    HollyContactHelper.Instance.ListenEnd();
                }
                timer_zhaiji.Enabled = false;
            }
        }
        /// 保持
        /// <summary>
        /// 保持
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnHold_Click(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[Main] 点击按钮==保持/取消");
            if (toolSbtnHold.Text == "保持")
            {
                HollyContactHelper.Instance.HoldStart();
            }
            else if (toolSbtnHold.Text == "取消")
            {
                HollyContactHelper.Instance.HoldEnd();
            }
        }

        /// 转IVR
        /// <summary>
        /// 转IVR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnIVRSatisfaction_Click(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[Main] 点击按钮==转IVR");
            if (BusinessProcess.CallRecordORIG != null && BusinessProcess.CallRecordORIG.CallID.HasValue)
            {
                HollyContactHelper.Instance.TransferCallForIVR(BusinessProcess.CallRecordORIG.CallID.Value);
            }
            else
            {
                MessageBox.Show("本次通话不能转IVR");
            }
        }
        /// 转接
        /// <summary>
        /// 转接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentConsult_Click(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[Main] 点击按钮==转接/完成");
            if (toolSbtnAgentConsult.Text == "转接")
            {
                TransferForm al = new TransferForm();
                if (al.ShowDialog() == DialogResult.OK)
                {
                    if (al.ConDeviceType == HollyContactHelper.ConDeviceType.技能组)
                    {
                        //强转
                        if (!HollyContactHelper.Instance.TransferStart(al.OutNum))
                        {
                            MessageBox.Show("转接" + al.ConDeviceType + "失败！");
                        }
                        else
                        {
                            //转接完成
                            OnEndTransfer();
                        }
                    }
                    else
                    {
                        //咨询转接
                        if (!HollyContactHelper.Instance.ConsultStart(al.ConDeviceType, al.OutNum))
                        {
                            MessageBox.Show("转接" + al.ConDeviceType + al.OutNum + "失败！");
                        }
                        else
                        {
                            //咨询发起成功-数据入库
                            OnBeginConsult();
                        }
                    }
                }
            }
            else if (toolSbtnAgentConsult.Text == "完成")
            {
                HollyContactHelper.Instance.ConsultToTransfer();
                //转接完成
                OnEndTransfer();
            }
        }
        /// 恢复
        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentReconnect_Click(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[Main] 点击按钮==恢复");
            if (HollyContactHelper.Instance.ConsultEnd())
            {
                //咨询结束成功-数据入库
                OnEndConsult();
            }
        }

        /// 会议
        /// <summary>
        /// 会议
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentConference_Click(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[Main] 点击按钮==会议/结束");
            if (toolSbtnAgentConference.Text == "会议")
            {
                HollyContactHelper.Instance.ConsultToConference();
            }
            else if (toolSbtnAgentConference.Text == "结束")
            {
                HollyContactHelper.Instance.ConferenceEnd();
            }
        }
        /// 监控
        /// <summary>
        /// 监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentListen_Click(object sender, EventArgs e)
        {
            //预防代码进入
            if (sender != null && !Common.IsOldVersionContinue())
            {
                return;
            }
            Loger.Log4Net.Info("[Main] 点击按钮==监控");
            AddListenTabPage();
        }

        /// 主页
        /// <summary>
        /// 主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnHomePage_Click(object sender, EventArgs e)
        {
            //预防代码进入
            if (sender != null && !Common.IsOldVersionContinue())
            {
                return;
            }
            //取消代理
            RefreshIESettings("");
            GetCurrentBrowser().Navigate(DefaultURL);
        }
        /// 关于
        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnHelp_Click(object sender, EventArgs e)
        {
            AboutBox box = new AboutBox();
            box.ShowDialog();
        }
        #endregion

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
        /// 取消代理
        /// <summary>
        /// 取消代理
        /// </summary>
        /// <param name="strProxy"></param>
        public void RefreshIESettings(string strProxy)
        {
            try
            {
                const int INTERNET_OPTION_PROXY = 38;
                const int INTERNET_OPEN_TYPE_PROXY = 3;
                const int INTERNET_OPEN_TYPE_DIRECT = 1;

                Struct_INTERNET_PROXY_INFO struct_IPI;

                if (String.IsNullOrEmpty(strProxy))
                    struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_DIRECT;
                else
                    struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_PROXY;

                struct_IPI.proxy = Marshal.StringToHGlobalAnsi(strProxy);
                struct_IPI.proxyBypass = Marshal.StringToHGlobalAnsi("local");

                IntPtr intptrStruct = Marshal.AllocCoTaskMem(Marshal.SizeOf(struct_IPI));
                Marshal.StructureToPtr(struct_IPI, intptrStruct, true);
                bool iReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, intptrStruct, Marshal.SizeOf(struct_IPI));
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[Main] RefreshIESettings 异常：" + ex.Message);
            }
        }

        /// 新窗口
        /// <summary>
        /// 新窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            IsNewWin = !IsNewWin;
            if (IsNewWin)
            {
                toolStripButtonNew.Image = global::CC2015_HollyFormsApp.Properties.Resources.on;
            }
            else
            {
                toolStripButtonNew.Image = global::CC2015_HollyFormsApp.Properties.Resources.off;
            }
        }
        /// 重更新
        /// <summary>
        /// 重更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonRestart_Click(object sender, EventArgs e)
        {
            var a = MessageBox.Show("进行修复将关闭当前客户端，是否要继续？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (a == System.Windows.Forms.DialogResult.OK)
            {
                Common.SetValByKey("Versions_Local", "0.0", "HTTP");
                Common.Restart();
            }
        }
        private void ShowTest()
        {
            toolStripButtonNew.Visible = Common.IsTestVersion();
        }
    }

    public struct Struct_INTERNET_PROXY_INFO
    {
        public int dwAccessType;
        public IntPtr proxy;
        public IntPtr proxyBypass;
    }
}
