using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    /// <summary>
    /// SMSTemplateEdit 的摘要说明
    /// </summary>
    public class SMSTemplateEdit : IHttpHandler, IRequiresSessionState
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
        private string RequestBGID
        {
            get { return HttpContext.Current.Request["BGID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BGID"].ToString()); }
        }
        private string RequestSCID
        {
            get { return HttpContext.Current.Request["SCID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SCID"].ToString()); }
        }
        private string RequestSMSTitle
        {
            get { return HttpContext.Current.Request["SMSTitle"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SMSTitle"].ToString()); }
        }
        private string RequestContent
        {
            get { return HttpContext.Current.Request["Content"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Content"].ToString()); }
        }
        private int userid;
        #endregion
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            userid = BLL.Util.GetLoginUserID();
            switch (RequestAction)
            {
                case "Delete":
                    if (!BLL.Util.CheckRight(userid, "SYS024MOD510403"))
                    {
                        msg = "result:'false',msg:'你没有权限执行此操作！'";
                    }
                    else
                    {
                        DeleteTemplate(out msg);
                    }
                    break;
                case "Edit":
                    EditTemplate(out msg);
                    break;
                case "ADD":
                    EditTemplate(out msg);
                    break;
            }

            context.Response.Write("{" + msg + "}");
        }
        private bool IsCheck(out string msg)
        {
            msg = string.Empty;
            bool flag = true;
            if (string.IsNullOrEmpty(RequestBGID) || RequestBGID == "-1")
            {
                flag = false;
                msg = "result:'false',msg:'分组不能为空！'";
                return flag;
            }
            if (string.IsNullOrEmpty(RequestSCID) || RequestSCID == "-1")
            {
                flag = false;
                msg = "result:'false',msg:'分类不能为空！'";
                return flag;
            }
            if (string.IsNullOrEmpty(RequestSMSTitle))
            {
                flag = false;
                msg = "result:'false',msg:'标题不能为空！'";
                return flag;
            }
            if (RequestSMSTitle.Length > 50)
            {
                flag = false;
                msg = "result:'false',msg:'标题超长！'";
                return flag;
            }
            if (string.IsNullOrEmpty(RequestContent))
            {
                flag = false;
                msg = "result:'false',msg:'内容不能为空！'";
                return flag;
            }
            if (RequestContent.Length > 250)
            {
                flag = false;
                msg = "result:'false',msg:'内容超长！'";
                return flag;
            }
            return flag;

        }
        //新增，编辑
        private void EditTemplate(out string msg)
        {
            msg = string.Empty;
            bool flag = false;
            flag = IsCheck(out msg);

            if (flag)
            {
                Entities.SMSTemplate templatemodel = new Entities.SMSTemplate();
                templatemodel.Content = RequestContent;
                int _bgid = 0;
                int.TryParse(RequestBGID, out _bgid);
                templatemodel.BGID = _bgid;
                int _scid = 0;
                int.TryParse(RequestSCID, out _scid);
                templatemodel.SCID = _scid;
                templatemodel.Title = RequestSMSTitle;
                templatemodel.Content = RequestContent;
                templatemodel.Status = 0;
                if (string.IsNullOrEmpty(RequestRecID))
                {
                    try
                    {
                        templatemodel.CreateTime = System.DateTime.Now;
                        templatemodel.CreateUserID = userid;
                        BLL.SMSTemplate.Instance.Insert(templatemodel);

                        string loginfo = "新增短信模板，模板分组为 " + templatemodel.BGID + ",模板分类为 " + templatemodel.SCID + ",标题为 " + templatemodel.Title + ",内容为 " + templatemodel.Content;
                        BLL.Util.InsertUserLog(loginfo);
                    }
                    catch (Exception ex)
                    {
                        msg = "result:'false',msg:'执行出现异常，操作失败！'";
                        return;
                    }
                }
                else
                {
                    int _recID;
                    if (!int.TryParse(RequestRecID, out _recID))
                    {
                        msg = "result:'false',msg:'未找到主键，操作失败！'";
                        return;
                    }
                    templatemodel.RecID = _recID;
                    try
                    {
                        Entities.SMSTemplate oldmodel = BLL.SMSTemplate.Instance.GetSMSTemplate(_recID);
                        templatemodel.CreateUserID = oldmodel.CreateUserID;
                        templatemodel.CreateTime = oldmodel.CreateTime;

                        BLL.SMSTemplate.Instance.Update(templatemodel);
                        string loginfo = "修改短信模板，短信模板主键为 " + templatemodel.RecID + "，修改后模板分组为 " + templatemodel.BGID + ",模板分类为 " + templatemodel.SCID + ",标题为 " + templatemodel.Title + ",内容为 " + templatemodel.Content;
                        BLL.Util.InsertUserLog(loginfo);
                    }
                    catch (Exception ex)
                    {
                        msg = "result:'false',msg:'执行出现异常，操作失败！'";
                        return;
                    }
                }
                msg = "result:'true'";
            }
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
                BLL.SMSTemplate.Instance.Delete(_recID);
                string loginfo = "删除短信模板，短信模板主键为 " + _recID;
                BLL.Util.InsertUserLog(loginfo);
            }
            catch (Exception ex)
            {
                msg = "result:'false',msg:'执行出现异常，操作失败！'";
                return;
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