using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;
using BitAuto.ISDC.CC2012.Web.Util;

namespace BitAuto.ISDC.CC2012.Web.CustCheck
{
    public class CustCheckHelper
    {
        #region Query Properties
        private string custId;
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustID
        {
            get
            {
                if (custId == null)
                {
                    custId = HttpUtility.UrlDecode((Request["CustID"] + "").Trim());
                }
                return custId;
            }
        }
        private string custName;
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustName
        {
            get
            {
                if (custName == null)
                {
                    custName = HttpUtility.UrlDecode((Request["CustName"] + "").Trim());
                }
                return custName;
            }
        }

        private string abbrName;
        /// <summary>
        /// 客户简称
        /// </summary>
        public string AbbrName
        {
            get
            {
                if (abbrName == null)
                {
                    abbrName = HttpUtility.UrlDecode((Request["AbbrName"] + "").Trim());
                }
                return abbrName;
            }
        }

        private string provinceID;
        /// <summary>
        /// 省份
        /// </summary>
        public string ProvinceID
        {
            get
            {
                if (provinceID == null)
                {
                    provinceID = HttpUtility.UrlDecode((Request["ProvinceID"] + "").Trim());
                }
                return provinceID;
            }
        }

        private string provinceName;
        /// <summary>
        /// 省份
        /// </summary>
        public string ProvinceName
        {
            get
            {
                if (provinceName == null)
                {
                    provinceName = HttpUtility.UrlDecode((Request["ProvinceName"] + "").Trim());
                }
                return provinceName;
            }
        }

        private string cityID;
        /// <summary>
        /// 城市
        /// </summary>
        public string CityID
        {
            get
            {
                if (cityID == null)
                {
                    cityID = HttpUtility.UrlDecode((Request["CityID"] + "").Trim());
                }
                return cityID;
            }
        }

        private string cityName;
        /// <summary>
        /// 城市
        /// </summary>
        public string CityName
        {
            get
            {
                if (cityName == null)
                {
                    cityName = HttpUtility.UrlDecode((Request["CityName"] + "").Trim());
                }
                return cityName;
            }
        }
        private string address;
        /// <summary>
        /// 城市
        /// </summary>
        public string Address
        {
            get
            {
                if (address == null)
                {
                    address = HttpUtility.UrlDecode((Request["Address"] + "").Trim());
                }
                return address;
            }
        }
        private string trademarketid;
        /// <summary>
        /// 城市
        /// </summary>
        public string TradeMarketID
        {
            get
            {
                if (trademarketid == null)
                {
                    trademarketid = HttpUtility.UrlDecode((Request["TradeMarketID"] + "").Trim());
                }
                return trademarketid;
            }
        }

        private string countyID;
        /// <summary>
        /// 区县
        /// </summary>
        public string CountyID
        {
            get
            {
                if (countyID == null)
                {
                    countyID = HttpUtility.UrlDecode((Request["CountyID"] + "").Trim());
                }
                return countyID;
            }
        }

        private string countyName;
        /// <summary>
        /// 区县
        /// </summary>
        public string CountyName
        {
            get
            {
                if (countyName == null)
                {
                    countyName = HttpUtility.UrlDecode((Request["CountyName"] + "").Trim());
                }
                return countyName;
            }
        }

        private string brandIDs;
        /// <summary>
        /// 
        /// </summary>
        public string BrandIDs
        {
            get
            {
                if (brandIDs == null)
                {
                    brandIDs = HttpUtility.UrlDecode((Request["BrandIDs"] + "").Trim());
                }
                return brandIDs;
            }
        }

        private string brandName;
        /// <summary>
        /// 
        /// </summary>
        public string BrandName
        {
            get
            {
                if (brandName == null)
                {
                    brandName = HttpUtility.UrlDecode((Request["BrandName"] + "").Trim());
                }
                return brandName;
            }
        }

        private string startTime;
        /// <summary>
        /// 
        /// </summary>
        public string StartTime
        {
            get
            {
                if (startTime == null)
                {
                    startTime = HttpUtility.UrlDecode((Request["StartTime"] + "").Trim());
                }
                return startTime;
            }
        }

