using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.BatchMedia.DTO.RequestDto.V1_2_4
{
    public class ReqBatchMediaSubmitDto
    {
        public int MediaType { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string HeadImg { get; set; }
        public List<ReqBatchMediaSubmitTitleDto> Category { get; set; }
        public List<ReqBatchMediaSubmitTitleDto> MarketScene { get; set; }
        public List<ReqBatchMediaSubmitTitleDto> DistributeScene { get; set; }
        public List<ReqBatchMediaSubmitIplabelDto> IPLabel { get; set; }
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (!Enum.IsDefined(typeof(Entities.ENUM.ENUM.EnumMediaType), MediaType))
                sb.Append($"媒体类型错误：{MediaType}!");

            if (MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.微信)
            {
                if (string.IsNullOrEmpty(Number))
                    sb.Append($"参数：Number为必填项!");
            }
            else
            {
                if (string.IsNullOrEmpty(Name))
                    sb.Append($"参数：Name为必填项!");
            }

            if (!BLL.MediaLabel.MediaLabel.Instance.IsExistsLabelByMedia(MediaType, MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.微信 ? Number : Name))
            {
                if (MarketScene == null || MarketScene.Count == 0)
                    sb.Append($"参数：市场场景 为必填项!");

                if (DistributeScene == null || DistributeScene.Count == 0)
                    sb.Append($"参数：分发场景 为必填项!");
            }


            if (IPLabel == null || IPLabel.Count == 0)
                sb.Append($"参数：IP标签 为必填项!");

            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }

    public class ReqBatchMediaSubmitTitleDto
    {
        public int DictId { get; set; }
        public string DictName { get; set; }
    }
    
    public class ReqBatchMediaSubmitIplabelDto
    {
        public int DictId { get; set; }
        public string DictName { get; set; }
        public List<ReqBatchMediaSubmitSubipDto> SubIP { get; set; }
    }

    public class ReqBatchMediaSubmitSubipDto
    {
        public int DictId { get; set; }
        public string DictName { get; set; }
        public List<ReqBatchMediaSubmitTitleDto> Label { get; set; }
    }    
}
