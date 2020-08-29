namespace XYAuto.ChiTu2018.Entities.Chitunion2017.View
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_AppADList
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PubID { get; set; }

        public int? TemplateID { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MediaID { get; set; }

        public int? CreateUserID { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal MinPrice { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal MaxPrice { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PriceCount { get; set; }

        public int? PubStatus { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(100)]
        public string PubStatusName { get; set; }

        public bool? CanAddToRecommend { get; set; }

        public bool? IsRange { get; set; }
    }
}
