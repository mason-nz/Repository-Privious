using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.Extend.Profit.LE
{
    /// <summary>
    /// 注释：受邀人信息
    /// 作者：zhanglb
    /// 日期：2018/5/14 18:27:45
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class BeInviterModel
    {
        /// <summary>
        /// 邀请表主键ID
        /// </summary>
        public int RecId { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Nickname { get; set; } = "";
        /// <summary>
        /// 微信头像
        /// </summary>
        public string HeadImgurl { get; set; } = "";
        /// <summary>
        /// 受邀关注时间
        /// </summary>
        public DateTime InviteTime { get; set; }
        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal RedEvesPrice { get; set; }
        /// <summary>
        /// 红包状态（104001 领取红包;104002 已领取;104003 当日红包已领完,104004 尚未完成分享）
        /// </summary>
        public int RedEvesStatus { get; set; }
    }
}
