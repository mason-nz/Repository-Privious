namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class App_Device
    {
        [Key]
        public int RecID { get; set; }

        public int? UserID { get; set; }

        [StringLength(200)]
        public string EMEI { get; set; }

        [StringLength(200)]
        public string EMSI { get; set; }

        [StringLength(200)]
        public string AndroidId { get; set; }

        [StringLength(200)]
        public string Network { get; set; }

        [StringLength(200)]
        public string AppVersion { get; set; }

        [StringLength(100)]
        public string Channel { get; set; }

        [StringLength(200)]
        public string SystemVersion { get; set; }

        [StringLength(200)]
        public string PhoneModel { get; set; }

        [StringLength(200)]
        public string ScreenResolution { get; set; }

        public DateTime? ActivationTime { get; set; }

        [StringLength(200)]
        public string Carrier { get; set; }

        [StringLength(1000)]
        public string AllowLocationInfo { get; set; }

        [StringLength(1000)]
        public string AllowNoticeInfo { get; set; }

        public string ReleatedInstallAppInfo { get; set; }

        public DateTime? CreateTime { get; set; }
        public bool IsAllowMsgNotice { get; set; }
        /// <summary>
        /// Æ½Ì¨£¬1£ºandroid    2£ºios
        /// </summary>
        public int Platform { get; set; }
    }
}
