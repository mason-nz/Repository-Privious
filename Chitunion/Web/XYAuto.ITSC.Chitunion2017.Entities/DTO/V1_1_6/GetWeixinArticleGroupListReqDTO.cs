using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6
{
    public class GetWeixinArticleGroupListReqDTO
    {
        public GetWeixinArticleGroupListReqDTO()
        {
            this.PageIndex = 1;
            this.PageSize = Entities.Constants.Constant.PageSize;
        }

        public bool IsGoodTJ { get; set; }
        public string Key { get; set; }
        public int WxID { get; set; }
        public string WxName { get; set; }
        public string WxNumber { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
