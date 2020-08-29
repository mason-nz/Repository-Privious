namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Publish_ExtendInfoPCAPP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ADDetailID { get; set; }

        public int PubID { get; set; }

        public int? MediaType { get; set; }

        public int MediaID { get; set; }

        [StringLength(200)]
        public string AdLegendURL { get; set; }

        [StringLength(100)]
        public string AdPosition { get; set; }

        [StringLength(100)]
        public string AdForm { get; set; }

        public int? DisplayLength { get; set; }

        public bool? CanClick { get; set; }

        public int? CarouselCount { get; set; }

        [StringLength(100)]
        public string PlayPosition { get; set; }

        public int? DailyExposureCount { get; set; }

        public bool? CPM { get; set; }

        public bool? CarouselPlay { get; set; }

        public int? DailyClickCount { get; set; }

        public bool? CPM2 { get; set; }

        public bool? CarouselPlay2 { get; set; }

        [StringLength(20)]
        public string ThrMonitor { get; set; }

        [StringLength(20)]
        public string SysPlatform { get; set; }

        [StringLength(100)]
        public string Style { get; set; }

        public bool? IsDispatching { get; set; }

        [StringLength(2000)]
        public string ADShow { get; set; }

        [StringLength(2000)]
        public string ADRemark { get; set; }

        [StringLength(200)]
        public string AcceptBusinessIDs { get; set; }

        [StringLength(200)]
        public string NotAcceptBusinessIDs { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }

        [StringLength(200)]
        public string AcceptBusinessNames { get; set; }

        [StringLength(200)]
        public string NotAcceptBusinessNames { get; set; }
    }
}
