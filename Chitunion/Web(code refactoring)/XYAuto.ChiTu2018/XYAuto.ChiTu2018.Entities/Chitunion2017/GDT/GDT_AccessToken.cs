namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_AccessToken
    {
        public int Id { get; set; }

        public int RelationType { get; set; }

        [StringLength(200)]
        public string AccessToken { get; set; }

        [StringLength(200)]
        public string RefreshToken { get; set; }

        public int ClientId { get; set; }

        [Required]
        [StringLength(300)]
        public string ClientSecret { get; set; }

        public int? AccessTokenExpiresIn { get; set; }

        public int? RefreshTokenExpiresIn { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserId { get; set; }

        public DateTime? UpdateTime { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }
    }
}
