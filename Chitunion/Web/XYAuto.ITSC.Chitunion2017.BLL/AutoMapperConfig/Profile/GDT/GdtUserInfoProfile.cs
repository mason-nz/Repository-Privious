/********************************************************
*创建人：lixiong
*创建时间：2017/8/16 13:18:20
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Response;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.GDT;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Fund;

namespace XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.Profile.GDT
{
    public class GdtUserInfoProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            //广点通推送的用户-->映射到 userInfo
            Mapper.CreateMap<RequestPushUserDto, Entities.GDT.UserInfo>()
              .ForMember(desc => desc.Pwd, opt => opt.MapFrom(s => XYAuto.Utils.Security.DESEncryptor.MD5Hash("")))
              .ForMember(desc => desc.Type, opt => opt.MapFrom(s => 1001))
               .ForMember(desc => desc.Source, opt => opt.MapFrom(s => 3004))
              .ForMember(desc => desc.Category, opt => opt.MapFrom(s => 29001));

            //广点通推送的需求-->映射到 GdtDemand
            Mapper.CreateMap<RequestPushDemandDto, Entities.GDT.GdtDemand>()
                .ForMember(desc => desc.Name, opt => opt.MapFrom(s => s.DemandName))
                .ForMember(desc => desc.TotalBudget, opt => opt.MapFrom(s => s.DayBudget))
                .ForMember(desc => desc.BeginDate, opt => opt.MapFrom(s => DateTime.Parse(s.BeginDate).Date))
                .ForMember(desc => desc.EndDate, opt => opt.MapFrom(s => DateTime.Parse(s.EndDate).Date))
                .ForMember(desc => desc.BrandSerialJson, opt => opt.MapFrom(s => GetJson(s.CarInfo)))
                .ForMember(desc => desc.DistributorJson, opt => opt.MapFrom(s => s.Distributor == null ? string.Empty : JsonConvert.SerializeObject(s.Distributor)))
                .ForMember(desc => desc.ProvinceCityJson, opt => opt.MapFrom(s => s.AreaInfo == null ? string.Empty : JsonConvert.SerializeObject(s.AreaInfo)));

            //广点通推送的充值单号-->映射到 GdtDemand
            Mapper.CreateMap<RequestRechargeReceiptDto, Entities.GDT.GdtRechargeRelation>();

            //返回给智慧云的报表数据
            Mapper.CreateMap<Entities.GDT.GdtHourlyRrportForZhy, GetToZhyReportDto>()
                .ForMember(desc => desc.Date, opt => opt.MapFrom(s => s.Date.ToString("yyyy-MM-dd")));

            Mapper.CreateMap<Entities.GDT.GdtHourlyRrport, GetToZhyReportDto>();
        }

        private string GetJson(List<CarInfoDto> carInfo)
        {
            if (carInfo == null) return string.Empty;
            return JsonConvert.SerializeObject(carInfo);
        }
    }
}