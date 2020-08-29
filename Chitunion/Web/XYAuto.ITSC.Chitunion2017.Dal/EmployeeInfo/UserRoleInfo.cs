using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.EmployeeInfo
{
    public class UserRoleInfo : DataBase
    {
        public static readonly UserRoleInfo Instance = new UserRoleInfo();

        public DataTable GetInsideRoles()
        {
            string sql = "SELECT SysID,RoleID,RoleName FROM dbo.RoleInfo WHERE Status=0 AND not (RoleID='SYS001RL00002' OR  RoleID='SYS001RL00003') ";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

    }
}
