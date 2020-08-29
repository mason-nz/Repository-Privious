using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.HD
{
    public partial class HD_LuckDrawRecord_Moni
    {
        [Key]
        public int RecId { get; set; }

        public int ActivityId { get; set; }

        public int UserId { get; set; }

        public decimal DrawPrice { get; set; }

        [Required]
        [StringLength(100)]
        public string DrawDescribe { get; set; }

        public DateTime DrawTime { get; set; }

        public DateTime CreateTime { get; set; }

        public int Status { get; set; }

        [StringLength(200)]
        public string NickName { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }
    }
}
