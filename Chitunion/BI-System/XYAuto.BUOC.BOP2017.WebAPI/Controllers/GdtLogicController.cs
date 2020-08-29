using System.Net;
using System.Text;
using System.Web.Http;
using XYAuto.BUOC.BOP2017.BLL.GDT;
using XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.Infrastruction.Security;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;
using XYAuto.BUOC.BOP2017.WebAPI.App_Start;
using XYAuto.BUOC.BOP2017.WebAPI.Common;
using XYAuto.BUOC.BOP2017.WebAPI.Filter;

namespace XYAuto.BUOC.BOP2017.WebAPI.Controllers
{
    /// <summary>
    /// 对智慧云提供的接口api
    /// </summary>
    [CrossSite]
    //[VerifyApiToken]
    public class GdtLogicController : ApiController
    {
        private readonly string _embedChiTuDesStr = Utils.Config.ConfigurationUtil.GetAppSettingValue("EmbedChiTu_DesStr");

        //private readonly Lazy<LogicTransferProvider> _lazyTransferProvider;//lazy加载
        private readonly LogicTransferProvider _logicTransferProvider;

        public GdtLogicController()
        {
            //_lazyTransferProvider = new Lazy<LogicTransferProvider>(() => new LogicTransferProvider() { });
            _logicTransferProvider = new LogicTransferProvider();
        }

        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateTicket(string accessToken)
        {
            //解密Ticket
            return ITSC.Chitunion2017.Common.AuthorizeLogin.Instance.Verification(accessToken);
        }

        //校验用户名密码（正式环境中应该是数据库校验）
        private JsonResult ValidateTicket(JsonResult jsonResult, RequestBaseTokenDto token)
        {
            if (token == null)
            {
                return UnAuthenResult(jsonResult, "sign校验失败,请输入验签相关参数");
            }
            //验签的开关
            if (!AppSettingsForZhy.Instance.ZhySignOff)
            {
                jsonResult.Status = 0;
                return jsonResult;
            }
            if (string.IsNullOrWhiteSpace(token.P) || string.IsNullOrWhiteSpace(token.AccessToken)
                || string.IsNullOrWhiteSpace(token.Sign) || token.Appid <= 0)
            {
                return UnAuthenResult(jsonResult, "sign校验失败,请输入验签相关参数");
            }

            if (!ValidateTicket(token.AccessToken))
            {
                return UnAuthenResult(jsonResult, "AccessToken验证信息过期或不存在");
            }

            token.P = token.P.Replace(" ", "+");
            var md5Code = SignUtility.Md5Hash(token.Appid + token.AccessToken + token.P + _embedChiTuDesStr, Encoding.UTF8);
            if (md5Code != token.Sign)
            {
                return UnAuthenResult(jsonResult, "sign校验失败");
            }
            ITSC.Chitunion2017.Common.Entities.AuthorizeLogin model = new ITSC.Chitunion2017.Common.Entities.AuthorizeLogin()
            {
                APPID = token.Appid,
                MD5Code = token.AccessToken
            };
            ITSC.Chitunion2017.Common.AuthorizeLogin.Instance.Update(model);

            jsonResult.Status = 0;
            return jsonResult;
        }

        private JsonResult UnAuthenResult(JsonResult jsonResult, string message)
        {
            jsonResult.Status = (int)HttpStatusCode.Unauthorized;
            jsonResult.Message = $"身份验证失败|Msg:{message}";
            jsonResult.Result = false;
            return jsonResult;
        }

        /// <summary>
        /// 智慧云推送用户
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult UserPush([FromBody]RequestPushUserDto requestDto)
        {
            var jsonResult = new JsonResult();

            jsonResult = ValidateTicket(jsonResult, requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            var retValue = _logicTransferProvider.PushUser(requestDto);
            if (retValue.HasError)
            {
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                jsonResult.Message = retValue.Message;
                return jsonResult;
            }
            jsonResult.Result = retValue.ReturnObject;
            jsonResult.Message = "success";

            return jsonResult;
        }

        /// <summary>
        /// 智慧云推送的需求详情
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult Demand([FromBody] RequestPushDemandDto requestDto)
        {
            var jsonResult = new JsonResult();
            jsonResult = ValidateTicket(jsonResult, requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            var retValue = _logicTransferProvider.PushDemand(requestDto);
            if (retValue.HasError)
            {
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                jsonResult.Message = retValue.Message;
                return jsonResult;
            }
            jsonResult.Message = "success";
            return jsonResult;
        }

        /// <summary>
        /// 充值单回传接口,智慧云充值成功之后会给系统一个充值单号，及时判断金额是否对不对
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult RechargeReceipt([FromBody] RequestRechargeReceiptDto requestDto)
        {
            var jsonResult = new JsonResult();
            jsonResult = ValidateTicket(jsonResult, requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            var retValue = _logicTransferProvider.RechargeReceipt(requestDto);
            if (retValue.HasError)
            {
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                jsonResult.Message = retValue.Message;
                return jsonResult;
            }
            jsonResult.Message = "success";
            return jsonResult;
        }

        /// <summary>
        /// 智慧云获取小时报表数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult Getfunds([FromBody]RequestReportByZhyDto requestDto)
        {
            var jsonResult = new JsonResult();
            jsonResult = ValidateTicket(jsonResult, requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            var tp = _logicTransferProvider.GetToZhyReport(requestDto);

            if (tp.Item1.HasError)
            {
                jsonResult.Status = tp.Item1.ErrorCode.ToInt(-1);
                jsonResult.Message = tp.Item1.Message;
                return jsonResult;
            }

            jsonResult.Result = tp.Item2;
            jsonResult.Message = "success";
            return jsonResult;
        }

        /// <summary>
        /// 需求撤销
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult DemandRevoke([FromBody]RequestDemandRevokeDto requestDto)
        {
            var jsonResult = new JsonResult();
            jsonResult = ValidateTicket(jsonResult, requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            var retValue = _logicTransferProvider.DemandRevoke(requestDto);
            if (retValue.HasError)
            {
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                jsonResult.Message = retValue.Message;
                return jsonResult;
            }
            jsonResult.Message = "success";
            return jsonResult;
        }
    }
}