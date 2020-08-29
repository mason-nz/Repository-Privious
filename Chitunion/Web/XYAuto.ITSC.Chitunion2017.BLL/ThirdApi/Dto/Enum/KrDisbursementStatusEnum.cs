using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Enum
{
    public enum KrDisbursementStatusEnum
    {
        待处理 = 0,

        已锁定 = 2,
        已撤销 = 4,
        处理中 = 6,
        待审核 = 8,
        结算中 = 10,
        结算成功 = 12,
        结算失败 = 14,
    }
}
