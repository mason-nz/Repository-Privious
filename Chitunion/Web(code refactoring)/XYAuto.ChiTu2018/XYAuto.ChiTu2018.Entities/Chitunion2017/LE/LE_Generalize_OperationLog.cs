using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_Generalize_OperationLog
    {
        [Key]
        public int RecID { get; set; }

        public int? BeforeStatus { get; set; }

        [StringLength(50)]
        public string BeforeStatusName { get; set; }

        public int? CurrentStatus { get; set; }

        [StringLength(50)]
        public string CurrentStatusName { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? OperationTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? PromotionType { get; set; }

        public int? PromotionID { get; set; }
    }
}
