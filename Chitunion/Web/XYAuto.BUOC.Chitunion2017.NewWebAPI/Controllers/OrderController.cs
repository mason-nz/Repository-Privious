using System;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Common;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Media;
using XYAuto.ITSC.Chitunion2017.BLL.Query.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.LeOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [CrossSite]

    public class OrderController : BaseApiController
    {

        /// <summary>
        /// auth:lixiong
        /// desc:订单管理-内容分发订单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetDistributeList([FromUri] ReqOrderCoverImageDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            request.UserId = GetUserInfo.UserID;
            jsonResult.Result = new OrderDistributeQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:订单管理-贴片广告订单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetCoverImageList([FromUri] ReqOrderCoverImageDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            request.UserId = GetUserInfo.UserID;
            jsonResult.Result = new OrderCoverImageQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:订单管理-订单详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetInfo([FromUri]ReqOrderInfoDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            jsonResult.Result = new OrderProvider().GetOrderInfo(request.OrderId, GetUserInfo.UserID);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:订单管理-订单收入详情列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetIncomeDetails([FromUri]ReqOrderInfoDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            jsonResult.Result = new OrderProvider().GetOrderIncomeList(request.OrderId, GetUserInfo.UserID);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:收入管理-提现明细列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetWithdrawalsList([FromUri]ReqInComeDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            request.UserId = GetUserInfo.UserID;
            jsonResult.Result = new IncomeWithdrawalsQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }


        /// <summary>
        /// auth:lixiong
        /// desc:收入管理-收入明细列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetIncomeList([FromUri]ReqInComeDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            request.UserId = GetUserInfo.UserID;
            jsonResult.Result = new IncomeQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:收入管理-收入统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetIncomeInfo()
        {
            var jsonResult = new JsonResult
            { Result = new OrderProvider().GetIncomeInfo(GetUserInfo.UserID) };

            return jsonResult;

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
            var jsonResult = new JsonResult
            {
                Result = new WithdrawalsProvider(new ConfigEntity()
                {
                    CreateUserId = GetUserInfo.UserID,
                }, request).PriceCalc()
            };

            return jsonResult;
        }

        /// <summary>
        /// 收入管理-提现操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Withdrawals([FromBody]ReqWithdrawalsDto request)
        {
            var jsonResult = new JsonResult();

            var userInfo = GetUserInfo;
            request.ApplySource = WithdrawalsApplySourceEnum.Pc;
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                UserType = (UserTypeEnum)userInfo.Type,
                LoginUser = userInfo
            }, request).Withdrawals();
            
            return jsonResult.GetReturn(retValue);
        }
        /// <summary>
        /// 收入管理-提现操作（再次发起提现申请）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult WithdrawalsAgain([FromBody]ReqWithdrawalsAgainDto request)
        {
            var jsonResult = new JsonResult();

            var userInfo = GetUserInfo;
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                UserType = (UserTypeEnum)userInfo.Type,
                LoginUser = userInfo
            }, request).WithdrawalsAgain();

            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// 收入管理-提现操作-校验点击按钮
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult VerifyWithdrawalsClick()
        {
            var jsonResult = new JsonResult();

            var userInfo = GetUserInfo;
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                UserType = (UserTypeEnum)userInfo.Type,
                LoginUser = userInfo
            }, new ReqWithdrawalsDto()).VerifyWithdrawalsClick();
            
            return jsonResult.GetReturn(retValue);
        }
    }
}