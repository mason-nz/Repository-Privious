namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MaterielChannelData")]
    public partial class MaterielChannelData
    {
        [Key]
        public int RecID { get; set; }

        public int? ChannelID { get; set; }

        public int? MaterielID { get; set; }

        public DateTime? DataDate { get; set; }

        public int? ReadCount { get; set; }

        public int? LikeCount { get; set; }

        public int? CommentCount { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
