using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Entities.Dto
{
    public class GetDemandListResDTO
    {
        public List<DemandDTO> List { get; set; } = new List<DemandDTO>();
        public int TotalCount { get; set; }
    }
}