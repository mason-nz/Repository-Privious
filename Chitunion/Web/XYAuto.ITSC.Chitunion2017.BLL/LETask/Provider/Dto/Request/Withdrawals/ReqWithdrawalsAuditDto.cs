using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals
{
    public class ReqWithdrawalsAuditDto
    {

        [Necessary(MtName = "WithdrawalsId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入WithdrawalsId")]
        public int WithdrawalsId { get; set; }

        public WithdrawalsAuditStatusEnum AuditStatus { get; set; }


        public string RejectMsg { get; set; }
    }
}
