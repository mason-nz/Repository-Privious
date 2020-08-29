using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Channel;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class GetChannelInfoResDTO : ChannelInfo
    {
        public List<PolicyInfo> PolicyList { get; set; }
    }
}
