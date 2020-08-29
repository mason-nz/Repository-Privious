using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.LuckDrawActivity
{
    /// <summary>
    /// 注释：Prize
    /// 作者：zhanglb
    /// 日期：2018/5/25 9:49:09
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class Prize
    {
        public int PrizeId { get; set; }
        public int ActivityId { get; set; }
        public string AwardName { get; set; }
        public decimal DrawRatio { get; set; }
        public decimal DrawMinPrice { get; set; }
        public decimal DrawMaxPrice { get; set; }
        public decimal StartSection { get; set; }
        public decimal EndSection { get; set; }
        public int WinningMaxNum { get; set; }
        public decimal WinningMaxPrice { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }

    }
}
