using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.Base
{
    /// <summary>
    /// CustManager 的摘要说明
    /// </summary>
    public class CustManager : IHttpHandler, IRequiresSessionState
    {
        #region 属性
        private string RequestCustID
        {
            get { return BLL.Util.GetCurrentRequestStr("CustID"); }
        }
        private string RequestAction
        {
            get { return BLL.Util.GetCurrentRequestStr("Action"); }
        }
        #endregion
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string msg = string.Empty;

            if (RequestAction.ToLower().Equals("getcustpidsandbrandids") &&
                !string.IsNullOrEmpty(RequestCustID))
            {
                GetCustPidsAndBrandIDs(RequestCustID, out msg);
            }

            context.Response.Write("{" + msg + "}");
            context.Response.End();
        }

        /// <summary>
        /// 根据客户ID，返回客户所属厂商、品牌ID
        /// </summary>
        /// <param name="RequestCustID"></param>
        /// <param name="msg"></param>
        private void GetCustPidsAndBrandIDs(string CustID, out string msg)
        {
            msg = "'GetInfo':'no'";
            BitAuto.YanFa.Crm2009.Entities.CustInfo cust = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(CustID);
            if (cust != null)
            {
                string pid = cust.Pid;
                string pidName = "";
                string custPid = cust.CustPid; //所属厂商
                string custPidName = "";
                string[] array = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetBrandIDsAndNamesByCustID(CustID);
                string brandIDs = array[0]; //主营品牌IDs
                string brandNames = array[1];//主营品牌Names

                if (!string.IsNullOrEmpty(custPid))
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo custPidModel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(cust.CustPid);
                    if (custPidModel != null)
                    {
                        custPidName = custPidModel.CustName;
                    }
                }
                if (!string.IsNullOrEmpty(pid))
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo pidModel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(pid);
                    if (pidModel != null)
                    {
                        pidName = pidModel.CustName;
                    }
                }
                msg = "'GetInfo':'yes','CustPid':'" + custPid + "','CustPidName':'" + custPidName + "','Pid':'"+pid+"','PidName':'"+pidName+"','BrandIDs':'" + brandIDs + "','BrandNames':'" + brandNames + "'";
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
}