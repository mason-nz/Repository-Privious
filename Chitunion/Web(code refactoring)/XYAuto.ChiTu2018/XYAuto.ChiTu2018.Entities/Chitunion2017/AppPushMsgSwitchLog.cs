using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    /// <summary>
    /// 注释：AppPushMsgSwitchLog
    /// 作者：lix
    /// 日期：2018/6/5 10:16:29
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public partial class AppPushMsgSwitchLog
    {
        [Key]
        public int RecId { get; set; }

        //设备号
        [StringLength(200)]
        public string DeviceId { get; set; }

        //是否开启
        public bool IsOpen { get; set; }

        //推送显示开启的时间
        public DateTime PushShowTime { get; set; }

        public bool IsClosed { get; set; }
    }
}
