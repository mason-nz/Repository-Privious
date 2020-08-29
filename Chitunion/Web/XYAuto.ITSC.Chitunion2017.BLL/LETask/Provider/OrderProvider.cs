using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.ThirdApi;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider
{
    /// <summary>
    /// auth:lixiong
    /// desc:媒体主-后台-订单
    /// </summary>
    public class OrderProvider : VerifyOperateBase
    {
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public RespOrderInfoDto GetOrderInfo(int orderId, int userId)
        {
            var respDto = new RespOrderInfoDto();
            var orderInfo = Dal.LETask.LeAdOrderInfo.Instance.GetAdOrderInfo(orderId, userId);
            if (orderInfo == null)
            {
                return respDto;
            }
            return AutoMapper.Mapper.Map<Entities.LETask.LeAdOrderInfo, RespOrderInfoDto>(orderInfo);
        }

        /// <summary>
        /// 订单收入详情列表-统计
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public RespOrderIncomeDto GetOrderIncomeList(int orderId, int userId)
        {
            var respDto = new RespOrderIncomeDto();
            var tupList = Dal.LETask.LeAccountBalance.Instance.GetAccountBalances(orderId, userId);
            respDto.List = AutoMapper.Mapper.Map<List<Entities.LETask.LeAccountBalance>, List<OrderIncomeItem>>(tupList.Item1);
            respDto.Extend = AutoMapper.Mapper.Map<Entities.LETask.TotalAccountBalance, OrderIncomeTotalItem>(tupList.Item2);
            return respDto;
        }

        /// <summary>
        /// 收入管理-收益详情统计
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RespIncomeInfoDto GetIncomeInfo(int userId)
        {
            var resp = new RespIncomeInfoDto();
            var info = Dal.LETask.LeWithdrawalsStatistics.Instance.GetInfo(userId);
            if (info == null)
            {
                return resp;
            }
            resp.AlreadyWithdrawalsMoney = info.HaveWithdrawals;
            resp.EarningsPrice = info.AccumulatedIncome;
            resp.WithdrawalsMoneyIng = info.WithdrawalsProcess;
            //todo:可提现金额：累计收益-已提现-提现中

            resp.CanWithdrawalsMoney = info.RemainingAmount;
            resp.TodayIncome = Dal.LETask.LeWithdrawalsStatistics.Instance.GetTodayIncome(userId);
            return resp;
        }

        /// <summary>
        /// 媒体主个人中心-统计信息
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        public RespUserStatDto GetUserStatInfo(LoginUser userEntity)
        {
            var resp = Dal.LETask.LeAccountBalance.Instance.GetUserStatInfo(userEntity.UserID) ?? new RespUserStatDto();

            //获取微信信息
            var weixinUser = Dal.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(userEntity.UserID, (UserCategoryEnum)userEntity.Category);
            //weixinUser.NickName = weixinUser != null ? weixinUser.nickname : string.Empty;

            resp.UserId = userEntity.UserID;
            resp.UserName = userEntity.UserName;
            return resp;
        }

    }
}
