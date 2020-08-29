using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetMediaListFResDTO
    {
        public List<MediaItemFDTO> List { get; set; }
        public int Total { get; set; }
    }

    public class MediaItemFDTO
    {
        public int MediaID { get; set; }
        public int PubID { get; set; }
        public int ADDetailID { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string HeadImg { get; set; }
        public bool IsVerify { get; set; }
        public string OwnerName { get; set; }
        public string CategoryNames { get; set; }
        public int FansCount { get; set; }
        public int AverageReading { get; set; }
        public int MaxReading { get; set; }
        public string Price { get; set; }
        public decimal MaLiIndex { get; set; }
        public string RoleId { get; set; }
    }
}