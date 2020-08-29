using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using System.Net.Http.Headers;
using XYAuto.ITSC.Chitunion2017.BLL.ADOrderInfoDto.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8;
using System.Xml;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class ADOrderInfoV1_1Controller : ApiController
    {
        private int _currentUserID;

        public int currentUserID
        {
            get
            {
                try
                {
                    _currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    _currentUserID = 1225;
                }
                return _currentUserID;
            }
        }

        private static string LoginPwdKey = Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");

        #region 提交、修改主订单

        [HttpPost]
        [ApiLog]
        public JsonResult AddOrUpdate_ADOrderInfo([FromBody] JObject requestObj)
        {
            try
            {
                RequestADOrderInfoDto requestDto = requestObj.ToObject<RequestADOrderInfoDto>();
                string vmsg = string.Empty;
                string orderid = string.Empty;
                string msg = string.Empty;
                ADOrderInfo.Instance.AddOrderInfo(requestDto, out orderid, out msg);
                if (string.IsNullOrEmpty(msg))
                    return Common.Util.GetJsonDataByResult(null, orderid);

                return Common.Util.GetJsonDataByResult(null, msg, -1);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****AddOrUpdate_ADOrderInfo 出错errormsg:" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错:" + ex.Message, -1);
            }
        }

        #endregion 提交、修改主订单

        #region 根据项目号查看项目

        [HttpGet]
        [ApiLog]
        public JsonResult GetByOrderID_ADOrderInfo(string orderid)
        {
            BLL.Loger.Log4Net.Info(string.Format($"查询项目------项目号<{orderid}>方法开始"));
            //参数验证
            if (string.IsNullOrEmpty(orderid))
            {
                BLL.Loger.Log4Net.Info(string.Format($"查询项目------项目号必填项"));
                return Common.Util.GetJsonDataByResult(null, "项目号是必填项", -1);
            }

            try
            {
                ResponseADOrderInfoDto resDto = new ResponseADOrderInfoDto();
                ResponseADOrderDto resADOrder = null;
                List<ResponseMediaOrderInfoDto> resMOIList = null;
                List<ResponseSubADInfoDto> resSubADInfoList = null;
                ADOrderInfo.Instance.QueryADOrderInfoByOrderIDV1_1_8(orderid, out resADOrder, out resMOIList, out resSubADInfoList);
                try
                {
                    //加密
                    resADOrder.CustomerID = XYAuto.Utils.Security.DESEncryptor.Encrypt(resADOrder.CustomerIDINT.ToString(), LoginPwdKey);
                    //编码
                    resADOrder.CustomerID = System.Web.HttpUtility.UrlEncode(resADOrder.CustomerID, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info("[Delivery_ShoppingCart]QueryAEOrderInfo 客户ID加密编码出错：->" + ex.Message);
                }
                foreach (ResponseSubADInfoDto resSubEntity in resSubADInfoList)
                {
                    if (resSubEntity.MediaType == 14001)
                    {
                        resSubEntity.SelfDetails = ADDetailInfo.Instance.QueryADDetailBySubOrderID<ResponseSelfDetailDto>(resSubEntity.MediaType,
                                resSubEntity.SubOrderID);

                        foreach (var selfEntity in resSubEntity.SelfDetails)
                        {
                            selfEntity.ADSchedules = ADDetailInfo.Instance.QueryADScheduleInfoByADDetailID<ResponseSelfADSchedule>(selfEntity.ADDetailID);
                        }
                    }
                    else if (resSubEntity.MediaType == 14002)
                    {
                        resSubEntity.APPDetails = ADDetailInfo.Instance.QueryADDetailBySubOrderID<ResponseAPPDetailDto>(resSubEntity.MediaType,
                            resSubEntity.SubOrderID);

                        foreach (var appEntity in resSubEntity.APPDetails)
                        {
                            appEntity.ADSchedules = new List<ResponseAPPADScheduleDto>();
                            appEntity.ADSchedules = ADDetailInfo.Instance.QueryADScheduleInfoByADDetailID<ResponseAPPADScheduleDto>(appEntity.ADDetailID);
                            foreach (var adScheduleItem in appEntity.ADSchedules)
                            {
                                List<ResponseHolidayDto> listHoliday = new List<ResponseHolidayDto>();
                                adScheduleItem.Holidays = CartInfo.Instance.p_CALHolidays(adScheduleItem.BeginData, adScheduleItem.EndData, out listHoliday);
                                adScheduleItem.Holiday = listHoliday;
                            }
                        }
                    }
                }
                resDto.ADOrderInfo = resADOrder;
                foreach (var resMOI in resMOIList)
                {
                    if (!string.IsNullOrEmpty(resMOI.UploadFileURL))
                        resMOI.UploadFileName = BLL.Util.GetFileNameByUpload(resMOI.UploadFileURL);
                }
                resDto.MediaOrderInfos = resMOIList;
                resDto.SubADInfos = resSubADInfoList;
                BLL.Loger.Log4Net.Info(string.Format($"查询项目------项目号<{orderid}>方法结束"));
                return Common.Util.GetJsonDataByResult(resDto, "操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format($"查询项目------项目号<{orderid}>方法出错：{ex.Message}"));
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }
        }

        #endregion 根据项目号查看项目

        #region 根据订单号查看订单

        [HttpGet]
        [ApiLog]
        public JsonResult GetBySubOrderID_ADOrderInfo(string suborderid)
        {
            BLL.Loger.Log4Net.Info(string.Format($"查询订单------订单号<{suborderid}>方法开始"));
            //参数验证
            if (string.IsNullOrEmpty(suborderid))
            {
                return Common.Util.GetJsonDataByResult(null, "订单号是必填项", -1);
            }

            try
            {
                ResponseADOrderDto resADOrder = null;
                ResponseMediaOrderInfoDto resMOI = null;
                ResponseSubADInfoDto resSubAInfo = null;
                List<ResponseSelfDetailDto> resSelfDetailList = null;
                List<ResponseAPPDetailDto> resAPPDetailList = null;
                List<ResponseOperateInfoDto> resOperateList = null;
                //不显示过期、失效、下架广告位(Dto:改为显示20170704)
                SubADInfo.Instance.QuerySubADInfoBySubOrderID(suborderid, out resADOrder, out resMOI, out resSubAInfo, out resSelfDetailList,
                    out resAPPDetailList, out resOperateList);

                if (resADOrder != null && !string.IsNullOrEmpty(resADOrder.UploadFileURL))
                    resADOrder.UploadFileName = BLL.Util.GetFileNameByUpload(resADOrder.UploadFileURL);
                if (resMOI != null && !string.IsNullOrEmpty(resMOI.UploadFileURL))
                    resMOI.UploadFileName = BLL.Util.GetFileNameByUpload(resMOI.UploadFileURL);
                try
                {
                    //加密
                    resADOrder.CustomerID = Utils.Security.DESEncryptor.Encrypt(resADOrder.CustomerIDINT.ToString(), LoginPwdKey);
                    //编码
                    resADOrder.CustomerID = System.Web.HttpUtility.UrlEncode(resADOrder.CustomerID, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info("[Delivery_ShoppingCart]QueryAEOrderInfo 客户ID加密编码出错：->" + ex.Message);
                }

                ResponseGetSubADinfoDto resDto = new ResponseGetSubADinfoDto()
                {
                    ADOrderInfo = resADOrder,
                    MediaOrderInfo = resMOI,
                    SubADInfo = resSubAInfo,
                };

                if (resDto?.SubADInfo?.MediaType == 14001)
                {
                    resDto.SubADInfo.SelfDetails = resSelfDetailList;
                    foreach (var selfEntity in resDto.SubADInfo.SelfDetails)
                    {
                        selfEntity.ADSchedules = ADDetailInfo.Instance.QueryADScheduleInfoByADDetailID<ResponseSelfADSchedule>(selfEntity.ADDetailID);
                    }
                }
                else if (resDto?.SubADInfo?.MediaType == 14002)
                {
                    resDto.SubADInfo.APPDetails = resAPPDetailList;
                    foreach (var appEntity in resDto.SubADInfo.APPDetails)
                    {
                        appEntity.ADSchedules = ADDetailInfo.Instance.QueryADScheduleInfoByADDetailID<ResponseAPPADScheduleDto>(appEntity.ADDetailID);
                        foreach (var adScheduleItem in appEntity.ADSchedules)
                        {
                            List<ResponseHolidayDto> listHoliday = new List<ResponseHolidayDto>();
                            adScheduleItem.Holidays = CartInfo.Instance.p_CALHolidays(adScheduleItem.BeginData, adScheduleItem.EndData, out listHoliday);
                            adScheduleItem.Holiday = listHoliday;
                        }
                    }
                }
                BLL.Loger.Log4Net.Info(string.Format($"查询订单------订单号<{suborderid}>方法结束"));
                return Common.Util.GetJsonDataByResult(resDto, "操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format($"查询订单------订单号<{suborderid}>出错：{ex.Message}"));
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }
        }

        #endregion 根据订单号查看订单

        #region 删除项目

        [HttpGet]
        public Common.JsonResult DeleteADOrderInfoByOrderID(string orderid)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******UpdateStatus_ADOrderInfo begin...orerid->" + orderid + "******");
            Common.JsonResult jr = new Common.JsonResult();
            try
            {
                #region 验证参数

                string vermsg = string.Empty;
                if (string.IsNullOrEmpty(orderid))
                {
                    vermsg += "主订单号为必填项!\n";
                }
                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);

                #endregion 验证参数

                //更改主订单状态

                ADOrderInfo.Instance.UpdateStatus_ADOrder(orderid, (int)Entities.EnumOrderStatus.Deleted);
                //更改子订单状态
                SubADInfo.Instance.UpdateStatusByOrderID_SubADInfo(orderid, (int)Entities.EnumOrderStatus.Deleted);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1ControllerUpdateStatus_ADOrderInfo...orerid->" + orderid + ",errormsg:" + ex.Message);
                jr.Status = 2;
                jr.Message = ex.Message;

                return jr;
            }

            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******UpdateStatus_ADOrderInfo end...orerid->" + orderid + "******");
            jr.Message = "更新成功";
            return jr;
        }

        #endregion 删除项目

        #region 信息页查询

        [HttpGet]
        public JsonResult QuerytAuditInfo(int PageIndex = 1, int PageSize = 20)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******QuerytAuditInfo begin...currentUserID:" + currentUserID);
            try
            {
                Chitunion2017.Common.UserRole userrole;
                try
                {
                    userrole = Chitunion2017.Common.UserInfo.GetUserRole();
                }
                catch (Exception)
                {
                    userrole = new Chitunion2017.Common.UserRole(1225, "SYS001RL00004");
                }
                int totalCount = 0;
                List<Entities.WeChatOperateMsg> list_WeChatOperateMsg = WeChatOperateMsg.Instance.GetWeChatOperateMsgList(currentUserID, PageIndex, PageSize, out totalCount);
                JSONWeChatOperateMsg jsonResult_JSONWeChatOperateMsg = new JSONWeChatOperateMsg();
                jsonResult_JSONWeChatOperateMsg.TotalCount = totalCount;
                jsonResult_JSONWeChatOperateMsg.List = new List<JSONQueryWeChatOperateMsg>();
                if (list_WeChatOperateMsg != null && list_WeChatOperateMsg.Count > 0)
                {
                    foreach (Entities.WeChatOperateMsg model_WeChatOperateMsg in list_WeChatOperateMsg)
                    {
                        JSONQueryWeChatOperateMsg jw = null;
                        if (model_WeChatOperateMsg.MediaType == 14001)
                        {
                            #region 微信

                            //媒体主
                            if (userrole.IsMedia)
                            {
                                if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.媒体审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.已通过)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【媒体提醒】：您添加的【微信】{0}已通过审核，立即<a class='red' href={1}>发布广告</a>吧。", model_WeChatOperateMsg.MediaName,
                                            string.Format("/PublishManager/addEditPublishForWeiChat.html?MediaID={0}&entrance=1", model_WeChatOperateMsg.MediaID))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.媒体审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.驳回)
                                {
                                    Entities.Media.MediaWeixin modelWeChat = Dal.Media.MediaWeixin.Instance.GetEntity(model_WeChatOperateMsg.MediaID);
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【媒体提醒】：您添加的【微信】{0}未通过审核，<a class='red' href={1}>立即修改</a>。", model_WeChatOperateMsg.MediaName,
                                            string.Format("/mediamanager/addWeChatmedia.html?WxID={0}&wxae=0&OAuthType={1}&IsAuditPass={2}&q=0", model_WeChatOperateMsg.MediaID, modelWeChat?.AuthType, 43003))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例过期)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：您的【微信】{0}的广告{1}还有{2}天过期，<a class='red' href={3}>立即维护</a>。", model_WeChatOperateMsg.MediaName,
                                        model_WeChatOperateMsg.PublishName,
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期1天 ? 1 :
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期3天 ? 3 :
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期7天 ? 7 :
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期30天 ? 30 : 0,
                                            string.Format("/PublishManager/periodication-see.html?PubID={0}&MediaType=14001", model_WeChatOperateMsg.PublishID))
                                    };
                                }
                            }
                            else if (userrole.IsAE)
                            {
                                if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.已通过)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：您添加的【微信】{0}的广告，已通过审核，<a class='red' href={1}>立即查看</a>。", model_WeChatOperateMsg.MediaName,
                                        string.Format("/PublishManager/periodication-see.html?PubID={0}&MediaType=14001", model_WeChatOperateMsg.PublishID))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.驳回)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：您添加的【微信】{0}的广告，未通过审核，<a class='red' href={1}>立即修改</a>。", model_WeChatOperateMsg.MediaName,
                                            string.Format("/PublishManager/addEditPublishForWeiChat.html?PubID={0}&isAdd=1", model_WeChatOperateMsg.PublishID))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例过期)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：您的【微信】{0}的广告{1}还有{2}天过期，<a class='red' href={3}>立即维护</a>。", model_WeChatOperateMsg.MediaName, model_WeChatOperateMsg.PublishName,
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期1天 ? 1 :
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期3天 ? 3 :
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期7天 ? 7 :
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期30天 ? 30 : 0,
                                        string.Format("/PublishManager/periodication-see.html?PubID={0}&MediaType=14001", model_WeChatOperateMsg.PublishID))
                                    };
                                }
                            }
                            else //if (userrole.IsYY)
                            {
                                if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.待审核)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：{0}的【微信】{1}添加了新的广告，<a class='red' href={2}>立即审核</a>。", model_WeChatOperateMsg.SubmitUserName, model_WeChatOperateMsg.MediaName,
                                            string.Format("/PublishManager/advertisementCheck.html?PubID={0}", model_WeChatOperateMsg.PublishID))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.媒体审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.待审核)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【媒体提醒】：{0}添加了新的{1}，<a class='red' href={2}>立即审核</a>。", model_WeChatOperateMsg.SubmitUserName, model_WeChatOperateMsg.MediaName,
                                            string.Format("/MediaManager/mediaaudit.html?mediaId={0}&MediaType=14001&Wx_Status=43001", model_WeChatOperateMsg.MediaID))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例过期)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：{0}的【微信】{1}的广告{2}还有{3}天过期，<a class='red' href={4}>立即维护</a>。", model_WeChatOperateMsg.SubmitUserName,
                                        model_WeChatOperateMsg.MediaName, model_WeChatOperateMsg.PublishName,
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期1天 ? 1 :
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期3天 ? 3 :
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期7天 ? 7 :
                                        model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期30天 ? 30 : 0,
                                        string.Format("/PublishManager/periodication-see.html?PubID={0}&MediaType=14001", model_WeChatOperateMsg.PublishID))
                                    };
                                }
                            }

                            #endregion 微信
                        }
                        else if (model_WeChatOperateMsg.MediaType == 14002)
                        {
                            #region APP

                            //媒体主
                            if (userrole.IsMedia)
                            {
                                if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.媒体审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.已通过)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【媒体提醒】：您添加的【APP】{0}已通过审核，立即<a class='red' href={1}>发布广告</a>吧。", model_WeChatOperateMsg.MediaName,
                                            WeChatOperateMsg.Instance.HasPulblicTemplate(model_WeChatOperateMsg.MediaID) == true ?
                                            string.Format($"/PublishManager/addPublishForApp.html?MediaID={model_WeChatOperateMsg.MediaID}&PubID=0&TemplateID={model_WeChatOperateMsg.ADTemID}") :
                                             string.Format($"/PublishManager/add_template.html?MediaID={model_WeChatOperateMsg.MediaID}"))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.媒体审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.驳回)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【媒体提醒】：您添加的【APP】{0}未通过审核，<a class='red' href={1}>立即修改</a>。", model_WeChatOperateMsg.MediaName,
                                            model_WeChatOperateMsg.BaseMediaID > 0 ?
                                            string.Format($"/mediamanager/addAppmedia02.html?OperateType=2&MediaId={model_WeChatOperateMsg.MediaID}&BaseMediaid={model_WeChatOperateMsg.BaseMediaID}")
                                            : string.Format($"/mediamanager/addAppmedia.html?OperateType=2&MediaId={model_WeChatOperateMsg.MediaID}&BaseMediaid={model_WeChatOperateMsg.BaseMediaID}"))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例过期)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：您的【APP】{0}的广告还有{1}天过期，<a class='red' href={2}>立即维护</a>。", model_WeChatOperateMsg.MediaName,
                                            model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期1天 ? 1 :
                                            model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期3天 ? 3 :
                                            model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期7天 ? 7 :
                                            model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期30天 ? 30 : 0,
                                            string.Format("/publishmanager/Advertisinglist_app.html?mediaName={0}", model_WeChatOperateMsg.MediaName))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.广告下架)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：您的【APP】{0}有广告已下架，赶紧去<a class='red' href={1}>看看</a>吧。", model_WeChatOperateMsg.MediaName,
                                            string.Format("/publishmanager/Advertisinglist_app.html?mediaName={0}", model_WeChatOperateMsg.MediaName))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.模板审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.模板已通过)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【模板提醒】：您好，您编辑的模板[{0}]已审核通过，赶紧去<a class='red' href={1}>添加广告价格</a>吧。", model_WeChatOperateMsg.ADTemName,
                                            string.Format($"/PublishManager/addPublishForApp.html?MediaID={model_WeChatOperateMsg.MediaID}&PubID=0&TemplateID={model_WeChatOperateMsg.ADTemID}"))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.模板审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.模板驳回)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【模板提醒】：您好，您编辑的模板[{0}]未通过审核，您可以<a class='red' href={1}>修改</a>之后再次提交审核。", model_WeChatOperateMsg.MediaName,
                                            string.Format($"/PublishManager/audit_price.html?MediaID={model_WeChatOperateMsg.MediaID}&TemplateID={model_WeChatOperateMsg.ADTemID}"))
                                    };
                                }
                            }
                            else if (userrole.IsAE)
                            {
                                if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.模板审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.模板已通过)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【模板提醒】：您好，您编辑的模板[{0}]已审核通过，赶紧去<a class='red' href={1}>添加广告价格</a>吧。", model_WeChatOperateMsg.ADTemName,
                                            string.Format($"/PublishManager/addPublishForApp.html?MediaID={model_WeChatOperateMsg.MediaID}&PubID=0&TemplateID={model_WeChatOperateMsg.ADTemID}"))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.模板审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.模板驳回)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【模板提醒】：您好，您编辑的模板[{0}]未通过审核，您可以<a class='red' href={1}>修改</a>之后再次提交审核。", model_WeChatOperateMsg.MediaName,
                                            $"/PublishManager/audit_price.html?MediaID={model_WeChatOperateMsg.MediaID}&TemplateID={model_WeChatOperateMsg.ADTemID}")
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.已通过)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：您添加的【APP】{0}的广告，已通过审核，<a class='red' href={1}>立即查看</a>。", model_WeChatOperateMsg.MediaName,
                                        string.Format("/publishmanager/Advertisinglist_app.html?mediaName={0}", model_WeChatOperateMsg.MediaName))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.驳回)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：您添加的【APP】{0}的广告，未通过审核，<a class='red' href={1}>立即修改</a>。", model_WeChatOperateMsg.MediaName,
                                            string.Format($"/PublishManager/addPublishForApp.html?MediaID={model_WeChatOperateMsg.MediaID}&PubID={model_WeChatOperateMsg.PublishID}&TemplateID=0"))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例过期)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：您的【APP】{0}的广告还有{1}天过期，<a class='red' href={2}>立即维护</a>。", model_WeChatOperateMsg.MediaName,
                                            model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期1天 ? 1 :
                                            model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期3天 ? 3 :
                                            model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期7天 ? 7 :
                                            model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期30天 ? 30 : 0,
                                        string.Format("/publishmanager/Advertisinglist_app.html?mediaName={0}", model_WeChatOperateMsg.MediaName))
                                    };
                                }
                            }
                            else //if (userrole.IsYY)
                            {
                                if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.模板审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.模板待审核)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：{0}的【APP】{1}添加了新的广告模板，<a class='red' href={2}>立即审核</a>。", model_WeChatOperateMsg.SubmitUserName,
                                            model_WeChatOperateMsg.MediaName,
                                            string.Format("/publishmanager/advTempAudit.html?TemplateID=", model_WeChatOperateMsg.ADTemID))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.媒体审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.待审核)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【媒体提醒】：{0}添加了新的{1}，<a class='red' href={2}>立即审核</a>。", model_WeChatOperateMsg.SubmitUserName,
                                            model_WeChatOperateMsg.MediaName,
                                            string.Format($"/mediamanager/appAuditing.html?MediaId={model_WeChatOperateMsg.MediaID}"))
                                    };
                                }
                                else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.待审核)
                                {
                                    jw = new JSONQueryWeChatOperateMsg()
                                    {
                                        CreateTime = model_WeChatOperateMsg.CreateTime,
                                        Msg = string.Format("【广告提醒】：{0}的【APP】{1}添加了新的广告，<a class='red' href={2}>立即审核</a>。", model_WeChatOperateMsg.SubmitUserName,
                                            model_WeChatOperateMsg.MediaName,
                                            string.Format($"/PublishManager/audit_price.html?PubID={model_WeChatOperateMsg.PublishID}"))
                                    };
                                }
                            }

                            #endregion APP
                        }
                        if (jw != null)
                            jsonResult_JSONWeChatOperateMsg.List.Add(jw);
                        else
                        {
                            jsonResult_JSONWeChatOperateMsg.TotalCount--;
                        }
                    }

                    //try
                    //{
                    //    string listd = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult_JSONWeChatOperateMsg);
                    //    BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****QuerytAuditInfo->" + listd + "******");
                    //}
                    //catch (Exception ex)
                    //{ }
                    return Common.Util.GetJsonDataByResult(jsonResult_JSONWeChatOperateMsg, "操作成功");
                }
                else
                {
                    return Common.Util.GetJsonDataByResult(null, "没有数据", -1);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]QuerytAuditInfo...->errormsg:" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }
        }

        #endregion 信息页查询

        #region 信息页更新为已读

        [HttpGet]
        [ApiLog]
        public JsonResult UpdateAuditInfo()
        {
            try
            {
                string vermsg = string.Empty;
                int totalCount = 0;
                vermsg = BLL.WeChatOperateMsg.Instance.p_WeChatOperateMsg_UpdateReadV1_1(2, currentUserID, out totalCount);

                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);

                return Common.Util.GetJsonDataByResult(null, "操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]UpdateAuditInfo...->errormsg:" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }
        }

        #endregion 信息页更新为已读

        #region 根据微信号或名称模类查询V1.1

        [HttpGet]
        [ApiLog]
        public JsonResult QueryWeChat_NumerOrName(string NumberORName, int AuditStatus)
        {
            try
            {
                #region 验证参数

                string vermsg = string.Empty;
                if (string.IsNullOrEmpty(NumberORName))
                    vermsg += "参数NumberORName为必填项/n";

                if (AuditStatus != (int)Entities.EnumWeChatOperateMsg.待审核 &&
                    AuditStatus != (int)Entities.EnumWeChatOperateMsg.已通过 &&
                    AuditStatus != (int)Entities.EnumWeChatOperateMsg.驳回 &&
                    AuditStatus != -2 &&
                    AuditStatus != -4)
                    vermsg += "参数AuditStatus错误/n";

                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);

                #endregion 验证参数

                List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
                vermsg = BLL.ADOrderInfo.Instance.QueryWeChat_NumerOrNameV1_1(currentUserID, NumberORName, AuditStatus, out dicList);

                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);

                //try
                //{
                //    string listd = Newtonsoft.Json.JsonConvert.SerializeObject(Common.Util.GetJsonDataByResult(dicList, "操作成功"));
                //    BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****QueryWeChat_NumerOrNameV1_1->" + listd + "******");
                //}
                //catch (Exception ex)
                //{ }

                return Common.Util.GetJsonDataByResult(dicList, "操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]QueryWeChat_NumerOrNameV1_1...->errormsg:" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }
        }

        #endregion 根据微信号或名称模类查询V1.1

        #region 根据APP名称模类查询V1.4

        [HttpGet]
        [ApiLog]
        public JsonResult QueryAPPByName(string Name, int AuditStatus)
        {
            try
            {
                #region 验证参数

                string vermsg = string.Empty;
                if (string.IsNullOrEmpty(Name))
                    vermsg += "参数Name为必填项/n";

                if (AuditStatus != (int)Entities.EnumWeChatOperateMsg.待审核 &&
                    AuditStatus != (int)Entities.EnumWeChatOperateMsg.已通过 &&
                    AuditStatus != (int)Entities.EnumWeChatOperateMsg.驳回 &&
                    AuditStatus != -2 &&
                    AuditStatus != -4 &&
                    AuditStatus != -5)
                    vermsg += "参数AuditStatus错误/n";

                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);

                #endregion 验证参数

                List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
                vermsg = BLL.ADOrderInfo.Instance.p_APPSelectByName(currentUserID, Name, AuditStatus, out dicList);

                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);

                if (dicList.Count == 0)
                    return Common.Util.GetJsonDataByResult(null, "没有数据");
                return Common.Util.GetJsonDataByResult(dicList, "操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]QueryAPPByName...->errormsg:" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }
        }

        #endregion 根据APP名称模类查询V1.4

        #region 根据个人姓名、公司名称或手机号模糊查询广告主

        [HttpGet]
        [ApiLog]
        public Common.JsonResult GetADMaster(string NameOrMobile, int IsAEAuth)
        {
            try
            {
                List<ResponseADMasterDto> admasterlist = new List<ResponseADMasterDto>();
                if (IsAEAuth == 2)
                {
                    CRMWebService.CustomerServiceSoapClient crmWeb = new CRMWebService.CustomerServiceSoapClient();
                    string ret = crmWeb.GetCustomByEmpNoOrCustName(NameOrMobile, "");

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(ret);
                    XmlNodeList xmlCust = doc.GetElementsByTagName("Table");

                    var query = from XmlNode node in xmlCust
                                select new
                                {
                                    UserID = node.LastChild.InnerText,
                                    UserName = node.FirstChild.InnerText,
                                    Mobile = string.Empty,
                                    TrueName = node.FirstChild.InnerText
                                };

                    if (query.ToList().Count > 0)
                        return WebAPI.Common.Util.GetJsonDataByResult(query, "操作成功");
                    return WebAPI.Common.Util.GetJsonDataByResult(null, "没有数据", -1);
                }
                System.Data.DataTable dt = BLL.ADOrderInfo.Instance.p_ADMaster_SelectV1_1_8(currentUserID, NameOrMobile, IsAEAuth);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return WebAPI.Common.Util.GetJsonDataByResult(null, "没有数据", -1);
                }

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    ResponseADMasterDto admaster = new ResponseADMasterDto();
                    admaster.UserID = Convert.ToString(row["UserID"]);
                    //加密
                    admaster.UserID = XYAuto.Utils.Security.DESEncryptor.Encrypt(admaster.UserID, LoginPwdKey);
                    //编码
                    admaster.UserID = System.Web.HttpUtility.UrlEncode(admaster.UserID, Encoding.UTF8);

                    admaster.UserName = Convert.ToString(row["UserName"]);
                    admaster.Mobile = Convert.ToString(row["Mobile"]);
                    admaster.TrueName = Convert.ToString(row["TrueName"]);
                    admasterlist.Add(admaster);
                }
                return WebAPI.Common.Util.GetJsonDataByResult(admasterlist, "", 0);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]******GetADMaster end...currentUserID->" + currentUserID + ",NameOrMobile->" + NameOrMobile + ",errorMsg:" + ex.Message);
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }

        #endregion 根据个人姓名、公司名称或手机号模糊查询广告主

        #region 根据项目号查看二维码页面

        [HttpGet]
        [ApiLog]
        public JsonResult GetTwoBarCodeHistory(string orderID)
        {
            try
            {
                if (string.IsNullOrEmpty(orderID))
                    return WebAPI.Common.Util.GetJsonDataByResult(null, "项目号不能为空!", -1);

                GetTwoBarCodeHistoryDto resDto = new GetTwoBarCodeHistoryDto();
                List<TwoBarCodeDetailDto> listDto = new List<TwoBarCodeDetailDto>();
                listDto = BLL.ADOrderInfo.Instance.GetTwoBarCodeHistory(orderID, out resDto);
                resDto.TwoBarCodeHistory = listDto;
                if (listDto?.Count > 0)
                    return WebAPI.Common.Util.GetJsonDataByResult(resDto, "操作成功");

                return WebAPI.Common.Util.GetJsonDataByResult(null, "没有数据", -1);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[ADOrderInfoController]GetTwoBarCodeHistory 出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }

        #endregion 根据项目号查看二维码页面

        #region 根据投放城市、投放日期、已投放媒体帐号查询广告位

        [HttpGet]
        [ApiLog]
        public JsonResult IntelligenceRecommend_PubQuery(string MeidiaNumbers, DateTime LaunchTime, int ProvinceID = -2, int CityID = -2)
        {
            try
            {
                if (ProvinceID == -2)
                    return WebAPI.Common.Util.GetJsonDataByResult(null, "省份ID必填项!", -1);

                List<GetPublishDetailForRecommendAddPDDto> listDto = new List<GetPublishDetailForRecommendAddPDDto>();
                listDto = BLL.ADOrderInfo.Instance.GetPublishDetailForRecommendAddPD(MeidiaNumbers, LaunchTime, ProvinceID, CityID);

                if (listDto?.Count > 0)
                    return WebAPI.Common.Util.GetJsonDataByResult(listDto, "操作成功");

                return WebAPI.Common.Util.GetJsonDataByResult(null, "没有数据", -1);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[ADOrderInfoController]IntelligenceRecommend_PubQuery出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }

        #endregion

        #region 智投推荐

        [HttpPost]
        [ApiLog]
        public JsonResult IntelligenceRecommend([FromBody] RequestIntelligenceRecommendDto reqDto)
        {
            try
            {
                TimeSpan ts = reqDto.LaunchTime.Date.Subtract(DateTime.Now.Date);
                if (ts.Days < 20)
                    return WebAPI.Common.Util.GetJsonDataByResult("验证错误", "预计投放日期只能选择20天以后的日期", -1);
                List<ResponseIntelligenceRecommendDto> listDto = new List<ResponseIntelligenceRecommendDto>();
                listDto = BLL.ADOrderInfo.Instance.IntelligenceRecommend(reqDto);

                if (listDto?.Count > 0)
                    return WebAPI.Common.Util.GetJsonDataByResult(listDto, "操作成功");

                return WebAPI.Common.Util.GetJsonDataByResult(null, "没有数据", -1);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[ADOrderInfoController]IntelligenceRecommend出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }

        #endregion

        #region 提交、修改智投项目
        [HttpPost]
        [ApiLog]
        public JsonResult IntelligenceADOrderInfoCrud([FromBody] RequestIntelligenceADOrderInfoCrudDto reqDto)
        {
            try
            {
                BLL.Loger.Log4Net.Info("提交修改智投项目-----接口开始");
                string orderid = string.Empty;
                string msg = string.Empty;
                BLL.ADOrderInfo.Instance.IntelligenceADOrderInfoCrud(reqDto, out orderid, out msg);
                if (!string.IsNullOrEmpty(msg))
                    return Common.Util.GetJsonDataByResult(null, $"验证失败：{msg}");
                BLL.Loger.Log4Net.Info("提交修改智投项目-----接口结束");
                return Common.Util.GetJsonDataByResult(null, orderid);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[ADOrderInfoV1_1Controller]:IntelligenceADOrderInfoCrud出错：{ex.Message}");
                return Common.Util.GetJsonDataByResult(null, $"出错:{ex.Message}", -1);
            }
        }

        #endregion 提交、修改智投项目

        #region 根据项目号查看智投项目

        [HttpGet]
        [ApiLog]
        public JsonResult IntelligenceADOrderInfoQuery(string orderID)
        {
            try
            {
                if (string.IsNullOrEmpty(orderID))
                    return WebAPI.Common.Util.GetJsonDataByResult(null, "项目号不能为空!", -1);

                IntelligenceADOrderInfoQueryDto resDto = new IntelligenceADOrderInfoQueryDto();
                resDto = BLL.ADOrderInfo.Instance.IntelligenceADOrderInfoQuery(orderID);
                if (resDto != null)
                    return WebAPI.Common.Util.GetJsonDataByResult(resDto, "操作成功");

                return WebAPI.Common.Util.GetJsonDataByResult(null, "没有数据", -1);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[ADOrderInfoController]GetTwoBarCodeHistory 出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }

        #endregion

        #region 智投推荐导出媒体

        [HttpPost]
        [ApiLog]
        //public HttpResponseMessage IntelligenceRecommendExport([FromBody] BLL.V1_8_3.Dto.IRecommendExportDto exportDto)
        //{
        //    var response = new HttpResponseMessage();
        //    try
        //    {
        //        string msg = string.Empty;
        //        var ret = BLL.V1_8_3.IntelligenceADOrderInfo.Instance.IntelligenceRecommendExport(exportDto, out msg);
        //        if(!string.IsNullOrWhiteSpace(msg))
        //        {
        //            response.StatusCode = HttpStatusCode.InternalServerError;
        //            response.Content = new StringContent($"验证失败：{msg}");
        //            return response;
        //        }

        //        response.StatusCode = HttpStatusCode.OK;
        //        response.Content = new StreamContent(new FileStream(ret.Item1, FileMode.Open, FileAccess.Read));

        //        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //        {
        //            FileName =System.Web.HttpUtility.UrlEncode(ret.Item1, System.Text.Encoding.UTF8),
        //            Name = System.Web.HttpUtility.UrlEncode(ret.Item2.Replace(".xlsx", ""), System.Text.Encoding.UTF8)
        //        };
        //        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        BLL.Loger.Log4Net.Info($"IntelligenceRecommendExport出错：{ex.Message}");
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //        response.Content = new StringContent($"出错：{ex.Message}");
        //        return response;
        //    }
        //}
        public JsonResult IntelligenceRecommendExport([FromBody] BLL.V1_8_3.Dto.IRecommendExportDto exportDto)
        {
            try
            {
                string msg = string.Empty;
                var ret = BLL.V1_8_3.IntelligenceADOrderInfo.Instance.IntelligenceRecommendExport(exportDto, out msg);
                if (!string.IsNullOrWhiteSpace(msg))
                    return WebAPI.Common.Util.GetJsonDataByResult("验证失败", msg, -1);
                return WebAPI.Common.Util.GetJsonDataByResult(System.Web.HttpUtility.UrlEncode(ret.Item1, Encoding.UTF8));
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"IntelligenceRecommendExport出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }

        #endregion

        #region 智投项目导出媒体

        [HttpGet]
        [ApiLog]        
        public JsonResult IntelligenceRecommendADOrderExport(string orderID,string orderName)
        {
            try
            {                
                string msg = string.Empty;
                var ret = BLL.V1_8_3.IntelligenceADOrderInfo.Instance.IntelligenceRecommendADOrderExport(orderID, orderName, out msg);
                if (!string.IsNullOrWhiteSpace(msg))
                    return WebAPI.Common.Util.GetJsonDataByResult("验证失败", msg, -1);
                return WebAPI.Common.Util.GetJsonDataByResult(System.Web.HttpUtility.UrlEncode(ret, Encoding.UTF8));
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"IntelligenceRecommendADOrderExport出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }

        #endregion

        #region 根据条件查询订单列表

        /// <summary>
        /// 2017-02-24 张立彬
        /// 根据条件查询订单信息
        /// </summary>
        /// <param name="orderType">订单类型（0:主订单；1：子订单）</param>
        /// <param name="orderNum">  订单编号（模糊查找）</param>
        /// <param name="CustomerId">  广告主ID（精确查找）</param>
        /// <param name="demandDescribe">需求名称（模糊查询</param>
        /// <param name="startTime">执行周期开始时间</param>
        /// <param name="endTime">执行周期结束时间</param>
        /// <param name="mediaType">资源类型 （全部为0）</param>
        /// <param name="creater">创建人(模糊查询）</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="PageIndex">第几页</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="OrderSource"> 项目类型(0全部 1代客下单 2自助下单) </param>
        ///  <param name="IsCRM"> 是否关联CRM(0全部 1关联CRM 2否 不关联CRM) </param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult SelectOrderInfo(int orderType, string orderNum, string CustomerID, string demandDescribe, int mediaType, string creater, int orderState, int OrderSource, int IsCRM, string SubOrderNum, int pagesize = 20, int PageIndex = 1)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                if ((!Enum.IsDefined(typeof(Entities.EnumMediaType), mediaType) && mediaType != 0) || !Enum.IsDefined(typeof(Entities.EnumOrderStatus), orderState) || (orderType != 0 && orderType != 1) || (OrderSource != 0 && OrderSource != 1 && OrderSource != 2 && OrderSource != 3) || (IsCRM != 0 && IsCRM != 1 && IsCRM != 2))

                {
                    return Common.Util.GetJsonDataByResult(false, "参数错误", -1);
                }
                //TODO: 代码走查 - 代码优化：bool变量,Add = masj,Date = 2017-05-10
                bool resultOrder = AuthorizationCommon.OrderSelectVerification(orderType);
                if (!resultOrder)
                {
                    return Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                }

                dic = BLL.ADOrderInfo.Instance.SelectOrderInfo(orderType, orderNum == null ? orderNum : orderNum.Trim(), demandDescribe == null ? demandDescribe : demandDescribe.Trim(), mediaType, creater == null ? creater : creater.Trim(), pagesize, PageIndex, orderState, CustomerID, OrderSource, IsCRM, SubOrderNum == null ? SubOrderNum : SubOrderNum.Trim());
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****SelectOrderInfo ->orderNum:" + orderNum + " ->orderType:" + orderType + ",查询订单信息出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        #endregion 根据条件查询订单列表

        #region 信息页实体

        public class JSONQueryWeChatOperateMsg
        {
            public string Msg { get; set; }
            public DateTime CreateTime { get; set; }
        }

        public class JSONWeChatOperateMsg
        {
            public List<JSONQueryWeChatOperateMsg> List { get; set; }
            public int TotalCount { get; set; }
        }

        #endregion 信息页实体

        #region 1.1.8

        [HttpPost]
        /// <summary>
        ///  修改CRM编号
        /// </summary>
        /// <param name="OrderNum">订单号</param>
        /// <param name="CrmNum">CRM编号</param>
        /// <returns></returns>
        public JsonResult UpdateCrmNum(UpdateCrmNumResDTO dto)
        {
            string strJson = Json.JsonSerializerBySingleData(dto);
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******UpdateCrmNum begin...->dto:" + strJson + "");
            string messageStr = "";
            try
            {
                messageStr = BLL.ADOrderInfo.Instance.UpdateCrmNum(dto);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****UpdateCrmNum ->dto:" + strJson + ",修改CRM编号出错:" + ex.Message);
                throw ex;
            }
            JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******UpdateCrmNum end->");
            return jr;
        }

        /// <summary>
        /// zlb 2017-07-25
        /// 保存发文链接
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePostingAddressBySubID(UpdatePostingAddressResDTO dto)
        {
            string strJson = Json.JsonSerializerBySingleData(dto);
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******UpdatePostingAddressBySubID begin...->dto:" + strJson + "");
            string messageStr = "";
            try
            {
                messageStr = BLL.ADOrderInfo.Instance.UpdatePostingAddressBySubID(dto);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****UpdatePostingAddressBySubID ->dto:" + strJson + ",保存发文地址失败:" + ex.Message);
                throw ex;
            }
            JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******UpdatePostingAddressBySubID end->");
            return jr;
        }

        /// <summary>
        /// zlb 2017-0-27
        /// 下载结案报告
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectFinalReportUrlByOrderID(string OrderID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                string errorMessage;
                string result = BLL.ADOrderInfo.Instance.SelectFinalReportUrlByOrderID(OrderID, out errorMessage);
                if (errorMessage != "")
                {
                    return Common.Util.GetJsonDataByResult(null, errorMessage, -1);
                }
                else
                {
                    return Common.Util.GetJsonDataByResult(result, "Success", 0);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****SelectFinalReportUrlByOrderID ->OrderID:" + OrderID + ",下载结案报告失败:" + ex.Message);
                throw ex;
            }
        }
       /// <summary>
       /// zlb 2017-07-31
       /// 根据子订单ID查看发文地址
       /// </summary>
       /// <param name="SubOrderID">子单ID</param>
       /// <returns></returns>
        [HttpGet]
        public JsonResult SelectPostingAddress(string SubOrderID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                string result = BLL.ADOrderInfo.Instance.SelectPostingAddress(SubOrderID);
                return Common.Util.GetJsonDataByResult(result, "Success", 0);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****SelectPostingAddress ->SubOrderID:" + SubOrderID + ",查看发文地址失败:" + ex.Message);
                throw ex;
            }
        }


        #endregion 1.1.8

        #region 二维码相关

        /// <summary>
        /// Auth:lixiong
        /// Desc:打包下载
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public HttpResponseMessage DownloadZip([FromUri] string ids)
        {
            var response = new HttpResponseMessage();

            if (string.IsNullOrWhiteSpace(ids))
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent("请输入参数");
            }
            try
            {
                var provider = new TwoBarCodeHistoryProvider(new RequestTwoBarCodeDto()
                {
                }, new ConfigEntity());

                var retValue = provider.GetTempPhysicalPath(ids);

                //
                ContentDispositionHeaderValue disposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName =
                        System.Web.HttpUtility.UrlEncode(retValue.Item3, System.Text.Encoding.UTF8),
                    Name = System.Web.HttpUtility.UrlEncode(retValue.Item3.Replace(".zip", ""), System.Text.Encoding.UTF8),
                };
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StreamContent(new FileStream(retValue.Item1, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = disposition;
                //new ContentDispositionHeaderValue("attachment") { FileName = retValue.Item3 };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.ToString());
            }
            return response;
        }

        /// <summary>
        /// Auth:lixiong
        /// Desc:生成二维码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult Generate([FromBody]RequestTwoBarCodeDto request)
        {
            var jsonResult = new JsonResult();
            var userInfo = UserInfo.GetLoginUser();
            var config = new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                //RoleTypeEnum = RoleInfoMapping.GetUserRole(userInfo.UserID),
                UserType = (UserTypeEnum)userInfo.Type
            };
            var provider = new TwoBarCodeHistoryProvider(request, config);

            var retValue = provider.Excute();
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Message = jsonResult.Message ?? "操作成功";
            return jsonResult;
        }

        #endregion 二维码相关

        #region 选择渠道

        /// <summary>
        /// Auth:lixiong
        /// Desc:选择渠道谈层
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetChannelList([FromUri]RequestGetChannelDto request)
        {
            var jsonResult = new JsonResult { Result = new ChannelInfoProvider().Query(request) };

            return jsonResult;
        }

        #endregion 选择渠道
    }
}