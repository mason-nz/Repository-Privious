using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xy.ToolBox.Config;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Config;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Response;
using XYAuto.ITSC.Chitunion2017.WebService.Common;

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.ContentDistribute
{
    public class DistributeProvider
    {
        private readonly DoHttpClient _doHttpClient;

        private readonly OpApiNoteDistributeConfigSection _configSection;
        public DistributeProvider(DoHttpClient doHttpClient)
        {
            _configSection = SectionInvoke<OpApiNoteDistributeConfigSection>.GetConfig(OpApiNoteDistributeConfigSection.SectionName);
            _doHttpClient = doHttpClient;
        }

        /// <summary>
        /// 贴片任务领取之后通知op分发物料
        /// </summary>
        /// <param name="reqDistributeDto"></param>
        /// <returns></returns>
        public ReturnValue PushDistribute(ReqDistributeDto reqDistributeDto)
        {
            //http://op1.chitunion.com/api/MaterielChannel/SetDistribute
            var requestUrl = _configSection.ApiUrl + $"api/MaterielChannel/SetDistribute";
            var postData = JsonConvert.SerializeObject(reqDistributeDto);
            var result = new DoPostApiLogClient(requestUrl, postData).GetPostResult<RespChituOpBaseDto<dynamic>>
               (s => _doHttpClient.PostByJson(requestUrl, postData).Result, Loger.Log4Net.Info);

            if (result == null)
            {
                Loger.Log4Net.Error("MaterielChannel/SetDistribute 接口未知错误");
                return null;
            }
            return result.Result;
        }
    }
}
