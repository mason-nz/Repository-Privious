using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    /// <summary>
    /// 前台返回的统计对象
    /// </summary>
    public class GetMediaDetailStatisticResDTO
    {
        public ReadInfo ReadForWeb { get; set; }
        public List<StatisticItem> DayUpdateForWeb { get; set; }
        public List<StatisticItem> HourUpdateForWeb { get; set; }
    }

    public class StatisticItem {
        public string Key { get; set; }
        public int Value { get; set; }
    }

}
