using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryStopCustApply 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-07-01 12:21:55 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryStopCustApply
    {
        public QueryStopCustApply()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _crmstopcustapplyid = Constant.INT_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _applyerid = Constant.INT_INVALID_VALUE;
            _areaname = Constant.STRING_INVALID_VALUE;
            _areaid = Constant.INT_INVALID_VALUE;
            _applytime = Constant.DATE_INVALID_VALUE;
            _audittime = Constant.DATE_INVALID_VALUE;
            _stoptime = Constant.DATE_INVALID_VALUE;
            _stopstatus = Constant.INT_INVALID_VALUE;
            _rejectreason = Constant.STRING_INVALID_VALUE;
            _auditopinion = Constant.STRING_INVALID_VALUE;
            _remark = Constant.STRING_INVALID_VALUE;
            _ApplyBeginTime = Constant.STRING_INVALID_VALUE;
            _ApplyEndTime = Constant.STRING_INVALID_VALUE;
            _SubmitBeginTime = Constant.STRING_INVALID_VALUE;
            _SubmitEndTime = Constant.STRING_INVALID_VALUE;
            _StopBeginTime = Constant.STRING_INVALID_VALUE;
            _StopEndTime = Constant.STRING_INVALID_VALUE;
            _CustName = Constant.STRING_INVALID_VALUE;
            _OperID = Constant.INT_INVALID_VALUE;
            _ApplerName = Constant.STRING_INVALID_VALUE;
            _loginid = Constant.INT_INVALID_VALUE;//登陆人ID
            _owngroup = Constant.STRING_EMPTY_VALUE;//权限是本组的权限组；格式：个人用户组,业务组...
            _oneself = Constant.STRING_EMPTY_VALUE;//权限是本人的权限组；格式：个人用户组,业务组...

            _DeleteMemberID = Constant.STRING_INVALID_VALUE;
            _Reason = Constant.STRING_INVALID_VALUE;
            _taskstatus = Constant.INT_INVALID_VALUE;
            _cartype = Constant.STRING_INVALID_VALUE;
            applytype = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private int _recid;
        private int? _crmstopcustapplyid;
        private string _custid;
        private int? _applyerid;
        private string _areaname;
        private int? _areaid;
        private DateTime? _applytime;
        private DateTime? _audittime;
        private DateTime? _stoptime;
        private int? _stopstatus;
        private string _rejectreason;
        private string _auditopinion;
        private string _remark;
        private string _ApplyBeginTime;
        private string _ApplyEndTime;
        private string _SubmitBeginTime;
        private string _SubmitEndTime;
        private string _StopBeginTime;
        private string _StopEndTime;
        private string _CustName;
        private int _OperID;
        private string _ApplerName;
        private string _Reason;
        private int _taskstatus;

        private string _DeleteMemberID;
        private string _cartype;
        private int applytype;

        public int ApplyType
        {
            set { applytype = value; }
            get { return applytype; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int RecID
        {
            set { _recid = value; }
            get { return _recid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CRMStopCustApplyID
        {
            set { _crmstopcustapplyid = value; }
            get { return _crmstopcustapplyid; }
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
        public int? ApplyerID
        {
            set { _applyerid = value; }
            get { return _applyerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AreaName
        {
            set { _areaname = value; }
            get { return _areaname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AreaID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ApplyTime
        {
            set { _applytime = value; }
            get { return _applytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AuditTime
        {
            set { _audittime = value; }
            get { return _audittime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StopTime
        {
            set { _stoptime = value; }
            get { return _stoptime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? StopStatus
        {
            set { _stopstatus = value; }
            get { return _stopstatus; }
        }
        public int TaskStatus
        {
            set { _taskstatus = value; }
            get { return _taskstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RejectReason
        {
            set { _rejectreason = value; }
            get { return _rejectreason; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AuditOpinion
        {
            set { _auditopinion = value; }
            get { return _auditopinion; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        public string ApplyBeginTime
        {
            set { _ApplyBeginTime = value; }
            get { return _ApplyBeginTime; }
        }
        public string ApplyEndTime
        {
            set { _ApplyEndTime = value; }
            get { return _ApplyEndTime; }
        }
        public string SubmitBeginTime
        {
            set { _SubmitBeginTime = value; }
            get { return _SubmitBeginTime; }
        }
        public string SubmitEndTime
        {
            set { _SubmitEndTime = value; }
            get { return _SubmitEndTime; }
        }
        public string StopBeginTime
        {
            set { _StopBeginTime = value; }
            get { return _StopBeginTime; }
        }
        public string StopEndTime
        {
            set { _StopEndTime = value; }
            get { return _StopEndTime; }
        }
        public string CustName
        {
            set { _CustName = value; }
            get { return _CustName; }
        }
        public int OperID
        {
            set { _OperID = value; }
            get { return _OperID; }
        }
        public string ApplerName
        {
            set { _ApplerName = value; }
            get { return _ApplerName; }
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
        public string DeleteMemberID
        {
            set { _DeleteMemberID = value; }
            get { return _DeleteMemberID; }
        }
        public string Reason
        {
            set { _Reason = value; }
            get { return _Reason; }
        }
        public string CarType
        {
            set { _cartype = value; }
            get { return _cartype; }
        }
        #endregion Model

    }
}

