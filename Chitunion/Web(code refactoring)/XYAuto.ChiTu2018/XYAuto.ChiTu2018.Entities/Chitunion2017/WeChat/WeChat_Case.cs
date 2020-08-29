namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WeChat_Case
    {
        [Key]
        public int RecID { get; set; }

        public int MediaType { get; set; }

        public int MediaID { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string CaseContent { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }

        public int? Status { get; set; }
    }
}
