using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryCrmCustInfo
    {
        #region Model
        private string _custid;
        private string _custname;
        private string _abbrname;
        private string _custengname;
        private string _licensenumber;
        private string _levelid;
        private string _industryid;
        private string _typeid;
        private string _producerid;
        private string _pid;
        private string _shoplevel;
        private string _biztype;
        private string _brandid;
        private string _saleorgid;
        private string _provinceid;
        private string _cityid;
        private string _address;
        private string _officetel;
        private string _fax;
        private string _statusids;
        private string _notes;
        private string _url;
        private string _zipcode;
        private string _contactname;
        private string _createtime;
        private string _lastupdatetime;
        private int _createuserid;
        private int _lastupdateuserid;
        private int _ishaveuser;
        private int _ishavedepart;
        private string _createtimestart;
        private string _createtimeend;
        private string _existCustID;
        private string _saleorgids;
        private string _existCustName;
        private int _userID;
        private string _departID;
        private int _lock;
        private string _custpid;
        private bool isCompany;
        private bool _containChildDepart;
        private string _brandids;

        private string _CountyID;
        private string _trueName;
        private string _callrecordscount;
        private bool _IsHaveMember = false;
        private bool _IsHaveNoMember = false;
        private string _cooperatestatusids;
        private string _cooperatedstatusids;
        private int _batch;
        private int _tid;
        private string _beginmembercooperatedtime;
        private string _endmembercooperatedtime;
        private string _beginnomembercooperatedtime;
        private string _endnomembercooperatedtime;
        private string _membercooperatestatus;
        private int _tasksource;
        private string _areatypeids;
        private string _cartype;

        private string _createsource;

        private int _ismagazinereturn;
        private string _execcycle;

        private string _projectname;
        private string _startmembercooperatedbegintime;
        private string _endmembercooperatedbegintime;
        private string _membersyncstatus;

        private string _districtname;
        /// <summary>
        /// 
        /// </summary>
        public string CustID
        {
            set { _custid = value; }
            get { return _custid; }
        }
        public string ExistCustID
        {
            set { _existCustID = value; }
            get { return _existCustID; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustName
        {
            set { _custname = value; }
            get { return _custname; }
        }
        public string ExistCustName
        {
            set { _existCustName = value; }
            get { return _existCustName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AbbrName
        {
            set { _abbrname = value; }
            get { return _abbrname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustEngName
        {
            set { _custengname = value; }
            get { return _custengname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LicenseNumber
        {
            set { _licensenumber = value; }
            get { return _licensenumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LevelID
        {
            set { _levelid = value; }
            get { return _levelid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IndustryID
        {
            set { _industryid = value; }
            get { return _industryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TypeID
        {
            set { _typeid = value; }
            get { return _typeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProducerID
        {
            set { _producerid = value; }
            get { return _producerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Pid
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShopLevel
        {
            set { _shoplevel = value; }
            get { return _shoplevel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BizType
        {
            set { _biztype = value; }
            get { return _biztype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BrandID
        {
            set { _brandid = value; }
            get { return _brandid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SaleOrgID
        {
            set { _saleorgid = value; }
            get { return _saleorgid; }
        }
        public string SaleOrgIDS
        {
            set { _saleorgids = value; }
            get { return _saleorgids; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProvinceID
        {
            set { _provinceid = value; }
            get { return _provinceid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CityID
        {
            set { _cityid = value; }
            get { return _cityid; }
        }
        public string CountyID
        {
            set { _CountyID = value; }
            get { return _CountyID; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Officetel
        {
            set { _officetel = value; }
            get { return _officetel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Fax
        {
            set { _fax = value; }
            get { return _fax; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StatusIDs
        {
            set { _statusids = value; }
            get { return _statusids; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Notes
        {
            set { _notes = value; }
            get { return _notes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string zipcode
        {
            set { _zipcode = value; }
            get { return _zipcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string contactName
        {
            set { _contactname = value; }
            get { return _contactname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int LastUpdateUserID
        {
            set { _lastupdateuserid = value; }
            get { return _lastupdateuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsHaveUser
        {
            set { _ishaveuser = value; }
            get { return _ishaveuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsHaveDepart
        {
            set { _ishavedepart = value; }
            get { return _ishavedepart; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateTimeStart
        {
            set { _createtimestart = value; }
            get { return _createtimestart; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateTimeEnd
        {
            set { _createtimeend = value; }
            get { return _createtimeend; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        /// <summary>
        /// 部门ID
        /// </summary>
        public string DepartID
        {
            get { return _departID; }
            set { _departID = value; }
        }
        public int Lock
        {
            set { _lock = value; }
            get { return _lock; }
        }
        public string CustPid
        {
            set { _custpid = value; }
            get { return _custpid; }
        }
        /// <summary>
        /// true 是经销商false不是经销商
        /// </summary>
        public bool IsCompany
        {
            set { isCompany = value; }
            get { return isCompany; }
        }
        public bool ContainChildDepart
        {
            set { _containChildDepart = value; }
            get { return _containChildDepart; }
        }
        public string Brandids
        {
            set { _brandids = value; }
            get { return _brandids; }
        }
        public bool IsHaveMember
        {
            set { _IsHaveMember = value; }
            get { return _IsHaveMember; }
        }
        public bool IsHaveNoMember
        {
            set { _IsHaveNoMember = value; }
            get { return _IsHaveNoMember; }
        }

        public string DistrictName
        {
            set { _districtname = value; }
            get { return _districtname; }
        }
        #endregion Model


        public QueryCrmCustInfo()
        {
            isCompany = false;
            _custid = Constant.STRING_INVALID_VALUE;
            _custname = Constant.STRING_INVALID_VALUE;
            _abbrname = Constant.STRING_INVALID_VALUE;
            _custengname = Constant.STRING_INVALID_VALUE;
            _licensenumber = Constant.STRING_INVALID_VALUE;
            _levelid = Constant.STRING_INVALID_VALUE;
            _industryid = Constant.STRING_INVALID_VALUE;
            _typeid = Constant.STRING_INVALID_VALUE;
            _producerid = Constant.STRING_INVALID_VALUE;
            _pid = Constant.STRING_INVALID_VALUE;
            _shoplevel = Constant.STRING_INVALID_VALUE;
            _biztype = Constant.STRING_INVALID_VALUE;
            _brandid = Constant.STRING_INVALID_VALUE;
            _saleorgid = Constant.STRING_INVALID_VALUE;
            _provinceid = Constant.STRING_INVALID_VALUE;
            _cityid = Constant.STRING_INVALID_VALUE;
            _address = Constant.STRING_INVALID_VALUE;
            _officetel = Constant.STRING_INVALID_VALUE;
            _fax = Constant.STRING_INVALID_VALUE;
            _statusids = Constant.STRING_INVALID_VALUE;
            _notes = Constant.STRING_INVALID_VALUE;
            _url = Constant.STRING_INVALID_VALUE;
            _zipcode = Constant.STRING_INVALID_VALUE;
            _contactname = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.STRING_INVALID_VALUE;
            _lastupdatetime = Constant.STRING_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _lastupdateuserid = Constant.INT_INVALID_VALUE;
            _ishaveuser = Constant.INT_INVALID_VALUE;
            _ishavedepart = Constant.INT_INVALID_VALUE;
            _createtimestart = Constant.STRING_INVALID_VALUE;
            _createtimeend = Constant.STRING_INVALID_VALUE;
            _existCustID = Constant.STRING_INVALID_VALUE;
            _saleorgids = Constant.STRING_INVALID_VALUE;
            _existCustName = Constant.STRING_INVALID_VALUE;
            _userID = Constant.INT_INVALID_VALUE;
            _departID = Constant.STRING_INVALID_VALUE;
            _lock = Constant.INT_INVALID_VALUE;
            _custpid = Constant.STRING_INVALID_VALUE;
            _containChildDepart = false;
            _brandids = Constant.STRING_INVALID_VALUE;
            _CountyID = Constant.STRING_INVALID_VALUE;

            userIDAssigned = Constant.INT_INVALID_VALUE;
            taskType = Constant.INT_INVALID_VALUE;
            _trueName = Constant.STRING_INVALID_VALUE;

            lastUpdateTime_StartTime = Constant.DATE_INVALID_VALUE;
            lastUpdateTime_EndTime = Constant.DATE_INVALID_VALUE;
            _callrecordscount = Constant.STRING_INVALID_VALUE;
            _cooperatestatusids = Constant.STRING_INVALID_VALUE;
            _cooperatedstatusids = Constant.STRING_INVALID_VALUE;
            _batch = Constant.INT_INVALID_VALUE;
            _tid = Constant.INT_INVALID_VALUE;

            _beginmembercooperatedtime = Constant.STRING_INVALID_VALUE;
            _endmembercooperatedtime = Constant.STRING_INVALID_VALUE;
            _beginnomembercooperatedtime = Constant.STRING_INVALID_VALUE;
            _endnomembercooperatedtime = Constant.STRING_INVALID_VALUE;
            _membercooperatestatus = Constant.STRING_INVALID_VALUE;
            _tasksource = Constant.INT_INVALID_VALUE;
            _areatypeids = Constant.STRING_INVALID_VALUE;
            _cartype = Constant.STRING_INVALID_VALUE;
            _ismagazinereturn = Constant.INT_INVALID_VALUE;
            _execcycle = Constant.STRING_EMPTY_VALUE;
            _trademarketid = Constant.STRING_EMPTY_VALUE;
            _createsource = Constant.STRING_INVALID_VALUE;
            _projectname = Constant.STRING_INVALID_VALUE;
            _dmsmemberbrandid = Constant.STRING_INVALID_VALUE;  //易湃会员主营品牌 add lxw 2013-1-8
            _startmembercooperatedbegintime = Constant.STRING_INVALID_VALUE;//有排期开始时间段（开始） add=masj date=2013-07-04
            _endmembercooperatedbegintime = Constant.STRING_INVALID_VALUE;//有排期开始时间段（结束） add=masj date=2013-07-04
            _membersyncstatus = Constant.STRING_INVALID_VALUE;//会员同步状态 add=yangyh date=2013-08-23

            _districtname = Constant.STRING_INVALID_VALUE;
        }


        //private List<int> bLAnnualSurveyStatus = new List<int>();
        ///// <summary>
        ///// 年检记录状态
        ///// </summary>
        //public List<int> BLAnnualSurveyStatus { get { return bLAnnualSurveyStatus; } set { bLAnnualSurveyStatus = value; } }

        /*
        private bool aSAvailable = true;
        /// <summary>
        /// 年检通过
        /// </summary>
        public bool ASAvailable { get { return aSAvailable; } set { aSAvailable = value; } }

        private bool aSInvalid = true;
        /// <summary>
        /// 年检未通过
        /// </summary>
        public bool ASInvalid { get { return aSInvalid; } set { aSInvalid = value; } }

        private bool aSNone = true;
        /// <summary>
        /// 没年检记录
        /// </summary>
        public bool ASNone { get { return aSNone; } set { aSNone = value; } }
        */

        //private string starLevel;
        ///// <summary>
        ///// 信用星级
        ///// </summary>
        //public string StarLevel { get { return starLevel; } set { starLevel = value; } }

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

        private int taskType;
        /// <summary>
        /// 任务类型（分配任务时不需要，查看已分配的任务时可能需要）
        /// </summary>
        public int TaskType { get { return taskType; } set { taskType = value; } }

        /// <summary>
        /// 负责员工
        /// </summary>
        public string TrueName { get { return _trueName; } set { _trueName = value; } }


        private DateTime lastUpdateTime_StartTime;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime LastUpdateTime_StartTime
        {
            get { return lastUpdateTime_StartTime; }
            set { lastUpdateTime_StartTime = value; }
        }

        private DateTime lastUpdateTime_EndTime;
        /// <summary>
        /// 结束时间
        /// </summary>
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

        private string _trademarketid;
        /// <summary>
        /// 附加状态
        /// </summary>
        public string TradeMarketID
        {
            get { return _trademarketid; }
            set { _trademarketid = value; }
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
        /// 合作状态 1==========会员合作中
        ///          2==========无会员合作
        /// </summary>
        public string CooperateStatusIDs
        {
            get { return _cooperatestatusids; }
            set { _cooperatestatusids = value; }
        }

        /// <summary>
        /// 合作状态 1==========曾经合作过（不包括当天）
        ///          0==========曾经未合作过（不包括当天）
        /// </summary>
        public string CooperatedStatusIDs
        {
            get { return _cooperatedstatusids; }
            set { _cooperatedstatusids = value; }
        }

        /// <summary>
        /// 任务批次
        /// </summary>
        public int Batch
        {
            set { _batch = value; }
            get { return _batch; }
        }


        /// <summary>
        /// 任务ID
        /// </summary>
        public int TID
        {
            get { return _tid; }
            set { _tid = value; }
        }

        /// <summary>
        /// 有排期——（开始时间）
        /// </summary>
        public string BeginMemberCooperatedTime
        {
            get { return _beginmembercooperatedtime; }
            set { _beginmembercooperatedtime = value; }
        }

        /// <summary>
        /// 有排期——（结束时间）
        /// </summary>
        public string EndMemberCooperatedTime
        {
            get { return _endmembercooperatedtime; }
            set { _endmembercooperatedtime = value; }
        }

        /// <summary>
        /// 无排期——（开始时间）
        /// </summary>
        public string BeginNoMemberCooperatedTime
        {
            get { return _beginnomembercooperatedtime; }
            set { _beginnomembercooperatedtime = value; }
        }

        /// <summary>
        /// 无排期——（结束时间）
        /// </summary>
        public string EndNoMemberCooperatedTime
        {
            get { return _endnomembercooperatedtime; }
            set { _endnomembercooperatedtime = value; }
        }

        /// <summary>
        /// 会员合作状态：销售或试用，如1,2
        /// </summary>
        public string MemberCooperateStatus
        {
            get { return _membercooperatestatus; }
            set { _membercooperatestatus = value; }
        }

        /// <summary>
        /// //任务数据来源，1-Excel，2-CRM
        /// </summary>
        public int TaskSource
        {
            get { return _tasksource; }
            set { _tasksource = value; }
        }

        /// <summary>
        /// 客户行政区划(1-163城区, 2-163郊区, 3-178无人城城区，3-178无人城郊区)
        /// </summary>
        public string AreaTypeIDs
        {
            set { _areatypeids = value; }
            get { return _areatypeids; }
        }

        /// <summary>
        /// 客户经营范围0—未知，1—新车，2—二手车，3—新车和二手车，默认为0
        /// </summary>
        public string CarType
        {
            set { _cartype = value; }
            get { return _cartype; }
        }

        /// <summary>
        /// 是否投寄杂志
        /// </summary>
        public int IsMagazineReturn
        {
            set { _ismagazinereturn = value; }
            get { return _ismagazinereturn; }
        }

        /// <summary>
        /// 杂志期数
        /// </summary>
        public string ExecCycle
        {
            set { _execcycle = value; }
            get { return _execcycle; }
        }

        public string CreateSource
        {
            set { _createsource = value; }
            get { return _createsource; }
        }

        private string _dmsmemberbrandid;

        public string DMSMemberBrandID
        {
            get { return _dmsmemberbrandid; }
            set { _dmsmemberbrandid = value; }
        }


        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }
        /// <summary>
        /// 有排期开始时间段（开始）
        /// </summary>
        public string StartMemberCooperatedBeginTime
        {
            set { _startmembercooperatedbegintime = value; }
            get { return _startmembercooperatedbegintime; }
        }
        /// <summary>
        /// 有排期开始时间段（结束）
        /// </summary>
        public string EndMemberCooperatedBeginTime
        {
            set { _endmembercooperatedbegintime = value; }
            get { return _endmembercooperatedbegintime; }
        }

        public string MemberSyncStatus
        {
            set { _membersyncstatus = value; }
            get { return _membersyncstatus; }
        }
    }
}
