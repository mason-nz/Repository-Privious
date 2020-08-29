using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_Car_Promotion
    {
        [Key]
        public int RecID { get; set; }

        public int? MakeID { get; set; }

        public int? ModelID { get; set; }

        public int? MediaID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
