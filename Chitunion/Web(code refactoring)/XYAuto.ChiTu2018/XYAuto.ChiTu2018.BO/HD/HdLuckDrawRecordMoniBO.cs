using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.HD;
using XYAuto.ChiTu2018.Entities.Chitunion2017.HD;

namespace XYAuto.ChiTu2018.BO.HD
{
    /// <summary>
    /// 注释：HdLuckDrawRecordMoniBO
    /// 作者：lix
    /// 日期：2018/6/12 14:29:53
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class HdLuckDrawRecordMoniBO
    {
        private readonly IHdLuckDrawRecordMoni _hdLuckDrawRecordMoni;
        public HdLuckDrawRecordMoniBO()
        {
            _hdLuckDrawRecordMoni = IocMannager.Instance.Resolve<IHdLuckDrawRecordMoni>();
        }

        /// <summary>
        /// 查询获奖人名单（假数据）
        /// </summary>
        /// <param name="topCount">行数</param>
        /// <returns></returns>
        public List<HD_LuckDrawRecord_Moni> GetAwardeeMoniList(int topCount)
        {
            return _hdLuckDrawRecordMoni.Queryable().AsNoTracking().Where(s => s.Status == 0 && s.ActivityId == 1 && DbFunctions.DiffDays(DateTime.Now, s.DrawTime) >= 0)
                .OrderBy(s => s.DrawTime).Take(topCount).ToList();
        }
    }
}
