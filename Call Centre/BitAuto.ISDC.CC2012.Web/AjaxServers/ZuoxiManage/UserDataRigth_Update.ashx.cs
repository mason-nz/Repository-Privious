using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage
{
    /// <summary>
    /// UserDataRigth_Update 的摘要说明
    /// </summary>
    public class UserDataRigth_Update : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/plain";
            try
            {
                BitAuto.ISDC.CC2012.BLL.EmployeeSuper.Instance.UserDataRight_Update(
                    HttpContext.Current.Request["userID"].ToString(),
                    HttpContext.Current.Request["creatUserID"].ToString()
                    );
                //写入日志
                BLL.Util.InsertUserLog("【修改】用户数据权限，用户ID：" 
                    + HttpContext.Current.Request["userID"].ToString() 
                    + "。操作人ID：" + BLL.Util.GetLoginUserID().ToString());

                context.Response.Write("true");
            }
            catch
            {
                context.Response.Write("false");
 
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void test(HttpContext con)
        {
            
        }
    }
}