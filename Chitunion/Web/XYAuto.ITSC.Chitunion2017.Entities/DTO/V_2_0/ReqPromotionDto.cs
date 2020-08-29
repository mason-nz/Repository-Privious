using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V_2_0
{
    public class ReqPromotionDto
    {
        public string Name { get; set; }
        public string MaterialUrl { get; set; }
        public string Remark { get; set; } = string.Empty;
        public List<PromotionArea> AreaList { get; set; }
        public List<PromotionCar> CarList { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public decimal BudgetPrice { get; set; }

    }
    public class PromotionArea
    {
        public int ProvinceID { get; set; }
        public int CityID { get; set; }
    }
    public class PromotionCar
    {
        public int MakeID { get; set; }
        public int ModelID { get; set; }
    }
}
