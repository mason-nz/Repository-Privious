/**
*
*创建人：lixiong
*创建时间：2018/5/9 9:54:54
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/

using System;
using System.Data.Entity;
using System.Transactions;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.LE
{
    public sealed class LeWithdrawalsDetailImpl : RepositoryImpl<LE_WithdrawalsDetail>, ILeWithdrawalsDetail
    {
        public void UpdateStatus(int recId)
        {
            var entity = context.LE_WithdrawalsDetail.Attach(new LE_WithdrawalsDetail()
            {
                IsActive = 0,
                RecID = recId
            });
            context.Entry(entity).Property(p => p.IsActive).IsModified = true;
            context.SaveChanges();
        }

        /// <summary>
        /// 提交提现申请
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>item1:提现id item2:提交过程 id>0 && true 才是真正的成功</returns>
        [Obsolete("此方法已放弃")]
        public Tuple<int, bool> PostApplication(LE_WithdrawalsDetail entity)
        {
            int executeCount = 0;
            int count = 0;
            bool result = false;
            using (var db = new Chitunion2017DbContext())
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))//开启事务
                {
                    //添加申请
                    db.Entry(entity).State = EntityState.Added;

                    //修改用户资金冻结
                    var sql = $@"
                            UPDATE  dbo.LE_WithdrawalsStatistics
                            SET     WithdrawalsProcess = ISNULL(WithdrawalsProcess, 0) + {entity.WithdrawalsPrice}
                                    , RemainingAmount = ISNULL(RemainingAmount,0) - {entity.WithdrawalsPrice}
                            WHERE   UserID = {entity.PayeeID};
                            ";
                    executeCount += db.SaveChanges();
                    count++;


                    executeCount += db.Database.ExecuteSqlCommand(sql);
                    count++;

                    if (executeCount == count)
                    {
                        result = true;
                        scope.Complete();
                    }
                }
            }
            return Tuple.Create(entity.RecID, result);
        }

       
    }
}
