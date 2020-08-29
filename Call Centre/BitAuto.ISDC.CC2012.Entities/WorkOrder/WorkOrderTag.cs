using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类WorkOrderTag 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:21 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class WorkOrderTag
    {
        public WorkOrderTag()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _pid = Constant.INT_INVALID_VALUE;
            _tagname = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _modifytime = Constant.DATE_INVALID_VALUE;
            _modifyuserid = Constant.INT_INVALID_VALUE;
            _orderunm = Constant.INT_INVALID_VALUE;

        }
        #region Model
        private int _recid;
        private int _bgid;
        private int _pid;
        private string _tagname;
        private int? _status;
        private DateTime? _createtime;
        private int? _createuserid;
        private DateTime? _modifytime;
        private int? _modifyuserid;
        private int? _orderunm;
        /// <summary>
        /// 
        /// </summary>
        public int RecID
        {
            set { _recid = value; }
            get { return _recid; }
        }

        public int? OrderNum
        {
            set { _orderunm = value; }
            get { return _orderunm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
        }
        public int PID
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TagName
        {
            set { _tagname = value; }
            get { return _tagname; }
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
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyTime
        {
            set { _modifytime = value; }
            get { return _modifytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ModifyUserID
        {
            set { _modifyuserid = value; }
            get { return _modifyuserid; }
        }
        #endregion Model

    }
}

