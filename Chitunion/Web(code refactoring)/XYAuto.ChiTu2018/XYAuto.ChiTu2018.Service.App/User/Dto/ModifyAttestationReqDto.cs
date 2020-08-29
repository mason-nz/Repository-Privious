/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.User.Dto
* 类 名 称 ：ModifyAttestationReqDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/24 11:04:03
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.User.Dto
{
    public class ModifyAttestationReqDto:ModifyMobileReqDto
    {
        public int Type { get; set; }
        public string TrueName { get; set; }
        public string BLicenceURL { get; set; }
        public string IdentityNo { get; set; }
        public string IDCardFrontURL { get; set; }
        public int Sex { get; set; }
        public int CheckCode { get; set; }
    }
}
