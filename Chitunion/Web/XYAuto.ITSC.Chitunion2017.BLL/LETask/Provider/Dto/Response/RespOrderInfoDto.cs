using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.AdOrder;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response
{
    public class RespOrderInfoDto : RespOrderDto
    {

        public string OrderUrl { get; set; }       //专属链接

        public string PasterUrl { get; set; }//图片地址
        
    }
}
