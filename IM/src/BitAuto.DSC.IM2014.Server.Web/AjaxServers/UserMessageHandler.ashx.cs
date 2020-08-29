using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.DSC.IM2014.Core;
using BitAuto.DSC.IM2014.Server.Web.Channels;

namespace BitAuto.DSC.IM2014.Server.Web.AjaxServers
{
    /// <summary>
    /// UserMessageHandler 的摘要说明
    /// </summary>
    public class UserMessageHandler : IHttpHandler
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string Action { get { return (HttpContext.Current.Request["Action"] + "").Trim().ToLower(); } }
        /// <summary>
        ///  留言内容
        /// </summary>
        public string UserMessage
        {
            get
            {
                if (HttpContext.Current.Request["UserMessage"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["UserMessage"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        ///  网友id
        /// </summary>
        public string UserMessageIMID
        {
            get
            {
                if (HttpContext.Current.Request["UserMessageIMID"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["UserMessageIMID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string LeaveMessageType
        {
            get
            {
                if (HttpContext.Current.Request["LeaveMessageType"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["LeaveMessageType"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string Email
        {
            get
            {
                if (HttpContext.Current.Request["Email"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Email"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string LeaveUserName
        {
            get
            {
                if (HttpContext.Current.Request["LeaveUserName"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["LeaveUserName"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string LeavePhone
        {
            get
            {
                if (HttpContext.Current.Request["LeavePhone"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["LeavePhone"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            string msg = string.Empty;
            context.Response.ContentType = "text/plain";
            switch (Action)
            {
                case "addusermessage"://保存留言
                    SaveUserMessage(out msg);
                    break;
                default:
                    msg = "{'success':'no','recordid':'请求参数错误！'}";
                    break;
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
        private void SaveUserMessage(out string msg)
        {
            msg = string.Empty;
            int _LeaveMessageType = 0;
            if (string.IsNullOrEmpty(UserMessage))
            {
                msg = "{'success':'no','result':'留言不能为空！'}";
            }
            else if (!string.IsNullOrEmpty(UserMessage) && BLL.Util.GetLength(UserMessage) > 2000)
            {
                msg = "{'success':'no','result':'留言超长！'}";
            }
            else if (string.IsNullOrEmpty(UserMessageIMID))
            {
                msg = "{'success':'no','result':'网友标识不能为空！'}";
            }
            else if (string.IsNullOrEmpty(LeaveMessageType))
            {
                msg = "{'success':'no','result':'留言类型不能为空！'}";
            }
            else if ((!string.IsNullOrEmpty(LeaveMessageType)) && (!int.TryParse(LeaveMessageType, out _LeaveMessageType)))
            {
                msg = "{'success':'no','result':'留言类型格式不正确！'}";
            }
            else if (string.IsNullOrEmpty(Email))
            {
                msg = "{'success':'no','result':'邮箱不能为空！'}";
            }
            else if (!BLL.Util.IsEmail(Email))
            {
                msg = "{'success':'no','result':'邮箱格式不正确！'}";
            }
            else if ((!string.IsNullOrEmpty(LeavePhone)) && (!BLL.Util.IsHandset(LeavePhone)))
            {

                msg = "{'success':'no','result':'手机格式不正确！'}";
            }
            else if ((!string.IsNullOrEmpty(LeaveUserName)) && (BLL.Util.GetLength(LeaveUserName) > 200))
            {
                msg = "{'success':'no','result':'姓名超长！'}";
            }
            else
            {
                //保存留言
                Entities.UserMessage model = new Entities.UserMessage();
                model.Content = UserMessage;
                model.CreateTime = System.DateTime.Now;
                model.UserID = UserMessageIMID;
                model.TypeID = _LeaveMessageType;
                model.Email = Email;
                model.Pheone = LeavePhone;
                model.UserName = LeaveUserName;
                BLL.UserMessage.Instance.Insert(model);
                msg = "{'success':'yes','result':'您的留言已提交成功，我们会尽快回复！'}";
            }
        }
    }
}