using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;

namespace XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request.Withdrawals
{
    public class ReqWithdrawalsDto
    {
        /// <summary>
        /// 提现金额：必填
        /// </summary>
        [Necessary(MtName = "WithdrawalsPrice", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入WithdrawalsPrice")]
        public decimal WithdrawalsPrice { get; set; }

        public string Mobile { get; set; }

        public WithdrawalsApplySourceEnum ApplySource { get; set; }
    }
}
