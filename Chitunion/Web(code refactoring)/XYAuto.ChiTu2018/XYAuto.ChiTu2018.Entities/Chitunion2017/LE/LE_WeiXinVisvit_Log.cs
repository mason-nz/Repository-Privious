using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_WeiXinVisvit_Log
    {
        [Key]
        public int RecID { get; set; }

        public int? UserID { get; set; }

        public long? ChannelID { get; set; }

        [StringLength(300)]
        public string Url { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? Type { get; set; }

        [StringLength(200)]
        public string ChannelCode { get; set; }

        [StringLength(500)]
        public string UserAgent { get; set; }
    }
}
