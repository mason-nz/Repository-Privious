using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类GroupOrder 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-11-04 09:34:14 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class GroupOrder
    {
        public GroupOrder()
        {
            _taskid = Constant.INT_INVALID_VALUE;
            _orderid = Constant.INT_INVALID_VALUE;
            _ordercode = Constant.INT_INVALID_VALUE;
            _customername = Constant.STRING_INVALID_VALUE;
            _customertel = Constant.STRING_INVALID_VALUE;
            _usergender = Constant.INT_INVALID_VALUE;
            _provinceid = Constant.INT_INVALID_VALUE;
            _provincename = Constant.STRING_INVALID_VALUE;
            _cityid = Constant.INT_INVALID_VALUE;
            _cityname = Constant.STRING_INVALID_VALUE;
            _areaid = Constant.STRING_INVALID_VALUE;
            _ordercreatetime = Constant.DATE_INVALID_VALUE;
            _carmasterid = Constant.INT_INVALID_VALUE;
            _carmastername = Constant.STRING_INVALID_VALUE;
            _carserialid = Constant.INT_INVALID_VALUE;
            _carserialname = Constant.STRING_INVALID_VALUE;
            _carid = Constant.INT_INVALID_VALUE;
            _carname = Constant.STRING_INVALID_VALUE;
            _dealerid = Constant.INT_INVALID_VALUE;
            _dealername = Constant.STRING_INVALID_VALUE;
            _callrecord = Constant.STRING_INVALID_VALUE;
            _isreturnvisit = Constant.INT_INVALID_VALUE;
            _orderprice = Constant.DECIMAL_INVALID_VALUE;
            _failreasonid = Constant.INT_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _lastupdatetime = Constant.DATE_INVALID_VALUE;
            _lastupdateuserid = Constant.INT_INVALID_VALUE;
            _username = Constant.STRING_INVALID_VALUE;
            _failreason = Constant.STRING_INVALID_VALUE;
            _taskstatus = Constant.INT_INVALID_VALUE;
            _assignuserid = Constant.INT_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _wantcarserialid = Constant.INT_INVALID_VALUE;
            _wantcarmasterid = Constant.INT_INVALID_VALUE;
            _wantcarid = Constant.INT_INVALID_VALUE;
            _wantcarmastername = Constant.STRING_INVALID_VALUE;
            _wantcarserialname = Constant.STRING_INVALID_VALUE;
            _wantcarname = Constant.STRING_INVALID_VALUE;
            _planbuycartime = Constant.INT_INVALID_VALUE;
        }
        #region Model

        private int? _wantcarserialid;
        public int? WantCarSerialID
        {
            set { _wantcarserialid = value; }
            get { return _wantcarserialid; }
        }
        private int? _wantcarid;
        public int? WantCarID
        {
            set { _wantcarid = value; }
            get { return _wantcarid; }
        }

        private int? _wantcarmasterid;
        public int? WantCarMasterID
        {
            set { _wantcarmasterid = value; }
            get { return _wantcarmasterid; }
        }

        private string _wantcarmastername;
        public string WantCarMasterName
        {
            set { _wantcarmastername = value; }
            get { return _wantcarmastername; }

        }
        private string _wantcarserialname;
        public string WantCarSerialName
        {
            set { _wantcarserialname = value; }
            get { return _wantcarserialname; }
        }
        private string _wantcarname;
        public string WantCarName
        {
            set { _wantcarname = value; }
            get { return _wantcarname; }
        }
        private int? _planbuycartime;
        public int? PlanBuyCarTime
        {
            set { _planbuycartime = value; }
            get { return _planbuycartime; }
        }

        private long _taskid;
        private int? _orderid;
        private int? _ordercode;
        private string _customername;
        private string _customertel;
        private int? _usergender;
        private int? _provinceid;
        private string _provincename;
        private int? _cityid;
        private string _cityname;
        private string _areaid;
        private DateTime? _ordercreatetime;
        private int? _carmasterid;
        private string _carmastername;
        private int? _carserialid;
        private string _carserialname;
        private int? _carid;
        private string _carname;
        private int? _dealerid;
        private string _dealername;
        private string _callrecord;
        private int? _isreturnvisit;
        private decimal? _orderprice;
        private int? _failreasonid;
        private int? _createuserid;
        private DateTime? _createtime;
        private DateTime? _lastupdatetime;
        private int? _lastupdateuserid;
        private string _username;
        private string _failreason;
        private int? _taskstatus;
        private int? _assignuserid;
        private string _custid;
        /// <summary>
        /// 客户池客户ID
        /// </summary>
        public string CustID
        {
            set { _custid = value; }
            get { return _custid; }
        }

        /// <summary>
        /// 分配坐席
        /// </summary>
        public int? AssignUserID
        {
            set { _assignuserid = value; }
            get { return _assignuserid; }
        }
        /// <summary>
        /// 任务状态
        /// </summary>
        public int? TaskStatus
        {
            set { _taskstatus = value; }
            get { return _taskstatus; }
        }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailReason
        {
            set { _failreason = value; }
            get { return _failreason; }
        }
        /// <summary>
        /// 汽车通
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
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
        public int? OrderID
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderCode
        {
            set { _ordercode = value; }
            get { return _ordercode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerName
        {
            set { _customername = value; }
            get { return _customername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerTel
        {
            set { _customertel = value; }
            get { return _customertel; }
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
        public string ProvinceName
        {
            set { _provincename = value; }
            get { return _provincename; }
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
        public string CityName
        {
            set { _cityname = value; }
            get { return _cityname; }
        }
        /// <summary>
        /// 大区
        /// </summary>
        public string AreaID
        {
            set { _areaid = value; }
            get { return _areaid; }
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
        public int? CarMasterID
        {
            set { _carmasterid = value; }
            get { return _carmasterid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarMasterName
        {
            set { _carmastername = value; }
            get { return _carmastername; }
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
        public string CarSerialName
        {
            set { _carserialname = value; }
            get { return _carserialname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CarID
        {
            set { _carid = value; }
            get { return _carid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarName
        {
            set { _carname = value; }
            get { return _carname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DealerID
        {
            set { _dealerid = value; }
            get { return _dealerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DealerName
        {
            set { _dealername = value; }
            get { return _dealername; }
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
        public int? IsReturnVisit
        {
            set { _isreturnvisit = value; }
            get { return _isreturnvisit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? OrderPrice
        {
            set { _orderprice = value; }
            get { return _orderprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? FailReasonID
        {
            set { _failreasonid = value; }
            get { return _failreasonid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
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
        /// 
        /// </summary>
        public DateTime? LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LastUpdateUserID
        {
            set { _lastupdateuserid = value; }
            get { return _lastupdateuserid; }
        }
        #endregion Model

    }
}

