using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto
{
    public class CreateQueryBase
    {
        //public DateTime StartTime { get; set; } = new DateTime(1900, 1, 1);
        //public DateTime EndTime { get; set; } = new DateTime(1900, 1, 1);
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
