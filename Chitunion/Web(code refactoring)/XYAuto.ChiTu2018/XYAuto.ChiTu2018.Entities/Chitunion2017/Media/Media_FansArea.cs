namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Media_FansArea
    {
        [Key]
        public int RecID { get; set; }

        public int MediaID { get; set; }

        public int MediaType { get; set; }

        public int ProvinceID { get; set; }

        public decimal UserScale { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUserID { get; set; }
    }
}
