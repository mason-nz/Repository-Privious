namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tmptb")]
    public partial class tmptb
    {
        [Key]
        public int f1 { get; set; }

        [StringLength(50)]
        public string f2 { get; set; }
    }
}
