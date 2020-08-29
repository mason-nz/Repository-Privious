using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
{
    public class GetDemandListResDTO
    {
        public List<DemandDTO> List { get; set; } = new List<DemandDTO>();
        public int TotalCount { get; set; }
    }
}
