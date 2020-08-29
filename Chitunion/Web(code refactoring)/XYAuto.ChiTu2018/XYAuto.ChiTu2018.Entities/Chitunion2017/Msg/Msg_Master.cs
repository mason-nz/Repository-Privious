namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Msg_Master
    {
        [Key]
        public int RecID { get; set; }

        public int? BusinessType { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public int? MessageType { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int? Status { get; set; }
    }
}
