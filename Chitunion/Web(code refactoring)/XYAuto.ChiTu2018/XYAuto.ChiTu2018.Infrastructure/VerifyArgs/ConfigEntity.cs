using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Infrastructure.VerifyArgs
{
    /// <summary>
    /// 注释：ConfigEntity,因为service层需要用，涉及到app service，H5 service..等等，只能抽出来
    /// 作者：lix
    /// 日期：2018/5/21 19:23:40
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ConfigEntity
    {
        public int UserId { get; set; }
        public string Ip { get; set; }
    }
}
