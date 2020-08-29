namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WxMaterial_ArticleGroup
    {
        [Key]
        public int GroupID { get; set; }

        [StringLength(100)]
        public string WxMaterialID { get; set; }

        public int? ArticleCount { get; set; }

        public int? SourceType { get; set; }

        [StringLength(500)]
        public string FromUrl { get; set; }

        public int? FromWxID { get; set; }

        [StringLength(200)]
        public string FromWxNumber { get; set; }

        [StringLength(200)]
        public string FromWxName { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? Status { get; set; }
    }
}
