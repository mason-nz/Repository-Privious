using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.User.Dto
{
    /// <summary>
    /// 注释：ReqMobileInfoDto
    /// 作者：zhanglb
    /// 日期：2018/5/16 19:16:19
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqMobileInfoDto
    {
        public int SmsAction { get; set; }
        public string Mobile { get; set; }
        public int CheckCode { get; set; }
    }
}
