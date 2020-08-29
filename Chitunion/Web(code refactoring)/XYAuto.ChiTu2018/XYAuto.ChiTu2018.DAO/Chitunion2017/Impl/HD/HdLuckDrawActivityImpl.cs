using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.DAO.Chitunion2017.HD;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.HD;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.HD
{
    /// <summary>
    /// 注释：HdLuckDrawActivity
    /// 作者：lix
    /// 日期：2018/6/11 15:12:30
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public sealed class HdLuckDrawActivityImpl : RepositoryImpl<HD_LuckDrawActivity>, IHdLuckDrawActivity
    {
        public int UpdateBonusBaseDrawNum(int activityId)
        {
            var info = context.HD_LuckDrawActivity.Find(activityId);
            if (info == null)
            {
                return 0;
            }
            //info.DrawNum = info.DrawNum + 1;
            //return context.SaveChanges();
            string strSql = $"UPDATE dbo.HD_LuckDrawActivity SET DrawNum=DrawNum+1 WHERE ActivityId={activityId}";

            return context.Database.ExecuteSqlCommand(strSql);
        }
    }
}
