using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using WebGrease.Css.Extensions;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.ErrorException;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
using JsonResult = XYAuto.ITSC.Chitunion2017.WebAPI.Common.JsonResult;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using Util = XYAuto.ITSC.Chitunion2017.WebAPI.Common.Util;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    /// <summary>
    /// 媒体新增/编辑
    /// auth：李雄
    /// </summary>
    [CrossSite]
    public class MediaController : ApiController
    {
        /// <summary>
        /// 媒体新增/编辑
        /// auth：李雄
        /// </summary>
        /// <param name="publicParam"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Curd([FromBody] RequestMediaPublicParam publicParam)
        {
            var jsonResult = new JsonResult();
            var retValue = GetMediaOperateRetValue(publicParam);
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Result = new OperateResult
            {
                MediaId = retValue.ReturnObject == null
                    ? 0 : retValue.ReturnObject.ToString().ToInt()
            };
            jsonResult.Message = jsonResult.Message ?? "操作成功";
            return jsonResult;
        }

        private ReturnValue GetMediaOperateRetValue(RequestMediaPublicParam publicParam)
        {
            var requestDic = FilterParams(GetWebApiRequestsCollection());
            RequestMediaWeiXinDto requestMediaWeiXin = null;
            RequestMediaWeiBoDto requestMediaWeiBo = null;
            RequestMediaPcAppDto requestMediaPcApp = null;
            RequestMediaVideoDto requestMediaVideo = null;
            RequestMediaBroadcastDto requestMediaBroadcast = null;
            switch (publicParam.BusinessType)
            {
                case (int)MediaType.WeiXin:
                    requestMediaWeiXin = GetRequestEntity<RequestMediaWeiXinDto>(requestDic);
                    break;

                case (int)MediaType.WeiBo:
                    requestMediaWeiBo = GetRequestEntity<RequestMediaWeiBoDto>(requestDic);
                    break;

                case (int)MediaType.APP:
                    requestMediaPcApp = GetRequestEntity<RequestMediaPcAppDto>(requestDic);
                    break;

                case (int)MediaType.Video:
                    requestMediaVideo = GetRequestEntity<RequestMediaVideoDto>(requestDic);
                    break;

                case (int)MediaType.Broadcast:
                    requestMediaBroadcast = GetRequestEntity<RequestMediaBroadcastDto>(requestDic);
                    break;
            }
            var retValue = SetCurdUser(publicParam);//设置用户信息
            if (retValue.HasError) return retValue;
            return new MediaBusinessProxy(publicParam, requestMediaWeiXin, requestMediaWeiBo, requestMediaPcApp, requestMediaVideo,
               requestMediaBroadcast).Excute();
        }

        /// <summary>
        /// 获取webapi当中的请求参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetRequestEntity<T>(Dictionary<string, string> requestDic)
        {
            try
            {
                var requestDto = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(requestDic));
                return requestDto;
            }
            catch (Exception exception)
            {
                BLL.Loger.Log4Net.Error("反序列化失败。", exception);
                throw new RequestParamsException();//暂不定位到具体的参数
            }
        }

        /// <summary>
        /// 获取webapi当中的请求参数集合
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetWebApiRequestsCollection()
        {
            var context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            var request = context.Request;//定义传统request对象
            var requestDic = request.HttpMethod.ToLower() == "get" ?
                request.QueryString.AllKeys.ToDictionary(key => key, key => request.QueryString[key])
                : request.Form.AllKeys.ToDictionary(key => key, key => request.Form[key]);
            return requestDic;
        }

        /// <summary>
        /// 将参数中有NaN的值替换
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        private Dictionary<string, string> FilterParams(Dictionary<string, string> requestParams)
        {
            var fitlerDic = new Dictionary<string, string>();
            requestParams.ForEach(thisValue =>
            {
                if (!thisValue.Value.Equals("NaN", StringComparison.OrdinalIgnoreCase))
                {
                    fitlerDic.Add(thisValue.Key, thisValue.Value);
                }
            });
            return fitlerDic;
        }

        [Obsolete("此类是测试类")]
        public JsonResult Create()
        {
            var requestDic = GetWebApiRequestsCollection();
            var requestDto =
           JsonConvert.DeserializeObject<RequestMediaPublicParam>(JsonConvert.SerializeObject(requestDic));

            return new JsonResult()
            {
                Result = requestDto
            };
        }

        private ReturnValue SetCurdUser(RequestMediaPublicParam publicParam)
        {
            var userId = 0;
            var retValue = new ReturnValue() { HasError = true, ErrorCode = "500", Message = "暂无用户登录信息" };
            try
            {
                var userInfo = Chitunion2017.Common.UserInfo.GetLoginUser();
                if (userInfo == null)
                    return retValue;
                userId = userInfo.UserID; //获取用户id
                publicParam.Source = userInfo.Source;//获取用户是 自营 or 自助
            }
            catch (Exception exception)
            {
                BLL.Loger.Log4Net.ErrorFormat("{0}Curd.SetCurdUser方法出错,异常：{1}", System.Environment.NewLine, exception + (exception.StackTrace ?? string.Empty));
                return retValue;
            }
            publicParam.CreateTime = DateTime.Now;
            publicParam.CreateUserID = userId;
            publicParam.LastUpdateTime = DateTime.Now;
            publicParam.LastUpdateUserID = userId;
            retValue.Message = string.Empty;
            retValue.HasError = false;
            return retValue;
        }

        /// <summary>
        /// 获取媒体列表
        /// ls
        /// </summary>
        /// <param name="QueryMediaListParamsDTO">查询参数实体</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetMediaList([FromUri] QueryMediaListParamsDTO request)
        {
            try
            {
                Entities.DTO.MediaListDTO dto = null;
                switch (request.MediaType)
                {
                    case MediaTypeEnum.微信:
                        dto = BLL.MediaInfo.Instance.GetWeixinList(request.Name, request.Number, request.Source, request.CreateUser, request.BeginDate, request.EndDate,
                            request.CategoryID, request.LevelType, request.IsAuth, request.OrderBy,
                            request.PageIndex, request.PageSize);
                        break;

                    case MediaTypeEnum.APP:
                        dto = BLL.MediaInfo.Instance.GetPCAPPList(request.Name, request.Source, request.CreateUser, request.BeginDate, request.EndDate, request.CategoryID, request.PageIndex, request.PageSize);
                        break;

                    case MediaTypeEnum.微博:
                        dto = BLL.MediaInfo.Instance.GetWeiboList(request.Name, request.Number, request.Source, request.CreateUser, request.BeginDate, request.EndDate,
                            request.CategoryID, request.LevelType, request.IsAuth, request.OrderBy,
                            request.PageIndex, request.PageSize);
                        break;

                    case MediaTypeEnum.视频:
                        dto = BLL.MediaInfo.Instance.GetVideoList(request.Platform, request.Name, request.Number, request.Source, request.CreateUser, request.BeginDate, request.EndDate,
                            request.CategoryID, request.OrderBy, request.PageIndex, request.PageSize);
                        break;

                    case MediaTypeEnum.直播:
                        dto = BLL.MediaInfo.Instance.GetBroadcastList(request.Platform, request.Name, request.Number, request.Source, request.CreateUser, request.BeginDate, request.EndDate,
                                request.CategoryID, request.OrderBy, request.PageIndex, request.PageSize);
                        break;

                    default:
                        return Util.GetJsonDataByResult(dto, "媒体类型不存在！", -1);
                }
                var res = Util.GetJsonDataByResult(dto);
                //LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.媒体管理, Chitunion2017.Common.LogInfo.ActionType.Select, "调用了查询媒体（媒体类型："+Enum.GetName(typeof(MediaTypeEnum),request.MediaType)+"）列表接口");
                return res;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MediaController]*****GetMediaList ->request:" + JsonConvert.SerializeObject(request) + ",调用查询媒体列表接口出错:" + ex.Message);
                return Util.GetJsonDataByResult(null, "系统异常:" + ex.Message, 500);
            }
        }

        /// <summary>
        /// 获取媒体详情
        /// ls
        /// </summary>
        /// <param name="QueryMediaDetailParamsDTO"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetMediaDetail([FromUri] QueryMediaParamsDTO request)
        {
            try
            {
                JsonResult res;
                switch (request.MediaType)
                {
                    case MediaTypeEnum.微信:
                        res = Util.GetJsonDataByResult(BLL.MediaInfo.Instance.GetWeixinDetail(request.MediaID));
                        break;

                    case MediaTypeEnum.APP:
                        res = Util.GetJsonDataByResult(BLL.MediaInfo.Instance.GetPCAPPDetail(request.MediaID));
                        break;

                    case MediaTypeEnum.微博:
                        res = Util.GetJsonDataByResult(BLL.MediaInfo.Instance.GetWeiboDetail(request.MediaID));
                        break;

                    case MediaTypeEnum.视频:
                        res = Util.GetJsonDataByResult(BLL.MediaInfo.Instance.GetVideoDetail(request.MediaID));
                        break;

                    case MediaTypeEnum.直播:
                        res = Util.GetJsonDataByResult(BLL.MediaInfo.Instance.GetBroadcastDetail(request.MediaID));
                        break;

                    default:
                        res = Util.GetJsonDataByResult(null, "媒体类型不存在！", -1);
                        break;
                }
                //LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.媒体管理, Chitunion2017.Common.LogInfo.ActionType.Select, "调用了查询媒体（媒体类型：" + Enum.GetName(typeof(MediaTypeEnum), request.MediaType) + " ID："+request.MediaID+"）详情接口");
                return res;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MediaController]*****GetMediaDetail ->request:" + JsonConvert.SerializeObject(request) + ",调用查询媒体详情接口出错:" + ex.Message);
                return Util.GetJsonDataByResult(null, "系统异常:" + ex.Message, 500);
            }
        }

        /// <summary>
        /// 查询是否存在指定名称或账号的媒体
        /// ls
        /// </summary>
        /// <param name="QueryMediaParamsDTO"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult MediaExists([FromUri] QueryMediaParamsDTO request)
        {
            try
            {
                if (!Enum.IsDefined(typeof(MediaTypeEnum), request.MediaType))
                    return Util.GetJsonDataByResult(null, "媒体类型不存在！", -1);
                var dto = BLL.MediaInfo.Instance.MediaIsExists(request.MediaType, request.MediaID, request.Name, request.Number, false);
                //LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.媒体管理, Chitunion2017.Common.LogInfo.ActionType.Select, "调用了验证媒体（媒体类型：" + Enum.GetName(typeof(MediaTypeEnum), request.MediaType) + " Name：" + request.Name + " Number："+request.Number+"）存在性接口");
                return Util.GetJsonDataByResult(dto, "", dto.IsExists ? 1 : 0);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MediaController]*****MediaExists ->request:" + JsonConvert.SerializeObject(request) + ",调用验证媒体存在接口出错:" + ex.Message);
                return Util.GetJsonDataByResult(null, "系统异常:" + ex.Message, 500);
            }
        }

        /// <summary>
        /// 根据名称或账号获取相似的字典
        /// ls
        /// </summary>
        /// <param name="QueryMediaParamsDTO"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetMediaPairs([FromUri] QueryMediaParamsDTO request)
        {
            try
            {
                List<MediaDictDTO> res = BLL.MediaInfo.Instance.GetMediaDict(request.MediaType, request.Name, request.Number);
                //LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.媒体管理, Chitunion2017.Common.LogInfo.ActionType.Select, "调用了获取媒体（媒体类型：" + Enum.GetName(typeof(MediaTypeEnum), request.MediaType) + " Name：" + request.Name + " Number：" + request.Number + "）字典接口");
                return Util.GetJsonDataByResult(res);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MediaController]*****GetMediaPairs ->request:" + JsonConvert.SerializeObject(request) + ",调用获取媒体字典接口出错:" + ex.Message);
                return Util.GetJsonDataByResult(null, "系统异常:" + ex.Message, 500);
            }
        }
    }
}