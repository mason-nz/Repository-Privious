using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.YanFa.Crm2009.Entities;
namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_Contact
{
    public partial class Add : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TID
        {
            get { return Request["TID"] + ""; }
        }

        public string CustID
        {
            get { return HttpContext.Current.Request["CustID"] == null ? "" : HttpContext.Current.Request["CustID"].ToString(); }
        }

        public string CustType
        {
            get { return Request["CustType"] + ""; }
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
        public string strFillPage = "";
        public string addurl = "";
        public string strAction = "";

        private bool _isShowMember;
        public bool isShowMember
        {
            get { return _isShowMember; }
            set { _isShowMember = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //得到客户的所有联系人，绑定到直接上级                
                CC_Contact_Helper.BindParentContactToSelectEle(this.SelectPID, TID, -1);
            }
            if (!IsPostBack)
            {
                FillPage();
                _isShowMember = CheckShowMember();
            }
        }

        /// <summary>
        /// 检查客户类别，显示是否分配会员及负责会员
        /// </summary> 
        /// <returns></returns>
        private bool CheckShowMember()
        {
            bool isShow = false;

            if (CustID != "")
            {
                //判断客户类型：综合店、特许经销商、4S、展厅、集团显示
                if (CustType == ((int)EnumCustomType.SynthesizedShop).ToString()
                    || CustType == ((int)EnumCustomType.Licence).ToString()
                    || CustType == ((int)EnumCustomType.FourS).ToString()
                    || CustType == ((int)EnumCustomType.Showroom).ToString()
                    || CustType == ((int)EnumCustomType.Bloc).ToString())
                {
                    isShow = true;
                }

                //判断客户下面有开通的易湃会员显示
                List<DMSMember> list = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByCustID(CustID);
                if (list.Count > 0 && isShow)
                {
                    rtpMemberList.DataSource = list;
                    rtpMemberList.DataBind();
                }
                else
                {
                    isShow = false;
                }
            }
            return isShow;
        }

        private void FillPage()
        {
            if (Request["ID"] != null && Request["ID"] != "0")
            {
                strFillPage = "fillpage();";
                strAction = "editcontact";
                //addurl = "Action=editcontact&ContactID=" + Request["ID"];
            }
            else
            {
                //addurl = "Action=AddContact";
                strAction = "AddContact";
            }
        }
    }
}