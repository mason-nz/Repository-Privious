using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities.SysRight
{
    /// <summary>
    /// 实体类RoleInfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class RoleInfo
    {
        public RoleInfo()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _roleid = Constant.STRING_INVALID_VALUE;
            _sysid = Constant.STRING_INVALID_VALUE;
            _rolename = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _intro = Constant.STRING_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private int _recid;
        private string _roleid;
        private string _sysid;
        private string _rolename;
        private string _intro;
        private int _status;
        private DateTime _createtime;
        private int _createuserid;
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
        public string RoleID
        {
            set { _roleid = value; }
            get { return _roleid; }
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
        public string RoleName
        {
            set { _rolename = value; }
            get { return _rolename; }
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

        public int CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        #endregion Model

    }
}
