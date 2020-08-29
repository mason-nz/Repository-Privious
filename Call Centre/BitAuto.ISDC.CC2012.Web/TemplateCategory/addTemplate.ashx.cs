using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

using System.Data;
namespace BitAuto.ISDC.CC2012.Web.TemplateCategory
{
    /// <summary>
    /// addTemplate 的摘要说明
    /// </summary>
    public class addTemplate : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string type = HttpContext.Current.Request["Type"].ToString();
            string userIDs = HttpContext.Current.Request["Persion"].ToString();
            string tcID = HttpContext.Current.Request["TCID"].ToString();
            string title = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Title"].ToString());
            string con = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Con"].ToString());
            string userName = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["PersionName"].ToString());
            Entities.TemplateInfo template = new Entities.TemplateInfo();
            template.TCID = Convert.ToInt32(tcID);
            template.Title = title;
            template.Content = con;
            template.CreateUserID = BLL.Util.GetLoginUserID();
            template.CreateTime = DateTime.Now;
            template.ReplaceTag = "-";
            template.Status = 0;
            //模板查重
            if (BLL.TemplateInfo.Instance.IsExist((int)template.TCID, title))
            {
                context.Response.Write("repeated");
            }
            else
            {
                //模板写入
                int templateID = BLL.TemplateInfo.Instance.Insert(template);
                //收件人写入
                if (type == "2")
                {
                    string[] userIDArr = userIDs.Split(',');
                    string[] userNameArr = userName.Split(',');
                    int length = userIDArr.Length;
                    for (int i = 0; i < length; i++)
                    {
                        Entities.ConsultSolveUserMapping map = new Entities.ConsultSolveUserMapping();
                        map.CreateUserID = BLL.Util.GetLoginUserID();//创建者
                        map.SolveUserEID = Convert.ToInt32(userIDArr[i]);
                        map.TemplateID = templateID;
                        map.SolveUserEName = userNameArr[i];
                        BLL.ConsultSolveUserMapping.Instance.Insert(map);
                    }
                }
                context.Response.Write("success");
                BLL.Util.InsertUserLog("【添加】模板，模板名称：\"" + title + "\",模板内容：" + con + "。" + "操作人：" + BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(BLL.Util.GetLoginUserID()));
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