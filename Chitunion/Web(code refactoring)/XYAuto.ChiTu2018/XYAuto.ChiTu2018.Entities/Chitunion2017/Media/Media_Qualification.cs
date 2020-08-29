namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Media_Qualification
    {
        [Key]
        public int RecID { get; set; }

        public int MediaID { get; set; }

        public int? MediaType { get; set; }

        [Required]
        [StringLength(100)]
        public string EnterpriseName { get; set; }

        [StringLength(300)]
        public string QualificationOne { get; set; }

        [StringLength(300)]
        public string QualificationTwo { get; set; }

        [StringLength(300)]
        public string BusinessLicense { get; set; }

        [StringLength(300)]
        public string IDCardFrontURL { get; set; }

        [StringLength(300)]
        public string IDCardBackURL { get; set; }

        [StringLength(300)]
        public string AgentContractFrontURL { get; set; }

        [StringLength(300)]
        public string AgentContractBackURL { get; set; }

        public int? MediaRelations { get; set; }

        public int? OperatingType { get; set; }

        public int CreateUserID { get; set; }

        public int Status { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
