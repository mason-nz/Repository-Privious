using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryOtherTaskInfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-03-20 03:24:41 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryOtherTaskInfo
    {
        public QueryOtherTaskInfo()
        {
            _ptid = Constant.STRING_INVALID_VALUE;
            _projectid = Constant.INT_INVALID_VALUE;
            _relationtableid = Constant.STRING_INVALID_VALUE;
            _relationid = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _lastopttime = Constant.DATE_INVALID_VALUE;
            _lastoptuserid = Constant.INT_INVALID_VALUE;
            _taskstatus = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;

            _projectname = Constant.STRING_INVALID_VALUE;
            _statuss = Constant.STRING_INVALID_VALUE;
            _begintime = Constant.STRING_INVALID_VALUE;//创建时间的开始时间
            _endtime = Constant.STRING_INVALID_VALUE;//创建时间的结束时间
            _bgid = Constant.INT_INVALID_VALUE;
            _scid = Constant.INT_INVALID_VALUE;
            _oper = Constant.INT_INVALID_VALUE;
            _loginid = Constant.INT_INVALID_VALUE;//登陆人ID
            _owngroup = Constant.STRING_EMPTY_VALUE;//权限是本组的权限组；格式：个人用户组,业务组...
            _oneself = Constant.STRING_EMPTY_VALUE;//权限是本人的权限组；格式：个人用户组,业务组...
            _agent = Constant.INT_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _lastoptbegintime = Constant.STRING_INVALID_VALUE; 
            _lastoptendtime = Constant.STRING_INVALID_VALUE;
            _custname = Constant.STRING_INVALID_VALUE;

        }
        #region Model
        private string _ptid;
        private long _projectid;
        private string _relationtableid;
        private string _relationid;
        private DateTime? _createtime;
        private int? _createuserid;
        private DateTime? _lastopttime;
        private int? _lastoptuserid;
        private int? _taskstatus;
        private int? _status;
        private string _custid;
        private string _projectname;
        private string _statuss;
        private int _oper;
        private int _agent;

        private string _custname;
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
        public long ProjectID
        {
            set { _projectid = value; }
            get { return _projectid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RelationTableID
        {
            set { _relationtableid = value; }
            get { return _relationtableid; }
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
        public int? TaskStatus
        {
            set { _taskstatus = value; }
            get { return _taskstatus; }
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
        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }
        public string Statuss
        {
            set { _statuss = value; }
            get { return _statuss; }
        }
        #endregion Model

        private int? _bgid;
        private int? _scid;
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
        public int? SCID
        {
            set { _scid = value; }
            get { return _scid; }
        }
        private string _begintime;
        private string _endtime;
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
        private string _lastoptbegintime;
        private string _lastoptendtime;
        public string LastOptBeginTime
        {
            set { _lastoptbegintime = value; }
            get { return _lastoptbegintime; }
        }
        public string LastOptEndTime
        {
            set { _lastoptendtime = value; }
            get { return _lastoptendtime; }
        }
        public int Oper
        {
            set { _oper = value; }
            get { return _oper; }
        }
        private int _loginid;

        public int LoginID
        {
            get { return _loginid; }
            set { _loginid = value; }
        }

        private string _owngroup;
        private string _oneself;
        public string OwnGroup
        {
            set { _owngroup = value; }
            get { return _owngroup; }
        }
        public string OneSelf
        {
            set { _oneself = value; }
            get { return _oneself; }
        }
        public int Agent
        {
            get { return _agent; }
            set { _agent = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustID
        {
            set { _custid = value; }
            get { return _custid; }
        }

        public string CustName
        {
            set { _custname = value; }
            get { return _custname; }
        }
    }
}

