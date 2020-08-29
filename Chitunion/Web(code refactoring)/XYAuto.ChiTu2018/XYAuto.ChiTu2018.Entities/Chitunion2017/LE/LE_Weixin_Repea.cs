using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_Weixin_Repea
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(50)]
        public string WxNumber { get; set; }

        [StringLength(20)]
        public string OriginalID { get; set; }

        [StringLength(50)]
        public string NickName { get; set; }

        public int? ServiceType { get; set; }

        public bool? IsVerify { get; set; }

        public int? VerifyType { get; set; }

        [StringLength(200)]
        public string HeadImg { get; set; }

        public int? FansCount { get; set; }

        public string Summary { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(300)]
        public string Sign { get; set; }

        public int? ReadNum { get; set; }

        public int? CategoryID { get; set; }

        public bool? IsOriginal { get; set; }

        public int? SmartSearchID { get; set; }

        public int? SourceID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Status { get; set; }

        public int? CreateUserID { get; set; }

        public int? TotalScores { get; set; }

        [StringLength(200)]
        public string QrCodeUrl { get; set; }

        public string TagText { get; set; }
    }
}
