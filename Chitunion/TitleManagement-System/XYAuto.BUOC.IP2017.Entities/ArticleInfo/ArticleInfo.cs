using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.ArticleInfo
{
    public class ArticleInfo
    {
        public long ArticleID { get; set; } = -2;
        public string Title { get; set; } = string.Empty;
        public string HeadImg { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Resource { get; set; } = -2;
        public string Tag { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
    }
}
