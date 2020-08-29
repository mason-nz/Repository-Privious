namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Fans_Weixin
    {
        [Key]
        public int RecID { get; set; }

        public int? WxID { get; set; }

        public int? StatisticType { get; set; }

        public int? StatisticKey { get; set; }

        [StringLength(20)]
        public string StatisticText { get; set; }

        public decimal StatisticValue { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
