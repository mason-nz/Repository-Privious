using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.SysRight
{
    public class QueryModuleInfo
    {
        public QueryModuleInfo()
        {
            m_RecID = Constants.Constant.INT_INVALID_VALUE;
            m_pid = Constants.Constant.STRING_INVALID_VALUE;
            m_ExistModuleName = Constants.Constant.STRING_INVALID_VALUE;
            m_SysID = Constants.Constant.STRING_INVALID_VALUE;
            m_ModuleName = Constants.Constant.STRING_INVALID_VALUE;
            m_ModuleID = Constants.Constant.STRING_INVALID_VALUE;
            m_Level = Constants.Constant.INT_INVALID_VALUE;
            m_Intro = Constants.Constant.STRING_INVALID_VALUE;
            m_URL = Constants.Constant.STRING_INVALID_VALUE;
            m_Status = 0;
            m_ExceptModuleID = Constants.Constant.STRING_INVALID_VALUE;
            m_DomainCode = Constants.Constant.STRING_INVALID_VALUE;
        }
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
        private int m_Level;
        public int Level
        {
            get
            {
                return m_Level;
            }
            set
            {
                m_Level = value;
            }
        }
        private int m_RecID;
        public int RecID
        {
            get
            {
                return m_RecID;
            }
            set
            {
                m_RecID = value;
            }
        }
        private string m_pid;
        public string PID
        {
            get { return m_pid; }
            set { m_pid = value; }
        }
        private string m_ExistModuleName;
        public string ExistModuleName
        {
            get
            {
                return m_ExistModuleName;
            }
            set
            {
                m_ExistModuleName = value;
            }
        }
        private string m_ExceptModuleID;
        public string ExceptModuleID
        {
            get
            {
                return m_ExceptModuleID;
            }
            set
            {
                m_ExceptModuleID = value;
            }
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
        private string m_ModuleName;
        public string ModuleName
        {
            get
            {
                return m_ModuleName;
            }
            set
            {
                m_ModuleName = value;
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
        private string m_Intro;
        public string Intro
        {
            get
            {
                return m_Intro;
            }
            set
            {
                m_Intro = value;
            }
        }
        private string m_URL;
        public string URL
        {
            get
            {
                return m_URL;
            }
            set
            {
                m_URL = value;
            }
        }

        private string m_DomainCode;
        public string DomainCode
        {
            get
            {
                return m_DomainCode;
            }
            set
            {
                m_DomainCode = value;
            }
        }
    }
}
