/********************************
* 项目名称 ：XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.VerifyUser
* 类 名 称 ：VerifyUserDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/6/12 19:10:50
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.VerifyUser
{
    public class VerifyUserDto
    {
        public bool OrderStatus { get; set; } = false;

        public bool IsNewUser { get; set; } = false;

        public string OrderUrl { get; set; }


        public int OrderNum { get; set; } = 0;
    }
}
