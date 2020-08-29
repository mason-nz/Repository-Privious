namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MaterielExtendFoot")]
    public partial class MaterielExtendFoot
    {
        [Key]
        public int RecID { get; set; }

        public int? MaterielID { get; set; }

        public int? FootContentType { get; set; }

        [StringLength(200)]
        public string FootContentUrl { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
