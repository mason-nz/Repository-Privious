using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.Entities
{
    public class ArticleInfo
    {
        public int XyAttr { get; set; } = -1;
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string HeadImg { get; set; } = string.Empty;
        public int ReadNum { get; set; } = 0;
        public int LikeNum { get; set; } = 0;
        public int ComNum { get; set; } = 0;
        public string Content { get; set; } = string.Empty;
        public string JsonContent { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
        public int CopyrightState { get; set; } = 2;
        public string CarSerial { get; set; } = string.Empty;
        public int Resource { get; set; } = -1;
        public string Category { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string DataId { get; set; } = string.Empty;
        public string DataName { get; set; } = string.Empty;
        public string RowKey { get; set; } = string.Empty;
        public DateTime PublishTime { get; set; } = new DateTime(1990, 1, 1);
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; } = new DateTime(1990, 1, 1);
        public int IsIndex { get; set; } = 0;
        public decimal? Score { get; set; } = null;
        public string CategoryNew { get; set; } = string.Empty;
        public string HeadImgNew { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int CategoryID { get; set; } = -2;
        public string HeadImgNew2 { get; set; } = string.Empty;
        public string HeadImgNew3 { get; set; } = string.Empty;
    }
}
