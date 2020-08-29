using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.HD
{
    public partial class HD_LuckDrawActivity
    {
        [Key]
        public int ActivityId { get; set; }

        public decimal BonusBase { get; set; }

        public int DrawNum { get; set; }

        public decimal Price { get; set; }

        public decimal MaxBonusBase { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime CreateTime { get; set; }

        public int Status { get; set; }
    }
}
