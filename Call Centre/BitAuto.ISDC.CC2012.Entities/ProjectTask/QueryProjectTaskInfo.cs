using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryProjectTaskInfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:32 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryProjectTaskInfo
    {
        public QueryProjectTaskInfo()
        {
            _ptid = Constant.STRING_INVALID_VALUE;
            _pdsid = Constant.INT_INVALID_VALUE;
            _projectid = Constant.INT_INVALID_VALUE;
            _custname = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _type = Constant.INT_INVALID_VALUE;
            _relationid = Constant.STRING_INVALID_VALUE;
            _lastopttime = Constant.DATE_INVALID_VALUE;
            _lastoptuserid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _taskstatus = Constant.INT_INVALID_VALUE;

            _projectname = Constants.Constant.STRING_INVALID_VALUE;
            _bgid = Constants.Constant.STRING_INVALID_VALUE;
            _scid = Constants.Constant.STRING_INVALID_VALUE;
            _begintime = Constants.Constant.STRING_INVALID_VALUE;
            _endtime = Constants.Constant.STRING_INVALID_VALUE;
            _taskstatus_s = Constants.Constant.STRING_INVALID_VALUE;
            _operationstatus_s = Constant.STRING_INVALID_VALUE;
            _employeeid = Constant.INT_INVALID_VALUE;
            str_AdditionalStatus = Constant.STRING_INVALID_VALUE;
            _crmcustid = Constant.STRING_INVALID_VALUE;
            _crmbrandids = Constant.STRING_INVALID_VALUE;
            _nocrmbrand = Constant.STRING_INVALID_VALUE;
            _lastoperstarttime = Constant.STRING_INVALID_VALUE;
            _lastoperendtime = Constant.STRING_INVALID_VALUE;
            
        }
        #region Model
        private string _ptid;
        private long _pdsid;
        private long _projectid;
        private string _custname;
        private int? _status;
        private int? _type;
        private string _relationid;
        private DateTime? _lastopttime;
        private int? _lastoptuserid;
        private DateTime? _createtime;
        private int? _createuserid;
        private int _taskstatus;

        private string _taskstatus_s;
        private string _operationstatus_s;
        public string _projectname;
        public string _bgid;
        public string _scid;
        public string _begintime;
        public string _endtime;
        public string str_AdditionalStatus;

        public int? _employeeid;

        public string _crmcustid;

        public string _crmbrandids;
        public string _nocrmbrand;
        private string _custtype;
        private string _lastoperstarttime;

        private string _lastoperendtime;
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
        public long PDSID
        {
            set { _pdsid = value; }
            get { return _pdsid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long ProjectID
        {
            set { _projectid = value; }
            get { return _projectid; }
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
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RelationID
        {
            set { _relationid = value; }
            get { return _relationid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastOptTime
        {
            set { _lastopttime = value; }
            get { return _lastopttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LastOptUserID
        {
            set { _lastoptuserid = value; }
            get { return _lastoptuserid; }
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
        public int TaskStatus
        {
            set { _taskstatus = value; }
            get { return _taskstatus; }
        }

        public string TaskStatus_s
        {
            set { _taskstatus_s = value; }
            get { return _taskstatus_s; }
        }

        public string OperationStatus_s
        {
            set { _operationstatus_s = value; }
            get { return _operationstatus_s; }
        }

        /// <summary>
        /// 附加状态
        /// </summary>
        public string AdditionalStatus
        {
            get { return str_AdditionalStatus; }
            set { str_AdditionalStatus = value; }
        }

        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }

        public string BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
        }

        public string PCatageID
        {
            set { _scid = value; }
            get { return _scid; }
        }

        public string BeginTime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }

        public string EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }


        public int? EmployeeID
        {
            set { _employeeid = value; }
            get { return _employeeid; }
        }

        public string CRMCustID
        {
            set { _crmcustid = value; }
            get { return _crmcustid; }
        }

        public string CRMBrandIDs
        {
            set { _crmbrandids = value; }
            get { return _crmbrandids; }
        }

        public string NoCRMBrand
        {
            set { _nocrmbrand = value; }
            get { return _nocrmbrand; }
        }
        public string CustType
        {
            set { _custtype = value; }
            get { return _custtype; }
        }
        public string LastOperStartTime
        {
            set { _lastoperstarttime = value; }
            get { return _lastoperstarttime; }
        }
        public string LastOperEndTime
        {
            set { _lastoperendtime = value; }
            get { return _lastoperendtime; }
        }
        #endregion Model

    }
}

