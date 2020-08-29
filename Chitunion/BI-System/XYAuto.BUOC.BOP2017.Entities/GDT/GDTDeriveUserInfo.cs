using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.Entities.GDT
{
    public class GDTDeriveUserInfo
    {
        public int DeriveUserInfoId { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Brand { get; set; }
        public int Province { get; set; }
        public int City { get; set; }
        public int Dealer { get; set; }
        public int CarType { get; set; }
        public int CarModel { get; set; }
        public string OnLikeType { get; set; }
        public string DeviceNumber { get; set; }
        public string Time { get; set; }
        public string VisitIP { get; set; }
        public string SourceUrl { get; set; }
        public DateTime CreateTime { get; set; }

        public int Status { get; set; }
    }
}
