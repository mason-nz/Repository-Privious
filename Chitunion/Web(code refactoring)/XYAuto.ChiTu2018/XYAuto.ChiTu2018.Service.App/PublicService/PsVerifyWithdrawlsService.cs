using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.BO.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Query;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.App.AppInfo.VerifyEntity;

namespace XYAuto.ChiTu2018.Service.App.PublicService
{
    /// <summary>
    /// 注释：PsVerifyWithdrawlsService
    /// 作者：lix
    /// 日期：2018/6/1 16:19:55
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class PsVerifyWithdrawlsService : VerifyOperateBase
    {
        #region 返回错误消息

        public string GetVerifyMessage(int key)
        {
            var dicError = new Dictionary<int, string>()
            {
                { 0,"验证通过"},
                { 1011,"请先补充认证信息！"},
                { 1012,"认证信息审核未通过，请修改认证信息！"},
                { 1013,"请到账号管理中完善提现账号！"},
                { 1014,"用户提现手机号与注册手机号不一致！"},
                { 1015,"可提现金额不足，无法提现！"},
                { 1016,"每天只能提现1次，请明天再试！"},
                { 1017,"您有正在支付中的提现申请，请在提现完成后再申请！"},
                { 1018,"还未绑定手机号！"},
                { 1019,"提现账户有金额,无法提现！"}
            };
            return dicError.Where(s => s.Key == key).Select(s => s.Value).FirstOrDefault() ?? "校验未通过！";
        }


        public string GetVerifyAuditMessage(int key)
        {
            var dicError = new Dictionary<int, string>()
            {
                { 0,"验证通过"},
                { 1011,"请先补充认证信息！"},
                { 1012,"认证信息审核未通过，请修改认证信息！"},
                { 1013,"请到账号管理中完善提现账号！"},
                //{ 1014,"用户提现手机号与注册手机号不一致"},
                { 1015,"可提现金额不足，无法提现(对账错误)"},
            };
            return dicError.Where(s => s.Key == key).Select(s => s.Value).FirstOrDefault() ?? "校验未通过！";
        }


        #endregion

        #region 提现校验规则

        /// <summary>
        /// 提现操作-校验规则
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="withdrawalsMobile">提现手机号</param>
        /// <param name="withdrawalsMoney">提现金额</param>
        /// <returns></returns>
        public VerifyResultCode VerifyWithdrawals(int userId, string withdrawalsMobile, decimal withdrawalsMoney)
        {
            var respCode = new VerifyResultCode();
            var userInfoDo = new UserInfoBO().GetJoinDo(userId);
            if (userInfoDo == null)
            {
                respCode.ResultCode = 1011;//请先补充认证信息,或用户信息不存在
                return respCode;
            }
            if (userInfoDo.AuditStatus != 2)
            {
                respCode.ResultCode = 1012;//用户资质没有审核通过
                return respCode;
            }
            if (string.IsNullOrWhiteSpace(userInfoDo.Mobile))
            {
                respCode.ResultCode = 1018;//未绑定手机号
                return respCode;
            }

            //if (new WithdrawalsProvider(new ConfigEntity() { UserId = userId }, new ReqWithdrawalsDto()).VerifyUserBank(new ReturnValue()).HasError)
            //{
            //    respCode.ResultCode = 1013;//用户还未添加提现账户
            //    return respCode;
            //}
            var leWithdrawalsDetailBo = new LeWithdrawalsDetailBO();

            var query = new GetPageBase<LE_WithdrawalsDetail, int>()
            {
                Expression = s => s.IsActive == 1 && s.PayeeID == userId && DbFunctions.DiffDays(DateTime.Now, s.ApplicationDate).Value == 0
            };
            if (leWithdrawalsDetailBo.GetList(query).Any())
            {
                respCode.ResultCode = 1016;//一天只能提现一次
                return respCode;
            }
            query.Expression =
                s => s.IsActive == 1 && s.PayeeID == userId && s.Status == (int)WithdrawalsStatusEnum.支付中;
            if (leWithdrawalsDetailBo.GetList(query).Any())
            {
                respCode.ResultCode = 1017;//有正在提现中的申请，不能多次申请同时进行
                return respCode;
            }

            var leStatistic = new LeWithdrawalsStatisticsBO().GetInfo(userId);
            if (leStatistic.RemainingAmount.GetValueOrDefault(0) < withdrawalsMoney)
            {
                respCode.ResultCode = 1015;//可提现金额不足，无法提现
                return respCode;
            }
            if (leStatistic.WithdrawalsProcess > 0)
            {
                respCode.ResultCode = 1019;//提现账户有金额,无法提现
                return respCode;
            }
            return respCode;
        }


        #endregion

        #region 校验ip黑名单，用户黑名单

        /// <summary>
        /// 校验ip黑名单，用户黑名单
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="ip">客户端ip</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public ReturnValue VerifyAntiCheating(ReturnValue retValue, string ip, int userId)
        {
            if (new LeIpBlacklistBO().VeriftIsExists(ip, LeIPBlacklistStatus.启用))
            {
                return CreateFailMessage(retValue, "10055", "此ip已被拉入黑名单");
            }
            if (new LeUserBlacklistBO().VeriftIsExists(userId, LeIPBlacklistStatus.启用))
            {
                return CreateFailMessage(retValue, "10056", "此用户已被拉入黑名单");
            }

            return retValue;
        }

        #endregion

        #region 校验用户提现帐号

        /// <summary>
        /// 校验用户提现帐号
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ReturnValue VerifyUserBank(ReturnValue retValue, int userId)
        {
            var accountBank = new LeUserBankAccountBO().GetByUserId(userId, UserBankAccountTypeEnum.Zfb).FirstOrDefault();
            if (accountBank == null)
            {
                return CreateFailMessage(retValue, "1013", "暂未查询到提现帐号");
            }
            retValue.ReturnObject = accountBank.AccountName;
            return retValue;
        }

        #endregion

        #region 校验提现来源

        public ReturnValue VerifyApplySource(ReturnValue retValue, WithdrawalsApplySourceEnum applySource)
        {
            if (!new DictInfoBO().GetList(201).Exists(s => s.DictId == (int)applySource))
            {
                return CreateFailMessage(retValue, "3019", "提现申请渠道错误");
            }
            return retValue;
        }

        #endregion
    }
}