        private string endTime;
        /// <summary>
        /// 
        /// </summary>
        public string EndTime
        {
            get
            {
                if (endTime == null)
                {
                    endTime = HttpUtility.UrlDecode((Request["EndTime"] + "").Trim());
                }
                return endTime;
            }
        }
        /// <summary>
        /// 创建客户开始日期
        /// </summary>
        public string RequestCreateCustStartDate
        {
            get { return BLL.Util.GetCurrentRequestStr("CreateCustStartDate"); }
        }
        /// <summary>
        /// 创建客户结束日期
        /// </summary>
        public string RequestCreateCustEndDate
        {
            get { return BLL.Util.GetCurrentRequestStr("CreateCustEndDate"); }
        }

        private string noDeal;
        /// <summary>
        /// 
        /// </summary>
        public string StatusNoManage
        {
            get
            {
                if (noDeal == null)
                {
                    noDeal = HttpUtility.UrlDecode((Request["NoDeal"] + "").Trim());
                }
                return noDeal;
            }
        }

        private string pending;
        /// <summary>
        /// 
        /// </summary>
        public string StatusManaging
        {
            get
            {
                if (pending == null)
                {
                    pending = HttpUtility.UrlDecode((Request["Pending"] + "").Trim());
                }
                return pending;
            }
        }

        private string dealt;
        /// <summary>
        /// 
        /// </summary>
        public string StatusManageFinsh
        {
            get
            {
                if (dealt == null)
                {
                    dealt = HttpUtility.UrlDecode((Request["Dealt"] + "").Trim());
                }
                return dealt;
            }
        }

        private string additionalStatus;
        /// <summary>
        /// 附加状态
        /// </summary>
        public string AdditionalStatus
        {
            get
            {
                if (additionalStatus == null)
                {
                    additionalStatus = HttpUtility.UrlDecode((Request["AdditionalStatus"] + "").Trim());
                }
                return additionalStatus;
            }
        }

        private string tID;
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TID
        {
            get
            {
                if (tID == null)
                {
                    tID = HttpUtility.UrlDecode((Request["TID"] + "").Trim());
                }
                return tID;
            }
        }

        /// <summary>
        /// 任务批次
        /// </summary>
        public int TaskBatch
        {
            get { return BLL.Util.GetCurrentRequestInt("TaskBatch"); }
        }

        private string queryParams;
        /// <summary>
        /// 查询参数
        /// </summary>
        public string QueryParams
        {
            get
            {
                if (queryParams == null)
                {
                    queryParams = HttpUtility.UrlDecode((Request["QueryParams"] + "").Trim());
                }
                return queryParams;
            }
        }

        private string callRecordsCount;
        /// <summary>
        /// 通话次数
        /// </summary>
        public string CallRecordsCount
        {
            get
            {
                if (callRecordsCount == null)
                {
                    callRecordsCount = HttpUtility.UrlDecode((Request["CallRecordsCount"] + "").Trim());
                }
                return callRecordsCount;
            }
        }


        private string crmcuststatus;
        /// <summary>
        /// 客户状态
        /// </summary>
        public string CrmCustStatus
        {
            get
            {
                if (crmcuststatus == null)
                {
                    crmcuststatus = HttpUtility.UrlDecode((Request["CustStatus"] + "").Trim());
                }
                return crmcuststatus;
            }
        }
        private string crmcusthavemember;
        /// <summary>
        /// 客户有会员标识
        /// </summary>
        public string CrmCustHaveMember
        {
            get
            {
                if (crmcusthavemember == null)
                {
                    crmcusthavemember = HttpUtility.UrlDecode((Request["CustHaveMember"] + "").Trim());
                }
                return crmcusthavemember;
            }
        }
        private string crmcusthavenomember;
        /// <summary>
        /// 客户无会员标识
        /// </summary>
        public string CrmCustHaveNoMember
        {
            get
            {
                if (crmcusthavenomember == null)
                {
                    crmcusthavenomember = HttpUtility.UrlDecode((Request["CustHaveNoMember"] + "").Trim());
                }
                return crmcusthavenomember;
            }
        }

        private string datasource;
        /// <summary>
        /// 客户数据来源（1-Excel导入，2-CRM库）
        /// </summary>
        public string DataSource
        {
            get
            {
                if (datasource == null)
                {
                    datasource = HttpUtility.UrlDecode((Request["DataSource"] + "").Trim());
                }
                return datasource;
            }
        }
        private string crmtypeid;
        /// <summary>
        /// 客户状态
        /// </summary>
        public string CrmTypeID
        {
            get
            {
                if (crmtypeid == null)
                {
                    crmtypeid = HttpUtility.UrlDecode((Request["Type"] + "").Trim());
                }
                return crmtypeid;
            }
        }
        private string cartype;
        /// <summary>
        /// 经营范围
        /// </summary>
        public string CarType
        {
            get
            {
                if (cartype == null)
                {
                    cartype = HttpUtility.UrlDecode((Request["CarType"] + "").Trim());
                }
                return cartype;
            }
        }

