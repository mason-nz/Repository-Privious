using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.WechatSign
{
    public class WxSignRespDTO
    {
        public List<string> SignDayList { get; set; }
        public bool IsSign { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
