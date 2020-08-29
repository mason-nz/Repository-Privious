using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_UserDetailInfo
    {
        [Key]
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
        public string OrganizationURL { get; set; }

        [StringLength(200)]
        public string IDCardFrontURL { get; set; }

        [StringLength(200)]
        public string IDCardBackURL { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }
    }
}
