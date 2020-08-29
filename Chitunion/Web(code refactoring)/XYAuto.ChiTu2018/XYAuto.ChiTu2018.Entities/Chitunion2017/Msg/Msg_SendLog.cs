namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Msg_SendLog
    {
        [Key]
        public int RecID { get; set; }

        public int? SendRecordID { get; set; }

        [StringLength(100)]
        public string OpenID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
