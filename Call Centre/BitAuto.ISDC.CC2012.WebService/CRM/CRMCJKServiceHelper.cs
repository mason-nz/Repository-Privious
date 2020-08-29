using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace BitAuto.ISDC.CC2012.WebService.CRM
{
    public class CRMCJKServiceHelper
    {
        #region Instance
        public static readonly CRMCJKServiceHelper Instance = new CRMCJKServiceHelper();
        #endregion


        private CJiKe.ServiceReference.CJKDemandServiceSoapClient c = new CJiKe.ServiceReference.CJKDemandServiceSoapClient();
        private CJiKe.ServiceReference.CJKDemandSoapHeader SoapHeader =
            new CJiKe.ServiceReference.CJKDemandSoapHeader
            {
                SecretKey = System.Configuration.ConfigurationManager.AppSettings["CRMCJKWebServiceKey"].ToString()

                //SecretKey = "bitauto.yhn123(*&"
            };
        /// <summary>
        /// 回写接口
        /// </summary>
        /// <param name="RecID"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        public bool ReCallLeadsInfo(int RecID, int isValid)
        {
            BackgroundWorker work = new BackgroundWorker();
            work.DoWork += new DoWorkEventHandler((s, e) =>
            {
                try
                {
                    c.ReCallLeadsInfo(SoapHeader, RecID, isValid);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("调用CRM回写接口(ReCallLeadsInfo)时出现异常，传递参数（RecID=" + RecID + "，isValid=" + isValid + "），异常信息（" + ex.Message + ")");
                }
            });

            work.RunWorkerAsync();
            return true;
        }
        /// <summary>
        /// 新增leads接口
        /// </summary>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public bool AddLeadsInfo(string strXml)
        {
            BackgroundWorker work = new BackgroundWorker();
            work.DoWork += new DoWorkEventHandler((s, e) =>
            {
                try
                {
                    c.AddLeadsInfo(SoapHeader, strXml);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("调用新增leads接口(AddLeadsInfo)时出现异常，传递参数（strXml=" + strXml + "），异常信息（" + ex.Message + ")");
                }
            });

            work.RunWorkerAsync();
            return true;

        }
        /// <summary>
        /// 通知crm cc 已结束某个需求单某个批次
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="batch"></param>
        /// <returns></returns>
        public bool NoticeBatchEnd(string orderCode, int batch)
        {
            bool flag = false;
            flag = c.NoticeBatchEnd(SoapHeader, orderCode, batch);
            return flag;
        }
        /// <summary>
        /// 获取线索数据
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
