using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.Activity;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.Withdrawals;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [CrossSite]
    public class OneYuanTxController : ApiController
    {
        /// <summary>
        /// 提现操作
        /// </summary>
        /// <param name="request">提现信息</param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Withdrawals([FromBody]ReqWithdrawalsDto request)
        {
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            int userType = WechatUser.Instance.GetUserTypeByUserId(userId);
            var retValue = new OneYuanTxProvider(new ConfigEntity()
            {
                CreateUserId = userId,
                UserType = (UserTypeEnum)userType,
            }, request).PostWithdrawas();
            return Common.Util.GetReturn(retValue);
        }

        /// <summary>
        /// 收入管理-提现-个税计算
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult WithdrawalsPriceCalc([FromBody]ReqWithdrawalsDto request)
        {
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            var jsonResult = new JsonResult
            {
                Result = new WithdrawalsProvider(new ConfigEntity()
                {
                    CreateUserId = userId,
                }, request).PriceCalc()
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

            var userInfo = ITSC.Chitunion2017.Common.UserInfo.GetLoginUser();
            var retValue = new OneYuanTxProvider(new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                UserType = (UserTypeEnum)userInfo.Type,
                LoginUser = userInfo
            }, new ReqWithdrawalsDto()).VerifyWithdrawalsClick();
            return Common.Util.GetReturn(retValue);
        }
    }
}
