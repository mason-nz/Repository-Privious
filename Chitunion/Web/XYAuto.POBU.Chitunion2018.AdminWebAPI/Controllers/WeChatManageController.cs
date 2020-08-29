using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Containers;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.WxMenu;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.WxTemp;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Common;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Filter;
using Util = XYAuto.POBU.Chitunion2018.AdminWebAPI.Common.Util;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI.Controllers
{
    public class WeChatManageController : ApiController
    {
        #region 模板相关

        /// <summary>
        /// 获取微信模板数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetWxTempData()
        {
            try
            {
                var tempDataList = XYAuto.ITSC.Chitunion2017.BLL.WeChat.TempHelper.Instance.GetWxTempConfigData();
                foreach (var tempData in tempDataList)
                {
                    tempData.AppSecret = null;
                }
                return Util.GetJsonDataByResult(tempDataList);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("获取微信模板数据接口异常", ex);
                return Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }

        /// <summary>
        /// 发送模板内容给指定的人
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult SendWxTempData([FromBody] SendWxTempDataDto req)
        {
            string msg = string.Empty;
            if (XYAuto.ITSC.Chitunion2017.BLL.WeChat.TempHelper.Instance.VerifyRequestBySendWxTempData(req, out msg))
            {
                bool flag = XYAuto.ITSC.Chitunion2017.BLL.WeChat.TempHelper.Instance.SendTempMsg(req, ref msg);
                return Util.GetJsonDataByResult(null, msg, flag ? 0 : -1);
            }
            else
            {
                return Util.GetJsonDataByResult(null, msg, -1);
            }
        }

        /// <summary>
        /// 发送模板内容给指定的人（异步方法）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult SendWxTempDataAsync([FromBody] SendWxTempDataDto req)
        {
            string msg = string.Empty;
            if (XYAuto.ITSC.Chitunion2017.BLL.WeChat.TempHelper.Instance.VerifyRequestBySendWxTempData(req, out msg))
            {
                XYAuto.ITSC.Chitunion2017.BLL.WeChat.TempHelper.Instance.SendTempMsgAsync(req, ref msg);
                return Util.GetJsonDataByResult(null, msg);
            }
            else
            {
                return Util.GetJsonDataByResult(null, msg, -1);
            }
        }

        #endregion

        #region 菜单相关

        /// <summary>
        /// 调用微信接口，获取某个微信公众号的菜单数据
        /// </summary>
        /// <param name="appid">AppId</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetMenuData(string appid)
        {
            string msg = string.Empty;
            var tempDataList = XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.GetWxMenuConfigData();
            var result = tempDataList?.FirstOrDefault(s => s.AppId == appid);
            if (result != null)
            {
                AccessTokenContainer.Register(result.AppId, result.AppSecret);
                var data = XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.GetMenu(appid);
                return Util.GetJsonDataByResult(data.menu.button, msg);
                //if (data != null && data.menu != null && data.menu.button != null)
                //{
                //    foreach (var buton in data.menu.button)
                //    {
                //        //data.menu.button[0].
                //        string name = buton.name;
                //    }
                //    return Util.GetJsonDataByResult(data.menu.button, msg);
                //}
            }

            return Util.GetJsonDataByResult(null, msg);
        }

        /// <summary>
        /// 获取配置文件（ConfigFile/WxMenuData.config）中的微信公众号菜单数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetMenuData()
        {
            try
            {
                var tempDataList = XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.GetWxMenuConfigData();
                foreach (var tempData in tempDataList)
                {
                    tempData.AppSecret = null;
                }
                return Util.GetJsonDataByResult(tempDataList);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("获取微信菜单数据接口异常", ex);
                return Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }

        /// <summary>
        /// 保存微信公众号菜单数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult SaveMenuData([FromBody] SaveWxMenuDataDto req)
        {
            string msg = string.Empty;

            if (XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.VerifyRequestBySaveMenuData(req, out msg))
            {
                bool flag = XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.SaveMenuData(req, ref msg);
                return Util.GetJsonDataByResult(null, msg, flag ? 0 : -1);
            }
            else
            {
                return Util.GetJsonDataByResult(null, msg, -1);
            }
        }


        /// <summary>
        /// 清空微信公众号菜单数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult ClearMenuData([FromBody] SaveWxMenuDataDto req)
        {
            string msg = string.Empty;
            if (string.IsNullOrEmpty(req.AppId))
            {
                msg = "您必须选择一个微信公众号";
                return Util.GetJsonDataByResult(null, msg, -1);
            }
            else
            {
                bool flag = XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.ClearMenu(req.AppId);
                return Util.GetJsonDataByResult(null, msg, flag ? 0 : -1);
            }
        }

        #endregion


        #region 自定义回复及关注回复相关
        /// <summary>
        /// 修改公众号自定义回复数据以及关注回复数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult ModifyCustomData([FromBody] WxMenuDataDto req)
        {
            string msg = string.Empty;
            if (XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.VerifyRequestBySaveCustomMsgData(req, out msg))
            {
                bool flag =XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.SaveCustomMsgData(req, ref msg);
                return Util.GetJsonDataByResult(null, msg, flag ? 0 : -1);
            }
            else
            {
                return Util.GetJsonDataByResult(null, msg, -1);
            }
        }
        /// <summary>
        /// 清空公众号自定义回复数据以及关注回复数据
        /// </summary>
        /// <param name="req">参数对象，只用到AppId这一个属性，其余的可以不维护</param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult ClearCustomData([FromBody] WxMenuDataDto req)
        {
            string msg = string.Empty;
            if (string.IsNullOrEmpty(req.AppId))
            {
                msg = "您必须选择一个微信公众号";
                return Util.GetJsonDataByResult(null, msg, -1);
            }
            else
            {
                bool flag = XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.ClearCustomMsg(req.AppId);
                return Util.GetJsonDataByResult(null, msg, flag ? 0 : -1);
            }
        }
        #endregion


        [HttpGet]
        [ApiLog]
        public JsonResult test()
        {
            string WeChatMenuClickDataPath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeChatMenuClickDataPath", true);
            var list =
                    XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.GetWxMenuConfigData(WeChatMenuClickDataPath);
            return Util.GetJsonDataByResult(list);
        }
    }
}
