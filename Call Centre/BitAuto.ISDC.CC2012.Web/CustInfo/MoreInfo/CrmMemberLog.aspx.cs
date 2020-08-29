using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class CrmMemberLog : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string OriginalDMSMemberID
        {
            get
            {
                return Request["OriginalDMSMemberID"] == null ? string.Empty : Request["OriginalDMSMemberID"].ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            if (string.IsNullOrEmpty(OriginalDMSMemberID)) { return; }
            else
            {
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetSyncLog(new Guid(OriginalDMSMemberID));
                //设置数据源
                if (dt != null && dt.Rows.Count > 0)
                {
                    repeater_Contact.DataSource = dt;
                    repeater_Contact.DataBind();
                }
            }
        }


        public string GetSyncStatusDesc(string s)
        {
            switch (s)
            {
                case "170000":
                    return "删除申请";
                case "170001":
                    return "申请会员";
                case "170002":
                    return "开通会员";
                case "170003":
                    return "开通会员";
                case "170008":
                    return "打回";
                default:
                    return "";
            }
        }

        public string GetDateTimeDesc(string s)
        {
            DateTime d;
            if (DateTime.TryParse(s, out d))
            {
                return d.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return s;
            }
        }
    }
}