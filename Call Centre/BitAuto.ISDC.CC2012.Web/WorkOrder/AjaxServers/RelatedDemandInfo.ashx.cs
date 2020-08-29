using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder.AjaxServers
{
    /// <summary>
    /// RelatedDemandInfo 的摘要说明
    /// </summary>
    public class RelatedDemandInfo : IHttpHandler, IRequiresSessionState
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string Action
        {
            get
            {
                if (Request["Action"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 品牌ID
        /// </summary>
        public string CarBrandID
        {
            get
            {
                if (Request["carBrandID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["carBrandID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustomID
        {
            get
            {
                if (Request["customID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["customID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;

            GetCarTypesByBrandID(out msg);

            context.Response.Write(msg);
        }
        private void GetCarTypesByBrandID(out string msg)
        {
            msg = string.Empty;

            DataTable dt = BitAuto.YanFa.Crm2009.BLL.YJKDemandBLL.Instance.GetYJKDemandSerials(CustomID, CarBrandID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (i == 0)
                {
                    msg += "[{SerialIDs:'" + dr["SerialIDs"] + "',SerialNames:'" + dr["SerialNames"] + "'}";
                }
                if (i > 0)
                {
                    msg += ",{SerialIDs:'" + dr["SerialIDs"] + "',SerialNames:'" + dr["SerialNames"] + "'}";
                }
                if (i == dt.Rows.Count - 1)
                {
                    msg += "]";
                }
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