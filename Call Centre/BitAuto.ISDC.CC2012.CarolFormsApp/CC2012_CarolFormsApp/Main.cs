using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RECONCOMLibrary;
using mshtml;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web;
using System.Configuration;

namespace CC2012_CarolFormsApp
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Main : Form
    {
        #region 程序关闭按钮处理事件
        private int WM_SYSCOMMAND = 0x112;
        private long SC_MAXIMIZE = 0xF030;
        private long SC_MINIMIZE = 0xF020;
        private long SC_CLOSE = 0xF060;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                //if (m.WParam.ToInt64() == SC_MAXIMIZE)
                //{
                //    MessageBox.Show("MAXIMIZE ");   
                //    return;
                //}
                //if (m.WParam.ToInt64() == SC_MINIMIZE)
                //{
                //    MessageBox.Show("MINIMIZE ");   
                //    return;
                //}
                if (m.WParam.ToInt64() == SC_CLOSE)
                {
                    //MessageBox.Show("SC_CLOSE "); 
                    //if (!IsCallingTel())
                    {
                        timer1.Stop();
                        CarolHelper.Instance.AllClear(Program.rc, LoginUser.ExtensionNum);
                        //this.Close();
                        System.Environment.Exit(0);
                    }
                    return;
                }
            }
            base.WndProc(ref m);
        }
        #endregion

        #region 声明变量
        private int TITLE_COUNT = 8;//TabPage每页标题显示字符数
        private string DefaultURL = System.Configuration.ConfigurationSettings.AppSettings["defaultURL"].ToString();
        private string CallRecordAuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["CallRecordAuthorizeCode"];//调用话务记录接口授权码
        public static int CallID;//当前电话ID
        public static Int64 NewCallID;//当前新生成电话ID(西门子CallID会有重复的情况)
        private ArrayList StrMsg = new ArrayList();//CTI处理事件字符串
        private int CallRecordLength = 0;//客户端下方，电话通话时长
        private bool IsCallRecordTimer = false;//是否计时统计通话时长
        private static bool IsEstablished = false;//是否接通电话
        const int CLOSE_SIZE = 12;//关闭图片大小
        private DateTime? RecomCOMEventStartTime = null;//CTI各个事件间隔时长（秒）
        private DateTime? RecomCOMEventEstablishedStartTime = null;//CTI接通事件当前时间
        private string CurrentEventName = string.Empty;
        private DataTable BusyReasonDT = null;//置忙原因数据
        Bitmap image = new Bitmap(System.AppDomain.CurrentDomain.BaseDirectory + "image\\closeGray.png");//tabControl中关闭按钮的图标
        private bool IsScriptErrorsSuppressed = false;//是否忽略脚本错误
        private string CurrentOutBoundTabPageName = string.Empty;//当前呼出窗口名称(GUID)

        public static int outBoundType = 0;//点击拨号时，是客户端还是网页；页面-1；客户端-2；
        private static int releaseCount = 0;            //挂断次数，如果等于2肯定是客户先挂断，反之如果等于1肯定是坐席先挂，只会记坐席挂断时间
        public static readonly DateTime DATE_INVALID_VALUE = new DateTime(1900, 01, 01);
        private DateTime? releaseTime1 = DATE_INVALID_VALUE;//第一次挂断时间
        private DateTime? releaseTime2 = DATE_INVALID_VALUE;//第二次挂断时间，有可能为空


        Int64 eventInitiatedCallID = -2;
        public static Int64 eventRingingCallID = -2;
        Int64 eventReleasedCallID = -2;

        int gAgentState = -2;
        int PreAgentState = -2;
        int gAuxState = -2;
        int PreAuxState = -2;
        int gCallType = -2;
        int gTimestamp = -2;
        int gStateDetailID = -2;
        //AgentEvent gAgentEvent = null;
        static int gTimetick = 0;
        private delegate void EVTHandler_SaveAgentState2DB(AgentEvent ae);
        private event EVTHandler_SaveAgentState2DB EVTSaveAgentState2DB;

        private delegate void EVTHandler_CTIEventMsg(string msg);
        private event EVTHandler_CTIEventMsg EVTCTIEventMsg;

        //发送邮件
        private delegate void EVTHandler_SendErrorEmail(string errorMsg, string source, string stackTrace);
        private event EVTHandler_SendErrorEmail EVTSendErrorEmail;

        #endregion

        #region 主函数入口
        public Main()
        {
            GC.KeepAlive(timer1);
            InitializeComponent();

            #region 初始化西门子插件中，事件委托
            Program.rc.OnConnectionEvent += new RECONCOMLibrary._IReconCOMEvents_OnConnectionEventEventHandler(recon_OnConnectionEvent);
            Program.rc.OnCTIEvent += new RECONCOMLibrary._IReconCOMEvents_OnCTIEventEventHandler(recon_OnCTIEvent);
            Program.rc.OnAgentEvent += new RECONCOMLibrary._IReconCOMEvents_OnAgentEventEventHandler(recon_OnAgentEvent);
            #endregion

            #region SSK初始化样式文件
            this.skinEngine1.DisableTag = 9999;
            this.tabControl1.Tag = 9999;
            this.skinEngine1.SkinFile = "skin/" + Common.GetSkinFileName();
            #endregion

            #region TabControl右侧添加关闭图片
            //this.tabControl1.SizeMode = TabSizeMode.Fixed;
            //this.tabControl1.ItemSize = new Size(300, 20);
            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Padding = new System.Drawing.Point(5, 5);
            this.tabControl1.DrawItem += new DrawItemEventHandler(this.MainTabControl_DrawItem);
            this.tabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainTabControl_MouseDown);
            #endregion

            #region 初始化置忙内容
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");

            dt.Rows.Add(new object[] { "小休", "1" });
            dt.Rows.Add(new object[] { "任务回访", "2" });
            dt.Rows.Add(new object[] { "业务处理", "3" });
            dt.Rows.Add(new object[] { "会议", "4" });
            dt.Rows.Add(new object[] { "培训", "5" });
            dt.Rows.Add(new object[] { "离席", "6" });
            BusyReasonDT = dt;
            //this.toolStripComboBox1.ComboBox.DataSource = dt;
            //toolStripComboBox1.ComboBox.DisplayMember = "Name";
            //toolStripComboBox1.ComboBox.ValueMember = "Value";
            //this.toolStripComboBox1.ComboBox.SelectedIndex = 0;
            #endregion

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionFunction);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

        }
        #endregion

        #region 异常处理
        static void UnhandledExceptionFunction(Object sender, UnhandledExceptionEventArgs args)
        {

            Loger.Log4Net.Info("[Main]UnhandledExceptionFunction begin...");

            Exception e = (Exception)args.ExceptionObject;
            Loger.Log4Net.Info("[Main]CC客户端出错...", e);
            SqlTool tool = new SqlTool();
            DateTime tdate = tool.GetCurrentTime();
            if (LoginUser.LoginOnOid != null)
                tool.UpdateStateDetail2DB(Convert.ToInt32(LoginUser.LoginOnOid), tdate);

            if (LoginUser.UserID != null)
                tool.DeleteAgentState2DB();

            Loger.Log4Net.Info("[Main]UnhandledExceptionFunction bye...");
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs args)
        {

            Loger.Log4Net.Info("[Main]Application_ThreadException begin...");

            Loger.Log4Net.Info("[Main]CC客户端出错...");

            SqlTool tool = new SqlTool();
            DateTime tdate = tool.GetCurrentTime();
            if (LoginUser.LoginOnOid != null)
                tool.UpdateStateDetail2DB(Convert.ToInt32(LoginUser.LoginOnOid), tdate);

            if (LoginUser.UserID != null)
                tool.DeleteAgentState2DB();

            Loger.Log4Net.Info("[Main]Application_ThreadException bye...");
        }

        #endregion

        #region 初始化工作
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("易车客服中心管理系统                            当前登录坐席分机：{0}，登录姓名：{1}，所在部门：{2}",
                LoginUser.ExtensionNum, LoginUser.TrueName, LoginUser.Department == null ? string.Empty : LoginUser.Department.Name);
            initMainForm();
            //lblUserName.Text = Login.UserName;

            ChangeBtnStyle();

            EVTSaveAgentState2DB += new EVTHandler_SaveAgentState2DB(Main_EVTSaveAgentState2DB);

            EVTCTIEventMsg += new EVTHandler_CTIEventMsg(Main_EVTCTIEventMsg);

            EVTSendErrorEmail += new EVTHandler_SendErrorEmail(Main_EVTSendErrorEmail);
        }

        void Main_EVTSendErrorEmail(string errorMsg, string source, string stackTrace)
        {
            Loger.Log4Net.Info("[Main]开始发送报错邮件，CallID：" + NewCallID);
            string mailBody = string.Format("错误信息：{0}<br/>错误Source：{1}<br/>错误StackTrace：{2}<br/>登录坐席：{3}<br/>",
                    errorMsg, source, stackTrace, LoginUser.TrueName);
            string subject = "北京CC客户端记录话务总表数据出错";

            string userEmails = ConfigurationManager.AppSettings["ReceiveErrorEmail"];
            string[] userEmail = userEmails.Split(';');
            BitAuto.ISDC.CC2012.BLL.EmailHelper.Instance.SendErrorMail(mailBody, subject, userEmail);
        }

        void Main_EVTCTIEventMsg(string msg)
        {
            CTIEventMsg(msg);
        }

        void Main_EVTSaveAgentState2DB(AgentEvent ae)
        {
            SaveAgentState2DB(ae);
        }

        /// <summary>
        /// 修改工具栏按钮样式
        /// </summary>
        private void ChangeBtnStyle()
        {
            int btnCount = this.toolStripAgentTool.Items.Count;
            for (int i = 0; i < btnCount; i++)
            {
                if (this.toolStripAgentTool.Items[i].GetType() == typeof(ToolStripButton) ||
                     this.toolStripAgentTool.Items[i].GetType() == typeof(System.Windows.Forms.ToolStripSplitButton))
                {
                    this.toolStripAgentTool.Items[i].AutoSize = false;
                    this.toolStripAgentTool.Items[i].Size = new Size(60, 60);
                    this.toolStripAgentTool.Items[i].Font = new Font("微软雅黑", (float)10.5);
                    this.toolStripAgentTool.Items[i].ForeColor = Color.FromArgb(11, 107, 178);
                    this.toolStripAgentTool.Items[i].Margin = new Padding(0, 10, 0, 2);
                }
            }
        }
        /// <summary>
        /// 初始化浏览器
        /// </summary>
        private void initMainForm()
        {
            TabPage mypage = new TabPage();
            mypage.Name = Guid.NewGuid().ToString();
            WebBrowser tempBrowser = new WebBrowser();
            tempBrowser.Navigated += new WebBrowserNavigatedEventHandler(tempBrowser_Navigated);
            tempBrowser.NewWindow += new CancelEventHandler(tempBrowser_NewWindow);
            tempBrowser.StatusTextChanged += new EventHandler(tempBrowser_StatusTextChanged);
            tempBrowser.ProgressChanged += new WebBrowserProgressChangedEventHandler(tempBrowser_ProgressChanged);
            tempBrowser.Dock = DockStyle.Fill;

            string url = "/?DomainAccount=" + LoginUser.DomainAccount + "&Password=" + HttpUtility.UrlEncode(LoginUser.Password);
            //tempBrowser.GoHome();//和新建空白页不同
            tempBrowser.Navigate(DefaultURL + url);
            tempBrowser.ObjectForScripting = this;
            tempBrowser.ScriptErrorsSuppressed = IsScriptErrorsSuppressed;
            mypage.Controls.Add(tempBrowser);
            tabControl1.TabPages.Add(mypage);
            //增加selectedtab
            //tabControl1.SelectedTab = mypage;
        }
        #endregion

        #region 坐席状态记录
        public void SaveAgentState2DB(AgentEvent ae)
        {
            Loger.Log4Net.Info("[Main]SaveAgentState2DB...begin...");
            SqlTool tool = new SqlTool();

            gAgentState = ae.AgentState;
            gAuxState = ae.AgentAuxState;
            gTimestamp = ae.Timestamp;

            DateTime startTime = GetTime(gTimestamp);

            if (gAgentState == 1)
            {
                LoginUser.EndTime = startTime;
                Loger.Log4Net.Info("[Main]SaveAgentState2DB...退出时间..." + startTime.ToLongTimeString());
            }

            if (gAgentState != -2)
            {
                Loger.Log4Net.Info("[Main]SaveAgentState2DB...事件发生...gAgentState..." + gAgentState);
                if (PreAgentState == -2)
                {
                    Loger.Log4Net.Info("[Main]SaveAgentState2DB...上个状态初始化...PreAgentState=-2...");
                    //登录后第一次事件发生
                    /*向CAgen、AgentStateDetail表插入记录*/
                    //Loger.Log4Net.Info("[Main]SaveAgentState2DB...插入临时状态01...");
                    if (tool.InsertAgentState2DB(gAgentState, gAuxState, gCallType, 0, startTime))
                    {
                        //Loger.Log4Net.Info("[Main]InsertAgentState2DB...向临时状态表插入数据失败...");
                    }
                    //Loger.Log4Net.Info("[Main]SaveAgentState2DB...插入明细状态01...");
                    gStateDetailID = tool.InsertAgentStateDetail2DB(gAgentState, gAuxState, gCallType, startTime, startTime);
                    if (gStateDetailID < 0)
                    {
                        //Loger.Log4Net.Info("[Main]InsertAgentState2DB...向状态明细表插入数据失败...");
                    }

                    PreAgentState = gAgentState;
                    PreAuxState = gAuxState;
                }
                else
                {
                    //Loger.Log4Net.Info("[Main]SaveAgentState2DB...上个状态有值...PreAgentState..." + PreAgentState);
                    if (PreAgentState != gAgentState)
                    {
                        gTimetick = 0;
                        //Loger.Log4Net.Info("[Main]SaveAgentState2DB...上个状态与当前状态不同...gAgentState..." + gAgentState);
                        //更新状态临时表为下个状态，持续时间清零
                        tool.UpdateAgentState2DB(gAgentState, gAuxState, gCallType, 0, startTime);

                        //更新上个状态明细，结束时间（gTimestamp）
                        //Loger.Log4Net.Info("[Main]SaveAgentState2DB...更新明细状态02...");
                        tool.UpdateStateDetail2DB(gStateDetailID, startTime);
                        //插入下个状态明细，开始时间（gTimestamp）
                        //Loger.Log4Net.Info("[Main]SaveAgentState2DB...插入明细状态02...");
                        gStateDetailID = tool.InsertAgentStateDetail2DB(gAgentState, gAuxState, gCallType, startTime, startTime);
                        PreAgentState = gAgentState;
                        PreAuxState = gAuxState;
                    }
                    else
                    {
                        //Loger.Log4Net.Info("[Main]SaveAgentState2DB...上个状态与当前状态相同...gAgentState..." + gAgentState);
                        //Loger.Log4Net.Info("[Main]SaveAgentState2DB...上个辅助状态与当前辅助状态作比较...PreAuxState..." + PreAuxState);
                        if (PreAuxState != gAuxState)
                        {
                            gTimetick = 0;
                            //Loger.Log4Net.Info("[Main]SaveAgentState2DB...上个辅助状态与当前辅助状态不同...gAuxState..." + gAuxState);
                            //更新状态临时表为下个状态，持续时间清零
                            tool.UpdateAgentState2DB(gAgentState, gAuxState, gCallType, 0, startTime);

                            //更新上个状态明细，结束时间（gTimestamp）
                            //Loger.Log4Net.Info("[Main]SaveAgentState2DB...更新明细状态03...");
                            tool.UpdateStateDetail2DB(gStateDetailID, startTime);
                            //插入下个状态明细，开始时间（gTimestamp）
                            //Loger.Log4Net.Info("[Main]SaveAgentState2DB...插入明细状态03...");
                            gStateDetailID = tool.InsertAgentStateDetail2DB(gAgentState, gAuxState, gCallType, startTime, startTime);
                            //PreAgentState = gAgentState;
                            PreAuxState = gAuxState;
                        }
                        else
                        {
                            Loger.Log4Net.Info("[Main]SaveAgentState2DB...上个辅助状态与当前辅助状态相同...gAuxState..." + gAuxState);
                        }
                    }
                }
            }
            else
            {
                Loger.Log4Net.Info("[Main]SaveAgentState2DB...gAgentState=-2...事件还未发生过...");
            }

            Loger.Log4Net.Info("[Main]SaveAgentState2DB...bye...");
        }
        #endregion

        #region 西门子电话3个回调事件（连接、坐席、CTI）
        public void recon_OnAgentEvent(AgentEvent ae)
        {
            //Loger.Log4Net.Info("[Main]recon_OnAgentEvent...PreAgentState..." + PreAgentState + ",PreAuxState is:" + PreAuxState);
            //Loger.Log4Net.Info("[Main]recon_OnAgentEvent...AgentState..." + ae.AgentState + ",AgentAuxState is:" + ae.AgentAuxState);
            //gAgentEvent = ae;
            if (ae.AgentState != 2 && ae.AgentState != 1)//登录退出状态手动插入，所以这里不处理。
            {
                EVTSaveAgentState2DB(ae);
            }

            RecomCOMEventStartTime = DateTime.Now;
            System.Console.WriteLine("AgentState:" + (enAgentState)ae.AgentState + "|AgentAuxState:" + ae.AgentAuxState);
            ShowAgentState(ae.AgentState, ae.AgentAuxState);

            KeyValueList kvl = new KeyValueList();
            int k = Program.rc.T_GetAttachedDataList(CallID, out kvl);//获取通话随路数据
            //string Reordreference = string.Empty;
            //string RecordIDURL = string.Empty;
            //if (Login.IsBindRecord)
            //{
            //    Reordreference = VCLogServiceHelper.Instance.GetRefID(Login.UserName); //获取指定分机录音流水号    
            //    RecordIDURL = VCLogServiceHelper.Instance.GetFileHttpPath(Reordreference);
            //}
            string strMsg = string.Format("UserEvent={0}&UserName={1}&CalledNum={2}&CallerNum={3}&CallID={4}&UserChoice={5}&AgentState={6}&AgentAuxState={7}&MediaType={8}&CurrentDate={9}",
                  ae.EventName, LoginUser.ExtensionNum, kvl.GetValue("sys_ANI"), ae.Party_Number, NewCallID.ToString(), kvl.GetValue("UserChoice"),
            (enAgentState)ae.AgentState, ae.AgentAuxState, (enMediaType)ae.MediaType, EscapeString(GetTime(ae.Timestamp).ToString("yyyy-MM-dd HH:mm:ss")));
            //Loger.Log4Net.Info(StrMsg);//添加客户端日志
            if (CurrentEventName != ae.EventName)
            {
                if (RecomCOMEventStartTime != null)
                {
                    TimeSpan ts = DateTime.Now - RecomCOMEventStartTime.Value;
                    strMsg += "&EventInterval=" + ts.TotalSeconds;
                    RecomCOMEventStartTime = DateTime.Now;
                }
                CurrentEventName = ae.EventName;
            }

            StrMsg.Add(strMsg);


            //Loger.Log4Net.Info("[recon_OnAgentEvent]EVTCTIEventMsg begin...");
            ////StrMsg.Add(strMsg);
            ////往站点推送消息
            //EVTCTIEventMsg(strMsg);
            //Loger.Log4Net.Info("[recon_OnAgentEvent]EVTCTIEventMsg bye...");

            string serviceMsg = string.Empty;//存储话务数据逻辑
            if (!WebService.CallRecordHelper.Instance.InsertCallRecord(ae, kvl, ref serviceMsg))
            {
                //MessageBox.Show(serviceMsg);
                Loger.Log4Net.Info("[recon_OnAgentEvent]WebService.CallRecordHelper.Instance.InsertCallRecord error is:" + serviceMsg);
            }
        }

        private void ShowAgentState(int agentState, int agentAuxState)
        {
            lblAgentStatusName.ForeColor = Color.Black;
            string AgentState = string.Empty;
            switch (agentState)
            {
                case (int)enAgentState.agentStateAfterCallWork: AgentState = "话后";
                    toolSbtnAgentMakeCall.Enabled = false;
                    toolSbtnAgentReleaseCall.Enabled = false;
                    toolSbtnAfterCallWork.Enabled = false;
                    toolSbtnSetBusy.Enabled = false;
                    toolSbtnAgentCheckOff.Enabled = false;
                    break;
                case (int)enAgentState.agentStateIntiated: AgentState = "状态初始化";
                    break;
                case (int)enAgentState.agentStateLoggedOff: AgentState = "签出";
                    break;
                case (int)enAgentState.agentStateLoggedOn: AgentState = "签入";
                    break;
                case (int)enAgentState.agentStateNotReady://置忙
                    if (BusyReasonDT != null)
                    {
                        BusyReasonDT.DefaultView.RowFilter = "Value=" + agentAuxState;
                        DataTable dt = BusyReasonDT.DefaultView.ToTable();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            AgentState = "置忙\r\n(" + dt.Rows[0]["Name"].ToString() + ")";
                        }
                        else
                        {
                            AgentState = "置忙\r\n(" + agentAuxState + ")";
                        }
                        toolSbtnSetBusy.Enabled = false;
                        //toolSbtnAgentReady.Enabled = true;
                        lblAgentStatusName.ForeColor = Color.Red;
                    }
                    break;
                case (int)enAgentState.agentStateReady: AgentState = "置闲";
                    toolSbtnSetBusy.Enabled = true;
                    toolSbtnAgentMakeCall.Enabled = true;
                    toolSbtnAgentReleaseCall.Enabled = true;
                    toolSbtnAgentCheckOff.Enabled = true;
                    toolSbtnAfterCallWork.Enabled = true;
                    break;
                case (int)enAgentState.agentStateRinging: AgentState = "振铃";
                    break;
                case (int)enAgentState.agentStateUnknown: AgentState = "未知";
                    break;
                case (int)enAgentState.agentStateWorking: AgentState = "工作中";
                    toolSbtnAgentCheckOff.Enabled = false;
                    toolSbtnAgentReady.Enabled = false;
                    toolSbtnSetBusy.Enabled = false;
                    toolSbtnAfterCallWork.Enabled = false;
                    toolSbtnAgentMakeCall.Enabled = false;
                    break;
                default: AgentState = "错误";
                    break;
            }
            //toolStripLabel2.Text = AgentState;
            lblAgentStatusName.Text = AgentState;
        }

        protected void Trace(string s)
        {
            System.Console.WriteLine(s);
            Program.rc.R_Trace(3, s);
        }

        public void recon_OnConnectionEvent(ConnectionEvent e)
        {
            Trace("recon_OnConnectionEvent: " + e.ComponentName +
                " (type: " + e.ComponentType +
                ") " + (e.Connected > 0 ? "is connected." : "is disconnected."));
            if (e.Connected <= 0)
            {
                PopReLoginLayer(string.Format("组件名称：{0}，组件类型：{1}，未连接！", e.ComponentName, e.ComponentType));
            }
        }


        public void recon_OnCTIEvent(CTIEvent e)
        {
            Loger.Log4Net.Info("[recon_OnCTIEvent]begin...");
            try
            {
                gCallType = e.CallType;
                string sEventType = "";
                sEventType = GetCTIEventType(e);

                string sCallType = "";
                sCallType = GetCTICallType(e);

                //Add=Masj,Date=2014-01-21 NewCallID初始化内容
                if ((sEventType == "eventInitiated" ||
                    sEventType == "eventRinging") && CallID != e.CallID)
                {
                    NewCallID = GetNewCallID(e.Timestamp);
                }

                if (sEventType == "eventInitiated")//如果是外呼 转满意度不可用
                {
                    this.toolSbtnIVRSatisfaction.Enabled = false;
                    Loger.Log4Net.Info("[recon_OnCTIEvent]eventInitiated...满意度不可用");
                }

                if (sEventType == "eventRinging")
                {
                    this.toolSbtnIVRSatisfaction.Enabled = true;
                    Loger.Log4Net.Info("[recon_OnCTIEvent]eventRinging...满意度可用");
                }

                Loger.Log4Net.Info("[recon_OnCTIEvent]eventType is:" + sEventType + ",callType is:" + sCallType + ",eventname is " + e.EventName + ",CallID is " + e.CallID.ToString() + ",NewCallID is " + NewCallID.ToString());
                toolStripStatuslblCTIEventName.Text = e.EventName;
                string SYS_DNIS = string.Empty;
                KeyValueList kvl = new KeyValueList();

                int k = Program.rc.T_GetAttachedDataList(e.CallID, out kvl);//获取通话随路数据
                if (kvl.Size() > 0)
                {
                    SYS_DNIS = kvl.GetValue("sys_DNIS");
                    lblSYSDNIS.Text = SYS_DNIS;
                }

                CallID = e.CallID;
                
                string strMsg = "";

                if (e.EventName == "Released" && (enCallType)e.CallType != enCallType.callTypeOutbound && (enCallType)e.CallType != enCallType.callTypeUnknown)//外呼电话 无振铃事件发生 为避免CallID=-2，固不应取eventRingingCallID
                {
                    strMsg = string.Format("UserEvent={0}&UserName={1}&CalledNum={2}&CallerNum={3}&CallID={4}&UserChoice={5}&CallType={6}&MediaType={7}&CallState={8}&CurrentDate={9}&SYS_DNIS={10}&EventType={11}&CallTypeID={12}",
                        e.EventName, LoginUser.ExtensionNum, e.PartyA_Number, e.PartyB_Number, eventRingingCallID, kvl.GetValue("UserChoice"),
                        (enCallType)e.CallType, (enMediaType)e.MediaType, (enCallState)e.State, EscapeString(GetTime(e.Timestamp).ToString("yyyy-MM-dd HH:mm:ss")),
                        SYS_DNIS, e.EventType, e.CallType);
                    Loger.Log4Net.Info("[recon_OnCTIEvent]eventname is release,strMsg is:" + strMsg);

                }
                else
                {
                    strMsg = string.Format("UserEvent={0}&UserName={1}&CalledNum={2}&CallerNum={3}&CallID={4}&UserChoice={5}&CallType={6}&MediaType={7}&CallState={8}&CurrentDate={9}&SYS_DNIS={10}&EventType={11}&CallTypeID={12}",
                        e.EventName, LoginUser.ExtensionNum, e.PartyA_Number, e.PartyB_Number, NewCallID.ToString(), kvl.GetValue("UserChoice"),
                        (enCallType)e.CallType, (enMediaType)e.MediaType, (enCallState)e.State, EscapeString(GetTime(e.Timestamp).ToString("yyyy-MM-dd HH:mm:ss")),
                        SYS_DNIS, e.EventType, e.CallType);
                }

                
                if (CurrentEventName != e.EventName)
                {
                    if (RecomCOMEventStartTime != null)
                    {
                        TimeSpan ts = DateTime.Now - RecomCOMEventStartTime.Value;
                        strMsg += "&EventInterval=" + ts.TotalSeconds;
                        RecomCOMEventStartTime = DateTime.Now;
                    }
                    CurrentEventName = e.EventName;
                }

                toolStripStatusLabelCallerNum.Text = e.PartyA_Number;
                toolStripStatusLabelCalledNum.Text = e.PartyB_Number;

                if (e.EventType == (int)enCTIEventType.eventInitiated)
                {
                    CallRecordLength = 0;
                    toolSbtnAgentCheckOff.Enabled = false;
                    toolSbtnAgentReady.Enabled = false;
                    toolSbtnSetBusy.Enabled = false;
                    toolSbtnAfterCallWork.Enabled = false;
                    toolSbtnAgentMakeCall.Enabled = false;

                    releaseCount = 0;//当呼出初始化时，将挂断次数置为0，表示新一轮通话，挂断时间要清空
                    releaseTime1 = null;
                    releaseTime2 = null;

                    eventInitiatedCallID = NewCallID;//Modify=Masj,Date=2014-01-21
                    //外呼转满意度 记录CallID到全局变量
                    if (e.CallType == (int)enCallType.callTypeOutbound)
                    {
                        eventRingingCallID = NewCallID;
                        Loger.Log4Net.Info("[recon_OnCTIEvent]eventInitiated外呼转满意度记录eventRingingCallID):" + eventRingingCallID);
                    }
                }
                if (e.EventType == (int)enCTIEventType.eventRinging)
                {
                    releaseCount = 0;//当呼入振铃时，将挂断次数置为0，表示新一轮通话，挂断时间要清空
                    releaseTime1 = null;
                    releaseTime2 = null;

                    eventRingingCallID = NewCallID;//Modify=Masj,Date=2014-01-21
                }
                if (e.EventType == (int)enCTIEventType.eventEstablished)
                {
                    RecomCOMEventEstablishedStartTime = GetTime(e.Timestamp);
                    IsEstablished = true;
                    IsCallRecordTimer = true;
                    toolSbtnAgentConsult.Enabled = true;
                    toolSbtnAgentReconnect.Enabled = true;
                    if (e.CallType == (int)enCallType.callTypeInbound)
                    {
                        tabControl1.Enabled = false;
                        toolSbtnAgentConsult.Enabled = true;
                    }
                    else if (e.CallType == (int)enCallType.callTypeOutbound)
                    {
                        toolSbtnAgentConsult.Enabled = false;
                        toolSbtnAgentReconnect.Enabled = false;
                    }
                }
                if (e.EventType == (int)enCTIEventType.eventTransferred)
                {
                    toolSbtnAgentConsult.Enabled = true;
                    toolSbtnAgentReconnect.Enabled = true;
                    if ((enCallState)e.State == enCallState.callStateTalking)
                    {
                        RecomCOMEventEstablishedStartTime = GetTime(e.Timestamp);
                        IsEstablished = true;
                    }
                }
                strMsg += "&IsEstablished=" + IsEstablished.ToString();//放在Released之前
                if (e.EventType == (int)enCTIEventType.eventReleased)
                {
                    eventReleasedCallID = NewCallID;//Modify=Masj,Date=2014-01-21

                    if (IsEstablished)//若接通时，赋值接通时间
                    {
                        strMsg += "&EstablishedStartTime=" + EscapeString(RecomCOMEventEstablishedStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        RecomCOMEventEstablishedStartTime = null;
                    }

                    IsCallRecordTimer = false;
                    tabControl1.Enabled = true;

                    toolSbtnAgentCheckOff.Enabled = true;
                    toolSbtnAgentReady.Enabled = true;
                    toolSbtnSetBusy.Enabled = true;
                    toolSbtnAfterCallWork.Enabled = true;
                    toolSbtnAgentMakeCall.Enabled = true;
                    toolSbtnAgentReleaseCall.Enabled = true;
                    toolSbtnAgentConsult.Enabled = false;
                    toolSbtnAgentReconnect.Enabled = false;
                    IsEstablished = false;

                    if (releaseCount == 0)
                    {
                        releaseTime1 = GetTime(e.Timestamp);
                        releaseCount++;
                        Loger.Log4Net.Info("[recon_OnCTIEvent]CallID is:" + e.CallID + ",releaseTime1 is:" + Convert.ToDateTime(releaseTime1).ToString("yyyy-MM-dd hh:mm:ss") + ",releaseCount is:" + releaseCount);
                    }
                    else if (releaseCount == 1)
                    {
                        releaseTime2 = GetTime(e.Timestamp);
                        releaseCount++;
                        Loger.Log4Net.Info("[recon_OnCTIEvent]CallID is:" + e.CallID + ",releaseTime2 is:" + Convert.ToDateTime(releaseTime2).ToString("yyyy-MM-dd hh:mm:ss") + ",releaseCount is:" + releaseCount);
                    }

                    if (outBoundType == 2)//客户端外呼
                    {
                        Loger.Log4Net.Info("[recon_OnCTIEvent]eventReleased客户端外呼操作，不调用浏览器js方法....");
                        strMsg = "";
                    }
                    outBoundType = 0;//挂断后呼出类型清空
                }

                #region 获取录音
                string Reordreference = string.Empty;
                string RecordIDURL = string.Empty;
                if (Login.IsBindRecord)
                {
                    Loger.Log4Net.Info("[Main]Recon_OnCTIEvent...VCLogServiceHelper...start...");
                    Reordreference = VCLogServiceHelper.Instance.GetRefID(LoginUser.ExtensionNum); //获取指定分机录音流水号    
                    RecordIDURL = VCLogServiceHelper.Instance.GetFileHttpPath(Reordreference);
                    Loger.Log4Net.Info("[Main]Recon_OnCTIEvent...VCLogServiceHelper...end...sessionid="+ Reordreference);
                }
                string para = string.Format("&RecordID={0}&RecordIDURL={1}&outBoundType={2}", Reordreference, RecordIDURL, outBoundType);
                #endregion

                if (outBoundType == 2 || strMsg == "")//客户端外呼
                {
                    Loger.Log4Net.Info("[recon_OnCTIEvent]客户端外呼操作，不调用浏览器js方法....");
                }
                else
                {
                    Loger.Log4Net.Info("[recon_OnCTIEvent]eventType is:" + sEventType + ",StrMsg.Add begin...");
                    strMsg += para;
                    StrMsg.Add(strMsg);
                    //往站点推送消息
                    //EVTCTIEventMsg(strMsg);
                    Loger.Log4Net.Info("[recon_OnCTIEvent]eventType is:" + sEventType + ",StrMsg.Add finish...");
                }


                Trace("recon_OnCTIEvent: " + e.EventName + " event");               
                Trace(String.Format(" PartyA_Number:{0}", e.PartyA_Number));
                Trace(String.Format(" PartyA_Descr:{0}", e.PartyA_Descr));
                Console.WriteLine("===================================");
                Trace(String.Format(" PartyB_Number:{0}", e.PartyB_Number));
                Trace(String.Format(" PartyB_Descr:{0}", e.PartyB_Descr));
                Console.WriteLine("===================================");
                Trace(String.Format(" Timestamp:{0}", e.Timestamp));                

                //其中UserChoice:Personal
                //sys_ANI:1000  呼入分机号码
                //sys_DNIS:2448 呼入测试号码
                //e.PartyB_Number 呼出号码
                if (kvl.Size() > 0)
                {
                    for (int i = 0; i < kvl.Size(); i++)
                    {
                        string skey = "";
                        kvl.GetPair(i, out skey);
                        Console.WriteLine("GetAttachedData:");
                        Trace(skey + ":" + kvl.GetValue(skey));
                        Console.WriteLine();
                    }
                }
                Trace((enErrorCode)k + "|  KeyValueListCount:" + kvl.Size());

                Loger.Log4Net.Info("[recon_OnCTIEvent]eventInitiatedCallID is:" + eventInitiatedCallID + ",eventRingingCallID) is:" + eventRingingCallID + ",eventReleasedCallID is:" + eventReleasedCallID);
                if (releaseTime1 != null && releaseTime2 != null)
                {
                    Loger.Log4Net.Info("[recon_OnCTIEvent]CallID is:" + e.CallID + ",releaseTime1 is:" + Convert.ToDateTime(releaseTime1).ToString("yyyy-MM-dd hh:mm:ss") + ",releaseTime2 is:" + Convert.ToDateTime(releaseTime2).ToString("yyyy-MM-dd hh:mm:ss") + ",releaseCount is:" + releaseCount);
                }

                string serviceMsg = string.Empty;//存储话务数据逻辑
                if (!WebService.CallRecordHelper.Instance.InsertCallRecord(e, kvl, ref serviceMsg, releaseTime1, releaseTime2, releaseCount, Reordreference,RecordIDURL))
                {
                    //屏蔽客户端回调函数出错时，弹出框“参数CallRecord_ORIG的Model为空”
                    //MessageBox.Show(serviceMsg);
                    Loger.Log4Net.Info("[recon_OnCTIEvent]WebService.CallRecordHelper.Instance.InsertCallRecord error is:" + serviceMsg);

                    if (serviceMsg.Contains("存储话务数据，授权失败") && e.EventName == "Released")
                    {
                        EVTSendErrorEmail(serviceMsg, "Main.cs", "[recon_OnCTIEvent]事件名称：" + e.EventName);
                    }
                }

            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[recon_OnCTIEvent]出错...callid:" + e.CallID + ",newcallid=" + NewCallID + ",errorMessage:" + ex.Message + ",errorStackTrace:" + ex.StackTrace);
            }
            Loger.Log4Net.Info("[recon_OnCTIEvent]bye...");
        }


        public string GetCTIEventType(CTIEvent e)
        {
            string retval = "";
            switch (e.EventType)
            {
                case (int)enCTIEventType.eventAbandoned:
                    retval = "eventAbandoned";
                    break;
                case (int)enCTIEventType.eventBackInService:
                    retval = "eventBackInService";
                    break;
                case (int)enCTIEventType.eventConferenced:
                    retval = "eventConferenced";
                    break;
                case (int)enCTIEventType.eventConsultation:
                    retval = "eventConsultation";
                    break;
                case (int)enCTIEventType.eventDiverted:
                    retval = "eventDiverted";
                    break;
                case (int)enCTIEventType.eventEstablished:
                    retval = "eventEstablished";
                    break;
                case (int)enCTIEventType.eventHeld:
                    retval = "eventHeld";
                    break;
                case (int)enCTIEventType.eventInformation:
                    retval = "eventInformation";
                    break;
                case (int)enCTIEventType.eventInitiated:
                    retval = "eventInitiated";
                    break;
                case (int)enCTIEventType.eventLinkDisconnected:
                    retval = "eventLinkDisconnected";
                    break;
                case (int)enCTIEventType.eventLinkReconnected:
                    retval = "eventLinkReconnected";
                    break;
                case (int)enCTIEventType.eventLinkStatus:
                    retval = "eventLinkStatus";
                    break;
                case (int)enCTIEventType.eventNetworkReached:
                    retval = "eventNetworkReached";
                    break;
                case (int)enCTIEventType.eventOutOfService:
                    retval = "eventOutOfService";
                    break;
                case (int)enCTIEventType.eventPartyInfo:
                    retval = "eventPartyInfo";
                    break;
                case (int)enCTIEventType.eventPredictiveInteractionRelease:
                    retval = "eventPredictiveInteractionRelease";
                    break;
                case (int)enCTIEventType.eventQueued:
                    retval = "eventQueued";
                    break;
                case (int)enCTIEventType.eventReleased:
                    retval = "eventReleased";
                    break;
                case (int)enCTIEventType.eventRetrieved:
                    retval = "eventRetrieved";
                    break;
                case (int)enCTIEventType.eventRingback:
                    retval = "eventRingback";
                    break;
                case (int)enCTIEventType.eventRinging:
                    retval = "eventRinging";
                    break;
                case (int)enCTIEventType.eventTransferred:
                    retval = "eventTransferred";
                    break;
                case (int)enCTIEventType.eventUserEvent:
                    retval = "eventUserEvent";
                    break;
                default:
                    break;
            }

            return retval;
        }

        public string GetCTICallType(CTIEvent e)
        {
            string retVal = "";

            switch (e.CallType)
            {
                case (int)enCallType.callTypeConference:
                    retVal = "callTypeConference";
                    break;
                case (int)enCallType.callTypeConsult:
                    retVal = "callTypeConsult";
                    break;
                case (int)enCallType.callTypeInbound:
                    retVal = "callTypeInbound";
                    break;
                case (int)enCallType.callTypeInternal:
                    retVal = "callTypeInternal";
                    break;
                case (int)enCallType.callTypeOutbound:
                    retVal = "callTypeOutbound";
                    break;
                case (int)enCallType.callTypePredictiveOutbound:
                    retVal = "callTypePredictiveOutbound";
                    break;
                case (int)enCallType.callTypeTransfer:
                    retVal = "callTypeTransfer";
                    break;
                case (int)enCallType.callTypeUnknown:
                    retVal = "callTypeUnknown";
                    break;
                default:
                    retVal = Convert.ToString(e.CallType);
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public DateTime GetTime(int timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp.ToString() + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        //private void Window_Error(object sender, HtmlElementErrorEventArgs e)
        //{
        //    e.Handled = true; // 阻止其他地方继续处理
        //}

        /// <summary>
        /// 电话接通后记录通话时长
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Elapsed(object sender, EventArgs e)
        {
            try
            {
                lblAllCallRecordLength.Text = Convert.ToString(int.Parse(lblAllCallRecordLength.Text) + 1);
                CallRecordLength++;
                lblCallRecordLength.Text = CallRecordLength.ToString();
                if (tabControl1.Enabled == false && CallRecordLength > 3)
                {
                    tabControl1.Enabled = true;
                }
            }
            catch
            {

            }

        }


        private void CTIEventMsg(string msg)
        {
            Loger.Log4Net.Info("[Main]CTIEventMsg...begin..." + msg);
            try
            {
                this.Invoke((EventHandler)delegate
                //if (msg != "")
                {
                    //timer1.Enabled = false;

                    //this.getCurrentBrowser().Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
                    //this.getCurrentBrowser().Document.InvokeScript("func", new string[] { StrMsg });
                    IHTMLWindow2 win;
                    if (string.IsNullOrEmpty(CurrentOutBoundTabPageName))
                    {
                        win = (IHTMLWindow2)this.getCurrentBrowser().Document.Window.DomWindow;
                    }
                    else
                    {
                        win = (IHTMLWindow2)this.getCurrentOutBoundBrowser(CurrentOutBoundTabPageName).Document.Window.DomWindow;
                    }
                    string Reordreference = string.Empty;
                    string RecordIDURL = string.Empty;
                    if (Login.IsBindRecord)
                    {
                        Loger.Log4Net.Info("[Main]CTIEventMsg...VCLogServiceHelper...start...");
                        Reordreference = VCLogServiceHelper.Instance.GetRefID(LoginUser.ExtensionNum); //获取指定分机录音流水号    
                        RecordIDURL = VCLogServiceHelper.Instance.GetFileHttpPath(Reordreference);
                        Loger.Log4Net.Info("[Main]CTIEventMsg...VCLogServiceHelper...end...");
                    }
                    string para = string.Format("&RecordID={0}&RecordIDURL={1}&outBoundType={2}", Reordreference, RecordIDURL, outBoundType);

                    //if (outBoundType == 2)//说明是客户端直接外呼，不调用JS脚本
                    //{
                    //    Loger.Log4Net.Info("[timer1_Elapsed]客户端外呼操作，不调用浏览器js方法....");
                    //}
                    //else
                    //{
                    msg += para;
                    win.execScript("try{MethodScript('" + msg + "');}catch(e){}", "Javascript");
                    //}
                    Loger.Log4Net.Info("[Main]CTIEventMsg...msg..." + msg);

                    //timer1.Enabled = true;
                    if ((!IsEstablished) && msg.StartsWith("UserEvent=Released"))
                    {
                        CurrentOutBoundTabPageName = string.Empty;
                    }
                });
                if (IsCallRecordTimer)
                {
                    lblCallRecordLength.Text = CallRecordLength.ToString();
                    timer2.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("客户端回调函数出错：" + ex.Message + "|" + ex.StackTrace);
            }
            Loger.Log4Net.Info("[Main]CTIEventMsg...bye..." + msg);
        }

        /// <summary>
        /// CTI事件响应时，调用浏览器js方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Elapsed(object sender, EventArgs e)
        {
            try
            {
                if (StrMsg != null && StrMsg.Count > 0)
                {
                    //timer1.Enabled = false;

                    //this.getCurrentBrowser().Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
                    //this.getCurrentBrowser().Document.InvokeScript("func", new string[] { StrMsg });
                    IHTMLWindow2 win;
                    if (string.IsNullOrEmpty(CurrentOutBoundTabPageName))
                    {
                        win = (IHTMLWindow2)this.getCurrentBrowser().Document.Window.DomWindow;
                    }
                    else
                    {
                        win = (IHTMLWindow2)this.getCurrentOutBoundBrowser(CurrentOutBoundTabPageName).Document.Window.DomWindow;
                    }
                    #region 改为在CTI事件中获取录音
                    //string Reordreference = string.Empty;
                    //string RecordIDURL = string.Empty;
                    //if (Login.IsBindRecord)
                    //{
                    //    Loger.Log4Net.Info("[Main]timer1_Elapsed...VCLogServiceHelper...start...");
                    //    Reordreference = VCLogServiceHelper.Instance.GetRefID(LoginUser.ExtensionNum); //获取指定分机录音流水号    
                    //    RecordIDURL = VCLogServiceHelper.Instance.GetFileHttpPath(Reordreference);
                    //    Loger.Log4Net.Info("[Main]timer1_Elapsed...VCLogServiceHelper...end...");
                    //}
                    //string para = string.Format("&RecordID={0}&RecordIDURL={1}&outBoundType={2}", Reordreference, RecordIDURL, outBoundType);


                    //win.execScript("try{MethodScript('" + StrMsg[0] + para + "');}catch(e){}", "Javascript");
                    #endregion
                    win.execScript("try{MethodScript('" + StrMsg[0] + "');}catch(e){}", "Javascript");
                    Loger.Log4Net.Info(StrMsg[0]);//添加客户端日志

                    if ((!IsEstablished) && StrMsg[0].ToString().StartsWith("UserEvent=Released"))
                    {
                        CurrentOutBoundTabPageName = string.Empty;
                        Loger.Log4Net.Info("[Main]设置 有电话正在通话 标志CurrentOutBoundTabPageName为空!");
                    }

                    #region 呼入弹屏逻辑
                    if (StrMsg[0].ToString().StartsWith("UserEvent=Transferred"))
                    {
                        string inboundUrl = System.Configuration.ConfigurationManager.AppSettings["defaultURL"] + "/CTI/PopTransfer.aspx?" + System.Web.HttpUtility.UrlDecode(StrMsg[0].ToString());
                        Loger.Log4Net.Info("[Main]弹屏URL：" + inboundUrl);
                        newPage(inboundUrl, true);
                    }
                    #endregion
                    StrMsg.RemoveAt(0);


                }
                if (IsCallRecordTimer)
                {
                    lblCallRecordLength.Text = CallRecordLength.ToString();
                    timer2.Enabled = true;
                }
                else
                {
                    timer2.Enabled = false;
                }
                GC.KeepAlive(timer1);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("客户端回调函数出错：" + ex.Message + "|" + ex.StackTrace);
            }
        }
        #endregion

        #region 调用前端JS方法
        private void SendCMD2JS(string msg)
        {
            Loger.Log4Net.Info("[Main]SendCMD2JS...begin..." + msg);
            IHTMLWindow2 win;
            if (string.IsNullOrEmpty(CurrentOutBoundTabPageName))
            {
                win = (IHTMLWindow2)this.getCurrentBrowser().Document.Window.DomWindow;
            }
            else
            {
                win = (IHTMLWindow2)this.getCurrentOutBoundBrowser(CurrentOutBoundTabPageName).Document.Window.DomWindow;
            }

            win.execScript("try{Response2CC('" + msg + "');}catch(e){}", "Javascript");
            Loger.Log4Net.Info("[Main]SendCMD2JS...bye..." + msg);
        }
        #endregion

        #region 提供给JavaScript调用的方法(外部公开事件)
        /// <summary>
        /// 提供给JavaScript调用的方法
        /// </summary>
        /// <param name="para">参数类似（/CallControl/MakeCall?TargetDN=XXX）</param>
        public string MethodScript(string urlPath)
        {
            if (urlPath == "colsewindow")
            {
                CarolHelper.Instance.AllClear(Program.rc, LoginUser.ExtensionNum);
                System.Environment.Exit(0);
            }
            Loger.Log4Net.Info("[Main]MethodScript...begin...");
            string msg = string.Empty;
            try
            {
                System.Uri url = new Uri("http://localhost" + urlPath);
                string[] absPath = url.AbsolutePath.Split('/');
                if (absPath.Length > 0)
                {
                    switch (absPath[1].ToLower())
                    {
                        case "callcontrol":
                            #region callcontrol-方法
                            switch (absPath[2].ToLower())
                            {
                                case "makecall":
                                    #region 调整分组前
                                    //string[] para = url.Query.Substring(1).Split('&');
                                    //if (para[0].ToLower().StartsWith("targetdn"))
                                    //{
                                    //    string tag = "9";//默认外显号码标记
                                    //    if (para.Length > 1 && para[1].ToLower().StartsWith("outshowtag"))//外显号码参数
                                    //    {
                                    //        tag = System.Web.HttpUtility.UrlDecode(para[1].Split('=')[1].Trim());
                                    //    }
                                    //    msg = MakeCall(para[0].Split('=')[1].Trim(), tag);
                                    //    CurrentOutBoundTabPageName = ((TabPage)getCurrentBrowser().Parent).Name;
                                    //}
                                    //else
                                    //{
                                    //    MessageBox.Show("呼叫参数错误！应为格式：/CallControl/MakeCall?TargetDN=XXX&OutShowTag=X");
                                    //}
                                    #endregion
                                    string[] para = url.Query.Substring(1).Split('&');
                                    if (para[0].ToLower().StartsWith("targetdn"))
                                    {
                                        string tag = "9";//默认外显号码标记
                                        if (!string.IsNullOrEmpty(LoginUser.OutNum))
                                        {
                                            tag = LoginUser.OutNum;
                                        }
                                        msg = MakeCall(para[0].Split('=')[1].Trim(), tag);
                                        CurrentOutBoundTabPageName = ((TabPage)getCurrentBrowser().Parent).Name;
                                    }
                                    else
                                    {
                                        MessageBox.Show("呼叫参数错误！应为格式：/CallControl/MakeCall?TargetDN=XXX&OutShowTag=X");
                                    }
                                    break;
                                case "releasecall":
                                    ReleaseCall();
                                    break;
                                default: break;
                            }
                            #endregion
                            break;
                        case "agent":
                            #region agent-方法
                            switch (absPath[2].ToLower())
                            {
                                case "username":
                                    msg = LoginUser.ExtensionNum;
                                    break;
                                case "userid":
                                    msg = LoginUser.EmployeeID == null ? "" : LoginUser.EmployeeID.Value.ToString();
                                    break;
                                default: break;
                            }
                            #endregion
                            break;
                        case "recordcontrol":
                            #region getrefid-方法
                            switch (absPath[2].ToLower())
                            {
                                case "getrefid"://获取指定分机录音流水号
                                    if (Login.IsBindRecord)
                                    {
                                        string reordreference = VCLogServiceHelper.Instance.GetRefID(LoginUser.ExtensionNum);
                                        msg = "{'Reordreference':'" + reordreference + "','RecordURL':'" + VCLogServiceHelper.Instance.GetFileHttpPath(reordreference) + "'}";
                                    }
                                    else
                                    {
                                        msg = "NotBindRecord";
                                    }
                                    break;
                                case "updaterecorddatabyext"://更新当前分机录音数据
                                    if (Login.IsBindRecord)
                                    {
                                        string[] para = url.Query.Substring(1).Split('&');
                                        if (para[0].ToLower().StartsWith("data"))
                                        {
                                            msg = VCLogServiceHelper.Instance.UpdateRecordDataByExt(LoginUser.ExtensionNum,
                                               System.Web.HttpUtility.UrlDecode(System.Web.HttpUtility.UrlDecode(para[0].Split('=')[1].Trim()))).ToString();
                                        }
                                        else
                                        {
                                            MessageBox.Show("更新当前分机录音数据参数错误！应为JSON格式");
                                        }
                                    }
                                    else
                                    {
                                        msg = "NotBindRecord";
                                    }
                                    break;
                                case "updaterecorddatabyid"://更新指定流水号录音的数据
                                    if (Login.IsBindRecord)
                                    {
                                        string[] para = url.Query.Substring(1).Split('&');
                                        if (para[0].ToLower().StartsWith("refid") && para[1].ToLower().StartsWith("data"))
                                        {
                                            msg = VCLogServiceHelper.Instance.UpdateRecordDataByID(para[0].Split('=')[1].Trim(), System.Web.HttpUtility.UrlDecode(para[1].Split('=')[1].Trim())).ToString();
                                        }
                                        else
                                        {
                                            MessageBox.Show("更新当前分机录音数据参数错误！应为refid=123&data=JSON格式");
                                        }
                                    }
                                    else
                                    {
                                        msg = "NotBindRecord";
                                    }
                                    break;
                                default: break;
                            }
                            #endregion
                            break;
                        case "browsercontrol":
                            #region 客户端浏览器接口
                            switch (absPath[2].ToLower())
                            {
                                case "newpage"://新建窗口，并导航到参数url地址
                                    string[] para = url.Query.Substring(1).Split('&');
                                    if (para[0].ToLower().StartsWith("url"))
                                    {
                                        if (para[0].IndexOf("=") >= 0)
                                        {
                                            string ss = para[0].Substring(para[0].IndexOf("=") + 1).Trim();
                                            newPage(System.Web.HttpUtility.UrlDecode(ss), false);
                                            msg = "ok";
                                        }
                                        else
                                        {
                                            msg = "failed";
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("新建窗口，必须有url这一参数");
                                        msg = "failed";
                                    }
                                    break;
                                case "newpageinbound"://呼入时，新建窗口，并导航到参数url地址
                                    string[] para2 = url.Query.Substring(1).Split('&');
                                    if (para2[0].ToLower().StartsWith("url"))
                                    {
                                        if (para2[0].IndexOf("=") >= 0)
                                        {
                                            string ss = para2[0].Substring(para2[0].IndexOf("=") + 1).Trim();
                                            newPage(System.Web.HttpUtility.UrlDecode(ss), true);
                                            msg = "ok";
                                        }
                                        else
                                        {
                                            msg = "failed";
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("新建窗口，必须有url这一参数");
                                        msg = "failed";
                                    }
                                    break;
                                case "closepage"://关闭当前窗口
                                    tabControl1_DoubleClick(null, null);
                                    msg = "ok";
                                    break;
                                case "closepagereloadppage"://关闭当前窗口，并刷新父窗口
                                    string ptabPageName = (((TabPage)getCurrentBrowser().Parent).Tag == null ? string.Empty : (string)((TabPage)getCurrentBrowser().Parent).Tag);
                                    tabControl1_DoubleClick(null, null);
                                    //getCurrentBrowser().Refresh();
                                    if (ptabPageName != string.Empty)
                                    {
                                        foreach (TabPage tp in tabControl1.TabPages)
                                        {
                                            if (//tp.Tag != null &&
                                                (!string.IsNullOrEmpty(ptabPageName)) &&
                                                ((string)tp.Name).Equals(ptabPageName))
                                            {
                                                ((WebBrowser)tp.Controls[0]).Refresh(WebBrowserRefreshOption.Completely);
                                                tabControl1.SelectedTab = tp;
                                                //getCurrentBrowser().Refresh();
                                                //((WebBrowser)tp.Controls[0]).Refresh(WebBrowserRefreshOption.Completely);
                                                break;
                                            }
                                        }
                                    }
                                    msg = "ok";
                                    break;
                                case "closepagecallparentpagefun"://关闭当前窗口，并调用父窗口中指定标签中的click方法
                                    string[] parentPageTagID = url.Query.Substring(1).Split('&');
                                    if (parentPageTagID[0].ToLower().StartsWith("parentpagecontrolid"))
                                    {
                                        string inputID = parentPageTagID[0].Substring(parentPageTagID[0].IndexOf("=") + 1).Trim();
                                        string ptabPageName2 = (((TabPage)getCurrentBrowser().Parent).Tag == null ? string.Empty : (string)((TabPage)getCurrentBrowser().Parent).Tag);
                                        tabControl1_DoubleClick(null, null);
                                        //getCurrentBrowser().Refresh();
                                        if (ptabPageName2 != string.Empty)
                                        {
                                            foreach (TabPage tp in tabControl1.TabPages)
                                            {
                                                if ((!string.IsNullOrEmpty(ptabPageName2)) &&
                                                    ((string)tp.Name).Equals(ptabPageName2))
                                                {
                                                    //((WebBrowser)tp.Controls[0]).Refresh(WebBrowserRefreshOption.Completely);
                                                    tabControl1.SelectedTab = tp;
                                                    IHTMLWindow2 win;
                                                    win = (IHTMLWindow2)this.getCurrentBrowser().Document.Window.DomWindow;
                                                    win.execScript("try{document.getElementById('" + inputID + "').click();}catch(e){}", "Javascript");
                                                    break;
                                                }
                                            }
                                        }
                                        msg = "ok";
                                    }
                                    else
                                    {
                                        MessageBox.Show("父窗口中指定标签ID，必须有这一参数");
                                        msg = "failed";
                                    }
                                    break;
                                default: break;
                            }
                            #endregion
                            break;
                        default: break;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                MessageBox.Show("调用方法出错：" + ex.Message);
            }
            return msg;
        }

        /// <summary>
        /// ASPNET实现javascript的unescape
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>返回解密后字符串</returns>
        private string UnEscapeString(string s)
        {
            if ((!string.IsNullOrEmpty(s)) && s.Length > 2)
            {
                string str = s.Remove(0, 2);//删除最前面两个＂%u＂
                string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//以子字符串＂%u＂分隔
                byte[] byteArr = new byte[strArr.Length * 2];
                for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
                {
                    byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16); //把十六进制形式的字串符串转换为二进制字节
                    byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
                }
                str = System.Text.Encoding.Unicode.GetString(byteArr);　//把字节转为unicode编码
                return str;
            }
            return string.Empty;
        }

        /// <summary>
        /// ASPNET实现javascript的escape 
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回加密后字符串</returns>
        private string EscapeString(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                StringBuilder sb = new StringBuilder();
                byte[] ba = Encoding.Unicode.GetBytes(str);
                for (int i = 0; i < ba.Length; i += 2)
                {
                    sb.Append("%u");
                    sb.Append(ba[i + 1].ToString("X2"));
                    sb.Append(ba[i].ToString("X2"));
                }
                return sb.ToString();
            }
            return string.Empty;
        }
        #endregion

        #region//菜单栏
        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowSaveAsDialog();
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPrintDialog();
        }
        /// <summary>
        /// 打印御览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printPreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPrintPreviewDialog();
        }
        /// <summary>
        /// 关闭浏览器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            CarolHelper.Instance.AllClear(Program.rc, LoginUser.ExtensionNum);
            Application.Exit();
        }
        /// <summary>
        /// 页面设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPageSetupDialog();
        }
        /// <summary>
        /// 属性设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPropertiesDialog();
        }
        #region//关于
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox myabout = new AboutBox();
            myabout.Show();
        }

        private void tipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("小提示：双击分页标题即可关闭当前页面。");
        }
        #endregion
        #endregion

        #region//工具栏
        /// <summary>
        /// 后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backButton_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().GoBack();
            setStatusButton();
        }
        /// <summary>
        /// 前进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forwordButton_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().GoForward();
            setStatusButton();
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopButton_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Stop();
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshButton_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Refresh();
        }
        /// <summary>
        /// 定向到主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void homeButton_Click(object sender, EventArgs e)
        {
            //getCurrentBrowser().GoHome();
            getCurrentBrowser().Navigate(DefaultURL);
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchButton_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().GoSearch();
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printButton_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Print();
        }
        /// <summary>
        /// 新建空白页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newButton_Click(object sender, EventArgs e)
        {
            newPage();
        }
        /// <summary>
        /// 使当前的浏览器定位到给定url
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gotoButton_Click(object sender, EventArgs e)
        {
            newCurrentPageUrl(tscbUrl.Text);
        }

        /// <summary>
        /// 帮助按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnHelp_Click(object sender, EventArgs e)
        {
            AboutBox myabout = new AboutBox();
            myabout.ShowDialog();
        }

        /// <summary>
        /// 签入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentCheckIn_Click(object sender, EventArgs e)
        {
            if (toolSbtnAgentCheckIn.Enabled)
            {
                if (CarolHelper.Instance.AgentLogOn(Program.rc, LoginUser.ExtensionNum))
                {
                    toolSbtnAgentCheckIn.Enabled = false;
                    toolSbtnAgentCheckOff.Enabled = true;

                    toolSbtnSetBusy.Enabled = true;
                    toolSbtnAgentReady.Enabled = true;
                    toolSbtnAfterCallWork.Enabled = true;
                    toolSbtnAgentMakeCall.Enabled = true;
                    toolSbtnAgentReleaseCall.Enabled = true;
                }
                else
                {
                    PopErrorLayer("坐席签入失败！");
                }
            }
        }

        /// <summary>
        /// 签出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentCheckOff_Click(object sender, EventArgs e)
        {
            if (toolSbtnAgentCheckOff.Enabled)
            {

                if (CarolHelper.Instance.AgentLogOff(Program.rc, LoginUser.ExtensionNum))
                {
                    toolSbtnAgentCheckIn.Enabled = true;
                    toolSbtnAgentCheckOff.Enabled = false;
                    toolSbtnAgentConsult.Enabled = false;
                    toolSbtnAgentReconnect.Enabled = false;
                    toolSbtnSetBusy.Enabled = false;
                    toolSbtnAgentReady.Enabled = false;
                    toolSbtnAfterCallWork.Enabled = false;
                    toolSbtnAgentMakeCall.Enabled = false;
                    toolSbtnAgentReleaseCall.Enabled = false;
                }
                else
                {
                    PopErrorLayer("坐席签入失败！");
                }
            }

        }


        private void PopReLoginLayer(string msg)
        {
            DisconnectedForm ef = new DisconnectedForm();
            ef.Msg = msg + "\r\n您确定要退出系统吗？";
            if (ef.ShowDialog(this) == DialogResult.OK)
            {
                timer1.Stop(); timer2.Stop();
                System.Environment.Exit(0);
                //Application.Run(new Login());
            }
            else
            {
                //MessageBox.Show("Cancel");
            }
            ef.Dispose();
        }

        private void PopErrorLayer(string msg)
        {
            ErrorForm ef = new ErrorForm();
            ef.Msg = msg + "\r\n您确定要退出系统吗？";
            if (ef.ShowDialog(this) == DialogResult.OK)
            {
                timer1.Stop(); timer2.Stop();
                CarolHelper.Instance.AllClear(Program.rc, LoginUser.ExtensionNum);
                System.Environment.Exit(0);
            }
            else
            {
                //MessageBox.Show("Cancel");
            }
            ef.Dispose();
        }

        /// <summary>
        /// 坐席置忙状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgentSetStateNotReady_Click(object sender, EventArgs e)
        {
            if (toolSbtnSetBusy.Enabled)
            {
                CarolHelper.Instance.AgentSetState(Program.rc, LoginUser.ExtensionNum, 4, int.Parse(((ToolStripItem)sender).Tag.ToString()));
            }
        }

        /// <summary>
        /// 置忙操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnSetBusy_ButtonClick(object sender, EventArgs e)
        {
            toolSbtnSetBusy.DropDown.Show(1, 2);
            //toolSbtnAgentReady.Enabled = true;
        }

        /// <summary>
        /// 坐席就绪状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgentSetStateReady_Click(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[Main]AgentSetStateReady_Click...begin...");
            //if (toolStripButton2.Enabled)
            {
                CarolHelper.Instance.AgentSetState(Program.rc, LoginUser.ExtensionNum, 3, 0);
                toolSbtnAfterCallWork.Enabled = true;//话后处理按钮
                toolSbtnAgentMakeCall.Enabled = true;//呼出按钮
                toolSbtnAgentReleaseCall.Enabled = true;//挂断按钮
                //toolSbtnAgentReady.Enabled = false;

                //如果从话后到置闲 ，则修改CallRecord_ORIG表的事后处理时长 add lxw 13.4.22
                //if (lblAgentStatusName.Text == "话后" && NewCallID > 0)
                //{
                //    try
                //    {
                //        bitauto.sys.ncc.CallRecord.CallRecordService crService = new bitauto.sys.ncc.CallRecord.CallRecordService();
                //        string msg = string.Empty;

                //        bitauto.sys.ncc.CallRecord.CallRecord_ORIG model = crService.GetCallRecord_ORIGByCallID(CallRecordAuthorizeCode, NewCallID, ref msg);
                //        if (model != null)
                //        {
                //            TimeSpan tsSpan = (TimeSpan)(DateTime.Now - model.AfterWorkBeginTime);
                //            model.AfterWorkTime = (int)tsSpan.TotalSeconds;//事后处理时长
                //            crService.InsertCallRecord(CallRecordAuthorizeCode, model, ref msg);
                //        }
                //    }
                //    catch (Exception ex)
                //    { }
                //}
            }
            Loger.Log4Net.Info("[Main]AgentSetStateReady_Click...bye...");
        }

        /// <summary>
        /// 坐席事后处理状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgentSetStateAfterCallWork_Click(object sender, EventArgs e)
        {
            //if (toolStripButton4.Enabled)
            {
                CarolHelper.Instance.AgentSetState(Program.rc, LoginUser.ExtensionNum, 5, 0);

                toolSbtnAfterCallWork.Enabled = false;//事后处理按钮
                //toolStripButton3.Enabled = true;//置忙按钮
                toolSbtnAgentReady.Enabled = true;//就绪按钮
            }
        }

        /// <summary>
        /// 外呼操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnAgentMakeCall_Click(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[Main]toolSbtnAgentMakeCall_Click begin...");
            KeyBoard k = new KeyBoard();
            k.ShowDialog();
            string phoneNum = k.ReturnNum;
            if (k.IsCallOut)
            {
                if (string.IsNullOrEmpty(phoneNum))
                {
                    MessageBox.Show("外呼号码不能为空！");
                }
                else
                {
                    MakeCall(phoneNum);
                }
            }
            Loger.Log4Net.Info("[Main]toolSbtnAgentMakeCall_Click bye...");
        }

        private void MakeCall(string mobile)
        {
            Loger.Log4Net.Info("[Main]MakeCall begin...");
            Loger.Log4Net.Info("[Main]MakeCall mobile..." + mobile);
            if (mobile != string.Empty && toolSbtnAfterCallWork.Enabled)
            {
                if (CarolHelper.Instance.MakeCall(Program.rc, LoginUser.ExtensionNum, mobile))
                {
                    outBoundType = 2;//客户端呼出
                    toolSbtnAgentMakeCall.Enabled = false;//呼出按钮
                    toolSbtnAfterCallWork.Enabled = false;//事后处理按钮
                }
                else
                {
                    MessageBox.Show("调用CarolSDK接口，外呼失败！");
                }
            }
            Loger.Log4Net.Info("[Main]MakeCall bye...");
        }

        /// <summary>
        /// 根据外显号码标记，呼出电话
        /// </summary>
        /// <param name="mobile">电话号码</param>
        /// <param name="outshowTag">外显号码标记</param>
        private string MakeCall(string mobile, string outshowTag)
        {
            string outNumber = ""; string errorMsg = "";
            bitauto.sys.ncc.VerifyPhoneFormat vpf = new bitauto.sys.ncc.VerifyPhoneFormat();
            vpf.VerifyFormat("E0F3C0C3-5317-4D5E-9548-7E31A506EC37", mobile, out outNumber, out errorMsg);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return errorMsg;
            }
            else
            {
                MakeCall(outshowTag + outNumber);
                outBoundType = 1;//网页呼出
                return string.Empty;
            }
        }

        /// <summary>
        /// 挂断电话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReleaseCall_Click(object sender, EventArgs e)
        {
            ReleaseCall();
        }

        private void ReleaseCall()
        {
            if (this.toolSbtnAgentReleaseCall.Enabled)
            {
                if (CarolHelper.Instance.ReleaseCall(Program.rc, LoginUser.ExtensionNum, CallID))
                {
                    toolSbtnAgentMakeCall.Enabled = true;//呼出按钮
                    toolSbtnAfterCallWork.Enabled = false;//事后处理按钮
                }
            }
        }


        /// <summary>
        /// 转接电话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsultCall_Click(object sender, EventArgs e)
        {
            SendCMD2JS("CMDTransfer");
            AgentList al = new AgentList();
            al.ShowDialog();
            string ConsultExtension = al.ConsultExtension;
            if (!string.IsNullOrEmpty(ConsultExtension))
            {
                if (CarolHelper.Instance.ConsultCall(Program.rc, LoginUser.ExtensionNum, ConsultExtension))
                {
                    toolSbtnAgentConsult.Enabled = false;
                    toolSbtnAgentReconnect.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 恢复电话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReconnectCall_Click(object sender, EventArgs e)
        {
            //if (tooltxtConsultCall.Text.Trim() != string.Empty)
            {
                if (CarolHelper.Instance.ReconnectCall(Program.rc, LoginUser.ExtensionNum, CallID))
                {
                    toolSbtnAgentConsult.Enabled = true;
                    toolSbtnAgentReconnect.Enabled = false;

                    //如果点击恢复 ，则修改CallRecord_ORIG表的转接恢复时间 add lxw 13.4.22
                    if (CallID > 0)
                    {
                        try
                        {
                            bitauto.sys.ncc.CallRecord.CallRecordService crService = new bitauto.sys.ncc.CallRecord.CallRecordService();
                            string msg = string.Empty;

                            bitauto.sys.ncc.CallRecord.CallRecord_ORIG model = crService.GetCallRecord_ORIGByCallID(CallRecordAuthorizeCode, NewCallID, ref msg);
                            if (model != null)
                            {
                                model.ReconnectCall = DateTime.Now;
                                crService.InsertCallRecord(CallRecordAuthorizeCode, model, ref msg);
                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
        }
        #endregion

        #region 临时浏览器事件
        /// <summary>
        /// 临时浏览器状态变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tempBrowser_StatusTextChanged(object sender, EventArgs e)
        {
            WebBrowser myBrowser = (WebBrowser)sender;
            TabPage mypage = (TabPage)myBrowser.Parent;
            //设置临时浏览器所在tab标题
            mypage.Text = newstring(myBrowser.DocumentTitle);
            mypage.ToolTipText = mypage.Text;
            if (myBrowser != getCurrentBrowser())
            {
                return;
            }
            else
            {
                toolStripStatusLabel1.Text = myBrowser.StatusText;
            }
        }
        /// <summary>
        /// 在当前页面上重新定向
        /// </summary>
        /// <param name="address">url</param>
        private void newCurrentPageUrl(String address)
        {
            getCurrentBrowser().Navigate(getUrl(address));
        }
        /// <summary>
        /// 临时浏览器产生新窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tempBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            //获取触发tempBrowser_NewWindow事件的浏览器
            WebBrowser myBrowser = (WebBrowser)sender;
            //获取触发tempBrowser_NewWindow事件的浏览器所在TabPage
            TabPage mypage = (TabPage)myBrowser.Parent;
            //通过StatusText属性获得新的url
            string NewURL = ((WebBrowser)sender).StatusText;

            if (NewURL == "完成")
            {
                //myBrowser.Document.ActiveElement.InvokeMember("Click");

                //string html = myBrowser.Document.ActiveElement.OuterHtml.ToLower().Trim();
                //string pattern = @"onclick=.*\('(.*)'\).*";
                //MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase);

                IHTMLWindow2 win = (IHTMLWindow2)myBrowser.Document.Window.DomWindow;
                win.execScript("try{ document.getElementById('" + myBrowser.Document.ActiveElement.Id + "').click();}catch(e){}", "Javascript");
            }
            else
            {
                //string html = myBrowser.Document.ActiveElement.OuterHtml.ToLower().Trim();
                //string pattern = @"onclick=.*\('(.*)'\).*";
                //MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase);
                //if (matches.Count == 1)
                //{
                //    Match m = matches[0];
                //    Group g = m.Groups[1];
                //    if (g != null && g.Length > 0)
                //    {
                //        //string address = myBrowser.Url.Scheme + "://" + myBrowser.Url.Host + ":" + myBrowser.Url.Port + g.ToString();
                //        string address = myBrowser.Url.Scheme + "://" + myBrowser.Url.Host + g.ToString();
                //        NewURL = address.Replace("&amp;", "&");
                //    }
                //}
                //NewURL = string.IsNullOrEmpty(NewURL) ? myBrowser.StatusText : NewURL;

                //生成新的一页
                TabPage TabPageTemp = new TabPage();
                TabPageTemp.Name = Guid.NewGuid().ToString();
                TabPageTemp.Tag = ((TabPage)getCurrentBrowser().Parent).Name;//存储父页面Name
                //生成新的tempBrowser
                WebBrowser tempBrowser = new WebBrowser();
                //临时浏览器定向到新的url
                tempBrowser.Navigate(NewURL);
                tempBrowser.Dock = DockStyle.Fill;
                //为临时浏览器关联NewWindow等事件
                tempBrowser.NewWindow += new CancelEventHandler(tempBrowser_NewWindow);
                tempBrowser.Navigated += new WebBrowserNavigatedEventHandler(tempBrowser_Navigated);
                tempBrowser.ProgressChanged += new WebBrowserProgressChangedEventHandler(tempBrowser_ProgressChanged);
                tempBrowser.StatusTextChanged += new EventHandler(tempBrowser_StatusTextChanged);
                tempBrowser.Dock = DockStyle.Fill;
                tempBrowser.ObjectForScripting = this;
                tempBrowser.ScriptErrorsSuppressed = IsScriptErrorsSuppressed;

                //将临时浏览器添加到临时TabPage中
                TabPageTemp.Controls.Add(tempBrowser);
                //将临时TabPage添加到主窗体中
                this.tabControl1.TabPages.Add(TabPageTemp);
                tabControl1.SelectedTab = TabPageTemp;
                //使外部无法捕获此事件
                e.Cancel = true;
            }
        }
        /// <summary>
        /// 临时浏览器定向完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            tscbUrl.Text = getCurrentBrowser().Url.ToString();
            WebBrowser mybrowser = (WebBrowser)sender;
            TabPage mypage = (TabPage)mybrowser.Parent;
            //设置临时浏览器所在tab标题
            mypage.Text = newstring(mybrowser.DocumentTitle);
            mypage.ToolTipText = mypage.Text;
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 当在浏览器地址栏敲“回车”时当前浏览器重定向到指定url（tscbUrl.Tex）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                newCurrentPageUrl(tscbUrl.Text);
            }
        }
        /// <summary>
        /// 新建空白页
        /// </summary>
        private void newPage()
        {
            tscbUrl.Text = "about:blank";
            TabPage mypage = new TabPage();
            WebBrowser tempBrowser = new WebBrowser();
            tempBrowser.Navigated += new WebBrowserNavigatedEventHandler(tempBrowser_Navigated);
            tempBrowser.NewWindow += new CancelEventHandler(tempBrowser_NewWindow);

            tempBrowser.ProgressChanged += new WebBrowserProgressChangedEventHandler(tempBrowser_ProgressChanged);
            tempBrowser.StatusTextChanged += new EventHandler(tempBrowser_StatusTextChanged);
            tempBrowser.Dock = DockStyle.Fill;
            tempBrowser.ObjectForScripting = this;
            tempBrowser.Navigate(DefaultURL);
            tempBrowser.ScriptErrorsSuppressed = IsScriptErrorsSuppressed;
            mypage.Controls.Add(tempBrowser);

            tabControl1.TabPages.Add(mypage);
            tabControl1.SelectedTab = mypage;
        }
        /// <summary>
        /// 临时浏览器进度变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tempBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Maximum = (int)e.MaximumProgress;
            toolStripProgressBar1.Value = (int)e.CurrentProgress;
        }
        /// <summary>
        /// 新建一页并定向到指定url
        /// </summary>
        /// <param name="address">新一页的浏览器重新定向到的url</param>
        /// <param name="isCalling">是否通话中</param>
        private void newPage(string address, bool isCalling)
        {
            Loger.Log4Net.Info("[Main]newPage...begin...address is:" + address);
            TabPage mypage = new TabPage();
            mypage.Name = Guid.NewGuid().ToString();
            if (isCalling)
            {
                CurrentOutBoundTabPageName = mypage.Name;
            }
            mypage.Tag = ((TabPage)getCurrentBrowser().Parent).Name;//存储父页面Name
            WebBrowser tempBrowser = new WebBrowser();
            tempBrowser.Navigated += new WebBrowserNavigatedEventHandler(tempBrowser_Navigated);
            tempBrowser.NewWindow += new CancelEventHandler(tempBrowser_NewWindow);
            tempBrowser.StatusTextChanged += new EventHandler(tempBrowser_StatusTextChanged);
            tempBrowser.ProgressChanged += new WebBrowserProgressChangedEventHandler(tempBrowser_ProgressChanged);
            if (address.StartsWith("http://"))
            {
                tempBrowser.Url = getUrl(address);
            }
            else
            {
                Uri u = new Uri(DefaultURL);
                tempBrowser.Url = getUrl("http://" + u.Host + address);
            }
            tempBrowser.Dock = DockStyle.Fill;
            tempBrowser.ObjectForScripting = this;
            tempBrowser.ScriptErrorsSuppressed = IsScriptErrorsSuppressed;
            mypage.Controls.Add(tempBrowser);
            tabControl1.TabPages.Add(mypage);
            tabControl1.SelectedTab = mypage;

            Loger.Log4Net.Info("[Main]newPage...bye...address is:" + address);
        }
        /// <summary>
        /// 获取当前浏览器
        /// </summary>
        /// <returns>当前浏览器</returns>
        private WebBrowser getCurrentBrowser()
        {
            //initMainForm();
            //if (tabControl1.TabPages.Count == 0)
            //{
            //    initMainForm();
            //}
            WebBrowser currentBrowser = (WebBrowser)tabControl1.SelectedTab.Controls[0];
            return currentBrowser;
        }

        /// <summary>
        /// 获取当前呼出电话的浏览器
        /// </summary>
        /// <returns>当前浏览器</returns>
        private WebBrowser getCurrentOutBoundBrowser(string tabPageName)
        {
            TabPage outBoundTP = null;
            foreach (TabPage tp in tabControl1.TabPages)
            {
                if (tp.Name.Equals(tabPageName))
                {
                    outBoundTP = tp; break;
                }
            }
            WebBrowser currentBrowser = (WebBrowser)outBoundTP.Controls[0];
            return currentBrowser;
        }
        /// <summary>
        /// 处理字符串为合法url
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private Uri getUrl(string address)
        {
            string tempaddress = address;
            if ((!address.StartsWith("http://")) && (!address.StartsWith("https://")) && (!address.StartsWith("ftp://")))
            {
                tempaddress = "http://" + address;
            }
            Uri myurl;
            try
            {
                myurl = new Uri(tempaddress);
            }
            catch
            {
                myurl = new Uri("about:blank");
            }
            return myurl;
        }
        /// <summary>
        /// 截取字符串为指定长度
        /// </summary>
        /// <param name="oldstring"></param>
        /// <returns></returns>
        private string newstring(string oldstring)
        {
            string temp;
            if (oldstring.Length < TITLE_COUNT)
            {
                temp = oldstring;
            }
            else
            {
                temp = oldstring.Substring(0, TITLE_COUNT);
            }
            return temp + "   ";
        }
        /// <summary>
        /// 设置＂前进＂，＂后退＂button的可用状态
        /// </summary>
        private void setStatusButton()
        {
            backButton.Enabled = getCurrentBrowser().CanGoBack;
            forwordButton.Enabled = getCurrentBrowser().CanGoForward;
        }
        #endregion

        #region tabControl1事件
        /// <summary>
        /// 切换tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            WebBrowser mybor = (WebBrowser)tabControl1.SelectedTab.Controls[0];
            if (mybor.Url != null)
            {
                //地址输入框
                tscbUrl.Text = mybor.Url.ToString();
                tabControl1.SelectedTab.Text = newstring(mybor.DocumentTitle);
            }
            else
            {
                tscbUrl.Text = "about:blank";
                tabControl1.SelectedTab.Text = "空白页";
            }
            setStatusButton();
        }
        /// <summary>
        /// 关闭当前tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_DoubleClick(object sender, EventArgs e)
        {
            //仅仅剩下一个tab时返回
            if (tabControl1.TabPages.Count <= 1)
            {
                //tabControl1.SelectedTab.Text = "空白页";
                //getCurrentBrowser().Navigate("about:blank");
                getCurrentBrowser().Navigate(DefaultURL);
            }
            else
            {
                //先将tabControl1隐藏然后remove掉目标tab（如果不隐藏则出现闪烁，即系统自动调转到tabControl1的第一个tab然后跳会。）最后显示tabControl1。
                //tabControl1.Visible = false;
                WebBrowser mybor = getCurrentBrowser();
                //释放资源
                mybor.Dispose();
                mybor.Controls.Clear();
                TabPage page = this.tabControl1.SelectedTab;
                this.tabControl1.TabPages.Remove(page);
                page.Dispose();
                //重新设置当前tab
                //tabControl1.SelectedTab = tabControl1.TabPages[tabControl1.TabPages.Count - 1];
                //tabControl1.SelectedTab = tabControl1.TabPages[tabControl1.SelectedIndex];
                if (tabControl1.TabPages.Count == 0)
                {
                    tabControl1.Visible = false;
                }
                else
                {
                    tabControl1.Visible = true;
                }
            }
        }

        private void MainTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                Rectangle myTabRect = this.tabControl1.GetTabRect(e.Index);

                //先添加TabPage属性      
                e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text
                , this.Font, SystemBrushes.ControlText, myTabRect.X + 2, myTabRect.Y + 2);

                //再画一个矩形框   
                using (Pen p = new Pen(Color.Transparent))
                {
                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;
                    e.Graphics.DrawRectangle(p, myTabRect);

                }

                //填充矩形框   
                Color recColor = e.State == DrawItemState.Selected ? Color.Transparent : Color.Transparent;
                using (Brush b = new SolidBrush(recColor))
                {
                    e.Graphics.FillRectangle(b, myTabRect);
                }

                //画关闭符号
                using (Pen objpen = new Pen(Color.Gray))
                {
                    ////"\"线
                    //Point p1 = new Point(myTabRect.X + 3, myTabRect.Y + 3);
                    //Point p2 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + myTabRect.Height - 3);
                    //e.Graphics.DrawLine(objpen, p1, p2);

                    ////"/"线
                    //Point p3 = new Point(myTabRect.X + 3, myTabRect.Y + myTabRect.Height - 3);
                    //Point p4 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + 3);
                    //e.Graphics.DrawLine(objpen, p3, p4);

                    //使用图片  

                    Bitmap bt = new Bitmap(image);

                    Point p5 = new Point(myTabRect.X, 4);

                    e.Graphics.DrawImage(bt, p5);


                }
                //绘制小图标                 
                //==============================================================================   
                //Bitmap bt = new Bitmap("E://1//2.jpg");   
                //Point p5 = new Point(4, 4);   
                ////e.Graphics.DrawImage(bt, e.Bounds);   
                //e.Graphics.DrawImage(bt, p5);   
                //Pen pt = new Pen(Color.Red);   
                ////e.Graphics.DrawString(this.MainTabControl.TabPages[e.Index].Text, this.Font, pt.Brush, e.Bounds);   
                //e.Graphics.DrawString(this.MainTabControl.TabPages[e.Index].Text, this.Font, pt.Brush, p5);   

                e.Graphics.Dispose();
            }
            catch
            {

            }
        }

        private void MainTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!IsCallingTel())
                {
                    int x = e.X, y = e.Y;

                    //计算关闭区域      
                    Rectangle myTabRect = this.tabControl1.GetTabRect(this.tabControl1.SelectedIndex);

                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;

                    //如果鼠标在区域内就关闭选项卡      
                    bool isClose = x > myTabRect.X && x < myTabRect.Right
                     && y > myTabRect.Y && y < myTabRect.Bottom;

                    if (isClose == true)
                    {
                        //仅仅剩下一个tab时返回
                        if (tabControl1.TabPages.Count <= 1)
                        {
                            //tabControl1.SelectedTab.Text = "空白页";
                            //getCurrentBrowser().Navigate("about:blank");
                            getCurrentBrowser().Navigate(DefaultURL);
                        }
                        else
                        {
                            this.tabControl1.TabPages.Remove(this.tabControl1.SelectedTab);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断是否有电话正在通话
        /// </summary>
        /// <returns>有，返回True，否决返回False</returns>
        private bool IsCallingTel()
        {
            Loger.Log4Net.Info("[Main]IsCallingTel判断是否有电话正在通话SelectedTab.Name:" + tabControl1.SelectedTab.Name);
            if (tabControl1.SelectedTab.Name.Equals(CurrentOutBoundTabPageName))
            {
                MessageBox.Show("当前窗口有电话正在通话，请勿关闭！！"); return true;
            }
            return false;
        }
        #endregion

        #region 其他事件
        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SysConfig newSoft = new SysConfig();
            newSoft.ShowDialog();
            this.skinEngine1.SkinFile = "skin/" + Common.GetSkinFileName();
        }

        /// <summary>
        /// 设置皮肤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSbtnChangeSkin_Click(object sender, EventArgs e)
        {
            SysConfig newSoft = new SysConfig();
            newSoft.ShowDialog();
            this.skinEngine1.SkinFile = "skin/" + Common.GetSkinFileName();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AgentList al = new AgentList();
            al.ShowDialog();

            string ConsultExtension = al.ConsultExtension;
            if (!string.IsNullOrEmpty(ConsultExtension))
            {
                if (CarolHelper.Instance.ConsultCall(Program.rc, LoginUser.ExtensionNum, ConsultExtension))
                {
                    toolSbtnAgentConsult.Enabled = false;
                    toolSbtnAgentReconnect.Enabled = true;
                }
            }
        }
        #endregion

        private void toolSbtnIVRSatisfaction_Click(object sender, EventArgs e)
        {
            //Loger.Log4Net.Info(StrMsg[0] + para);//添加客户端日志
            Loger.Log4Net.Info("[IVRSatisfaction_Click]Main.CallID is " + CallID.ToString() + ",Main.NewCallID is " + NewCallID.ToString() + ",Program.CallID is " + Program.CallID.ToString());
            int initCallID = -2;
            initCallID = CallID;
            //CarolHelper.Instance.AttachDataPair(Program.rc, CallID, "CallRecordID", "10086");
            if (CarolHelper.Instance.ConsultCall(Program.rc, LoginUser.ExtensionNum, "2442"))
            {
                //设置随路数据，传递CallID，录音ID到IVR
                //两种添加方法：T_AttachDataPair，T_AttachDataList

                //KeyValueList kvl = new KeyValueList();
                //Program.CallID
                Loger.Log4Net.Info("[IVRSatisfaction_Click]ConsultCall initCallID is " + initCallID.ToString() + ",CallRecordID is " + eventRingingCallID.ToString());
                CarolHelper.Instance.AttachDataPair(Program.rc, initCallID, "CallRecordID", eventRingingCallID.ToString());

                if (CarolHelper.Instance.TransferCall(Program.rc, LoginUser.ExtensionNum))
                {
                    //CarolHelper.Instance.AttachDataPair(Program.rc, "CallRecordID", "10086");
                    //toolSbtnIVRSatisfaction.Enabled = false;

                    Loger.Log4Net.Info("[IVRSatisfaction_Click]TransferCall initCallID is " + initCallID.ToString() + ",CallRecordID is " + eventRingingCallID.ToString());
                    CarolHelper.Instance.AgentSetState(Program.rc, LoginUser.ExtensionNum, 5, 0);
                }
                else
                {
                    MessageBox.Show("调用CarolSDK接口，转满意度转接失败！");
                    Loger.Log4Net.Info("[IVRSatisfaction_Click]TransferCall 失败 initCallID is " + initCallID.ToString() + ",CallRecordID is " + eventRingingCallID.ToString());
                }
            }
            else
            {
                MessageBox.Show("调用CarolSDK接口，转满意度磋商失败！");
                Loger.Log4Net.Info("[IVRSatisfaction_Click]ConsultCall 失败 initCallID is " + initCallID.ToString() + ",CallRecordID is " + eventRingingCallID.ToString());
            }
        }

        private void timer3_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //try
            //{
            //    //if(gAgentEvent != null)
            //    //SaveAgentState2DB(gAgentEvent);
            //    //gTimetick += Convert.ToInt32(timer3.Interval / 1000);
            //    gTimetick++;
            //    SqlTool tool = new SqlTool();
            //    tool.UpdateAgentState2DB(gTimetick);
            //}
            //catch (Exception err)
            //{

            //    Loger.Log4Net.Info("[Main]timer3_Elapsed SaveAgentState2DB err is:" + err.Message);
            //    Loger.Log4Net.Info("[Main]timer3_Elapsed SaveAgentState2DB err is:" + err.Source);
            //    Loger.Log4Net.Info("[Main]timer3_Elapsed SaveAgentState2DB err is:" + err.StackTrace);
            //}

            try
            {
                SqlTool tool = new SqlTool();
                //记录坐席退出状态时间
                DateTime tdate = tool.GetCurrentTime();
                if (LoginUser.LoginOnOid != null)
                    tool.UpdateStateDetail2DB(Convert.ToInt32(LoginUser.LoginOnOid), tdate);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[Main]timer3_Elapsed errorMessage:" + ex.Message + ",StackTrace:" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 生成新CallID
        /// </summary>
        /// <param name="timestamp">CTI事件时间戳</param>
        /// <returns>返回新CallID（10位）</returns>
        //private int GetNewCallID(int timestamp)
        //{
        //    Loger.Log4Net.Error("GetNewCallID:生成新CallID时 timestamp：" + timestamp);
        //    Random r = new Random();
        //    DateTime dt = new DateTime();
        //    string extNum = LoginUser.ExtensionNum;//分机号码
        //    if (String.IsNullOrEmpty(extNum))
        //    {
        //        extNum = r.Next(1000, 10000).ToString();
        //    }
        //    try
        //    {
        //        dt = GetTime(timestamp);
        //    }
        //    catch (Exception ex)
        //    {
        //        Loger.Log4Net.Error("生成新CallID时，出错，timestamp格式不正确", ex);
        //        dt = DateTime.Now;
        //    }
        //    return Convert.ToInt32(extNum + dt.ToString("hmmss"));
        //}

        /// <summary>
        /// 生成新CallID
        /// </summary>
        /// <param name="timestamp">CTI事件时间戳</param>
        /// <returns>返回新CallID（10位）</returns>
        private Int64 GetNewCallID(int timestamp)
        {
            Loger.Log4Net.Error("GetNewCallID:生成新CallID时 timestamp：" + timestamp);
            Random r = new Random();
            Int64 callid = 0;
            string scallid = "";
            string extNum = LoginUser.ExtensionNum;//分机号码
            if (String.IsNullOrEmpty(extNum))
            {
                extNum = r.Next(1000, 10000).ToString();
            }
            try
            {
                scallid = extNum + timestamp.ToString() + r.Next(100, 1000).ToString();
                callid = Convert.ToInt64(scallid);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("生成新CallID时，出错:", ex);
            }
            return callid;
        }
    }
}
