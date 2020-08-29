using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_SmartSearchMapping
    {
        [Key]
        public int RecID { get; set; }

        public int? PromoteID { get; set; }

        public int? MediaID { get; set; }

        public int? MediaTypeID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
