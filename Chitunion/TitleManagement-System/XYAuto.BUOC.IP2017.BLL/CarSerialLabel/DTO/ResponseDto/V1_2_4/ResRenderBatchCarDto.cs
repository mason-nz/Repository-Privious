using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.CarSerialLabel.DTO.ResponseDto.V1_2_4
{
    public class ResRenderBatchCarDto
    {
        public int MasterId { get; set; } = -2;
        public int BrandID { get; set; } = -2;
        public int SerialID { get; set; } = -2;
        public string MasterName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string SerialName { get; set; } = string.Empty;
        public List<MediaLabel.DTO.ResponseDto.V1_2_4.ResBatchMediaIplabelDto> IPLabel { get; set; }
    }
}
