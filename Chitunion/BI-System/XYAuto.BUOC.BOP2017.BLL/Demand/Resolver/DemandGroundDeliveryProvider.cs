/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 17:39:27
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Request;
using XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Response;
using XYAuto.BUOC.BOP2017.BLL.GDT;
using XYAuto.BUOC.BOP2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.BOP2017.Entities.Demand;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;
using XYAuto.BUOC.BOP2017.Entities.GDT;
using XYAuto.BUOC.BOP2017.Entities.Query.Demand;
using XYAuto.BUOC.BOP2017.Infrastruction;
using XYAuto.BUOC.BOP2017.Infrastruction.Extend;
using XYAuto.BUOC.BOP2017.Infrastruction.Security;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;

namespace XYAuto.BUOC.BOP2017.BLL.Demand.Resolver
{
    /// <summary>
    /// auth:lixiong
    /// desc:落地页加参管理
    /// </summary>
    public class DemandGroundDeliveryProvider : Infrastruction.Verification.VerifyOperateBase
    {
        private readonly RequestGroundDeliveryDto _contextGroundDeliveryDto;
        private readonly RequestDeleteDeliveryDto _contextDeleteDeliveryDto;
        private readonly ConfigEntity _configEntity;

        public DemandGroundDeliveryProvider(RequestGroundDeliveryDto contextGroundDeliveryDto,
            RequestDeleteDeliveryDto contextDeleteDeliveryDto,
            ConfigEntity configEntity)
        {
            _contextGroundDeliveryDto = contextGroundDeliveryDto;
            _contextDeleteDeliveryDto = contextDeleteDeliveryDto;
            _configEntity = configEntity;
        }

        #region 加参操作

        public ReturnValue Excute()
        {
            var retValue = VerifyOfExcute();
            if (retValue.HasError)
                return retValue;

            if (Dal.Demand.DemandGroundDelivery.Instance.Insert(GetEntity()) <= 0)
            {
                Loger.Log4Net.Error($" 落地页加参失败,添加参数：{JsonConvert.SerializeObject(GetEntity())}");
                return CreateFailMessage(retValue, "60006", "加参失败！");
            }
            return retValue;
        }

        public ReturnValue VerifyOfExcute()
        {
            var retValue = VerifyOfNecessaryParameters<RequestGroundDeliveryDto>(_contextGroundDeliveryDto);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyOfGroundId(retValue);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyOfAdName(retValue, _contextGroundDeliveryDto.DemandBillNo);
            if (retValue.HasError)
                return retValue;
            return retValue;
        }

        private ReturnValue VerifyOfGroundId(ReturnValue retValue)
        {
            var groundPages = Dal.Demand.DemandGroundPage.Instance.GetGroundPages(new DemandGroundQuery<DemandGroundPage>()
            {
                GroundId = _contextGroundDeliveryDto.GroundId
            });
            if (!groundPages.Any())
            {
                return CreateFailMessage(retValue, "60002", $"当前需求id不存在或groundId不存在：{_contextGroundDeliveryDto.GroundId}");
            }
            var demandInfo = groundPages.FirstOrDefault();
            if (demandInfo == null)
                return CreateFailMessage(retValue, "60003", $"当前需求id不存在或groundId不存在：{_contextGroundDeliveryDto.GroundId}");
            if (demandInfo.AuditStatus != (int)DemandAuditStatusEnum.Puting && demandInfo.AuditStatus != (int)DemandAuditStatusEnum.PendingPutIn)
            {
                return CreateFailMessage(retValue, "60004", $"当前需求状态下不允许落地页加参操作，必须为：待投放/投放中状态");
            }
            _contextGroundDeliveryDto.DemandBillNo = demandInfo.DemandBillNo;
            _contextGroundDeliveryDto.PromotionUrl = demandInfo.PromotionUrl;
            return retValue;
        }

