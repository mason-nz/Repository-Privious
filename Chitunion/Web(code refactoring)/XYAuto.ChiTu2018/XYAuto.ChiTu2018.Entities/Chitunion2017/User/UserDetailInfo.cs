using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.User
{
    [Table("UserDetailInfo")]
    public partial class UserDetailInfo
    {
        [Key]
        //[Key, ForeignKey("UserInfo")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [Required]
        [StringLength(200)]
        public string TrueName { get; set; }

        public int? BusinessID { get; set; }

        public int? ProvinceID { get; set; }

        public int? CityID { get; set; }

        public int? CounntyID { get; set; }

        [StringLength(50)]
        public string Contact { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(200)]
        public string BLicenceURL { get; set; }

        [StringLength(200)]
        public string IDCardFrontURL { get; set; }

        [StringLength(200)]
        public string IDCardBackURL { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }

        [StringLength(200)]
        public string OrganizationURL { get; set; }

        public int Status { get; set; }

        [StringLength(50)]
        public string IdentityNo { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        public DateTime? ApplyTime { get; set; }

        public DateTime? AuditTime { get; set; }

        public int? AuditUserID { get; set; }

        public int? Sex { get; set; }

        //[JsonIgnore]
        //public virtual UserInfo UserInfo { get; set; }
    }
}
