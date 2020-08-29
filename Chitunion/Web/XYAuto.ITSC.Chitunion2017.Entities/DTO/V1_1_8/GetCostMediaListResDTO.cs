using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class GetCostMediaListResDTO
    {
        public List<GetCostMediaItem> List { get; set; }
    }

    public class GetCostMediaItem {
        public int MediaID { get; set; }
        public string MediaName { get; set; }
    }
}
