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
    /// 注释：HdLuckDrawActivityBO
    /// 作者：lix
    /// 日期：2018/6/11 15:16:08
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class HdLuckDrawActivityBO
    {
        private readonly IHdLuckDrawActivity _hdLuckDrawActivity;
        public HdLuckDrawActivityBO()
        {
            _hdLuckDrawActivity = IocMannager.Instance.Resolve<IHdLuckDrawActivity>();
        }

        /// <summary>
        ///  获取抽奖活动有效期和奖池金额
        /// </summary>
        /// <returns></returns>

        public List<HD_LuckDrawActivity> GetActivityInfo()
        {
            return _hdLuckDrawActivity.Queryable().AsNoTracking().Where(s => s.Status == 0).ToList();
        }

   }
}
