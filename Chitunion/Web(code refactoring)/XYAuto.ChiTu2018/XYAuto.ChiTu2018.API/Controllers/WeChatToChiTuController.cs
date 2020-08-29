using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using XYAuto.ChiTu2018.API.Filter;
using XYAuto.ChiTu2018.API.Models.Enum;
using XYAuto.ChiTu2018.API.Models.Withdrawals;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ChiTu2018.API.Controllers
{
    /// <summary>
    /// 微信工具类：推送模版消息接口
    /// </summary>
    [CrossSite]
    public class WeChatToChiTuController : ApiController
    {
        private readonly string _appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private readonly string _secret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        private readonly string _appIdNew = ConfigurationManager.AppSettings["NewWeixinAppId"];
        private readonly string _dominNew = ConfigurationManager.AppSettings["NewDomin"];

        /// <summary>
        /// 获取wx模版配置的跳转地址
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetRedirectUrl(WxTemplateRedirectEnum type)
        {
            var redirectList = ConfigurationManager.AppSettings["WxNoticeTemplateUrl"];
            var list = JsonConvert.DeserializeObject<List<WxTemplateRedirectDto>>(redirectList);
            var info = list.FirstOrDefault(s => s.Type.Equals((int)type));
            return info != null ? info.RedirectUrl : string.Empty;
        }

        /// <summary>
        /// 提现成功推送模版消息
        /// </summary>
        /// <param name="openId">openId</param>
        /// <param name="dt">时间</param>
        /// <param name="price">价格</param>
        /// <param name="txid">提现id</param>
        /// <returns></returns>
        [HttpGet]
        public bool WithdrawalsNotice(string openId, DateTime dt, string price, int txid)
        {

            try
            {
                string templateId = ConfigurationManager.AppSettings["Withdrawals"];
                var testData = new
                {
                    first = new TemplateDataItem("您好，您已提现成功，请到支付宝账户查看！\r\n", "#173177"),
                    keyword1 = new TemplateDataItem(price + "元\r\n", "#173177"),
                    keyword2 = new TemplateDataItem(string.Format("{0:F}\r\n", dt), "#173177"),
                    remark = new TemplateDataItem("非常感谢您对赤兔联盟的支持！如有任何问题，请联系兔妹哦～", "#173177")
                };
                var url = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("Domin") +
                          $"{GetRedirectUrl(WxTemplateRedirectEnum.提现成功)}?WithdrawalsId=" + txid;
                var result = TemplateApi.SendTemplateMessage(_appId, openId, templateId, url, testData, null);
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("赤兔联盟WithdrawalsNotice->提现：" + JsonConvert.SerializeObject(result));
                //if (result.errmsg != "ok")
                //{
                //    templateId = ConfigurationManager.AppSettings["NewWithdrawals"];
                //    result = TemplateApi.SendTemplateMessage(appIdNew, openId, templateId, dominNew + "/cashManager/cashDetail.html?WithdrawalsId=" + txid, testData, null);
                //    Loger.Log4Net.Info("赤兔快赚WithdrawalsNotice->提现：" + JsonConvert.SerializeObject(result));
                //}
                return result.errmsg == "ok";
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error("赤兔联盟[WithdrawalsNotice]:", ex);
                try
                {
                    string templateId = ConfigurationManager.AppSettings["NewWithdrawals"];
                    var testData = new
                    {
                        first = new TemplateDataItem("您好，您的提现操作已经成功\r\n", "#173177"),
                        keyword1 = new TemplateDataItem(string.Format("{0:F}\r\n", dt), "#173177"),
                        keyword2 = new TemplateDataItem(price + "元\r\n", "#173177"),
                        remark = new TemplateDataItem("感谢您的使用", "#173177")
                    };
                    var url = _dominNew +
                        $"{GetRedirectUrl(WxTemplateRedirectEnum.提现成功)}?WithdrawalsId=" + txid;
                    var result = TemplateApi.SendTemplateMessage(_appIdNew, openId, templateId, url, testData, null);
                    XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("赤兔快赚WithdrawalsNotice->提现：" + JsonConvert.SerializeObject(result));
                    return result.errmsg == "ok";
                }
                catch (Exception ex1)
                {
                    XYAuto.CTUtils.Log.Log4NetHelper.Default().Error("赤兔快赚[WithdrawalsNotice]:", ex1);
                    return false;
                }
            }
        }


        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="name">姓名</param>
        /// <param name="result">审核结果</param>
        /// <returns></returns>
        [HttpGet]
        public bool NoticePass(string openId, string name, string result)
        {
            try
            {
                string templateId = ConfigurationManager.AppSettings["NoticePass"];
                var testData = new
                {
                    first = new TemplateDataItem("您好，您提交的个人信息已审核通过\r\n", "#173177"),
                    keyword1 = new TemplateDataItem(name + "\r\n", "#173177"),
                    keyword2 = new TemplateDataItem(result + "\r\n", "#173177"),
                    remark = new TemplateDataItem("查看个人信息", "#173177")
                };
                var url = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("Domin") +
                  $"{GetRedirectUrl(WxTemplateRedirectEnum.用户审核)}";
                var resultmsg = TemplateApi.SendTemplateMessage(_appId, openId, templateId, url, testData, null);
                return resultmsg.errmsg == "ok";
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("[NoticePass]:" + ex.ToString() + "");
                return false;
            }
        }

        /// <summary>
        /// 审核不通过通知
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="name">姓名</param>
        /// <param name="result">审核结果</param>
        /// <param name="reason">不通过原因</param>
        /// <returns></returns>
        [HttpGet]
        public bool NoticeNoPass(string openId, string name, string result, string reason)
        {
            try
            {
                string templateId = ConfigurationManager.AppSettings["NoticeNoPass"];
                var testData = new
                {
                    first = new TemplateDataItem("您好，您提交的个人信息未审核通过\r\n", "#173177"),
                    keyword1 = new TemplateDataItem(name + "\r\n", "#173177"),
                    keyword2 = new TemplateDataItem(result + "\r\n", "#173177"),
                    keyword3 = new TemplateDataItem(reason + "\r\n", "#173177"),
                    remark = new TemplateDataItem("请您修改相关信息后再提交", "#173177")

                };
                var url = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("Domin") +
                            $"{GetRedirectUrl(WxTemplateRedirectEnum.用户审核)}";
                var resultmsg = TemplateApi.SendTemplateMessage(_appId, openId, templateId, url, testData, null);
                return resultmsg.errmsg == "ok";
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("[NoticeNoPass]:" + ex.ToString() + "");
                return false;
            }
        }

    }
}
