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
    public partial class CallList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //int RecordCount = 0;
                DataTable dt = null; //作废 BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIG("", "order by CreateTime Desc", 1, 100000, out RecordCount);
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();

                lbCount.InnerText = dt.Rows.Count.ToString();
            }
        }
    }
}