namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DictInfo")]
    public partial class DictInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DictId { get; set; }

        public int? DictType { get; set; }

        [StringLength(100)]
        public string DictName { get; set; }

        public int? Status { get; set; }

        public int? OrderNum { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
