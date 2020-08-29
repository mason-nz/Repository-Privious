using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.User
{
    [Table("UserLoginInfo")]
    public partial class UserLoginInfo
    {
        [Key]
        public int Logid { get; set; }

        public int? UserID { get; set; }

        [StringLength(1000)]
        public string LoginInfo { get; set; }

        [StringLength(50)]
        public string IP { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
