using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.User
{
    [Table("UserActionLog")]
    public partial class UserActionLog
    {
        [Key]
        public long RecID { get; set; }

        public int userid { get; set; }

        [StringLength(1000)]
        public string loginfo { get; set; }

        public DateTime? createtime { get; set; }

        [StringLength(20)]
        public string ip { get; set; }
    }
}
