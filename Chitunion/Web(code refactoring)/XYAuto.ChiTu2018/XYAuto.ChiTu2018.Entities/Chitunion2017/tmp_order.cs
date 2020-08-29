namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tmp_order
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(50)]
        public string TaskId { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        [StringLength(50)]
        public string ChannelId { get; set; }

        [StringLength(4000)]
        public string OrderUrl { get; set; }

        [StringLength(50)]
        public string IP { get; set; }

        [StringLength(50)]
        public string PromotionChannelID { get; set; }

        [StringLength(50)]
        public string utm_term { get; set; }
    }
}
