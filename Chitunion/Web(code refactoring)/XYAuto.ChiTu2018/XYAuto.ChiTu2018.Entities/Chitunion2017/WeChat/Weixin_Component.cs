namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Weixin_Component
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(20)]
        public string AppID { get; set; }

        [StringLength(50)]
        public string Secret { get; set; }

        [StringLength(20)]
        public string Token { get; set; }

        [StringLength(50)]
        public string EncodingAESKey { get; set; }

        [StringLength(200)]
        public string VerifyTicket { get; set; }

        public DateTime? TicketTime { get; set; }

        [StringLength(200)]
        public string AccessToken { get; set; }

        public DateTime? AccessTokenTime { get; set; }
    }
}
