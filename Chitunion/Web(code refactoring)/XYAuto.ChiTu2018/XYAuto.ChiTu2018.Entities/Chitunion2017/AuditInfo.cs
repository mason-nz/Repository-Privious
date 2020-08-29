namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuditInfo")]
    public partial class AuditInfo
    {
        public int Id { get; set; }

        public int RelationType { get; set; }

        public int RelationId { get; set; }

        public int? AuditStatus { get; set; }

        [StringLength(500)]
        public string RejectMsg { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserId { get; set; }
    }
}
