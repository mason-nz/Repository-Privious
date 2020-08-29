using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;

namespace XYAuto.ChiTu2018.Service.App.PublicService.Dto.Request.Withdrawals
{
    public class PsReqWithdrawalsDto
    {
        /// <summary>
        /// 提现金额：必填
        /// </summary>
        [Necessary(MtName = "WithdrawalsPrice", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入WithdrawalsPrice")]
        public decimal WithdrawalsPrice { get; set; }

        public string Mobile { get; set; }
        
        public WithdrawalsApplySourceEnum ApplySource { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Necessary(MtName = "UserId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入UserId")]
        public int UserId { get; set; }

        /// <summary>
        /// 客户端ip
        /// </summary>
        [Necessary(MtName = "IP")]
        public string Ip { get; set; }
    }
    
}
