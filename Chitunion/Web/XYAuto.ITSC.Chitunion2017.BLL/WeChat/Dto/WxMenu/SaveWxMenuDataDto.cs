using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.WxMenu
{
    /// <summary>
    /// 注释：SaveWxMenuDataDto
    /// 作者：masj
    /// 日期：2018/5/30 18:30:11
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class SaveWxMenuDataDto
    {
        /// <summary>
        /// 微信开发者AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 微信菜单点击事件集合
        /// </summary>
        public List<WxMenuButton> Buttons { get; set; }
    }

    /// <summary>
    /// 微信公众号菜单对象（包含一级或二级）
    /// </summary>
    public class WxMenuButton
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型具体的值，若view时，则维护Url；若click，则维护Click-Key信息
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 微信端MediaId，当Type=若click，需要维护此属性
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 菜单类型（view、click）
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 菜单层级（1、2）
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 子菜单数据
        /// </summary>
        public List<WxMenuButton> SubButtons { get; set; }
    }
}
