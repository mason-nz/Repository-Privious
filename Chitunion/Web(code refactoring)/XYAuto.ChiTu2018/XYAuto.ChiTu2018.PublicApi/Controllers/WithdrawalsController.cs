using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.PublicApi.Common;
using XYAuto.ChiTu2018.PublicApi.Filter;
using XYAuto.ChiTu2018.PublicApi.Models;
using XYAuto.ChiTu2018.PublicApi.Models.Withdrawals;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request.Withdrawals;
using XYAuto.ChiTu2018.Service.App.PublicService;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Request.Withdrawals;

namespace XYAuto.ChiTu2018.PublicApi.Controllers
{
    /// <summary>
    /// 公共业务接口-提现申请相关
    /// </summary>
    [CrossSite]
    public class WithdrawalsController : ApiController
    {
        /// <summary>
        /// 提现申请操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult WithdrawalsDo([FromBody]ReqThirdWithdrawlsDto request)
        {
            var jsonResult = new JsonResult();

            //if (!ModelState.IsValid)
            //{
            //    // 在响应体中返回验证错误
            //    var errors = new Dictionary<string, IEnumerable<string>>();
            //    foreach (KeyValuePair<string, ModelState> keyValue in ModelState)
            //    {
            //        errors[keyValue.Key] = keyValue.Value.Errors.Select(e => e.ErrorMessage);
            //    }
            //    jsonResult.Result = errors;
            //    jsonResult.Status = -1;
            //    return jsonResult;
            //}
            if (request == null)
            {
                jsonResult.Message = "请输入参数";
                jsonResult.Status = -1;
                return jsonResult;
            }
            var retValue = new PsWithdrawalsService(new PsReqWithdrawalsDto()
            {
                WithdrawalsPrice = request.WithdrawalsPrice,
                ApplySource = (WithdrawalsApplySourceEnum)request.ApplySource,
                Ip = request.Ip,
                UserId = request.UserId
            }).Withdrawals();
            return jsonResult.GetReturn(retValue);
        }


        /// <summary>
        /// 收入管理-提现操作-校验点击按钮
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult VerifyWithdrawalsClick([FromBody]ReqThirdBaseDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Message = "请输入参数";
                jsonResult.Status = -1;
                return jsonResult;
            }
            var retValue = new PsWithdrawalsService(new PsReqWithdrawalsDto()
            {
                UserId = request.UserId,
                Ip = request.Ip
            }).VerifyWithdrawalsClick();
            return jsonResult.GetReturn(retValue);
        }


        /// <summary>
        /// 提现操作-计算个税金额
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult PriceCalc([FromBody]ReqThirdBaseDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null || request.UserId <= 0)
            {
                jsonResult.Message = "请输入参数";
                jsonResult.Status = -1;
                return jsonResult;
            }
            jsonResult.Result = new PsWithdrawalsService(new PsReqWithdrawalsDto()
            {
                UserId = request.UserId
            }).PriceCalc();
            return jsonResult;
        }
    }
}
