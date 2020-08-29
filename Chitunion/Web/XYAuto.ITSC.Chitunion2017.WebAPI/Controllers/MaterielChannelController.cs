using Newtonsoft.Json;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_11;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class MaterielChannelController : ApiController
    {

        [HttpPost]
        public JsonResult SaveChannel([FromBody]SaveChannelReqDTO req)
        {
            string msg = string.Empty;
            if (req.ChannelList != null && req.ChannelList.Count > 0)
            {
                req.ChannelList.ForEach(item => 
                {
                    if(!string.IsNullOrEmpty(item.MediaTypeName))
                        item.MediaTypeName = item.MediaTypeName.Trim();
                    if (!string.IsNullOrEmpty(item.MediaNumber))
                        item.MediaNumber = item.MediaNumber.Trim();
                });
            }

            if(!req.CheckSelfModel(out msg))
                return WebAPI.Common.Util.GetJsonDataByResult(null, msg, 1);
            Loger.Log4Net.Info("SaveChannel:"+JsonConvert.SerializeObject(req));
            bool res = BLL.Materiel.MaterielChannel.Instance.SaveChannel(req, ref msg);
            return WebAPI.Common.Util.GetJsonDataByResult(null, res ? "成功" : msg, res ? 0 : 1);
        }

    }
}
