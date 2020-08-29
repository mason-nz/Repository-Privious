using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response
{
    public class RespWeiXinInfoDto : RespWeiXinBindingsDto
    {
        public int CategoryId { get; set; }
        public int FansCount { get; set; }
        public decimal FansMalePer { get; set; }
        public decimal FansFemalePer { get; set; }
        public Overlayarea OverlayArea { get; set; }
    }
}
