using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_DMS2014.Entities;

namespace BitAuto.DSC.IM_DMS2014.Web.AjaxServers.SeniorManage
{
    public partial class FreProblemList : System.Web.UI.Page
    {
        public int MinRecID = 0;
        public int MaxRecID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            DataTable dt = BLL.FreProblem.Instance.GetAllFreProblem(9999);
            if (dt.Rows.Count > 0)
            {
                MinRecID = CommonFunc.ObjectToInteger(dt.Rows[0]["RecID"]);
                MaxRecID = CommonFunc.ObjectToInteger(dt.Rows[dt.Rows.Count - 1]["RecID"]);
            }
            dataRepeater.DataSource = dt;
            dataRepeater.DataBind();
        }
    }
}