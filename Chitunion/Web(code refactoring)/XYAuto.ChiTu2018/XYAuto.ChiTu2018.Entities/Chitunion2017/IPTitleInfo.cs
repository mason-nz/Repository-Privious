namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IPTitleInfo")]
    public partial class IPTitleInfo
    {
        [Key]
        public int RecID { get; set; }

        public int? PIP { get; set; }

        public int? SubIP { get; set; }

        public int? Relation { get; set; }

        public int? TitleID { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
