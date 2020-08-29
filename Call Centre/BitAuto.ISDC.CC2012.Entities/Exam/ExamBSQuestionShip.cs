using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类ExamBSQuestionShip 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-03 02:04:15 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class ExamBSQuestionShip
    {
        public ExamBSQuestionShip()
        {
            _bqid = Constant.INT_INVALID_VALUE;
            _klqid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _No = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private long _bqid;
        private long _klqid;
        private DateTime? _createtime;
        private int? _createuserid;
        private int _No;
        /// <summary>
        /// 
        /// </summary>
        public long BQID
        {
            set { _bqid = value; }
            get { return _bqid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long KLQID
        {
            set { _klqid = value; }
            get { return _klqid; }
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
        /// 编号
        /// </summary>
        public int  NO
        {
            set { _No = value; }
            get { return _No; }
        }

        #endregion Model

    }
}

