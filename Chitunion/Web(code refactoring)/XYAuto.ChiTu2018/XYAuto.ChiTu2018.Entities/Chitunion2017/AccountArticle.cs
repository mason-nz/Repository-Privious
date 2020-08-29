namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccountArticle")]
    public partial class AccountArticle
    {
        [Key]
        public int RecID { get; set; }

        public int? AccountID { get; set; }

        public int? ArticleID { get; set; }

        public int? Operate { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        [Column(TypeName = "text")]
        public string CleanContent { get; set; }

        public DateTime? ReceiveCleanTime { get; set; }

        public int? ReceiveCleanMan { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }
    }
}
