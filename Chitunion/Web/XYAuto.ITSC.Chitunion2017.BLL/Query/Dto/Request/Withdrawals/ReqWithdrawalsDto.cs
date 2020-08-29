using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Withdrawals
{
    public class ReqWithdrawalsDto : ReqInComeDto
    {
        public int OrderStatus { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public string UserName { get; set; }//申请人

        public int AuditStatus { get; set; }

        public string BeginPayDate { get; set; }//支付时间

        public string EndPayDate { get; set; }//支付时间
    }
}
