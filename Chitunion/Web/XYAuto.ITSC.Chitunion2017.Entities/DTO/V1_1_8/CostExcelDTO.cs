using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class CostExcelDTO
    {
        public CostExcelDTO()
        {
            this.ImportRowList = new List<ImportCostRow>();
        }

        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public DateTime CooperateBeginDate { get; set; }
        public DateTime CooperateEndDate { get; set; }
        public List<ImportCostRow> ImportRowList { get; set; }
    }

    public class ImportCostRow
    {
        public ImportCostRow()
        {
            this.PriceList = new List<ImportCostPrice>();
            this.DeleteDetailIDs = new List<int>();
            this.ProvinceID = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.CityID = Entities.Constants.Constant.INT_INVALID_VALUE;
        }
        /// <summary>
        /// 附表媒体 0:新增 >0:修改和不变
        /// </summary>
        public int MediaID { get; set; }
        public bool MediaIsOld { get; set; }
        /// <summary>
        /// 成本ID 0:新增 >0:修改
        /// </summary>
        public int CostID { get; set; }
        public string WxNumber { get; set; }
        public string WxName { get; set; }
        public int FansCount { get; set; }
        public Dictionary<int,string> CategoryDict { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public int LevelType { get; set; }
        public string LevelTypeName { get; set; }
        public Dictionary<int, string> RemarkDict { get; set; }
        public decimal OriginalPrice { get; set; }
        public List<ImportCostPrice> PriceList { get; set; }

        /// <summary>
        /// 要删除的成本广告位ID集合
        /// </summary>
        public List<int> DeleteDetailIDs { get; set; }
    }

    public class ImportCostPrice {

        /// <summary>
        /// 成本广告位ID 0:新增 >0:修改
        /// </summary>
        public int DetailID { get; set; }
        /// <summary>
        /// 6+
        /// </summary>
        public int ADPosition1 { get; set; }
        /// <summary>
        /// 7002
        /// </summary>
        public int ADPosition2 { get; set; }
        /// <summary>
        /// 8+
        /// </summary>
        public int ADPosition3 { get; set; }
        public string ADPosition { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }

    }
}
