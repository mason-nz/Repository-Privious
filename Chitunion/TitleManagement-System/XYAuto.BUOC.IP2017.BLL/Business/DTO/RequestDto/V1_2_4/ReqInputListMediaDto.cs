using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4
{
    public class ReqInputListMediaDto : CreateQueryBase
    {
        public int MediaType { get; set; } = -2;
        public int HasArticleType { get; set; } = -2;
        public int DictId { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public int LabelStatus { get; set; } = -2;
        public int SelfDoBusiness { get; set; } = -2;
        public int OrderBy { get; set; } = -2;
        [JsonIgnore]
        public int CurrentUserID { get; set; } = -2;        
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (!Enum.IsDefined(typeof(Entities.ENUM.ENUM.EnumMediaType), MediaType))
                sb.Append($"媒体类型错误：{MediaType}!");

            if (Name == "null")
                sb.Append($"参数Name错误不能为null!");

            if (!Enum.IsDefined(typeof(Query.V1_2_4.Enum.EnumHasArticleType), HasArticleType) && HasArticleType != -2)
                sb.Append($"是否有文章类型错误：{HasArticleType}!");

            if ((OrderBy == 2001 || OrderBy == 2002) && MediaType != (int)Entities.ENUM.ENUM.EnumMediaType.微信)
                sb.Append($"参数错误:只有微信可按头条阅读数排序!");
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
