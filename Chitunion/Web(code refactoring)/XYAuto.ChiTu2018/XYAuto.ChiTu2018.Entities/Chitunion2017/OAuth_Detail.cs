namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OAuth_Detail
    {
        [Key]
        public int RecID { get; set; }

        public int? HisID { get; set; }

        public int? OAuthID { get; set; }
    }
}
