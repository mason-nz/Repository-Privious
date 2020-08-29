using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.WebService.IM
{
    public class IMtcUtilsServiceHelper
    {
        private string IMEPAndIMTCAuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["IMEPAndIMTCAuthorizeCode"].ToString();//IMep和IMtc统一验证码
        IMtcUtilsServiceProxy proxy = new IMtcUtilsServiceProxy();

        public static readonly IMtcUtilsServiceHelper Instance = new IMtcUtilsServiceHelper();

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
            return proxy.UpdateCCWorkOrder2IMHaveKey(imType, id, orderid, IMEPAndIMTCAuthorizeCode);
        }
    }

    class IMtcUtilsServiceProxy : IMtc.UtilsService.Utils
    {
        public IMtcUtilsServiceProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["IMtcUtilsServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["IMtcUtilsServiceURL"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }

}
