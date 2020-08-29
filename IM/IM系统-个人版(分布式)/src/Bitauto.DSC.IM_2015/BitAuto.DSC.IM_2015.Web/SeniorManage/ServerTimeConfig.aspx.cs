using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.DSC.IM_2015.Web.Controls;

namespace BitAuto.DSC.IM_2015.Web.SeniorManage
{
    public partial class ServerTimeConfig : PageBase
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
            List<TimeModelClss> dictime = new List<TimeModelClss>();
            BLL.BaseData.Instance.ReadTimeAll(out dictime);
            if (dictime != null && dictime.Count > 0)
            {
                this.repeaterTableList.DataSource = dictime;
                this.repeaterTableList.DataBind();
            }
        }
    }
}