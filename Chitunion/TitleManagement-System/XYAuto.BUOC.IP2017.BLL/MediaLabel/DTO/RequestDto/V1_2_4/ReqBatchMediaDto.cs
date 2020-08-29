using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.MediaLabel.DTO.RequestDto.V1_2_4
{
    public class ReqBatchMediaDto
    {
        public int MediaType { get; set; } = -2;
        public string NumberOrName { get; set; } = string.Empty;
        public string HomeUrl { get; set; } = string.Empty;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (!Enum.IsDefined(typeof(Entities.ENUM.ENUM.EnumMediaType), MediaType))
                sb.Append($"媒体类型错误：{MediaType}!");

            if(string.IsNullOrEmpty(NumberOrName))
                sb.Append($"参数：NumberOrName为必填项!");

            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
