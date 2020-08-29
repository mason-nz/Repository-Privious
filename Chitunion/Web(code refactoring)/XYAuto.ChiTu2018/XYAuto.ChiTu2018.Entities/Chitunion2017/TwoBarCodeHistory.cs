namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TwoBarCodeHistory")]
    public partial class TwoBarCodeHistory
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(20)]
        public string OrderID { get; set; }

        public int? MediaType { get; set; }

        public int? MediaID { get; set; }

        [StringLength(200)]
        public string URL { get; set; }

        [StringLength(200)]
        public string TwoBarUrl { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
