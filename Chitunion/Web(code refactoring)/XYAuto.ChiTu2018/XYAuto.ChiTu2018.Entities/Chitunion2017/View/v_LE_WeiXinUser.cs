using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.View
{
    /// <summary>
    /// 注释：v_LE_WeiXinUser
    /// 作者：lix
    /// 日期：2018/6/11 16:59:57
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>

    public partial class v_LE_WeiXinUser
    {
        [Key]
        [Column(Order = 0)]
        public int WeiXinUserID { get; set; }

        public int? subscribe { get; set; }

        [StringLength(200)]
        public string openid { get; set; }

        [StringLength(200)]
        public string nickname { get; set; }

        public int? sex { get; set; }

        [StringLength(20)]
        public string city { get; set; }

        [StringLength(20)]
        public string country { get; set; }

        [StringLength(20)]
        public string province { get; set; }

        [StringLength(20)]
        public string language { get; set; }

        [StringLength(500)]
        public string headimgurl { get; set; }

        public DateTime? subscribe_time { get; set; }

        [StringLength(200)]
        public string unionid { get; set; }

        [StringLength(500)]
        public string remark { get; set; }

        public int? groupid { get; set; }

        [StringLength(1000)]
        public string tagid_list { get; set; }

        public int? UserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public DateTime? AuthorizeTime { get; set; }

        [StringLength(500)]
        public string QRcode { get; set; }

        [StringLength(200)]
        public string Inviter { get; set; }

        [StringLength(500)]
        public string InvitationQR { get; set; }

        public int? Status { get; set; }

        public int? Source { get; set; }

        //[Key]
        //[Column(Order = 1)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AdvertiserUserId { get; set; }
    }
}
