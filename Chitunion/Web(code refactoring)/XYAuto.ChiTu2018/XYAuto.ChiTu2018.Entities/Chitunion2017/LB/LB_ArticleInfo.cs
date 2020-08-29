namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LB_ArticleInfo
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(512)]
        public string Url { get; set; }

        public int? MeidaType { get; set; }

        [StringLength(64)]
        public string DataId { get; set; }

        [StringLength(64)]
        public string DataName { get; set; }

        public int? ArticleID { get; set; }

        [StringLength(100)]
        public string ArticleTitle { get; set; }

        public int? KeyWord { get; set; }

        public int? Summary { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
