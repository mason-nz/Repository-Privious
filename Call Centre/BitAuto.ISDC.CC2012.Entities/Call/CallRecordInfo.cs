using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类CallRecordInfo 。(属性说明自动提取数据库字段的描述信息)
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
    public class CallRecordInfo
    {
        public CallRecordInfo()
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

            _custname = Constant.STRING_INVALID_VALUE;
            _contact = Constant.STRING_INVALID_VALUE;
            _tasktypeid = Constant.INT_INVALID_VALUE;
            _taskid = Constant.STRING_INVALID_VALUE;

            _agentringtime = Constant.INT_INVALID_VALUE;
            _customringtime = Constant.INT_INVALID_VALUE;
            _afterworktime = Constant.INT_INVALID_VALUE;
            _afterworkbegintime = Constant.DATE_INVALID_VALUE;
            _rvid = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _scid = Constant.INT_INVALID_VALUE;
            _callid = Constant.INT_INVALID_VALUE;

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
        private string _custname;
        private string _contact;
        private int? _tasktypeid;
        private string _taskid;
        private string _skillgroup;
        private int _agentringtime;
        private int _customringtime;
        private int _afterworktime;
        private DateTime? _afterworkbegintime;
        private int _rvid;
        private int _bgid;
        private int _scid;

        public int RVID
        {
            set { _rvid = value; }
            get { return _rvid; }
        }
        /// <summary>
        /// cc客户id
        /// </summary>
        private string cccustid;
        public string CCCustID
        {
            set { cccustid = value; }
            get { return cccustid; }
        }

        /// <summary>
        /// 易派会员id
        /// </summary>
        private string dmsmemberid;
        public string DMSMemberID
        {
            set { dmsmemberid = value; }
            get { return dmsmemberid; }
        }

        /// <summary>
        /// cc易派会员id
        /// </summary>
        private string ccmemberid;
        public string CCMemberID
        {
            set { ccmemberid = value; }
            get { return ccmemberid; }
        }

        /// <summary>
        /// 车商通会员id
        /// </summary>
        private string cstmemberid;
        public string CSTMemberID
        {
            set { cstmemberid = value; }
            get { return cstmemberid; }
        }
        /// <summary>
        /// 新车商通会员id
        /// </summary>
        private string cccstmemberid;
        public string CC_CSTMemberID
        {
            set { cccstmemberid = value; }
            get { return cccstmemberid; }
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
        /// 技能组
        /// </summary>
        public string SkillGroup
        {
            set { _skillgroup = value; }
            get { return _skillgroup; }
        }
        /// <summary>
        /// 坐席振铃时间
        /// </summary>
        public int AgentRingTime
        {
            set { _agentringtime = value; }
            get { return _agentringtime; }
        }
        /// <summary>
        /// 客户振铃时间
        /// </summary>
        public int CustomRingTime
        {
            set { _customringtime = value; }
            get { return _customringtime; }
        }
        /// <summary>
        /// 事后处理状态时间
        /// </summary>
        public int AfterWorkTime
        {
            set { _afterworktime = value; }
            get { return _afterworktime; }
        }
        /// <summary>
        /// 事后处理状态开始时间
        /// </summary>
        public DateTime? AfterWorkBeginTime
        {
            set { _afterworkbegintime = value; }
            get { return _afterworkbegintime; }
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

        #endregion Model

    }
}

