using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.HD;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.HD;
using XYAuto.ChiTu2018.Entities.Chitunion2017.HD;

namespace XYAuto.ChiTu2018.BO.HD
{
    /// <summary>
    /// 注释：HdLuckDrawPrizeBO
    /// 作者：lix
    /// 日期：2018/6/11 15:16:39
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class HdLuckDrawPrizeBO
    {
        private readonly IHdLuckDrawPrize _hdLuckDrawPrize;
        public HdLuckDrawPrizeBO()
        {
            _hdLuckDrawPrize = IocMannager.Instance.Resolve<IHdLuckDrawPrize>();
        }

        /// <summary>
        ///  获取奖项列表
        /// </summary>
        /// <param name="activityId">活动Id</param>
        /// <returns></returns>

        public List<HD_LuckDrawPrize> GetPrizeList(int activityId)
        {
            return _hdLuckDrawPrize.Queryable().AsNoTracking().Where(s => s.ActivityId == activityId && s.Status == 0).ToList();
        }
    }
}
