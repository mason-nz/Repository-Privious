/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 19:06:59
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.Config;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Http;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Security;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;
using XYAuto.ChiTu.Config;

namespace XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY
{
    /// <summary>
    /// auth:lixiong
    /// desc:对智慧云相关业务
    /// </summary>
    public class MaterielStatisticsProvider : VerifyOperateBase
    {
        private readonly ZhyApiConfigSection _configSection;
        private readonly Infrastruction.Http.DoHttpClient _doHttpClient;
        protected readonly string Timestamp;

        public MaterielStatisticsProvider(Infrastruction.Http.DoHttpClient doHttpClient)
        {
            _doHttpClient = doHttpClient;
            _configSection = SectionInvoke<ZhyApiConfigSection>.GetConfig(ZhyApiConfigSection.SectionName);
            Timestamp = SignUtility.ConvertDateTimeInt(DateTime.Now).ToString();
        }

        /// <summary>
        /// 拉取物料统计信息
        /// </summary>
        /// <returns></returns>
        public RespMaterielDto PullStatistics(PullStatisticsDto pullStatisticsDto)
        {
            var dicParams = GetSignatureParams();
            dicParams.Insert("MaterielID", pullStatisticsDto.HeadArticleId);
            dicParams.Insert("TaskID", pullStatisticsDto.MaterielId);
            dicParams.Insert("DateTime", pullStatisticsDto.DateTime);
            var postData = $"MaterielID={pullStatisticsDto.HeadArticleId}&TaskID={pullStatisticsDto.MaterielId}" +
                           $"&DateTime={pullStatisticsDto.DateTime}";

            var sign = ZhySignatureProvider.GetSignatureText(dicParams);
            var requestUrl = _configSection.ApiUrl + $"chitunion/MaterialStatistics?appkey={_configSection.AppKey}&signature={sign}&timestamp={Timestamp}";

            var result = new DoPostApiLogClient(requestUrl, postData).GetPostResult<RespZhyBaseDto<RespMaterielDto>>
               (s => _doHttpClient.PostByForm(requestUrl, postData).Result, Loger.ZhyLogger.Info);

            if (result == null || !result.Success)
            {
                Loger.ZhyLogger.Error(result == null ? "接口未知错误" : result.ErrorMessage);
                return null;
            }
            return result.Data;
        }

        public List<RespMaterielDetailDto> PullMaterielDetails(string queryDate)
        {
            var dicParams = GetSignatureParams();
            dicParams.Insert("AllocDate", queryDate);
            var postData = $"AllocDate={queryDate}";

            var sign = ZhySignatureProvider.GetSignatureText(dicParams);
            var requestUrl = _configSection.ApiUrl + $"chitunion/AllocInfo?appkey={_configSection.AppKey}&signature={sign}&timestamp={Timestamp}";

            var result = new DoPostApiLogClient(requestUrl, postData).GetPostResult<RespZhyBaseDto<List<RespMaterielDetailDto>>>
               (s => _doHttpClient.PostByForm(requestUrl, postData).Result, Loger.ZhyLogger.Info);

            if (result == null || !result.Success)
            {
                Loger.ZhyLogger.Error(result == null ? "接口未知错误" : result.ErrorMessage);
                return null;
            }
            return result.Data;
        }

        private Dictionary<string, object> GetSignatureParams()
        {
            return new Dictionary<string, object>
            {
                {"appkey", _configSection.AppKey},
                {"appsecret", _configSection.Secret},
                {"timestamp", Timestamp},
            };
        }
    }
}