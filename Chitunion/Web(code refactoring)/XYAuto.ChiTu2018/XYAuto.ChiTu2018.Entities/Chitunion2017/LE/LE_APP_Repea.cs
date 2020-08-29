using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_APP_Repea
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string HeadIconURL { get; set; }

        public int? ProvinceID { get; set; }

        public int? CityID { get; set; }

        public int? DailyLive { get; set; }

        public string Remark { get; set; }

        public int Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? SourceID { get; set; }

        public int? CategoryID { get; set; }

        public bool? IsMonitor { get; set; }

        public bool? IsLocate { get; set; }

        public int? TotalUser { get; set; }

        public int? SmartSearchID { get; set; }

        public int? CreateUserID { get; set; }

        public string TagText { get; set; }
    }
}
