using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CustBaseInfo
{
    public partial class CustSearch : PageBase
    {
        public string SysUrl = System.Configuration.ConfigurationManager.AppSettings["SysUrl"].ToString();
        public string WpUrl = System.Configuration.ConfigurationManager.AppSettings["WpUrl"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlDistrict.DataSource = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAllDistrict();
                ddlDistrict.DataTextField = "DepartName";
                ddlDistrict.DataValueField = "DepartName";
                ddlDistrict.DataBind();
                ddlDistrict.Items.Insert(0, new ListItem("请选择", ""));
            }
        }
    }
}