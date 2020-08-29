using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto.V1_2_4
{
    public class ResInputListCarDto
    {
        public int MasterId { get; set; } = -2;
        public int BrandID { get; set; } = -2;
        public int SerialID { get; set; } = -2;
        public string BrandName { get; set; } = string.Empty;
        public string SerialName { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public List<ResInputListCarOperateinfoDto> OperateInfo { get; set; }
        public int BatchMediaID { get; set; } = -2;
        public string MasterName { get; set; } = string.Empty;
        public DateTime AuditTime { get; set; } = new DateTime(1900, 1, 1);
        public string AuditUser { get; set; } = string.Empty;
    }

    public class ResInputListCarOperateinfoDto
    {
        public string UserName { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
    }

}
