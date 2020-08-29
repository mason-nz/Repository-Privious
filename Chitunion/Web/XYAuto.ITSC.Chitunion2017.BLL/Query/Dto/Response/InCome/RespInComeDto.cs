using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.InCome
{
    public class RespInComeDto
    {

        public int OrderId { get; set; }
        public string OrderTypeName { get; set; }
        public string OrderName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime IncomeTime { get; set; }

    }
}
