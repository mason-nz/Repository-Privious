namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WeChatAuditInfo")]
    public partial class WeChatAuditInfo
    {
        [Key]
        public int RecID { get; set; }

        public int? Status { get; set; }

        public int MediaType { get; set; }

        public int? MediaID { get; set; }

        public int? PublishID { get; set; }

        public int OptType { get; set; }

        public int? ExpiredDays { get; set; }

        public int? SubmitUserID { get; set; }

        public int? MsgType { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
