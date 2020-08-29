using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_DisbursementPay
    {
        [Key]
        public int RecID { get; set; }

        [ForeignKey("LeWithdrawalsDetail")]
        public int? WithdrawalsId { get; set; }

        [StringLength(256)]
        public string DisbursementNo { get; set; }

        [StringLength(256)]
        public string BizDisbursementNo { get; set; }

        [StringLength(256)]
        public string BizNo { get; set; }

        [StringLength(256)]
        public string ContractNo { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Status { get; set; }

        public virtual LE_WithdrawalsDetail LeWithdrawalsDetail { get; set; }
    }
}
