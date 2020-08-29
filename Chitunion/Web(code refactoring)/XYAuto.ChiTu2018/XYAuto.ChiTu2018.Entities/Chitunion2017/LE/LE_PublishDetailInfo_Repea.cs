using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_PublishDetailInfo_Repea
    {
        [Key]
        public int RecID { get; set; }

        public int? MediaType { get; set; }

        public int MediaID { get; set; }

        public int ADPosition1 { get; set; }

        public int ADPosition2 { get; set; }

        public int ADPosition3 { get; set; }

        public decimal Price { get; set; }

        public int? PublishStatus { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
