using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.BLL;

namespace BitAuto.DSC.IM_2015.Web.SeniorManage
{
    public partial class MoreUrlConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userid = BLL.Util.GetLoginUserID();
                BindData();
            }
        }
        protected void BindData()
        {
            List<MoreURlModelClss> dictime = new List<MoreURlModelClss>();
            BLL.BaseData.Instance.ReadMoreURLAll(out dictime);
            if (dictime != null && dictime.Count > 0)
            {
                this.repeaterTableList.DataSource = dictime;
                this.repeaterTableList.DataBind();
            }
        }
    }
}