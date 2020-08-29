namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Interaction_Weixin
    {
        [Key]
        public int RecID { get; set; }

        public int? MeidaType { get; set; }

        public int? MediaID { get; set; }

        public int WxID { get; set; }

        public int? ReferReadCount { get; set; }

        public int? AveragePointCount { get; set; }

        public int? MoreReadCount { get; set; }

        public int? OrigArticleCount { get; set; }

        public int? UpdateCount { get; set; }

        public int? MaxinumReading { get; set; }

        [StringLength(200)]
        public string ScreenShotURL { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }
    }
}
