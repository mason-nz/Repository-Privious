using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Response
{
    /// <summary>
    /// 注释：RespAppPushDto
    /// 作者：lix
    /// 日期：2018/6/5 9:26:04
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class RespAppPushDto
    {
        public bool GlobalSwitch { get; set; }
        public bool IsOpen { get; set; }
        /// <summary>
        /// 现在 是否 需要显示
        /// </summary>
        public bool IsShowNow { get; set; }
    }
}
