using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryCRMDMSMember
    {
        public QueryCRMDMSMember()
        {
            _id = Constant.STRING_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _CustName = Constant.STRING_INVALID_VALUE;
            _notThisMemberId = Constant.STRING_INVALID_VALUE;
            _membercode = Constant.STRING_INVALID_VALUE;
            _membertype = Constant.STRING_INVALID_VALUE;
            _name = Constant.STRING_INVALID_VALUE;
            _abbr = Constant.STRING_INVALID_VALUE;
            _phone = Constant.STRING_INVALID_VALUE;
            _fax = Constant.STRING_INVALID_VALUE;
            _companywebsite = Constant.STRING_INVALID_VALUE;
            _email = Constant.STRING_INVALID_VALUE;
            _postcode = Constant.STRING_INVALID_VALUE;
            _provinceid = Constant.STRING_INVALID_VALUE;
            _cityid = Constant.STRING_INVALID_VALUE;
            _countyid = Constant.STRING_INVALID_VALUE;
            _contactaddress = Constant.STRING_INVALID_VALUE;
            _trafficinfo = Constant.STRING_INVALID_VALUE;
            _brandgroupid = null;
            _enterprisebrief = Constant.STRING_INVALID_VALUE;
            _remarks = Constant.STRING_INVALID_VALUE;
            _syncstatus = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _modifyuserid = Constant.INT_INVALID_VALUE;
            _modifytime = Constant.DATE_INVALID_VALUE;

            _FuzzyCustName = Constant.STRING_INVALID_VALUE;
            _FuzzyMemberName = Constant.STRING_INVALID_VALUE;
            _FuzzyApplyDeptName = Constant.STRING_INVALID_VALUE;
            _FuzzyApplyUserName = Constant.STRING_INVALID_VALUE;
            _ApplyForBeginTime = Constant.DATE_INVALID_VALUE;
            _ApplyForEndTime = Constant.DATE_INVALID_VALUE;
            _SyncBeginTime = Constant.DATE_INVALID_VALUE;
            _SyncEndTime = Constant.DATE_INVALID_VALUE;
            _areatypewherestr = Constant.STRING_INVALID_VALUE;
            _membercooperatestatus = Constant.STRING_INVALID_VALUE;
            _membercreatetimestart = Constant.STRING_INVALID_VALUE;
            _membercreatetimeend = Constant.STRING_INVALID_VALUE;
            _returnvisittimestart = Constant.STRING_INVALID_VALUE;

            _confirmdatestart = Constant.STRING_INVALID_VALUE;
            _confirmdateend = Constant.STRING_INVALID_VALUE;

            _returnvisittimeend = Constant.STRING_INVALID_VALUE;
            _dmsmembercode = Constant.STRING_INVALID_VALUE;
            _dmsmembername = Constant.STRING_INVALID_VALUE;
            _areatypeids = Constant.STRING_INVALID_VALUE;
            _brandids = Constant.STRING_INVALID_VALUE;

            _beginmembercooperatedtime = Constant.STRING_INVALID_VALUE;
            _endmembercooperatedtime = Constant.STRING_INVALID_VALUE;
            _beginnomembercooperatedtime = Constant.STRING_INVALID_VALUE;
            _endnomembercooperatedtime = Constant.STRING_INVALID_VALUE;
            _cooperatedstatusids = Constant.STRING_INVALID_VALUE;
            _custcontactofficetypecode = Constant.INT_INVALID_VALUE;
            _ismagazinereturn = Constant.INT_INVALID_VALUE;
            _execcycle = Constant.STRING_EMPTY_VALUE;
            _startmembercooperatedbegintime = Constant.STRING_INVALID_VALUE;//有排期开始时间段（开始） add=masj date=2013-07-04
            _endmembercooperatedbegintime = Constant.STRING_INVALID_VALUE;//有排期开始时间段（结束） add=masj date=2013-07-04
            _membersyncstatus = Constant.STRING_INVALID_VALUE; //会员状态  add by yangyh date=2013-08-26
            _selectDeptID = Constant.STRING_INVALID_VALUE;
            _strDeptS = Constant.STRING_INVALID_VALUE;
        }

        private bool? _iscccreate = null;
        private bool? _isccusermapping = null;
        private bool? _isccreturnvisit = null;
        private string _areatypewherestr;
        private string _selectDeptID;
        private string _strDeptS;
        private string _id;
        private string _membercooperatestatus;
        private string _membercreatetimestart;
        private string _membercreatetimeend;
        private string _returnvisittimestart;
        private string _returnvisittimeend;

        private string _confirmdatestart;
        private string _confirmdateend;

        private string _dmsmembercode;
        private string _dmsmembername;
        private string _areatypeids;
        private string _brandids;

        private string _beginmembercooperatedtime;
        private string _endmembercooperatedtime;
        private string _beginnomembercooperatedtime;
        private string _endnomembercooperatedtime;
        private string _cooperatedstatusids;
        private int _custcontactofficetypecode;
        private int _ismagazinereturn;
        private string _execcycle;
        private string _startmembercooperatedbegintime;
        private string _endmembercooperatedbegintime;
        private string _membersyncstatus;


        public string SelectDeptID
        {
            get { return _selectDeptID; }
            set { _selectDeptID = value; }
        }

        public string StrDeptS
        {
            get { return _strDeptS; }
            set { _strDeptS = value; }
        }


        public string ID
        {
            set { _id = value; }
            get { return _id; }
        }

        private string _custid;
        public string CustID
        {
            set { _custid = value; }
            get { return _custid; }
        }
        private string _CustName;
        public string CustName
        {
            set { _CustName = value; }
            get { return _CustName; }
        }
        private string _notThisMemberId;
        public string NotThisMemberId
        {
            set { _notThisMemberId = value; }
            get { return _notThisMemberId; }
        }

        private string _membercode;
        public string MemberCode
        {
            set { _membercode = value; }
            get { return _membercode; }
        }

        private string _membertype;
        public string MemberType
        {
            set { _membertype = value; }
            get { return _membertype; }
        }
        private string _name;
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        private string _abbr;
        public string Abbr
        {
            set { _abbr = value; }
            get { return _abbr; }
        }

        private string _phone;
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }

        private string _fax;
        public string Fax
        {
            set { _fax = value; }
            get { return _fax; }
        }

        private string _companywebsite;
        public string CompanyWebSite
        {
            set { _companywebsite = value; }
            get { return _companywebsite; }
        }

        private string _email;
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }

        private string _postcode;
        public string Postcode
        {
            set { _postcode = value; }
            get { return _postcode; }
        }

        private string _provinceid;
        public string ProvinceID
        {
            set { _provinceid = value; }
            get { return _provinceid; }
        }

        private string _cityid;
        public string CityID
        {
            set { _cityid = value; }
            get { return _cityid; }
        }

        private string _countyid;
        public string CountyID
        {
            set { _countyid = value; }
            get { return _countyid; }
        }

        private string _contactaddress;
        public string ContactAddress
        {
            set { _contactaddress = value; }
            get { return _contactaddress; }
        }

        private string _trafficinfo;
        public string TrafficInfo
        {
            set { _trafficinfo = value; }
            get { return _trafficinfo; }
        }

        private int? _brandgroupid;
        public int? BrandGroupID
        {
            set { _brandgroupid = value; }
            get { return _brandgroupid; }
        }

        private string _enterprisebrief;
        public string EnterpriseBrief
        {
            set { _enterprisebrief = value; }
            get { return _enterprisebrief; }
        }

        private string _remarks;
        public string Remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
        }

        private int _syncstatus;
        public int SyncStatus
        {
            set { _syncstatus = value; }
            get { return _syncstatus; }
        }

        private int _status;
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }

        private int _createuserid;
        public int CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }

        private DateTime _createtime;
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        private int _modifyuserid;
        public int ModifyUserID
        {
            set { _modifyuserid = value; }
            get { return _modifyuserid; }
        }

        private DateTime _modifytime;
        public DateTime ModifyTime
        {
            set { _modifytime = value; }
            get { return _modifytime; }
        }

        /////////////////////////

        private string _FuzzyCustName;
        public string FuzzyCustName
        {
            set { _FuzzyCustName = value; }
            get { return _FuzzyCustName; }
        }

        private string _FuzzyMemberName;
        public string FuzzyMemberName
        {
            set { _FuzzyMemberName = value; }
            get { return _FuzzyMemberName; }
        }

        private string _FuzzyApplyDeptName;
        /// <summary>
        /// 申请部门
        /// </summary>
        public string FuzzyApplyDeptName
        {
            set { _FuzzyApplyDeptName = value; }
            get { return _FuzzyApplyDeptName; }
        }

        private string _FuzzyApplyUserName;
        /// <summary>
        /// 申请人
        /// </summary>
        public string FuzzyApplyUserName
        {
            set { _FuzzyApplyUserName = value; }
            get { return _FuzzyApplyUserName; }
        }

        private DateTime _ApplyForBeginTime;
        public DateTime ApplyForBeginTime
        {
            set { _ApplyForBeginTime = value; }
            get { return _ApplyForBeginTime; }
        }

        private DateTime _ApplyForEndTime;
        public DateTime ApplyForEndTime
        {
            set { _ApplyForEndTime = value; }
            get { return _ApplyForEndTime; }
        }

        private DateTime _SyncBeginTime;
        public DateTime SyncBeginTime
        {
            set { _SyncBeginTime = value; }
            get { return _SyncBeginTime; }
        }

        private DateTime _SyncEndTime;
        public DateTime SyncEndTime
        {
            set { _SyncEndTime = value; }
            get { return _SyncEndTime; }
        }

        private List<int> _SyncStatusList = new List<int>();
        public List<int> SyncStatusList
        {
            set { _SyncStatusList = value; }
            get { return _SyncStatusList; }
        }

        /// <summary>
        /// 是否有呼叫中心部门创建的会员
        /// </summary>
        public bool? IsCCCreate
        {
            set { _iscccreate = value; }
            get { return _iscccreate; }
        }

        /// <summary>
        /// 是否有呼叫中心部门的负责员工
        /// </summary>
        public bool? IsCCUserMapping
        {
            set { _isccusermapping = value; }
            get { return _isccusermapping; }
        }
        /// <summary>
        /// 是否有呼叫中心部门的回访记录
        /// </summary>
        public bool? IsCCReturnVisit
        {
            set { _isccreturnvisit = value; }
            get { return _isccreturnvisit; }
        }
        /// <summary>
        /// 区域类型筛选条件（SQL语句）
        /// </summary>
        public string AreaTypeWhereStr
        {
            set { _areatypewherestr = value; }
            get { return _areatypewherestr; }
        }
        ///// <summary>
        ///// 会员销售类型（0-无合作，1-销售，2-试用）
        ///// </summary>
        //public int MemberCooperateStatus
        //{
        //    set { _membercooperatestatus = value; }
        //    get { return _membercooperatestatus; }
        //}

        /// <summary>
        /// 会员创建开始时间（查询）
        /// </summary>
        public string MemberCreateTimeStart
        {
            set { _membercreatetimestart = value; }
            get { return _membercreatetimestart; }
        }
        /// <summary>
        /// 会员创建结束时间（查询）
        /// </summary>
        public string MemberCreateTimeEnd
        {
            set { _membercreatetimeend = value; }
            get { return _membercreatetimeend; }
        }
        /// <summary>
        /// 回访记录创建开始时间（查询）
        /// </summary>
        public string ReturnVisitTimeStart
        {
            set { _returnvisittimestart = value; }
            get { return _returnvisittimestart; }
        }
        /// <summary>
        /// 回访记录创建结束时间（查询）
        /// </summary>
        public string ReturnVisitTimeEnd
        {
            set { _returnvisittimeend = value; }
            get { return _returnvisittimeend; }
        }

        //add by qizq 2012-8-2 排期确认日期
        /// <summary>
        /// 排期确认日期开始时间（查询）
        /// </summary>
        public string ConfirmDateStart
        {
            set { _confirmdatestart = value; }
            get { return _confirmdatestart; }
        }
        /// <summary>
        /// 排期确认日期结束时间（查询）
        /// </summary>
        public string ConfirmDateEnd
        {
            set { _confirmdateend = value; }
            get { return _confirmdateend; }
        }

        /// <summary>
        /// 会员ID
        /// </summary>
        public string DMSMemberCode
        {
            set { _dmsmembercode = value; }
            get { return _dmsmembercode; }
        }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string DMSMemberName
        {
            set { _dmsmembername = value; }
            get { return _dmsmembername; }
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
        /// 会员主营品牌IDs
        /// </summary>
        public string BrandIDs
        {
            set { _brandids = value; }
            get { return _brandids; }
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
        /// 合作状态 1==========曾经合作过（不包括当天）
        ///          0==========曾经未合作过（不包括当天）
        /// </summary>
        public string CooperatedStatusIDs
        {
            get { return _cooperatedstatusids; }
            set { _cooperatedstatusids = value; }
        }

        /// <summary>
        /// 客户联系人职级（枚举）
        /// </summary>
        public int CustContactOfficeTypeCode
        {
            get { return _custcontactofficetypecode; }
            set { _custcontactofficetypecode = value; }
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

        /// <summary>
        /// 会员状态
        /// </summary>
        public string MemberSyncStatus
        {
            set { _membersyncstatus = value; }
            get { return _membersyncstatus; }
        }
    }
}
