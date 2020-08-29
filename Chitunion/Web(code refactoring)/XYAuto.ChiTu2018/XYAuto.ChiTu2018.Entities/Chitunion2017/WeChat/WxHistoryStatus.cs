namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WxHistoryStatus
    {
        [Key]
        public int RecID { get; set; }

        public int? WxGRID { get; set; }

        public int? GroupID { get; set; }

        public int? WxID { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public int? Ranking { get; set; }

        public int? CreateUserID { get; set; }
    }
}
