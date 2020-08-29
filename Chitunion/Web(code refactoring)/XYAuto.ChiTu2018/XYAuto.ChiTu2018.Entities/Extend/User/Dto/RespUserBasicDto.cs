using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.User.Dto
{
    /// <summary>
    /// 注释：RespUserBasicDto
    /// 作者：zhanglb
    /// 日期：2018/5/15 20:49:18
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class RespUserBasicDto
    {
        public string NickName { get; set; }
        public string HeadImgUrl { get; set; }
        public string Mobile { get; set; }
        public string AccountName { get; set; }
        public int AccountType { get; set; }
        public int Status { get; set; }
        public int IsFollow { get; set; }
        public int Sex { get; set; }
        public string IdentityNo { get; set; }
        public string TrueName { get; set; }

    }
}
