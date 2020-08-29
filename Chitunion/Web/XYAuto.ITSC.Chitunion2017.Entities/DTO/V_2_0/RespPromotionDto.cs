using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V_2_0
{
    public class RespPromotionDto
    {
        public int RecID { get; set; }
        public string Name { get; set; }
        public string CarStyleText { get; set; }
        public string AreaText { get; set; }
        public string MaterialUrl { get; set; }
        public string Remark { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public decimal BudgetPrice { get; set; }
        public string StatusName { get; set; }
        public string CreateTime { get; set; }
    }
}