        private ReturnValue VerifyOfAdName(ReturnValue retValue, int demandBillNo)
        {
            //todo:一个需求单下面，广告名称不能重复

            var groundDeliveryList = Dal.Demand.DemandGroundDelivery.Instance.GetList(new DemandGroundQuery<DemandGroundDelivery>()
            {
                DemandBillNo = demandBillNo,
                AdName = _contextGroundDeliveryDto.AdName
            });

            if (groundDeliveryList.Any())
            {
                return CreateFailMessage(retValue, "60001", "广告名称已存在!(同一个需求下不能有重复)");
            }
            return retValue;
        }

        private Entities.Demand.DemandGroundDelivery GetEntity()
        {
            var info = AutoMapper.Mapper.Map<RequestGroundDeliveryDto, Entities.Demand.DemandGroundDelivery>(
                   _contextGroundDeliveryDto);
            info.CreateUserId = _configEntity.CreateUserId;
            info.PromotionUrlCode = SignUtility.GenerateRandomCode(10);
            info.PromotionUrl = GetPromotionUrl(info.PromotionUrl, info.PromotionUrlCode);
            return info;
        }

        #endregion 加参操作

        #region 删除操作

        public ReturnValue Delete()
        {
            var retValue = VerifyOfDelete();
            if (retValue.HasError)
                return retValue;
            var demandBillNo = (int)retValue.ReturnObject;
            Dal.Demand.DemandGroundDelivery.Instance.Delete(demandBillNo, _contextDeleteDeliveryDto.DeliveryId);
            return retValue;
        }

        public ReturnValue VerifyOfDelete()
        {
            var retValue = VerifyOfNecessaryParameters<RequestDeleteDeliveryDto>(_contextDeleteDeliveryDto);
            if (retValue.HasError)
                return retValue;
            var demandInfo = GetGroundDelivery(retValue, _contextDeleteDeliveryDto.DeliveryId);
            if (demandInfo == null)
                return CreateFailMessage(retValue, "60009", $"不存在当前信息 DeliveryId：{_contextDeleteDeliveryDto.DeliveryId}");
            if (demandInfo.AuditStatus == (int)DemandAuditStatusEnum.IsOver || demandInfo.AuditStatus == (int)DemandAuditStatusEnum.Terminated)
                return CreateFailMessage(retValue, "60005", "需求已结束/已终止，不能删除");
            retValue.ReturnObject = demandInfo.DemandBillNo;
            return retValue;
        }

        private Entities.Demand.DemandGroundDelivery GetGroundDelivery(ReturnValue retValue, int deliveryId)
        {
            var groundDeliveryList = Dal.Demand.DemandGroundDelivery.Instance.GetList(new DemandGroundQuery<DemandGroundDelivery>()
            {
                DeliveryId = deliveryId
            });
            if (!groundDeliveryList.Any())
            {
                CreateFailMessage(retValue, "60008", $"不存在当前信息 DeliveryId：{deliveryId}");
                return null;
            }
            return groundDeliveryList.FirstOrDefault();
        }

        #endregion 删除操作

        #region 查询相关

