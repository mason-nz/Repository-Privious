using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Entities.Dto
{
    public class MapGetRightADListResDTO
    {
        public List<ADGroupDTO> List { get; set; } = new List<ADGroupDTO>();
        public int TotalCount { get; set; }
    }
}