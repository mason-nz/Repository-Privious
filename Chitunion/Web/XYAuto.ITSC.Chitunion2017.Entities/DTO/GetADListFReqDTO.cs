using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetADListFReqDTO
    {
        public int MediaType { get; set; }
        public int MediaID { get; set; }
        public string Key { get; set; }
        public string Price { get; set; }

        private int orderby = Constants.Constant.INT_INVALID_VALUE;
        public int Orderby
        {
            get { return orderby; }
            set { orderby = value; }
        }

        private int categoryid = Constants.Constant.INT_INVALID_VALUE;
        public int CategoryID
        {
            get { return categoryid; }
            set { categoryid = value; }
        }

        private int saletype = Constants.Constant.INT_INVALID_VALUE;
        public int SaleType
        {
            get { return saletype; }
            set { saletype = value; }
        }

        private int cityid = Constants.Constant.INT_INVALID_VALUE;
        public int CityID
        {
            get { return cityid; }
            set { cityid = value; }
        }


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
