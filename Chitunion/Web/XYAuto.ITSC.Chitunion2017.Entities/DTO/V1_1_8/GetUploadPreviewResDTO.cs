using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class GetUploadPreviewResDTO
    {
        public GetUploadPreviewResDTO()
        {
            this.ColumnNameList = new List<ColumnNameItem>();
            this.List = new List<PreviewItem>();
        }

        public List<ColumnNameItem> ColumnNameList { get; set; }
        public List<PreviewItem> List { get; set; }
    }

    public class ColumnNameItem
    {
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public string CooperateDate { get; set; }
    }

    public class PreviewItem
    {
        public PreviewItem()
        {
            this.PriceList = new List<decimal>();
            this.CooperateBeginDate = Constants.Constant.DATE_INVALID_VALUE;
            this.CooperateEndDate = Constants.Constant.DATE_INVALID_VALUE;
        }

        public int MediaID { get; set; }
        public string WxNumber { get; set; }
        public string WxName { get; set; }
        public int ADPosition1 { get; set; }
        public int ADPosition2 { get; set; }
        public int ADPosition3 { get; set; }
        public string ADPosition { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime CooperateBeginDate { get; set; }
        public DateTime CooperateEndDate { get; set; }
        public string CooperateDate { get; set; }
        public List<decimal> PriceList { get; set; }
    }

    public class DateItem
    {
        public bool IsBeginDate { get; set; }
        public DateTime Date { get; set; }
    }

}
