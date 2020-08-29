using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.Media
{
    public class RespWeiBoBindingsDto
    {
        public int MediaId { get; set; }
        public string HeadImg { get; set; }
        public string Sex { get; set; }
        public int FansCount { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public decimal Forward { get; set; }//转发
        public decimal Direct { get; set; }//直发
        [JsonIgnore]
        public string PricesInfo { get; set; }

    }
}
