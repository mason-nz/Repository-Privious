using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4
{
    public class ReqArticleQueryOrReciveDto : CreateQueryBase
    {
        public bool IsRecive { get; set; } = false;
        public DateTime StartDate { get; set; } = new DateTime(1900, 1, 1);
        public DateTime EndDate { get; set; } = new DateTime(1900, 1, 1);
        public int Resource { get; set; } = -2;
        public string Number { get; set; } = string.Empty;
        public int ArticleCount { get; set; } = 0;
        public string ArticleIds { get; set; } = string.Empty;
        public int CurrentUserID { get; set; } = -2;

        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (!Enum.IsDefined(typeof(Entities.ENUM.ENUM.EnumResourceType), Resource))
                sb.Append($"媒体类型错误：{Resource}!");

            if (string.IsNullOrEmpty(Number))
                sb.Append($"参数：Number为必填项!");

            if (Resource != (int)Entities.ENUM.ENUM.EnumResourceType.微信
                && Resource != (int)Entities.ENUM.ENUM.EnumResourceType.搜狐
                && Resource != (int)Entities.ENUM.ENUM.EnumResourceType.今日头条)
                sb.Append($"渠道类型错误：必须 微信&搜狐&头条!");

            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