        public RespGetGroundDeliveryDto GetGroundDeliverys(int demandBillNo)
        {
            var respDto = new RespGetGroundDeliveryDto();
            var groundDeliveriesList = Dal.Demand.DemandGroundDelivery.Instance.GetGroundDeliveries(demandBillNo);
            if (!groundDeliveriesList.Any())
            {
                return respDto;
            }
            //todo:只获取当前需求下的城市信息，品牌信息
            var groupByList = groundDeliveriesList.GroupBy(t => t.DemandBillNo).ToList();

            respDto.GroundInfo = new List<GroundInfoDto>();
            var groundIdTagList = new List<int> { };
            var index = 0;
            groupByList.ForEach(t =>
            {
                foreach (var s in t)
                {
                    if (index == 0)
                    {
                        respDto.DemandBillNo = t.Key;
                        respDto.AuditStatus = s.AuditStatus;
                        respDto.AuditStatusName = s.AuditStatusName;
                        respDto.DemandName = s.DemandName;
                    }
                    index++;
                    if (groundIdTagList.Any(k => k.Equals(s.GroundId)))
                        continue;
                    var groundIdList = t.Where(x => x.GroundId == s.GroundId).ToList();
                    if (groundIdList.Count > 1)
                    {
                        //说明有重复的
                        var groundInfo = new GroundInfoDto
                        {
                            GroundId = s.GroundId,
                            PromotionUrl = s.PromotionUrl,
                            AreaInfo = GetCitysInfoDtos(s.CityInfo).FilterCitys(s.ProvinceId, s.CityId),
                            CarInfo = GetCarInfoDtos(s.CarInfo).FilterCarInfo(s.BrandId, s.SerielId),
                            DeliveryList = new List<DeliveryInfoDto>()
                        };
                        groundIdTagList.Add(s.GroundId);
                        groundIdList.ForEach(gd =>
                        {
                            if (gd.DeliveryId > 0)
                            {
                                var deliveryInfo = new DeliveryInfoDto
                                {
                                    PromotionUrl = GetPromotionUrl(gd.PromotionUrl, gd.PromotionUrlCode),
                                    AdCreativeName = gd.AdCreativeName,
                                    AdName = gd.AdName,
                                    AdSiteSetName = gd.AdSiteSetName,
                                    AdgroupName = gd.AdgroupName,
                                    CampaignName = gd.CampaignName,
                                    DeliveryId = gd.DeliveryId,
                                    AdgroupId = gd.AdgroupId,
                                    DeliveryTypeName = gd.DeliveryTypeName
                                };
                                groundInfo.DeliveryList.Add(deliveryInfo);
                            }
                        });

                        respDto.GroundInfo.Add(groundInfo);
                    }
                    else
                    {
                        var groundInfo = new GroundInfoDto
                        {
                            GroundId = s.GroundId,
                            PromotionUrl = s.PromotionUrl,
                            AreaInfo = GetCitysInfoDtos(s.CityInfo).FilterCitys(s.ProvinceId, s.CityId),
                            CarInfo = GetCarInfoDtos(s.CarInfo).FilterCarInfo(s.BrandId, s.SerielId),
                            DeliveryList = new List<DeliveryInfoDto>()
                        };
                        if (s.DeliveryId > 0)
                        {
                            var deliveryInfo = new DeliveryInfoDto
                            {
                                PromotionUrl = GetPromotionUrl(s.PromotionUrl, s.PromotionUrlCode),
                                AdCreativeName = s.AdCreativeName,
                                AdName = s.AdName,
                                AdSiteSetName = s.AdSiteSetName,
                                AdgroupName = s.AdgroupName,
                                AdgroupId = s.AdgroupId,
                                CampaignName = s.CampaignName,
                                DeliveryId = s.DeliveryId,
                                DeliveryTypeName = s.DeliveryTypeName
                            };
                            groundInfo.DeliveryList.Add(deliveryInfo);
                        }
                        respDto.GroundInfo.Add(groundInfo);
                    }
                }
            });

            return respDto;
        }

        public static string GetPromotionUrl(string promotionUrl, string promotionUrlCode)
        {
            if (string.IsNullOrWhiteSpace(promotionUrl))
                return promotionUrl;
            if (promotionUrl.IndexOf('?') > -1)
            {
                return $"{promotionUrl}&utm_term={promotionUrlCode}";
            }
            else
            {
                return $"{promotionUrl}?utm_term={promotionUrlCode}";
            }
        }

