using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
{
    public class AdvIdToOptIdReqDTO
    {
        public int OperaterId { get; set; }
        public int OperateType { get; set; }
        public List<int> AdvertiserIds { get; set; }
    }
}
