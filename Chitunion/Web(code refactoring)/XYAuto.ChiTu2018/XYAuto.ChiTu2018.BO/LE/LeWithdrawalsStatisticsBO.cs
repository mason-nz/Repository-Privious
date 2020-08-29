using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.BO.LE
{
    /// <summary>
    /// 注释：LeWithdrawalsStatisticsBO
    /// 作者：lix
    /// 日期：2018/5/11 10:55:04
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LeWithdrawalsStatisticsBO
    {
        public LE_WithdrawalsStatistics GetInfo(int userId)
        {
            return IocMannager.Instance.Resolve<ILeWithdrawalsStatistics>().Retrieve(s => s.UserID == userId);
        }
    }
}
