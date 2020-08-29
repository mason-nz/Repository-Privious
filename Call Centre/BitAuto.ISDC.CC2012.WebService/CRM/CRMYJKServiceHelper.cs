using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.WebService.CRM
{
    public class CRMYJKServiceHelper
    {

        #region Instance
        public static readonly CRMYJKServiceHelper Instance = new CRMYJKServiceHelper();
        #endregion

        private YiJiKe.ServiceReference.YJKDemandServiceSoapClient c = new YiJiKe.ServiceReference.YJKDemandServiceSoapClient();
        private YiJiKe.ServiceReference.YJKDemandSoapHeader SoapHeader =
            new YiJiKe.ServiceReference.YJKDemandSoapHeader { 
                //SecretKey = System.Configuration.ConfigurationManager.AppSettings["CRMYiJiKeWebServiceKey"].ToString()
                SecretKey = "bitauto.yhn123(*&"
            };

        /// <summary>
        /// 新增leads
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public bool AddLeadsInfo(string xmlstr)
        {
            return c.AddLeadsInfo(SoapHeader, xmlstr);
            //return (bool)WebServiceHelper.InvokeWebService(CRMYJKServiceURL, "AddLeadsInfo", new object[] { xmlstr });
            //return true;
        }

        /// <summary>
        /// CRM根据竞品补充规则生成外呼任务后通知CC，CC通过此接口获取任务明细。
        /// </summary>
        /// <param name="DemandID"></param>
        /// <param name="BathNO"></param>
        /// <returns></returns>
        public DataTable GetDemandCallDetail(string DemandID, int BathNO)
        {
            //通过接口获取数据
            var arrTest = c.GetDemandCallLeads(SoapHeader, DemandID, BathNO);

            if (arrTest != null)
            {
                var dsTest = BLL.Util.RetrieveDataSet(arrTest);
                return dsTest.Tables[0];
            }
            else
            {
                return null;
            }
        }

    }
}
