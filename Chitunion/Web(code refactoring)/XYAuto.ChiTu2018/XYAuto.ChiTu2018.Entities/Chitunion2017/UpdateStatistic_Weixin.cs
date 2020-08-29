namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UpdateStatistic_Weixin
    {
        [Key]
        public int RecID { get; set; }

        public int? WxID { get; set; }

        public DateTime? StatisticDate { get; set; }

        public int? UpdateCount { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
