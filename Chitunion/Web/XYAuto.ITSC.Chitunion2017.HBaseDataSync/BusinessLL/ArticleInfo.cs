using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.HBaseDataSync.DAL;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.BusinessLL
{
    public class ArticleInfo
    {
        public static readonly ArticleInfo Instance = new ArticleInfo();
        private readonly int iBatchSize = 1;//ConfigurationManager.AppSettings["iBatchSize"] == "" ? 2000 : Convert.ToInt16(ConfigurationManager.AppSettings["iBatchSize"]);
        private string Conn_BaseData = ConfigurationManager.AppSettings["ConnectionStrings_BaseData"];

        public bool IsExistByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return false;
            }
            return DAL.ArticleInfo.Instance.IsExistByTitle(title);
        }

        /// <summary>
        /// 根据topNum的数量，获取Weixin_ArticleInfo中，待同步的数据，然后清洗文章内容，入ArticleInfo表中
        /// </summary>
        /// <param name="topNum">每次获取文章数量</param>
        public void SyncDataByWeixin(int topNum)
        {
            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步微信文章数据-----开始");
            string errormsg = string.Empty;
            DataTable dt = Weixin_ArticleInfo.Instance.GetData(topNum, 1);
            try
            {
                List<Entities.ArticleInfo> listDto = new List<Entities.ArticleInfo>();
                int iBatch = 0;
                int dataIndex = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dataIndex++;
                        string recid = dr["RecID"].ToString();
                        BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],微信文章数据,目前循环到第{dataIndex}个，总共需要{dt.Rows.Count}条数据要清洗");
                        Entities.ArticleInfo article = new Entities.ArticleInfo()
                        {
                            XyAttr = 1,
                            ComNum = 0,
                            Resource = 1,
                            IsIndex = 0,
                            CreateTime = DateTime.Now,
                            LastUpdateTime = DateTime.Now
                        };
                        article.Url = dr["ContentURL"].ToString();
                        article.Title = dr["Title"].ToString();
                        article.Author = dr["Author"].ToString();
                        if (string.IsNullOrWhiteSpace(Util.Instance.FilterArticleTitle(article.Title)))
                        {
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],微信文章数据,SN={recid}，文章标题={article.Title}，有特定短语，已剔除");
                            Weixin_ArticleInfo.Instance.UpdateStatusByIDs(recid, 2);
                            continue;
                        }
                        article.HeadImg = dr["CoverURL"].ToString();
                        article.ReadNum = int.Parse(dr["ReadNum"].ToString());
                        article.LikeNum = int.Parse(dr["LikeNum"].ToString());
                        string content = dr["Content"].ToString().Trim();
                        if (string.IsNullOrWhiteSpace(content) ||
                            content == "该文章可能被删除" ||
                            content == "该内容已被发布者删除" ||
                            content == "此内容因违规无法查看" ||
                            content == "此帐号已被屏蔽, 内容无法查看" ||
                            BusinessLL.ArticleInfo.Instance.IsExistByTitle(article.Title))
                        //article.Title.Contains("小三") || article.Title.Contains("出轨") ||
                        //article.Title.Contains("胸大") || article.Title.Contains("奶大") ||
                        //article.Title.Contains("睾")
                        {
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],微信文章数据,SN={recid}，有特定词语，已剔除");
                            Weixin_ArticleInfo.Instance.UpdateStatusByIDs(recid, 2);
                            continue;
                        }
                        string headImage = Util.Instance.CleanImg(article.HeadImg);
                        if (!string.IsNullOrWhiteSpace(headImage))
                        {
                            //临时注释
                            if (!Util.Instance.CheckImgContent(recid, headImage, article.Url))
                            {
                                BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],微信文章数据,SN={recid}，头图不符合规则，已剔除");
                                Weixin_ArticleInfo.Instance.UpdateStatusByIDs(recid, 2);
                                continue;
                            }
                            article.HeadImgNew = Util.Instance.GetUrlByCutForCustom(article.HeadImg, headImage);
                            article.HeadImg = headImage;
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],微信文章数据,SN={recid}，没有头图，已剔除");
                            Weixin_ArticleInfo.Instance.UpdateStatusByIDs(recid, 2);
                            continue;
                        }


                        article.Content = Util.Instance.FilterADByContent(content, article.Title, recid, 1, article.Url, article);
                        if (string.IsNullOrWhiteSpace(article.Content))
                        {
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],微信文章数据,SN={recid}，没有文章内容，已剔除");
                            Weixin_ArticleInfo.Instance.UpdateStatusByIDs(recid, 2);
                            continue;
                        }

                        //article.Content = content;
                        article.JsonContent = "[{\"Content\":" + Newtonsoft.Json.JsonConvert.SerializeObject(article.Content) + "}]";
                        article.Abstract = dr["Digest"].ToString();
                        article.CopyrightState = int.Parse(dr["CopyrightState"].ToString()) == 1 ? 1 : 2;
                        article.DataId = dr["WxNum"].ToString();
                        article.Score = DAL.ArticleInfo.Instance.ComputeArticleValueByNumber(article.DataId, 1);
                        article.RowKey = dr["Rowkey"].ToString();
                        article.PublishTime = DateTime.Parse(dr["PubTime"].ToString());

                        listDto.Add(article);

                        if (listDto.Count >= iBatchSize)
                        {
                            iBatch++;
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步微信文章数据，批次<" + iBatch + ">-----开始");
                            HBaseDataSync.DAL.WXArticleInfo.Instance.SyncData(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto.Take<Entities.ArticleInfo>(iBatchSize).ToList()), "ArticleInfo", Conn_BaseData, out errormsg, iBatchSize);
                            //HBaseDataSync.DAL.WXArticleInfo.Instance.SyncData(article, "ArticleInfo", Conn_BaseData,out errormsg);
                            if (!string.IsNullOrEmpty(errormsg))
                            {
                                BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步微信文章数据，批次<{iBatch}>-----出错：{errormsg}");
                            }
                            //string ids = Util.Instance.GetRecIDsByDataTable(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto), "RecID");
                            Weixin_ArticleInfo.Instance.UpdateStatusByIDs(recid, 2);
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步微信文章数据，批次<" + iBatch + ">-----结束");
                            listDto.RemoveRange(0, iBatchSize);
                        }
                    }
                    //string ids = Util.Instance.GetRecIDsByDataTable(dt, "RecID");
                    //Weixin_ArticleInfo.Instance.UpdateStatusByIDs(ids, 2);
                }


                //if (listDto.Count > 0)
                //{
                //    HBaseDataSync.DAL.WXArticleInfo.Instance.SyncData(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto.Take<Entities.ArticleInfo>(iBatchSize).ToList()), "ArticleInfo", Conn_BaseData, out errormsg, iBatchSize);
                //    if (!string.IsNullOrEmpty(errormsg))
                //    {
                //        BLL.Loger.Log4Net.Info($"同步微信文章数据，最后批次-----出错：{errormsg}");
                //    }
                //    string ids = Util.Instance.GetRecIDsByDataTable(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto), "RecID");
                //    Weixin_ArticleInfo.Instance.UpdateStatusByIDs(ids, 2);
                //}
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("同步微信文章数据，出错;", ex);
            }

            BLL.Loger.Log4Net.Info("同步微信文章数据-----结束");
        }

        /// <summary>
        /// 根据topNum的数量，获取搜狐SouHuArticleInfo中，待同步的数据，然后清洗文章内容，入ArticleInfo表中
        /// </summary>
        /// <param name="queryDataTopNum"></param>
        internal void SyncDataBySouhu(int topNum)
        {
            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步搜狐文章数据-----开始");
            string errormsg = string.Empty;
            DataTable dt = Weixin_ArticleInfo.Instance.GetData(topNum, 6);
            try
            {
                List<Entities.ArticleInfo> listDto = new List<Entities.ArticleInfo>();
                int iBatch = 0;
                int parseInt = 0;
                int dataIndex = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dataIndex++;
                        string recid = dr["RecID"].ToString();
                        BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],搜狐文章数据,目前循环到第{dataIndex}个，总共需要{dt.Rows.Count}条数据要清洗");
                        Entities.ArticleInfo article = new Entities.ArticleInfo()
                        {
                            XyAttr = 1,
                            LikeNum = 0,
                            Resource = 6,
                            IsIndex = 0,
                            CopyrightState = 0,
                            CreateTime = DateTime.Now,
                            LastUpdateTime = DateTime.Now,
                            RowKey = string.Empty
                        };

                        article.Url = dr["Url"].ToString();
                        article.Title = dr["Title"].ToString().Trim();
                        if (BusinessLL.ArticleInfo.Instance.IsExistByTitle(article.Title))
                        {
                            SouHuArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(Util.Instance.FilterArticleTitle(article.Title)))
                        {
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],搜狐文章数据,SN={dr["RecID"].ToString()}，文章标题={article.Title}，有特定短语，已剔除");
                            Weixin_ArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            continue;
                        }
                        article.HeadImg = dr["HeadImg"].ToString();
                        string headImage = Util.Instance.CleanImg(article.HeadImg);
                        if (!string.IsNullOrWhiteSpace(headImage))
                        {
                            article.HeadImg = headImage;
                            //临时注释
                            //if (!Util.Instance.CheckImgContent(dr["RecID"].ToString(), headImage, article.Url))
                            //{
                            //    BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],搜狐文章数据,SN={dr["RecID"].ToString()}，头图不符合规则，已剔除");
                            //    SouHuArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            //    continue;
                            //}
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],搜狐文章数据,SN={dr["RecID"].ToString()}，没有头图，已剔除");
                            SouHuArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            continue;
                        }
                        article.ReadNum = int.Parse(dr["ReadNum"].ToString());
                        article.ComNum = int.Parse(dr["ComNum"].ToString());
                        string content = dr["Content"].ToString().Trim();
                        article.Content = Util.Instance.FilterADByContent(content, article.Title, dr["RecID"].ToString(), 2, article.Url, article);
                        //article.Content = content;
                        if (string.IsNullOrWhiteSpace(article.Content))
                        {
                            SouHuArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            continue;
                        }
                        article.JsonContent = "[{\"Content\":" + Newtonsoft.Json.JsonConvert.SerializeObject(article.Content) + "}]";
                        article.Abstract = dr["Abstract"].ToString();
                        if (int.TryParse(dr["CopyrightState"].ToString(), out parseInt))
                        {
                            article.CopyrightState = parseInt == 1 ? 1 : 2;
                        }
                        article.DataId = dr["UserId"].ToString();
                        article.Score = DAL.ArticleInfo.Instance.ComputeArticleValueByNumber(article.DataId, 6);
                        article.DataName = dr["UserName"].ToString();
                        article.PublishTime = DateTime.Parse(dr["PublishTime"].ToString());
                        article.Category = dr["Category"].ToString().Trim();
                        if (article.Category == "汽车")
                        {
                            article.XyAttr = 2;
                        }
                        article.Tag = dr["Tag"].ToString();

                        listDto.Add(article);

                        if (listDto.Count >= iBatchSize)
                        {
                            iBatch++;
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步搜狐文章数据，批次<" + iBatch + ">-----开始");
                            HBaseDataSync.DAL.WXArticleInfo.Instance.SyncData(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto.Take<Entities.ArticleInfo>(iBatchSize).ToList()), "ArticleInfo", Conn_BaseData, out errormsg, iBatchSize);
                            if (!string.IsNullOrEmpty(errormsg))
                            {
                                BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步搜狐文章数据，批次<{iBatch}>-----出错：{errormsg}");
                            }
                            //string ids = Util.Instance.GetRecIDsByDataTable(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto), "RecID");
                            SouHuArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步搜狐文章数据，批次<" + iBatch + ">-----结束");
                            listDto.RemoveRange(0, iBatchSize);
                        }

                    }
                    //string ids = Util.Instance.GetRecIDsByDataTable(dt, "RecID");
                    //SouHuArticleInfo.Instance.UpdateStatusByIDs(ids, 1);
                }

                //if (listDto.Count > 0)
                //{
                //    HBaseDataSync.DAL.WXArticleInfo.Instance.SyncData(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto.Take<Entities.ArticleInfo>(iBatchSize).ToList()), "ArticleInfo", Conn_BaseData, out errormsg, iBatchSize);
                //    if (!string.IsNullOrEmpty(errormsg))
                //    {
                //        BLL.Loger.Log4Net.Info($"同步搜狐文章数据，最后批次-----出错：{errormsg}");
                //    }
                //    //string ids = Util.Instance.GetRecIDsByDataTable(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto), "RecID");
                //    SouHuArticleInfo.Instance.UpdateStatusByIDs(ids, 1);
                //}
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步搜狐文章数据，出错;", ex);
            }

            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步搜狐文章数据-----结束");
        }


        /// <summary>
        /// 根据topNum的数量，获取今日头条Weixin_ArticleInfo中，待同步的数据，然后清洗文章内容，入ArticleInfo表中
        /// </summary>
        /// <param name="topNum">每次获取文章数量</param>
        public void SyncDataByJinRiTouTiao(int topNum)
        {
            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步今日头条文章数据-----开始");
            string errormsg = string.Empty;
            DataTable dt = Weixin_ArticleInfo.Instance.GetData(topNum, 3);
            try
            {
                List<Entities.ArticleInfo> listDto = new List<Entities.ArticleInfo>();
                int iBatch = 0;
                int parseInt = 0;
                int dataIndex = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dataIndex++;
                        string recid = dr["Id"].ToString();
                        BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],今日头条文章数据,目前循环到第{dataIndex}个，总共需要{dt.Rows.Count}条数据要清洗");
                        Entities.ArticleInfo article = new Entities.ArticleInfo()
                        {
                            XyAttr = 1,
                            LikeNum = 0,
                            Resource = 3,
                            IsIndex = 0,
                            CopyrightState = 0,
                            CreateTime = DateTime.Now,
                            LastUpdateTime = DateTime.Now,
                            RowKey = string.Empty
                        };

                        article.Url = dr["Url"].ToString();
                        article.Title = dr["Title"].ToString().Trim();
                        if (BusinessLL.ArticleInfo.Instance.IsExistByTitle(article.Title))
                        {
                            TouTiaoArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(Util.Instance.FilterArticleTitle(article.Title)))
                        {
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],头条文章数据,SN={dr["Id"].ToString()}，文章标题={article.Title}，有特定短语，已剔除");
                            Weixin_ArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            continue;
                        }
                        article.HeadImg = dr["Cover"].ToString();
                        string headImage = Util.Instance.CleanImg(article.HeadImg);
                        if (!string.IsNullOrWhiteSpace(headImage))
                        {
                            article.HeadImg = headImage;
                            //临时注释
                            //if (!Util.Instance.CheckImgContent(dr["Id"].ToString(), headImage, article.Url))
                            //{
                            //    BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],头条文章数据,SN={dr["Id"].ToString()}，头图不符合规则，已剔除");
                            //    TouTiaoArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            //    continue;
                            //}
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],头条文章数据,SN={dr["Id"].ToString()}，没有头图，已剔除");
                            TouTiaoArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            continue;
                        }
                        article.ReadNum = int.Parse(dr["ReadNum"].ToString());
                        article.ComNum = int.Parse(dr["ComNum"].ToString());
                        string content = dr["Content"].ToString().Trim();
                        article.Content = Util.Instance.FilterADByContent(content, article.Title, dr["Id"].ToString(), 2, article.Url, article);
                        //article.Content = content;
                        if (string.IsNullOrWhiteSpace(article.Content))
                        {
                            TouTiaoArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            continue;
                        }
                        article.JsonContent = "[{\"Content\":" + Newtonsoft.Json.JsonConvert.SerializeObject(article.Content) + "}]";
                        article.Abstract = dr["Discription"].ToString();
                        if (int.TryParse(dr["CopyRight"].ToString(), out parseInt))
                        {
                            article.CopyrightState = parseInt == 1 ? 1 : 2;
                        }
                        article.DataId = dr["UserId"].ToString();
                        article.Score = DAL.ArticleInfo.Instance.ComputeArticleValueByNumber(article.DataId, 3);
                        article.DataName = dr["UserName"].ToString();
                        article.PublishTime = DateTime.Parse(dr["PublishTime"].ToString());
                        article.Category = dr["Category"].ToString();
                        if (article.Category == "汽车")
                        {
                            article.XyAttr = 2;
                        }
                        article.Tag = dr["Tags"].ToString();

                        listDto.Add(article);

                        if (listDto.Count >= iBatchSize)
                        {
                            iBatch++;
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步今日头条文章数据，批次<" + iBatch + ">-----开始");
                            HBaseDataSync.DAL.WXArticleInfo.Instance.SyncData(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto.Take<Entities.ArticleInfo>(iBatchSize).ToList()), "ArticleInfo", Conn_BaseData, out errormsg, iBatchSize);
                            if (!string.IsNullOrEmpty(errormsg))
                            {
                                BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步今日头条文章数据，批次<{iBatch}>-----出错：{errormsg}");
                            }
                            //string ids = Util.Instance.GetRecIDsByDataTable(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto), "Id");
                            TouTiaoArticleInfo.Instance.UpdateStatusByIDs(recid, 1);
                            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步今日头条文章数据，批次<" + iBatch + ">-----结束");
                            listDto.RemoveRange(0, iBatchSize);
                        }

                    }
                    //string ids = Util.Instance.GetRecIDsByDataTable(dt, "Id");
                    //TouTiaoArticleInfo.Instance.UpdateStatusByIDs(ids, 1);
                }

                //if (listDto.Count > 0)
                //{
                //    HBaseDataSync.DAL.WXArticleInfo.Instance.SyncData(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto.Take<Entities.ArticleInfo>(iBatchSize).ToList()), "ArticleInfo", Conn_BaseData, out errormsg, iBatchSize);
                //    if (!string.IsNullOrEmpty(errormsg))
                //    {
                //        BLL.Loger.Log4Net.Info($"同步今日头条文章数据，最后批次-----出错：{errormsg}");
                //    }
                //    string ids = Util.Instance.GetRecIDsByDataTable(BLL.Util.ListToDataTable<Entities.ArticleInfo>(listDto), "Id");
                //    TouTiaoArticleInfo.Instance.UpdateStatusByIDs(ids, 1);
                //}
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步今日头条文章数据，出错;", ex);
            }

            BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],同步今日头条文章数据-----结束");
        }

    }
}
