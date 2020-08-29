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
    public class LeIPBlacklist : DataBase
    {

        public static readonly LeIPBlacklist Instance = new LeIPBlacklist();

        public Entities.LETask.LeIPBlacklist GetInfo(string requestIp, LeIPBlacklistStatus status)
        {
            var sql = $@"
                    SELECT TOP 1
                            B.RecID ,
                            B.IP ,
                            B.ConstraintID ,
                            B.Status ,
                            B.CreateTime
                    FROM    dbo.LE_IP_Blacklist AS B
                    WHERE   B.IP = '{requestIp}' AND B.Status = {(int)status}
                ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeIPBlacklist>(obj.Tables[0]);
        }

    }
}
