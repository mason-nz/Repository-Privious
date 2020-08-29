namespace XYAuto.ChiTu2018.Entities.Chitunion2017.View
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_UserInfo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string Mobile { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Pwd { get; set; }

        public int? Type { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Source { get; set; }

        public bool? IsAuthMTZ { get; set; }

        public int? AuthAEUserID { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool IsAuthAE { get; set; }

        public int? SysUserID { get; set; }

        [StringLength(20)]
        public string EmployeeNumber { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }

        public int? Category { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public int? LockState { get; set; }

        public int? SleepState { get; set; }

        public int? LockType { get; set; }

        public int? SleepStatus { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RegisterType { get; set; }

        public int? CleanStatus { get; set; }

        [StringLength(200)]
        public string TrueName { get; set; }

        [StringLength(20)]
        public string SysName { get; set; }
    }
}
