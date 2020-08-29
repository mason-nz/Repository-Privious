using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Activity
{
    /// <summary>
    /// 注释：OneYuanActivityEntity
    /// 作者：lix
    /// 日期：2018/6/12 17:48:50
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class OneYuanActivityEntity
    {
        public decimal Price { get; set; }
        public int OrderNum { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
