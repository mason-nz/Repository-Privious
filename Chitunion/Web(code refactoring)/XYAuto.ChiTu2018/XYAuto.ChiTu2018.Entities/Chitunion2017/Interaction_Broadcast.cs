namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Interaction_Broadcast
    {
        [Key]
        public int RecID { get; set; }

        public int? MeidaType { get; set; }

        public int? MediaID { get; set; }

        public int? AudienceCount { get; set; }

        public int? MaximumAudience { get; set; }

        public int? AverageAudience { get; set; }

        public int? CumulateReward { get; set; }

        public int? CumulateIncome { get; set; }

        public int? CumulatePoints { get; set; }

        public int? CumulateSendCount { get; set; }

        [StringLength(200)]
        public string ScreenShotURL { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }
    }
}
