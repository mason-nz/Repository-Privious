using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.ErrorException;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.FrontPage;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    /// <summary>
    /// ls
    /// </summary>
    [CrossSite]
    public class PublishController : ApiController
    {

        /// <summary>
        /// 新增、修改刊例
        /// ls
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult ModifyPublish([FromBody]PublishInfoDTO request)
        {
            try
            {
                BLL.Loger.Log4Net.Info("开始开始:" + JsonConvert.SerializeObject(request));
                string msg = string.Empty;
                if (!request.CheckSelfModel(out msg))
                {
                    return Util.GetJsonDataByResult(null, msg, -1);
                }
                #region 按媒体类型验证
                switch (request.Publish.MediaType)
                {
                    case MediaTypeEnum.微信:
                        if (request.Publish.PubID.Equals(0) ? !FuncValidate(FuncModuleEnum.刊例添加一微信) : !FuncValidate(FuncModuleEnum.刊例编辑一微信))
                            return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                        break;
                    case MediaTypeEnum.微博:
                        if (request.Publish.PubID.Equals(0) ? !FuncValidate(FuncModuleEnum.刊例添加一微博) : !FuncValidate(FuncModuleEnum.刊例编辑一微博))
                            return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                        break;
                    case MediaTypeEnum.视频:
                        if (request.Publish.PubID.Equals(0) ? !FuncValidate(FuncModuleEnum.刊例添加一视频) : !FuncValidate(FuncModuleEnum.刊例编辑一视频))
                            return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                        break;
                    case MediaTypeEnum.直播:
                        if (request.Publish.PubID.Equals(0) ? !FuncValidate(FuncModuleEnum.刊例添加一直播) : !FuncValidate(FuncModuleEnum.刊例编辑一直播))
                            return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                        break;
                    case MediaTypeEnum.APP:
                        if (request.Publish.PubID.Equals(0) ? !FuncValidate(FuncModuleEnum.刊例添加一APP) : !FuncValidate(FuncModuleEnum.刊例编辑一APP))
                            return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                        break;
                }
                #endregion
                bool res = false;
                int status = 0;
                if (request.Publish.PubID.Equals(0))//新增
                {
                    if (request.Publish.MediaType == MediaTypeEnum.APP)
                    {
                        #region APP按媒体名称添加刊例 检查查询MediaID
                        var tup = BLL.PublishInfo.Instance.CheckAppExists(request.Publish.MediaName);
                        status = tup.Item1;
                        if (!status.Equals(0))
                            return Util.GetJsonDataByResult(tup.Item2, "添加APP刊例失败", status);
                        request.Publish.MediaID = tup.Item2.MediaID;
                        #endregion
                    }
                    int pubID = BLL.PublishInfo.Instance.AddPublishBasicInfo(request);
                    res = pubID > 0;
                    XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.刊例管理, Chitunion2017.Common.LogInfo.ActionType.Add, "调用了新增刊例（媒体类型：" + Enum.GetName(typeof(MediaTypeEnum), request.Publish.MediaType) + "）接口");
                    if (!res)
                        BLL.Loger.Log4Net.Info("[PublishController]*****ModifyPublish ->request:" + JsonConvert.SerializeObject(request));
                    return Util.GetJsonDataByResult(res ? new MediaExistsDTO() { PubID = pubID } : null, res ? "成功" : "失败", res ? 0 : 1);
                }
                else//编辑
                {
                    res = BLL.PublishInfo.Instance.UpdataPublishBasicInfo(request);
                    XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.刊例管理, Chitunion2017.Common.LogInfo.ActionType.Modify, "调用了编辑刊例（媒体类型：" + Enum.GetName(typeof(MediaTypeEnum), request.Publish.MediaType) + " ID：" + request.Publish.PubID + "）接口");
                    if (!res)
                        BLL.Loger.Log4Net.Info("[PublishController]*****ModifyPublish ->request:" + JsonConvert.SerializeObject(request));
                    return Util.GetJsonDataByResult(res, res ? "成功" : "失败", res ? 0 : 1);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[PublishController]*****ModifyPublish ->request:" + JsonConvert.SerializeObject(request) + ",调用新增编辑刊例接口出错:" + ex.Message);
                return Util.GetJsonDataByResult(null, "系统异常:" + ex.Message, 500);
            }

            #region 测试数据
            //{
            //    "Publish": {
            //        "PubID": 22,
            //        "MediaType": 1,
            //        "MediaID": 1,
            //        "BeginTime": "2017-01-01",
            //        "EndTime": "2017-01-31",
            //        "PurchaseDiscount": 0.25,
            //        "SaleDiscount": 0.11,
            //        "PublishStatus": 1
            //    },
            //    "Prices": [
            //        "6001-7001-8001-11.2",
            //        "6001-7001-8001-11.2",
            //        "6001-7001-8001-11.2",
            //        "6001-7001-8003-11.2"
            //    ]
            //}          
            #endregion
        }

        /// <summary>
        /// 修改上下架状态
        /// ls
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult ModifyPublishStatus([FromBody]ModifyPublishStatusDTO request)
        {
            try
            {
                string msg = string.Empty;
                if (!request.CheckSelfModel(out msg))
                {
                    return Util.GetJsonDataByResult(null, msg, -1);
                }
                MediaTypeEnum mediaType;
                if (!BLL.PublishInfo.Instance.GetMediaTypeByPubID(request.PubID, out mediaType))
                    return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                else
                {
                    #region 按媒体类型验证
                    switch (mediaType)
                    {
                        case MediaTypeEnum.微信:
                            if (!FuncValidate(FuncModuleEnum.刊例上下架一微信))
                                return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                            break;
                        case MediaTypeEnum.微博:
                            if (!FuncValidate(FuncModuleEnum.刊例上下架一微博))
                                return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                            break;
                        case MediaTypeEnum.视频:
                            if (!FuncValidate(FuncModuleEnum.刊例上下架一视频))
                                return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                            break;
                        case MediaTypeEnum.直播:
                            if (!FuncValidate(FuncModuleEnum.刊例上下架一直播))
                                return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                            break;
                        case MediaTypeEnum.APP:
                            if (!FuncValidate(FuncModuleEnum.刊例添加一APP))
                                return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                            break;
                    }
                    #endregion
                }
                bool res = BLL.PublishInfo.Instance.ModifyPublishStatus(request);
                XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.刊例管理, Chitunion2017.Common.LogInfo.ActionType.Modify, "调用了修改刊例上下架状态（刊例ID：" + request.PubID + " 状态：" + Enum.GetName(typeof(PublishStatusEnum), request.Status) + "）接口");
                if (!res)
                    BLL.Loger.Log4Net.Info("[PublishController]*****ModifyPublishStatus ->request:" + JsonConvert.SerializeObject(request));
                return Util.GetJsonDataByResult(res, res ? "成功" : "失败", res ? 0 : 1);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[PublishController]*****ModifyPublishStatus ->request:" + JsonConvert.SerializeObject(request) + ",调用修改刊例上下架状态接口出错:" + ex.Message);
                return Util.GetJsonDataByResult(null, "系统异常:" + ex.Message, 500);
            }

        }

        /// <summary>
        /// 新增、修改广告位
        /// ls
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult ModifyADPosition([FromBody]ADPositionDTO request)
        {
            try
            {
                string msg = string.Empty;
                if (!request.CheckSelfModel(out msg))
                {
                    return Util.GetJsonDataByResult(null, msg, -1);
                }
                if (request.ADDetailID.Equals(0) ? !FuncValidate(FuncModuleEnum.刊例添加一APP) : !FuncValidate(FuncModuleEnum.刊例编辑一APP))
                    return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                bool res = false;
                if (request.ADDetailID.Equals(0))
                {
                    res = BLL.PublishInfo.Instance.AddPublishExtendInfo(request);
                    XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.刊例管理, Chitunion2017.Common.LogInfo.ActionType.Modify, "调用了新增广告位（刊例ID：" + request.PubID + "）接口");
                }
                else
                {
                    res = BLL.PublishInfo.Instance.UpdatePublishExtendInfo(request);
                    XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.刊例管理, Chitunion2017.Common.LogInfo.ActionType.Modify, "调用了编辑广告位（刊例ID：" + request.PubID + " 刊例详情ID：" + request.ADDetailID + "）接口");
                }
                if (!res)
                    BLL.Loger.Log4Net.Info("[PublishController]*****ModifyADPosition ->request:" + JsonConvert.SerializeObject(request));
                return Util.GetJsonDataByResult(res, res ? "成功" : "失败", res ? 0 : 1);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[PublishController]*****ModifyADPosition ->request:" + JsonConvert.SerializeObject(request) + ",调用新增编辑广告位接口出错:" + ex.Message);
                return Util.GetJsonDataByResult(null, "系统异常:" + ex.Message, 500);
            }

            #region 测试数据
            //{
            //    "ADDetailID":203,
            //    "PubID": 4,
            //    "AdLegendURL": "http://www.baidu.com",
            //    "AdPosition": "右左下上ABAB",
            //    "AdForm": "Good好好",
            //    "DisplayLength": 10,
            //    "CanClick": true,
            //    "CarouselCount": 5,
            //    "PlayPosition": "下面",
            //    "DailyExposureCount": 5,
            //    "CPM": true,
            //    "CarouselPlay": true,
            //    "DailyClickCount": 9,
            //    "CPM2": true,
            //    "CarouselPlay2": false,
            //    "ThrMonitor": [13001,13002],
            //    "SysPlatform": [12001,12002],
            //    "Style": "形式形式形式",
            //    "IsDispatching": false,
            //    "ADShow": "展示展示展示",
            //    "ADRemark": "备注备注备注备注",
            //    "AcceptBusinessIDs": [2001,2002],
            //    "NotAcceptBusinessIDs": [2003,2004],
            //    "SaleType": 11001,
            //    "Price": 888,
            //    "IsCarousel": true,
            //    "BeginPlayDays": 10
            //}
            #endregion
        }

        /// <summary>
        /// 复制广告位
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult CopyADPosition([FromBody]ADCopyDTO request)
        {
            try
            {
                if (!FuncValidate(FuncModuleEnum.刊例添加一APP))
                    return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                bool res = BLL.PublishInfo.Instance.CopyPublishExtendInfo(request);
                XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.刊例管理, Chitunion2017.Common.LogInfo.ActionType.Modify, "调用了复制广告位（广告位ID：" + request.ADDetailID + "）接口");
                if (!res)
                    BLL.Loger.Log4Net.Info("[PublishController]*****CopyADPosition ->request:" + JsonConvert.SerializeObject(request));
                return Util.GetJsonDataByResult(res, res ? "成功" : "失败", res ? 0 : 1);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[PublishController]*****CopyADPosition ->request:" + JsonConvert.SerializeObject(request) + ",调用复制广告位接口出错:" + ex.Message);
                return Util.GetJsonDataByResult(null, "系统异常:" + ex.Message, 500);
            }
        }

        /// <summary>
        /// 模块功能权限验证
        /// </summary>
        /// <returns></returns>
        private bool FuncValidate(FuncModuleEnum module)
        {
#if DEBUG
            return true;
#endif
            string moduleID = Chitunion2017.Common.UserInfo.SYSID + "BUT" + (int)module;
            return Chitunion2017.Common.UserInfo.CheckRight(moduleID, Chitunion2017.Common.UserInfo.SYSID);
        }

        /// <summary>
        /// Auth:lixiong
        /// 刊例查询接口-前台页面
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Query([FromUri]RequestFrontPublishQueryDto requestDto)
        {
            var jsonResult = Util.Verify<RequestFrontPublishQueryDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            jsonResult.Message = "success";
            jsonResult.Status = 0;
            jsonResult.Result = new PublishQueryProxy(requestDto).GetQuery();
            return jsonResult;
        }

        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult SearchAuto([FromUri]RequestFrontPublishQueryDto requestDto)
        {
            if (requestDto != null)
            {
                requestDto.PageIndex = 1;
                requestDto.PageSize = requestDto.PageSize == 0 ? 10 : requestDto.PageSize;
            }
            var jsonResult = Util.Verify<RequestFrontPublishQueryDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            if (string.IsNullOrWhiteSpace(HttpUtility.UrlDecode(requestDto.Keyword)))
            {
                jsonResult.Message = "请输入搜索关键字";
                return jsonResult;
            }
            jsonResult.Message = "success";
            jsonResult.Status = 0;
            jsonResult.Result = new SearchAutoComplete().GetSeaarchTitle(requestDto);
            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// 刊例查询接口-后台页面
        /// 现阶段AE，媒体主角色的页面基本一样，所以返回的字段一样，前台页面调用接口自己分配所需的字段
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult BackGQuery([FromUri]RequestPublishQueryDto requestDto)
        {
            var jsonResult = Util.Verify<RequestPublishQueryDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            requestDto.CreateUserId = RoleInfoMapping.GetBackQueryUserIdByRole();//后台查询-需注意角色,如果有用户Id ，则带入参数
            if (requestDto.CreateUserId == 0)
            {
                jsonResult.Message = "无此权限访问";
                jsonResult.Status = 400;
                return jsonResult;
            }
            jsonResult.Message = "success";
            jsonResult.Status = 0;
            jsonResult.Result = new PublishQueryProxy(requestDto).GetQuery();
            return jsonResult;
        }

      
    }
}