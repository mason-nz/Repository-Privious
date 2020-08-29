using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetPublishListBResDTO
    {
        public List<ADPublishItemB> List { get; set; }
        public int Total { get; set; }
    }

    public class ADPublishItemB
    {
        public int TemplateID { get; set; }
        public string ADName { get; set; }
        public string MediaName { get; set; }
        public int ADStatus { get; set; }
        public string ADStatusName { get; set; }
        public int PubID { get; set; }
        public int PriceCount { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PubStatus { get; set; }
        public string PubStatusName { get; set; }
        public string CreateUserName { get; set; }
        public string CreateUserRole { get; set; }
        public bool CanAddToRecommend { get; set; }
        public bool IsRange { get; set; }
    }
}
