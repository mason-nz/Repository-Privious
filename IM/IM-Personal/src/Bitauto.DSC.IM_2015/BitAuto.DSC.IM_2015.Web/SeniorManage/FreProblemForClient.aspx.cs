using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web.SeniorManage
{
    public partial class FreProblemForClient : System.Web.UI.Page
    {
        //业务线
        private int SourceType
        {
            get
            {
                int _sourcetype = 0;
                string sourcetype = HttpContext.Current.Request["SourceType"];
                if (!string.IsNullOrEmpty(sourcetype))
                {
                    if (int.TryParse(sourcetype, out _sourcetype))
                    {
                    }
                }
                return _sourcetype;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Databound(SourceType);
            }
        }
        private void Databound(int SourceType)
        {
            DataTable dt = BLL.FreProblem.Instance.GetAllFreProblem(15,SourceType);
            this.repeaterList.DataSource = dt;
            this.repeaterList.DataBind();
        }
    }
}