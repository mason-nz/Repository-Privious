using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_WithdrawalsDetail
    {
        [Key]
        public int RecID { get; set; }

        public decimal? WithdrawalsPrice { get; set; }

        public decimal? IndividualTaxPeice { get; set; }

        public decimal? PracticalPrice { get; set; }

        [StringLength(50)]
        public string PayeeAccount { get; set; }

        public int? Status { get; set; }

        public DateTime? ApplicationDate { get; set; }

        public DateTime? PayDate { get; set; }

        public int? OrderID { get; set; }

        public int? PayeeID { get; set; }

        public string Reason { get; set; }

        public DateTime? CreateTime { get; set; }

        public string SyncResult { get; set; }

        public string AsynResult { get; set; }

        public int? AuditStatus { get; set; }

        public int ApplySource { get; set; }

        public int IsActive { get; set; }

        public int? SyncPayStatus { get; set; }
        public bool IsLock { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<LE_DisbursementPay> LeDisbursementPays { get; set; }
    }
}
