using System;
using System.Linq;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.LE
{
    /// <summary>
    /// 注释：LeWithdrawalsStatisticsImpl
    /// 作者：lix
    /// 日期：2018/5/11 10:54:00
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public sealed class LeWithdrawalsStatisticsImpl : RepositoryImpl<LE_WithdrawalsStatistics>, ILeWithdrawalsStatistics
    {
        /// <summary>
        /// 用户提现-更新资金账户统计-修改用户资金冻结
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public int UpdatePostWithdrawas(int userId, decimal money)
        {
            var userStat = context.LE_WithdrawalsStatistics.Where(s => s.UserID == userId);
            var userStatCount = userStat.Count();
            if (userStatCount > 1)
            {
                throw new Exception($"用户存在多个统计账户:{userId}");
            }
            if (userStatCount == 0)
            {
                throw new Exception($"用户不存在统计账户:{userId}");
            }
            //var info = userStat.FirstOrDefault();

            //info.WithdrawalsProcess = info.WithdrawalsProcess + money;
            //info.RemainingAmount = info.RemainingAmount - money;
            
            //context.LE_WithdrawalsStatistics.Attach(info);
            //var entry = context.Entry(info);
            //entry.Property(e => e.WithdrawalsProcess).IsModified = true;
            //entry.Property(e => e.RemainingAmount).IsModified = true;

            //return context.SaveChanges();

            //todo:这里还是用sql 会比较安全，怕中途有资金变动
            var sql = $@"
                            UPDATE  dbo.LE_WithdrawalsStatistics
                            SET     WithdrawalsProcess = ISNULL(WithdrawalsProcess, 0) + {money}
                                    , RemainingAmount = ISNULL(RemainingAmount,0) - {money}
                            WHERE   UserID = {userId};
                            ";

            return context.Database.ExecuteSqlCommand(sql);

        }

        /// <summary>
        /// 添加用户金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="incomPrice"></param>
        /// <returns></returns>
        public int AddWithdrawals(int userId, decimal incomPrice)
        {
            string sql = $@"DECLARE @StatisticsCount INT;
                            SELECT @StatisticsCount = COUNT(1)
                            FROM LE_WithdrawalsStatistics
                            WHERE UserID = {userId};
                                        IF(@StatisticsCount > 0)
                                BEGIN
                                    UPDATE  LE_WithdrawalsStatistics
                                    SET     AccumulatedIncome += {incomPrice} ,
                                            RemainingAmount = ISNULL(RemainingAmount,
                                                                     AccumulatedIncome - HaveWithdrawals)
                                            + {incomPrice}
                                    WHERE UserID = {userId};
                                        END;
                                        ELSE
                                            BEGIN
                                    INSERT INTO dbo.LE_WithdrawalsStatistics
                                           (AccumulatedIncome,
                                             HaveWithdrawals,
                                             WithdrawalsProcess,
                                             UserID,
                                             CreateTime,
                                             Status,
                                             RemainingAmount
                                           )
                                    VALUES( {incomPrice}, 
                                              0, 
                                              0, 
                                              {userId}, 
                                              GETDATE(), 
                                              0, 
                                              {incomPrice}
                                            );
                                        END; ";
            return context.Database.ExecuteSqlCommand(sql);
        }
    }
}
