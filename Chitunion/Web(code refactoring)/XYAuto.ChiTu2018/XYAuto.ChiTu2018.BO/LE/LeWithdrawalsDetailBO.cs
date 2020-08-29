/**
*
*创建人：lixiong
*创建时间：2018/5/9 9:57:05
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO;
using XYAuto.ChiTu2018.DAO.Chitunion2017;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Extend.LE;
using XYAuto.ChiTu2018.Entities.Query;
using XYAuto.ChiTu2018.Infrastructure.Exceptions;
using XYAuto.ChiTu2018.Infrastructure.Extensions;

namespace XYAuto.ChiTu2018.BO.LE
{
    public class LeWithdrawalsDetailBO
    {
        private readonly ILeWithdrawalsDetail _withdrawalsDetailContext;
        private readonly ILeWithdrawalsStatistics _leWithdrawalsStatistics;
        public LeWithdrawalsDetailBO()
        {
            _withdrawalsDetailContext = IocMannager.Instance.Resolve<ILeWithdrawalsDetail>();
            _leWithdrawalsStatistics = IocMannager.Instance.Resolve<ILeWithdrawalsStatistics>();
        }

        #region 基础测试

        public LE_WithdrawalsDetail GetById(int recId)
        {
            return _withdrawalsDetailContext.Retrieve(s => s.RecID == recId);
        }

        public void UpdateStatus(int recId)
        {
            var withdrawalContext = IocMannager.Instance.Resolve<ILeWithdrawalsDetail>();
            withdrawalContext.UpdateStatus(recId);
        }

        #endregion

        /// <summary>
        /// 当天是否存在提现记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsExist(int userId)
        {
            return _withdrawalsDetailContext.Queryable().AsNoTracking().Count(s => s.PayeeID == userId
                        && s.IsActive == 1 && DbFunctions.DiffDays(s.ApplicationDate, DateTime.Now) == 0) > 0;
        }

        /// <summary>
        /// 提交提现申请
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>item1:提现id item2:提交过程 id>0 && true 才是真正的成功</returns>
        [Obsolete("此方法暂时过期")]
        public Tuple<int, bool> PostApplication(LE_WithdrawalsDetail entity)
        {
            return IocMannager.Instance.Resolve<ILeWithdrawalsDetail>().PostApplication(entity);
        }

        public LE_WithdrawalsDetail GetMonthOfWithdrawalsMoney(int userId, DateTime startDay, DateTime endDay)
        {
            var resp = new LE_WithdrawalsDetail();
            var withdrawalContext = IocMannager.Instance.Resolve<ILeWithdrawalsDetail>().Queryable().AsNoTracking();

            var infos = withdrawalContext.Where(t => t.IsActive == 1
                                          && t.PayeeID == userId && t.ApplicationDate >= startDay
                                          && t.ApplicationDate < endDay
                                          && new int?[] { (int)WithdrawalsStatusEnum.支付中, (int)WithdrawalsStatusEnum.已支付 }.Contains(t.Status)).ToList();

            resp.WithdrawalsPrice = infos.Sum(t => t.WithdrawalsPrice.GetValueOrDefault(0));
            resp.IndividualTaxPeice = infos.Sum(t => t.IndividualTaxPeice.GetValueOrDefault(0));

            return resp;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<LE_WithdrawalsDetail> GetList(GetPageBase<LE_WithdrawalsDetail, int> query)
        {
            return _withdrawalsDetailContext.Queryable().AsNoTracking().Where(query.Expression).ToList();
        }

        /// <summary>
        /// 获取结算的总金额
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="statusEnum"></param>
        /// <returns></returns>
        public decimal GetSettlement(string startDate, string endDate, WithdrawalsStatusEnum statusEnum)
        {
            var stDate = Convert.ToDateTime(startDate);
            var edDate = Convert.ToDateTime(endDate);
            var prices = _withdrawalsDetailContext.Queryable()
                  .AsNoTracking()
                  .Where(s => s.AuditStatus == (int)WithdrawalsAuditStatusEnum.通过
                          && s.Status == (int)statusEnum && s.PayDate >= stDate && s.PayDate < edDate).Sum(s => s.WithdrawalsPrice);
            return prices ?? 0;
        }

        /// <summary>
        /// 提现明细列表接口
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<LeWithdrawalsIncomeDo> GetIncomeWithdrawalsQuery(GetPageBase<LeWithdrawalsIncomeDo, int> query)
        {
            var detailQueryable = _withdrawalsDetailContext.Queryable();
            var dicQueryable = IocMannager.Instance.Resolve<IDictInfo>().Queryable();
            var auditQueryable = IocMannager.Instance.Resolve<IAuditInfo>().Queryable();
            var count = 0;
            var queryList = from detail in detailQueryable
                            join dictInfo in dicQueryable on detail.Status equals dictInfo.DictId into tt
                            from t in tt.DefaultIfEmpty()
                            join auditInfo in auditQueryable on new { id = detail.RecID, tp = (int)AuditTypeEnum.提现审核 } equals new { id = auditInfo.RelationId, tp = auditInfo.RelationType } into aud
                            from t1 in aud.DefaultIfEmpty()
                            where detail.IsActive == 1
                            select new LeWithdrawalsIncomeDo
                            {
                                Id = detail.RecID,
                                RecId = detail.RecID,
                                PayeeId = detail.PayeeID.Value,
                                ApplicationDate = detail.ApplicationDate ?? new DateTime(),
                                AuditStatus = detail.AuditStatus ?? 0,
                                WithdrawalsPrice = detail.WithdrawalsPrice ?? 0,
                                IndividualTaxPeice = detail.IndividualTaxPeice ?? 0,
                                PracticalPrice = detail.PracticalPrice ?? 0,
                                PayeeAccount = detail.PayeeAccount,
                                PayStatus = detail.Status ?? 0,
                                PayDate = detail.PayDate ?? new DateTime(),
                                Reason = detail.Reason,
                                PayStatusName = t.DictName,
                                RejectMsg = t1.RejectMsg
                            };

            queryList = queryList.AsNoTracking().Pagination(query.Expression, query.Order, query.SortOrder, query.PageIndex, query.PageSize, out count);
            query.Total = count;
            return query.DataList = queryList.ToList();
        }

        public decimal GetTotalAmount(Expression<Func<LE_WithdrawalsDetail, bool>> expression)
        {
            return _withdrawalsDetailContext.Queryable().AsNoTracking().Where(s => s.IsActive == 1).Where(expression).Sum(s => s.WithdrawalsPrice).GetValueOrDefault();
        }

        /// <summary>
        /// 获取提现明细
        /// </summary>
        /// <param name="withdrawalsId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public LeWithdrawalsIncomeDo GetWithdrawalsDetailInfo(int withdrawalsId, int userId)
        {
            var query = (from w in IocMannager.Instance.Resolve<ILeWithdrawalsDetail>().Queryable().Where(p => p.RecID == withdrawalsId && p.PayeeID == userId)
                         join u in IocMannager.Instance.Resolve<IVUserInfo>().Queryable() on w.PayeeID equals u.UserID into wu
                         from wUser in wu.DefaultIfEmpty()
                         join d in IocMannager.Instance.Resolve<IDictInfo>().Queryable() on wUser.Type equals d.DictId into du
                         from dUser in du.DefaultIfEmpty()
                         join d2 in IocMannager.Instance.Resolve<IDictInfo>().Queryable() on w.Status equals d2.DictId into dw
                         from dWithdrawals in dw.DefaultIfEmpty()
                         join p in IocMannager.Instance.Resolve<ILeDisbursementPay>().Queryable() on w.RecID equals p.WithdrawalsId into dp
                         from pWithdrawals in dp.DefaultIfEmpty()
                         join a in IocMannager.Instance.Resolve<IAuditInfo>().Queryable().Where(p => p.RelationType == (int)AuditTypeEnum.提现审核) on w.RecID equals a.RelationId into aw
                         from aWithdrawals in aw.DefaultIfEmpty()
                         select new LeWithdrawalsIncomeDo
                         {
                             RecId = w.RecID,
                             WithdrawalsPrice = w.WithdrawalsPrice ?? 0,
                             IndividualTaxPeice = w.IndividualTaxPeice ?? 0,
                             PracticalPrice = w.PracticalPrice ?? 0,
                             PayeeAccount = w.PayeeAccount,
                             PayStatus = w.Status ?? 0,
                             ApplicationDate = w.ApplicationDate ?? new DateTime(),
                             PayDate = w.PayDate,
                             PayeeId = w.PayeeID ?? 0,
                             Reason = w.Reason,
                             CreateTime = w.CreateTime ?? new DateTime(),
                             TrueName = wUser.SysName,
                             UserName = wUser.UserName,
                             UserTypeName = dUser.DictName,
                             PayStatusName = dWithdrawals.DictName,
                             OrderNum = pWithdrawals.DisbursementNo,
                             RejectMsg = aWithdrawals.RejectMsg
                         }).FirstOrDefault();

            return query;

        }

        #region 事物相关

        /// <summary>
        /// 提交提现申请
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>item1:提现id item2:提交过程 id>0 && true 才是真正的成功</returns>
        public Tuple<int, bool> PostWithdrawas(LE_WithdrawalsDetail entity)
        {
            var executeCount = 0;
            var withdrawasId = 0;
            var count = 0;
            var result = false;

            using (var scope = new TransactionScope(TransactionScopeOption.Required)) //开启事务
            {
                try
                {
                    //添加申请
                    var returnEntity = _withdrawalsDetailContext.Add(entity);
                    if (returnEntity != null && returnEntity.RecID > 0)
                    {
                        executeCount++;
                        withdrawasId = returnEntity.RecID;
                    }
                    count++;

                    //修改用户资金冻结
                    executeCount += _leWithdrawalsStatistics.UpdatePostWithdrawas(entity.PayeeID.GetValueOrDefault(), entity.WithdrawalsPrice.GetValueOrDefault());
                    count++;

                    if (executeCount == count)
                    {
                        result = true;
                        scope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    //todo:异常捕获，不提交
                    throw new PostWithdrawasException($"PostWithdrawas 提现申请操作异常错误.{exception.Message}" +
                                                      $"{exception.StackTrace ?? string.Empty}");
                }
            }

            return Tuple.Create(withdrawasId, result);
        }

        #endregion
    }
}
