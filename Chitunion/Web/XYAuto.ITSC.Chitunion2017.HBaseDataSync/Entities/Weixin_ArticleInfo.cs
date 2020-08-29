using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.Entities
{
    public class Weixin_ArticleInfo
    {
        public string WxNum { get; set; } = string.Empty;
        public string Biz { get; set; } = string.Empty;
        public string SN { get; set; } = string.Empty;
        public string ContentURL { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Digest { get; set; } = string.Empty; 
        public int CopyrightState { get; set; } = 0;
        public string SourceURL { get; set; } = string.Empty;
        public string CoverURL { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PubTime { get; set; } = new DateTime(1990, 1, 1);
        public bool IsMulti { get; set; } = false;
        public int Location { get; set; } = 0;
        public string FartherSN { get; set; } = string.Empty;
        public DateTime PushTime { get; set; } = new DateTime(1990, 1, 1);
        public DateTime CreateTime { get; set; }
        public string Rowkey { get; set; } = string.Empty;
        //public string ContentM { get; set; } = string.Empty;
    }
}
