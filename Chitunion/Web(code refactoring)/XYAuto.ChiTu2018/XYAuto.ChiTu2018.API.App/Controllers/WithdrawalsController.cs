using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ChiTu2018.API.App.Common;
using XYAuto.ChiTu2018.API.App.Filter;
using XYAuto.ChiTu2018.API.App.Models;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.App.AppInfo;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request.Withdrawals;
using XYAuto.ChiTu2018.Service.App.AppInfo.Provider;
using XYAuto.ChiTu2018.Service.App.Query.Dto.Request;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ChiTu2018.API.App.Controllers
{
    /// <summary>
    /// 提现申请相关
    /// </summary>
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class WithdrawalsController : BaseApiController
    {
        /// <summary>
        /// 查询收入管理-详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetIncomeInfo()
        {
            var jsonResult = new JsonResult()
            { Result = LeWithdrawalsStatisticsService.Instance.GetIncomeInfo(GetUserInfo.UserID) };

            return jsonResult;
        }

        /// <summary>
        /// 提现操作
        /// </summary>
        /// <param name="dto">提现信息</param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult WithdrawalsOpt([FromBody]ReqWithdrawalsDto dto)
        {
            var jsonResult = new JsonResult();
            if (dto == null)
            {
                return Common.Util.GetJsonDataByResult(null, "error", 500);
            }
       
            var retValue = new AppWithdrawalsProvider(new ConfigEntity()
            {
                UserId = GetUserInfo.UserID,
                Ip = CTUtils.Html.RequestHelper.GetIpAddress()
            }, dto).Withdrawals();
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        ///  收入管理-提现明细列表
        ///  
        /// </summary>
        /// <param name="requeryArgs"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetWithdrawalsList([FromUri]ReqInComeDto requeryArgs)
        {
            requeryArgs.UserId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            var list = Service.App.AppInfo.LeWithdrawalsDetailService.Instance.GetIncomeWithdrawalsQuery(requeryArgs);

            return new JsonResult { Result = list };
        }

        /// <summary>
        /// 提现操作-计算金额
        /// </summary>
        /// <param name="reqWithdrawals"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult WithdrawalsPriceCalc([FromBody]ReqWithdrawalsDto reqWithdrawals)
        {

            var jsonResult = new JsonResult
            {
                Result = new AppWithdrawalsProvider(new ConfigEntity()
                {
                    UserId = UserInfo.GetLoginUserID()
                }, reqWithdrawals).PriceCalc()
            };
            return jsonResult;
        }

        /// <summary>
        /// 收入管理-提现操作-校验点击按钮
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult VerifyWithdrawalsClick()
        {
            var jsonResult = new JsonResult();

            var retValue = new AppWithdrawalsProvider(new ConfigEntity()
            {
                UserId = UserInfo.GetLoginUserID(),
                Ip = CTUtils.Html.RequestHelper.GetIpAddress(string.Empty)
            }, new ReqWithdrawalsDto())
                .VerifyWithdrawalsClick();
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// 查询提现详情
        /// </summary>
        /// <param name="withdrawalsId"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetInfo(int withdrawalsId)
        {
            Dictionary<string, object> dic = LeWithdrawalsDetailService.Instance.GetWithdrawalsInfo(withdrawalsId, UserInfo.GetLoginUserID());
            return Common.Util.GetJsonDataByResult(dic, "Success", 0);
        }
    }
}
