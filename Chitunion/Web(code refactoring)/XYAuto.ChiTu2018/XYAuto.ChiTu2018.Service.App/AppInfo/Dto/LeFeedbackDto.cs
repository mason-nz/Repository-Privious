/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.AppInfo.Dto
* 类 名 称 ：LeFeedbackDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/23 17:16:46
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.AppInfo.Dto
{
    public class LeFeedbackDto
    {
        public int RecID { get; set; }

        public int UserID { get; set; }

        public string OpinionText { get; set; }

        public string CreateTime { get; set; }

        public string ReplyText { get; set; }

        public string ReplyTime { get; set; }
    }
}
