using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.LeTask
{
    public class RespUserStatDto
    {
        public int UserId { get; set; }
        public string UserHeadImg { get; set; }
        public string UserName { get; set; }
        public decimal EarningsPrice { get; set; }
        public int OrderCount { get; set; }
        public int BindingsCount { get; set; }

    }
}
