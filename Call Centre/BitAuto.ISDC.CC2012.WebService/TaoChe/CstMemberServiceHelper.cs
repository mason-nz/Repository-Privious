using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

namespace BitAuto.ISDC.CC2012.WebService
{
    public class CstMemberServiceHelper
    {
        CstMemberServiceProxy proxy = new CstMemberServiceProxy();
        //private string CstMemberServiceHandlerUrl = System.Configuration.ConfigurationManager.AppSettings["CstMemberServiceHandlerUrl"].ToString();//服务URL

        #region Instance
        public static readonly CstMemberServiceHelper Instance = new CstMemberServiceHelper();
        #endregion


        /// <summary>
        /// 获取上级公司信息
        /// </summary>
        /// <returns>返回DataTable</returns>
        public DataTable GetSuperVendor(out string msg)
        {
            DataTable dt = null; msg = string.Empty;
            try
            {
                dt = (DataTable)HttpRuntime.Cache.Get("CstMemberService_GetSuperVendor");
                if (dt == null)
                {
                    //object[] args = new object[] { string.Empty };
                    //DataTable flag = (DataTable)WebServiceHelper.InvokeWebService(CstMemberServiceHandlerUrl, "GetSuperVendor", args);
                    //msg = args[0].ToString().Trim();
                    DataTable flag = proxy.GetSuperVendor(out msg);
                    HttpRuntime.Cache.Insert("CstMemberService_GetSuperVendor", flag == null ? new DataTable() : flag, null, DateTime.Now.AddDays(1), TimeSpan.Zero);
                    dt = (DataTable)HttpRuntime.Cache.Get("CstMemberService_GetSuperVendor");
                }
            }
            catch (Exception e)
            {
                dt = null;
                msg = "调用【获取上级公司信息】接口出错" + e.Message + e.StackTrace;
            }
            return dt;
        }
    }

    class CstMemberServiceProxy :TaoChe.CstMemberService.TranstarToBitautoCRM
    {
        public CstMemberServiceProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["CstMemberServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["CstMemberServiceHandlerUrl"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }
}
