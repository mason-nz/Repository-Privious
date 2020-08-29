using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.CustInfo;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.ReturnVisit.CC_Contact
{
    public partial class ListWithEdit : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string CustID
        {
            get { return BLL.Util.GetCurrentRequestStr("CustID"); }
        }
        public string TaskType
        {
            get { return BLL.Util.GetCurrentRequestStr("TaskType"); }
        }
        public string FirstMemberCode = "";
        public string FirstMemberName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        CustInfoHelper ch = new CustInfoHelper();

        private void BindData()
        {
            //查询第一个经销商
            List<BitAuto.YanFa.Crm2009.Entities.DMSMember> list = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(CustID);
            if (list != null && list.Count > 0)
            {
                FirstMemberCode = list[0].MemberCode;
                FirstMemberName = list[0].Name;
            }
            //查询
            int totalCount = 0;
            BitAuto.YanFa.Crm2009.Entities.QueryContactInfo qci = new BitAuto.YanFa.Crm2009.Entities.QueryContactInfo();
            qci.CustID = CustID;
            DataTable table = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetContactInfo(qci, "ModifyTime desc", ch.CurrentPage, ch.PageSize, out totalCount);
            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                repeater_Contact.DataSource = table;
            }
            //绑定列表数据
            repeater_Contact.DataBind();
            //分页控件
            this.AjaxPager_Contact.InitPager(totalCount, "ContactInfoContent", ch.PageSize, ch.CurrentPage);
        }

        public string GetAddWOrderUrl(string contactid, string phone, string cbname, string cbsex)
        {
            if (phone.Trim() == "")
            {
                return "href=\"javascript:void(0)\" style=\"display:none\"";
            }
            else
            {
                phone = phone.Replace("-", "").Trim();
                phone = BLL.Util.HaoMaProcess(phone);
                //crm 的性别是 0男1女 cc的性别是 1男2女
                int sex = CommonFunction.ObjectToInteger(cbsex, -1) + 1;
                string url = BLL.WOrderRequest.AddWOrderComeIn_CallOut(phone, this.CustID, cbname, sex, FirstMemberCode, CommonFunction.ObjectToInteger(contactid)).ToString();
                return "href=\"/WOrderV2/AddWOrderInfo.aspx?" + url + "\" target=\"_blank\"";
            }
        }

        public string ShowPhoneCallImg(string phone)
        {
            if (phone.Trim() == "")
            {
                return "style=\"float:right; display:none\"";
            }
            else
            {
                return "style=\"float:right;\" cc:cc";
            }
        }

        public string ShowSmSSendImg(string phone)
        {
            if (phone.Trim() == "")
            {
                return "style=\"display:none\"";
            }
            else
            {
                return "";
            }
        }
    }
}