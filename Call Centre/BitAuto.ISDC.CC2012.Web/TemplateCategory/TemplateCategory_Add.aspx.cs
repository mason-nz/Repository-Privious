using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.TemplateCategory
{
    public partial class TemplateCategory_Add : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = BLL.Util.GetLoginUserID();

            if (!BLL.Util.CheckRight(userID, "SYS024MOD5101"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
        }

        protected void Bt_SubClick(object sender, EventArgs e)
        {
            Entities.TemplateInfo template = new Entities.TemplateInfo();

            if (Request["hiddenFile"].ToString() == "0")
            {
                Response.Write("<script> alert('请选择模板分类！') </script>");
                return;
            }
            else
            {
                template.TCID = Convert.ToInt32(Request["hiddenFile"]);
            }

            if (Request["TemplateTile"].ToString() == "")
            {
                Response.Write("<script> alert('请输入模板标题！') </script>");
                return;
            }
            else
            {
                template.Title = Request["TemplateTile"].ToString();
            }
            if (Request["TemplateCon"].ToString() == "")
            {
                Response.Write("<script> alert('请输入文本内容！') </script>");
                return;
            }
            else
            {
                template.Content = Request["TemplateCon"].ToString();
            }
            if (Request["type"].ToString() == "2")
            {//邮件
                Response.Write("<script> alert('邮件！') </script>");
                if (Request["PersonsGetEmailIN"].ToString() == "")
                {
                    Response.Write("<script> alert('请填写邮件接收人！') </script>");
                    return;
                }
            }
            else if (Request["type"].ToString() == "1")
            {//短信
                Response.Write("<script> alert('短信！') </script>");
            }

            template.CreateUserID = 0;
            template.CreateTime = DateTime.Now;
            template.ReplaceTag = "-";
            template.Status = 0;
            //模板写入
            int templateID = BLL.TemplateInfo.Instance.Insert(template);
            //收件人写入
            if (Request["type"].ToString() == "2")
            {
                string[] userIDArr = Request["PersonsGetEmailIN"].ToString().Split(',');   
                foreach (string userID in userIDArr)
                {
                    string sqlStr = 
                        "insert into ConsultSolveUserMapping TemplateID,SolveUserEID values(" 
                        + templateID.ToString() + "," + userID + ");";
                    Entities.ConsultSolveUserMapping map = new Entities.ConsultSolveUserMapping();
                    map.CreateUserID = 100;//创建者
                    map.SolveUserEID = Convert.ToInt32(userID);
                    map.TemplateID = templateID;
                    map.SolveUserEName = "";
                    BLL.ConsultSolveUserMapping.Instance.Insert(map);
                }                
            }
            Response.Write("<script> alert('添加成功！'); </script>");
        }
        
    }
}