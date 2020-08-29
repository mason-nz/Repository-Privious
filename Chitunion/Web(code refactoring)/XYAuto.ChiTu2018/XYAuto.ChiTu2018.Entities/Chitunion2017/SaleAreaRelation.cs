namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SaleAreaRelation")]
    public partial class SaleAreaRelation
    {
        [Key]
        public int RecID { get; set; }

        public int? GroupID { get; set; }

        public int? ProvinceID { get; set; }

        public int? CityID { get; set; }

        public bool IsPublic { get; set; }

        public int TemplateID { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
