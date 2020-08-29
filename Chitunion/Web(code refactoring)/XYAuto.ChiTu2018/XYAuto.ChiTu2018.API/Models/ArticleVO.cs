using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYAuto.ChiTu2018.API.Models
{
    public class ArticleVO
    {
        /// <summary>
        /// 文章id
        /// </summary>
        public int RecID { get; set; }
        /// <summary>
        /// AccountID
        /// </summary>
        public int? AccountID { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public int? ArticleID { get; set; }

        public int? Operate { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public string CleanContent { get; set; }

        public DateTime? ReceiveCleanTime { get; set; }

        public int? ReceiveCleanMan { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }
    }
}