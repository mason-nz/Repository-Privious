namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WxMaterial_ArticleTemplate
    {
        [Key]
        public int TemplateID { get; set; }

        [StringLength(200)]
        public string TemplateName { get; set; }

        [StringLength(500)]
        public string CoverPicUrl { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }

        public int? Status { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
