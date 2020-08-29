namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Weixin_OAuth
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

        [StringLength(500)]
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

        [StringLength(2500)]
        public string Sign { get; set; }

        public bool IsAreaMedia { get; set; }
    }
}
