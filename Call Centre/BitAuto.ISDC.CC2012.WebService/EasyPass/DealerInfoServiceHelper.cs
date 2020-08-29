using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.WebService
{
    public class DealerInfoServiceHelper
    {
        DealerInfoServiceProxy proxy = new DealerInfoServiceProxy();
        //private string ServiceURL = System.Configuration.ConfigurationManager.AppSettings["DealerInfoServiceURL"].ToString();//服务URL

        #region Instance
        public static readonly DealerInfoServiceHelper Instance = new DealerInfoServiceHelper();
        #endregion

        /// <summary>
        /// 返回行记录有3个字段 DealerId 经销商id 、[ServiceTypeID] 服务类型id 1 为400大号、2为400小号 、 Dealer400 为400号码
        /// </summary>
        /// <param name="MemberCode">经销商编号</param>
        /// <returns>返回DataSet类表</returns>
        public DataSet GetDealer400(int MemberCode)
        {
            //return (DataSet)WebServiceHelper.InvokeWebService(ServiceURL, "GetDealer400", new object[] { MemberCode });
            return proxy.GetDealer400(MemberCode);
        }

        /// <summary>
        /// 修改经销商信息（允许修改的经销商类型：导入、免费未付费的！)。注：电话号码必须按照以下格式：固话：010-68492345 010-68492345-8217 手机：直接输入13245687921 特殊号码：4000689960
        /// </summary>
        /// <param name="DealerID">经销商ID</param>
        /// <param name="ContactAddress">联系地址</param>
        /// <param name="PostCode">邮编</param>
        /// <param name="SalesPhones">销售电话</param>
        /// <param name="FaxNumbers">传真</param>
        /// <param name="WebSiteUrl">站点URL</param>
        /// <param name="EmailAddress">Email</param>
        /// <returns></returns>
        public int UpdateDealerInfo(int DealerID, string ContactAddress, string PostCode, string SalesPhones, string FaxNumbers, string WebSiteUrl, string EmailAddress)
        {
            //return (int)WebServiceHelper.InvokeWebService(ServiceURL, "UpdateDealerInfo", new object[] { DealerID, ContactAddress, PostCode, SalesPhones, FaxNumbers, WebSiteUrl, EmailAddress });
            return proxy.UpdateDealerInfo(DealerID, ContactAddress, PostCode, SalesPhones, FaxNumbers, WebSiteUrl, EmailAddress);
        }
    }

    class DealerInfoServiceProxy :EasyPass.DealerInfoService.DealerInfoService
    {
        public DealerInfoServiceProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["DealerInfoServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["DealerInfoServiceURL"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }
}
