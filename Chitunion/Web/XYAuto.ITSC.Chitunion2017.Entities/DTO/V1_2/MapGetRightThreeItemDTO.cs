using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
{
    public class MapGetRightThreeItemDTO
    {
        public DateTime Date { get; set; } = Entities.Constants.Constant.DATE_INVALID_VALUE;
        public string Key { get; set; }
        public decimal Value { get; set; }
    }
}
