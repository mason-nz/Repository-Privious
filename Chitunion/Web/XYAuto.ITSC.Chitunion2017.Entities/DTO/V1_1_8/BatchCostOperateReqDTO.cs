using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class BatchCostOperateReqDTO
    {
        public List<int> CostIDList { get; set; }
        public int OpType { get; set; }
    }
}
