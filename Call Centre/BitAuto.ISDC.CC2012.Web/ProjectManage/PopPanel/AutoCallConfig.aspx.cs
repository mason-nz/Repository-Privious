using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;


namespace BitAuto.ISDC.CC2012.Web.ProjectManage.PopPanel
{
    public partial class AutoCallConfig :PageBase
    {

        public string PID
        {
            get { return HttpContext.Current.Request["pid"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["pid"].ToString()); }
        }
        public string PName
        {
            get { return HttpContext.Current.Request["pn"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["pn"].ToString()); }
        }
        public string Skgid
        {
            get { return HttpContext.Current.Request["skgid"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["skgid"].ToString()); }
        }
        public string Cdid
        {
            get { return HttpContext.Current.Request["cdid"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["cdid"].ToString()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {

            var data400 = BLL.AutoCall_ProjectInfo.Instance.GetOutCall400Number();

            




            StringBuilder sb400 = new StringBuilder();

            sb400.Append("<select id=\"selBusinessType\" style=\"width: 261px; padding: 0 2px; height: 22px; line-height: 22px;border: 1px solid #ccc;\"><option value=\"-1\" >请选择</option>");
           
            if (data400.Tables.Count > 0 && data400.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in data400.Tables[0].Rows)
                {
                    //ddlSG.Items.Add(new ListItem(row["Name"].ToString(), row["SGID"].ToString()));
                    sb400.Append(String.Format(" <option value='{0}' >{1}</option>", row["cdid"], row["remark"]));
                }
            }
            sb400.Append("</select>");
            selBusinessType.Text = sb400.ToString();

            var data = BLL.SkillGroupDataRight.Instance.GetAutoCallSkillGroup();

            StringBuilder sbSel = new StringBuilder();

            sbSel.Append(" <select id='selSKG' style='width: 265px; padding: 0 2px; height: 22px; line-height: 22px;border: 1px solid #ccc;'><option value='-1' >请选择</option>");
            //</select>

            //ddlSG.Items.Clear();
            //ddlSG.Items.Add(new ListItem("请选择...", "-1"));
            if (data!=null&&data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    //ddlSG.Items.Add(new ListItem(row["Name"].ToString(), row["SGID"].ToString()));
                    sbSel.Append(String.Format(" <option value='{0}' >{1}</option>", row["SGID"], row["Name"]));
                }

            }
            sbSel.Append("</select>");
            skg.Text = sbSel.ToString();
        }
    }
}