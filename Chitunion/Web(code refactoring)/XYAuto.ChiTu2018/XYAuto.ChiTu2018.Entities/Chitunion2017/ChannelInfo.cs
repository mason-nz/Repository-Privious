namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChannelInfo")]
    public partial class ChannelInfo
    {
        [Key]
        public int ChannelID { get; set; }

        [StringLength(200)]
        public string ChannelName { get; set; }

        public bool? IncludingTax { get; set; }

        public DateTime? CooperateBeginDate { get; set; }

        public DateTime? CooperateEndDate { get; set; }

        [StringLength(1000)]
        public string Remark { get; set; }

        public int? Status { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
