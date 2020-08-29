using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.TemplateCategory
{
    /// <summary>
    /// 通过PID（父级ID）与Type（模板类别：1-短信，2-邮件）
    /// </summary>
    public class getTemplateCategroys : IHttpHandler, IRequiresSessionState
    {
        #region 属性定义
        //模板分类类型
        private string RequstType
        {
            get { return HttpContext.Current.Request["Type"] == null ? "" : HttpContext.Current.Request["Type"].ToString(); }
        }
        //父级ID
        private string RequestPID
        {
            get { return HttpContext.Current.Request["PID"] == null ? "" : HttpContext.Current.Request["PID"].ToString(); }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string msg = string.Empty;
            DataTable dt = new DataTable();
            QueryTemplateCategory queryCustInfo = new QueryTemplateCategory();
            int _pid;
            if (int.TryParse(RequestPID, out _pid))
            {
                queryCustInfo.Pid = _pid;
            }
            int _type;
            if (int.TryParse(RequstType, out _type))
            {
                queryCustInfo.Type = _type;
            }
            queryCustInfo.Status = 1;
            int count;
            dt = BitAuto.ISDC.CC2012.BLL.TemplateCategory.Instance.GetTemplateCategory(queryCustInfo, "", 1, 100, out count);

            if (dt != null && dt.Rows.Count > 0)
            {
                msg += "{root:[";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                msg += "{'RecID':'" + dt.Rows[i]["RecID"] + "',";
                msg += "'Name':'" + dt.Rows[i]["Name"] + "'},";
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                msg = msg.Substring(0, msg.Length - 1);
                msg += "]}";
            }

            context.Response.Write(msg);
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