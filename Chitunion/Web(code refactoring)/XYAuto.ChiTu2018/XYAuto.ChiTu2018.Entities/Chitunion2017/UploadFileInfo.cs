namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UploadFileInfo")]
    public partial class UploadFileInfo
    {
        [Key]
        public int RecID { get; set; }

        public int Type { get; set; }

        [StringLength(50)]
        public string RelationTableName { get; set; }

        public int? RelationID { get; set; }

        [Required]
        [StringLength(200)]
        public string FilePah { get; set; }

        [StringLength(100)]
        public string FileName { get; set; }

        [StringLength(50)]
        public string ExtendName { get; set; }

        public int? FileSize { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreaetUserID { get; set; }
    }
}
