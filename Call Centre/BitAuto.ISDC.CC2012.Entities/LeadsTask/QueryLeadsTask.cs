using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryLeadsTask 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-05-19 11:30:49 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryLeadsTask
    {
        public QueryLeadsTask()
        {
            _taskid = Constant.STRING_INVALID_VALUE;
            _projectid = Constant.INT_INVALID_VALUE;
            _demandid = Constant.STRING_INVALID_VALUE;
            //_relationid没有默认值
            _username = Constant.STRING_INVALID_VALUE;
            _tel = Constant.STRING_INVALID_VALUE;
            _sex = Constant.INT_INVALID_VALUE;
            _provinceid = Constant.INT_INVALID_VALUE;
            _cityid = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _ordercarmasterid = Constant.INT_INVALID_VALUE;
            _ordercarmaster = Constant.STRING_INVALID_VALUE;
            _ordercarserialid = Constant.INT_INVALID_VALUE;
            _ordercarserial = Constant.STRING_INVALID_VALUE;
            _ordercarid = Constant.INT_INVALID_VALUE;
            _ordercar = Constant.STRING_INVALID_VALUE;
            _dealerid = Constant.STRING_INVALID_VALUE;
            _dealername = Constant.STRING_INVALID_VALUE;
            _ordercreatetime = Constant.DATE_INVALID_VALUE;
            _dcarmasterid = Constant.INT_INVALID_VALUE;
            _dcarmaster = Constant.STRING_INVALID_VALUE;
            _dcarserialid = Constant.INT_INVALID_VALUE;
            _dcarserial = Constant.STRING_INVALID_VALUE;
            _dcarid = Constant.INT_INVALID_VALUE;
            _dcarname = Constant.STRING_INVALID_VALUE;
            _issuccess = Constant.INT_INVALID_VALUE;
            _failreason = Constant.INT_INVALID_VALUE;
            _remark = Constant.STRING_INVALID_VALUE;
            _assignuserid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _lastupdatetime = Constant.DATE_INVALID_VALUE;
            _lastupdateuserid = Constant.INT_INVALID_VALUE;
            _lastdealtime = Constant.DATE_INVALID_VALUE;
            _projectname = Constant.STRING_INVALID_VALUE;

            _loginid = Constant.INT_INVALID_VALUE;//登陆人ID
            _begindealtime = Constant.STRING_INVALID_VALUE;
            _enddealtime = Constant.STRING_INVALID_VALUE;
            _taskcbegintime = Constant.STRING_INVALID_VALUE;
            _taskcendtime = Constant.STRING_INVALID_VALUE;
        }
        #region Model
        private string _taskid;
        private int _projectid;
        private string _demandid;
        private Guid _relationid;
        private string _username;
        private string _tel;
        private int? _sex;
        private int? _provinceid;
        private int? _cityid;
        private int? _status;
        private int? _ordercarmasterid;
        private string _ordercarmaster;
        private int? _ordercarserialid;
        private string _ordercarserial;
        private int? _ordercarid;
        private string _ordercar;
        private string _dealerid;
        private string _dealername;
        private DateTime? _ordercreatetime;
        private int? _dcarmasterid;
        private string _dcarmaster;
        private int? _dcarserialid;
        private string _dcarserial;
        private int? _dcarid;
        private string _dcarname;
        private int? _issuccess;
        private int? _failreason;
        private string _remark;
        private int? _assignuserid;
        private DateTime? _createtime;
        private int? _createuserid;
        private DateTime? _lastupdatetime;
        private int? _lastupdateuserid;
        private DateTime? _lastdealtime;
        private string _projectname;
        private string _begindealtime;
        private string _enddealtime;

        private string _taskcbegintime;
        private string _taskcendtime;
        /// <summary>
        /// 
        /// </summary>
        public string TaskID
        {
            set { _taskid = value; }
            get { return _taskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ProjectID
        {
            set { _projectid = value; }
            get { return _projectid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DemandID
        {
            set { _demandid = value; }
            get { return _demandid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid RelationID
        {
            set { _relationid = value; }
            get { return _relationid; }
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
        public string Tel
        {
            set { _tel = value; }
            get { return _tel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Sex
        {
            set { _sex = value; }
            get { return _sex; }
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
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderCarMasterID
        {
            set { _ordercarmasterid = value; }
            get { return _ordercarmasterid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderCarMaster
        {
            set { _ordercarmaster = value; }
            get { return _ordercarmaster; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderCarSerialID
        {
            set { _ordercarserialid = value; }
            get { return _ordercarserialid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderCarSerial
        {
            set { _ordercarserial = value; }
            get { return _ordercarserial; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderCarID
        {
            set { _ordercarid = value; }
            get { return _ordercarid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderCar
        {
            set { _ordercar = value; }
            get { return _ordercar; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DealerID
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
        public DateTime? OrderCreateTime
        {
            set { _ordercreatetime = value; }
            get { return _ordercreatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DCarMasterID
        {
            set { _dcarmasterid = value; }
            get { return _dcarmasterid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DCarMaster
        {
            set { _dcarmaster = value; }
            get { return _dcarmaster; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DCarSerialID
        {
            set { _dcarserialid = value; }
            get { return _dcarserialid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DCarSerial
        {
            set { _dcarserial = value; }
            get { return _dcarserial; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DCarID
        {
            set { _dcarid = value; }
            get { return _dcarid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DCarName
        {
            set { _dcarname = value; }
            get { return _dcarname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsSuccess
        {
            set { _issuccess = value; }
            get { return _issuccess; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? FailReason
        {
            set { _failreason = value; }
            get { return _failreason; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AssignUserID
        {
            set { _assignuserid = value; }
            get { return _assignuserid; }
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
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastDealTime
        {
            set { _lastdealtime = value; }
            get { return _lastdealtime; }
        }

        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }
        private int _loginid;

        public int LoginID
        {
            get { return _loginid; }
            set { _loginid = value; }
        }
        #endregion Model


        public string TaskCBeginTime
        {
            set { _taskcbegintime = value; }
            get { return _taskcbegintime; }
        }
        public string TaskCEndTime
        {
            set
            {
                _taskcendtime = value;
            }
            get
            {
                return _taskcendtime;
            }
        }

        public string BeginDealTime
        {
            set { _begindealtime = value; }
            get { return _begindealtime; }
        }

        public string EndDealTime
        {
            set { _enddealtime = value; }
            get { return _enddealtime; }
        }
    }
}

