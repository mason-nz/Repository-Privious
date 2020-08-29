/********************************************************
*创建人：lixiong
*创建时间：2017/8/21 15:34:01
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
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Ads;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Fund;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Report;

namespace XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.Profile.GDT
{
    public class GdtServerProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            //拉取数据-->对应的实体
            Mapper.CreateMap<RespAccountInfoDto, Entities.GDT.GdtAccountInfo>()
                 .ForMember(desc => desc.SystemStatus, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicSystemStatus(s.SystemStatus)));
            Mapper.CreateMap<RespFundDto, Entities.GDT.GdtAccountBalance>()
                .ForMember(desc => desc.FundStatus, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicFundStatus(s.FundStatus)))
                .ForMember(desc => desc.FundType, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicFundType(s.FundType)));

            Mapper.CreateMap<FundStatementsDailyDto, Entities.GDT.GdtStatementDaily>()
                 .ForMember(desc => desc.FundType, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicFundType(s.FundType)))
                  .ForMember(desc => desc.TradeType, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicTradeType(s.TradeType)));

            Mapper.CreateMap<FundStatementsDailyDto, Entities.GDT.GdtStatementsDetailed>()
                 .ForMember(desc => desc.Date, opt => opt.MapFrom(s => DateTime.Parse(s.Date).Date))
                 .ForMember(desc => desc.FundType, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicFundType(s.FundType)))
                  .ForMember(desc => desc.TradeType, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicTradeType(s.TradeType)));

            Mapper.CreateMap<RespAdGroupDto, Entities.GDT.GdtAdGroup>()
                 .ForMember(desc => desc.ConfiguredStatus, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicConfiguredStatus(s.ConfiguredStatus)))
                  .ForMember(desc => desc.SystemStatus, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicAdSystemStatus(s.SystemStatus)))
                  .ForMember(desc => desc.OptimizationGoal, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicOptimizationGoal(s.OptimizationGoal)))
                  .ForMember(desc => desc.BillingEvent, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicBillingEvent(s.BillingEvent)))
                  .ForMember(desc => desc.BeginDate, opt => opt.MapFrom(s => DateTime.Parse(s.BeginDate).Date))
                  .ForMember(desc => desc.EndDate, opt => opt.MapFrom(s => DateTime.Parse(s.EndDate).Date))
                  .ForMember(desc => desc.SiteSet, opt => opt.MapFrom(s => s.SiteSet == null ? string.Empty : JsonConvert.SerializeObject(s.SiteSet)));

            Mapper.CreateMap<RespReportDto, Entities.GDT.GdtDailyRrport>()
                .ForMember(desc => desc.Date, opt => opt.MapFrom(s => DateTime.Parse(s.Date).Date));

            Mapper.CreateMap<RespReportDto, Entities.GDT.GdtHourlyRrport>()
                ;

            //推广计划
            Mapper.CreateMap<RespAdCampaignDto, Entities.GDT.GdtCampaign>()
                .ForMember(desc => desc.ConfiguredStatus, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicConfiguredStatus(s.ConfiguredStatus)))
                .ForMember(desc => desc.CampaignType, opt => opt.MapFrom(s => GdtDataEnumProvider.GetDicCampaignType(s.CampaignType)))
                ;
        }
    }
}