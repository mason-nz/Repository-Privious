/********************************************************
*创建人：lixiong
*创建时间：2017/11/23 15:42:10
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.DataView;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Response;

namespace XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.Profile
{
    public class StatSpiderProfile : AutoMapper.Profile
    {
        public StatSpiderProfile()
        {
            CreateMap<Entities.Statistics.StatSpiderSummary, StatInfo>()
                .ForMember(desc => desc.Name,
                    map => map.MapFrom(s => s.ChannelName))
                ;

            CreateMap<Entities.Statistics.StatAutoSummary, StatInfo>()
                .ForMember(desc => desc.Name,
                    map => map.MapFrom(s => s.ChannelName))
                    .ForMember(desc => desc.ArticleCount,
                    map => map.MapFrom(s => s.StorageArticleCount))
                    .ForMember(desc => desc.AccountCount,
                    map => map.MapFrom(s => s.StorageAccountCount))
                ;

            CreateMap<Entities.Statistics.StatPrimarySummary, StatInfo>()
                .ForMember(desc => desc.Name,
                    map => map.MapFrom(s => s.ConditionName))
                ;
            CreateMap<Entities.Statistics.StatArtificialSummary, StatInfo>()
              .ForMember(desc => desc.Name,
                  map => map.MapFrom(s => s.ConditionName))
              ;

            CreateMap<Entities.Statistics.StatCarMatchSummary, StatInfo>()
             .ForMember(desc => desc.Name,
                 map => map.MapFrom(s => s.ChannelName))
             ;

            CreateMap<Entities.Statistics.StatDataProfiling, RespDvYesterdayDto>()
              ;
        }
    }
}