using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Web.SessionState;
namespace BitAuto.ISDC.CC2012.Web.TemplateCategory
{
    /// <summary>
    /// updateTemplate 的摘要说明
    /// </summary>
    public class updateTemplate : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/plain";
            string recID = HttpContext.Current.Request["RecID"].ToString();
            string type = HttpContext.Current.Request["Type"].ToString();
            string userIDs = HttpContext.Current.Request["Persion"].ToString();
            string tcID = HttpContext.Current.Request["TCID"].ToString();
            string title = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Title"].ToString());
            string con = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Con"].ToString());
            string userName = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["PersionName"].ToString());
            Entities.TemplateInfo template = new Entities.TemplateInfo();
            template.RecID = Convert.ToInt32(recID);
            template.TCID = Convert.ToInt32(tcID);
            template.Title = title;
            template.Content = con;
            template.CreateUserID = 100;
            template.CreateTime = DateTime.Now;
            template.ReplaceTag = "-";
            template.Status = 0;

            if (BLL.TemplateInfo.Instance.IsExistNotThisRecID(int.Parse(recID), (int)template.TCID, template.Title))
            {
                context.Response.Write("repeated");
            }
            else
            {
                DataTable dt = new DataTable();
                Entities.QueryTemplateInfo query = new Entities.QueryTemplateInfo();
                query.TCID = template.TCID;
                int totle = 0;
                dt = BLL.TemplateInfo.Instance.GetTemplateInfo(query, "", 1, 10, out totle);
                Entities.TemplateInfo templateOri = BLL.TemplateInfo.Instance.GetTemplateInfo(template.RecID);
                #region 邮件接收人
                DataTable dtEmailServersOri = BLL.TemplateInfo.Instance.getEmailServers(template.RecID);
                string strEmailServersOri = "";
                if (dtEmailServersOri != null && dt.Rows.Count > 0)
                {
                    int intVal = 0;
                    foreach (DataRow drEmailServers in dtEmailServersOri.Rows)
                    {
                        if (int.TryParse(drEmailServers["UserID"].ToString(), out intVal))
                        {
                            strEmailServersOri += BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(intVal) + ",";
                        }
                    }
                    if (strEmailServersOri.ToString().Length > 0)
                    {
                        strEmailServersOri = strEmailServersOri.ToString().Substring(0, strEmailServersOri.ToString().Length - 1);
                    }
                }
                #endregion
                //模板写入
                int templateID = BLL.TemplateInfo.Instance.Update(template);

                //收件人写入
                if (type == "2")
                {
                    //清空原有
                    BLL.TemplateInfo.Instance.ClearUser(template.RecID);
                    string[] userIDArr = userIDs.Split(',');
                    string[] userNameArr = userName.Split(',');
                    int length = userIDArr.Length;
                    for (int i = 0; i < length; i++)
                    {
                        Entities.ConsultSolveUserMapping map = new Entities.ConsultSolveUserMapping();
                        map.CreateUserID = BLL.Util.GetLoginUserID();//创建者
                        map.SolveUserEID = Convert.ToInt32(userIDArr[i]);
                        map.TemplateID = template.RecID;
                        map.SolveUserEName = userNameArr[i];
                        BLL.ConsultSolveUserMapping.Instance.Insert(map);
                    }
                }
                context.Response.Write("success");

                string userLog = "【修改】模板。";
                if (template.Title != templateOri.Title)
                {
                    userLog += "模板标题“" + templateOri.Title + "”改为“" + template.Title + "”；";
                }
                if (template.Content != templateOri.Content)
                {
                    userLog += "模板内容“" + templateOri.Content + "”改为“" + template.Content + "”；";
                }
                if (template.TCID != templateOri.TCID)
                {
                    userLog += "模板分类“"
                        + BLL.TemplateCategory.Instance.GetTemplateCategory(Convert.ToInt32(template.TCID)).Name
                        + "”改为“" + BLL.TemplateCategory.Instance.GetTemplateCategory(Convert.ToInt32(templateOri.TCID)).Name + "”；";
                }
                #region 邮件接收人
                DataTable dtEmailServers = BLL.TemplateInfo.Instance.getEmailServers(template.RecID);
                string strEmailServers = "";
                if (dtEmailServers != null && dt.Rows.Count > 0)
                {
                    int intVal = 0;
                    foreach (DataRow drEmailServers in dtEmailServers.Rows)
                    {
                        if (int.TryParse(drEmailServers["UserID"].ToString(), out intVal))
                        {
                            strEmailServers += BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(intVal) + ",";
                        }
                    }
                    if (strEmailServers.ToString().Length > 0)
                    {
                        strEmailServers += strEmailServers.ToString().Substring(0, strEmailServers.ToString().Length - 1);
                    }
                }
                #endregion

                if (strEmailServers != strEmailServersOri)
                {
                    userLog += "邮件接收人“" + strEmailServers + "”改为“" + strEmailServersOri + "”";

                }

                BLL.Util.InsertUserLog("【修改】模板\"" + title + "\"(ID:"
                    + template.RecID.ToString() + ")。修改者："
                    + BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(BLL.Util.GetLoginUserID()));
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