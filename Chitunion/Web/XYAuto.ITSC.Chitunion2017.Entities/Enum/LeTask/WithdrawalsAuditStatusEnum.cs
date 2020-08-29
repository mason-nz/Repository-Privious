using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask
{
    public enum WithdrawalsAuditStatusEnum
    {
        无 = Entities.Constants.Constant.INT_INVALID_VALUE,
        通过 = 197001,
        驳回 = 197002,
        待审核 = 197003,
        //通过但支付失败 = 197003,
        //再次提交审核 = 197004,
    }
}
