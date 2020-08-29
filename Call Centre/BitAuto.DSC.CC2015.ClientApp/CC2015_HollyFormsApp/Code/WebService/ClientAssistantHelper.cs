using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CC2015_HollyFormsApp.ClientAssistantService;
using System.Data;
using System.Web;
using System.Net;

namespace CC2015_HollyFormsApp
{
    public class ClientAssistantHelper
    {
        public static readonly ClientAssistantHelper Instance = new ClientAssistantHelper();
        private ClientAssistantService.ClientAssistantServiceSoapClient server = null;
        private static readonly string key = "yiche-ClineLog-!@#$#@!";

        protected ClientAssistantHelper()
        {
            server = new ClientAssistantService.ClientAssistantServiceSoapClient();
        }

        /// 上传日志
        /// <summary>
        /// 上传日志
        /// </summary>
        /// <param name="data"></param>
        /// <param name="date"></param>
        /// <param name="userid"></param>
        /// <param name="vender"></param>
        /// <returns></returns>
        public bool PushClientLogForAgent(byte[] data, DateTime date, int userid)
        {
            return server.PushClientLogForAgent(key, data, date, userid, Vender.Holly);
        }
        /// 获取服务器最新版本号
        /// <summary>
        /// 获取服务器最新版本号
        /// </summary>
        /// <param name="ServerVersionsName"></param>
        /// <returns></returns>
        public string GetClientServerVersion(string ServerVersionsName)
        {
            return server.GetClientServerVersion(key, ServerVersionsName);
        }
        /// 获取需要处理的请求
        /// <summary>
        /// 获取需要处理的请求
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetClientLogRequireInfo(int userid)
        {
            return server.GetClientLogRequireInfo(key, userid, Vender.Holly);
        }
        /// 获取当前的任务ID
        /// <summary>
        /// 获取当前的任务ID
        /// </summary>
        /// <param name="beijiao"></param>
        /// <returns></returns>
        public string GetCurrentTaskID(string beijiao)
        {
            try
            {
                string GetCurrentTaskIDURL = BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToString(System.Configuration.ConfigurationManager.AppSettings["GetCurrentTaskIDURL"]);
                string cckey = HttpUtility.UrlEncode(key);
                string url = string.Format(GetCurrentTaskIDURL, beijiao, cckey);
                HttpWebResponse rep = BitAuto.ISDC.CC2012.BLL.HttpHelper.CreateGetHttpResponse(url);
                string taskid = BitAuto.ISDC.CC2012.BLL.HttpHelper.GetResponseString(rep);
                Loger.Log4Net.Info("[接口] [GetCurrentTaskID] 获取任务ID " + taskid);
                return taskid;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[接口] [GetCurrentTaskID] 异常 ", ex);
                return "";
            }
        }
    }
}
