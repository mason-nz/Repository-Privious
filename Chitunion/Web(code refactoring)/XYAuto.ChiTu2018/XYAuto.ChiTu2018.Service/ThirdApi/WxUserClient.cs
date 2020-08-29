using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// 注释：WxUserClient
    /// 作者：lix
    /// 日期：2018/6/8 14:47:17
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class WxUserClient : VerifyOperateBase
    {
        private readonly AppPostThirdApiConfigSection _configSection;

        public WxUserClient()
        {
            _configSection = SectionInvoke<AppPostThirdApiConfigSection>.GetConfig(AppPostThirdApiConfigSection.SectionName);
        }

        /// <summary>
        /// 微信授权操作相关
        /// </summary>
        /// <param name="postWxUser"></param>
        /// <returns></returns>
        public ReturnValue PostWxUserOperation(ReqPostWxUserOperationDto postWxUser)
        {
            var retValue = new ReturnValue();
            var requestUrl = _configSection.PostWithdrawalsUrl;
            var postData = JsonConvert.SerializeObject(postWxUser);
            var result = new DoPostApiLogClient(requestUrl, postData).GetPostResult<RespChituBaseDto<RespUserOpeationDto>>(
                   s => Infrastructure.HttpLog.HttpClient.PostByJson(requestUrl, postData), CTUtils.Log.Log4NetHelper.Default().Info);

            if (result == null || result.Status != 0)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"PostWxUserOperation http post fail." +
                                    (result == null ? string.Empty : JsonConvert.SerializeObject(result)));
                return CreateFailMessage(retValue, "-1", "微信用户授权操作失败");
            }
            return CreateSuccessMessage(retValue, result.Status.ToString(), result.Message, result.Result);
        }
    }
}
