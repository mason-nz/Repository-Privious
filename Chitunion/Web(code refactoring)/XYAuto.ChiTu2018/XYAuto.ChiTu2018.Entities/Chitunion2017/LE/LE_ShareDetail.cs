using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_ShareDetail
    {
        [Key]
        public int RecID { get; set; }

        public int? Type { get; set; }

        [StringLength(200)]
        public string ShareURL { get; set; }

        [StringLength(50)]
        public string OrderCoding { get; set; }

        public int? CategoryID { get; set; }

        public short? ShareResult { get; set; }

        [StringLength(50)]
        public string IP { get; set; }

        public int? Status { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
