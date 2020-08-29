using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.IpBlacklist
{
    public class IpBlacklist
    {
        public readonly static IpBlacklist Instance = new IpBlacklist();

        /// <summary>
        /// 查询Ip是否在黑名单中
        /// </summary>
        /// <param name="ip">用户IP</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public bool IsBlackIp(string ip, int userId)
        {
            if (Dal.IpBlacklist.IpBlacklist.Instance.IsBlackIp(ip.Trim()))
                return true;
            if (Dal.LETask.LeUserBlacklist.Instance.VeriftIsExists(userId, LeIPBlacklistStatus.启用))
                return true;
            return false;
        }

        /// <summary>
        /// 查询UserID，是否在黑名单账号表中
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsBlack(int userId)
        {
            return Dal.LETask.LeUserBlacklist.Instance.VeriftIsExists(userId, LeIPBlacklistStatus.启用);
        }
    }
}
