using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_Weixin
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(20)]
        public string AppID { get; set; }

        [StringLength(200)]
        public string AccessToken { get; set; }

        [StringLength(200)]
        public string RefreshAccessToken { get; set; }

        public DateTime? GetTokenTime { get; set; }

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

        [StringLength(200)]
        public string QrCodeUrl { get; set; }

        public int? FansCount { get; set; }

        [StringLength(16)]
        public string Biz { get; set; }

        public int? Status { get; set; }

        public int? OAuthStatus { get; set; }

        public DateTime? RegTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        public int? SourceType { get; set; }

        public string Summary { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(50)]
        public string CreditCode { get; set; }

        [StringLength(50)]
        public string BusinessScope { get; set; }

        [StringLength(100)]
        public string EnterpriseType { get; set; }

        [StringLength(20)]
        public string EnterpriseCreateDate { get; set; }

        [StringLength(50)]
        public string EnterpriseBusinessTerm { get; set; }

        [StringLength(20)]
        public string EnterpriseVerifyDate { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        public int? LevelType { get; set; }

        public int? ProvinceID { get; set; }

        public int? CityID { get; set; }

        [StringLength(300)]
        public string Sign { get; set; }

        public bool IsAreaMedia { get; set; }

        public int? ReadNum { get; set; }

        public int? CategoryID { get; set; }

        public bool? IsOriginal { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] TimestampSign { get; set; }

        public int? CreateUserID { get; set; }

        public decimal? ManFansRatio { get; set; }

        public decimal? WomanFansRatio { get; set; }

        public int? TotalScores { get; set; }

        public string TagText { get; set; }
    }
}
