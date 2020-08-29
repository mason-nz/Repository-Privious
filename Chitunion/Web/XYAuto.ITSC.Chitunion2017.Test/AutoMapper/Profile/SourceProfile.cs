using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.Test.AutoMapper.Dto;
using XYAuto.ITSC.Chitunion2017.Test.AutoMapper.Resolve;

namespace XYAuto.ITSC.Chitunion2017.Test.AutoMapper.Profile
{
    internal class SourceProfile : global::AutoMapper.Profile
    {
        protected override void Configure()
        {
            //第一种方式
            Mapper.CreateMap<SourceDto, Destination>().ForMember(dtm => dtm.Total
                , opt => opt.MapFrom(dtm => dtm.Value1 + dtm.Value2));

            //第二种实现Resolve
            Mapper.CreateMap<SourceDto, Destination>().ForMember(det => det.Total,
                opt => opt.ResolveUsing<SourceResolve>());
        }
    }
}
