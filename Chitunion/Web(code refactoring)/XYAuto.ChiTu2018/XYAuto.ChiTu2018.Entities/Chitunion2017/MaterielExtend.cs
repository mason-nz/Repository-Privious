namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MaterielExtend")]
    public partial class MaterielExtend
    {
        [Key]
        public int MaterielID { get; set; }

        public int? ThirdID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public int ArticleID { get; set; }

        public int? ArticleFrom { get; set; }

        [StringLength(200)]
        public string HeadContentURL { get; set; }

        public int? HeadContentType { get; set; }

        public int? BodyContentType { get; set; }

        [StringLength(200)]
        public string FootContentURL { get; set; }

        [StringLength(50)]
        public string ContractNumber { get; set; }

        public int SerialID { get; set; }

        [StringLength(200)]
        public string Tag { get; set; }

        [StringLength(200)]
        public string Category { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
