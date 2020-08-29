using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：CallRecordInfoInfo [话务管理]-[话务]-来电去电表 
    /// <summary>
    /// 实体类：CallRecordInfoInfo [话务管理]-[话务]-来电去电表 
    /// 自动生成（Copyright ?  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2016-07-30
    /// </summary>
    [DBTableAttribute("CallRecordInfo")]
    public class CallRecordInfoInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public CallRecordInfoInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public CallRecordInfoInfo(long _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public CallRecordInfoInfo(DataRow dr)
            : this()
        {
            AttributeHelper.SetValues(this, dr);
            SetModify(false);
        }

        #endregion

        #region 属性
        #region RecID [自增主键]
        /// <summary>
        /// 自增主键
        /// </summary>
        [DBField("RecID", SqlDbType.BigInt, 8, true, true)]
        public long? RecID { get { return _recid; } set { if (_recid != value) { _recid = value; IsModify_RecID = true; } } }
        /// <summary>
        /// 自增主键
        /// </summary>
        public long RecID_Value { get { return _recid.HasValue ? _recid.Value : 0; } }
        private long? _recid = null;

        [IsModify("RecID")]
        public bool IsModify_RecID { get; set; }
        #endregion

        #region SessionID [厂家ID]
        /// <summary>
        /// 厂家ID
        /// </summary>
        [DBField("SessionID", SqlDbType.VarChar, 50)]
        public string SessionID { get { return _sessionid; } set { if (_sessionid != value) { _sessionid = value; IsModify_SessionID = true; } } }
        /// <summary>
        /// 厂家ID
        /// </summary>
        public string SessionID_Value { get { return _sessionid != null ? _sessionid : ""; } }
        private string _sessionid = null;

        [IsModify("SessionID")]
        public bool IsModify_SessionID { get; set; }
        #endregion

        #region ExtensionNum [坐席分机号码]
        /// <summary>
        /// 坐席分机号码
        /// </summary>
        [DBField("ExtensionNum", SqlDbType.VarChar, 20)]
        public string ExtensionNum { get { return _extensionnum; } set { if (_extensionnum != value) { _extensionnum = value; IsModify_ExtensionNum = true; } } }
        /// <summary>
        /// 坐席分机号码
        /// </summary>
        public string ExtensionNum_Value { get { return _extensionnum != null ? _extensionnum : ""; } }
        private string _extensionnum = null;

        [IsModify("ExtensionNum")]
        public bool IsModify_ExtensionNum { get; set; }
        #endregion

        #region PhoneNum [被叫]
        /// <summary>
        /// 被叫
        /// </summary>
        [DBField("PhoneNum", SqlDbType.VarChar, 50)]
        public string PhoneNum { get { return _phonenum; } set { if (_phonenum != value) { _phonenum = value; IsModify_PhoneNum = true; } } }
        /// <summary>
        /// 被叫
        /// </summary>
        public string PhoneNum_Value { get { return _phonenum != null ? _phonenum : ""; } }
        private string _phonenum = null;

        [IsModify("PhoneNum")]
        public bool IsModify_PhoneNum { get; set; }
        #endregion

        #region ANI [主叫]
        /// <summary>
        /// 主叫
        /// </summary>
        [DBField("ANI", SqlDbType.VarChar, 50)]
        public string ANI { get { return _ani; } set { if (_ani != value) { _ani = value; IsModify_ANI = true; } } }
        /// <summary>
        /// 主叫
        /// </summary>
        public string ANI_Value { get { return _ani != null ? _ani : ""; } }
        private string _ani = null;

        [IsModify("ANI")]
        public bool IsModify_ANI { get; set; }
        #endregion

        #region CallStatus [电话状态（1-呼入，2-呼出）]
        /// <summary>
        /// 电话状态（1-呼入，2-呼出）
        /// </summary>
        [DBField("CallStatus", SqlDbType.Int, 4)]
        public int? CallStatus { get { return _callstatus; } set { if (_callstatus != value) { _callstatus = value; IsModify_CallStatus = true; } } }
        /// <summary>
        /// 电话状态（1-呼入，2-呼出）
        /// </summary>
        public int CallStatus_Value { get { return _callstatus.HasValue ? _callstatus.Value : 0; } }
        private int? _callstatus = null;

        [IsModify("CallStatus")]
        public bool IsModify_CallStatus { get; set; }
        #endregion

        #region BeginTime [录音开始时间]
        /// <summary>
        /// 录音开始时间
        /// </summary>
        [DBField("BeginTime", SqlDbType.DateTime, 8)]
        public DateTime? BeginTime { get { return _begintime; } set { if (_begintime != value) { _begintime = value; IsModify_BeginTime = true; } } }
        /// <summary>
        /// 录音开始时间
        /// </summary>
        public DateTime BeginTime_Value { get { return _begintime.HasValue ? _begintime.Value : new DateTime(); } }
        private DateTime? _begintime = null;

        [IsModify("BeginTime")]
        public bool IsModify_BeginTime { get; set; }
        #endregion

        #region EndTime [录音结束时间]
        /// <summary>
        /// 录音结束时间
        /// </summary>
        [DBField("EndTime", SqlDbType.DateTime, 8)]
        public DateTime? EndTime { get { return _endtime; } set { if (_endtime != value) { _endtime = value; IsModify_EndTime = true; } } }
        /// <summary>
        /// 录音结束时间
        /// </summary>
        public DateTime EndTime_Value { get { return _endtime.HasValue ? _endtime.Value : new DateTime(); } }
        private DateTime? _endtime = null;

        [IsModify("EndTime")]
        public bool IsModify_EndTime { get; set; }
        #endregion

        #region TallTime [录音总时长（单位：秒）]
        /// <summary>
        /// 录音总时长（单位：秒）
        /// </summary>
        [DBField("TallTime", SqlDbType.Int, 4)]
        public int? TallTime { get { return _talltime; } set { if (_talltime != value) { _talltime = value; IsModify_TallTime = true; } } }
        /// <summary>
        /// 录音总时长（单位：秒）
        /// </summary>
        public int TallTime_Value { get { return _talltime.HasValue ? _talltime.Value : 0; } }
        private int? _talltime = null;

        [IsModify("TallTime")]
        public bool IsModify_TallTime { get; set; }
        #endregion

        #region AudioURL [录音地址URL]
        /// <summary>
        /// 录音地址URL
        /// </summary>
        [DBField("AudioURL", SqlDbType.VarChar, 800)]
        public string AudioURL { get { return _audiourl; } set { if (_audiourl != value) { _audiourl = value; IsModify_AudioURL = true; } } }
        /// <summary>
        /// 录音地址URL
        /// </summary>
        public string AudioURL_Value { get { return _audiourl != null ? _audiourl : ""; } }
        private string _audiourl = null;

        [IsModify("AudioURL")]
        public bool IsModify_AudioURL { get; set; }
        #endregion

        #region CustID [客户ID]
        /// <summary>
        /// 客户ID
        /// </summary>
        [DBField("CustID", SqlDbType.VarChar, 20)]
        public string CustID { get { return _custid; } set { if (_custid != value) { _custid = value; IsModify_CustID = true; } } }
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustID_Value { get { return _custid != null ? _custid : ""; } }
        private string _custid = null;

        [IsModify("CustID")]
        public bool IsModify_CustID { get; set; }
        #endregion

        #region CustName [客户姓名]
        /// <summary>
        /// 客户姓名
        /// </summary>
        [DBField("CustName", SqlDbType.VarChar, 50)]
        public string CustName { get { return _custname; } set { if (_custname != value) { _custname = value; IsModify_CustName = true; } } }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustName_Value { get { return _custname != null ? _custname : ""; } }
        private string _custname = null;

        [IsModify("CustName")]
        public bool IsModify_CustName { get; set; }
        #endregion

        #region Contact [联系人]
        /// <summary>
        /// 联系人
        /// </summary>
        [DBField("Contact", SqlDbType.VarChar, 50)]
        public string Contact { get { return _contact; } set { if (_contact != value) { _contact = value; IsModify_Contact = true; } } }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact_Value { get { return _contact != null ? _contact : ""; } }
        private string _contact = null;

        [IsModify("Contact")]
        public bool IsModify_Contact { get; set; }
        #endregion

        #region TaskTypeID [任务分类id]
        /// <summary>
        /// 任务分类id
        /// </summary>
        [DBField("TaskTypeID", SqlDbType.Int, 4)]
        public int? TaskTypeID { get { return _tasktypeid; } set { if (_tasktypeid != value) { _tasktypeid = value; IsModify_TaskTypeID = true; } } }
        /// <summary>
        /// 任务分类id
        /// </summary>
        public int TaskTypeID_Value { get { return _tasktypeid.HasValue ? _tasktypeid.Value : 0; } }
        private int? _tasktypeid = null;

        [IsModify("TaskTypeID")]
        public bool IsModify_TaskTypeID { get; set; }
        #endregion

        #region TaskID [任务id]
        /// <summary>
        /// 任务id
        /// </summary>
        [DBField("TaskID", SqlDbType.VarChar, 50)]
        public string TaskID { get { return _taskid; } set { if (_taskid != value) { _taskid = value; IsModify_TaskID = true; } } }
        /// <summary>
        /// 任务id
        /// </summary>
        public string TaskID_Value { get { return _taskid != null ? _taskid : ""; } }
        private string _taskid = null;

        [IsModify("TaskID")]
        public bool IsModify_TaskID { get; set; }
        #endregion

        #region SkillGroup [所属技能组（仅呼入使用）]
        /// <summary>
        /// 所属技能组（仅呼入使用）
        /// </summary>
        [DBField("SkillGroup", SqlDbType.VarChar, 200)]
        public string SkillGroup { get { return _skillgroup; } set { if (_skillgroup != value) { _skillgroup = value; IsModify_SkillGroup = true; } } }
        /// <summary>
        /// 所属技能组（仅呼入使用）
        /// </summary>
        public string SkillGroup_Value { get { return _skillgroup != null ? _skillgroup : ""; } }
        private string _skillgroup = null;

        [IsModify("SkillGroup")]
        public bool IsModify_SkillGroup { get; set; }
        #endregion

        #region BGID [业务分组ID]
        /// <summary>
        /// 业务分组ID
        /// </summary>
        [DBField("BGID", SqlDbType.Int, 4)]
        public int? BGID { get { return _bgid; } set { if (_bgid != value) { _bgid = value; IsModify_BGID = true; } } }
        /// <summary>
        /// 业务分组ID
        /// </summary>
        public int BGID_Value { get { return _bgid.HasValue ? _bgid.Value : 0; } }
        private int? _bgid = null;

        [IsModify("BGID")]
        public bool IsModify_BGID { get; set; }
        #endregion

        #region SCID [项目分类ID]
        /// <summary>
        /// 项目分类ID
        /// </summary>
        [DBField("SCID", SqlDbType.Int, 4)]
        public int? SCID { get { return _scid; } set { if (_scid != value) { _scid = value; IsModify_SCID = true; } } }
        /// <summary>
        /// 项目分类ID
        /// </summary>
        public int SCID_Value { get { return _scid.HasValue ? _scid.Value : 0; } }
        private int? _scid = null;

        [IsModify("SCID")]
        public bool IsModify_SCID { get; set; }
        #endregion

        #region CallID [话务ID]
        /// <summary>
        /// 话务ID
        /// </summary>
        [DBField("CallID", SqlDbType.BigInt, 8)]
        public long? CallID { get { return _callid; } set { if (_callid != value) { _callid = value; IsModify_CallID = true; } } }
        /// <summary>
        /// 话务ID
        /// </summary>
        public long CallID_Value { get { return _callid.HasValue ? _callid.Value : 0; } }
        private long? _callid = null;

        [IsModify("CallID")]
        public bool IsModify_CallID { get; set; }
        #endregion

        #region CreateTime [创建时间]
        /// <summary>
        /// 创建时间
        /// </summary>
        [DBField("CreateTime", SqlDbType.DateTime, 8)]
        public DateTime? CreateTime { get { return _createtime; } set { if (_createtime != value) { _createtime = value; IsModify_CreateTime = true; } } }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime_Value { get { return _createtime.HasValue ? _createtime.Value : new DateTime(); } }
        private DateTime? _createtime = null;

        [IsModify("CreateTime")]
        public bool IsModify_CreateTime { get; set; }
        #endregion

        #region CreateUserID [创建人]
        /// <summary>
        /// 创建人
        /// </summary>
        [DBField("CreateUserID", SqlDbType.Int, 4)]
        public int? CreateUserID { get { return _createuserid; } set { if (_createuserid != value) { _createuserid = value; IsModify_CreateUserID = true; } } }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserID_Value { get { return _createuserid.HasValue ? _createuserid.Value : 0; } }
        private int? _createuserid = null;

        [IsModify("CreateUserID")]
        public bool IsModify_CreateUserID { get; set; }
        #endregion
        #endregion

        //#region 作废字段
        //#region AgentRingTime [坐席振铃时长（单位：秒）]
        ///// <summary>
        ///// 坐席振铃时长（单位：秒）
        ///// </summary>
        //[DBField("AgentRingTime", SqlDbType.Int, 4)]
        //public int? AgentRingTime { get { return _agentringtime; } set { if (_agentringtime != value) { _agentringtime = value; IsModify_AgentRingTime = true; } } }
        ///// <summary>
        ///// 坐席振铃时长（单位：秒）
        ///// </summary>
        //public int AgentRingTime_Value { get { return _agentringtime.HasValue ? _agentringtime.Value : 0; } }
        //private int? _agentringtime = null;

        //[IsModify("AgentRingTime")]
        //public bool IsModify_AgentRingTime { get; set; }
        //#endregion

        //#region CustomRingTime [客户振铃时长（单位：秒）]
        ///// <summary>
        ///// 客户振铃时长（单位：秒）
        ///// </summary>
        //[DBField("CustomRingTime", SqlDbType.Int, 4)]
        //public int? CustomRingTime { get { return _customringtime; } set { if (_customringtime != value) { _customringtime = value; IsModify_CustomRingTime = true; } } }
        ///// <summary>
        ///// 客户振铃时长（单位：秒）
        ///// </summary>
        //public int CustomRingTime_Value { get { return _customringtime.HasValue ? _customringtime.Value : 0; } }
        //private int? _customringtime = null;

        //[IsModify("CustomRingTime")]
        //public bool IsModify_CustomRingTime { get; set; }
        //#endregion

        //#region AfterWorkTime [事后处理时长（单位：秒）]
        ///// <summary>
        ///// 事后处理时长（单位：秒）
        ///// </summary>
        //[DBField("AfterWorkTime", SqlDbType.Int, 4)]
        //public int? AfterWorkTime { get { return _afterworktime; } set { if (_afterworktime != value) { _afterworktime = value; IsModify_AfterWorkTime = true; } } }
        ///// <summary>
        ///// 事后处理时长（单位：秒）
        ///// </summary>
        //public int AfterWorkTime_Value { get { return _afterworktime.HasValue ? _afterworktime.Value : 0; } }
        //private int? _afterworktime = null;

        //[IsModify("AfterWorkTime")]
        //public bool IsModify_AfterWorkTime { get; set; }
        //#endregion

        //#region AfterWorkBeginTime [事后处理开始时间]
        ///// <summary>
        ///// 事后处理开始时间
        ///// </summary>
        //[DBField("AfterWorkBeginTime", SqlDbType.DateTime, 8)]
        //public DateTime? AfterWorkBeginTime { get { return _afterworkbegintime; } set { if (_afterworkbegintime != value) { _afterworkbegintime = value; IsModify_AfterWorkBeginTime = true; } } }
        ///// <summary>
        ///// 事后处理开始时间
        ///// </summary>
        //public DateTime AfterWorkBeginTime_Value { get { return _afterworkbegintime.HasValue ? _afterworkbegintime.Value : new DateTime(); } }
        //private DateTime? _afterworkbegintime = null;

        //[IsModify("AfterWorkBeginTime")]
        //public bool IsModify_AfterWorkBeginTime { get; set; }
        //#endregion

        //#region NewCustID [新增客户ID]
        ///// <summary>
        ///// 新增客户ID
        ///// </summary>
        //[DBField("NewCustID", SqlDbType.Int, 4)]
        //public int? NewCustID { get { return _newcustid; } set { if (_newcustid != value) { _newcustid = value; IsModify_NewCustID = true; } } }
        ///// <summary>
        ///// 新增客户ID
        ///// </summary>
        //public int NewCustID_Value { get { return _newcustid.HasValue ? _newcustid.Value : 0; } }
        //private int? _newcustid = null;

        //[IsModify("NewCustID")]
        //public bool IsModify_NewCustID { get; set; }
        //#endregion

        //#region DMSMemberID [易湃会员ID]
        ///// <summary>
        ///// 易湃会员ID
        ///// </summary>
        //[DBField("DMSMemberID", SqlDbType.VarChar, 50)]
        //public string DMSMemberID { get { return _dmsmemberid; } set { if (_dmsmemberid != value) { _dmsmemberid = value; IsModify_DMSMemberID = true; } } }
        ///// <summary>
        ///// 易湃会员ID
        ///// </summary>
        //public string DMSMemberID_Value { get { return _dmsmemberid != null ? _dmsmemberid : ""; } }
        //private string _dmsmemberid = null;

        //[IsModify("DMSMemberID")]
        //public bool IsModify_DMSMemberID { get; set; }
        //#endregion

        //#region NewMemberID [新增易湃会员ID]
        ///// <summary>
        ///// 新增易湃会员ID
        ///// </summary>
        //[DBField("NewMemberID", SqlDbType.Int, 4)]
        //public int? NewMemberID { get { return _newmemberid; } set { if (_newmemberid != value) { _newmemberid = value; IsModify_NewMemberID = true; } } }
        ///// <summary>
        ///// 新增易湃会员ID
        ///// </summary>
        //public int NewMemberID_Value { get { return _newmemberid.HasValue ? _newmemberid.Value : 0; } }
        //private int? _newmemberid = null;

        //[IsModify("NewMemberID")]
        //public bool IsModify_NewMemberID { get; set; }
        //#endregion

        //#region CSTMemberID [车商通会员ID]
        ///// <summary>
        ///// 车商通会员ID
        ///// </summary>
        //[DBField("CSTMemberID", SqlDbType.VarChar, 10)]
        //public string CSTMemberID { get { return _cstmemberid; } set { if (_cstmemberid != value) { _cstmemberid = value; IsModify_CSTMemberID = true; } } }
        ///// <summary>
        ///// 车商通会员ID
        ///// </summary>
        //public string CSTMemberID_Value { get { return _cstmemberid != null ? _cstmemberid : ""; } }
        //private string _cstmemberid = null;

        //[IsModify("CSTMemberID")]
        //public bool IsModify_CSTMemberID { get; set; }
        //#endregion

        //#region NewCSTMemberID [新增车商通会员ID]
        ///// <summary>
        ///// 新增车商通会员ID
        ///// </summary>
        //[DBField("NewCSTMemberID", SqlDbType.Int, 4)]
        //public int? NewCSTMemberID { get { return _newcstmemberid; } set { if (_newcstmemberid != value) { _newcstmemberid = value; IsModify_NewCSTMemberID = true; } } }
        ///// <summary>
        ///// 新增车商通会员ID
        ///// </summary>
        //public int NewCSTMemberID_Value { get { return _newcstmemberid.HasValue ? _newcstmemberid.Value : 0; } }
        //private int? _newcstmemberid = null;

        //[IsModify("NewCSTMemberID")]
        //public bool IsModify_NewCSTMemberID { get; set; }
        //#endregion

        //#region RVID [客户回访表主键ID]
        ///// <summary>
        ///// 客户回访表主键ID
        ///// </summary>
        //[DBField("RVID", SqlDbType.Int, 4)]
        //public int? RVID { get { return _rvid; } set { if (_rvid != value) { _rvid = value; IsModify_RVID = true; } } }
        ///// <summary>
        ///// 客户回访表主键ID
        ///// </summary>
        //public int RVID_Value { get { return _rvid.HasValue ? _rvid.Value : 0; } }
        //private int? _rvid = null;

        //[IsModify("RVID")]
        //public bool IsModify_RVID { get; set; }
        //#endregion

        //#region BFTaskID [备份-任务ID]
        ///// <summary>
        ///// 备份-任务ID
        ///// </summary>
        //[DBField("BFTaskID", SqlDbType.VarChar, 20)]
        //public string BFTaskID { get { return _bftaskid; } set { if (_bftaskid != value) { _bftaskid = value; IsModify_BFTaskID = true; } } }
        ///// <summary>
        ///// 备份-任务ID
        ///// </summary>
        //public string BFTaskID_Value { get { return _bftaskid != null ? _bftaskid : ""; } }
        //private string _bftaskid = null;

        //[IsModify("BFTaskID")]
        //public bool IsModify_BFTaskID { get; set; }
        //#endregion
        //#endregion

        #region 方法
        /// 设置是否更新所有字段 (自动生成)
        /// <summary>
        /// 设置是否更新所有字段 (自动生成)
        /// </summary>
        /// <param name="b"></param>
        public void SetModify(bool b)
        {
            IsModify_RecID = b;
            IsModify_SessionID = b;
            IsModify_ExtensionNum = b;
            IsModify_PhoneNum = b;
            IsModify_ANI = b;
            IsModify_CallStatus = b;
            IsModify_BeginTime = b;
            IsModify_EndTime = b;
            IsModify_TallTime = b;
            IsModify_AudioURL = b;
            IsModify_CustID = b;
            IsModify_CreateTime = b;
            IsModify_CreateUserID = b;
            IsModify_CustName = b;
            IsModify_Contact = b;
            IsModify_TaskTypeID = b;
            IsModify_TaskID = b;
            IsModify_SkillGroup = b;            
            IsModify_BGID = b;
            IsModify_SCID = b;
            IsModify_CallID = b;

            //IsModify_AgentRingTime = b;
            //IsModify_CustomRingTime = b;
            //IsModify_AfterWorkTime = b;
            //IsModify_AfterWorkBeginTime = b;
            //IsModify_NewCustID = b;
            //IsModify_DMSMemberID = b;
            //IsModify_NewMemberID = b;
            //IsModify_CSTMemberID = b;
            //IsModify_NewCSTMemberID = b;
            //IsModify_RVID = b;
            //IsModify_BFTaskID = b;
        }

        #endregion

    }
}
