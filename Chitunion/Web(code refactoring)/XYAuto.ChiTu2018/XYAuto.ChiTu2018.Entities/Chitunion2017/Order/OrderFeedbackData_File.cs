namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderFeedbackData_File
    {
        [Key]
        public int RecID { get; set; }

        public int MediaType { get; set; }

        public int FeedBackID { get; set; }

        [Required]
        [StringLength(200)]
        public string UploadFileURL { get; set; }
    }
}
