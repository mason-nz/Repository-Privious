using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.Statistics
{
    public partial class CrmCustList : PageBase
    {
        public string SysUrl = System.Configuration.ConfigurationManager.AppSettings["SysUrl"].ToString();
        #region  属性定义
        public string RequestPageSize
        {
            get { return Request["PageSize"] == null ? PageCommon.Instance.PageSize.ToString() : Request["PageSize"].Trim(); }
        }
        public string EventStr
        {
            get { return Request["Search"] == null ? string.Empty : Request["Search"].ToString().Trim(); }
        }
        public string CustID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustID"); }
        }
        public string Address
        {
            get { return BLL.Util.GetCurrentRequestFormStr("Address"); }
        }
        public string TradeMarketID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("TradeMarketID"); }
        }
        public string CustName
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustName"); }
        }
        public string CustAbbr
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustAbbr"); }
        }
        public string BrandIDs
        {
            get { return BLL.Util.GetCurrentRequestFormStr("BrandIDs"); }
        }
        public int ProvinceID
        {
            get { return BLL.Util.GetCurrentRequestFormInt("ProvinceID"); }
        }
        public int CityID
        {
            get { return BLL.Util.GetCurrentRequestFormInt("CityID"); }
        }
        public int CountyID
        {
            get { return BLL.Util.GetCurrentRequestFormInt("CountyID"); }
        }
        public string CustStatus
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustStatus"); }
        }

        /// <summary>
        /// 有会员信息
        /// </summary>
        public bool IsHavMember
        {
            get { return Request["CustHaveMember"] == null ? false : Convert.ToBoolean(Request["CustHaveMember"]); }
        }
        /// <summary>
        /// 无会员信息
        /// </summary>
        public bool IsNoHavMember
        {
            get { return Request["CustHaveNoMember"] == null ? false : Convert.ToBoolean(Request["CustHaveNoMember"]); }
        }
        public string CustType
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustType"); }
        }
        public string CarType
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CarType"); }
        }
        public string CustLockStatus
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustLockStatus"); }
        }
        public string DataSource
        {
            get { return BLL.Util.GetCurrentRequestFormStr("DataSource"); }
        }
        /// <summary>
        /// 合作状态IDs
        /// </summary>
        public string CooperateStatusIDs
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CooperateStatusIDs"); }
        }
        /// <summary>
        /// 曾经合作状态IDs,1--有排期，0--无排期
        /// </summary>
        public string CooperatedStatusIDs
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CooperatedStatusIDs"); }
        }
        /// <summary>
        /// 有排期开始时间
        /// </summary>
        public string RequestBeginMemberCooperatedTime
        {
            get { return BLL.Util.GetCurrentRequestStr("BeginMemberCooperatedTime"); }
        }
        /// <summary>
        /// 有排期结束时间
        /// </summary>
        public string RequestEndMemberCooperatedTime
        {
            get { return BLL.Util.GetCurrentRequestStr("EndMemberCooperatedTime"); }
        }
        /// <summary>
        /// 无排期开始时间
        /// </summary>
        public string RequestBeginNoMemberCooperatedTime
        {
            get { return BLL.Util.GetCurrentRequestStr("BeginNoMemberCooperatedTime"); }
        }
        /// <summary>
        /// 无排期结束时间
        /// </summary>
        public string RequestEndNoMemberCooperatedTime
        {
            get { return BLL.Util.GetCurrentRequestStr("EndNoMemberCooperatedTime"); }
        }
        /// <summary>
        /// 会员合作状态：销售或试用，如1,2
        /// </summary>
        public string RequestMemberCooperateStatus
        {
            get { return BLL.Util.GetCurrentRequestStr("MemberCooperateStatus"); }
        }
        /// <summary>
        /// 客户创建开始时间
        /// </summary>
        public string RequestCreateCustStartDate
        {
            get { return BLL.Util.GetCurrentRequestStr("CreateCustStartDate"); }
        }
        /// <summary>
        /// 客户创建结束时间
        /// </summary>
        public string RequestCreateCustEndDate
        {
            get { return BLL.Util.GetCurrentRequestStr("CreateCustEndDate"); }
        }

        public string RequestDistrictName
        {
            get { return BLL.Util.GetCurrentRequestStr("DistrictName"); }
        }

        public string WpUrl = System.Configuration.ConfigurationManager.AppSettings["WpUrl"].ToString();
        public int GroupLength = 8;
        public int RecordCount = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                rptDataBind();
            }
        }
        private void rptDataBind()
        {
            QueryCrmCustInfo queryCustInfo = new QueryCrmCustInfo();
            if (CustID != string.Empty)
            {
                queryCustInfo.CustID = CustID;
            }
            if (CustName != string.Empty)
            {
                queryCustInfo.CustName = CustName;
            }
            if (CustAbbr != string.Empty)
            {
                queryCustInfo.AbbrName = CustAbbr;
            }
            if (BrandIDs != string.Empty)
            {
                queryCustInfo.BrandID = BrandIDs;
            }
            if (CarType != string.Empty)
            {
                queryCustInfo.CarType = CarType;
            }
            if (ProvinceID > 0)
            {
                queryCustInfo.ProvinceID = ProvinceID.ToString();
            }
            if (CityID > 0)
            {
                queryCustInfo.CityID = CityID.ToString();
            }
            if (CountyID > 0)
            {
                queryCustInfo.CountyID = CountyID.ToString();
            }
            if (IsHavMember)
            {
                queryCustInfo.IsHaveMember = true;
            }
            if (IsNoHavMember)
            {
                queryCustInfo.IsHaveNoMember = true;
            }

            if (!string.IsNullOrEmpty(CustType))
            {
                queryCustInfo.TypeID = CustType;
            }
            if (!string.IsNullOrEmpty(Address))
            {
                queryCustInfo.Address = Address;
            }
            if (!string.IsNullOrEmpty(TradeMarketID))
            {
                queryCustInfo.TradeMarketID = TradeMarketID;
            }

            if (CustStatus != string.Empty && CustStatus != "0,1")
            {
                queryCustInfo.StatusIDs = CustStatus;
            }
            if (CooperateStatusIDs != string.Empty)
            {
                queryCustInfo.CooperateStatusIDs = CooperateStatusIDs;
            }
            if (CooperatedStatusIDs != string.Empty)
            {
                queryCustInfo.CooperatedStatusIDs = CooperatedStatusIDs;
            }
            if (CustLockStatus != string.Empty && CustLockStatus != "0,1")
            {
                queryCustInfo.Lock = int.Parse(CustLockStatus);
            }
            if (DataSource != string.Empty && DataSource != "1,0")
            {
                queryCustInfo.TaskType = int.Parse(DataSource);
            }
            if (!string.IsNullOrEmpty(RequestBeginMemberCooperatedTime))
            {
                queryCustInfo.BeginMemberCooperatedTime = RequestBeginMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestEndMemberCooperatedTime))
            {
                queryCustInfo.EndMemberCooperatedTime = RequestEndMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestBeginNoMemberCooperatedTime))
            {
                queryCustInfo.BeginNoMemberCooperatedTime = RequestBeginNoMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestEndNoMemberCooperatedTime))
            {
                queryCustInfo.EndNoMemberCooperatedTime = RequestEndNoMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestMemberCooperateStatus))
            {
                queryCustInfo.MemberCooperateStatus = RequestMemberCooperateStatus;
            }
            if (!string.IsNullOrEmpty(RequestCreateCustStartDate))
            {
                queryCustInfo.CreateTimeStart = RequestCreateCustStartDate;
            }
            if (!string.IsNullOrEmpty(RequestCreateCustEndDate))
            {
                queryCustInfo.CreateTimeEnd = RequestCreateCustEndDate;
            }
            if (!string.IsNullOrEmpty(RequestDistrictName))
            {
                queryCustInfo.DistrictName = RequestDistrictName;
            }
            DataTable dt = BLL.CrmCustInfo.Instance.GetCC_CrmCustInfoByAlone(queryCustInfo, "", PageCommon.Instance.PageIndex, PageCommon.Instance.PageSize, out RecordCount);
            Repeater_Custs.DataSource = dt;
            Repeater_Custs.DataBind();

            this.AjaxPager_Custs.InitPager(RecordCount);
        }

        protected string getDMSMember(string CustID)
        {
            string memberIDs = "";
            return memberIDs;
        }

        public string ctrLen(string id)
        {
            string str = id;

            if (id.Length > 40)
            {
                str = id.Substring(0, 40) + "...";
            }

            return str;
        }
    }
}