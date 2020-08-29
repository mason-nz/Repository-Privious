using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4
{
    public class ReqInputListCarDto : CreateQueryBase
    {
        public int MasterId { get; set; } = -2;
        public int BrandID { get; set; } = -2;
        public int LabelStatus { get; set; } = -2;
        public DateTime StartDate { get; set; } = new DateTime(1900, 1, 1);
        public DateTime EndDate { get; set; } = new DateTime(1900, 1, 1);
        public int Status { get; set; } = -2;
        public int CurrentUserID { get; set; } = -2;
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            //if (MasterId == -2)
            //    sb.Append($"参数MasterId为必填项!");            
            //to do
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
