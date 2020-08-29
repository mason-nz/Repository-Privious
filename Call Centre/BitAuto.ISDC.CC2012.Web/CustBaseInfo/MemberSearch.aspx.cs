using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CustBaseInfo
{
    public partial class MemberSearch : PageBase
    {
        public bool right_Magazine = false;
        public bool right_Member = false;
        public bool right_Contact = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                //杂志导入
                right_Magazine = BLL.Util.CheckRight(userID, "SYS024BUG200301");
                //会员导出
                right_Member = BLL.Util.CheckRight(userID, "SYS024BUG200302");
                //联系人导出
                right_Contact = BLL.Util.CheckRight(userID, "SYS024BUG200303");

                ExecCycleBind();
                rptAreaBind();
            }
        }

        private void ExecCycleBind()
        {
            DataTable dt = BLL.CC_MagazineReturn.Instance.GetDistinctTitle();
            sltExecCycle.DataSource = dt;
            sltExecCycle.DataTextField = "Title";
            sltExecCycle.DataValueField = "Title";
            sltExecCycle.DataBind();

        }

        private void rptAreaBind()
        {
            area.DataSource = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAllDistrict();
            area.DataTextField = "DepartName";
            area.DataValueField = "DepartID";
            area.DataBind();
            area.Items.Insert(0, new ListItem("全部", "-1"));
        }
    }
}