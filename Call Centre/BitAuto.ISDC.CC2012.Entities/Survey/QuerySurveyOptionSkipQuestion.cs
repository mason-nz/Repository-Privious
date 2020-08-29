using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类SurveyOptionSkipQuestion 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-11-30 02:07:13 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QuerySurveyOptionSkipQuestion
    {
        public QuerySurveyOptionSkipQuestion()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _soid = Constant.INT_INVALID_VALUE;
            _sqid = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private int _recid;
        private int _soid;
        private int _sqid;
        private int? _status;
        private DateTime? _createtime;
        private int? _createuserid;
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
        public int SOID
        {
            set { _soid = value; }
            get { return _soid; }
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
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
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
        #endregion Model

    }
}

