using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryOrderTask 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryOrderTask
    {
        public QueryOrderTask()
        {
            _taskid = Constant.INT_INVALID_VALUE;
            _source = Constant.INT_INVALID_VALUE;
            _taskstatus = Constant.INT_INVALID_VALUE;
            _relationid = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _assignuserid = Constant.INT_INVALID_VALUE;
            _assigntime = Constant.DATE_INVALID_VALUE;
            _username = Constant.STRING_INVALID_VALUE;
            //_isselectdmsmember没有默认值
            _status = Constant.INT_INVALID_VALUE;
            _submittime = Constant.DATE_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            //add by qizq 2012-9-24易湃订单id
            _yporderid = Constant.INT_INVALID_VALUE;
            _typestr = Constant.STRING_INVALID_VALUE;
            _statustr = Constant.STRING_INVALID_VALUE;
            _isselectdmsmemberstr = Constant.STRING_INVALID_VALUE;
            _createtimebegin = Constant.DATE_INVALID_VALUE;
            _createtimeend = Constant.DATE_INVALID_VALUE;
            _submittimebegin = Constant.DATE_INVALID_VALUE;
            _submittimeend = Constant.DATE_INVALID_VALUE;

            _nodealerreasonid = Constant.INT_INVALID_VALUE;
            _provinceid = Constant.INT_INVALID_VALUE;
            _cityid = Constant.INT_INVALID_VALUE;
            _tasktype = Constant.STRING_INVALID_VALUE;
        }
        #region Model
        private long _taskid;
        private int? _source;
        private int? _taskstatus;
        private long? _relationid;
        private int? _bgid;
        private int? _assignuserid;
        private DateTime? _assigntime;
        private string _username;
        private bool _isselectdmsmember;
        private int? _status;
        private DateTime? _submittime;
        private DateTime? _createtime;
        private int? _createuserid;
        //add by qizq 2012-9-24易湃订单id
        private int? _yporderid;
        private string _typestr;
        private string _statustr;
        private string _isselectdmsmemberstr;
        private DateTime? _createtimebegin;
        private DateTime? _createtimeend;
        private DateTime? _submittimebegin;
        private DateTime? _submittimeend;
        private int? _nodealerreasonid;
        private int? _provinceid;
        private int? _cityid;
        private string _tasktype;

        public string TaskType
        {
            get { return _tasktype; }
            set { _tasktype = value; }
        }

        public int? NoDealerReasonID
        {
            get { return _nodealerreasonid; }
            set { _nodealerreasonid = value; }
        }
        private string _area;


        public string Area
        {
            set { _area = value; }
            get { return _area; }
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
        public int? Source
        {
            set { _source = value; }
            get { return _source; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TaskStatus
        {
            set { _taskstatus = value; }
            get { return _taskstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long? RelationID
        {
            set { _relationid = value; }
            get { return _relationid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
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
        public DateTime? AssignTime
        {
            set { _assigntime = value; }
            get { return _assigntime; }
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
        public bool IsSelectDMSMember
        {
            set { _isselectdmsmember = value; }
            get { return _isselectdmsmember; }
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
        public DateTime? SubmitTime
        {
            set { _submittime = value; }
            get { return _submittime; }
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

        //add by qizq 2012-9-24易湃订单id
        public int? YpOrderID
        {
            set { _yporderid = value; }
            get { return _yporderid; }
        }
        public string TypeStr
        {
            set { _typestr = value; }
            get { return _typestr; }
        }
        public string StatuStr
        {
            set { _statustr = value; }
            get { return _statustr; }
        }
        public string IsSelectdMsmemberstr
        {
            set { _isselectdmsmemberstr = value; }
            get { return _isselectdmsmemberstr; }
        }
        public DateTime? CreateTimeBegin
        {
            set { _createtimebegin = value; }
            get { return _createtimebegin; }
        }
        public DateTime? CreateTimeEnd
        {
            set { _createtimeend = value; }
            get { return _createtimeend; }
        }
        public DateTime? SubmitTimeBegin
        {
            set { _submittimebegin = value; }
            get { return _submittimebegin; }
        }
        public DateTime? SubmitTimeEnd
        {
            set { _submittimeend = value; }
            get { return _submittimeend; }
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
        #endregion Model

    }
}

