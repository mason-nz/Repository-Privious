using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetTwoBarCodeHistoryDto
    {
        public string OrderID { get; set; } = string.Empty;
        public string OrderName { get; set; } = string.Empty;
        public List<TwoBarCodeDetailDto> TwoBarCodeHistory { get; set; }
    }

    public class TwoBarCodeDetailDto
    {
        public int RecID { get; set; }
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public string MediaNumber { get; set; } = string.Empty;
        public string MediaName { get; set; } = string.Empty;
        public string HeadIconURL { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string TwoBarUrl { get; set; } = string.Empty;
    }
}