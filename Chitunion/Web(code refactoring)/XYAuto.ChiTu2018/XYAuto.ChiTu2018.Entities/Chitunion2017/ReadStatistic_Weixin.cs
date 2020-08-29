namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReadStatistic_Weixin
    {
        [Key]
        public int RecID { get; set; }

        public int? WxID { get; set; }

        public int? ArticleType { get; set; }

        public int? AverageReading { get; set; }

        public int? MaxReading { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
