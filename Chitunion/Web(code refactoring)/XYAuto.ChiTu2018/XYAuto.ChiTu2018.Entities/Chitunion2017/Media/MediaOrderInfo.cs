namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MediaOrderInfo")]
    public partial class MediaOrderInfo
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(20)]
        public string OrderID { get; set; }

        public int? MediaType { get; set; }

        [StringLength(1000)]
        public string Note { get; set; }

        [StringLength(200)]
        public string UploadFileURL { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
