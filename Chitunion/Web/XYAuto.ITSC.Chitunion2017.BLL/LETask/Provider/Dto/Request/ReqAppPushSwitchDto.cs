using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request
{
    /// <summary>
    /// 注释：ReqAppPushSwitchDto
    /// 作者：lix
    /// 日期：2018/5/24 14:28:05
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqAppPushSwitchDto
    {
        public string DeviceId { get; set; }
        public bool IsOpend { get; set; }
        /// <summary>
        /// 平台，1：android    2：ios
        /// </summary>
        public int Platform { get; set; } = (int)PlatformTypeEnum.Android;
    }
}
