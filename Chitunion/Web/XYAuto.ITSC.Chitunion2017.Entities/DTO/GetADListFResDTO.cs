using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetADListFResDTO
    {
        public List<ADItemF> List { get; set; }
        public int Total { get; set; }
    }

    public class ADItemF
    {
        public int MediaID { get; set; }
        public int TemplateID { get; set; }
        //public int PubID { get; set; }
        public string ADName { get; set; }
        public string MediaName { get; set; }
        public string HeadIconURL { get; set; }
        public string CategoryNames { get; set; }
        public string OwnerName { get; set; }
        public decimal Price { get; set; }
        public string ADFormName { get; set; }
        public bool IsAE { get; set; }
    }
}
