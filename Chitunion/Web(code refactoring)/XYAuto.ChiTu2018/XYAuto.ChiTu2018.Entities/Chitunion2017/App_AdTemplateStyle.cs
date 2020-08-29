namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class App_AdTemplateStyle
    {
        [Key]
        public int RecID { get; set; }

        public int BaseMediaID { get; set; }

        public int? AdTemplateID { get; set; }

        public bool IsPublic { get; set; }

        [StringLength(200)]
        public string AdStyle { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
