using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.WxEditor
{
    public class ArticleInfo
    {
        public int ArticleID { get; set; }
        public int GroupID { get; set; }
        public int Orderby { get; set; }
        public string Title { get; set; }
        public string CoverPicUrl { get; set; }
        public string Author { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }
        public string OriginalUrl { get; set; }
        public int Status { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string PCViewUrl { get; set; }
        public string MobileViewUrl { get; set; }

        /// <summary>
        /// 封面上传为永久图片素材后 回写的
        /// </summary>
        public string ConverPicMediaID { get; set; }
    }
}
