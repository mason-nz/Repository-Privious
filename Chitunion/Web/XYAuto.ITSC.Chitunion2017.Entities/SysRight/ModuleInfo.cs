using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities.SysRight
{
    /// <summary>
    /// 实体类ModuleInfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class ModuleInfo
    {
        public ModuleInfo()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _sysid = Constant.STRING_INVALID_VALUE;
            _moduleid = Constant.STRING_INVALID_VALUE;
            _modulename = Constant.STRING_INVALID_VALUE;
            _pid = Constant.STRING_INVALID_VALUE;
            _level = Constant.INT_INVALID_VALUE;
            _intro = Constant.STRING_INVALID_VALUE;
            _url = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _links = Constant.STRING_INVALID_VALUE;
            _ordernum = Constant.INT_INVALID_VALUE;
            _domainid = Constant.INT_INVALID_VALUE;
        }
        #region Model
        //public int Blank { get; set; }
        private int _recid;
        private string _moduleid;
        private string _sysid;
        private string _modulename;
        private string _pid;
        private int _level;
        private string _intro;
        private string _url;
        private int _status;
        private DateTime _createtime;
        private string _links;
        private int _ordernum;
        private int _domainid;
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
        public string ModuleID
        {
            set { _moduleid = value; }
            get { return _moduleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SysID
        {
            set { _sysid = value; }
            get { return _sysid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ModuleName
        {
            set { _modulename = value; }
            get { return _modulename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PID
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
        public string Intro
        {
            set { _intro = value; }
            get { return _intro; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        public string Links
        {
            set { _links = value; }
            get { return _links; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OrderNum
        {
            set { _ordernum = value; }
            get { return _ordernum; }
        }

        /// <summary>
        /// 系统域名ID
        /// </summary>
        public int DomainID
        {
            set { _domainid = value; }
            get { return _domainid; }
        }
        #endregion Model

    }
}
