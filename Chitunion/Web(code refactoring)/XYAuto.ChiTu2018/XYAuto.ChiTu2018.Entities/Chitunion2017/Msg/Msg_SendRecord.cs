namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Msg_SendRecord
    {
        [Key]
        public int RecID { get; set; }

        public int? MsgMasterID { get; set; }

        public int? OfficialAccountsID { get; set; }

        public int? CrowdType { get; set; }

        public int? SendStatus { get; set; }

        public DateTime? SettingTime { get; set; }

        public DateTime? ActualTime { get; set; }

        public int? SucceedNum { get; set; }

        public int? FailureNum { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
