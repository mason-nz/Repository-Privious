namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TaskScheduler_User
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? GroupId { get; set; }

        public int? TaskStatus { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserId { get; set; }

        public int Status { get; set; }
    }
}
