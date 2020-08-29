namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Media_Remark_Basic
    {
        [Key]
        public int RecID { get; set; }

        public int? RelationID { get; set; }

        public int? RemarkID { get; set; }

        [StringLength(200)]
        public string OtherContent { get; set; }

        public int? EnumType { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