        public static List<DeliveryCitysInfoDto> GetCitysInfoDtos(string cityInfo)
        {
            /*
              1,安徽@=102,安庆|1,安徽@=103,蚌埠|1,安徽@=125,亳州
             |1,安徽@=104,巢湖|1,安徽@=105,池州|1,安徽@=116,滁州
             |1,安徽@=106,阜阳|1,安徽@=101,合肥|1,安徽@=107,淮北
            */
            var resp = new List<DeliveryCitysInfoDto>();
            if (string.IsNullOrWhiteSpace(cityInfo))
                return resp;

            var spList = cityInfo.Split(new string[] { "|" }, StringSplitOptions.None);

            foreach (var item in spList)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                var cityInfoDto = new DeliveryCitysInfoDto();
                //1,安徽@=102,安庆
                var info = item.Split(new string[] { "@=" }, StringSplitOptions.None);
                //info:item:102,安庆
                var sp0 = info[0].Split(',');
                var sp1 = info[1].Split(',');
                cityInfoDto.ProvinceId = GetArrayContent(sp0, 0).ToInt();
                cityInfoDto.ProvinceName = GetArrayContent(sp0, 1);
                cityInfoDto.CityId = GetArrayContent(sp1, 0).ToInt();
                cityInfoDto.CityName = GetArrayContent(sp1, 1);
                resp.Add(cityInfoDto);
            }

            return resp;
        }

