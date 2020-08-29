using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals
{
    public class ReqWithdrawalsDto
    {
        [Necessary(MtName = "WithdrawalsPrice", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入WithdrawalsPrice")]
        public decimal WithdrawalsPrice { get; set; }

        //[Necessary(MtName = "MsgCode")]
        public string MsgCode { get; set; }
        //[Necessary(MtName = "Mobile")]
        public string Mobile { get; set; }
        public WithdrawalsApplySourceEnum ApplySource { get; set; } = WithdrawalsApplySourceEnum.Pc;

    }


}
