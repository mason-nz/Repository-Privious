using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web.SeniorManage
{
    public partial class LabelEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        //绑定数据
        public void BindData()
        {
            DataTable dt = null;

            dt = BLL.LabelTable.Instance.GetAllList();

            if (dt != null)
            {                
                //dt.DefaultView.RowFilter = where;
                rptLabel.DataSource = dt.DefaultView;
                rptLabel.DataBind();
            }
        }

        public int CanStop(object ltid)
        {
            if (BLL.ComSentence.Instance.LabelIsUsedInCS(Convert.ToInt32(ltid)))
            {                
                return 1;
            }

            return 0;
        }
    }
}