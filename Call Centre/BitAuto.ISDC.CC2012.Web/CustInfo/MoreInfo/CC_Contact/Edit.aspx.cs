using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_Contact
{
    public partial class Edit : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //客户联系人ID
        public int ContactID
        {
            get
            {
                return BitAuto.ISDC.CC2012.Web.WebUtil.Converter.String2Int(Request["ContactID"] + "", -1);
            }
        }

        /// <summary>
        /// 弹出框名称
        /// </summary>
        public string PopupName
        {
            get
            {
                string str = (Request["PopupName"] + "").Trim();
                if (string.IsNullOrEmpty(str)) { str = "AnonymousPopup"; }
                return str;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Entities.ProjectTask_Cust_Contact c = BLL.ProjectTask_Cust_Contact.Instance.GetContactInfo(this.ContactID);
                this.txtAddress.Value = c.Address;
                this.txtBirthday.Value = c.Birthday;
                this.txtContactDepartment.Value = c.DepartMent;
                this.txtContactDuty.Value = c.Title;
                this.txtContactEmali.Value = c.Email;
                this.txtContactFax.Value = c.Fax;
                this.txtContactModel.Value = c.Phone;
                this.txtContactName.Value = c.CName;
                this.txtContactRemark.Value = c.Remarks;
                this.txtContactTele.Value = c.OfficeTel;
                this.txtMSN.Value = c.MSN;
                this.txtOfficeTypeCode.Value = c.OfficeTypeCode.ToString();
                this.txtZipCode.Value = c.ZipCode;
                if (c.Sex == "1 ") { this.radioBoy.Checked = true; }
                else { this.radioGirl.Checked = true; }

                //this.PID = c.PID.ToString();
                CC_Contact_Helper.BindParentContactToSelectEle(this.SelectPID, c.PTID, c.ID);
                this.SelectPID.SelectedIndex = this.SelectPID.Items.IndexOf(this.SelectPID.Items.FindByValue(c.PID.ToString()));
            }
        }
    }
}