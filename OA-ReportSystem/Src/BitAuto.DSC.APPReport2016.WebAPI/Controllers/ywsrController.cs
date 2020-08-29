using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;
using BitAuto.DSC.OASysRightManager2016.Common.WebAPI;
using System.Data;
using BitAuto.DSC.APPReport2016.WebAPI.Common;

namespace BitAuto.DSC.APPReport2016.WebAPI.Controllers
{ 
    [BitAuto.DSC.APPReport2016.WebAPI.Filter.LoginAuthorize(CheckReportRight = "SYS020MOD1006")]
    public class ywsrController : ApiController
    {
        //
        // GET: /ywsr/
        /// <summary>
        /// 获取业务收入饼图
        /// </summary>
        /// <param name="operationYear"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDataByYear(int? operationYear)
        {

            int retYear = DateTime.Now.Year;        
            if (operationYear.HasValue)
            {
                retYear = operationYear.Value;
            }
            else
            {
                retYear = BLL.Amount.Instance.GetLatestYear();//DB中查询出来的最新年份
            }
            List<object> list = BLL.Amount.Instance.GetDataByYear(retYear);

            decimal totalAmount = BLL.Amount.Instance.GetTotalAmountByYear(retYear);
            bool preTotal = BLL.Amount.Instance.CheckHasDataPreYearByYear(retYear);
            bool nextTotal = BLL.Amount.Instance.CheckHasDataNextYearByYear(retYear);

            var retData = new { retYear = retYear, details = list,preTotal=preTotal,nextTotal=nextTotal, total = totalAmount };
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(new RetJsonModel() { Success = true, ErrorMsg = "", RetData = retData });

        }


        ///获取业务收入柱状图数据
        /// <summary>
        /// 获取业务收入柱状图数据
        /// </summary>
        /// <param name="operationYear"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        /// http://localhost:42241/api/ywsr/GetDataByYearAndDataType?operationYear=2015&dataType=80001
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDataByYearAndDataType(int operationYear, int dataType)
        {
            DataTable dt = BLL.Amount.Instance.GetAmountBarData(operationYear, dataType);
            CompareBarData amountBarData = Common.Common.CreateCompareBarData(dt);
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(amountBarData);
        }
    }
}
