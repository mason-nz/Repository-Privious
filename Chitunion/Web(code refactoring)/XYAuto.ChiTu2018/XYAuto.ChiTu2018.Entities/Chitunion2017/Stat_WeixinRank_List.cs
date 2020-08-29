namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Stat_WeixinRank_List
    {
        [Key]
        public int RecID { get; set; }

        [Required]
        [StringLength(50)]
        public string WxNum { get; set; }

        public int? AVG_TopArticleReadNum { get; set; }

        public int? AVG_TopArticleLikeNum { get; set; }

        public int? MaxReadNum { get; set; }

        public int? PublishArticleNum { get; set; }

        public int? PublishCount { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
