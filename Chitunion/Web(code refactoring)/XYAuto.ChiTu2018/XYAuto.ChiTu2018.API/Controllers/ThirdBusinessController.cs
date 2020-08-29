using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using XYAuto.ChiTu2018.API.Common;
using XYAuto.ChiTu2018.API.Filter;
using XYAuto.ChiTu2018.API.Models;
using XYAuto.ChiTu2018.API.Models.Withdrawals;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Enum.User;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.LE.Provider;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto.Request.Withdrawals;

namespace XYAuto.ChiTu2018.API.Controllers
{
    /// <summary>
    /// 对外提供业务
    /// </summary>
    public class ThirdBusinessController : ApiController
    {
        /// <summary>
        /// 提现申请操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult Withdrawals([FromBody]ReqThirdWithdrawlsDto request)
        {
            var jsonResult = new JsonResult();

            if (!ModelState.IsValid)
            {
                // 在响应体中返回验证错误
                var errors = new Dictionary<string, IEnumerable<string>>();
                foreach (KeyValuePair<string, ModelState> keyValue in ModelState)
                {
                    errors[keyValue.Key] = keyValue.Value.Errors.Select(e => e.ErrorMessage);
                }
                jsonResult.Result = errors;
                jsonResult.Status = -1;
                return jsonResult;
            }
            var retValue = new WxWithdrawalsProvider(new ConfigEntity()
            {
                UserId = request.UserId
            }, new ReqWithdrawalsDto()
            {
                WithdrawalsPrice = request.WithdrawalsPrice,
                ApplySource = (WithdrawalsApplySourceEnum)request.ApplySource,
                Ip = request.Ip
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
            var retValue = new WxWithdrawalsProvider(new ConfigEntity()
            {
                UserId = request.UserId
            }, new ReqWithdrawalsDto() { Ip = request.Ip })
                .VerifyWithdrawalsClick();
            return jsonResult.GetReturn(retValue);
        }
    }
}
