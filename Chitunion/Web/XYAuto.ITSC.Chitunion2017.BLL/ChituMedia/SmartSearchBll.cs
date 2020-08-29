using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Dto;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Dal.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia
{
    public class SmartSearchBll
    {
        #region 初始化
        private SmartSearchBll() { }

        public static SmartSearchBll instance = null;
        public static readonly object padlock = new object();

        public static SmartSearchBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new SmartSearchBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion


        public Dictionary<string, dynamic> GetSmartSearchMediaList(int SmartSearchID)
        {
            return new Dictionary<string, dynamic>() {
                { "CartWeiXin",GetSmartSearchWeiXin(SmartSearchID)},
                { "CartWeiBo",GetSmartSearchWeiBo(SmartSearchID)},
                { "CartApp",GetSmartSearchApp(SmartSearchID)}
            };
        }
        /// <summary>
        /// 根据用户ID获取购物车中 微信媒体信息列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private BasicResultDto GetSmartSearchWeiXin(int SmartSearchID)
        {
            var resultList = SmartSearchDa.Instance.GetSmartSearchWeiXin(SmartSearchID, UserInfo.GetLoginUser().UserID);
            ///微信列表信息排重
            var weixinList = (from q in resultList.AsEnumerable()
                              group q by new
                              {
                                  q.CategoryName,
                                  q.FansCount,
                                  q.FullName,
                                  q.HeadImg,
                                  q.IsOriginal,
                                  q.NickName,
                                  q.ReadNum,
                                  q.Sign,
                                  q.Summary,
                                  q.WxNumber,
                                  q.QrCodeUrl,
                                  q.TotalScores,
                                  q.RecID
                              } into G
                              select new WeiXinListModel
                              {
                                  CategoryName = G.Key.CategoryName,
                                  FansCount = G.Key.FansCount,
                                  FullName = G.Key.FullName,
                                  HeadImg = G.Key.HeadImg,
                                  QrCodeUrl = G.Key.QrCodeUrl,
                                  IsOriginal = G.Key.IsOriginal,
                                  NickName = G.Key.NickName,
                                  ReadNum = G.Key.ReadNum,
                                  Sign = G.Key.Sign,
                                  Summary = G.Key.Summary,
                                  WxNumber = G.Key.WxNumber,
                                  TotalScores= G.Key.TotalScores,
                                  RecID = G.Key.RecID
                              }).ToList();

            foreach (WeiXinListModel item in weixinList)
            {
                //格式化刊位信息
                var query = resultList.Where(p => p.MediaID == item.RecID).GroupBy(p => p.ADPosition1).Select(x => x.Key).ToList();
                List<PositionModel> positionList = new List<PositionModel>();
                foreach (int PositionID in query)
                {
                    PositionModel position = new PositionModel();
                    string name = SplitHelper.GetEnumDescriptionList<PositionEnum>(PositionID.ToString()).Value;
                    positionList.Add(new PositionModel
                    {
                        PositionName = name == PositionEnum.多图文3_N条.ToString() ? name.Replace('_', '-') : name,
                        IssuePrice = resultList.Where(p => p.MediaID == item.RecID && p.ADPosition1 == PositionID && p.ADPosition2 == 8001).Select(p => p.Price).FirstOrDefault(),
                        TotalPrice = resultList.Where(p => p.MediaID == item.RecID && p.ADPosition1 == PositionID && p.ADPosition2 == 8002).Select(p => p.Price).FirstOrDefault()

                    });
                }
                item.Position = positionList;
            }

            return new BasicResultDto { List = weixinList, TotalCount = weixinList.Count };
        }
        /// <summary>
        /// 根据用户ID获取购物车 微博媒体信息列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private BasicResultDto GetSmartSearchWeiBo(int SmartSearchID)
        {
            List<WeiBoModel> list = SmartSearchDa.Instance.GetSmartSearchWeiBo(SmartSearchID, UserInfo.GetLoginUser().UserID);

            return new BasicResultDto { List = list, TotalCount = list.Count };
        }


        /// <summary>
        /// 根据用户ID获取购物车 APP媒体信息列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private BasicResultDto GetSmartSearchApp(int SmartSearchID)
        {
            List<AppModel> list = SmartSearchDa.Instance.GetSmartSearchApp(SmartSearchID, UserInfo.GetLoginUser().UserID);

            return new BasicResultDto { List = list, TotalCount = list.Count };
        }
        /// <summary>
        /// 获取智能搜索列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BasicResultDto GetSmartSearchList(QueryArgs query, int UserID)
        {
            Tuple<int, List<SmartSearchListModel>> model = SmartSearchDa.Instance.GetSmartSearchList(query, UserID);

            return new BasicResultDto { List = model.Item2, TotalCount = model.Item1 };
        }

        /// <summary>
        /// 添加智能搜索列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int AddSmartSearchInfo(SmartSearchModel SearchModel)
        {
            if (SearchModel != null)
            {
                SearchModel.UserID = UserInfo.GetLoginUser().UserID;
            }
            return SmartSearchDa.Instance.AddSmartSearchInfo(SearchModel);
        }
        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        /// <param name="RecID"></param>
        /// <returns></returns>
        public SmartSearchListModel GetSmartSearchDetailInfo(int RecID)
        {
            return SmartSearchDa.Instance.GetSmartSearchDetailInfo(RecID, UserInfo.GetLoginUser().UserID);
        }
        /// <summary>
        /// 根据用户ID获取推广任务总数
        /// </summary>
        /// <param name="RecID"></param>
        /// <returns></returns>
        public int GetSmartSearchCount(int UserID)
        {
            return SmartSearchDa.Instance.GetSmartSearchCount(UserID);
        }

    }
}
