using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.DSC.IM_2015.BLL;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.SeniorManage
{

    /// <summary>
    /// BussinessConfigDeal 的摘要说明
    /// </summary>
    public class BussinessConfigDeal : IHttpHandler, IRequiresSessionState
    {
        #region 属性
        /// 操作方式
        /// <summary>
        /// 操作方式
        /// </summary>
        public string Action { get { return BLL.Util.GetCurrentRequestFormStr("Action"); } }
        /// 服务时间
        /// <summary>
        /// 服务时间
        /// </summary>
        public string ServerTimeStr { get { return BLL.Util.GetCurrentRequestFormStr("ServerTimeStr"); } }

        /// 更多url
        /// <summary>
        /// 更多url
        /// </summary>
        public string MoreURLStr { get { return BLL.Util.GetCurrentRequestFormStr("MoreURLStr"); } }
        #endregion
        private bool SaveTime(out string msg)
        {
            msg = string.Empty;
            List<TimeModelClss> Info = (List<TimeModelClss>)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(ServerTimeStr, typeof(List<TimeModelClss>));
            if (Info != null && Info.Count() > 0)
            {
                for (int i = 0; i < Info.Count; i++)
                {
                    Info[i].ET = HttpUtility.UrlDecode(Info[i].ET);
                    Info[i].ST = HttpUtility.UrlDecode(Info[i].ST);
                    Info[i].SourceType = HttpUtility.UrlDecode(Info[i].SourceType);
                    Info[i].SourceTypeName = HttpUtility.UrlDecode(Info[i].SourceTypeName);
                }

                BLL.BaseData.Instance.SaveTime(Info);
            }
            return true;
        }
        private bool SaveMoreURL(out string msg)
        {
            msg = string.Empty;
            List<MoreURlModelClss> Info = (List<MoreURlModelClss>)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(MoreURLStr, typeof(List<MoreURlModelClss>));
            if (Info != null && Info.Count() > 0)
            {
                for (int i = 0; i < Info.Count; i++)
                {
                    Info[i].MoreURL = HttpUtility.UrlDecode(Info[i].MoreURL);
                    Info[i].SourceType = HttpUtility.UrlDecode(Info[i].SourceType);
                    Info[i].SourceTypeName = HttpUtility.UrlDecode(Info[i].SourceTypeName);
                }

                BLL.BaseData.Instance.SaveMoreURL(Info);
            }
            return true;
        }
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            bool success = false;
            string msg = "";
            switch (Action.ToLower())
            {
                case "savetime":
                    success = SaveTime(out msg);
                    break;
                case "savemoreurl":
                    success = SaveMoreURL(out msg);
                    break;
            }
            if (success)
            {
                context.Response.Write("{'result':'success'}");
            }
            else
            {
                context.Response.Write("{'result':'error','msg':'" + msg + "'}");
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