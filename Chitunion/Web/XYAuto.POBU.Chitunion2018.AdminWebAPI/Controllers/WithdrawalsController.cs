using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Withdrawals;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Common;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Filter;
using ReqWithdrawalsDto = XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Withdrawals.ReqWithdrawalsDto;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI.Controllers
{
    [CrossSite]
    public class WithdrawalsController : BaseApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:财务管理-提现管理列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetList([FromUri]ReqWithdrawalsDto request)
        {
            var jsonResult = new Common.JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            //request.UserId = GetUserInfo.UserID;
            jsonResult.Result = new WithdrawalsQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:财务管理-提现详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetInfo([FromUri]
            ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals.ReqWithdrawalsAuditDto request)
        {
            var jsonResult = new Common.JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }

            jsonResult.Result = new WithdrawalsProvider(new ConfigEntity(),
                new ReqWithdrawalsAgainDto()).GetWithdrawals(request.WithdrawalsId);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:提现审核操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS008BUT100201")]
        public Common.JsonResult Audit([FromBody]ReqWithdrawalsAuditDto request)
        {
            var jsonResult = new Common.JsonResult();
            var userInfo = GetUserInfo;
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                // LoginUser = null
            }, new ReqWithdrawalsAgainDto()).Audit(request);
            return jsonResult.GetReturn(retValue);
        }


        /// <summary>
        /// auth:lixiong
        /// desc:财务管理-审核详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetAuditDetails([FromUri] ReqWithdrawalsAuditDto request)
        {
            var jsonResult = new Common.JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }

            jsonResult.Result = new WithdrawalsProvider(new ConfigEntity(),
                new ReqWithdrawalsAgainDto()).GetAuditDetails(request.WithdrawalsId);
            return jsonResult;
        }
    }
}