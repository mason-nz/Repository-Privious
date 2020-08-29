using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.IpBlacklist
{
    public class IpBlacklist : DataBase
    {
        public readonly static IpBlacklist Instance = new IpBlacklist();
        /// <summary>
        /// 查询Ip是否在黑名单中
        /// </summary>
        /// <param name="Ip"></param>
        /// <returns></returns>
        public bool IsBlackIp(string Ip)
        {
            string sql = $@"SELECT COUNT(1) FROM  LE_IP_Blacklist WHERE IP='{Ip}' AND Status=0";

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? false : (Convert.ToInt32(obj) > 0 ? true : false);
        }

    }
}
