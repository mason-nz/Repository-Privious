using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ProjectTask_DMSMember 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ProjectTask_DMSMember
	{
        public ProjectTask_DMSMember()
        {
            _memberid = Constant.INT_INVALID_VALUE;
            _ptid = Constant.STRING_INVALID_VALUE;
            _originaldmsmemberid = Constant.STRING_EMPTY_VALUE; //Constant.STRING_INVALID_VALUE;
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
            _longitude = Constant.STRING_INVALID_VALUE;
            _lantitude = Constant.STRING_INVALID_VALUE;
            _trafficinfo = Constant.STRING_INVALID_VALUE;
            _brandgroupid = Constant.INT_INVALID_VALUE;
            _enterprisebrief = Constant.STRING_INVALID_VALUE;
            _remarks = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _memberType = Constant.STRING_INVALID_VALUE;
            _membercode = Constant.STRING_INVALID_VALUE;
        }


        private int _memberid;
        private string _ptid;
        private string _originaldmsmemberid;
        private string _name;
        private string _abbr;
        private string _phone;
        private string _fax;
        private string _companywebsite;
        private string _email;
        private string _postcode;
        private string _provinceid;
        private string _cityid;
        private string _countyid;
        private string _contactaddress;
        private string _longitude;
        private string _lantitude;
        private string _trafficinfo;
        private int _brandgroupid;
        private string _enterprisebrief;
        private string _remarks;
        private int _status;
        private string _memberType;
        private int? _syncstatus;
        private string _membercode;
        /// <summary>
        /// 
        /// </summary>
        public int MemberID
        {
            set { _memberid = value; }
            get { return _memberid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PTID
        {
            set { _ptid = value; }
            get { return _ptid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OriginalDMSMemberID
        {
            set { _originaldmsmemberid = value; }
            get { return _originaldmsmemberid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Abbr
        {
            set { _abbr = value; }
            get { return _abbr; }
        }
        /// <summary>
        /// 会员类型
        /// </summary>
        public string MemberType
        {
            set { _memberType = value; }
            get { return _memberType; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
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
        public string CompanyWebSite
        {
            set { _companywebsite = value; }
            get { return _companywebsite; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Postcode
        {
            set { _postcode = value; }
            get { return _postcode; }
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
        /// <summary>
        /// 
        /// </summary>
        public string CountyID
        {
            set { _countyid = value; }
            get { return _countyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContactAddress
        {
            set { _contactaddress = value; }
            get { return _contactaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Longitude
        {
            set { _longitude = value; }
            get { return _longitude; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Lantitude
        {
            set { _lantitude = value; }
            get { return _lantitude; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TrafficInfo
        {
            set { _trafficinfo = value; }
            get { return _trafficinfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BrandGroupID
        {
            set { _brandgroupid = value; }
            get { return _brandgroupid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EnterpriseBrief
        {
            set { _enterprisebrief = value; }
            get { return _enterprisebrief; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }

        /// <summary>
        /// 会员账号状态（仅查询用，枚举）
        /// </summary>
        public int? SyncStatus
        {
            set { _syncstatus = value; }
            get { return _syncstatus; }
        }

        private string brandIDs;
        /// <summary>
        /// 主营品牌ID集合
        /// </summary>
        public string BrandIDs { get { return brandIDs; } set { brandIDs = value; } }

        private string serialIds;
        /// <summary>
        /// 附加子品牌ID集合
        /// </summary>
        public string SerialIds { get { return serialIds; } set { serialIds = value; } }


        private string brandNames;
        /// <summary>
        /// 主营品牌集合
        /// </summary>
        public string BrandNames { get { return brandNames; } set { brandNames = value; } }

        private string serialNames;
        /// <summary>
        /// 附加子品牌集合
        /// </summary>
        public string SerialNames { get { return serialNames; } set { serialNames = value; } }
        /// <summary>
        /// 会员ID（同步之前）
        /// </summary>
        public string MemberCode
        {
            set { _membercode = value; }
            get { return _membercode; }
        }

	}
}

