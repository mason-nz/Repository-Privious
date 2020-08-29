using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto
{
    public class ReqKeyValueDto
    {
        public ReqMessageType t { get; set; }
        public string v { get; set; }
    }

    public enum ReqMessageType
    {
        无 = Entities.Constants.Constant.INT_INVALID_VALUE,
        邀请 = 1,
        Pc端登录 = 2,

        场景 = 3
    }
}
