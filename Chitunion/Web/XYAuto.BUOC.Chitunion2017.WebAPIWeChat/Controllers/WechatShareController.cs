using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.App;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Media;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Media;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.WechatShare;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class WechatShareController : ApiController
    {
        public XYAuto.ITSC.Chitunion2017.Common.LoginUser GetUserInfo =>
            XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUser();

        [HttpPost]
        public JsonResult AddWechatShare([FromBody] ReqsShare DTO)
        {
            string strJson = XYAuto.ITSC.Chitunion2017.BLL.Json.JsonSerializerBySingleData(DTO);
            Loger.Log4Net.Info("[WechatShareController]******AddWechatShare begin...->ReqsShare:" + strJson + "");
            try
            {
                XYAuto.ITSC.Chitunion2017.BLL.WechatShare.WechatShare.Instance.AddWechatShare(DTO);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[WechatShareController]*****AddWechatShare ->ReqsShare:" + strJson + ",分享失败:" + ex.Message);
                throw ex;
            }
            Common.JsonResult jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
            Loger.Log4Net.Info("[WechatShareController]******AddWechatShare end->");
            return jr;
        }


        /// <summary>
        /// auth:lixiong
        /// desc:分享之前的推广url
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetPostOrderUrl([FromUri] ReqTaskReceiveDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            jsonResult.Result = new ShareProvider(new ConfigEntity(), new ReqCreateShareDto()).GetOrderUrl(request.TaskId).ReturnObject;
            return jsonResult;
        }


        /// <summary>
        /// auth:lixiong
        /// desc:app客户端分享（包括签到，首次欢迎奖励，提现成功奖励）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult CreateOrder([FromBody] ReqCreateShareDto reqCreate)
        {
            var jsonResult = new JsonResult();
            var retValue = new ShareProvider(new ConfigEntity() { CreateUserId = GetUserInfo.UserID 
            }, reqCreate).Log();
            return jsonResult.GetReturn(retValue);
        }
    }
}
