namespace XYAuto.ChiTu2018.Entities.Chitunion2017.View
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_RankList
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string WxNum { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal MaLiIndex { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastModifyTime { get; set; }

        public int? AvgTopArticleReadNum { get; set; }

        public int? AvgTopArticleLikeNum { get; set; }

        public int? MaxReadNum { get; set; }

        public int? PublishArticleNum { get; set; }

        public int? PublishCount { get; set; }
    }
}
