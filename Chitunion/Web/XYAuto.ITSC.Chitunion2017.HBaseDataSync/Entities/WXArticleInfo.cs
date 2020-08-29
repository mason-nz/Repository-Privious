using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.Entities
{
    public class WXArticleInfo
    {
        public string WxNum { get; set; } = string.Empty;
        public string SN { get; set; } = string.Empty;
        public DateTime PubTime { get; set; } = new DateTime(1990, 1, 1);
        public bool IsMulti { get; set; } = false;
        public int Location { get; set; } = 0;
        public string FartherSN { get; set; } = string.Empty;
        public DateTime PushTime { get; set; } = new DateTime(1990, 1, 1);
        public DateTime CreateTime { get; set; }
    }
}
