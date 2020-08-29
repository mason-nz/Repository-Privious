namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class newLB_TaskAssign
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(100)]
        public string TaskID { get; set; }

        public int? Summary { get; set; }

        public int? Status { get; set; }

        public int? AssignUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
