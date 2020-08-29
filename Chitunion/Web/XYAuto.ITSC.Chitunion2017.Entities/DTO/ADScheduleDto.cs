using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class ADScheduleDto
    {
        public int ADDetailID { get; set; } = -2;
        public string OrderID { get; set; } = string.Empty;
        public string SubOrderID { get; set; } = string.Empty;
        public int MediaID { get; set; } = -2;
        public int PubID { get; set; } = -2;
        public DateTime BeginData { get; set; } = Constants.Constant.DATE_INVALID_VALUE;
        public DateTime EndData { get; set; }= Constants.Constant.DATE_INVALID_VALUE;
        public DateTime CreateTime { get; set; }= Constants.Constant.DATE_INVALID_VALUE;
        public int CreateUserID { get; set; } = -2;
    }
}
