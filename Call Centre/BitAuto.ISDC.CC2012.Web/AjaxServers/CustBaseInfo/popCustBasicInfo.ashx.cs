using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo
{
    /// <summary>
    /// popCustBasicInfo 的摘要说明
    /// </summary>
    public class popCustBasicInfo : IHttpHandler, IRequiresSessionState
    {
        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        private string Action
        {
            get
            {
                if (Request["Action"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string CustName
        {
            get
            {
                if (Request["CustName"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CustName"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string Sex
        {
            get
            {
                if (Request["Sex"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Sex"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string Tel
        {
            get
            {
                if (Request["Tel"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Tel"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string TaskID
        {
            get
            {
                if (Request["TaskID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TaskID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string CallRecordID
        {
            get
            {
                if (Request["CallRecordID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CallRecordID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string RecordType   //记录类型（1-呼入，2-呼出）
        {
            get
            {
                if (Request["RecordType"] != null)
                {
                    return HttpUtility.UrlDecode(Request["RecordType"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string BusinessType   //业务类型：1工单，2团购订单，3客户核实，4其他任务
        {
            get
            {
                if (Request["BusinessType"] != null)
                {
                    return HttpUtility.UrlDecode(Request["BusinessType"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string CustID
        {
            get
            {
                if (Request["CustID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CustID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string CallID
        {
            get
            {
                if (Request["CallID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CallID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            OperPopCustBasicInfo cbi = new OperPopCustBasicInfo();
            string msg = string.Empty;
            switch (Action)
            {
                case "telIsExists": cbi.telIsExists(Tel, out msg);
                    break;
                case "operCustHistoryInfo":
                    long callid = CommonFunction.ObjectToLong(CallID, -2);
                    long recid = CommonFunction.ObjectToLong(CallRecordID, -2);
                    int businesstype = CommonFunction.ObjectToInteger(BusinessType);
                    int tasksource = CommonFunction.ObjectToInteger(RecordType);
                    int userid = BLL.Util.GetLoginUserID();
                    OperPopCustBasicInfo.OperCustVisitBusiness(CustID, TaskID, callid, businesstype, tasksource, userid, Tel);
                    break;
                case "operCustTel2":
                    CustBasicInfo info3 = new CustBasicInfo();
                    info3.CustName = CustName;
                    info3.Sex = Sex;
                    info3.Tel = Tel;
                    info3.OperID = BLL.Util.GetLoginUserID();
                    cbi.InsertCustInfo(info3, out msg);
                    break;
            }
            context.Response.Write("{" + msg + "}");
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