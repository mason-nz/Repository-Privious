using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    /// <summary>
    /// ProjectHandler 的摘要说明
    /// </summary>
    public class ProjectHandler : IHttpHandler, IRequiresSessionState
    {
        #region 属性

        public string Action
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryStr("Action");
            }
        }

        public string ProjectName
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryStr("ProjectName");
            }
        }

        public int BGID
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryInt("BGID");
            }
        }

        /// <summary>
        /// 分类ID
        /// </summary>
        public int SCID
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryInt("SCID");
            }
        }

        /// <summary>
        /// 项目类型ID
        /// </summary>
        public int PCatageID
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryInt("PCatageID");
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;

            if (Action.ToLower().Equals("getproejctnamebyautocomplete")
                && !string.IsNullOrEmpty(ProjectName))//项目名称自动查询匹配逻辑
            {
                msg = GetProjectName(ProjectName);
            }
            context.Response.Write(msg);
        }

        private string GetProjectName(string projectName)
        {
            string msg = string.Empty;
            DataTable dt = BLL.ProjectInfo.Instance.GetProjectNames(projectName, BGID, SCID, PCatageID,BLL.Util.GetLoginUserID());
            if (dt!=null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        msg += "{[";
                    }
                    msg += "{'ProjectID':'" + dt.Rows[i]["ProjectID"] + "','ProjectName':'" + dt.Rows[i]["Name"] + "'},";
                    if (i == dt.Rows.Count - 1)
                    {
                        msg = msg.TrimEnd(',') + "]}";
                    }
                }
            }
            //else
            //{
            //    msg = "{['ProjectID':'-1','ProjectName':'']}";
            //}
            return msg;
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