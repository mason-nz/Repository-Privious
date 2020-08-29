/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.Wechat.Dto
* 类 名 称 ：Class1
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/14 11:14:23
********************************/

using System.Collections.Generic;

namespace XYAuto.ChiTu2018.Service.App.Sign.Dto
{
    public class AppRespWeChatSignDto
    {
        public List<string> SignDayList { get; set; }
        public bool IsSign { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
