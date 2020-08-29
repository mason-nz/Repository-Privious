namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WxMaterial_Article
    {
        [Key]
        public int ArticleID { get; set; }

        public int? GroupID { get; set; }

        public int? Orderby { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string CoverPicUrl { get; set; }

        [StringLength(100)]
        public string Author { get; set; }

        [StringLength(500)]
        public string Abstract { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }

        [StringLength(500)]
        public string OriginalUrl { get; set; }

        public int? Status { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        [StringLength(500)]
        public string PCViewUrl { get; set; }

        [StringLength(500)]
        public string MobileViewUrl { get; set; }
    }
}
