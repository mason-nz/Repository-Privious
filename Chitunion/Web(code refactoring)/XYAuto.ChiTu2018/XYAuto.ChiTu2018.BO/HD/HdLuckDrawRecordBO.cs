using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.HD;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Profit;
using XYAuto.ChiTu2018.DAO.Chitunion2017.User;
using XYAuto.ChiTu2018.DAO.Chitunion2017.View;
using XYAuto.ChiTu2018.Entities.Chitunion2017.HD;
using XYAuto.ChiTu2018.Entities.Chitunion2017.View;
using XYAuto.ChiTu2018.Entities.Enum.Profit;
using XYAuto.ChiTu2018.Entities.Extend.HD;

namespace XYAuto.ChiTu2018.BO.HD
{
    /// <summary>
    /// 注释：HdLuckDrawRecordBO
    /// 作者：lix
    /// 日期：2018/6/11 15:16:55
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class HdLuckDrawRecordBO
    {
        private readonly IHdLuckDrawRecord _leHdLuckDrawRecord;
        private readonly IHdLuckDrawPrize _leHdLuckDrawPrize;
        private readonly IHdLuckDrawActivity _hdLuckDrawActivity;
        private readonly ILeWithdrawalsStatistics _leWithdrawalsStatistics;
        private readonly IProfit _profit;
        public HdLuckDrawRecordBO()
        {
            _leHdLuckDrawRecord = IocMannager.Instance.Resolve<IHdLuckDrawRecord>();
            _leHdLuckDrawPrize = IocMannager.Instance.Resolve<IHdLuckDrawPrize>();
            _hdLuckDrawActivity = IocMannager.Instance.Resolve<IHdLuckDrawActivity>();
            _leWithdrawalsStatistics = IocMannager.Instance.Resolve<ILeWithdrawalsStatistics>();
            _profit = IocMannager.Instance.Resolve<IProfit>();
        }

        public int Insert(HD_LuckDrawRecord hdLuckDrawRecord)
        {
            var entity = _leHdLuckDrawRecord.Add(hdLuckDrawRecord);
            return entity?.RecId ?? 0;
        }

        /// <summary>
        /// 获取已抽奖次数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="drawTime"></param>
        /// <returns></returns>
        public int GetDrawRemainder(int userId, DateTime drawTime)
        {
            return _leHdLuckDrawRecord.Queryable().AsNoTracking().Count(s => s.Status == 0 && s.UserId == userId && DbFunctions.DiffDays(s.DrawTime, DateTime.Now) == 0);
        }

        public int GetDrawRemainder(int userId)
        {
            return _leHdLuckDrawRecord.Queryable().AsNoTracking().Count(s => s.Status == 0 && s.UserId == userId);
        }

        public HdSumDrawInfoDo GetSumDrawInfo(int prizeId)
        {
            //var list = _leHdLuckDrawRecord.Queryable().AsNoTracking().Where(s => s.Status == 0 && s.PrizeId == prizeId).GroupBy(s => s.PrizeId)
            //    .Select(s => new HdSumDrawInfoDo
            //    {
            //        SumDrawPrice = s.Sum(t => t.DrawPrice),
            //        SumCount = s.Count()
            //    }).ToList();

            var info = from a in _leHdLuckDrawRecord.Queryable().AsNoTracking()
                       where a.Status == 0 && a.PrizeId == prizeId
                       group a by a.PrizeId
                        into b
                       select new HdSumDrawInfoDo
                       {
                           SumDrawPrice = b.Sum(s => s.DrawPrice),
                           SumCount = b.Count()
                       };

            return info.FirstOrDefault();
        }

        /// <summary>
        /// 获取用户获奖记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页码</param>
        /// <param name="totalCount">总条数</param>
        public List<HdAwardRecordDo> GetAwardRecord(int userId, int pageIndex, int pageSize, out int totalCount)
        {
            var list = from drecord in _leHdLuckDrawRecord.Queryable().AsNoTracking()
                       join prize in _leHdLuckDrawPrize.Queryable().AsNoTracking() on drecord.ActivityId equals prize.ActivityId into tt
                       from t in tt.DefaultIfEmpty()
                       where drecord.Status == 0 && drecord.UserId == 1 && drecord.DrawPrice > 0
                       select new HdAwardRecordDo
                       {
                           AwardName = t.AwardName,
                           DrawPrice = drecord.DrawPrice,
                           DrawTime = drecord.DrawTime
                       };
            totalCount = list.Count();
            return list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 查询获奖人名单
        /// </summary>
        /// <param name="topCount">行数</param>
        /// <returns></returns>
        public List<HdAwardeeInfoDo> GetAwardeeList(int topCount)
        {
            var wxUserQueryable = IocMannager.Instance.Resolve<IVLeWeiXinUser>().Queryable().AsNoTracking();
            var userQueryable = IocMannager.Instance.Resolve<IUserInfo>().Queryable().AsNoTracking();
            var list = from drecord in _leHdLuckDrawRecord.Queryable().AsNoTracking()
                       join wxUser in wxUserQueryable on drecord.UserId equals wxUser.UserID into tt
                       from t in tt.DefaultIfEmpty()
                       join user in userQueryable on drecord.UserId equals user.UserID
                       select new HdAwardeeInfoDo
                       {
                           NickName = t.nickname,
                           DrawPrice = drecord.DrawPrice,
                           DrawTime = drecord.DrawTime,
                           DrawDescribe = drecord.DrawDescribe,
                           Mobile = user.Mobile
                       };
            return list.Take(topCount).ToList();
        }

        public string LotteryDraw(HD_LuckDrawRecord drawRecord, int maxDrawCount)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required)) //开启事务
            {
                try
                {
                    var count = _profit.InsertProfit(drawRecord.UserId, (int)ProfitTypeEnum.签到抽奖, drawRecord.DrawDescribe, drawRecord.DrawPrice, DateTime.Now, maxDrawCount);
                    if (count > 0)
                    {
                        //todo:InsertAwardRecord
                        drawRecord.CreateTime = DateTime.Now;
                        drawRecord.DrawTime = DateTime.Now;
                        Insert(drawRecord);
                        //todo:UpdateBonusBaseDrawNum
                        _hdLuckDrawActivity.UpdateBonusBaseDrawNum(drawRecord.ActivityId);
                        //todo:AddWithdrawals
                        _leWithdrawalsStatistics.AddWithdrawals(drawRecord.UserId, drawRecord.DrawPrice);
                    }
                    scope.Complete();
                    return string.Empty;
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }
        }
    }
}
