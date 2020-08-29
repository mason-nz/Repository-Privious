
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class ReqUserDetailDto
    {
        public int Type { get; set; }
        public string TrueName { get; set; }
        public string BLicenceURL { get; set; }
        public string IdentityNo { get; set; }
        public string IDCardFrontURL { get; set; }
    }
}
