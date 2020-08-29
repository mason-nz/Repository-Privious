using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities.SysRight
{
    public class QuerySysInfo
    {
        public QuerySysInfo()
        {
            m_userID = Constant.INT_INVALID_VALUE;
            m_RecID = Constant.INT_INVALID_VALUE;
            m_SysID = Constant.STRING_INVALID_VALUE;
            m_SysName = Constant.STRING_INVALID_VALUE;
            m_ExistSysName = Constant.STRING_INVALID_VALUE;
            m_Status = 0;
            m_SysURL = Constant.STRING_INVALID_VALUE;
            ModuleType = Constant.INT_INVALID_VALUE;
        }
        /// <summary>
        /// 系统名称
        /// </summary>
        private string m_SysName;
        public string SysName
        {
            get
            {
                return m_SysName;
            }
            set
            {
                m_SysName = value;
            }
        }
        /// <summary>
        /// 系统URL
        /// </summary>
        private string m_SysURL;
        public string SysURL
        {
            get
            {
                return m_SysURL;
            }
            set
            {
                m_SysURL = value;
            }
        }
        private string m_ExistSysName;
        /// <summary>
        /// 存在查询时，系统名称
        /// </summary>
        public string ExistSysName
        {
            get
            {
                return m_ExistSysName;
            }
            set
            {
                m_ExistSysName = value;
            }
        }
        /// <summary>
        /// 系统名称
        /// </summary>
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
        /// <summary>
        /// 系统编号
        /// </summary>
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
        /// <summary>
        /// Status
        /// </summary>
        private int m_userID;
        public int UserID
        {
            get
            {
                return m_userID;
            }
            set
            {
                m_userID = value;
            }
        }
        /// <summary>
        /// 查询有特定类型模块权限的系统
        /// </summary>
        public int ModuleType { get; set; }
    }
}
