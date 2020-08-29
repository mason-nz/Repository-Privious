using System.Collections.Generic;
using System.Web.Http;
using XYAuto.ChiTu2018.API.Common;
using XYAuto.ChiTu2018.API.Filter;
using XYAuto.ChiTu2018.API.Models;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.LE;
using XYAuto.ChiTu2018.Service.LE.Provider;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto.Request.Withdrawals;
using XYAuto.ChiTu2018.Service.LE.Query.Dto.Request;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ChiTu2018.API.Controllers
{
    /// <summary>
    /// 提现相关
    /// </summary>
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class WithdrawalsController : ApiController
    {
        /// <summary>
        /// 查询收入管理-详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetIncomeInfo()
        {
            var info = LeWithdrawalsStatisticsService.Instance.GetIncomeInfo(UserInfo.GetLoginUserID());
            return Common.Util.GetJsonDataByResult(info);
        }
        /// <summary>
        /// 提现操作
        /// </summary>
        /// <param name="DTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult WithdrawalsOpt([FromBody]ReqWithdrawalsDto DTO)
        {

            if (DTO == null)
            {
                return Common.Util.GetJsonDataByResult(null, "error", 500);
            }
            else
            {
                DTO.ApplySource = WithdrawalsApplySourceEnum.WeiXin;
                DTO.Ip = CTUtils.Html.RequestHelper.GetIpAddress(string.Empty);
                var retValue = new WxWithdrawalsProvider(new ConfigEntity()
                {
                    UserId = UserInfo.GetLoginUserID()
                }, DTO).Withdrawals();
                return new JsonResult().GetReturn(retValue);
            }
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
            var list = Service.LE.LeWithdrawalsDetailService.Instance.GetIncomeWithdrawalsQuery(requeryArgs);

            return new JsonResult { Result = list };
        }
        /// <summary>
        /// 提现操作-计算金额
        /// </summary>
        /// <param name="DTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult WithdrawalsPriceCalc([FromBody]ReqWithdrawalsDto DTO)
        {

            var jsonResult = new JsonResult
            {
                Result = new WxWithdrawalsProvider(new ConfigEntity()
                {
                    UserId = UserInfo.GetLoginUserID()
                }, DTO).PriceCalc()
            };
            return jsonResult;
        }
        /// <summary>
        /// 查询提现详情
        /// </summary>
        /// <param name="WithdrawalsId"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetInfo(int WithdrawalsId)
        {
            Dictionary<string, object> dic = LeWithdrawalsDetailService.Instance.GetWithdrawalsInfo(WithdrawalsId, UserInfo.GetLoginUserID());
            return Common.Util.GetJsonDataByResult(dic, "Success", 0);
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

            var retValue = new WxWithdrawalsProvider(new ConfigEntity()
            {
                UserId = UserInfo.GetLoginUserID()
            }, new ReqWithdrawalsDto() { Ip = CTUtils.Html.RequestHelper.GetIpAddress(string.Empty) })
                .VerifyWithdrawalsClick();
            return jsonResult.GetReturn(retValue);
        }
    }
}
