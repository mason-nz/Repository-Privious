using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.BusinessReport
{
    public partial class B_ReturnVisitReport : PageBase
    {
        public bool right_Export = false;
        public string returnvisitBg = string.Empty;
        public string NowMonth = DateTime.Now.Month.ToString();
        public string NowYear = DateTime.Now.Year.ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            int currentUserID = BLL.Util.GetLoginUserID();
            right_Export = BLL.Util.CheckRight(currentUserID, "SYS024MOD8201");
            if (!IsPostBack)
            {
                ddlBussiGroupBind();
                bindtime();
            }
        }
        private void bindtime()
        {
            int nowyear = DateTime.Now.Year;
            for (int i = nowyear; i >= 2015; i--)
            {
                this.SelYear.Items.Add(new ListItem(i + "年", i.ToString()));
                this.SelYear.SelectedIndex = this.SelYear.Items.IndexOf(new ListItem(DateTime.Now.Year + "年", DateTime.Now.Year.ToString()));
            }
            //for (int i = 1; i <= 12; i++)
            //{
            //    this.SelMonth.Items.Add(new ListItem(i + "月", i.ToString()));
            //    this.SelMonth.SelectedIndex = this.SelMonth.Items.IndexOf(new ListItem(DateTime.Now.Month + "月", DateTime.Now.Month.ToString()));
            //}
        }
        private void ddlBussiGroupBind()
        {
            DataTable bgdt = new DataTable();
            int userid = BLL.Util.GetLoginUserID();
            //所属分组
            bgdt = BLL.EmployeeSuper.Instance.GetCurrentUseBusinessGroup(userid);
            //取配置文件配置的分组，和管理组取交集
            returnvisitBg = BLL.Util.GetReturnVisitBG();
            if (!string.IsNullOrEmpty(returnvisitBg) && bgdt != null && bgdt.Rows.Count > 0)
            {
                string[] bgrow = returnvisitBg.Split(',');

                Dictionary<int, string> dc = new Dictionary<int, string>();
                for (int i = 0; i < bgrow.Length; i++)
                {
                    int bgid = 0;
                    int.TryParse(bgrow[i].Split('|')[0], out bgid);
                    dc.Add(bgid, bgrow[i].Split('|')[1]);
                }
                for (int i = 0; i < bgdt.Rows.Count; i++)
                {
                    DataRow r = bgdt.Rows[i];
                    int bgid = Convert.ToInt32(r["bgid"].ToString());
                    if (!dc.ContainsKey(bgid))
                    {
                        bgdt.Rows.Remove(r);
                        i--;
                    }
                }

                //绑定数据
                ddlBussiGroup.DataSource = bgdt;
                ddlBussiGroup.DataTextField = "Name";
                ddlBussiGroup.DataValueField = "BGID";
                ddlBussiGroup.DataBind();
            }
        }
    }
}