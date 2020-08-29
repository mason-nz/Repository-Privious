namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class App_AdTemplate
    {
        [Key]
        public int RecID { get; set; }

        public int BaseMediaID { get; set; }

        public int BaseAdID { get; set; }

        [Required]
        [StringLength(200)]
        public string AdTemplateName { get; set; }

        [StringLength(200)]
        public string OriginalFile { get; set; }

        public int AdForm { get; set; }

        public int CarouselCount { get; set; }

        public int SellingPlatform { get; set; }

        public int SellingMode { get; set; }

        [StringLength(2000)]
        public string AdLegendURL { get; set; }

        [StringLength(2000)]
        public string AdDisplay { get; set; }

        [Column(TypeName = "text")]
        public string AdDescription { get; set; }

        [Column(TypeName = "text")]
        public string Remarks { get; set; }

        public int AdDisplayLength { get; set; }

        public int AuditStatus { get; set; }

        public int CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int Status { get; set; }
    }
}
