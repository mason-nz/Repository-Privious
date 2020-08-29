using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.Infrastructure.Extensions;

namespace XYAuto.ChiTu2018.Settlement.ConfigEntity
{
    /// <summary>
    /// 注释：ConfigEntityBase
    /// 作者：lix
    /// 日期：2018/5/22 11:12:18
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
   public class ConfigEntityBase
    {
        public int AtStartDateOffest { get; set; }
        public int NextLastDateOffset { get; set; }
        /// <summary>
        /// 用读取字符串
        /// </summary>
        public string AtEveryDayHourRange { get; set; }

        /// <summary>
        /// 每天定时的小时
        /// </summary>
        public int AtEveryDayRangeForHour
        {
            get
            {
                return AtEveryDayHourRange?.Split(':')[0].ToInt() ?? 0;
            }
        }
        /// <summary>
        /// 每天定时的分钟
        /// </summary>
        public int AtEveryDayRangeForMinutes
        {
            get
            {
                return AtEveryDayHourRange?.Split(':')[1].ToInt() ?? 0;
            }
        }
        public bool IsStart { get; set; }

        public int SelectOffsetNum { get; set; }
        public int SelectTopSize { get; set; }

        //每个小时的在固定几分钟开始
        public int AtEveryHoursForMinutes { get; set; }
    }
}
