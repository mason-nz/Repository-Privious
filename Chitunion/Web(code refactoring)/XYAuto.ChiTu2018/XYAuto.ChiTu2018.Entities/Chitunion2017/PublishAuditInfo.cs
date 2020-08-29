namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PublishAuditInfo")]
    public partial class PublishAuditInfo
    {
        [Key]
        public int RecID { get; set; }

        public int MediaType { get; set; }

        public int? MediaID { get; set; }

        public int? PublishID { get; set; }

        public int OptType { get; set; }

        public int? PubStatus { get; set; }

        [Column(TypeName = "text")]
        public string RejectMsg { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public int? TemplateID { get; set; }
    }
}
