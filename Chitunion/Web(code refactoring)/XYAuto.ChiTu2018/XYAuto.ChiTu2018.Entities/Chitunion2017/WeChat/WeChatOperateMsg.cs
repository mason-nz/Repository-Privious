namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WeChatOperateMsg")]
    public partial class WeChatOperateMsg
    {
        [Key]
        public int RecID { get; set; }

        public int? Status { get; set; }

        public int MediaType { get; set; }

        public int MediaID { get; set; }

        [StringLength(200)]
        public string MediaName { get; set; }

        public int? PublishID { get; set; }

        [StringLength(200)]
        public string PublishName { get; set; }

        public int OptType { get; set; }

        [StringLength(200)]
        public string SubmitUserName { get; set; }

        public int? SubmitUserID { get; set; }

        public int? MsgType { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public int? ADTemID { get; set; }

        [StringLength(200)]
        public string ADTemName { get; set; }
    }
}
