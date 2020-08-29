using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.QrLogin
{
   public class ReqVerifyQrLoginDto
    {
        [Necessary(MtName = "Ticket")]

        public string Ticket { get; set; }
    }
}
