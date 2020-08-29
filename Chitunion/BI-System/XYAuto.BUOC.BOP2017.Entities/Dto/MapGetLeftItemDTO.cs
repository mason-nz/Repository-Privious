using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Entities.Dto
{
    public class MapGetLeftItemDTO
    {
        public int DemandBillNo { get; set; }
        public string Name { get; set; }
        public List<ADGroupDTO> ADGroupList { get; set; } = new List<ADGroupDTO>();
    }
}