using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryTPage 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:35 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryTPage
    {
        public QueryTPage()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _tpname = Constant.STRING_INVALID_VALUE;
            _tpcid = Constant.INT_INVALID_VALUE;
            _tpref = Constant.STRING_INVALID_VALUE;
            _ttcode = Constant.STRING_INVALID_VALUE;
            _sttcode = Constant.STRING_INVALID_VALUE;
            _tpcontent = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _scid = Constant.INT_INVALID_VALUE;
            _statuss = Constant.STRING_INVALID_VALUE;
            _ttname = Constant.STRING_INVALID_VALUE;
            _loginid = Constant.INT_INVALID_VALUE;//登陆人ID
            _owngroup = Constant.STRING_EMPTY_VALUE;//权限是本组的权限组；格式：个人用户组,业务组...
            _oneself = Constant.STRING_EMPTY_VALUE;//权限是本人的权限组；格式：个人用户组,业务组...

            _begintime = Constant.STRING_INVALID_VALUE;//创建时间的开始时间
            _endtime = Constant.STRING_INVALID_VALUE;//创建时间的结束时间
            _isused = Constant.INT_INVALID_VALUE;
            _isshowqichetong = Constant.INT_INVALID_VALUE;
            _isshowsubmitorder = Constant.INT_INVALID_VALUE;
        }
        private string _statuss;
        public string Statuss
        {
            set { _statuss = value; }
            get { return _statuss; }

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
        private string _begintime;
        private string _endtime;
        private int? _bgid;
        private int? _scid;
        #region Model
        private int _recid;
        private string _tpname;
        private int? _tpcid;
        private string _tpref;
        private string _ttcode;
        private string _sttcode;
        private string _tpcontent;
        private int? _status;
        private DateTime? _createtime;
        private int? _createuserid;
        private int _isshowqichetong;
        private int _isshowsubmitorder;
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
        public string TPName
        {
            set { _tpname = value; }
            get { return _tpname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TPCID
        {
            set { _tpcid = value; }
            get { return _tpcid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TPRef
        {
            set { _tpref = value; }
            get { return _tpref; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TTCode
        {
            set { _ttcode = value; }
            get { return _ttcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string STTCode
        {
            set { _sttcode = value; }
            get { return _sttcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TPContent
        {
            set { _tpcontent = value; }
            get { return _tpcontent; }
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
        private string _ttname;
        /// <summary>
        /// 
        /// </summary>
        public string TTName
        {
            set { _ttname = value; }
            get { return _ttname; }
        }

        private int _loginid;

        public int LoginID
        {
            get { return _loginid; }
            set { _loginid = value; }
        }

        private string _owngroup;
        private string _oneself;
        public string OwnGroup
        {
            set { _owngroup = value; }
            get { return _owngroup; }
        }
        public string OneSelf
        {
            set { _oneself = value; }
            get { return _oneself; }
        }
        private int _isused;

        public int IsUsed
        {
            get { return _isused; }
            set { _isused = value; }
        }
        public int IsShowQiCheTong
        {
            set { _isshowqichetong = value; }
            get { return _isshowqichetong; }
        }
        public int IsShowSubmitOrder
        {
            set { _isshowsubmitorder = value; }
            get { return _isshowsubmitorder; }
        }

    }
}

