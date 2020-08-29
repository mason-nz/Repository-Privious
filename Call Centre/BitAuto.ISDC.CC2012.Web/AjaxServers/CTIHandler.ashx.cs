using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.BLL;
using System.Web.SessionState;
using System.Data;
using BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo;
using System.Threading;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    /// <summary>
    /// CTIHandler 的摘要说明
    /// </summary>
    public class CTIHandler : IHttpHandler, IRequiresSessionState
    {
        #region 属性定义
        public string RequestAction
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Action").ToString();
            }
        }
        public string RequestTel
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Tel").ToString().Trim();
            }
        }
        public string RequestSessionID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("SessionID").ToString().Trim();
            }
        }
        public string RequestExtensionNum
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ExtensionNum").ToString().Trim();
            }
        }
        public string RequestPhoneNum
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("PhoneNum").ToString().Trim();
            }
        }
        public string RequestANI
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ANI").ToString().Trim();
            }
        }
        public string RequestCallStatus
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CallStatus").ToString().Trim();
            }
        }
        public string RequestTaskTypeID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("TaskTypeID").ToString().Trim();
            }
        }
        public string RequestAudioURL
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("AudioURL").ToString().Trim();
            }
        }
        public string RequestAgentRingTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("AgentRingTime").ToString().Trim();
            }

        }
        public string RequestCustomerRingTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CustomerRingTime").ToString().Trim();
            }
        }
        public string RequestEstablishBeginTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("EstablishBeginTime").ToString().Trim();
            }
        }
        public string RequestEstablishEndTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("EstablishEndTime").ToString().Trim();
            }
        }
        public string RequestSkillGroup
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("SkillGroup").ToString().Trim();
            }
        }
        public string RequestCustID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CustID").ToString().Trim();
            }
        }
        public string RequestCustName
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CustName").ToString().Trim();
            }
        }
        public string RequestCallRecordID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CallRecordID").ToString().Trim();
            }
        }
        public string RequestAfterWorkTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("AfterWorkTime").ToString().Trim();
            }
        }
        public string RequestLogMsg
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("LogMsg").ToString().Trim();
            }
        }
        public string RequestEventName
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("EventName").ToString().Trim();
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
                    callID = BLL.Util.GetCurrentRequestStr("CallID").ToString().Trim();
                }
                return callID;
            }
        }
        /// <summary>
        /// 电话
        /// </summary>
        private string teldata;
        public string TelData
        {
            get
            {
                if (teldata == null)
                {
                    teldata = BLL.Util.GetCurrentRequestStr("TelData").ToString().Trim();
                }
                return teldata;
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            BLL.Loger.Log4Net.Info("[CTIHandler.ashx]Inbound ProcessRequest begin...PhoneNum:" + RequestPhoneNum + ",ANI:" + RequestANI + ",RequestAction:" + RequestAction);
            string msg = string.Empty;
            context.Response.ContentType = "text/plain";
            //获取客户ID
            if (RequestAction.ToLower() == "GetCustID".ToLower())
            {
                try
                {
                    msg = CTIHandlerHelper.GetCustIDByTel(RequestTel);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            else if (RequestAction.ToLower() == "GetAreaID".ToLower())
            {
                try
                {
                    GetAreaID(out msg);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            else if (RequestAction.ToLower() == "uccalloutlog")
            {
                int loginID = BLL.Util.GetLoginUserID();
                BLL.Loger.Log4Net.Info("[CTIHandler.ashx]UCCallOut...Start...UserID:" + loginID + ",phoneNum:" + RequestPhoneNum);
            }
            BLL.Loger.Log4Net.Info("[CTIHandler.ashx]Inbound ProcessRequest bye...PhoneNum:" + RequestPhoneNum + ",ANI:" + RequestANI);
            context.Response.Write(msg);
        }
        //根据电话号码查询城市ID,如果没有找到返回0
        public void GetAreaID(out string msg)
        {
            msg = string.Empty;
            if (!string.IsNullOrEmpty(RequestPhoneNum))
            {
                int provinceId = -1;
                int cityId = -1;
                BLL.PhoneNumDataDict.GetAreaId(RequestPhoneNum, out provinceId, out cityId);
                if (provinceId <= 0) provinceId = -1;
                if (cityId <= 0) cityId = -1;
                msg = "{ProvinceID:'" + provinceId + "',CityID:'" + cityId + "'}";
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class CTIHandlerHelper
    {
        public static string GetCustIDByTel(string tel)
        {
            string msg = "";
            BLL.Loger.Log4Net.Info("[CTIHandlerHelper] GetCustIDByTel=" + tel);
            if (!string.IsNullOrEmpty(tel))
            {
                //查询个人用户ID
                string custid = BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(tel);
                if (!string.IsNullOrEmpty(custid))
                {
                    msg = "{TotalCount:'1',CustID:'" + custid + "'}";
                }
                else
                {
                    BLL.Loger.Log4Net.Info("[CTIHandlerHelper]开始在CRM客户库查找!主叫号：" + tel);
                    //从CRM中找一个最新的数据同步
                    DataTable crm_dt = BLL.CrmCustInfo.Instance.GetCC_CrmContactInfo(tel);
                    if (crm_dt.Rows.Count > 0)
                    {
                        //取最新的数据
                        DataRow row = crm_dt.Rows[0];
                        BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.CustBasicInfo info = new CustBaseInfo.CustBasicInfo();
                        info.CustName = row["CustName"].ToString();
                        info.Sex = "1";//性别默认先生
                        info.Tel = tel;
                        info.CustCategoryID = "3";//3经销商,4个人
                        string[] array = getMemberInfo(row["CustID"].ToString());
                        info.MemberID = array[0];
                        info.MemberName = array[1];
                        info.OperID = BLL.Util.GetLoginUserID();

                        string errmsg = "";
                        OperPopCustBasicInfo cbi = new OperPopCustBasicInfo();
                        cbi.InsertCustInfo(info, out errmsg);

                        if (errmsg.Contains("true"))
                        {
                            msg = "{TotalCount:'1',CustID:'" + cbi.ccCustID + "'}";
                        }
                    }
                }
            }
            if (msg == "")
            {
                msg = "{TotalCount:'0',CustID:''}";
            }
            BLL.Loger.Log4Net.Info("[CTIHandlerHelper] 查找结果：" + msg);
            return msg;
        }

        private static string[] getMemberInfo(string CRMCustID)
        {
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.SelectByCustID(CRMCustID);
            string[] array = new string[2];
            if (dt.Rows.Count > 0)
            {
                array[0] = dt.Rows[0]["MemberCode"].ToString();
                array[1] = dt.Rows[0]["FullName"].ToString();
            }
            return array;
        }
    }

    public class TelResult
    {
        public int TotalCount { get; set; }
        public string CustID { get; set; }
        public string CustType { get; set; }
    }
}