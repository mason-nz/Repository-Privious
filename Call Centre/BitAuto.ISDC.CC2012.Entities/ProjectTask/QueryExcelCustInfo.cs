using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryExcelCustInfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryExcelCustInfo
    {

        private int _id;
        private string _custname;
        private string _typename;
        private string _typenames;
        private string _industryname;
        private string _provincename;
        private string _cityname;
        private string _countyname;
        private string _zipcode;
        private string _parentcust;
        private string _brandname;
        private string _contactname;
        private string _officetel;
        private string _membername;
        private string _memberabbr;
        private string _membersalephonenum;
        private string _membersaleaddress;
        private string _memberbrandname;
        private string _memberbrandgroup;
        private string _trueName;
        private string _callrecordscount;
        private string _ptid;
        private string _cartype;
        private string _typeid;

        public QueryExcelCustInfo()
        {
            _id = Constant.INT_INVALID_VALUE;
            _custname = Constant.STRING_EMPTY_VALUE;
            _typename = Constant.STRING_EMPTY_VALUE;
            _typenames = Constant.STRING_EMPTY_VALUE;
            _industryname = Constant.STRING_EMPTY_VALUE;
            _provincename = Constant.STRING_EMPTY_VALUE;
            _cityname = Constant.STRING_EMPTY_VALUE;
            _countyname = Constant.STRING_EMPTY_VALUE;
            _zipcode = Constant.STRING_EMPTY_VALUE;
            _parentcust = Constant.STRING_EMPTY_VALUE;
            _brandname = Constant.STRING_EMPTY_VALUE;
            _contactname = Constant.STRING_EMPTY_VALUE;
            _officetel = Constant.STRING_EMPTY_VALUE;
            _membername = Constant.STRING_EMPTY_VALUE;
            _memberabbr = Constant.STRING_EMPTY_VALUE;
            _membersalephonenum = Constant.STRING_EMPTY_VALUE;
            _membersaleaddress = Constant.STRING_EMPTY_VALUE;
            _memberbrandname = Constant.STRING_EMPTY_VALUE;
            _memberbrandgroup = Constant.STRING_EMPTY_VALUE;
            _trueName = Constant.STRING_EMPTY_VALUE;

            userIDAssigned = Constant.INT_INVALID_VALUE;

            lastUpdateTime_StartTime = Constant.DATE_INVALID_VALUE;
            lastUpdateTime_EndTime = Constant.DATE_INVALID_VALUE;

            str_AdditionalStatus = Constant.STRING_INVALID_VALUE;
            _callrecordscount = Constant.STRING_INVALID_VALUE;
            _ptid = Constant.STRING_INVALID_VALUE;
            _cartype = Constant.STRING_EMPTY_VALUE;
            _typeid = Constant.STRING_EMPTY_VALUE;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustName
        {
            set { _custname = value; }
            get { return _custname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TypeName
        {
            set { _typename = value; }
            get { return _typename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TypeNames
        {
            set { _typenames = value; }
            get { return _typenames; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IndustryName
        {
            set { _industryname = value; }
            get { return _industryname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProvinceName
        {
            set { _provincename = value; }
            get { return _provincename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CityName
        {
            set { _cityname = value; }
            get { return _cityname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CountyName
        {
            set { _countyname = value; }
            get { return _countyname; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Zipcode
        {
            set { _zipcode = value; }
            get { return _zipcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ParentCust
        {
            set { _parentcust = value; }
            get { return _parentcust; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BrandName
        {
            set { _brandname = value; }
            get { return _brandname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContactName
        {
            set { _contactname = value; }
            get { return _contactname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OfficeTel
        {
            set { _officetel = value; }
            get { return _officetel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MemberName
        {
            set { _membername = value; }
            get { return _membername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MemberAbbr
        {
            set { _memberabbr = value; }
            get { return _memberabbr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MemberSalePhoneNum
        {
            set { _membersalephonenum = value; }
            get { return _membersalephonenum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MemberSaleAddress
        {
            set { _membersaleaddress = value; }
            get { return _membersaleaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MemberBrandName
        {
            set { _memberbrandname = value; }
            get { return _memberbrandname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MemberBrandGroup
        {
            set { _memberbrandgroup = value; }
            get { return _memberbrandgroup; }
        }

        /// <summary>
        /// 经营范围
        /// </summary>
        public string CarType
        {
            set { _cartype = value; }
            get { return _cartype; }
        }

        private bool statusNoManage = false;
        /// <summary>
        /// 未处理
        /// </summary>
        public bool StatusNoManage { get { return statusNoManage; } set { statusNoManage = value; } }

        private bool statusManaging = false;
        /// <summary>
        /// 处理中
        /// </summary>
        public bool StatusManaging { get { return statusManaging; } set { statusManaging = value; } }

        private bool statusManageFinsh = false;
        /// <summary>
        /// 已处理
        /// </summary>
        public bool StatusManageFinsh { get { return statusManageFinsh; } set { statusManageFinsh = value; } }

        private bool statusNoAssign = false;
        /// <summary>
        /// 未分配
        /// </summary>
        public bool StatusNoAssign { get { return statusNoAssign; } set { statusNoAssign = value; } }

        private int userIDAssigned;
        /// <summary>
        /// 分配的用户ID
        /// </summary>
        public int UserIDAssigned { get { return userIDAssigned; } set { userIDAssigned = value; } }

        public string TrueName
        {
            get { return _trueName; }
            set { _trueName = value; }
        }


        private DateTime lastUpdateTime_StartTime;
        /// <summary>
        /// 开始时间
        public DateTime LastUpdateTime_StartTime
        {
            get { return lastUpdateTime_StartTime; }
            set { lastUpdateTime_StartTime = value; }
        }

        private DateTime lastUpdateTime_EndTime;
        /// <summary>
        /// 结束时间
        public DateTime LastUpdateTime_EndTime
        {
            get { return lastUpdateTime_EndTime; }
            set { lastUpdateTime_EndTime = value; }
        }

        private string str_AdditionalStatus;
        /// <summary>
        /// 附加状态
        /// </summary>
        public string AdditionalStatus
        {
            get { return str_AdditionalStatus; }
            set { str_AdditionalStatus = value; }
        }
        /// <summary>
        /// 通话次数查询
        /// </summary>
        public string CallRecordsCount
        {
            get { return _callrecordscount; }
            set { _callrecordscount = value; }
        }

        /// <summary>
        /// 任务ID
        /// </summary>
        public string PTID
        {
            get { return _ptid; }
            set { _ptid = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TypeID
        {
            set { _typeid = value; }
            get { return _typeid; }
        }
    }

}


