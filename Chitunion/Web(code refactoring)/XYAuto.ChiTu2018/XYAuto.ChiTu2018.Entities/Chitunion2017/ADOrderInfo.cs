namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADOrderInfo")]
    public partial class ADOrderInfo
    {
        [Key]
        public int RecID { get; set; }

        [Required]
        [StringLength(20)]
        public string OrderID { get; set; }

        [StringLength(50)]
        public string OrderName { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(1000)]
        public string Note { get; set; }

        [StringLength(200)]
        public string UploadFileURL { get; set; }

        public int? MediaType { get; set; }

        public decimal? TotalAmount { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public int? CustomerID { get; set; }

        [StringLength(20)]
        public string RoleID { get; set; }

        [StringLength(20)]
        public string CrmNum { get; set; }

        [StringLength(200)]
        public string CustomerText { get; set; }

        [StringLength(200)]
        public string FinalReportURL { get; set; }

        public int? OrderType { get; set; }

        [StringLength(100)]
        public string CRMCustomerID { get; set; }

        public decimal? CostTotal { get; set; }
    }
}
