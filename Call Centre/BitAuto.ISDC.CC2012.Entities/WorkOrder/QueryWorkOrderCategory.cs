using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryWorkOrderCategory 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:20 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryWorkOrderCategory
    {
        public QueryWorkOrderCategory()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _name = Constant.STRING_INVALID_VALUE;
            _pid = Constant.INT_INVALID_VALUE;
            _level = Constant.INT_INVALID_VALUE;
            _usescope = Constant.INT_INVALID_VALUE;
            _ordernum = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;

            _usescopestr = Constant.STRING_INVALID_VALUE;
        }
        #region Model
        private int _recid;
        private string _name;
        private int _pid;
        private int _level;
        private int? _usescope;
        private int? _ordernum;
        private int? _status;
        private DateTime? _createtime;

        private string _usescopestr;
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PID
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            set { _level = value; }
            get { return _level; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? UseScope
        {
            set { _usescope = value; }
            get { return _usescope; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderNum
        {
            set { _ordernum = value; }
            get { return _ordernum; }
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
        public string UseScopeStr
        {
            set { _usescopestr = value; }
            get { return _usescopestr; }
        }
        #endregion Model

    }
}

