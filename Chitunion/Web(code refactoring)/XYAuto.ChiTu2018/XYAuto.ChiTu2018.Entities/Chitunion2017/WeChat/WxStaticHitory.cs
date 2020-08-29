namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WxStaticHitory")]
    public partial class WxStaticHitory
    {
        [Key]
        public int RecID { get; set; }

        public int? WxGRID { get; set; }

        public int? GroupID { get; set; }

        public int ArticleID { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string CoverPicUrl { get; set; }

        [StringLength(500)]
        public string Abstract { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Orderby { get; set; }

        [StringLength(500)]
        public string StaticUrl { get; set; }

        public int? CreateUserID { get; set; }
    }
}
