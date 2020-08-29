using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.Demand.DemandHandle
{
    public partial class List : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAllDistrict();

                sltArea.Items.Add(new ListItem("请选择", "-1"));
                foreach (DataRow dr in dt.Rows)
                {
                    sltArea.Items.Add(new ListItem(CommonFunction.ObjectToString(dr["DepartName"]), CommonFunction.ObjectToString(dr["DepartID"])));
                }
            }
        }
    }
}