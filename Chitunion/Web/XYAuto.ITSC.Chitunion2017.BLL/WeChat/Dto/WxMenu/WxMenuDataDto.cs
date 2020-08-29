using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.WxTemp;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.WxMenu
{
    /// <summary>
    /// 注释：WxMenuDataDto
    /// 作者：masj
    /// 日期：2018/5/29 15:36:57
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class WxMenuDataDto
    {
        /// <summary>
        /// 微信号名称
        /// </summary>
        public string WxNum { get; set; }

        /// <summary>
        /// 微信开发者AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 微信开发者AppSecret
        /// </summary>
        //[Newtonsoft.Json.JsonIgnore()]
        public string AppSecret { get; set; }

        /// <summary>
        /// 微信菜单点击事件集合
        /// </summary>
        public List<WxMenuClick> MenuClickList { get; set; }

        /// <summary>
        /// 公众号关注后，自动回复的信息
        /// </summary>
        public string SubscribeMsg { get; set; }

        /// <summary>
        /// 公众号关注后，默认自动回复的信息（仅支持文本格式）
        /// </summary>
        public string DefaultCustomMsg { get; set; }

        /// <summary>
        /// 微信自定义回复消息集合
        /// </summary>
        public List<WxCustomMsg> CustomMsgList { get; set; }

        /// <summary>
        /// 微信关注后，（1元提现活动）新用户弹出文章消息
        /// </summary>
        public WxSubscribeArticle SubArticleInfo { get; set; }
    }

    /// <summary>
    /// 微信关注后，（1元提现活动）新用户弹出文章消息
    /// </summary>
    public class WxSubscribeArticle
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 文章头图URL
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 文章链接URL
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// 微信公众号菜单中涉及Click对象
    /// </summary>
    public class WxMenuClick
    {
        /// <summary>
        /// EventKey
        /// </summary>
        public string EventKey { get; set; }


        /// <summary>
        /// 点击后展示的媒体图片ID
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 菜单点击事件描述
        /// </summary>
        public string Desc { get; set; }
    }

    /// <summary>
    /// 微信公众号-自定义回复消息
    /// </summary>
    public class WxCustomMsg
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 类型具体的值，若media时，则维护MediaID信息
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 自定义回复类型（msg、media）
        /// </summary>
        public string Type { get; set; }
    }
}
