using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetPublishDetailForRecommendAddPDDto
    {
        public int PublishDetailID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public string MediaName { get; set; } = string.Empty;
        public string MediaNumber { get; set; } = string.Empty;
        public string HeadIconURL { get; set; } = string.Empty;
        public int FansCount { get; set; } = 0;
        public string ADPosition { get; set; } = string.Empty;
        public string CreateType { get; set; } = string.Empty;
        public int ADPositionID { get; set; } = -2;
        public int CreateTypeID { get; set; } = -2;
        public decimal SalePrice { get; set; } = 0;
        public decimal CostReferencePrice { get; set; } = 0;
        public decimal OriginalReferencePrice { get; set; } = 0;
        public int ChannelID { get; set; } = -2;
        public string ChannelName { get; set; } = string.Empty;
    }
}
