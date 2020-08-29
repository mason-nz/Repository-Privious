namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderFeedbackData_Video
    {
        [Key]
        public int RecID { get; set; }

        [Required]
        [StringLength(20)]
        public string SubOrderCode { get; set; }

        public int? ADDetailID { get; set; }

        public int? ExposeCount { get; set; }

        public int? ViewCount { get; set; }

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
