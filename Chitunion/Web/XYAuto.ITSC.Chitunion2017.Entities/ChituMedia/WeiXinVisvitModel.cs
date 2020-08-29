using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class WeiXinVisvitModel
    {
        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 渠道编码
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 访问URL地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 类型 1：微信 2：订单
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 当前请求客户端UserAgent信息
        /// </summary>
        public string UserAgent { get; set; }
    }
}
