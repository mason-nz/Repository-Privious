using System;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Collections.Generic;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ProjectTask_Cust 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:29 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ProjectTask_Cust
	{
        public ProjectTask_Cust()
        {
            _ptid = Constants.Constant.STRING_INVALID_VALUE;
            _originalcustid = Constants.Constant.STRING_INVALID_VALUE;
            _custname = Constants.Constant.STRING_INVALID_VALUE;
            _abbrname = Constants.Constant.STRING_INVALID_VALUE;
            _levelid = Constants.Constant.STRING_INVALID_VALUE;
            _industryid = Constants.Constant.STRING_INVALID_VALUE;
            _typeid = Constants.Constant.STRING_INVALID_VALUE;
            _custpid = Constants.Constant.STRING_INVALID_VALUE;
            _pid = Constants.Constant.STRING_INVALID_VALUE;
            _shoplevel = Constants.Constant.STRING_INVALID_VALUE;
            _provinceid = Constants.Constant.STRING_INVALID_VALUE;
            _cityid = Constants.Constant.STRING_INVALID_VALUE;
            _countyid = Constants.Constant.STRING_INVALID_VALUE;
            _address = Constants.Constant.STRING_INVALID_VALUE;
            _zipcode = Constants.Constant.STRING_INVALID_VALUE;
            _officetel = Constants.Constant.STRING_INVALID_VALUE;
            _fax = Constants.Constant.STRING_INVALID_VALUE;
            _notes = Constants.Constant.STRING_INVALID_VALUE;
            _contactname = Constants.Constant.STRING_INVALID_VALUE;
            _createUserID = Constants.Constant.INT_INVALID_VALUE;
            _lastUpdateUserID = Constants.Constant.INT_INVALID_VALUE;
            _createtime = Constants.Constant.DATE_INVALID_VALUE;
            _lastUpdateTime = Constants.Constant.DATE_INVALID_VALUE;

            _trademarketid = Constants.Constant.STRING_INVALID_VALUE;
            _cartype = Constants.Constant.INT_INVALID_VALUE;
            _cstmemberid = Constants.Constant.STRING_INVALID_VALUE;
            _usedcarbusinesstype = Constants.Constant.STRING_INVALID_VALUE;
        }
        #region Model
        private string _ptid;
        private string _originalcustid;
        private string _custname;
        private string _abbrname;
        private string _levelid;
        private string _industryid;
        private string _typeid;
        private string _custpid;
        private string _pid;
        private string _shoplevel;
        private string _provinceid;
        private string _cityid;
        private string _countyid;
        private string _address;
        private string _zipcode;
        private string _officetel;
        private string _fax;
        private string _notes;
        private string _contactname;
        private int _createUserID;
        private int _lastUpdateUserID;
        private DateTime _createtime;
        private DateTime _lastUpdateTime;
        private int? _lock = null;
        private int? _status = null;

        private string _trademarketid;
        private int _cartype;
        private string _cstmemberid;
        private string _usedcarbusinesstype;
        private string _fourspid;
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
        public string OriginalCustID
        {
            set { _originalcustid = value; }
            get { return _originalcustid; }
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
        public string AbbrName
        {
            set { _abbrname = value; }
            get { return _abbrname; }
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
        public string CustPid
        {
            set { _custpid = value; }
            get { return _custpid; }
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
        public string Address
        {
            set { _address = value; }
            get { return _address; }
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
        public string OfficeTel
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
        public string Notes
        {
            set { _notes = value; }
            get { return _notes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContactName
        {
            set { _contactname = value; }
            get { return _contactname; }
        }



        public int CreateUserID
        {
            set { _createUserID = value; }
            get { return _createUserID; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        public int LastUpdateUserID
        {
            set { _lastUpdateUserID = value; }
            get { return _lastUpdateUserID; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdateTime
        {
            set { _lastUpdateTime = value; }
            get { return _lastUpdateTime; }
        }
        /// <summary>
        /// CRM合同锁定状态（仅查询用），1-锁定，0-未锁定
        /// </summary>
        public int? Lock
        {
            set { _lock = value; }
            get
            {
                return _lock;
            }
        }
        /// <summary>
        /// CRM合同状态（仅查询用）,0-正常，1-禁用，-1-删除
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get
            {
                return _status;
            }
        }
        #endregion Model

        private List<int> brandIDs = new List<int>();
        /// <summary>
        /// 主营品牌ID集合
        /// </summary>
        public List<int> BrandIDs { get { return brandIDs; } set { brandIDs = value; } }

        private List<string> brandNames = new List<string>();
        /// <summary>
        /// 主营品牌名称集合
        /// </summary>
        public List<string> BrandNames { get { return brandNames; } set { brandNames = value; } }

        public string TradeMarketID
        {
            set
            {
                _trademarketid = value;
            }
            get
            {
                return _trademarketid;
            }
        }
        public int CarType
        {
            set
            {
                _cartype = value;
            }
            get
            {
                return _cartype;
            }
        }
        public string CstMemberID
        {
            set
            {
                _cstmemberid = value;
            }
            get
            {
                return _cstmemberid;
            }
        }
        public string UsedCarBusinessType
        {
            set
            {
                _usedcarbusinesstype = value;
            }
            get
            {
                return _usedcarbusinesstype;
            }
        }
        public string FoursPid
        {
            set
            {
                _fourspid = value;
            }
            get
            {
                return _fourspid;
            }
        }
	}
}

