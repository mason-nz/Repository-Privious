using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome
{
    public class ReqInComeByMediaOwnDto : ReqInComeDto
    {
        public string OrderId { get; set; }
        public string TaskId { get; set; }
        public string MaterielId { get; set; }
        public string UserName { get; set; }
        public int ChannelId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
    }
}
