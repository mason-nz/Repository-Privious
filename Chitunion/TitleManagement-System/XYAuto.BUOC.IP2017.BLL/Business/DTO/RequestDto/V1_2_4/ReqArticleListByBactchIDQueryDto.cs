using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4
{
    public class ReqArticleListByBactchIDQueryDto : CreateQueryBase
    {
        public int BatchMediaID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (BatchMediaID == -2)
                sb.Append($"参数BatchMediaID为必填项!");            

            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
