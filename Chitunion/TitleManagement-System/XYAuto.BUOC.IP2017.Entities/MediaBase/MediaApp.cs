using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.MediaBase
{
    public class MediaApp
    {
        public string Name { get; set; }

        public string HeadIconURL { get; set; }

        public int ProvinceID { get; set; } = -2;
        public int CityID { get; set; } = -2;
        public decimal DailyLive { get; set; } = 0;

        public string Remark { get; set; }

        public int Status { get; set; } = 0;

    }
}
