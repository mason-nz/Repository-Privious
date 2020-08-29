namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LB_TaskLabel
    {
        [Key]
        public int LabelID { get; set; }

        public int TaskID { get; set; }

        public int? DictType { get; set; }

        public bool? IsCustom { get; set; }

        public int? DictId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
