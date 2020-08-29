using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.SysRight
{
    public class QueryRoleModule
    {
        public QueryRoleModule()
        {
            m_ModuleID = Constants.Constant.STRING_INVALID_VALUE;
            m_RoleID = Constants.Constant.STRING_INVALID_VALUE;
            m_SysID = Constants.Constant.STRING_INVALID_VALUE;
            m_Status = 0;
        }
        private string m_SysID;
        public string SysID
        {
            get
            {
                return m_SysID;
            }
            set
            {
                m_SysID = value;
            }
        }
        private string m_ModuleID;
        public string ModuleID
        {
            get
            {
                return m_ModuleID;
            }
            set
            {
                m_ModuleID = value;
            }
        }
        private string m_RoleID;
        public string RoleID
        {
            get
            {
                return m_RoleID;
            }
            set
            {
                m_RoleID = value;
            }
        }
        /// <summary>
        /// Status
        /// </summary>
        private int m_Status;
        public int Status
        {
            get
            {
                return m_Status;
            }
            set
            {
                m_Status = value;
            }
        }
    }
}
