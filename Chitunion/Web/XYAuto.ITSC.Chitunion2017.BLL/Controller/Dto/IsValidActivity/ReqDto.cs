using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Controller.Dto.IsValidActivity
{
    public enum EnumActivityType
    {
        邀请有礼=1,
        签到有礼=2
    }
    public class ReqDto
    {
        public int ActivityType { get; set; } = -2;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (!Enum.IsDefined(typeof(EnumActivityType), ActivityType))
                sb.Append($"参数ActivityType：{ActivityType}错误!");
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
