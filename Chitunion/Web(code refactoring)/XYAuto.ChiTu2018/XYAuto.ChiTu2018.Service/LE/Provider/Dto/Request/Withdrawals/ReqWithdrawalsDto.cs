using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;

namespace XYAuto.ChiTu2018.Service.LE.Provider.Dto.Request.Withdrawals
{
    public class ReqWithdrawalsDto
    {
        /// <summary>
        /// 提现金额：必填
        /// </summary>
        [Necessary(MtName = "WithdrawalsPrice", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入WithdrawalsPrice")]
        public decimal WithdrawalsPrice { get; set; }

        //[Necessary(MtName = "MsgCode")]
        //public string MsgCode { get; set; }
        //[Necessary(MtName = "Mobile")]
        public string Mobile { get; set; }
        public WithdrawalsApplySourceEnum ApplySource { get; set; }

        /// <summary>
        /// 客户端ip
        /// </summary>
        public string Ip { get; set; }
    }


}
