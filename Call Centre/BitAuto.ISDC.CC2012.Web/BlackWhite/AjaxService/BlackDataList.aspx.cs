using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.BlackWhite.AjaxService
{
    public partial class BlackDataList : PageBase
    {
        public string PhoneNumber
        {
            get { return Request["PhoneNumber"] == null ? string.Empty : HttpUtility.UrlDecode(Request["PhoneNumber"].ToString()); }
        }
        public string EffectiveBeginTime
        {
            get { return Request["EffectiveBeginTime"] == null ? string.Empty : HttpUtility.UrlDecode(Request["EffectiveBeginTime"].ToString()); }
        }
        public string EffectiveEndTime
        {
            get { return Request["EffectiveEndTime"] == null ? string.Empty : HttpUtility.UrlDecode(Request["EffectiveEndTime"].ToString()); }
        }
        public string CallTypes
        {
            get { return Request["CallTypes"] == null ? string.Empty : HttpUtility.UrlDecode(Request["CallTypes"].ToString()); }
        }
        public string UserId
        {
            get { return Request["UserId"] == null ? string.Empty : HttpUtility.UrlDecode(Request["UserId"].ToString()); }
        }
        public string AddBeginTime
        {
            get { return Request["AddBeginTime"] == null ? string.Empty : HttpUtility.UrlDecode(Request["AddBeginTime"].ToString()); }
        }
        public string AddEndTime
        {
            get { return Request["AddEndTime"] == null ? string.Empty : HttpUtility.UrlDecode(Request["AddEndTime"].ToString()); }
        }
        public string BusinessTypes
        {
            get { return Request["BusinessTypes"] == null ? string.Empty : HttpUtility.UrlDecode(Request["BusinessTypes"].ToString()); }
        }
        public int PageIndex
        {
            get
            {
                return CommonFunction.ObjectToInteger(BLL.PageCommon.Instance.PageIndex, 1);
            }
        }

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindListData();  
                
            }
        }
        private void BindListData()
        {
            Entities.QueryBlackWhite query = new Entities.QueryBlackWhite();
            query.Type = 0;
            query.QueryLoginUserId = BLL.Util.GetLoginUserID();
            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                query.PhoneNum = PhoneNumber;
            }
            if (!string.IsNullOrEmpty(EffectiveBeginTime))
            {
                query.EffectiveDate = Convert.ToDateTime(EffectiveBeginTime);
            }
            if (!string.IsNullOrEmpty(EffectiveEndTime))
            {
                query.ExpiryDate = Convert.ToDateTime(EffectiveEndTime);
            }
            if (!string.IsNullOrEmpty(CallTypes))
            {
                query.QueryCallTypes = CallTypes;
            }
            if (!string.IsNullOrEmpty(UserId) && UserId.Trim() != "-1")
            {
                query.CreateUserId = Convert.ToInt32(UserId);
            }
            if (!string.IsNullOrEmpty(AddBeginTime))
            {
                query.QueryCreateStartDate = Convert.ToDateTime(AddBeginTime);
            }
            if (!string.IsNullOrEmpty(AddEndTime))
            {
                query.QueryCreateEndDate = Convert.ToDateTime(AddEndTime);
            }
            if (!string.IsNullOrEmpty(BusinessTypes) && BusinessTypes != "-1")
            {
                query.QueryCDIDs = BusinessTypes;
            }
            DataTable dt = BLL.BlackWhiteList.Instance.GetBlackWhiteData(query, "a.CreateDate desc", PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();
            litPagerDown.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, PageCommon.Instance.PageIndex, 1);

        }

        public string GetCallOutNDTypeName(string CallOutNDType)
        {
            int CallOutNDTypeVal;
            if (int.TryParse(CallOutNDType, out CallOutNDTypeVal))
            {
                try
                {
                    return BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.NoDisturbReason), CallOutNDTypeVal);
                }
                catch
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public string getCurrentPage()
        {
            return BLL.PageCommon.Instance.PageIndex.ToString();
        }
    }

}