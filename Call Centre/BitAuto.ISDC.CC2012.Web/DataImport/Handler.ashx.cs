using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Web.Util;
namespace BitAuto.ISDC.CC2012.Web.DataImport
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        public string RequesetUserID
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["UserID"]))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Request["UserID"].ToString();
                }
            }
        }


        /// <summary>
        /// 集中权限系统登录是的cookies的内容
        /// </summary>
        public string LoginCookiesContent
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("LoginCookiesContent");
            }
        }
		   

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (UserRightsService.CheckLoginUserIDByCookies(LoginCookiesContent) > 0)
                {
                    int UserID = Convert.ToInt32(RequesetUserID);
                    //int UserID = 6074;
                    if (UserID >= 0)
                    {
                        DataImportHelper helper = new DataImportHelper();
                        //UploadDeal helper = new UploadDeal();
                        string msg = string.Empty;
                        switch (helper.Action)
                        {
                            case "batchimport":
                                helper.BatchImport(out msg, UserID);
                                break;
                            default:
                                msg = "没有对应的操作";
                                break;
                        }
                        context.Response.Write(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                string Message = BLL.Util.EscapeString(ex.Message);
                string msgstr = string.Empty;
                msgstr += "{root:[";
                msgstr += "{'information':'" + Message + "'},";
                msgstr = msgstr.Substring(0, msgstr.Length - 1);
                msgstr += "],ExportData:[";
                msgstr += "{'information':''}],success:[{'information':'此次所有导入数据均失败'}]}";

                context.Response.Write(msgstr);
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