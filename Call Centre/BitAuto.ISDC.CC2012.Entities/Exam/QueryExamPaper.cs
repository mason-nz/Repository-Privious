using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryExamPaper 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-03 02:04:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryExamPaper
    {
        public QueryExamPaper()
        {
            _epid = Constant.INT_INVALID_VALUE;
            _name = Constant.STRING_INVALID_VALUE;
            _ecid = Constant.INT_INVALID_VALUE;
            _ecidStr = Constant.STRING_INVALID_VALUE;
            _examdesc = Constant.STRING_INVALID_VALUE;
            _totalscore = Constant.INT_INVALID_VALUE;
            _status = Constant.STRING_INVALID_VALUE;
            _createBegintime = Constant.DATE_INVALID_VALUE;
            _createEndtime = Constant.DATE_INVALID_VALUE;
            _creaetuserid = Constant.STRING_INVALID_VALUE;
            _lastmodifytime = Constant.DATE_INVALID_VALUE;
            _lastmodifyuserid = Constant.INT_INVALID_VALUE;

            _examcategory = Constant.STRING_INVALID_VALUE; //分类串
            _exampersonid = Constant.INT_INVALID_VALUE; //  考试人员ID
            _testoverendtime = Constant.STRING_INVALID_VALUE;   //  考试已过时间
            _notestendtime = Constant.STRING_INVALID_VALUE;   //  考试未考时间

            _bgid = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private long _epid;
        private string _name;
        private int _ecid;
        private string _ecidStr;
        private string _examdesc;
        private int _totalscore;
        private string _status;
        private DateTime? _createBegintime;
        private DateTime? _createEndtime;
        private string _creaetuserid;
        private DateTime? _lastmodifytime;
        private int? _lastmodifyuserid;
        private string _examcategory;
        private int _exampersonid;
        private string _testoverendtime;
        private string _notestendtime;

        private int? _bgid;
        /// <summary>
        /// 
        /// </summary>
        public long EPID
        {
            set { _epid = value; }
            get { return _epid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ECID
        {
            set { _ecid = value; }
            get { return _ecid; }
        }

        public string ECIDStr
        {
            set { _ecidStr = value; }
            get { return _ecidStr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExamDesc
        {
            set { _examdesc = value; }
            get { return _examdesc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TotalScore
        {
            set { _totalscore = value; }
            get { return _totalscore; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 创建开始时间
        /// </summary>
        public DateTime? CreateBeginTime
        {
            set { _createBegintime = value; }
            get { return _createBegintime; }
        }
        /// <summary>
        /// 创建结束时间
        /// </summary>
        public DateTime? CreateEndTime
        {
            set { _createEndtime = value; }
            get { return _createEndtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreaetUserID
        {
            set { _creaetuserid = value; }
            get { return _creaetuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastModifyTime
        {
            set { _lastmodifytime = value; }
            get { return _lastmodifytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LastModifyUserID
        {
            set { _lastmodifyuserid = value; }
            get { return _lastmodifyuserid; }
        }
        public string ExamCategory
        {
            set { _examcategory = value; }
            get { return _examcategory; }
        }
        public int ExamPersonID
        {
            set { _exampersonid = value; }
            get { return _exampersonid; }
        }
        public string TestOverEndTime
        {
            set { _testoverendtime = value; }
            get { return _testoverendtime; }
        }
        public string NoTestEndTime
        {
            set { _notestendtime = value; }
            get { return _notestendtime; }
        }

        public int? BGID
        {
            get { return _bgid; }
            set { _bgid = value; }
        }
        #endregion Model

    }
}

