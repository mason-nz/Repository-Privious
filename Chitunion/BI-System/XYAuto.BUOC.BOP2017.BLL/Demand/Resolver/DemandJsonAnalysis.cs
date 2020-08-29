/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 11:14:57
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Response;
using XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.Entities.Dto.Demand;
using XYAuto.BUOC.BOP2017.Infrastruction;

namespace XYAuto.BUOC.BOP2017.BLL.Demand.Resolver
{
    public class DemandJsonAnalysis
    {
        public static readonly DemandJsonAnalysis Instance = new DemandJsonAnalysis();

        public void CarSerielAnalysis(List<CarInfoDto> carInfos, int demandBillNo)
        {
            var dto = AutoMapper.Mapper.Map<List<CarInfoDto>, List<DemandCarSerielDto>>(carInfos);
            Dal.Demand.DemandCarSeriel.Instance.Insert(dto, demandBillNo);
        }

        public void CitysAnalysis(List<AreaInfoDto> areaInfos, int demandBillNo)
        {
            var dto = AutoMapper.Mapper.Map<List<AreaInfoDto>, List<DemandCitysDto>>(areaInfos);
            Dal.Demand.DemandCitys.Instance.Insert(dto, demandBillNo);
        }

        public void CarSerielJsonAnalysis(string carSerielJson, int demandBillNo)
        {
            if (string.IsNullOrWhiteSpace(carSerielJson))
                return;
            try
            {
                var carSerielInfo = JsonConvert.DeserializeObject<List<DemandCarSerielDto>>(carSerielJson);
                Dal.Demand.DemandCarSeriel.Instance.Insert(carSerielInfo, demandBillNo);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.Error($" CarSerielJsonAnalysis is error . json: {carSerielJson}:" +
                                    $"{exception.Message}" +
                                    $"{exception.StackTrace ?? string.Empty}");
            }
        }

        public void CitysJsonAnalysis(string citysJson, int demandBillNo)
        {
            if (string.IsNullOrWhiteSpace(citysJson))
                return;
            try
            {
                var citysInfo = JsonConvert.DeserializeObject<List<DemandCitysDto>>(citysJson);
                Dal.Demand.DemandCitys.Instance.Insert(citysInfo, demandBillNo);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.Error($" CarSerielJsonAnalysis is error . json: {citysJson}:" +
                                    $"{exception.Message}" +
                                    $"{exception.StackTrace ?? string.Empty}");
            }
        }

        public void CleanData()
        {
            var demandList = Dal.GDT.GdtDemand.Instance.GetList();
            demandList.ForEach(s =>
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(s.BrandSerialJson))
                    {
                        CarSerielAnalysis(JsonConvert.DeserializeObject<List<CarInfoDto>>(s.BrandSerialJson), s.DemandBillNo);
                    }
                    if (!string.IsNullOrWhiteSpace(s.ProvinceCityJson))
                    {
                        CitysAnalysis(JsonConvert.DeserializeObject<List<AreaInfoDto>>(s.ProvinceCityJson), s.DemandBillNo);
                    }
                }
                catch (Exception exception)
                {
                    Loger.Log4Net.Error($"CleanData is error 需求单号:{s.DemandBillNo} 错误:{exception.Message}{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}");
                }
            });
        }

        public RespDemandCarAndCitysDto GetDemandCarAndCityInfos(int demandBillNo)
        {
            var respTunpInfos = Dal.Demand.DemandCitys.Instance.GetCarAndCityInfo(demandBillNo);
            var resp = new RespDemandCarAndCitysDto
            {
                BrandInfos = new List<DeliveryBrandInfoDto>(),
                SerieInfos = new List<DeliveryCarInfoDto>(),
                ProvinceInfos = new List<DeliveryProvinceInfoDto>(),
                CitysInfos = new List<DeliveryCitysInfoDto>()
            };
            var cityInfos = respTunpInfos.Item1;
            var carInfos = respTunpInfos.Item2;

            resp.DemandBillNo = demandBillNo;
            carInfos.GroupBy(s => s.BrandId).ToList().ForEach(s =>
            {
                var brandInfo = new DeliveryBrandInfoDto();

                foreach (var item in s)
                {
                    brandInfo.BrandId = s.Key;
                    brandInfo.BrandName = item.BrandName;
                    var serielInfo = new DeliveryCarInfoDto
                    {
                        BrandId = item.BrandId,
                        BrandName = item.BrandName,
                        SerielId = item.SerielId,
                        SerialName = item.SerielName
                    };
                    resp.SerieInfos.Add(serielInfo);
                }
                resp.BrandInfos.Add(brandInfo);
            });

            cityInfos.GroupBy(s => s.ProvinceId).ToList().ForEach(s =>
              {
                  var provinceInfo = new DeliveryProvinceInfoDto();

                  foreach (var item in s)
                  {
                      provinceInfo.ProvinceId = s.Key;
                      provinceInfo.ProvinceName = item.ProvinceName;
                      var cityInfo = new DeliveryCitysInfoDto
                      {
                          ProvinceId = s.Key,
                          ProvinceName = item.ProvinceName,
                          CityId = item.CityId,
                          CityName = item.CityName
                      };
                      resp.CitysInfos.Add(cityInfo);
                  }
                  resp.ProvinceInfos.Add(provinceInfo);
              });

            return resp;
        }
    }
}