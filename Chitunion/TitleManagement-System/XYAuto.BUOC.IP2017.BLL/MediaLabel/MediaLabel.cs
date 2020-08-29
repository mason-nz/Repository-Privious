using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.BLL.BatchMedia.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.Business.Query.V1_2_4;
using XYAuto.BUOC.IP2017.Entities.BatchMedia;

namespace XYAuto.BUOC.IP2017.BLL.MediaLabel
{
    public class MediaLabel
    {
        #region 当前登录人UserID
        private int _currentUserID;
        public int currentUserID
        {
            get
            {
                try
                {
                    _currentUserID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    _currentUserID = 1298;
                }
                return _currentUserID;
            }
        }
        private static string LoginPwdKey = Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        #endregion        
        public static readonly MediaLabel Instance = new MediaLabel();
        #region 获取文章摘要关键词
        public dynamic GetSummaryKeyWord(int mediaType, int articleID, int summarySize, int keyWordSize)
        {
            string SummaryKeyWordUrl = ConfigurationManager.AppSettings["InterfaceGetSummaryKeyWord"];
            switch (mediaType)
            {
                case (int)Entities.ENUM.ENUM.EnumMediaType.微信:
                    mediaType = 1;
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.头条:
                    mediaType = 2;
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.搜狐:
                    mediaType = 3;
                    break;
                default:
                    break;
            }
            string url = string.Format(SummaryKeyWordUrl, articleID, summarySize, keyWordSize, mediaType);
            var obj = Util.HttpWebRequestCreate<DTO.ResSummaryKeyWordDTO>(url);
            if (obj.code == 10000)
            {
                return obj.data;
            }
            else
                return "没有数据";
        }
        #endregion
        #region 标签录入列表
        public Business.DTO.ResponseDto.BaseResponseEntity<ResInputListMediaDto> InputListMedia(ReqInputListMediaDto request, ref string errmsg)
        {
            //request.CreateUserID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
            errmsg = string.Empty;
            if (request == null)
            {
                errmsg = "请求参数不能为null";
                return null;
            }
            if (!request.CheckSelfModel(out errmsg))
                return null;
            var result = new InputListMediaQuery().GetQueryList(request);
            if(result.List==null)
            {
                errmsg = "没有数据";
                return null;
            }
            result.List.ForEach(x =>
            {
                var operateinfoList = BLL.BatchMedia.BatchMedia.Instance.GetListByMedia(x.MediaType, x.MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.微信 ? x.Number : x.Name);
                x.OperateInfo = new List<ResOperateinfo>();
                x.OperateInfo = operateinfoList;
            });
            return result;
        }
        #endregion
        #region 打标签媒体或车型渲染
        public DTO.ResponseDto.V1_2_4.ResBatchMediaDto RenderBatchMedia(DTO.RequestDto.V1_2_4.ReqBatchMediaDto req, ref string errmsg)
        {
            errmsg = string.Empty;
            if (!req.CheckSelfModel(out errmsg))
                return null;

            //查询媒体是否存在
            //检查是否有待打批次
            var bmModel = ValidBatchMedia(req.MediaType, req.NumberOrName, req.NumberOrName, ref errmsg);
            if (!string.IsNullOrEmpty(errmsg))
                return null;

            var tp = Dal.MediaLabel.MediaLabel.Instance.RenderBatchMedia(req.MediaType, req.NumberOrName, bmModel.BatchMediaID);

            List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto> Category = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>();
            List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto> MarketScene = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>();
            List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto> DistributeScene = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>();
            List<DTO.ResponseDto.V1_2_4.ResBatchMediaIplabelDto> IPLabel = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaIplabelDto>();
            var resDto = new DTO.ResponseDto.V1_2_4.ResBatchMediaDto()
            {
                Category = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(),
                MarketScene = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(),
                DistributeScene = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(),
                IPLabel = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaIplabelDto>()
            };

            if (tp.Item1 != null)
                resDto = Util.DataTableToEntity<DTO.ResponseDto.V1_2_4.ResBatchMediaDto>(tp.Item1);

            if (tp.Item2 != null)
                Category = Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(tp.Item2);

            if (tp.Item3 != null)
                MarketScene = Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(tp.Item3);

            if (tp.Item4 != null)
                DistributeScene = Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(tp.Item4);

            if (tp.Item5 != null)
                IPLabel = Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResBatchMediaIplabelDto>(tp.Item5);

            //DTO.ResponseDto.V1_2_4.ResBatchMediaDto resDto = new DTO.ResponseDto.V1_2_4.ResBatchMediaDto()
            //{
            //    Category = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(),
            //    MarketScene = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(),
            //    DistributeScene = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(),
            //    IPLabel = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaIplabelDto>()
            //};
            //if (tp.Item1 != null)
            //    resDto = Util.DataTableToEntity<DTO.ResponseDto.V1_2_4.ResBatchMediaDto>(tp.Item1);

            if (tp.Item6 != null)
                resDto.ArticleIDs = tp.Item6.Rows[0][0].ToString();

            resDto.Category = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>() { };//Category;
            resDto.MarketScene = MarketScene;
            resDto.DistributeScene = DistributeScene;
            resDto.IPLabel = IPLabel;

            resDto.IPLabel.ForEach(x =>
            {
                x.SubIP = new List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>();
                x.SubIP = Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(Dal.IPTitleInfo.IPTitleInfo.Instance.GetSubIPByPID(x.DictId));
            });

            return resDto;
        }
        #endregion
        #region 打标签媒体或打标签文章提交
        public int BatchMediaSubmit(ReqBatchMediaSubmitDto req, ref string errmsg)
        {
            errmsg = string.Empty;
            if (!req.CheckSelfModel(out errmsg))
                return -2;

            //查询媒体是否存在
            MediaModel mediaModel = BLL.BatchMedia.BatchMedia.Instance.GetMediaModel(req.MediaType, req.MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.微信 ? req.Number : req.Name);
            if (mediaModel == null)
            {
                errmsg = "媒体不存在";
                return -2;
            }

            //获取待打批次
            Entities.BatchMedia.QueryBatchMedia query = new Entities.BatchMedia.QueryBatchMedia()
            {
                MediaType = req.MediaType,
                MediaNumber = req.Number,
                MediaName = req.Name,
                HomeUrl = mediaModel.HomeUrl,
                CurrentUserID = currentUserID,
                Status = (int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打
            };
            Entities.BatchMedia.BatchMedia bmModel = BLL.BatchMedia.BatchMedia.Instance.GetModelByMedia(query);
            if (bmModel == null)//没有待打批次
            {
                errmsg = "待打批次不存在";
                return -2;
            }

            //记录标签
            BLL.BatchLabelHistory.BatchLabelHistory.Instance.InsertBatch(req, bmModel.BatchMediaID, currentUserID);

            //检查待审记录
            var batchAuditMode = BLL.BatchMediaAudit.BatchMediaAudit.Instance.GetModelPending(new Entities.BatchMediaAudit.QueryBatchMediaAudit()
            {
                TaskType = (int)Entities.ENUM.ENUM.EnumTaskType.媒体,
                MediaType = req.MediaType,
                MediaName = req.Name,
                MediaNumber = req.Number,
                Status = (int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待审
            });

            if (batchAuditMode == null)
            {
                //插入待审记录
                int batchAuditID = BLL.BatchMediaAudit.BatchMediaAudit.Instance.Insert(new Entities.BatchMediaAudit.BatchMediaAudit()
                {
                    TaskType = (int)Entities.ENUM.ENUM.EnumTaskType.媒体,
                    MediaType = req.MediaType,
                    MediaName = req.Name,
                    MediaNumber = req.Number,
                    HomeUrl = mediaModel.HomeUrl,
                    HeadImg = mediaModel.HeadImg,
                    IsSelfDo = mediaModel.IsSelfDo,
                    Status = (int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待审,
                    CreateUserID = currentUserID
                });
                batchAuditMode = new Entities.BatchMediaAudit.BatchMediaAudit()
                {
                    BatchAuditID = batchAuditID
                };
            }

            //更新打标签批次为待审
            BLL.BatchMedia.BatchMedia.Instance.UpdatePendingSubmitTime(bmModel.BatchMediaID, batchAuditMode.BatchAuditID);
            return bmModel.BatchMediaID;
        }
        #endregion
        #region 根据标签母IP获取子IP
        public List<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto> GetSubIPByPID(int dictID)
        {
            return Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(Dal.IPTitleInfo.IPTitleInfo.Instance.GetSubIPByPID(dictID));
        }
        #endregion
        #region 查看文章或媒体已审批次详情
        public DTO.ResponseDto.V1_2_4.ResViewBatchMediaDto ViewBatchMedia(int BatchMediaID, ref string errmsg)
        {
            errmsg = string.Empty;
            var batchMediaModel = BLL.BatchMedia.BatchMedia.Instance.GetModelByRecID(BatchMediaID);
            if (batchMediaModel == null)
            {
                errmsg = $"参数错误，批次:{BatchMediaID}不存在";
                return null;

            }

            DTO.ResponseDto.V1_2_4.ResViewBatchMediaDto result = new DTO.ResponseDto.V1_2_4.ResViewBatchMediaDto();
            var tp = Dal.MediaLabel.MediaLabel.Instance.ViewBatchMedia(BatchMediaID, batchMediaModel.MediaType, batchMediaModel.BatchAuditID);

            //媒体信息、提交人、审核人
            if (tp.Item1 != null)
            {
                var resMediaInfo = Util.DataTableToEntity<DTO.ResponseDto.V1_2_4.ResViewBatchMediaInfoDto>(tp.Item1);
                if (resMediaInfo != null)
                {
                    result.MediaType = resMediaInfo.MediaType;
                    result.Name = resMediaInfo.Name;
                    result.Number = resMediaInfo.Number;
                    result.HeadImg = resMediaInfo.HeadImg;
                    result.HomeUrl = resMediaInfo.HomeUrl;

                    result.OperateInfo = new DTO.ResponseDto.V1_2_4.Operateinfo()
                    {
                        UserName = resMediaInfo.OperateInfoUserName,
                        CreateTime = resMediaInfo.CreateTime
                    };

                    result.AuditInfo = new DTO.ResponseDto.V1_2_4.Operateinfo()
                    {
                        UserName = resMediaInfo.AuditInfoUserName,
                        CreateTime = resMediaInfo.AuditTime
                    };
                }
            }

            result.Category = new DTO.ResponseDto.V1_2_4.Category();
            result.MarketScene = new DTO.ResponseDto.V1_2_4.Category();
            result.DistributeScene = new DTO.ResponseDto.V1_2_4.Category();
            result.IPLabel = new DTO.ResponseDto.V1_2_4.IPCategory();
            //打标签项
            if (tp.Item2 != null)
            {
                var batchOriginal = Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item2);
                //分类                
                result.Category.Original = new List<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>();
                result.Category.Original = batchOriginal.Where(t => t.Type == (int)Entities.ENUM.ENUM.EnumLabelType.分类).ToList();

                //市场场景                
                result.MarketScene.Original = new List<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>();
                result.MarketScene.Original = batchOriginal.Where(t => t.Type == (int)Entities.ENUM.ENUM.EnumLabelType.市场场景).ToList();

                //分发场景                
                result.DistributeScene.Original = new List<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>();
                result.DistributeScene.Original = batchOriginal.Where(t => t.Type == (int)Entities.ENUM.ENUM.EnumLabelType.分发场景).ToList();

                //IP标签                
                result.IPLabel.Original = new List<DTO.ResponseDto.V1_2_4.IPLabel>();
                foreach (var item in batchOriginal.Where(t => t.Type == (int)Entities.ENUM.ENUM.EnumLabelType.IP).ToList())
                {
                    result.IPLabel.Original.Add(new DTO.ResponseDto.V1_2_4.IPLabel()
                    {
                        DictId = item.DictId,
                        DictName = item.DictName,
                        LabelID = item.LabelID
                    });
                }

                //子IP项
                if (tp.Item4 != null)
                {
                    var ipSUB = Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item4);
                    var ipSON = new List<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>();
                    if (tp.Item5 != null)
                    {
                        ipSON = Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item5);
                    }

                    //遍历IP标签
                    result.IPLabel.Original.ForEach(x =>
                    {
                        x.SubIP = new List<DTO.ResponseDto.V1_2_4.SubIPLabelDto>();
                        foreach (var item in ipSUB.Where(t => t.LabelID == x.LabelID).ToList())
                        {
                            x.SubIP.Add(new DTO.ResponseDto.V1_2_4.SubIPLabelDto()
                            {
                                DictId = item.DictId,
                                DictName = item.DictName,
                                SubIPLabelID = item.SubIPID
                            });
                        }


                        //遍历子IP
                        x.SubIP.ForEach(x1 =>
                        {
                            x1.Label = new List<DTO.ResponseDto.V1_2_4.LabelDto>();
                            foreach (var item in ipSON.Where(t => t.SubIPID == x1.SubIPLabelID).ToList())
                            {
                                x1.Label.Add(new DTO.ResponseDto.V1_2_4.LabelDto()
                                {
                                    DictId = item.DictId,
                                    DictName = item.DictName
                                });
                            }
                        });
                    });

                }

            }
            //审标签项
            if (tp.Item3 != null)
            {
                var batchAudit = Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item3);
                //分类
                result.Category.Audit = new List<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>();
                result.Category.Audit = batchAudit.Where(t => t.Type == (int)Entities.ENUM.ENUM.EnumLabelType.分类).ToList();

                //市场场景                
                result.MarketScene.Audit = new List<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>();
                result.MarketScene.Audit = batchAudit.Where(t => t.Type == (int)Entities.ENUM.ENUM.EnumLabelType.市场场景).ToList();

                //分发场景                
                result.DistributeScene.Audit = new List<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>();
                result.DistributeScene.Audit = batchAudit.Where(t => t.Type == (int)Entities.ENUM.ENUM.EnumLabelType.分发场景).ToList();

                //IP标签                
                result.IPLabel.Audit = new List<DTO.ResponseDto.V1_2_4.IPLabel>();
                foreach (var item in batchAudit.Where(t => t.Type == (int)Entities.ENUM.ENUM.EnumLabelType.IP).ToList())
                {
                    result.IPLabel.Audit.Add(new DTO.ResponseDto.V1_2_4.IPLabel()
                    {
                        DictId = item.DictId,
                        DictName = item.DictName,
                        LabelID = item.LabelID
                    });
                }

                //子IP项（审核）item6
                if (tp.Item6 != null)
                {
                    var ipSUB = Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item6);
                    //标签项（审核）item7
                    var ipSON = new List<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>();
                    if (tp.Item7 != null)
                    {
                        ipSON = Util.DataTableToList<DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item7);
                    }

