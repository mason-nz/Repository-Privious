
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Function;
using Lucene.Net.Store;
using PanGu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Dto;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Dal.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia
{
    public class MediaBll
    {
        #region 单例
        private MediaBll() { }

        public static MediaBll instance = null;
        public static readonly object padlock = new object();

        public static MediaBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new MediaBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion


        public int GetWeiXinCount(LuceneQuery query)
        {

            return MediaDa.Instance.GetWeiXinCount(query);
        }

        #region 获取微信原始表数据
        /// <summary>
        /// 获取微信原始表数据
        /// </summary>
        /// <returns></returns>
        public Tuple<int, List<WeiXinLuceneModel>> GetWeiXinLuceneList(LuceneQuery query)
        {

            return MediaDa.Instance.GetWeiXinLuceneList(query);
        }
        #endregion

        #region 获取微博原始表数据
        /// <summary>
        /// 获取微博原始表数据
        /// </summary>
        /// <returns></returns>
        public Tuple<int, List<WeiBoModel>> GetWeiBoLuceneList(LuceneQuery query)
        {

            return MediaDa.Instance.GetWeiBoLuceneList(query);
        }
        #endregion

        #region 列表入口
        public Dictionary<string, dynamic> GetMediaMatchingList(MediaQueryArgs queryArgs)
        {
            Dictionary<string, Func<MediaQueryArgs, Tuple<string, BasicResultDto>>> _dic = new Dictionary<string, Func<MediaQueryArgs, Tuple<string, BasicResultDto>>>{
                { "wx_list",m=> GetWeiXinList(m) },

                { "wb_list", m=> GetWeiBoList(m) },

                { "app_list", m=> GetAppList(m) }
            };
            Dictionary<string, dynamic> dynamicList = new Dictionary<string, dynamic>();
            //获取需要查询图形集合
            List<string> chartList = queryArgs.ListType.Split(',').ToList();

            foreach (var item in chartList)
            {
                if (_dic.ContainsKey(item.ToLower()))
                {
                    var query = _dic[item].Invoke(queryArgs);
                    dynamicList.Add(query.Item1, query.Item2);
                }
            }

            return dynamicList;
        }
        #endregion

        #region 获取标签信息


        private List<TagVehicleInfoList> GetTagVehicleLucene(string Keyword)
        {
            var TagList = new List<TagVehicleInfoList>();

            try
            {
                string strPath = ConfigurationUtil.GetAppSettingValue("LuceneFile") + "\\Index\\TagVehicle";

                FSDirectory directory = FSDirectory.Open(new DirectoryInfo(strPath), new NoLockFactory());
                IndexReader reader = IndexReader.Open(directory, true);

                IndexSearcher searcher = new IndexSearcher(reader);

                //获取有效数据总量
                int PageCount = reader.NumDocs();


                BooleanQuery KeywordQuery = new BooleanQuery();
                //搜索关键字不为空
                if (!string.IsNullOrEmpty(Keyword))
                {

                    //根据品牌
                    QueryParser MasterNamePars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "MasterName", new PanGuAnalyzer());
                    Lucene.Net.Search.Query MasterNameQuery = MasterNamePars.Parse(Keyword);
                    MasterNamePars.SetDefaultOperator(QueryParser.Operator.AND);
                    KeywordQuery.Add(MasterNameQuery, BooleanClause.Occur.SHOULD);

                    //根据子品牌
                    QueryParser BrandNamePars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "BrandName", new PanGuAnalyzer());
                    Lucene.Net.Search.Query BrandNameQuery = BrandNamePars.Parse(Keyword);
                    BrandNamePars.SetDefaultOperator(QueryParser.Operator.AND);
                    KeywordQuery.Add(BrandNameQuery, BooleanClause.Occur.SHOULD);

                    //根据车系
                    QueryParser SerialNameNamePars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "SerialName", new PanGuAnalyzer());
                    Lucene.Net.Search.Query SerialNameNameQuery = SerialNameNamePars.Parse(Keyword);
                    SerialNameNamePars.SetDefaultOperator(QueryParser.Operator.AND);
                    KeywordQuery.Add(SerialNameNameQuery, BooleanClause.Occur.SHOULD);

                }

                Sort sort = new Sort();
                sort.SetSort(SortField.FIELD_SCORE);
                //根据字段排序
                TopFieldCollector collector = TopFieldCollector.create(sort, reader.NumDocs(), false, false, false, false);

                searcher.Search(KeywordQuery, collector);

                if (collector.GetTotalHits() > 0)
                {
                    // 从查询结果中取出第m条到第n条的数据
                    ScoreDoc[] ScoreDoc = collector.TopDocs(0, 5).scoreDocs;
                    for (int i = 0; i < ScoreDoc.Length; i++)
                    {
                        int docid = ScoreDoc[i].doc;

                        TagList.Add(new TagVehicleInfoList
                        {
                            MediaTagName = searcher.Doc(docid).Get("MediaTagName"),
                            SerialID = ConvertHelper.ToInt32(searcher.Doc(docid).Get("SerialID"))
                        });
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return TagList;
        }

        #endregion

        #region 获取微信列表

        /// <summary>
        /// 根据条件从索引库中获取微信列表
        /// </summary>
        public Tuple<string, BasicResultDto> GetWeiXinList(MediaQueryArgs queryArgs)
        {
            string fielPath = ConfigurationUtil.GetAppSettingValue("LuceneFile");
            List<WeiXinListModel> weixinList = new List<WeiXinListModel>();
            //获取总数
            int totalNum = 0;
            bool IsSearch = false;



            List<TagVehicleInfoList> TagArray = null;
            try
            {

                #region MyRegion

                #endregion
                string strPath = fielPath + "\\Index\\WeiXin";

                FSDirectory directory = FSDirectory.Open(new DirectoryInfo(strPath), new NoLockFactory());
                IndexReader reader = IndexReader.Open(directory, true);

                IndexSearcher searcher = new IndexSearcher(reader);


                //声明微信组合查询
                BooleanQuery booleanQuery = new BooleanQuery();


                //获取有效数据总量
                int PageCount = reader.NumDocs();



                Lucene.Net.Search.Query query = new TermQuery(new Term("WeiXin", "all"));
                booleanQuery.Add(query, BooleanClause.Occur.MUST);

                //搜索关键字不为空
                if (!string.IsNullOrEmpty(queryArgs.Keyword))
                {
                    //如果有搜索条件最多显示20条
                    queryArgs.PageSize = 20;
                    queryArgs.PageIndex = 1;

                    //记录关键字日志
                    Loger.LuceneLogger.Info($"微信—关键字搜索：{queryArgs.Keyword}");

                    BooleanQuery KeywordQuery = new BooleanQuery();
                    string keyValue = GetKeyWordsSplitBySpace(queryArgs.Keyword);

                    TagArray = GetTagVehicleLucene(keyValue);
                    //云词图查询
                    if (TagArray.Count > 0)
                    {
                        foreach (TagVehicleInfoList itemValue in TagArray)
                        {
                            //根据标签搜索
                            QueryParser TagTextPars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "TagText", new PanGuAnalyzer());

                            string TagKyeValue = GetKeyWordsSplitBySpace(itemValue.MediaTagName);
                            Lucene.Net.Search.Query TagTextQuery = TagTextPars.Parse(TagKyeValue);
                            TagTextPars.SetDefaultOperator(QueryParser.Operator.AND);
                            KeywordQuery.Add(TagTextQuery, BooleanClause.Occur.SHOULD);
                        }
                    }

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

                    ////根据简介搜索
                    //QueryParser SummaryPars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Summary", new PanGuAnalyzer());
                    //Lucene.Net.Search.Query SummaryQuery = SummaryPars.Parse(keyValue);
                    //SummaryPars.SetDefaultOperator(QueryParser.Operator.AND);

                    //KeywordQuery.Add(SummaryQuery, BooleanClause.Occur.SHOULD);

                    //根据分类名称搜索
                    QueryParser CategoryNamePars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "CategoryName", new PanGuAnalyzer());
                    Lucene.Net.Search.Query CategoryNameQuery = CategoryNamePars.Parse(keyValue);
                    CategoryNamePars.SetDefaultOperator(QueryParser.Operator.AND);

                    KeywordQuery.Add(CategoryNameQuery, BooleanClause.Occur.SHOULD);

                    booleanQuery.Add(KeywordQuery, BooleanClause.Occur.MUST);

                    IsSearch = true;

                }


                //根据行业分类ID查询
                if (queryArgs.CategoryID > 0)
                {
                    Lucene.Net.Search.Query CategoryIDQuery = new TermQuery(new Term("CategoryID", queryArgs.CategoryID + string.Empty));
                    booleanQuery.Add(CategoryIDQuery, BooleanClause.Occur.MUST);
                    IsSearch = true;
                }
                //粉丝量区间查询
                if (queryArgs.MaxFansCount > 0 || queryArgs.MinFansCount > 0)
                {

                    string MaxFans = queryArgs.MaxFansCount <= 0 ? SplitHelper.AddedRightFloat(1) : SplitHelper.AddedDigitsInt(queryArgs.MaxFansCount);
                    string MinFans = queryArgs.MinFansCount <= 0 ? SplitHelper.AddedDigitsInt(0) : SplitHelper.AddedDigitsInt(queryArgs.MinFansCount);
                    TermRangeQuery FansTerm = new TermRangeQuery("FansCount", MinFans, MaxFans, true, true);

                    booleanQuery.Add(FansTerm, BooleanClause.Occur.MUST);
                    IsSearch = true;
                }
                //参考价格区间查询
                if (queryArgs.MaxPrice > 0 || queryArgs.MinPrice > 0)
                {
                    string MaxPrice = queryArgs.MaxPrice <= 0 ? SplitHelper.AddedRightFloat(1) : SplitHelper.AddedDigitsFloat(queryArgs.MaxPrice);
                    string MinPrice = queryArgs.MinPrice <= 0 ? SplitHelper.AddedDigitsFloat(0) : SplitHelper.AddedDigitsFloat(queryArgs.MinPrice);

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
                        TermRangeQuery BeginPrice = new TermRangeQuery("MinPrice", MinPrice, MaxPrice, true, true);
                        TermRangeQuery EndPrice = new TermRangeQuery("MaxPrice", MinPrice, MaxPrice, true, true);
                        priceQuery.Add(BeginPrice, BooleanClause.Occur.SHOULD);
                        priceQuery.Add(EndPrice, BooleanClause.Occur.SHOULD);
                    }
                    booleanQuery.Add(priceQuery, BooleanClause.Occur.MUST);
                    IsSearch = true;
                }
                //省级ID
                if (queryArgs.ProvinceID >= 0)
                {
                    Lucene.Net.Search.Query ProvinceQuery = new TermQuery(new Term("ProvinceID", queryArgs.ProvinceID + string.Empty));
                    booleanQuery.Add(ProvinceQuery, BooleanClause.Occur.MUST);
                    IsSearch = true;
                }
                //城市ID
                if (queryArgs.CityID > 0)
                {
                    Lucene.Net.Search.Query CityQuery = new TermQuery(new Term("CityID", queryArgs.CityID + string.Empty));
                    booleanQuery.Add(CityQuery, BooleanClause.Occur.MUST);
                    IsSearch = true;
                }
                Sort sort = new Sort();
                #region 按字段排序
                //if (queryArgs.SortField > 0)
                //{
                //    bool sortBool = true;
                //    EnumInfo sortEnum = SplitHelper.GetEnumDescriptionList<SortEnum>(((int)queryArgs.SortField).ToString());

                //    EnumInfo sortInde = SplitHelper.GetEnumDescriptionList<SortInde>(((int)queryArgs.SortIndex).ToString());

                //    if (sortInde != null)
                //    {
                //        sortBool = sortEnum.Key == 1 ? false : sortEnum.Key == 2 ? true : false;
                //    }
                //    if (sortEnum != null)
                //    {
                //        //搜索结果放入collector
                //        sort.SetSort(new SortField(sortEnum.Value, SortField.DOC, sortBool));
                //    }
                //}
                //else
                //{
                //    if (IsSearch)
                //    {

                //        sort.SetSort(new SortField("TimestampSign", SortField.STRING, true));
                //    }
                //}
                #endregion

                //判断显示条数
                if (UserInfo.GetLoginUser() == null || IsSearch)
                {
                    queryArgs.PageSize = 20;
                    queryArgs.PageIndex = 1;
                }
                if (IsSearch)
                {
                    sort.SetSort(SortField.FIELD_SCORE);
                }
                else
                {
                    sort.SetSort(new SortField("TotalScores", SortField.STRING, true));
                }

                //根据字段排序
                TopFieldCollector collector = TopFieldCollector.create(sort, reader.NumDocs(), false, true, false, false);


                searcher.Search(booleanQuery, collector);


                //TopScoreDocCollector collector = TopScoreDocCollector.create(reader.NumDocs(), true);
                // 使用query这个查询条件进行搜索，搜索结果放入collector
                //searcher.Search(booleanQuery, null, collector);

                //获取总数
                totalNum = collector.GetTotalHits();

                // 从查询结果中取出第m条到第n条的数据
                ScoreDoc[] ScoreDoc = collector.TopDocs((queryArgs.PageIndex - 1) * queryArgs.PageSize, queryArgs.PageSize).scoreDocs;

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
                            IssuePrice = ConvertHelper.ToDecimal(ConvertHelper.ToDecimal(doc.Get("P60018001")) / 100),
                            TotalPrice = ConvertHelper.ToDecimal(ConvertHelper.ToDecimal(doc.Get("P60018002")) / 100)
                        });
                    }
                    //多图文第一条
                    if (!string.IsNullOrEmpty(doc.Get("P60028001")) && !string.IsNullOrEmpty(doc.Get("P60028002")))
                    {
                        var queryModel = SplitHelper.GetEnumDescriptionList<PositionEnum>("6002");

                        positionModel.Add(new PositionModel
                        {
                            PositionName = queryModel.Value,
                            IssuePrice = ConvertHelper.ToDecimal(ConvertHelper.ToDecimal(doc.Get("P60028001")) / 100),
                            TotalPrice = ConvertHelper.ToDecimal(ConvertHelper.ToDecimal(doc.Get("P60028002")) / 100)
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
                    var GroupByModel = positionModel.Where(p => p.IssuePrice > 0 || p.TotalPrice > 0).ToList();
                    WeiXinListModel weiXinModel = new WeiXinListModel()
                    {
                        RecID = ConvertHelper.ToInt32(doc.Get("RecID")),
                        WxNumber = doc.Get("WxNumber"),
                        NickName =  doc.Get("NickName"),
                        Summary = doc.Get("Summary"),
                        CategoryName = doc.Get("CategoryName"),
                        QrCodeUrl = doc.Get("QrCodeUrl"),
                        HeadImg = doc.Get("HeadImg"),
                        FansCount = ConvertHelper.ToInt32(doc.Get("FansCount")),
                        FullName = doc.Get("FullName"),
                        Sign = doc.Get("Sign"),
                        ReadNum = ConvertHelper.ToInt32(doc.Get("ReadNum")),
                        IsOriginal = ConvertHelper.ToInt32(doc.Get("IsOriginal")),
                        TotalScores = ConvertHelper.ToInt32(doc.Get("TotalScores")),
                        TagText = doc.Get("TagText"),
                        IndexScore = (float.IsNaN(ScoreDoc[i].score) ? -1 : (decimal)ScoreDoc[i].score),
                        Position = GroupByModel
                    };
                    weixinList.Add(weiXinModel);
                }

            }
            catch (Exception ex)
            {

            }
            return new Tuple<string, BasicResultDto>("WeiXinList", new BasicResultDto
            {
                SourceTagList = TagArray,
                TotalCount = totalNum,
                List = weixinList
            });

        }

        #endregion

        #region 获取微博列表

        public Tuple<string, BasicResultDto> GetWeiBoList(MediaQueryArgs queryArgs)
        {
            string fielPath = ConfigurationUtil.GetAppSettingValue("LuceneFile");
            List<WeiBoModel> weiboList = new List<WeiBoModel>();

            bool IsSearch = false;
            int totalNum = 0;

            List<TagVehicleInfoList> TagArray = null;
            try
            {
                string strPath = fielPath + "\\Index\\WeiBo";

                FSDirectory directory = FSDirectory.Open(new DirectoryInfo(strPath), new NoLockFactory());
                IndexReader reader = IndexReader.Open(directory, true);

                IndexSearcher searcher = new IndexSearcher(reader);

                //声明微信组合查询
                BooleanQuery booleanQuery = new BooleanQuery();
                //获取有效数据总量
                int PageCount = reader.NumDocs();

                Lucene.Net.Search.Query query = new TermQuery(new Term("WeiBo", "all"));
                booleanQuery.Add(query, BooleanClause.Occur.MUST);

                //搜索关键字不为空
                if (!string.IsNullOrEmpty(queryArgs.Keyword))
                {
                    //如果有搜索条件最多显示20条
                    queryArgs.PageSize = 20;
                    queryArgs.PageIndex = 1;
                    BooleanQuery KeywordQuery = new BooleanQuery();
                    string keyValue = GetKeyWordsSplitBySpace(queryArgs.Keyword);

                    //记录关键字日志
                    Loger.LuceneLogger.Info($"微博—关键字搜索：{queryArgs.Keyword}");

                    TagArray = GetTagVehicleLucene(keyValue);
                    //云词图查询
                    if (TagArray.Count > 0)
                    {
                        foreach (TagVehicleInfoList itemEntity in TagArray)
                        {
                            //根据标签搜索
                            QueryParser TagTextPars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "TagText", new PanGuAnalyzer());

                            string TagKyeValue = GetKeyWordsSplitBySpace(itemEntity.MediaTagName);
                            Lucene.Net.Search.Query TagTextQuery = TagTextPars.Parse(TagKyeValue);
                            TagTextPars.SetDefaultOperator(QueryParser.Operator.AND);
                            KeywordQuery.Add(TagTextQuery, BooleanClause.Occur.SHOULD);
                        }
                    }


                    //根据微博号搜索
                    QueryParser NumberberPars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Number", new PanGuAnalyzer());
                    Lucene.Net.Search.Query NumberberQuery = NumberberPars.Parse(keyValue);
                    NumberberPars.SetDefaultOperator(QueryParser.Operator.AND);
                    KeywordQuery.Add(NumberberQuery, BooleanClause.Occur.SHOULD);

                    //根据昵称搜索
                    QueryParser NickNamePars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Name", new PanGuAnalyzer());
                    Lucene.Net.Search.Query NickNameQuery = NickNamePars.Parse(keyValue);
                    NickNamePars.SetDefaultOperator(QueryParser.Operator.AND);

                    KeywordQuery.Add(NickNameQuery, BooleanClause.Occur.SHOULD);

                    //根据简介搜索
                    //QueryParser SignPars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Summary", new PanGuAnalyzer());
                    //Lucene.Net.Search.Query SignQuery = SignPars.Parse(keyValue);
                    //SignPars.SetDefaultOperator(QueryParser.Operator.AND);

                    //KeywordQuery.Add(SignQuery, BooleanClause.Occur.SHOULD);

                    //根据分类名称搜索
                    QueryParser CategoryNamePars = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "CategoryName", new PanGuAnalyzer());
                    Lucene.Net.Search.Query CategoryNameQuery = CategoryNamePars.Parse(keyValue);
                    CategoryNamePars.SetDefaultOperator(QueryParser.Operator.AND);

                    KeywordQuery.Add(CategoryNameQuery, BooleanClause.Occur.SHOULD);

                    booleanQuery.Add(KeywordQuery, BooleanClause.Occur.MUST);
                    IsSearch = true;
                }
                //根据行业分类ID查询
                if (queryArgs.CategoryID > 0)
                {
                    Lucene.Net.Search.Query CategoryIDQuery = new TermQuery(new Term("CategoryID", queryArgs.CategoryID + string.Empty));
                    booleanQuery.Add(CategoryIDQuery, BooleanClause.Occur.MUST);
                    IsSearch = true;
                }
                //粉丝量区间查询
                if (queryArgs.MaxFansCount > 0 || queryArgs.MinFansCount > 0)
                {

                    string MaxFans = queryArgs.MaxFansCount <= 0 ? SplitHelper.AddedRightFloat(1) : SplitHelper.AddedDigitsInt(queryArgs.MaxFansCount);
                    string MinFans = queryArgs.MinFansCount <= 0 ? SplitHelper.AddedDigitsInt(0) : SplitHelper.AddedDigitsInt(queryArgs.MinFansCount);
                    TermRangeQuery FansTerm = new TermRangeQuery("FansCount", MinFans, MaxFans, true, true);

                    booleanQuery.Add(FansTerm, BooleanClause.Occur.MUST);
                    IsSearch = true;
                }
                //参考价格区间查询
                if (queryArgs.MaxPrice > 0 || queryArgs.MinPrice > 0)
                {
                    string MaxPrice = queryArgs.MaxPrice <= 0 ? SplitHelper.AddedRightFloat(1) : SplitHelper.AddedDigitsFloat(queryArgs.MaxPrice);
                    string MinPrice = queryArgs.MinPrice <= 0 ? SplitHelper.AddedDigitsFloat(0) : SplitHelper.AddedDigitsFloat(queryArgs.MinPrice);

                    if (queryArgs.ReferenceType == 1)//直发参考价
                    {
                        TermRangeQuery FansTerm = new TermRangeQuery("DirectPrice", MinPrice, MaxPrice, true, true);

                        booleanQuery.Add(FansTerm, BooleanClause.Occur.MUST);
                    }
                    else if (queryArgs.ReferenceType == 2)//转发参考价
                    {
                        TermRangeQuery FansTerm = new TermRangeQuery("ForwardPrice", MinPrice, MaxPrice, true, true);

                        booleanQuery.Add(FansTerm, BooleanClause.Occur.MUST);

                    }
                    else if (queryArgs.ReferenceType <= 0)
                    {
                        BooleanQuery ReferenceQuery = new BooleanQuery();
                        TermRangeQuery DirectTerm = new TermRangeQuery("DirectPrice", MinPrice, MaxPrice, true, true);

                        ReferenceQuery.Add(DirectTerm, BooleanClause.Occur.SHOULD);

                        TermRangeQuery ForwardTerm = new TermRangeQuery("ForwardPrice", MinPrice, MaxPrice, true, true);

                        ReferenceQuery.Add(ForwardTerm, BooleanClause.Occur.SHOULD);


                        booleanQuery.Add(ReferenceQuery, BooleanClause.Occur.MUST);
                    }
                    IsSearch = true;
                }

                Sort sort = new Sort();

                #region 按字段排序
                //if (queryArgs.SortField > 0)
                //{
                //    bool sortBool = true;
                //    EnumInfo sortEnum = SplitHelper.GetEnumDescriptionList<SortEnum>(((int)queryArgs.SortField).ToString());

                //    EnumInfo sortInde = SplitHelper.GetEnumDescriptionList<SortInde>(((int)queryArgs.SortIndex).ToString());

                //    if (sortInde != null)
                //    {
                //        sortBool = sortEnum.Key == 1 ? false : sortEnum.Key == 2 ? true : false;
                //    }
                //    if (sortEnum != null)
                //    {
                //        //搜索结果放入collector
                //        sort.SetSort(new SortField(sortEnum.Value, SortField.DOC, sortBool));
                //    }
                //}
                //else
                //{
                //    if (IsSearch)
                //    {
                //        sort.SetSort(new SortField("TimestampSign", SortField.STRING, true));
                //    }
                //}
                #endregion

                if (IsSearch)
                {
                    sort.SetSort(SortField.FIELD_SCORE);
                }
                else
                {
                    sort.SetSort(new SortField("TotalScores", SortField.STRING, true));
                }
                ////判断显示条数
                if (UserInfo.GetLoginUser() == null || IsSearch)
                {
                    queryArgs.PageSize = 20;
                    queryArgs.PageIndex = 1;
                }

                //根据字段排序
                TopFieldCollector collector = TopFieldCollector.create(sort, reader.NumDocs(), false, true, false, false);

                //TopScoreDocCollector collector = TopScoreDocCollector.create(reader.NumDocs(), true);
                // 使用query这个查询条件进行搜索，搜索结果放入collector
                //searcher.Search(booleanQuery, null, collector);

                searcher.Search(booleanQuery, collector);
                //获取总数
                totalNum = collector.GetTotalHits();

                // 从查询结果中取出第m条到第n条的数据
                ScoreDoc[] ScoreDoc = collector.TopDocs((queryArgs.PageIndex - 1) * queryArgs.PageSize, queryArgs.PageSize).scoreDocs;

                for (int i = 0; i < ScoreDoc.Length; i++)
                {
                    int docid = ScoreDoc[i].doc;

                    Document doc = searcher.Doc(docid);

                    WeiBoModel weiXinModel = new WeiBoModel()
                    {
                        RecID = ConvertHelper.ToInt32(doc.Get("RecID")),
                        Number = doc.Get("Number"),
                        Name = doc.Get("Name"),
                        Sign = doc.Get("Sign"),
                        Summary = doc.Get("Summary"),
                        CategoryName = doc.Get("CategoryName"),
                        HeadIconURL = doc.Get("HeadIconURL"),
                        FansCount = ConvertHelper.ToInt32(doc.Get("FansCount")),
                        Sex = doc.Get("Sex"),
                        AuthType = ConvertHelper.ToInt32(doc.Get("AuthType")),
                        ForwardAvg = ConvertHelper.ToInt32(doc.Get("ForwardAvg")),
                        CommentAvg = ConvertHelper.ToInt32(doc.Get("CommentAvg")),
                        LikeAvg = ConvertHelper.ToInt32(doc.Get("LikeAvg")),
                        TotalScores = ConvertHelper.ToInt32(doc.Get("TotalScores")),
                        IndexScore = (float.IsNaN(ScoreDoc[i].score) ? -1 : (decimal)ScoreDoc[i].score),
                        DirectPrice = Convert.ToDecimal(ConvertHelper.ToDecimal(doc.Get("DirectPrice")) / 100),
                        ForwardPrice = Convert.ToDecimal(ConvertHelper.ToDecimal(doc.Get("ForwardPrice")) / 100),
                        TagText = doc.Get("TagText")

                    };
                    weiboList.Add(weiXinModel);
                }

            }
            catch (Exception ex)
            {

            }
            return new Tuple<string, BasicResultDto>("WeiBoList", new BasicResultDto
            {
                SourceTagList = TagArray,
                TotalCount = totalNum,
                List = weiboList
            });
        }

        #endregion

        #region 获取APP列表

        public Tuple<string, BasicResultDto> GetAppList(MediaQueryArgs queryArgs)
        {

            //判断是否登录
            if (UserInfo.GetLoginUser() == null || queryArgs.CategoryID > 0)
            {
                queryArgs.PageSize = 20;
                queryArgs.PageIndex = 1;
            }
            var listKeyword = new List<TagVehicleInfoList>();
            if (!string.IsNullOrEmpty(queryArgs.Keyword))
            {
                //记录关键字日志
                Loger.LuceneLogger.Info($"APP—关键字搜索：{queryArgs.Keyword}");

                string keyValue = GetKeyWordsSplitBySpace(queryArgs.Keyword);
                listKeyword = GetTagVehicleLucene(keyValue);
            }

            Tuple<int, List<AppModel>> tem = MediaDa.Instance.GetAppLuceneList(queryArgs, listKeyword);

            return new Tuple<string, BasicResultDto>("AppList", new BasicResultDto
            {
                SourceTagList = listKeyword,
                TotalCount = tem.Item1,
                List = tem.Item2
            });
        }
        #endregion

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

        public EnumInfo GetEnumDescription<T>(string enumName)
        {
            return Enum.GetValues(typeof(T)).OfType<Enum>().Where(m => m.ToString() == enumName).Select(x => new EnumInfo
            {
                Key = Convert.ToInt32(x),
                Value = x.ToString(),
                Description = x.GetDescription()
            }).FirstOrDefault();
        }
    }
}
