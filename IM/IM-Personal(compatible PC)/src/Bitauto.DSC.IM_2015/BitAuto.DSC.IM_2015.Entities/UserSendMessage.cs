using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Entities
{
    [Serializable]
    public class SMSTemplate
    {
        public SMSTemplate()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _scid = Constant.INT_INVALID_VALUE;
            _title = Constant.STRING_INVALID_VALUE;
            _content = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;

        }
        #region Model
        private int _recid;
        private int? _bgid;
        private int? _scid;
        private string _title;
        private string _content;
        private int? _status;
        private DateTime? _createtime;
        private int? _createuserid;
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
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
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

    [Serializable]
    public class QuerySMSTemplate
    {
        public QuerySMSTemplate()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _scid = Constant.INT_INVALID_VALUE;
            _title = Constant.STRING_INVALID_VALUE;
            _content = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _loginid = Constant.INT_INVALID_VALUE;

        }
        #region Model
        private int _recid;
        private int? _bgid;
        private int? _scid;
        private string _title;
        private string _content;
        private int? _status;
        private DateTime? _createtime;
        private int? _createuserid;
        private int? _loginid;
        public int? LoginID
        {
            set { _loginid = value; }
            get { return _loginid; }
        }
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
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
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

    }


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

        #endregion Model

    }


    [Serializable]
    public class SMSSendHistory
    {
        public SMSSendHistory()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _templateid = Constant.INT_INVALID_VALUE;
            _phone = Constant.STRING_INVALID_VALUE;
            _content = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _crmcustid = Constant.STRING_INVALID_VALUE;
            _tasktype = Constant.INT_INVALID_VALUE;
            _taskid = Constant.STRING_INVALID_VALUE;

        }
        #region Model
        private int _recid;
        private int? _bgid;
        private int? _templateid;
        private string _phone;
        private string _content;
        private int? _status;
        private DateTime? _createtime;
        private int? _createuserid;
        private string _custid;
        private string _crmcustid;
        private int? _tasktype;
        private string _taskid;
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
        public int? BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TemplateID
        {
            set { _templateid = value; }
            get { return _templateid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
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
        public string CustID
        {
            set { _custid = value; }
            get { return _custid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CRMCustID
        {
            set { _crmcustid = value; }
            get { return _crmcustid; }
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
        public string TaskID
        {
            set { _taskid = value; }
            get { return _taskid; }
        }
        #endregion Model

    }
}
