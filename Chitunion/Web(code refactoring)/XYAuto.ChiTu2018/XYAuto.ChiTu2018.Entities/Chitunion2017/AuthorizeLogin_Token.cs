namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AuthorizeLogin_Token
    {
        [Key]
        public int RecID { get; set; }

        public int? APPID { get; set; }

        [StringLength(20)]
        public string IP { get; set; }

        public long? TimeStamp { get; set; }

        [StringLength(200)]
        public string MD5Code { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}
