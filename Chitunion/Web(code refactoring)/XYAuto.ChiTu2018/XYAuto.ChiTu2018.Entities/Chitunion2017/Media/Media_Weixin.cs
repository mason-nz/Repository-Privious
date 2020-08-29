namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Media_Weixin
    {
        [Key]
        public int MediaID { get; set; }

        [StringLength(100)]
        public string Number { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string HeadIconURL { get; set; }

        [StringLength(200)]
        public string TwoCodeURL { get; set; }

        public int? FansCount { get; set; }

        [StringLength(200)]
        public string FansCountURL { get; set; }

        public decimal? FansMalePer { get; set; }

        public decimal? FansFemalePer { get; set; }

        public int? CategoryID { get; set; }

        public int? ProvinceID { get; set; }

        public int? CityID { get; set; }

        [StringLength(300)]
        public string Sign { get; set; }

        public int? AreaID { get; set; }

        public int? LevelType { get; set; }

        public bool? IsAuth { get; set; }

        [StringLength(300)]
        public string OrderRemark { get; set; }

        public bool? IsReserve { get; set; }

        public bool IsAreaMedia { get; set; }

        public int Source { get; set; }

        public int? Status { get; set; }

        public int PublishStatus { get; set; }

        public int AuditStatus { get; set; }

        [StringLength(250)]
        public string FansSexScaleUrl { get; set; }

        [StringLength(250)]
        public string FansAreaShotUrl { get; set; }

        public int AuthType { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }

        public int? WxID { get; set; }

        [StringLength(100)]
        public string ADName { get; set; }
    }
}
