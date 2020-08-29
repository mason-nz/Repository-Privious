using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.User
{
    public class ReqUserPasswordDto
    {
        [Necessary(MtName = "Password")]
        public string Password { get; set; }
        [Necessary(MtName = "PasswordAgain")]
        public string PasswordAgain { get; set; }
    }
}
