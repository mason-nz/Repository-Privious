using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.WebService;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    /// <summary>
    /// SMSSend1 的摘要说明
    /// </summary>
    public class SMSSend1 : IHttpHandler, IRequiresSessionState
    {


        public string Tels
        {
            get
            {
                return HttpContext.Current.Request["Tels"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["Tels"].ToString());
            }
        }

        public string SendContent
        {
            get
            {
                return HttpContext.Current.Request["SendContent"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["SendContent"].ToString());
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string msg = "";

            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            Send(out msg);

            if (msg == "")
            {
                msg = "success";
            }

            context.Response.Write(msg);
        }

        private void Send(out string msg)
        {
            msg = "";
            string[] telArry = Tels.Split(',');
            if (telArry.Length == 0)
            {
                msg = "电话号码不能为空";
                return;
            }
            if (telArry.Length > 2)
            {
                msg = "电话号码不能超过3个";
                return;
            }
            //Regex reTel = new Regex(@"(^13[0-9]{9}$)|(^15[0-9]{9}$)|(^17[0-9]{9}$)|(^18[0-9]{9}$)|(^14[0-9]{9}$)");
            foreach (string str in telArry)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (!BLL.Util.IsHandset(str))
                    {
                        msg = "电话(" + str + ")不符合规则";
                        return;
                    }

                    #region 记录发送日志

                    Entities.SendSMSLog smsLogModel = new Entities.SendSMSLog();
                    smsLogModel.CreateUserID = BLL.Util.GetLoginUserID();
                    smsLogModel.CustID = "无主订单";
                    smsLogModel.Mobile = str;
                    smsLogModel.SendContent = SendContent;
                    smsLogModel.SendTime = DateTime.Now;
                    smsLogModel.TemplateID = -1;
                    BLL.SendSMSLog.Instance.Insert(smsLogModel);

                    #endregion

                    //发送短信
                    //com.bitauto.mobile.MsgService msgService = new com.bitauto.mobile.MsgService();
                    //string md5 = msgService.MixMd5("6116" + str + SendContent + "Ytt1TEy3hnYIgqTOOIGEc0MFL9wrN0yJJuUUPVfjyM+dkY3ei/8WUc8L7qFqgCbp");
                    //int msgid = msgService.SendMsgImmediately("6116", str, SendContent, "", DateTime.Now.AddHours(1), md5);
                    string md5 = SMSServiceHelper.Instance.MixMd5(str, SendContent);
                    int msgid = SMSServiceHelper.Instance.SendMsgImmediately(str, SendContent, DateTime.Now.AddHours(1), md5);
                    if (msgid > 0)
                    {
                        //插入发送短信记录                      
                        BLL.Util.InsertUserLog("给手机（" + str + "）发送短信成功,内容：【" + SendContent + "】");
                    }
                    else
                    {
                        msg = BLL.Util.GetEnumOptText(typeof(Entities.SendSMSInfo), msgid);
                        BLL.Util.InsertUserLog("给手机（" + str + "）发送短信失败【错误信息：" + msg + "】");
                    }

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