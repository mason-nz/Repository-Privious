using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYAuto.ChiTu2018.API.Models.Withdrawals
{
    /// <summary>
    /// 注释：WxTemplateRedirectDto
    /// 作者：lix
    /// 日期：2018/5/28 10:20:45
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class WxTemplateRedirectDto
    {
        /// <summary>
        /// 配置的类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 跳转的地址
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}