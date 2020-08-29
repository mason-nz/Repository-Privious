using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
{

    public class GetDemandDetailResDTO
    {
        public DemandDTO Demand { get; set; }
        public List<ADGroupDTO> ADGroupList { get; set; } = new List<ADGroupDTO>();
    }
}
