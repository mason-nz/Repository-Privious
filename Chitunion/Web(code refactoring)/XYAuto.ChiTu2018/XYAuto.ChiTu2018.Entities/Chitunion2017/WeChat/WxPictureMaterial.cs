namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WxPictureMaterial")]
    public partial class WxPictureMaterial
    {
        [Key]
        public int PicID { get; set; }

        public int? WxID { get; set; }

        public int? WxMaterialID { get; set; }

        [StringLength(200)]
        public string PicName { get; set; }

        [StringLength(500)]
        public string PicUrl { get; set; }

        public int? Status { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
