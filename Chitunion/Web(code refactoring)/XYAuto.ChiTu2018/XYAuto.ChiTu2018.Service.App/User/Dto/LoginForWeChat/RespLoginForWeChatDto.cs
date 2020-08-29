using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.User.Dto.LoginForWeChat
{
    /// <summary>
    /// 注释：RespLoginForWeChatDto
    /// 作者：lix
    /// 日期：2018/6/8 16:07:34
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class RespLoginForWeChatDto
    {
        /// <summary>
        /// 沿用之前的风格
        /// </summary>
        public string IsNewUser { get; set; }
        public string CookiesVal { get; set; }
        public string HomeUrl { get; set; }
    }
}
