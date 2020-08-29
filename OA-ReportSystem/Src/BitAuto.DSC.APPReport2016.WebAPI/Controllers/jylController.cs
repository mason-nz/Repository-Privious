using System;
using System.Web.Http;
using System.Net.Http;

namespace BitAuto.DSC.APPReport2016.WebAPI.Controllers
{    
    [BitAuto.DSC.APPReport2016.WebAPI.Filter.LoginAuthorize(CheckReportRight = "SYS020MOD1004")]
    public class jylController : ApiController
    {
        //
        // GET: /jyl/

        /// <summary>
        /// 获取最新日期的交易量片图
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetData()
        {
            var dataList = BLL.Trades.Instance.GetRetrunObjectData();

            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(new RetJsonModel() { Success = true, ErrorMsg = "", RetData = dataList });
        }



        /// <summary>
        /// 获取交易量数据的线图
        /// </summary>
        /// <param name="operationDate"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDataTrend()
        {
            var dataList = BLL.Trades.Instance.GetDataTrendByDate();

            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(new RetJsonModel() { Success = true, ErrorMsg = "", RetData = dataList });
        }
    }
}
