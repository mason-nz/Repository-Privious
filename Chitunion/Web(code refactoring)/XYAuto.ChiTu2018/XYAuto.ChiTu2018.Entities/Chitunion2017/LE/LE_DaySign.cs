using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_DaySign
    {
        [Key]
        public int RecID { get; set; }

        public DateTime? SignTime { get; set; }

        public int? SignUserID { get; set; }

        public int? SignNumber { get; set; }

        public decimal? SignPrice { get; set; }

        [StringLength(50)]
        public string IP { get; set; }
    }
}
