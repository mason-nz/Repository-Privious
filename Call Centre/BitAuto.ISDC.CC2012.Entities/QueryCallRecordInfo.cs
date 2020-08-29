using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryCallRecordInfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:07 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryCallRecordInfo
    {
        public QueryCallRecordInfo()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _sessionid = Constant.STRING_INVALID_VALUE;
            _extensionnum = Constant.STRING_INVALID_VALUE;
            _phonenum = Constant.STRING_INVALID_VALUE;
            _ani = Constant.STRING_INVALID_VALUE;
            _callstatus = Constant.INT_INVALID_VALUE;
            _begintime = Constant.DATE_INVALID_VALUE;
            _endtime = Constant.DATE_INVALID_VALUE;
            _talltime = Constant.INT_INVALID_VALUE;
            _audiourl = Constant.STRING_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;

            _begincreatetime = Constant.DATE_INVALID_VALUE;
            _endcreatetime = Constant.DATE_INVALID_VALUE;

            _custname = Constant.STRING_INVALID_VALUE;
            _contact = Constant.STRING_INVALID_VALUE;
            _tasktypeid = Constant.INT_INVALID_VALUE;
            _taskid = Constant.STRING_INVALID_VALUE;
            _agent = Constant.STRING_INVALID_VALUE;
            _agentnum = Constant.STRING_INVALID_VALUE;
            _spantime1 = Constant.INT_INVALID_VALUE;
            _spantime2 = Constant.INT_INVALID_VALUE;
            _agentgroup = Constant.INT_INVALID_VALUE;

            _loginid = Constant.INT_INVALID_VALUE;//登陆人ID
            _owngroup = Constant.STRING_EMPTY_VALUE;//权限是本组的权限组；格式：个人用户组,业务组...
            _oneself = Constant.STRING_EMPTY_VALUE;//权限是本人的权限组；格式：个人用户组,业务组...
            _category = Constant.INT_INVALID_VALUE;

            _bgid = Constant.INT_INVALID_VALUE;
            _scid = Constant.INT_INVALID_VALUE;

            _appealbegintime = Constant.STRING_INVALID_VALUE;
            _appealendtime = Constant.STRING_INVALID_VALUE;
            _scorebegintime = Constant.STRING_INVALID_VALUE;
            _scoreendtime = Constant.STRING_INVALID_VALUE;
            _callinsource = Constant.STRING_INVALID_VALUE;
            _calltype = Constant.STRING_INVALID_VALUE;

            _qsresultstatus = Constant.STRING_INVALID_VALUE;
            _scorecreater = Constant.INT_INVALID_VALUE;
            _qsstateresult = Constant.STRING_EMPTY_VALUE;//申诉结果
            _scoretable = Constant.INT_INVALID_VALUE;//评分表
            _callid = Constant.INT_INVALID_VALUE;
            _businessid = Constant.STRING_INVALID_VALUE;

            _ivrscore = Constant.INT_INVALID_VALUE;//IVR满意度
            _incomingsource = Constant.INT_INVALID_VALUE;//呼入来源
            _isfilternull = Constant.INT_INVALID_VALUE;// 是否过滤录音时长为空或录音为空的录音
            _QueryType = Constant.INT_INVALID_VALUE;
            _Qualified = Constant.STRING_INVALID_VALUE;
            _QSResultScore = Constant.STRING_INVALID_VALUE;
            _QSScoreCreaters = Constant.STRING_INVALID_VALUE;

            SelSolve = -1;

            _projectid = Constant.INT_INVALID_VALUE; ;
            _issuccess = Constant.INT_INVALID_VALUE; ;
            _failreason = Constant.INT_INVALID_VALUE; ;
        }

        #region Model
        private long _recid;
        private string _sessionid;
        private string _extensionnum;
        private string _phonenum;
        private string _ani;
        private int? _callstatus;
        private DateTime? _begintime;
        private DateTime? _endtime;
        private int? _talltime;
        private string _audiourl;
        private string _custid;
        private DateTime? _createtime;
        private int? _createuserid;

        private DateTime _begincreatetime;
        private DateTime _endcreatetime;
        private string _custname;
        private string _contact;
        private int? _tasktypeid;
        private string _taskid;
        private string _agent;
        private string _agentnum;
        private int _spantime1;
        private int _spantime2;
        private int _agentgroup;
        private int _category;

        private int _loginid;
        private string _owngroup;
        private string _oneself;
        private bool _relationidorcrmcustid = false;
        private int _newcustid;

        private int _bgid;
        private int _scid;

        private string _qsstateresult;
        private int _scoretable;

        private string _businessid;
        private int _isfilternull;

        private string _QSScoreCreaters;

        public int ScoreTable
        {
            set { _scoretable = value; }
            get { return _scoretable; }
        }
        public string QSStateResult
        {
            set { _qsstateresult = value; }
            get { return _qsstateresult; }
        }

        private string _qsresultstatus;
        private int _scorecreater;
        private int _QueryType;

        private int _projectid;
        private int _issuccess;
        private int _failreason;

        public int ProjectId
        {
            set { _projectid = value; }
            get { return _projectid; }
        }
        public int IsSuccess
        {
            set { _issuccess = value; }
            get { return _issuccess; }
        }
        public int FailReason
        {
            set { _failreason = value; }
            get { return _failreason; }
        }

        /// <summary>
        /// 查询类型。1月，2周，3天,4 小时
        /// </summary>
        public int QueryType
        {
            set { _QueryType = value; }
            get { return _QueryType; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long RecID
        {
            set { _recid = value; }
            get { return _recid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SessionID
        {
            set { _sessionid = value; }
            get { return _sessionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtensionNum
        {
            set { _extensionnum = value; }
            get { return _extensionnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PhoneNum
        {
            set { _phonenum = value; }
            get { return _phonenum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ANI
        {
            set { _ani = value; }
            get { return _ani; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CallStatus
        {
            set { _callstatus = value; }
            get { return _callstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? BeginTime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TallTime
        {
            set { _talltime = value; }
            get { return _talltime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AudioURL
        {
            set { _audiourl = value; }
            get { return _audiourl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustID
        {
            set { _custid = value; }
            get { return _custid; }
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
        public DateTime BeginCreateTime
        {
            set { _begincreatetime = value; }
            get { return _begincreatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndCreateTime
        {
            set { _endcreatetime = value; }
            get { return _endcreatetime; }
        }
        #endregion Model


        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustName
        {
            set { _custname = value; }
            get { return _custname; }
        }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact
        {
            set { _contact = value; }
            get { return _contact; }
        }

        /// <summary>
        /// 任务分类ID
        /// </summary>
        public int? TaskTypeID
        {
            set { _tasktypeid = value; }
            get { return _tasktypeid; }
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
        /// 坐席
        /// </summary>
        public string Agent
        {
            set { _agent = value; }
            get { return _agent; }
        }
        /// <summary>
        /// 工号
        /// </summary>
        public string AgentNum
        {
            set { _agentnum = value; }
            get { return _agentnum; }
        }
        /// <summary>
        /// 时长（小）
        /// </summary>
        public int SpanTime1
        {
            set { _spantime1 = value; }
            get { return _spantime1; }
        }
        /// <summary>
        /// 时长（大）
        /// </summary>
        public int SpanTime2
        {
            set { _spantime2 = value; }
            get { return _spantime2; }
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
        /// 是否为关联TID或者客户ID
        /// </summary>
        public bool IsRelationIDOrCRMCustID
        {
            set { _relationidorcrmcustid = value; }
            get { return _relationidorcrmcustid; }
        }
        public int NewCustID
        {
            get { return _newcustid; }
            set { _newcustid = value; }
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
        /// <summary>
        /// 申诉开始时间
        /// </summary>
        private string _appealbegintime;
        public string AppealBeginTime
        {
            set { _appealbegintime = value; }
            get { return _appealbegintime; }
        }
        /// <summary>
        /// 申诉结束时间
        /// </summary>
        private string _appealendtime;
        public string AppealEndTime
        {
            set { _appealendtime = value; }
            get { return _appealendtime; }
        }


        /// <summary>
        /// 评分开始时间
        /// </summary>
        private string _scorebegintime;
        public string ScoreBeginTime
        {
            set { _scorebegintime = value; }
            get { return _scorebegintime; }
        }
        /// <summary>
        /// 评分结束时间
        /// </summary>
        private string _scoreendtime;
        public string ScoreEndTime
        {
            set { _scoreendtime = value; }
            get { return _scoreendtime; }
        }
        /// <summary>
        /// 呼入来源串
        /// </summary>
        private string _callinsource;
        public string CallInSource
        {
            set { _callinsource = value; }
            get { return _callinsource; }
        }


        /// <summary>
        /// 录音类型
        /// </summary>
        private string _calltype;
        public string CallType
        {
            set { _calltype = value; }
            get { return _calltype; }
        }

        public string QSResultStatus
        {
            set { _qsresultstatus = value; }
            get { return _qsresultstatus; }
        }
        public int ScoreCreater
        {
            set { _scorecreater = value; }
            get { return _scorecreater; }
        }
        private Int64 _callid;
        public Int64 CallID
        {
            set
            {
                _callid = value;
            }
            get
            {
                return _callid;
            }
        }
        public string BusinessID
        {
            set { _businessid = value; }
            get { return _businessid; }
        }

        /// <summary>
        /// IVR满意度
        /// </summary>
        private int _ivrscore;
        public int IVRScore
        {
            set
            {
                _ivrscore = value;
            }
            get
            {
                return _ivrscore;
            }
        }

        /// <summary>
        /// 呼入来源
        /// </summary>
        private int _incomingsource;
        public int IncomingSource
        {
            set
            {
                _incomingsource = value;
            }
            get
            {
                return _incomingsource;
            }
        }

        /// <summary>
        /// 是否过滤录音为null或者时长为0的录音
        /// </summary>
        public int IsFilterNull
        {
            set
            {
                _isfilternull = value;
            }
            get
            {
                return _isfilternull;
            }
        }

        /// <summary>
        /// 合格型：1合格，其它值不合格
        /// </summary>
        private string _Qualified;
        public string Qualified
        {
            set
            {
                _Qualified = value;
            }
            get
            {
                return _Qualified;
            }
        }
        /// <summary>
        /// 满意度
        /// </summary>
        private string _QSResultScore;
        public string QSResultScore
        {
            set
            {
                _QSResultScore = value;
            }
            get
            {
                return _QSResultScore;
            }
        }
        public string QSScoreCreaters
        {
            get { return _QSScoreCreaters; }
            set { _QSScoreCreaters = value; }
        }

        /// <summary>
        /// 热线来源
        /// </summary>
        public string selBusinessType { get; set; }
        /// <summary>
        /// 问题解决
        /// </summary>
        public int SelSolve { get; set; }

        public string OutTypes { get; set; }

        /// <summary>
        /// 话务坐席所属分组
        /// </summary>
        public string CallAgentBGID { get; set; }

        //CRM的客户id
        public string CRMCustID { get; set; }
    }
}

