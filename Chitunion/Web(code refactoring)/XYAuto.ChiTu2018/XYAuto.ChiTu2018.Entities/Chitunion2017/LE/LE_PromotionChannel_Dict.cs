using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_PromotionChannel_Dict
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long DictID { get; set; }

        public int? ParentID { get; set; }

        public int? Level { get; set; }

        [StringLength(50)]
        public string ChannelName { get; set; }

        [StringLength(50)]
        public string ChannelCode { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
