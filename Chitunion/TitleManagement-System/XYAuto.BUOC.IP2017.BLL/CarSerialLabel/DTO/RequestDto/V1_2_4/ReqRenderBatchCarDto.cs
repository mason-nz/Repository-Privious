using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.CarSerialLabel.DTO.RequestDto.V1_2_4
{
    public class ReqRenderBatchCarDto
    {
        public int BrandID { get; set; } = -2;
        public int SerialID { get; set; } = -2;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (BrandID == -2)
                sb.Append($"参数BrandID为必填项!");

            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
