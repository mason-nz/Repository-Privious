using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.APP
{
    /// <summary>
    /// 注释：AppPushMsgSwitchLog
    /// 作者：lix
    /// 日期：2018/5/24 14:02:29
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class AppPushMsgSwitchLog
    {
        public int RecId { get; set; }
        /// <summary>
        /// 设备号IMEI
        /// </summary>
        public string DeviceId { get; set; }
        public bool IsOpen { get; set; }
        public DateTime PushShowTime { get; set; }
        public int Platform { get; set; }
    }
}
