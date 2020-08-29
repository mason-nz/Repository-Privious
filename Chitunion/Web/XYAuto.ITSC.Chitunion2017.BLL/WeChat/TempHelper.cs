using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.WxTemp;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat
{
    /// <summary>
    /// 注释：TempHelper
    /// 作者：masj
    /// 日期：2018/5/17 20:26:58
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class TempHelper
    {
        public static readonly TempHelper Instance = new TempHelper();
        private string WeChatMenuClickDataPath_User = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeChatMenuClickDataPath_User", true);
        private string WeChatMenuClickDataPath_Pwd = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeChatMenuClickDataPath_Pwd", true);


        /// <summary>
        /// 发送微信模板消息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <param name="templateId"></param>
        /// <param name="url"></param>
        /// <param name="objData"></param>
        /// <returns></returns>
        public bool SendTempMsg(string appId, string openId, string templateId, string url, object objData)
        {
            //var testData = new
            //{
            //    first = new TemplateDataItem("您好，您已提现成功，请到支付宝账户查看！\r\n", "#173177"),
            //    keyword1 = new TemplateDataItem(price + "元\r\n", "#173177"),
            //    keyword2 = new TemplateDataItem(string.Format("{0:F}\r\n", dt), "#173177"),
            //    remark = new TemplateDataItem("非常感谢您对赤兔联盟的支持！如有任何问题，请联系兔妹哦～", "#173177")
            //};
            try
            {
                if (objData == null)
                {
                    return false;
                }
                SendTemplateMessageResult result = TemplateApi.SendTemplateMessage(appId, openId, templateId, url, objData, null);
                Loger.Log4Net.Info($"发送模板消息完成，appId={appId},openId={openId},templateId={templateId},url={url}, objData={JsonConvert.SerializeObject(objData)},result={JsonConvert.SerializeObject(result)}");
                if (result != null && result.errmsg == "ok")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"在appid={appId}中，给openid={openId}，用模板id={templateId}，发送消息失败", ex);
                return false;
            }

        }


        /// <summary>
        /// 发送微信模板消息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <param name="templateId"></param>
        /// <param name="url"></param>
        /// <param name="objData"></param>
        /// <returns></returns>
        public void SendTempMsgAsync(string appId, string openId, string templateId, string url, object objData)
        {
            //var testData = new
            //{
            //    first = new TemplateDataItem("您好，您已提现成功，请到支付宝账户查看！\r\n", "#173177"),
            //    keyword1 = new TemplateDataItem(price + "元\r\n", "#173177"),
            //    keyword2 = new TemplateDataItem(string.Format("{0:F}\r\n", dt), "#173177"),
            //    remark = new TemplateDataItem("非常感谢您对赤兔联盟的支持！如有任何问题，请联系兔妹哦～", "#173177")
            //};
            try
            {
                if (objData == null)
                {
                    return;
                }
                Task.Factory.StartNew(async () =>
                {
                    SendTemplateMessageResult result = await TemplateApi.SendTemplateMessageAsync(appId, openId, templateId, url, objData, null);
                    Loger.Log4Net.Info($"发送模板消息完成，appId={appId},openId={openId},templateId={templateId},url={url}, objData={JsonConvert.SerializeObject(objData)},result={JsonConvert.SerializeObject(result)}");
                    //if (result != null && result.errmsg == "ok")
                    //{
                    //    return true;
                    //}
                    //return false;
                });
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"在appid={appId}中，给openid={openId}，用模板id={templateId}，发送消息失败", ex);
                return;
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SendTempMsg(SendWxTempDataDto req, ref string msg)
        {
            bool flag = false;
            try
            {
                string errorMsg = string.Empty;
                string[] openids = req.SendOpenIds.Split(',');

                var objTempData = GetObjTempDataByRequestData(req);
                if (objTempData.Count > 0)
                {
                    AccessTokenContainer.Register(req.AppId, req.AppSecret);
                    foreach (string openid in openids)
                    {
                        if (!SendTempMsg(req.AppId, openid, req.WxTempId, req.WxTempUrl, objTempData))
                        {
                            errorMsg += $"在appid={req.AppId}中，给openid={openid}，用模板id={req.WxTempId}，发送消息失败";
                        }
                    }
                }
                if (string.IsNullOrEmpty(errorMsg))
                {
                    flag = true;
                }
                else
                {
                    msg = errorMsg;
                }

            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("批量发送微信模板消息异常", ex);
                msg = ex.Message;
                return false;
            }
            return flag;
        }


        public void SendTempMsgAsync(SendWxTempDataDto req, ref string msg)
        {
            try
            {
                string[] openids = req.SendOpenIds.Split(',');

                var objTempData = GetObjTempDataByRequestData(req);
                if (objTempData.Count > 0)
                {
                    AccessTokenContainer.Register(req.AppId, req.AppSecret);
                    foreach (string openid in openids)
                    {
                        SendTempMsgAsync(req.AppId, openid, req.WxTempId, req.WxTempUrl, objTempData);
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("批量发送微信模板消息异常", ex);
                msg = ex.Message;
            }
        }

        private static Dictionary<string, object> GetObjTempDataByRequestData(SendWxTempDataDto req)
        {
            Dictionary<string, object> objTempData = new Dictionary<string, object>();

            var paras = JsonConvert.DeserializeObject<Dictionary<string, object>>(req.WxTempParas);
            if (paras != null)
            {
                for (int i = 0; i < paras.Keys.Count; i++)
                {
                    string key = paras.Keys.ElementAt(i);
                    var val = new TemplateDataItem(paras[key].ToString(), "#173177");
                    objTempData.Add(key, val);
                }
            }
            return objTempData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool VerifyRequestBySendWxTempData(SendWxTempDataDto req, out string msg)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(req.AppId))
            {
                msg = "缺少AppId参数";
            }
            else if (string.IsNullOrEmpty(req.WxTempId))
            {
                msg = "缺少WxTempId参数";
            }
            else if (GetTempDataByTempId(req.AppId, req.WxTempId, req) == null)
            {
                msg = $"根据WxTempId={req.WxTempId}参数，找不到对应的模板数据";
            }
            else if (string.IsNullOrEmpty(req.WxTempParas))
            {
                msg = "缺少WxTempParas参数";
            }
            else if (string.IsNullOrEmpty(req.SendOpenIds))
            {
                msg = "缺少SendOpenIds参数";
            }
            else if (string.IsNullOrEmpty(req.WxTempUrl) == false &&
                    !BLL.Util.IsUrl(req.WxTempUrl))
            {
                msg = "参数WxTempUrl必须是url格式";
            }
            else
            {
                flag = true;
                msg = "";
            }
            return flag;
        }

        /// <summary>
        /// 获取微信模板信息数据
        /// </summary>
        /// <returns></returns>
        public List<WxTempDataDto> GetWxTempConfigData()
        {
            string wxTempDataFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WxTempDataInfo");
            string content = XYAuto.Utils.FileHelper.ReadFile(System.AppDomain.CurrentDomain.BaseDirectory + wxTempDataFilePath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<List<WxTempDataDto>>(content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="tempId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public WxTempDataTemp GetTempDataByTempId(string appid, string tempId, SendWxTempDataDto req)
        {
            WxTempDataTemp ret = null;
            try
            {
                List<WxTempDataDto> data = GetWxTempConfigData();
                var wxObj = data.FirstOrDefault(s => s.AppId == appid);
                if (wxObj != null)
                {
                    if (req != null) req.AppSecret = wxObj.AppSecret;
                    ret = wxObj.TempList.FirstOrDefault(s => s.Id == tempId);
                }
                return ret;
            }
            catch (Exception ex)
            {
                return ret;
            }
        }


        /// <summary>
        /// 获取微信模板信息数据
        /// 若filePath不为空，可以支持共享文件夹，如：\\192.168.3.19\wwwroot\XYAuto.POBU.Chitunion2018.AdminWebAPI\ConfigFile\test.config
        /// </summary>
        /// <returns></returns>
        public List<WxTempDataDto> GetWxTempConfigData(string filePath)
        {
            string content = string.Empty;
            string wxTempDataFilePath = string.Empty;
            //if (string.IsNullOrEmpty(filePath))
            //{
            //    wxTempDataFilePath = System.AppDomain.CurrentDomain.BaseDirectory +
            //                         XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WxMenuDataInfo");
            //    content = XYAuto.Utils.FileHelper.ReadFile(wxTempDataFilePath, Encoding.UTF8);
            //}
            //else
            //{
            if (filePath.StartsWith(@"\\"))
            {
                string ip = filePath.Replace(@"\\", "").Substring(0, filePath.Replace(@"\\", "").IndexOf(@"\", StringComparison.Ordinal));
                using (FileShareHelper tool = new FileShareHelper(WeChatMenuClickDataPath_User, WeChatMenuClickDataPath_Pwd, ip))
                {
                    content = BLL.FileShareHelper.ReadFiles(filePath);
                }
            }
            else
            {
                wxTempDataFilePath = filePath;
                content = XYAuto.Utils.FileHelper.ReadFile(wxTempDataFilePath, Encoding.UTF8);
            }

            return JsonConvert.DeserializeObject<List<WxTempDataDto>>(content);
        }
    }
}
