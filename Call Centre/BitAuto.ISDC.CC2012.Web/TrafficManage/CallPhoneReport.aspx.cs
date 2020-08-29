using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TrafficManage
{
    public partial class CallPhoneReport : PageBase
    {
        public bool IsExport = false;//是否可以“导出”功能
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                IsExport = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024MOD400501");                
            }
        }

        /// <summary>
        /// 绑定坐席组
        /// </summary>
        //private void BindAgentGroup()
        //{
        //    DataTable dt = BLL.BusinessGroup.Instance.GetAllBusinessGroup();
        //    if (dt != null && dt.Rows.Count != 0)
        //    {
        //        selAgentGroup.DataSource = dt;
        //        selAgentGroup.DataTextField = "Name";
        //        selAgentGroup.DataValueField = "BGID";
        //        selAgentGroup.DataBind();
        //    }
        //    selAgentGroup.Items.Insert(0, new ListItem("请选择", "-1"));
        //}
        
    }
}