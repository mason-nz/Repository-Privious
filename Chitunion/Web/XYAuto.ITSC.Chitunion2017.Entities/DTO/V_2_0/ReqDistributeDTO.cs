using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V_2_0
{
    public class ReqDistributeDto
    {

        public string Name { get; set; }
        public string Link { get; set; }
        public string Synopsis { get; set; }
        public int Platform { get; set; }
        public string ImgUrl { get; set; }
        public int BillingMode { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public decimal BudgetPrice { get; set; }

    }
}
