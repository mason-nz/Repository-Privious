using System;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto.Response.Withdrawals;

namespace XYAuto.ChiTu2018.Service.LE
{
    /// <summary>
    /// 注释：LeWithdrawalsStatisticsService
    /// 作者：lix
    /// 日期：2018/5/14 15:33:15
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LeWithdrawalsStatisticsService
    {
        #region 单例

        private LeWithdrawalsStatisticsService() { }
        private static readonly Lazy<LeWithdrawalsStatisticsService> Linstance = new Lazy<LeWithdrawalsStatisticsService>(() => { return new LeWithdrawalsStatisticsService(); });

        public static LeWithdrawalsStatisticsService Instance => Linstance.Value;

        #endregion

        /// <summary>
        /// 收入管理-收益详情统计
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RespIncomeInfoDto GetIncomeInfo(int userId)
        {
            var resp = new RespIncomeInfoDto();
            var info = new LeWithdrawalsStatisticsBO().GetInfo(userId);
            if (info == null)
            {
                return resp;
            }
            resp.AlreadyWithdrawalsMoney = info.HaveWithdrawals.GetValueOrDefault(0);
            resp.EarningsPrice = info.AccumulatedIncome.GetValueOrDefault(0);
            resp.WithdrawalsMoneyIng = info.WithdrawalsProcess.GetValueOrDefault(0);
            //todo:可提现金额：累计收益-已提现-提现中
            resp.CanWithdrawalsMoney = info.RemainingAmount.GetValueOrDefault(0);
            
            return resp;
        }
    }
}
