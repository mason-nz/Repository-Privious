using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Senparc.Weixin.MP.Containers;
using XYAuto.ITSC.Chitunion2017.BLL;

namespace XYAuto.BUOC.Chitunion2018.SendWechatUserMsg
{
    public class SyncService
    {
        private System.Timers.Timer timer = null;

        private readonly int serviceInterval = ConfigurationManager.AppSettings["ServiceInterval"] == ""
            ? 60
            : Convert.ToInt16(ConfigurationManager.AppSettings["ServiceInterval"]);

        private readonly int OperateHour = ConfigurationManager.AppSettings["OperateHour"] == ""
            ? 9
            : Convert.ToInt16(ConfigurationManager.AppSettings["OperateHour"]);

        private readonly int OperateMinute = ConfigurationManager.AppSettings["OperateMinute"] == ""
            ? 30
            : Convert.ToInt16(ConfigurationManager.AppSettings["OperateMinute"]);

        //private string noOrderOneDay = ConfigurationManager.AppSettings["NoOrderOneDay"];
        //private string orderWeekProfit = ConfigurationManager.AppSettings["OrderWeekProfit"];
        //private string Activity = ConfigurationManager.AppSettings["Activity"];
        //string weixinAppId = ConfigurationManager.AppSettings["WeixinAppId"];
        //private string domin = ConfigurationManager.AppSettings["Domin"];

        private string activityStartDate = ConfigurationManager.AppSettings["1YuanTX_StartDate"];
        private string activityEndDate = ConfigurationManager.AppSettings["1YuanTX_EndDate"];
        private string WeChatMenuClickDataPath = ConfigurationManager.AppSettings["WeChatMenuClickDataPath"];

        private static List<Tuple<int, string, string, string>> IdAndSecrets = null;
        //GetWeChatIdAndSecrets(WeixinAppIdAndSecret);

        public SyncService()
        {
            IdAndSecrets = GetWeChatIdAndSecrets();
            timer = new System.Timers.Timer(serviceInterval * 1000) { AutoReset = true };
            timer.Elapsed += (sender, eventArgs) => Run(sender, eventArgs);
        }

        public void Strat()
        {
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("统计微信数据-----服务开始");
            timer.Start();
        }

        public void Stop()
        {
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("统计微信数据-----服务结束");
            timer.Stop();
        }

        private List<Tuple<int, string, string, string>> GetWeChatIdAndSecrets()
        {
            List<Tuple<int, string, string, string>> temp = new List<Tuple<int, string, string, string>>();
            DataTable dt = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeixinInfo.Instance.GetList();
            if (dt != null)
            {
                //string[] list = weixinAppIdAndSecret.Split('|');
                foreach (DataRow dr in dt.Rows)
                {
                    //string[] vals = idAndSecrets.Split(',');
                    int id = int.Parse(dr["RecID"].ToString());
                    string name = dr["Name"].ToString();
                    string appId = dr["AppID"].ToString();
                    string appSecrets = dr["AppSecret"].ToString();
                    temp.Add(new Tuple<int, string, string, string>(id, name, appId, appSecrets));
                }
            }
            return temp;
        }


