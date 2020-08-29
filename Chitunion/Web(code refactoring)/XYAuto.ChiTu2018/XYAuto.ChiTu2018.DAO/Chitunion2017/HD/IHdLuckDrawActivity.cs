using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.HD;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.HD
{
    /// <summary>
    /// 注释：IHdLuckDrawActivity
    /// 作者：lix
    /// 日期：2018/6/11 15:08:33
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public interface IHdLuckDrawActivity : Repository<HD_LuckDrawActivity>
    {
        int UpdateBonusBaseDrawNum(int activityId);
    }
}