                    //遍历IP标签
                    result.IPLabel.Audit.ForEach(x =>
                    {
                        x.SubIP = new List<DTO.ResponseDto.V1_2_4.SubIPLabelDto>();
                        foreach (var item in ipSUB.Where(t => t.LabelID == x.LabelID).ToList())
                        {
                            x.SubIP.Add(new DTO.ResponseDto.V1_2_4.SubIPLabelDto()
                            {
                                DictId = item.DictId,
                                DictName = item.DictName,
                                SubIPLabelID = item.SubIPID
                            });
                        }


                        //遍历子IP
                        x.SubIP.ForEach(x1 =>
                        {
                            x1.Label = new List<DTO.ResponseDto.V1_2_4.LabelDto>();
                            foreach (var item in ipSON.Where(t => t.SubIPID == x1.SubIPLabelID).ToList())
                            {
                                x1.Label.Add(new DTO.ResponseDto.V1_2_4.LabelDto()
                                {
                                    DictId = item.DictId,
                                    DictName = item.DictName
                                });
                            }
                        });
                    });

                }
            }
            if (tp.Rest.Item1 != null)
            {
                result.ArticleIDs = tp.Rest.Item1.Rows[0][0].ToString();
            }
            //验证IP标签，原跟审核是否完全一样
            result.IpIsSame = StatisticsLabel(BatchMediaID);
            return result;
        }
        public bool StatisticsLabel(int BatchMediaID)
        {
            var ipLabelOriginalList = Util.DataTableToList<Entities.Examine.IpLabelInfo>(Dal.ExamineLabel.BasicLabel.Instance.QueryPendingLabelList(BatchMediaID));
            var ipLabelAuditlList = Util.DataTableToList<Entities.Examine.IpLabelInfo>(Dal.ExamineLabel.BasicLabel.Instance.QueryLabelList(BatchMediaID));

            if (ipLabelOriginalList != null && ipLabelAuditlList != null)
            {
                foreach (var item in ipLabelOriginalList)
                {
                    Entities.Examine.IpLabelInfo ipLabelInfo = ipLabelAuditlList.Where(t => t.IpId == item.IpId && t.IpName == item.IpName && t.SonIpId == item.SonIpId && t.SonIpName == item.SonIpName && t.LabelName == item.LabelName).FirstOrDefault();
                    if (ipLabelInfo != null)
                    {
                        ipLabelAuditlList.Remove(ipLabelInfo);
                    }
                    else
                    {
                        return false;
                    }
                }
                if (ipLabelAuditlList.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        #region 打标签媒体文章列表查询或领取
        /// <summary>
        /// 查询文章列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public object ArticleQuery(ReqArticleQueryOrReciveDto req, ref string errmsg)
        {
            //request.CreateUserID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
            errmsg = string.Empty;
            if (req == null)
            {
                errmsg = "请求参数不能为null";
                return null;
            }
            if (!req.CheckSelfModel(out errmsg))
                return null;

            int mediaType = -2;
            switch (req.Resource)
            {
                case (int)Entities.ENUM.ENUM.EnumResourceType.微信:
                    mediaType = (int)Entities.ENUM.ENUM.EnumMediaType.微信;
                    break;
                case (int)Entities.ENUM.ENUM.EnumResourceType.搜狐:
                    mediaType = (int)Entities.ENUM.ENUM.EnumMediaType.搜狐;
                    break;
                case (int)Entities.ENUM.ENUM.EnumResourceType.今日头条:
                    mediaType = (int)Entities.ENUM.ENUM.EnumMediaType.头条;
                    break;
            }
            //查询媒体是否存在
            //检查是否有待打批次
            var bmModel = ValidBatchMedia(mediaType, req.Number, req.Number, ref errmsg);
            if (!string.IsNullOrEmpty(errmsg))
                return null;

            var result = new ArticleQuery().GetQueryList(req);
            ResArticleQueryOrReciveDto resQueryOrRecive = new ResArticleQueryOrReciveDto()
            {
                ListInfo = result,
                MediaInfo = new ResArticleQueryOrReciveMediainfo()
                {
                    MediaType = bmModel.MediaType,
                    Name = bmModel.MediaName,
                    Number = bmModel.MediaNumber,
                    HeadImg = bmModel.HeadImg,
                    HomeUrl = bmModel.HomeUrl
                }
            };

            return resQueryOrRecive;

        }
        /// <summary>
        /// 领取文章
        /// </summary>
        /// <param name="req"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public int ArticleRecive(ReqArticleQueryOrReciveDto req, ref string errmsg)
        {
            //request.CreateUserID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
            errmsg = string.Empty;
            if (req == null)
            {
                errmsg = "请求参数不能为null";
                return -2;
            }
            if (!req.CheckSelfModel(out errmsg))
                return -2;

            int mediaType = -2;
            switch (req.Resource)
            {
                case (int)Entities.ENUM.ENUM.EnumResourceType.微信:
                    mediaType = (int)Entities.ENUM.ENUM.EnumMediaType.微信;
                    break;
                case (int)Entities.ENUM.ENUM.EnumResourceType.搜狐:
                    mediaType = (int)Entities.ENUM.ENUM.EnumMediaType.搜狐;
                    break;
                case (int)Entities.ENUM.ENUM.EnumResourceType.今日头条:
                    mediaType = (int)Entities.ENUM.ENUM.EnumMediaType.头条;
                    break;
            }
            //查询媒体是否存在
            //检查是否有待打批次
            var bmModel = ValidBatchMedia(mediaType, req.Number, req.Number, ref errmsg);
            if (!string.IsNullOrEmpty(errmsg))
                return -2;

            int iCount = Dal.MediaLabel.MediaLabel.Instance.ArticleReceive(req.ArticleIds, req.StartDate, req.EndDate, req.ArticleCount, req.Resource, req.Number, currentUserID, bmModel.BatchMediaID);
            //检查是否有
            return bmModel.BatchMediaID;
        }
        #endregion
        #region 根据批次ID查询领取的文章        
        public object QueryArticleListByBactchID(ReqArticleListByBactchIDQueryDto req, ref string errmsg)
        {
            //request.CreateUserID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
            errmsg = string.Empty;
            if (req == null)
            {
                errmsg = "请求参数不能为null";
                return null;
            }
            if (!req.CheckSelfModel(out errmsg))
                return null;

            //检查是否有待打批次
            var bmModel = BLL.BatchMedia.BatchMedia.Instance.GetModelByRecID(req.BatchMediaID);
            if (bmModel == null)
            {
                errmsg = "待打批次不存在";
                return null;
            }
            req.MediaType = bmModel.MediaType;
            var result = new ArticleListByBactchIDQuery().GetQueryList(req);
            ResArticleQueryOrReciveDto resQueryOrRecive = new ResArticleQueryOrReciveDto()
            {
                ListInfo = result,
                MediaInfo = new ResArticleQueryOrReciveMediainfo()
                {
                    MediaType = bmModel.MediaType,
                    Name = bmModel.MediaName,
                    Number = bmModel.MediaNumber,
                    HeadImg = bmModel.HeadImg,
                    HomeUrl = bmModel.HomeUrl
                }
            };
            return resQueryOrRecive;
        }

        #endregion

        /// <summary>
        /// ZLB 2017-11-28
        /// 查询打标签的文章数量
        /// </summary>
        /// <param name="BatchMediaID"></param>
        /// <param name="BatchType">类型（1媒体批次 2审核批次）</param>
        /// <returns></returns>
        public Dictionary<string, object> SelectArticleCount(int BatchMediaID, int BatchType)
        {
            int ArticleCount = 0;
            if (BatchType == 1)
            {
                ArticleCount = Dal.MediaLabel.MediaLabel.Instance.SelectArticleCount(BatchMediaID);
            }
            else
            {
                ArticleCount = Dal.ExamineLabel.ExamineLableOperate.Instance.SelectArticleCount(BatchMediaID);
            }
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("ArticleCount", ArticleCount);
            return dicAll;
        }

        #region 公用方法
        public Entities.BatchMedia.BatchMedia ValidBatchMedia(int mediaType, string number, string name, ref string errmsg)
        {
            errmsg = string.Empty;
            //查询媒体是否存在
            MediaModel mediaModel = BLL.BatchMedia.BatchMedia.Instance.GetMediaModel(mediaType, number);
            if (mediaModel == null)
            {
                errmsg = "媒体不存在";
                return null;
            }

            //检查是否有待打批次
            Entities.BatchMedia.QueryBatchMedia query = new Entities.BatchMedia.QueryBatchMedia()
            {
                MediaType = mediaModel.MediaType,
                MediaNumber = mediaModel.Number,
                MediaName = mediaModel.Name,
                CurrentUserID = currentUserID,
                Status = (int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打
            };
            Entities.BatchMedia.BatchMedia bmModel = BLL.BatchMedia.BatchMedia.Instance.GetModelByMedia(query);
            if (bmModel == null)//没有待打批次
            {
                query.BatchMediaID = BLL.BatchMedia.BatchMedia.Instance.Insert(new Entities.BatchMedia.BatchMedia()
                {
                    MediaType = query.MediaType,
                    MediaName = query.MediaName,
                    MediaNumber = query.MediaNumber,
                    HomeUrl = mediaModel.HomeUrl,
                    HeadImg = mediaModel.HeadImg,
                    CreateUserID = currentUserID,
                    Status = query.Status,
                    TaskType = (int)Entities.ENUM.ENUM.EnumTaskType.媒体,
                    IsSelfDo = mediaModel.IsSelfDo
                });
            }
            else
                query.BatchMediaID = bmModel.BatchMediaID;

            return BLL.BatchMedia.BatchMedia.Instance.GetModelByRecID(query.BatchMediaID);
        }
        /// <summary>
        /// 根据媒体类型、帐号或名称判断是有审核通过的标签记录
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="numberORname"></param>
        /// <returns></returns>
        public bool IsExistsLabelByMedia(int mediaType, string numberORname)
        {
            return Dal.MediaLabel.MediaLabel.Instance.IsExistsLabelByMedia(mediaType, numberORname);
        }
        #endregion
    }
}
