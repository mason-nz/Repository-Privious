using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryOrderNewCar 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 05:58:24 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryOrderNewCar
    {
        public QueryOrderNewCar()
        {
            _taskid = Constant.INT_INVALID_VALUE;
            _yporderid = Constant.INT_INVALID_VALUE;
            _username = Constant.STRING_INVALID_VALUE;
            _userphone = Constant.STRING_INVALID_VALUE;
            _usermobile = Constant.STRING_INVALID_VALUE;
            _usermail = Constant.STRING_INVALID_VALUE;
            _usergender = Constant.INT_INVALID_VALUE;
            _provinceid = Constant.INT_INVALID_VALUE;
            _cityid = Constant.INT_INVALID_VALUE;
            _countyid = Constant.INT_INVALID_VALUE;
            _areaid = Constant.INT_INVALID_VALUE;
            _useraddress = Constant.STRING_INVALID_VALUE;
            _ordercreatetime = Constant.DATE_INVALID_VALUE;
            _orderremark = Constant.STRING_INVALID_VALUE;
            _carmasterid = Constant.INT_INVALID_VALUE;
            _carserialid = Constant.INT_INVALID_VALUE;
            _cartypeid = Constant.INT_INVALID_VALUE;
            _carcolor = Constant.STRING_INVALID_VALUE;
            _dmsmembercode = Constant.STRING_INVALID_VALUE;
            _dmsmembername = Constant.STRING_INVALID_VALUE;
            _callrecord = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _isupdate = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private long _taskid;
        private int? _yporderid;
        private string _username;
        private string _userphone;
        private string _usermobile;
        private string _usermail;
        private int? _usergender;
        private int? _provinceid;
        private int? _cityid;
        private int? _countyid;
        private int? _areaid;
        private string _useraddress;
        private DateTime? _ordercreatetime;
        private string _orderremark;
        private int? _carmasterid;
        private int? _carserialid;
        private int? _cartypeid;
        private string _carcolor;
        private string _dmsmembercode;
        private string _dmsmembername;
        private string _callrecord;
        private int? _status;
        private DateTime? _createtime;
        private int? _isupdate;

        /// <summary>
        /// 
        /// </summary>
        public long TaskID
        {
            set { _taskid = value; }
            get { return _taskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? YPOrderID
        {
            set { _yporderid = value; }
            get { return _yporderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserPhone
        {
            set { _userphone = value; }
            get { return _userphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserMobile
        {
            set { _usermobile = value; }
            get { return _usermobile; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserMail
        {
            set { _usermail = value; }
            get { return _usermail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? UserGender
        {
            set { _usergender = value; }
            get { return _usergender; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProvinceID
        {
            set { _provinceid = value; }
            get { return _provinceid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CityID
        {
            set { _cityid = value; }
            get { return _cityid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CountyID
        {
            set { _countyid = value; }
            get { return _countyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AreaID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserAddress
        {
            set { _useraddress = value; }
            get { return _useraddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? OrderCreateTime
        {
            set { _ordercreatetime = value; }
            get { return _ordercreatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderRemark
        {
            set { _orderremark = value; }
            get { return _orderremark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CarMasterID
        {
            set { _carmasterid = value; }
            get { return _carmasterid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CarSerialID
        {
            set { _carserialid = value; }
            get { return _carserialid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CarTypeID
        {
            set { _cartypeid = value; }
            get { return _cartypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarColor
        {
            set { _carcolor = value; }
            get { return _carcolor; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DMSMemberCode
        {
            set { _dmsmembercode = value; }
            get { return _dmsmembercode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DMSMemberName
        {
            set { _dmsmembername = value; }
            get { return _dmsmembername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CallRecord
        {
            set { _callrecord = value; }
            get { return _callrecord; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        /// <summary>
        /// 需要重新处理 调用接口没成功的记录(请赋值为-1)
        /// </summary>
        public int? IsUpdate
        {
            set { _isupdate = value; }
            get { return _isupdate; }
        }
        #endregion Model

    }
}

