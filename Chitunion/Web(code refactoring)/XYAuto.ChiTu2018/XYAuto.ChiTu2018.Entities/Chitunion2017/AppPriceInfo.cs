namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AppPriceInfo")]
    public partial class AppPriceInfo
    {
        [Key]
        public int RecID { get; set; }

        public int? PubID { get; set; }

        public int? TemplateID { get; set; }

        public int? MediaID { get; set; }

        public int? ADStyle { get; set; }

        public int? CarouselNumber { get; set; }

        public int? SalePlatform { get; set; }

        public int? SaleType { get; set; }

        public int? SaleArea { get; set; }

        public int? ClickCount { get; set; }

        public int? ExposureCount { get; set; }

        public decimal? PubPrice_Holiday { get; set; }

        public decimal? SalePrice_Holiday { get; set; }

        public decimal? PubPrice { get; set; }

        public decimal? SalePrice { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Status { get; set; }
    }
}
