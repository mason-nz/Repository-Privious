using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.InCome
{
    public class RespInComeByMediaOwnDto
    {
        public int OrderId { get; set; }
        public string OrderTypeName { get; set; }
        public string OrderName { get; set; }
        public int TaskId { get; set; }
        public int MaterielId { get; set; }
        public string UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime IncomeTime { get; set; }
        public string MaterialUrl { get; set; }
    }
}
