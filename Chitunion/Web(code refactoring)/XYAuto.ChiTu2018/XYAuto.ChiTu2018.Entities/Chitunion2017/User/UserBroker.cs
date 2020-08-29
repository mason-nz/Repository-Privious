using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.User
{
    [Table("UserBroker")]
    public partial class UserBroker
    {
        [Key]
        public int RecID { get; set; }

        public int? BrokerID { get; set; }

        public int? UserID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
