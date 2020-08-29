using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.BlackWhite
{
    public partial class AddWhiteData : PageBase
    {
        public string RecId
        {
            get
            {
                return HttpContext.Current.Request["RecId"] == null ? string.Empty : HttpContext.Current.Request["RecId"].ToString();
            }
        }
        public string TitleString;
        public string strAction;
        public int cdids = 0;
        public string strReason;
        public string DateNow = DateTime.Now.ToString("yyyy-MM-dd");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(RecId))
                {
                    TitleString = "修改白名单";
                    strAction = "UpdateData";
                    BindData();
                }
                else
                {
                    TitleString = "新增白名单";
                    strAction = "AddData";
                }
            }
        }
        public void BindData()
        {
            int recid;
            if (int.TryParse(RecId, out recid))
            {
                Entities.BlackWhiteList model = BLL.BlackWhiteList.Instance.GetModel(recid);

                edit_txtPhoneNum.Value = model.PhoneNum;
                edit_txtYouXiaoQi.Value = model.ExpiryDate.ToString("yyyy-MM-dd");
                if (model.CallType == 1)  //呼入
                {
                    edit_ckb_calltype_1.Checked = true;
                }
                if (model.CallType == 2)  //呼出
                {
                    edit_ckb_calltype_2.Checked = true;
                }
                if (model.CallType == 3)  //呼入和呼出
                {
                    edit_ckb_calltype_1.Checked = true;
                    edit_ckb_calltype_2.Checked = true;
                }

                strReason = model.Reason;
                cdids = model.CDIDS;
            }
        }
    }
}