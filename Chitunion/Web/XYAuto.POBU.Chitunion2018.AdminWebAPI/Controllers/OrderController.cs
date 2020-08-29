using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.ChannelStat;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.ChannelStat;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.InCome;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Common;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Filter;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI.Controllers
{
    [CrossSite]
    public class OrderController : BaseApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:财务管理-媒体主收入记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetIncomeList([FromUri]ReqInComeByMediaOwnDto request)
        {
            var jsonResult = new Common.JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            request.UserId = GetUserInfo.UserID;
            jsonResult.Result = new InComeByMediaOwnQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:财务管理-渠道月结数据列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetMonthlyList([FromUri]ReqChannelStatDto request)
        {
            var jsonResult = new Common.JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            request.UserId = GetUserInfo.UserID;
            jsonResult.Result = new ChannelStatMonthQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:财务管理-渠道月结数据列表-汇总年月选择
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetStatMonthlySelect()
        {
            var jsonResult = new Common.JsonResult();

            jsonResult.Result = new
            {
                List =
                    new ChannelStatMonthProvider(new ConfigEntity(), new ReqChannelStatMonthPayDto())
                        .GetChannelStatMonths()
            };
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:财务管理-渠道月结数据列表-月结付款操作
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true , CheckModuleRight = "SYS008BUT100301")]
        public Common.JsonResult PayMonthly([FromBody] ReqChannelStatMonthPayDto request)
        {
            var jsonResult = new Common.JsonResult();

            var retValue = new ChannelStatMonthProvider(new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID
            }, request).Pay();
            return jsonResult.GetReturn(retValue);
        }
    }
}