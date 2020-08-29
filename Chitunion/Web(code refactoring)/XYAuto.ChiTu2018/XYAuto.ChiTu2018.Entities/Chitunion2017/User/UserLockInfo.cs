using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.User
{
    [Table("UserLockInfo")]
    public partial class UserLockInfo
    {
        public int? UserID { get; set; }

        public int? Count { get; set; }

        public DateTime? LastTime { get; set; }

        [Key]
        public int RecID { get; set; }
    }
}
