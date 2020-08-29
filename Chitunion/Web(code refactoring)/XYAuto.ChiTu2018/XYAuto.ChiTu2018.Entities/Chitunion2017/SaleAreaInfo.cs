namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SaleAreaInfo")]
    public partial class SaleAreaInfo
    {
        [Key]
        public int GroupID { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        public int? TemplateID { get; set; }

        public bool IsPublic { get; set; }

        public int? GroupType { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
