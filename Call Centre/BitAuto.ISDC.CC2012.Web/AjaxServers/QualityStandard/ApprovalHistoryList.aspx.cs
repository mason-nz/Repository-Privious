using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard
{
    public partial class ApprovalHistoryList : PageBase
    {
        public string RequestQS_RID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("QS_RID")); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int rid = 0;
                if (int.TryParse(RequestQS_RID, out rid))
                {
                    Entities.QueryQS_ApprovalHistory query = new Entities.QueryQS_ApprovalHistory();
                    query.QS_RID = rid;
                    int totalCount = 0;
                    DataTable dt = BLL.QS_ApprovalHistory.Instance.GetQS_ApprovalHistory(query, "QS_ApprovalHistory.CreateTime Desc", 1, 1000, out totalCount);
                    rptApprovalHistoryList.DataSource = dt;
                    rptApprovalHistoryList.DataBind();
                }
            }
        }
    }
}