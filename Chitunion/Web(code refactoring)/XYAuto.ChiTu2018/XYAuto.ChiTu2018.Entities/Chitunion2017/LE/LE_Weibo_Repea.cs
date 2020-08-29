using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_Weibo_Repea
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(100)]
        public string Number { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(2)]
        public string Sex { get; set; }

        [StringLength(200)]
        public string HeadIconURL { get; set; }

        public int? FansCount { get; set; }

        [StringLength(2)]
        public string FansSex { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(200)]
        public string Sign { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }

        public int? SmartSearchID { get; set; }

        public int? SourceID { get; set; }

        public int? ForwardAvg { get; set; }

        public int? CommentAvg { get; set; }

        public int? LikeAvg { get; set; }

        public decimal? DirectPrice { get; set; }

        public decimal? ForwardPrice { get; set; }

        public int? ProvinceID { get; set; }

        public int? CityID { get; set; }

        public int? Status { get; set; }

        public int? TotalScores { get; set; }

        public string Summary { get; set; }

        public int? AuthType { get; set; }

        public bool? IsReserve { get; set; }

        public string TagText { get; set; }
    }
}
