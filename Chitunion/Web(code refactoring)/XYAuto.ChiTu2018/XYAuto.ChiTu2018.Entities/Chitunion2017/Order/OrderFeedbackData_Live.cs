namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderFeedbackData_Live
    {
        [Key]
        public int RecID { get; set; }

        [Required]
        [StringLength(20)]
        public string SubOrderCode { get; set; }

        public int? ADDetailID { get; set; }

        public int? TotalViewCount { get; set; }

        public int? OnlineCount { get; set; }

        public decimal? VirtualValue { get; set; }

        public int? MentionCount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FeedbackBeginDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FeedbackEndDate { get; set; }

        [StringLength(200)]
        public string UploadFileURL { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }
    }
}
