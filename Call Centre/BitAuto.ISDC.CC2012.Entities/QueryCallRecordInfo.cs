using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ʵ����QueryCallRecordInfo ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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

            _loginid = Constant.INT_INVALID_VALUE;//��½��ID
            _owngroup = Constant.STRING_EMPTY_VALUE;//Ȩ���Ǳ����Ȩ���飻��ʽ�������û���,ҵ����...
            _oneself = Constant.STRING_EMPTY_VALUE;//Ȩ���Ǳ��˵�Ȩ���飻��ʽ�������û���,ҵ����...
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
            _qsstateresult = Constant.STRING_EMPTY_VALUE;//���߽��
            _scoretable = Constant.INT_INVALID_VALUE;//���ֱ�
            _callid = Constant.INT_INVALID_VALUE;
            _businessid = Constant.STRING_INVALID_VALUE;

            _ivrscore = Constant.INT_INVALID_VALUE;//IVR�����
            _incomingsource = Constant.INT_INVALID_VALUE;//������Դ
            _isfilternull = Constant.INT_INVALID_VALUE;// �Ƿ����¼��ʱ��Ϊ�ջ�¼��Ϊ�յ�¼��
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
        /// ��ѯ���͡�1�£�2�ܣ�3��,4 Сʱ
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
        /// �ͻ�����
        /// </summary>
        public string CustName
        {
            set { _custname = value; }
            get { return _custname; }
        }

        /// <summary>
        /// ��ϵ��
        /// </summary>
        public string Contact
        {
            set { _contact = value; }
            get { return _contact; }
        }

        /// <summary>
        /// �������ID
        /// </summary>
        public int? TaskTypeID
        {
            set { _tasktypeid = value; }
            get { return _tasktypeid; }
        }

        /// <summary>
        /// ��Ӧ����ID
        /// </summary>
        public string TaskID
        {
            set { _taskid = value; }
            get { return _taskid; }
        }
        /// <summary>
        /// ��ϯ
        /// </summary>
        public string Agent
        {
            set { _agent = value; }
            get { return _agent; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string AgentNum
        {
            set { _agentnum = value; }
            get { return _agentnum; }
        }
        /// <summary>
        /// ʱ����С��
        /// </summary>
        public int SpanTime1
        {
            set { _spantime1 = value; }
            get { return _spantime1; }
        }
        /// <summary>
        /// ʱ������
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
        /// �Ƿ�Ϊ����TID���߿ͻ�ID
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
        /// ���߿�ʼʱ��
        /// </summary>
        private string _appealbegintime;
        public string AppealBeginTime
        {
            set { _appealbegintime = value; }
            get { return _appealbegintime; }
        }
        /// <summary>
        /// ���߽���ʱ��
        /// </summary>
        private string _appealendtime;
        public string AppealEndTime
        {
            set { _appealendtime = value; }
            get { return _appealendtime; }
        }


        /// <summary>
        /// ���ֿ�ʼʱ��
        /// </summary>
        private string _scorebegintime;
        public string ScoreBeginTime
        {
            set { _scorebegintime = value; }
            get { return _scorebegintime; }
        }
        /// <summary>
        /// ���ֽ���ʱ��
        /// </summary>
        private string _scoreendtime;
        public string ScoreEndTime
        {
            set { _scoreendtime = value; }
            get { return _scoreendtime; }
        }
        /// <summary>
        /// ������Դ��
        /// </summary>
        private string _callinsource;
        public string CallInSource
        {
            set { _callinsource = value; }
            get { return _callinsource; }
        }


        /// <summary>
        /// ¼������
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
        /// IVR�����
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
        /// ������Դ
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
        /// �Ƿ����¼��Ϊnull����ʱ��Ϊ0��¼��
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
        /// �ϸ��ͣ�1�ϸ�����ֵ���ϸ�
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
        /// �����
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
        /// ������Դ
        /// </summary>
        public string selBusinessType { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public int SelSolve { get; set; }

        public string OutTypes { get; set; }

        /// <summary>
        /// ������ϯ��������
        /// </summary>
        public string CallAgentBGID { get; set; }

        //CRM�Ŀͻ�id
        public string CRMCustID { get; set; }
    }
}

