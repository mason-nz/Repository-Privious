using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.WebUtil;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    /// <summary>
    /// GetCustGetIDs 的摘要说明
    /// </summary>
    public class GetCustGetIDs : IHttpHandler, IRequiresSessionState
    {

        #region  属性定义


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
        ///// <summary>
        ///// 有会员信息
        ///// </summary>
        //public bool IsHavMember
        //{
        //    get { return Request["CustHaveMember"] == null ? false : Convert.ToBoolean(Request["CustHaveMember"]); }
        //}
        ///// <summary>
        ///// 无会员信息
        ///// </summary>
        //public bool IsNoHavMember
        //{
        //    get { return Request["CustHaveNoMember"] == null ? false : Convert.ToBoolean(Request["CustHaveNoMember"]); }
        //}
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
            get { return BLL.Util.GetCurrentRequestFormStr("BeginMemberCooperatedTime"); }
        }
        /// <summary>
        /// 有排期结束时间
        /// </summary>
        public string RequestEndMemberCooperatedTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("EndMemberCooperatedTime"); }
        }
        /// <summary>
        /// 无排期开始时间
        /// </summary>
        public string RequestBeginNoMemberCooperatedTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("BeginNoMemberCooperatedTime"); }
        }
        /// <summary>
        /// 无排期结束时间
        /// </summary>
        public string RequestEndNoMemberCooperatedTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("EndNoMemberCooperatedTime"); }
        }
        /// <summary>
        /// 会员合作状态：销售或试用，如1,2
        /// </summary>
        public string RequestMemberCooperateStatus
        {
            get { return BLL.Util.GetCurrentRequestFormStr("MemberCooperateStatus"); }
        }
        /// <summary>
        /// 客户创建开始时间
        /// </summary>
        public string RequestCreateCustStartDate
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CreateCustStartDate"); }
        }
        /// <summary>
        /// 客户创建结束时间
        /// </summary>
        public string RequestCreateCustEndDate
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CreateCustEndDate"); }
        }
        /// <summary>
        /// 客户行政区划(1-163城区, 2-163郊区, 3-178无人城城区，3-178无人城郊区)
        /// </summary>
        public string RequestAreaTypeIDs
        {
            get { return BLL.Util.GetCurrentRequestFormStr("AreaTypeIDs"); }
        }

        /// <summary>
        /// 集采项目名
        /// </summary>
        public string RequestProjectName
        {
            get { return BLL.Util.GetCurrentRequestFormStr("ProjectName"); }
        }

        /// <summary>
        /// 有排期开始时间段（开始）
        /// </summary>
        public string RequestStartMemberCooperatedBeginTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("StartMemberCooperatedBeginTime"); }
        }
        /// <summary>
        /// 有排期开始时间段（结束）
        /// </summary>
        public string RequestEndMemberCooperatedBeginTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("EndMemberCooperatedBeginTime"); }
        }

        public int GroupLength = 8;
        public int RecordCount = 0;
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string JsonStr = GetList();

            context.Response.Write(JsonStr);
        }

        private string GetList()
        {
            QueryCrmCustInfo queryCustInfo = new QueryCrmCustInfo();

            #region 条件

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
            //if (IsHavMember)
            //{
            //    queryCustInfo.IsHaveMember = true;
            //}
            //if (IsNoHavMember)
            //{
            //    queryCustInfo.IsHaveNoMember = true;
            //}

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
            if (!string.IsNullOrEmpty(RequestAreaTypeIDs))
            {
                queryCustInfo.AreaTypeIDs = RequestAreaTypeIDs;
            }

            if (!string.IsNullOrEmpty(RequestProjectName))
            {
                queryCustInfo.ProjectName = RequestProjectName.Trim();
            }
            if (!string.IsNullOrEmpty(RequestStartMemberCooperatedBeginTime) ||
                !string.IsNullOrEmpty(RequestEndMemberCooperatedBeginTime))
            {
                queryCustInfo.CooperatedStatusIDs = "1";
                if (!string.IsNullOrEmpty(RequestStartMemberCooperatedBeginTime))
                {
                    queryCustInfo.StartMemberCooperatedBeginTime = RequestStartMemberCooperatedBeginTime.Trim();
                }
                if (!string.IsNullOrEmpty(RequestEndMemberCooperatedBeginTime))
                {
                    queryCustInfo.EndMemberCooperatedBeginTime = RequestEndMemberCooperatedBeginTime.Trim();
                }
            }
            #endregion

            DataTable dt = BLL.CrmCustInfo.Instance.GetCC_CrmCustIDsByAlone(queryCustInfo);

            List<string> colList = new List<string>();
            colList.Add("CustID");
            string jsonStr = Converter.DataTable2Json(dt, colList);
            return jsonStr;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}