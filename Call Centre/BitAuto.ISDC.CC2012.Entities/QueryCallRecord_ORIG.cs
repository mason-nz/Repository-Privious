using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryCallRecord_ORIG
    {
        public QueryCallRecord_ORIG()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _sessionid = Constant.STRING_INVALID_VALUE;
            _callid = Constant.INT_INVALID_VALUE;
            _extensionnum = Constant.STRING_INVALID_VALUE;
            _phonenum = Constant.STRING_INVALID_VALUE;
            _ani = Constant.STRING_INVALID_VALUE;
            _callstatus = Constant.INT_INVALID_VALUE;
            _switchinnum = Constant.STRING_INVALID_VALUE;
            _outboundtype = Constant.INT_INVALID_VALUE;
            _skillgroup = Constant.STRING_INVALID_VALUE;
            _initiatedtime = Constant.DATE_INVALID_VALUE;
            _ringingtime = Constant.DATE_INVALID_VALUE;
            _establishedtime = Constant.DATE_INVALID_VALUE;
            _customerreleasetime = Constant.DATE_INVALID_VALUE;
            _agentreleasetime = Constant.DATE_INVALID_VALUE;
            _afterworkbegintime = Constant.DATE_INVALID_VALUE;
            _afterworktime = Constant.INT_INVALID_VALUE;
            _consulttime = Constant.DATE_INVALID_VALUE;
            _reconnectcall = Constant.DATE_INVALID_VALUE;
            _talltime = Constant.INT_INVALID_VALUE;
            _audiourl = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;

            _begintime = Constant.STRING_INVALID_VALUE;
            _endtime = Constant.STRING_INVALID_VALUE;
            _spantime1 = Constant.INT_INVALID_VALUE;
            _spantime2 = Constant.INT_INVALID_VALUE;
            _talltime1 = Constant.INT_INVALID_VALUE;
            _talltime2 = Constant.INT_INVALID_VALUE;
            _callrelease = Constant.STRING_INVALID_VALUE;
            _calltypes = Constant.STRING_INVALID_VALUE;

            _loginid = Constant.INT_INVALID_VALUE;//登陆人ID
            _owngroup = Constant.STRING_EMPTY_VALUE;//权限是本组的权限组；格式：个人用户组,业务组...
            _oneself = Constant.STRING_EMPTY_VALUE;//权限是本人的权限组；格式：个人用户组,业务组...
            _taskid = Constant.STRING_INVALID_VALUE;
            _AgentNum = Constant.STRING_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _businessGroup = Constant.INT_INVALID_VALUE;

            _outtypes = null;
        }
        #region Model
        private long _recid;
        private string _sessionid;
        private Int64? _callid;
        private string _extensionnum;
        private string _phonenum;
        private string _ani;
        private int? _callstatus;
        private string _switchinnum;
        private int? _outboundtype;
        private string _skillgroup;
        private DateTime? _initiatedtime;
        private DateTime? _ringingtime;
        private DateTime? _establishedtime;
        private DateTime? _customerreleasetime;
        private DateTime? _agentreleasetime;
        private DateTime? _afterworkbegintime;
        private int? _afterworktime;
        private DateTime? _consulttime;
        private DateTime? _reconnectcall;
        private int? _talltime;
        private string _audiourl;
        private DateTime? _createtime;
        private string _begintime;
        private string _endtime;
        private int _spantime1;
        private int _spantime2;
        private int _talltime1;
        private int _talltime2;
        private string _callrelease;
        private string _calltypes;

        private int _loginid;
        private string _owngroup;
        private string _oneself;
        private string _taskid;
        private string _AgentNum;
        private int? _createuserid;

        private int _businessGroup;
        private string _outtypes;
        /// <summary>
        /// 录音创建人
        /// </summary>
        public int? CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 话务ID
        /// </summary>
        public long RecID
        {
            set { _recid = value; }
            get { return _recid; }
        }
        /// <summary>
        /// 宇高录音流水号
        /// </summary>
        public string SessionID
        {
            set { _sessionid = value; }
            get { return _sessionid; }
        }
        /// <summary>
        /// 西门子CarolSDK中CallID
        /// </summary>
        public Int64? CallID
        {
            set { _callid = value; }
            get { return _callid; }
        }
        /// <summary>
        /// 分机号码
        /// </summary>
        public string ExtensionNum
        {
            set { _extensionnum = value; }
            get { return _extensionnum; }
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNum
        {
            set { _phonenum = value; }
            get { return _phonenum; }
        }
        /// <summary>
        /// 来电号码
        /// </summary>
        public string ANI
        {
            set { _ani = value; }
            get { return _ani; }
        }
        /// <summary>
        /// 电话状态（1-呼入，2-胡扯）
        /// </summary>
        public int? CallStatus
        {
            set { _callstatus = value; }
            get { return _callstatus; }
        }
        /// <summary>
        /// 接入号码
        /// </summary>
        public string SwitchINNum
        {
            set { _switchinnum = value; }
            get { return _switchinnum; }
        }
        /// <summary>
        /// 呼出类型（1、页面呼出；2、客户端呼出）
        /// </summary>
        public int? OutBoundType
        {
            set { _outboundtype = value; }
            get { return _outboundtype; }
        }
        /// <summary>
        /// 所属技能组（仅呼入使用）
        /// </summary>
        public string SkillGroup
        {
            set { _skillgroup = value; }
            get { return _skillgroup; }
        }
        /// <summary>
        /// 初始化时间
        /// </summary>
        public DateTime? InitiatedTime
        {
            set { _initiatedtime = value; }
            get { return _initiatedtime; }
        }
        /// <summary>
        /// 振铃时间
        /// </summary>
        public DateTime? RingingTime
        {
            set { _ringingtime = value; }
            get { return _ringingtime; }
        }
        /// <summary>
        /// 接通时间
        /// </summary>
        public DateTime? EstablishedTime
        {
            set { _establishedtime = value; }
            get { return _establishedtime; }
        }
        /// <summary>
        /// 客户挂断时间
        /// </summary>
        public DateTime? CustomerReleaseTime
        {
            set { _customerreleasetime = value; }
            get { return _customerreleasetime; }
        }
        /// <summary>
        /// 坐席挂断时间
        /// </summary>
        public DateTime? AgentReleaseTime
        {
            set { _agentreleasetime = value; }
            get { return _agentreleasetime; }
        }
        /// <summary>
        /// 话后处理开始时间
        /// </summary>
        public DateTime? AfterWorkBeginTime
        {
            set { _afterworkbegintime = value; }
            get { return _afterworkbegintime; }
        }
        /// <summary>
        /// 话后处理时长
        /// </summary>
        public int? AfterWorkTime
        {
            set { _afterworktime = value; }
            get { return _afterworktime; }
        }
        /// <summary>
        /// 转接开始时间
        /// </summary>
        public DateTime? ConsultTime
        {
            set { _consulttime = value; }
            get { return _consulttime; }
        }
        /// <summary>
        /// 转接恢复时间
        /// </summary>
        public DateTime? ReconnectCall
        {
            set { _reconnectcall = value; }
            get { return _reconnectcall; }
        }
        /// <summary>
        /// 录音总时长（单位：秒）
        /// </summary>
        public int? TallTime
        {
            set { _talltime = value; }
            get { return _talltime; }
        }
        /// <summary>
        /// 录音地址URL
        /// </summary>
        public string AudioURL
        {
            set { _audiourl = value; }
            get { return _audiourl; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        #endregion Model

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
        public string CallRelease
        {
            set { _callrelease = value; }
            get { return _callrelease; }
        }
        public string CallTypes
        {
            set { _calltypes = value; }
            get { return _calltypes; }
        }
        public int SpanTime1
        {
            set { _spantime1 = value; }
            get { return _spantime1; }
        }
        public int SpanTime2
        {
            set { _spantime2 = value; }
            get { return _spantime2; }
        }
        public int TallTime1
        {
            set { _talltime1 = value; }
            get { return _talltime1; }
        }
        public int TallTime2
        {
            set { _talltime2 = value; }
            get { return _talltime2; }
        }


        public int LoginID
        {
            set { _loginid = value; }
            get { return _loginid; }
        }

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
        /// <summary>
        /// 对应任务ID
        /// </summary>
        public string TaskID
        {
            set { _taskid = value; }
            get { return _taskid; }
        }

        /// <summary>
        /// 工号
        /// </summary>
        public string AgentNum
        {
            set { _AgentNum = value; }
            get { return _AgentNum; }
        }

        public int BusinessGroup
        {
            set { _businessGroup = value; }
            get { return _businessGroup; }
        }

        public string OutTypes
        {
            set { _outtypes = value; }
            get { return _outtypes; }
        }
    }
}
