using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_chitu_blackip_stat
    {
        [Key]
        [Column(Order = 0, TypeName = "date")]
        public DateTime dt { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(64)]
        public string ip { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(64)]
        public string channel { get; set; }

        [StringLength(64)]
        public string filter_type { get; set; }

        public DateTime? modify_time { get; set; }
    }
}
