namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Notice")]
    public partial class Notice
    {
        public int id { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? status { get; set; }

        public int? CreateUser { get; set; }

        public int? LastUpdateUser { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }
    }
}
