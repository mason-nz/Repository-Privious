using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.V2_3;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.VerifyUser;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [CrossSite]

    public class TaskController : ApiController
    {
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]

        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetTaskListByUserId([FromUri]TaskResDTO res)
        {
            DateTime dtStart = DateTime.Now;
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetTaskListByUserId->开始时间：{dtStart}");
            TimeSpan ts1 = new TimeSpan(dtStart.Ticks);
            JsonResult jsonResult = null;
            //int totalcount = 0;
            try
            {

                int userId = -2;
                try
                {
                    userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception ex)
                {
                    userId = 1637;
                }
                res.UserID = userId;
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetTaskListByUserId->获取用户ID耗时：{ts1.Subtract(ts2).Duration().TotalMilliseconds.ToString()}毫秒");
                //var list = LE_Task.Instance.GetDataByPage(res, out totalcount);



                var list = LE_Task.Instance.GetDataByPageV2_5(res);
                TimeSpan ts3 = new TimeSpan(DateTime.Now.Ticks);
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetTaskListByUserId->获取数据耗时：{ts2.Subtract(ts3).Duration().TotalMilliseconds.ToString()}毫秒");
                Dictionary<string, object> retDic = new Dictionary<string, object>();
                retDic.Add("List", list);
                var sceneModel = XYAuto.ITSC.Chitunion2017.BLL.WeChat.LE_WXUserScene.Instance.GetSceneByID(res.SceneID);
                string sceneName = string.Empty;
                if (sceneModel != null)
                    sceneName = sceneModel.SceneName;
                retDic.Add("SceneName", sceneName);
                jsonResult = Util.GetJsonDataByResult(retDic, "Success", 0);
                TimeSpan ts4 = new TimeSpan(DateTime.Now.Ticks);
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetTaskListByUserId->格式化数据耗时：{ts3.Subtract(ts4).Duration().TotalMilliseconds.ToString()}毫秒");
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[GetTaskListByUserId]报错", ex);

                jsonResult = Util.GetJsonDataByResult(null, $"出错：{ex.Message}", -1);
            }
            DateTime dtEnd = DateTime.Now;
            TimeSpan ts5 = new TimeSpan(dtEnd.Ticks);
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetTaskListByUserId->结束时间：{dtEnd} 总耗时：{ts1.Subtract(ts5).Duration().TotalMilliseconds.ToString()}毫秒");
            return jsonResult;
        }

        /// <summary>
        /// 根据用户ID获取场景
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        //[HttpGet]
        //public JsonResult GetSceneInfoByUserId(int sencetype)
        //{
        //    JsonResult jsonResult = null;
        //    try
        //    {
        //        int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
        //        List<WXUserSceneRspDTO> list = XYAuto.ITSC.Chitunion2017.BLL.LETask.V2_3.LE_WXUserScene.Instance.GetUserSceneByUserId(sencetype, userId);
        //        jsonResult = Util.GetJsonDataByResult(list, "Success", 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[GetSceneInfoByUserId]报错", ex);
        //        jsonResult = Util.GetJsonDataByResult("操作失败", ex.Message, -1);
        //    }
        //    return jsonResult;
        //}

        [HttpGet]
        public JsonResult IsSelectedSceneByUser()
        {
            JsonResult jsonResult = null;
            try
            {
                int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                var result = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserSence(userId);
                jsonResult = Util.GetJsonDataByResult(result, "Success", 0);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[IsSelectedSceneByUser]报错", ex);

                jsonResult = Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
            return jsonResult;
        }


        /// <summary>
        /// 更新用户场景
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult UpdateUserScene()
        {
            JsonResult jsonResult = null;
            try
            {
                var sr = new StreamReader(HttpContext.Current.Request.InputStream);
                var stream = sr.ReadToEnd();
                ITSC.Chitunion2017.Entities.DTO.V2_3.WXUserSceneResDTO res = Newtonsoft.Json.JsonConvert.DeserializeObject<ITSC.Chitunion2017.Entities.DTO.V2_3.WXUserSceneResDTO>(stream);
                int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                bool result = XYAuto.ITSC.Chitunion2017.BLL.LETask.V2_3.LE_WXUserScene.Instance.UpdateWeiXinUserScene(userId, res);
                jsonResult = Util.GetJsonDataByResult(result, "Success", 0);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[UpdateUserScene]报错", ex);

                jsonResult = Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
            return jsonResult;
        }

        /// <summary>
        /// 根据openid 获取UnionId 和 UserId
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetUnionAndUserId()
        {
            JsonResult jsonResult = null;
            try
            {
                int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(userId);
                if (user == null)
                    return Util.GetJsonDataByResult("操作失败", $"UserID:{userId},未找到用户", -2);
                var result = WeiXinUser.Instance.GetUnionAndUserId(user.openid);
                jsonResult = Util.GetJsonDataByResult(result, "Success", 0);

            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[GetUnionAndUserId]报错", ex);

                jsonResult = Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
            return jsonResult;
        }

        /// <summary>
        /// 根据ID获取分享信息
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetShareOrderInfo([FromUri]int OrderId)
        {
            JsonResult jsonResult = null;
            try
            {
                var result = XYAuto.ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.GetInfoByOrderId(OrderId);
                jsonResult = Util.GetJsonDataByResult(result, "Success", 0);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[GetShareOrderInfo]报错", ex);

                jsonResult = Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
            return jsonResult;
        }

        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetOrderByStatus(int status, int pageindex = 1, int pagesize = 10)
        {
            JsonResult jsonResult = null;
            try
            {
                int totalCount = 0;
                int userId = 0;
                string openid = HttpContext.Current.Request["openid"];
                if (openid != null && openid.Length > 0)
                {
                    var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfo(openid);
                    userId = user.UserID;
                }
                else
                {
                    userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                ITSC.Chitunion2017.Entities.DTO.V2_3.OrderResDTO list = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetOrderByStatus(userId, status, pageindex, pagesize, out totalCount);
                jsonResult = Util.GetJsonDataByResult(list, "Success", 0);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[GetOrderByStatus]报错", ex);

                jsonResult = Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
            return jsonResult;
        }


        /// <summary>
        /// 获取临时订单URL
        /// </summary>
        /// <param name="MaterialID"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetOrderUrl(int MaterialID)
        {
            JsonResult jsonResult = null;
            try
            {
                int userId = -2;
                string orderurl = string.Empty;
                try
                {
                    userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID(); ;
                }
                catch (Exception ex)
                {
                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[GetOrderUrl]GetLoginUserID 报错:", ex);
                    userId = -1;
                }
                int taskId = XYAuto.ITSC.Chitunion2017.BLL.LETask.V2_3.LE_Task.Instance.GetTaskIdByMaterialID(MaterialID);
                var task = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetTaskInfo(taskId);
                if (userId > 0)
                    orderurl = XYAuto.ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.GetOrderUrl(taskId, userId);
                else
                    orderurl = task.MaterialUrl;
                //if (orderurl.Contains("ct_m"))
                //{
                //    int index = orderurl.IndexOf("ct_m");
                //    orderurl = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("DominArticle") + orderurl.Substring(index - 1);
                //}



                XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3.TemporaryOrderRepDTO entity = new TemporaryOrderRepDTO { TaskId = taskId, OrderUrl = ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.GetDomainByRandom_ShareArticle(orderurl), Synopsis = task.Synopsis, TaskName = task.TaskName, ImgUrl = task.ImgUrl };

                jsonResult = Util.GetJsonDataByResult(entity, "Success", 0);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[GetOrderUrl]报错", ex);

                jsonResult = Util.GetJsonDataByResult(null, $"GetOrderUrl接口出错：{ex.Message}", -1);
            }
            return jsonResult;
        }

        /// <summary>
        /// 提交订单URL
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        [ApiLog]
        public JsonResult SubmitOrderUrl([FromBody]OrderUrlResDTO resOrder)
        {
            JsonResult jsonResult = null;
            try
            {
                int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                string ip = ITSC.Chitunion2017.BLL.Util.GetIP($"用户{userId}分享订单");
                //resOrder.ChannelId = Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeiXinChannelId"));

                //分支合并后，需要自行修改逻辑
                //resOrder.ChannelId = 101005;
                resOrder.UserId = userId;


                if (!XYAuto.ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.IsOrder(resOrder.TaskId, userId))
                {


                    bool result = XYAuto.ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.SubmitOrder(resOrder.OrderUrl, resOrder.TaskId, resOrder.UserId, resOrder.ChannelId, resOrder.PromotionChannelID, ip);

                    if (result)
                    {
                        jsonResult = Util.GetJsonDataByResult(result, "Success", 0);
                    }
                    else
                    {
                        jsonResult = Util.GetJsonDataByResult(result, "提交订单失败", -1);
                    }
                }
                else
                {


                    jsonResult = Util.GetJsonDataByResult(resOrder.OrderUrl, "订单已经存在", 0);
                }


            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("[SubmitOrderUrl]报错" + ex.ToString());

                jsonResult = Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
            return jsonResult;
        }

        /// <summary>
        /// 判断是否新用户
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult NewUserVerify()
        {
            var reutrnResult = XYAuto.ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.AddOrderVerify(XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID());

            return Util.GetJsonDataByResult(reutrnResult);
        }


        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetOrderInfo(int orderid)
        {
            JsonResult jsonResult = null;
            try
            {
                int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                var result = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetOrder(orderid, userId);
                jsonResult = Util.GetJsonDataByResult(result, "Success", 0);

            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[GetOrderInfo]报错", ex);

                jsonResult = Util.GetJsonDataByResult(null, "操作出错：" + ex.Message, -1);
            }
            return jsonResult;
        }
        #region V2.5
        /// <summary>
        /// •保存跳过分类接口
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult ToSkip()
        {
            try
            {
                string errorMsg = string.Empty;
                object ret = ITSC.Chitunion2017.BLL.WeChat.LE_WXUserScene.Instance.ToSkip(ref errorMsg);
                return errorMsg == string.Empty ? Util.GetJsonDataByResult(ret) : Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"ToSkip出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 根据用户ID获取场景
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetSceneInfoByUserId()
        {
            try
            {
                string errorMsg = string.Empty;
                object ret = XYAuto.ITSC.Chitunion2017.BLL.WeChat.LE_WXUserScene.Instance.GetSceneInfoByUserId(ref errorMsg);
                return errorMsg == string.Empty ? Util.GetJsonDataByResult(ret) : Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetSceneInfoByUserId出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        #endregion
        #region V2.5.1
        /// <summary>
        ///获取用户前一天订单数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetUserDayOrderCount()
        {
            JsonResult jsonResult = null;
            try
            {
                var result = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetUserDayOrderCount();
                jsonResult = Util.GetJsonDataByResult(result, "Success", 0);

            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[GetUserDayOrderCount]报错", ex);
                jsonResult = Util.GetJsonDataByResult(0, "Fail", -1);
            }
            return jsonResult;
        }
        #endregion V2.8
        #region
        /// <summary>
        ///获取用所有订单数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetUserTotalOrderCount()
        {
            JsonResult jsonResult = null;
            try
            {
                var result = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetUserTotalOrderCount();
                jsonResult = Util.GetJsonDataByResult(result, "Success", 0);

            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[GetUserDayOrderCount]报错", ex);
                jsonResult = Util.GetJsonDataByResult(0, "Fail", -1);
            }
            return jsonResult;
        }
        #endregion
        #region •获取分享平台配置
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetSharingPlatform(int type)
        {
            try
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetSharingPlatform:enter...{type}");
                if (type != 105 && type != 106 && type != 107 && type != 108)
                    return Util.GetJsonDataByResult("操作失败", $"参数type:{type}不存在", -2);
                string errorMsg = string.Empty;
                object ret = ITSC.Chitunion2017.BLL.DictInfo.DictInfo.Instance.GetSharingPlatform(type);
                return errorMsg == string.Empty ? Util.GetJsonDataByResult(ret) : Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetSharingPlatform:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        #endregion
    }
}