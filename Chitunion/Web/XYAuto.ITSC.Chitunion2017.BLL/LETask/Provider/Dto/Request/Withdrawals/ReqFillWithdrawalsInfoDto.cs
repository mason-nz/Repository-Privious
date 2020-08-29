using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals
{
    public class ReqFillWithdrawalsInfoDto
    {
        [Necessary(MtName = "AccountType", IsValidateThanAt = true, ThanMaxValue = 50, Message = "{0}必须大于{1},请输入AccountType")]

        public int AccountType { get; set; }

        [Necessary(MtName = "AccountName")]
        public string AccountName { get; set; }

        [Necessary(MtName = "AccountNameAgain")]
        public string AccountNameAgain { get; set; }

        //[Necessary(MtName = "OldAccountName")]
        public string OldAccountName { get; set; }
    }
    
}
