using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AlloCustomer
{
    public partial class List : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {

        //功能权限
        protected bool right_allocation;        //分配权限
        protected bool right_withdraw;         //收回权限
        protected bool right_export;         //导出权限

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                right_allocation = BLL.Util.CheckRight(userID, "SYS024BUT10110101");
                right_withdraw = BLL.Util.CheckRight(userID, "SYS024BUT10110102");
                right_export = BLL.Util.CheckRight(userID, "SYS024BUT10110103");

                DataTable dt = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAllDistrict();

                selArea.Items.Add(new ListItem("请选择", "-1"));
                foreach (DataRow dr in dt.Rows)
                {
                    selArea.Items.Add(new ListItem(CommonFunction.ObjectToString(dr["DepartName"]), CommonFunction.ObjectToString(dr["DepartID"])));
                }
            }
        }
    }
}