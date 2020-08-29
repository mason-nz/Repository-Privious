using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Verify;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.Recommend
{
    public class RecommendQueryProxy : CurrentOperateBase
    {
        private const int FilterAppPublishCount = 4;
        private const int FilterWeiXinPublishCount = 6;
        private const int FilterWeiBoPublishCount = 7;
        private const int FilterVideoPublishCount = 7;
        private const int FilterBroadcastPublishCount = 4;

        private readonly ConfigEntity _configEntity;
        private readonly RecommendSearchDto _recommendSearchDto;
        private readonly AddRecommendDto _addRecommendDto;
        private readonly UpdateRecommendDto _updateRecommendDto;
        private Dictionary<int, Func<string, dynamic>> _dictionary; //推荐到首页查询接口泛型集合
        private List<Entities.Media.MediaCommonlyClass> _commonlyClass;

        public RecommendQueryProxy(RecommendSearchDto recommendSearchDto)
        {
            _recommendSearchDto = recommendSearchDto;
            _configEntity = new ConfigEntity();
            Initialization();
        }

        public RecommendQueryProxy(AddRecommendDto addRecommendDto, UpdateRecommendDto updateRecommendDto)
        {
            _addRecommendDto = addRecommendDto;
            _updateRecommendDto = updateRecommendDto;
        }

        private void Initialization()
        {
            if (_recommendSearchDto == null)
                return;
            _dictionary = new Dictionary<int, Func<string, dynamic>>()
            {
                {(int) MediaType.WeiXin, s => new RecommendWeiXinQuery(_configEntity).GetQueryList(_recommendSearchDto)},
                {(int) MediaType.WeiBo, s => new RecommendWeiBoQuery(_configEntity).GetQueryList(_recommendSearchDto)},
                {(int) MediaType.Video, s => new RecommendVideoQuery(_configEntity).GetQueryList(_recommendSearchDto)},
                {
                    (int) MediaType.Broadcast,
                    s => new RecommendBroadcastQuery(_configEntity).GetQueryList(_recommendSearchDto)
                },
                {(int) MediaType.APP, s => new RecommendAppQuery(_configEntity).GetQueryList(_recommendSearchDto)}
            };
        }

        /// <summary>
        /// 推荐到首页查询接口
        /// </summary>
        /// <returns></returns>
        public dynamic GetQuery()
        {
            return _dictionary.ContainsKey(_recommendSearchDto.BusinessType)
                ? _dictionary[_recommendSearchDto.BusinessType].Invoke(string.Empty)
                : default(dynamic);
        }

        public T GetEntity<T>(MediaType mediaType, ReturnValue returnValue, T t) where T : class, new()
        {
            return returnValue.ReturnObject as T;
        }

        public dynamic GetEntity(MediaType mediaType, ReturnValue returnValue)
        {
            switch (mediaType)
            {
                case MediaType.WeiXin:
                    return returnValue.ReturnObject as Entities.WeixinOAuth.WeixinInfo;

                case MediaType.Video:
                    return returnValue.ReturnObject as Entities.Media.MediaVideo;

                case MediaType.APP:
                    return returnValue.ReturnObject as Entities.Media.MediaBasePCAPP;

                case MediaType.Broadcast:
                    return returnValue.ReturnObject as Entities.Media.MediaBroadcast;

                case MediaType.WeiBo:
                    return returnValue.ReturnObject as Entities.Media.MediaWeibo;

                default:
                    returnValue.HasError = true;
                    returnValue.ErrorCode = "30012";
                    returnValue.Message = "拆箱失败";
                    return null;
            }
        }

        public Entities.Recommend.HomeMedia GetRecommendEntity(int categoryId)
        {
            //get sortNumber
            var sortNumber =
                _addRecommendDto.BusinessType == (int)MediaType.APP
                    ? Dal.Recommend.HomeMedia.Instance.GetSortNumberApp(categoryId)
                    : Dal.Recommend.HomeMedia.Instance.GetSortNumber(_addRecommendDto.BusinessType, categoryId);

            return new Entities.Recommend.HomeMedia()
            {
                MediaID = _addRecommendDto.MediaId,
                CategoryID = categoryId,
                MediaType = _addRecommendDto.BusinessType,
                PublishState = (int)HomePublishStateEnum.Unpublished,
                CreateUserId = _addRecommendDto.CreateUserId,
                SortNumber = sortNumber,
                ImageUrl = _addRecommendDto.ImageUrl.ToAbsolutePath(),
                VideoUrl = _addRecommendDto.VideoUrl.ToAbsolutePath()
            };
        }

        /// <summary>
        /// 添加到推荐首页
        /// </summary>
        /// <returns></returns>
        public ReturnValue AddToRecommend()
        {
            var retValue = new ReturnValue() { ErrorCode = "30016", HasError = true, Message = "请输入合法的BusinessType" };
            if (!Enum.IsDefined(typeof(MediaType), _addRecommendDto.BusinessType))
                return retValue;
            //验证推荐权限
            retValue = VerifyOfAddRecommendModule(retValue);
            if (retValue.HasError)
                return retValue;

            Entities.Recommend.HomeMedia insertRecommendEntity;
            if (_addRecommendDto.BusinessType == (int)MediaType.APP)
            {
                #region app 没有分类信息

                retValue = VerifyAddForApp(retValue);
                if (retValue.HasError)
                    return retValue;

                insertRecommendEntity = GetRecommendEntity(0);
                insertRecommendEntity.ADDetailID = _addRecommendDto.ADDetailID;
                insertRecommendEntity.TemplateID = _addRecommendDto.TemplateID;

                #endregion app 没有分类信息
            }
            else
            {
                retValue = VerifyOfAddToRecommend();
                if (retValue.HasError)
                    return retValue;
                //获取实体
                var mediaInfo = GetEntity((MediaType)_addRecommendDto.BusinessType, retValue);
                if (mediaInfo == null)
                    return retValue;
                insertRecommendEntity = GetRecommendEntity(GetCategoryId());
            }

            //验证是否上架并且在有效期内
            //retValue = VerifyOfAddToRecommendPublishStatus(retValue);
            //if (retValue.HasError)
            //    return retValue;
            var mediaId = Dal.Recommend.HomeMedia.Instance.Insert(insertRecommendEntity);

            if (mediaId > 0)
            {
                retValue.HasError = false;
                retValue.ReturnObject = mediaId;
                retValue.Message = "操作成功";
                return retValue;
            }
            retValue.HasError = true;
            retValue.Message = "操作失败";
            return retValue;
        }

        private ReturnValue VerifyAddForApp(ReturnValue retValue)
        {
            if (_addRecommendDto.ADDetailID <= 0)
            {
                return CreateFailMessage(retValue, "30015", "app推荐，请输入刊例 ADDetailID");
            }
            if (_addRecommendDto.TemplateID <= 0)
            {
                return CreateFailMessage(retValue, "30020", "app推荐请输入参数TemplateId");
            }

            if (Dal.AdTemplate.AppAdTemplate.Instance.GetEntity(_addRecommendDto.TemplateID) == null)
            {
                return CreateFailMessage(retValue, "30021", string.Format("模板信息不存在 TemplateId={0} ", _addRecommendDto.TemplateID));
            }

            //验证是否已经存在
            if (Dal.Recommend.HomeMedia.Instance.GetEntityByApp(0, (int)MediaType.APP, _addRecommendDto.TemplateID) != null)
            {
                retValue.HasError = true;
                retValue.ErrorCode = "30014";
                retValue.Message = string.Format("当前模板Id已经存在推荐列表中，Id={0}", _addRecommendDto.TemplateID);
                return retValue;
            }
            retValue.HasError = false;
            retValue.ErrorCode = string.Empty;

            return retValue;
        }

        private ReturnValue VerifyCategoryId(ReturnValue retValue)
        {
            if (_addRecommendDto.CategoryId > 0)
            {
                if (_addRecommendDto.BusinessType == (int)MediaType.WeiXin)
                {
                    _commonlyClass =
                        Dal.Media.MediaCommonlyClass.Instance.GetListByMediaCategory(new MediaQuery<Entities.Media.MediaCommonlyClass>()
                        {
                            MediaType = _addRecommendDto.BusinessType,
                            MediaId = _addRecommendDto.MediaId
                        });
                }
                else
                {
                    _commonlyClass =
                      Dal.Media.MediaCommonlyClass.Instance.GetList(new MediaQuery<Entities.Media.MediaCommonlyClass>()
                      {
                          MediaType = _addRecommendDto.BusinessType,
                          MediaId = _addRecommendDto.MediaId
                      });
                }
                if (_commonlyClass.Count == 0)
                {
                    return CreateFailMessage(retValue, "30013", "此媒体常见分类列表没有数据");
                }
                var list = _commonlyClass.Where(s => s.CategoryID == _addRecommendDto.CategoryId).ToList();
                if (list.Count == 0)
                {
                    return CreateFailMessage(retValue, "30014", "分类id，CategoryId在此媒体常见分类列表中不存在");
                }
            }

            return retValue;
        }

        private int GetCategoryId()
        {
            if (_addRecommendDto.CategoryId > 0)
            {
                //大于0 说明要添加的就是这个分类
                return _addRecommendDto.CategoryId;
            }
            else
            {
                //注意：这个版本为了区分 微信 和 其他自媒体的分类校验
                //说明选择的是全部，则要要当前媒体常见分类的主分类id
                if (_addRecommendDto.BusinessType == (int)MediaType.WeiXin)
                {
                    return Dal.Media.MediaCommonlyClass.Instance.GetListByMediaCategory(new MediaQuery<Entities.Media.MediaCommonlyClass>()
                    {
                        MediaType = _addRecommendDto.BusinessType,
                        MediaId = _addRecommendDto.MediaId
                    }).OrderByDescending(s => s.SortNumber)
                                        .Select(s => s.CategoryID).FirstOrDefault();
                }
                else
                {
                    return Dal.Media.MediaCommonlyClass.Instance.GetList(new MediaQuery<Entities.Media.MediaCommonlyClass>()
                    {
                        MediaType = _addRecommendDto.BusinessType,
                        MediaId = _addRecommendDto.MediaId
                    }).OrderByDescending(s => s.SortNumber)
                                        .Select(s => s.CategoryID).FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public ReturnValue UpdateRecommend()
        {
            var retValue = VerifyOfUpdateRecommend();
            if (retValue.HasError) return retValue;
            var info = retValue.ReturnObject as Entities.Recommend.HomeMedia;
            if (info == null)
            {
                retValue.HasError = true;
                retValue.ErrorCode = "30004";
                retValue.Message = "拆箱错误";
                return retValue;
            }
            info.SortNumber = _updateRecommendDto.SortNumber;
            info.VideoUrl = _updateRecommendDto.VideoUrl.ToAbsolutePath();
            info.ImageUrl = _updateRecommendDto.ImageUrl.ToAbsolutePath();
            if (Dal.Recommend.HomeMedia.Instance.Update(info) <= 0)
            {
                retValue.HasError = true;
                retValue.ErrorCode = "30005";
                retValue.Message = "编辑错误";
                return retValue;
            }
            return retValue;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public ReturnValue DeleteRecommend(int recId)
        {
            var retValue = new ReturnValue()
            {
                HasError = true,
                ErrorCode = "30026",
                Message = string.Format("当前信息不存在，Id={0}", recId)
            };
            var recommendInfo = Dal.Recommend.HomeMedia.Instance.GetEntity(recId);
            if (recommendInfo == null)
            {
                return retValue;
            }
            if (Dal.Recommend.HomeMedia.Instance.Delete(recId) == 0)
            {
                retValue.ErrorCode = "30007";
                retValue.Message = "删除失败";
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public ReturnValue UpdatePublishState(int mediaType)
        {
            var retValue = new ReturnValue();
            retValue = VerifyPublishCount(retValue, mediaType);
            if (retValue.HasError)
                return retValue;
            var excuteCount = Dal.Recommend.HomeMedia.Instance.UpdatePublishState(mediaType,
                HomePublishStateEnum.Alreadypublished);
            if (excuteCount == 0)
            {
                retValue.HasError = true;
                retValue.ErrorCode = "30006";
                retValue.Message = string.Format("发布失败，mediaType={0}", mediaType);
                return retValue;
            }
            Dal.HomeCategory.Instance.UpdatePublishState(mediaType, HomePublishStateEnum.Alreadypublished);
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        private ReturnValue VerifyOfAddToRecommend()
        {
            var retValue = new VerifyOfNecessaryParameters<AddRecommendDto>()
                .VerifyNecessaryParameters(_addRecommendDto);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyOfAddToRecommendMediaInfo();
            if (retValue.HasError)
                return retValue;

            if (_addRecommendDto.BusinessType != (int)MediaType.APP)
            {
                //验证分类id
                retValue = VerifyCategoryId(retValue);
                if (retValue.HasError)
                    return retValue;
            }

            if (Dal.Recommend.HomeMedia.Instance.GetEntity(_addRecommendDto.MediaId, _addRecommendDto.BusinessType) !=
                null)
            {
                retValue.HasError = true;
                retValue.ErrorCode = "30011";
                retValue.Message = "该媒体推荐列表已经存在";
                return retValue;
            }

            return retValue;
        }

        private ReturnValue VerifyOfAddToRecommendMediaInfo()
        {
            var returnValue = new ReturnValue()
            {
                HasError = true,
                ErrorCode = "30001",
                Message = string.Format("当前媒体信息不存在，BusinessType={0}&MediaId={1}",
                    _addRecommendDto.BusinessType, _addRecommendDto.MediaId)
            };
            var mediaDic = new Dictionary<int, Func<string, dynamic>>()
            {
                {(int) MediaType.WeiXin, s => Dal.WeixinOAuth.Instance.GetWeixinInfoByID(_addRecommendDto.MediaId)},
                {(int) MediaType.WeiBo, s => Dal.Media.MediaWeibo.Instance.GetEntity(_addRecommendDto.MediaId)},
                {(int) MediaType.Video, s => Dal.Media.MediaVideo.Instance.GetEntity(_addRecommendDto.MediaId)},
                {(int) MediaType.Broadcast, s => Dal.Media.MediaBroadcast.Instance.GetEntity(_addRecommendDto.MediaId)},
                {(int) MediaType.APP, s => Dal.Media.MediaPCAPP.Instance.GetBaseEntity(_addRecommendDto.ADDetailID)}
            };
            if (mediaDic.ContainsKey(_addRecommendDto.BusinessType))
            {
                var mediaInfo = mediaDic[_addRecommendDto.BusinessType].Invoke(string.Empty);
                if (mediaInfo == null)
                {
                    return returnValue;
                }
                returnValue.HasError = false;
                returnValue.Message = string.Empty;
                returnValue.ReturnObject = mediaInfo;
                return returnValue;
            }
            else
            {
                returnValue.HasError = true;
                returnValue.ErrorCode = "30000";
                returnValue.Message = "请输入合法的BusinessType";
                return returnValue;
            }
        }

        private ReturnValue VerifyOfAddToRecommendPublishStatus(ReturnValue retValue)
        {
            var returnValue = new ReturnValue()
            {
                HasError = true,
                ErrorCode = "30019",
                Message = string.Format("当前媒体信息不存在,或者刊例状态不为已上架，BusinessType={0}&MediaId={1}&ADDetailID={2}",
                   _addRecommendDto.BusinessType, _addRecommendDto.MediaId, _addRecommendDto.ADDetailID)
            };
            var mediaDic = new Dictionary<int, Func<string, dynamic>>()
            {
                {(int) MediaType.WeiXin, s => Dal.Media.MediaWeixin.Instance.GetBaseEntity(_addRecommendDto.MediaId)},
                {(int) MediaType.WeiBo, s => Dal.Media.MediaWeibo.Instance.GetBaseEntity(_addRecommendDto.MediaId)},
                {(int) MediaType.Video, s => Dal.Media.MediaVideo.Instance.GetBaseEntity(_addRecommendDto.MediaId)},
                {(int) MediaType.Broadcast, s => Dal.Media.MediaBroadcast.Instance.GetBaseEntity(_addRecommendDto.MediaId)},
                {(int) MediaType.APP, s => Dal.Media.MediaPCAPP.Instance.GetBaseEntity(_addRecommendDto.ADDetailID)}
            };
            if (mediaDic.ContainsKey(_addRecommendDto.BusinessType))
            {
                var mediaInfo = mediaDic[_addRecommendDto.BusinessType].Invoke(string.Empty);
                if (mediaInfo == null)
                {
                    return returnValue;
                }
                returnValue.HasError = false;
                returnValue.Message = string.Empty;
                returnValue.ReturnObject = mediaInfo;
                return returnValue;
            }
            else
            {
                returnValue.HasError = true;
                returnValue.ErrorCode = "30000";
                returnValue.Message = "请输入合法的BusinessType";
                return returnValue;
            }
        }

        private ReturnValue VerifyOfUpdateRecommend()
        {
            var retValue = new VerifyOfNecessaryParameters<UpdateRecommendDto>()
                        .VerifyNecessaryParameters(_updateRecommendDto);
            if (retValue.HasError)
                return retValue;
            var recommendInfo = Dal.Recommend.HomeMedia.Instance.GetEntity(_updateRecommendDto.RecId);
            if (recommendInfo == null)
            {
                retValue.HasError = true;
                retValue.Message = string.Format("当前信息不存在，Id={0}", _updateRecommendDto.RecId);
                return retValue;
            }
            retValue.HasError = false;
            retValue.ReturnObject = recommendInfo;
            return retValue;
        }

        private ReturnValue VerifyOfRole(ReturnValue retValue, string moduleId)
        {
            if (!Chitunion2017.Common.UserInfo.CheckRight(moduleId, "SYS001"))
            {
                retValue.HasError = true;
                retValue.ErrorCode = "30020";
                retValue.Message = "您暂没有推荐权限";
                return retValue;
            }

            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        private ReturnValue VerifyOfAddRecommendModule(ReturnValue returnValue)
        {
            var dic = new Dictionary<int, string>()
            {
                { (int) MediaType.WeiXin,"SYS001MOD1301"},
                { (int) MediaType.APP,"SYS001MOD1305"},
                { (int) MediaType.Broadcast,"SYS001MOD1304"},
                { (int) MediaType.WeiBo,"SYS001MOD1302"},
                { (int) MediaType.Video,"SYS001MOD1303"}
            };

            var value = dic.FirstOrDefault(s => s.Key == _addRecommendDto.BusinessType);

            if (string.IsNullOrWhiteSpace(value.Value))
            {
                returnValue.HasError = true;
                returnValue.ErrorCode = "30017";
                returnValue.Message = "VerifyOfAddRecommendModule 异常";
                return returnValue;
            }

            return VerifyOfRole(returnValue, value.Value);
        }

        public ReturnValue VerifyPublishCount(ReturnValue returnValue, int mediaType)
        {
            var filterConfig = GetFilterCount(mediaType);
            if (mediaType == (int)MediaType.APP)
            {
                return VerifyAppPublishCount(returnValue, filterConfig);
            }
            else
            {
                return VerifyMediaPublishCount(returnValue, mediaType, filterConfig);
            }
        }

        /// <summary>
        /// 验证各分类下的推荐数量是否满足设定的值
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="mediaType"></param>
        /// <param name="filterCount"></param>
        /// <returns></returns>
        public ReturnValue VerifyMediaPublishCount(ReturnValue returnValue, int mediaType, int filterCount)
        {
            var doesNotPublishList = Dal.Recommend.HomeMedia.Instance.GetFilterCount(mediaType, filterCount);
            if (doesNotPublishList.Count > 0)
            {
                var sbMsg = new StringBuilder();
                var retMsg = "分类下：";
                bool isFilter = false;
                foreach (var item in doesNotPublishList)
                {
                    if (item.TotleCount < filterCount)
                    {
                        isFilter = true;
                        sbMsg.AppendFormat(" {0} ", item.CategoryName);
                    }
                }
                if (isFilter)
                {
                    retMsg += sbMsg;
                    retMsg += string.Format("媒体数量少于{0}条，不能生成", filterCount);
                }

                returnValue.HasError = true;
                returnValue.Message = retMsg;
                return returnValue;
            }
            else
            {
                //没有数据有2种条件，第一：条件满足可以发布，第二：根本没有数据
                //如果没有数据，再次查询推荐表有没有数据，如果没有则提示，如果有则满足条件，可以发布
                var list = Dal.Recommend.HomeMedia.Instance.GetEntityByMediaType(mediaType);
                var sbMsg = new StringBuilder();
                var retMsg = string.Empty;
                bool isFilter = false;
                foreach (var item in list)
                {
                    if (item.MediaID <= 0)
                    {
                        isFilter = true;
                        sbMsg.AppendFormat(" {0} ", item.CategoryName);
                    }
                }
                if (isFilter)
                {
                    retMsg += "分类下：";
                    retMsg += sbMsg;
                    retMsg += string.Format("媒体数量少于{0}条，不能生成", filterCount);
                }

                returnValue.HasError = isFilter;
                returnValue.Message = retMsg;
                return returnValue;
            }
        }

        public ReturnValue VerifyAppPublishCount(ReturnValue returnValue, int filterCount)
        {
            var doesNotCount = Dal.Recommend.HomeMedia.Instance.GetAppFilterCount(filterCount);

            if (doesNotCount > 0)
            {
                returnValue.HasError = true;
                returnValue.Message = string.Format("媒体app数量少于{0}条，不能生成", filterCount);
                return returnValue;
            }
            else
            {//没有数据有2种条件，第一：条件满足可以发布，第二：根本没有数据
             //如果没有数据，再次查询推荐表有没有数据，如果没有则提示，如果有则满足条件，可以发布
                if (Dal.Recommend.HomeMedia.Instance.GetEntityByMediaTypeApp() == 0)
                {
                    returnValue.HasError = true;
                    returnValue.Message = string.Format("媒体app数量少于{0}条，不能生成", filterCount);
                    return returnValue;
                }
            }

            returnValue.HasError = false;
            returnValue.Message = string.Empty;
            return returnValue;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        //public FilterHomeMediaCountConfig GetFilterHomeMediaCountConfig()
        //{
        //    return SectionInvoke<FilterHomeMediaCountConfig>.GetConfig(FilterHomeMediaCountConfig.SectionName);
        //}

        public int GetFilterCount(int mediaType)
        {
            //var filterConfig = GetFilterHomeMediaCountConfig();
            switch (mediaType)
            {
                case (int)MediaType.APP:
                    //return filterConfig.FilterAppPublishCount;
                    return FilterAppPublishCount;

                case (int)MediaType.Broadcast:
                    //return filterConfig.FilterBroadcastPublishCount;
                    return FilterBroadcastPublishCount;

                case (int)MediaType.Video:
                    //return filterConfig.FilterVideoPublishCount;
                    return FilterVideoPublishCount;

                case (int)MediaType.WeiBo:
                    //return filterConfig.FilterWeiBoPublishCount;
                    return FilterWeiBoPublishCount;

                case (int)MediaType.WeiXin:
                    //return filterConfig.FilterWeiXinPublishCount;
                    return FilterWeiXinPublishCount;

                default:
                    Loger.Log4Net.ErrorFormat("GetFilterCount mediaType Exception..mediaType:{0}", mediaType);
                    throw new Exception("GetFilterCount mediaType Exception..");
            }
        }
    }

    public enum RecommendOperaeteEnum
    {
        [Description("发布")]
        Publish = 1,

        [Description("添加到推荐列表")]
        AddToRecommend = 2,

        [Description("修改")]
        Edit = 3,

        [Description("删除")]
        Delete = 4
    }
}