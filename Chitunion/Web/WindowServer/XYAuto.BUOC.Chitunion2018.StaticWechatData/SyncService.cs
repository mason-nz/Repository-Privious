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
using XYAuto.ITSC.Chitunion2017.Entities;

namespace XYAuto.BUOC.Chitunion2018.StaticWechatData
{
    public class SyncService
    {
        private System.Timers.Timer timer = null;
        private readonly int serviceInterval = ConfigurationManager.AppSettings["ServiceInterval"] == "" ? 60 : Convert.ToInt16(ConfigurationManager.AppSettings["ServiceInterval"]);
        private readonly int OperateHour = ConfigurationManager.AppSettings["OperateHour"] == "" ? 9 : Convert.ToInt16(ConfigurationManager.AppSettings["OperateHour"]);
        private readonly int OperateMinute = ConfigurationManager.AppSettings["OperateMinute"] == "" ? 30 : Convert.ToInt16(ConfigurationManager.AppSettings["OperateMinute"]);
        public SyncService()
        {
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


        public void Run(object obj, ElapsedEventArgs e)
        {
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("统计微信数据-----Run......");
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info(string.Format($"统计微信数据-----Hour:{e.SignalTime.Hour},Minute:{e.SignalTime.Minute}"));
            if (e.SignalTime.Hour == OperateHour && e.SignalTime.Minute == OperateMinute)
                StaticDataForDay();

            string week = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            if (week == "星期四")
            {
                if (e.SignalTime.Hour == OperateHour && e.SignalTime.Minute == OperateMinute)
                {
                    StaticDataForWeek();
                }
            }

        }
        /// <summary>
        /// 日统计
        /// </summary>
        public void StaticDataForDay()
        {
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("统计微信数据(日统计)-----方法开始");
            try
            {
                Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[分发]，统计开始...");
                dic.Add("分发", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDistributeForDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[邀请好友]，统计开始...");
                dic.Add("邀请好友", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticInviteDataForDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[签到]，统计开始...");
                dic.Add("签到", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticSignDataForDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[用户]，统计开始...");
                dic.Add("用户", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinUserDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[渠道]，统计开始...");
                dic.Add("渠道", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinChannelDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[抢单赚钱]，统计开始...");
                dic.Add("抢单赚钱", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinOrderDay());

                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[渠道效果数据表]，统计开始...");
                dic.Add("渠道效果数据表", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinChannelResultDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[新关注粉丝明细]，统计开始...");
                dic.Add("新关注粉丝明细", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinNewAttentionDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[老关注用户明细]，统计开始...");
                dic.Add("老关注用户明细", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinOldAttentionDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[注册用户明细]，统计开始...");
                dic.Add("注册用户明细", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinRegisterUserDay());

                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[类目选择的分类数据]，统计开始...");
                dic.Add("类目选择的分类数据", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinCategorySelectDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[类目被选择数据]，统计开始...");
                dic.Add("类目被选择数据", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinGetCategoryUserDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[分发用户明细数据]，统计开始...");
                dic.Add("分发用户明细数据", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataDistributionDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[物料领取明细]，统计开始...");
                dic.Add("物料领取明细", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataMaterialCollectingDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[物料详情]，统计开始...");
                dic.Add("物料详情", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataMaterialDetailDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[物料分发每周数据汇总]，统计开始...");
                dic.Add("物料分发每周数据汇总", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinDispenseForWeek());

                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[抢单赚钱登录用户明细]，统计开始...");
                dic.Add("抢单赚钱登录用户明细", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticUserLoginLogDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[邀请有礼用户明细]，统计开始...");
                dic.Add("邀请有礼用户明细", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticInviteRecordDay());

                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[分类文章数据]，统计开始...");
                dic.Add("分类文章数据", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticCategoryArticleDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[个人信息页面数据]，统计开始...");
                dic.Add("个人信息页面数据", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticUserInfoDay());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[经纪人周数据]，统计开始...");
                dic.Add("经纪人周数据", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticBrokerForWeek());
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("查询[老关注用户明细2]，统计开始...");
                dic.Add("老关注用户明细2", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinOldAttentionDay());
               
                string FileName = "微信数据日统计" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls";
                string excelName = "";
                if (NpoiExcel.ExportToExcel(dic, FileName, out excelName))
                {
                    string[] userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
                    if (userEmail != null && userEmail.Length > 0)
                    {
                        if (File.Exists(excelName))
                        {
                            ITSC.Chitunion2017.BLL.EmailHelper.Instance.SendWechatReportDataMail(userEmail, excelName);
                        }
                        else
                        {
                            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("微信数据日统计查无数据");
                        }
                    }
                }
                else
                {
                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("StaticDataForDay:统计微信数据插入excel失败");
                }
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("统计微信数据(日统计)-----方法结束");
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("统计微信数据出错(日统计):" + ex.Message);
            }
        }
        public void StaticDataForWeek()
        {
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("统计微信数据(周统计)-----方法开始");
            try
            {
                Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
                dic.Add("活动每周数据汇总", ITSC.Chitunion2017.BLL.WechatDataStatic.WechatDataStatic.Instance.StaticDataSumForWeek());

                string FileName = "微信数据周统计" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls";
                string excelName = "";
                if (NpoiExcel.ExportToExcel(dic, FileName, out excelName))
                {
                    string[] userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
                    if (userEmail != null && userEmail.Length > 0)
                    {
                        if (File.Exists(excelName))
                        {
                            ITSC.Chitunion2017.BLL.EmailHelper.Instance.SendWechatReportDataMail(userEmail, excelName);
                        }
                        else
                        {
                            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("微信数据周统计查无数据");
                        }
                    }
                }
                else
                {
                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("StaticDataForWeek:统计微信数据插入excel失败");
                }
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("统计微信数据(周统计)-----方法结束");
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("统计微信数据出错(周统计):" + ex.Message);
            }
        }
    }
}
