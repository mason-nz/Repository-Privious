using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.Test.AutoMapper.Dto;

namespace XYAuto.ITSC.Chitunion2017.Test.AutoMapper.Resolve
{
    internal class SourceResolve : ValueResolver<SourceDto, int>
    {
        protected override int ResolveCore(SourceDto source)
        {
            return source.Value2 + source.Value1;
        }
    }
}
