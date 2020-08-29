using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_Area_Promotion
    {
        [Key]
        public int RecID { get; set; }

        public int? ProvinceID { get; set; }

        public int? CityID { get; set; }

        public int? MediaID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
