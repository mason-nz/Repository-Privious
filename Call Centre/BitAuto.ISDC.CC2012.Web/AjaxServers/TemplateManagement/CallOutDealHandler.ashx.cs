using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.WebService;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    /// 老版本工单使用 强斐 2016-8-19
    /// <summary>
    /// 老版本工单使用 强斐 2016-8-19
    /// </summary>
    public class CallOutDealHandler : IHttpHandler, IRequiresSessionState
    {

        #region Query Properties
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        /// <summary>
        /// 操作类型
        /// </summary>
        public string Action { get { return (Request["Action"] + "").Trim(); } }

        //private string newcustid;
        //public string NewCustID
        //{
        //    get
        //    {
        //        if (newcustid == null)
        //        {
        //            newcustid = HttpUtility.UrlDecode((Request["UserChoice"] + "").Trim());
        //        }
        //        return newcustid;
        //    }
        //}

        private string userChoice;
        /// <summary>
        /// 电话呼入来源
        /// </summary>
        public string UserChoice
        {
            get
            {
                if (userChoice == null)
                {
                    userChoice = HttpUtility.UrlDecode((Request["UserChoice"] + "").Trim());
                }
                return userChoice;
            }
        }

        private string callID;
        /// <summary>
        /// CTI中通话ID
        /// </summary>
        public string CallID
        {
            get
            {
                if (callID == null)
                {
                    callID = HttpUtility.UrlDecode((Request["CallID"] + "").Trim());
                }
                return callID;
            }
        }

        private string recordID;
        /// <summary>
        /// 通话流水号
        /// </summary>
        public string RecordID
        {
            get
            {
                if (recordID == null)
                {
                    recordID = HttpUtility.UrlDecode((Request["RecordID"] + "").Trim());
                }
                return recordID;
            }
        }

        private string recordIDURL;
        /// <summary>
        /// 通话URL地址
        /// </summary>
        public string RecordIDURL
        {
            get
            {
                if (recordIDURL == null)
                {
                    recordIDURL = HttpUtility.UrlDecode((Request["RecordIDURL"] + "").Trim());
                }
                return recordIDURL;
            }
        }

        private string agentState;
        /// <summary>
        /// 坐席当前状态（枚举名称:就绪3、置忙4、事后处理5）
        /// </summary>
        public string AgentState
        {
            get
            {
                if (agentState == null)
                {
                    agentState = HttpUtility.UrlDecode((Request["AgentState"] + "").Trim());
                }
                return agentState;
            }
        }

        private string agentAuxState;
        /// <summary>
        /// 坐席置忙状态原因（int类型）
        /// </summary>
        public string AgentAuxState
        {
            get
            {
                if (agentAuxState == null)
                {
                    agentAuxState = HttpUtility.UrlDecode((Request["AgentAuxState"] + "").Trim());
                }
                return agentAuxState;
            }
        }

        private string calledNum;
        /// <summary>
        /// 当前被叫号码
        /// </summary>
        public string CalledNum
        {
            get
            {
                if (calledNum == null)
                {
                    calledNum = HttpUtility.UrlDecode((Request["CalledNum"] + "").Trim());
                }
                return calledNum;
            }
        }

        private string callerNum;
        /// <summary>
        /// 当前主叫号码
        /// </summary>
        public string CallerNum
        {
            get
            {
                if (callerNum == null)
                {
                    callerNum = HttpUtility.UrlDecode((Request["CallerNum"] + "").Trim());
                }
                return callerNum;
            }
        }

        private string mediaType;
        /// <summary>
        /// 业务性质(枚举名称)
        /// </summary>
        public string MediaType
        {
            get
            {
                if (mediaType == null)
                {
                    mediaType = HttpUtility.UrlDecode((Request["MediaType"] + "").Trim());
                }
                return mediaType;
            }
        }

        private string userEvent;
        /// <summary>
        /// 当前CTI事件名称
        /// </summary>
        public string UserEvent
        {
            get
            {
                if (userEvent == null)
                {
                    userEvent = HttpUtility.UrlDecode((Request["UserEvent"] + "").Trim());
                }
                return userEvent;
            }
        }

        private string userName;
        /// <summary>
        /// 工号码
        /// </summary>
        public string UserName
        {
            get
            {
                if (userName == null)
                {
                    userName = HttpUtility.UrlDecode((Request["UserName"] + "").Trim());
                }
                return userName;
            }
        }
        private string custID;
        /// <summary>
        /// CRM客户ID
        /// </summary>
        public string CustID
        {
            get
            {
                if (custID == null)
                {
                    custID = HttpUtility.UrlDecode((Request["CustID"] + "").Trim());
                }
                return custID;
            }
        }
        private string custname;
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustName
        {
            get
            {
                if (custname == null)
                {
                    custname = HttpUtility.UrlDecode((Request["CustName"] + "").Trim());
                }
                return custname;
            }
        }
        private string contact;
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact
        {
            get
            {
                if (contact == null)
                {
                    contact = HttpUtility.UrlDecode((Request["Contact"] + "").Trim());
                }
                return contact;
            }
        }
        private string taskid;
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID
        {
            get
            {
                if (taskid == null)
                {
                    taskid = HttpUtility.UrlDecode((Request["TaskID"] + "").Trim());
                }
                return taskid;
            }
        }
        private string callrecordID;
        public string CallRecordID
        {
            get
            {
                if (callrecordID == null)
                {
                    callrecordID = HttpUtility.UrlDecode((Request["CallRecordID"] + "").Trim());
                }
                return callrecordID;
            }
        }
        //是否提交过
        private string issub;
        public string IsSub
        {
            get
            {
                if (issub == null)
                {
                    issub = HttpUtility.UrlDecode((Request["IsSub"] + "").Trim());
                }
                return issub;
            }
        }
        //NetworkRTimeSpan: escape($('#hidTimeSpan').val()),
        //       EstablishTimeSpan: escape(timespan)

        //电话播出到坐席接起的时间
        private string networkrtimespan;
        public string NetworkRTimeSpan
        {
            get
            {
                if (networkrtimespan == null)
                {
                    networkrtimespan = HttpUtility.UrlDecode((Request["NetworkRTimeSpan"] + "").Trim());
                }
                return networkrtimespan;
            }
        }
        //客户响铃时长
        private string establishtimespan;
        public string EstablishTimeSpan
        {
            get
            {
                if (establishtimespan == null)
                {
                    establishtimespan = HttpUtility.UrlDecode((Request["EstablishTimeSpan"] + "").Trim());
                }
                return establishtimespan;
            }
        }

        /// <summary>
        /// cc客户id
        /// </summary>
        private string newcustid;
        public string NewCustID
        {
            get
            {
                if (newcustid == null)
                {
                    newcustid = HttpUtility.UrlDecode((Request["NewCustID"] + "").Trim());
                }
                return newcustid;
            }
        }

        /// <summary>
        /// 易派会员id
        /// </summary>
        private string dmsmemberid;
        public string DMSMemberID
        {
            get
            {
                if (dmsmemberid == null)
                {
                    dmsmemberid = HttpUtility.UrlDecode((Request["DMSMemberID"] + "").Trim());
                }
                return dmsmemberid;
            }
        }

        /// <summary>
        /// cc易派会员id
        /// </summary>
        private string newmemberid;
        public string NewMemberID
        {
            get
            {
                if (newmemberid == null)
                {
                    newmemberid = HttpUtility.UrlDecode((Request["NewMemberID"] + "").Trim());
                }
                return newmemberid;
            }
        }

        /// <summary>
        /// 车商通会员id
        /// </summary>
        private string cstmemberid;
        public string CSTMemberID
        {
            get
            {
                if (cstmemberid == null)
                {
                    cstmemberid = HttpUtility.UrlDecode((Request["CSTMemberID"] + "").Trim());
                }
                return cstmemberid;
            }
        }
        /// <summary>
        /// 新车商通会员id
        /// </summary>
        private string newcstmemberid;
        public string NewCSTMemberID
        {
            get
            {
                if (newcstmemberid == null)
                {
                    newcstmemberid = HttpUtility.UrlDecode((Request["NewCSTMemberID"] + "").Trim());
                }
                return newcstmemberid;
            }
        }

        #endregion

        /// <summary>
        /// 接通开始时间
        /// </summary>
        public string RequestEstablishBeginTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("EstablishBeginTime").ToString().Trim();
            }
        }
        /// <summary>
        /// 接通结束时间
        /// </summary>
        public string RequestEstablishEndTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("EstablishEndTime").ToString().Trim();
            }
        }


        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action)
            {
                case "InsertCallRecordORIGBusiness"://呼出初始化，插入中间表表记录
                    Loger.Log4Net.Info("[WorkOrder\\CallOutDealHandler.ashx]insertcallrecordorigbusiness begin...TaskID:" + TaskID);
                    try
                    {
                        InsertCallRecord_ORIG_Business(out msg);
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        Loger.Log4Net.Info("[WorkOrder\\CallOutDealHandler.ashx]insertcallrecordorigbusiness errorMessage...is:" + ex.Message);
                        Loger.Log4Net.Info("[WorkOrder\\CallOutDealHandler.ashx]insertcallrecordorigbusiness errorSource...is:" + ex.Source);
                        Loger.Log4Net.Info("[WorkOrder\\CallOutDealHandler.ashx]insertcallrecordorigbusiness errorStackTrace...is:" + ex.StackTrace);
                    }
                    break;
                case "Established"://回访呼出接通
                    Established(out msg);
                    break;
                case "Released":
                    Released(out msg);
                    break;
                case "EstablishedCallBack":
                    NewEstablished(out msg);
                    break;
                default:
                    msg = "{'success':'no','recordid':'请求参数错误！'}";
                    break;
            }
            context.Response.Write(msg);
        }


        //插入话务总表跟业务分组中间表记录
        public void InsertCallRecord_ORIG_Business(out string msg)
        {
            msg = string.Empty;
            int loginID = BLL.Util.GetLoginUserID();
            Entities.CallRecord_ORIG_Business callrecordorgbusiness = new Entities.CallRecord_ORIG_Business();

            callrecordorgbusiness.CreateUserID = loginID;

            //工单取业务组，分类
            int _bgid = BLL.SurveyCategory.Instance.GetSelfBGIDByUserID(loginID);//登陆者所在业务组ID
            int _scid = BLL.SurveyCategory.Instance.GetSelfSCIDByUserID(loginID);//登陆者所在业务组下的工单分类ID
            callrecordorgbusiness.BGID = _bgid;
            callrecordorgbusiness.SCID = _scid;

            //西门子callid
            Int64 _callID = 0;
            if (Int64.TryParse(CallID, out _callID))
            {
            }
            callrecordorgbusiness.CallID = _callID;
            callrecordorgbusiness.CreateTime = DateTime.Now;
            callrecordorgbusiness.BusinessID = TaskID;

            //update by lihf on 2014-01-23
            //工单处理页外呼时 也需根据先判是否有该录音记录，防止重复数据
            int RecID = 0;
            //查询现在表
            if (!BLL.CallRecord_ORIG_Business.Instance.IsExistsByCallID(_callID))
            {
                RecID = BLL.CallRecord_ORIG_Business.Instance.Insert(callrecordorgbusiness);
            }
            else
            {
                RecID = BLL.CallRecord_ORIG_Business.Instance.Update(callrecordorgbusiness);
            }
            msg = "{'success':'yes','recordid':'" + RecID + "'}";
        }

        /// <summary>
        ///工单外呼电话呼出接通。 
        ///2040-06-18 毕帆 返回数据添加WORID值
        /// </summary>
        /// <param name="msg"></param>
        private void Established(out string msg)
        {
            msg = string.Empty;
            Entities.CallRecordInfo model = new Entities.CallRecordInfo();
            //通话流水号
            model.SessionID = RecordID;
            //坐席分机号
            model.ExtensionNum = UserName;

            //对方号码
            model.PhoneNum = CallerNum;
            //呼出号码
            model.ANI = CalledNum;

            //呼出
            model.CallStatus = 2;
            //录音开始时间
            model.BeginTime = System.DateTime.Now;

            DateTime beginTime = DateTime.Now;
            if (DateTime.TryParse(RequestEstablishBeginTime, out beginTime))
            {

            }
            model.BeginTime = beginTime;

            //录音地址
            model.AudioURL = RecordIDURL;
            //CRM客户ID
            model.CustID = CustID;
            //客户名称
            model.CustName = CustName;
            model.CreateTime = System.DateTime.Now;
            int loginID = BLL.Util.GetLoginUserID();
            model.CreateUserID = loginID;
            //联系人
            model.Contact = Contact;
            //任务ID
            model.TaskID = TaskID;

            model.TallTime = 0;

            //坐席振铃时长
            int AgentRingTime = 0;
            if (int.TryParse(NetworkRTimeSpan, out AgentRingTime))
            {
                model.AgentRingTime = AgentRingTime;
            }
            //客户振铃时长
            int CustomRingTime = 0;
            if (int.TryParse(EstablishTimeSpan, out CustomRingTime))
            {
                model.CustomRingTime = CustomRingTime;
            }
            int _newcustid;
            if (int.TryParse(NewCustID, out _newcustid))
            {
                model.CCCustID = _newcustid.ToString();
            }
            //工单取业务组，分类
            int _bgid = BLL.SurveyCategory.Instance.GetSelfBGIDByUserID(loginID);//登陆者所在业务组ID
            int _scid = BLL.SurveyCategory.Instance.GetSelfSCIDByUserID(loginID);//登陆者所在业务组下的工单分类ID
            model.BGID = _bgid;
            model.SCID = _scid;

            //西门子callid
            Int64 _callID = 0;
            if (Int64.TryParse(CallID, out _callID))
            {
            }
            model.CallID = _callID;


            int RecID = BLL.CallRecordInfo.Instance.Insert(model);

            //插入工单回复信息
            Entities.WorkOrderRevert workorderrevert = new Entities.WorkOrderRevert();
            workorderrevert.OrderID = model.TaskID;
            workorderrevert.CreateTime = DateTime.Now;
            workorderrevert.CreateUserID = model.CreateUserID;
            workorderrevert.CallID = model.CallID;
            workorderrevert.ReceiverDepartName = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetDistrictNameByUserID(Convert.ToInt32(model.CreateUserID));
            long ival = BLL.WorkOrderRevert.Instance.Insert(workorderrevert);

            BLL.Loger.Log4Net.Info("【工单外呼】准备调用接口CallRecordServiceHelper.Instance.UpdateBusinessDataByCallID开始");

            //调用webservice,保存callid,业务id，业务组，分类对应关系
            //int Result = CallRecordServiceHelper.Instance.UpdateBusinessDataByCallID(model.CallID, model.TaskID, model.BGID, model.SCID, Convert.ToInt32(model.CreateUserID), ref msg);
            int Result = BLL.CallRecord_ORIG_Business.Instance.UpdateBusinessDataByCallID(model.CallID, model.TaskID, model.BGID, model.SCID, Convert.ToInt32(model.CreateUserID), ref msg);
            BLL.Loger.Log4Net.Info("【工单外呼】准备调用接口CallRecordServiceHelper.Instance.UpdateBusinessDataByCallID结束返回值Result=" + Result);

            msg = "{'success':'yes','recordid':'" + RecID + "','WORID':'" + ival + "'}";
        }


        /// <summary>
        /// 电话呼出挂断
        /// </summary>
        /// <param name="msg"></param>
        private void Released(out string msg)
        {
            msg = string.Empty;
            string HistoryLogID = "";
            Int64 RecID = 0;
            //录音表主键格式正确
            if (Int64.TryParse(CallRecordID, out RecID))
            {
                //取本地录音记录
                Entities.CallRecordInfo model = BLL.CallRecordInfo.Instance.GetCallRecordInfo(RecID);
                if (model != null && model.RecID > 0)
                {
                    ////给录音结束时间付值
                    ////model.EndTime = System.DateTime.Now;
                    //model.EndTime = Convert.ToDateTime(RequestEstablishEndTime);
                    //System.DateTime begintime = new DateTime();
                    ////如果录音开始时间为合法时间
                    //if (model.BeginTime != null && model.BeginTime != BitAuto.ISDC.CC2012.Entities.Constants.Constant.DATE_INVALID_VALUE)
                    //{
                    //    if (DateTime.TryParse(model.BeginTime.ToString(), out begintime))
                    //    {
                    //        //取录音结束时间与开始时间直接的通话时长描述
                    //        //TimeSpan s = System.DateTime.Now - begintime;
                    //        TimeSpan s = Convert.ToDateTime(model.EndTime) - begintime;
                    //        model.TallTime = Convert.ToInt32(s.TotalSeconds);
                    //    }
                    //}

                    DateTime endTime = DateTime.Now;
                    model.TallTime = 0;
                    if (DateTime.TryParse(RequestEstablishEndTime, out endTime))
                    {
                        if (model.BeginTime != null && model.BeginTime != BitAuto.ISDC.CC2012.Entities.Constants.Constant.DATE_INVALID_VALUE)
                        {
                            TimeSpan tsSpan = (TimeSpan)(endTime - model.BeginTime);
                            model.TallTime = (int)tsSpan.TotalSeconds;
                        }
                    }
                    model.EndTime = endTime;
                    model.SessionID = RecordID;
                    model.AudioURL = RecordIDURL;
                    int result = 0;
                    result = BLL.CallRecordInfo.Instance.Update(model);
                    //更新录音结束时间成功
                    if (result > 0)
                    {
                        //调用接口更新宇高数据

                        //没有提交过，插入CustHistoryLog

                    }
                }
            }
            //返回处理记录主键
            msg = "{'success':'yes','recordid':'" + HistoryLogID + "'}";
        }

        private void NewEstablished(out string msg)
        {
            int logUser = BLL.Util.GetLoginUserID();
            //插入工单回复信息
            Entities.WorkOrderRevert workorderrevert = new Entities.WorkOrderRevert();
            workorderrevert.OrderID = TaskID;
            workorderrevert.CreateTime = DateTime.Now;
            workorderrevert.CreateUserID = logUser;

            Int64 _callID = 0;
            if (Int64.TryParse(CallID, out _callID))
            {
            }
            workorderrevert.CallID = _callID;
            workorderrevert.ReceiverDepartName = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetDistrictNameByUserID(Convert.ToInt32(logUser));
            long ival = BLL.WorkOrderRevert.Instance.Insert(workorderrevert);
            msg = "{'worid':'" + ival + "'}";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

}
