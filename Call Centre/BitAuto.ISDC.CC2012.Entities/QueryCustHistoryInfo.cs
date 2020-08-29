using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryCustHistoryInfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:14 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryCustHistoryInfo
    {
        public QueryCustHistoryInfo()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _taskid = Constant.STRING_INVALID_VALUE;
            _callrecordid = Constant.INT_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _recordtype = Constant.INT_INVALID_VALUE;
            _consultid = Constant.INT_INVALID_VALUE;
            _consultdataid = Constant.INT_INVALID_VALUE;
            _questionquality = Constant.INT_INVALID_VALUE;
            _lasttreatmenttime = Constant.DATE_INVALID_VALUE;
            //_iscomplaint没有默认值
            _iscomplaintstr = Constant.STRING_INVALID_VALUE;
            _processstatus = Constant.INT_INVALID_VALUE;
            //_issendemail没有默认值
            _issendemailstr = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;

            _begintime = Constant.STRING_INVALID_VALUE;
            _endtime = Constant.STRING_INVALID_VALUE;

            _consultdcooptype = Constant.INT_INVALID_VALUE;//咨询类型-经销商合作表 类型
            _questiontype = Constant.STRING_INVALID_VALUE; //问题类别.
            _custname = Constant.STRING_INVALID_VALUE;  //客户姓名
            _status = Constant.STRING_INVALID_VALUE;   //处理状态

            _rightstr = Constant.STRING_INVALID_VALUE;    //任务列表权限
            _questionqualitystr = Constant.STRING_INVALID_VALUE;    //问题性质字符串 

            _consultidstr = Constant.STRING_INVALID_VALUE;

            _isforwardingstr = Constant.STRING_INVALID_VALUE;//是否转发
        }
        #region Model
        private long _recid;
        private string _taskid;
        private long _callrecordid;
        private string _custid;
        private int? _recordtype;
        private int? _consultid;
        private int? _consultdataid;
        private int? _questionquality;
        private DateTime? _lasttreatmenttime;
        private bool _iscomplaint;
        private int? _processstatus;
        private bool _issendemail;
        private DateTime? _createtime;
        private string _begintime;
        private string _endtime;
        private int? _createuserid;
        private int? _consultdcooptype;
        private string _questiontype;
        private string _custname;
        private string _status;
        private string _iscomplaintstr;
        private string _issendemailstr;
        private string _rightstr;
        private string _consultidstr;
        private string _questionqualitystr;
        private string _isforwardingstr;
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
        public string TaskID
        {
            set { _taskid = value; }
            get { return _taskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long CallRecordID
        {
            set { _callrecordid = value; }
            get { return _callrecordid; }
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
        public int? RecordType
        {
            set { _recordtype = value; }
            get { return _recordtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ConsultID
        {
            set { _consultid = value; }
            get { return _consultid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ConsultDataID
        {
            set { _consultdataid = value; }
            get { return _consultdataid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? QuestionQuality
        {
            set { _questionquality = value; }
            get { return _questionquality; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastTreatmentTime
        {
            set { _lasttreatmenttime = value; }
            get { return _lasttreatmenttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsComplaint
        {
            set { _iscomplaint = value; }
            get { return _iscomplaint; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProcessStatus
        {
            set { _processstatus = value; }
            get { return _processstatus; }
        }
        public string ProcessStatusStr
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSendEmail
        {
            set { _issendemail = value; }
            get { return _issendemail; }
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
        public string BeginTime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ConsultDCoopType
        {
            set { _consultdcooptype = value; }
            get { return _consultdcooptype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QuestionType
        {
            set { _questiontype = value; }
            get { return _questiontype; }
        }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustName
        {
            set { _custname = value; }
            get { return _custname; }
        }

        /// <summary>
        /// 处理状态
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }

        public string IsCompaintStr
        {
            set { _iscomplaintstr = value; }
            get { return _iscomplaintstr; }
        }

        public string IsIsSendEmailStr
        {
            set { _issendemailstr = value; }
            get { return _issendemailstr; }
        }

        public string RightStr
        {
            set { _rightstr = value; }
            get { return _rightstr; }
        }

        public string ConsultIDStr
        {
            set { _consultidstr = value; }
            get { return _consultidstr; }
        }
        
        public string QuestionQualityStr
        {
            set { _questionqualitystr = value; }
            get { return _questionqualitystr; }
        }

        public string IsForwardingStr
        {
            set { _isforwardingstr = value; }
            get { return _isforwardingstr; }
        }
        #endregion Model

    }
}

