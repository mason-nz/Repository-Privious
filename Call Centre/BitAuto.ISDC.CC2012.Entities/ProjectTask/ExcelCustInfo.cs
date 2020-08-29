using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ExcelCustInfo 。(属性说明自动提取数据库字段的描述信息)
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
	public class ExcelCustInfo
	{
        public ExcelCustInfo()
        {
            _id = Constant.INT_INVALID_VALUE;
            _custname = Constant.STRING_INVALID_VALUE;
            _typename = Constant.STRING_INVALID_VALUE;
            _provincename = Constant.STRING_INVALID_VALUE;
            _cityname = Constant.STRING_INVALID_VALUE;
            _countyname = Constant.STRING_INVALID_VALUE;
            _fax = Constant.STRING_INVALID_VALUE;
            _zipcode = Constant.STRING_INVALID_VALUE;
            _brandname = Constant.STRING_INVALID_VALUE;
            _officetel = Constant.STRING_INVALID_VALUE;
            _address = Constant.STRING_INVALID_VALUE;

            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _cartype = Constant.INT_INVALID_VALUE;
            _trademarketid = Constant.INT_INVALID_VALUE;
            _monthstock = Constant.INT_INVALID_VALUE;
            _monthsales = Constant.INT_INVALID_VALUE;
            _monthtrade = Constant.INT_INVALID_VALUE;
            _contactname = Constant.STRING_INVALID_VALUE;
        }
        #region Model
        private int _id;
        private string _custname;
        private string _typename;
        private string _provincename;
        private string _cityname;
        private string _countyname;
        private string _fax;
        private string _zipcode;
        private string _brandname;
        private string _officetel;
        private string _address;

        private DateTime _createtime;
        private int _createuserid;
        private int _cartype;
        private int _trademarketid;
        private int _monthstock;
        private int _monthsales;
        private int _monthtrade;
        private string _contactname;
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
        public string Address
        {
            set { _address = value; }
            get { return _address; }
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
        public string Zipcode
        {
            set { _zipcode = value; }
            get { return _zipcode; }
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
        public string OfficeTel
        {
            set { _officetel = value; }
            get { return _officetel; }
        }

        public DateTime Createtime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        public int CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        public int CarType
        {
            set { _cartype = value; }
            get { return _cartype; }
        }
        public int TradeMarketID
        {
            set { _trademarketid = value; }
            get { return _trademarketid; }
        }
        public int MonthStock
        {
            set { _monthstock = value; }
            get { return _monthstock; }
        }
        public int MonthSales
        {
            set { _monthsales = value; }
            get { return _monthsales; }
        }
        public int MonthTrade
        {
            set { _monthtrade = value; }
            get { return _monthtrade; }
        }
        public string ContactName
        {
            set { _contactname = value; }
            get { return _contactname; }
        }
        #endregion Model

	}
}

