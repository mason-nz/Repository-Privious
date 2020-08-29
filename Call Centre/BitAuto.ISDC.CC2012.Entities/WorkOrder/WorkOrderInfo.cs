using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类WorkOrderInfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:20 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class WorkOrderInfo
    {
        public WorkOrderInfo()
        {
            _orderid = Constant.STRING_INVALID_VALUE;
            _categoryid = Constant.INT_INVALID_VALUE;
            _datasource = Constant.INT_INVALID_VALUE;
            _custname = Constant.STRING_INVALID_VALUE;
            _crmcustid = Constant.STRING_INVALID_VALUE;
            _provinceid = Constant.INT_INVALID_VALUE;
            _cityid = Constant.INT_INVALID_VALUE;
            _countyid = Constant.INT_INVALID_VALUE;
            _contact = Constant.STRING_INVALID_VALUE;
            _contacttel = Constant.STRING_INVALID_VALUE;
            _prioritylevel = Constant.INT_INVALID_VALUE;
            _lastprocessdate = Constant.STRING_INVALID_VALUE;
            //_iscomplainttype没有默认值
            _title = Constant.STRING_INVALID_VALUE;
            _workorderstatus = Constant.INT_INVALID_VALUE;
            _content = Constant.STRING_INVALID_VALUE;
            _receiverid = Constant.INT_INVALID_VALUE;
            _receivername = Constant.STRING_INVALID_VALUE;
            _receiverdepartname = Constant.STRING_INVALID_VALUE;
            //_issales没有默认值
            _attentioncarbrandid = Constant.INT_INVALID_VALUE;
            _attentioncarserialid = Constant.INT_INVALID_VALUE;
            _attentioncartypeid = Constant.INT_INVALID_VALUE;
            _attentioncarbrandname = Constant.STRING_INVALID_VALUE;
            _attentioncarserialname = Constant.STRING_INVALID_VALUE;
            _attentioncartypename = Constant.STRING_INVALID_VALUE;
            _selectdealerid = Constant.STRING_INVALID_VALUE;
            _selectdealername = Constant.STRING_INVALID_VALUE;
            _isreturnvisit = Constant.INT_INVALID_VALUE;
            _nominateactivity = Constant.STRING_INVALID_VALUE;
            _salecarbrandid = Constant.INT_INVALID_VALUE;
            _salecarserialid = Constant.INT_INVALID_VALUE;
            _salecartypeid = Constant.INT_INVALID_VALUE;
            _salecarbrandname = Constant.STRING_INVALID_VALUE;
            _salecarserialname = Constant.STRING_INVALID_VALUE;
            _salecartypename = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            //_isrevert没有默认值
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _modifytime = Constant.DATE_INVALID_VALUE;
            _modifyuserid = Constant.INT_INVALID_VALUE;

            _workcategory = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _scid = Constant.INT_INVALID_VALUE;
            _demandId = Constant.STRING_INVALID_VALUE;
            _callid = Constant.INT_INVALID_VALUE;
            _csid = Constant.INT_INVALID_VALUE;
            _lyid = Constant.INT_INVALID_VALUE;
            _cartype = Constant.INT_INVALID_VALUE;
            _systype = Constant.STRING_INVALID_VALUE;
        }
        #region Model
        private string _orderid;
        private int _categoryid;
        private int? _datasource;
        private string _custname;
        private string _crmcustid;
        private int? _provinceid;
        private int? _cityid;
        private int? _countyid;
        private string _contact;
        private string _contacttel;
        private int? _prioritylevel;
        private string _lastprocessdate;
        private bool _iscomplainttype;
        private string _title;
        private int? _workorderstatus;
        private string _content;
        private int? _receiverid;
        private string _receivername;
        private string _receiverdepartname;
        private bool _issales;
        private int? _attentioncarbrandid;
        private int? _attentioncarserialid;
        private int? _attentioncartypeid;
        private string _attentioncarbrandname;
        private string _attentioncarserialname;
        private string _attentioncartypename;
        private string _selectdealerid;
        private string _selectdealername;
        private int? _isreturnvisit;
        private string _nominateactivity;
        private int? _salecarbrandid;
        private int? _salecarserialid;
        private int? _salecartypeid;
        private string _salecarbrandname;
        private string _salecarserialname;
        private string _salecartypename;
        private int? _status;
        private bool _isrevert;
        private DateTime? _createtime;
        private int? _createuserid;
        private DateTime? _modifytime;
        private int? _modifyuserid;
        private int? _workcategory;
        private int _bgid;
        private int _scid;
        private string _demandId;
        private long? _callid;
        /// <summary>
        /// IM类型,isIM:经销商，isIM2:个人
        /// </summary>
        private string _systype;
        public string SYSType
        {
            set { _systype = value; }
            get { return _systype; }
        }
        /// <summary>
        /// IM车车类型,1:新车，2:二手车
        /// </summary>
        private int _cartype;
        public int CarType
        {
            set { _cartype = value; }
            get { return _cartype; }
        }
        /// <summary>
        /// IM留言表ID
        /// </summary>
        private int _lyid;
        public int LYID
        {
            set { _lyid = value; }
            get { return _lyid; }
        }
        /// <summary>
        /// IM会话ID
        /// </summary>
        private int _csid;
        public int CSID
        {
            set { _csid = value; }
            get { return _csid; }
        }
        //add by qizq 2013-11-6
        private Guid _guid;
        public Guid GUID
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderID
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CategoryID
        {
            set { _categoryid = value; }
            get { return _categoryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DataSource
        {
            set { _datasource = value; }
            get { return _datasource; }
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
        public string CRMCustID
        {
            set { _crmcustid = value; }
            get { return _crmcustid; }
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
        public string Contact
        {
            set { _contact = value; }
            get { return _contact; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContactTel
        {
            set { _contacttel = value; }
            get { return _contacttel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PriorityLevel
        {
            set { _prioritylevel = value; }
            get { return _prioritylevel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LastProcessDate
        {
            set { _lastprocessdate = value; }
            get { return _lastprocessdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsComplaintType
        {
            set { _iscomplainttype = value; }
            get { return _iscomplainttype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? WorkOrderStatus
        {
            set { _workorderstatus = value; }
            get { return _workorderstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ReceiverID
        {
            set { _receiverid = value; }
            get { return _receiverid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverName
        {
            set { _receivername = value; }
            get { return _receivername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverDepartName
        {
            set { _receiverdepartname = value; }
            get { return _receiverdepartname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSales
        {
            set { _issales = value; }
            get { return _issales; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AttentionCarBrandID
        {
            set { _attentioncarbrandid = value; }
            get { return _attentioncarbrandid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AttentionCarSerialID
        {
            set { _attentioncarserialid = value; }
            get { return _attentioncarserialid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AttentionCarTypeID
        {
            set { _attentioncartypeid = value; }
            get { return _attentioncartypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AttentionCarBrandName
        {
            set { _attentioncarbrandname = value; }
            get { return _attentioncarbrandname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AttentionCarSerialName
        {
            set { _attentioncarserialname = value; }
            get { return _attentioncarserialname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AttentionCarTypeName
        {
            set { _attentioncartypename = value; }
            get { return _attentioncartypename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SelectDealerID
        {
            set { _selectdealerid = value; }
            get { return _selectdealerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SelectDealerName
        {
            set { _selectdealername = value; }
            get { return _selectdealername; }
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
        public string NominateActivity
        {
            set { _nominateactivity = value; }
            get { return _nominateactivity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SaleCarBrandID
        {
            set { _salecarbrandid = value; }
            get { return _salecarbrandid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SaleCarSerialID
        {
            set { _salecarserialid = value; }
            get { return _salecarserialid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SaleCarTypeID
        {
            set { _salecartypeid = value; }
            get { return _salecartypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SaleCarBrandName
        {
            set { _salecarbrandname = value; }
            get { return _salecarbrandname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SaleCarSerialName
        {
            set { _salecarserialname = value; }
            get { return _salecarserialname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SaleCarTypeName
        {
            set { _salecartypename = value; }
            get { return _salecartypename; }
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
        public bool IsRevert
        {
            set { _isrevert = value; }
            get { return _isrevert; }
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
        public int? CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyTime
        {
            set { _modifytime = value; }
            get { return _modifytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ModifyUserID
        {
            set { _modifyuserid = value; }
            get { return _modifyuserid; }
        }
        public int? WorkCategory
        {
            set { _workcategory = value; }
            get { return _workcategory; }
        }

        public int BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
        }

        public int SCID
        {
            set { _scid = value; }
            get { return _scid; }
        }

        public string DemandID
        {
            get { return _demandId; }
            set { _demandId = value; }
        }

        public long? CallID
        {
            get { return _callid; }
            set { _callid = value; }
        }
        #endregion Model

    }
}