        //add by qizq  2012-6-8添加客户类别
        private string custtype;
        /// <summary>
        /// 客户类型
        /// </summary>
        public string CustType
        {
            get
            {
                if (custtype == null)
                {
                    custtype = HttpUtility.UrlDecode((Request["CustType"] + "").Trim());
                }
                return custtype;
            }
        }
        //


        #endregion

        #region Common Properties

        private int currentPage = -1;
        public int CurrentPage
        {
            get
            {
                if (currentPage <= 0) { currentPage = PagerHelper.GetCurrentPage(); }
                return currentPage;
            }
            set { currentPage = value; }
        }

        private int pageSize = -1;
        public int PageSize
        {
            get
            {
                if (pageSize <= 0) { pageSize = PagerHelper.GetPageSize(); }
                return pageSize;
            }
            set { pageSize = value; }
        }

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string Action { get { return (Request["Action"] + "").Trim().ToLower(); } }

        #endregion

        /// <summary>
        /// 得到坐席要核实的“新增客户”数据
        /// </summary>
        public DataTable GetNewCustCheckRecordsByUserID(bool isManager, out int totalCount)
        {
            Entities.QueryExcelCustInfo query = new Entities.QueryExcelCustInfo();
            if (CustName != string.Empty)
            {
                query.CustName = CustName;
            }
            if (BrandName != string.Empty)
            {
                query.BrandName = BrandName;
            }
            if (string.IsNullOrEmpty(this.ProvinceID) == false && this.ProvinceID != "-1")
            {
                query.ProvinceName = ProvinceName;
            }
            if (string.IsNullOrEmpty(this.CityID) == false && this.CityID != "-1")
            {
                query.CityName = CityName;
            }
            if (string.IsNullOrEmpty(this.CountyID) == false && this.CountyID != "-1")
            {
                query.CountyName = CountyName;
            }
            DateTime dtime;
            if (string.IsNullOrEmpty(this.StartTime) == false && DateTime.TryParse(this.StartTime, out dtime))
            {
                query.LastUpdateTime_StartTime = dtime;
            }
            if (string.IsNullOrEmpty(this.EndTime) == false && DateTime.TryParse(this.EndTime, out dtime))
            {
                query.LastUpdateTime_EndTime = dtime;
            }
            query.StatusNoManage = this.StatusNoManage == "1" ? true : false;
            query.StatusManaging = this.StatusManaging == "1" ? true : false;
            query.StatusManageFinsh = this.StatusManageFinsh == "1" ? true : false;
            if (query.StatusManaging == true)
            {
                query.AdditionalStatus = this.AdditionalStatus;
            }
            if (!isManager)
            {
                query.UserIDAssigned = BLL.Util.GetLoginUserID();
            }
            else
            {
                query.UserIDAssigned = 0;
            }
            if (!string.IsNullOrEmpty(CallRecordsCount))
            {
                query.CallRecordsCount = StringHelper.SqlFilter(CallRecordsCount);
            }
            int tid = -1;
            if (!string.IsNullOrEmpty(TID))
            {
                query.PTID = TID;
            }
            if (!string.IsNullOrEmpty(CarType))
            {
                query.CarType = CarType;
            }

            //add by qizq  2012-6-8客户类别
            if (!string.IsNullOrEmpty(CustType))
            {
                query.TypeID = CustType;
            }

            return BLL.ExcelCustInfo.Instance.GetExcelCustInfo_Manage(query, this.CurrentPage, this.PageSize, out totalCount);

        }

