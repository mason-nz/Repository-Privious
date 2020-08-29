using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider
{
    public class BindingWeiXinProvider : VerifyOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly ReqMediaUpdateWxOfferDto _reqMediaUpdateWxOffer;

        public BindingWeiXinProvider(ConfigEntity configEntity, ReqMediaUpdateWxOfferDto reqMediaUpdateWxOffer)
        {
            _configEntity = configEntity;
            _reqMediaUpdateWxOffer = reqMediaUpdateWxOffer;
        }


        #region 修改报价相关

        public ReturnValue UpdateOffer()
        {
            var retValue = VerifyOfMedia();

            if (retValue.HasError)
            {
                return retValue;
            }

            var excuteCount = Dal.LETask.LeWeixin.Instance.UpdateOffer(_reqMediaUpdateWxOffer.MediaId, _reqMediaUpdateWxOffer.FansCount,
                  _reqMediaUpdateWxOffer.CategoryId, _reqMediaUpdateWxOffer.FansMalePer,
                  _reqMediaUpdateWxOffer.FansFemalePer);
            if (excuteCount < 1)
            {
                return CreateFailMessage(retValue, "2003", "修改报价出错");
            }
            Task.Run(() => InsertAreaMapping())
                .ContinueWith(s => InsertPricesInfo());

            return retValue;
        }

        public ReturnValue VerifyOfMedia()
        {
            var retValue = VerifyOfNecessaryParameters(_reqMediaUpdateWxOffer);
            if (retValue.HasError)
                return retValue;
            if (_reqMediaUpdateWxOffer.OverlayArea == null)
            {
                return CreateFailMessage(retValue, "2001", "请输入覆盖区域参数");
            }
            if (!_reqMediaUpdateWxOffer.DeliveryPrices.Any())
            {
                return CreateFailMessage(retValue, "2002", "请输入投放价格");
            }
            var info = Dal.LETask.LeWeixin.Instance.GetInfo(_reqMediaUpdateWxOffer.MediaId);
            if (info == null)
            {
                return CreateFailMessage(retValue, "2000", "当前媒体id 不存在");
            }
            //if (info.CreateUserID != _configEntity.CreateUserId)
            //{
            //    return CreateFailMessage(retValue, "2005", "当前媒体不属于您，没有更改的权限");
            //}
            return retValue;
        }

        private void InsertAreaMapping()
        {
            var entity = new LeMediaAreaMapping()
            {
                RelateType = (int)MediaAreaMappingType.CoverageArea,
                CityID = _reqMediaUpdateWxOffer.OverlayArea.CityId,
                ProvinceID = _reqMediaUpdateWxOffer.OverlayArea.ProvinceId,
                MediaID = _reqMediaUpdateWxOffer.MediaId,
                CreateTime = DateTime.Now,
                CreateUserID = _configEntity.CreateUserId,
                MediaType = (int)MediaType.WeiXin
            };
            var excuteCount = Dal.LETask.LeMediaAreaMapping.Instance.Update(entity);
            if (excuteCount <= 1)
            {
                Loger.Log4Net.Info($"weixin InsertAreaMapping 失败。参数：{JsonConvert.SerializeObject(entity)}");
            }
        }

        private void InsertPricesInfo()
        {
            var list = _reqMediaUpdateWxOffer.DeliveryPrices.Select(s => new LePublishDetailInfo()
            {
                MediaID = _reqMediaUpdateWxOffer.MediaId,
                MediaType = (int)MediaType.WeiXin,
                ADPosition1 = s.ADPosition1,
                ADPosition2 = s.ADPosition2,
                ADPosition3 = -2,
                CreateUserID = _configEntity.CreateUserId,
                CreateTime = DateTime.Now,
                Price = s.Price,
                PublishStatus = 0
            }).ToList();

            var excuteCount = Dal.LETask.LePublishDetailInfo.Instance.Insert(list);
            if (excuteCount <= 0)
            {
                Loger.Log4Net.Info($"weixin InsertPricesInfo 失败。参数：{JsonConvert.SerializeObject(list)}");
            }
        }

        #endregion


        #region 修改报价详情

        /// <summary>
        /// 修改报价加载信息
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public RespWeiXinInfoDto GetInfo(int mediaId)
        {
            var resp = new RespWeiXinInfoDto()
            {
                OverlayArea = new Overlayarea()
            };
            var info = Dal.LETask.LeWeixin.Instance.GetInfoAndPrice(mediaId);
            if (info == null)
            {
                return resp;
            }
            resp = AutoMapper.Mapper.Map<Entities.LETask.LeWeixin, RespWeiXinInfoDto>(info);

            BindingsWeiXinQuery.SetPricesInfos(resp);
            resp.OverlayArea = new Overlayarea()
            {
                ProvinceId = info.MpProvinceId,
                CityId = info.MpCityId
            };
            return resp;
        }

        #endregion
    }
}
