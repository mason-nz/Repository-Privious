using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.Extend.HD
{
    /// <summary>
    /// 注释：HdAwardeeInfoDo
    /// 作者：lix
    /// 日期：2018/6/11 16:52:48
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class HdAwardeeInfoDo
    {
        public string NickName { get; set; }
        public string Mobile { get; set; }
        public DateTime DrawTime { get; set; }
        public decimal DrawPrice { get; set; }
        public string DrawDescribe { get; set; }
    }
}
