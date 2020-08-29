using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.SysRight
{
    public class SysInfo : DataBase
    {
        #region Instance
        public static readonly SysInfo Instance = new SysInfo();
        #endregion

        #region Contructor
        protected SysInfo()
        {
        }
        #endregion

        #region const
        public const string P_SYSINFO_SELECT = "p_SysInfo_select";
        public const string P_SYSINFO_UPDATE = "p_SysInfo_update";
        public const string P_SYSINFO_INSERT = "p_SysInfo_insert";
        public const string P_SYSINFO_SELECT_ALL = "p_SysInfo_select_All";
        #endregion

        public DataTable GetSysInfoAll()
        {
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SYSINFO_SELECT_ALL);
            return ds.Tables[0];
        }
    }
}
