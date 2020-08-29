namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PubDeatil_ImgInfo
    {
        [Key]
        public int RecID { get; set; }

        public int? ADDetailID { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }

        public int? ImagePosition { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
