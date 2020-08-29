using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities.SysRight
{
    public class QueryRoleInfo
    {
        public QueryRoleInfo()
        {
            _sysid = Constant.STRING_INVALID_VALUE;
            _rolename = Constant.STRING_INVALID_VALUE;
            _recid = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private string _sysid;
        private string _rolename;
        private int _recid;
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
        public int RecID
        {
            set { _recid = value; }
            get { return _recid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SysID
        {
            set { _sysid = value; }
            get { return _sysid; }
        }
        #endregion Model
    }
}
