using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    /// <summary>
    /// 媒体相关业务逻辑类
    /// ls
    /// </summary>
    public class MediaInfo
    {
        public static readonly MediaInfo Instance = new MediaInfo();

        #region V1_1

        public GetMediaListBResDTO GetMediaListB(GetMediaListBReqDTO req)
        {
            string orderBy = " CreateTime desc ";
            if (req.PageSize > Util.PageSize)
            {
                req.PageSize = Util.PageSize;
            }
            var ur = Common.UserInfo.GetUserRole();
            string rightSql = " and (1=2)";
            if (req.AuditStatus == -2 && (ur.IsYY || ur.IsAdministrator))
            {//运营 微信公众号列表页 纯基表
                rightSql = " and (1=1)";
                return Dal.Media.MediaWeixin.Instance.GetBasicList(req.Key, req.CategoryID, req.LevelType, req.OAuthType, req.OAuthStatus,
                    req.IsAreaMedia, req.AreaProvniceId, req.AreaCityId, rightSql, orderBy, req.PageIndex, req.PageSize);
            }
            else
            {
                if (ur.IsYY || ur.IsAdministrator)
                {//运营看所有
                    rightSql = " and (1=1)";
                    if (req.AuditStatus == 43001)
                    {//提交时间
                        req.SubmitStartDate = req.StartDate;
                        req.SubmitEndDate = req.EndDate;
                    }
                    else
                    {//审核时间
                        if (req.AuditStatus == 43002)
                        {
                            #region 排序

                            switch (req.OrderBy)
                            {
                                case 1001:
                                    orderBy = " FansCount desc ";
                                    break;

                                case 1002:
                                    orderBy = " FansCount asc ";
                                    break;

                                case 2001:
                                    orderBy = " ReferReadCount desc ";
                                    break;

                                case 2002:
                                    orderBy = " ReferReadCount asc ";
                                    break;

                                case 3001:
                                    orderBy = " AveragePointCount desc ";
                                    break;

                                case 3002:
                                    orderBy = " AveragePointCount asc ";
                                    break;

                                case 4001:
                                    orderBy = " OrigArticleCount desc ";
                                    break;

                                case 4002:
                                    orderBy = " OrigArticleCount asc ";
                                    break;

                                case 5001:
                                    orderBy = " UpdateCount desc ";
                                    break;

                                case 5002:
                                    orderBy = " UpdateCount asc ";
                                    break;

                                case 6001:
                                    orderBy = " MaxinumReading desc ";
                                    break;

                                case 6002:
                                    orderBy = " MaxinumReading asc ";
                                    break;

                                case 7001:
                                    orderBy = " MoreReadCount desc ";
                                    break;

                                case 7002:
                                    orderBy = " MoreReadCount asc ";
                                    break;
                            }

                            #endregion 排序
                        }
                        req.AuditStartDate = req.StartDate;
                        req.AuditEndDate = req.EndDate;
                    }
                    req.StartDate = string.Empty;
                    req.EndDate = string.Empty;
                }
                else if (ur.IsAE)
                {//AE看AE角色池
                    rightSql = " and (Media_Weixin.CreateUserID in (Select Distinct(UserID) From UserRole where RoleID = 'SYS001RL00005' ))";
                }
                else if (ur.IsMedia)
                {//媒体主看自己的
                    rightSql = " and (Media_Weixin.CreateUserID = " + ur.UserID + ")";
                }
                return Dal.Media.MediaWeixin.Instance.GetMediaListB(req.AuditStatus, req.Key, req.CategoryID, req.LevelType, req.Source, req.PublishStatus, req.OAuthType, req.OAuthStatus, req.StartDate, req.EndDate, req.SubmitUserName, req.SubmitStartDate, req.SubmitEndDate, req.AuditStartDate, req.AuditEndDate,
                     req.IsAreaMedia, req.AreaProvniceId, req.AreaCityId, rightSql, orderBy, req.PageIndex, req.PageSize);
            }
        }

        public GetMediaListFResDTO GetMediaListF(GetMediaListFReqDTO req)
        {
            string fansCountMax = string.Empty;
            string fansCountMin = string.Empty;
            string priceMax = string.Empty;
            string priceMin = string.Empty;
            string orderBy = " MediaID asc ";
            if (!string.IsNullOrWhiteSpace(req.FansCount) && req.FansCount.Contains("-"))
            {
                fansCountMin = req.FansCount.Split('-')[0];
                fansCountMax = req.FansCount.Split('-')[1];
            }
            if (!string.IsNullOrWhiteSpace(req.Price) && req.Price.Contains("-"))
            {
                priceMin = req.Price.Split('-')[0];
                priceMax = req.Price.Split('-')[1];
            }
            switch (req.OrderBy)
            {
                case 1001:
                    orderBy = " FansCount asc ";
                    break;

                case 1002:
                    orderBy = " FansCount desc ";
                    break;

                case 1003:
                    orderBy = " AverageReading asc ";
                    break;

                case 1004:
                    orderBy = " AverageReading desc ";
                    break;

                case 1005:
                    orderBy = " Price asc ";
                    break;

                case 1006:
                    orderBy = " Price desc ";
                    break;

                case 1007://玛丽值升序
                    orderBy = " MaLiIndex asc ";
                    break;

                case 1008://玛丽值降序
                    orderBy = " MaLiIndex desc ";
                    break;
            }
            GetMediaListBResDTO res = new GetMediaListBResDTO();
            var ur = Common.UserInfo.GetUserRole();
            int userID = ur.UserID;
            if (req.PageSize > Util.PageSize)
            {
                req.PageSize = Util.PageSize;
            }
            string rightSql = " and 1=1 ";
            return Dal.Media.MediaWeixin.Instance.GetMediaListF(req.Key, req.CategoryID, fansCountMax, fansCountMin, priceMax, priceMin, req.IsVerify, req.CanReceive, userID, rightSql, orderBy, req.PageIndex, req.PageSize);
        }

        public GetMediaAuditStatusCountResDTO GetAuditCount()
        {
            var ur = Common.UserInfo.GetUserRole();
            bool flag = ur.IsYY || ur.IsAdministrator;
            return Dal.Media.MediaWeixin.Instance.GetAuditCount(ur.UserID, flag);
        }

        public bool CheckCanAdd(int wxID)
        {
            var ur = Common.UserInfo.GetUserRole();
            return Dal.Media.MediaWeixin.Instance.CheckCanAdd(wxID, ur.IsAE ? 0 : ur.UserID);
        }

        public List<MediaDictDTO> GetWeixin_OAuthDict(string key)
        {
            return Dal.Media.MediaWeixin.Instance.GetWeixin_OAuthDict(key);
        }

        #endregion V1_1

        #region 列表

        /// <summary>
        /// 获取微信列表
        /// ls
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="number">账号</param>
        /// <param name="source">来源ID</param>
        /// <param name="createUser">创建人名</param>
        /// <param name="createTime">创建日期</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public MediaListDTO GetWeixinList(string name, string number, int source, string createUser, string beginDate, string endDate,
            int categoryId, int levelType, int isAuth, int orderby,
            int pageIndex, int pageSize)
        {
            int totalCount = 0;
            var ur = Common.UserInfo.GetUserRole();
            int userID = ur.UserID;
            string msg = string.Empty;
            if (pageSize > Util.PageSize)
            {
                pageSize = Util.PageSize;
            }
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.MediaORPublish, "Media_WeiXin", "CreateUserID", userID, out msg);
            List<Entities.Media.MediaWeixin> list = Dal.Media.MediaWeixin.Instance.GetList(name, number, source, createUser, beginDate, endDate, rightSql,
                categoryId, levelType, isAuth, GetOrderBy(orderby),
                pageIndex, pageSize, out totalCount);
            MediaListDTO dto = new MediaListDTO() { TotalCount = totalCount, List = new List<MediaItemDTO>() };
            foreach (var model in list)
            {
                dto.List.Add(new MediaItemDTO()
                {
                    MediaID = model.MediaID,
                    Name = model.Name,
                    Number = model.Number,
                    HeadIconURL = model.HeadIconURL,
                    FansCount = model.FansCount,
                    LevelType = model.LevelTypeName,
                    CreateUser = string.IsNullOrWhiteSpace(model.TrueName) ? model.UserName : model.TrueName,
                    CreateDate = model.CreateTime.ToString("yyyy-MM-dd"),
                    Source = model.SourceName,
                    PubCount = model.PubCount,
                    PubID = model.PubID,
                    CanEdit = !(ur.IsMedia && model.Status.Equals((int)AuditStatusEnum.已通过)),
                    CanAddToRecommend = model.CanAddToRecommend,
                    Category = model.CategoryName,
                    IsRange = model.IsRange
                });
            }
            return dto;
        }

        private string GetOrderBy(int orderBy)
        {
            var orderByStr = " CreateTime DESC ";
            var orderDictionary = new Dictionary<int, string>()
            {
                {1001," FansCount DESC "},
                {1002," FansCount ASC "},
                {2001," ReferReadCount DESC "},//参考阅读数
                {2002," ReferReadCount ASC "},
                {3001," AveragePointCount DESC "},//平均点赞数
                {3002," AveragePointCount ASC "},
                {4001," OrigArticleCount DESC "},//原创文章数
                {4002," OrigArticleCount ASC "},
                {5001," UpdateCount DESC "},//更新次数
                {5002," UpdateCount ASC "},
                {6001," MaxinumReading DESC "},//最高阅读数
                {6002," MaxinumReading ASC "},
                {7001," MoreReadCount DESC "},//10W+阅读量文章数
                {7002," MoreReadCount ASC "},
            };

            var value = orderDictionary.FirstOrDefault(s => s.Key == orderBy);
            return value.Value ?? orderByStr;
        }

        private string GetWeiBoOrderBy(int orderBy)
        {
            var orderByStr = " CreateTime DESC ";
            var orderDictionary = new Dictionary<int, string>()
            {
                {1001," FansCount DESC"},
                {1002," FansCount ASC"},
                {2001," AverageForwardCount DESC"},//平均转发数
                {2002," AverageForwardCount ASC"},
                {3001," AverageCommentCount DESC"},//平均评论数
                {3002," AverageCommentCount ASC"},
                {4001," AveragePointCount DESC"},//平均点赞数
                {4002," AveragePointCount ASC"},
            };

            var value = orderDictionary.FirstOrDefault(s => s.Key == orderBy);
            return value.Value ?? orderByStr;
        }

        private string GetVideoOrderBy(int orderBy)
        {
            var orderByStr = " CreateTime DESC ";
            var orderDictionary = new Dictionary<int, string>()
            {
                {1001," FansCount DESC"},
                {1002," FansCount ASC"},
                {2001," AveragePlayCount DESC"},//平均播放数
                {2002," AveragePlayCount ASC"},
                {3001," AveragePointCount DESC"},//平均点赞数
                {3002," AveragePointCount ASC"},
                {4001," AverageCommentCount DESC"},//平均评论数
                {4002," AverageCommentCount ASC"},
                {5001," AverageBarrageCount DESC"},//平均弹幕数
                {5002," AverageBarrageCount ASC"},
            };

            var value = orderDictionary.FirstOrDefault(s => s.Key == orderBy);
            return value.Value ?? orderByStr;
        }

        private string GetBroadcastOrderBy(int orderBy)
        {
            var orderByStr = " CreateTime DESC ";
            var orderDictionary = new Dictionary<int, string>()
            {
                {1001," FansCount DESC"},
                {1002," FansCount ASC"},
                {2001," CumulateReward DESC"},//累计打赏数
                {2002," CumulateReward ASC"},
                {3001," CumulatePoints DESC"},//累计点赞数
                {3002," CumulatePoints ASC"},
                {4001," AverageAudience DESC"},//平均观众数
                {4002," AverageAudience ASC"}
            };

            var value = orderDictionary.FirstOrDefault(s => s.Key == orderBy);
            return value.Value ?? orderByStr;
        }

        /// <summary>
        /// 获取微博列表
        /// ls
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="number">账号</param>
        /// <param name="source">来源ID</param>
        /// <param name="createUser">创建人名</param>
        /// <param name="createTime">创建日期</param>
        /// <param name="orderby"></param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="categoryId"></param>
        /// <param name="levelType"></param>
        /// <param name="isAuth"></param>
        /// <returns></returns>
        public MediaListDTO GetWeiboList(string name, string number, int source, string createUser, string beginDate, string endDate,
    int categoryId, int levelType, int isAuth, int orderby,
            int pageIndex, int pageSize)
        {
            int totalCount = 0;
            var ur = Common.UserInfo.GetUserRole();
            int userID = ur.UserID;
            string msg = string.Empty;
            if (pageSize > Util.PageSize)
            {
                pageSize = Util.PageSize;
            }
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.MediaORPublish, "Media_Weibo", "CreateUserID", userID, out msg);
            List<Entities.Media.MediaWeibo> list = Dal.Media.MediaWeibo.Instance.GetList(name, number, source, createUser, beginDate, endDate, rightSql,
                categoryId, levelType, isAuth, GetWeiBoOrderBy(orderby),
                pageIndex, pageSize, out totalCount);
            MediaListDTO dto = new MediaListDTO() { TotalCount = totalCount, List = new List<MediaItemDTO>() };
            foreach (var model in list)
            {
                dto.List.Add(new MediaItemDTO()
                {
                    MediaID = model.MediaID,
                    Name = model.Name,
                    Number = model.Number,
                    HeadIconURL = model.HeadIconURL,
                    FansCount = model.FansCount,
                    LevelType = model.LevelTypeName,
                    CreateUser = string.IsNullOrWhiteSpace(model.TrueName) ? model.UserName : model.TrueName,
                    CreateDate = model.CreateTime.ToString("yyyy-MM-dd"),
                    Source = model.SourceName,
                    PubCount = model.PubCount,
                    PubID = model.PubID,
                    Category = model.CategoryName,
                    CanEdit = !(ur.IsMedia && model.Status.Equals((int)AuditStatusEnum.已通过)),
                    CanAddToRecommend = model.CanAddToRecommend,
                    IsRange = model.IsRange
                });
            }
            return dto;
        }

        /// <summary>
        /// 获取视频列表
        /// ls
        /// </summary>
        /// <param name="platform">平台ID</param>
        /// <param name="name">名称</param>
        /// <param name="number">账号</param>
        /// <param name="source">来源ID</param>
        /// <param name="createUser">创建人名</param>
        /// <param name="createTime">创建日期</param>
        /// <param name="categoryId"></param>
        /// <param name="orderby"></param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public MediaListDTO GetVideoList(int platform, string name, string number, int source, string createUser, string beginDate, string endDate,
