/********************************
* 项目名称 ：XYAuto.ITSC.Chitunion2017.Entities.WeChat
* 类 名 称 ：VerifyUserModel
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/6/12 17:50:17
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.WeChat
{
    public class VerifyUserModel
    {
        public int UserID { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Source { get; set; } = 103009;
    }
}
