using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.WebService;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Diagnostics;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.LeadsTask
{
    /// <summary>
    /// CSLeadsTaskDeal 的摘要说明
    /// </summary>
    public class CSLeadsTaskDeal : IHttpHandler, IRequiresSessionState
    {
        #region 参数
        /// <summary>
        /// 项目ID
        /// </summary>
        private string RequestProjectID
        {
            get { return HttpContext.Current.Request["ProjectID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProjectID"].ToString()); }
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        private string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        /// <summary>
        /// 失败原因
        /// </summary>
        private string RequestIsSuccess
        {
            get { return HttpContext.Current.Request["IsSuccess"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsSuccess"].ToString()); }
        }
        /// <summary>
        /// 失败原因
        /// </summary>
        private string RequestFailReson
        {
            get { return HttpContext.Current.Request["FailReson"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["FailReson"].ToString()); }
        }
        /// <summary>
        /// 未接通原因
        /// </summary>
        private string RequestNotEstablishReason
        {
            get { return HttpContext.Current.Request["NotEstablishReason"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["NotEstablishReason"].ToString()); }
        }
        /// <summary>
        /// 接通后失败原因
        /// </summary>
        private string RequestNotSuccessReason
        {
            get { return HttpContext.Current.Request["NotSuccessReason"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["NotSuccessReason"].ToString()); }
        }
        /// <summary>
        /// 备注
        /// </summary>
        private string RequestRemark
        {
            get { return HttpContext.Current.Request["Remark"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Remark"].ToString()); }
        }
        /// <summary>
        /// 类型
        /// </summary>
        private string Action
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString()); }
        }
        /// <summary>
        /// guid串
        /// </summary>
        private string RequestGuid
        {
            get
            {
                return HttpContext.Current.Request["GuidStr"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["GuidStr"].ToString());
            }
        }
        /// <summary>
        /// 经销商编号
        /// </summary>
        private string RequestMemberCode
        {
            get
            {
                return HttpContext.Current.Request["MemberCode"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["MemberCode"].ToString());
            }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        private string RequestUserName
        {
            get
            {
                return HttpContext.Current.Request["UserName"] == null ? "" :
                     HttpUtility.UrlDecode(HttpUtility.UrlDecode(HttpContext.Current.Request["UserName"].ToString()));
            }
        }
        /// <summary>
        /// 手机
        /// </summary>
        private string RequestMobilePhone
        {
            get
            {
                return HttpContext.Current.Request["MobilePhone"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["MobilePhone"].ToString());
            }
        }
        /// <summary>
        /// 需匹配车款ID
        /// </summary>
        private string RequestDCarID
        {
            get
            {
                return HttpContext.Current.Request["DCarID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DCarID"].ToString());
            }
        }

        /// <summary>
        /// 需匹配车型ID
        /// </summary>
        private string RequestDSerialID
        {
            get
            {
                return HttpContext.Current.Request["DSerialID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DSerialID"].ToString());
            }
        }
        /// <summary>
        /// 需匹配车型
        /// </summary>
        private string RequestDSerialName
        {
            get
            {
                return HttpContext.Current.Request["DSerialName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DSerialName"].ToString());
            }
        }

        /// <summary>
        /// 需匹配车款名称
        /// </summary>
        private string RequestDCarName
        {
            get
            {
                return HttpContext.Current.Request["DCarName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DCarName"].ToString());
            }
        }
        /// <summary>
        /// 省份
        /// </summary>
        private string RequestProvinceID
        {
            get
            {
                return HttpContext.Current.Request["ProvinceID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProvinceID"].ToString());
            }
        }
        /// <summary>
        /// 城市
        /// </summary>
        private string RequestCityID
        {
            get
            {
                return HttpContext.Current.Request["CityID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CityID"].ToString());
            }
        }


        /// <summary>
        /// 是否接通
        /// </summary>
        private string RequestIsJT
        {
            get
            {
                return HttpContext.Current.Request["IsJT"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsJT"].ToString());
            }
        }

        /// <summary>
        /// 预计购车时间
        /// </summary>
        private string RequestPBuyCarTime
        {
            get
            {
                return HttpContext.Current.Request["PBuyCarTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["PBuyCarTime"].ToString());
            }
        }

        /// <summary>
        /// 考虑车型
        /// </summary>
        private string RequestThinkCar
        {
            get
            {
                return HttpContext.Current.Request["ThinkCar"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ThinkCar"].ToString());
            }
        }

        /// <summary>
        /// 厂商需求单号
        /// </summary>
        private string RequestDemandID
        {
            get
            {
                return HttpContext.Current.Request["DemandID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DemandID"].ToString());
            }
        }
        /// <summary>
        /// BGID
        /// </summary>
        private string RequestBGID
        {
            get
            {
                return HttpContext.Current.Request["BGID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BGID"].ToString());
            }

        }
        /// <summary>
        /// SCID
        /// </summary>
        private string RequestSCID
        {
            get
            {
                return HttpContext.Current.Request["SCID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SCID"].ToString());
            }

        }
        /// <summary>
        /// SEX
        /// </summary>
        private string RequestSEX
        {
            get
            {
                return HttpContext.Current.Request["SEX"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SEX"].ToString());
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
                    recordID = HttpUtility.UrlDecode((HttpContext.Current.Request["RecordID"] + "").Trim());
                }
                return recordID;
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
                    calledNum = HttpUtility.UrlDecode((HttpContext.Current.Request["CalledNum"] + "").Trim());
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
                    callerNum = HttpUtility.UrlDecode((HttpContext.Current.Request["CallerNum"] + "").Trim());
                }
                return callerNum;
            }
        }


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
                    recordIDURL = HttpUtility.UrlDecode((HttpContext.Current.Request["RecordIDURL"] + "").Trim());
                }
                return recordIDURL;
            }
        }

        //电话播出到坐席接起的时间
        private string networkrtimespan;
        public string NetworkRTimeSpan
        {
            get
            {
                if (networkrtimespan == null)
                {
                    networkrtimespan = HttpUtility.UrlDecode((HttpContext.Current.Request["NetworkRTimeSpan"] + "").Trim());
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
                    establishtimespan = HttpUtility.UrlDecode((HttpContext.Current.Request["EstablishTimeSpan"] + "").Trim());
                }
                return establishtimespan;
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

        public string RequestCustName
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CustName").ToString().Trim();
            }
        }
        private string RequestDCarMaster
        {
            get
            {
                return HttpContext.Current.Request["DCarMaster"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DCarMaster"].ToString());
            }
        }
        private string RequestDCarMasterID
        {
            get
            {
                return HttpContext.Current.Request["DCarMasterID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DCarMasterID"].ToString());
            }
        }
        ///// <summary>
        ///// 促销单版本号
        ///// </summary>
        //public string RequestDemandVersion
        //{
        //    get
        //    {
        //        return BLL.Util.GetCurrentRequestStr("DemandVersion").ToString().Trim();
        //    }
        //}
        private string RequestIsBuyCar
        {
            get
            {
                return HttpContext.Current.Request["IsBuyCar"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsBuyCar"].ToString());
            }
        }

        private string RequestBoughtCarMasterID
        {
            get
            {
                return HttpContext.Current.Request["BoughtCarMasterID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BoughtCarMasterID"].ToString());
            }
        }
        private string RequestBoughtCarMasterName
        {
            get
            {
                return HttpContext.Current.Request["BoughtCarMasterName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BoughtCarMasterName"].ToString());
            }
        }
        private string RequestBoughtSerialID
        {
            get
            {
                return HttpContext.Current.Request["BoughtSerialID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BoughtSerialID"].ToString());
            }
        }
        private string RequestBoughtSerialName
        {
            get
            {
                return HttpContext.Current.Request["BoughtSerialName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BoughtSerialName"].ToString());
            }
        }
        private string RequestBoughtYearMonth
        {
            get
            {
                return HttpContext.Current.Request["BoughtYearMonth"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BoughtYearMonth"].ToString());
            }
        }
        private string RequestBuyCarMemberCode
        {
            get
            {
                return HttpContext.Current.Request["BuyCarMemberCode"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BuyCarMemberCode"].ToString());
            }
        }
        private string RequestBuyCarMemberName
        {
            get
            {
                return HttpContext.Current.Request["BuyCarMemberName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BuyCarMemberName"].ToString());
            }
        }

        private string RequestHasBuyCarPlan
        {
            get
            {
                return HttpContext.Current.Request["HasBuyCarPlan"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["HasBuyCarPlan"].ToString());
            }
        }



        private string RequestIsAttention
        {
            get
            {
                return HttpContext.Current.Request["IsAttention"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsAttention"].ToString());
            }
        }
        private string RequestIsContactedDealer
        {
            get
            {
                return HttpContext.Current.Request["IsContactedDealer"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsContactedDealer"].ToString());
            }
        }
        private string RequestIsSatisfiedService
        {
            get
            {
                return HttpContext.Current.Request["IsSatisfiedService"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsSatisfiedService"].ToString());
            }
        }
        private string RequestContactedWhichDealer
        {
            get
            {
                return HttpContext.Current.Request["ContactedWhichDealer"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ContactedWhichDealer"].ToString());
            }
        }


        private string RequestIntentionCarMasterID
        {
            get
            {
                return HttpContext.Current.Request["IntentionCarMasterID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IntentionCarMasterID"].ToString());
            }
        }
        private string RequestIntentionCarMaster
        {
            get
            {
                return HttpContext.Current.Request["IntentionCarMaster"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IntentionCarMaster"].ToString());
            }
        }
        private string RequestIntentionCarSerialID
        {
            get
            {
                return HttpContext.Current.Request["IntentionCarSerialID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IntentionCarSerialID"].ToString());
            }
        }
        private string RequestIntentionCarSerial
        {
            get
            {
                return HttpContext.Current.Request["IntentionCarSerial"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IntentionCarSerial"].ToString());
            }
        }
        #endregion

        private int userId;
        private string RealName;
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";
            CheckData(out msg);
            if (msg == "")
            {
                userId = BLL.Util.GetLoginUserID();
                RealName = BLL.Util.GetLoginRealName();
                try
                {
                    switch (Action.ToLower())
                    {
                        case "saveinfo":
                            Stopwatch stopwatch = new Stopwatch();
                            stopwatch.Start();
                            saveinfo(out msg);
                            stopwatch.Stop();
                            BLL.Loger.Log4Net.Info(string.Format("【CJK任务保存Step8——总耗时】：{0}毫秒", stopwatch.Elapsed.TotalMilliseconds));
                            break;
                        case "subinfo":
                            Stopwatch stopwatch2 = new Stopwatch();
                            stopwatch2.Start();
                            subinfo(out msg);
                            stopwatch2.Stop();
                            BLL.Loger.Log4Net.Info(string.Format("【CJK任务提交Step8——总耗时】：{0}毫秒", stopwatch2.Elapsed.TotalMilliseconds));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("CSLeadsTaskDeal.ashx 异常信息：" + ex.Message);
                }
            }
            if (msg == "")
            {
                msg = "{\"Result\":true}";
            }
            context.Response.Write(msg);
        }

        /// <summary>
        /// 后台验证
        /// </summary>
        /// <param name="msg"></param>
        protected void CheckData(out string msg)
        {
            msg = "";
            if (string.IsNullOrEmpty(RequestTaskID))
            {
                msg = "{\"Result\":false,\"Msg\":\"任务不能为空\"}";
            }
            else
            {
                if (string.IsNullOrEmpty(Action))
                {
                    msg = "{\"Result\":false,\"Msg\":\"请求参数错误！\"}";
                }
                else
                {
                    if (!string.IsNullOrEmpty(RequestUserName))
                    {
                        if (RequestUserName.Length > 100)
                        {
                            msg = "{\"Result\":false,\"Msg\":\"姓名超长！\"}";
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(RequestThinkCar))
                    {
                        if (RequestThinkCar.Length > 50)
                        {
                            msg = "{\"Result\":false,\"Msg\":\"其他考虑车型超长！\"}";
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(RequestRemark))
                    {
                        if (RequestRemark.Length > 200)
                        {
                            msg = "{\"Result\":false,\"Msg\":\"备注超长！\"}";
                            return;
                        }
                    }
                    if (Action == "subinfo" && RequestIsSuccess == "1" && (string.IsNullOrEmpty(RequestDSerialID) || RequestDSerialID == "-2"))
                    {
                        msg = "{\"Result\":false,\"Msg\":\"目标车型必填！\"}";
                        return;
                    }
                    if (Action == "subinfo" && string.IsNullOrEmpty(RequestIsSuccess))
                    {
                        msg = "{\"Result\":false,\"Msg\":\"是否成功必须选择！\"}";
                        return;
                    }
                    if (!string.IsNullOrEmpty(RequestIsJT))
                    {
                        if (Action == "subinfo" && RequestIsJT == "0" && (string.IsNullOrEmpty(RequestNotEstablishReason) || RequestNotEstablishReason == "-1"))
                        {
                            msg = "{\"Result\":false,\"Msg\":\"未接通原因必填！\"}";
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(RequestIsSuccess))
                    {
                        if (Action == "subinfo" && RequestIsSuccess == "0" && RequestIsJT == "1" && (string.IsNullOrEmpty(RequestNotSuccessReason) || RequestNotSuccessReason == "-1"))
                        {
                            msg = "{\"Result\":false,\"Msg\":\"接通后失败原因必填！\"}";
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 取新增Leads xml字符串
        /// </summary>
        /// <returns></returns>
        protected string GetLeadsXmlStr(string ordernum)
        {
            StringBuilder xmlstr = new StringBuilder();
            xmlstr.Append("<?xml version='1.0' encoding='utf-8' ?>");
            xmlstr.Append("<Leads>");
            xmlstr.Append("<OrderCode>" + RequestDemandID + "</OrderCode>");
            xmlstr.Append("<OrderID>" + ordernum + "</OrderID>");
            xmlstr.Append("<Name>" + RequestUserName + "</Name>");
            xmlstr.Append("<MobilePhone>" + RequestMobilePhone + "</MobilePhone>");
            string sex = "1";
            if (RequestSEX == "2")
            {
                sex = "0";
            }
            xmlstr.Append("<Sex>" + sex + "</Sex>");
            xmlstr.Append("<SerialID>" + RequestDSerialID + "</SerialID>");
            xmlstr.Append("<CityID>" + RequestCityID + "</CityID>");
            //涞源为CC
            xmlstr.Append("<Source>5</Source >");
            xmlstr.Append("<Cookieid ></Cookieid>");
            xmlstr.Append("<UserIP></UserIP>");
            xmlstr.Append("</Leads>");
            return xmlstr.ToString();
        }
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="msg"></param>
        protected void saveinfo(out string msg)
        {
            msg = "";
            //处理Lead是，修改任务状态，插入任务操作日志
            DealTask(1, out msg);
        }
        /// <summary>
        /// ordertype 1是保存，2是提交
        /// </summary>
        /// <param name="ordertype"></param>
        protected void DealTask(int ordertype, out string msg)
        {
            msg = "";
            string strDealType = ordertype == 1 ? "保存" : (ordertype == 2 ? "提交" : ordertype.ToString());
            if (!string.IsNullOrEmpty(RequestTaskID))
            {
                BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step1——根据TaskID获取实体开始】");
                Entities.LeadsTask model = BLL.LeadsTask.Instance.GetLeadsTask(RequestTaskID);
                BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step1——根据TaskID获取实体结束");
                if (model != null)
                {
                    if (model.Status != (int)Entities.LeadsTaskStatus.Processing && model.Status != (int)Entities.LeadsTaskStatus.NoProcess)
                    {
                        msg = "{\"Result\":false,\"Msg\":\"任务不处于处理状态！\"}";
                    }
                    else
                    {
                        #region 保存或提交订单信息，修改任务状态，插入任务操作状态
                        model.LastUpdateTime = System.DateTime.Now;
                        model.LastUpdateUserID = userId;
                        model.Remark = RequestRemark;

                        model.UserName = RequestUserName;
                        int _sex = -2;
                        if (!string.IsNullOrEmpty(RequestSEX))
                        {
                            int.TryParse(RequestSEX, out _sex);
                        }
                        model.Sex = _sex;

                        int _targetProvinceid = -2;
                        if (int.TryParse(RequestProvinceID, out _targetProvinceid))
                        {
                            model.TargetProvinceID = _targetProvinceid;
                        }
                        int _targetCityid = -2;
                        if (int.TryParse(RequestCityID, out _targetCityid))
                        {
                            model.TargetCityID = _targetCityid;
                        }
                        int _DserialID = -2;
                        if (!string.IsNullOrEmpty(RequestDSerialID))
                        {
                            int.TryParse(RequestDSerialID, out _DserialID);
                        }

                        model.DCarSerialID = _DserialID;
                        model.DCarSerial = RequestDSerialName;

                        model.ThinkCar = RequestThinkCar;
                        int PBuyCarTime = -2;
                        if (!string.IsNullOrEmpty(RequestPBuyCarTime))
                        {
                            int.TryParse(RequestPBuyCarTime, out PBuyCarTime);
                        }
                        model.PBuyCarTime = PBuyCarTime;
                        int IsJT = -2;
                        if (!string.IsNullOrEmpty(RequestIsJT))
                        {
                            int.TryParse(RequestIsJT, out IsJT);
                        }
                        model.IsJT = IsJT;

                        //保存
                        if (ordertype == 1)
                        {
                            model.Status = (int)Entities.LeadsTaskStatus.Processing;
                        }
                        //提交
                        else if (ordertype == 2)
                        {
                            model.Status = (int)Entities.LeadsTaskStatus.Processed;
                        }
                        //是否成功
                        int _IsSuccess = -2;
                        if (!string.IsNullOrEmpty(RequestIsSuccess))
                        {
                            int.TryParse(RequestIsSuccess, out _IsSuccess);
                        }
                        model.IsSuccess = _IsSuccess;

                        //失败原因
                        int FailReson = -2;
                        if (!string.IsNullOrEmpty(RequestFailReson))
                        {
                            int.TryParse(RequestFailReson, out FailReson);
                        }
                        model.FailReason = FailReson;

                        int notEstablishReason = -2;
                        if (!string.IsNullOrEmpty(RequestNotEstablishReason))
                        {
                            int.TryParse(RequestNotEstablishReason, out notEstablishReason);
                        }
                        model.NotEstablishReason = notEstablishReason;

                        int notSuccessReason = -2;
                        if (!string.IsNullOrEmpty(RequestNotSuccessReason))
                        {
                            int.TryParse(RequestNotSuccessReason, out notSuccessReason);
                        }
                        model.NotSuccessReason = notSuccessReason;

                        model.DCarMaster = RequestDCarMaster;

                        int thecarmasterid = -2;
                        if (int.TryParse(RequestDCarMasterID, out thecarmasterid))
                        {
                            model.DCarMasterID = thecarmasterid;
                        }

                        int isboughtcar;
                        if (int.TryParse(RequestIsBuyCar, out isboughtcar))
                        {
                            try
                            {
                                if (isboughtcar == 1)
                                {
                                    model.IsBoughtCar = isboughtcar;

                                    model.BoughtCarMasterID = string.IsNullOrEmpty(RequestBoughtCarMasterID) ? Constant.INT_INVALID_VALUE : Convert.ToInt32(RequestBoughtCarMasterID.Trim());
                                    model.BoughtCarMaster = RequestBoughtCarMasterName;
                                    model.BoughtCarSerialID = string.IsNullOrEmpty(RequestBoughtSerialID) ? Constant.INT_INVALID_VALUE : Convert.ToInt32(RequestBoughtSerialID.Trim());
                                    model.BoughtCarSerial = RequestBoughtSerialName;
                                    model.BoughtCarYearMonth = RequestBoughtYearMonth;
                                    model.BoughtCarDealerID = RequestBuyCarMemberCode;
                                    model.BoughtCarDealerName = RequestBuyCarMemberName;

                                    model.HasBuyCarPlan = Constant.INT_INVALID_VALUE;
                                    model.IntentionCarMasterID = Constant.INT_INVALID_VALUE;
                                    model.IntentionCarMaster = Constant.STRING_INVALID_VALUE;
                                    model.IntentionCarSerialID = Constant.INT_INVALID_VALUE;
                                    model.IntentionCarSerial = Constant.STRING_INVALID_VALUE;
                                }
                                else if (isboughtcar == 0)
                                {
                                    model.IsBoughtCar = isboughtcar;

                                    model.BoughtCarMasterID = Constant.INT_INVALID_VALUE;
                                    model.BoughtCarMaster = Constant.STRING_INVALID_VALUE;
                                    model.BoughtCarSerialID = Constant.INT_INVALID_VALUE;
                                    model.BoughtCarSerial = Constant.STRING_INVALID_VALUE;
                                    model.BoughtCarYearMonth = Constant.STRING_INVALID_VALUE;
                                    model.BoughtCarDealerID = Constant.STRING_INVALID_VALUE;
                                    model.BoughtCarDealerName = Constant.STRING_INVALID_VALUE;
                                    //未购车
                                    model.HasBuyCarPlan = string.IsNullOrEmpty(RequestHasBuyCarPlan) ? Constant.INT_INVALID_VALUE : Convert.ToInt32(RequestHasBuyCarPlan.Trim());
                                    model.IsAttention = string.IsNullOrEmpty(RequestIsAttention) ? Constant.INT_INVALID_VALUE : Convert.ToInt32(RequestIsAttention.Trim());
                                    model.IsContactedDealer = string.IsNullOrEmpty(RequestIsContactedDealer) ? Constant.INT_INVALID_VALUE : Convert.ToInt32(RequestIsContactedDealer.Trim());
                                    model.IsSatisfiedService = string.IsNullOrEmpty(RequestIsSatisfiedService) ? Constant.INT_INVALID_VALUE : Convert.ToInt32(RequestIsSatisfiedService.Trim());
                                    model.ContactedWhichDealer = RequestContactedWhichDealer;


                                    model.IntentionCarMasterID = string.IsNullOrEmpty(RequestIntentionCarMasterID) ? Constant.INT_INVALID_VALUE : Convert.ToInt32(RequestIntentionCarMasterID.Trim());
                                    model.IntentionCarMaster = RequestIntentionCarMaster;
                                    model.IntentionCarSerialID = string.IsNullOrEmpty(RequestIntentionCarSerialID) ? Constant.INT_INVALID_VALUE : Convert.ToInt32(RequestIntentionCarSerialID.Trim());
                                    model.IntentionCarSerial = RequestIntentionCarSerial;
                                }
                                else if (isboughtcar == 2)
                                {
                                    model.IsBoughtCar = isboughtcar;

                                    model.BoughtCarMasterID = Constant.INT_INVALID_VALUE;
                                    model.BoughtCarMaster = Constant.STRING_INVALID_VALUE;
                                    model.BoughtCarSerialID = Constant.INT_INVALID_VALUE;
                                    model.BoughtCarSerial = Constant.STRING_INVALID_VALUE;
                                    model.BoughtCarYearMonth = Constant.STRING_INVALID_VALUE;
                                    model.BoughtCarDealerID = Constant.STRING_INVALID_VALUE;
                                    model.BoughtCarDealerName = Constant.STRING_INVALID_VALUE;

                                    model.HasBuyCarPlan = Constant.INT_INVALID_VALUE;
                                    model.IntentionCarMasterID = Constant.INT_INVALID_VALUE;
                                    model.IntentionCarMaster = Constant.STRING_INVALID_VALUE;
                                    model.IntentionCarSerialID = Constant.INT_INVALID_VALUE;
                                    model.IntentionCarSerial = Constant.STRING_INVALID_VALUE;
                                }
                            }
                            catch (Exception ex)
                            {
                                BLL.Loger.Log4Net.Error("【CJK任务" + strDealType + " Step2——开始前出现参数转换出现异常】：" + ex.Message);
                                msg = "{\"Result\":false,\"Msg\":\"参数转换出现异常！\"}";
                                return;
                            }
                        }
                        //更新线索任务信息
                        BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step2——更新线索任务信息开始】");
                        BLL.LeadsTask.Instance.Update(model);
                        BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step2——更新线索任务信息结束】");

                        #region 插入报表统计数据
                        BLL.Loger.Log4Net.Info("【插入线索邀约(CJK)统计报表数据" + strDealType + " Step2.1——插入线索邀约(CJK)统计数据开始】");
                        BLL.CallResult_ORIG_Task.Instance.InseretOrUpdateOneData(model.TaskID, model.ProjectID, ProjectSource.S6_厂家集客, model.IsJT, model.NotEstablishReason, model.IsSuccess, model.NotSuccessReason);
                        BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step2.1——插入线索邀约(CJK)统计数据结束】");
                        #endregion

                        //插入任务操作日志
                        BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step3——插入任务操作记录开始】");
                        DealLog(ordertype);
                        BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step3——插入任务操作记录结束】");
                        #endregion

                        #region 提交处理
                        if (ordertype == 2)
                        {
                            //回写crm
                            int isvalid = 0;
                            int.TryParse(RequestIsSuccess, out isvalid);
                            BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step5——回写crm接口开始】TaskID：" + RequestTaskID + "；UserID：" + userId + "；厂商需求单" + RequestDemandID + ",编号" + model.OrderNum + "，回写crm接口开始，参数是RecID:" + model.OrderNum + ",IsValid:" + isvalid);
                            bool ReCallflag = false;
                            try
                            {
                                ReCallflag = WebService.CRM.CRMCJKServiceHelper.Instance.ReCallLeadsInfo((int)model.OrderNum, isvalid);
                            }
                            catch (Exception ex)
                            {
                                BLL.Loger.Log4Net.Debug("【CJK任务" + strDealType + " Step5——调用crm回写接口出现异常】:", ex);
                            }
                            BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step5——回写crm接口结束】TaskID：" + RequestTaskID + "；UserID：" + userId + "；厂商需求单" + RequestDemandID + ",编号" + model.OrderNum + "，回写crm接口结束，返回结果为" + (ReCallflag == true ? "成功" : "失败"));


                            #region 如果选择成功则调Crm接口,插入调用日志

                            if (model.IsSuccess == 1)
                            {
                                #region 调用新增Leads接口
                                ////logmodel.CreateTime = System.DateTime.Now;

                                //取xml
                                BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step6——获取XML字符串开始】");
                                string addleadsstr = GetLeadsXmlStr(model.OrderNum.ToString());
                                BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step6——获取XML字符串结束】");
                                //记录调用开始
                                ////logmodel.Remark = "为厂商需求单" + RequestDemandID + "调用新增Leads接口开始,xml字符串数据：" + addleadsstr;
                                ////BLL.LeadsTaskOperationLog.Instance.Insert(logmodel);
                                BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step7——调用新增Leads接口开始】TaskID：" + RequestTaskID + "；UserID：" + userId + "；为厂商需求单" + RequestDemandID + "调用新增Leads接口开始,xml字符串数据：" + addleadsstr);
                                string strloginfo = string.Empty;
                                bool flag = false;
                                ////logmodel.CreateTime = System.DateTime.Now;
                                try
                                {
                                    Stopwatch sw6 = new Stopwatch();
                                    sw6.Start();
                                    flag = WebService.CRM.CRMCJKServiceHelper.Instance.AddLeadsInfo(addleadsstr);
                                    //调用成功
                                    if (flag)
                                    {
                                        ////logmodel.Remark = "为厂商需求单" + RequestDemandID + "调用新增Leads接口结束，调用结果是成功！";
                                        strloginfo = "TaskID：" + RequestTaskID + "；UserID：" + userId + "；为厂商需求单" + RequestDemandID + "调用新增Leads接口结束，调用结果是成功！";
                                    }
                                    //调用失败
                                    else
                                    {
                                        ////logmodel.Remark = "为厂商需求单" + RequestDemandID + "调用新增Leads接口结束，调用结果是失败！";
                                        strloginfo = "TaskID：" + RequestTaskID + "；UserID：" + userId + "；为厂商需求单" + RequestDemandID + "调用新增Leads接口结束，调用结果是失败！";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //msg = ex.Message;
                                    BLL.Loger.Log4Net.Debug("【CJK任务" + strDealType + " Step7——调用新增Leads接口出现异常】为厂商需求单" + RequestDemandID + "调用新增Leads接口出现异常，异常信息：", ex);
                                    ////logmodel.Remark = "为厂商需求单" + RequestDemandID + "调用新增Leads接口结束，调用结果是出现异常，异常信息为请查看日志文件";
                                    strloginfo = "TaskID：" + RequestTaskID + "；UserID：" + userId + "；为厂商需求单" + RequestDemandID + "调用新增Leads接口结束，调用结果是出现异常，异常信息为请查看日志文件";
                                    UnhandledExceptionFunction(null, new UnhandledExceptionEventArgs(ex, false));
                                }
                                ////记录调用结束
                                ////BLL.LeadsTaskOperationLog.Instance.Insert(logmodel);
                                BLL.Loger.Log4Net.Info("【CJK任务" + strDealType + " Step7——调用新增Leads接口结束】" + strloginfo);

                                //
                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
                else
                {
                    msg = "{\"Result\":false,\"Msg\":\"任务不存在！\"}";
                }
            }
        }

        /// <summary>
        /// ordertype 1是保存，2是提交
        /// </summary>
        /// <param name="ordertype"></param>
        protected void DealLog(int ordertype)
        {
            Entities.LeadsTaskOperationLog logmodel = new Entities.LeadsTaskOperationLog();
            logmodel.Remark = RequestRemark;
            logmodel.TaskID = RequestTaskID;
            logmodel.CreateTime = System.DateTime.Now;
            logmodel.CreateUserID = userId;
            if (ordertype == 1)
            {
                logmodel.OperationStatus = (int)Entities.Leads_OperationStatus.Save;
                logmodel.TaskStatus = (int)Entities.LeadsTaskStatus.Processing;
                logmodel.Remark = "保存";
            }
            else if (ordertype == 2)
            {
                logmodel.OperationStatus = (int)Entities.Leads_OperationStatus.Submit;
                logmodel.TaskStatus = (int)Entities.LeadsTaskStatus.Processed;
                logmodel.Remark = "提交";
            }
            //if (!string.IsNullOrEmpty(RequestRemark))
            //{
            //    logmodel.Remark += "，备注信息：" + RequestRemark;
            //}
            BLL.LeadsTaskOperationLog.Instance.Insert(logmodel);
        }

        /// <summary>
        /// 提交信息
        /// </summary>
        /// <param name="msg"></param>
        protected void subinfo(out string msg)
        {
            msg = "";
            //1.处理Leads任务，2.修改任务状态，3.插入任务操作日志，4.调用新增leads接口，插入调用日志 5.查询是否补满leads，如果补满，结束项目撤销其他未处理任务，发送短信
            DealTask(2, out msg);

        }
        #region 异常处理
        private void UnhandledExceptionFunction(Object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            BLL.Loger.Log4Net.Debug("调用新增Leads接口出错", e);

            string errorMsg = e.Message;
            string stackTrace = e.StackTrace;
            string source = e.Source;

            string mailBody = string.Format("错位信息：{0}<br/>错误Source：{1}<br/>错误StackTrace：{2}<br/>IsTerminating:{3}<br/>",
                errorMsg, source, stackTrace, args.IsTerminating);
            string subject = "客户呼叫中心系统厂商集客——调用新增Leads接口出错";
            string[] userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
            if (userEmail != null && userEmail.Length > 0)
            {
                BLL.EmailHelper.Instance.SendErrorMail(mailBody, subject, userEmail);
            }
        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}