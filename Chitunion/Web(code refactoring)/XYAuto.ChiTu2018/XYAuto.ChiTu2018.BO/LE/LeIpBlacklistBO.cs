/**
*
*创建人：lixiong
*创建时间：2018/5/10 14:47:21
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;

namespace XYAuto.ChiTu2018.BO.LE
{
    public class LeIpBlacklistBO
    {
        public bool VeriftIsExists(string requestIp, LeIPBlacklistStatus status)
        {
            return
                new IocMannager()
                    .Resolve<ILeIpBlacklist>()
                    .Queryable()
                    .AsNoTracking().Count(s => s.IP.Equals(requestIp) && s.Status == (int)status) > 0;
        }
    }
}
