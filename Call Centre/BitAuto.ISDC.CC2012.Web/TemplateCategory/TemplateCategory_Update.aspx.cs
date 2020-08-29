using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Services.Organization.Remoting;
using System.Configuration;
namespace BitAuto.ISDC.CC2012.Web.TemplateCategory
{
    public partial class TemplateCategory_Update : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public Entities.TemplateInfo Template = new Entities.TemplateInfo();
        public string type = "";
        public string pID = "";
        public string level = "";
        public string TcID = "";
        public string recID = "";
        public string ToUserIDs = "";
        public string ToUserNames = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = BLL.Util.GetLoginUserID();

            if (!BLL.Util.CheckRight(userID, "SYS024MOD5101"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }

            Template = BLL.TemplateInfo.Instance.GetTemplateInfo(Convert.ToInt32(Request.QueryString["TemplateID"]));
            DataTable db = new DataTable(); //Template.TCID;
            QueryTemplateCategory query = new QueryTemplateCategory();
            query.RecID = Convert.ToInt32(Template.TCID);
            int totle;
            db = BLL.TemplateCategory.Instance.GetTemplateCategory(query, "RecID", 1, 1, out totle);
            recID = Template.RecID.ToString();
            TcID = Template.TCID.ToString();
            type = db.Rows[0]["Type"].ToString();
            level = db.Rows[0]["Level"].ToString();
            pID = db.Rows[0]["PID"].ToString();
            DataTable db_EmailServers = BLL.TemplateInfo.Instance.getEmailServers(Template.RecID);

            if (db_EmailServers != null && db_EmailServers.Rows.Count > 0)
            {
                foreach (DataRow row in db_EmailServers.Rows)
                {
                    ToUserIDs += row["UserID"].ToString() + ",";
                    //根据UserID获取用户名称
                    ToUserNames += BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(row["UserID"])) + ",";
                }
                if (ToUserIDs.Length > 0)
                {
                    ToUserIDs = "'" + ToUserIDs.Substring(0, ToUserIDs.Length - 1) + "'";
                    ToUserNames = "'" + ToUserNames.Substring(0, ToUserNames.Length - 1) + "'";
                }
            }
            else
            {
                ToUserIDs = "''";
                ToUserNames = "''";
            }
        }
    }
}