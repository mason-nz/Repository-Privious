using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Publish
{
    public class AppPriceInfo
    {
        public int RecID { get; set; }
        public int PubID { get; set; }
        public int TemplateID { get; set; }
        public int MediaID { get; set; }
        public int ADStyle { get; set; }
        public string ADStyleName { get; set; }
        public int CarouselNumber { get; set; }

        private int saleplatform = Constants.Constant.INT_INVALID_VALUE;
        public int SalePlatform {
            get { return saleplatform; }
            set { saleplatform = value; }
        }
        public string SalePlatformName { get; set; }

        private int saletype = Constants.Constant.INT_INVALID_VALUE;
        public int SaleType {
            get { return saletype; }
            set { saletype = value; }
        }
        public string SaleTypeName { get; set; }

        private int salearea = Constants.Constant.INT_INVALID_VALUE;
        public int SaleArea {
            get { return salearea; }
            set { salearea = value; }
        }
        public string SaleAreaName { get; set; }

        public int ClickCount { get; set; }
        public int ExposureCount { get; set; }
        public decimal PubPrice_Holiday { get; set; }
        public decimal SalePrice_Holiday { get; set; }
        public decimal PubPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Status { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
