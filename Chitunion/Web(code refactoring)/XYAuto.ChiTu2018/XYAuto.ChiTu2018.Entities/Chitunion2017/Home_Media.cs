namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Home_Media
    {
        [Key]
        public int RecID { get; set; }

        public int CategoryID { get; set; }

        public int ADDetailID { get; set; }

        public int TemplateID { get; set; }

        public int MediaID { get; set; }

        public int MediaType { get; set; }

        public int PublishState { get; set; }

        public int SortNumber { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }

        [StringLength(200)]
        public string VideoUrl { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
