using Newtonsoft.Json;
using XYAuto.ChiTu2018.Infrastructure.HttpLog;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.ThirdApi.Config;
using XYAuto.ChiTu2018.Service.ThirdApi.Dto.Request;
using XYAuto.ChiTu2018.Service.ThirdApi.Dto.Response;
using XYAuto.CTUtils.Image.FastDFS.ToolBox.Config;

namespace XYAuto.ChiTu2018.Service.ThirdApi
{
    /// <summary>
    /// 注释：WithdrawalsClient
    /// 作者：lix
    /// 日期：2018/5/23 17:19:46
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class WithdrawalsClient : VerifyOperateBase
    {
        private readonly AppPostThirdApiConfigSection _configSection;

        public WithdrawalsClient()
        {
            _configSection = SectionInvoke<AppPostThirdApiConfigSection>.GetConfig(AppPostThirdApiConfigSection.SectionName);
        }

        /// <summary>
        /// 提交提现申请
        /// </summary>
        /// <param name="postWithdrawlsDto"></param>
        /// <returns></returns>
        public ReturnValue PostWithdrawals(ReqPostWithdrawlsDto postWithdrawlsDto)
        {
            var retValue = new ReturnValue();
            var requestUrl = _configSection.PostWithdrawalsUrl;
            var postData = JsonConvert.SerializeObject(postWithdrawlsDto);
            var result = new DoPostApiLogClient(requestUrl, postData).GetPostResult<RespChituBaseDto<dynamic>>(
                   s => Infrastructure.HttpLog.HttpClient.PostByJson(requestUrl, postData), CTUtils.Log.Log4NetHelper.Default().Info);

            if (result == null || result.Status != 0)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"PostWithdrawals http post Withdrawals fail." +
                                    (result == null ? string.Empty : JsonConvert.SerializeObject(result)));
                return CreateFailMessage(retValue, "-1", "生成订单错误");
            }
            return CreateSuccessMessage(retValue, result.Status.ToString(), result.Message, result.Result);
        }

        /// <summary>
        /// 提现点击
        /// </summary>
        /// <param name="postWithdrawalsClickDto"></param>
        /// <returns></returns>
        public ReturnValue PostVerifyWithdrawalsClick(ReqPostWithdrawalsClickDto postWithdrawalsClickDto)
        {
            var retValue = new ReturnValue();
            var requestUrl = _configSection.PostVerifyWithdrawalsClickUrl;
            var postData = JsonConvert.SerializeObject(postWithdrawalsClickDto);
            var result = new DoPostApiLogClient(requestUrl, postData).GetPostResult<RespChituBaseDto<dynamic>>(
                   s => Infrastructure.HttpLog.HttpClient.PostByJson(requestUrl, postData), CTUtils.Log.Log4NetHelper.Default().Info);

            if (result == null || result.Status != 0)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"PostVerifyWithdrawalsClick http post Withdrawals fail." +
                                    (result == null ? string.Empty : JsonConvert.SerializeObject(result)));
                return CreateFailMessage(retValue, "-1", "接口错误");
            }
            return CreateSuccessMessage(retValue, result.Status.ToString(), result.Message, result.Result);
        }

        /// <summary>
        /// 计算个税金额
        /// </summary>
        /// <param name="postWithdrawalsPriceCalcDto">只需要UserId</param>
        /// <returns></returns>
        public ReturnValue PostWithdrawalsPriceCalc(ReqPostWithdrawalsClickDto postWithdrawalsPriceCalcDto)
        {
            var retValue = new ReturnValue();
            var requestUrl = _configSection.PostWithdrawalsPriceCalcUrl;
            var postData = JsonConvert.SerializeObject(postWithdrawalsPriceCalcDto);
            var result = new DoPostApiLogClient(requestUrl, postData).GetPostResult<RespChituBaseDto<dynamic>>(
                   s => Infrastructure.HttpLog.HttpClient.PostByJson(requestUrl, postData), CTUtils.Log.Log4NetHelper.Default().Info);

            if (result == null || result.Status != 0)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"PostVerifyWithdrawalsClick http post Withdrawals fail." +
                                    (result == null ? string.Empty : JsonConvert.SerializeObject(result)));
                return CreateFailMessage(retValue, "-1", "接口错误");
            }
            return CreateSuccessMessage(retValue, result.Status.ToString(), result.Message, result.Result);
        }
    }
}
