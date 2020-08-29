namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Publish_DetailInfo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecID { get; set; }

        public int? PubID { get; set; }

        public int? MediaType { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MediaID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ADPosition1 { get; set; }

        public int? ADPosition2 { get; set; }

        public int? ADPosition3 { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal Price { get; set; }

        public bool? IsCarousel { get; set; }

        public int? BeginPlayDays { get; set; }

        public int? PublishStatus { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public decimal? SalePrice { get; set; }

        public decimal? CostReferencePrice { get; set; }

        public int? CostDetailID { get; set; }
    }
}
