using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QS_RulesTable 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:37 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QS_RulesTable
    {
        public QS_RulesTable()
        {
            _qs_rtid = Constant.INT_INVALID_VALUE;
            _name = Constant.STRING_INVALID_VALUE;
            _scoretype = Constant.INT_INVALID_VALUE;
            _description = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _statusInUse = Constant.INT_INVALID_VALUE;
            _deaditemnum = Constant.INT_INVALID_VALUE;
            _nodeaditemnum = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _lastmodifytime = Constant.DATE_INVALID_VALUE;
            _lastmodifyuserid = Constant.STRING_INVALID_VALUE;
            _haveqappraisal = Constant.INT_INVALID_VALUE;
            _regionid = Constant.STRING_INVALID_VALUE;
        }
        #region Model
        private int _qs_rtid;
        private string _name;
        private int _scoretype;
        private string _description;
        private int? _status;
        private int? _statusInUse;//使用状态：该评分表是否被使用过
        private int? _deaditemnum;
        private int? _nodeaditemnum;
        private DateTime? _createtime;
        private int? _createuserid;
        private DateTime? _lastmodifytime;
        private string _lastmodifyuserid;
        private int? _haveqappraisal;
        private string _regionid;

        public string RegionID
        {
            get { return _regionid; }
            set { _regionid = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int QS_RTID
        {
            set { _qs_rtid = value; }
            get { return _qs_rtid; }
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
        public int ScoreType
        {
            set { _scoretype = value; }
            get { return _scoretype; }
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
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? StatusInUse
        {
            set { _statusInUse = value; }
            get { return _statusInUse; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DeadItemNum
        {
            set { _deaditemnum = value; }
            get { return _deaditemnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? NoDeadItemNum
        {
            set { _nodeaditemnum = value; }
            get { return _nodeaditemnum; }
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
        public DateTime? LastModifyTime
        {
            set { _lastmodifytime = value; }
            get { return _lastmodifytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LastModifyUserID
        {
            set { _lastmodifyuserid = value; }
            get { return _lastmodifyuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? HaveQAppraisal
        {
            set { _haveqappraisal = value; }
            get { return _haveqappraisal; }
        }
        #endregion Model

    }
}

