namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Msg_ActiveUser
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(50)]
        public string NikeName { get; set; }

        [StringLength(100)]
        public string OpenID { get; set; }

        [StringLength(100)]
        public string WxAppID { get; set; }

        public int? Status { get; set; }

        [StringLength(100)]
        public string WxAppName { get; set; }
    }
}
