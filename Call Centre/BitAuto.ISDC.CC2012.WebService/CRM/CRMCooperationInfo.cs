using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.WebService.CRM
{
    public class CRMCooperationInfo
    {
        //private string CRMCooperationInfoService = System.Configuration.ConfigurationManager.AppSettings["CRMCooperationInfo"].ToString();//服务URL
        CRMCooperationInfoProxy proxy = new CRMCooperationInfoProxy();

        #region Instance
        public static readonly CRMCooperationInfo Instance = new CRMCooperationInfo();
        #endregion

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="CustID">客户ID</param>
        /// <returns>返回DataSet类表</returns>
        public DataSet GetCooperationByCustID(string CustID)
        {
            string msg = string.Format("【调用CRM接口GetCooperationByCustID({0})】", CustID);
            Loger.Log4Net.Info(msg + "——开始");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            DataSet ds = null;
            try
            {
                ds = proxy.GetCustCoorperation(CustID);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error(ex);
            }
            sw.Stop();
            Loger.Log4Net.Info(msg + "——结束，总耗时：" + sw.ElapsedMilliseconds + "毫秒");
            return ds;
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="memberCodes">会员编号</param>
        /// <returns></returns>
        public DataTable GetMemberCoorperation(string[] memberCodes)
        {
            //return (DataTable)WebServiceHelper.InvokeWebService(CRMCooperationInfoService, "GetMemberCoorperation", new object[] { memberCodes });
            return proxy.GetMemberCoorperation(memberCodes);
        }

    }


    class CRMCooperationInfoProxy : CRM.CooperationService.CustCoorperation
    {
        public CRMCooperationInfoProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["CRMCooperationInfoTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["CRMCooperationInfo"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }
}
