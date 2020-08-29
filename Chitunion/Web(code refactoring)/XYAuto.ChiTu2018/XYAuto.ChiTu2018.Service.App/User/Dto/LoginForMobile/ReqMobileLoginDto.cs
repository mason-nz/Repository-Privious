using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.User.Dto.LoginForMobile
{
    /// <summary>
    /// 注释：ReqMobileLoginDto
    /// 作者：zhanglb
    /// 日期：2018/6/12 15:42:10
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqMobileLoginDto
    {
        public string mobile { get; set; }
        public string mobileCheckCode { get; set; }
        public int from { get; set; } = 3008; //3008:android   3009：ios
    }
}
