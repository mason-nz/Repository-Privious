using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_User_Blacklist
    {
        [Key]
        public int RecID { get; set; }

        public int? UserID { get; set; }

        public int? ConstraintID { get; set; }

        public int? Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreateTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UpdateTime { get; set; }
    }
}
