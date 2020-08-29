using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    /// <summary>
    /// Summary description for TagManagement
    /// </summary>
    public class TagManagement : IHttpHandler, IRequiresSessionState
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        #region 定义属性
        public string OrderID
        {
            get { return Request.QueryString["OrderID"] == null ? string.Empty : Request.QueryString["OrderID"].ToString().Trim(); }
        }

        public int userID {
            get { return BLL.Util.GetLoginUserID(); }
        }
        #endregion
        bool success = true;
        string result = "";
        string message = "";
        public HttpContext currentContext;
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            currentContext = context;
            if ((context.Request["TagsSelectAll"] + "").Trim() == "yes")
            {
                //取当前登录人的业务组
                if (userID != -1)
                {
                    DataTable dt = BLL.BusinessGroup.Instance.GetBusinessGroupTagsByUserID(userID);

                    if (dt.Rows.Count > 0)
                    {
                        //在已选标签中遍历
                        //判断标签是否在已选择标签里，是则isSelected=true
                        StringBuilder sb = new StringBuilder();

                        foreach (DataRow row in dt.Rows)
                        {
                            sb.Append("{'BGID':'" + row["BGID"].ToString() + "','GroupName':'" + row["GroupName"].ToString() + "','TagID':'" + row["TagID"].ToString() +
                                        "','TagName':'" + row["TagName"].ToString() + "','IsUsed':'" + row["IsUsed"].ToString() + "'},");
                        }

                        message = sb.ToString();
                        if (message.EndsWith(","))
                            message = message.Substring(0, message.Length - 1);
                    }

                
                }               
                
                
                context.Response.Write("[" + message + "]");
                context.Response.End();
            }            
            else
            {
                success = false;
                message = "request error";
                BitAuto.ISDC.CC2012.BLL.AJAXHelper.WrapJsonResponse(success, result, message);
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