int categoryId, int orderby, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            var ur = Common.UserInfo.GetUserRole();
            int userID = ur.UserID;
            string msg = string.Empty;
            if (pageSize > Util.PageSize)
            {
                pageSize = Util.PageSize;
            }
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.MediaORPublish, "Media_Video", "CreateUserID", userID, out msg);
            List<Entities.Media.MediaVideo> list = Dal.Media.MediaVideo.Instance.GetList(platform, name, number, source, createUser, beginDate, endDate, rightSql,
               categoryId, GetVideoOrderBy(orderby), pageIndex, pageSize, out totalCount);
            MediaListDTO dto = new MediaListDTO() { TotalCount = totalCount, List = new List<MediaItemDTO>() };
            foreach (var model in list)
            {
                dto.List.Add(new MediaItemDTO()
                {
                    MediaID = model.MediaID,
                    Name = model.Name,
                    Number = model.Number,
                    HeadIconURL = model.HeadIconURL,
                    FansCount = model.FansCount,
                    LevelType = model.LevelTypeName,
                    CreateUser = string.IsNullOrWhiteSpace(model.TrueName) ? model.UserName : model.TrueName,
                    CreateDate = model.CreateTime.ToString("yyyy-MM-dd"),
                    Source = model.SourceName,
                    Platform = model.PlatformName,
                    PubCount = model.PubCount,
                    PubID = model.PubID,
                    CanEdit = !(ur.IsMedia && model.Status.Equals((int)AuditStatusEnum.已通过)),
                    CanAddToRecommend = model.CanAddToRecommend,
                    Category = model.CategoryName,
                    IsRange = model.IsRange
                });
            }
            return dto;
        }

        /// <summary>
        /// 获取直播列表
        /// ls
        /// </summary>
        /// <param name="platform">平台ID</param>
        /// <param name="name">名称</param>
        /// <param name="number">账号</param>
        /// <param name="source">来源ID</param>
        /// <param name="createUser">创建人</param>
        /// <param name="createTime">创建时间</param>
        /// <param name="categoryId"></param>
        /// <param name="orderby"></param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public MediaListDTO GetBroadcastList(int platform, string name, string number, int source, string createUser, string beginDate, string endDate,
 int categoryId, int orderby, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            var ur = Common.UserInfo.GetUserRole();
            int userID = ur.UserID;
            string msg = string.Empty;
            if (pageSize > Util.PageSize)
            {
                pageSize = Util.PageSize;
            }
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.MediaORPublish, "Media_Broadcast", "CreateUserID", userID, out msg);
            List<Entities.Media.MediaBroadcast> list = Dal.Media.MediaBroadcast.Instance.GetList(platform, name, number, source, createUser, beginDate, endDate, rightSql,
                categoryId, GetBroadcastOrderBy(orderby), pageIndex, pageSize, out totalCount);
            MediaListDTO dto = new MediaListDTO() { TotalCount = totalCount, List = new List<MediaItemDTO>() };
            foreach (var model in list)
            {
                dto.List.Add(new MediaItemDTO()
                {
                    MediaID = model.MediaID,
                    Name = model.Name,
                    Number = model.Number,
                    HeadIconURL = model.HeadIconURL,
                    FansCount = model.FansCount,
                    LevelType = model.LevelTypeName,
                    CreateUser = string.IsNullOrWhiteSpace(model.TrueName) ? model.UserName : model.TrueName,
                    CreateDate = model.CreateTime.ToString("yyyy-MM-dd"),
                    Source = model.SourceName,
                    Platform = model.PlatformName,
                    PubCount = model.PubCount,
                    PubID = model.PubID,
                    Category = model.CategoryName,
                    CanEdit = !(ur.IsMedia && model.Status.Equals((int)AuditStatusEnum.已通过)),
                    CanAddToRecommend = model.CanAddToRecommend,
                    IsRange = model.IsRange
                });
            }
            return dto;
        }

        /// <summary>
        /// 获取PCAPP列表
        /// ls
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="source">来源ID</param>
        /// <param name="createUser">创建人名</param>
        /// <param name="categoryId"></param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public MediaListDTO GetPCAPPList(string name, int source, string createUser, string beginDate, string endDate,
