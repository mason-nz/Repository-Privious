using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_Feedback
    {
        [Key]
        public int RecID { get; set; }

        public int? UserID { get; set; }

        [StringLength(1000)]
        public string OpinionText { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(1000)]
        public string ReplyText { get; set; }

        public DateTime? ReplyTime { get; set; }
    }
}
