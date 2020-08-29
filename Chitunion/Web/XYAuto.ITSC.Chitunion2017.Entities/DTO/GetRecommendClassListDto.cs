using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetRecommendClassListDto
    {
        public int MediaId { get; set; }
        public int WxId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string HeadIconURL { get; set; }
        public int FansCount { get; set; }
        public int PublishStatus { get; set; }
        public decimal Price { get; set; }
    }
}