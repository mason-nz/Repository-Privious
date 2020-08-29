using System.Collections.Generic;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class ChannelController : ApiController
    {
        #region 第一部分
        [HttpPost]
        public JsonResult ModifyChannel([FromBody]ModifyChannelReqDTO req)
        {
            bool res = true;
            string msg = "失败";
            if (!req.CheckSelfModel(out msg))
            {
                return Util.GetJsonDataByResult(null, msg, -1);
            }
            res = BLL.Channel.Instance.ModifyChannel(req, ref msg);
            return Util.GetJsonDataByResult(res, res ? "成功" : msg, res ? 0 : 1);
        }

        [HttpPost]
        public JsonResult DeleteChannel([FromBody] ModifyChannelReqDTO req)
        {
            bool res = true;
            string msg = "删除失败";
            res = BLL.Channel.Instance.DeleteChannel(req.ChannelID, ref msg);
            return Util.GetJsonDataByResult(res, res ? "删除成功" : msg, res ? 0 : 1);
        }

        [HttpGet]
        public JsonResult GetChannelList([FromUri]GetChannelListReqDTO req)
        {
            string msg = "失败";
            GetChannelListResDTO res = BLL.Channel.Instance.GetChannelList(req);
            return Util.GetJsonDataByResult(res, res != null ? "成功" : msg, res != null ? 0 : 1);
        }

        [HttpGet]
        public JsonResult GetChannelInfo(int channelID)
        {
            string msg = "失败";
            GetChannelInfoResDTO res = BLL.Channel.Instance.GetChannelDetail(channelID);
            return Util.GetJsonDataByResult(res, res != null ? "成功" : msg, res != null ? 0 : 1);
        }
        #endregion

        #region 第二部分
        [HttpPost]
        public JsonResult UploadChannelResource([FromBody]UploadChannelResourceReqDTO req )
        {
            string msg = "";
            List<string> rowErrorList = new List<string>();
            var res = BLL.Channel.Instance.UploadChannelResource(req, ref msg);
            return Util.GetJsonDataByResult(res, msg, res != null ? 0 : 1);
        }

        [HttpGet]
        public JsonResult GetCostList([FromUri]GetCostListReqDTO req)
        {
            GetCostListResDTO res = BLL.Channel.Instance.GetCostList(req);
            return Util.GetJsonDataByResult(res);
        }

        [HttpGet]
        public JsonResult GetCostDetail(int costID) 
        {
            GetCostDetailResDTO res = BLL.Channel.Instance.GetCostDetail(costID);
            return Util.GetJsonDataByResult(res);
        }

        [HttpGet]
        public JsonResult GetUploadPreview()
        {
            string msg = string.Empty;
            var res = BLL.Channel.Instance.GetUploadPreview(ref msg);
            return Util.GetJsonDataByResult(res, res == null ? msg : "成功", res == null ? 1 : 0);
        }

        [HttpPost]
        public JsonResult SubmitUploadResource()
        {
            string msg = "提交失败";
            var res = BLL.Channel.Instance.SubmitUploadResource(ref msg);
            return Util.GetJsonDataByResult(res, res ? "提交成功" : msg, res ? 0 : 1);
        }

        [HttpPost]
        public JsonResult BatchCostOperate([FromBody] BatchCostOperateReqDTO req)
        {
            string msg = "操作失败";
            bool res = BLL.Channel.Instance.BatchCostOperate(req, ref msg);
            return Util.GetJsonDataByResult(res, res ? "操作成功" : msg, res ? 0 : 1);
        }

        [HttpPost]
        public JsonResult DeleteCost([FromBody]dynamic req)
        {
            bool res = BLL.Channel.Instance.DeleteCost((int)req.CostID);
            return Util.GetJsonDataByResult(res, res ? "操作成功" : "操作失败", res ? 0 : 1);
        }

        [HttpGet]
        public JsonResult GetCost_ChannelList()
        {
            var res = BLL.Channel.Instance.GetCostChannelList();
            return Util.GetJsonDataByResult(res);
        }

        [HttpGet]
        public JsonResult GetCost_MediaList(string mediaName = "")
        {
            var res = BLL.Channel.Instance.GetCostMediaList(mediaName);
            return Util.GetJsonDataByResult(res);
        }

        #endregion
    }
}
