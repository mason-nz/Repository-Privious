namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Article_Weixin
    {
        [Key]
        public int RecID { get; set; }

        public int? WxID { get; set; }

        [StringLength(20)]
        public string AppID { get; set; }

        [Required]
        [StringLength(20)]
        public string MsgID { get; set; }

        public DateTime? PubDate { get; set; }

        public int? ArticleType { get; set; }

        public int? IntReadUserCount { get; set; }

        public int? IntReadCount { get; set; }

        public int? OriReadUserCount { get; set; }

        public int? OriReadCount { get; set; }

        public int? ShareUserCount { get; set; }

        public int? ShareCount { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