        /// <summary>
        /// 统计 坐席要核实的“新增客户”数据
        /// </summary>
        public void StatNewCustCheckRecordsByUserID(bool isManager, out int totalCount, out  int noProcessCount, out int processingCount, out  int finishedCount)
        {
            Entities.QueryExcelCustInfo query = new Entities.QueryExcelCustInfo();
            if (CustName != string.Empty)
            {
                query.CustName = CustName;
            }
            if (BrandName != string.Empty)
            {
                query.BrandName = BrandName;
            }
            if (string.IsNullOrEmpty(this.ProvinceID) == false && this.ProvinceID != "-1")
            {
                query.ProvinceName = ProvinceName;
            }
            if (string.IsNullOrEmpty(this.CityID) == false && this.CityID != "-1")
            {
                query.CityName = CityName;
            }
            if (string.IsNullOrEmpty(this.CountyID) == false && this.CountyID != "-1")
            {
                query.CountyName = CountyName;
            }
            DateTime dtime;
            if (string.IsNullOrEmpty(this.StartTime) == false && DateTime.TryParse(this.StartTime, out dtime))
            {
                query.LastUpdateTime_StartTime = dtime;
            }
            if (string.IsNullOrEmpty(this.EndTime) == false && DateTime.TryParse(this.EndTime, out dtime))
            {
                query.LastUpdateTime_EndTime = dtime;
            }
            query.StatusNoManage = this.StatusNoManage == "1" ? true : false;
            query.StatusManaging = this.StatusManaging == "1" ? true : false;
            query.StatusManageFinsh = this.StatusManageFinsh == "1" ? true : false;
            if (query.StatusManaging == true)
            {
                query.AdditionalStatus = this.AdditionalStatus;
            }
            if (!isManager)
            {
                query.UserIDAssigned = BLL.Util.GetLoginUserID();
            }
            else
            {
                query.UserIDAssigned = 0;
            }
            if (!string.IsNullOrEmpty(CallRecordsCount))
            {
                query.CallRecordsCount = StringHelper.SqlFilter(CallRecordsCount);
            }
            if (!string.IsNullOrEmpty(TID))
            {
                query.PTID = TID;
            }
            if (!string.IsNullOrEmpty(CarType))
            {
                query.CarType = CarType;
            }

            //add by qizq  2012-6-8客户类别
            if (!string.IsNullOrEmpty(CustType))
            {
                query.TypeID = CustType;
            }

            BLL.ExcelCustInfo.Instance.StatNewCustCheckRecordsByUserID(query, out totalCount, out  noProcessCount, out processingCount, out  finishedCount);
        }

        /// <summary>
        /// 得到坐席要核实的“CRM客户”数据
        /// </summary>
        public DataTable GetCrmCustCheckRecordsByUserID(bool isManager, out int totalCount)
        {
            Entities.QueryCrmCustInfo query = new QueryCrmCustInfo();
            if (CustID != string.Empty)
            {
                query.CustID = CustID;
            }
            if (CustName != string.Empty)
            {
                query.CustName = CustName;
            }
            if (BrandIDs != string.Empty)
            {
                query.Brandids = this.BrandIDs;
            }
            if (this.ProvinceID != string.Empty && this.ProvinceID != "-1")
            {
                query.ProvinceID = this.ProvinceID;
            }
            if (CityID != string.Empty && this.CityID != "-1")
            {
                query.CityID = CityID;
            }
            if (CountyID != string.Empty && CountyID != "-1")
            {
                query.CountyID = CountyID;
            }
            if (AbbrName != string.Empty)
            {
                query.AbbrName = AbbrName;
            }
            DateTime dtime;
            if (string.IsNullOrEmpty(this.StartTime) == false && DateTime.TryParse(this.StartTime, out dtime))
            {
                query.LastUpdateTime_StartTime = dtime;
            }
            if (string.IsNullOrEmpty(this.EndTime) == false && DateTime.TryParse(this.EndTime, out dtime))
            {
                query.LastUpdateTime_EndTime = dtime;
            }

            query.StatusNoManage = this.StatusNoManage == "1" ? true : false;
            query.StatusManaging = this.StatusManaging == "1" ? true : false;
            query.StatusManageFinsh = this.StatusManageFinsh == "1" ? true : false;
            if (query.StatusManaging == true)
            {
                query.AdditionalStatus = this.AdditionalStatus;
            }
            if (!query.StatusNoManage && !query.StatusManaging && !query.StatusManageFinsh)
            {
                query.StatusNoManage = true;
                query.StatusManaging = true;
                query.StatusManageFinsh = true;
            }
            if (!isManager)
            {
                query.UserIDAssigned = BLL.Util.GetLoginUserID();
            }
            else
            {
                query.UserIDAssigned = Constant.INT_INVALID_VALUE;
            }
            query.TaskType = 2;
            if (!string.IsNullOrEmpty(CallRecordsCount))
            {
                query.CallRecordsCount = StringHelper.SqlFilter(CallRecordsCount);
            }
            int tid = -1;
            if (!string.IsNullOrEmpty(TID))
            {
                query.TID = int.TryParse(TID, out tid) ? tid : -1;
            }
            if (!string.IsNullOrEmpty(CarType))
            {
                query.CarType = CarType;
            }
            if (TaskBatch > 0)
            {
                query.Batch = TaskBatch;
            }
            query.TaskSource = 2;//任务数据来源，1-Excel，2-CRM

            //add by qizq  2012-6-8客户类别
            if (!string.IsNullOrEmpty(CustType))
            {
                query.TypeID = CustType;
            }

            return BLL.CrmCustInfo.Instance.GetCC_CrmCustInfo(query, "tk.PTID desc", this.CurrentPage, this.PageSize, out totalCount);

        }

