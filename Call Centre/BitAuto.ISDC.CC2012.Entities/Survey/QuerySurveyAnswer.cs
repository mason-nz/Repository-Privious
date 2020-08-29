using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QuerySurveyAnswer 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:19 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QuerySurveyAnswer
    {
        public QuerySurveyAnswer()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _spiid = Constant.INT_INVALID_VALUE;
            _siid = Constant.INT_INVALID_VALUE;
            _sqid = Constant.INT_INVALID_VALUE;
            _smrtid = Constant.INT_INVALID_VALUE;
            _smctid = Constant.INT_INVALID_VALUE;
            _soid = Constant.INT_INVALID_VALUE;
            _answercontent = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;

            _filternull = Constant.INT_INVALID_VALUE;//是否过滤空选项。1-过滤
            _ptid = Constant.STRING_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
        }
        private string _custid;
        public string CustID
        {
            set
            {
                _custid = value;
            }
            get
            {
                return _custid;
            }
        }
        #region Model

        private string _ptid;
        public string PTID
        {
            get
            {
                return _ptid;
            }
            set
            {
                _ptid = value;
            }
        }

        private int _recid;

        public int RecID
        {
            get { return _recid; }
            set { _recid = value; }
        }
        private int _spiid;
        private int _siid;
        private int _sqid;
        private int? _smrtid;
        private int? _smctid;
        private int? _soid;
        private string _answercontent;
        private DateTime? _createtime;
        private int _createuserid;
        private int _filternull;
        /// <summary>
        /// 
        /// </summary>
        public int SPIID
        {
            set { _spiid = value; }
            get { return _spiid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SIID
        {
            set { _siid = value; }
            get { return _siid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SQID
        {
            set { _sqid = value; }
            get { return _sqid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SMRTID
        {
            set { _smrtid = value; }
            get { return _smrtid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SMCTID
        {
            set { _smctid = value; }
            get { return _smctid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SOID
        {
            set { _soid = value; }
            get { return _soid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AnswerContent
        {
            set { _answercontent = value; }
            get { return _answercontent; }
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
        public int CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        public int FilterNull
        {
            set { _filternull = value; }
            get { return _filternull; }
        }
        #endregion Model

    }
}

