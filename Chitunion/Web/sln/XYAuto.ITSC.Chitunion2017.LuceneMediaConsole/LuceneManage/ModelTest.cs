using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using PanGu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.LuceneMediaConsole.LuceneManage
{
    public class ModelTest
    {
        public void GetMediaMatchingList()
        {
            MediaQueryArgs queryArgs = new MediaQueryArgs
            {
              
            };
            List<WeiXinListModel> weixinList = new List<WeiXinListModel>();
            try
            {

                //获取当前登录用户信息
                int pageSize = 20;
                int pageIndex = 1;

               
                bool queryBool = false;
                #region MyRegion

                #endregion
                string strPath = Path.GetFullPath("Index/WeiXin");

                FSDirectory directory = FSDirectory.Open(new DirectoryInfo(strPath), new NoLockFactory());
                IndexReader reader = IndexReader.Open(directory, true);

                IndexSearcher searcher = new IndexSearcher(reader);


                //声明微信组合查询
                BooleanQuery booleanQuery = new BooleanQuery();


                //获取有效数据总量
                int PageCount = reader.NumDocs();



                Lucene.Net.Search.Query query = new TermQuery(new Term("WeiXin", "all"));
                booleanQuery.Add(query, BooleanClause.Occur.MUST);



                TopDocs docss = searcher.Search(booleanQuery, (Filter)null, 1 * 20);
                //搜索关键字不为空
                if (!string.IsNullOrEmpty(queryArgs.Keyword))
                {
                    BooleanQuery KeywordQuery = new BooleanQuery();
                    string keyValue = GetKeyWordsSplitBySpace(queryArgs.Keyword);

                    //根据微信号搜索
                    QueryParser WxNumberPars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "WxNumber", new PanGuAnalyzer());
                    Lucene.Net.Search.Query WxNumberQuery = WxNumberPars.Parse(keyValue);
                    WxNumberPars.SetDefaultOperator(QueryParser.Operator.AND);
                    KeywordQuery.Add(WxNumberQuery, BooleanClause.Occur.SHOULD);

                    //根据昵称搜索
                    QueryParser NickNamePars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "NickName", new PanGuAnalyzer());
                    Lucene.Net.Search.Query NickNameQuery = NickNamePars.Parse(keyValue);
                    NickNamePars.SetDefaultOperator(QueryParser.Operator.AND);

                    KeywordQuery.Add(NickNameQuery, BooleanClause.Occur.SHOULD);

                    //根据简介搜索
                    QueryParser SummaryPars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Summary", new PanGuAnalyzer());
                    Lucene.Net.Search.Query SummaryQuery = SummaryPars.Parse(keyValue);
                    SummaryPars.SetDefaultOperator(QueryParser.Operator.AND);

                    KeywordQuery.Add(SummaryQuery, BooleanClause.Occur.SHOULD);

                    //根据分类名称搜索
                    QueryParser CategoryNamePars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "CategoryName", new PanGuAnalyzer());
                    Lucene.Net.Search.Query CategoryNameQuery = CategoryNamePars.Parse(keyValue);
                    CategoryNamePars.SetDefaultOperator(QueryParser.Operator.AND);

                    KeywordQuery.Add(CategoryNameQuery, BooleanClause.Occur.SHOULD);

                    booleanQuery.Add(KeywordQuery, BooleanClause.Occur.MUST);

                }
                //根据行业分类ID查询
                if (queryArgs.CategoryID > 0)
                {
                    Lucene.Net.Search.Query CategoryIDQuery = new TermQuery(new Term("CategoryID", queryArgs.CategoryID + string.Empty));
                    booleanQuery.Add(CategoryIDQuery, BooleanClause.Occur.MUST);
                }
                //粉丝量区间查询
                if (queryArgs.MaxFansCount > 0 || queryArgs.MinFansCount > 0)
                {

                    string MaxFans = queryArgs.MaxFansCount <= 0 ? SplitHelper.AddedDigitsInt(1) : SplitHelper.AddedDigitsInt(queryArgs.MaxFansCount);
                    string MinFans = queryArgs.MinFansCount <= 0 ? SplitHelper.AddedDigitsInt(0) : SplitHelper.AddedDigitsInt(queryArgs.MinFansCount);
                    TermRangeQuery FansTerm = new TermRangeQuery("FansCount", MinFans, MaxFans, true, true);

                    booleanQuery.Add(FansTerm, BooleanClause.Occur.MUST);
                }
                //参考价格区间查询
                if (queryArgs.MaxPrice > 0 || queryArgs.MinPrice > 0)
                {
                    string MaxPrice = queryArgs.MaxPrice <= 0 ? SplitHelper.AddedDigitsFloat(1) : SplitHelper.AddedDigitsFloat(queryArgs.MaxFansCount);
                    string MinPrice = queryArgs.MinPrice <= 0 ? SplitHelper.AddedDigitsFloat(0) : SplitHelper.AddedDigitsFloat(queryArgs.MinFansCount);

                    BooleanQuery priceQuery = new BooleanQuery();
                    if (queryArgs.ADPositionID > 0)
                    {
                        EnumInfo position = SplitHelper.GetEnumDescriptionList<PositionEnum>(queryArgs.ADPositionID.ToString());
                        if (position != null)
                        {
                            TermRangeQuery BeginPrice = new TermRangeQuery(position.Description.Split(',')[0] + string.Empty, MinPrice, MaxPrice, true, true);
                            TermRangeQuery EndPrice = new TermRangeQuery(position.Description.Split(',')[1] + string.Empty, MinPrice, MaxPrice, true, true);
                            priceQuery.Add(BeginPrice, BooleanClause.Occur.SHOULD);
                            priceQuery.Add(EndPrice, BooleanClause.Occur.SHOULD);

                        }
                    }
                    else
                    {
                        TermRangeQuery BeginPrice = new TermRangeQuery("MaxPrice", MinPrice, MaxPrice, true, true);
                        TermRangeQuery EndPrice = new TermRangeQuery("MaxPrice", MinPrice, MaxPrice, true, true);
                        priceQuery.Add(BeginPrice, BooleanClause.Occur.SHOULD);
                        priceQuery.Add(EndPrice, BooleanClause.Occur.SHOULD);
                    }
                    booleanQuery.Add(priceQuery, BooleanClause.Occur.MUST);
                }
                //省级ID
                if (queryArgs.ProvinceID >= 0)
                {
                    Lucene.Net.Search.Query ProvinceQuery = new TermQuery(new Term("ProvinceID", queryArgs.ProvinceID + string.Empty));
                    booleanQuery.Add(ProvinceQuery, BooleanClause.Occur.MUST);
                }
                //城市ID
                if (queryArgs.CityID > 0)
                {
                    Lucene.Net.Search.Query CityQuery = new TermQuery(new Term("CityID", queryArgs.CityID + string.Empty));
                    booleanQuery.Add(CityQuery, BooleanClause.Occur.MUST);
                }
                Sort sort = new Sort();
                if (queryArgs.SortField > 0)
                {
                    bool sortBool = true;
                    EnumInfo sortEnum = SplitHelper.GetEnumDescriptionList<SortEnum>(((int)queryArgs.SortField).ToString());

                    EnumInfo sortInde = SplitHelper.GetEnumDescriptionList<SortInde>(((int)queryArgs.SortIndex).ToString());

                    if (sortInde != null)
                    {
                        sortBool = sortEnum.Key == 1 ? false : sortEnum.Key == 2 ? true : false;
                    }
                    if (sortEnum != null)
                    {
                        //搜索结果放入collector
                        sort.SetSort(new SortField(sortEnum.Value, SortField.DOC, sortBool));
                    }
                }
                else
                {

                    sort.SetSort(new SortField("TotalScores", SortField.DOC, true));
                    sort.SetSort(new SortField("TimestampSign", SortField.DOC, true));
                }
                //根据字段排序
                TopFieldCollector collector = TopFieldCollector.create(sort, 20, false, false, false, false);


                searcher.Search(booleanQuery, collector);

                //获取总数
                int totalNum = collector.GetTotalHits();

                // 从查询结果中取出第m条到第n条的数据
                ScoreDoc[] ScoreDoc = collector.TopDocs(pageIndex, pageSize).scoreDocs;

                for (int i = 0; i < ScoreDoc.Length; i++)
                {
                    int docid = ScoreDoc[i].doc;

                    Document doc = searcher.Doc(docid);

                    List<PositionModel> positionModel = new List<PositionModel>();

                    //单图文
                    if (!string.IsNullOrEmpty(doc.Get("P60018001")) && !string.IsNullOrEmpty(doc.Get("P60018002")))
                    {
                        var queryModel = SplitHelper.GetEnumDescriptionList<PositionEnum>("6001");

                        positionModel.Add(new PositionModel
                        {
                            PositionName = queryModel.Value,
                            IssuePrice = ConvertHelper.ToDecimal(ConvertHelper.ToInt32(doc.Get("P60018001")) / 100),
                            TotalPrice = ConvertHelper.ToDecimal(ConvertHelper.ToInt32(doc.Get("P60018002")) / 100)
                        });
                    }
                    //多图文第一条
                    if (!string.IsNullOrEmpty(doc.Get("P60028001")) && !string.IsNullOrEmpty(doc.Get("P60028002")))
                    {
                        var queryModel = SplitHelper.GetEnumDescriptionList<PositionEnum>("6002");

                        positionModel.Add(new PositionModel
                        {
                            PositionName = queryModel.Value,
                            IssuePrice = ConvertHelper.ToDecimal(ConvertHelper.ToInt32(doc.Get("P60028001")) / 100),
                            TotalPrice = ConvertHelper.ToDecimal(ConvertHelper.ToInt32(doc.Get("P60028002")) / 100)
                        });
                    }
                    //多图文第二条
                    if (!string.IsNullOrEmpty(doc.Get("P60038001")) && !string.IsNullOrEmpty(doc.Get("P60038002")))
                    {
                        var queryModel = SplitHelper.GetEnumDescriptionList<PositionEnum>("6003");

                        positionModel.Add(new PositionModel
                        {
                            PositionName = queryModel.Value,
                            IssuePrice = ConvertHelper.ToDecimal(ConvertHelper.ToInt32(doc.Get("P60038001")) / 100),
                            TotalPrice = ConvertHelper.ToDecimal(ConvertHelper.ToInt32(doc.Get("P60038002")) / 100)
                        });
                    }
                    //多图文3-N条
                    if (!string.IsNullOrEmpty(doc.Get("P60048001")) && !string.IsNullOrEmpty(doc.Get("P60048002")))
                    {
                        var queryModel = SplitHelper.GetEnumDescriptionList<PositionEnum>("6004");

                        positionModel.Add(new PositionModel
                        {
                            PositionName = queryModel.Value.Replace('_', '-'),
                            IssuePrice = ConvertHelper.ToDecimal(ConvertHelper.ToInt32(doc.Get("P60048001")) / 100),
                            TotalPrice = ConvertHelper.ToDecimal(ConvertHelper.ToInt32(doc.Get("P60048002")) / 100)
                        });
                    }


                    WeiXinListModel weiXinModel = new WeiXinListModel()
                    {
                        WxNumber = doc.Get("WxNumber"),
                        NickName = doc.Get("NickName"),
                        Summary = doc.Get("Summary"),
                        CategoryName = doc.Get("CategoryName"),
                        HeadImg = doc.Get("HeadImg"),
                        FansCount = ConvertHelper.ToInt32(doc.Get("FansCount")),
                        FullName = doc.Get("FullName"),
                        Sign = doc.Get("Sign"),
                        ReadNum = ConvertHelper.ToInt32(doc.Get("ReadNum")),
                        IsOriginal = ConvertHelper.ToInt32(doc.Get("IsOriginal")),
                        Position = positionModel
                    };
                    weixinList.Add(weiXinModel);
                }

            }
            catch (Exception ex)
            {

            }
        }
        private string GetKeyWordsSplitBySpace(string keywords)
        {
            PanGuTokenizer ktTokenizer = new PanGuTokenizer();
            StringBuilder result = new StringBuilder();
            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);
            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }
                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }
            return result.ToString().Trim();
        }
    }
}
