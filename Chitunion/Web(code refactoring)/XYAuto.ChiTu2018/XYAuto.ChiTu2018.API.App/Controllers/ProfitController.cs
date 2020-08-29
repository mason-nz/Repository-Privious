using System;
using System.Collections.Generic;
using System.Web.Http;
using XYAuto.ChiTu2018.API.App.Common;
using XYAuto.ChiTu2018.API.App.Filter;
using XYAuto.ChiTu2018.API.App.Models;
using XYAuto.ChiTu2018.Service.App.Profiles;
using XYAuto.ChiTu2018.Service.App.Profiles.Dto;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.API.App.Controllers
{
    [CrossSite]
    public class ProfitController : ApiController
    {
        /// <summary>
        /// 获取收益信息列表
        /// </summary>
        /// <param name="TopCount">查询数量</param>
        /// <param name="RowNum">排序ID</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetProfitList(int TopCount = 20, int RowNum = 0)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = AppProfitService.Instance.GetProfitList(new AppProfitQueryDto { PageIndex = RowNum, PageSize = TopCount, UserID = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID() });
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Info("[ProfitController]*****GetProfitList ->RowNum:" + RowNum + ",查询收益列表失败:" + ex.Message);
            }
            return Util.GetJsonDataByResult(dic, "Success");
        }
    }
}
