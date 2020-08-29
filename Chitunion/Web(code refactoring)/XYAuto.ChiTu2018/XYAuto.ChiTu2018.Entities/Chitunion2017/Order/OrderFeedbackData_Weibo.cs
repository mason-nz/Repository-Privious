namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderFeedbackData_Weibo
    {
        [Key]
        public int RecID { get; set; }

        [Required]
        [StringLength(20)]
        public string SubOrderCode { get; set; }

        public int? ADDetailID { get; set; }

        public int? ReadCount { get; set; }

        public int? GoodPointCount { get; set; }

        public int? CommentCount { get; set; }

        public int? ForwardCount { get; set; }

        public int? ClickCount { get; set; }

        public int? PV { get; set; }

        public int? UV { get; set; }

        public int? OrderCount { get; set; }

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
