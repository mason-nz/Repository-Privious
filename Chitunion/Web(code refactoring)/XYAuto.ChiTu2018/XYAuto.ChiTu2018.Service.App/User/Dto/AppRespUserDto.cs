/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.User.Dto
* 类 名 称 ：RespUserDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/23 19:46:46
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.User.Dto
{
    public class AppRespUserDto
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
