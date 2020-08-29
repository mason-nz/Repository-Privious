namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Publish_BasicInfo
    {
        [Key]
        public int PubID { get; set; }

        public int? MediaType { get; set; }

        public int MediaID { get; set; }

        [StringLength(20)]
        public string PubCode { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public decimal? PurchaseDiscount { get; set; }

        public decimal? SaleDiscount { get; set; }

        public int? Status { get; set; }

        public int? PublishStatus { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }

        public bool? IsAppointment { get; set; }

        public int? Wx_Status { get; set; }

        [StringLength(50)]
        public string PubName { get; set; }

        public int IsDel { get; set; }

        public int? TemplateID { get; set; }

        public bool? HasHoliday { get; set; }

        public decimal? OriginalReferencePrice { get; set; }
    }
}