int categoryId, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            var ur = Common.UserInfo.GetUserRole();
            int userID = ur.UserID;
            string msg = string.Empty;
            if (pageSize > Util.PageSize)
            {
                pageSize = Util.PageSize;
            }
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.MediaORPublish, "Media_PCAPP", "CreateUserID", userID, out msg);
            List<Entities.Media.MediaPcApp> list = Dal.Media.MediaPCAPP.Instance.GetList(name, source, createUser, beginDate, endDate, rightSql, categoryId, pageIndex, pageSize, out totalCount);
            MediaListDTO dto = new MediaListDTO() { TotalCount = totalCount, List = new List<MediaItemDTO>() };
            foreach (var model in list)
            {
                dto.List.Add(new MediaItemDTO()
                {
                    MediaID = model.MediaID,
                    Name = model.Name,
                    CreateUser = string.IsNullOrWhiteSpace(model.TrueName) ? model.UserName : model.TrueName,
                    CreateDate = model.CreateTime.ToString("yyyy-MM-dd"),
                    Source = model.SourceName,
                    ADCount = model.ADCount,
                    Category = model.CategoryName,
                    OverlayName = model.OverlayName,
                    DailyLive = model.DailyLive,
                    City = model.AreaName,
                    PubCount = model.PubCount,
                    PubID = model.PubID,
                    CanEdit = !(ur.IsMedia && model.Status.Equals((int)AuditStatusEnum.已通过)),
                });
            }
            return dto;
        }

        #endregion 列表

        #region 详情

        /// <summary>
        /// 获取微信详情
        /// ls
        /// </summary>
        /// <param name="mediaID">媒体ID</param>
        /// <returns></returns>
        public MediaWeixinDTO GetWeixinDetail(int mediaID)
        {
            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.MediaORPublish, "Media_Weixin", "CreateUserID", userID, out msg);
            return Dal.Media.MediaWeixin.Instance.GetDetail(mediaID, rightSql);
        }

        /// <summary>
        /// 获取微博详情
        /// ls
        /// </summary>
        /// <param name="mediaID">媒体ID</param>
        /// <returns></returns>
        public MediaWeiboDTO GetWeiboDetail(int mediaID)
        {
            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.MediaORPublish, "Media_Weibo", "CreateUserID", userID, out msg);
            return Dal.Media.MediaWeibo.Instance.GetDetail(mediaID, rightSql);
        }

        /// <summary>
        /// 获取视频详情
        /// ls
        /// </summary>
        /// <param name="mediaID">媒体ID</param>
        /// <returns></returns>
        public MediaVideoDTO GetVideoDetail(int mediaID)
        {
            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.MediaORPublish, "Media_Video", "CreateUserID", userID, out msg);
            return Dal.Media.MediaVideo.Instance.GetDetail(mediaID, rightSql);
        }

        /// <summary>
        /// 获取直播详情
        /// ls
        /// </summary>
        /// <param name="mediaID">媒体ID</param>
        /// <returns></returns>
        public MediaBroadcastDTO GetBroadcastDetail(int mediaID)
        {
            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.MediaORPublish, "Media_Broadcast", "CreateUserID", userID, out msg);
            return Dal.Media.MediaBroadcast.Instance.GetDetail(mediaID, rightSql);
        }

        /// <summary>
        /// 获取PCAPP详情
        /// ls
        /// </summary>
        /// <param name="mediaID">媒体ID</param>
        /// <returns></returns>
        public MediaPCAPPDTO GetPCAPPDetail(int mediaID)
        {
            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.MediaORPublish, "Media_PCAPP", "CreateUserID", userID, out msg);
            return Dal.Media.MediaPCAPP.Instance.GetDetail(mediaID, rightSql);
        }

        #endregion 详情

        #region 存在验证、字典

        /// <summary>
        /// 验证媒体是否存在
        /// ls
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="name">名称</param>
        /// <param name="number">账号</param>
        /// <param name="needCount">（APP用）是否需要返回刊例数量及广告位数量</param>
        /// <returns></returns>
        public MediaExistsDTO MediaIsExists(MediaTypeEnum mediaType, int mediaID, string name, string number, bool needCount = false)
        {
            string rightSql = string.Empty;
            string msg = string.Empty;
            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            if (needCount)//添加刊例时候的验证
            {
                rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.添加刊例, "", "CreateUserID", userID, out msg);
            }
            else
            {
                if (mediaID > 0)
                {//编辑的时候需要追溯到创建者
                    userID = Dal.PublishInfo.Instance.GetMediaCreateUserIDByMediaID(mediaType, mediaID);
                }
                rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.媒体存在验证, "", "CreateUserID", userID, out msg);
            }
            return Dal.PublishInfo.Instance.MediaIsExists(mediaType, mediaID, name, number, needCount, rightSql);
        }

        /// <summary>
        /// 获取媒体字典
        /// ls
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="name">名称</param>
        /// <param name="number">账号</param>
        /// <returns></returns>
        public List<MediaDictDTO> GetMediaDict(MediaTypeEnum mediaType, string name, string number)
        {
            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.媒体存在验证, "t", "CreateUserID", userID, out msg);
            return Dal.PublishInfo.Instance.GetMediaDict(mediaType, name, number, rightSql);
        }

        #endregion 存在验证、字典

        #region V1.1.4

        public GetAppQualifucationResDTO GetAppQualificationInfo(int mediaID, int mediaType)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (mediaID.Equals(0) && !ur.IsMedia)
                return null;
            return Dal.Media.MediaQualification.Instance.GetQualificationInfo(mediaID, mediaType, ur.UserID);
        }

        public GetADListBResDTO GetAppADListB(GetADListBReqDTO req)
        {
            string rightSql = string.Empty;
            var ur = Common.UserInfo.GetUserRole();
            if (ur.IsAdministrator)
                rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.系统, ur);
            else if (ur.IsAE)
                rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.角色, ur);
            else if (ur.IsMedia)
                rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.用户, ur);
            else
                return null;
            return Dal.Media.MediaPCAPP.Instance.GetAppADListB(req.MediaID, req.MediaName, req.ADName, req.BeginTime, req.EndTime, req.PubStatus, rightSql, null, req.PageIndex, req.PageSize);
        }

        public GetADListFResDTO GetAppADListF(GetADListFReqDTO req)
        {
            string rightSql = string.Empty;
            var ur = Common.UserInfo.GetUserRole();
            string minPrice = string.Empty;
            string maxPrice = string.Empty;
            string orderBy = string.Empty;
            if (!string.IsNullOrWhiteSpace(req.Price) && req.Price.Contains("-"))
            {
                minPrice = req.Price.Split('-')[0];
                maxPrice = req.Price.Split('-')[1];
            }
            switch (req.Orderby)
            {
                case -2:
                    orderBy = " Price asc ";
                    break;

                case 2:
                    orderBy = " Price desc ";
                    break;
            }
            return Dal.Media.MediaPCAPP.Instance.GetAppADListF(req.MediaID, req.Key, req.CategoryID,
                req.SaleType, req.CityID, minPrice, maxPrice, ur.UserID, rightSql,
                orderBy, req.PageIndex, req.PageSize);
        }

        public List<GetRecommendADResDTO> GetSimilarAD(int mediaID, int templateID)
        {
            var ur = Common.UserInfo.GetUserRole();
            var list = Dal.Media.MediaPCAPP.Instance.GetSimilarAD(mediaID, templateID, ur.UserID);
            if (list != null && list.Count > 0)
            {
                list.ForEach(i =>
                {
                    if (!string.IsNullOrWhiteSpace(i.ADLogo))
                    {
                        i.ADLogo = i.ADLogo.Split('|')[0];
                    }
                });
            }
            return list;
        }

        public GetADDetailResDTO GetAppADItem(int mediaType, int pubID, int mediaID, int templateID)
        {//里面根据CreateUserID做了判断
            var ur = Common.UserInfo.GetUserRole();
            if (pubID.Equals(0) && !ur.IsAE && !ur.IsMedia)
                return null;
            return Dal.Media.MediaPCAPP.Instance.GetAppADItem(mediaType, pubID, mediaID, templateID, ur.UserID);
        }

        public GetAuditADPriceListResDTO GetAuditAppADList(int pubID)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsYY && !ur.IsADAudit && !ur.IsAdministrator)
            {
                return null;
            }
            GetAuditADPriceListResDTO res = new GetAuditADPriceListResDTO();
            var list = Dal.Media.MediaPCAPP.Instance.GetAuditAppADList(pubID);
            if (list != null && list.Count > 0)
            {
                #region ADInfo

                res.ADInfo = new OtherItem()
                {
                    MediaName = list[0].MediaName,
                    MediaLogo = list[0].MediaLogo,
                    BeginTime = list[0].BeginTime,
                    EndTime = list[0].EndTime,
                    PubFileUrl = list[0].PubFileUrl,
                    SubmitUserName = list[0].SubmitUserName
                };
                if (!string.IsNullOrEmpty(res.ADInfo.PubFileUrl))//
                    res.ADInfo.PubFileName = res.ADInfo.PubFileUrl.Split('/').Last();

                #endregion ADInfo

                #region PriceListItem

                res.List = new List<PriceListItem>();
                var query = list.GroupBy(i => i.PubID);
                foreach (var group in query)
                {
                    var pb = group.First(i => i.PubID.Equals(group.Key));
                    PriceListItem item = new PriceListItem();
                    item.PriceList = new List<PriceItem>();
                    item.PublishBasicInfo = new PublishBasicItem()
                    {
                        PubID = pb.PubID,
                        MediaLogo = pb.MediaLogo,
                        MediaName = pb.MediaName,
                        ADName = pb.ADName,
                        BeginTime = pb.BeginTime,
                        EndTime = pb.EndTime,
                        PubFileUrl = pb.PubFileUrl,
                        PurchaseDiscount = pb.PurchaseDiscount,
                        SaleDiscount = pb.SaleDiscount,
                        HasHoliday = pb.HasHoliday,
                        SubmitUserName = pb.SubmitUserName
                    };

                    #region Price 默认rowstate =1

                    foreach (var one in group)
                    {
                        if (one.RecID.Equals(0))
                            continue;
                        item.PriceList.Add(new PriceItem()
                        {
                            RecID = one.RecID,
                            ADStyle = one.ADStyle,
                            ADStyleName = one.ADStyleName,
                            SaleArea = one.SaleArea,
                            SaleAreaName = one.SaleAreaName,
                            SalePlatform = one.SalePlatform,
                            SalePlatformName = one.SalePlatformName,
                            SaleType = one.SaleType,
                            SaleTypeName = one.SaleTypeName,
                            SalePrice = one.SalePrice,
                            SalePrice_Holiday = one.SalePrice_Holiday,
                            PubPrice = one.PubPrice,
                            PubPrice_Holiday = one.PubPrice_Holiday,
                            ClickCount = one.ClickCount,
                            CarouselNumber = one.CarouselNumber,
                            ExposureCount = one.ExposureCount
                        });
                    }

                    #endregion Price 默认rowstate =1

                    #region rowstate

                    string rowstate = string.Empty;
                    string json = group.First().RowState;
                    if (string.IsNullOrEmpty(json))
                    {
                        rowstate = "1";
                    }
                    else
                    {
                        OldPublishDTO oldPub = JsonConvert.DeserializeObject<OldPublishDTO>(json);
                        if (oldPub.Prices == null || oldPub.Prices.Count.Equals(0))
                        {
                            item.PriceList.ForEach(n => n.RowState = "1");
                        }
                        else
                        {
                            item.PriceList.ForEach(n =>
                            {
                                if (!oldPub.Prices.Exists(o => o.RecID.Equals(n.RecID)))
                                    n.RowState = "1";
                                else
                                {
                                    #region 修改

                                    var oldPrice = oldPub.Prices.Find(o => o.RecID.Equals(n.RecID));
                                    StringBuilder sb = new StringBuilder();
                                    if (oldPrice.CarouselNumber != n.CarouselNumber)
                                        sb.Append("CarouselNumber,");
                                    if (oldPrice.ADStyleName != n.ADStyleName)
                                        sb.Append("ADStyleName,");
                                    if (oldPrice.SalePlatformName != n.SalePlatformName)
                                        sb.Append("SalePlatformName,");
                                    if (oldPrice.SaleTypeName != n.SaleTypeName)
                                        sb.Append("SaleTypeName,");
                                    if (oldPrice.SaleAreaName != n.SaleAreaName)
                                        sb.Append("SaleAreaName,");
                                    if (oldPrice.ClickCount != n.ClickCount)
                                        sb.Append("ClickCount,");
                                    if (oldPrice.ExposureCount != n.ExposureCount)
                                        sb.Append("ExposureCount,");
                                    if (oldPrice.PubPrice != n.PubPrice)
                                        sb.Append("PubPrice,");
                                    if (oldPrice.PubPrice_Holiday != n.PubPrice_Holiday)
                                        sb.Append("PubPrice_Holiday,");
                                    if (oldPrice.SalePrice != n.SalePrice)
                                        sb.Append("SalePrice,");
                                    if (oldPrice.SalePrice_Holiday != n.SalePrice_Holiday)
                                        sb.Append("SalePrice_Holiday,");
                                    if (sb.Length > 0)
                                        sb = sb.Remove(sb.Length - 1, 1);
                                    n.RowState = sb.ToString();

                                    #endregion 修改
                                }
                            });

                            #region 删除的

                            List<PriceItem> dellist = new List<PriceItem>();
                            oldPub.Prices.ForEach(o =>
                            {
                                if (!item.PriceList.Exists(n => n.RecID.Equals(o.RecID)))
                                {
                                    dellist.Add(new PriceItem()
                                    {
                                        RecID = o.RecID,
                                        ADStyle = o.ADStyle,
                                        ADStyleName = o.ADStyleName,
                                        SaleArea = o.SaleArea,
                                        SaleAreaName = o.SaleAreaName,
                                        SalePlatform = o.SalePlatform,
                                        SalePlatformName = o.SalePlatformName,
                                        SaleType = o.SaleType,
                                        SaleTypeName = o.SaleTypeName,
                                        CarouselNumber = o.CarouselNumber,
                                        ClickCount = o.ClickCount,
                                        ExposureCount = o.ExposureCount,
                                        PubPrice = o.PubPrice,
                                        PubPrice_Holiday = o.PubPrice_Holiday,
                                        SalePrice = o.SalePrice,
                                        SalePrice_Holiday = o.SalePrice_Holiday,
                                        RowState = "-1"
                                    });
                                }
                            });
                            item.PriceList.AddRange(dellist);

                            #endregion 删除的
                        }
                    }

                    #endregion rowstate

                    res.List.Add(item);
                }

                #endregion PriceListItem
            }
            return res;
        }

        #endregion V1.1.4
    }
}