using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.ITSC.Chitunion2017.LuceneMediaConsole.Common;

namespace XYAuto.ITSC.Chitunion2017.LuceneMedia.LuceneManage
{
    public class WeiBoMediaLucene
    {
        #region 单例
        private WeiBoMediaLucene() { }

        public static WeiBoMediaLucene instance = null;
        public static readonly object padlock = new object();
        public static readonly string path = ConfigurationManager.AppSettings["ConfigArgsPath"];


        public static WeiBoMediaLucene Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new WeiBoMediaLucene();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        public void AddWeiBoIndex()
        {
            try
            {


                //获取微博最后一次更新时间
                string WeiBoLastTime = ConfigXmlInfo.Instance.GetLastTime("WeiBoLastTime");
                int PageSize = 10000;

                int PageCount = 0;
                Tuple<int, List<WeiBoModel>> list = MediaBll.Instance.GetWeiBoLuceneList(new LuceneQuery { LastTime = WeiBoLastTime });
                if (list.Item1 > 0)
                {
                    if (list.Item1 > PageSize)
                    {
                        PageCount = list.Item1 % PageSize > 0 ? (list.Item1 / PageSize) + 1 : list.Item1 / PageSize;
                    }
                    else
                    {
                        PageCount = 1;
                    }
                    DateTime? MaxTime = null;


                    for (int i = 0; i < PageCount; i++)
                    {

                        Tuple<int, List<WeiBoModel>> queryList = MediaBll.Instance.GetWeiBoLuceneList(new LuceneQuery { PageIndex = i + 1, PageSize = PageSize, LastTime = WeiBoLastTime });

                        //微信存储目录
                        string strPath = path + "\\Index\\WeiBo";
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

                        IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate,
                            IndexWriter.MaxFieldLength.UNLIMITED);

                        foreach (WeiBoModel entity in queryList.Item2)
                        {

                            //  一条Document相当于一条记录
                            Document document = new Document();
                            //  每个Document可以有自己的属性（字段），所有字段名都是自定义的，值都是string类型
                            //  Field.Store.YES不仅要对文章进行分词记录，也要保存原文，就不用去数据库里查一次了

                            Field NumberFile = new Field("Number", entity.Number + string.Empty, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                            NumberFile.SetBoost(2);
                            document.Add(NumberFile);

                            Field NameFile = new Field("Name", entity.Name + string.Empty, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                            NameFile.SetBoost(2);
                            document.Add(NameFile);

                            Field SummaryFile = new Field("Summary", entity.Summary + string.Empty, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                            SummaryFile.SetBoost(1);
                            document.Add(SummaryFile);

                            Field CategoryNameFile = new Field("CategoryName", entity.CategoryName + string.Empty, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                            CategoryNameFile.SetBoost(1);
                            document.Add(CategoryNameFile);


                            document.Add(new Field("HeadIconURL", entity.HeadIconURL + string.Empty, Field.Store.YES, Field.Index.NO));

                            document.Add(new Field("FansCount", SplitHelper.AddedDigitsInt(entity.FansCount), Field.Store.YES, Field.Index.NOT_ANALYZED));
                            document.Add(new Field("CategoryID", entity.CategoryID + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                            document.Add(new Field("Sex", entity.Sex + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));
                            document.Add(new Field("ForwardAvg", entity.ForwardAvg + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                            document.Add(new Field("CommentAvg", entity.CommentAvg + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

                            document.Add(new Field("LikeAvg", entity.LikeAvg + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                            document.Add(new Field("DirectPrice", SplitHelper.AddedDigitsFloat(entity.DirectPrice), Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                            document.Add(new Field("ForwardPrice", SplitHelper.AddedDigitsFloat(entity.ForwardPrice), Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                            document.Add(new Field("ProvinceID", entity.ProvinceID + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                            document.Add(new Field("CityID", entity.CityID + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                            document.Add(new Field("RecID", entity.RecID + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                            document.Add(new Field("AuthType", entity.AuthType + string.Empty, Field.Store.YES, Field.Index.NO));
                            document.Add(new Field("TotalScores", SplitHelper.AddedDigitsInt(entity.TotalScores), Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                            document.Add(new Field("TagText", SplitHelper.SplitText(entity.TagText + string.Empty, 5), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

                            document.Add(new Field("WeiBo", "all", Field.Store.YES, Field.Index.NOT_ANALYZED));
                            document.Add(new Field("WBID", entity.Number + string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED));

                            //  防止重复索引，如果不存在则删除0条
                            writer.DeleteDocuments(new Term("WBID", entity.Number + string.Empty));// 防止已存在的数据 => delete from t where id=i
                                                                                                   //  把文档写入索引库
                            writer.AddDocument(document);
                        }
                        var nowTime = queryList.Item2.Max(x => x.TimestampSign);
                        if (MaxTime == null)
                        {
                            MaxTime = nowTime;
                        }
                        else
                        {
                            MaxTime = MaxTime > nowTime ? MaxTime : nowTime;
                        }
                        writer.Optimize();
                        writer.Close(); // Close后自动对索引库文件解锁

                        directory.Close();  //  不要忘了Close，否则索引结果搜不到}

                    }
                    Log4NetHelper.Debug($"微博 上次索引获取时间：{WeiBoLastTime}  更新后索引时间：{MaxTime}");
                    ConfigXmlInfo.Instance.UpdateLastTime(MaxTime, "WeiBoLastTime");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {

                Log4NetHelper.Error("微博Lucene索引发生异常", ex);
            }
        }
    }
}
