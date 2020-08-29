using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{
    public class LeUserBlacklist : DataBase
    {
        public static readonly LeUserBlacklist Instance = new LeUserBlacklist();

        public bool VeriftIsExists(int userId, LeIPBlacklistStatus status)
        {
            var sql = $@"
                    SELECT COUNT(*) FROM dbo.LE_User_Blacklist WITH(NOLOCK) WHERE UserID = {userId} AND Status = {(int)status}
                ";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (obj == null)
            {
                return false;
            }
            else
            {
                return Convert.ToInt32(obj) > 0;
            }
        }
    }
}
