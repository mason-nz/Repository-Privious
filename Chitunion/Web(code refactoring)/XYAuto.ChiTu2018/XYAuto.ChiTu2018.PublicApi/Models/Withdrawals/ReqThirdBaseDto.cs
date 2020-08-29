using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.PublicApi.Models.Withdrawals
{
    /// <summary>
    /// 注释：ReqThirdBaseDto
    /// 作者：lix
    /// 日期：2018/5/24 19:16:18
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqThirdBaseDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int UserId { get; set; }

        /// <summary>
        /// 客户端ip
        /// </summary>
        [Required]
        public string Ip { get; set; }
    }
}