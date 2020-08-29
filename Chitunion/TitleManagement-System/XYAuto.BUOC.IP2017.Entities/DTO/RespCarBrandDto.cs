using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.DTO
{
    public class RespCarBrandDto
    {
        public int BrandId { get; set; }
        public int MasterId { get; set; }
        public string Name { get; set; }
    }

    public class RespCarSerialDto : RespCarBrandDto
    {
        public int CarSerialId { get; set; }
        public string ShowName { get; set; }
    }
}
