namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Car_Serial
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string MasterBrandID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string BrandID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string SerialID { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string SEOName { get; set; }

        [StringLength(100)]
        public string AllSpell { get; set; }

        [StringLength(100)]
        public string Spell { get; set; }

        [StringLength(20)]
        public string CsSaleState { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime CreateTime { get; set; }

        [StringLength(50)]
        public string CsLevel { get; set; }
    }
}
