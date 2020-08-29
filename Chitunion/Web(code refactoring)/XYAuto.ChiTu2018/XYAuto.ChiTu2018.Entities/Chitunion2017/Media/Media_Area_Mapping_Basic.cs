namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Media_Area_Mapping_Basic
    {
        [Key]
        public int RecID { get; set; }

        public int MediaType { get; set; }

        public int BaseMediaID { get; set; }

        public int ProvinceID { get; set; }

        public int? CityID { get; set; }

        public int RelateType { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
