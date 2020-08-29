using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.WxTemp
{
    /// <summary>
    /// 注释：WxTempDataDto
    /// 作者：masj
    /// 日期：2018/5/19 14:47:02
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class SendWxTempDataDto
    {
        /// <summary>
        /// 微信开发者AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 微信开发者AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 微信模板ID
        /// </summary>
        public string WxTempId { get; set; }

        /// <summary>
        /// 发送模板数据参数集合（json格式）
        /// </summary>
        public string WxTempParas { get; set; }

        /// <summary>
        /// 发送模板数据后的跳转URL
        /// </summary>
        public string WxTempUrl { get; set; }

        /// <summary>
        /// 发送模板消息给谁（多个人openid用逗号分隔）
        /// </summary>
        public string SendOpenIds { get; set; }
    }
}
