namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MaterielChannel")]
    public partial class MaterielChannel
    {
        [Key]
        public int ChannelID { get; set; }

        public int? MaterielID { get; set; }

        [StringLength(50)]
        public string MediaTypeName { get; set; }

        public int? ChannelType { get; set; }

        [StringLength(100)]
        public string MediaNumber { get; set; }

        [StringLength(100)]
        public string MediaName { get; set; }

        public int? PayType { get; set; }

        public int? PayMode { get; set; }

        public decimal? UnitCost { get; set; }

        [StringLength(200)]
        public string PromotionUrl { get; set; }

        [StringLength(50)]
        public string PromotionUrlCode { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
