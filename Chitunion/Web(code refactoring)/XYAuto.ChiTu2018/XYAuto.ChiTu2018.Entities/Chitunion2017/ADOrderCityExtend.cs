namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADOrderCityExtend")]
    public partial class ADOrderCityExtend
    {
        [Key]
        public int RecID { get; set; }

        [Required]
        [StringLength(20)]
        public string OrderID { get; set; }

        public int? ProvinceID { get; set; }

        public int? CityID { get; set; }

        public decimal? Budget { get; set; }

        public int? MediaCount { get; set; }

        public bool? OriginContain { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreaetUserID { get; set; }
    }
}
