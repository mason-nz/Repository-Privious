namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CartScheduleInfo")]
    public partial class CartScheduleInfo
    {
        [Key]
        public int RecID { get; set; }

        public int? CartID { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
