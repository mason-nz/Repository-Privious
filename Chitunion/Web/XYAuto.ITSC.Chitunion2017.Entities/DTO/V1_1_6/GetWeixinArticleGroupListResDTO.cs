using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6
{
    public class GetWeixinArticleGroupListResDTO
    {
        public List<ArticleGroupItem> List { get; set; }
        public int TotalCount { get; set; }
    }

    public class ArticleGroupItem {
        public int GroupID { get; set; }
        public string FromWxName { get; set; }
        public string UpdateDate { get; set; }
        public List<ArticleItem> ArticleList { get; set; }
        //由LastUpdateTime得出UpdateDate 由CombinStr得出ArticleList
        public DateTime LastUpdateTime { get; set; }
        public string CombinStr { get; set; }
        public int SyncCount { get; set; }
    }

    public class ArticleItem {

        public int ArticleID { get; set; }
        public int Orderby { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string CoverPicUrl { get; set; }
        //以下为新增编辑用到的
        public string Content { get; set; }
        public string Author { get; set; }
        public string OriginalUrl { get; set; }
        public string PCViewUrl { get; set; }
        public string MobileUrl { get; set; }
        //以下为图文列表用到的
        public int GroupID { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
