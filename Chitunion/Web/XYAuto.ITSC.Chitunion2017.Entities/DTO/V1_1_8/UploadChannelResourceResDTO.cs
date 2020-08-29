using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class UploadChannelResourceResDTO
    {
        public UploadChannelResourceResDTO()
        {
            this.ErrorList = new List<string>();
            this.Data = new UploadSuccessRes();
        }

        public List<string> ErrorList { get; set; }
        public UploadSuccessRes Data { get; set; }

    }

    public class UploadSuccessRes
    {
        public UploadSuccessRes()
        {
            this.UpdateMedias = new UploadSuccessMediaObj();
            this.NewMedias = new UploadSuccessMediaObj();
            this.OldMedias = new UploadSuccessMediaObj();
            this.UpdatePrice = new UploadSuccessPriceObj();
            this.NewPrice = new UploadSuccessPriceObj();
            this.OldPrice = new UploadSuccessPriceObj();
        }

        public int TotalCount { get; set; }
        public UploadSuccessMediaObj UpdateMedias { get; set; }
        public UploadSuccessMediaObj NewMedias { get; set; }
        public UploadSuccessMediaObj OldMedias { get; set; }

        public UploadSuccessPriceObj UpdatePrice { get; set; }
        public UploadSuccessPriceObj NewPrice { get; set; }
        public UploadSuccessPriceObj OldPrice { get; set; }

    }

    public class UploadSuccessMediaObj
    {
        public UploadSuccessMediaObj()
        {
            this.List = new List<UploadSuccessMediaItem>();
        }
        public int Total { get; set; }
        public List<UploadSuccessMediaItem> List { get; set; }
    }

    public class UploadSuccessPriceObj
    {
        public UploadSuccessPriceObj()
        {
            this.List = new List<UploadSuccessPriceItem>();
        }

        public int Total { get; set; }
        public List<UploadSuccessPriceItem> List { get; set; }
    }

    public class UploadSuccessMediaItem
    {
        public string WxNumber { get; set; }
        public string WxName { get; set; }
        public int FansCount { get; set; }
        public string CategoryName { get; set; }
        public string AreaName { get; set; }
        public string LevelTypeName { get; set; }
        public string OrderRemarkName { get; set; }

        public string OldWxName { get; set; }
        public int OldFansCount { get; set; }
        public string OldCategoryName { get; set; }
        public string OldAreaName { get; set; }
        public string OldLevelTypeName { get; set; }
        public string OldOrderRemarkName { get; set; }
    }

    public class UploadSuccessPriceItem
    {
        public string WxNumber { get; set; }
        public string WxName { get; set; }
        public string ADPosition { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }

        public decimal OldCostPrice { get; set; }
        public decimal OldSalePrice { get; set; }
    }
}
