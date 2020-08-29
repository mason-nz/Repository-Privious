using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_ChannelStatMonthRelation
    {
        [Key]
        public int RecId { get; set; }

        public int StatisticsId { get; set; }

        public int PayStatus { get; set; }

        public DateTime? PayTime { get; set; }

        public int CreateUserId { get; set; }

        public DateTime? CreateTime { get; set; }

        public int Status { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }
    }
}
