using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto.V1_2_4
{    
    public class ResBatchListMediaDto
    {
        public int BatchMediaID { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public string HeadImg { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public int MediaType { get; set; } = -2;
        public bool IsSelfDo { get; set; } = false;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public DateTime AuditTime { get; set; } = new DateTime(1900, 1, 1);
        public string AuditUser { get; set; }
        public int Status { get; set; } = -2;
    }

}
