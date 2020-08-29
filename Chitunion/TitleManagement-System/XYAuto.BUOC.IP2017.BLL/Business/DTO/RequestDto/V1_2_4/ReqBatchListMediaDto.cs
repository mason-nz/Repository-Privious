using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4
{    
    public class ReqBatchListMediaDto : CreateQueryBase
    {
        public int MediaType { get; set; } = -2;
        public int DictId { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public int SelfDoBusiness { get; set; } = -2;
        public DateTime StartDate { get; set; } = new DateTime(1900, 1, 1);
        public DateTime EndDate { get; set; } = new DateTime(1900, 1, 1);
        public int CurrentUserID { get; set; } = -2;
        public int Status { get; set; } = -2;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (!Enum.IsDefined(typeof(Entities.ENUM.ENUM.EnumMediaType), MediaType) && MediaType != -2)
                sb.Append($"媒体类型错误：{MediaType}!");


            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
