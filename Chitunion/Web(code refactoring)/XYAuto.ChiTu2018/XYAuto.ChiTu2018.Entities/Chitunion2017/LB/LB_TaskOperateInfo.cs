namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LB_TaskOperateInfo
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(100)]
        public string TaskID { get; set; }

        public int? OptType { get; set; }

        [Column(TypeName = "text")]
        public string OptContent { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
