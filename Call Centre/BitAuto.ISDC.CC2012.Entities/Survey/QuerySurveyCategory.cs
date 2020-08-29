using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QuerySurveyCategory 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QuerySurveyCategory
    {
        public QuerySurveyCategory()
        {
            _scid = Constant.INT_INVALID_VALUE;
            _name = Constant.STRING_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _level = Constant.INT_INVALID_VALUE;
            _pid = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;

            _selecttype = Constant.INT_INVALID_VALUE;//为1，则筛选登陆人管理的所属业务组的信息
            _loginid = Constant.INT_INVALID_VALUE;//登陆人ID
            _groupname = Constant.STRING_INVALID_VALUE;//筛选所属业务组名称的业务组分类名称
            _typeid = Constant.INT_INVALID_VALUE;
            _isfilterstop = false;
            _nostatus = Constant.INT_INVALID_VALUE;

            _groupStatus = Constant.STRING_EMPTY_VALUE;
        }
        #region Model
        //add by qizq 2014-4-17 状态不能某值，用于过滤等于-3（固定的分类）的
        private int? _nostatus;
        private int _scid;
        private string _name;
        private int? _bgid;
        private int? _level;
        private int? _pid;
        private int? _status;
        private DateTime? _createtime;
        private int? _createuserid;
        private int _selecttype;
        private int _loginid;
        private string _groupname;
        private int? _typeid;

        private bool _isfilterstop;

        private string _groupStatus;
        /// <summary>
        /// 
        /// </summary>
        public int SCID
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
        public int? BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Level
        {
            set { _level = value; }
            get { return _level; }
        }
        //add by qizq 2014-4-17 状态不能某值，用于过滤等于-3（固定的分类）的
        public int? NoStatus
        {
            set { _nostatus = value; }
            get { return _nostatus; }
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
        public int SelectType
        {
            set { _selecttype = value; }
            get { return _selecttype; }
        }
        public int LoginID
        {
            set { _loginid = value; }
            get { return _loginid; }
        }
        public string GroupName
        {
            set { _groupname = value; }
            get { return _groupname; }
        }
        public string GroupStatus
        {
            set { _groupStatus = value; }
            get { return _groupStatus; }
        }
        
        #endregion Model

        /// <summary>
        /// 1 问卷类型   2 项目类型
        /// </summary>
        public int? TypeId
        {
            set { _typeid = value; }
            get { return _typeid; }
        }

        public bool IsFilterStop
        {
            set { _isfilterstop = value; }
            get { return _isfilterstop; }
        }

        /// <summary>
        /// 排除选项：强斐 2016-5-17
        /// </summary>
        public string Exclude { get; set; }
    }
}

