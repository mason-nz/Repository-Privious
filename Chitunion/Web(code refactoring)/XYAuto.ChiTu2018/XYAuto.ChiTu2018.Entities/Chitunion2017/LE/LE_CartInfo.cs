using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_CartInfo
    {
        [Key]
        public int RecID { get; set; }

        public int? MediaType { get; set; }

        public int? MediaID { get; set; }

        public int? UserID { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
