using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider
{
    public class BindingWeiBoProvider : VerifyOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly ReqMediaUpdateWbOfferDto _reqMediaUpdateWbOfferDto;

        public BindingWeiBoProvider(ConfigEntity configEntity, ReqMediaUpdateWbOfferDto reqMediaUpdateWbOfferDto)
        {
            _configEntity = configEntity;
            _reqMediaUpdateWbOfferDto = reqMediaUpdateWbOfferDto;
        }

        #region 修改报价相关

        public ReturnValue UpdateOffer()
        {
            var retValue = VerifyOfMedia();

            if (retValue.HasError)
            {
                return retValue;
            }

            var excuteCount = Dal.LETask.LeWeibo.Instance.UpdateOffer(_reqMediaUpdateWbOfferDto.MediaId, _reqMediaUpdateWbOfferDto.CategoryId);
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
            var retValue = VerifyOfNecessaryParameters(_reqMediaUpdateWbOfferDto);
            if (retValue.HasError)
                return retValue;
            if (_reqMediaUpdateWbOfferDto.OverlayArea == null)
            {
                return CreateFailMessage(retValue, "2001", "请输入覆盖区域参数");
            }
            if (!_reqMediaUpdateWbOfferDto.DeliveryPrices.Any())
            {
                return CreateFailMessage(retValue, "2002", "请输入投放价格");
            }
            var info = Dal.LETask.LeWeibo.Instance.GetInfo(_reqMediaUpdateWbOfferDto.MediaId);
            if (info == null)
            {
                return CreateFailMessage(retValue, "2000", "当前媒体id 不存在");
            }
            return retValue;
        }

        private void InsertAreaMapping()
        {
            var entity = new LeMediaAreaMapping()
            {
                RelateType = (int)MediaAreaMappingType.CoverageArea,
                CityID = _reqMediaUpdateWbOfferDto.OverlayArea.CityId,
                ProvinceID = _reqMediaUpdateWbOfferDto.OverlayArea.ProvinceId,
                MediaID = _reqMediaUpdateWbOfferDto.MediaId,
                CreateTime = DateTime.Now,
                CreateUserID = _configEntity.CreateUserId,
                MediaType = (int)MediaType.WeiBo
            };
            var excuteCount = Dal.LETask.LeMediaAreaMapping.Instance.Update(entity);
            if (excuteCount <= 1)
            {
                Loger.Log4Net.Info($"weibo InsertAreaMapping 失败。参数：{JsonConvert.SerializeObject(entity)}");
            }
        }

        private void InsertPricesInfo()
        {
            var list = _reqMediaUpdateWbOfferDto.DeliveryPrices.Select(s => new LePublishDetailInfo()
            {
                MediaID = _reqMediaUpdateWbOfferDto.MediaId,
                MediaType = (int)MediaType.WeiBo,
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
                Loger.Log4Net.Info($"weibo InsertPricesInfo 失败。参数：{JsonConvert.SerializeObject(list)}");
            }
        }

        #endregion

    }
}
