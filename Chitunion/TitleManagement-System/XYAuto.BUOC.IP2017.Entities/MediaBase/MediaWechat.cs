using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.MediaBase
{
    public class MediaWechat
    {
        public string WxNumber { get; set; }
        public string NickName { get; set; }
        public int ServiceType { get; set; } = -2;
        public int ProvinceID { get; set; } = -2;
        public int CityID { get; set; } = -2;
        public int FansCount { get; set; } = 0;
        public int IsAreaMedia { get; set; } = 0;
        public int IsVerify { get; set; } = 0;
        public int LevelType { get; set; } = -2;

        public string Sign { get; set; }
    }
}
