using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.User
{
    [Table("UserInfo")]
    public partial class UserInfo
    {
        [Key]
        public int UserID { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string Pwd { get; set; }

        public int? Type { get; set; }

        public int Source { get; set; }

        public bool? IsAuthMTZ { get; set; }

        public int? AuthAEUserID { get; set; }

        public bool IsAuthAE { get; set; }

        public int? SysUserID { get; set; }

        [StringLength(20)]
        public string EmployeeNumber { get; set; }

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

        public int RegisterType { get; set; }

        public int? CleanStatus { get; set; }

        public long? PromotionChannelID { get; set; }

        [StringLength(500)]
        public string HeadimgURL { get; set; }

        [StringLength(100)]
        public string ChannelUserID { get; set; }

        public string RegisterIp { get; set; }
    }
}
