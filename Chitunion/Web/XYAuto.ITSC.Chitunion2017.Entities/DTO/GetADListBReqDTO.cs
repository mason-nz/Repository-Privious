using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetADListBReqDTO
    {
        public int MediaType { get; set; }
        public int MediaID { get; set; }
        public string MediaName { get; set;}
        public string ADName { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        private int pubstatus = Constants.Constant.INT_INVALID_VALUE;
        public int PubStatus {
            get { return pubstatus; }
            set { pubstatus = value; }
        }

        public string UserName { get; set; }
        /// <summary>
        /// 是否为AE领导页请求
        /// </summary>
        public bool IsAE { get; set; }

        private int pagesize = Constants.Constant.PageSize;
        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value; }
        }
        private int pageindex = 1;
        public int PageIndex
        {
            get { return pageindex; }
            set { pageindex = value; }
        }
    }
}
