using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetRecommendADResDTO
    {
        public int MediaID { get; set; }
        public int TemplateID { get; set; }
        public string ADName { get; set; }
        public string ADFormName { get; set; }
        public string ADLogo { get; set; }
        public decimal Price { get; set; }
    }
}
