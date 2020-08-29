namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IPLimitList")]
    public partial class IPLimitList
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string IP { get; set; }

        public int? Flag { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
