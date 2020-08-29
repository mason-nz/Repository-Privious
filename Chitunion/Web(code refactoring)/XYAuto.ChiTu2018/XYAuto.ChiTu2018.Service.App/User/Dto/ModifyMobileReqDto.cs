/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.User.Dto
* 类 名 称 ：ModifyMobileReqDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/24 11:04:25
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.User.Dto
{
    public class ModifyMobileReqDto
    {
        public string Mobile { get; set; }
        public string AccountName { get; set; }
        public int AccountType { get; set; }
    }
}
