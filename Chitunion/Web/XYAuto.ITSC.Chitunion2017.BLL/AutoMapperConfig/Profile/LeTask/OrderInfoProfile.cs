using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto.Request;

namespace XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.Profile.LeTask
{
    public class OrderInfoProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Entities.LETask.LeAdOrderInfo, RespOrderInfoDto>()
                .ForMember(desc => desc.CPCPrice, opt => opt.MapFrom(s => s.CPCUnitPrice))
                .ForMember(desc => desc.CPLPrice, opt => opt.MapFrom(s => s.CPLUnitPrice))
                .ForMember(desc => desc.OrderId, opt => opt.MapFrom(s => s.RecID))
                 .ForMember(desc => desc.ReceiveTime, opt => opt.MapFrom(s => s.CreateTime));

            //收入详情列表
            Mapper.CreateMap<Entities.LETask.LeAccountBalance, OrderIncomeItem>()
                .ForMember(desc => desc.CPCCount, opt => opt.MapFrom(s => s.CPCShowCount))
                .ForMember(desc => desc.CPLCount, opt => opt.MapFrom(s => s.CPLShowCount))
                .ForMember(desc => desc.Date, opt => opt.MapFrom(s => s.StatisticsTime));

            Mapper.CreateMap<Entities.LETask.TotalAccountBalance, OrderIncomeTotalItem>();

            //task api入库
            Mapper.CreateMap<ReqTaskStorageDto, Entities.LETask.LeTaskInfo>()
                .ForMember(desc => desc.CategoryID, opt => opt.MapFrom(s => s.CategoryId));

            //微信详情
            Mapper.CreateMap<Entities.LETask.LeWeixin, RespWeiXinInfoDto>()
                .ForMember(desc => desc.FansFemalePer, opt => opt.MapFrom(s => s.WomanFansRatio))
                .ForMember(desc => desc.FansMalePer, opt => opt.MapFrom(s => s.ManFansRatio))
                .ForMember(desc => desc.MediaId, opt => opt.MapFrom(s => s.RecID));


            Mapper.CreateMap<Entities.LETask.LeWithdrawalsDetail, RespWithdrawalsInfoDto>()
                .ForMember(desc => desc.PayStatus, opt => opt.MapFrom(s => s.Status));

            Mapper.CreateMap<Entities.LETask.LeWithdrawalsDetail, RespWithdrawalsAuditDetailDto>();

            Mapper.CreateMap<ReqReportAppDto, Entities.LETask.AppDevice>()
                .ForMember(desc => desc.EMEI, opt => opt.MapFrom(s => s.IMEI))
                .ForMember(desc => desc.EMSI, opt => opt.MapFrom(s => s.IMSI));

        }
    }
}
