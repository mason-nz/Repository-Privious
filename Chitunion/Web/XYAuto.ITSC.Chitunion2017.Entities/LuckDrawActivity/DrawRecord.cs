using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.LuckDrawActivity
{
    /// <summary>
    /// 注释：DrawRecord
    /// 作者：zhanglb
    /// 日期：2018/5/25 10:25:21
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class DrawRecord
    {
        public int ActivityId { get; set; }
        public int PrizeId { get; set; }
        public int UserId { get; set; }
        public decimal DrawPrice { get; set; }
        public string DrawDescribe { get; set; }
        public DateTime DrawTime { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
    }
}
