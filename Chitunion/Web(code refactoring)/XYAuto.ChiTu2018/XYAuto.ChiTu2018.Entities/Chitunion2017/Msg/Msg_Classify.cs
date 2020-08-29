namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Msg_Classify
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(200)]
        public string GraphicTitle { get; set; }

        [StringLength(200)]
        public string RedirectUrl { get; set; }

        [StringLength(200)]
        public string ImgUrl { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int? MasterID { get; set; }
    }
}
