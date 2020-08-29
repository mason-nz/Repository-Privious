
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Transactions;
using System.Web;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.ITSC.Chitunion2017.LuceneMedia.Common;
using XYAuto.ITSC.Chitunion2017.LuceneMediaConsole.Common;

namespace XYAuto.ITSC.Chitunion2017.LuceneMedia.LuceneManage
{
    public class WeiXinMediaLucene
    {
        #region 单例
        private WeiXinMediaLucene() { }

        public static WeiXinMediaLucene instance = null;
        public static readonly object padlock = new object();

        public static readonly string path = ConfigurationManager.AppSettings["ConfigArgsPath"];

        public static WeiXinMediaLucene Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new WeiXinMediaLucene();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 添加索引


        public void AddIndexInfo()
        {
            try
            {

                //获取微信最后一次更新时间
                string WeiXinLastTime = ConfigXmlInfo.Instance.GetLastTime("WeiXinLastTime");

                int PageSize = 5000;

                int PageCount = 0;
                //获取微信总数
                int NumCount = MediaBll.Instance.GetWeiXinCount(new LuceneQuery { LastTime = WeiXinLastTime });

                if (NumCount > 0)
                {
                    if (NumCount > PageSize)
                    {
                        PageCount = NumCount % PageSize > 0 ? (NumCount / PageSize) + 1 : NumCount / PageSize;
                    }
                    else
                    {
                        PageCount = 1;
                    }
                    DateTime? MaxTime = null;


                    for (int i = 0; i < PageCount; i++)
                    {
                        Tuple<int, List<WeiXinLuceneModel>> queryList = MediaBll.Instance.GetWeiXinLuceneList(new LuceneQuery { PageIndex = i + 1, PageSize = PageSize, LastTime = WeiXinLastTime });
                        if (queryList.Item2.Count > 0)
                        {
                            //微信存储目录
                            WeiXinIndex(queryList.Item2);
                            ////存储微信刊位索引
                            //WeiXinPublishIndex(queryList.Item2);

                            var nowTime = queryList.Item2.Max(x => x.TimestampSign);
                            if (MaxTime == null)
                            {
                                MaxTime = nowTime;
                            }
                            else
                            {
                                MaxTime = MaxTime > nowTime ? MaxTime : nowTime;
                            }
                        }

                        Thread.Sleep(1000);
                    }

                    Log4NetHelper.Debug($"微信 上次索引获取时间：{WeiXinLastTime}  更新后索引时间：{MaxTime}");
                    ConfigXmlInfo.Instance.UpdateLastTime(MaxTime, "WeiXinLastTime");

                }

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("微信Lucene索引发生异常", ex);
                string t = ex.Message;
            }

        }
        #endregion

        #region 微信索引


        public void WeiXinIndex(List<WeiXinLuceneModel> queryList)
        {

            //微信存储目录
            string strPath = path + "\\Index\\WeiXin";
            //Log4NetHelper.Debug("微信索引存储位置：" + strPath);
            if (!System.IO.Directory.Exists(strPath))
                System.IO.Directory.CreateDirectory(strPath);

            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(strPath), new NativeFSLockFactory());
            bool isUpdate = IndexReader.IndexExists(directory); //判断索引库是否存在

            if (isUpdate)
            {
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            //  创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)

            Log4NetHelper.Debug("开始 执行 " + strPath);
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate,
                IndexWriter.MaxFieldLength.UNLIMITED);

