using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.Sign.Dto
{
    /// <summary>
    /// 注释：ReqKeyValueDto
    /// 作者：lihf
    /// 日期：2018/5/16 14:05:19
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class AppReqKeyValueDto
    {
        public AppReqMessageType t { get; set; }
        public string v { get; set; }
    }

    public enum AppReqMessageType
    {
        无 = Entities.Constants.Constant.INT_INVALID_VALUE,
        邀请 = 1,
        Pc端登录 = 2,

        场景 = 3
    }
}
