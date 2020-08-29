using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using BitAuto.DSC.OASysRightManager2016.Common.WebAPI;

namespace BitAuto.DSC.APPReport2016.WebAPI.Controllers
{
    [BitAuto.DSC.APPReport2016.WebAPI.Filter.LoginAuthorize(CheckReportRight = "SYS020MOD1001")]
    public class yyrbController : ApiController
    {
        /// 查询运营日报界面卡片信息
        /// <summary>
        /// 查询运营日报界面卡片信息
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetPageData()
        {
            var dataList = BLL.Operation.Instance.GetReturnObjectData();
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(dataList);
        }
        /// 获取运营日报线图信息
        /// <summary>
        /// 获取运营日报线图信息
        /// </summary>
        /// <param name="operationDate"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDataTrend()
        {
            var dataList = BLL.Operation.Instance.GetDataTrendByDate();
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(dataList);
        }
    }
    /// JSON实体
    /// <summary>
    /// JSON实体
    /// </summary>
    public class RetJsonModel
    {
        public bool Success { get; set; }
        public string ErrorMsg { get; set; }
        public object RetData { get; set; }
    }
}
