/********************************************************
*创建人：lixiong
*创建时间：2017/10/27 20:20:41
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.Config;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.Op.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.Op.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Http;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;
using XYAuto.ChiTu.Config;

namespace XYAuto.BUOC.ChiTuData2017.ExternalApi.Op
{
    public class PushMaterielDetailsProvider
    {
        private readonly DoHttpClient _doHttpClient;
        private readonly OpApiNoteDistributeConfigSection _configSection;

        public PushMaterielDetailsProvider(Infrastruction.Http.DoHttpClient doHttpClient)
        {
            _doHttpClient = doHttpClient;
            _configSection = SectionInvoke<OpApiNoteDistributeConfigSection>.GetConfig(OpApiNoteDistributeConfigSection.SectionName);
        }

        /// <summary>
        /// 推送物料分发详情给op系统
        /// </summary>
        /// <param name="reqDetailsDto"></param>
        /// <returns></returns>
        public dynamic PushDistributeDetails(PushDistributeDetailsDto reqDetailsDto)
        {
            //http://op1.chitunion.com/api/MaterielDistributeResult/Add
            var requestUrl = _configSection.ApiUrl + $"api/MaterielDistributeResult/Add";
            var postData = JsonConvert.SerializeObject(reqDetailsDto);
            var result = new DoPostApiLogClient(requestUrl, postData).GetPostResult<RespChituOpBaseDto<dynamic>>
               (s => _doHttpClient.PostByJson(requestUrl, postData).Result, Loger.Log4Net.Info);

            if (result == null)
            {
                Loger.Log4Net.Error("MaterielDistributeResult/Add 接口未知错误");
                return null;
            }
            return result.Result;
        }
    }
}