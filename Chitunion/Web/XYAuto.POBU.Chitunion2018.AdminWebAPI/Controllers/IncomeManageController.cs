using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Common;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Filter;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI.Controllers
{
   [CrossSite]
    public class IncomeManageController : ApiController
    {
        /// <summary>
        /// 获取收益统计列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetWithdrawalsStatisticsList([FromUri]QueryWithdrawalsArgs query)
        {
            var ResultNum = WithdrawalsBll.Instance.GetWithdrawalsStatisticsList(query);
            return Util.GetJsonDataByResult(ResultNum);
        }

        /// <summary>
        /// 根据用户ID获取收益明细列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetIncomeDetailModelList([FromUri]QueryWithdrawalsArgs query)
        {
            var ResultNum = WithdrawalsBll.Instance.GetIncomeDetailModelList(query);
            return Util.GetJsonDataByResult(ResultNum);
        }
    }
}
