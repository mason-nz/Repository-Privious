using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Common;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [CrossSite]
    public class IncomeManageController : ApiController
    {
        /// <summary>
        /// 根据用户ID获取收益明细列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetIncomeDetailModelList([FromUri]QueryWithdrawalsArgs query)
        {
            query.UserID = ITSC.Chitunion2017.Common.UserInfo.GetLoginUser().UserID;
            var ResultNum = WithdrawalsBll.Instance.GetIncomeDetailModelList(query);
            return Util.GetJsonDataByResult(ResultNum);
        }
    }
}
