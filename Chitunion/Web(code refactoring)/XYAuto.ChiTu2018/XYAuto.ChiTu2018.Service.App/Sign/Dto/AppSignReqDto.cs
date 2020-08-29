/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.Wechat.Dto
* 类 名 称 ：SignReqDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/16 15:55:36
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.Entities.Enum.Wechat;

namespace XYAuto.ChiTu2018.Service.App.Sign.Dto
{
    public class AppSignReqDto
    {
        public int ActivityType { get; set; } = -2;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (!Enum.IsDefined(typeof(ActivityTypeEnum), ActivityType))
                sb.Append($"参数ActivityType：{ActivityType}错误!");
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
