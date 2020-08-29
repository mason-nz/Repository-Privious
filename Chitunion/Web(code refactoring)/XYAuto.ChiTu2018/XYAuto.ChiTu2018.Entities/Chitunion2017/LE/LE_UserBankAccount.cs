using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_UserBankAccount
    {
        [Key]
        public int RecID { get; set; }

        public int? UserID { get; set; }

        [StringLength(50)]
        public string AccountName { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Status { get; set; }

        public int? AccountType { get; set; }
    }
}
