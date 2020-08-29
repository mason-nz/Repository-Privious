namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Media_BasePCAPP
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string HeadIconURL { get; set; }

        public int? ProvinceID { get; set; }

        public int? CityID { get; set; }

        public int? DailyLive { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }
    }
}
