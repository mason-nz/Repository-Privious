using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Entities;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.QrLogin
{
    public class ReqSimulationLoginDto
    {
        public string WeixinAppId { get; set; }
        public string WeixinAppSecret { get; set; }
        public string WeixinOpendId { get; set; }

        public string Ticket { get; set; }

        public string EventKey { get; set; }

        public ResponseMessageText ResponseMessageText { get; set; }
    }
}
