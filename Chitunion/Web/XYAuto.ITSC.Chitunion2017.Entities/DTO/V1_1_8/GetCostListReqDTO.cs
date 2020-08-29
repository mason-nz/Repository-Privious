using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class GetCostListReqDTO
    {
        public GetCostListReqDTO()
        {
            this.MediaID = Constants.Constant.INT_INVALID_VALUE;
            this.ChannelID = Constants.Constant.INT_INVALID_VALUE;
            this.SaleStatus = Constants.Constant.INT_INVALID_VALUE;
            this.PageIndex = 1;
            this.PageSize = Entities.Constants.Constant.PageSize;
        }

        public int MediaID { get; set; }
        public int ChannelID { get; set; }
        public int SaleStatus { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
