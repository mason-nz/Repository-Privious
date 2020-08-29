using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.SeniorManage
{
    public partial class CityManageList : System.Web.UI.Page
    {
        public ServeTime st = null;
        public ServeTime et = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        private void InitData()
        {
            DataTable dt = BLL.BaseData.Instance.GetAllDistrict();
            rpt_district.DataSource = dt;
            rpt_district.DataBind();

            BLL.BaseData.Instance.ReadTime(out st, out et);
        }
    }
}