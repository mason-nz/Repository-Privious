using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Dto;
using XYAuto.ITSC.Chitunion2017.Dal.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia
{
    public class WithdrawalsBll
    {
        #region 初始化
        private WithdrawalsBll() { }

        public static WithdrawalsBll instance = null;
        public static readonly object padlock = new object();

        public static WithdrawalsBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new WithdrawalsBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 获取收益统计列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public WithdrawalsDto GetWithdrawalsStatisticsList(QueryWithdrawalsArgs query)
        {
            var enumModel = SplitHelper.GetEnumDescriptionList<IncomeOrder>(query.OrderBy + string.Empty);
            var ResultQuery = WithdrawalsDa.Instance.GetWithdrawalsStatisticsList(query, enumModel.Description);
           
            return new WithdrawalsDto
            {
                List = ResultQuery.Item2,
                TotalCount = ResultQuery.Item1,
                AccountBalanceTotal = ResultQuery.Item3.AccountBalanceTotal,
                HaveWithdrawalsTotal = ResultQuery.Item3.HaveWithdrawalsTotal,
                RemainingAmountTotal = ResultQuery.Item3.RemainingAmountTotal,
                WithdrawalsProcessTotal = ResultQuery.Item3.WithdrawalsProcessTotal
            };
        }
        /// <summary>
        /// 根据用户ID获取收益明细列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IncomeDetailDto GetIncomeDetailModelList(QueryWithdrawalsArgs query)
        {
            var ResultQuery = WithdrawalsDa.Instance.GetIncomeDetailModelList(query);
            return new IncomeDetailDto
            {
                List = ResultQuery.Item2,
                TotalCount = ResultQuery.Item1,
                DaySignSum = ResultQuery.Item3!=null? ResultQuery.Item3.DaySignSum:0,
                InciteSum = ResultQuery.Item3 != null ? ResultQuery.Item3.InciteSum : 0,
                OrderSum = ResultQuery.Item3 != null ? ResultQuery.Item3.OrderSum : 0
            };
        }
    }
}
