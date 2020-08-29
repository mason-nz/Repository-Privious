using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.BO.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Enum.UserInfo;
using XYAuto.ChiTu2018.Service.UserBankAccount.Dto;

namespace XYAuto.ChiTu2018.Service.UserBankAccount
{
    /// <summary>
    /// 注释：UserBankAccountService
    /// 作者：zhanglb
    /// 日期：2018/5/15 15:19:51
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class UserBankAccountService
    {
        private static readonly Lazy<UserBankAccountService> Linstance = new Lazy<UserBankAccountService>(() => new UserBankAccountService());

        public static UserBankAccountService Instance => Linstance.Value;

        /// <summary>
        /// 验证支付宝账号是否可用
        /// </summary>
        /// <param name="dto">请求实体</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        private string VerifBankAccount(ReqPayInfoDto dto, ref int userId)
        {
            if (dto == null)
                return "参数错误";

            if (!dto.IsAdd)
            {
                if (!Enum.IsDefined(typeof(UserBankAccountTypeEnum), dto.OldAccountType))
                    return "参数错误";

                if (string.IsNullOrWhiteSpace(dto.OldAccountName))
                    return "请输入旧支付宝账号！";
            }
            if (!Enum.IsDefined(typeof(UserBankAccountTypeEnum), dto.AccountType))
                return "参数错误";

            if (string.IsNullOrWhiteSpace(dto.AccountName))
                return "请输入新支付宝账号！";

            userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            if (!dto.IsAdd)
            {
                if (dto.OldAccountName.Trim() == dto.AccountName.Trim())
                    return "新支付宝账号与老支付宝账号一致";
                var bankAccount = new LeUserBankAccountBO().GetBankAccountInfo(dto.OldAccountType, dto.OldAccountName.Trim());
                if (bankAccount == null || bankAccount.UserID != userId)
                    return "旧支付宝账号输入有误！";
            }
            if (!XYAuto.CTUtils.Sys.VerifyHelper.IsHandset(dto.AccountName.Trim()) && !XYAuto.CTUtils.Sys.VerifyHelper.IsEmail(dto.AccountName.Trim()))
                return "支付宝账号格式不正确";
            var newBankAccount = new LeUserBankAccountBO().GetBankAccountInfo(dto.AccountType, dto.AccountName.Trim());
            return newBankAccount != null ? "您输入的支付宝账号已绑定其他账户" : "";
        }
        /// <summary>
        /// 验证支付宝账号是否可用
        /// </summary>
        /// <param name="dto">请求实体</param>
        /// <returns></returns>
        public string VerifBankAccount(ReqVerifPayInfoDto dto)
        {
            if (dto == null)
                return "参数错误";
            ReqPayInfoDto payInfo = new ReqPayInfoDto()
            {
                IsAdd = true,
                OldAccountName = dto.OldAccountName,
                OldAccountType = dto.AccountType,
                AccountName = dto.NewAccountName,
                AccountType = dto.AccountType
            };
            int userId = 0;
            var errorMsg = VerifBankAccount(payInfo, ref userId);
            if (!string.IsNullOrEmpty(errorMsg))
                return errorMsg;
            return "";
        }
        /// <summary>
        /// 保存保存支付账号
        /// </summary>
        /// <param name="dto">实体</param>
        /// <returns></returns>
        public string SavePayInfo(ReqPayInfoDto dto)
        {
            int userId = 0;
            var errorMsg = VerifBankAccount(dto, ref userId);
            if (!string.IsNullOrEmpty(errorMsg))
                return errorMsg;

            if (string.IsNullOrEmpty(new UserInfoBO().GetMobileByUserId(userId)))
                return "请先填写手机号";

            LE_UserBankAccount resultBankAccount;
            var bankAccountBo = new LeUserBankAccountBO();
            var oldBankAccount = bankAccountBo.GetBankAccountByUserId(userId);

            if (oldBankAccount == null)
            {
                var bankAccount = new LE_UserBankAccount()
                {
                    UserID = userId,
                    AccountName = dto.AccountName.Trim(),
                    Status = 0,
                    AccountType = dto.AccountType,
                    CreateTime = DateTime.Now
                };
                resultBankAccount = bankAccountBo.AddBankAccount(bankAccount);
            }
            else
            {
                oldBankAccount.AccountName = dto.AccountName.Trim();
                oldBankAccount.Status = 0;
                oldBankAccount.AccountType = dto.AccountType;
                resultBankAccount = bankAccountBo.UpdateBankAccount(oldBankAccount);
            }
            if (resultBankAccount != null) return "";
            CTUtils.Log.Log4NetHelper.Default($"SavePayInfo 失败：{JsonConvert.SerializeObject(dto)}");
            return "保存失败，请重试！";
        }

    }
}
