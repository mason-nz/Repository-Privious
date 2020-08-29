using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.Withdrawals;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
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
            var jsonResult = new JsonResult
            { Result = new OrderProvider().GetIncomeInfo(ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID()) };

            return jsonResult;
        }
        /// <summary>
        /// 提现操作
        /// </summary>
        /// <param name="DTO">提现信息</param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult WithdrawalsOpt([FromBody]WithdrawalsPriceReqDTO DTO)
        {

            if (!Enum.IsDefined(typeof(WithdrawalsApplySourceEnum), DTO.ApplySource))
            {
                return new JsonResult() { Status = -1, Message = "参数错误", Result = -1 };
            }
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            int userType = WechatUser.Instance.GetUserTypeByUserId(userId);
            ReqWithdrawalsDto request = new ReqWithdrawalsDto() { WithdrawalsPrice = DTO.WithdrawalsPrice, ApplySource = (WithdrawalsApplySourceEnum)DTO.ApplySource };
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = userId,
                UserType = (UserTypeEnum)userType
            }, request).Withdrawals();
            return Common.Util.GetReturn(retValue);
        }

        /// <summary>
        /// 收入管理-提现明细列表
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetWithdrawalsList(int PageIndex = 1, int PageSize = 20)
        {
            ReqInComeDto request = new ReqInComeDto();
            request.PageIndex = PageIndex;
            request.PageSize = PageSize;
            request.UserId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            var jsonResult = new JsonResult();
            jsonResult.Result = new IncomeWithdrawalsQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }
        /// <summary>
        /// 提现操作-计算金额
        /// </summary>
        /// <param name="DTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult WithdrawalsPriceCalc([FromBody]WithdrawalsPriceReqDTO DTO)
        {
            var jsonResult = new JsonResult
            {
                Result = new WithdrawalsProvider(new ConfigEntity()
                {
                    CreateUserId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID()
                }, new ReqWithdrawalsDto() { WithdrawalsPrice = DTO.WithdrawalsPrice }).PriceCalc()
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
            Dictionary<string, object> dic = ITSC.Chitunion2017.BLL.WechatWithdrawals.WechatWithdrawals.Instance.GetWithdrawalsInfo(WithdrawalsId);
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

            var userInfo = ITSC.Chitunion2017.Common.UserInfo.GetLoginUser();
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                UserType = (UserTypeEnum)userInfo.Type,
                LoginUser = userInfo
            }, new ReqWithdrawalsDto()).VerifyWithdrawalsClick();
            return Common.Util.GetReturn(retValue);
        }
    }
}
