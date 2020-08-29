using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Entities.Dto
{
    public class GetDemandDetailResDTO
    {
        public DemandDTO Demand { get; set; }
        public List<ADGroupDTO> ADGroupList { get; set; } = new List<ADGroupDTO>();
    }
}