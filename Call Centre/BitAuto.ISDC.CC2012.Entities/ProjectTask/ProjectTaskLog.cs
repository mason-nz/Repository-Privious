using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类ProjectTaskLog 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class ProjectTaskLog
    {
        public ProjectTaskLog()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _ptid = Constant.STRING_INVALID_VALUE;
            _taskstatus = Constant.INT_INVALID_VALUE;
            _description = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _operationstatus = Constant.INT_INVALID_VALUE;

        }
        #region Model
        private long _recid;
        private string _ptid;
        private int? _taskstatus;
        private string _description;
        private DateTime? _createtime;
        private int? _createuserid;
        private int? _operationstatus;

        public int? OperationStatus
        {
            set { _operationstatus = value; }
            get { return _operationstatus; }
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
        public string PTID
        {
            set { _ptid = value; }
            get { return _ptid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TaskStatus
        {
            set { _taskstatus = value; }
            get { return _taskstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
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

