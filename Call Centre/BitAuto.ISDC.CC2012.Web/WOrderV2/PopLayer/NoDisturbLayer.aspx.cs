using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer
{
    public partial class NoDisturbLayer : PageBase
    {
        public string PhoneNumber
        {
            get
            {
                return HttpContext.Current.Request["PhoneNumber"] == null ? string.Empty : HttpContext.Current.Request["PhoneNumber"].ToString();
            }
        }
        public string CallId
        {
            get
            {
                return HttpContext.Current.Request["CallId"] == null ? string.Empty : HttpContext.Current.Request["CallId"].ToString();
            }
        }
        public string ResponseFrom
        {
            get
            {
                return HttpContext.Current.Request["ResponseFrom"] == null ? string.Empty : HttpContext.Current.Request["ResponseFrom"].ToString();
            }
        }

        public string TitleString;
        public string strAction;
        public string strRecID;
        public string strPhoneNumber;
        public string strCallId;
        public int cdids = 0;
        public string strReason;
        public string DateNow = DateTime.Now.ToString("yyyy-MM-dd");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                strPhoneNumber = PhoneNumber;
                strCallId = CallId;
                BindNoDisturbReason();

                //判断此电话号码是否已经是免打扰号码（status=0和status=1）
                if (!string.IsNullOrEmpty(PhoneNumber))
                {
                    int backVal = BLL.BlackWhiteList.Instance.PhoneNumIsNoDisturb(PhoneNumber);
                    if (backVal != -1 && backVal != 2)
                    {
                        TitleString = "修改免打扰";
                        strAction = "UpdateNoDisturbData";
                        //将已存在的信息绑定上去
                        BindData();
                    }
                    else
                    {
                        AddNewNodisturbNumber();
                    }
                }
                else //status=-1按新增算，这样可以关联上以前的操作记录
                {
                    AddNewNodisturbNumber();
                }
            }
        }

        private void AddNewNodisturbNumber()
        {
            TitleString = "新增免打扰";
            strAction = "AddNoDisturbData";
            edit_txtPhoneNum.Value = PhoneNumber;
            txtCallID.Value = CallId.Trim();
            edit_ckb_calltype_1.Checked = false;
            edit_ckb_calltype_2.Checked = true;
            edit_txtYouXiaoQi.Value = DateTime.Now.AddMonths(3).ToString("yyyy-MM-dd");
        }

        private void BindData()
        { 
            //修改免打扰号码,需要传递RecID
            int backRecID = BLL.BlackWhiteList.Instance.GetRecIDByPhoneNumberAndType(PhoneNumber, 0);
            if (backRecID > 0)
            {
                BindModelData(backRecID);
            }
        }

        private void BindModelData(int recid)
        {
            strRecID = recid.ToString();
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

            txtCallID.Value = string.IsNullOrEmpty(CallId.Trim()) ? (model.CallID.HasValue ? model.CallID.Value.ToString() : "") : CallId.Trim();
            selNoDisturbReason.SelectedIndex = model.CallOutNDType < 0 ? 0 : model.CallOutNDType;

        }
        /// <summary>
        /// 绑定免打扰原因
        /// </summary>
        private void BindNoDisturbReason()
        {
            selNoDisturbReason.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.NoDisturbReason));
            selNoDisturbReason.DataTextField = "name";
            selNoDisturbReason.DataValueField = "value";
            selNoDisturbReason.DataBind();
            selNoDisturbReason.Items.Insert(0, new ListItem("请选择", "0"));
        }
    }
}