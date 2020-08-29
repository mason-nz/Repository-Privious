namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogInfo")]
    public partial class LogInfo
    {
        [Key]
        public int RecID { get; set; }

        public int? Module { get; set; }

        public int? ActionType { get; set; }

        [StringLength(2000)]
        public string Content { get; set; }

        [StringLength(50)]
        public string IP { get; set; }

        public int? UserID { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(50)]
        public string SysID { get; set; }
    }
}
