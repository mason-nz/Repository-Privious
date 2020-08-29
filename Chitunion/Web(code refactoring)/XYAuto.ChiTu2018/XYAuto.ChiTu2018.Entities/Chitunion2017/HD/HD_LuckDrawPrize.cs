using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.HD
{
    public partial class HD_LuckDrawPrize
    {
        [Key]
        public int PrizeId { get; set; }

        public int ActivityId { get; set; }

        [Required]
        [StringLength(100)]
        public string AwardName { get; set; }

        public decimal DrawRatio { get; set; }

        public decimal DrawMinPrice { get; set; }

        public decimal DrawMaxPrice { get; set; }

        public decimal StartSection { get; set; }

        public decimal EndSection { get; set; }

        public int WinningMaxNum { get; set; }

        public decimal WinningMaxPrice { get; set; }

        public DateTime CreateTime { get; set; }

        public int Status { get; set; }
    }
}
