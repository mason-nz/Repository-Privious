using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Response;

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Kr
{
    /// <summary>
    /// 注释：KrErrorMessageProvider
    /// 作者：lix
    /// 日期：2018/5/22 16:25:52
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class KrErrorMessageProvider
    {
        /// <summary>
        /// 特殊处理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public RespKrBaseDto GetKrBaseDto(string message)
        {
            var resp = new RespKrBaseDto();
            if (string.IsNullOrWhiteSpace(message))
            {
                return resp;
            }
            try
            {
                message = message.Remove(0, message.IndexOf(':') + 1);
                resp = JsonConvert.DeserializeObject<RespKrBaseDto>(message);
            }
            catch
            {
                resp.ErrorMessage = message;
                Loger.ZhyLogger.Error($"GetKrBaseDto is fail.message:{message}");
            }

            return resp;
        }
    }
}
