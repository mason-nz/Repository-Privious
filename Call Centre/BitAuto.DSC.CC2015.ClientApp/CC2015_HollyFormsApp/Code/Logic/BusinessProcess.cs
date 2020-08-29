using System;
using CC2015_HollyFormsApp.CCWeb.CallRecordService;
using System.Configuration;

namespace CC2015_HollyFormsApp
{
    public class BusinessProcess
    {
        public readonly static BusinessProcess Instance = new BusinessProcess();

        /// 话务备份实体类（来自Main.gmodel ）
        /// <summary>
        /// 话务备份实体类（来自Main.gmodel ）
        /// </summary>
        public static CallRecord_ORIG CallRecordORIG = null;
        /// 点击拨号时，是客户端还是网页；页面-1；客户端-2；
        /// <summary>
        /// 点击拨号时，是客户端还是网页；页面-1；客户端-2；
        /// </summary>
        public static OutBoundTypeEnum OutBoundType = OutBoundTypeEnum.OT0_未知;
        /// 页面ID
        /// <summary>
        /// 页面ID
        /// </summary>
        private static int WebPageID = 1;

        private BusinessProcess()
        {
        }

        /// 获取发送msg类
        /// <summary>
        /// 获取发送msg类
        /// </summary>
        /// <param name="userevent"></param>
        /// <param name="callid"></param>
        /// <param name="calltype"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public UserEventData InitUserEventData(UserEvent userevent, Calltype calltype, DateTime date, long? callid = null, string taskid = null, int? tasktype = null)
        {
            UserEventData ud = new UserEventData();
            ud.UserEvent = userevent;
            ud.CallType = calltype;
            ud.CurrentDate = date;
            //如果callid有值
            if (callid.HasValue == true)
            {
                ud.CallID = callid.Value;
            }
            //主叫
            if (CallRecordORIG == null || string.IsNullOrEmpty(CallRecordORIG.PhoneNum))
            {
                ud.ZhujiaoNum = HollyContactHelper.Instance.GetZhujiaoPhone();
            }
            else
            {
                ud.ZhujiaoNum = CallRecordORIG.PhoneNum;//主叫
            }
            //被叫
            if (CallRecordORIG == null || string.IsNullOrEmpty(CallRecordORIG.ANI))
            {
                ud.BejiaoNum = HollyContactHelper.Instance.GetBeijiaoPhone();
            }
            else
            {
                ud.BejiaoNum = CallRecordORIG.ANI;//被叫
            }

            ud.UserChoice = HollyContactHelper.Instance.GetSkillGroup();//技能组           
            ud.SYS_DNIS = HollyContactHelper.Instance.GetLuodiNum();//落地号码    
            ud.RecordID = HollyContactHelper.Instance.GetHollyCallID();//厂家callid

            //取值
            if (CallRecordORIG != null)
            {
                ud.IsEstablished = CallRecordORIG.EstablishedTime.HasValue;
                ud.EstablishedStartTime = CallRecordORIG.EstablishedTime;
                ud.RecordIDURL = CallRecordORIG.AudioURL;//录音地址
                //callid 没有值，从CallRecordORIG中取
                if (callid.HasValue == false)
                {
                    ud.CallID = CallRecordORIG.CallID.HasValue ? CallRecordORIG.CallID.Value : -1;
                }
            }

            ud.TaskID = taskid;
            ud.TaskType = tasktype;

            Loger.Log4Net.Info("[==BusinessProcess=] InitUserEventData " + userevent + "-" + ud.CallID + "-" + calltype + "-" + date);
            return ud;
        }
        /// 获取话务类
        /// <summary>
        /// 获取话务类
        /// </summary>
        /// <param name="callid"></param>
        /// <param name="calltype"></param>
        /// <returns></returns>
        public void InitCallRecordORIG(long callid, Calltype calltype)
        {
            CallRecordORIG = new CallRecord_ORIG();
            CallRecordORIG.CallID = callid;
            CallRecordORIG.SiemensCallID = -1;
            CallRecordORIG.ExtensionNum = LoginUser.ExtensionNum;
            //设置主叫和被叫
            CallRecordORIG.CallStatus = (int)calltype;
            SetPhoneNumAndAni();
            CallRecordORIG.OutBoundType = (int)OutBoundType;
            CallRecordORIG.AfterWorkTime = 0;
            CallRecordORIG.TallTime = 0;
            CallRecordORIG.CreateTime = Common.GetCurrentTime();
            CallRecordORIG.CreateUserID = LoginUser.UserID;
            //设置厂家id
            CallRecordORIG.SessionID = HollyContactHelper.Instance.GetHollyCallID();//厂家callid
            CallRecordORIG.GenesysCallID = HollyContactHelper.Instance.GetHollyCallID();//厂家callid
            Loger.Log4Net.Info("[==BusinessProcess=] InitCallRecordORIG callid：" + callid + "-calltype：" + calltype);
        }

