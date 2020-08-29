using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [CrossSite]
    public class WeChatToChiTuController : ApiController
    {
        private string appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private string secret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        private string appIdNew = ConfigurationManager.AppSettings["NewWeixinAppId"];
        private string dominNew = ConfigurationManager.AppSettings["NewDomin"];

        //提现
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

                var result = TemplateApi.SendTemplateMessage(appId, openId, templateId, XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("Domin") + "/cashManager/cashDetail.html?channel=ctyz052213&WithdrawalsId=" + txid, testData, null);
                Loger.Log4Net.Info("赤兔联盟WithdrawalsNotice->提现：" + JsonConvert.SerializeObject(result));
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
                Loger.Log4Net.Error("赤兔联盟[WithdrawalsNotice]:", ex);
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
                    var result = TemplateApi.SendTemplateMessage(appIdNew, openId, templateId, dominNew + "/cashManager/cashDetail.html?channel=ctyz052213WithdrawalsId=" + txid, testData, null);
                    Loger.Log4Net.Info("赤兔快赚WithdrawalsNotice->提现：" + JsonConvert.SerializeObject(result));
                    return result.errmsg == "ok";
                }
                catch (Exception ex1)
                {
                    Loger.Log4Net.Error("赤兔快赚[WithdrawalsNotice]:", ex1);
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
                var resultmsg = TemplateApi.SendTemplateMessage(appId, openId, templateId, XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("Domin") + "/userManager/userInfo.html", testData, null);
                return resultmsg.errmsg == "ok";
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[NoticePass]:" + ex.ToString() + "");
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

                var resultmsg = TemplateApi.SendTemplateMessage(appId, openId, templateId, XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("Domin") + "/userManager/userInfo.html", testData, null);
                return resultmsg.errmsg == "ok";
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[NoticeNoPass]:" + ex.ToString() + "");
                return false;
            }
        }
    }
}