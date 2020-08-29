namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Media_SouHu
    {
        [Key]
        public int MediaID { get; set; }

        [StringLength(512)]
        public string Url { get; set; }

        [StringLength(64)]
        public string UserID { get; set; }

        [StringLength(64)]
        public string UserName { get; set; }

        [StringLength(1024)]
        public string Abstract { get; set; }

        public int? FollowCount { get; set; }

        public int? FansCount { get; set; }

        [StringLength(512)]
        public string HeadImg { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? UStatus { get; set; }

        public int? CreateUserID { get; set; }
    }
}
