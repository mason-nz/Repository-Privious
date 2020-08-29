using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类ProjectInfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:28 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class ProjectInfo
    {
        public ProjectInfo()
        {
            _projectid = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _scid = Constant.INT_INVALID_VALUE;
            _name = Constant.STRING_INVALID_VALUE;
            _notes = Constant.STRING_INVALID_VALUE;
            _Source = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _pcatageid = Constant.INT_INVALID_VALUE;
            _isolddata = Constant.INT_INVALID_VALUE;
            _ttcode = Constant.STRING_INVALID_VALUE;
            _demandid = Constant.STRING_EMPTY_VALUE;
            _batch = null;
            _expectednum = null;
            _isblacklistcheck = null;
            _blacklistchecktype = null;
        }
        #region Model
        private long _projectid;
        private int? _bgid;
        private int? _scid;
        private string _name;
        private string _notes;
        private int? _Source;
        private int? _status;
        private DateTime? _createtime;
        private int? _createuserid;
        private int? _pcatageid;
        private int? _isolddata;
        private string _ttcode;
        private string _demandid;
        private int? _batch;
        private int? _expectednum;
        private int? _isblacklistcheck;
        private int? _blacklistchecktype;
        /// <summary>
        /// 
        /// </summary>
        public long ProjectID
        {
            set { _projectid = value; }
            get { return _projectid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SCID
        {
            set { _scid = value; }
            get { return _scid; }
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
        public string Notes
        {
            set { _notes = value; }
            get { return _notes; }
        }
        /// <summary>
        /// 数据来源：1——Excel导入，2——CRM，3——回访，4——其他任务
        /// </summary>
        public int? Source
        {
            set { _Source = value; }
            get { return _Source; }
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

        public int? PCatageID
        {
            set { _pcatageid = value; }
            get { return _pcatageid; }
        }

        public int? IsOldData
        {
            set { _isolddata = value; }
            get { return _isolddata; }
        }
        public string TTCode
        {
            set { _ttcode = value; }
            get { return _ttcode; }
        }
        public string DemandID
        {
            set { _demandid = value; }
            get { return _demandid; }
        }

        public int? Batch
        {
            get
            {
                return _batch;
            }
            set
            {
                _batch = value;
            }
        }
        public int? ExpectedNum
        {
            get
            {
                return _expectednum;
            }
            set
            {
                _expectednum = value;
            }
        }
        /// <summary>
        /// 是否启用黑名单校验 1启用 0禁用
        /// </summary>
        public int? IsBlacklistCheck
        {
            get { return _isblacklistcheck; }
            set { _isblacklistcheck = value; }
        }
        /// <summary>
        /// 黑名单校验方式：CRM=1 CC=2 全部=3
        /// </summary>
        public int? BlacklistCheckType
        {
            get { return _blacklistchecktype; }
            set { _blacklistchecktype = value; }
        }
        #endregion Model
    }
}

