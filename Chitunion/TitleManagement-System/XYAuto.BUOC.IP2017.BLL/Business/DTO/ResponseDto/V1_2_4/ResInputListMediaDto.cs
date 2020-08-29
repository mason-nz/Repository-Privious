using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto.V1_2_4
{
    public class ResInputListMediaDto
    {
        public int MediaType { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public string HeadImg { get; set; } = string.Empty;
        public string HomeUrl { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int StatisticsCount { get; set; } = 0;
        public int ReadCount { get; set; } = 0;
        public int BatchMediaID { get; set; } = -2;
        public List<ResOperateinfo> OperateInfo { get; set; }
    }
    public class ResOperateinfo
    {
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
    }
}
