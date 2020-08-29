using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustCheck
{
    public partial class ViewJiCaiProject : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rptProjectList_DataBind();
        }

        private void rptProjectList_DataBind()
        {
            BitAuto.YanFa.Crm2009.Entities.QueryJiCaiProject query = new BitAuto.YanFa.Crm2009.Entities.QueryJiCaiProject();
            query.MemberID = Request.QueryString["MemberID"];
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.JiCaiProject.Instance.GetJiCaiProjectMember(query);
            rptProjectList.DataSource = dt;
            rptProjectList.DataBind();
        }
        
    }
}