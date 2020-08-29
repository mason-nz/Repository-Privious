using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryProjectInfo 。(属性说明自动提取数据库字段的描述信息)
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
    public class QueryProjectInfo
    {
        public QueryProjectInfo()
        {
            _projectid = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _scid = Constant.INT_INVALID_VALUE;
            _name = Constant.STRING_INVALID_VALUE;
            _notes = Constant.STRING_INVALID_VALUE;
            _source = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;

            _begintime = Constant.STRING_INVALID_VALUE;//创建时间的开始时间
            _endtime = Constant.STRING_INVALID_VALUE;//创建时间的结束时间
            _statuss = Constant.STRING_INVALID_VALUE;//状态串 
            _pcatageid = Constant.INT_INVALID_VALUE;
            _ttcode = Constant.STRING_INVALID_VALUE;
            _demandid = Constant.STRING_EMPTY_VALUE;

            _batch = null;
        }

        public string wherePlus = string.Empty;

        #region Model
        private long _projectid;
        private int? _bgid;
        private int? _scid;
        private int? _pcatageid;
        private string _name;
        private string _notes;
        private int? _source;
        private int? _status;
        private DateTime? _createtime;
        private int? _createuserid;

        private string _begintime;
        private string _endtime;
        private string _statuss;
        private string _ttcode;
        private string _demandid;
        private int? _batch;

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

        public int? PCatageID
        {
            set { _pcatageid = value; }
            get { return _pcatageid; }
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
        /// 
        /// </summary>
        public int? Source
        {
            set { _source = value; }
            get { return _source; }
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



        public string BeginTime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        public string EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        public string Statuss
        {
            set { _statuss = value; }
            get { return _statuss; }
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

        public string ISAutoCall { get; set; }
        public string ACStatus { get; set; }

        #endregion Model


        public string Sources { get; set; }
    }
}

