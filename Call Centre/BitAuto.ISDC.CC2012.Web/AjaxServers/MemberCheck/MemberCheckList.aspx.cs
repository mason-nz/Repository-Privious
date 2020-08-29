using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.MemberCheck
{
    public partial class MemberCheckList : PageBase
    {
        #region 定义属性
        public string RequestMemberName
        {
            get { return BLL.Util.GetCurrentRequestStr("MemberName"); }
        }
        public string RequestMemberAddr
        {
            get { return BLL.Util.GetCurrentRequestStr("MemberAddr"); }
        }
        public string RequestCustName
        {
            get { return BLL.Util.GetCurrentRequestStr("CustName"); }
        }
        public string RequestCustID
        {
            get { return BLL.Util.GetCurrentRequestStr("CustID"); }
        }
        public string RequestApplyStartTime
        {
            get { return BLL.Util.GetCurrentRequestStr("ApplyStartTime"); }
        }
        public string RequestApplyEndTime
        {
            get { return BLL.Util.GetCurrentRequestStr("ApplyEndTime"); }
        }
        public string RequestApplyUserName
        {
            get { return BLL.Util.GetCurrentRequestStr("ApplyUserName"); }
        }
        public string RequestMemberOptStartTime
        {
            get { return BLL.Util.GetCurrentRequestStr("MemberOptStartTime"); }
        }
        public string RequestMemberOptEndTime
        {
            get { return BLL.Util.GetCurrentRequestStr("MemberOptEndTime"); }
        }
        public string RequestDMSSyncStatus
        {
            get { return BLL.Util.GetCurrentRequestStr("DMSSyncStatus"); }
        }
        public string RequestDMSStatus
        {
            get { return BLL.Util.GetCurrentRequestStr("DMSStatus"); }
        }

        //查询类型：0 车易通；1 车商通
        public string RequestType
        {
            get { return BLL.Util.GetCurrentRequestStr("Type"); }
        }

        #endregion

        private string RequestPageSize
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["pageSize"]) ? "20" :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["pageSize"].ToString());
            }
        }
        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!int.TryParse(RequestPageSize, out PageSize))
                {
                    PageSize = 20;
                }

                if (RequestType == "0")//选择车易通信息查询
                {
                    BindDMSData();
                }
                else if (RequestType == "1")//选择车商通信息查询
                {
                    BindCSTData();
                }
            }
        }

        //易湃会员信息查询方法
        private void BindDMSData()
        {
            Entities.QueryProjectTask_DMSMember query = new Entities.QueryProjectTask_DMSMember();

            if (!string.IsNullOrEmpty(RequestMemberName))
            {
                query.MemberName = RequestMemberName;
            }
            if (!string.IsNullOrEmpty(RequestMemberAddr))
            {
                query.MemberAbbr = RequestMemberAddr;
            }
            if (!string.IsNullOrEmpty(RequestCustName))
            {
                query.CustName = RequestCustName;
            }
            if (!string.IsNullOrEmpty(RequestCustID))
            {
                query.CustID = RequestCustID;
            }
            if (!string.IsNullOrEmpty(RequestApplyStartTime))
            {
                query.ApplyStartTime = RequestApplyStartTime;
            }
            if (!string.IsNullOrEmpty(RequestApplyEndTime))
            {
                query.ApplyEndTime = RequestApplyEndTime;
            }
            if (!string.IsNullOrEmpty(RequestApplyUserName))
            {
                query.ApplyUserName = RequestApplyUserName;
            }
            if (!string.IsNullOrEmpty(RequestMemberOptStartTime))
            {
                query.MemberOptStartTime = RequestMemberOptStartTime;
            }
            if (!string.IsNullOrEmpty(RequestMemberOptEndTime))
            {
                query.MemberOptEndTime = RequestMemberOptEndTime;
            }
            if (!string.IsNullOrEmpty(RequestDMSSyncStatus))
            {
                query.DMSSyncStatus = RequestDMSSyncStatus;
            }
            else
            {
                query.DMSSyncStatus = "170001,170002,170003,170008";
            }

            if (BLL.UserDataRigth.Instance.IsExistsByUserID(BLL.Util.GetLoginUserID()))
            {
                query.DMSMemberApplyUserID = Constant.INT_INVALID_VALUE;
            }
            else
            {
                query.DMSMemberApplyUserID = BLL.Util.GetLoginUserID();
            }
            if (!string.IsNullOrEmpty(RequestDMSStatus))
            {
                query.DMSStatus = RequestDMSStatus;
            }

            int count;
            DataTable dt = BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMemberBySourceCC(query, "ApplyTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterList.DataSource = dt;
            repeaterList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);

            //AjaxPager_Member.InitPager(count);
            //selectDown.Value = RequestPageSize.ToString();

            StatusCountStat(query, RecordCount);
        }

        private void StatusCountStat(Entities.QueryProjectTask_DMSMember query, int allCount)
        {
            int applyForCount = 0;
            int createSuccessfulCount = 0;
            int createUnsuccessful = 0;
            int rejected = 0;
            BLL.ProjectTask_DMSMember.Instance.StatCC_DMSMembersBySourceCC(query, out applyForCount, out createSuccessfulCount, out createUnsuccessful, out rejected);

            sml.InnerHtml = "总计：";
            sml.InnerHtml += allCount;
            sml.InnerHtml += "&nbsp;&nbsp;&nbsp;审批中：";
            sml.InnerHtml += applyForCount;
            sml.InnerHtml += "&nbsp;&nbsp;&nbsp;通过：";
            sml.InnerHtml += createSuccessfulCount;
            sml.InnerHtml += "&nbsp;&nbsp;&nbsp;失败：";
            sml.InnerHtml += createUnsuccessful;
            sml.InnerHtml += "&nbsp;&nbsp;&nbsp;打回：";
            sml.InnerHtml += rejected;
        }

        //车商通会员信息查询方法
        private void BindCSTData()
        {
            Entities.QueryProjectTask_CSTMember query = new Entities.QueryProjectTask_CSTMember();

            if (!string.IsNullOrEmpty(RequestMemberName))
            {
                query.FullName = RequestMemberName;
            }
            if (!string.IsNullOrEmpty(RequestMemberAddr))
            {
                query.ShortName = RequestMemberAddr;
            }
            if (!string.IsNullOrEmpty(RequestCustName))
            {
                query.CustName = RequestCustName;
            }
            if (!string.IsNullOrEmpty(RequestCustID))
            {
                query.CustID = RequestCustID;
            }
            if (!string.IsNullOrEmpty(RequestApplyStartTime))
            {
                query.ApplyStartTime = RequestApplyStartTime;
            }
            if (!string.IsNullOrEmpty(RequestApplyEndTime))
            {
                query.ApplyEndTime = RequestApplyEndTime;
            }
            if (!string.IsNullOrEmpty(RequestApplyUserName))
            {
                query.ApplyUserName = RequestApplyUserName;
            }
            if (!string.IsNullOrEmpty(RequestMemberOptStartTime))
            {
                query.MemberOptStartTime = RequestMemberOptStartTime;
            }
            if (!string.IsNullOrEmpty(RequestMemberOptEndTime))
            {
                query.MemberOptEndTime = RequestMemberOptEndTime;
            }
            if (!string.IsNullOrEmpty(RequestDMSSyncStatus))
            {
                query.CSTSyncStatus = RequestDMSSyncStatus;
            }
            else
            {
                query.CSTSyncStatus = "170001,170002,170003,170008";
            }
            if (BLL.UserDataRigth.Instance.IsExistsByUserID(BLL.Util.GetLoginUserID()))
            {
                query.CSTMemberApplyUserID = Constant.INT_INVALID_VALUE;
            }
            else
            {
                query.CSTMemberApplyUserID = BLL.Util.GetLoginUserID();
            }
            if (!string.IsNullOrEmpty(RequestDMSStatus))
            {
                query.CSTStatus = RequestDMSStatus;
            }

            int count;
            DataTable dt = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMembersBySourceCC(query, "ApplyTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterList.DataSource = dt;
            repeaterList.DataBind();

            //AjaxPager_Member.InitPager(count);
            //selectDown.Value = RequestPageSize.ToString();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);

            CSTStatusCountStat(query, RecordCount);
        }

        private void CSTStatusCountStat(Entities.QueryProjectTask_CSTMember
 query, int allCount)
        {
            int applyForCount = 0;
            int createSuccessfulCount = 0;
            int createUnsuccessful = 0;
            int rejected = 0;
            BLL.ProjectTask_CSTMember.Instance.StatProjectTask_CSTMembersBySourceCC(query, out applyForCount, out createSuccessfulCount, out createUnsuccessful, out rejected);

            sml.InnerHtml = "总计：";
            sml.InnerHtml += allCount;
            sml.InnerHtml += "&nbsp;&nbsp;&nbsp;审批中：";
            sml.InnerHtml += applyForCount;
            sml.InnerHtml += "&nbsp;&nbsp;&nbsp;通过：";
            sml.InnerHtml += createSuccessfulCount;
            sml.InnerHtml += "&nbsp;&nbsp;&nbsp;失败：";
            sml.InnerHtml += createUnsuccessful;
            sml.InnerHtml += "&nbsp;&nbsp;&nbsp;打回：";
            sml.InnerHtml += rejected;
        }

        //查看、编辑链接
        protected string GetUrl(string SyncStatus, string ID, string TID, string status)
        {
            string url = "";
            if (SyncStatus == Convert.ToString((int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.CreateSuccessful) ||
                SyncStatus == Convert.ToString((int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.CreateUnsuccessful) ||
                SyncStatus == Convert.ToString((int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.ApplyFor) ||
                SyncStatus == Convert.ToString((int)BitAuto.YanFa.Crm2009.Entities.EnumCSTSyncStatus.CreateSuccessful) ||
                SyncStatus == Convert.ToString((int)BitAuto.YanFa.Crm2009.Entities.EnumCSTSyncStatus.CreateUnsuccessful) ||
                SyncStatus == Convert.ToString((int)BitAuto.YanFa.Crm2009.Entities.EnumCSTSyncStatus.ApplyFor))
            {
                url = @" <a target='_blank' href='/CustCheck/EditMemberDetail.aspx?TID=" + TID + "&MemberID=" + ID + "&Type=" + RequestType + "' target='_self'>查看</a>";
            }
            else
            {
                url = @" <a target='_blank' href='/CustCheck/EditMemberDetail.aspx?TID=" + TID + "&MemberID=" + ID + "&Type=" + RequestType + "' target='_self'>编辑</a>";
            }

            if (status == "-1")
            {
                url = "";
            }

            return url;
        }

        //
        protected string getStatus(string status)
        {
            string str = "";
            if (RequestType == "0")//选择车易通
            {
                str = (status == "0" ? "正常" : "删除");
            }
            else if (RequestType == "1")//选择车商通 1正常 -1删除 0禁用
            {
                str = (status == "1" ? "正常" : status == "-1" ? "删除" : "禁用");
            }
            return str;
        }
    }
}