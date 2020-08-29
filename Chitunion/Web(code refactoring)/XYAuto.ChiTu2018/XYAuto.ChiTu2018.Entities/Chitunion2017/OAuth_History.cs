namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OAuth_History
    {
        [Key]
        public int RecID { get; set; }

        public int? WxID { get; set; }

        [StringLength(20)]
        public string AppID { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
