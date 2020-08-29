using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.CarSerialLabel.DTO.ResponseDto.V1_2_4
{
    public class ResViewBatchCarDto
    {
        public int MasterId { get; set; } = -2;
        public int BrandID { get; set; } = -2;
        public int SerialID { get; set; } = -2;
        public string MasterName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string SerialName { get; set; } = string.Empty;
        public string OperateUser { get; set; } = string.Empty;
        public string AuditUser { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public DateTime AuditTime { get; set; } = new DateTime(1900, 1, 1);
        public MediaLabel.DTO.ResponseDto.V1_2_4.IPCategory IPLabel { get; set; }
        public MediaLabel.DTO.ResponseDto.V1_2_4.Operateinfo OperateInfo { get; set; }
        public MediaLabel.DTO.ResponseDto.V1_2_4.Operateinfo AuditInfo { get; set; }
        public bool IpIsSame { get; set; } = false;
    }
}
