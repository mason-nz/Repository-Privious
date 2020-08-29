namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MediaCategory")]
    public partial class MediaCategory
    {
        [Key]
        public int RecID { get; set; }

        public int? MediaType { get; set; }

        public int? WxID { get; set; }

        public int? CategoryID { get; set; }

        public int? SortNumber { get; set; }
    }
}
