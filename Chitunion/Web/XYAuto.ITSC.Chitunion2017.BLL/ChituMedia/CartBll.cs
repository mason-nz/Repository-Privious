using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Dto;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Dal.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia
{
    public class CartBll
    {
        #region 初始化
        private CartBll() { }

        public static CartBll instance = null;
        public static readonly object padlock = new object();

        public static CartBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new CartBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 根据用户ID获取购物车列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<CartModel> GetCartInfoList(int UserID)
        {
            return CartDa.Instance.GetCartInfoList(UserID);
        }
        /// <summary>
        /// 根据ID删购物车信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int DeleteCartInfo(DeleteCartInfoDto deleteArgs)
        {
            return CartDa.Instance.DeleteCartInfo(deleteArgs, UserInfo.GetLoginUser().UserID);
        }
        /// <summary>
        /// 批量添加购物车
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int AddCartInfo(List<CartModel> list, int UserID)
        {
            return CartDa.Instance.AddCartInfo(list, UserID);
        }
        /// <summary>
        /// 根据用户ID获取购物车总数量
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int GetCartCount(int UserID)
        {
            return CartDa.Instance.GetCartCount(UserID);
        }
        /// <summary>
        /// 获取购物媒体车列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public Dictionary<string, dynamic> GetCartMediaList(int UserID)
        {
            return new Dictionary<string, dynamic>() {
                { "CartWeiXin",GetCartWeiXin(UserID)},
                { "CartWeiBo",GetCartWeiBo(UserID)},
                { "CartApp",GetCartApp(UserID)}
            };
        }
        /// <summary>
        /// 根据用户ID获取购物车中 微信媒体信息列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private BasicResultDto GetCartWeiXin(int UserID)
        {
            var resultList = CartDa.Instance.GetCartWeiXin(UserID);
            ///微信列表信息排重
            var weixinList = (from q in resultList.AsEnumerable()
                              group q by new
                              {
                                  q.CategoryName,
                                  q.FansCount,
                                  q.FullName,
                                  q.QrCodeUrl,
                                  q.HeadImg,
                                  q.IsOriginal,
                                  q.NickName,
                                  q.ReadNum,
                                  q.Sign,
                                  q.Summary,
                                  q.WxNumber,
                                  q.TotalScores,
                                  q.RecID
                              } into G
                              select new WeiXinListModel
                              {
                                  CategoryName = G.Key.CategoryName,
                                  FansCount = G.Key.FansCount,
                                  FullName = G.Key.FullName,
                                  HeadImg = G.Key.HeadImg,
                                  IsOriginal = G.Key.IsOriginal,
                                  NickName = G.Key.NickName,
                                  QrCodeUrl= G.Key.QrCodeUrl,
                                  ReadNum = G.Key.ReadNum,
                                  Sign = G.Key.Sign,
                                  Summary = G.Key.Summary,
                                  WxNumber = G.Key.WxNumber,
                                  TotalScores = G.Key.TotalScores,
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
                        IssuePrice = resultList.Where(p => p.MediaID == item.RecID && p.ADPosition1==PositionID && p.ADPosition2 == 8001).Select(p => p.Price).FirstOrDefault(),
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
        private BasicResultDto GetCartWeiBo(int UserID)
        {
            List<WeiBoModel> list = CartDa.Instance.GetCartWeiBo(UserID);

            return new BasicResultDto { List = list, TotalCount = list.Count };
        }


        /// <summary>
        /// 根据用户ID获取购物车 APP媒体信息列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private BasicResultDto GetCartApp(int UserID)
        {
            List<AppModel> list = CartDa.Instance.GetCartApp(UserID);

            return new BasicResultDto { List = list, TotalCount = list.Count };
        }
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        public BasicResultDto TaskListInfo(QueryTaskargs queryArgs)
        {
            Tuple<int, List<TaskListModel>> item= CartDa.Instance.TaskListInfo(queryArgs);

            return new BasicResultDto { List = item.Item2, TotalCount = item.Item1 };
        }

        public MemoryStream GetCartExport()
        {
            Dictionary<string, BasicResultDto> ExportData = new Dictionary<string, BasicResultDto> {
                { "微信",GetCartWeiXin(UserInfo.GetLoginUser().UserID)},
                { "微博",GetCartWeiBo(UserInfo.GetLoginUser().UserID)},
                { "APP",GetCartApp(UserInfo.GetLoginUser().UserID)}
            };
            //Dictionary<string, BasicResultDto> ExportData = new Dictionary<string, BasicResultDto> {
            //    { "微信",GetCartWeiXin(18)},
            //    { "微博",GetCartWeiBo(18)},
            //    { "APP",GetCartApp(18)}
            //};
            return ExcelHelper.Export(ExportData);
        }
    }
}
