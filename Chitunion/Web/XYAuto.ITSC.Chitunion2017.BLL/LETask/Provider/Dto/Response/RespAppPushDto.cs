using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response
{
    /// <summary>
    /// 注释：RespAppPushDto
    /// 作者：lix
    /// 日期：2018/5/24 14:25:53
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
