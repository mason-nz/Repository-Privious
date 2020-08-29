using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using XYAuto.ITSC.Chitunion2017.HBaseDataSync.BusinessLL;
using XYAuto.ITSC.Chitunion2017.HBaseDataSync.Thrift;
using XYAuto.ITSC.Chitunion2017.HBaseDataSync.WangyiDun;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync
{
    public class SyncService
    {
        //private System.Timers.Timer timer = null;//每天每个小时20分钟，从Hbase中获取微信公众号文章数据，同步到SQL Server[NLP2017.dbo.Weixin_ArticleInfo]中。
        private System.Timers.Timer timer_MailReminder = null;//每天早上8点，发邮件提醒给相关人，统计内容发布系统中封装物料相关数据
        //private System.Timers.Timer timer_CleanArticleData = null;//每天每个小时清洗微信公众号及头条号文章内容数据，清洗后，统一存放到表中去
        private readonly int serviceInterval = int.Parse(ConfigurationManager.AppSettings["ServiceInterval"]);
        private int OperateMinute = ConfigurationManager.AppSettings["OperateMinute"] == "" ? 0 : Convert.ToInt16(ConfigurationManager.AppSettings["OperateMinute"]);
        private string MailReminderTime = ConfigurationManager.AppSettings["MailReminderTime"] == "" ? "05:00" : ConfigurationManager.AppSettings["MailReminderTime"].ToString();
        private readonly int iBatchSize = ConfigurationManager.AppSettings["iBatchSize"] == "" ? 2000 : Convert.ToInt16(ConfigurationManager.AppSettings["iBatchSize"]);
        private int SyncByTimeEnable = ConfigurationManager.AppSettings["SyncByTimeEnable"] == "" ? 0 : Convert.ToInt16(ConfigurationManager.AppSettings["SyncByTimeEnable"]);
        private DateTime SyncByTimeBeginTime = ConfigurationManager.AppSettings["SyncByTimeBeginTime"] == "" ? new DateTime(1990, 1, 1) : Convert.ToDateTime(ConfigurationManager.AppSettings["SyncByTimeBeginTime"]);
        private DateTime SyncByTimeEndTime = ConfigurationManager.AppSettings["SyncByTimeEndTime"] == "" ? new DateTime(1990, 1, 1) : Convert.ToDateTime(ConfigurationManager.AppSettings["SyncByTimeEndTime"]);

        private string Conn_NLP = ConfigurationManager.AppSettings["ConnectionStrings_ITSC"];
        private string Conn_BaseData = ConfigurationManager.AppSettings["ConnectionStrings_BaseData"];
        private int QueryDataTopNum = ConfigurationManager.AppSettings["QueryDataTopNum"] == "" ? 0 : Convert.ToInt16(ConfigurationManager.AppSettings["QueryDataTopNum"]);
        private int TaskThreadCount = ConfigurationManager.AppSettings["TaskThreadCount"] == "" ? 1 : Convert.ToInt16(ConfigurationManager.AppSettings["TaskThreadCount"]);

        public SyncService()
        {
            //timer = new System.Timers.Timer(serviceInterval * 1000) { AutoReset = true };
            //timer.Elapsed += (sender, eventArgs) => Run(sender, eventArgs);

            timer_MailReminder = new System.Timers.Timer(serviceInterval * 1000) { AutoReset = true };
            timer_MailReminder.Elapsed += (sender, eventArgs) => RunByStatWXArticle(sender, eventArgs);

            //timer_CleanArticleData = new System.Timers.Timer(serviceInterval * 1000) { AutoReset = true };
            //timer_CleanArticleData.Elapsed += (sender, eventArgs) => RunCleanArticleData(sender, eventArgs);
        }

        public void Start()
        {
            BLL.Loger.Log4Net.Info("同步微信文章明细数据-----服务开始");
            //timer.Start();
            timer_MailReminder.Start();
            //timer_CleanArticleData.Start();

            RunCleanArticleData(null, null);
        }

        public void Stop()
        {
            BLL.Loger.Log4Net.Info("同步微信文章明细数据-----服务结束");
            //timer.Stop();
            timer_MailReminder.Stop();
            //timer_CleanArticleData.Stop();
        }


        //public void Run(object obj, ElapsedEventArgs e)
        //{
        //    BLL.Loger.Log4Net.Info("同步微信文章明细数据-----Run......");
        //    OperateMinute = GetAppSettings("OperateMinute") == "" ? 0 : Convert.ToInt16(GetAppSettings("OperateMinute"));
        //    SyncByTimeEnable = GetAppSettings("SyncByTimeEnable") == "" ? 0 : Convert.ToInt16(GetAppSettings("SyncByTimeEnable"));
        //    SyncByTimeBeginTime = GetAppSettings("SyncByTimeBeginTime") == "" ? new DateTime(1990, 1, 1) : Convert.ToDateTime(GetAppSettings("SyncByTimeBeginTime"));
        //    SyncByTimeEndTime = GetAppSettings("SyncByTimeEndTime") == "" ? new DateTime(1990, 1, 1) : Convert.ToDateTime(GetAppSettings("SyncByTimeEndTime"));
        //    BLL.Loger.Log4Net.Info(string.Format($"同步微信文章明细数据-----OperateMinute:{OperateMinute}，当前Hour:{e.SignalTime.Hour},Minute:{e.SignalTime.Minute},SyncByTimeEnable:{SyncByTimeEnable},GetAppSettings:{GetAppSettings("SyncByTimeEnable")}"));
        //    if (e.SignalTime.Minute == OperateMinute && SyncByTimeEnable != 1)
        //        SyncTask();

        //    if (SyncByTimeEnable == 1)
        //        SyncByTime();
        //}

        public void RunByStatWXArticle(object obj, ElapsedEventArgs e)
        {
            try
            {
                string hour = e.SignalTime.Hour.ToString("00") + ":" + e.SignalTime.Minute.ToString("00");
                if (hour == MailReminderTime)
                {
                    BLL.Loger.Log4Net.Info("发送邮件提醒开始：（每天早上8点，发邮件提醒给相关人，统计内容发布系统中封装物料相关数据）");
                    string subject = "赤兔平台——内容发布系统-物料封装统计情况";
                    //int wXArticleCount = 0;
                    //ArrayList al_WXNum = new ArrayList();
                    //DataTable dt = QueryHbaseHelper.Instance.GetStatWeixin_ArticleByDate(DateTime.Now.AddDays(-1), out wXArticleCount, out al_WXNum);
                    //string mailBody = string.Format("&nbsp;&nbsp;&nbsp;&nbsp;您{0}，汇总一共有{1}篇文章，涉及微信公众号{2}个，具体统计Hbse数据情况如下：<br/>{3}",
                    //    DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), wXArticleCount, al_WXNum.Count, GetMailBodyByDT(dt));
                    DateTime dtCurrent = DateTime.Now;
                    string statDate = dtCurrent.AddDays(-1).ToString("yyyy-MM-dd");
                    DataSet ds = MaterielStat.Instance.GetStatData(statDate);

                    StringBuilder mailBody = new StringBuilder();
                    if (ds != null && ds.Tables.Count == 11)
                    {
                        DataTable dt_Total = ds.Tables[0];
                        DataTable dt_Channel = ds.Tables[1];
                        DataTable dt_FengZhuang = ds.Tables[2];
                        DataTable dt_TopByDay = ds.Tables[3];
                        DataTable dt_4 = ds.Tables[4];
                        DataTable dt_5 = ds.Tables[5];
                        DataTable dt_6 = ds.Tables[6];
                        DataTable dt_7 = ds.Tables[7];
                        DataTable dt_8 = ds.Tables[8];
                        DataTable dt_9 = ds.Tables[9];
                        DataTable dt_10 = ds.Tables[10];

                        if (dt_Total != null)
                        {
                            mailBody.Append(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;截止到当前时间（{0}），统计（{2}），头部文章，抓取渠道数统计：<br/>{1}<br/><br/>",
                                dtCurrent.ToString("yyyy-MM-dd hh:mm:ss"), GetMailBodyByDT(dt_Total), statDate));
                        }
                        if (dt_Channel != null)
                        {
                            mailBody.Append(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;统计（{0}），头部文章，入库量统计：<br/>{1}<br/><br/>",
                                statDate, GetMailBodyByDT(dt_Channel)));
                        }
                        if (dt_10 != null)
                        {
                            mailBody.Append(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;统计（{0}），机洗入库后文章分类分布情况：<br/>{1}<br/><br/>",
                                statDate, GetMailBodyByDT(dt_10)));
                        }
                        if (dt_FengZhuang != null)
                        {
                            mailBody.Append(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;腰部文章，总计：<br/>{0}<br/><br/>",
                                 GetMailBodyByDT(dt_FengZhuang)));
                        }
                        if (dt_TopByDay != null)
                        {
                            mailBody.Append(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;统计（{0}），腰部文章，抓取文章数统计：<br/>{1}<br/><br/>",
                                statDate, GetMailBodyByDT(dt_TopByDay)));
                        }

                        if (dt_4 != null)
                        {
                            mailBody.Append(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;统计（{0}），腰部文章，入库量统计：<br/>{1}<br/><br/>",
                                statDate, GetMailBodyByDT(dt_4)));
                        }
                        if (dt_5 != null)
                        {
                            mailBody.Append(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;统计（{0}），封装及分发，总计：<br/>{1}<br/><br/>",
                                statDate, GetMailBodyByDT(dt_5)));
                        }
                        if (dt_6 != null)
                        {
                            mailBody.Append(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;统计（{0}），封装及分发，封装头部文章使用量统计：<br/>{1}<br/><br/>",
                                statDate, GetMailBodyByDT(dt_6)));
                        }
                        if (dt_7 != null)
                        {
                            mailBody.Append(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;统计（{0}），封装及分发，封装腰部文章使用量统计：<br/>{1}<br/><br/>",
                                statDate, GetMailBodyByDT(dt_7)));
                        }
                        if (dt_9 != null)
                        {
                            mailBody.Append(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;统计（{0}），封装及分发，经纪人实际分发统计：<br/>{1}<br/><br/>",
                                statDate, GetMailBodyByDT(dt_9)));
                        }
                        if (dt_8 != null)
                        {
                            mailBody.Append(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;统计（{0}），封装流程，统计日期内各业务状态下数据统计：<br/>{1}<br/><br/>",
                                statDate, GetMailBodyByDT(dt_8)));
                        }
                    }
                    BLL.Loger.Log4Net.Info("准备好要发邮件了");
                    MailReminderService.Instance.SendMailReminder(subject, mailBody.ToString());
                    BLL.Loger.Log4Net.Info("发送邮件提醒结束：（每天早上8点，发邮件提醒给相关人，统计内容发布系统中封装物料相关数据）");
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("发送邮件提醒出错：", ex);
            }
        }

        public void RunCleanArticleData(object obj, ElapsedEventArgs e)
        {
            //if (e.SignalTime.Minute == OperateMinute)
            {
                BLL.Loger.Log4Net.Info("同步文章数据-----开始");

                Task t1 = Task.Factory.StartNew(() =>
                {
                    Parallel.For(0, TaskThreadCount, i =>
                    {
                        while (true)
                        {
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步微信文章数据 任务开始");
                            DateTime d = DateTime.Now;
                            BusinessLL.ArticleInfo.Instance.SyncDataByWeixin(QueryDataTopNum);
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步微信文章数据 任务完成 耗时：" + (DateTime.Now - d).ToString());
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步微信文章数据，当前线程等待2秒");
                            System.Threading.Thread.Sleep(2000);
                        }
                    });
                });

                //Task t2 = Task.Factory.StartNew(() =>
                //{
                //    Parallel.For(0, TaskThreadCount, i =>
                //    {
                //        while (true)
                //        {
                //            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步头条号数据 任务开始");
                //            DateTime d = DateTime.Now;
                //            BusinessLL.ArticleInfo.Instance.SyncDataByJinRiTouTiao(QueryDataTopNum);
                //            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步头条号数据 任务完成 耗时：" + (DateTime.Now - d).ToString());
                //            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步头条号数据，当前线程等待2秒");
                //            System.Threading.Thread.Sleep(2000);
                //        }
                //    });
                //});

                //Task t3 = Task.Factory.StartNew(() =>
                //{
                //    Parallel.For(0, TaskThreadCount, i =>
                //    {
                //        while (true)
                //        {
                //            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步搜狐文章数据 任务开始");
                //            DateTime d = DateTime.Now;
                //            BusinessLL.ArticleInfo.Instance.SyncDataBySouhu(QueryDataTopNum);
                //            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步搜狐文章数据 任务完成 耗时：" + (DateTime.Now - d).ToString());
                //            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步搜狐文章数据，当前线程等待2秒");
                //            System.Threading.Thread.Sleep(2000);
                //        }
                //    });
                //});

                //ThreadPool.QueueUserWorkItem(x =>
                //{
                //    while (true)
                //    {
                //        BLL.Loger.Log4Net.Info("同步微信文章数据 任务开始");
                //        DateTime d = DateTime.Now;
                //        BusinessLL.ArticleInfo.Instance.SyncDataByWeixin(QueryDataTopNum);
                //        BLL.Loger.Log4Net.Info("同步微信文章数据 任务完成 耗时：" + (DateTime.Now - d).ToString());
                //        BLL.Loger.Log4Net.Info("同步微信文章数据，当前线程等待2秒");
                //        System.Threading.Thread.Sleep(2000);
                //    }
                //}, null);

                //ThreadPool.QueueUserWorkItem(x =>
                //{
                //    while (true)
                //    {
                //        BLL.Loger.Log4Net.Info("同步头条号数据 任务开始");
                //        DateTime d = DateTime.Now;
                //        //BusinessLL.ArticleInfo.Instance.SyncDataByJinRiTouTiao(QueryDataTopNum);
                //        BLL.Loger.Log4Net.Info("同步头条号数据 任务完成 耗时：" + (DateTime.Now - d).ToString());
                //        BLL.Loger.Log4Net.Info("同步头条号数据，当前线程等待2秒");
                //        System.Threading.Thread.Sleep(2000);
                //    }
                //}, null);

                //ThreadPool.QueueUserWorkItem(x =>
                //{
                //    while (true)
                //    {
                //        BLL.Loger.Log4Net.Info("同步搜狐文章数据 任务开始");
                //        DateTime d = DateTime.Now;
                //        //BusinessLL.ArticleInfo.Instance.SyncDataBySouhu(QueryDataTopNum);
                //        BLL.Loger.Log4Net.Info("同步搜狐文章数据 任务完成 耗时：" + (DateTime.Now - d).ToString());
                //        BLL.Loger.Log4Net.Info("同步搜狐文章数据，当前线程等待2秒");
                //        System.Threading.Thread.Sleep(2000);
                //    }
                //}, null);
                BLL.Loger.Log4Net.Info("同步文章数据-----结束");
            }
        }

        private string GetMailBodyByDT(DataTable dt)
        {
            StringBuilder mailContent = new StringBuilder();
            if (dt != null)
            {
                mailContent.Append("<table width='100%' border='1' cellpadding='0' cellspacing='0' align='center'  style='text-align:center;'>");
                if (dt.Columns.Count > 0)
                {
                    mailContent.Append("<tr>");
                    foreach (DataColumn item in dt.Columns)
                    {
                        mailContent.Append($"<td width='{100 / dt.Columns.Count}%'>{item.ColumnName}</td>");
                    }
                    mailContent.Append("</tr>");
                }
                foreach (DataRow dr in dt.Rows)
                {
                    mailContent.Append("<tr>");
                    foreach (DataColumn item in dt.Columns)
                    {
                        mailContent.Append($"<td>{dr[item.ColumnName].ToString()}</td>");
                    }
                    mailContent.Append("</tr>");
                }
                mailContent.Append("</table>");
            }
            return mailContent.ToString();

            //StringBuilder mailContent = new StringBuilder();
            //mailContent.Append("<table width='100%' border='1' cellpadding='0' cellspacing='0' align='center'  style='text-align:center;'>");
            //mailContent.Append("<tr> <td width='20%'>日期</td> <td width='20%'>小时</td> <td width='30%'>文章数</td> <td width='30%'>微信账号数</td></tr>");
            //if (dt != null)
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        string date = dr["Date"].ToString();
            //        string hour = dr["Hour"].ToString();
            //        string wxNumCount = dr["WXNumCount"].ToString();
            //        string wxArticleCount = dr["WXArticleCount"].ToString();
            //        mailContent.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", date, hour, wxArticleCount, wxNumCount));
            //    }
            //}
            //mailContent.Append("</table>");
            //return mailContent.ToString();
        }

        private void WriteAppSettings(string keyName, string value)
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //根据Key读取<add>元素的Value
            //string name = config.AppSettings.Settings["name"].Value;
            //写入<add>元素的Value
            config.AppSettings.Settings[keyName].Value = value;
            //增加<add>元素
            //config.AppSettings.Settings.Add("url", "http://www.fx163.net");
            //删除<add>元素
            //config.AppSettings.Settings.Remove("name");
            //一定要记得保存，写不带参数的config.Save()也可以
            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }

        private string GetAppSettings(string keyName)
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return config.AppSettings.Settings[keyName].Value;
        }

        public void SyncTask()
        {
            GetArticleInfo();
        }

        public void SyncByTime()
        {
            try
            {
                WriteAppSettings("SyncByTimeEnable", "8888");
                TimeSpan ts = SyncByTimeEndTime.Date.Subtract(SyncByTimeBeginTime.Date);
                BLL.Loger.Log4Net.Info(string.Format($"同步微信文章明细数据-----按时间段同步开始，开始时间:{SyncByTimeBeginTime.ToString()}，结束时间:{SyncByTimeEndTime.ToString()}，相差天数:{ts.Days}，SyncByTimeEndTime.Date:{SyncByTimeEndTime.Date}"));
                DateTime cur = SyncByTimeBeginTime;

                for (int i = 0; i <= ts.Days; i++)
                {
                    BLL.Loger.Log4Net.Info(string.Format($"同步微信文章明细数据-----按时间段同步，i:{i}"));
                    string filter = cur.ToString("yyyyMMdd");
                    int beginJ = 0;
                    int endJ = 0;
                    if (i == 0)
                    {
                        beginJ = SyncByTimeBeginTime.Hour;
                        endJ = 23;
                    }
                    else if (i == ts.Days)
                    {
                        beginJ = 0;
                        endJ = SyncByTimeEndTime.Hour;
                    }
                    else
                    {
                        beginJ = 0;
                        endJ = 23;
                    }
                    for (int j = beginJ; j <= endJ; j++)
                    {
                        string tmp = filter + j.ToString("00");
                        BLL.Loger.Log4Net.Info(string.Format($"同步微信文章明细数据-----按时间段同步，j:{j},filter:{tmp}"));
                        GetArticleInfo(tmp);
                    }
                    cur = cur.AddDays(1);
                }
                WriteAppSettings("SyncByTimeEnable", "9999");
                BLL.Loger.Log4Net.Info(string.Format($"同步微信文章明细数据-----按时间段同步结束，开始时间:{SyncByTimeBeginTime.ToString()}，结束时间:{SyncByTimeEndTime.ToString()}，相差天数:{ts.Days}"));
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format($"同步微信文章明细数据-----按时间段同步出错:{ex.Message}"));
            }
        }

        public void GetArticleInfo()
        {
            BLL.Loger.Log4Net.Info("同步获取明细数据-----方法开始");
            try
            {
                string errormsg = string.Empty;
                HBaseThriftHelper syncHelper = new HBaseThriftHelper();
                //List<string> listTables = syncHelper.GetTables();
                string tableName = "llbx:spider_data";
                List<string> cols = new List<string> { "ct:wxid", "ct:sn", "ct:farther_sn", "ct:pub_time", "ct:is_multi", "ct:location", "ct:push_time", "ct:biz", "ct:c_url", "ct:title", "ct:digest", "ct:cr_state", "ct:s_url", "ct:content", "ct:cover" };
                //string filterString = $"RowFilter(=,'regexstring:.*{DateTime.Now.AddDays(-5).ToString("yyyyMMddHH")}_wx_ct_.*')";
                string filter = DateTime.Now.AddHours(-1).ToString("yyyyMMddHH");
                string filterString = $"RowFilter(=,'regexstring:.*{filter}_wx_ct_.*')";
                //string filterString = "RowFilter(=,'regexstring:.*2017062600_wx_ct_.*')";
                var listRet = syncHelper.ScannerOpenWithScan(tableName, filterString, cols);//通过正则表达式，条件查找逻辑
                syncHelper.Close();//每次查询完之后，要关闭通道
                BLL.Loger.Log4Net.Info($"同步获取明细数据<{filter}>-----查询HBase结束,总记数：{listRet.Count}条!");
                errormsg = AddDataFromHbaseDB(errormsg, listRet);
                errormsg = AddDataALLFromHbaseDB(errormsg, listRet);

                if (!string.IsNullOrEmpty(errormsg))
                    BLL.Loger.Log4Net.Info($"同步获取明细数据最后批次-----出错：{errormsg}");

                BLL.Loger.Log4Net.Info($"同步获取明细数据<{filter}>-----方法结束");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("同步获取明细数据出错;" + ex.Message);
            }
        }

        private string AddDataFromHbaseDB(string errormsg, Dictionary<string, Dictionary<string, string>> listRet)
        {
            List<Entities.WXArticleInfo> listDto = new List<Entities.WXArticleInfo>();
            int iBatch = 0;
            foreach (var listItem in listRet)
            {
                Entities.WXArticleInfo article = new Entities.WXArticleInfo()
                {
                    WxNum = listItem.Value["ct:wxid"],
                    SN = listItem.Value["ct:sn"],
                    IsMulti = listItem.Value["ct:is_multi"] == "true" ? true : false,
                    FartherSN = listItem.Value["ct:farther_sn"],
                    CreateTime = DateTime.Now
                };
                DateTime dTmp = new DateTime(1990, 1, 1);
                if (DateTime.TryParse(listItem.Value["ct:pub_time"], out dTmp))
                    article.PubTime = dTmp;
                if (DateTime.TryParse(listItem.Value["ct:push_time"], out dTmp))
                    article.PushTime = dTmp;
                int iTmp = 0;
                if (int.TryParse(listItem.Value["ct:location"], out iTmp))
                    article.Location = iTmp;
                listDto.Add(article);

                if (listDto.Count >= iBatchSize)
                {
                    iBatch++;
                    BLL.Loger.Log4Net.Info("同步获取明细数据批次<" + iBatch + ">-----开始");
                    HBaseDataSync.DAL.WXArticleInfo.Instance.SyncData(BLL.Util.ListToDataTable<Entities.WXArticleInfo>(listDto.Take<Entities.WXArticleInfo>(iBatchSize).ToList()), "Weixin_ArticleInfo", Conn_NLP, out errormsg, iBatchSize);
                    if (!string.IsNullOrEmpty(errormsg))
                        BLL.Loger.Log4Net.Info($"同步获取明细数据批次<{iBatch}>-----出错：{errormsg}");

                    BLL.Loger.Log4Net.Info("同步获取明细数据批次<" + iBatch + ">-----结束");
                    listDto.RemoveRange(0, iBatchSize);
                }
            }

            if (listDto.Count > 0)
                HBaseDataSync.DAL.WXArticleInfo.Instance.SyncData(BLL.Util.ListToDataTable<Entities.WXArticleInfo>(listDto.Take<Entities.WXArticleInfo>(iBatchSize).ToList()), "Weixin_ArticleInfo", Conn_NLP, out errormsg, iBatchSize);
            return errormsg;
        }

        private string AddDataALLFromHbaseDB(string errormsg, Dictionary<string, Dictionary<string, string>> listRet)
        {
            List<Entities.Weixin_ArticleInfo> listDto = new List<Entities.Weixin_ArticleInfo>();
            int iBatch = 0;
            foreach (var listItem in listRet)
            {
                Entities.Weixin_ArticleInfo article = new Entities.Weixin_ArticleInfo()
                {
                    WxNum = listItem.Value["ct:wxid"],
                    SN = listItem.Value["ct:sn"],
                    IsMulti = listItem.Value["ct:is_multi"] == "true" ? true : false,
                    FartherSN = listItem.Value["ct:farther_sn"],
                    CreateTime = DateTime.Now
                };
                DateTime dTmp = new DateTime(1990, 1, 1);
                if (DateTime.TryParse(listItem.Value["ct:pub_time"], out dTmp))
                    article.PubTime = dTmp;
                if (DateTime.TryParse(listItem.Value["ct:push_time"], out dTmp))
                    article.PushTime = dTmp;
                int iTmp = 0;
                if (int.TryParse(listItem.Value["ct:location"], out iTmp))
                    article.Location = iTmp;
                article.Biz = listItem.Value["ct:biz"];
                article.ContentURL = listItem.Value["ct:c_url"];
                article.Title = listItem.Value["ct:title"];
                article.Digest = listItem.Value["ct:digest"];
                article.CopyrightState = listItem.Value["ct:cr_state"] != null && listItem.Value["ct:cr_state"].ToString() == "非原创" ? 0 : 1;
                article.SourceURL = listItem.Value["ct:s_url"];
                string content = listItem.Value["ct:content"].ToString();
                //article.Content = content;
                article.Content = Regex.Replace(content, @"(?<![&])nbsp;", "&nbsp;");//根据文章内容，找到缺少的nbsp;空格标签，替换完整;
                article.CoverURL = listItem.Value["ct:cover"];
                article.Rowkey = listItem.Key;

                listDto.Add(article);

                if (listDto.Count >= iBatchSize)
                {
                    iBatch++;
                    BLL.Loger.Log4Net.Info("同步获取明细数据批次<" + iBatch + ">-----开始");
                    HBaseDataSync.DAL.WXArticleInfo.Instance.SyncData(BLL.Util.ListToDataTable<Entities.Weixin_ArticleInfo>(listDto.Take<Entities.Weixin_ArticleInfo>(iBatchSize).ToList()), "Weixin_ArticleInfo", Conn_BaseData, out errormsg, iBatchSize);
                    if (!string.IsNullOrEmpty(errormsg))
                        BLL.Loger.Log4Net.Info($"同步获取明细数据批次<{iBatch}>-----出错：{errormsg}");

                    BLL.Loger.Log4Net.Info("同步获取明细数据批次<" + iBatch + ">-----结束");
                    listDto.RemoveRange(0, iBatchSize);
                }
            }

            if (listDto.Count > 0)
                HBaseDataSync.DAL.WXArticleInfo.Instance.SyncData(BLL.Util.ListToDataTable<Entities.Weixin_ArticleInfo>(listDto.Take<Entities.Weixin_ArticleInfo>(iBatchSize).ToList()), "Weixin_ArticleInfo", Conn_BaseData, out errormsg, iBatchSize);
            return errormsg;
        }



        //public void SyncTest()
        //{
        //    GetArticleInfo("2017061520");
        //    //2017062600到2017063011
        //    //for (int i = 2600; i <= 3011; i++)
        //    //{
        //    //    BLL.Loger.Log4Net.Info("SyncTest<201706" + i + ">-----");
        //    //    GetArticleInfo("201706" + i);
        //    //}            
        //}
        public void GetArticleInfo(string filter)
        {
            BLL.Loger.Log4Net.Info($"同步获取明细数据<{filter}>-----方法开始");
            try
            {
                string errormsg = string.Empty;
                HBaseThriftHelper syncHelper = new HBaseThriftHelper();
                //List<string> listTables = syncHelper.GetTables();
                string tableName = "llbx:spider_data";
                List<string> cols = new List<string> { "ct:wxid", "ct:sn", "ct:farther_sn", "ct:pub_time", "ct:is_multi", "ct:location", "ct:push_time", "ct:biz", "ct:c_url", "ct:title", "ct:digest", "ct:cr_state", "ct:s_url", "ct:content", "ct:cover" };
                string filterString = $"RowFilter(=,'regexstring:.*{filter}_wx_ct_.*')";
                var listRet = syncHelper.ScannerOpenWithScan(tableName, filterString, cols);//通过正则表达式，条件查找逻辑
                syncHelper.Close();//每次查询完之后，要关闭通道
                BLL.Loger.Log4Net.Info($"同步获取明细数据-----查询HBase结束,总记数：{listRet.Count}条!");
                errormsg = AddDataFromHbaseDB(errormsg, listRet);
                errormsg = AddDataALLFromHbaseDB(errormsg, listRet);

                if (!string.IsNullOrEmpty(errormsg))
                    BLL.Loger.Log4Net.Info($"同步获取明细数据最后批次-----出错：{errormsg}");

                BLL.Loger.Log4Net.Info($"同步获取明细数据<{filter}>-----方法结束");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("同步获取明细数据出错;" + ex.Message);
            }
        }

    }
}
