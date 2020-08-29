using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CSTMember
{
    public partial class CSTAccountInfo : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            int CstMemberID = Convert.ToInt32(Request.QueryString["CSTMemberID"].ToString());
            //DataTable dt = BitAuto.YanFa.DMSInterface.CSTMemberServiceHelper.GetAccontInfo(CstMemberID);
            string strMessage;
            DataTable dt = BitAuto.YanFa.DMSInterface.CstMemberServiceHandler.GetAccontInfo(CstMemberID, out strMessage);
            repterCSTAcountList.DataSource = dt;
            repterCSTAcountList.DataBind();

        }

        public string GetCstAccountStatusString(int status)
        {
            string CstAccountStatusString = "";
            switch (status)
            {
                case 0:
                    CstAccountStatusString = "禁用";
                    break;
                case 1:
                    CstAccountStatusString = "启用";
                    break;
                case -1:
                    CstAccountStatusString = "已删除";
                    break;
            }
            return CstAccountStatusString;

        }
    }
}