        public void Run(object obj, ElapsedEventArgs e)
        {
            #region 发送消息给X天没有分发订单的用户

            //if (e.SignalTime.Hour == OperateHour && e.SignalTime.Minute == OperateMinute)
            //{
            //    DataSet ds = XYAuto.ITSC.Chitunion2017.BLL.WechatUserMsg.WechatUmsg.Instance.NoOrderOpenIds();
            //    #region 前一天未进行分发订单用户
            //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            //    {
            //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("ToOpenIdsForNoOrder_OneDay-----方法开始");
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            DataRow row = ds.Tables[0].Rows[i];
            //            if (row["openid"].ToString() != "")
            //            {
            //                ToOpenIdsForNoOrder_OneDay(row["openid"].ToString(), row["nickname"].ToString(), row["CreateTime"].ToString() == "" ? "" : Convert.ToDateTime(row["CreateTime"]).ToString("yyyy-MM-dd HH:mm"));
            //            }
            //        }
            //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("ToOpenIdsForNoOrder_OneDay-----方法结束");
            //    }
            //    else
            //    {
            //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("没有查询到前天未分发订单用户");
            //    }
            //    #endregion
            //}

            #endregion

            #region 发送消息给前一周有分发收益的用户

            //string week = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            //if (week == "星期一")
            //{
            //    if (e.SignalTime.Hour == OperateHour && e.SignalTime.Minute == OperateMinute)
            //    {
            //        DataTable dt = XYAuto.ITSC.Chitunion2017.BLL.WechatUserMsg.WechatUmsg.Instance.
            //            OrderWeekProfitOpenIds();
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("ToOpenIdsForOrderWeekProfit-----方法开始");
            //            for (int i = 0; i < dt.Rows.Count; i++)
            //            {
            //                DataRow row = dt.Rows[i];
            //                if (row["openid"].ToString() != "")
            //                {
            //                    ToOpenIdsForOrderWeekProfit(row["openid"].ToString(), row["nickname"].ToString(), row["TimeSlot"].ToString(), row["TotalMoney"].ToString());
            //                }
            //            }
            //            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("ToOpenIdsForOrderWeekProfit-----方法结束");
            //        }
            //        else
            //        {
            //            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("没有查询到前一周有分发收益的用户");
            //        }
            //    }
            //}

            #endregion

            string currentTime = e.SignalTime.Hour.ToString("00") + ":" + e.SignalTime.Minute.ToString("00");
            if (currentTime == OperateHour.ToString("00") + ":" + OperateMinute.ToString("00"))
            {
                if (IdAndSecrets != null && IdAndSecrets.Count > 0)
                {
                    foreach (var idAndSecret in IdAndSecrets)
                    {
                        string appId = idAndSecret.Item3;
                        string secrets = idAndSecret.Item4;
                        AccessTokenContainer.Register(appId, secrets);
                    }
                }
                DataTable dt =
                    XYAuto.ITSC.Chitunion2017.BLL.WechatUserMsg.WechatUmsg.Instance.GetWechatUserBy1YuanTx(
                        activityStartDate, activityEndDate);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int success = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        int userId = int.Parse(dr["UserID"].ToString());
                        string openId = dr["openid"].ToString();
                        string nickName = dr["nickname"].ToString();
                        string unionId = dr["unionid"].ToString();
                        string date = dr["Date"].ToString();
                        string price = dr["Price"].ToString();
                        string appId = dr["AppID"].ToString();
                        string domain = dr["Domain"].ToString();
                        DateTime createTime = DateTime.Parse(dr["createTime"].ToString());

                        //ITSC.Chitunion2017.BLL.Loger.Log4Net.Info();
                        var list =
                            ITSC.Chitunion2017.BLL.WeChat.TempHelper.Instance.GetWxTempConfigData(WeChatMenuClickDataPath);
                        var wxInfo = list.FirstOrDefault(s => s.AppId == appId);
                        if (wxInfo != null && wxInfo.TempList != null)
                        {
                            if (!string.IsNullOrEmpty(date))
                            {
                                #region 发送"收益到账提醒"模板消息
                                var temp = wxInfo.TempList.FirstOrDefault(s => s.Title == "收益到账提醒");
                                if (temp != null)
                                {
                                    Dictionary<string, object> objTempData = new Dictionary<string, object>();
                                    string[] keys = temp.Paras.Split(',');
                                    string dateTag = DateTime.Now.ToString("yyyy-MM-dd") == date ? "今日" : "昨日";
                                    string content1 = $"{nickName}，您{dateTag}的任务收益已到账，可在【收益】页面查看哦！";
                                    string url = string.IsNullOrEmpty(domain) ? "" : $"http://{domain}/moneyManager/index.html";
                                    var val1 = new TemplateDataItem(content1, "#173177");
                                    var val2 = new TemplateDataItem(dateTag, "#173177");
                                    var val3 = new TemplateDataItem(price + "元", "#173177");
                                    var val4 = new TemplateDataItem("赤兔联盟", "#173177");
                                    var val5 = new TemplateDataItem("今日奖励已经为您奉上！参与抽奖最高可获千元现金，赶紧领取>>", "#173177");
                                    objTempData.Add(keys[0], val1);
                                    objTempData.Add(keys[1], val2);
                                    objTempData.Add(keys[2], val3);
                                    objTempData.Add(keys[3], val4);
                                    objTempData.Add(keys[4], val5);
                                    bool flag = ITSC.Chitunion2017.BLL.WeChat.TempHelper.Instance.SendTempMsg(appId, openId,
                                        temp.Id,
                                        url, objTempData);
                                    string info =
                                        $"给用户UserID={userId},openid={openId},appid={appId},tempID={temp.Id},tempName={temp.Title},发送模板消息={(flag ? "成功" : "失败")}";
                                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info(info);
                                    if (flag)
                                    {
                                        success++;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 发送"注册成功通知"模板消息
                                var temp = wxInfo.TempList.FirstOrDefault(s => s.Title == "注册成功通知");
                                if (temp != null)
                                {
                                    Dictionary<string, object> objTempData = new Dictionary<string, object>();
                                    string[] keys = temp.Paras.Split(',');
                                    string dateTag = DateTime.Now.ToString("yyyy-MM-dd") == date ? "今日" : "昨日";
                                    string content1 = $"恭喜您注册成功！今日特别送上3次抽奖机会，最高100%中奖，千元现金红包在等你！";
                                    string url = string.IsNullOrEmpty(domain) ? "" : $"http://{domain}/activityManager/index.html";//抽奖页面
                                    var val1 = new TemplateDataItem(content1, "#173177");
                                    var val2 = new TemplateDataItem(nickName, "#173177");
                                    var val3 = new TemplateDataItem(createTime.ToString("yyyy-MM-dd hh:mm:ss"), "#173177");
                                    var val4 = new TemplateDataItem("抽奖机会将在今晚24点前失效，点击立即参与>>", "#173177");
                                    objTempData.Add(keys[0], val1);
                                    objTempData.Add(keys[1], val2);
                                    objTempData.Add(keys[2], val3);
                                    objTempData.Add(keys[3], val4);
                                    bool flag = ITSC.Chitunion2017.BLL.WeChat.TempHelper.Instance.SendTempMsg(appId, openId,
                                        temp.Id,
                                        url, objTempData);
                                    string info =
                                        $"给用户UserID={userId},openid={openId},appid={appId},tempID={temp.Id},tempName={temp.Title},发送模板消息={(flag ? "成功" : "失败")}";
                                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info(info);
                                    if (flag)
                                    {
                                        success++;
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"发送模板消息全部完成，待发送{dt.Rows.Count }个,其中成功{success}个，失败{dt.Rows.Count - success}个");
                }
                else
                {
                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"库中没有查到相应的数据，因此无法发送模板消息");
                }
            }

            ///// <summary>
            ///// 发送消息给前一天没有分发订单的用户
            ///// </summary>
            //public void ToOpenIdsForNoOrder_OneDay(string OpenId, string NickName, string RegisterTime)
            //{
            //    try
            //    {
            //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"ToOpenIdsForNoOrder_OneDay请求参数--OpenId:{OpenId} NickName:{NickName} RegisterTime:{RegisterTime}");
            //        var testData = new
            //        {
            //            first = new TemplateDataItem("恭喜您注册成功！并获得一次领取新任务的机会～\r\n"),
            //            keyword1 = new TemplateDataItem(NickName + "\r\n"),
            //            keyword2 = new TemplateDataItem(RegisterTime + "\r\n"),
            //            remark = new TemplateDataItem("点击查看详情>>")
            //        };
            //        var resultmsg = TemplateApi.SendTemplateMessage(weixinAppId, OpenId, noOrderOneDay, domin + "/moneyManager/make_money.html?channel=ctlmwfftz24", testData, null);
            //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"ToOpenIdsForNoOrder_OneDay返回参数--OpenId:{OpenId} resultmsg:{XYAuto.ITSC.Chitunion2017.BLL.Json.JsonSerializerBySingleData(resultmsg)}");
            //    }
            //    catch (Exception ex)
            //    {
            //        Loger.Log4Net.Error($"ToOpenIdsForNoOrder_OneDay--发送消息出错 OpenId:{OpenId}", ex);
            //    }
            //}

            ///// <summary>
            ///// 发送消息给前一周有分发收益的用户
            ///// </summary>
            //public void ToOpenIdsForOrderWeekProfit(string OpenId, string NickName, string TimeSlot, string TotalMoney)
            //{
            //    try
            //    {
            //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"ToOpenIdsForOrderWeekProfit--OpenId:{OpenId} NickName:{NickName} RegisterTime:{TimeSlot} TotalMoney:{TotalMoney}");
            //        var testData = new
            //        {
            //            first = new TemplateDataItem($"您好，您上周的任务收益已到账，可在【我的账户-提现】页面查看余额详情哦～\r\n"),
            //            keyword1 = new TemplateDataItem(TimeSlot + "\r\n"),
            //            keyword2 = new TemplateDataItem(TotalMoney + "\r\n"),
            //            remark = new TemplateDataItem("今天的任务收益领了吗？速戳>>")
            //        };
            //        var resultmsg = TemplateApi.SendTemplateMessage(weixinAppId, OpenId, orderWeekProfit, domin + "/moneyManager/make_money.html?channel=ctlmsytzmz", testData, null);
            //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"ToOpenIdsForOrderWeekProfit--OpenId:{OpenId} resultmsg:{XYAuto.ITSC.Chitunion2017.BLL.Json.JsonSerializerBySingleData(resultmsg)}");
            //    }
            //    catch (Exception ex)
            //    {
            //        Loger.Log4Net.Error($"ToOpenIdsForOrderWeekProfit--发送消息出错 OpenId:{OpenId}", ex);
            //    }
            //}
            ///// <summary>
            ///// 发送活动信息
            ///// </summary>
            ///// <param name="First"></param>
            ///// <param name="Keyword1"></param>
            ///// <param name="Keyword2"></param>
            ///// <param name="Keyword3"></param>
            ///// <param name="Remark"></param>
            //public void ToOpenIdsForActivity(string First, string Keyword1, string Keyword2, string Keyword3, string Remark, string Link)
            //{
            //    int successCount = 0;
            //    int errorCount = 0;
            //    DataTable dt = XYAuto.ITSC.Chitunion2017.BLL.WechatUserMsg.WechatUmsg.Instance.GetAllWechatUser();
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"ToOpenIdsForActivity{Keyword1}-----方法开始");
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            DataRow row = dt.Rows[i];
            //            if (row["openid"].ToString() != "")
            //            {
            //                string OpenId = row["openid"].ToString();
            //                string NickName = row["nickname"].ToString();
            //                try
            //                {
            //                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"ToOpenIdsForActivity--OpenId:{OpenId} NickName:{NickName}");
            //                    var testData = new
            //                    {
            //                        first = new TemplateDataItem($"{First}\r\n"),
            //                        keyword1 = new TemplateDataItem(Keyword1 + "\r\n"),
            //                        keyword2 = new TemplateDataItem(DateTime.Now.GetDateTimeFormats('f')[0].ToString() + "\r\n"),
            //                        keyword3 = new TemplateDataItem(Keyword3 + "\r\n"),
            //                        remark = new TemplateDataItem(Remark)
            //                    };
            //                    var resultmsg = TemplateApi.SendTemplateMessage(weixinAppId, OpenId, Activity, Link, testData, null);
            //                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"ToOpenIdsForActivity--OpenId:{OpenId} resultmsg:{XYAuto.ITSC.Chitunion2017.BLL.Json.JsonSerializerBySingleData(resultmsg)}");
            //                    if (resultmsg.errcode == 0)
            //                    {
            //                        successCount++;
            //                    }
            //                    else
            //                    {
            //                        errorCount++;
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    Loger.Log4Net.Error($"ToOpenIdsForActivity--发送消息出错 OpenId:{OpenId}", ex);
            //                }
            //            }
            //        }
            //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"发送成功个数：{successCount}  发送失败个数：{errorCount}");
            //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"ToOpenIdsForActivity{Keyword1}-----方法结束");
            //    }
            //    else
            //    {
            //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("没有查询到微信关注用户");
            //    }
            //}
        }
    }
}