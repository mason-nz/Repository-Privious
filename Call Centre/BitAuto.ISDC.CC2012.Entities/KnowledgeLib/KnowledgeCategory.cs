using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类KnowledgeCategory 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:09 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class KnowledgeCategory
    {
        public KnowledgeCategory()
        {
            _kcid = Constant.INT_INVALID_VALUE;
            _name = Constant.STRING_INVALID_VALUE;
            _level = Constant.INT_INVALID_VALUE;
            _pid = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _regionid = 0;
        }
        #region Model
        private int _kcid;
        private string _name;
        private int? _level;
        private int? _pid;
        private int? _status;
        private DateTime? _createtime;
        private int? _createuserid;
        private int _regionid;

        public int Regionid
        {
            get { return _regionid; }
            set { _regionid = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int KCID
        {
            set { _kcid = value; }
            get { return _kcid; }
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
        public int? Level
        {
            set { _level = value; }
            get { return _level; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Pid
        {
            set { _pid = value; }
            get { return _pid; }
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

