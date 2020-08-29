/********************************
* 项目名称 ：XYAuto.ChiTu2018.BO.Profit
* 项目描述 ：
* 类 名 称 ：ProfitBO
* 类 描 述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/10 10:00:00
********************************/
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Profit;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Task;
using XYAuto.ChiTu2018.Entities.Enum.Profit;
using XYAuto.ChiTu2018.Entities.Extend.Profit;
using XYAuto.CTUtils.Log;
using XYAuto.CTUtils.Sys;

namespace XYAuto.ChiTu2018.BO.Profit
{
    public class ProfitBO
    {
        private readonly IProfit _profit;
        private readonly ILeWithdrawalsStatistics _leWithdrawalsStatistics;
        public ProfitBO()
        {
            _profit = IocMannager.Instance.Resolve<IProfit>();
            _leWithdrawalsStatistics = IocMannager.Instance.Resolve<ILeWithdrawalsStatistics>();
        }
        
        /// <summary>
        /// 添加用户收益
        /// </summary>
        /// <param name="profitType">收益类型</param>
        /// <param name="detailDescription">收益描述</param>
        /// <param name="incomPrice">收益金额</param>
        /// <param name="userId">用户Id</param>
        /// <param name="dtDate"></param>
        /// <param name="insertCount"></param>
        /// <returns></returns>
        public string AddProfit(int userId, ProfitTypeEnum profitType, string detailDescription, decimal incomPrice, DateTime? dtDate, int insertCount)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required)) //开启事务
            {
                try
                {
                    int count = _profit.InsertProfit(userId, (int)profitType, detailDescription, incomPrice, dtDate, insertCount);
                    if (count > 0)
                    {
                        _leWithdrawalsStatistics.AddWithdrawals(userId, incomPrice);
                    }
                    scope.Complete();
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }


        /// <summary>
        /// 获取收益列表
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        public Tuple<List<ProfitDo>, int> GetProFitList(ProfitQuery queryArgs)
        {
            Log4NetHelper.Default().Info($"ProfitBO：{queryArgs.PageIndex}——{queryArgs.PageSize}——{queryArgs.UserID}");
            var leftQuery = from p in IocMannager.Instance.Resolve<IProfit>().Queryable()
                            join w in IocMannager.Instance.Resolve<ILEWeiXinUser>().Queryable()
                            on p.ClickCount equals w.UserID into pw
                            from t in pw.DefaultIfEmpty()
                            where p.UserID == queryArgs.UserID
                            select new { t, p };
            var queryList = leftQuery.OrderByDescending(m => m.p.IncomeTime).Skip((queryArgs.PageIndex - 1) * queryArgs.PageSize).
                Take(queryArgs.PageSize).AsEnumerable().Select(m =>
                   new ProfitDo
                   {
                       RowNum = m.p.RecID,
                       ProfitType = m.p.CategoryID,
                       ProfitDate = m.p.IncomeTime != null ? ConverHelper.ObjectToDateTime(m.p.IncomeTime).ToString("yyyy-MM-dd") : string.Empty,
                       IncomeTime = m.p.IncomeTime.ToString(),
                       ProfitPrice = m.p.IncomePrice,
                       ProfitDescribe = m.p.DetailDescription,
                       TimeOrClick = (m.p.CategoryID == (int)ProfitTypeEnum.订单统计 ? $"{m.p.ClickCount}次" : ConverHelper.ObjectToDateTime(m.p.IncomeTime).ToString("hh:ss")),
                       Nickname = m.t?.nickname,
                       Headimgurl = m.t?.headimgurl,
                       ReadCount = m.p?.ClickCount
                   }
            ).ToList();
            return new Tuple<List<ProfitDo>, int>(queryList, leftQuery.Count());
        }
    }
}
