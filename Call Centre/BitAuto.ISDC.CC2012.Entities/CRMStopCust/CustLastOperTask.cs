using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类CustLastOperTask 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-07-23 10:58:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class CustLastOperTask
    {
        public CustLastOperTask()
        {
            _custid = Constant.STRING_INVALID_VALUE;
            _taskid = Constant.STRING_INVALID_VALUE;
            _tasktype = Constant.INT_INVALID_VALUE;
            _lastopertime = Constant.DATE_INVALID_VALUE;
            _lastoperuserid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;

        }
        #region Model
        private string _custid;
        private string _taskid;
        private int? _tasktype;
        private DateTime? _lastopertime;
        private int? _lastoperuserid;
        private DateTime? _createtime;
        private int? _createuserid;
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
        public string TaskID
        {
            set { _taskid = value; }
            get { return _taskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TaskType
        {
            set { _tasktype = value; }
            get { return _tasktype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastOperTime
        {
            set { _lastopertime = value; }
            get { return _lastopertime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LastOperUserID
        {
            set { _lastoperuserid = value; }
            get { return _lastoperuserid; }
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