            foreach (WeiXinLuceneModel entity in queryList)
            {
                //  一条Document相当于一条记录
                Document document = new Document();
                //  每个Document可以有自己的属性（字段），所有字段名都是自定义的，值都是string类型
                //  Field.Store.YES不仅要对文章进行分词记录，也要保存原文，就不用去数据库里查一次了

                Field WxNumberFile = new Field("WxNumber", entity.WxNumber + string.Empty, Field.Store.YES, Field.Index.ANALYZED);
                WxNumberFile.SetBoost(2);
                document.Add(WxNumberFile);

                Field NickNameFile = new Field("NickName", entity.NickName + string.Empty, Field.Store.YES, Field.Index.ANALYZED);
                NickNameFile.SetBoost(2);
                document.Add(NickNameFile);

                Field SummaryFile = new Field("Summary", entity.Summary + string.Empty, Field.Store.YES, Field.Index.ANALYZED);
                SummaryFile.SetBoost(1);
                document.Add(SummaryFile);

                Field CategoryNameFile = new Field("CategoryName", entity.CategoryName + string.Empty, Field.Store.YES, Field.Index.ANALYZED);
                CategoryNameFile.SetBoost(1);
                document.Add(CategoryNameFile);

                document.Add(new Field("RecID", entity.RecID + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("ServiceType", entity.ServiceType + string.Empty, Field.Store.YES, Field.Index.NO));
                document.Add(new Field("IsVerify", entity.IsVerify + string.Empty, Field.Store.YES, Field.Index.NO));
                document.Add(new Field("VerifyType", entity.VerifyType + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("HeadImg", entity.HeadImg + string.Empty, Field.Store.YES, Field.Index.NO));
                document.Add(new Field("FansCount", SplitHelper.AddedDigitsInt(entity.FansCount), Field.Store.YES, Field.Index.NOT_ANALYZED));

                document.Add(new Field("FullName", entity.FullName + string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("Sign", entity.Sign + string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("ReadNum", entity.ReadNum + string.Empty, Field.Store.YES, Field.Index.NO));
                document.Add(new Field("CategoryID", entity.CategoryID + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("IsOriginal", entity.IsOriginal + string.Empty, Field.Store.YES, Field.Index.NO));

                document.Add(new Field("ServiceTypeName", entity.ServiceTypeName + string.Empty, Field.Store.YES, Field.Index.NO));
                document.Add(new Field("VerifyTypeName", entity.VerifyTypeName + string.Empty, Field.Store.YES, Field.Index.NO));
                document.Add(new Field("ProvinceID", entity.ProvinceID + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("CityID", entity.CityID + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("RecID", entity.RecID + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("QrCodeUrl", entity.QrCodeUrl + string.Empty, Field.Store.YES, Field.Index.NO));

                document.Add(new Field("P60018001", SplitHelper.AddedDigitsFloat(entity.P60018001), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("P60018002", SplitHelper.AddedDigitsFloat(entity.P60018002), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("P60028001", SplitHelper.AddedDigitsFloat(entity.P60028001), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("P60028002", SplitHelper.AddedDigitsFloat(entity.P60028002), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("P60038001", SplitHelper.AddedDigitsFloat(entity.P60038001), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("P60038002", SplitHelper.AddedDigitsFloat(entity.P60038002), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("P60048001", SplitHelper.AddedDigitsFloat(entity.P60048001), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("P60048002", SplitHelper.AddedDigitsFloat(entity.P60048002), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("MaxPrice", SplitHelper.AddedDigitsFloat(entity.MaxPrice), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("MinPrice", SplitHelper.AddedDigitsFloat(entity.MinPrice), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("TotalScores", SplitHelper.AddedDigitsInt(entity.TotalScores), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("TimestampSign", entity.TimestampSign.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("TagText", SplitHelper.SplitText(entity.TagText+string.Empty, 5), Field.Store.YES, Field.Index.ANALYZED));

                document.Add(new Field("WeiXin", "all", Field.Store.YES, Field.Index.NOT_ANALYZED));


                //Lucene.Net.Search.Query CategoryIDQuery = new TermQuery(new Term("CategoryID", queryArgs.CategoryID + string.Empty));

                document.Add(new Field("WXID", entity.WxNumber + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                //  防止重复索引，如果不存在则删除0条
                writer.DeleteDocuments(new Term("WXID", entity.WxNumber + string.Empty));// 防止已存在的数据 => delete from t where id=i
                                                                                         //  把文档写入索引库
                writer.AddDocument(document);
            }

            Log4NetHelper.Debug("执行 结束：" + strPath);

            writer.Optimize();
            writer.Close(); // Close后自动对索引库文件解锁

            directory.Close(); //不要忘了Close，否则索引结果搜不到}
        }

        #endregion

        #region 微信刊位索引
        //public void WeiXinPublishIndex(List<WeiXinModel> queryList)
        //{

        //    //微信存储目录
        //    string strPath = Path.GetFullPath("Index/Publish");
        //    if (!System.IO.Directory.Exists(strPath))
        //        System.IO.Directory.CreateDirectory(strPath);

        //    FSDirectory directory = FSDirectory.Open(new DirectoryInfo(strPath), new NativeFSLockFactory());
        //    bool isUpdate = IndexReader.IndexExists(directory); //判断索引库是否存在
        //    if (isUpdate)
        //    {
        //        if (IndexWriter.IsLocked(directory))
        //        {
        //            IndexWriter.Unlock(directory);
        //        }
        //    }
        //    //  创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)

        //    IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate,
        //        IndexWriter.MaxFieldLength.UNLIMITED);


        //    foreach (WeiXinModel entity in queryList)
        //    {
        //        //  一条Document相当于一条记录
        //        Document document = new Document();
        //        //  每个Document可以有自己的属性（字段），所有字段名都是自定义的，值都是string类型
        //        //  Field.Store.YES不仅要对文章进行分词记录，也要保存原文，就不用去数据库里查一次了

        //        Field WxNumberFile = new Field("PositionName", entity.PositionName + string.Empty, Field.Store.YES, Field.Index.NO);

        //        document.Add(new Field("PublishName", entity.PublishName + string.Empty, Field.Store.YES, Field.Index.NO));

        //        document.Add(new Field("ADPosition1", entity.ADPosition1 + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));

        //        document.Add(new Field("ADPosition2", entity.ADPosition2 + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));

        //        document.Add(new Field("Price", SplitHelper.AddedDigitsFloat(entity.Price), Field.Store.YES, Field.Index.NOT_ANALYZED));

        //        document.Add(new Field("MediaID", entity.MediaID + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
        //        document.Add(new Field("IdentityNum", entity.IdentityNum + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));


        //        //  防止重复索引，如果不存在则删除0条
        //        writer.DeleteDocuments(new Term("IdentityNum", entity.IdentityNum + string.Empty));// 防止已存在的数据 => delete from t where id=i
        //                                                                                           //  把文档写入索引库
        //        writer.AddDocument(document);
        //    }

        //    writer.Close(); // Close后自动对索引库文件解锁
        //    directory.Close();  //  不要忘了Close，否则索引结果搜不到}
        //}

        #endregion




    }
}
