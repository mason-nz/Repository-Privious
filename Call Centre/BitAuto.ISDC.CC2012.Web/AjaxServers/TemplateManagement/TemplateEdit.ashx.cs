using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    /// <summary>
    /// TemplateEdit 的摘要说明
    /// </summary>
    public class TemplateEdit : IHttpHandler, IRequiresSessionState
    {
        #region 属性

        private string RequestAction
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString()); }
        }
        private string RequestRecID
        {
            get { return HttpContext.Current.Request["RecID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RecID"].ToString()); }
        }

        #endregion
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            int userID = BLL.Util.GetLoginUserID();
            switch (RequestAction)
            {
                case "Delete":
                    if (!BLL.Util.CheckRight(userID, "SYS024MOD510202"))
                    {
                        msg = "result:'false',msg:'您没有权限执行此操作！'";
                    }
                    else
                    {
                        DeleteTemplate(out msg);
                    }
                    break;
                case "Enable":
                    if (!BLL.Util.CheckRight(userID, "SYS024MOD510206"))
                    {
                        msg = "result:'false',msg:'您没有权限执行此操作！'";
                    }
                    else
                    {
                        EnableTemplate(out msg);
                    }
                    break;
                case "Disable":
                    if (!BLL.Util.CheckRight(userID, "SYS024MOD510207"))
                    {
                        msg = "result:'false',msg:'您没有权限执行此操作！'";
                    }
                    else
                    {
                        DisableTemplate(out msg);
                    }
                    break;

            }

            context.Response.Write("{" + msg + "}");
        }
        //停用
        private void DisableTemplate(out string msg)
        {
            msg = string.Empty;
            int _recID;
            if (!int.TryParse(RequestRecID, out _recID))
            {
                msg = "result:'false',msg:'未找到主键，操作失败！'";
                return;
            }

            try
            {
                Entities.TPage model_tPage = BLL.TPage.Instance.GetTPage(_recID);
                model_tPage.IsUsed = 0;
                BLL.TPage.Instance.Update(model_tPage);
            }
            catch (Exception ex)
            {
                msg = "result:'false',msg:'执行出现异常，操作失败！'";
            }
            msg = "result:'true'";
        }
        //启用
        private void EnableTemplate(out string msg)
        {
            msg = string.Empty;
            int _recID;
            if (!int.TryParse(RequestRecID, out _recID))
            {
                msg = "result:'false',msg:'未找到主键，操作失败！'";
                return;
            }

            try
            {
                Entities.TPage model_tPage = BLL.TPage.Instance.GetTPage(_recID);
                model_tPage.IsUsed = 1;
                BLL.TPage.Instance.Update(model_tPage);
            }
            catch (Exception ex)
            {
                msg = "result:'false',msg:'执行出现异常，操作失败！'";
            }
            msg = "result:'true'";
        }

        private void DeleteTemplate(out string msg)
        {
            msg = string.Empty;
            int _recID;
            if (!int.TryParse(RequestRecID, out _recID))
            {
                msg = "result:'false',msg:'未找到主键，操作失败！'";
                return;
            }

            try
            {
                Entities.TPage model_tPage = BLL.TPage.Instance.GetTPage(_recID);
                BLL.TPage.Instance.Delete(_recID);
                if (!string.IsNullOrEmpty(model_tPage.GenTempletPath))
                {
                    //删除文件
                    string root = BLL.Util.GetUploadWebRoot() + BLL.Util.GetUploadProject(BLL.Util.ProjectTypePath.Template, "\\");
                    string fullname = root + model_tPage.GenTempletPath;
                    File.Delete(fullname);
                }
            }
            catch
            {
                msg = "result:'false',msg:'执行出现异常，操作失败！'";
            }

            msg = "result:'true'";
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