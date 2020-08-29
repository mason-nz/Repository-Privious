using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.User.Dto
{
    /// <summary>
    /// 注释：RespUserDetailDto
    /// 作者：zhanglb
    /// 日期：2018/5/16 11:34:12
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class RespUserDetailDto
    {
        public int Type { get; set; } = -2;
        public int Status { get; set; } = -2;
        public string TrueName { get; set; } = string.Empty;
        public string IdentityNo { get; set; } = string.Empty;
        public string IdCardFrontUrl { get; set; } = string.Empty;
        public string IdCardBackUrl { get; set; } = string.Empty;
        public string BLicenceUrl { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;

    }
}