        /// 设置初始化时间
        /// <summary>
        /// 设置初始化时间
        /// </summary>
        /// <param name="date"></param>
        public void SetInitiatedTime(DateTime date)
        {
            //初始化时间，呼出存在，在点击按钮时
            CallRecordORIG.InitiatedTime = date;
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> InitiatedTime >>>> " + date);
        }
        /// 设置振铃时间
        /// <summary>
        /// 设置振铃时间
        /// </summary>
        /// <param name="date"></param>
        public void SetRingingTime(DateTime date)
        {
            //设置振铃时间
            CallRecordORIG.RingingTime = date;
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> RingingTime >>>> " + date);
        }
        /// 设置接通时间
        /// <summary>
        /// 设置接通时间
        /// </summary>
        /// <param name="date"></param>
        public void SetEstablishedTime(DateTime date)
        {
            //设置接通时间
            CallRecordORIG.EstablishedTime = date;
            //设置主叫被叫
            SetPhoneNumAndAni();
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> EstablishedTime >>>> " + date + "-hollycallid：" + CallRecordORIG.SessionID);
        }
        /// 设置主叫和被叫 （是否覆盖）
        /// <summary>
        /// 设置主叫和被叫（是否覆盖）
        /// </summary>
        private void SetPhoneNumAndAni(bool isover = true)
        {
            //呼入：主叫-客户(长)  被叫-客服(短)
            //呼出：主叫-客服(短)  被叫-客户(长)

            //         phone	ani
            //呼出	分机号	手机号
            //呼入	手机号	分机号            
            //         主叫	被叫

            string a = HollyContactHelper.Instance.GetZhujiaoPhone();
            string b = HollyContactHelper.Instance.GetBeijiaoPhone();

            if ((isover == false && string.IsNullOrEmpty(CallRecordORIG.PhoneNum)) || isover)
            {
                //主叫
                if (CallRecordORIG.CallStatus == (int)Calltype.C2_呼出 || OutBoundType == OutBoundTypeEnum.OT4_自动外呼)
                {
                    //呼出
                    CallRecordORIG.PhoneNum = Common.ChooseString(a, b, Choose.分机);
                }
                else
                {
                    //呼入
                    CallRecordORIG.PhoneNum = Common.ChooseString(a, b, Choose.手机号);
                }
            }
            if ((isover == false && string.IsNullOrEmpty(CallRecordORIG.ANI)) || isover)
            {
                if (CallRecordORIG.CallStatus == (int)Calltype.C2_呼出 || OutBoundType == OutBoundTypeEnum.OT4_自动外呼)
                {
                    //呼出
                    CallRecordORIG.ANI = Common.ChooseString(a, b, Choose.手机号);
                }
                else
                {
                    //呼入
                    CallRecordORIG.ANI = Common.ChooseString(a, b, Choose.分机);
                }
            }
            //落地号码
            CallRecordORIG.SwitchINNum = HollyContactHelper.Instance.GetLuodiNum();
            //技能组
            CallRecordORIG.SkillGroup = HollyContactHelper.Instance.GetSkillGroup();
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> PhoneNumAndAni >>>> " + CallRecordORIG.PhoneNum + "->" + CallRecordORIG.ANI);
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> SwitchINNum >>>> " + CallRecordORIG.SwitchINNum);
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> SkillGroup >>>> " + CallRecordORIG.SkillGroup);
        }
        /// 设置挂断时间
        /// <summary>
        /// 设置挂断时间
        /// </summary>
        /// <param name="date"></param>
        /// <param name="releasetype"></param>
        /// <param name="AudioURL"></param>
        public void SetReleaseTime(DateTime date, ReleaseType releasetype)
        {
            //设置厂家id
            CallRecordORIG.SessionID = HollyContactHelper.Instance.GetHollyCallID();//厂家callid
            CallRecordORIG.GenesysCallID = HollyContactHelper.Instance.GetHollyCallID();//厂家callid
            //设置主叫和被叫 (已存在数据则不覆盖)
            SetPhoneNumAndAni(false);
            //设置挂断时间
            if (releasetype == ReleaseType.客服挂断)
            {
                CallRecordORIG.AgentReleaseTime = date;
                Loger.Log4Net.Info("[==BusinessProcess=] >>>> AgentReleaseTime >>>> " + date);
            }
            else if (releasetype == ReleaseType.用户挂断)
            {
                CallRecordORIG.CustomerReleaseTime = date;
                Loger.Log4Net.Info("[==BusinessProcess=] >>>> CustomerReleaseTime >>>> " + date);
            }
            //设置话后开始时间
            CallRecordORIG.AfterWorkBeginTime = date;
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> AfterWorkBeginTime >>>> " + date);
            //获取录音地址
            GetURL(date);
            //计算通话时长
            CalcTotalTime(date);
            //检查是否转接出去的电话
            //存在转接开始时间，没有转接取消时间
            if (CallRecordORIG.ConsultTime.HasValue == true
                && CallRecordORIG.ReconnectCall.HasValue == false
                && CallRecordORIG.TransferOutTime.HasValue == false)
            {
                SetTransferOutTime(date);
            }
        }
        /// 设置话后持续秒数
        /// <summary>
        /// 设置话后持续秒数
        /// </summary>
        /// <param name="date"></param>
        public void SetAfterWorkTime(DateTime date)
        {
            if (CallRecordORIG != null && CallRecordORIG.AfterWorkBeginTime.HasValue)
            {
                //设置话后持续秒数
                double a = (date - CallRecordORIG.AfterWorkBeginTime.Value).TotalSeconds;
                CallRecordORIG.AfterWorkTime = (int)a;
                Loger.Log4Net.Info("[==BusinessProcess=] >>>> AfterWorkTime >>>> " + date + "-" + a + "秒");
            }
        }
        /// 设置转接开始时间
        /// <summary>
        /// 设置转接开始时间
        /// </summary>
        /// <param name="date"></param>
        public void SetConsultTime(DateTime date)
        {
            if (CallRecordORIG == null) return;

            //设置转接开始时间
            CallRecordORIG.ConsultTime = date;
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> ConsultTime >>>> " + date);
        }
        /// 设置转接取消时间
        /// <summary>
        /// 设置转接取消时间
        /// </summary>
        /// <param name="date"></param>
        public void SetReconnectCall(DateTime date)
        {
            if (CallRecordORIG == null) return;

            //设置转接取消时间
            CallRecordORIG.ReconnectCall = date;
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> ReconnectCall >>>> " + date);
        }
        /// 设置转入时间
        /// <summary>
        /// 设置转入时间
        /// </summary>
        /// <param name="date"></param>
        public void SetTransferInTime(DateTime date)
        {
            if (CallRecordORIG == null) return;

            //设置转入时间
            CallRecordORIG.TransferInTime = date;
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> TransferInTime >>>> " + date);
        }
        /// 设置转出时间
        /// <summary>
        /// 设置转出时间
        /// </summary>
        /// <param name="date"></param>
        public void SetTransferOutTime(DateTime date)
        {
            if (CallRecordORIG == null) return;

            //设置转出时间
            CallRecordORIG.TransferOutTime = date;
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> TransferOutTime >>>> " + date);
        }
        /// 获取录音地址
        /// <summary>
        /// 获取录音地址
        /// </summary>
        /// <param name="date"></param>
        private void GetURL(DateTime date)
        {
            //获取录音地址
            string RecordURl = BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToString(BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("RecordURl", false));
            string path = HollyContactHelper.Instance.GetRecordFileName();
            string url = "";
            if (path != "")
            {
                url = RecordURl.TrimEnd('/') + path;
            }
            //设置录音地址
            CallRecordORIG.AudioURL = url;
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> AudioURL >>>> " + url);
        }
        /// 计算通话时长
        /// <summary>
        /// 计算通话时长
        /// </summary>
        /// <param name="date"></param>
        private static void CalcTotalTime(DateTime date)
        {
            //计时通话时长
            double a = 0;
            //如果有接通时间 呼入和转接 从接通时开始计时
            if (CallRecordORIG.EstablishedTime.HasValue)
            {
                //设置录音时长（挂断时间-接通时间）
                a = (date - CallRecordORIG.EstablishedTime.Value).TotalSeconds;
            }
            CallRecordORIG.TallTime = (int)a;
            Loger.Log4Net.Info("[==BusinessProcess=] >>>> TallTime >>>> " + a);
        }
        /// 更新话后信息
        /// <summary>
        /// 更新话后信息
        /// </summary>
        public void UpdateCallRecordAfterTime()
        {
            //初始化话务数据
            DateTime date = Common.GetCurrentTime();
            //更新退出话后时间
            SetAfterWorkTime(date);
            //更新话后数据 (只更新，不插入)
            bool a = CallRecordHelper.Instance.InsertCallRecordNew(true);
            //数据还原
            CallRecordORIG = null;
            Loger.Log4Net.Info("[==BusinessProcess=][UpdateCallRecordAfterTime] 更新话后信息 " + a);
        }

        public static readonly object locked = new object();
        /// 获取WebPageID
        /// <summary>
        /// 获取WebPageID
        /// </summary>
        /// <returns></returns>
        public static string GetWebPageID()
        {
            lock (locked)
            {
                string webpageid = "WebPage" + WebPageID.ToString("00000");
                WebPageID++;
                return webpageid;
            }
        }
    }
}
