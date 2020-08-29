namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADOrderOperateInfo")]
    public partial class ADOrderOperateInfo
    {
        [Key]
        public int RecID { get; set; }

        [Required]
        [StringLength(20)]
        public string OrderID { get; set; }

        [StringLength(20)]
        public string SubOrderID { get; set; }

        public int OptType { get; set; }

        public int? OrderStatus { get; set; }

        [StringLength(200)]
        public string RejectMsg { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public int? CurrentStatus { get; set; }
    }
}
