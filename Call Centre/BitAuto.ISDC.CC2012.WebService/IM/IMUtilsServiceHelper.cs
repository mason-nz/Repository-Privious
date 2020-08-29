using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.WebService.IM
{
    public class IMUtilsServiceHelper
    {
        private string IMAuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["IMAuthorizeCode"].ToString();
        IMUtilsServiceProxy proxy = new IMUtilsServiceProxy();

        public static readonly IMUtilsServiceHelper Instance = new IMUtilsServiceHelper();

        /// 根据会话ID更新工单ID
        /// <summary>
        /// 根据会话ID更新工单ID
        /// </summary>
        /// <param name="imType">1—会话，2—留言</param>
        /// <param name="id">相关的ID</param>
        /// <param name="orderid">CC系统工单ID</param>
        /// <returns>正常返回空，出错后返回异常信息</returns>
        public string UpdateCCWorkOrderToIM(int imType, int id, string orderid)
        {
            return proxy.UpdateCCWorkOrder2IMHaveKey(imType, id, orderid, IMAuthorizeCode);
        }
    }

    class IMUtilsServiceProxy : IM.UtilsService.Utils
    {
        public IMUtilsServiceProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["IMUtilsServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["IMUtilsServiceURL"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }

}
