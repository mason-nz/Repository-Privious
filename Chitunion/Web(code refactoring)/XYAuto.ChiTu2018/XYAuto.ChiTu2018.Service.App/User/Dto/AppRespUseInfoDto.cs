/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.User.Dto
* 类 名 称 ：AppRespUseInfoDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/24 18:57:31
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.User.Dto
{
    public class AppRespUseInfoDto
    {
        public string NickName { get; set; }
        public string HeadImgUrl { get; set; }
        public string Mobile { get; set; }
        public string TrueName { get; set; }
        public string AccountName { get; set; }
        public int AccountType { get; set; }
        public int Status { get; set; }
        public int IsFollow { get; set; }
        public int Sex { get; set; }
        public string IdentityNo { get; set; }
    }
}
