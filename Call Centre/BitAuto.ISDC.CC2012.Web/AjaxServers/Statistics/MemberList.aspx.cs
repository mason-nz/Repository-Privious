using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using System.Collections;
using BitAuto.Utils;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.Statistics
{
    public partial class MemberList : PageBase
    {
        #region 定义属性
        public string RequestShowDMSMemberPart
        {
            get { return BLL.Util.GetCurrentRequestFormStr("ShowDMSMemberPart"); }
        }
        public string RequestDMSMemberCode
        {
            get { return BLL.Util.GetCurrentRequestFormStr("DMSMemberCode"); }
        }
        public string RequestDMSMemberName
        {
            get { return BLL.Util.GetCurrentRequestFormStr("DMSMemberName"); }
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
        /// 创建会员状态（1：呼叫中心创建的会员，0：区域创建的会员），格式为：1,2,3,4
        /// </summary>
        public string RequestAddMemberFlag
        {
            get { return BLL.Util.GetCurrentRequestFormStr("AddMemberFlag"); }
        }
        /// <summary>
        /// 负责员工（1：呼叫中心部门的负责员工，0：非呼叫中心部门的负责员工），格式为：1,2,3,4
        /// </summary>
        public string RequestUserMappingFlag
        {
            get { return BLL.Util.GetCurrentRequestFormStr("UserMappingFlag"); }
        }
        /// <summary>
        /// 回访记录（1：呼叫中心部门的回访记录，0：非呼叫中心部门的回访记录），格式为：1,2,3,4
        /// </summary>
        public string RequestReturnVisitFlag
        {
            get { return BLL.Util.GetCurrentRequestFormStr("ReturnVisitFlag"); }
        }
        ///// <summary>
        ///// 区域类型为1的值（1:86城）
        ///// </summary>
        //public string RequestAreaType86_Value
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("AreaType86_Value"); }
        //}
        ///// <summary>
        ///// 86城的区域类型值（1:市区，2：郊区）
        ///// </summary>
        //public string RequestAreaValue86
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("AreaValue86"); }
        //}
        ///// <summary>
        ///// 区域类型为1的值（2：246城）
        ///// </summary>
        //public string RequestAreaType246_Value
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("AreaType246_Value"); }
        //}
        ///// <summary>
        ///// 会员销售类型
        ///// </summary>
        //public int RequestMemberCooperateStatus
        //{
        //    get { return BLL.Util.GetCurrentRequestFormInt("MemberCooperateStatus"); }
        //}
        /// <summary>
        /// 会员创建开始时间
        /// </summary>
        public string RequestMemberCreateTimeStart
        {
            get { return BLL.Util.GetCurrentRequestFormStr("MemberCreateTimeStart"); }
        }
        /// <summary>
        /// 会员创建结束时间
        /// </summary>
        public string RequestMemberCreateTimeEnd
        {
            get { return BLL.Util.GetCurrentRequestFormStr("MemberCreateTimeEnd"); }
        }
        /// <summary>
        /// 回访记录开始时间
        /// </summary>
        public string RequestReturnVisitTimeStart
        {
            get { return BLL.Util.GetCurrentRequestFormStr("ReturnVisitTimeStart"); }
        }
        /// <summary>
        /// 回访记录结束时间
        /// </summary>
        public string RequestReturnVisitTimeEnd
        {
            get { return BLL.Util.GetCurrentRequestFormStr("ReturnVisitTimeEnd"); }
        }

        //add by qizq 2012-8-2排期确认时间

        /// <summary>
        /// 排期确认日期开始
        /// </summary>
        public string RequestConfirmDateStart
        {
            get { return BLL.Util.GetCurrentRequestFormStr("ConfirmDateStart"); }
        }
        /// <summary>
        /// 排期确认日期结束
        /// </summary>
        public string RequestConfirmDateEnd
        {
            get { return BLL.Util.GetCurrentRequestFormStr("ConfirmDateEnd"); }
        }

        /// <summary>
        /// 会员行政区划(1-163城区, 2-163郊区, 3-178无人城城区，3-178无人城郊区)
        /// </summary>
        public string RequestAreaTypeIDs
        {
            get { return BLL.Util.GetCurrentRequestStr("AreaTypeIDs"); }
        }

        public string SelectDeptID
        {
            get { return BLL.Util.GetCurrentRequestStr("selectDeptID"); }
        }

        public string StrDeptS
        {
            get { return BLL.Util.GetCurrentRequestStr("strDeptS"); }
        }
        /// <summary>
        /// 会员主营品牌IDs
        /// </summary>
        public string RequestBrandIDs
        {
            get { return BLL.Util.GetCurrentRequestStr("BrandIDs"); }
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
        /// 会员类别，如1,2
        /// </summary>
        public string RequestMemberTypeIDs
        {
            get { return BLL.Util.GetCurrentRequestStr("MemberTypeIDs"); }
        }

        //add by qizhiqiang 2012-4-13 为了判断查询来源
        /// <summary>
        /// 判断是否是统计查询,如果是为1
        /// </summary>
        public string RequestComeExcel
        {
            get { return BLL.Util.GetCurrentRequestFormStr("ComeExcel"); }
        }
        /// <summary>
        /// add by masj 2013-06-28
        /// 客户联系人职级
        /// </summary>
        public int RequestContactOfficeTypeCode
        {
            get { return BLL.Util.GetCurrentRequestFormInt("ContactOfficeTypeCode"); }
        }
        /// <summary>
        /// add by masj 2013-06-28
        /// 杂志投递周期
        /// </summary>
        public string RequestExecCycle
        {
            get { return BLL.Util.GetCurrentRequestFormStr("ExecCycle"); }
        }
        /// <summary>
        /// add by masj 2013-06-28
        /// 是否杂志投递
        /// </summary>
        public string RequestIsReturnMagazine
        {
            get { return BLL.Util.GetCurrentRequestFormStr("IsReturnMagazine"); }
        }
        /// <summary>
        /// 有排期开始时间段（开始）add by masj 2013-07-04
        /// </summary>
        public string RequestStartMemberCooperatedBeginTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("StartMemberCooperatedBeginTime"); }
        }
        /// <summary>
        /// 有排期开始时间段（结束）add by masj 2013-07-04
        /// </summary>
        public string RequestEndMemberCooperatedBeginTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("EndMemberCooperatedBeginTime"); }
        }
        /// <summary>
        /// 会员状态 add by yangyh 2013-08-26
        /// </summary>
        public string RequestMemberSyncStatus
        {
            get { return BLL.Util.GetCurrentRequestFormStr("MemberSyncStatus"); }
        }

        public int CountOfRecords = 0;
        public int PageSize = 20;
        #endregion

        public string showCustName = string.Empty;
        public bool right_Magazine = false;
        public bool right_Member = false;
        public bool right_Contact = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                right_Magazine = BLL.Util.CheckRight(userID, "SYS024BUG200301");
                right_Member = BLL.Util.CheckRight(userID, "SYS024BUG200302");
                right_Contact = BLL.Util.CheckRight(userID, "SYS024BUG200303");
                BindData();
            }
        }
        private void BindData()
        {
            string areaTypeWhereStr = string.Empty;
            Entities.QueryCRMDMSMember query = new Entities.QueryCRMDMSMember();

            if (!string.IsNullOrEmpty(RequestDMSMemberCode))
            {
                query.DMSMemberCode = RequestDMSMemberCode;
            }
            if (!string.IsNullOrEmpty(RequestDMSMemberName))
            {
                query.DMSMemberName = RequestDMSMemberName;
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
            //}
            //if (!string.IsNullOrEmpty(RequestAreaTypeIDs))
            //{
            //    query.AreaTypeIDs = RequestAreaTypeIDs;
            //}
            if (!string.IsNullOrEmpty(SelectDeptID))
            {
                query.SelectDeptID = SelectDeptID;
            }
            if (!string.IsNullOrEmpty(StrDeptS))
            {
                if (StrDeptS.Trim() != "1,0" && StrDeptS.Trim() != "1,0")
                {
                    query.StrDeptS = StrDeptS;
                }
            }

            if (RequestAddMemberFlag.Equals("1"))
            {
                query.IsCCCreate = true;
            }
            else if (RequestAddMemberFlag.Equals("0"))
            {
                query.IsCCCreate = false;
            }

            if (RequestUserMappingFlag.Equals("1"))
            {
                query.IsCCUserMapping = true;
            }
            else if (RequestUserMappingFlag.Equals("0"))
            {
                query.IsCCUserMapping = false;
            }

            if (RequestReturnVisitFlag.Equals("1"))
            {
                query.IsCCReturnVisit = true;
            }
            else if (RequestReturnVisitFlag.Equals("0"))
            {
                query.IsCCReturnVisit = false;
            }

            //if (RequestMemberCooperateStatus >= 0)
            //{
            //    query.MemberCooperateStatus = RequestMemberCooperateStatus;
            //}
            if (!string.IsNullOrEmpty(RequestMemberCreateTimeStart))
            {
                query.MemberCreateTimeStart = RequestMemberCreateTimeStart;
            }
            if (!string.IsNullOrEmpty(RequestMemberCreateTimeEnd))
            {
                query.MemberCreateTimeEnd = RequestMemberCreateTimeEnd;
            }
            if (!string.IsNullOrEmpty(RequestReturnVisitTimeStart))
            {
                query.ReturnVisitTimeStart = RequestReturnVisitTimeStart;
            }
            if (!string.IsNullOrEmpty(RequestReturnVisitTimeEnd))
            {
                query.ReturnVisitTimeEnd = RequestReturnVisitTimeEnd;
            }

            //add by qizq 2012-8-2排期确认时间
            if (!string.IsNullOrEmpty(RequestConfirmDateStart))
            {
                query.ConfirmDateStart = RequestConfirmDateStart;
            }
            if (!string.IsNullOrEmpty(RequestConfirmDateEnd))
            {
                query.ConfirmDateEnd = RequestConfirmDateEnd;
            }



            if (!string.IsNullOrEmpty(RequestBrandIDs))
            {
                query.BrandIDs = BLL.Util.GetStringBySplitArray(RequestBrandIDs);
            }

            if (CooperatedStatusIDs != string.Empty)
            {
                query.CooperatedStatusIDs = CooperatedStatusIDs;
            }
            if (!string.IsNullOrEmpty(RequestBeginMemberCooperatedTime))
            {
                query.BeginMemberCooperatedTime = RequestBeginMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestEndMemberCooperatedTime))
            {
                query.EndMemberCooperatedTime = RequestEndMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestBeginNoMemberCooperatedTime))
            {
                query.BeginNoMemberCooperatedTime = RequestBeginNoMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestEndNoMemberCooperatedTime))
            {
                query.EndNoMemberCooperatedTime = RequestEndNoMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestMemberCooperateStatus))
            {
                query.MemberCooperateStatus = RequestMemberCooperateStatus;
            }
            if (!string.IsNullOrEmpty(RequestMemberTypeIDs))
            {
                query.MemberType = BLL.Util.GetStringBySplitArray(RequestMemberTypeIDs);
            }
            if (!string.IsNullOrEmpty(RequestIsReturnMagazine))
            {
                if (RequestIsReturnMagazine == "0")
                {
                    query.IsMagazineReturn = 0;
                }
                else if (RequestIsReturnMagazine == "1")
                {
                    query.IsMagazineReturn = 1;
                    if (!string.IsNullOrEmpty(RequestExecCycle))
                    {
                        query.ExecCycle = RequestExecCycle;
                    }
                }
            }
            if (RequestContactOfficeTypeCode > 0)
            {
                query.CustContactOfficeTypeCode = RequestContactOfficeTypeCode;
            }

            if (!string.IsNullOrEmpty(RequestStartMemberCooperatedBeginTime) ||
               !string.IsNullOrEmpty(RequestEndMemberCooperatedBeginTime))
            {
                query.CooperatedStatusIDs = "1";
                if (!string.IsNullOrEmpty(RequestStartMemberCooperatedBeginTime))
                {
                    query.StartMemberCooperatedBeginTime = RequestStartMemberCooperatedBeginTime.Trim();
                }
                if (!string.IsNullOrEmpty(RequestEndMemberCooperatedBeginTime))
                {
                    query.EndMemberCooperatedBeginTime = RequestEndMemberCooperatedBeginTime.Trim();
                }
            }
            if (!string.IsNullOrEmpty(RequestMemberSyncStatus) && RequestMemberSyncStatus != "-1")
            {
                query.MemberSyncStatus = RequestMemberSyncStatus;
            }

            query.Status = 0;

            DataTable dt = BLL.CRMDMSMember.Instance.GetCC_CRMDMSMemberInfo(query, "DMSMember.CustID Desc", PageCommon.Instance.PageIndex, PageSize, query.SelectDeptID, out CountOfRecords);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("showCustName");

                dt.Columns.Add("titleCustName");

                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    string[] depts = departName(dt.Rows[k]["CustID"].ToString());

                    dt.Rows[k]["showCustName"] = depts[1];

                    dt.Rows[k]["titleCustName"] = depts[0];

                }


                this.Repeater_DMSMember.DataSource = DealWithByTable(dt);
            }
            //绑定列表数据
            this.Repeater_DMSMember.DataBind();
            //分页控件(要明确页面容器ID，这里没有写是因为请求页面在Request参数中声明了)
            this.AjaxPager_DMSMember.InitPager(this.CountOfRecords, "divQueryResultContent", PageSize);
        }


        private DataTable DealWithByTable(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                //dt.Columns.Add("MemberRecordIsTrail", typeof(string));//销售类型
                dt.Columns.Add("MemberLevelName", typeof(string));//易湃会员版本
                dt.Columns.Add("MemberRecordTime", typeof(string));//执行周期
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["MemberCode"].ToString()))
                    {
                        DataTable memberDt = null;
                        memberDt = BitAuto.YanFa.Crm2009.BLL.CYTMember.Instance.GetCYTMemberLastByMemberCode(dr["MemberCode"].ToString(), 1);
                        dr["MemberLevelName"] = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetMemberTypeText(dr["MemberType"].ToString());
                        if (memberDt != null && memberDt.Rows.Count > 0)
                        {
                            //string memberLevelName = memberDt.Rows[0]["todaysMemberRankName"].ToString().Trim();
                            string memberRecordBeginTime = memberDt.Rows[0]["begintime"].ToString().Trim();
                            string memberRecordEndTime = memberDt.Rows[0]["endtime"].ToString().Trim();

                            //dr["MemberLevelName"] = memberLevelName;
                            dr["MemberRecordTime"] = getStrTime(memberRecordBeginTime, memberRecordEndTime);
                        }
                    }
                }
            }
            return dt;
        }


        ///// <summary>
        ///// 原来通过webservice调用方法获取会员最后版本号和开始结束时间
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <returns></returns>
        //private DataTable DealWithByTable(DataTable dt)
        //{
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        //dt.Columns.Add("MemberRecordIsTrail", typeof(string));//销售类型
        //        dt.Columns.Add("MemberLevelName", typeof(string));//易湃会员版本
        //        dt.Columns.Add("MemberRecordTime", typeof(string));//执行周期
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            DataTable memberDt = null;
        //            DataSet ds = SysRightManager.Common.API.DMS.DMSWebServers.Instance.GetMemberRecordsByIDs(dr["MemberCode"].ToString());
        //            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                memberDt = ds.Tables[0];

        //                Hashtable ht = new Hashtable();
        //                DataTable dtTemp = new DataTable();
        //                memberDt.DefaultView.Sort = "DealerID,MemberRecordCreateTime Desc";
        //                dtTemp = memberDt.DefaultView.ToTable();

        //                memberDt.Rows.Clear();
        //                //string memberRecordIsTrail = dtTemp.Rows[0]["MemberRecordIsTrail"].ToString().Trim();
        //                string memberLevelName = dtTemp.Rows[0]["MemberLevelName"].ToString().Trim();
        //                string dealerID = dtTemp.Rows[0]["DealerID"].ToString().Trim();
        //                string memberRecordBeginTime = dtTemp.Rows[0]["MemberRecordBeginTime"].ToString().Trim();
        //                string memberRecordEndTime = dtTemp.Rows[0]["MemberRecordEndTime"].ToString().Trim();

        //                //dr["MemberRecordIsTrail"] = (memberRecordIsTrail == "1" ? "试用" : "销售");
        //                dr["MemberLevelName"] = memberLevelName;
        //                dr["MemberRecordTime"] = getStrTime(dealerID, memberRecordBeginTime, memberRecordEndTime);
        //            }
        //        }
        //    }
        //    return dt;
        //}

        protected string getStrTime(string MemberRecordBeginTime, string MemberRecordEndTime)
        {
            string s = "";
            try
            {
                //if (!string.IsNullOrEmpty(adordercode))
                //{
                //    s = contractPeriod;
                //}
                //if (!string.IsNullOrEmpty(DealerID))
                //{
                s = Convert.ToDateTime(MemberRecordBeginTime).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(MemberRecordEndTime).ToString("yyyy-MM-dd");
                //}

            }
            catch (Exception)
            {


            }
            //if (string.IsNullOrEmpty(adordercode) && string.IsNullOrEmpty(DealerID))
            //{
            //    s = DateTime.Parse(MemberRecordCreateTime).ToString("yyyy年MM月dd日");
            //}
            return s;
        }

        protected string GetMemberCooperateStatus(object cooperateStatus)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(cooperateStatus)))
            {
                switch (Convert.ToString(cooperateStatus))
                {
                    case "1":
                        return "销售";
                    case "2":
                        return "试用";
                    case "0":
                        return "无合作";
                    default:
                        return "";
                }
            }
            return "";
        }

        /// <summary>
        /// 根据CustID获取负责部门名称
        /// </summary>
        /// <param name="custID">客户ID</param>
        /// <returns>数组[0]-部门名；数组[1]显示的部门名（最多9个字）</returns>
        public string[] departName(string custID)
        {
            string[] dept = new string[2];
            dept[0] = dept[1] = BitAuto.YanFa.Crm2009.BLL.CustDepartMapping.Instance.GetCustDepartNamesByCustID(custID);

            if (dept[0].Length > 9)
            {
                dept[1] = dept[0].Substring(0, 9) + "...";
            }

            return dept;
        }

        //protected string getMemberRecordIsTrailstr(string MemberRecordIsTrail, string DealerID, string adordercode)
        //{
        //    if (!string.IsNullOrEmpty(adordercode))
        //    {
        //        switch (MemberRecordIsTrail)
        //        {
        //            case "4001":
        //                return "销售";
        //            case "4002":
        //                return "互换";
        //            case "4003":
        //                return "配送";
        //            case "4004":
        //                return "自用";
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(DealerID))
        //    {
        //        return MemberRecordIsTrail == "1" ? "试用" : "销售";
        //    }
        //    return "";
        //}
    }
}
