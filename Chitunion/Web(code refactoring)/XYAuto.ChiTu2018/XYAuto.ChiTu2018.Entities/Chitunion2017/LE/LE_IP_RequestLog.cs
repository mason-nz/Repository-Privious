namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LE_IP_RequestLog
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(50)]
        public string IP { get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        [StringLength(50)]
        public string ProvinceName { get; set; }

        [StringLength(50)]
        public string CityName { get; set; }

        public int? ProvinceID { get; set; }

        public int? CityID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
