using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类OrderTask 。(属性说明自动提取数据库字段的描述信息)
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
    public class OrderTask
    {
        public OrderTask()
        {
            _taskid = Constant.INT_INVALID_VALUE;
            _source = Constant.INT_INVALID_VALUE;
            _taskstatus = Constant.INT_INVALID_VALUE;
            _relationid = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _assignuserid = Constant.INT_INVALID_VALUE;
            _assigntime = Constant.DATE_INVALID_VALUE;
            _username = Constant.STRING_INVALID_VALUE;
            _isselectdmsmember = null;
            _status = Constant.INT_INVALID_VALUE;
            _submittime = Constant.DATE_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _nodealerreasonid = Constant.INT_INVALID_VALUE;
            _nodealerreason = Constant.STRING_INVALID_VALUE;
            _dealerid = Constant.INT_INVALID_VALUE;
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
        private bool? _isselectdmsmember;
        private int? _status;
        private DateTime? _submittime;
        private DateTime? _createtime;
        private int? _createuserid;
        private int? _nodealerreasonid;
        private string _nodealerreason;
        private int? _dealerid;
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
        public bool? IsSelectDMSMember
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
        public int? NoDealerReasonID
        {
            set { _nodealerreasonid = value; }
            get { return _nodealerreasonid; }
        }

        public string NoDealerReason
        {
            set { _nodealerreason = value; }
            get { return _nodealerreason; }
        }
        /// <summary>
        /// 订单中，经销商id(0为无主，非0为有主)
        /// </summary>
        public int? DealerID
        {
            set { _dealerid = value; }
            get { return _dealerid; }
        }
        #endregion Model

    }
}

