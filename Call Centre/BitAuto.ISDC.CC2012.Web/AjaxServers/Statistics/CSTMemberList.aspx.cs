using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.BLL;
using System.Collections;
using BitAuto.Utils;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.Statistics
{
    public partial class CSTMemberList : PageBase
    {
        #region 定义属性
        //public string RequestShowDMSMemberPart
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("ShowDMSMemberPart"); }
        //}
        public string RequestCSTMemberCode
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CSTMemberCode"); }
        }
        public string RequestCSTMemberName
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CSTMemberName"); }
        }
        public string RequestProvinceID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("ProvinceID"); }
        }
        public string RequestCityID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CityID"); }
        }
        public string RequestCountyID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CountyID"); }
        }
        /// <summary>
        /// 所属交易市场，格式为：1,2,3,4
        /// </summary>
        public string RequestTFCustPids
        {
            get { return BLL.Util.GetCurrentRequestFormStr("TFCustPids"); }
        }

        /// <summary>
        /// 会员类别（3： 专业公司，1：个人，2:4S店，4：厂商，5：其他），格式为：1,2,3,4,5
        /// </summary>
        public string RequestMemberTypeIDs
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("MemberTypeIDs");
            }
        }
        /// <summary>
        /// 累计充值车商币开始范围
        /// </summary>
        public string RequestLoggingAmountStarts
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("LoggingAmountStarts");
            }
        }
        /// <summary>
        /// 累计充值车商币结束范围
        /// </summary>
        public string RequestLoggingAmountEnds
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("LoggingAmountEnds");
            }
        }

        /// <summary>
        /// 车商币余额开始范围
        /// </summary>
        public string RequestRemainAmountStarts
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("RemainAmountStarts");
            }
        }
        /// <summary>
        /// 车商币余额结束范围
        /// </summary>
        public string RequestRemainAmountEnds
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("RemainAmountEnds");
            }
        }

        /// <summary>
        /// 车商币有效期范围开始
        /// </summary>
        public string RequestAvailabilityTimeStarts
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("AvailabilityTimeStarts");
            }
        }
        /// <summary>
        /// 车商币有效期范围结束
        /// </summary>
        public string RequestAvailabilityTimeEnds
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("AvailabilityTimeEnds");
            }
        }

        /// <summary>
        /// 累计消费车商币范围开始
        /// </summary>
        public string RequestUserdAmountStarts
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("UserdAmountStarts");
            }
        }
        /// <summary>
        /// 累计消费车商币范围结束
        /// </summary>
        public string RequestUserdAmountEnds
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("UserdAmountEnds");
            }
        }
        /// <summary>
        /// 会员状态 add by yangyh 2013-08-26
        /// </summary>
        public string RequestMemberSyncStatus
        {
            get { return BLL.Util.GetCurrentRequestFormStr("MemberSyncStatus"); }
        }


        ///// <summary>
        ///// 创建会员状态（1：呼叫中心创建的会员，0：区域创建的会员），格式为：1,2,3,4
        ///// </summary>
        //public string RequestAddMemberFlag
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("AddMemberFlag"); }
        //}
        ///// <summary>
        ///// 负责员工（1：呼叫中心部门的负责员工，0：非呼叫中心部门的负责员工），格式为：1,2,3,4
        ///// </summary>
        //public string RequestUserMappingFlag
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("UserMappingFlag"); }
        //}
        ///// <summary>
        ///// 回访记录（1：呼叫中心部门的回访记录，0：非呼叫中心部门的回访记录），格式为：1,2,3,4
        ///// </summary>
        //public string RequestReturnVisitFlag
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("ReturnVisitFlag"); }
        //}
        /////// <summary>
        /////// 区域类型为1的值（1:86城）
        /////// </summary>
        ////public string RequestAreaType86_Value
        ////{
        ////    get { return BLL.Util.GetCurrentRequestFormStr("AreaType86_Value"); }
        ////}
        /////// <summary>
        /////// 86城的区域类型值（1:市区，2：郊区）
        /////// </summary>
        ////public string RequestAreaValue86
        ////{
        ////    get { return BLL.Util.GetCurrentRequestFormStr("AreaValue86"); }
        ////}
        /////// <summary>
        /////// 区域类型为1的值（2：246城）
        /////// </summary>
        ////public string RequestAreaType246_Value
        ////{
        ////    get { return BLL.Util.GetCurrentRequestFormStr("AreaType246_Value"); }
        ////}
        /////// <summary>
        /////// 会员销售类型
        /////// </summary>
        ////public int RequestMemberCooperateStatus
        ////{
        ////    get { return BLL.Util.GetCurrentRequestFormInt("MemberCooperateStatus"); }
        ////}
        ///// <summary>
        ///// 会员创建开始时间
        ///// </summary>
        //public string RequestMemberCreateTimeStart
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("MemberCreateTimeStart"); }
        //}
        ///// <summary>
        ///// 会员创建结束时间
        ///// </summary>
        //public string RequestMemberCreateTimeEnd
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("MemberCreateTimeEnd"); }
        //}
        ///// <summary>
        ///// 回访记录开始时间
        ///// </summary>
        //public string RequestReturnVisitTimeStart
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("ReturnVisitTimeStart"); }
        //}
        ///// <summary>
        ///// 回访记录结束时间
        ///// </summary>
        //public string RequestReturnVisitTimeEnd
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("ReturnVisitTimeEnd"); }
        //}
        ///// <summary>
        ///// 会员行政区划(1-163城区, 2-163郊区, 3-178无人城城区，3-178无人城郊区)
        ///// </summary>
        //public string RequestAreaTypeIDs
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("AreaTypeIDs"); }
        //}
        ///// <summary>
        ///// 会员主营品牌IDs
        ///// </summary>
        //public string RequestBrandIDs
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("BrandIDs"); }
        //}
        ///// <summary>
        ///// 曾经合作状态IDs,1--有排期，0--无排期
        ///// </summary>
        //public string CooperatedStatusIDs
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("CooperatedStatusIDs"); }
        //}
        ///// <summary>
        ///// 有排期开始时间
        ///// </summary>
        //public string RequestBeginMemberCooperatedTime
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("BeginMemberCooperatedTime"); }
        //}
        ///// <summary>
        ///// 有排期结束时间
        ///// </summary>
        //public string RequestEndMemberCooperatedTime
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("EndMemberCooperatedTime"); }
        //}
        ///// <summary>
        ///// 无排期开始时间
        ///// </summary>
        //public string RequestBeginNoMemberCooperatedTime
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("BeginNoMemberCooperatedTime"); }
        //}
        ///// <summary>
        ///// 无排期结束时间
        ///// </summary>
        //public string RequestEndNoMemberCooperatedTime
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("EndNoMemberCooperatedTime"); }
        //}
        ///// <summary>
        ///// 会员合作状态：销售或试用，如1,2
        ///// </summary>
        //public string RequestMemberCooperateStatus
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("MemberCooperateStatus"); }
        //}
        ///// <summary>
        ///// 会员类别，如1,2
        ///// </summary>
        //public string RequestMemberTypeIDs
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("MemberTypeIDs"); }
        //}

        //add by qizhiqiang 2012-4-13 为了判断查询来源
        /// <summary>
        /// 判断是否是统计查询,如果是为1
        /// </summary>
        public string RequestComeExcel
        {
            get { return BLL.Util.GetCurrentRequestFormStr("ComeExcel"); }
        }

        public int CountOfRecords = 0;
        public int PageSize = 20;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }


        private void BindData()
        {
            string areaTypeWhereStr = string.Empty;
            Entities.QueryCRMCSTMember query = new Entities.QueryCRMCSTMember();

            if (!string.IsNullOrEmpty(RequestCSTMemberCode) && RequestCSTMemberCode.Length > 0)
            {
                query.CSTMemberCode = RequestCSTMemberCode;
            }
            if (!string.IsNullOrEmpty(RequestCSTMemberName))
            {
                query.CSTMemberName = RequestCSTMemberName;
            }
            //if (!string.IsNullOrEmpty(RequestAreaType86_Value) ||
            //    RequestAreaValue86 != "-1" ||
            //    !string.IsNullOrEmpty(RequestAreaType246_Value))
            //{
            //    if (RequestAreaType86_Value.Equals("1"))
            //    {
            //        areaTypeWhereStr += " And " + (areaTypeWhereStr.Length > 0 || !RequestAreaType246_Value.Equals("2") ? "" : "(") +
            //                            "DMSMember.CityID IN (SELECT AreaID FROM AreaType WHERE Type=1 And Value=1) ";
            //    }
            //    if (RequestAreaValue86 != "-1")
            //    {
            //        areaTypeWhereStr += " And " + (areaTypeWhereStr.Length > 0 || !RequestAreaType246_Value.Equals("2") ? "" : "(") +
            //                            "DMSMember.CountyID IN (SELECT AreaID FROM AreaType WHERE Type=2 And Value=" + StringHelper.SqlFilter(RequestAreaValue86) + ") ";
            //    }
            //    if (RequestAreaType246_Value.Equals("2"))
            //    {
            //        string sqlWhere = " DMSMember.CityID IN (SELECT AreaID FROM AreaType WHERE Type=1 And Value=2)";
            //        if (areaTypeWhereStr.Length > 0)
            //        {
            //            areaTypeWhereStr += " OR " + sqlWhere + ")";
            //        }
            //        else
            //        {
            //            areaTypeWhereStr += " And " + sqlWhere;
            //        }
            //    }
            //    if (areaTypeWhereStr.Length > 0)
            //    {
            //        query.AreaTypeWhereStr = areaTypeWhereStr;
            //    }
            //}
            //else
            //{
            if (!string.IsNullOrEmpty(RequestProvinceID))
            {
                query.ProvinceID = RequestProvinceID;
            }
            if (!string.IsNullOrEmpty(RequestCityID))
            {
                query.CityID = RequestCityID;
            }
            if (!string.IsNullOrEmpty(RequestCountyID))
            {
                query.CountyID = RequestCountyID;
            }
            //所属交易市场
            if (!string.IsNullOrEmpty(RequestTFCustPids) && RequestTFCustPids.Length > 0)
            {
                query.TFCustPids = BLL.Util.GetStringBySplitArray(RequestTFCustPids); ;
            }
            //会员类别
            if (!string.IsNullOrEmpty(RequestMemberTypeIDs) && RequestMemberTypeIDs.Length > 0)
            {
                query.MemberTypeIDs = BLL.Util.GetStringBySplitArray(RequestMemberTypeIDs);
            }

            if (!string.IsNullOrEmpty(RequestLoggingAmountStarts) && RequestLoggingAmountStarts.Length > 0)
            {
                query.LoggingAmountStarts = RequestLoggingAmountStarts;
            }

            if (!string.IsNullOrEmpty(RequestLoggingAmountEnds) && RequestLoggingAmountEnds.Length > 0)
            {
                query.LoggingAmountEnds = RequestLoggingAmountEnds;
            }


            if (!string.IsNullOrEmpty(RequestRemainAmountStarts) && RequestRemainAmountStarts.Length > 0)
            {
                query.RemainAmountStarts = RequestRemainAmountStarts;
            }

            if (!string.IsNullOrEmpty(RequestRemainAmountEnds) && RequestRemainAmountEnds.Length > 0)
            {
                query.RemainAmountEnds = RequestRemainAmountEnds;
            }


            if (!string.IsNullOrEmpty(RequestAvailabilityTimeStarts) && RequestAvailabilityTimeStarts.Length > 0)
            {
                query.AvailabilityTimeStarts = RequestAvailabilityTimeStarts;
            }

            if (!string.IsNullOrEmpty(RequestAvailabilityTimeEnds) && RequestAvailabilityTimeEnds.Length > 0)
            {
                query.AvailabilityTimeEnds = RequestAvailabilityTimeEnds;
            }

            if (!string.IsNullOrEmpty(RequestUserdAmountStarts) && RequestUserdAmountStarts.Length > 0)
            {
                query.UserdAmountStarts = RequestUserdAmountStarts;
            }

            if (!string.IsNullOrEmpty(RequestUserdAmountEnds) && RequestUserdAmountEnds.Length > 0)
            {
                query.UserdAmountEnds = RequestUserdAmountEnds;
            }
            //add by yangyh date=2013-08-26
            if (!string.IsNullOrEmpty(RequestMemberSyncStatus) && RequestMemberSyncStatus != "-1")
            {
                query.MemberSyncStatus = RequestMemberSyncStatus;
            }

            //if (!string.IsNullOrEmpty(RequestMemberTypeIDs))
            //{
            //    query.MemberType = BLL.Util.GetStringBySplitArray(RequestMemberTypeIDs);
            //}
            //query.Status = 0;

            DataTable dt = BLL.CRMCSTMember.Instance.GetCC_CRMCSTMemberInfo(query, "CSTMember.CustID Desc", PageCommon.Instance.PageIndex, PageSize, out CountOfRecords);
            if (dt != null && dt.Rows.Count > 0)
            {
                this.Repeater_DMSMember.DataSource = dt;
            }
            //绑定列表数据
            this.Repeater_DMSMember.DataBind();
            //分页控件(要明确页面容器ID，这里没有写是因为请求页面在Request参数中声明了)
            this.AjaxPager_DMSMember.InitPager(this.CountOfRecords, "divQueryResultContent", PageSize);
        }
    }
}