using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类CallRecord_ORIG 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-16 03:40:09 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class CallRecord_ORIG
    {
        public CallRecord_ORIG()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _sessionid = Constant.STRING_INVALID_VALUE;
            _callid = Constant.INT_INVALID_VALUE;
            _siemenscallid = Constant.INT_INVALID_VALUE;
            _genesyscallid = Constant.STRING_INVALID_VALUE;
            _extensionnum = Constant.STRING_INVALID_VALUE;
            _phonenum = Constant.STRING_INVALID_VALUE;
            _ani = Constant.STRING_INVALID_VALUE;
            _callstatus = Constant.INT_INVALID_VALUE;
            _switchinnum = Constant.STRING_INVALID_VALUE;
            _outboundtype = Constant.INT_INVALID_VALUE;
            _skillgroup = Constant.STRING_INVALID_VALUE;
            _initiatedtime = null;
            _ringingtime = null;
            _establishedtime = null;
            _customerreleasetime = null;
            _agentreleasetime = null;
            _afterworkbegintime = null;
            _afterworktime = Constant.INT_INVALID_VALUE;
            _consulttime = null;
            _reconnectcall = null;
            _talltime = Constant.INT_INVALID_VALUE;
            _audiourl = Constant.STRING_INVALID_VALUE;
            _createtime = null;
            _createuserid = Constant.INT_INVALID_VALUE;
            _content = Constant.STRING_INVALID_VALUE;
            _TransferInTime = null;
            _TransferOutTime = null;
        }
        #region Model
        private long _recid;
        private string _sessionid;
        private Int64? _callid;
        private string _genesyscallid;
        private int? _siemenscallid;
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
        private int? _createuserid;
        private string _content;
        private DateTime? _TransferInTime;
        private DateTime? _TransferOutTime;
        /// <summary>
        /// 转出时间
        /// </summary>
        public DateTime? TransferOutTime
        {
            set { _TransferOutTime = value; }
            get { return _TransferOutTime; }
        }
        /// <summary>
        /// 转入时间
        /// </summary>
        public DateTime? TransferInTime
        {
            set { _TransferInTime = value; }
            get { return _TransferInTime; }
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
        /// 客户端生成代替西门子CarolSDK中CallID
        /// </summary>
        public Int64? CallID
        {
            set { _callid = value; }
            get { return _callid; }
        }

        /// <summary>
        /// 西门子CarolSDK中CallID
        /// </summary>
        public int? SiemensCallID
        {
            set { _siemenscallid = value; }
            get { return _siemenscallid; }
        }

        /// <summary>
        /// GenesysCallID
        /// </summary>
        public string GenesysCallID
        {
            set { _genesyscallid = value; }
            get { return _genesyscallid; }
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
        /// 电话状态（2-外呼）
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

        /// <summary>
        /// 创建人
        /// </summary>
        public int? CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }

        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        #endregion Model


        #region 额外字段
        /// 任务ID
        /// <summary>
        /// 任务ID
        /// </summary>
        public string BusinessID { set; get; }
        /// 分组id
        /// <summary>
        /// 分组id
        /// </summary>
        public int BGID { set; get; }
        /// 分类id
        /// <summary>
        /// 分类id
        /// </summary>
        public int SCID { set; get; }

        /// 实际的手机号码
        /// <summary>
        /// 实际的手机号码
        /// </summary>
        public string Phone
        {
            get
            {
                return ExtensionNum == PhoneNum ? ANI : PhoneNum;
            }
        }
        #endregion
    }
}

