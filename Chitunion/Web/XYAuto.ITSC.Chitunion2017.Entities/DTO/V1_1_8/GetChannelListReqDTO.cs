using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class GetChannelListReqDTO
    {
        public GetChannelListReqDTO()
        {
            this.PageIndex = 1;
            this.PageSize = Entities.Constants.Constant.PageSize;
        }

        public string ChannelName { get; set; }
        public int Status { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
