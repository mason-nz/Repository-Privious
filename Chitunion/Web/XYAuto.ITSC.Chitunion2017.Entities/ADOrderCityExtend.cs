using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    public class ADOrderCityExtend
    {
        public string OrderID { get; set; } = string.Empty;
        public int ProvinceID { get; set; } = -2;
        public int CityID { get; set; } = -2;
        public decimal Budget { get; set; } = 0;
        public int MediaCount { get; set; } = 0;
        public bool OriginContain { get; set; } = false;
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; } = -2;
        public int CityExtendID { get; set; } = -2;
    }
}