        public static List<DeliveryCarInfoDto> GetCarInfoDtos(string carInfo)
        {
            /*
            8,北汽@=2049,勇士
            |8,北汽@=2061,战旗2024
            |8,北汽@=2066,陆霸
            |8,北汽@=4157,越铃
            |8,北汽@=4158,锐铃
            */
            var resp = new List<DeliveryCarInfoDto>();
            if (string.IsNullOrWhiteSpace(carInfo))
                return resp;
            var spList = carInfo.Split(new string[] { "|" }, StringSplitOptions.None);

            foreach (var item in spList)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                var carInfoDto = new DeliveryCarInfoDto();
                //item:8,北汽@=2049,勇士
                var brandInfo = item.Split(new string[] { "@=" }, StringSplitOptions.None);
                //brandInfo:item:8,北汽
                var sp0 = brandInfo[0].Split(',');
                var sp1 = brandInfo[1].Split(',');
                carInfoDto.BrandId = GetArrayContent(sp0, 0).ToInt();
                carInfoDto.BrandName = GetArrayContent(sp0, 1);
                carInfoDto.SerielId = GetArrayContent(sp1, 0).ToInt();
                carInfoDto.SerialName = GetArrayContent(sp1, 1);
                resp.Add(carInfoDto);
            }
            return resp;
        }

        #endregion 查询相关

        #region 关联广告组

        public ReturnValue RelateToAdGroup(RequestRelateToAdGroupDto requestRelateToDto)
        {
            var retValue = VerifyOfNecessaryParameters<RequestRelateToAdGroupDto>(requestRelateToDto);
            if (retValue.HasError)
                return retValue;
            var demandInfo = GetGroundDelivery(retValue, requestRelateToDto.DeliveryId);
            if (demandInfo == null)
                return CreateFailMessage(retValue, "60011", $"不存在当前信息 DeliveryId：{requestRelateToDto.DeliveryId}");

            if (demandInfo.AuditStatus != (int)DemandAuditStatusEnum.Puting)
            {
                return CreateFailMessage(retValue, "60004", $"当前需求状态下不允许关联广告组操作，必须为：投放中状态");
            }
            //todo:校验当前广告组id是否被当前需求下别的DeliveryId关联
            retValue = VerifyOfRalateAdGroupId(retValue, demandInfo.DemandBillNo, requestRelateToDto);
            if (retValue.HasError)
                return retValue;

            if (Dal.GDT.GdtDemandRelation.Instance.UpdateRelateToAdGroup(GetGdtDemandRelation(demandInfo.DemandBillNo, requestRelateToDto)) <= 0)
            {
                Loger.Log4Net.Error($"落地页加参，关联广告组失败，入库参数:{ JsonConvert.SerializeObject(GetGdtDemandRelation(demandInfo.DemandBillNo, requestRelateToDto))}");
                return CreateFailMessage(retValue, "60012", "关联广告组失败");
            }

            return retValue;
        }

        private GdtDemandRelation GetGdtDemandRelation(int demandBillNo, RequestRelateToAdGroupDto requestRelateToDto)
        {
            return new GdtDemandRelation()
            {
                CreateUserId = _configEntity.CreateUserId,
                DemandBillNo = demandBillNo,
                AdgroupId = requestRelateToDto.AdGroupId,
                DeliveryId = requestRelateToDto.DeliveryId
            };
        }

        private ReturnValue VerifyOfRalateAdGroupId(ReturnValue retValue, int demandBillNo, RequestRelateToAdGroupDto requestRelateToDto)
        {
            var infos = Dal.GDT.GdtDemandRelation.Instance.GetList(new DemandGroundQuery<GdtDemandRelation>()
            {
                DemandBillNo = demandBillNo
            });
            var filterList = infos.Where(s => s.DeliveryId != requestRelateToDto.DeliveryId && s.AdgroupId == requestRelateToDto.AdGroupId);
            if (filterList.Any())
            {
                return CreateFailMessage(retValue, "60012", $"当前广告组id：{ requestRelateToDto.AdGroupId} 已经被关联，请更换");
            }
            return retValue;
        }

        #endregion 关联广告组
    }

    public static class GroundDeliveryExtend
    {
        public static List<DeliveryCitysInfoDto> FilterCitys(this List<DeliveryCitysInfoDto> sources, int provinceId, int cityId)
        {
            //全部：-1，（全国：0）
            var respList = new List<DeliveryCitysInfoDto>();
            var resp = new DeliveryCitysInfoDto();
            if (provinceId <= 0)
            {
                resp.ProvinceId = -1;
                resp.ProvinceName = "全部";
                resp.CityId = -1;
                resp.CityName = "全部";
                respList.Add(resp);
                return respList;
            }
            else
            {
                var province = sources.FirstOrDefault(s => s.ProvinceId == provinceId);
                if (province != null)
                {
                    resp.ProvinceId = province.ProvinceId;
                    resp.ProvinceName = province.ProvinceName;
                    respList.Add(resp);
                }
            }
            if (cityId > 0)
            {
                var city = sources.FirstOrDefault(s => s.CityId == cityId);//全部的城市(保定，石家庄，邯郸)
                if (city != null)
                {
                    respList.ForEach(s =>
                    {
                        s.CityId = city.CityId;
                        s.CityName = city.CityName;
                    });
                }
            }
            else
            {
                respList.ForEach(s =>
                {
                    s.CityId = -1;
                    s.CityName = "全部";
                });
            }
            return respList;
        }

        public static List<DeliveryCarInfoDto> FilterCarInfo(this List<DeliveryCarInfoDto> sources, int brandId, int serielId)
        {
            var respList = new List<DeliveryCarInfoDto>();
            var resp = new DeliveryCarInfoDto();
            if (brandId <= 0)
            {
                resp.BrandId = -1;
                resp.BrandName = "全部";
                resp.SerielId = -1;
                resp.SerialName = "全部";
                respList.Add(resp);
                return respList;
            }
            else
            {
                var brand = sources.FirstOrDefault(s => s.BrandId == brandId);
                if (brand != null)
                {
                    resp.BrandId = brand.BrandId;
                    resp.BrandName = brand.BrandName;
                    respList.Add(resp);
                }
            }
            if (serielId > 0)
            {
                var seriel = sources.FirstOrDefault(s => s.SerielId == serielId);
                if (seriel != null)
                {
                    respList.ForEach(s =>
                    {
                        s.SerialName = seriel.SerialName;
                        s.SerielId = seriel.SerielId;
                    });
                }
            }
            else
            {
                respList.ForEach(s =>
                {
                    s.SerialName = "全部";
                    s.SerielId = -1;
                });
            }
            return respList;
        }
    }
}