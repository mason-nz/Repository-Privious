using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class SMSSendHistoryListForPersonal : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {

        private string RequestCustID
        {
            get
            {
                return Request["CustID"] == null ? "" :
                HttpUtility.UrlDecode(Request["CustID"].ToString().Trim());
            }
        }

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        public int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                BindData();
            }
        }

        CustInfoHelper ch = new CustInfoHelper();
        private void BindData()
        {
            Entities.QuerySMSSendHistory query = new Entities.QuerySMSSendHistory();
            int _loginID = -2;
            _loginID = userID;
            query.LoginID = userID;
            if (!string.IsNullOrEmpty(RequestCustID))
            {
                if (RequestCustID.ToUpper().StartsWith("CB"))
                {
                    query.CBID = RequestCustID;
                }
                else
                {
                    query.CRMCustID = RequestCustID;
                }
            }
            query.LoginID = BitAuto.ISDC.CC2012.Entities.Constants.Constant.INT_INVALID_VALUE;//add by wangtonghai
            DataTable dt = BLL.SMSSendHistory.Instance.GetSMSSendHistory(query, "a.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }


            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 40);

        }

        public string GetUrl(object obj, object custobj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return "../../TaskManager/CustInformation.aspx?CustID=" + custobj.ToString();
            }
            else
            {
                return "../../CustCheck/CrmCustSearch/CustDetail.aspx?CustID=" + obj.ToString();
            }

        }

        public string getCrmUrl(object CRMCustID, object custid)
        {
            if (CRMCustID == null || CRMCustID.ToString() == "")
            {
                return "&nbsp;";
            }
            else
            {
                return "<a href='../../CustCheck/CrmCustSearch/CustDetail.aspx?CustID= " + CRMCustID + "'  target='_blank'>" + CRMCustID + "</a>&nbsp;";
            }
        }
    }
}