/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.User.Dto
* 类 名 称 ：UpdateUserIdDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/24 18:47:45
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.User.Dto
{
    public class AppUpdateUserIdDto
    {
        public int Source { get; set; }
        public int OldUserId { get; set; }
        public int NewUserId { get; set; }
    }
}
