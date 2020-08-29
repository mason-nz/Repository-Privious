using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM_DMS2014.Web.SeniorManage
{
    public partial class FreProblemForClient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Databound();
            }
        }
        private void Databound()
        {
            DataTable dt = BLL.FreProblem.Instance.GetAllFreProblem(10);
            this.repeaterList.DataSource = dt;
            this.repeaterList.DataBind();
        }
    }
}