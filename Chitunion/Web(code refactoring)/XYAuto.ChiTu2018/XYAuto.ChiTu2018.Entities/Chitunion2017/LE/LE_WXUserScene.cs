using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_WXUserScene
    {
        [Key]
        public int RecID { get; set; }

        public int? UserID { get; set; }

        public int? SceneID { get; set; }

        [StringLength(20)]
        public string SceneName { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Status { get; set; }
    }
}
