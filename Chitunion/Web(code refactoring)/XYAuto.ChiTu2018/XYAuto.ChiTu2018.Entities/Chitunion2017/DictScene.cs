namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DictScene")]
    public partial class DictScene
    {
        [Key]
        public int SceneID { get; set; }

        public int? ParenID { get; set; }

        [StringLength(50)]
        public string SceneName { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
