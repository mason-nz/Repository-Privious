using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    //V1_1
    public class GetMediaListFReqDTO
    {
        public int MediaType { get; set;}
        public string Key { get; set; }
        private int categoryid = Constants.Constant.INT_INVALID_VALUE;
        public int CategoryID {
            get { return categoryid; }
            set { categoryid = value; }
        }
        public string FansCount { get; set; }
        public string Price { get; set; }
        private int isverify = Constants.Constant.INT_INVALID_VALUE;
        public int IsVerify {
            get { return isverify; }
            set { isverify = value; }
        }
        private bool canreceive = false;
        public bool CanReceive {
            get { return canreceive; }
            set { canreceive = value; }
        }
        public int OrderBy { get; set; }
        private int pagesize = Constants.Constant.PageSize;
        public int PageSize {
            get { return pagesize; }
            set { pagesize = value; }
        }
        private int pageindex = 1;
        public int PageIndex {
            get { return pageindex; }
            set { pageindex = value; }
        }
    }
}
