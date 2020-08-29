namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_DemandRelation
    {
        public int Id { get; set; }

        public int? DemandBillNo { get; set; }

        public int? AdgroupId { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserId { get; set; }
    }
}
