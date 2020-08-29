using System;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Web.Util;
using BitAuto.ISDC.CC2012.Web.Statistics;
using System.Web.Script.Serialization;

namespace BitAuto.ISDC.CC2012.Web.ReturnVisit.ReturnVisitAssignImport
{
    public class ReturnVisitFile
    {
        public string fileName { get; set; }
        public string filePath { get; set; }
    }
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
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
            string fileName = "";
            string filePath = "";
            try
            {
                //BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                if (UserRightsService.CheckLoginUserIDByCookies(LoginCookiesContent) > 0)
                {
                    ReturnVisitAssignImportHelper helper = new ReturnVisitAssignImportHelper();
                    switch (helper.Action)
                    {
                        case "batchimport":
                            string msg = "";
                            JavaScriptSerializer jsserializer = new JavaScriptSerializer();

                            AJAXHelper.WrapJsonResponse(helper.BatchImport(out msg, out fileName, out filePath), msg, jsserializer.Serialize(new ReturnVisitFile() { fileName = fileName, filePath = filePath }));
                            break;
                        case "updatereturnvisit":
                            fileName = context.Request["fileName"];
                            filePath = context.Request["filePath"];
                            string updateflag = context.Request["updateflag"];
                            bool flag = helper.UpdateImport(fileName, filePath, updateflag);
                            AJAXHelper.WrapJsonResponse(flag, "", "");
                            break;
                        default:
                            AJAXHelper.WrapJsonResponse(false, "", "没有对应的操作");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                AJAXHelper.WrapJsonResponse(false, "分配失败！", ex.Message);
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