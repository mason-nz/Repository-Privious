using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Response
{
    /// <summary>
    /// 注释：RespKrBaseDto
    /// 作者：lix
    /// 日期：2018/5/22 16:09:01
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class RespKrBaseDto
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
    }
}
