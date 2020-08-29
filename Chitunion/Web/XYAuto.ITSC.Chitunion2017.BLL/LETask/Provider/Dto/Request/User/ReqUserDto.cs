using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.User
{
    public class ReqUserDto
    {
        [Necessary(MtName = "Mobile")]
        public string Mobile { get; set; }
        [Necessary(MtName = "MobileCode")]
        public string MobileCode { get; set; }

        public int ProvinceId { get; set; } = -2;
        public int CityId { get; set; } = -2;
        public string Address { get; set; }
    }
}