        /// <summary>
        /// 统计 坐席要核实的“CRM客户”数据
        /// </summary>
        internal void StatCrmCustCheckRecordsByUserID(bool isManager, out int totalCount, out  int noProcessCount, out int processingCount, out  int finishedCount)
        {
            Entities.QueryCrmCustInfo query = new QueryCrmCustInfo();
            if (CustID != string.Empty)
            {
                query.CustID = CustID;
            }
            if (CustName != string.Empty)
            {
                query.CustName = CustName;
            }
            if (AbbrName != string.Empty)
            {
                query.AbbrName = AbbrName;
            }
            if (BrandIDs != string.Empty)
            {
                query.Brandids = this.BrandIDs;
            }
            if (this.ProvinceID != string.Empty && this.ProvinceID != "-1")
            {
                query.ProvinceID = this.ProvinceID;
            }
            if (CityID != string.Empty && this.CityID != "-1")
            {
                query.CityID = CityID;
            }
            if (CountyID != string.Empty && CountyID != "-1")
            {
                query.CountyID = CountyID;
            }
            DateTime dtime;
            if (string.IsNullOrEmpty(this.StartTime) == false && DateTime.TryParse(this.StartTime, out dtime))
            {
                query.LastUpdateTime_StartTime = dtime;
            }
            if (string.IsNullOrEmpty(this.EndTime) == false && DateTime.TryParse(this.EndTime, out dtime))
            {
                query.LastUpdateTime_EndTime = dtime;
            }
            query.StatusNoManage = this.StatusNoManage == "1" ? true : false;
            query.StatusManaging = this.StatusManaging == "1" ? true : false;
            query.StatusManageFinsh = this.StatusManageFinsh == "1" ? true : false;
            if (query.StatusManaging == true)
            {
                query.AdditionalStatus = this.AdditionalStatus;
            }
            if (!query.StatusNoManage && !query.StatusManaging && !query.StatusManageFinsh)
            {
                query.StatusNoManage = true;
                query.StatusManaging = true;
                query.StatusManageFinsh = true;
            }
            if (!isManager)
            {
                query.UserIDAssigned = BLL.Util.GetLoginUserID();
            }
            else
            {
                query.UserIDAssigned = Constant.INT_INVALID_VALUE;
            }
            query.TaskType = 2;
            if (!string.IsNullOrEmpty(CallRecordsCount))
            {
                query.CallRecordsCount = StringHelper.SqlFilter(CallRecordsCount);
            }
            if (!string.IsNullOrEmpty(CarType))
            {
                query.CarType = StringHelper.SqlFilter(CarType);
            }
            int tid = -1;
            if (!string.IsNullOrEmpty(TID))
            {
                query.TID = int.TryParse(TID, out tid) ? tid : -1;
            }
            if (TaskBatch > 0)
            {
                query.Batch = TaskBatch;
            }

            //add by qizq  2012-6-8客户类别
            if (!string.IsNullOrEmpty(CustType))
            {
                query.TypeID = CustType;
            }
            BLL.ProjectTaskInfo.Instance.StatProjectTaskInfo(2, query, out totalCount, out noProcessCount, out processingCount, out finishedCount);
        }

        /// <summary>
        /// 得到状态描述
        /// </summary>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public static string GetStatusDescription(string taskStatus, string additionalStatus)
        {
            if (taskStatus == "180001") { return "处理中" + (string.IsNullOrEmpty(additionalStatus) ? string.Empty : "(" + additionalStatus.Substring(3, 1) + ")"); }
            else if (taskStatus == "180002") { return "处理中(审核拒绝)"; }
            else if (taskStatus == "180009") { return "处理中(审核驳回)"; }
            else if (taskStatus == "180003" || taskStatus == "180004" || taskStatus == "180005" || taskStatus == "180006" || taskStatus == "180007" || taskStatus == "180008" || taskStatus == "180010" || taskStatus == "180011") { return "已处理"; }
            else if (taskStatus == "180000") { return "未处理"; }
            else { return "未分配"; }
        }
    }
}
