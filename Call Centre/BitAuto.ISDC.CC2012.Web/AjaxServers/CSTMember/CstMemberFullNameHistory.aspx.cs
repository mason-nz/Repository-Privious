using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CSTMember
{
    public partial class CstMemberFullNameHistory : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string memberId = Request.QueryString["CstMemberID"].Trim();
                spanCustID.InnerText = "车商通会编号：" + memberId;
                ShowNameHistory(memberId);
            }
        }

        private void ShowNameHistory(string memberId)
        {
            int id = -1;
            if (int.TryParse(memberId, out id))
            {
                Entities.ProjectTask_CSTMember info = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberModel(id);
                if (info != null)
                {
                    if (!string.IsNullOrEmpty(info.OriginalCSTRecID))
                    {
                        BitAuto.YanFa.Crm2009.Entities.CstMember cstMemberInfo = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCstMemberModel(info.OriginalCSTRecID);
                        if (cstMemberInfo != null)
                        {
                            spanCustID.InnerText = "车商通会编号：" + cstMemberInfo.CstMemberID;
                        }
                        repterCustNameHistoryList.DataSource = BLL.ProjectTask_CSTMember.Instance.GetCstMemberFullNameHistory(info.OriginalCSTRecID);
                        repterCustNameHistoryList.DataBind();
                    }
                }
            }
        }
    }
}