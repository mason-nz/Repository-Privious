using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.WebService
{
    public class CRMCustomerServiceHelper
    {

        //private string CRMQueryServiceURL = System.Configuration.ConfigurationManager.AppSettings["CRMQueryServiceURL"].ToString();//服务URL
        CRMCustomerServiceProxy proxy = new CRMCustomerServiceProxy();

        #region Instance
        public static readonly CRMCustomerServiceHelper Instance = new CRMCustomerServiceHelper();
        #endregion

        /// <summary>
        /// 返回DataTable，共4列，CName：姓名、Phone：手机、Email：邮箱、ContactID：主键ID 
        /// </summary>
        /// <param name="MemberCode">经销商编号</param>
        /// <returns>返回DataSet类表</returns>
        public DataTable GetMemberContactByMemberCode(string MemberCode)
        {
            //return (DataTable)WebServiceHelper.InvokeWebService(CRMQueryServiceURL, "GetMemberContactByMemberCode", new object[] { MemberCode });
            return proxy.GetMemberContactByMemberCode(MemberCode);
        }

        /// <summary>
        /// 更新联系人邮箱
        /// </summary>
        /// <param name="recid">联系人ID</param>
        /// <param name="email">邮箱内容</param>
        /// <returns></returns>
        public bool UpdateContactEmail(int recid, string email, out string msg)
        {
            //object[] args = new object[] { recid, email, string.Empty };
            //bool flag = (bool)WebServiceHelper.InvokeWebService(CRMQueryServiceURL, "UpdateContactEmail", args);
            //msg = args[2].ToString().Trim();
            //return flag;
            return proxy.UpdateContactEmail(recid, email, out msg);
        }
    }

    class CRMCustomerServiceProxy : CRM.CustomerService.CustomerService
    {
        public CRMCustomerServiceProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["CRMCustomerServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["CRMQueryServiceURL"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }
}
