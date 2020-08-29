/********************************
* 项目名称 ：XYAuto.ChiTu2018.BO.Wechat
* 类 名 称 ：WechatSignBO
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/11 14:52:51
********************************/
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Wechat;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using System.Data.Entity.SqlServer;
using System.Data.Entity;

namespace XYAuto.ChiTu2018.BO.Wechat
{
    public class WechatSignBO
    {
        private readonly ILeDaySign _leDaySign;
        public WechatSignBO()
        {
            _leDaySign = IocMannager.Instance.Resolve<ILeDaySign>();
        }

        /// <summary>
        /// 根据用户ID获取最新签到信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public LE_DaySign GetSignNumber(int UserID)
        {
            return IocMannager.Instance.Resolve<ILeDaySign>().Queryable()
                .Where(p => p.SignUserID == UserID && DbFunctions.DiffDays(p.SignTime, DateTime.Now) >= 0)
                .OrderByDescending(p => p.SignNumber).FirstOrDefault();
        }
        /// <summary>
        /// 判断当前用户ID、IP是否黑名单
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public bool VeriftIsDaySign(int UserID, string IP)
        {
            return new LeIpBlacklistBO().VeriftIsExists(IP, LeIPBlacklistStatus.启用) == new LeUserBlacklistBO().VeriftIsExists(UserID, LeIPBlacklistStatus.启用);
        }
        /// <summary>
        /// 添加签到记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public LE_DaySign AddDaySign(LE_DaySign entity)
        {
            return IocMannager.Instance.Resolve<ILeDaySign>().Add(entity);
        }
        /// <summary>
        /// 查询通用类
        /// </summary>
        /// <returns></returns>
        public IQueryable<LE_DaySign> SelectDaySignListByMonth()
        {
            return IocMannager.Instance.Resolve<ILeDaySign>().Queryable();
        }
        public decimal GetTotalPriceByUserId(int signUserId, int categoryId)
        {
            var leIncomeInfo = new IocMannager()
                .Resolve<ILeIncomeStatisticsCategory>()
                .Queryable().FirstOrDefault(q => q.IncomeCategoryID == categoryId && q.UserID == signUserId);
            if (leIncomeInfo?.IncomePrice != null)
                return (decimal)leIncomeInfo
                    .IncomePrice;
            return 0;
        }

        /// <summary>
        /// 判断当天是否签到
        /// </summary>
        /// <param name="signUserId"></param>
        /// <param name="signTime"></param>
        /// <returns></returns>
        public bool IsDaySign(int signUserId, DateTime signTime)
        {
            return _leDaySign.Queryable().AsNoTracking().Count(s => s.SignUserID == signUserId && DbFunctions.DiffDays(s.SignTime, DateTime.Now) == 0) > 0;
        }
    }
}
