using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BitAuto.DSC.OASysRightManager2016.Common.WebAPI;
using System.Data;
using BitAuto.DSC.APPReport2016.WebAPI.Common;

namespace BitAuto.DSC.APPReport2016.WebAPI.Controllers
{
    [BitAuto.DSC.APPReport2016.WebAPI.Filter.LoginAuthorize(CheckReportRight = "SYS020MOD1008")]
    public class ppgghzController : ApiController
    {
        /// 文字中的信息获取
        /// <summary>
        /// 文字中的信息获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[LoginAuthorize(IsCheckIP = true, IsCheckLogin = true)]
        public HttpResponseMessage GetBrandOverViewData()
        {
            DataSet ds = BLL.BrandAd.Instance.GetBrandOverViewData();
            if (ds != null && ds.Tables.Count == 2)
            {
                var dataObj = new
                {
                    summaryInfo = new
                    {
                        Year = ds.Tables[0].Rows[0]["Year"],
                        Count = ds.Tables[0].Rows[0]["Count"],
                        Amount = ds.Tables[0].Rows[0]["Amount"]
                    },
                    rankInfo = new
                    {
                        Year = ds.Tables[1].Rows[0]["Year"],
                        Top10Amount = ds.Tables[1].Rows[0]["Top10Amount"],
                        Top10Percent = ds.Tables[1].Rows[0]["Top10Percent"],
                        Top40Percent = ds.Tables[1].Rows[0]["Top40Percent"]
                    }
                };
                return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(dataObj);
            }
            else
            {
                var dataObj = new
                  {
                      summaryInfo = new
                      {
                          Year = 0,
                          Count = 0,
                          Amount = 0
                      },
                      rankInfo = new
                      {
                          Year = 0,
                          Top10Amount = 0,
                          Top10Percent = 0,
                          Top40Percent = 0
                      }
                  };
                return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(ds);
            }
        }
        /// 柱图信息获取
        /// <summary>
        /// 柱图信息获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[LoginAuthorize(IsCheckIP = true, IsCheckLogin = true)]
        public HttpResponseMessage GetCooperateBarChartData()
        {
            DataTable dt = BLL.BrandAd.Instance.GetBrandBarData();
            CompareBarData amountBarData = Common.Common.CreateCompareBarData(dt);
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(amountBarData);
        }
        /// 获取表格数据
        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        [HttpGet]
        //[LoginAuthorize(IsCheckIP = true, IsCheckLogin = true)]
        public HttpResponseMessage GetCooperateBrandRanklist(string orderBy, int pageIndex, int pageSize, int lastno)
        {
            int totalCount = 0;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            DataTable dt = BLL.BrandAd.Instance.GetBrandRankData(orderBy, pageIndex, pageSize, out totalCount);
            int newlastno = BLL.Util.SetNoForDataTable(ref dt, orderBy, totalCount, lastno);
            var dataObj = new { pageIndex = pageIndex, totalCount = totalCount, data = dt, lastno = newlastno };
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(dataObj);
        }
    }
}
