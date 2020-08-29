using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_InviteRecord
    {
        [Key]
        public int RecID { get; set; }

        public int? InviteUserID { get; set; }

        public int? BeInvitedUserID { get; set; }

        public DateTime? InviteTime { get; set; }

        public decimal? RedEvesPrice { get; set; }

        public DateTime? ReceiveTime { get; set; }

        public int? RedEvesStatus { get; set; }

        [StringLength(50)]
        public string IP { get; set; }
    }
}
