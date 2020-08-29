using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RECONCOMLibrary;

namespace CC2012_CarolFormsApp.WebService
{
    public class CallRecordHelper
    {
        #region Instance
        public static readonly CallRecordHelper Instance = new CallRecordHelper();
        bitauto.sys.ncc.CallRecord.CallRecordService crService;
        private string CallRecordAuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["CallRecordAuthorizeCode"];//调用话务记录接口授权码
        Main main = new Main();
        #endregion

        #region Contructor
        protected CallRecordHelper()
        {
            crService = new bitauto.sys.ncc.CallRecord.CallRecordService();
        }
        #endregion


        /// <summary>
        /// 插入或更新话务数据
        /// </summary>
        /// <param name="e">CTIEvent</param>
        /// <param name="kvl">话务随路数据List</param>
        /// <param name="msg">输出信息</param>
        /// <returns></returns>
        public bool InsertCallRecord(CTIEvent e, KeyValueList kvl, ref string msg, DateTime? releaseTime1, DateTime? releaseTime2, int releaseCount,string sessionid,string audiourl)
        {
            try
            {
                Loger.Log4Net.Info("[CallRecordHelper_InsertCallRecord_CTIEvent]CallID is:" + e.CallID + ",NewCallID is :" + Main.NewCallID + ",sessionid:" + sessionid + ",audiourl:"+ audiourl);                            
                bitauto.sys.ncc.CallRecord.CallRecord_ORIG model = crService.GetCallRecord_ORIGByCallID(CallRecordAuthorizeCode, Main.NewCallID, ref msg);

                string numberB = e.PartyB_Number;
                string numberA = e.PartyA_Number;

                switch (e.EventType)
                {
                    //状态：初始化
                    case (int)enCTIEventType.eventInitiated:
                        //磋商转接情况，不生成新记录
                        //if (e.CallType == (int)enCallType.callTypeConsult)
                        //{
                        //    Loger.Log4Net.Info("[recon_OnCTIEvent]InsertCallRecord callType is callTypeConsult,not insert callRecordORIG record...return...");
                        //    return true;
                        //}
                        //else
                        //{
                        model = createNewCallRecord_ORIG(e, kvl, Main.NewCallID);
                        //}
                        break;
                    //状态：呼出-振铃
                    case (int)enCTIEventType.eventNetworkReached:
                        //电话状态：呼出-2；呼入-1    除了状态为33、34是呼入的其余都是呼出
                        if (e.CallType != 33 && e.CallType != 34 && model != null)
                        {
                            model.RingingTime = main.GetTime(e.Timestamp);
                            model.ANI = numberB;//呼出号（被叫）
                            model.SessionID = sessionid;//VCLogServiceHelper.Instance.GetRefID(LoginUser.ExtensionNum);//流水号
                            model.AudioURL = audiourl;//VCLogServiceHelper.Instance.GetFileHttpPath(model.SessionID);//录音地址
                        }
                        break;
                    //状态：呼入-振铃
                    case (int)enCTIEventType.eventRinging:
                        //电话状态：呼出-2；呼入-1    除了状态为33、34是呼入的其余都是呼出,9为磋商呼入振铃
                        if (e.CallType == 33 || e.CallType == 34 || e.CallType == 9)//呼入
                        {
                            model = createNewCallRecord_ORIG(e, kvl, Main.NewCallID);
                            if (e.CallType == 9)
                            {
                                model.TransferInTime = main.GetTime(e.Timestamp);
                                Loger.Log4Net.Info("[CallRecordHelper]转接开始 转入时间:" + model.TransferInTime);
                            }
                        }
                        break;
                    //状态：接通
                    case (int)enCTIEventType.eventEstablished:
                        if (model != null)
                        {
                            model.EstablishedTime = main.GetTime(e.Timestamp);
                        }
                        break;
                    case (int)enCTIEventType.eventRingback://只在中间转接时触发该事件
                        if (model != null)
                        {
                            model.OutBoundType = 3;//转接
                            model.ANI = numberB;//记录转接的分机号
                            model.RingingTime = main.GetTime(e.Timestamp);//记录中间转到下一个分机号的振铃时间
                        }
                        break;
                    //状态：挂断
                    case (int)enCTIEventType.eventReleased:
                        if (model != null && model.OutBoundType == 3)//说明是磋商转接电话
                        {
                            Loger.Log4Net.Info("[CallRecordHelper_InsertCallRecord_CTIEvent]Consult Call CallID is:" + e.CallID + ",NewCallID is :" + Main.NewCallID + ",磋商转接话务保存开始。");
                            bitauto.sys.ncc.CallRecord.CallRecord_ORIG model2 = crService.GetCallRecord_ORIGByCallID(CallRecordAuthorizeCode, Main.eventRingingCallID, ref msg);
                            if (model.RingingTime.HasValue)
                            {
                                model.EstablishedTime = model.RingingTime;
                                TimeSpan ts = (TimeSpan)(main.GetTime(e.Timestamp) - model.EstablishedTime);
                                model.TallTime = (int)ts.TotalSeconds;
                            }
                            model.AgentReleaseTime = main.GetTime(e.Timestamp);
                            model.SessionID = sessionid;//VCLogServiceHelper.Instance.GetRefID(LoginUser.ExtensionNum);//流水号
                            model.AudioURL = audiourl;//VCLogServiceHelper.Instance.GetFileHttpPath(model.SessionID);//录音地址
                            model.PhoneNum = model2.PhoneNum;
                            model.ExtensionNum = model2.ExtensionNum;
                            model.CallStatus = 1;
                            crService.InsertCallRecord(CallRecordAuthorizeCode, model, ref msg);
                            Loger.Log4Net.Info("[CallRecordHelper_InsertCallRecord_CTIEvent]CallID is:" + e.CallID + ",NewCallID is :" + Main.NewCallID + ",磋商转接话务保存完毕。");
                            model = crService.GetCallRecord_ORIGByCallID(CallRecordAuthorizeCode, Main.eventRingingCallID, ref msg);
                            if (!string.IsNullOrEmpty(msg))
                            {
                                Loger.Log4Net.Info("[CallRecordHelper_InsertCallRecord_CTIEvent]磋商转接话务保存失败，error is:" + msg);
                            }
                        }

                        if (model != null)
                        {
                            Loger.Log4Net.Info("[CallRecordHelper_InsertCallRecord_CTIEvent]normal Call CallID is:" + e.CallID + ",NewCallID is :" + Main.NewCallID + ",eventRingingCallID is:" + Main.eventRingingCallID);                            
                            Loger.Log4Net.Info("[CallRecordHelper_InsertCallRecord_CTIEvent]normal Call SessionID is:" + model.SessionID);                            


                            if (releaseCount == 2)
                            {
                                model.AgentReleaseTime = releaseTime2;
                                model.CustomerReleaseTime = releaseTime1;
                            }
                            else if (releaseCount == 1)
                            {
                                model.SessionID = sessionid;//VCLogServiceHelper.Instance.GetRefID(LoginUser.ExtensionNum);//流水号
                                model.AudioURL = audiourl;//VCLogServiceHelper.Instance.GetFileHttpPath(model.SessionID);//录音地址

                                model.AgentReleaseTime = releaseTime1;
                                model.CustomerReleaseTime = null;

                                if (model.EstablishedTime.HasValue)
                                {
                                    TimeSpan ts = (TimeSpan)(main.GetTime(e.Timestamp) - model.EstablishedTime);
                                    model.TallTime = (int)ts.TotalSeconds;//录音总时间
                                }
                            }

                            Loger.Log4Net.Info("[CallRecordHelper_InsertCallRecord_CTIEvent]normal Call CallID is:" + e.CallID + ",NewCallID is :" + Main.NewCallID + ",releaseCount is:" + releaseCount + ",main.GetTime is:" + main.GetTime(e.Timestamp).ToString("yyyy-MM-dd hh:mm:ss"));
                            if (model.EstablishedTime != null)
                            {
                                Loger.Log4Net.Info("[CallRecordHelper_InsertCallRecord_CTIEvent]normal Call CallID is:" + e.CallID + ",NewCallID is :" + Main.NewCallID + ",EstablishedTime is:" + Convert.ToDateTime(model.EstablishedTime).ToString("yyyy-MM-dd hh:mm:ss"));
                            }
                            if (model.AgentReleaseTime.HasValue)
                                Loger.Log4Net.Info("[CallRecordHelper_InsertCallRecord_CTIEvent]normal Call CallID is:" + e.CallID + ",NewCallID is :" + Main.NewCallID + ",AgentReleaseTime is:" + Convert.ToDateTime(model.AgentReleaseTime).ToString("yyyy-MM-dd hh:mm:ss"));
                        }
                        break;
                    //状态：转接开始
                    case (int)enCTIEventType.eventConsultation:
                        if (model != null)
                        {
                            model.EstablishedTime = main.GetTime(e.Timestamp);
                            model.ConsultTime = main.GetTime(e.Timestamp);
                            model.TransferOutTime = main.GetTime(e.Timestamp);
                            Loger.Log4Net.Info("[CallRecordHelper]转接开始 转出时间:" + model.TransferOutTime);
                        }
                        break;
                    //状态：转接恢复
                    case (int)enCTIEventType.eventLinkReconnected:
                        if (model != null)
                        {
                            //model.ReconnectCall = main.GetTime(e.Timestamp);
                        }
                        break;
                }
                Loger.Log4Net.Info(string.Format("调用插入客户端话务接口——事件名称：{0},CallID:{1},NewCallID:{4},ANI:{2},PhoneNum:{3}", e.EventName, e.CallID, numberA, numberB, Main.NewCallID));//添加客户端日志
                if (model != null)
                {
                    Loger.Log4Net.Info("[CallRecordHelper]model is not null,and CallID is:" + model.CallID);
                }
                else
                {
                    Loger.Log4Net.Info("[CallRecordHelper]model.CallID is null...return...");
                    return true;
                }
                return crService.InsertCallRecord(CallRecordAuthorizeCode, model, ref msg);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("调用插入客户端话务接口出错：", ex);
                msg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 插入或更新话务数据
        /// </summary>
        /// <param name="ae">AgentEvent</param>
        /// <param name="kvl">话务随路数据List</param>
        /// <param name="msg">输出信息</param>
        /// <returns></returns>
        public bool InsertCallRecord(AgentEvent ae, KeyValueList kvl, ref string msg)
        {
            bitauto.sys.ncc.CallRecord.CallRecord_ORIG model = crService.GetCallRecord_ORIGByCallID(CallRecordAuthorizeCode, Main.NewCallID, ref msg);

            switch (ae.AgentState)
            {
                //
                case (int)enAgentState.agentStateIntiated:
                    break;
                case (int)enAgentState.agentStateRinging:
                    break;
                case (int)enAgentState.agentStateWorking:
                    break;


                //置为 话后（事后处理开始）
                case (int)enAgentState.agentStateAfterCallWork:
                    //电话状态：呼出-2；呼入-1  只有呼入状态下才会记录事后处理时间和事后处理时长
                    if (model != null)
                    {
                        if (model.CallStatus == 1)
                        {
                            model.AfterWorkBeginTime = main.GetTime(ae.Timestamp);
                        }
                    }
                    break;
                //置为 置闲（事后处理结束）
                case (int)enAgentState.agentStateReady:
                    //电话状态：呼出-2；呼入-1  只有呼入状态下才会记录事后处理时间和事后处理时长
                    if (model != null)
                    {
                        if (model.CallStatus == 1)
                        {
                            TimeSpan tsSpan = (TimeSpan)(main.GetTime(ae.Timestamp) - model.AfterWorkBeginTime);
                            model.AfterWorkTime = (int)tsSpan.TotalSeconds;//事后处理时长
                        }
                    }
                    break;
            }
            return crService.InsertCallRecord(CallRecordAuthorizeCode, model, ref msg);
        }

        //type=1  表示从状态为振铃时（呼入）进入，此时记振铃时间；type=2 表示从状态为初始化时（呼出）进入，记初始化时间
        private bitauto.sys.ncc.CallRecord.CallRecord_ORIG createNewCallRecord_ORIG(CTIEvent e, KeyValueList kvl, Int64 newCallID)
        {
            bitauto.sys.ncc.CallRecord.CallRecord_ORIG model = new bitauto.sys.ncc.CallRecord.CallRecord_ORIG();
            try
            {
                Loger.Log4Net.Info("[createNewCallRecord_ORIG]eventname is " + e.EventName + ",CallID is " + e.CallID.ToString() + ",NewCallID is :" + newCallID);
                model.CallID = newCallID;
                model.SiemensCallID = e.CallID;
                if (e.CallType == 9)
                {
                    //转接电话，分机号取发起转接的分机号
                    //用西门子CallID之前不用赋值
                    //自己生成CallID后需赋值
                    model.ExtensionNum = LoginUser.ExtensionNum;
                }
                else
                {
                    model.ExtensionNum = LoginUser.ExtensionNum;
                }
                //电话状态：呼出-2；呼入-1    除了状态为33、34的是呼入外其余都是呼出
                string numberB = e.PartyB_Number;
                string numberA = e.PartyA_Number;

                //呼出类型：页面呼出-1，客户端呼出-2，转接-3
                model.OutBoundType = Main.outBoundType;
                model.SessionID = string.Empty;
                model.SwitchINNum = string.Empty;
                model.AfterWorkTime = 0;
                model.TallTime = 0;
                model.AudioURL = string.Empty;

                if (e.CallType == 33 || e.CallType == 34)
                {
                    model.CallStatus = 1;
                    model.RingingTime = main.GetTime(e.Timestamp);
                    model.SwitchINNum = kvl.GetValue("sys_DNIS");
                    model.PhoneNum = numberB;//主叫
                    model.ANI = numberA;//被叫
                    model.SkillGroup = kvl.GetValue("UserChoice");//技能组
                }
                else if (e.CallType == 9)//9为磋商转接呼入电话
                {
                    //主叫PhoneNum：取发起转接坐席接入的外线电话。
                    //在使用西门子CallID时，转接电话时，发起转接当时生成的CallID跟接收转接CallID相同
                    //
                    //主叫号，不用重新赋值
                    //自己生成CallID后，则转接过来电话会生成自己CallID
                    //需给主号、被叫赋值
                    model.CallStatus = 1;
                    model.PhoneNum = numberB;//主叫
                    model.ANI = numberA;//被叫
                }
                else
                {
                    model.CallStatus = 2;
                    model.InitiatedTime = main.GetTime(e.Timestamp);
                    model.PhoneNum = numberA;//主叫
                }
                model.CreateTime = main.GetTime(e.Timestamp);
                model.CreateUserID = LoginUser.UserID;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("新增客户端话务数据出错：", ex);
                Loger.Log4Net.Error("错误Source：" + ex.Source);
                Loger.Log4Net.Error("错误StackTrace：" + ex.StackTrace);
            }
            return model;
        }

        private string getPhone(string phone)
        {
            string phoneStr = phone;

            //如果电话号码为8位，表示北京本地号码，加010区号
            if (phone.Length == 8)
            {
                phoneStr = "010" + phone;
            }
            //如果电话号码为12位，电话开头为 01 、91 则去掉第一个数
            if (phone.Length == 12 && (phone.Substring(0, 2) == "01" || phone.Substring(0, 2) == "91"))
            {
                phoneStr = phone.Substring(1, phone.Length - 1);
            }

            return phoneStr;
        }
    }
}
