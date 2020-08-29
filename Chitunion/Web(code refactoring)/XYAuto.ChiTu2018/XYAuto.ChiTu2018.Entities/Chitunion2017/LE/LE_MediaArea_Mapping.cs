using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_MediaArea_Mapping
    {
        [Key]
        public int RecID { get; set; }

        public int MediaType { get; set; }

        public int MediaID { get; set; }

        public int ProvinceID { get; set; }

        public int? CityID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public int RelateType { get; set; }
    }
}
