using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    public class MediaFansArea
    {
        public int RecID { get; set; }
        public int MediaID { get; set; }
        public int WxID { get; set; }
        public int MediaType { get; set; }
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public decimal UserScale { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }
    }
}